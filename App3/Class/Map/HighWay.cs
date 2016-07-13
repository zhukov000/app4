using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class HighWay : Layer
    {
        private Road road;

        public HighWay(): base("highway") { road = new Road(); }

        public HighWay(int pRegionId) : base(pRegionId, "highway") { road = new Road(pRegionId); }

        public HighWay(Road proad) : base(proad.RegionId, "highway") { road = proad; }

        public override void CreateTable()
        {
            if (DataBase.TableExist(TableName)) return;
            road.CreateTable();
            string reqCreate = "CREATE TABLE {0} AS ";
            if (RegionId != 0)
            {
                reqCreate += string.Format("(SELECT osm_id, way, name FROM {0} WHERE highway is not null)", road.TableName);
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
                    reqInsert += string.Format("(SELECT osm_id, way, name FROM {0} WHERE highway is not null)", road.TableName);
                    DataBase.RunCommand(string.Format(reqDelete, TableName));
                    DataBase.RunCommand(string.Format(reqInsert, TableName));
                }
                base.UpdateTable();
            }
        }
    }
}
