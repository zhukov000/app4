using System;
using System.Collections.Generic;

namespace App3.Class.Static
{
    internal static class OKO_Messages
    {
        public const uint Module_Started = 4146u;

        public const uint Module_AlreadyStarted = 1073745970u;

        public const uint Module_StartError = 3221291059u;

        public const uint Module_CanNotOpenPort = 3221291009u;

        private static IDictionary<uint, string> dict = new Dictionary<uint, string>
        {
            {
                4146u,
                "Модуль запущен"
            },
            {
                1073745970u,
                "Модуль уже запущен"
            },
            {
                3221291059u,
                "Ошибка запуска модуля"
            },
            {
                3221291009u,
                "Ошибка открытия COM порта"
            }
        };

        public static string Message(uint code)
        {
            if (OKO_Messages.dict.ContainsKey(code))
            {
                return OKO_Messages.dict[code];
            }
            return "Undefined messages";
        }
    }
}
