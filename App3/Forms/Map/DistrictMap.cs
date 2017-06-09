using App3.Class;
using App3.Tools;
using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using App3.Forms.Dialog;
using System.Windows.Forms.DataVisualization.Charting;
using App3.Class.Singleton;
using App3.Class.Map;
using App3.Forms.Map;

namespace App3.Forms
{

    public partial class DistrictMap : Form
    {
        private double MinScale = 1250000;
        private OneDistrictForm map = null;
        private DistrictMapWeb map2 = null;

        private int SelectedRegion = 0;
        private int positionRow = 0;
        // private static int UPDATE_CACHE = 10;

        private Mapper oMapper = new Mapper();
        private IDictionary<int, bool> Regions = new Dictionary<int, bool>();

        public void RefreshMaps()
        {
            /*if (DBDict.Settings["RESTART_TIME"] != "")
            {
                int mins = DBDict.Settings["RESTART_TIME"].ToInt();
                TimeSpan duration = DateTime.Now - DBDict.SessionStart;
                if (duration.TotalMinutes > mins)
                {
                    AutoClosingMessageBox.Show("Происходит плановая перезагрузка приложения...", "Предупреждение", 2000);
                    Logger.Instance.WriteToLog("Плановая перезагрузка приложения");
                    Application.Exit();
                }
            }*/
            ((MainForm)MdiParent).TestConnection();

            // if (UPDATE_CACHE < 0)
            {
                Utils.UpdateDistrictStatuses(Config.Get("CurrenRegion").ToInt());
                LayerCache.UpdateLayer(LayerType.AllRegion, -1);
                LayerCache.UpdateLayer(LayerType.Object, -1);
            //    UPDATE_CACHE = 10;
            }
            /*else
                UPDATE_CACHE--;*/

            if (map != null)
            {
                map.Refresh();
            }
            mapBox.Refresh();

        }

        private void SetGrBox2TextThreadSafe(string pText)
        {
            if (groupBox2.InvokeRequired)
                groupBox2.BeginInvoke(new Action(() => { groupBox2.Text = pText; }));
            else
                groupBox2.Text = pText;
        }

        public void SelectRegion(int idRegion)
        {
            if (idRegion > 0)
            {
                SelectedRegion = idRegion;
                SetGrBox2TextThreadSafe("Статистика для " + DBDict.TRegion[idRegion].Item1);
                pictureBox1.BeginInvoke(new Action(() => { pictureBox1.Visible = true; }));
                LoadStat(idRegion);
            }
            else
            {
                DeselectRegion();
            }
            //ShowStat(Utils.StatObjects((e.Node.Name)));
            // treeView1.SelectedNode = e.Node;
            //SelectedRegion = e.Node.Name.ToInt();
            //SelectNodeUpd(e.Node.Text);
        }

        public void DeselectRegion()
        {
            SelectedRegion = 0;
            SetGrBox2TextThreadSafe("Статистика по области");
            pictureBox1.BeginInvoke(new Action(() => { pictureBox1.Visible = false; }));
            LoadStat();
        }

        private void UpdateDataGrid(List<object[]>[] stats)
        {
            if (dataGridView1.InvokeRequired)
                dataGridView1.BeginInvoke(new Action(() => { BuildDataGridThreadSafe(stats[0], stats[1]); }));
            else
                BuildDataGridThreadSafe(stats[0], stats[1]);
        }

        private void UpdateDataChart(List<object[]>[] stats)
        {
            List<object[]> s = stats[0].Union(stats[1]).ToList();
            if (chart1.InvokeRequired)
                chart1.BeginInvoke(new Action(() => { BuildChartThreadSafe(s); }));
            else
                BuildChartThreadSafe(s);
        }

        private void LoadStat(int RegionId = 0)
        {
            List<object[]>[] stats = Utils.GetStatistic(RegionId);
            if (stats != null)
            {
                UpdateDataGrid(stats);
                UpdateDataChart(stats);
            }
            /////// ((MainForm)MdiParent).LoadStat();
        }

        public int UpdateTime
        {
            get { return label3.Text.Substring(0, 2).Trim().ToInt(); }
            set { label3.Text = value.ToString() + " сек."; }
        }
        //private int SelectedRegionId = 0;

        public int OnNodeConnected(string sAddress)
        {
            int i = 0;
            try
            {
                int IdRegion = DBDict.IPAddress[sAddress];
                bool flag = false;
                Regions.TryGetValue(IdRegion, out flag);
                Regions[IdRegion] = true;
                if (!flag)
                {
                    BuildTreeView();
                }
                i = Regions.Where(x => x.Value).Count();
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: Неизвестный адрес '{3}'. Для предотвращения появления этой ошибки добавьте этот адрес в справочник в БД: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, sAddress), Logger.LogLevel.ERROR);
            }
            return i;
        }

        public int OneDistrictMapOpened
        {
            get 
            {
                int i = -1;
                if (map != null && map.Visible)
                {
                    i = map.DistrictId;
                }
                return i;
            }
        }
        //private WarningForm WarnMonitor = null;

        public void FocusOneDistrictForm()
        {
            if (OneDistrictMapOpened > 0)
                Map.Focus();
        }

        public OneDistrictForm Map
        {
            get { return map; }
        }

        public DistrictMap(Form Parent)
        {
            InitializeComponent();
            MdiParent = Parent;
            ((MainForm)MdiParent).NodeConnectedHandle += OnNodeConnected;
            // карта
            mapBox.Map = oMapper.InitializeRegionMap();
            mapBox.Map.ZoomToExtents();
            SetScale(300);
            mapBox.Refresh();
            mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        /// <summary>
        /// Установить масштаб
        /// </summary>
        /// <param name="PerSent">100%-300%</param>
        public void SetScale(double PerSent)
        {
            if (PerSent >= 100 && PerSent <= 300)
            {
                mapBox.Map.MinimumZoom = MinScale / (PerSent / 100);
                mapBox.Map.Zoom = MinScale / (PerSent / 100);
                mapBox.Refresh();
            }
        }
        
        public void UpdSize()
        {
            this.Width = Parent.Width - 5;
            this.Height = Parent.Height - 5;

            this.Top = 0;
            this.Left = 0;
        }

        private void DistrictMap_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            UpdSize();
            
            string[] ChartTypes = Enum.GetNames(typeof(SeriesChartType));
            System.Array.Sort(ChartTypes);
            foreach (var name in ChartTypes)
            {
                comboBox1.Items.Add(new ComboboxItem(name, Enum.Parse(typeof(SeriesChartType), name)));
            }
            comboBox1.Text = "Pie";

            /// Enable tools
            var pt = new SelectRegionTool();
            pt.MapControl = mapBox;
            pt.Enabled = true ;
            pt.OnSelected += new SelectRegionHandler(OnRegionSelected);
            //
            BuildTreeView();
            LoadStat();
        }

        private void BuildDataGridThreadSafe(List<object[]> data1, List<object[]> data2)
        {
            dataGridView1.Rows.Clear();
            int sum1 = 0;
            int sum2 = 0;
            positionRow = 0;

            foreach(object[] row in data1)
            {
                int idx = dataGridView1.Rows.Add(row[0], row[2], row[3], row[4]);
                sum1 += row[2].ToInt();
                dataGridView1.Rows[idx].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(row[1].ToString());
                dataGridView1.Rows[idx].DefaultCellStyle.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(row[1].ToString()));
                positionRow++;
            }

            foreach (object[] row in data2)
            {
                int idx = dataGridView1.Rows.Add(row[0], row[2], row[3], row[4]);
                sum2 += row[2].ToInt();
                dataGridView1.Rows[idx].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(row[1].ToString());
                dataGridView1.Rows[idx].DefaultCellStyle.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(row[1].ToString()));
            }

            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            style1.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            style1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style1.BackColor = Color.FromArgb(59, 20, 175);

            style2.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = Color.FromArgb(59, 218, 0);

            style3.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            style3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style3.BackColor = Color.FromArgb(255, 49, 0);

            dataGridView1.Rows.Insert(positionRow, "Без договора ", sum2, null, false);
            dataGridView1.Rows[positionRow].DefaultCellStyle = style3;
            dataGridView1.Rows.Insert(0, "С договором ", sum1, null, true);
            dataGridView1.Rows[0].DefaultCellStyle = style2;
            dataGridView1.Rows.Insert(0, "Всего объектов ", sum1 + sum2, null, null);
            dataGridView1.Rows[0].DefaultCellStyle = style1;
            positionRow += 2;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor;
                row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
            }
        }

        private void BuildChartThreadSafe(List<object[]> data)
        {
            chart1.Series.Clear();
            var seriesColumns = new Series("RandomColumns");
            seriesColumns.ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBox1.SelectedItem.ToString());
            chart1.Series.Add(seriesColumns);

            foreach (object[] row in data)
            {
                seriesColumns.Points.Add(row[2].ToDouble()).Color = ColorTranslator.FromHtml(row[1].ToString());
            }

            
        }

        /*private void LoadStat(int RegionId = 0)
        {
            List<object[]>[] stats = new List<object[]>[2];
            if (RegionId == 0)
            {
                stats = Utils.CommonStatistic();
            }
            else
            {
                stats = Utils.RegionStatistic(RegionId);
            }
            if (stats != null)
            {
                if (dataGridView1.InvokeRequired)
                    dataGridView1.BeginInvoke(new Action(() => { BuildDataGridThreadSafe(stats[0], stats[1]); }));
                else
                    BuildDataGridThreadSafe(stats[0], stats[1]);

                if (chart1.InvokeRequired)
                    chart1.BeginInvoke(new Action(() => { BuildChartThreadSafe(stats[0]); }));
                else
                    BuildChartThreadSafe(stats[1]);
            }
            ((MainForm)MdiParent).LoadStat();
        }*/

        private void DistrictMap_Resize(object sender, EventArgs e)
        {
            if (OneDistrictMapOpened > 0)
            {
                map.Height = this.Height;
                map.Width = this.Width;
            }
        }

        private void DistrictMap_Shown(object sender, EventArgs e)
        {

        }

        private void mapBox_MouseMove(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
           /* double[] c = Geocoder.UTM2LL(new double[] { worldPos.X, worldPos.Y });
            Address addr = Geocoder.RGeocode(c);
            ((MainForm)this.MdiParent).SetStatusText(addr.District); */
        }

        private void mapBox_MouseDown(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            
        }

        private void mapBox_MouseUp(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            // XX = worldPos.X;
            // YY = worldPos.Y;            
        }

        private void mapBox_DoubleClick(object sender, EventArgs e)
        {
            // double[] c = Geocoder.UTM2LL(new double[] { this.XX, this.YY });
            // Address addr = Geocoder.RGeocode(c);
            // OpenOneDistrictForm(addr.District);
        }

        public void OpenOneDistrictForm(string DistrictName)
        {
            if (Config.Get("MapProvider", "None") == "None")
            {

                if (map != null && map.Visible)
                {
                    map.Close();
                }
                if (DistrictName != "")
                {
                    try
                    {
                        map = new OneDistrictForm(this.MdiParent, DistrictName);
                        map.Top = 0;
                        map.Left = 0;
                        map.Size = this.Size;
                        map.Show();
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                    }
                }
            }
            else
            {
                if (map2 != null && map2.Visible)
                {
                    map2.Close();
                }
                try
                {
                    map2 = new DistrictMapWeb(this.MdiParent, DistrictName);
                    map2.Size = this.Size;
                    map2.Top = 0;
                    map2.Left = 0;
                    map2.Show();
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                }
            }
        }

        private void OnRegionSelected(object sender, int region)
        {
            var wd = Utils.CreateWaitThread(this, Config.Get("MonitorNumber", "1").ToInt());
            if (SelectedRegion != region)
            {
                SelectRegion(region);
            }
            Utils.DestroyWaitThread(wd);
        }

        private void BuildTreeViewThreadSafe(List<object[]> regions)
        {
            treeView1.BeginUpdate();
            int i = -1;
            if (treeView1.SelectedNode != null)
            {
                i = treeView1.SelectedNode.Index;
            }
            treeView1.Nodes.Clear();
            foreach (object[] region in regions)
            {
                TreeNode node = new TreeNode();
                node.Name = region[1].ToString();
                node.Text = region[0].ToString();
                bool flag = false;
                Regions.TryGetValue(region[1].ToInt(), out flag);
                // раскрашиваем только когда объект на связи
                node.BackColor = ColorTranslator.FromHtml(region[2].ToString());
                node.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(region[2].ToString()));
                node.NodeFont = DefaultFont;
                if (flag)
                {
                    node.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
                }
                treeView1.Nodes.Add(node);
            }
            if (i >= 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[i];
            }
            treeView1.EndUpdate();
        }

        private void BuildTreeView()
        {
            List<object[]> regions = DataBase.RowSelect(
                @"select distinct rm.fullname, rm.num, rm.color, rm.name 
                    from public.regions2map rm
                      inner join oko.ipaddresses ip on rm.num = ip.id_region
                    where ip.listen
                    order by name");
            if (treeView1.InvokeRequired)
                treeView1.BeginInvoke(new Action(() => { BuildTreeViewThreadSafe(regions); }));
            else
                BuildTreeViewThreadSafe(regions);
        }

        /// <summary>
        /// Реакция на тревожное сообщение
        /// </summary>
        /// <param name="ObjNumber"></param>
        public void Alarm(int ObjNumber, string Message)
        {
            // ((MainForm)this.MdiParent).SetAlarmText(Message);
            // (new WarningForm(Message, Header)).ShowDialog();
            
            //simpleSound.Play();
            //((MainForm)MdiParent).DBEventsFocus();
            // MessageBox.Show("Сообщение получено");
         /*   
            AKObject obj = new AKObject(ObjNumber);
            if (obj.number != 0)
            {
                WarnMonitor.AddRow(ObjNumber, DateTime.Now.ToShortDateString() + "" + DateTime.Now.ToShortTimeString(), Message, obj.AddressStr, obj.color);
            }
            else
            {
                WarnMonitor.AddRow(ObjNumber, DateTime.Now.ToShortDateString() + "" + DateTime.Now.ToShortTimeString(), Message, "", Color.White);
            }
            
            if(!WarnMonitor.Visible)
            {
                WarnMonitor.Show();
            }
          */
        }

        public void Notify(int ObjNumber, string Message)
        {
            if (Map != null && Map.Visible)
            {
                Map.Notify(ObjNumber, Message);
            }
            else
            {
                mapBox.Refresh();
                // this.DistrictBlink(ObjNunmber, 5000);
            }
        }

        private void DistrictBlink(int InnerObjectNumber, int msIntBlink)
        {
            // TODO like tools
/*            AKObject obj = new AKObject(InnerObjectNumber);
            if (obj.number > 0)
            {
                double[] pnt = Geocoder.LL2UTM(new double[] { obj.latitude, obj.longitude });
                GeoAPI.Geometries.Envelope clickPnt = new GeoAPI.Geometries.Envelope(new GeoAPI.Geometries.Coordinate(pnt[0], pnt[1]) );
                SharpMap.Data.FeatureDataSet ds = new SharpMap.Data.FeatureDataSet();
                SharpMap.Layers.VectorLayer Regions = mapBox.Map.GetLayerByName("Regions") as SharpMap.Layers.VectorLayer;
                if (!Regions.DataSource.IsOpen)
                    Regions.DataSource.Open();

                Regions.DataSource.ExecuteIntersectionQuery(clickPnt, ds);
                if (ds.Tables.Count > 0)
                {
                    SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
                    laySelected.DataSource = new GeometryProvider(ds.Tables[0]);
                    laySelected.Style.Fill = new System.Drawing.SolidBrush(Color.FromArgb(0, 0, 0, 0));
                    laySelected.Style.EnableOutline = true;
                    laySelected.Style.Outline = new Pen(Color.Red, 2);

                    mapBox.Map.Layers.Add(laySelected);
                    mapBox.Refresh();
                    Thread backgroundThread = new Thread(
                        new ThreadStart(() =>
                        {
                            Thread.Sleep(msIntBlink);
                            mapBox.Map.Layers.Remove(laySelected);
                            mapBox.Refresh();
                        }
                   ));
                    backgroundThread.Start();
                }
            }*/
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            SetScale(Convert.ToDouble(comboBox2.Text));
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            
        }

        /*
        private void SelectNodeUpd(string DistrictName)
        {
            if (SelectedRegion > 0)
            {
                groupBox2.Text = "Статистика для " + DistrictName;
                pictureBox1.Visible = true;
                LoadStat(SelectedRegion);
            }
        }*/

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectRegion(e.Node.Name.ToInt());
            //ShowStat(Utils.StatObjects((e.Node.Name)));
            // treeView1.SelectedNode = e.Node;
            //SelectedRegion = e.Node.Name.ToInt();
            //SelectNodeUpd(e.Node.Text);
            
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            WaitDialog wd = Utils.CreateWaitThread(this, Config.Get("MonitorNumber", "1").ToInt());
            // открыть карту района
            OpenOneDistrictForm(e.Node.Text);

            Utils.DestroyWaitThread(wd);
        }

        private void mapBox_Click(object sender, EventArgs e)
        {

        }

        private void mapBox_MapRefreshed(object sender, EventArgs e)
        {
            /*if (needUpdate)
            {
                LoadStat(SelectedRegion);
                needUpdate = false;
            }*/
            UpdateTime = 60;
            timer1.Start();
        }

        private void mapBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WaitDialog wd = Utils.CreateWaitThread(this, Config.Get("MonitorNumber", "1").ToInt());

            GeoAPI.Geometries.Coordinate coor = mapBox.Map.ImageToWorld(new System.Drawing.PointF(e.X, e.Y));

            double[] c = Geocoder.UTM2LL(new double[] { coor.X, coor.Y });
            Address addr = Geocoder.RGeocode(c);
            OpenOneDistrictForm(addr.District);

            Utils.DestroyWaitThread(wd);
        }

        private void DistrictMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            // oMapper.ClearCache();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBox1.SelectedItem.ToString());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DeselectRegion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshMaps();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int s = UpdateTime;
            s = s - 1;
            if (s < 1)
            {
                timer1.Stop();
                RefreshMaps();
                LoadStat(SelectedRegion);
            }
            else
            {
                UpdateTime = s;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int state_id = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToInt();
            object dogovor = dataGridView1.Rows[e.RowIndex].Cells[3].Value;
            string filter = "true";
            if (state_id > 0)
            {
                filter = "osm_id in (select osm_id from oko.object_status where state_id = {0}) and " + filter;
            }
            if (SelectedRegion != 0)
            {
                filter = "region_id = {1} and " + filter;
            }
            if (dogovor != null)
            {
                if (dogovor.ToBool())
                {
                    filter = "dogovor and " + filter;
                }
                else
                {
                    filter = "not dogovor and " + filter;
                }
            }
            Logger.Instance.WriteToLog("Фильтр объектов: " + string.Format(filter, state_id, this.SelectedRegion), Logger.LogLevel.DEBUG);
            Handling.onObjectListOpen(string.Format(filter, state_id, SelectedRegion));
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (dataGridView1.Rows.Count > 0)
            {
                Utils.MergeCellsInRow(dataGridView1, 0, 0, 1);
                Utils.MergeCellsInRow(dataGridView1, 1, 0, 1);
                Utils.MergeCellsInRow(dataGridView1, positionRow, 0, 1);
            }
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor;
                row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
            }
        }
    }
}
