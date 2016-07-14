using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class Layer
    {
        protected Region region;

        protected delegate void ChangeRegionDelegate(int pRegionId);

        protected event ChangeRegionDelegate ChangeRegion = null;

        #region Constraints
        protected string primary_key = "osm_id";
        protected string way_idx = "way";
        protected List<string> idxs = new List<string>() { };
        #endregion

        private int region_id = 0;
        protected bool need_update = true;
        private string schemaName;
        private string tableName;
        public int RegionId
        {
            get
            {
                return region_id;
            }
            set
            {
                setRegion(value);
            }
        }

        public void HeatUp()
        {
            need_update = true;
        }

        public string TableName
        {
            get
            {
                if (schemaName + tableName != "")
                {
                    return schemaName + "." + tableName + RegionId.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public string Name
        {
            get
            {
                return tableName;
            }
        }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        public Layer(string pTableName, string pSchemaName = "cache")
        {
            region_id = 0;
            schemaName = pSchemaName;
            tableName = pTableName;
            ChangeRegion += Layer_ChangeRegion;
        }

        public Layer(int pRegionId, string pTableName, string pSchemaName = "cache")
        {
            region_id = pRegionId;
            schemaName = pSchemaName;
            tableName = pTableName;
            ChangeRegion += Layer_ChangeRegion;
        }

        private void Layer_ChangeRegion(int pRegionId)
        {
            region = new Region(pRegionId);
        }

        /// <summary>
        ///  Создать таблицу с данными для выбора по слою
        /// </summary>
        public virtual void CreateTable()
        {
            Updated = DateTime.Now;
            Created = Updated;
            string tn = TableName;
            string tn_ = tn.Replace('.','_');
            need_update = false;
            // ALTER TABLE cache.region0 ADD CONSTRAINT osm_id_pk PRIMARY KEY (osm_id)
            if (primary_key != "")
                DataBase.RunCommand(string.Format("ALTER TABLE {0} ADD CONSTRAINT {1}_{2}_pk PRIMARY KEY ({2})", tn,  tn_, primary_key));
            if (way_idx != "")
                DataBase.RunCommand(string.Format("CREATE INDEX {0}_index ON {1} USING gist({2});", tn_, tn, way_idx));

        }
        /// <summary>
        /// Обновить таблицу с данными
        /// </summary>
        public virtual void UpdateTable()
        {
            if (need_update)
            {
                Updated = DateTime.Now;
                need_update = false;
            }
        }
        /// <summary>
        /// Получить значение поля в первой строке таблицы
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public object GetValue(string FieldName)
        {
            return DataBase.First("SELECT * FROM " + TableName, FieldName);
        }

        /// <summary>
        /// Получить все значения поля и строки
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, object[]>> GetValues(string FieldName)
        {
            List<KeyValuePair<int, object[]>> ret = new List<KeyValuePair<int, object[]>>();
            List<object []> rows = DataBase.RowSelect(string.Format("SELECT {0}, * FROM {1} ", FieldName, TableName));
            foreach(object[] row in rows)
            {
                ret.Add( new KeyValuePair<int, object[]>(row[0].ToInt(), row) );
            }
            return ret;
        }

        public void setRegion(int pRegionId)
        {
            region_id = pRegionId;
            if (ChangeRegion != null) ChangeRegion(region_id);
            UpdateTable();
        }

        public virtual SharpMap.Data.Providers.IProvider GetProvider()
        {
            return new SharpMap.Data.Providers.PostGIS(
                DataBase.ConnectionString,
                TableName,
                "way",
                "osm_id"
            );
        }

        public virtual SharpMap.Layers.LabelLayer LLayer(string name = null, string col = "name")
        {
            string name_label = (name ?? Name) + "_label";
            SharpMap.Layers.LabelLayer layLabel = new SharpMap.Layers.LabelLayer(name_label);

            // layLabel.MultipartGeometryBehaviour = SharpMap.Layers.LabelLayer.MultipartGeometryBehaviourEnum.Largest;
            // layLabel.Theme = new SharpMap.Rendering.Thematics.CustomTheme(GeoStyles.ThemeLabel);
            // layLabel.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;

            SharpMap.Layers.VectorLayer LayerData = VLayer(name);

            layLabel.DataSource = LayerData.DataSource;
            layLabel.Enabled = true;
            layLabel.LabelColumn = col;

            layLabel.Style = new SharpMap.Styles.LabelStyle();
            string[] namesLinesLabels = new string[] { "highway_label" };
            if (namesLinesLabels.Contains(name_label))
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
            layLabel.SRID = LayerData.SRID;
            return layLabel;
        }

        public virtual SharpMap.Layers.VectorLayer VLayer(string name = null)
        {
            SharpMap.Layers.VectorLayer Layer = new SharpMap.Layers.VectorLayer(name ?? Name);
            Layer.DataSource = GetProvider();
            return Layer;
        }
    }
}
