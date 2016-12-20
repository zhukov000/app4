using SharpMap.Data.Providers;
using SharpMap.Layers;
using SharpMap.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App3.Class.Map
{
	internal class Layer
	{
		protected delegate void ChangeRegionDelegate(int pRegionId);

		protected Region region;

		protected string primary_key = "osm_id";

		protected string way_idx = "way";

		protected List<string> idxs = new List<string>();

		private int region_id;

		protected bool need_update = true;

		private string schemaName;

		private string tableName;

		[method: CompilerGenerated]
		[CompilerGenerated]
		protected event Layer.ChangeRegionDelegate ChangeRegion;

		public int RegionId
		{
			get
			{
				return this.region_id;
			}
			set
			{
				this.setRegion(value);
			}
		}

		public string TableName
		{
			get
			{
				if (this.schemaName + this.tableName != "")
				{
					return this.schemaName + "." + this.tableName + this.RegionId.ToString();
				}
				return "";
			}
		}

		public string Name
		{
			get
			{
				return this.tableName;
			}
		}

		public DateTime Updated
		{
			get;
			set;
		}

		public DateTime Created
		{
			get;
			set;
		}

		public void HeatUp()
		{
			this.need_update = true;
		}

		public Layer(string pTableName, string pSchemaName = "cache")
		{
			this.region_id = 0;
			this.schemaName = pSchemaName;
			this.tableName = pTableName;
			this.ChangeRegion += new Layer.ChangeRegionDelegate(this.Layer_ChangeRegion);
		}

		public Layer(int pRegionId, string pTableName, string pSchemaName = "cache")
		{
			this.region_id = pRegionId;
			this.schemaName = pSchemaName;
			this.tableName = pTableName;
			this.ChangeRegion += new Layer.ChangeRegionDelegate(this.Layer_ChangeRegion);
		}

		private void Layer_ChangeRegion(int pRegionId)
		{
			this.region = new Region(pRegionId);
		}

		public virtual void CreateTable()
		{
			this.Updated = DateTime.Now;
			this.Created = this.Updated;
			string text = this.TableName;
			string text2 = text.Replace('.', '_');
			this.need_update = false;
			if (this.primary_key != "")
			{
				DataBase.RunCommand(string.Format("ALTER TABLE {0} ADD CONSTRAINT {1}_{2}_pk PRIMARY KEY ({2})", text, text2, this.primary_key));
			}
			if (this.way_idx != "")
			{
				DataBase.RunCommand(string.Format("CREATE INDEX {0}_index ON {1} USING gist({2});", text2, text, this.way_idx));
			}
		}

		public virtual void UpdateTable()
		{
			if (this.need_update)
			{
				this.Updated = DateTime.Now;
				this.need_update = false;
			}
		}

		public object GetValue(string FieldName)
		{
			return DataBase.First("SELECT * FROM " + this.TableName, FieldName);
		}

		public List<KeyValuePair<int, object[]>> GetValues(string FieldName)
		{
			List<KeyValuePair<int, object[]>> list = new List<KeyValuePair<int, object[]>>();
			foreach (object[] current in DataBase.RowSelect(string.Format("SELECT {0}, * FROM {1} ", FieldName, this.TableName)))
			{
				list.Add(new KeyValuePair<int, object[]>(current[0].ToInt(), current));
			}
			return list;
		}

		public void setRegion(int pRegionId)
		{
			this.region_id = pRegionId;
			if (this.ChangeRegion != null)
			{
				this.ChangeRegion(this.region_id);
			}
			this.UpdateTable();
		}

		public virtual IProvider GetProvider()
		{
			return new PostGIS(DataBase.ConnectionString, this.TableName, "way", "osm_id");
		}

		public virtual LabelLayer LLayer(string name = null, string col = "name")
		{
			string text = (name ?? this.Name) + "_label";
			LabelLayer labelLayer = new LabelLayer(text);
			VectorLayer vectorLayer = this.VLayer(name);
			labelLayer.DataSource = vectorLayer.DataSource;
			labelLayer.Enabled = true;
			labelLayer.LabelColumn = col;
			labelLayer.Style = new LabelStyle();
			if (new string[]
			{
				"highway_label"
			}.Contains(text))
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
			labelLayer.SRID = vectorLayer.SRID;
			return labelLayer;
		}

		public virtual VectorLayer VLayer(string name = null)
		{
			return new VectorLayer(name ?? this.Name)
			{
				DataSource = this.GetProvider()
			};
		}
	}
}
