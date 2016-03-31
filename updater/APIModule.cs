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
using System.Configuration;
using FtpLib;

namespace RESTService
{
    public class APIModule : NancyModule
    {
        public APIModule()
        {
            StaticConfiguration.DisableErrorTraces = false;
            Get["/version"] = x =>
            {
                string s = "1.0";
                string server = ConfigurationManager.AppSettings["Host"];

                using (FtpConnection ftp = new FtpConnection(server, "anonymous", ""))
                {
                    ftp.Open();
                    ftp.Login();

                    if (ftp.DirectoryExists("/Spolox"))
                    {
                        ftp.SetCurrentDirectory("/Spolox");
                        string[] versions = ftp.GetDirectories("/Spolox").Select(y => y.Name).ToArray();
                        Array.Sort(versions, Program.CompareVersions);

                        if (versions.Length > 0)
                            s = versions[versions.Length - 1];
                    }                    
                }
                
                var response = (Response)s;
                response.ContentType = "text/plain";
                return response;
            };
        }
    }
}
