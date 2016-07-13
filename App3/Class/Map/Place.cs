using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class Place : Layer
    {
        Polygon polygon;
        public Place(): base("place") { polygon = new Polygon(); }

        public Place(int pRegionId) : base(pRegionId, "place") { polygon = new Polygon(RegionId); }

        public Place(Polygon ppolygon) : base(ppolygon.RegionId, "place") { polygon = ppolygon; }

        public override void CreateTable()
        {
            if (DataBase.TableExist(TableName)) return;
            string reqCreate = "CREATE TABLE {0} AS ";
            if (RegionId != 0)
            {
                reqCreate += string.Format("(SELECT osm_id, way, name FROM {0} WHERE place is not null)", polygon.TableName);
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
                    reqInsert += string.Format("(SELECT osm_id, way, name FROM {0} WHERE place is not null)", polygon.TableName);
                    DataBase.RunCommand(string.Format(reqDelete, TableName));
                    DataBase.RunCommand(string.Format(reqInsert, TableName));
                }
                base.UpdateTable();
            }
        }
    }
}
