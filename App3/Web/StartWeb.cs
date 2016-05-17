using Nancy;
using Owin;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using App3.Class;
using App3.Class.Singleton;

namespace App3.Web
{
    static class StartWeb
    {
        static Thread backgroundThread = null;

        static public void Start()
        {
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    // var url = "http://localhost:" + Config.Get("WebPort");

                    StartOptions options = new StartOptions();
                    options.Urls.Add("http://localhost:" + Config.Get("WebPort"));
                    options.Urls.Add("http://127.0.0.1:" + Config.Get("WebPort"));

                    string url = Config.Get("ModuleLocalServerIP");

                    if (!url.Equals("127.0.0.1") && !url.Equals("localhost"))
                    {
                        options.Urls.Add("http://" + url + ":" + Config.Get("WebPort"));
                    }

                    try
                    {
                        using (WebApp.Start<Startup>(options))
                        {
                            Logger.Instance.WriteToLog(String.Format("Web-service start at local and {0} on port {1} ", url, Config.Get("WebPort")));
                            while (true)
                            {
                                // wait...
                            }
                        }
                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                        // Do Nothing
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(String.Format("{0}.{1}: Ошибка при запуске web-сервиса: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    }
                }
            ));
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        static public void Stop()
        {
            if (backgroundThread != null)
                backgroundThread.Abort();
        }
    }
}
