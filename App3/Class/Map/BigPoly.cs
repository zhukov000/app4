using System;

namespace App3.Class.Map
{
	internal class BigPoly : Layer
	{
		public BigPoly() : base(LayerType.BigPoly, "cache")
		{
			this.region = new Region();
		}

		public BigPoly(int pRegionId) : base(pRegionId, LayerType.BigPoly, "cache")
		{
			this.region = new Region(pRegionId);
		}

		public BigPoly(Region pregion) : base(pregion.RegionId, LayerType.BigPoly, "cache")
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
				text += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 1000000, null) WHERE building is null)", this.region.GetValue("osm_id"));
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
				text += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 1000000, null) WHERE building is null)", this.region.GetValue("osm_id"));
				DataBase.RunCommand(string.Format(format, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.UpdateTable();
		}
	}
}
