using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    class ForeignKey
    {
        private Dictionary<string, string> Cache = new Dictionary<string, string>();
        List<string> fieldsName = new List<string>();
        public List<string> FieldsName
        {
            get
            {
                return fieldsName;
            }
        }

        string sourceRequest = "";
        List<string> sourceRefsName = new List<string>();
        List<string> showFieldsName = new List<string>();

        public string FieldName
        {
            get
            {
                return fieldsName.First();
            }
        }
        public string SourceRequest
        {
            get
            {
                return sourceRequest;
            }
        }
        public string RefFieldName
        {
            get
            {
                return sourceRefsName.First();
            }
        }

        public string Val(string pLinkValue)
        {
            string res = ""; 
            if (!Cache.TryGetValue(pLinkValue, out res))
            {
                UpdateCache();
                res = Value((new string[] { pLinkValue }).ToList<string>());
            }
            return res;
        }

        public string Value(List<string> pLinkValues)
        {
            string res = "";
            string req = "SELECT " + string.Join(",", showFieldsName.ToArray()) + " FROM (" + sourceRequest + ") src WHERE true ";
            var keysAndValues = sourceRefsName.Zip(pLinkValues, (k, v) => new { Key = k, Value = v });
            foreach (var kv in keysAndValues)
            {
                req += " AND " + kv.Key + " = '" + kv.Value + "'";
            }
            res = string.Join(",", DataBase.FirstRow(req, 0));
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pField">Список названий связанных полей</param>
        /// <param name="pLinkSource">Запрос на выбор данных</param>
        /// <param name="pLinkField">Список названий полей в запросе (связаны по порядку!)</param>
        /// <param name="pShowFieldsName">Список полей вывода</param>
        public ForeignKey(List<string> pField, string pLinkSource, List<string> pLinkField, List<string> pShowFieldsName)
        {
            fieldsName = pField;
            sourceRequest = pLinkSource;
            sourceRefsName = pLinkField;
            showFieldsName = pShowFieldsName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pField">Связанное поле</param>
        /// <param name="pLinkTable">Таблица</param>
        /// <param name="pLinkField">Поле в таблице</param>
        /// <param name="pShowField">Выводимое</param>
        public ForeignKey(string pField, string pLinkTable, string pLinkField, string pShowField)
        {
            fieldsName.Add(pField);
            sourceRequest = "select * from " + pLinkTable;
            sourceRefsName.Add(pLinkField);
            showFieldsName.Add(pShowField);
        }

        private void UpdateCache()
        {
            if (sourceRefsName.Count() == 1 && showFieldsName.Count() == 1)
            {
                Cache = new Dictionary<string, string>();
                string req = "SELECT " + string.Join(",", showFieldsName.ToArray()) +
                            "," + string.Join(",", sourceRefsName.ToArray()) + " FROM (" + sourceRequest + ") src";
            
                foreach (object[] cells in DataBase.RowSelect(req) )
                {
                    Cache.Add(cells[1].ToString(), cells[0].ToString());
                }
            }
        }
    }
}
