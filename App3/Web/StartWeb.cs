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
                    var url = "http://localhost:" + Config.Get("WebPort");

                    try
                    {
                        using (WebApp.Start<Startup>(url))
                        {
                            while (true)
                            {
                                // Do Nothing
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(String.Format("{0}.{1}: Ошибка при запуске web-сервиса на url = {2}: {3}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, url, ex.Message));
                    }
                }
            ));
            backgroundThread.Start();
        }

        static public void Stop()
        {
            if (backgroundThread != null)
                backgroundThread.Abort();
        }
    }
}
