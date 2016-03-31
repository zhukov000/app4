using App3.Class.Singleton;
using App3.Class.Static;
using OKOGate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public static bool ProcessingEvent(OKOGate.Message msg)
        {
            bool ret = true;
            int ObjectNumber = SStr.StrToInt(msg.Get("Object").ToString());
            DateTime TimeStamp = msg.TimeStamp;
            int AlarmGroupId = msg.Get("AlarmGroupId").ToInt();
            int UnpackError = SStr.StrToInt(msg.Get("UnpackError").ToString());
            int Code = SStr.StrToInt(msg.Get("Code").ToString());
            int TypeNumber = 1;
            int PartNumber = SStr.StrToInt(msg.Get("Part").ToString());
            int ZoneUserNumber = SStr.StrToInt(msg.Get("Zone").ToString());
            int Class = SStr.StrToInt(msg.Get("Class").ToString());
            string Address = msg.Address;

            if (!Utils.ListenIP(Address))
            {
                return false;
            }
            int Region = Utils.RegionByIP(Address);
            if (UnpackError == 0)
            {
                TypeNumber = SStr.StrToInt(msg.Get("TypeNumber").ToString());
                if (TypeNumber == 0) TypeNumber = 1; // ?
            }
            bool f = true;
            int eventId = 0;
            try
            {
                string req = DataBase.Help4Select(
                    "oko.event",
                    new Dictionary<string, object>(),
                    new Dictionary<string, object>
                    {
                        {"objectnumber", ObjectNumber},
                        {"alarmgroupid", AlarmGroupId},
                        {"code", Code},
                        {"typenumber", TypeNumber},
                        {"partnumber", PartNumber},
                        {"zoneusernumber", ZoneUserNumber},
                        {"class", Class},
                        {"address", Address.Q()},
                        {"region_id", Region}
                    }
                );

                req = req.Substring(9);
                object t = DataBase.First(string.Format("SELECT max(datetime) as mx {0} ", req), "mx");
                DateTime tt = DateTime.Today;

                if (DateTime.TryParse(t.ToString(), out tt))
                {
                    if (TimeStamp.Subtract(tt).TotalSeconds < DBDict.Settings["TIMEOUT_MESSAGE_INTERVAL"].ToInt())
                    {
                        f = false;
                    }
                }

                object[] pResult = new object[0];

                DataBase.RunCommandInsert(
                    "oko.event",
                    new Dictionary<string, object>
                    {
                        {"objectnumber", ObjectNumber},
                        {"alarmgroupid", AlarmGroupId},
                        {"datetime", TimeStamp.Q()},
                        {"code", Code},
                        {"typenumber", TypeNumber},
                        {"partnumber", PartNumber},
                        {"zoneusernumber", ZoneUserNumber},
                        {"class", Class},
                        {"address", Address.Q()},
                        {"region_id", Region}
                    },
                    "id", 
                    out pResult
                );
                if (pResult.Count() > 0)
                {
                    eventId = pResult[0].ToInt();
                }
            }
            catch(Exception ex)
            {
                ret = false;
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            AKObject obj = new AKObject(ObjectNumber, Region);
            string MessText = Utils.GetMessageText(Class, Code);
            f = true;
            if (f && obj.IsExists())
            {
                MessText = string.Format("Объект расположенный по адресу:{0}, \r\nсообщает:{1}", obj.AddressStr, MessText);
                if (GetMessageEvent != null)
                {
                    GetMessageEvent(eventId, (MessageGroupId)Utils.MessageGroup(Class, Code, 2), obj, MessText, string.Join(", ", obj.GetContacts()), TimeStamp.ToString());
                }
                ret = true;
            }
            
            return ret;
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
            AKObject obj = new AKObject(ObjectId);
            DataBase.RunCommandInsert(
                "oko.event",
                new Dictionary<string, object>() 
                { 
                    {"oko_version", 10},
                    {"address", "'127.0.0.1'"},
                    {"objectnumber", obj.number},
                    {"alarmgroupid",0},
                    {"code", Code},
                    {"partnumber", 0},
                    {"zoneusernumber", 0},
                    {"typenumber", 1},
                    {"class", 10},
                    {"datetime", (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()).Q()},
                    {"region_id", obj.RegionId}
                }
            );
        }
    }
}
