using App3.Class.Singleton;
using App3.Class.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace App3.Class.Static
{
    static class SyncronizerViaSocket
    {
        private static Synchronizer.Status_Sync status;

        public static Entity[] SYNC_NEW;

        private static Thread backgroundThread;

        public static bool NeedUpdate;

        public static event Action SyncStart;

        public static event Action SyncStop;

        public static Synchronizer.Status_Sync Status
        {
            get
            {
                return status;
            }
        }


        /// <summary>
        /// Старт фонового потока выполнения синхронизации
        /// </summary>
        public static void Start()
        {
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    while (true)
                    {
                        List<SyncResult> list = null;
                        try
                        {
                            Run(ref list);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                        }
                        Thread.Sleep(Config.Get("SynchSleepMinutes").ToInt() * 60 * 1000);
                    }
                }
            ));
            backgroundThread.Start();
        }

        public static List<string> Run(ref List<SyncResult> Data)
        {
            throw new Exception("Socket Synchronization TODO");
        }

    }
}
