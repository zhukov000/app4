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

        /// <summary>
        /// Запросить строки из БД согласно запросу
        /// </summary>
        /// <param name="sRequest"></param>
        public static void RowSelect(string sRequest, DataSet pDataSet)
        {
            if (isOpen)
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
                NpgsqlCommand oCommand = new NpgsqlCommand(pCommand, oConnection);
                try
                {
                    RowAffected = oCommand.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    throw new DataBaseRunCommandExcepion(e.Message);
                }
            }
            else
            {
                throw new DataBaseConnectionErrorExcepion();
            }
            return RowAffected;
        }

        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
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
