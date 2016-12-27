using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using App3.Class;
using OKOGate;
using App3.Class.Static;
using MessageGroupId = App3.Class.Utils.MessageGroupId;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using App3.Forms.Dialog;
using App3.Forms.Object;
using App3.Web;
using App3.Class.Singleton;
using App3.Class.Map;
using System.Reflection;
using System.Threading;
using App3.Class.Socket2;

namespace App3.Forms
{
    public partial class MainForm : Form
    {
        private StartForm sf;
        private int SessionID = -1;

        public const int FORM_TOP_OFFSET = 113;
        public const int FORM_BORDER_WIDTH = 10;

        public delegate int NodeConnected(string sIpAddress);
        public NodeConnected NodeConnectedHandle = null;

        // private Module oModule;

        private OKOGate.Module oModuleXML;
        private GuardAgent2.Module oModuleCOM;
        private PortListner oSocketSync;

        private int ChildFormNumber = 0;
        // Формы
        private MessForm oEventsForm;
        private SynchronizeForm oSynchronizeForm;
        private LogForm oLogForm;
        private Journal oJournal;
        private ConfigForm oConfigForm;
        private DBEvents oDBEvents;
        // private MapForm oMap;
        private DistrictMap oDistricts;
        private TableEditForm oMessagesText;
        private ObjectForm oObjectForm;
        private WarningForm oWarnMonitor;
        private GlobalStat oGlobalStatistic;
        private Objects oObjectList;

        private bool isShowEventForm = false;

        public void SetStatusText(string pText)
        {
            toolStripStatusLabel.Text = pText;
        }

        private void SetChildFormsPosition()
        {
            oEventsForm.Top = this.Height - oEventsForm.Height - FORM_TOP_OFFSET;
            oEventsForm.Width = this.Width - 2 * FORM_BORDER_WIDTH;
        }

        private void StartOkoGate()
        {
            // OkoConnection.CleanCounters();

            if (Config.Get("XMLGuard") == "1")
            {
                this.oModuleXML = new OKOGate.Module();
                this.oModuleXML.LogLevel = Tracer.eLogLevel.ERROR;
                this.oModuleXML.Protocol = OKOGate.Module.PROTOCOL.XML_GUARD;
                this.oModuleXML.RemotePort = Config.Get("ModuleRemotePort").ToInt();
                this.oModuleXML.LocalServerIP = Config.Get("ModuleLocalServerIP");
                this.oModuleXML.LocalServerPort = Config.Get("ModuleLocalServerPort").ToInt();
                this.oModuleXML.LocalGUID = Config.Get("ModuleLocalGUID");
                this.oModuleXML.ModuleId = Config.Get("ModuleModuleId").ToInt();
                this.oModuleXML.RestoreConnectionTime = 5;
                this.oModuleXML.GetModuleMessageEvent += new OKOGate.EventDelegate(this.ReciveMessage);
                this.oModuleXML.StartModule();
                this.oModuleXML.StartReceive();
                Tracer.LogFileName = Logger.LogDirectory() + "OKOGate.log";
                Logger.Instance.WriteToLog("XML Guard start");
            }
            if (Config.Get("COMConn") == "1")
            {
                this.oModuleCOM = new GuardAgent2.Module();
                this.oModuleCOM.LocalAddress = "11";
                this.oModuleCOM.ModuleId = 11;
                string[] arg_168_0 = SerialPort.GetPortNames();
                string a = Config.Get("COMPortName").ToString();
                string[] array = arg_168_0;
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    if (a == text)
                    {
                        this.oModuleCOM.Close();
                        if (this.oModuleCOM.Init(text, Config.Get("COMBaudrate").ToInt()) == OKO_Messages.Module_Started)
                        {
                            this.oModuleCOM.GetModuleMessageEvent += new GuardAgent2.EventDelegate(Handling.ProcessingComEvent);
                            this.oModuleCOM.ClearRetrAddrList();
                            this.oModuleCOM.SetRetrType(Config.Get("COMRetrType").ToByte());
                            this.oModuleCOM.AddRetrAddr(Config.Get("COMRetrAddr").ToUShort());
                            this.oModuleCOM.SetChannelsMask(Config.Get("COMChannelsMask").ToByte());
                            this.oModuleCOM.SendAskForState(1, 0, Config.Get("RemoteAddress").ToUShort());
                            Logger.Instance.WriteToLog("COM connector start, port: " + text);
                            this.IncConnectCnt();
                            break;
                        }
                        this.oModuleCOM.Close();
                    }
                }
            }
            Handling.GetMessageEvent = (Handling.GetMessageHandler)Delegate.Combine(Handling.GetMessageEvent, new Handling.GetMessageHandler(this.OnGetMessage));
            Handling.onObjectCardOpen = (Handling.ObjectCardOpen)Delegate.Combine(Handling.onObjectCardOpen, new Handling.ObjectCardOpen(this.OnObjectCardOpen));
            Handling.onObjectListOpen = (Handling.ObjectListOpen)Delegate.Combine(Handling.onObjectListOpen, new Handling.ObjectListOpen(this.OnObjectListOpen));
        }

        private void IncConnectCnt()
        {
            if (this.ConnectCnt.InvokeRequired)
            {
                this.ConnectCnt.Invoke(new Action(delegate
                {
                    this.ConnectCnt.Text = (this.ConnectCnt.Text.ToInt() + 1).ToString();
                }));
                return;
            }
            this.ConnectCnt.Text = (this.ConnectCnt.Text.ToInt() + 1).ToString();
        }

        private void OnObjectListOpen(string Filter)
        {
            int top = 100;
            int left = 100;
            if (oObjectList != null && oObjectList.IsHandleCreated)
            {
                top = oObjectList.Top;
                left = oObjectList.Left;
            }
            else
            {
                oObjectList = new Objects(this);
                oObjectList.Show();
            }
            oObjectList.Top = top;
            oObjectList.Left = left;
            oObjectList.Focus();
            if (Filter != "")
            {
                oObjectList.Invoke(new Action(() => { oObjectList.SetFilter(Filter); }));
            }
        }

        private void OnObjectCardOpen(long pIdObject)
        {
            int top = 100;
            int left = 100;
            if (oObjectForm != null && oObjectForm.IsHandleCreated)
            {
                top = oObjectForm.Top;
                left = oObjectForm.Left;
            }
            else
            {
                oObjectForm = new ObjectForm(this);
                oObjectForm.Show();
            }
            oObjectForm.Top = top;
            oObjectForm.Left = left;
            oObjectForm.Focus();
            oObjectForm.Invoke(new Action(() => { oObjectForm.DBLoad(pIdObject); }));
        }

        private void StopOkoGate()
        {
            if (Config.Get("XMLGuard") == "1")
            {
                try
                {
                    this.oModuleXML.StopReceive();
                    this.oModuleXML.StopModule();
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: Прослушка ОКО не остановлена: {2}", base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message));
                }
            }
            if (Config.Get("COMConn") == "1")
            {
                this.oModuleCOM.Close();
            }
        }

        private void ReciveMessage(object arg)
        {
            OKOGate.Message msg = (OKOGate.Message)arg;
            if (msg.Text.IndexOf("?xml") > 0)
            {
                if (msg.Content == null)
                {
                    msg.Content = System.Text.Encoding.Default.GetBytes(
                        Utils.CutPrefixRec(msg.Text) // отрезаем то, что не относится к XML
                    );
                }
                msg.Text = System.Text.Encoding.Default.GetString(msg.Content);
                if (msg.Address != "" && NodeConnectedHandle != null)
                {
                    string str = NodeConnectedHandle(msg.Address).ToString();
                    if (ConnectCnt.InvokeRequired)
                    {
                        ConnectCnt.Invoke(new Action(() => { ConnectCnt.Text = str; }));
                    }
                    else
                    {
                        ConnectCnt.Text = str;
                    }
                }

                if (XMLReader.MessagePacker.Unpack(msg))
                {
                    switch (msg.Type)
                    {
                        case "MAIN_HEADER_XGUARD_OKOGATE":
                            break;
                        case "ACK_CONNECT_OKOGATE": // пришла квитанция
                            OkoConnection.FixCounterError(msg.Text);
                            break;
                        case "MESSAGE_PULT_OKOGATE": // пришло событие
                            Handling.ProcessingXmlEvent(msg);
                            break;
                        case "ADD_OBJECT_XGUARD_OKOGATE": // добавление/изменение карточки объекта
                            MessageBox.Show("добавление/изменение карточки объекта");
                            break;
                        case "DEL_OBJECT_XGUARD_OKOGATE": // удаление объекта
                            MessageBox.Show("удаление объекта");
                            break;
                        case "USER_ACTION_XGUARD_OKOGATE": // действие по тревоге
                            MessageBox.Show("действие по тревоге");
                            break;
                        case "TEST_CONNECT_OKOGATE": // контроль канала связи
                            MessageBox.Show("контроль канала связи");
                            break;
                    }
                }
                else
                {
                    switch (msg.Type)
                    {
                        case "DEBUG_DRV_OKOGATE":
                            // Handling.ProcessingDbgEvent(msg);
                            OkoConnection.PushSystemId(msg.Text);
                            break;
                    }
                }
            }
            // oEventsForm.AddMessageType(msg.Type);
            // oEventsForm.AddEvent(msg.Type, msg.Text);
        }

        /// <summary>
        /// Отображение/неотображение тревожного экрана
        /// </summary>
        /// <param name="pMsgGrId"></param>
        /// <param name="pObject"></param>
        /// <param name="pMsgTxt"></param>
        /// <param name="pIpAddress"></param>
        private void ShowWarning(int pEventId, MessageGroupId pMsgGrId, AKObject pObject, string pMsgTxt, string pPhone, string pTime)
        {
            ObjectState state = DBDict.TState[(int)pMsgGrId];

            if (state.Warn && !warnToolStrip.Checked)
            {
                // Тревожный Экран
                oWarnMonitor.AddRow(pEventId, pObject.Id, pObject.number, pObject.DistrictName, pObject.name, pMsgTxt, state.Status, pPhone, state.Color, pTime);
                oWarnMonitor.Invoke(new Action(() => ShowWarnMon(state.Music & DBDict.Settings["ENABLE_MUSIC"].ToBool())));
            }
        }

        private void OnGetMessage(int pEventId, MessageGroupId pMsgGrId, AKObject pObject, string pMsgTxt, string pPhone, string pTime)
        {
            if (oDistricts != null)
            {
                ShowWarning(pEventId, pMsgGrId, pObject, pMsgTxt, pPhone, pTime);
                LayerCache.Get(LayerType.Object, pObject.RegionId).HeatUp();
                /*
                switch (pMsgGrId)
                {
                    case MessageGroupId.TREV_OHRAN:
                    case MessageGroupId.TREV_POGAR:
                    case MessageGroupId.NEISPRAVNO:
                    case MessageGroupId.CH_S: // ЧС
                        // if (oDistricts.OneDistrictMapOpened == pObject.RegionId)
                        {
                            
                        }
                        break;
                    case MessageGroupId.NORMA:
                        // active.Notify(ObjectNumber, MessText);
                        // TODO
                        break;
                    default:
                        break;
                }*/
            }
            oEventsForm.AddEvent(pObject.number.ToString(), pMsgTxt, pObject.Id);
        }

        /// <summary>
        /// Потокобезопасный метод установки фокуса на форму сообщений в БД
        /// </summary>
        public void DBEventsFocus()
        {
            if (InvokeRequired)
            {
                oDBEvents.BeginInvoke(new Action(() => { oDBEvents.Focus(); }));
            }
            else
            {
                oDBEvents.Focus();
            }
        }

        private void WarnMonitorFocus()
        {
            if (InvokeRequired)
            {
                oWarnMonitor.BeginInvoke(new Action(() => { oWarnMonitor.Focus(); }));
            }
            else
            {
                oWarnMonitor.Focus();
            }
        }

        public MainForm()
        {
            this.Visible = false;
            sf = Utils.CreateLoadThread();
            InitializeComponent();
            Logout();
            Boolean f = false;
            // открыть соединение с БД
            try
            {
                DataBase.OpenConnection(
                    string.Format(
                        "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;",
                        Config.Get("DBServerHost"),
                        Config.Get("DBServerPort"),
                        Config.Get("DBUser"),
                        Config.Get("DBPassword"),
                        Config.Get("DBName")
                    ));
                SetStatusText("Система запущена");
                f = true;
            }
            catch (DataBaseConnectionErrorExcepion ex)
            {
                MessageBox.Show("DataBasse: " + ex.Message);
                SetStatusText("Соединение с БД не было установлено. Причина: " + ex.Message);
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: Соединение с БД не было установлено. Причина: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: При открытии соединения с базой произошла ошибка: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            if (!f)
            {
                // Close();
                Logger.Instance.WriteToLog("Соединение с БД не было установлено");
            }
            else
            {
                Logger.Instance.WriteToLog("Соединение с БД было установлено");
                // справочники
                try
                {
                    DBDict.Update();
                }
                catch (Exception ex2)
                {
                    Logger.Instance.WriteToLog("При обновлении справочников произошла ошибка: " + ex2.Message);
                    Logger.Instance.FlushLog();
                }
                // окно событий
                oEventsForm = new MessForm(this);
                // oEventsForm.Show();
                SetChildFormsPosition();

                oConfigForm = new ConfigForm(this);
                oDBEvents = new DBEvents(this);
                oSynchronizeForm = new SynchronizeForm(this);
                oLogForm = new LogForm(this);
                // oMap = new MapForm(this);
                oMessagesText = new TableEditForm(this, "oko.message_text", "Справочник сообщений");
                oWarnMonitor = new WarningForm(this);

                // окошко статистики
                oGlobalStatistic = new GlobalStat();
                //
                oObjectList = new Objects(this);

                this.oJournal = new Journal(this);
                if (DBDict.Settings["START_XML_GUARD"].ToBool())
                {
                    Logger.Instance.WriteToLog("START_XML_GUARD = True");
                    try
                    {
                        this.StartOkoGate();
                    }
                    catch (Exception ex3)
                    {
                        Logger.Instance.WriteToLog("Проблемы: " + ex3.Message);
                    }
                    Logger.Instance.FlushLog();
                }
                
                // включить карту
                mapToolStrip.Checked = DBDict.Settings["ENABLE_MAP"].ToBool();
                soundToolStrip.Checked = DBDict.Settings["ENABLE_MUSIC"].ToBool();

                this.Text = this.Text + " версия " + Config.APPVERSION;
                Logger.Instance.WriteToLog(this.Text);
                // обновление статусов регионов
                DataBase.RunCommand("select oko.update_district_statuses()");

                // Старт web-серсиса
                if (Config.Get("StartWeb") != "0")
                {
                    StartWeb.Start();
                }
                // Старт сервиса обновления
                Updater.Start();
                // Старт сервиса синхронизации
                if (Config.Get("EnableSync") != "0")
                {
                    Synchronizer.SyncStart += new Action(this.SynchroneStart);
                    Synchronizer.SyncStop += new Action(this.SynchroneStop);
                    Synchronizer.Start();
                }
                // Старт сервера для получения сообщений
                if (Config.Get("SocketEnableSync") == "1")
                {
                    PortListner.onProcess += new ClientObject.ProcessDelegate(Handling.GetObjectDelegate);
                    oSocketSync = new PortListner(Config.Get("SynchPort").ToInt());
                }
                //
                int pRegionId = Config.Get("CurrenRegion").ToInt();
                LayerCache.Init(pRegionId);
                LayerCache.CreateAllLayers();
                LayerCache.UpdateLayer(LayerType.AllRegion, 0);
                if (Config.Get("CacheUpdateOnStart") == "1")
                {
                    LayerCache.UpdateLayers(pRegionId);
                }
                else
                {
                    LayerCache.UpdateLayer(LayerType.Object, pRegionId);
                }
                Logger.Instance.FlushLog();
            }
        }

        private void SynchroneStart()
        {
            if (this.toolStrip.InvokeRequired)
            {
                this.toolStrip.Invoke(new Action(delegate
                {
                    this.toolStripProgressBar1.Visible = (this.toolStripStatusLabel1.Visible = true);
                }));
                return;
            }
            this.toolStripProgressBar1.Visible = (this.toolStripStatusLabel1.Visible = true);
        }

        private void SynchroneStop()
        {
            if (this.toolStrip.InvokeRequired)
            {
                this.toolStrip.Invoke(new Action(delegate
                {
                    this.toolStripProgressBar1.Visible = (this.toolStripStatusLabel1.Visible = false);
                }));
                return;
            }
            this.toolStripProgressBar1.Visible = (this.toolStripStatusLabel1.Visible = false);
        }

        public void LoadStat()
        {
            oGlobalStatistic.LoadStat();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + ChildFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // отметка о запуске программы
            // SessionID
            object [] res;
            while (true)
            {
                DataBase.RunCommandInsert(
                    "journal",
                    new Dictionary<string, object>() { { "start", DateTime.Now.ToString().Q() } },
                    "id",
                    out res
                );
                if (res.Length != 0)
                {
                    break;
                }
                Logger.Instance.WriteToLog("При запуске не удалось получить ID сессии");
                Thread.Sleep(1000);
            }
            SessionID = (int)res[0];
            // установка цвета формы
            MdiClient ctlMDI;
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    ctlMDI = (MdiClient)ctl;
                    ctlMDI.BackColor = this.BackColor;
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                }
            }
            // события
            // ShowDBEvents();
            // запуск формы с картой районов
            oDistricts = new DistrictMap(this);
            oDistricts.Show();
            ShowMap();
            this.Visible = true;
            Utils.DestroyStartThread(sf);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            SetChildFormsPosition();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Updater.Stop();
            StartWeb.Stop();
            DataBase.RunCommandUpdate(
                "journal",
                new Dictionary<string, object>() { { "finish", DateTime.Now.ToString().Q() } },
                new Dictionary<string, object>() { { "id", SessionID } });
            StopOkoGate();
            DataBase.CloseConnection();
            if (Config.Get("SocketEnableSync") == "1") oSocketSync.Stop();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOption();
        }

        private void ShowOption()
        {
            // окно настроек
            if (!oConfigForm.Visible)
            {
                oConfigForm.Show();
            }
            oConfigForm.Focus();
        }

        private void ShowDBEvents()
        {
            if (!oDBEvents.Visible)
            {
                oDBEvents.Show();
            }
            oDBEvents.Focus();
        }

        private void ShowWarnMon(bool play)
        {
            if(!oWarnMonitor.Visible)
            {
                oWarnMonitor.Show();
            }
            WarnMonitorFocus();

            if (play)
            {
                oWarnMonitor.PlaySound();
            }
        }

        public void AddMapClickEvents(App3.Forms.MapForm.MapBoxClick pMethod)
        {
            // oMap.AddClickEvents(pMethod);
        }

        private void ShowMap()
        {
            if (mapToolStrip.Checked)
            {
                // oMap.Visible = mapToolStrip.Checked;
                oDistricts.Visible = Visible;
                // oMap.Show();
                // oMap.Left = this.Width - oMap.Width - 20;
                // oMap.Top = 0;
                UpdInsideSize();
            }
            else
            {
                oDistricts.Visible = false;
            }
            DBDict.Settings["ENABLE_MAP"] = mapToolStrip.Checked.ToString();

            DataBase.RunCommand(
                    string.Format("UPDATE settings SET value = '{0}' WHERE name = 'ENABLE_MAP'",
                        mapToolStrip.Checked.ToString()
                    )
                );

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowOption();
        }

        private void событияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDBEvents();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ShowDBEvents();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Если приложение будет закрыто - на данном узле мониторинг событий будет остановлен. Завершить работу с программой?", "Выйти из программы", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Close();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вам отображена карта районов Ростовской области");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectForm objFrm = new ObjectForm(this);
            AddMapClickEvents(objFrm.MapBoxClick);
            objFrm.Show();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchObject s = new SearchObject();
            s.OnSelectObject = new SearchObject.SelectObjectHandler(FoundedObject);
            oObjectForm = new ObjectForm(this);
            s.ShowDialog();
        }

        public void FoundedObject(Int64 object_id)
        {
            oObjectForm.Show(object_id);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

        }

        public void ChangeDistrictScale(double Scale)
        {
            oDistricts.SetScale(Scale);
        }

        public void ShowDistrict(string DistrictName)
        {
            oDistricts.OpenOneDistrictForm(DistrictName);
        }

        private void ShowEventForm()
        {
            if (PermissionControl.auth())
            {
                if (!isShowEventForm)
                {
                    oEventsForm.Focus();
                    oDistricts.Height = oDistricts.Height - oEventsForm.Height;
                }
                else
                {
                    oDistricts.Height = oDistricts.Height + oEventsForm.Height;
                    oDistricts.Focus();
                    oDistricts.FocusOneDistrictForm();
                }
            }
            isShowEventForm = !isShowEventForm;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ShowEventForm();
        }

        public void DistrictMapRefresh()
        {
            if (mapToolStrip.Checked)
            {
                oDistricts.RefreshMaps();
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            
        }

        private void ShowMessageEdit()
        {
            if (!oMessagesText.Visible)
            {
                oMessagesText.Show();
            }
            oMessagesText.Focus();
        }

        private void районыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableEditForm frmEdit = new TableEditForm(this, "oko.ipaddresses", "Справочник IP-адресов");
            frmEdit.AddForeign("id_region", "regions2map", "num", "name");
            frmEdit.CanEdit = true;
            frmEdit.Show();
        }

        private void районыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TableEditForm frmEdit = new TableEditForm(this, "regions2map", "Справочник районов");
            frmEdit.CanAdd = false;
            frmEdit.CanDel = false;
            frmEdit.CanEdit = true;
            frmEdit.Show();
        }

        private void типыКонтактовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableEditForm frmEdit = new TableEditForm(this, "oko.tcontact", "Типы контактов");
            frmEdit.CanDel = false;
            frmEdit.CanEdit = true;
            frmEdit.Show();
        }

        private void очиститьКэшToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистка кэша потребует перезапуска программы. Вы действительно хотите выполнить перезапуск?", "Очистить кэш", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Hide();
                GeoDataCache.ClearCache();
                Utils.restartApp();
            }
        }

        private void картаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ShowDiagramEvents()
        {
            this.panel1.Visible = !this.panel1.Visible;
            if (this.panel1.Visible && this.mapToolStrip.Checked)
            {
                this.oDistricts.Height = this.oDistricts.Height - this.panel1.Height;
                this.oEventsForm.Top = this.oEventsForm.Top - this.panel1.Height;
                this.UpdateDiagram();
                return;
            }
            this.oDistricts.Height = this.oDistricts.Height + this.panel1.Height;
            this.oEventsForm.Top = this.oEventsForm.Top + this.panel1.Height;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ShowDiagramEvents();
        }

        private void UpdateDiagram()
        {
            List<object[]> rows = DataBase.RowSelect(@"select t.mins, count(t.id) as cnt
                        from (
                        select 	case
	                        when DATE_PART('minute', now()) >= DATE_PART('minute', datetime) THEN
	                        DATE_PART('minute', now()) - DATE_PART('minute', datetime) 
	                        else 60 + DATE_PART('minute', now()) - DATE_PART('minute', datetime)
	                        end as mins, id
                        from oko.event
                        where (now() - datetime ) <  interval '55 minute'
                        ) t
                        group by t.mins
                        order by t.mins"
                );
            chart1.Series.Clear();
            chart1.Legends.Clear();
            var EventCols = new Series("EventCols");
            EventCols.ChartType = SeriesChartType.Column;
            chart1.Series.Add(EventCols);

            chart1.ChartAreas[0] = new ChartArea
            {
                BackColor = Color.Transparent,
                BorderColor = Color.FromArgb(240, 240, 240),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
                AxisX = new Axis
                {
                    Enabled = AxisEnabled.True,
                    IntervalAutoMode = IntervalAutoMode.VariableCount,
                    IsLabelAutoFit = false,
                    IsMarginVisible = true,
                    Minimum = 0,
                    Maximum = 40,
                    LabelStyle = new LabelStyle 
                    { 
                        ForeColor = Color.FromArgb(100, 100, 100), 
                        Font = new Font("Arial", 6, FontStyle.Regular) 
                    },
                    LineColor = Color.FromArgb(220, 220, 220),
                    MajorGrid = new Grid { LineColor = Color.FromArgb(240, 240, 240), LineDashStyle = ChartDashStyle.Solid },
                    MajorTickMark = new TickMark { LineColor = Color.FromArgb(220, 220, 220), Size = 4.0f },
                },
                AxisY = new Axis
                {
                    Enabled = AxisEnabled.True,
                    IntervalAutoMode = IntervalAutoMode.VariableCount,
                    IsLabelAutoFit = false,
                    IsMarginVisible = true,
                    LabelStyle = new LabelStyle 
                    { 
                        ForeColor = Color.FromArgb(100, 100, 100), 
                        Font = new Font("Arial", 6, FontStyle.Regular) 
                    },
                    LineColor = Color.Transparent,
                    MajorGrid = new Grid { LineColor = Color.FromArgb(240, 240, 240), LineDashStyle = ChartDashStyle.Solid },
                    MajorTickMark = new TickMark { LineColor = Color.FromArgb(240, 240, 240), Size = 2.0f }
                },
                Position = new ElementPosition { Height = 100, Width = 100, X = 0, Y = 0 }
            };

            EventCols.IsVisibleInLegend = false;

            foreach (object[] row in rows)
            {
                EventCols.Points.Add(new DataPoint(row[0].ToDouble(), row[1].ToDouble()));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateDiagram();
            label4.Text = (label4.Text.ToInt() + 1).ToString();
            if (Updater.NeedUpdate)
            {
                AutoClosingMessageBox.Show("Необходимо выполнить обновление. Программа будет закрыта.", "Предупреждение", 10000);
                Close();
            }
        }

        private void группыСообщенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMessageEdit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (oWarnMonitor != null)
            {
                oWarnMonitor.Close();
            }
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            if (!oGlobalStatistic.Visible)
            {
                oGlobalStatistic.Show();
                oGlobalStatistic.Left = 0;
                oGlobalStatistic.Top = 100;
            }
        }

        private void событийOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowEventForm();
        }

        private void графикСобытийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDiagramEvents();
        }

        private void иерархическийКлассификаторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new ClassifierForm(true)).ShowDialog();
        }

        private void списокОбъектовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // oObjectList.Show();
            Handling.onObjectListOpen("");
        }

        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            ShowWarnMon(false);
        }

        private void отчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new ReportForm()).ShowDialog();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            UpdInsideSize();
        }

        private void UpdInsideSize()
        {
            this.WindowState = FormWindowState.Maximized;
            oDistricts.UpdSize();

            SetChildFormsPosition();

            if (panel1.Visible)
            {
                panel1.Visible = false;
                ShowDBEvents();
            }
            if (isShowEventForm)
            {
                isShowEventForm = false;
                ShowEventForm();
            }
        }

        private void объектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableEditForm frmEdit = new TableEditForm("oko.real_object", "Справочник объектов");
            frmEdit.CanAdd = false;
            frmEdit.CanDel = true;
            frmEdit.CanEdit = true;
            frmEdit.ShowDialog();
            DBDict.UpdateDictionary("TRealObject");
        }

        private void заказчикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableEditForm frmEdit = new TableEditForm("oko.customer", "Справочник заказчиков");
            frmEdit.CanAdd = false;
            frmEdit.CanDel = true;
            frmEdit.CanEdit = true;
            frmEdit.ShowDialog();
            DBDict.UpdateDictionary("TCustomer");
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void организацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableEditForm frmEdit = new TableEditForm("oko.company", "Справочник компаний");
            frmEdit.CanAdd = true;
            frmEdit.CanDel = true;
            frmEdit.CanEdit = true;
            frmEdit.ShowDialog();
            DBDict.UpdateDictionary("TCompany");
        }

        private void картаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // mapToolStrip.Checked
            ShowMap();
        }

        private void архивацияСобытийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Функция архивации сообщений запускается автоматически раз в сутки. Выполнить внеплановую архивацию?", "Сообщение", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                DataBase.RunCommand("select * from archive.arh_events()");
            }
        }

        private void soundTooItem_Click(object sender, EventArgs e)
        {
            // soundTooItem.Checked
            DBDict.Settings["ENABLE_MUSIC"] = soundToolStrip.Checked.ToString();

            DataBase.RunCommand(
                    string.Format("UPDATE settings SET value = '{0}' WHERE name = 'ENABLE_MUSIC'",
                        soundToolStrip.Checked.ToString()
                    )
                );
        }

        private void синхронизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.Get("EnableSync") != "0")
            {
                oSynchronizeForm.Show();
            }
            else
            {
                MessageBox.Show("ВНИМАНИЕ!!! Функция синхронизации устарела!\r\nСинхроинзация отключена в настройках приложения. Измените конфигурационный файл и перезапустите программу, если желаете выполнить синхронизацию вручную.");
            }
        }

        private void входToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PermissionControl.auth())
            {
                Logout();
            }
            else
            {
                LoginDialog frm = new LoginDialog();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (PermissionControl.auth(frm.Result))
                    {
                        AutoClosingMessageBox.Show("Добро пожаловать, " + PermissionControl.UserName, "Приветствие", 1500);
                        Login();
                    }
                }
            }
        }

        private void Login()
        {
            MenuChange(true);
            входToolStripMenuItem.Text = "Выход";
        }

        private void Logout()
        {
            PermissionControl.logout();
            MenuChange();
            входToolStripMenuItem.Text = "Вход";
        }

        private void MenuChange(bool f = false)
        {
            синхронизацияToolStripMenuItem.Enabled = f;
            справочникиToolStripMenuItem.Enabled = f;
            addToolStripMenuItem.Enabled = f;
            архивацияСобытийToolStripMenuItem.Enabled = f;
            optionsToolStripMenuItem.Enabled = f;
            toolStripButton1.Visible = f;
            тестовоеСообщениеToolStripMenuItem.Enabled = f;
            мониторУзловToolStripMenuItem.Enabled = f;
        }

        private void фиксацияЖурналаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Instance.FlushLog();
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.oLogForm.UpdateView();
            this.oLogForm.Show();
            this.oLogForm.Focus();
        }

        private void журналРаботыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.oJournal.ShowData();
            this.oJournal.Show();
            this.oJournal.Focus();
        }

        private void тестовоеСообщениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new SocketTest()).ShowDialog();
        }

        private void мониторУзловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new MonitorForm()).ShowDialog();
        }
    }
}
