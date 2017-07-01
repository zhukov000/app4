using App3.Class.Singleton;
using App3.Class.Socket;
using App3.Class.Static;
using OKOGate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MessageGroupId = App3.Class.Utils.MessageGroupId;

namespace App3.Class
{
    /// <summary>
    /// Обработка событий
    /// </summary>
    class Handling
    {
        public delegate void ObjectCardOpen(long pIdObject);
        public static Handling.ObjectCardOpen onObjectCardOpen = null;

        public delegate void ObjectListOpen(string Filter);
        public static Handling.ObjectListOpen onObjectListOpen = null;

        public delegate void GetMessageHandler(int pEventId, MessageGroupId pMsgGrId, AKObject pObject, string pMsgTxt, string pPhone, string pTime);
        public static GetMessageHandler GetMessageEvent = null;

        /// <summary>
        /// Метод вызывается при получении события от другого узла через сокет
        /// </summary>
        /// <param name="data"></param>
        public static void GetObjectDelegate(SendObject data)
        {
            object[] array = new object[0];
            try
            {
                Logger.Log("SocketSync: Handle calling " + data.Message, Logger.LogLevel.DEBUG);
                if (data != null)
                    DataBase.RunCommandInsert("oko.event", data.GetInfo(), "id", out array);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
        }

        /// <summary>
        /// Отправить данные по сокету
        /// </summary>
        /// <param name="pData"></param>
        public static void SendDataBySocket(IDictionary<string, object> pData, long objectId)
        {
            SendObject data = new SendObject(pData);
            try
            {
                // data.IpAddress = Config.Get("SocketServerIP");
                foreach (object[] current in DataBase.RowSelect("select sn.id, sn.ipv4, sn.port from syn_nodes sn where sn.synout"))
                {
                    data.RetrNumber = (int)objectId; // Config.Get("CurrenRegion").ToInt(); // current[0].ToInt();
                    string _server = current[1].ToString();
                    int _port = current[2].ToInt();
                    SocketClient.SendObjectFromSocket2(data, _server, _port);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
        }

        private static bool ProcessingEvent(int OkoVersion, int RegionId, int ObjectNumber, int RetrNumber, int Class, int Code, int Part, int Zone, int ChnlMask, int Idx, DateTime TimeStamp, int SignalLevel, int AlarmGroupId = 0, string Address = "")
        {
            bool flag = true;
            int pEventId = 0;
            AKObject aKObject = new AKObject(ObjectNumber, RegionId);
            Utils.MessageGroupId item = DBDict.TMessage[OkoVersion, Class, Code].Item1;
            Tuple<DateTime, int> tuple = EventDispatcher.Instance[aKObject.Id, item, Idx];
            if (TimeStamp.Subtract(tuple.Item1).TotalSeconds < (double)DBDict.Settings["TIMEOUT_MESSAGE_INTERVAL"].ToInt() || tuple.Item2 == Idx)
            {
                flag = false;
            }
            try
            {
                object[] array = new object[0];

                IDictionary<string, object> data = new Dictionary<string, object>
                {
                    { "objectnumber", ObjectNumber },
                    { "alarmgroupid", AlarmGroupId },
                    { "datetime", TimeStamp.Q() },
                    { "code", Code },
                    { "typenumber", Idx },
                    { "partnumber",Part },
                    { "zoneusernumber", Zone },
                    { "class", Class },
                    { "address", Address.Q() },
                    { "region_id", RegionId },
                    { "channelnumber", ChnlMask },
                    { "oko_version", OkoVersion },
                    { "retrnumber", RetrNumber },
                    { "isrepeat", !flag },
                    { "siglevel", SignalLevel }
                };
                int row_aff = DataBase.RunCommandInsert("oko.event", data, "id", out array);
                if (Config.Get("SocketEnableSync") == "1")
                {
                    SendDataBySocket(data, aKObject.Id);
                }
                if (array.Count<object>() > 0)
                {
                    pEventId = array[0].ToInt();
                }
                if (row_aff > 0)
                {
                    Logger.Log("Событие записано в БД: " + pEventId.ToString(), Logger.LogLevel.DEBUG);
                }
                else
                {
                    Logger.Log("Событие не было записано в БД", Logger.LogLevel.ERROR);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                Logger.Log(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                Logger.Log(ex.StackTrace, Logger.LogLevel.DEBUG);
                Logger.Log(ex.GetaAllMessages(), Logger.LogLevel.DEBUG);
            }
            if (flag && aKObject.IsExists())
            {
                EventDispatcher.Instance[aKObject.Id, item, Idx] = new Tuple<DateTime, int>(DateTime.Now, Idx);
                string text = string.Format("{0}({1})", DBDict.TMessage[OkoVersion, Class, Code].Item2, DBDict.TMessage[OkoVersion, Class, Code].Item3);
                text = string.Format("Объект расположенный по адресу:{0}, \r\nсообщает:{1}", aKObject.AddressStr, text);
                if (Handling.GetMessageEvent != null)
                {
                    Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, text), Logger.LogLevel.DEBUG);
                    Handling.GetMessageEvent(pEventId, item, aKObject, text, string.Join(", ", aKObject.GetContacts()), TimeStamp.ToString());
                }
                flag = true;
            }
            else
            {
                if (!aKObject.IsExists())
                    Logger.Log(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "Сообщение не было сохранено в БД и/или АК объекта не создана: " + ObjectNumber), Logger.LogLevel.ERROR);
            }
            return flag;
        }

        public static void ProcessingComEvent(object Message)
        {
            GuardAgent2.Message message = (GuardAgent2.Message)Message;
            string address = message.Address;
            DateTime timeStamp = message.TimeStamp;
            object[] array = new object[9];
            int objectNumber = 0;
            int regionId = Config.Get("CurrenRegion").ToInt();
            int okoVersion = 0;
            int retrNumber = 0;
            int _class = 0;
            int code = 0;
            int part = 0;
            int zone = 0;
            int idx = 0;
            int chnlMask = 1;
            int signalLevel = 0;
            bool flag = true;
            string type = message.Type;
            if (!(type == "MESSAGE_OKO2_RM"))
            {
                if (!(type == "MESSAGE_OKO1_RM"))
                {
                    if (!(type == "MESSAGE_GUARD_RM"))
                    {
                        if (type == "MESSAGE_SYSTEM_RM")
                        {
                            array[0] = message.Get("Command");
                            int num = (int)array[0];
                            if (num != 128)
                            {
                                if (num == 186)
                                {
                                    array[1] = message.Get("Result");
                                    array[2] = message.Get("Text");
                                }
                            }
                            else
                            {
                                array[1] = message.Get("Result");
                            }
                        }
                    }
                    else
                    {
                        array[0] = message.Get("Result");
                        array[1] = message.Get("Part");
                        array[2] = message.Get("Status");
                        array[3] = message.Get("Code");
                        array[4] = message.Get("User");
                        array[5] = message.Get("ChannelsMask");
                    }
                }
                else
                {
                    flag = false;
                    okoVersion = 1;
                    retrNumber = message.Get("RetrNumber").ToInt();
                    code = message.Get("Code").ToInt();
                    objectNumber = message.Get("Object").ToInt();
                    chnlMask = message.Get("ChannelsMask").ToInt();
                }
            }
            else
            {
                flag = false;
                okoVersion = 2;
                retrNumber = message.Get("RadioRetr").ToInt();
                objectNumber = message.Get("Object").ToInt();
                _class = message.Get("Class").ToInt();
                code = message.Get("Code").ToInt();
                part = message.Get("Part").ToInt();
                zone = message.Get("Zone").ToInt();
                idx = message.Get("Number").ToInt();
                chnlMask = message.Get("ChannelsMask").ToInt();
                signalLevel = message.Get("Attributes").ToInt();
            }
            string.Concat(new object[]
            {
                address,
                ": =",
                timeStamp,
                "= : ",
                string.Join(",", array)
            });
            if (!flag)
            {
                Logger.Log("Объект: " + message.Get("Object").ToInt() + ", type = " + message.Type +
                        ", code = " + message.Get("Code").ToString() +
                        ", text = " + message.Get("Text").ToString(), Logger.LogLevel.DEBUG);
                Handling.ProcessingEvent(okoVersion, regionId, objectNumber, retrNumber, _class, code, part, zone, chnlMask, idx, DateTime.Now, signalLevel, 0, message.Address);
            }
            else
            {
                Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 
                    "Сообщение не было обработано" ), Logger.LogLevel.DEBUG);
                try
                {
                    Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 
                        "Объект: " + message.Get("Object").ToInt() + ", type = " + message.Type +
                        ", code = " + message.Get("Code").ToString() + 
                        ", text = " + message.Get("Text").ToString()), Logger.LogLevel.DEBUG);
                }
                catch { }
            }
        }

        public static bool ProcessingXmlEvent(OKOGate.Message msg)
        {
            int objectNumber = OKOGate.SStr.StrToInt(msg.Get("Object").ToString());
            DateTime timeStamp = msg.TimeStamp;
            int alarmGroupId = msg.Get("AlarmGroupId").ToInt();
            int num = OKOGate.SStr.StrToInt(msg.Get("UnpackError").ToString());
            int code = OKOGate.SStr.StrToInt(msg.Get("Code").ToString());
            int num2 = 1;
            int part = OKOGate.SStr.StrToInt(msg.Get("Part").ToString());
            int zone = OKOGate.SStr.StrToInt(msg.Get("Zone").ToString());
            int @class = OKOGate.SStr.StrToInt(msg.Get("Class").ToString());
            string address = msg.Address;
            if (!Utils.ListenIP(address))
            {
                return false;
            }
            int regionId = Utils.RegionByIP(address);
            if (num == 0)
            {
                num2 = OKOGate.SStr.StrToInt(msg.Get("TypeNumber").ToString());
                if (num2 == 0)
                {
                    num2 = 1;
                }
            }
            return Handling.ProcessingEvent(2, regionId, objectNumber, 0, @class, code, part, zone, 1, num2, timeStamp, 0, alarmGroupId, address);
        }

        public static void AcceptAlarm(long ObjectId)
        {
            HandleAlarm(ObjectId, 1);
        }

        public static void CancelAlarm(long ObjectId)
        {
            HandleAlarm(ObjectId, 2);
        }

        public static void TestAlarm(long ObjectId)
        {
            HandleAlarm(ObjectId, 3);
        }

        public static void HandleAlarm(long ObjectId, int Code)
        {
            AKObject aKObject = new AKObject(ObjectId);
            DataBase.RunCommandInsert("oko.event", new Dictionary<string, object>
            {
                { "oko_version", 10},
                { "address", "'127.0.0.1'" },
                { "objectnumber", aKObject.number},
                { "alarmgroupid", 0 },
                { "code", Code },
                { "partnumber", 0 },
                { "zoneusernumber", 0 },
                { "typenumber", 1 },
                { "class", 10 },
                {
                    "datetime",
                    (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()).Q()
                },
                { "region_id", aKObject.RegionId }
            });
        }
    }
}
