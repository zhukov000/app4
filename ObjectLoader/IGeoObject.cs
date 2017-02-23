using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLoader
{
    interface IGeoObject
    {
        /// <summary>
        /// Названия региона, например "Ростовская область"
        /// </summary>
        string region();
        /// <summary>
        /// Название района, например "Целинский район", возможно городской округ "город Шахты"
        /// </summary>
        string district();
        /// <summary>
        /// Название населенного пункта (для городского округа совпадает)
        /// </summary>
        string locality();
        /// <summary>
        /// Название улицы
        /// </summary>
        string street();
        /// <summary>
        /// Номер дома
        /// </summary>
        string house();
        /// <summary>
        /// Строка адреса
        /// </summary>
        string addressString();

        string latitude();

        string longitude();
    }
}
