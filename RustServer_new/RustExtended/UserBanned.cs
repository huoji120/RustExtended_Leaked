using System;

namespace RustExtended
{
	[Serializable]
	public class UserBanned
	{
		public string IP
		{
			get;
			private set;
		}

		public DateTime Time
		{
			get;
			private set;
		}

		public DateTime Period
		{
			get;
			private set;
		}

		public string Reason
		{
			get;
			private set;
		}

		public string Details
		{
			get;
			private set;
		}

		public bool Expired
		{
			get
			{
				return this.Period.Ticks > 0L && this.Period < DateTime.Now;
			}
		}

		public UserBanned(string ipAddr, DateTime time, DateTime period, string reason = "", string details = "")
		{
			this.IP = ipAddr;
			this.Time = time;
			this.Period = period;
			this.Reason = reason;
			this.Details = details;
		}
	}
}
