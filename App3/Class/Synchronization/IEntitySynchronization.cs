using App3.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Synchronization
{
    internal interface IEntitySynchronization
    {
        /// <summary>
        /// Получить список измененных объектов (которые необходимо обновить)
        /// </summary>
        /// <param name="ActualyPoint">Точка актуальности</param>
        /// <returns></returns>
        List<KeyValuePair<string, object>[]> getReversed(DateTime ActualyPoint);

        /// <summary>
        /// Получить список удаленных объектов (которые необходимо удалить)
        /// </summary>
        /// <param name="ActualyPoint"></param>
        /// <returns></returns>
        KeyValuePair<string, object>[] getDeleted(DateTime ActualyPoint);

        /// <summary>
        /// Получение имени объекта
        /// </summary>
        /// <returns></returns>
        // string getName();

        /// <summary>
        /// Получение имени таблицы
        /// </summary>
        /// <returns></returns>
        // string getTableName();

        /// <summary>
        /// Перегруженный метод = getName
        /// </summary>
        /// <returns></returns>
        // string ToString();
    }
}
