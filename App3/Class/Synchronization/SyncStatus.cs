using System;

namespace App3.Class.Synchronization
{
	public class SyncStatus
	{
		public string Value
		{
			get;
			set;
		}

		public static SyncStatus UNKNOWN
		{
			get
			{
				return new SyncStatus("UNKNOWN");
			}
		}

		public static SyncStatus OK
		{
			get
			{
				return new SyncStatus("OK");
			}
		}

		public static SyncStatus EMPTY
		{
			get
			{
				return new SyncStatus("EMPTY");
			}
		}

		private SyncStatus(string value)
		{
			this.Value = value;
		}

		public override string ToString()
		{
			return this.Value;
		}

		public static implicit operator string(SyncStatus t)
		{
			return t.ToString();
		}
	}
}
