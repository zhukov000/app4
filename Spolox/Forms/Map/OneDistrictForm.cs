using App3.Class;
using App3.Class.Map;
using App3.Class.Static;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using SharpMap.Forms;
using SharpMap.Layers;
using SharpMap.Rendering.Thematics;
using SharpMap.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class OneDistrictForm : Form
    {
        // private WarningForm Mon = new WarningForm();
        private Mapper oMapper = new Mapper();

        private bool[,] memoryExpand = new bool[3,10];
        // private string DistrictName = "";
        // private string DistrictWay = "";

        public int DistrictId
        {
            get
            {
                return this.oMapper.DistrictId;
            }
        }

        public OneDistrictForm(Form Parent, int pDistrictId)
        {
            this.InitializeComponent();
            base.MdiParent = Parent;
            this.oMapper.DistrictId = pDistrictId;
            this.Text = this.oMapper.DistrictName;
            this.InitMap();
        }

        public OneDistrictForm(Form Parent, string pDistrictName)
        {
            this.InitializeComponent();
            base.MdiParent = Parent;
            this.oMapper.DistrictName = pDistrictName;
            this.Text = pDistrictName;
            this.InitMap();
        }

        private void InitMap()
        {
            this.mapBox1.Map = this.oMapper.InitializeDistrictMap(null);
            this.mapBox1.Map.BackColor = Color.FromArgb(243, 226, 169);
            this.mapBox1.Map.MinimumZoom = 1000.0;
            this.mapBox1.Map.MaximumZoom = 700000.0;
            VectorLayer vLayer = LayerCache.GetVLayer(LayerType.Object, this.DistrictId);
            vLayer.Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeObject));
            this.mapBox1.Map.Layers.Add(vLayer);
            this.mapBox1.Map.ZoomToExtents();
            this.mapBox1.Refresh();
            this.mapBox1.ActiveTool = MapBox.Tools.Pan;
            this.InitObjects();
            this.ShowScale();
        }

        /// <summary>
        /// Вывод списка узлов
        /// </summary>
        private void InitObjectsAsync()
        {
            treeView1.Nodes.Clear();
            string tableName = LayerCache.Get(LayerType.Object, this.DistrictId).TableName;
            List<object[]> list = DataBase.RowSelect(@"SELECT status_id, status, 
                                                            count(osm_id) as cnt, color, state, state_id 
                                                        FROM " + tableName +
                                                        @" GROUP BY status_id, status, state, color, state_id, rank 
                                                        ORDER BY rank");
                                                        // ORDER BY state_id DESC, status_id, status, state");

            int AllCount = 0;
            foreach (object[] obj in list)
            {
                TreeNode node = new TreeNode();
                node.Name = obj[0].ToString() + ";" + obj[5].ToString();
                node.Text = string.Format("{0}:{2} (всего-{1})", obj[1], obj[2], obj[4]);
                AllCount += obj[2].ToInt();
                node.BackColor = ColorTranslator.FromHtml(obj[3].ToString());
                node.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(obj[3].ToString()));
                treeView1.Nodes.Add(node);
                List<object[]> list2 = DataBase.RowSelect(string.Format(@"SELECT osm_id, name, number, color, status, status_id, state_id FROM {0} " +
                                " WHERE status_id = {1} AND state_id = {2} ORDER BY number", tableName, obj[0], obj[5]));
                
                foreach (object[] obj2 in list2)
                {
                    TreeNode node2 = new TreeNode();
                    node2.Name = obj2[0].ToString();
                    node2.Text = string.Format("{0} ({1})", obj2[1], obj2[2]);
                    node2.ToolTipText = obj2[4].ToString();
                    node2.BackColor = ColorTranslator.FromHtml(obj2[3].ToString());
                    node2.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(obj2[3].ToString()));
                    node.Nodes.Add(node2);
                    
                }
                if (memoryExpand[obj[0].ToInt(), obj[5].ToInt()])
                {
                    node.Expand();
                }
            }
            groupBox1.Text = string.Format("Объекты в районе (всего - {0})", AllCount);
        }

        public void InitObjects()
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.BeginInvoke(new Action(() => { InitObjectsAsync();  }));
            }
            else
            {
                InitObjectsAsync();
            }
        }

        private void OneDistrictForm_Load(object sender, EventArgs e)
        {

        }

        private void OneDistrictForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // RemoveTemporary();
            // oMapper.ClearCache();
        }

        private void mapBox1_MouseMove(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
          /*  // double[] c = Geocoder.UTM2LL(new double[] { worldPos.X, worldPos.Y });
            // ((MainForm)this.MdiParent).SetStatusText(string.Format("{0} {1}", c[0].C2S(), c[1].C2S()));
            // OSMObjectInfo info = Geocoder.GetOSMObject(c, "temp_places");
            if (info != null)
            {
                // ((MainForm)this.MdiParent).SetInfoText(info.name);
            }
            else
            {
                // ((MainForm)this.MdiParent).SetInfoText("");
            } */
        }

        private void mapBox1_MouseClick(object sender, MouseEventArgs e)
        {
            long objId = GetObject(new double[] { e.X, e.Y });
            if (objId != 0)
            {
                AKObject obj = new AKObject(objId);
                ObjectSelect(obj.Id);
            }
        }

        /// <summary>
        /// Запуск мигания объекта на карте
        /// </summary>
        /// <param name="id">ID-объекта в БД</param>
        /// <param name="msIntBlink">Миллисекунд на одно мигание</param>
        /// <param name="msInt">Продолжительность мигания</param>
        /* public void ObjectBlink(Int64 id, int msIntBlink)
        {
            AKObject obj = new AKObject(id);
            if (obj.number > 0)
            {
                this.PointBlink(obj.Id, msIntBlink);
            }
        } */

        public void Notify(int ObjNumber, string Message)
        {
            mapBox1.Refresh();
            InitObjects();
        }

        public void ObjectSelect(Int64 pId)
        {
            AKObject obj = new AKObject(pId);
            if (obj.number > 0)
            {
                SharpMap.Layers.VectorLayer SelectedObject = mapBox1.Map.GetLayerByName("SelectedObject") as SharpMap.Layers.VectorLayer;
                if (SelectedObject != null)
                {
                    SharpMap.Layers.LabelLayer SelectedObjectLabel = mapBox1.Map.GetLayerByName("SelectedObjectLabel") as SharpMap.Layers.LabelLayer;
                    mapBox1.Map.Layers.Remove(SelectedObject);
                    mapBox1.Map.Layers.Remove(SelectedObjectLabel);
                }

                SharpMap.Data.FeatureDataSet ds = new SharpMap.Data.FeatureDataSet();
                SharpMap.Layers.VectorLayer Objects = mapBox1.Map.GetLayerByName("Object") as SharpMap.Layers.VectorLayer;
                if (!Objects.DataSource.IsOpen)
                    Objects.DataSource.Open();

                selectedObject.Text = string.Format( "Объект: {0}", obj.name );
                addressLabel.Text = string.Format("Адрес объекта: {0}", obj.AddressStr);

                FeatureDataRow row = Objects.DataSource.GetFeature((uint)obj.Id);
                if (row != null)
                {
                    int n = mapBox1.Map.Layers.Count;
                    double[] coor = OSMObjectInfo.GetObjectCoordinate(obj.Id, LayerCache.Get(LayerType.Object, this.DistrictId).TableName);

                    SharpMap.Layers.VectorLayer NewLayer = new SharpMap.Layers.VectorLayer("SelectedObject");
                    FeatureDataTable fdt = new SharpMap.Data.FeatureDataTable();
                    fdt.Columns.Add("name");
                    FeatureDataRow fdr = fdt.NewRow();
                    fdr.Geometry = new NetTopologySuite.Geometries.Point(coor[0], coor[1]);
                    
                    fdr["name"] = obj.name;
                    fdt.AddRow(fdr);
                    NewLayer.Style.PointSize = 15;
                    NewLayer.DataSource = new SharpMap.Data.Providers.GeometryFeatureProvider(fdt);

                    SharpMap.Layers.LabelLayer NewLabelLayer = oMapper.CreateLabelLayer(NewLayer, "SelectedObjectLabel");
                    NewLabelLayer.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Center;
                    NewLabelLayer.Style.VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Top;

                    mapBox1.Map.Layers.Add(NewLayer);
                    mapBox1.Map.Layers.Add(NewLabelLayer);

                    if (checkBox1.Checked)
                    {
                        mapBox1.Map.ZoomToBox(fdr.Geometry.EnvelopeInternal);
                    }

                    mapBox1.Refresh();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPos">Координаты объекта долгота/широта</param>
        /// <param name="msIntBlink"></param>
        private void PointBlink(Int64 id, int msIntBlink)
        {
            SharpMap.Data.FeatureDataSet ds = new SharpMap.Data.FeatureDataSet();
            SharpMap.Layers.VectorLayer Objects = mapBox1.Map.GetLayerByName("Object") as SharpMap.Layers.VectorLayer;
            if (!Objects.DataSource.IsOpen)
                Objects.DataSource.Open();

            FeatureDataRow row = Objects.DataSource.GetFeature((uint)id);
            if (row!= null)
            {
                int n = mapBox1.Map.Layers.Count;
                double [] coor = OSMObjectInfo.GetObjectCoordinate(id, "temp_objects");

                SharpMap.Layers.VectorLayer NewLayer = new SharpMap.Layers.VectorLayer(String.Format( "Selection{0}", id ));
                FeatureDataTable fdt = new SharpMap.Data.FeatureDataTable();
                FeatureDataRow fdr = fdt.NewRow();
                fdr.Geometry = new NetTopologySuite.Geometries.Point(coor[0], coor[1]);
                fdt.AddRow(fdr);
                NewLayer.DataSource = new SharpMap.Data.Providers.GeometryFeatureProvider(fdt);
                mapBox1.Map.Layers.Add(NewLayer);
                mapBox1.Refresh();
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        Thread.Sleep(msIntBlink);
                        mapBox1.Map.Layers.Remove(NewLayer);
                        mapBox1.Refresh();
                        InitObjects();
                    }
                ));
                backgroundThread.Start();
            }
        }

        private void mapBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // получаем объект, на который нажали на карте
            long objId = GetObject(new double[] { e.X, e.Y });
            // открываем для него окно
            if (objId > 0)
            {
                // (new ObjectForm(this.MdiParent, new AKObject(objId))).Show();
                Handling.onObjectCardOpen(objId);
            }
        }

        private long GetObject(double [] coor)
        {
            long objId = 0;
            double minDist = 1000; 
            PointF pntObj = new PointF((float)coor[0], (float)coor[1]);

            SharpMap.Layers.VectorLayer Objects = mapBox1.Map.GetLayerByName("Object") as SharpMap.Layers.VectorLayer;
            if (!Objects.DataSource.IsOpen)
                Objects.DataSource.Open();
            // SQL получить все точки, лежащие в районе
            DataSet ds = new DataSet();
            DataBase.RowSelect(string.Format(
                @"select obj.*, ST_AsText(obj.way) as point
                    from {0} obj, {1} reg 
                    where ST_covers(reg.way, obj.way)",
                LayerCache.Get(LayerType.Object, this.DistrictId).TableName,
                LayerCache.Get(LayerType.Region, this.DistrictId).TableName
                ),
                ds
            );
            
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                double[] r = row["point"].ToString().Substring(6).Trim('(', ')', ' ').Split(' ').Select(o => double.Parse(o, CultureInfo.InvariantCulture)).ToArray();
                PointF pnt = mapBox1.Map.WorldToImage(new Coordinate(r[0], r[1]));
                double dist = Math.Pow(pnt.X - pntObj.X, 2) + Math.Pow(pnt.Y - pntObj.Y, 2);
                if (dist < minDist)
                {
                    minDist = dist;
                    objId = row["osm_id"].ToInt();
                }
            }

            if (minDist > 10)
            {
                objId = 0;
            }

            return objId;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                ObjectSelect(e.Node.Name.ToInt64());
                // (new ObjectForm(this.MdiParent, new AKObject(e.Node.Name.ToInt64()))).Show();
                Handling.onObjectCardOpen(e.Node.Name.ToInt64());
            }
        }

        private void ShowScale()
        {
            StatusLabel.Text = string.Format("Масштаб 1:{0}", (int)mapBox1.Map.MapScale);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitMap();
        }

        private void mapBox1_MapRefreshed(object sender, EventArgs e)
        {
            ShowScale();
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            int[] ipair = e.Node.Name.Split(';').Select(x => x.ToInt()).ToArray();
            memoryExpand[ipair[0], ipair[1]] = e.Node.IsExpanded;
        }
    }
}
