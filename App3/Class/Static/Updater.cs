using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App3.Class.Static
{
    static class Updater
    {
        static Thread backgroundThread = null;
        public static bool NeedUpdate = false;

        static public void Start()
        {
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    while (true)
                    {
                        if (Utils.CompareVersions(Config.APPVERSION, LastVersion()) < 0)
                        {
                            NeedUpdate = true;
                        }
                        Thread.Sleep(600000);
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

        static string LastVersion()
        {
            // получить последнюю версию с сервера
            string version = Utils.getHtmlContent(Config.Get("UrlUpdate"));
            if (version == "") version = Config.APPVERSION;
            return version;
        }
    }
}
