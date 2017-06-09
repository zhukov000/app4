using System;
using System.Collections.Generic;
using System.IO;

namespace App3.Class.Singleton
{
    public class Logger
    {
        public enum LogLevel
        {
            DEBUG = 1,
            ERROR = 2,
            EVENTS = 4,
            ALL = 7
        }

        private static Logger instance;

        private static Queue<Log> logQueue;

        private static string logDir = Utils.AppDirectory() + "\\logs\\";

        private static string logFile = Config.Get("LogFile");

        private static int maxLogAge = int.Parse(Config.Get("MaxLogAge"));

        private static int queueSize = int.Parse(Config.Get("MaxLogQueue"));

        private static int logLevel = Config.Get("LogLevel", ((int)LogLevel.ALL).ToString()).ToInt();

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

        public void WriteToLog(string message, LogLevel level /*= LogLevel.ALL*/)
        {
            if ((((int)level) & logLevel) > 0)
            {
                Queue<Log> obj = Logger.logQueue;
                lock (obj)
                {
                    Log item = new Log(message, level);
                    Logger.logQueue.Enqueue(item);
                    if (Logger.logQueue.Count >= Logger.queueSize || this.DoPeriodicFlush())
                    {
                        this.FlushLog();
                    }
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
                            streamWriter.WriteLine(string.Format("{2}:{0}\t{1}", log.LogTime, log.Message, log.LogLevel)); // .Crypt()
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
        public string LogLevel { get; set; }

        public Log(string message, Logger.LogLevel level)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyyMMdd");
            LogTime = DateTime.Now.ToString("HH:mm:ss.fff tt");
            LogLevel = level.ToString();
        }
    }
}

