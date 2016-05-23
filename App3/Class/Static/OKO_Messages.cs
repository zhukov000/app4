using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4
{
    static class OKO_Messages
    {
        public const uint MODULE_RUN = 4146;
        public const uint MODULE_ALREADY_RUN = 1073745970;
        public const uint MODULE_ERROR_RUN = 3221291059;
        public const uint COMPORT_ERROR_OPEN = 3221291009;

        static private IDictionary<uint, string> dict = new Dictionary<uint, string>()
        {
            { MODULE_RUN, "Модуль запущен" },
            { MODULE_ALREADY_RUN, "Модуль уже запущен" },
            { MODULE_ERROR_RUN, "Ошибка запуска модуля" },
            { COMPORT_ERROR_OPEN, "Ошибка открытия COM порта" }
        };

        static public string Message(uint code)
        {
            if (dict.ContainsKey(code))
                return dict[code];
            else
                return "Undefined messages";
        }
    }
}
