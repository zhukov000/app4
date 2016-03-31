// using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoAPI;
using System.Globalization;
using App3.Class;

namespace App3.Forms
{
    public partial class MapForm : Form
    {
        public delegate void MapBoxClick(double lat, double lon);
        MapBoxClick mapBoxClick = null;

        public void AddClickEvents(MapBoxClick pMethod)
        {
            mapBoxClick += pMethod;
        }

        public MapForm(Form Parent)
        {
            InitializeComponent();

            MdiParent = Parent;

            mapBox1.BackColor = Color.White;
            InitMap();
        }

       /*
        public static SharpMap.Layers.LabelLayer CreateLabel(SharpMap.Layers.VectorLayer Layer, string name, string col = "name")
        {
            SharpMap.Layers.LabelLayer layLabel = new SharpMap.Layers.LabelLayer(name);
            layLabel.DataSource = Layer.DataSource;
            layLabel.Enabled = true;
            layLabel.LabelColumn = col;
            layLabel.Style = new SharpMap.Styles.LabelStyle();
            layLabel.Style.ForeColor = Color.Black;
            layLabel.Style.Font = new Font(FontFamily.GenericSerif, 16);
            layLabel.Style.Offset = new PointF(3, 3);
            layLabel.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Left;
            layLabel.Style.VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Bottom;
            layLabel.Style.Halo = new Pen(Color.Green, 2);
            layLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            layLabel.SmoothingMode = SmoothingMode.AntiAlias;
            layLabel.SRID = Layer.SRID;
            return layLabel;
        }
        
        private SharpMap.Styles.VectorStyle GetRegionStyle(SharpMap.Data.FeatureDataRow row)
        {
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();
            style.EnableOutline = true;
            style.Outline = new Pen(Color.FromArgb(167, 232, 232), 2);
            style.Outline.DashStyle = DashStyle.Dot;
            style.Fill = new SolidBrush(System.Drawing.ColorTranslator.FromHtml(row["color"].ToString()));
            return style;
        }

        private SharpMap.Styles.LabelStyle GetLabelStyle(SharpMap.Data.FeatureDataRow row)
        {
            string s = row["name"].ToString();
            if (s.Length > 10)
            {
                row["name"] += "_"; // Fix Bug with reder label
            }
            SharpMap.Styles.LabelStyle Style = new SharpMap.Styles.LabelStyle
            {
                ForeColor = Color.DarkBlue,
                Font = new Font(FontFamily.GenericSansSerif, 11),
                HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Center,
                VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Middle,
                CollisionBuffer = new SizeF(5, 5),
                Halo = new Pen(Color.Azure, 2),
                CollisionDetection = true,
                MaxVisible = 90,
                MinVisible = 30
            };
            return Style;
        } */

        private void InitMap()
        {
/*
            SharpMap.Map myMap = new SharpMap.Map(mapBox1.Size);
            myMap.MinimumZoom = 1e3;
            myMap.MaximumZoom = 1.7e6;

            SharpMap.Layers.VectorLayer layCountries = CreateLayer("borders_country", "BCountry");
            layCountries.Style.Line = new Pen(Color.FromArgb(167, 232, 232), 3);

            SharpMap.Layers.VectorLayer layRegions = CreateLayer("borders_region", "BRegion");
            layRegions.Style.Line = new Pen(Color.FromArgb(167, 232, 232), 1);
            layRegions.Style.Line.DashStyle = DashStyle.Dot;
            layRegions.Style.Fill = Brushes.WhiteSmoke;

            SharpMap.Layers.VectorLayer layRegions2 = CreateLayer("regions", "BRegion2");
            layRegions2.Style.Line = new Pen(Color.FromArgb(167, 232, 232), 1);
            layRegions2.Style.Line.DashStyle = DashStyle.Dot;
            layRegions2.Style.Fill = Brushes.WhiteSmoke;

            SharpMap.Layers.VectorLayer layRoads1 = CreateLayer("roads1", "Roads1");
            layRoads1.Style.Line = new Pen(Color.SandyBrown, 3);
            layRoads1.Style.Outline = new Pen(Color.Black);
            layRoads1.Style.EnableOutline = true;

            SharpMap.Layers.VectorLayer layBuildings = CreateLayer("buildings", "Buildings");
            layBuildings.Style.Line = new Pen(Color.LightGreen);
            layBuildings.Style.Fill = new System.Drawing.SolidBrush(Color.LightGreen);
            layBuildings.MaxVisible = 2e5;

            SharpMap.Layers.VectorLayer layRoads2 = CreateLayer("roads2", "Roads2");
            layRoads2.Style.Line = new Pen(Color.SandyBrown, 3);

            SharpMap.Layers.VectorLayer layRoads3 = CreateLayer("roads4", "Roads3");
            layRoads3.Style.Line = new Pen(Color.SandyBrown, 2);
            layRoads3.MaxVisible = 1e5;

            SharpMap.Layers.VectorLayer layTowns = CreateLayer("towns", "Towns");
            layTowns.Style.PointColor = new System.Drawing.SolidBrush(Color.Green);
            layTowns.Style.PointSize = 5;
            SharpMap.Layers.LabelLayer layTownsLabel = CreateLabel(layTowns, "Town labels");
            layTowns.MaxVisible = 5e5;
            layTownsLabel.MaxVisible = 5e5;
            layTownsLabel.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GetLabelStyle);

            SharpMap.Layers.VectorLayer layCities = CreateLayer("cities", "Cities");
            layCities.Style.PointColor = new System.Drawing.SolidBrush(Color.Green);
            layCities.Style.PointSize = 5;
            SharpMap.Layers.LabelLayer layCitiesLabel = CreateLabel(layCities, "City labels");

            // слой с регионами
            SharpMap.Layers.VectorLayer Regions = MapForm.CreateLayer("regions2map", "Regions");
            Regions.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            Regions.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GetRegionStyle);


            myMap.Layers.Add(layCountries);
            myMap.Layers.Add(layRegions);
            myMap.Layers.Add(layRegions2);
            myMap.Layers.Add(Regions);

            myMap.Layers.Add(layRoads3);
            myMap.Layers.Add(layRoads1);
            myMap.Layers.Add(layRoads2);

            myMap.Layers.Add(layBuildings);

            myMap.Layers.Add(layTowns);
            myMap.Layers.Add(layTownsLabel);
            myMap.Layers.Add(layCities);
            myMap.Layers.Add(layCitiesLabel);

            // myMap.Layers.Add(LayerObject);

            mapBox1.Map = myMap;

            mapBox1.Map.ZoomToExtents();

            RefreshMap();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan; */
        }

        private void MapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void RefreshMap()
        {
            mapBox1.Refresh();
        }

        private void MapForm_Resize(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.ZoomWindow;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            mapBox1.Map.ZoomToExtents();
            RefreshMap();
        }

        private void mapBox1_MapChanged(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void mapBox1_MouseDown_1(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (mapBoxClick != null && imagePos.Button == MouseButtons.Right)
            {
                mapBoxClick(worldPos.X, worldPos.Y);
                this.Close();
            }
        }

        private void mapBox1_MouseMove(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            /*var ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            var wgs84 = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
            var utm33 = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WGS84_UTM(36, true);
            var transformation = ctFact.CreateFromCoordinateSystems(utm33, wgs84);
            double[] coors = transformation.MathTransform.Transform(new double[] { worldPos.X, worldPos.Y });
            coorLabel.Text = string.Format("{0} {1} => {2} {3}", worldPos.X, worldPos.Y, coors[1], coors[0]); */
            double [] c = Geocoder.UTM2LL(new double[] {worldPos.X, worldPos.Y});
            coorLabel.Text = string.Format(
                "{0} {1}", c[0].C2S(), c[1].C2S()
            );
        }
        
    }
}
