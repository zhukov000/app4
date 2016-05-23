using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4
{
    static class OKO_Messages
    {
        static private IDictionary<uint, string> dict = new Dictionary<uint, string>()
        {
            { 4146, "Модуль запущен" },
            { 1073745970, "Модуль уже запущен" },
            { 3221291059, "Ошибка запуска модуля" },
            { 3221291009, "Ошибка открытия COM порта" }
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
