using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static App3.Class.Utils;

namespace App3.Class.Singleton
{
    class EventDispatcher
    {
        private static EventDispatcher instance;
        private static IDictionary<long, IDictionary<MessageGroupId, DateTime>> dict;
        private EventDispatcher() { }

        public static EventDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventDispatcher();
                    dict = new Dictionary<long, IDictionary<MessageGroupId, DateTime>>();
                }
                return instance;
            }
        }

        public DateTime this[long objid, MessageGroupId mgrid]
        {
            get
            {
                if (!dict.ContainsKey(objid))
                    dict[objid] = new Dictionary<MessageGroupId, DateTime>();
                if (!dict[objid].ContainsKey(mgrid))
                    dict[objid][mgrid] = DateTime.Today.Subtract(new TimeSpan(1,0,0,0));
                return dict[objid][mgrid];
            }
            set
            {
                if (!dict.ContainsKey(objid))
                    dict[objid] = new Dictionary<MessageGroupId, DateTime>();
                dict[objid][mgrid] = value;
            }
        }
    }
}
