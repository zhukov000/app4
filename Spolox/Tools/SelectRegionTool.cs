using App3.Class;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using SharpMap.Data.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Tools
{
    public delegate void SelectRegionHandler(object sender, int region);

    class SelectRegionTool : CustomMapTool
    {
        public event SelectRegionHandler OnSelected = null;

        private Thread backgroundThread = null;

        protected override void OnMapControlChanging(CancelEventArgs cea)
        {
            base.OnMapControlChanging(cea);
            if (cea.Cancel) return;

            if (MapControl == null) return;
            MapControl.MouseDown -= HandleMouseDown;
        }

        protected override void OnMapControlChanged(EventArgs e)
        {
            base.OnMapControlChanged(e);
            if (MapControl == null) return;
            MapControl.MouseDown += HandleMouseDown;
        }

        private void HandleMouseDown(GeoAPI.Geometries.Coordinate worldpos, MouseEventArgs imagepos)
        {
            if (!Enabled)
                return;

            if (imagepos.Button == MouseButtons.Left)
                return;
            
            GeoAPI.Geometries.Envelope clickPnt = new GeoAPI.Geometries.Envelope(worldpos);
            SharpMap.Layers.VectorLayer Regions = MapControl.Map.GetLayerByName("Regions") as SharpMap.Layers.VectorLayer;
            if (!Regions.DataSource.IsOpen)
                Regions.DataSource.Open();
            
            SharpMap.Layers.VectorLayer laySelected = MapControl.Map.GetLayerByName("SelectionRegion") as SharpMap.Layers.VectorLayer;
            NetTopologySuite.Geometries.GeometryFactory gf = new NetTopologySuite.Geometries.GeometryFactory();
            IGeometry geometry = null;
            
            SharpMap.Data.FeatureDataSet ds = new SharpMap.Data.FeatureDataSet();
            foreach(IGeometry geom in Regions.DataSource.GetGeometriesInView(Regions.Envelope) )
            {
                if (geom.Contains(gf.ToGeometry(clickPnt)))
                {
                    geometry = geom;
                    Regions.DataSource.ExecuteIntersectionQuery(gf.ToGeometry(clickPnt).EnvelopeInternal, ds);
                    break;
                }
            }
            int DistrictId = 0;
            if (geometry != null) 
            {
                if (laySelected == null)
                {
                    laySelected = new SharpMap.Layers.VectorLayer("SelectionRegion");
                    MapControl.Map.Layers.Add(laySelected);
                }
                laySelected.DataSource = new GeometryProvider(geometry);
                laySelected.Style.Fill = new System.Drawing.SolidBrush(Color.OrangeRed);
                laySelected.Style.EnableOutline = true;
                laySelected.Style.Outline = new Pen(Color.Black, 2);
                // событие о выборе района
                if (OnSelected != null)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DistrictId = ds.Tables[0].Rows[0]["num"].ToInt();
                    }
                    else
                    {
                        double[] c = Geocoder.UTM2LL(new double[] { worldpos.X, worldpos.Y });
                        Address addr = Geocoder.RGeocode(c);
                        DistrictId = addr.DistrictId;
                    }

                }
            }
            else
            {
                if (laySelected != null)
                {
                    MapControl.Map.Layers.Remove(laySelected);
                }
            }
            if (OnSelected != null)
            {
                if (backgroundThread != null )
                {
                    backgroundThread.Abort();
                }
                backgroundThread = new Thread(
                        new ThreadStart(() =>
                        {
                            Thread.Sleep(1000);
                            OnSelected(this, DistrictId);
                        }
                    ));
                backgroundThread.IsBackground = true;
                backgroundThread.Start();
            }
            MapControl.Refresh();
        }
    }
}
