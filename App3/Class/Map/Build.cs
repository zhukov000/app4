using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class Build : Layer
    {
        public Build(): base("build") { region = new Region(); }

        public Build(int pRegionId) : base(pRegionId, "build") { region = new Region(pRegionId); }

        public Build(Region pregion) : base(pregion.RegionId, "build") { region = pregion; }

        public override void CreateTable()
        {
            if (DataBase.TableExist(TableName)) return;
            string reqCreate = "CREATE TABLE {0} AS ";
            if (RegionId != 0)
            {
                reqCreate += string.Format("(SELECT osm_id, way, COALESCE(name, \"addr:housenumber\") as name FROM oko.district_weigth_polygons({0}, 0, 100000) WHERE building is not null)", region.GetValue("osm_id"));
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
                    reqInsert += string.Format("(SELECT osm_id, way, COALESCE(name, \"addr:housenumber\") as name FROM oko.district_weigth_polygons({0}, 0, 100000) WHERE building is not null)", region.GetValue("osm_id"));
                    DataBase.RunCommand(string.Format(reqDelete, TableName));
                    DataBase.RunCommand(string.Format(reqInsert, TableName));
                }
                base.UpdateTable();
            }
        }
    }
}
