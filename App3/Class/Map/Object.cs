using System;

namespace App3.Class.Map
{
	internal class Object : Layer
	{
		public Object() : base("object", "cache")
		{
			this.region = new Region();
		}

		public Object(int pRegionId) : base(pRegionId, "object", "cache")
		{
			this.region = new Region(pRegionId);
		}

		public Object(Region pregion) : base(pregion.RegionId, "object", "cache")
		{
			this.region = pregion;
		}

		public override void CreateTable()
		{
			if (DataBase.TableExist(base.TableName))
			{
				return;
			}
			this.region.CreateTable();
			string text = "CREATE TABLE {0} AS ";
			if (base.RegionId != 0)
			{
				text += string.Format("(SELECT odo.way, odo.name as name, odo.number, os.* FROM oko.district_objects3({0}) as odo INNER JOIN oko.object_status as os on odo.osm_id = os.osm_id)", this.region.GetValue("osm_id"));
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
				text += string.Format("(SELECT odo.way, odo.name as name, odo.number, os.* FROM oko.district_objects3({0}) as odo INNER JOIN oko.object_status as os on odo.osm_id = os.osm_id)", this.region.GetValue("osm_id"));
				DataBase.RunCommand(string.Format(format, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.UpdateTable();
		}
	}
}
