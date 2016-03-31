using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace App3.Class.Static
{
    static class OkoConnection
    {
        public static Stack<KeyValuePair<int, string>> Systems = new Stack<KeyValuePair<int,string>>();

        public static void CleanCounters()
        {
            string ConnectionsStrMask = @"Dbq={0};Uid=Admin;Pwd=;";
            try
            {
                var myConnection = new System.Data.Odbc.OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};" + string.Format(ConnectionsStrMask, "XGUARD_GUID.mdb"));
                myConnection.Open();
                var command = new System.Data.Odbc.OdbcCommand("DELETE FROM COUNTERS", myConnection);
                command.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex) 
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        public static void SetCounter(string GUID, string Value)
        {
            string ConnectionsStrMask = @"Dbq={0};Uid=Admin;Pwd=;";
            try
            {
                var myConnection = new System.Data.Odbc.OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};" + string.Format(ConnectionsStrMask, "XGUARD_GUID.mdb"));
                myConnection.Open();
                var command = new System.Data.Odbc.OdbcCommand(
                    string.Format("UPDATE COUNTERS set Value0 = {0} WHERE GUID = {1}", Value, GUID.Q()),
                    myConnection
                );
                command.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex) 
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        public static void PushSystemId(string pMessage)
        {
            int number = 0;
            string guid = "";
            do
            {
                string txt = Utils.CutPrefix(pMessage);
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(txt.Trim());
                }
                catch(Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    break;
                }

                XmlNode nodeHead = doc.SelectSingleNode("Envelope/MainHeader");
                if (nodeHead == null) break;
                
                XmlNode temp = null;

                temp = nodeHead.SelectSingleNode("Number");
                if (temp != null) number = temp.InnerText.ToInt();

                temp = nodeHead.SelectSingleNode("SystemID");
                if (temp != null) guid = temp.InnerText;

                Systems.Push(new KeyValuePair<int, string>(number, guid));
            } while (false);
        }

        public static void FixCounterError(string pMessage)
        {
            do
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(pMessage.Trim());
                }
                catch(Exception ex) 
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    break;
                }

                XmlNode nodeResp = doc.SelectSingleNode("Envelope/Response");
                if (nodeResp == null) break;

                XmlNode temp = null;
                string comment = "";
                int number = 0;
                int res = 0;
                int newVal = 0;

                temp = nodeResp.SelectSingleNode("Result");
                if (temp != null) res = temp.InnerText.ToInt();
                if (res != 102) break;

                temp = nodeResp.SelectSingleNode("Number");
                if (temp != null) number = temp.InnerText.ToInt();

                temp = nodeResp.SelectSingleNode("Comment");
                if (temp != null) comment = temp.InnerText;

                if (comment.Like("WRONG_COUNTER;%"))
                {
                    newVal = comment.Split(':').Last().ToInt();
                }

                KeyValuePair<int, string> lstSystem;
                if (Systems.Count > 0)
                {
                    lstSystem = Systems.Pop();
                    if (lstSystem.Key == number)
                    {
                        SetCounter(lstSystem.Value, newVal.ToString());
                    }
                    else
                    {
                        Systems.Push(lstSystem);
                    }
                }

            } while (false);
           
        }
    }
}
