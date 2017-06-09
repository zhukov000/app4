using System;

namespace App3.Class.Map
{
	internal class Polygon : Layer
	{
		public Polygon() : base("polygon", "cache")
		{
			this.region = new Region();
		}

		public Polygon(int pRegionId) : base(pRegionId, "polygon", "cache")
		{
			this.region = new Region(pRegionId);
		}

		public Polygon(Region pregion) : base(pregion.RegionId, "polygon", "cache")
		{
			this.region = pregion;
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
				text += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_points({0}))", this.region.GetValue("osm_id"));
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
				text += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_points({0}))", this.region.GetValue("osm_id"));
				DataBase.RunCommand(string.Format(format, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.UpdateTable();
		}
	}
}
