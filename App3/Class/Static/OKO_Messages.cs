using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4
{
    static class OKO_Messages
    {
        public const uint Module_Started = 4146;
        public const uint Module_AlreadyStarted = 1073745970;
        public const uint Module_StartError = 3221291059;
        public const uint Module_CanNotOpenPort = 3221291009;

        static private IDictionary<uint, string> dict = new Dictionary<uint, string>()
        {
            { Module_Started, "Модуль запущен" },
            { Module_AlreadyStarted, "Модуль уже запущен" },
            { Module_StartError, "Ошибка запуска модуля" },
            { Module_CanNotOpenPort, "Ошибка открытия COM порта" }
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
