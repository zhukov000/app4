///
/// http://blog.bondigeek.com/2011/09/08/a-simple-c-thread-safe-logging-class/
///
using System;
using System.Collections.Generic;
using System.IO;

namespace App3.Class.Singleton
{
    public class Logger
    {
        private static Logger instance;
        private static Queue<Log> logQueue;
        private static string logDir = "";
        private static string logFile = "logfile.txt";
        private static int maxLogAge = 10000;
        private static int queueSize = 10;
        private static DateTime LastFlushed = DateTime.Now;

        private Logger() { }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                    logQueue = new Queue<Log>();
                }
                return instance;
            }
        }

        public void WriteToLog(string message)
        {
            lock (logQueue)
            {
                Log logEntry = new Log(message);
                logQueue.Enqueue(logEntry);

                if (logQueue.Count >= queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }
        }

        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - LastFlushed;
            if (logAge.TotalSeconds >= maxLogAge)
            {
                LastFlushed = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FlushLog()
        {
            lock (logQueue)
            {
                while (logQueue.Count > 0)
                {
                    Log entry = logQueue.Dequeue();

                    if (logDir != "" && !Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);

                    string logPath = logDir + entry.LogDate + logFile;

                    using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter log = new StreamWriter(fs))
                        {
                            log.WriteLine(string.Format("{0}\t{1}", entry.LogTime, entry.Message));
                        }
                    }
                }
            }
        }

        ~Logger()
        {
            FlushLog();
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
            LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }
}
