using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Synchronization
{
    class SyncIPAddresses: Entity
    {
        
        override public string getName()
        {
            return "ipaddresses";
        }

        override public string getTableName()
        {
            return "oko.ipaddresses";
        }
    }
}
