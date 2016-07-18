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
using Nancy.Responses;
using System.Text;
using App3.Class.Static;
using App3.Class;
using App3.Class.Singleton;

namespace App3.Web
{
    public class APIModule : NancyModule
    {
        private JObject getAreas(double x1, double y1, double x2, double y2)
        {
            var areas = new JObject();
            Logger.Instance.WriteToLog(String.Format("{0} {1} {2} {3}", x1, y1, x2, y2));
            /*List<object[]> db_areas = DataBase.RowSelect(string.Format("SELECT osm_id, num, fias FROM regions2map WHERE "));

            foreach (object[] row in db_areas)
            {
                // var currentIndex = m_random.Next(m_availableObjectIds.Count());
                try
                {
                    // areas.Add(fias, число работающих?);
                }
                catch (System.ArgumentException)
                {
                    continue;
                }
            }*/

            return areas;
        }

        public APIModule()
        {
            this.ENableCors();

            Get["/"] = x => {
                var env = this.Context.GetOwinEnvironment();

                var requestBody = (Stream)env["owin.RequestBody"];
                var requestHeaders = (IDictionary<string, string[]>)env["owin.RequestHeaders"];
                var requestMethod = (string)env["owin.RequestMethod"];
                var requestPath = (string)env["owin.RequestPath"];
                var requestPathBase = (string)env["owin.RequestPathBase"];
                var requestProtocol = (string)env["owin.RequestProtocol"];
                var requestQueryString = (string)env["owin.RequestQueryString"];
                var requestScheme = (string)env["owin.RequestScheme"];

                var responseBody = (Stream)env["owin.ResponseBody"];
                var responseHeaders = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];

                var owinVersion = (string)env["owin.Version"];
                var cancellationToken = (CancellationToken)env["owin.CallCancelled"];

                var uri = (string)env["owin.RequestScheme"] + "://" + requestHeaders["Host"].First() +
                  (string)env["owin.RequestPathBase"] + (string)env["owin.RequestPath"];

                if (env["owin.RequestQueryString"] != "")
                    uri += "?" + (string)env["owin.RequestQueryString"];

                return string.Format("{0} {1}", requestMethod, uri);
            };

            Get["/areas/{x1}/{y1}/{x2}/{y2}"] = x =>
            {
                var responseContentObject = new JObject();
                getAreas(x.x1, x.y1, x.x2, x.y2);
                // responseContentObject.Add("areas", generateAreas());
                // responseContentObject.Add("colors", generateColorsArray());
                var response = (Response)responseContentObject.ToString();
                response.ContentType = "application/json";
                return response;
            };

            Get["/objects/{x1}/{y1}/{x2}/{y2}"] = x =>
            {
                return new Response();
            };

            Get["/stats/{id}"] = x =>
            {
                return new Response();
            };

            Get["/test"] = _ =>
            {
                var responseContentObject = new JObject();
                /*
                var responseThing = new
                {
                    this.Request.Headers,
                    this.Request.Query,
                    this.Request.Form,
                    this.Request.Session,
                    this.Request.Method,
                    this.Request.Url,
                    this.Request.Path
                };
                */
                // return Response.AsJson(responseThing);
                responseContentObject.Add("test", this.Request.UserHostAddress);
                var response = (Response)responseContentObject.ToString();
                response.ContentType = "application/json";
                return response;
            };

            // получение объектов для синхронизации
            Get["/get_new/{date}/{nid?}"] = x =>
            {
                var responseContentArray = new JArray();

                Logger.Instance.WriteToLog(string.Format("запрос синхронизации: id={0} url={1}", x.nid, this.Request.UserHostAddress));

                // проверить разрешено ли этому узлу получать данные
                if (DataBase.RowSelectCount(string.Format("SELECT * FROM syn_nodes WHERE synout AND ipv4 = '{0}' and id = {1}", this.Request.UserHostAddress, x.nid)) > 0)
                {
                    Logger.Instance.WriteToLog("Соединение разрешено!");
                    foreach (var el in Synchronizer.SYNC_NEW)
                    {
                        var responseContentObject = new JObject();
                        string name = el.getName();
                        responseContentObject.Add("objectname", name);
                        // точка актуальности
                        DateTime dt = Utils.StringToDateTime(x.date.ToString());

                        var jArray = new JArray();
                        foreach (KeyValuePair<string, object>[] pairs in el.getReversed(dt))
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
                        foreach (var pair in el.getDeleted(dt))
                        {
                            var jObject = new JObject();
                            jObject.Add(pair.Key, pair.Value.ToString());
                            jArray.Add(jObject);
                        }
                        responseContentObject.Add("deleted", jArray);
                        responseContentArray.Add(responseContentObject);
                    }
                }
                else
                {
                    Logger.Instance.WriteToLog("Соединение запрещено!");
                }
                var response = (Response)responseContentArray.ToString();
                response.ContentType = "application/json";
                return response;
            };
        }
    }

    public static class WebUtils
    {
        public static void ENableCors(this NancyModule module)
        {
            module.After.AddItemToEndOfPipeline(x =>
            {
                x.Response.WithHeader("Access-Control-Allow-Origin", "*");
            });
        }
    }
}
