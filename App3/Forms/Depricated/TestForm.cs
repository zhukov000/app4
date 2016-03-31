using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms.Map
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            f3();
        }

        public void f3()
        {
            TileLayer osmTileLayer = new TileLayer(
                new BruTile.Web.ArcGisTileSource(
                    // "http://remote.donstu.ru:6080/arcgis/rest/services/_RO_SAC_08/MapServer", 
                    "http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer",
                    new BruTile.PreDefined.SphericalMercatorInvertedWorldSchema()
                ),
                "OSM"
            );
            mapBox1.Map.Layers.Add(osmTileLayer);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
        }

        public void f2()
        {
            BruTile.Web.GoogleRequest tRec = new BruTile.Web.GoogleRequest(BruTile.Web.GoogleMapType.GoogleMap,
                        BruTile.Web.GoogleRequest.LanguageType.Russian);
            
            TileLayer osmTileLayer = new TileLayer(
                new BruTile.Web.GoogleTileSource(
                    tRec
                ),
                "OSM"
            );
            mapBox1.Map.Layers.Add(osmTileLayer);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
        }

        public void f1()
        {
            TileLayer osmTileLayer = new TileLayer(new BruTile.Web.OsmTileSource(), "OSM");
            mapBox1.Map.Layers.Add(osmTileLayer);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
        }

        public void f()
        {
            SharpMap.Layers.WmsLayer wmsL = new SharpMap.Layers.WmsLayer(
    "US Cities",
    "http://sampleserver1.arcgisonline.com/ArcGIS/services/Specialty/ESRI_StatesCitiesRivers_USA/MapServer/WMSServer");

            //Force PNG format. Else we can't see through
            wmsL.SetImageFormat("image/png");
            //Force version 1.1.0
            wmsL.Version = "1.2.0";
            //Add layer named 2 in the service (Cities)
            wmsL.AddLayer("1");
            //Set the SRID
            wmsL.SRID = 4326;

            //Add layer to map
            mapBox1.Map.Layers.Add(wmsL);
            mapBox1.Map.ZoomToExtents();
        }
    }
}
