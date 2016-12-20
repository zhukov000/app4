using System;
using System.Collections.Generic;

namespace App3.Class.Singleton
{
    internal class EventDispatcher
    {
        private static EventDispatcher instance;

        private static IDictionary<long, IDictionary<Utils.MessageGroupId, Tuple<DateTime, int>>> dict;

        public static EventDispatcher Instance
        {
            get
            {
                if (EventDispatcher.instance == null)
                {
                    EventDispatcher.instance = new EventDispatcher();
                    EventDispatcher.dict = new Dictionary<long, IDictionary<Utils.MessageGroupId, Tuple<DateTime, int>>>();
                }
                return EventDispatcher.instance;
            }
        }

        public Tuple<DateTime, int> this[long objid, Utils.MessageGroupId mgrid, int Idx]
        {
            get
            {
                if (!EventDispatcher.dict.ContainsKey(objid))
                {
                    EventDispatcher.dict[objid] = new Dictionary<Utils.MessageGroupId, Tuple<DateTime, int>>();
                }
                if (!EventDispatcher.dict[objid].ContainsKey(mgrid))
                {
                    EventDispatcher.dict[objid][mgrid] = new Tuple<DateTime, int>(DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)), -1);
                }
                return EventDispatcher.dict[objid][mgrid];
            }
            set
            {
                if (!EventDispatcher.dict.ContainsKey(objid))
                {
                    EventDispatcher.dict[objid] = new Dictionary<Utils.MessageGroupId, Tuple<DateTime, int>>();
                }
                EventDispatcher.dict[objid][mgrid] = value;
            }
        }

        private EventDispatcher()
        {
        }
    }
}
