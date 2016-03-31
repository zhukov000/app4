using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Text.RegularExpressions;

namespace App3
{
    public class DataBaseConnectionErrorExcepion : Exception
    {
        public DataBaseConnectionErrorExcepion() : base("Подключение не было установлено") { }
        public DataBaseConnectionErrorExcepion(string message) : base(message) { }
    }

    public class DataBaseRunCommandExcepion : Exception
    {
        public DataBaseRunCommandExcepion() : base("Команда не может быть выполнена") { }
        public DataBaseRunCommandExcepion(string message) : base(message) { }
    }

    public static class DataBase
    {
        private static NpgsqlConnection oConnection;
        private static bool isOpen = false;

        private static object locker = new object();

        public static bool IsOpen
        {
            get { return isOpen; }
        }

        /// <summary>
        /// Получить объект подключения к БД
        /// </summary>
        public static NpgsqlConnection Connection
        {
            get
            {
                if (isOpen)
                {
                    return oConnection;
                }
                else
                {
                    throw new DataBaseConnectionErrorExcepion();
                }
            }
        }

        public static string ConnectionString
        {
            get
            {
                if (isOpen)
                {
                    return oConnection.ConnectionString;
                }
                else
                {
                    throw new DataBaseConnectionErrorExcepion();
                }
            }
        }

        /// <summary>
        /// Получить значение столбца в первой строке
        /// </summary>
        /// <param name="sRequest"></param>
        /// <param name="ColName"></param>
        /// <returns></returns>
        public static object First(string sRequest, string ColName)
        {
            object o = null;
            DataRow dr = FirstRow(sRequest);
            if (dr != null)
            {
                try
                {
                    o = dr[ColName];
                }
                catch 
                {
                    o = null;
                }
            }
            return o;
        }

        /// <summary>
        /// Получить первую строку
        /// </summary>
        /// <param name="sRequest"></param>
        /// <returns></returns>
        public static DataRow FirstRow(string sRequest)
        {
            DataRow dr = null;
            DataSet ds = new DataSet();
            RowSelect(sRequest, ds);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];
                }
            }
            return dr;
        }

        public static object[] FirstRow(string sRequest, int i)
        {
            object [] dr = new object[0];
            DataRow row = FirstRow(sRequest);
            if(row != null)
            {
                dr = row.ItemArray;
            }
            return dr;
        }

        /// <summary>
        /// Возвращает количество строк, полученное по запросу
        /// </summary>
        /// <returns></returns>
        public static int RowSelectCount(string sRequest)
        {
            int ret = 0;
            DataSet oDataSet = new DataSet();
            try
            {
                RowSelect(sRequest, oDataSet);
                ret = oDataSet.Tables[0].Rows.Count;
            }
            catch (DataBaseRunCommandExcepion e1)
            {
                throw e1;
            }
            catch(Exception e)
            {
                throw new DataBaseRunCommandExcepion(e.Message);
            }
            return ret;
        }

        public static List<object[]> RowSelect(string sRequest)
        {
            List<object[]> list = new List<object[]>();
            DataSet ds = new DataSet();
            RowSelect(sRequest, ds);
            if (ds.Tables.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row.ItemArray);
                }
            }
            return list;
        }

        /// <summary>
        /// Запросить строки из БД согласно запросу
        /// </summary>
        /// <param name="sRequest"></param>
        public static void RowSelect(string sRequest, DataSet pDataSet)
        {
            if (isOpen)
            {
                lock(locker)
                {
                    NpgsqlDataAdapter oDataAdapter = new NpgsqlDataAdapter(sRequest, oConnection);
                    try
                    {
                        oDataAdapter.Fill(pDataSet);
                    }
                    catch (Exception e)
                    {
                        throw new DataBaseRunCommandExcepion(e.Message);
                    }
                }
            }
            else
            {
                throw new DataBaseConnectionErrorExcepion();
            }
        }

        /// <summary>
        /// Запустить команду на изменение
        /// </summary>
        /// <param name="pCommand"></param>
        public static int RunCommand(string pCommand)
        {
            int RowAffected = 0;
            if (isOpen)
            {
                lock (locker)
                {
                    NpgsqlCommand oCommand = new NpgsqlCommand(pCommand, oConnection);
                    try
                    {
                        RowAffected = oCommand.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw new DataBaseRunCommandExcepion(e.Message);
                    }
                }
            }
            else
            {
                throw new DataBaseConnectionErrorExcepion();
            }
            return RowAffected;
        }

        /// <summary>
        /// Выполнить команду обновления
        /// </summary>
        /// <param name="pTableName">Таблица</param>
        /// <param name="pUpdate">Параметры для обновления</param>
        /// <param name="pKeyFields">Ключевые значения</param>
        /// <returns>Число обработанных записей</returns>
        public static int RunCommandUpdate(string pTableName, IDictionary<string, object> pUpdate, IDictionary<string, object> pKeyFields)
        {
            string request = string.Format("UPDATE {0} SET ", pTableName) +
                string.Join(", ", pUpdate.Select(x => x.Key + "=" + x.Value).ToArray());
            
            if (pKeyFields.Count() > 0)
            {
                request += " WHERE " +
                    string.Join(" AND ", pKeyFields.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            
            return RunCommand(request);
        }

        public static int RunCommandInsert(string pTableName, IDictionary<string, object> pInsert)
        {
            object[] pResult;
            return RunCommandInsert(pTableName, pInsert, "", out pResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTableName">имя таблицы</param>
        /// <param name="pInsert">справочник для вставки</param>
        /// <param name="pFieldResult">посылать "" или наименование поля с результатом (например, id)</param>
        /// <param name="pResult">значения для вставленных записей</param>
        /// <returns>Число обработанных записей</returns>
        public static int RunCommandInsert(string pTableName, IDictionary<string, object> pInsert, string pFieldResult, out object[] pResult)
        {
            pResult = new object[0];
            string request = "INSERT INTO " + pTableName + "(" +
                string.Join(",", pInsert.Select(x => x.Key).ToArray()) + ") VALUES (" +
                string.Join(",", pInsert.Select(x => x.Value).ToArray()) + ")"; 
            if (pFieldResult != "")
            {
                request += " RETURNING " + pFieldResult;
                pResult = DataBase.RowSelect(request).Select(x => x[0]).ToArray();
                return pResult.Count();
            }
            else
            {
                return RunCommand(request);
            }
        }

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        public static void CloseConnection()
        {
            oConnection.Close();
            isOpen = false;
        }

        /// <summary>
        /// Открыть соединение с БД
        /// </summary>
        /// <param name="sConnectionString">Строка подключения</param>
        /// <returns></returns>
        public static bool OpenConnection(string pConnectionString)
        {
            isOpen = false;
            oConnection = new NpgsqlConnection(pConnectionString);
            try
            {
                oConnection.Open();
                isOpen = true;
            }
            catch(Exception e)
            {
                throw new DataBaseConnectionErrorExcepion(e.Message);
            }
            return isOpen;
        }
    }
}
