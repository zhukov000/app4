using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Text.RegularExpressions;
using System.Diagnostics;
using App3.Class.Singleton;
using System.Data.Odbc;

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

    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }

    public class MyConnection
    {
        public static int number = 0;
        private Pair<NpgsqlConnection, bool> pair;
        private int id;

        public MyConnection(NpgsqlConnection pConn, bool pFlag = true)
        {
            pair = new Pair<NpgsqlConnection, bool>(pConn, pFlag);
            id = number++;
        }

        public override bool Equals(object obj)
        {
            if (((MyConnection)obj).ID == this.id)
                return true;
            else
                return false;
        }

        public void Open()
        {
            pair.First.Open();
        }

        public void Close()
        {
            pair.First.Close();
        }

        public int ID
        {
            get { return id; }
        }

        public bool isAvailable
        {
            set { pair.Second = value; }
            get { return pair.Second; }
        }

        public NpgsqlConnection Get
        {
            get { return pair.First; }
        }
    }

    public class ConnectionPool
    {
        List<MyConnection> Connections;
        private Object getLock = new Object();
        private string sConnectionString = "";
        public string ConnectionString
        {
            get { return sConnectionString; }
        }
        
        public NpgsqlConnection Get()
        {
            NpgsqlConnection res = null;
            int deadlock = 100;
            lock (getLock)
                while ((res == null) && (deadlock > 0))
                {
                    deadlock--;
                    int i = 0;
                    foreach (MyConnection cn in Connections)
                    {
                        if (cn.isAvailable)
                        {
                            res = cn.Get;
                            cn.isAvailable = false;
                            if (i > 0)
                                Debug.WriteLine("Get " + i);
                            break;
                        }
                        i++;
                    }
                }
            return res;
        }

        public void Free(NpgsqlConnection pConnection)
        {
            int i = 0;
            lock (getLock)
                foreach(var cn in Connections)
                {
                    if (cn.Get.Equals(pConnection))
                    {
                        cn.isAvailable = true;
                        Debug.WriteLine("Free" + i);
                        break;
                    }
                    i++;
                }
        }

        public void OpenConnections(string pConnectionString, int PoolSize = 30)
        {
            Connections = new List<MyConnection>();
            sConnectionString = pConnectionString;
            for (int i = 0; i < PoolSize; i++)
            {
                NpgsqlConnection conn = new NpgsqlConnection(pConnectionString);
                conn.Open();
                Connections.Add(new MyConnection(conn));
            }
        }

        public void CloseConnections()
        {
            foreach(var cn in Connections)
            {
                cn.Close();
            }
            Connections.Clear();
        }
    }

    public static class DataBase
    {

        private static ConnectionPool oPool = new ConnectionPool();
        // private static NpgsqlConnection oConnection;
        private static bool isOpen = false;

        /// <summary>
        /// Получить объект подключения к БД
        /// </summary>
        public static NpgsqlConnection GetConnection()
        {
            if (isOpen)
            {
                return oPool.Get();
            }
            else
            {
                throw new DataBaseConnectionErrorExcepion();
            }
        }

        public static void FreeConn(NpgsqlConnection pConn)
        {
            oPool.Free(pConn);
        }

        public static string ConnectionString
        {
            get
            {
                if (isOpen)
                {
                    return oPool.ConnectionString;
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
                catch (Exception ex)
                {
                    o = null;
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
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
            object[] dr = new object[0];
            DataRow row = FirstRow(sRequest);
            if (row != null)
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
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                // throw new DataBaseRunCommandExcepion(ex.Message);
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
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row.ItemArray);
                }
            }
            return list;
        }

        public static List<KeyValuePair<string, object>[]> RowSelect(string sRequest, bool f)
        {
            List<KeyValuePair<string, object>[]> list = new List<KeyValuePair<string, object>[]>();
            DataSet ds = new DataSet();
            RowSelect(sRequest, ds);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    List<KeyValuePair<string, object>> arrow = new List<KeyValuePair<string,object>>();
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        arrow.Add(new KeyValuePair<string, object>(col.ColumnName, row[col.ColumnName]));
                    }
                    list.Add(arrow.ToArray());
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
                NpgsqlConnection oConnection = oPool.Get();
                NpgsqlDataAdapter oDataAdapter = new NpgsqlDataAdapter(sRequest, oConnection);
                try
                {
                    oDataAdapter.Fill(pDataSet);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    // throw new DataBaseRunCommandExcepion(e.Message);
                }
                finally
                {
                    oPool.Free(oConnection);
                }
            }
            else
            {
                // throw new DataBaseConnectionErrorExcepion();
            }
        }

        public static string Help4Select(string pTableName, IDictionary<string, object> pSelect, IDictionary<string, object> pWhere)
        {
            string request = "SELECT ";
            if (pSelect.Count == 0)
            {
                request += "*";
            }
            else
            {
                request += string.Join(",", pSelect.Select(x => x.Key ).ToArray());
            }
            request += " FROM " + pTableName;
            if (pWhere.Count() > 0)
            {
                request += " WHERE " +
                    string.Join(" AND ", pWhere.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pSelect"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static List<object[]> RunCommandSelect(string pTableName, IDictionary<string, object> pSelect, IDictionary<string, object> pWhere)
        {
            return RowSelect(Help4Select(pTableName, pSelect, pWhere));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pSelect"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static object[] RunCommandFirstSelect(string pTableName, IDictionary<string, object> pSelect, IDictionary<string, object> pWhere)
        {
            string s = Help4Select(pTableName, pSelect, pWhere);
            s += " LIMIT 1";
            return FirstRow(s, 0);
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
                NpgsqlConnection oConnection = oPool.Get();
                NpgsqlCommand oCommand = new NpgsqlCommand(pCommand, oConnection);
                try
                {
                    if (oConnection.State != ConnectionState.Open)
                        oConnection.Open();
                    RowAffected = oCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    // throw new DataBaseRunCommandExcepion(ex.Message);
                }
                finally
                {
                    oPool.Free(oConnection);
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


        public static int RunCommandDelete(string pTableName, IDictionary<string, object> pKeyFields)
        {
            string request = string.Format("DELETE FROM {0} WHERE ", pTableName) +
                string.Join(" AND ", pKeyFields.Select(x => x.Key + "=" + x.Value).ToArray());

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
            oPool.CloseConnections();
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
            // oConnection = new NpgsqlConnection(pConnectionString);
            try
            {
                oPool.OpenConnections(pConnectionString);
                isOpen = true;
            }
            catch(Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                // throw new DataBaseConnectionErrorExcepion(ex.Message);
            }
            return isOpen;
        }

        public static bool IsOpen()
        {
            return isOpen;
        }

        public static bool TableExist(string tableName)
        {
            bool exists = false;

            string[] ss = { "public", "" };
            string[] s2 = tableName.Split(".".ToCharArray());
            if (s2.Length == 1)
                ss[1] = s2[0];
            else if (s2.Length == 2)
            {
                ss[0] = s2[0];
                ss[1] = s2[1];
            }
            string req = string.Format(
                  "select case when exists((select * from information_schema.tables where table_schema = '{0}' and table_name = '{1}')) then 1 else 0 end as v", ss[0], ss[1]);
            try
            {
                int v = (int)DataBase.First(req, "v");
                if (v == 1) exists = true;
            }
            catch
            {}
            /*try
            {
                // ANSI SQL way.  Works in PostgreSQL, MSSQL, MySQL.  
                var cmd = new OdbcCommand(
                    string.Format(
                  "select case when exists((select * from information_schema.tables where table_schema = '{0}' and table_name = '{1}')) then 1 else 0 end", ss[0], ss[1]),  );

                exists = (int)cmd.ExecuteScalar() == 1;
            }
            catch
            {
                try
                {
                    // Other RDBMS.  Graceful degradation
                    exists = true;
                    var cmdOthers = new OdbcCommand("select 1 from " + tableName + " where 1 = 0");
                    cmdOthers.ExecuteNonQuery();
                }
                catch
                {
                    exists = false;
                }
            }*/
            return exists;
        }
    }
}
