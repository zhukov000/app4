using App3.Class;
using App3.Class.Singleton;
using App3.Class.Static;
using App3.Dialog;
using App3.Forms.Dialog;
using App3.Forms.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*using Ozeki.Camera;
using Ozeki.Controls;
using Ozeki.Media;
*/

namespace App3.Forms
{
    public partial class ObjectForm : Form
    {

        private void Init()
        {

        }

        private Controls.DBTable contractTable;

        private void InitTables()
        {
            this.contractTable = new App3.Controls.DBTable();
            // this.contactTable = new App3.Controls.DBTable();
            // this.eventTable = new App3.Controls.DBTable();
            this.panel4.Controls.Add(this.contractTable);
            // this.panel5.Controls.Add(this.contactTable);
            // this.panel7.Controls.Add(this.eventTable);
            // 
            // contractTable
            // 
            this.contractTable.CanAdd = false;
            this.contractTable.CanDel = true;
            this.contractTable.CanEdit = false;
            this.contractTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contractTable.Location = new System.Drawing.Point(0, 0);
            this.contractTable.Margin = new System.Windows.Forms.Padding(4);
            this.contractTable.Name = "contractTable";
            this.contractTable.Size = new System.Drawing.Size(417, 176);
            this.contractTable.TabIndex = 0;
            this.contractTable.TableName = "oko.contract";
            // 
            // contactTable
            // 
/*            this.contactTable.CanAdd = false;
            this.contactTable.CanDel = true;
            this.contactTable.CanEdit = true;
            this.contactTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contactTable.Location = new System.Drawing.Point(0, 0);
            this.contactTable.Name = "contactTable";
            this.contactTable.Size = new System.Drawing.Size(282, 124);
            this.contactTable.TabIndex = 0;
            this.contactTable.TableName = "oko.contact";
*/            
                // this.contactTable.RowHeadersVisible = true;
                //
                // eventTable
                //
                /*this.eventTable.CanAdd = false;
                this.eventTable.CanDel = false;
                this.eventTable.CanEdit = false;
                this.eventTable.Dock = System.Windows.Forms.DockStyle.Fill;
                this.eventTable.Location = new System.Drawing.Point(0, 0);
                this.eventTable.Name = "eventTable";
                this.eventTable.TabIndex = 0;
                // this.eventTable.TableName = "oko.event"; 
                this.eventTable.TableName = "oko.event_messages";*/
        }

        private AKObject Obj = null;

        public void MapBoxClick(double X, double Y)
        {
            double[] ll = Geocoder.UTM2LL(new double[] { X, Y });
            Obj.SetPoint(ll[0], ll[1]);
            addressBox.Text = Obj.AddressStr;
        }

        private void UpdateClassifier()
        {
            if (Obj.IsExists())
            {
                listBox1.Items.Clear();
                foreach (var s in Obj.Properties())
                {
                    ComboboxItem item = new ComboboxItem(s.Key, s.Value);
                    listBox1.Items.Add(item);
                }
            }
        }

        private void LoadData()
        {
            if (this.Obj.IsExists())
            {
                if (this.numberBox.InvokeRequired)
                {
                    this.numberBox.Invoke(new Action(delegate
                    {
                        this.numberBox.Text = this.Obj.number.ToString();
                    }));
                }
                else
                {
                    this.numberBox.Text = this.Obj.number.ToString();
                }
                
                this.nameBox.Text = this.Obj.name;
                this.makeDateTime.Text = this.Obj.makedatetime;
                this.addressBox.Text = this.Obj.AddressStr;
                this.cladrBox.Text = this.Obj.AddressCode;
                this.dogovorBox.Checked = this.Obj.Dogovor;
                this.autoCompleteTextbox2.MinTypedCharacters = 2000;
                this.autoCompleteTextbox2.Text = this.Obj.RealObjectName;
                this.autoCompleteTextbox1.MinTypedCharacters = 2000;
                this.autoCompleteTextbox1.Text = this.Obj.CustomerName;
                this.UpdateClassifier();
                this.UpdateContacts();
                this.autoCompleteTextbox2.MinTypedCharacters = 2;
                this.autoCompleteTextbox1.MinTypedCharacters = 2;
                Utils.UpdateState(this.Obj.number, this.Obj.RegionId);
            }
            else
            {
                MessageBox.Show("Объект не найден в базе данных объектов.","Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void UpdateContacts()
        {
            if (Obj.IsExists())
            {
                List<object[]> contacts = DataBase.RunCommandSelect(
                    "oko.contact",
                    new Dictionary<string,object>
                    {
                        {"tcontact",""},
                        {"value",""},
                        {"\"desc\"",""}
                    },
                    new Dictionary<string,object>
                    {
                        {"object_id", Obj.Id}
                    }
                );
                contactTable.Rows.Clear();
                foreach(object[] row in contacts)
                {
                    contactTable.Rows.Add(row);
                }
            }
        }

        private void LoadDictionaries()
        {
            try
            {
                /*DBDict.Load2Combobox(ref classBox, 
                    DBDict.TClass.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    Obj.TClass
                );*/
                DBDict.Load2Combobox(ref statusBox,
                    DBDict.TStatus.Select(x => new ComboboxItem(x.Value, x.Key)).ToList(),
                    Obj.TStatus
                );
                /*DBDict.Load2Combobox(
                    ref contactBox,
                    DBDict.TContact.Select(x => new ComboboxItem(x.Value, x.Key)).ToList(),
                    1
                );*/
                /* DBDict.Load2Combobox(
                    ref ministryBox,
                    DBDict.TMinistry.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    Obj.TMinistry
                ); */
                DBDict.Load2Combobox(
                    ref companyBox,
                    DBDict.TCompany.Where(x => x.Value.Item2 == 1).Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    1
                );

                DBDict.Load2Combobox(
                    ref serviceBox,
                    DBDict.TCompany.Where(x => x.Value.Item2 == 2).Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    null
                );
            }
            catch(Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
        }

        private int ComboboxVal(ref ComboBox combo)
        {
            int ret = 0;
            if (combo.SelectedIndex > -1)
            {
                ComboboxItem selItem = (ComboboxItem)combo.Items[combo.SelectedIndex];
                ret = selItem.Value.ToInt();
            }
            return ret;
        }

        private bool Save()
        {
            bool r = false;
            if ((numberBox.Text != "") && Int32.TryParse(numberBox.Text, out Obj.number))
            {
                Obj.name = nameBox.Text;
                Obj.makedatetime = makeDateTime.Text;
                Obj.TStatus = ComboboxVal(ref statusBox);
                Obj.Dogovor = dogovorBox.Checked;

                try
                {
                    Obj.Save2DB();
                    r = true;
                } catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                    MessageBox.Show(ex.Message, "Произошла ошибка");
                }
            }
            else
            {
                MessageBox.Show("Некорректно задано обязательное поле \"Номер\"");
            }
            return r;
        }

        public ObjectForm(Form Parent)
        {
            InitializeComponent();
            Auth();
            InitTables();
            MdiParent = Parent;
            numberBox.ReadOnly = false;
            Obj = new AKObject();
            Init();
        }

        public ObjectForm()
        {
            InitializeComponent();
            Auth();
        }

        public void DBLoad(Int64 pObjectId)
        {
            Obj = new AKObject(pObjectId);
            InitTables();
            LoadData();
            Init();
            LoadDictionaries();
        }

        public ObjectForm(Int64 pObjectId)
        {
            InitializeComponent();
            Auth();
            DBLoad(pObjectId);
        }

        public ObjectForm(Form Parent, Int64 pObjectId)
        {
            InitializeComponent();
            Auth();
            DBLoad(pObjectId);
            MdiParent = Parent;
        }

        /*public ObjectForm(Form Parent, string pNumber)
        {
            InitializeComponent();
            InitTables();
            MdiParent = Parent;
            if (!AKObject.TryGetObjectByNumber(pNumber, out Obj))
            {
                Obj = new AKObject();
                numberBox.ReadOnly = false;
            }
            else
            {
                LoadData();
            }
            Init();
        }*/

        public ObjectForm(Form Parent, AKObject pObj)
        {
            InitializeComponent();
            Auth();
            InitTables();
            MdiParent = Parent;
            Obj = pObj;
            LoadData();
            Init();
        }

        private List<ComboboxItem> AutoData(string table , string s = "")
        {
            List<ComboboxItem> ret = new List<ComboboxItem>();
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("select distinct id, name from {1} where name like '%{0}%'", s, table), ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ComboboxItem(
                    row["name"].ToString(),
                    row["id"].ToInt())
                );
            }
            return ret;
        }

        private void ObjectForm_Load(object sender, EventArgs e)
        {
            CancelButton = button1;
            // LoadDictionaries();
            // contactTable.CanAdd = false;
            // contactTable.CanDel = true;

            autoCompleteTextbox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            autoCompleteTextbox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            autoCompleteTextbox2.AutoCompleteList = AutoData("oko.real_object");

            autoCompleteTextbox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            autoCompleteTextbox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            autoCompleteTextbox1.AutoCompleteList = AutoData("oko.customer");

            autoCompleteTextbox2.SetLocationListBoxItem(new Point(autoCompleteTextbox2.Location.X + 130, autoCompleteTextbox2.Location.Y + 45));
            autoCompleteTextbox1.SetLocationListBoxItem(new Point(autoCompleteTextbox1.Location.X + 130, autoCompleteTextbox1.Location.Y + 45));

            // webcam = new WebCam();
            // webcam.InitializeWebCam(ref pictureBox1);
            // webCamCapture1.CaptureHeight = this.pictureBox1.Height;
            // webCamCapture1.CaptureWidth = this.pictureBox1.Width;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Сохранить сделанные изменения", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (Save())
                {
                    // contactTable.SaveChange();
                    contractTable.SaveChange();
                    // Close();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Восстановить значения до редактирования (при этом сделанные изменения будут потеряны)", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Update();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addressBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // редактирование адреса
            AddressDialog addr = new AddressDialog();
            if (addr.ShowDialog() == DialogResult.OK)
            {
                addressBox.Text = addr.GetAddress;
                cladrBox.Text = addr.GetCode;
                Obj.SetAddress(addr.oAddress);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WaitDialog wd = Utils.CreateWaitThread(this, Config.Get("MonitorNumber", "1").ToInt());

            PointSelect MapFrm = new PointSelect();
            MapFrm.AddClickEvents(new PointSelect.MapBoxClick(PointSelect));
            if (Obj.IsExists())
            {
                MapFrm.SelectRegion(Obj.RegionId);
            }

            Utils.DestroyWaitThread(wd);
            MapFrm.ShowDialog();
            
        }

        private void PointSelect(double lat, double lon, Address addr)
        {
            double[] coor = Geocoder.UTM2LL(new double[] { lat, lon });
            Obj.SetPoint(coor[0], coor[1], true);
            if (MessageBox.Show("Информация о координатах объекта заменена. Заменить информацию об адресе на: " + addr.FullAddress + "?", "Замена адреса", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                Obj.SetAddress(addr);
            }
        }

        public void Show(Int64 pObjectId)
        {
            Obj = new AKObject(pObjectId);
            LoadData();
            if (InvokeRequired)
            {
                Invoke(new Action(() => { Show(); }));
            }
            else
            {
                Show();
            }
        }
        /*
        void _webCamera_CameraStateChanged(object sender, CameraStateEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                switch (e.State)
                {
                    case CameraState.Streaming:
                        // btn_Disconnect.Enabled = true;
                        break;
                    case CameraState.Disconnected:
                        // btn_Disconnect.Enabled = false;
                        // btn_Connect.Enabled = true;
                        break;
                }
            });
        }*/

        private void InvokeGuiThread(Action action)
        {
            BeginInvoke(action);
        }
        
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 1:
                    /*eventTable.Filter = "";// string.Format("objectnumber = {0} AND region_id = {1}", Obj.number, Obj.RegionId);
                    eventTable.Sorting("datetime");                    
                    eventTable.Refresh();*/
                    if (eventTable.InvokeRequired)
                        eventTable.BeginInvoke(new Action(() => { UpdateEvents(); }));
                    else
                        UpdateEvents();

                    break;
                default:
                    break;
                /*case 3:
                    if (_webCamera != null)
                    {
                        videoViewerWF1.Stop();
                        _webCamera.Stop();
                        _mediaConnector.Disconnect(_webCamera.VideoChannel, _imageProvider);
                    }
                    // IPCameraFactory.GetCamera("", "", "") 
                    _webCamera = new OzekiCamera("usb://DeviceId=0;Name=USB2.0 Camera;");
                    // _webCamera.CameraStateChanged += _webCamera_CameraStateChanged;
                    _mediaConnector.Connect(_webCamera.VideoChannel, _imageProvider);
                    _webCamera.Start();
                    videoViewerWF1.Start();
                    break;
                default:
                    if (_webCamera != null)
                    {
                        videoViewerWF1.Stop();
                        _webCamera.Stop();
                        _mediaConnector.Disconnect(_webCamera.VideoChannel, _imageProvider);
                        _webCamera = null;
                    }
                    break;
                 */ 
            }
            
        }

        private void webCamCapture1_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
        {
            // this.pictureBox1.Image = e.WebCamImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // добавление контакта
            ContactDialog frm = new ContactDialog();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Obj.AddContact(frm.TypeContact, frm.Value, frm.Description);
                UpdateContacts();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string number = numberBox.Text;
            string cdate = createDate.Text;
            string sdate = dateTimePicker1.Text;
            string fdate = dateTimePicker2.Text;
            int icompany = ComboboxVal(ref companyBox);

            Obj.AddContract(number.ToInt(), cdate, sdate, fdate, icompany);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            /*if (MessageBox.Show("Желаете изменить картинку") == DialogResult.OK)
            {
            }*/
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClassifierForm frm = new ClassifierForm();
            frm.ShowDialog();
            if (frm.Result > 0)
            {
                int i = frm.Result;
                Obj.AddProperties(i);
                UpdateClassifier();
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ComboboxItem item = (ComboboxItem)listBox1.SelectedItem;
            if (MessageBox.Show("Вы действительно хотите удалить категорию " + item.Text, "Вопрос", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Obj.DelProperties(item.Value.ToInt());
                UpdateClassifier();
            }
        }

        private void LoadNext()
        {
            // 
            int pObjectId = Utils.GetNextNumber(Obj.number, Obj.RegionId);
            Obj = new AKObject(pObjectId);
            // InitTables();
            LoadData();
            Init();
            LoadDictionaries();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadNext();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int pObjectId = Utils.GetPrevNumber(Obj.number, Obj.RegionId);
            Obj = new AKObject(pObjectId);
            // InitTables();
            LoadData();
            Init();
            LoadDictionaries();
        }

        private void ObjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            contractTable.Dispose();
            // contactTable.Dispose();
            // eventTable.Dispose();
        }

        private void UpdateEvents()
        {
            eventTable.Rows.Clear();
            List<object[]> data = DataBase.RowSelect(
                String.Format(
                    @"select ev.datetime, mt.message, mt.notes, ts.status, ev.address
                    from oko.event ev
	                    inner join oko.object ob on ob.number = ev.objectnumber and ob.region_id = ev.region_id " +
	                    "inner join oko.message_text mt on mt.\"OKO\" = ev.oko_version and mt.code = ev.code and mt.class = ev.class " +
                        @"inner join oko.tstate ts on ts.id = mt.mgroup_id
                    where ob.osm_id = {0}
                    order by datetime desc",
                    Obj.Id
                )
            );
            foreach(object[] row in data)
            {
                eventTable.Rows.Add(row);
            }
        }

        /*private void button10_Click(object sender, EventArgs e)
        {
            eventTable.Filter = string.Format("region_id = {1}", Obj.number, Obj.RegionId);
            this.eventTable.Sorting("objectnumber");
            eventTable.Refresh();
        }*/

        private void autoCompleteTextbox2_TextChanged(object sender, EventArgs e)
        {
            var item = autoCompleteTextbox2.SelectedItem;
            if (item != null && item.Value.ToInt() != Obj.RealObject)
            {
                Obj.RealObject = item.Value.ToInt();
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            var item = autoCompleteTextbox2.SelectedItem;
            if (item == null)
            {
                string s = autoCompleteTextbox2.Text;
                if (s != "" && AutoData("oko.real_object", s).Count == 0)
                { // добавляем новый 
                    object[] pResult = new object[0];
                    DataBase.RunCommandInsert
                    (
                        "oko.real_object",
                        new Dictionary<string, object>
                        {
                            {"name", s.Q()}
                        },
                        "id",
                        out pResult
                    );
                    DBDict.TRealObject.Add(pResult[0].ToInt(), s);
                    Obj.RealObject = pResult[0].ToInt();
                }
                autoCompleteTextbox2.AutoCompleteList = AutoData("oko.real_object");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var item = autoCompleteTextbox1.SelectedItem;
            if (item == null)
            {
                string s = autoCompleteTextbox1.Text;
                if (s != "" && AutoData("oko.customer", s).Count == 0)
                { // добавляем новый 
                    object[] pResult = new object[0];
                    DataBase.RunCommandInsert
                    (
                        "oko.customer",
                        new Dictionary<string, object>
                        {
                            {"name", s.Q()}
                        },
                        "id",
                        out pResult
                    );
                    DBDict.TCustomer.Add(pResult[0].ToInt(), s);
                    Obj.RealObject = pResult[0].ToInt();
                }
                autoCompleteTextbox1.AutoCompleteList = AutoData("oko.customer");
            }
        }

        private void autoCompleteTextbox1_TextChanged(object sender, EventArgs e)
        {
            var item = autoCompleteTextbox1.SelectedItem;
            if (item != null && item.Value.ToInt() != Obj.Customer)
            {
                Obj.Customer = item.Value.ToInt();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            // удаление контакта
            if (contactTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Ниодного контакта не выбрано для изменения");
            }
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить выбранные контакты", "Вопрос", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in contactTable.SelectedRows)
                    {
                        Obj.DelContact(row.Cells[0].Value.ToInt(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                    }
                    UpdateContacts();
                }
            }
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            // изменени контакта
            foreach (DataGridViewRow row in contactTable.SelectedRows) 
            {
                ContactDialog frm = new ContactDialog(row.Cells[0].Value.ToInt(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Obj.UpdContact(frm.TypeContact, frm.Value, frm.Description);
                }
            }
            if (contactTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Ниодного контакта не выбрано для изменения");
            }
            else
            {
                UpdateContacts();
            }
        }

        private void Auth()
        {
            if (!PermissionControl.auth())
            {
                // tabPage1.Enabled = false;
                tabControl1.Enabled = false;
                groupBox1.Enabled = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (Obj.IsExists())
            {
                if (MessageBox.Show("Объект будет физически удален из базы. Продолжить удаление?", "Удалить объект", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Obj.RemoveObject();
                    LoadNext();
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить объект, который не сохранен в БД", "Предупреждение");
            }
        }
    }
}
/*
// 1. Обновить адрес
            DataBase.RunCommandUpdate(
                    "oko.addresses", 
                    new Dictionary<string, object>() 
                    {
                        {"lat", lat},
                        {"lon", lon}
                    },
                    new Dictionary<string, object>() 
                    {
                        {"id", addr.Id}
                    }
                );
            // 2. Обновить объект

*/