using System;

namespace App3.Class.Synchronization
{
	public partial class SyncResult
	{
		public SyncStatus Status = SyncStatus.UNKNOWN;

        public DateTime Timestamp
        {
            get;
            set;
        }

		public string IpAddress
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int Reserved
		{
			get;
			set;
		}

		public int Deleted
		{
			get;
			set;
		}

		public int Bytes
		{
			get;
			set;
		}

		public SyncResult()
		{
			this.Reserved = 0;
			this.Deleted = 0;
			this.Bytes = 0;
			this.IpAddress = "";
		}
	}
}
