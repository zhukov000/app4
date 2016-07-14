using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class Object : Layer
    {
        public Object(): base("object") { region = new Region(); }

        public Object(int pRegionId) : base(pRegionId, "object") { region = new Region(pRegionId); }

        public Object(Region pregion) : base(pregion.RegionId, "object") { region = pregion; }

        public override void CreateTable()
        {
            if (DataBase.TableExist(TableName)) return;
            region.CreateTable();
            string reqCreate = "CREATE TABLE {0} AS ";
            if (RegionId != 0)
            {
                reqCreate += string.Format("(SELECT odo.way, odo.name as name, odo.number, os.* FROM oko.district_objects3({0}) as odo INNER JOIN oko.object_status as os on odo.osm_id = os.osm_id)", region.GetValue("osm_id") );
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
                    reqInsert += string.Format("(SELECT odo.way, odo.name as name, odo.number, os.* FROM oko.district_objects3({0}) as odo INNER JOIN oko.object_status as os on odo.osm_id = os.osm_id)", region.GetValue("osm_id"));
                    DataBase.RunCommand(string.Format(reqDelete, TableName));
                    DataBase.RunCommand(string.Format(reqInsert, TableName));
                }
                base.UpdateTable();
            }
        }
    }
}
