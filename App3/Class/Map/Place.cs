using System;

namespace App3.Class.Map
{
	internal class Place : Layer
	{
		private Polygon polygon;

		public Place() : base("place", "cache")
		{
			this.polygon = new Polygon();
		}

		public Place(int pRegionId) : base(pRegionId, "place", "cache")
		{
			this.polygon = new Polygon(base.RegionId);
		}

		public Place(Polygon ppolygon) : base(ppolygon.RegionId, "place", "cache")
		{
			this.polygon = ppolygon;
		}

		public override void CreateTable()
		{
			if (DataBase.TableExist(base.TableName))
			{
				return;
			}
			string text = "CREATE TABLE {0} AS ";
			if (base.RegionId != 0)
			{
				text += string.Format("(SELECT osm_id, way, name FROM {0} WHERE place is not null)", this.polygon.TableName);
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.CreateTable();
		}

		public override void UpdateTable()
		{
			if (!DataBase.TableExist(base.TableName))
			{
				this.CreateTable();
				return;
			}
			string format = "DELETE FROM {0}";
			string text = "INSERT INTO {0} ";
			if (base.RegionId != 0)
			{
				text += string.Format("(SELECT osm_id, way, name FROM {0} WHERE place is not null)", this.polygon.TableName);
				DataBase.RunCommand(string.Format(format, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.UpdateTable();
		}
	}
}
