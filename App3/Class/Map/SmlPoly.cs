using System;

namespace App3.Class.Map
{
	internal class SmlPoly : Layer
	{
		public SmlPoly() : base(LayerType.SmlPoly, "cache")
		{
			this.region = new Region();
		}

		public SmlPoly(int pRegionId) : base(pRegionId, LayerType.SmlPoly, "cache")
		{
			this.region = new Region(pRegionId);
		}

		public SmlPoly(Region pregion) : base(pregion.RegionId, LayerType.SmlPoly, "cache")
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
				text += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 200, null) WHERE building is null)", this.region.GetValue("osm_id"));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.CreateTable();
		}

		public override void UpdateTable()
		{
			if (!DataBase.TableExist(base.TableName))
			{
				this.CreateTable();
			}
			else
			{
				string arg_4F_0 = "DELETE FROM {0}";
				string text = "INSERT INTO {0} ";
				if (base.RegionId != 0)
				{
					text += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 200, null) WHERE building is null)", this.region.GetValue("osm_id"));
				}
				DataBase.RunCommand(string.Format(arg_4F_0, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
			}
			base.UpdateTable();
		}
	}
}
