using App3.Class;
using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LayerType = App3.Class.GeoDataCache.LayerType;

namespace App3.Forms.Map
{
    public partial class PointSelect : Form
    {
        public delegate void MapBoxClick(double lat, double lon, Address addr);
        MapBoxClick mapBoxClick = null;

        private Mapper oMapper = new Mapper();

        public void AddClickEvents(MapBoxClick pMethod)
        {
            mapBoxClick += pMethod;
        }

        public PointSelect()
        {
            InitializeComponent();
        }

        public PointSelect(Form pParent)
        {
            MdiParent = pParent;
            InitializeComponent();            
        }

        private void PointSelect_Load(object sender, EventArgs e)
        {
            mapBox.BackColor = Color.White;
            if (districtBox.Items.Count == 0)
            {
                // инициализация карты районов
                mapBox.Map = oMapper.InitializeRegionMap();
                // загрузка районов
                DBDict.Load2Combobox(ref districtBox,
                    DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    null
                );
            }
            mapBox.Map.ZoomToExtents();
            mapBox.Refresh();
            mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        public void SelectRegion(string pName)
        {
            // загрузить карту региона
            oMapper.Clear();
            oMapper.DistrictName = pName;
            UpdateDistrictMap();
            panel2.Visible = true;
        }

        public void SelectRegion(int pNumber)
        {
            if (DBDict.TRegion.ContainsKey(pNumber))
            {
                if (districtBox.Items.Count > 0)
                {
                    SelectRegion(DBDict.TRegion[pNumber].Item1);
                }
                else
                {
                    DBDict.Load2Combobox(ref districtBox,
                        DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                        pNumber
                    );
                }
            }
        }

        private void PointSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            oMapper.ClearCache();
        }

        private void districtBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = districtBox.SelectedIndex;
            if (i >= 0)
            {
                ComboboxItem selVal = (ComboboxItem)districtBox.Items[i];
                // загрузить карту региона
                SelectRegion(selVal.Text);
                /*oMapper.Clear();
                oMapper.DistrictName = selVal.Text;
                UpdateDistrictMap();
                panel2.Visible = true;*/
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDistrictMap();
        }

        private Stopwatch stopWatch = new Stopwatch();

        private void UpdateDistrictMap()
        {
            mapBox.Map = oMapper.InitializeDistrictMap(
                        new bool[] { 
                            plgBox.Checked, roadBox.Checked, buildBox.Checked
                        }
                    );
            mapBox.Map.BackColor = Color.FromArgb(243, 226, 169);
            mapBox.Map.MinimumZoom = 1e3;
            mapBox.Map.MaximumZoom = 7e5;
            mapBox.Map.ZoomToExtents();
            stopWatch.Restart();
            mapBox.Refresh();
            mapBox.MapRefreshed += mapBox_MapRefreshed;
        }

        void mapBox_MapRefreshed(object sender, EventArgs e)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            labelTime.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
        }

        private void mapBox_MouseDown(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (mapBoxClick != null && imagePos.Button == MouseButtons.Right)
            {
                panel4.Visible = true;
                double[] c = Geocoder.UTM2LL(new double[] { worldPos.X, worldPos.Y });
                Address addr = Geocoder.RGeocode(c);

                string s = string.Format("Вы выбрали точку с координатами {0}°,{1}°, для которой был автоматически определен адрес: {2}.\r\nПодтвержаете свой выбор?", c[0], c[1], addr.FullAddress);
            
                if (MessageBox.Show(s, "Вопрос", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK )
                {
                    mapBoxClick(worldPos.X, worldPos.Y, addr);
                    Close();
                }
                panel4.Visible = false;
            }
        }


    }
}
