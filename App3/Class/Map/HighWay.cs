using System;

namespace App3.Class.Map
{
	internal class HighWay : Layer
	{
		private Road road;

		public HighWay() : base("highway", "cache")
		{
			this.road = new Road();
		}

		public HighWay(int pRegionId) : base(pRegionId, "highway", "cache")
		{
			this.road = new Road(pRegionId);
		}

		public HighWay(Road proad) : base(proad.RegionId, "highway", "cache")
		{
			this.road = proad;
		}

		public override void CreateTable()
		{
			if (DataBase.TableExist(base.TableName))
			{
				return;
			}
			this.road.CreateTable();
			string text = "CREATE TABLE {0} AS ";
			if (base.RegionId != 0)
			{
				text += string.Format("(SELECT osm_id, way, name FROM {0} WHERE highway is not null)", this.road.TableName);
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
				text += string.Format("(SELECT osm_id, way, name FROM {0} WHERE highway is not null)", this.road.TableName);
				DataBase.RunCommand(string.Format(format, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.UpdateTable();
		}
	}
}
