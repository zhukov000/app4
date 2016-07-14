using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Map
{
    class Region: Layer
    {
        public Region(): base(0, LayerType.Region) {}

        public Region(int pRegionId) : base(pRegionId, LayerType.Region) {}

        public override void CreateTable()
        {
            if ( DataBase.TableExist(TableName) ) return;
            string reqCreate = "CREATE TABLE {0} AS ";
            if ( RegionId == 0 )
            {
                reqCreate += string.Format("(SELECT * FROM regions2map);");
            }
            else
            {
                reqCreate += string.Format("(SELECT * FROM regions2map WHERE num = {0});", RegionId);
            }
            DataBase.RunCommand(string.Format(reqCreate, TableName));
            base.CreateTable();
        }

        public override void UpdateTable()
        {
            if (need_update)
                if (!DataBase.TableExist(TableName))
                {
                    CreateTable();
                }
                else
                {
                    string reqDelete = "DELETE FROM {0}";
                    string reqInsert = "INSERT INTO {0} ";
                    if (RegionId == 0)
                    {
                        reqInsert += string.Format("(SELECT * FROM regions2map);");
                    }
                    else
                    {
                        reqInsert += string.Format("(SELECT * FROM regions2map WHERE num = {0});", RegionId);
                    }
                    DataBase.RunCommand(string.Format(reqDelete, TableName));
                    DataBase.RunCommand(string.Format(reqInsert, TableName));
                    base.UpdateTable();
                }
        }
    }
}
