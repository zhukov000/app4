using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Socket
{
    [Serializable]
    class SendObject
    {
        private IDictionary<string, object> data;

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public int RetrNumber
        {
            get { return data["retrnumber"].ToInt(); }
            set { data["retrnumber"] = value; }
        }

        public int ObjectNum
        {
            get { return data["objectnumber"].ToInt(); }
        }

        public SendObject()
        {
            data = null;
        }

        public SendObject(string pMessage)
        {
            Message = pMessage;
        }

        public SendObject(IDictionary<string, object> pData)
        {
            data = pData;
        }

        public IDictionary<string, object> GetInfo()
        {
            return data;
        }
        
    }
}
