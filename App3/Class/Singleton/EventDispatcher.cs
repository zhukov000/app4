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
        private static IDictionary<long, IDictionary<MessageGroupId, Tuple<DateTime, int>>> dict;
        private EventDispatcher() { }

        public static EventDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventDispatcher();
                    dict = new Dictionary<long, IDictionary<MessageGroupId, Tuple<DateTime, int>>>();
                }
                return instance;
            }
        }

        public Tuple<DateTime, int> this[long objid, MessageGroupId mgrid, int Idx]
        {
            get
            {
                if (!dict.ContainsKey(objid))
                    dict[objid] = new Dictionary<MessageGroupId, Tuple<DateTime, int>>();
                if (!dict[objid].ContainsKey(mgrid))
                    dict[objid][mgrid] = new Tuple<DateTime, int>(DateTime.Today.Subtract(new TimeSpan(1,0,0,0)), -1);
                return dict[objid][mgrid];
            }
            set
            {
                if (!dict.ContainsKey(objid))
                    dict[objid] = new Dictionary<MessageGroupId, Tuple<DateTime, int>>();
                dict[objid][mgrid] = value;
            }
        }
    }
}
