using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class BigPoly : Layer
    {
        public BigPoly() : base("bigpoint") { region = new Region(); }

        public BigPoly(int pRegionId) : base(pRegionId, "bigpoint") { region = new Region(pRegionId); }

        public BigPoly(Region pregion) : base(pregion.RegionId, "bigpoint") { region = pregion; }

        public override void CreateTable()
        {
            if (DataBase.TableExist(TableName)) return;
            string reqCreate = "CREATE TABLE {0} AS ";
            if (RegionId != 0)
            {
                reqCreate += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 1000000, null) WHERE building is null)", region.GetValue("osm_id"));
                DataBase.RunCommand(string.Format(reqCreate, TableName));
            }
            base.CreateTable();
        }

        public override void UpdateTable()
        {
            if (!DataBase.TableExist(TableName))
            {
                CreateTable();
            }
            else
            {
                string reqDelete = "DELETE FROM {0}";
                string reqInsert = "INSERT INTO {0} ";
                if (RegionId != 0)
                {
                    reqInsert += string.Format("(SELECT osm_id, way, name, building, highway, place FROM oko.district_weigth_polygons({0}, 1000000, null) WHERE building is null)", region.GetValue("osm_id"));
                    DataBase.RunCommand(string.Format(reqDelete, TableName));
                    DataBase.RunCommand(string.Format(reqInsert, TableName));
                }
                base.UpdateTable();
            }
        }
    }
}
