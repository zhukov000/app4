using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Synchronization
{
    /// <summary>
    /// 
    /// </summary>
    class SyncObjectStatus : Entity
    {
        override public ReversedType Type
        {
            get { return ReversedType.INSERT; }
        }

        /// <summary>
        /// Получить события для обновления (вставки)
        /// </summary>
        /// <param name="ActualyPoint"></param>
        /// <returns></returns>
        override public List<KeyValuePair<string, object>[]> getReversed(DateTime ActualyPoint)
        {
            int region_id = Config.Get("CurrenRegion", "-1").ToInt();
            if (region_id == -1)
                throw new Exception("Для синхронизации объектов нужен район");

            List<KeyValuePair<string, object>[]> list = new List<KeyValuePair<string, object>[]>();
            foreach (IDictionary<string, object> objProps in Utils.GetObjectsStatuses(region_id))
            {
                list.Add(objProps.ToArray<KeyValuePair<string, object>>());
            }

            return list;
            
        }

        /// <summary>
        /// Удаление статусов (событий) не поддерживается
        /// </summary>
        /// <param name="ActualyPoint"></param>
        /// <returns></returns>
        override public KeyValuePair<string, object>[] getDeleted(DateTime ActualyPoint)
        {
            return new KeyValuePair<string, object>[0];
        }

        override public string getName()
        {
            return "event";
        }
    }
}
