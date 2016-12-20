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

    /*
    class Mapper
    {
        #region Поля и свойства

        private Mutex session_mtx = new Mutex();
        private int session_id = 0;
        private Int64 osm_id = 0;
        private NTree<LayerType> depends;
        private IDictionary<LayerType, bool> updated;
        private string districtName = null;

        public object[] Params
        {
            get 
            {
                return new object[] 
                {
                    osm_id,
                    DistrictName
                };
            }
        }

        public string DistrictName
        {
            get
            {
                return districtName;
            }
            set
            {
                districtName = value;
                updated[LayerType.REGION] = false;
            }
        }

        public int SessionId
        {
            get 
            {
                session_mtx.WaitOne();
                if (session_id == 0)
                {
                    session_id = GeoDataCache.OpenSession();
                    if (session_id == 0) throw new Exception("Не удалось открыть сессию для просмотра карт");
                }
                session_mtx.ReleaseMutex();
                return session_id;
            }
        }

        #endregion

        public Mapper()
        {
            #region Инициализация справочников зависимостей кэш-таблиц
            depends = new NTree<LayerType>(LayerType.REGION);
            NTree<LayerType> a = depends.AddChild(LayerType.POLYGON);
            depends.AddChild(LayerType.BUILD);
            a.AddChild(LayerType.PLACES);
            a = depends.AddChild(LayerType.ROAD);
            a.AddChild(LayerType.HIGHWAY);
            a.AddChild(LayerType.BUILD_BORDER);
            depends.AddChild(LayerType.POINT);
            depends.AddChild(LayerType.OBJECT);
            depends.AddChild(LayerType.BIG_POLYGON);
            depends.AddChild(LayerType.MID_POLYGON);
            depends.AddChild(LayerType.SML_POLYGON);

            Clear();
            #endregion
        }

        public void Clear()
        {
            osm_id = 0;
            updated = new Dictionary<LayerType, bool>();
            foreach (LayerType lType in GeoDataCache.tableName.Keys)
            {
                updated.Add(lType, false);
            }
        }

        public void ClearCache()
        {
            if (session_id != 0)
            {
                GeoDataCache.CloseSession(session_id);
            }
            session_mtx.WaitOne();
            session_id = 0;
            session_mtx.ReleaseMutex();
        }

        public string TableName(LayerType pType)
        {
            return GeoDataCache.TableName(SessionId, pType);
        }

        public SharpMap.Layers.VectorLayer CreateVLayer(String table, String name)
        {
            SharpMap.Layers.VectorLayer Layer = new SharpMap.Layers.VectorLayer(name);
            Layer.DataSource = new SharpMap.Data.Providers.PostGIS(
                DataBase.ConnectionString,
                table,
                "way",
                "osm_id"
            );
            return Layer;
        }

        public SharpMap.Layers.VectorLayer CreateVLayer(LayerType type, String name)
        {
            SharpMap.Layers.VectorLayer Layer = new SharpMap.Layers.VectorLayer(name);
            Layer.DataSource = GetProvider(type);
            return Layer;
        }

        /// <summary>
        /// Обновление с соблюдением зависимостей
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public string UpdateDataTable(LayerType pType, bool force = false)
        {
            List<LayerType> deps = new List<LayerType>();
            depends.RSearch(pType, ref deps);
            string sTableName = "";

            foreach(LayerType lType in deps)
            {
                if (!updated[lType] || force)
                {
                    sTableName = GeoDataCache.UpdateDataTable(SessionId, lType, Params);
                    updated[lType] = true;
                    force = true; // если обновится таблица верхнего уровня, то обновить и всех детей
                    if (lType == LayerType.REGION) updateOsmId(sTableName);
                }
            }
            sTableName = GeoDataCache.UpdateDataTable(SessionId, pType, Params);
            updated[pType] = true;
            return sTableName;
        }

        private void updateOsmId(string table)
        {
            osm_id = Convert.ToInt64(DataBase.First("SELECT osm_id FROM " + table, "osm_id"));
        }

        public SharpMap.Data.Providers.IProvider GetProvider(LayerType pType)
        {
            string table = UpdateDataTable(pType);

            if (pType == LayerType.REGION) updateOsmId(table);

            return new SharpMap.Data.Providers.PostGIS(
                DataBase.ConnectionString,
                table,
                "way",
                "osm_id"
            );
        }

        public SharpMap.Layers.LabelLayer CreateLabelLayer(SharpMap.Layers.VectorLayer Layer, string name, string col = "name")
        {
            SharpMap.Layers.LabelLayer layLabel = new SharpMap.Layers.LabelLayer(name);

            // layLabel.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.Largest;
            // layLabel.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
            // layLabel.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;

            layLabel.DataSource = Layer.DataSource;
            layLabel.Enabled = true;
            layLabel.LabelColumn = col;
            
            layLabel.Style = new SharpMap.Styles.LabelStyle();
            layLabel.Style.ForeColor = Color.Black;
            layLabel.Style.Font = new Font(FontFamily.GenericSerif, 16);
            layLabel.Style.Offset = new PointF(3, 3);
            layLabel.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Left;
            layLabel.Style.VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Bottom;
            // layLabel.Style.Halo = new Pen(Color.Green, 2);
            layLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            layLabel.SmoothingMode = SmoothingMode.AntiAlias; 
            layLabel.SRID = Layer.SRID;
            return layLabel;
        }

        public SharpMap.Map InitializeRegionMap()
        {
            SharpMap.Map pMap = new SharpMap.Map();
            pMap.BackColor = Color.FromArgb(167, 232, 232);
            // слой с регионами
            SharpMap.Layers.VectorLayer Regions = CreateVLayer("regions2map", "Regions");
            Regions.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            Regions.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeRegion);
            // Regions.DataSource
            // слой с подписями
            SharpMap.Layers.LabelLayer layLabel = CreateLabelLayer(Regions, "RegionLabel", "name");
            layLabel.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.Largest;
            layLabel.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
            layLabel.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;
            layLabel.MaxVisible = 7e5;

            // добавление слоев и подготовка карты
            pMap.Layers.Add(Regions);
            pMap.Layers.Add(layLabel);
            return pMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layers">Полигоны, дороги, здания</param>
        /// <returns></returns>
        public SharpMap.Map InitializeDistrictMap(bool[] layers = null)
        {
            if (layers == null)
            {
                layers = new bool[] { true, true, true };
            }
            SharpMap.Map pMap = new SharpMap.Map();
            // Главный слой
            SharpMap.Layers.VectorLayer MainLayer = CreateVLayer(LayerType.REGION, "Region");
            MainLayer.Style.EnableOutline = true;
            MainLayer.Style.Outline = new Pen(Color.FromArgb(167, 232, 232), 2);
            MainLayer.Style.Fill = new SolidBrush(Color.FromArgb(206, 246, 236));
            pMap.Layers.Add(MainLayer);
            SharpMap.Layers.VectorLayer[] PlgLayer = new SharpMap.Layers.VectorLayer[3];
            SharpMap.Layers.VectorLayer HighwayLayer = null;
            SharpMap.Layers.VectorLayer BuildingsLayer = null;
            // Полигоны
            if (layers[0])
            {
                // PlgLayer = CreateVLayer(LayerType.POLYGON, "Polygon");
                // PlgLayer.Style.EnableOutline = true;
                // PlgLayer.Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                // pMap.Layers.Add(PlgLayer);
                
                PlgLayer[0] = CreateVLayer(LayerType.BIG_POLYGON, "Polygon_BIG");
                PlgLayer[0].Style.EnableOutline = true;
                PlgLayer[0].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                pMap.Layers.Add(PlgLayer[0]);

                PlgLayer[1] = CreateVLayer(LayerType.MID_POLYGON, "Polygon_MID");
                PlgLayer[1].Style.EnableOutline = true;
                PlgLayer[1].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                PlgLayer[1].MaxVisible = 25e3 - 1;
                pMap.Layers.Add(PlgLayer[1]);

                PlgLayer[2] = CreateVLayer(LayerType.SML_POLYGON, "Polygon_SML");
                PlgLayer[2].Style.EnableOutline = true;
                PlgLayer[2].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                PlgLayer[2].MaxVisible = 10e3;
                pMap.Layers.Add(PlgLayer[1]);
            }
            if (layers[1])
            {
                // Дороги
                HighwayLayer = CreateVLayer(LayerType.HIGHWAY, "Highway");
                HighwayLayer.Style.Line = new Pen(Color.SandyBrown, 2);
                HighwayLayer.Style.Outline = new Pen(Color.Black);
                HighwayLayer.Style.EnableOutline = true;
                pMap.Layers.Add(HighwayLayer);
            }
            if (layers[2])
            {
                // Здания
                BuildingsLayer = CreateVLayer(LayerType.BUILD, "Buildings");
                BuildingsLayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                BuildingsLayer.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeBuilding);
                BuildingsLayer.MaxVisible = 10e3;
                pMap.Layers.Add(BuildingsLayer);
                //SharpMap.Layers.VectorLayer BuildingsLayer2 = CreateVLayer(LayerType.BUILD_BORDER, "Buildings_Border");
                //BuildingsLayer2.Style.Line = new Pen(Color.Black); ;
                //BuildingsLayer2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                //pMap.Layers.Add(BuildingsLayer2);
            }
            if (layers[0])
            {
                SharpMap.Layers.LabelLayer[] PolygonLabelLayer = new SharpMap.Layers.LabelLayer[3];
                PolygonLabelLayer[0] = CreateLabelLayer(PlgLayer[0], "PolygonLabel_BIG");
                PolygonLabelLayer[0].MinVisible = 25e3;
                PolygonLabelLayer[0].Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
                pMap.Layers.Add(PolygonLabelLayer[0]);

                PolygonLabelLayer[1] = CreateLabelLayer(PlgLayer[1], "PolygonLabel_MID");
                PolygonLabelLayer[1].MaxVisible = 25e3;
                PolygonLabelLayer[1].Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
                pMap.Layers.Add(PolygonLabelLayer[1]);

                PolygonLabelLayer[2] = CreateLabelLayer(PlgLayer[1], "PolygonLabel_SML");
                PolygonLabelLayer[2].MaxVisible = 10e3 - 1;
                PolygonLabelLayer[2].Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
                pMap.Layers.Add(PolygonLabelLayer[2]);

            }
            if (layers[1])
            {
                SharpMap.Layers.LabelLayer HighwayLabelLayer = CreateLabelLayer(HighwayLayer, "HighwayLabelLayer");
                HighwayLabelLayer.MaxVisible = 25e3 - 1;
                pMap.Layers.Add(HighwayLabelLayer);
            }
            if (layers[2])
            {
                SharpMap.Layers.LabelLayer BuildingLabelLayer = CreateLabelLayer(BuildingsLayer, "BuildingLabel");
                BuildingLabelLayer.MaxVisible = 2e3 - 1;
                BuildingLabelLayer.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
                pMap.Layers.Add(BuildingLabelLayer);
            }
            
            return pMap;
        }
    }
    */
}
