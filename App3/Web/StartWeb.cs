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
                    StartOptions options = new StartOptions("http://+:"+ Config.Get("WebPort"))
                    {
                        ServerFactory = "Microsoft.Owin.Host.HttpListener"
                    } ;

                    string url = Config.Get("ModuleLocalServerIP");

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
                        Logger.Instance.WriteToLog(Utils.GetaAllMessages(ex));
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
