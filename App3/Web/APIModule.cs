using Nancy;
using Nancy.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nancy.Responses.Negotiation;
using Nancy.Authentication.Forms;
using Nancy.Responses;
using System.Text;
using App3.Class.Static;
using App3.Class;
using Nancy.Security;
using App3.Class.Singleton;

namespace App3.Web
{
    public class APIModule : NancyModule
    {
        private string[] m_availableObjectIds = new string[] {
			"61101000000000000000000", "61043000000000000000000", "61042000000000000000000",
			"61105000000000000000000", "61010000000000000000000", "61011000000000000000000",
			"61104000000000000000000", "61017000000000000000000", "61018000000000000000000",
			"61020000000000000000000", "61021000000000000000000", "61023000000000000000000",
			"61024000000000000000000", "61025000000000000000000", "61028000000000000000000",
			"61031000000000000000000", "61032000000000000000000", "61033000000000000000000",
			"61034000000000000000000", "61100000000000000000000", "61035000000000000000000",
			"61036000000000000000000", "61037000000000000000000", "61039000000000000000000",
			"61110000000000000000000", "61038000000000000000000", "61040000000000000000000",
			"61008000000000000000000", "61012000000000000000000", "61014000000000000000000",
			"61106000000000000000000", "61109000000000000000000", "61103000000000000000000",
			"61016000000000000000000", "61005000000000000000000", "61107000000000000000000",
			"61009000000000000000000", "61111000000000000000000", "61108000000000000000000",
			"61030000000000000000000", "61102000000000000000000", "61026000000000000000000",
			"61003000000000000000000", "61004000000000000000000", "61041000000000000000000",
			"61015000000000000000000", "61022000000000000000000", "61013000000000000000000",
			"61029000000000000000000", "61027000000000000000000", "61002000000000000000000",
			"61019000000000000000000", "61006000000000000000000", "61044000000000000000000",
			"61007000000000000000000"
		};

        private Random m_random = new Random();

        private JObject generateAreas()
        {
            var areas = new JObject();
            var maxObjectsCountInResponse = m_random.Next(10, m_availableObjectIds.Count());
            for (var i = 0; i < maxObjectsCountInResponse; ++i)
            {
                var currentIndex = m_random.Next(m_availableObjectIds.Count());
                try
                {
                    areas.Add(m_availableObjectIds[currentIndex], m_random.Next(100));
                }
                catch (System.ArgumentException)
                {
                    continue;
                }
            }

            return areas;
        }

        private JArray generateColorsArray()
        {
            var result = new JArray();
            var maxColors = m_random.Next(6, 15);
            var prevValue = -1;
            for (var i = 0; i < maxColors; ++i)
            {
                var currentObject = new JObject();

                {
                    var builder = new StringBuilder();
                    builder.Append("rgba(");
                    builder.Append(m_random.Next(255));
                    builder.Append(", ");
                    builder.Append(m_random.Next(255));
                    builder.Append(", ");
                    builder.Append(m_random.Next(255));
                    builder.Append(", 0.3)");
                    currentObject.Add("color", builder.ToString());
                }

                var currentValue = m_random.Next(prevValue + 1, prevValue + 10);
                currentObject.Add("minValue", m_random.Next(prevValue, currentValue));
                prevValue = currentValue;

                result.Add(currentObject);
            }

            return result;
        }

        public APIModule()
        {
            this.ENableCors();

            Get["/"] = x =>
            {

                Logger.Instance.WriteToLog("Web Root");
                IDictionary<string, object> owinEnvironment = base.Context.GetOwinEnvironment();
                Stream arg_1C_0 = (Stream)owinEnvironment["owin.RequestBody"];
                IDictionary<string, string[]> dictionary = (IDictionary<string, string[]>)owinEnvironment["owin.RequestHeaders"];
                string arg = (string)owinEnvironment["owin.RequestMethod"];
                string arg_4F_0 = (string)owinEnvironment["owin.RequestPath"];
                string arg_60_0 = (string)owinEnvironment["owin.RequestPathBase"];
                string arg_71_0 = (string)owinEnvironment["owin.RequestProtocol"];
                string arg_82_0 = (string)owinEnvironment["owin.RequestQueryString"];
                string arg_93_0 = (string)owinEnvironment["owin.RequestScheme"];
                Stream arg_A4_0 = (Stream)owinEnvironment["owin.ResponseBody"];
                IDictionary<string, string[]> arg_B5_0 = (IDictionary<string, string[]>)owinEnvironment["owin.ResponseHeaders"];
                string arg_C6_0 = (string)owinEnvironment["owin.Version"];
                CancellationToken arg_D7_0 = (CancellationToken)owinEnvironment["owin.CallCancelled"];
                string text = string.Concat(new string[]
                {
                    (string)owinEnvironment["owin.RequestScheme"],
                    "://",
                    dictionary["Host"].First<string>(),
                    (string)owinEnvironment["owin.RequestPathBase"],
                    (string)owinEnvironment["owin.RequestPath"]
                });
                if (owinEnvironment["owin.RequestQueryString"] != "")
                {
                    text = text + "?" + (string)owinEnvironment["owin.RequestQueryString"];
                }
                return string.Format("{0} {1} Версия сервера: {2}", arg, text, Config.APPVERSION);
            };

            Get["/data"] = x =>
            {
                // Utils.UpdateDistrictStatuses();
                string query_dist = "select fullname as name, num as region_id, fias, color from regions2map reg inner join oko.ipaddresses addr on reg.num = addr.id_region where addr.listen order by reg.name";

                var responseContentArray = new JArray();
                // общая статистика
                var responseContentObject = new JObject();
                responseContentObject.Add("name", "Ростовская область");
                responseContentObject.Add("region_id", "0");
                responseContentObject.Add("fias", "61000000000000000000000");
                responseContentObject.Add("color", "#B39494");
                int num = 0;
                foreach (List<object[]> stats in Utils.GetStatistic(0))
                {
                    var arr = new JArray();
                    foreach (object[] row_stat in stats)
                    {
                        var row_obj = new JObject();
                        row_obj.Add("title", row_stat[0].ToString());
                        row_obj.Add("cnt", row_stat[2].ToInt());
                        arr.Add(row_obj);
                    }
                    responseContentObject.Add("stat" + num.ToString(), arr);
                    num++;
                }
                responseContentArray.Add(responseContentObject);
                // статистика по каждому муниципальному обр.
                foreach (object[] row in DataBase.RowSelect(query_dist))
                {
                    responseContentObject = new JObject();
                    responseContentObject.Add("name", row[0].ToString());
                    responseContentObject.Add("region_id", row[1].ToInt());
                    responseContentObject.Add("fias", row[2].ToString());
                    responseContentObject.Add("color", row[3].ToString());
                    num = 0;
                    foreach (List<object[]> stats in Utils.GetStatistic(row[1].ToInt()))
                    {
                        var arr = new JArray();
                        foreach(object[] row_stat in stats)
                        {
                            var row_obj = new JObject();
                            row_obj.Add("title", row_stat[0].ToString());
                            row_obj.Add("cnt", row_stat[2].ToInt());
                            arr.Add(row_obj);
                        }
                        responseContentObject.Add("stat" + num.ToString(), arr);
                        num++;
                    }
                    responseContentArray.Add(responseContentObject);
                }
                var response = (Response)responseContentArray.ToString();
                response.ContentType = "application/json";
                return response;
            };

            Get["/areas/{x1}/{y1}/{x2}/{y2}"] = x =>
            {
                 // TODO
                var responseContentObject = new JObject();
                responseContentObject.Add("areas", generateAreas());
                responseContentObject.Add("colors", generateColorsArray());
                var response = (Response)responseContentObject.ToString();
                response.ContentType = "application/json";
                return response;
            };

            Get["/events/{regid?}"] = x =>
            {
                int region_id = Config.Get("CurrenRegion", "-1").ToInt();
                if (x.regid != null)
                {
                    region_id = x.regid;
                }
                var responseContentArray = new JArray();
                
                foreach (IDictionary<string, object> data in Utils.GetObjectsStatuses(region_id))
                {
                    data.Add("message", string.Format("{0}({1})", DBDict.TMessage[data["oko_version"].ToInt(), data["class"].ToInt(), data["code"].ToInt()].Item2, DBDict.TMessage[data["oko_version"].ToInt(), data["class"].ToInt(), data["code"].ToInt()].Item3));
                    responseContentArray.Add(JObject.FromObject(data));
                }

                var response = (Response)responseContentArray.ToString();
                response.ContentType = "application/json";
                return response;
            };

            Get["/objects/{regid?}"] = x =>
            {
                int region_id = Config.Get("CurrenRegion", "-1").ToInt();
                if (x.regid != null)
                {
                    region_id = x.regid;
                }
                var responseContentArray = new JArray();

                foreach (IDictionary<string, object> data in Utils.GetObjects(region_id))
                {
                    responseContentArray.Add(JObject.FromObject(data));
                }

                var response = (Response)responseContentArray.ToString();
                response.ContentType = "application/json";
                return response;
            };

            Get["/dicts"] = x =>
            {
                var array = new JArray();

                var responseContentObject = new JObject();
                foreach (var data in DBDict.TState)
                {
                    var obj = JObject.FromObject(data);
                    array.Add(obj);
                }
                responseContentObject.Add("state", array);

                array = new JArray();
                foreach (var data in DBDict.TStatus)
                {
                    var obj = new JObject();
                    obj.Add("id", data.Key);
                    obj.Add("name", data.Value);
                    array.Add(obj);
                }
                responseContentObject.Add("status", array);

                array = new JArray();
                foreach (var data in DBDict.TRegion)
                {
                    var obj = new JObject();
                    obj.Add("id", data.Key);
                    obj.Add("fullname", data.Value.Item1);
                    obj.Add("name", data.Value.Item2);
                    obj.Add("color", data.Value.Item3);
                    array.Add(obj);
                }
                responseContentObject.Add("region", array);

                var response = (Response)responseContentObject.ToString();
                response.ContentType = "application/json";
                return response;
            };
            
            Get["/test"] = _ =>
            {
                var responseContentObject = new JObject();
                responseContentObject.Add("test", this.Request.UserHostAddress);
                var response = (Response)responseContentObject.ToString();
                response.ContentType = "application/json";
                return response;
            };

            // получение объектов для синхронизации
            Get["/get_new/{date}/{nid?}"] = x =>
            {
                var responseContentArray = new JArray();
                int region_id = Config.Get("CurrenRegion", "-1").ToInt();
                // если регион не установлен в конфигурации - метод не поддерживается
                if (region_id != -1)
                {
                    Logger.Instance.WriteToLog(string.Format("Web GET_NEW: {0}", this.Request.UserHostAddress));
                    // проверить разрешено ли этому узлу получать данные
                    if (DataBase.RowSelectCount(string.Format("SELECT * FROM syn_nodes WHERE synout AND ipv4 = '{0}' ", this.Request.UserHostAddress/*, x.nid*/)) > 0)
                    {
                        foreach (var el in Synchronizer.SYNC_NEW)
                        {
                            var responseContentObject = new JObject();
                            // имя объекта синхронизации
                            string name = el.Key;
                            responseContentObject.Add("objectname", name);
                            // тип синхронизации
                            responseContentObject.Add("type", el.Value.Type.ToString());
                            // ключевое поле
                            responseContentObject.Add("keyfield", el.Value.KeyField);

                            // точка актуальности
                            DateTime dt = Utils.StringToDateTime(x.date.ToString());

                            var jArray = new JArray();
                            foreach (KeyValuePair<string, object>[] pairs in el.Value.getReversed(dt))
                            {
                                var jArray2 = new JArray();
                                foreach (var pair in pairs)
                                {
                                    var jObject = new JObject();
                                    jObject.Add("key", pair.Key);
                                    jObject.Add("val", pair.Value.ToString());
                                    jArray2.Add(jObject);
                                }
                                jArray.Add(jArray2);
                            }
                            responseContentObject.Add("reversed", jArray);
                            jArray = new JArray();
                            foreach (var pair in el.Value.getDeleted(dt))
                            {
                                var jObject = new JObject();
                                jObject.Add(pair.Key, pair.Value.ToString());
                                jArray.Add(jObject);
                            }
                            responseContentObject.Add("deleted", jArray);
                            responseContentArray.Add(responseContentObject);
                        }
                    }
                }
                var response = (Response)responseContentArray.ToString();
                response.ContentType = "application/json";
                return response;
            };
        }

    }
}
