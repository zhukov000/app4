using System;
using System.Collections.Generic;
using System.IO;

namespace App3.Class.Singleton
{
    public class Logger
    {
        private static Logger instance;

        private static Queue<Log> logQueue;

        private static string logDir = Utils.AppDirectory() + "\\logs\\";

        private static string logFile = "install.log";

        private static int maxLogAge = 6000;

        private static int queueSize = 100;

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

        public static string LogDirectory()
        {
            return Logger.logDir;
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
                    if (!Directory.Exists(Logger.logDir))
                    {
                        Directory.CreateDirectory(Logger.logDir);
                    }
                    using (FileStream fileStream = File.Open(Logger.FileSave = Logger.logDir + log.LogDate + Logger.logFile, FileMode.Append, FileAccess.Write))
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


    public class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }

        public Log(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyyMMdd");
            LogTime = DateTime.Now.ToString("HH:mm:ss.fff tt");
        }
    }
}

