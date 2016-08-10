using App3.Class.Singleton;
using App3.Class.Synchronization;
using App3.Web;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App3.Class.Static
{
    static class Synchronizer
    {
        static public event Action SyncStart = null;
        static public event Action SyncStop = null;

        public enum Status_Sync { RUN, SUSPEND };
        static private Status_Sync status = Status_Sync.SUSPEND;
        static public Status_Sync Status
        {
            get
            {
                return status;
            }
        }
        /// <summary>
        /// Список объектов для синхронизации
        /// </summary>
        static public Entity[] SYNC_NEW = new Entity[] 
        {
            new EventSync(),
            new SyncAddresses(),
            new SyncClassifier(),
            new SyncCompany(),
            new SyncContact(),
            new SyncContract(),
            new SyncCustomer(),
            new SyncIPAddresses(),
            new SyncMessageText(),
            new SyncObject(),
            new SyncObjectInContract(),
            new SyncObjectProperties(),
            new SyncRealObject(),
            new SyncRegionStatus(),
            new SyncTContact(),
            new SyncTMessages(),
            new SyncTNumber(),
            new SyncTState(),
            new SyncTStatus(),
            new SyncZone()
        };

        static Thread backgroundThread = null;
        public static bool NeedUpdate = false;

        static public void Start()
        {
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    while (true)
                    {
                        List<SyncResult> Data = null;
                        Run(ref Data);
                        if (Data.Count > 0)
                        {
                            Logger.Instance.WriteToLog("Произведена автоматическая синхронизация с узлами");
                            foreach (SyncResult data in Data)
                            {
                                Logger.Instance.WriteToLog(
                                    string.Format("IP-адрес: {0} Район: {5} Статус: {1} Получено байт: {2} Изменено: {3} Удалено: {4}", data.IpAddress, data.Status, data.Bytes, data.Reserved, data.Deleted, data.Description));
                            }
                        }
                        Thread.Sleep(Config.Get("SynchSleepMinutes").ToInt() * 60 * 1000);
                    }
                }
            ));
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        static public string getTableName(string Name)
        {
            string s = "";
            foreach (var x in SYNC_NEW)
            {
                if (x.ToString() == Name)
                {
                    s = x.getTableName();
                    break;
                }
            }
            return s;
        }

        static public List<KeyValuePair<string, object>[]> getReversed(string table, DateTime dt)
        {
            return DataBase.RowSelect(string.Format("SELECT * FROM {0} WHERE dt > '{1}'", table, dt.ToString()), false);
        }

        static public List<object[]> getDeleted(string table, DateTime dt)
        {
            return DataBase.RowSelect(string.Format("SELECT * FROM deleted WHERE tablename = '{0}' and dt > '{1}'", table, dt.ToString()));
        }

        static private int[] SyncObject(JsonNewObject obj)
        {
            // Для Event-ов:
            // 1) удаление не выполняется; 2) новые события просто добавляются
            // Для всех остальных:
            // 1) удалить то, что удалено; 2) проверить существование "новых" объектов, если есть - обновить, иначе - добавить

            int deleted = 0;
            int reversed = 0;
            string field = "id";
            string ReqIns = "INSERT INTO oko." + obj.objectname + "({0}) VALUES({1})";
            string ReqDel = "DELETE FROM oko." + obj.objectname + " WHERE {1} = {2}";

            if (obj.objectname == "event")
            {    
                foreach(List<Attr> lll in obj.reversed)
                {
                    List<string> fields = new List<string>();
                    List<string> values = new List<string>();
                    foreach(Attr aaa in lll)
                    {
                        if (aaa.key == "id")
                            continue;
                        if (aaa.val != "")
                        {
                            fields.Add(aaa.key);
                            values.Add(aaa.val.Q());
                        }
                    }
                    if (fields.Count > 0)
                    {
                        bool f = true;
                        int ii = 0;
                        while (f)
                        {
                            string s = string.Format(ReqIns, string.Join(",", fields), string.Join(",", values));
                            try
                            {
                                reversed += DataBase.RunCommand(s);
                                f = false;
                            }
                            catch (Exception ex)
                            {
                                ii++;
                                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                                Logger.Instance.WriteToLog(s);
                                if (ii > 3) break;
                            }
                        }
                    }
                }
                
            }
            else
            {
                if (obj.objectname == "object") field = "osm_id";
                // 1. Удаление
                foreach (Deleted lll in obj.deleted)
                {
                    try
                    {
                        deleted += DataBase.RunCommand(string.Format(ReqDel, field, lll.id));
                    }
                    catch(Exception ex)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                        continue;
                    }
                }
                // 2. Модификация
                foreach (List<Attr> lll in obj.reversed)
                {
                    List<string> fields = new List<string>();
                    List<string> values = new List<string>();
                    List<string> pairs = new List<string>();
                    string vid = "";

                    foreach (Attr aaa in lll)
                    {
                        if (aaa.key == field) vid = aaa.val;
                        fields.Add(aaa.key);
                        if (aaa.val != "")
                        {
                            values.Add(aaa.val.Q());
                            pairs.Add(string.Format("{0} = '{1}'", aaa.key, aaa.val));
                        }
                        else
                        {
                            values.Add("null");
                            pairs.Add(string.Format("{0} = null", aaa.key));
                        }
                    }
                    int iii = 0;
                    // обновить
                    if (vid != "")
                    {
                        try
                        {
                            iii = DataBase.RunCommand("UPDATE oko." + obj.objectname + " SET " +
                                 string.Join(", ", pairs) +
                                 " WHERE " + field + " = " + lll.First(x => x.key == field).val);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                        }
                    }
                    // если ничего не обновлено -> вставить
                    if (iii == 0)
                    {
                        try
                        {
                            reversed += DataBase.RunCommand(string.Format(ReqIns, string.Join(",", fields), string.Join(",", values)));
                        }
                        catch(Exception ex)
                        {
                            Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                            continue;
                        }
                    }
                    else
                    {
                        reversed += iii;
                    }
                }
            }

            return new int[] { deleted, reversed };
        }
        

        static public List<string> Run(ref List<SyncResult> Data)
        {
            if (Data == null) Data = new List<SyncResult>();
            List<string> s = new List<string>();
            // +1. Получить узлы, для которых выполнить синхронизацию (nid, ipaddress)
            // +2. Для каждого узла:
            // +2.1 Получить из БД дату последней синхронизации с узлом (nid, таблица synchronize, поле start) - если записи нет, дата и время = 01.01.2016
            // +2.2 Синхронизировать
            // +3. Зафиксировать дату синхронизации в журнале
            lock (new object())
            {
                if (SyncStart != null)
                {
                    try
                    {
                        SyncStart();
                    }
                    catch(Exception ex)
                    {
                        Logger.Instance.WriteToLog("Событие SyncStart вызвало исключение: " + ex.Message);
                    }
                }
                Data.Clear();
                s.Add("запуск...");
                status = Status_Sync.RUN;
                List<object[]> nodes = DataBase.RowSelect("select sn.id, sn.ipv4, sn.port, ip.description from syn_nodes sn left join oko.ipaddresses ip on ipv4 = ip.ipaddress where sn.synin");
                foreach (object[] node in nodes)
                {
                    if (!Utils.IsLocalIpAddress(node[1].ToString()))
                    {
                        SyncResult syncRes = new SyncResult();
                        int reversed = 0;
                        int deleted = 0;
                        DateTime start = DateTime.Now;
                        System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
                        swatch.Start();

                        DateTime last = DateTime.Now.AddDays(-3);
                        object last2 = DataBase.First(string.Format("select id, start from synchronize where node_id = {0} order by start desc", node[0]), "start");
                        if (last2 != null)
                            last = DateTime.Parse(last2.ToString());
                        s.Add("соединяюсь с " + node[1] + "...");
                        string url = String.Format("http://{0}:{3}/get_new/{1}/{2}", node[1], Utils.DateTimeToString(last), node[0], node[2]);
                        syncRes.IpAddress = node[1].ToString();
                        Logger.Instance.WriteToLog(string.Format("соединение с {0}: {1}", node[1], url));
                        string content = Utils.getHtmlContent(url);
                        syncRes.Description = node[3].ToString();
                        if (content.Length > 0)
                        {
                            syncRes.Status = SyncStatus.OK;
                            s.Add("успех: " + content.Length + " байт");
                            syncRes.Bytes = content.Length;
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            js.MaxJsonLength = int.MaxValue;
                            JsonNewObject[] objects = js.Deserialize<JsonNewObject[]>(content);
                            bool fromServer = (node[1].ToString() == DBDict.Settings["SERVER_ADDRESS"].ToString());
                            foreach (JsonNewObject obj in objects)
                            {
                                s.Add(obj.objectname);
                                if (!fromServer && obj.objectname != "event")
                                    continue;
                                int[] a = SyncObject(obj);
                                deleted += a[0];
                                reversed += a[1];
                                string s1 = string.Format("{2} удалено {0}, изменено {1}", a[0], a[1], obj.objectname); 
                                s.Add(s1);
                                Logger.Instance.WriteToLog(s1);
                            }
                            s.Add(string.Format("Всего: удалено {0}, изменено {1}", deleted, reversed));
                            syncRes.Reserved = reversed;
                            syncRes.Deleted = deleted;
                        }
                        else
                        {
                            syncRes.Status = SyncStatus.EMPTY;
                            s.Add("получен пустой ответ");
                        }
                        swatch.Stop();
                        DateTime finish = start + swatch.Elapsed;
                        DataBase.RunCommandInsert("synchronize", new Dictionary<string, object>()
                            {
                                {"start", start.ToString().Q()},
                                {"finish", finish.ToString().Q()},
                                {"reversed", reversed},
                                {"deleted", deleted},
                                {"node_id", node[0].ToInt()}
                            }
                        );
                        Data.Add(syncRes);
                    }
                    else
                    {
                        s.Add(string.Format("Пропуск {0} - локальный адрес", node[1]));
                    }
                    s.Add("... синхронизация завершена");
                }
                status = Status_Sync.SUSPEND;
                if (SyncStop != null)
                {
                    try
                    {
                        SyncStop();
                    }
                    catch(Exception ex)
                    {
                        Logger.Instance.WriteToLog("Событие SyncStop вызвало исключение: " + ex.Message);
                    }
                }
            }
            return s;
        }
    }
}
