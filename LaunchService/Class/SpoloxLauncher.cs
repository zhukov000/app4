using App3.Class;
using App3.Class.Singleton;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace LaunchService
{
    public class SpoloxLauncher : ServiceBase
    {
        private System.Timers.Timer timer1;

        private IContainer components;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public SpoloxLauncher(string[] args)
        {
            this.InitializeComponent();
            if (!EventLog.SourceExists("LauncherSource"))
            {
                EventLog.CreateEventSource("LauncherSource", "LauncherLog");
            }
            this.timer1 = new System.Timers.Timer();
            this.timer1.Interval = (double)(Convert.ToInt32(Config.Get("IntervalSec")) * 1000);
            this.timer1.Elapsed += new ElapsedEventHandler(this.OnTimer);
        }

        protected override void OnStart(string[] args)
        {
            Logger.Instance.WriteToLog("Service start at " + DateTime.Now.ToString());
            this.timer1.Start();
            Logger.Instance.WriteToLog("Timer Start");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Logger.Instance.WriteToLog("Service stop at " + DateTime.Now.ToString());
            this.timer1.Stop();
            base.OnStop();
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            Logger.Instance.WriteToLog("Test application");
            using (Mutex mutex = new Mutex(false, Config.Get("MutexName")))
            {
                try
                {
                    if (mutex.WaitOne(TimeSpan.FromSeconds(5.0)))
                    {
                        Logger.Instance.WriteToLog("Run: " + Config.Get("LauncherPath"));
                        ProcessAsCurrentUser.CreateProcessAsCurrentUser(Config.Get("LauncherPath"));
                        Process.Start(Config.Get("LauncherPath"));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(ex.Message + " Path: " + Config.Get("LauncherPath"));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.ServiceName = "SpoloxLauncher";
        }
    }
}
