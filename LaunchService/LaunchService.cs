using System;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using App3.Class.Singleton;

namespace LaunchService
{
    public partial class SpoloxLauncher : ServiceBase
    {
        // private static System.Diagnostics.Process proc;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        // private System.Diagnostics.EventLog eventLog1;
        private System.Timers.Timer timer1;

        public SpoloxLauncher(string[] args)
        {
            InitializeComponent();
            // eventLog1 = new System.Diagnostics.EventLog();
            if ( !System.Diagnostics.EventLog.SourceExists("LauncherSource") )
            {
                System.Diagnostics.EventLog.CreateEventSource("LauncherSource", "LauncherLog");
            }
            // eventLog1.Source = "LauncherSource";
            // eventLog1.Log = "LauncherLog";
            timer1 = new System.Timers.Timer();
            timer1.Interval = Convert.ToInt32(App3.Class.Config.Get("IntervalSec")) * 1000;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            /*ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCheckPoint = 1;
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            */
            // eventLog1.WriteEntry("Service start at " + DateTime.Now.ToString());
            Logger.Instance.WriteToLog("Service start at " + DateTime.Now.ToString());
            timer1.Start();
            Logger.Instance.WriteToLog("Timer Start");
            base.OnStart(args);

            /* Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            serviceStatus.dwWaitHint = 0;
            serviceStatus.dwCheckPoint = 0;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);*/
        }

        protected override void OnStop()
        {
            // eventLog1.WriteEntry("Service stop at " + DateTime.Now.ToString());
            Logger.Instance.WriteToLog("Service stop at " + DateTime.Now.ToString());
            timer1.Stop();
            base.OnStop();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // eventLog1.WriteEntry("Test application");
            Logger.Instance.WriteToLog("Test application");
            using (var mutex = new System.Threading.Mutex(false, App3.Class.Config.Get("MutexName")))
            {
                try
                {
                    if (mutex.WaitOne(TimeSpan.FromSeconds(5)))
                    {
                        Logger.Instance.WriteToLog("Run: " + App3.Class.Config.Get("LauncherPath"));
                        ProcessAsCurrentUser.CreateProcessAsCurrentUser(App3.Class.Config.Get("LauncherPath"));
                        // proc = 
                        System.Diagnostics.Process.Start(App3.Class.Config.Get("LauncherPath"));
                    }
                }
                catch (Exception ex)
                {
                    // eventLog1.WriteEntry(ex.Message);
                    Logger.Instance.WriteToLog(ex.Message + " Path: " + App3.Class.Config.Get("LauncherPath"));
                }
            }
        }
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    };
}
