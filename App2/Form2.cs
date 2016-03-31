using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpMap;
using System.Drawing.Drawing2D;

namespace App2
{
    public partial class Form2 : Form
    {
        //Zoom Factor
        const float Zoom_Factor = 0.3f;

        //Data & Data Path
        string data_Connection = "Server=127.0.0.1;Port=5432;UserId=postgres;Password=postgres;Database=openstreetmap";
        string tablename = "planet_osm_roads";
        string idColumn = "osm_id";
        string geomCol = "way";

        public Form2()
        {
            InitializeComponent();

            SharpMap.Map myMap = new SharpMap.Map(new System.Drawing.Size(500, 250));
            mapBox1.BackColor = Color.LightBlue;

            SharpMap.Layers.VectorLayer layRoads = new SharpMap.Layers.VectorLayer("Roads");
            layRoads.DataSource = new SharpMap.Data.Providers.PostGIS(data_Connection, tablename, geomCol, idColumn);
            SharpMap.Layers.LabelLayer layRoadLabel = new SharpMap.Layers.LabelLayer("Road labels");
            layRoadLabel.DataSource = layRoads.DataSource;
            layRoadLabel.Enabled = true;
            layRoadLabel.LabelColumn = "name";
            //Set the label style
            layRoadLabel.Style = new SharpMap.Styles.LabelStyle();
            layRoadLabel.Style.ForeColor = Color.Black;
            layRoadLabel.Style.Font = new Font(FontFamily.GenericSerif, 11);
            layRoadLabel.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Left;
            layRoadLabel.Style.VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Bottom;
            layRoadLabel.Style.Offset = new PointF(3, 3);
            layRoadLabel.Style.Halo = new Pen(Color.Yellow, 2);
            layRoadLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            layRoadLabel.SmoothingMode = SmoothingMode.AntiAlias;
            layRoadLabel.SRID = layRoads.SRID;
            
                        //Create the style for Land
                        SharpMap.Styles.VectorStyle landStyle = new SharpMap.Styles.VectorStyle();
                        landStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(232, 232, 232));
                        /*
                                                //Create the style for Water
                                                SharpMap.Styles.VectorStyle waterStyle = new SharpMap.Styles.VectorStyle();
                                                waterStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(198, 198, 255));

                                                //Create the theme items
                                                Dictionary<string, SharpMap.Styles.IStyle> styles = new Dictionary<string, SharpMap.Styles.IStyle>();
                                                //styles.Add("land", landStyle);
                                                styles.Add("water", waterStyle);

                                                //Assign the theme
                                                layRoads.Theme = new SharpMap.Rendering.Thematics.UniqueValuesTheme<string>("class", styles, landStyle);
            
                                                ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
                                                layRoads.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                                                layRoads.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
                                                */
            myMap.Layers.Add(layRoads);
            myMap.Layers.Add(layRoadLabel);

            // SharpMap.Layers.TileLayer tile = new SharpMap.Layers.TileLayer()

/*            myMap.BackgroundLayer.Add(new SharpMap.Layers.TileAsyncLayer(
                        new BruTile.Web.OsmTileSource(), "OSM"));
            */
            // Render map
            mapBox1.Map = myMap;
            mapBox1.Map.ZoomToExtents();            
            RefreshMap();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void RefreshMap()
        {
            mapBox1.Refresh();
            
        }
    }
}
