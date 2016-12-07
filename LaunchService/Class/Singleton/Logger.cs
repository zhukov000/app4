using System;
using System.Collections.Generic;
using System.IO;

namespace App3.Class.Singleton
{
    public class Logger
    {
        private static Logger instance;

        private static Queue<Log> logQueue;

        private static string logFile = Config.Get("LogFile");

        private static int maxLogAge = int.Parse(Config.Get("MaxLogAge"));

        private static int queueSize = int.Parse(Config.Get("MaxLogQueue"));

        private static DateTime LastFlushed = DateTime.Now;

        private static string FileSave = "";

        public static Logger Instance
        {
            get
            {
                if (Logger.instance == null)
                {
                    Logger.instance = new Logger();
                    Logger.logQueue = new Queue<Log>();
                }
                return Logger.instance;
            }
        }

        private Logger()
        {
        }

        public string LogFileName()
        {
            return Logger.FileSave;
        }

        public void WriteToLog(string message)
        {
            Queue<Log> obj = Logger.logQueue;
            lock (obj)
            {
                Log item = new Log(message);
                Logger.logQueue.Enqueue(item);
                if (Logger.logQueue.Count >= Logger.queueSize || this.DoPeriodicFlush())
                {
                    this.FlushLog();
                }
            }
        }

        private bool DoPeriodicFlush()
        {
            if ((DateTime.Now - Logger.LastFlushed).TotalSeconds >= (double)Logger.maxLogAge)
            {
                Logger.LastFlushed = DateTime.Now;
                return true;
            }
            return false;
        }

        public void FlushLog()
        {
            Queue<Log> obj = Logger.logQueue;
            lock (obj)
            {
                while (Logger.logQueue.Count > 0)
                {
                    Log log = Logger.logQueue.Dequeue();
                    using (FileStream fileStream = File.Open(Logger.FileSave = Logger.logFile + log.LogDate + ".log", FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.WriteLine(string.Format("{0}\t{1}", log.LogTime, log.Message));
                        }
                    }
                }
            }
        }

        ~Logger()
        {
            this.FlushLog();
        }
    }
}
