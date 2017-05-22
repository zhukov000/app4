using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Synchronization
{
    class EventSync : Entity
    {
        override public List<KeyValuePair<string, object>[]> getReversed(DateTime ActualyPoint)
        {
            // return DataBase.RowSelect(string.Format("SELECT * FROM {0} WHERE datetime > '{1}'", getTableName(), ActualyPoint.ToString()), false);
            throw new Exception("Обновление для объектов Event не поддерживается");
        }

        override public KeyValuePair<string, object>[] getDeleted(DateTime ActualyPoint)
        {
            throw new Exception("Обновление для объектов Event не поддерживается");
        }

        override public string getName()
        {
            return "event";
        }

        override public string getTableName()
        {
            return "oko.event";
        }
    }
}
