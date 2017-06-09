using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Synchronization
{
    class SyncObject : Entity
    {
        public override string KeyField
        {
            get
            {
                return "osm_id";
            }
        }

        override public string getName()
        {
            return "object";
        }
    }
}
