using System;

namespace App3.Class.Singleton
{
    public class Log
    {
        public string Message
        {
            get;
            set;
        }

        public string LogTime
        {
            get;
            set;
        }

        public string LogDate
        {
            get;
            set;
        }

        public Log(string message)
        {
            this.Message = message;
            this.LogDate = DateTime.Now.ToString("yyyyMMdd");
            this.LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }
}
