using App3.Class;
using App3.Class.Map;
using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LayerType = App3.Class.GeoDataCache.LayerType;

namespace App3.Class
{
    class Mapper
    {
        private string districtName = null;
        private int districtId = 0;

        public int DistrictId
        {
            get
            {
                return districtId;
            }
            set
            {
                districtId = value;
                districtName = DBDict.TRegion[districtId].Item2;
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
                districtId = DBDict.TDistrict[districtName].Item1;
            }
        }

        public SharpMap.Map InitializeRegionMap()
        {
            // throw new Exception("TODO");

            SharpMap.Map pMap = new SharpMap.Map();
            pMap.BackColor = Color.FromArgb(167, 232, 232);
            // слой с регионами
            SharpMap.Layers.VectorLayer Regions = LayerCache.GetVLayer("regions"); // regions2map
            // SharpMap.Layers.VectorLayer Regions = CreateVLayer("cache.region0", "Regions"); // cache.region0
            Regions.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            Regions.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeRegion);
            // слой с подписями

            SharpMap.Layers.LabelLayer layLabel = LayerCache.GetLLayer("regions"); // CreateLabelLayer(Regions, "RegionLabel", "name");
            layLabel.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.Largest;
            layLabel.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
            layLabel.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;
            layLabel.MaxVisible = 7e5;

            // добавление слоев и подготовка карты
            pMap.Layers.Add(Regions);
            pMap.Layers.Add(layLabel);
            return pMap;
        }

        public SharpMap.Map InitializeDistrictMap(bool[] layers = null)
        {
            throw new Exception("TODO");
        }

        public string TableName(string pType, int RegionId)
        {
            throw new Exception("TODO");
        }

        /*
        #region Поля и свойства

        private Mutex session_mtx = new Mutex();
        private int session_id = 0;
        private Int64 osm_id = 0;
        private NTree<LayerType> depends;
        private IDictionary<LayerType, bool> updated;


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

        
        public int SessionId
        {
            get 
            {
                session_mtx.WaitOne();
                if (session_id == 0)
                {
                    session_id = GeoDataCache.BuildCacheForListened();
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

        public string TableName(LayerType pType, int RegionId)
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
                */
        /// <summary>
        /// Обновление с соблюдением зависимостей
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public string UpdateDataTable(LayerType pType, bool force = false)
        {
            throw new Exception("Deprecated");
            /*List<LayerType> deps = new List<LayerType>();
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
            return sTableName;*/
        }

        private void updateOsmId(string table)
        {
            throw new Exception("Deprecated");
            // osm_id = Convert.ToInt64(DataBase.First("SELECT osm_id FROM " + table, "osm_id"));
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
            string[] namesLinesLabels = new string[] { "HighwayLabelLayer" };
            if (namesLinesLabels.Contains(name))
            {
                layLabel.Style.IsTextOnPath = true;
            }

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

        public SharpMap.Map InitializeRegionMap_Depricated()
        {
            throw new Exception("Deprecated");

            SharpMap.Map pMap = new SharpMap.Map();
            pMap.BackColor = Color.FromArgb(167, 232, 232);
            // слой с регионами
            SharpMap.Layers.VectorLayer Regions = CreateVLayer(LayerType.REGION, "Regions"); // regions2map
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
        public SharpMap.Map InitializeDistrictMap_Depricated(bool[] layers = null)
        {
            throw new Exception("Deprecated");

            if (layers == null)
            {
                layers = new bool[] { true, true, true };
            }
            SharpMap.Map pMap = new SharpMap.Map();
            // Главный слой
            SharpMap.Layers.VectorLayer MainLayer = CreateVLayer(LayerType.REGION/*, RegionId*/, "Region");
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
                
                PlgLayer[0] = CreateVLayer(LayerType.BIG_POLYGON/*, RegionId*/, "Polygon_BIG");
                PlgLayer[0].Style.EnableOutline = true;
                PlgLayer[0].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                pMap.Layers.Add(PlgLayer[0]);

                PlgLayer[1] = CreateVLayer(LayerType.MID_POLYGON/*, RegionId*/, "Polygon_MID");
                PlgLayer[1].Style.EnableOutline = true;
                PlgLayer[1].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                PlgLayer[1].MaxVisible = 25e3 - 1;
                pMap.Layers.Add(PlgLayer[1]);

                PlgLayer[2] = CreateVLayer(LayerType.SML_POLYGON/*, RegionId*/, "Polygon_SML");
                PlgLayer[2].Style.EnableOutline = true;
                PlgLayer[2].Style.Fill = new SolidBrush(Color.FromArgb(224, 254, 224));
                PlgLayer[2].MaxVisible = 10e3;
                pMap.Layers.Add(PlgLayer[1]);
            }
            if (layers[1])
            {
                // Дороги
                HighwayLayer = CreateVLayer(LayerType.HIGHWAY/*, RegionId*/, "Highway");
                HighwayLayer.Style.Line = new Pen(Color.SandyBrown, 2);
                HighwayLayer.Style.Outline = new Pen(Color.Black);
                HighwayLayer.Style.EnableOutline = true;
                pMap.Layers.Add(HighwayLayer);
            }
            if (layers[2])
            {
                // Здания
                BuildingsLayer = CreateVLayer(LayerType.BUILD/*, RegionId*/, "Buildings");
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
                HighwayLabelLayer.MaxVisible = 15e3 - 1;
                pMap.Layers.Add(HighwayLabelLayer);

                // SharpMap.Layers.LabelLayer HighwayLabelLayer = new SharpMap.Layers.LabelLayer("HighwayLabelLayer");

                // layLabel.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.Largest;
                // layLabel.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
                // layLabel.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;
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
}
