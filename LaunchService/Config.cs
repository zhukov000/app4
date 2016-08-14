using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections;
using System.Configuration;
using App3.Class.Singleton;

namespace App3.Class
{
    public static class Config
    {
        public static string APPVERSION = "1.05";

        private static Dictionary<string, string> DAliases = new Dictionary<string, string>();

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static Config()
        {
            DAliases.Add("Удаленный порт", "ModuleRemotePort");
            DAliases.Add("Удаленный IP", "ModuleLocalServerIP");
            DAliases.Add("Локальный порт", "ModuleLocalServerPort");
            DAliases.Add("GUID", "ModuleLocalGUID");
            DAliases.Add("Номер модуля", "ModuleModuleId");
            DAliases.Add("Хост сервера БД", "DBServerHost");
            DAliases.Add("Порт сервера БД","DBServerPort");
            DAliases.Add("Пользователь БД","DBUser");
            DAliases.Add("Пароль БД","DBPassword");
            DAliases.Add("Имя БД", "DBName");
        }

        /// <summary>
        /// Получить значение параметра конфигурации по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetByKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string Get(string key)
        {
            return GetByKey(key);
        }

        /// <summary>
        /// Получение значение параметра конфигурации по псевдониму
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static string GetByAlias(string alias)
        {
            string ret = "";
            return DAliases.TryGetValue(alias, out ret) ? GetByKey(ret) : "";
        }

        public static void SetByAlias(string alias, string value)
        {
            Set(DAliases[alias], value);
        }

        /// <summary>
        /// Изменить значение параметра конфигурации
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; 
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            try
            {
                config.AppSettings.Settings[key].Value = value;
                config.Save();
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("Config.Set: Не получилось изменить значение параметра конфигурации: {0} - {1} ", key, ex.Message));
            }
        }

        /// <summary>
        /// Получение коллекции ключей
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection Keys()
        {
            return ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// Получение коллекции псевдонимов
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection Aliases()
        {
            NameValueCollection ret = new NameValueCollection();
            foreach(string alias in DAliases.Keys)
            {
                ret.Add(alias, alias);
            }
            return ret;
        }
    }
}
