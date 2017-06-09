using App3.Class.Map;
using App3.Class.Static;
using SharpMap;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using SharpMap.Rendering;
using SharpMap.Rendering.Thematics;
using SharpMap.Styles;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace App3.Class
{

    internal class Mapper
    {
        private string districtName;

        private int districtId;

        public int DistrictId
        {
            get
            {
                return this.districtId;
            }
            set
            {
                this.districtId = value;
                this.districtName = DBDict.TRegion[this.districtId].Item2;
            }
        }

        public string DistrictName
        {
            get
            {
                return this.districtName;
            }
            set
            {
                this.districtName = value;
                this.districtId = DBDict.TDistrict[this.districtName].Item1;
            }
        }

        public SharpMap.Map InitializeRegionMap()
        {
            SharpMap.Map expr_05 = new SharpMap.Map();
            expr_05.BackColor = Color.FromArgb(167, 232, 232);
            VectorLayer vLayer = LayerCache.GetVLayer("regions", 0);
            vLayer.SmoothingMode = SmoothingMode.None;
            vLayer.Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeRegion));
            LabelLayer lLayer = LayerCache.GetLLayer("regions", 0, "name");
            lLayer.MultipartGeometryBehaviour = LabelLayer.MultipartGeometryBehaviourEnum.Largest;
            lLayer.Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeLabel));
            lLayer.LabelFilter = new LabelCollisionDetection.LabelFilterMethod(LabelCollisionDetection.ThoroughCollisionDetection);
            lLayer.MaxVisible = 700000.0;
            expr_05.Layers.Add(vLayer);
            expr_05.Layers.Add(lLayer);
            return expr_05;
        }

        public SharpMap.Map InitializeDistrictMap(bool[] layers = null)
        {
            if (layers == null)
            {
                layers = new bool[]
                {
                    true,
                    true,
                    true
                };
            }
            SharpMap.Map map = new SharpMap.Map();
            VectorLayer vLayer = LayerCache.GetVLayer("region", this.districtId);
            vLayer.Style.EnableOutline = true;
            vLayer.Style.Outline = new Pen(Color.FromArgb(167, 232, 232), 2f);
            vLayer.Style.Fill = new SolidBrush(Color.FromArgb(206, 246, 236));
            map.Layers.Add(vLayer);
            VectorLayer[] array = new VectorLayer[3];
            if (layers[0])
            {
                array[0] = LayerCache.GetVLayer(LayerType.BigPoly, this.districtId);
                array[0].Style.EnableOutline = true;
                array[0].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                map.Layers.Add(array[0]);
                array[1] = LayerCache.GetVLayer(LayerType.MidPoly, this.districtId);
                array[1].Style.EnableOutline = true;
                array[1].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                array[1].MaxVisible = 24999.0;
                map.Layers.Add(array[1]);
                array[2] = LayerCache.GetVLayer(LayerType.SmlPoly, this.districtId);
                array[2].Style.EnableOutline = true;
                array[2].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                array[2].MaxVisible = 10000.0;
                map.Layers.Add(array[1]);
            }
            if (layers[1])
            {
                VectorLayer vLayer2 = LayerCache.GetVLayer(LayerType.HighWay, this.districtId);
                vLayer2.Style.Line = new Pen(Color.SandyBrown, 2f);
                vLayer2.Style.Outline = new Pen(Color.Black);
                vLayer2.Style.EnableOutline = true;
                map.Layers.Add(vLayer2);
                layers[1] = false;
            }
            if (layers[2])
            {
                VectorLayer vLayer3 = LayerCache.GetVLayer(LayerType.Build, this.districtId);
                vLayer3.SmoothingMode = SmoothingMode.None;
                vLayer3.Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeBuilding));
                vLayer3.MaxVisible = 10000.0;
                map.Layers.Add(vLayer3);
            }
            if (layers[0])
            {
                LabelLayer[] array2 = new LabelLayer[3];
                array2[0] = LayerCache.GetLLayer(LayerType.BigPoly, this.DistrictId, "name");
                array2[0].MinVisible = 25000.0;
                array2[0].Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeLabel));
                map.Layers.Add(array2[0]);
                array2[1] = LayerCache.GetLLayer(LayerType.MidPoly, this.DistrictId, "name");
                array2[1].MaxVisible = 25000.0;
                array2[1].Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeLabel));
                map.Layers.Add(array2[1]);
                array2[2] = LayerCache.GetLLayer(LayerType.SmlPoly, this.DistrictId, "name");
                array2[2].MaxVisible = 9999.0;
                array2[2].Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeLabel));
                map.Layers.Add(array2[2]);
            }
            if (layers[1])
            {
                LabelLayer lLayer = LayerCache.GetLLayer(LayerType.HighWay, this.districtId, "name");
                lLayer.MaxVisible = 14999.0;
                map.Layers.Add(lLayer);
            }
            if (layers[2])
            {
                LabelLayer lLayer2 = LayerCache.GetLLayer(LayerType.Build, this.districtId, "name");
                lLayer2.MaxVisible = 1999.0;
                lLayer2.Theme = new CustomTheme(new CustomTheme.GetStyleMethod(GeoStyles.ThemeLabel));
                map.Layers.Add(lLayer2);
            }
            return map;
        }

        public string TableName(string pType, int RegionId)
        {
            return LayerCache.Get(pType, RegionId).TableName;
        }

        public string UpdateDataTable(LayerType pType, bool force = false)
        {
            throw new Exception("Deprecated");
        }

        private void updateOsmId(string table)
        {
            throw new Exception("Deprecated");
        }

        public VectorLayer CreateVLayer(string table, string name)
        {
            return new VectorLayer(name)
            {
                DataSource = new PostGIS(DataBase.ConnectionString, table, "way", "osm_id")
            };
        }

        public VectorLayer CreateVLayer(LayerType type, string name)
        {
            return new VectorLayer(name)
            {
                DataSource = this.GetProvider(type)
            };
        }

        public IProvider GetProvider(LayerType pType)
        {
            throw new Exception("Deprecated");
        }

        public LabelLayer CreateLabelLayer(VectorLayer Layer, string name, string col = "name")
        {
            LabelLayer labelLayer = new LabelLayer(name);
            labelLayer.DataSource = Layer.DataSource;
            labelLayer.Enabled = true;
            labelLayer.LabelColumn = col;
            labelLayer.Style = new LabelStyle();
            if (new string[]
            {
                "HighwayLabelLayer"
            }.Contains(name))
            {
                labelLayer.Style.IsTextOnPath = true;
            }
            labelLayer.Style.ForeColor = Color.Black;
            labelLayer.Style.Font = new Font(FontFamily.GenericSerif, 16f);
            labelLayer.Style.Offset = new PointF(3f, 3f);
            labelLayer.Style.HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left;
            labelLayer.Style.VerticalAlignment = LabelStyle.VerticalAlignmentEnum.Bottom;
            labelLayer.TextRenderingHint = TextRenderingHint.AntiAlias;
            labelLayer.SmoothingMode = SmoothingMode.AntiAlias;
            labelLayer.SRID = Layer.SRID;
            return labelLayer;
        }

        public SharpMap.Map InitializeRegionMap_Depricated()
        {
            throw new Exception("Deprecated");
        }

        public SharpMap.Map InitializeDistrictMap_Depricated(bool[] layers = null)
        {
            throw new Exception("Deprecated");
        }
    }

}
