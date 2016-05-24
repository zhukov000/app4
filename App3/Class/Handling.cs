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

        private static bool ProcessingEvent(int OkoVersion, int RegionId, int ObjectNumber, int RetrNumber, int Class, int Code, int Part, int Zone, int ChnlMask, int Idx, DateTime TimeStamp, int SignalLevel, int AlarmGroupId = 0, string Address = "")
        {
            bool ret = true;

            int eventId = 0;
            AKObject obj = new AKObject(ObjectNumber, RegionId);

            MessageGroupId mgroup_id = DBDict.TMessage[OkoVersion, Class, Code].Item1;
            Tuple<DateTime, int> lt = EventDispatcher.Instance[obj.Id, mgroup_id, Idx];

            if (TimeStamp.Subtract(lt.Item1).TotalSeconds < DBDict.Settings["TIMEOUT_MESSAGE_INTERVAL"].ToInt() || lt.Item2 == Idx)
            {
                ret = false;
            }

            try
            {
                object[] pResult = new object[0];

                DataBase.RunCommandInsert(
                    "oko.event",
                    new Dictionary<string, object>
                    {
                        {"objectnumber", ObjectNumber},
                        {"alarmgroupid", AlarmGroupId},
                        {"datetime", TimeStamp.Q()},
                        {"code", Code},
                        {"typenumber", Idx},
                        {"partnumber", Part},
                        {"zoneusernumber", Zone},
                        {"class", Class},
                        {"address", Address.Q()},
                        {"region_id", RegionId},
                        {"channelnumber", ChnlMask},
                        {"oko_version",  OkoVersion},
                        {"retrnumber", RetrNumber },
                        { "isrepeat", !ret },
                        { "siglevel", SignalLevel }
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


            if (ret && obj.IsExists())
            {
                EventDispatcher.Instance[obj.Id, mgroup_id, Idx] = new Tuple<DateTime, int>(DateTime.Now, Idx);
                string MessText = string.Format("{0}({1})", DBDict.TMessage[OkoVersion, Class, Code].Item2, DBDict.TMessage[OkoVersion, Class, Code].Item3) ;
                MessText = string.Format("Объект расположенный по адресу:{0}, \r\nсообщает:{1}", obj.AddressStr, MessText);
                if (GetMessageEvent != null)
                {
                    GetMessageEvent(eventId, mgroup_id, obj, MessText, string.Join(", ", obj.GetContacts()), TimeStamp.ToString());
                }
                ret = true;
            }


            return ret;
        }

        public static void ProcessingComEvent(object Message)
        {
            // throw new NotImplementedException();
            GuardAgent2.Message msg = (GuardAgent2.Message)Message;
            string Var1 = msg.Address;
            DateTime Var2 = msg.TimeStamp;
            object[] Var = new object[9];
            int ObjectNumber = 0;
            int RegionId = Config.Get("CurrenRegion").ToInt();
            int OkoVersion = 0;
            int RetrNumber = 0;
            int Class = 0;
            int Code = 0;
            int Part = 0;
            int Zone = 0;
            int Idx = 0;
            int ChnlMask = 1;
            int SignalLevel = 0;
            bool f = true;
            Logger.Instance.WriteToLog(msg.Type);
            switch (msg.Type)
            {
                case "MESSAGE_OKO2_RM":
                    f = false;
                    OkoVersion = 2;
                    RetrNumber = msg.Get("RadioRetr").ToInt();
                    ObjectNumber = msg.Get("Object").ToInt();
                    Class = msg.Get("Class").ToInt();
                    Code = msg.Get("Code").ToInt();
                    Part = msg.Get("Part").ToInt();
                    Zone = msg.Get("Zone").ToInt();
                    Idx = msg.Get("Number").ToInt();
                    ChnlMask = msg.Get("ChannelsMask").ToInt();
                    SignalLevel = msg.Get("Attributes").ToInt();
                    break;
                case "MESSAGE_OKO1_RM":
                    f = false;
                    OkoVersion = 1;
                    RetrNumber = msg.Get("RetrNumber").ToInt();
                    Code = msg.Get("Code").ToInt();
                    ObjectNumber = msg.Get("Object").ToInt();
                    ChnlMask = msg.Get("ChannelsMask").ToInt();
                    break;
                case "MESSAGE_GUARD_RM":
                    Var[0] = msg.Get("Result");
                    Var[1] = msg.Get("Part");
                    Var[2] = msg.Get("Status");
                    Var[3] = msg.Get("Code");
                    Var[4] = msg.Get("User");
                    Var[5] = msg.Get("ChannelsMask");
                    break;
                case "MESSAGE_SYSTEM_RM":
                    Var[0] = msg.Get("Command");
                    switch ((int)Var[0])
                    {
                        case 128:
                            Var[1] = msg.Get("Result");
                            break;
                        case 186:
                            Var[1] = msg.Get("Result");
                            Var[2] = msg.Get("Text");
                            break;
                    }
                    break;
            }
            string s = Var1 + ": =" + Var2 + "= : " + string.Join(",", Var);
            Logger.Instance.WriteToLog(s);
            if (f)
            {
               
            }
            else
            {
                ProcessingEvent(OkoVersion, RegionId, ObjectNumber, RetrNumber, Class, Code, Part, Zone, ChnlMask, Idx, DateTime.Now, SignalLevel, 0, msg.Address);
            }
        }

        public static bool ProcessingXmlEvent(OKOGate.Message msg)
        {
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
                if (TypeNumber == 0) TypeNumber = 1;
            }            
            return ProcessingEvent(2, Region, ObjectNumber, 0, Class, Code, PartNumber, ZoneUserNumber, 1, TypeNumber, TimeStamp, 0, AlarmGroupId, Address); ;
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
