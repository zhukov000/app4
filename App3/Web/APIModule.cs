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
            Get["/areas"] = x =>
            {
                var responseContentObject = new JObject();
                responseContentObject.Add("areas", generateAreas());
                responseContentObject.Add("colors", generateColorsArray());
                var response = (Response)responseContentObject.ToString();
                response.ContentType = "application/json";
                return response;
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

                // проверить разрешено ли этому узлу получать данные
                if (DataBase.RowSelectCount(string.Format("SELECT * FROM syn_nodes WHERE synout AND ipv4 = '{0}' and id = {1}", this.Request.UserHostAddress, x.nid)) > 0)
                {

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
                var response = (Response)responseContentArray.ToString();
                response.ContentType = "application/json";
                return response;
            };
        }
    }
}
