using App3.Class;
using App3.Class.Map;
using App3.Class.Singleton;
using App3.Class.Static;
using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace App3.Forms.Map
{
    public partial class DistrictMapWeb : Form
    {

        public int DistrictId
        {
            get
            {
                return DBDict.TDistrict[this.Text].Item1;
            }
        }

        public DistrictMapWeb(Form Parent, string pDistrictName)
        {
            InitializeComponent();
            base.MdiParent = Parent;
            this.Text = pDistrictName;
            this.InitMap();
            this.InitObjects();
        }

        /// <summary>
        /// Параметры карты
        /// </summary>
        private void InitMap()
        {
            gMapControl1.ShowCenter = false;
            //Настройки для компонента GMap.
            gMapControl1.Bearing = 0;
            //CanDragMap - Если параметр установлен в True, пользователь может перетаскивать карту с помощью правой кнопки мыши. 
            gMapControl1.CanDragMap = true;
            //Указываем, что перетаскивание карты осуществляется  с использованием левой клавишей мыши. По умолчанию - правая.
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.GrayScaleMode = true;
            //MarkersEnabled - Если параметр установлен в True, любые маркеры, заданные вручную будет показаны. Если нет, они не появятся.
            gMapControl1.MarkersEnabled = true;
            //Указываем значение максимального приближения.
            gMapControl1.MaxZoom = 18;
            //Указываем значение минимального приближения.
            gMapControl1.MinZoom = 2;
            //Устанавливаем центр приближения/удаления
            //курсор мыши.
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            //Отказываемся от негативного режима.
            gMapControl1.NegativeMode = false;
            //Разрешаем полигоны.
            gMapControl1.PolygonsEnabled = true;
            //Разрешаем маршруты
            gMapControl1.RoutesEnabled = true;
            //Скрываем внешнюю сетку карты с заголовками.
            gMapControl1.ShowTileGridLines = false;
            //Указываем, что при загрузке карты будет использоваться 5ти кратное приближение.
            gMapControl1.Zoom = 11;
            //Указываем Провайдера.
            switch (Config.Get("MapProvider"))
            {
                case "GoogleMap":
                    gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
                    break;
                case "OpenStreetMap":
                    gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.OpenStreetMap;
                    break;
                case "BingMap":
                    gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.BingMap;
                    break;
                case "YandexMap":
                    gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.YandexMap;
                    break;
                default:
                    throw new Exception("Выбранный в конфигурации провайдер карт не поддерживается");
            }
            
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // установка позиции
            gMapControl1.SetPositionByKeywords("Ростовская область, " + this.Text);
            // отрисовка полигона
            object[] rdata = DataBase.FirstRow("select ST_AsText(ST_Multi( ST_Transform(way, 4326) ) ) poly, color from regions2map WHERE num = " + DistrictId, 0);
            if (rdata.Length > 0)
            {
                GMapOverlay polygons = new GMapOverlay("polygons");
                List<PointLatLng> points = new List<PointLatLng>();
                var poly = rdata[0].ToString().Substring(12).Split('(');
                foreach (string polystr in poly)
                {
                    if (polystr.Length == 0) continue;
                    var polypnt = polystr.TrimEnd(')').Split(',');
                    
                    foreach (string coor in polypnt)
                    {
                        try
                        {
                            var coors = coor.Split(' ');
                            if (coors[1].ToDouble() < 10) continue;
                            points.Add(new PointLatLng(coors[1].ToDouble(), coors[0].ToDouble()));
                        }
                        catch(Exception ex)
                        {
                            // Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.DEBUG);
                            continue;
                        }
                    }
                    GMapPolygon polygon = new GMapPolygon(points, this.Text);
                    polygons.Polygons.Add(polygon);
                    polygon.Fill = new SolidBrush(Color.FromArgb(50, ColorTranslator.FromHtml(rdata[1].ToString()))); // ColorTranslator.FromHtml(rdata[1].ToString())
                    polygon.Stroke = new Pen(Color.Blue, 1);
                }
                gMapControl1.Overlays.Add(polygons);
                gMapControl1.ZoomAndCenterRoutes(polygons.Id);
            }

        }

        /// <summary>
        /// Отобразить маркеры на карте
        /// </summary>
        public void InitObjects()
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.BeginInvoke(new Action(() => { InitObjectsAsync(); }));
            }
            else
            {
                InitObjectsAsync();
            }
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
            
            int AllCount = 0;
            GMap.NET.WindowsForms.GMapOverlay markers = new GMap.NET.WindowsForms.GMapOverlay("markers");
            foreach (object[] obj in list)
            {
                TreeNode node = new TreeNode();
                node.Name = obj[0].ToString() + ";" + obj[5].ToString();
                node.Text = string.Format("{0}:{2} (всего-{1})", obj[1], obj[2], obj[4]);
                AllCount += obj[2].ToInt();
                node.BackColor = ColorTranslator.FromHtml(obj[3].ToString());
                node.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(obj[3].ToString()));
                treeView1.Nodes.Add(node);
                List<object[]> list2 = DataBase.RowSelect(string.Format(@"SELECT osm_id, name, number, color, status, status_id, state_id, ST_AsText(ST_Transform(way, 4326)) as coor_txt FROM {0} " +
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
                    var coor = obj2[7].ToString().Substring(6).TrimEnd(')').Split(' ');

                    // рисуем маркер по координатам
                    GMap.NET.WindowsForms.Markers.GMarkerGoogleType type = GMap.NET.WindowsForms.Markers.GMarkerGoogleType.gray_small;
                    switch (obj2[6].ToInt())
                    {
                        case 1:
                            type = GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green;
                            break;
                        case 2:
                            type = GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue;
                            break;
                        case 6:
                            type = GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue;
                            break;
                        case 4:
                            type = GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red;
                            break;
                    }
                    GMap.NET.WindowsForms.GMapMarker marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                            new GMap.NET.PointLatLng(coor[1].ToDouble(), coor[0].ToDouble()),
                            type
                    );
                    marker.ToolTipText = node2.Text;
                    marker.ToolTip.Fill = Brushes.Black;
                    marker.ToolTip.Foreground = Brushes.White;
                    marker.ToolTip.Stroke = Pens.Black;
                    marker.ToolTip.TextPadding = new Size(20, 20);
                    marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    marker.Tag = obj2[0].ToString();

                    markers.Markers.Add(marker);
                }
                node.Expand();
            }
            gMapControl1.Overlays.Add(markers);
            groupBox1.Text = string.Format("Объекты в районе (всего - {0})", AllCount);
        }


        public void ObjectSelect(Int64 pId)
        {
            Handling.onObjectCardOpen(pId);
        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            ObjectSelect(item.Tag.ToInt64());
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                ObjectSelect(e.Node.Name.ToInt64());
            }
        }
    }
}
