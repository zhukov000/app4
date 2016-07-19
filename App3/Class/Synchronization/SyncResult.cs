using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Synchronization
{
    class SyncResult
    {
        public string IpAddress { get; set; }
        public int Reserved { get; set; }
        public int Deleted { get; set; }
        public int Bytes { get; set; }
        public SyncStatus Status = SyncStatus.UNKNOWN;

        public SyncResult()
        {
            Reserved = 0;
            Deleted = 0;
            Bytes = 0;
            IpAddress = "";
        }
    }

    public class SyncStatus
    {
        private SyncStatus(string value) { Value = value; }

        public string Value { get; set; }

        public static SyncStatus UNKNOWN { get { return new SyncStatus("UNKNOWN"); } }
        public static SyncStatus OK { get { return new SyncStatus("OK"); } }
        public static SyncStatus EMPTY { get { return new SyncStatus("EMPTY"); } }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string (SyncStatus t)
        {
            return t.ToString();
        }
    }
}
