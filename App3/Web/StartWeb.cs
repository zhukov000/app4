using Nancy;
using Owin;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using App3.Class;
using App3.Class.Singleton;
using System.Reflection;

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

                    StartOptions options = new StartOptions("http://+:" + Config.Get("WebPort"))
                    {
                        ServerFactory = "Microsoft.Owin.Host.HttpListener"
                    };
                    string arg = Config.Get("ModuleLocalServerIP");

                    try
                    {
                        using (WebApp.Start<Startup>(options))
                        {
                            Logger.Instance.WriteToLog(string.Format("Web-service start at local and {0} on port {1} ", arg, Config.Get("WebPort")));
                            while (true)
                            {
                                // Do Nothing
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: Ошибка при запуске web-сервиса: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
                        Logger.Instance.WriteToLog(ex.GetaAllMessages());
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
