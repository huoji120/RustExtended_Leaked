using System;

namespace RustExtended
{
	[Serializable]
	public class Countdown
	{
		public DateTime Stamp
		{
			get;
			private set;
		}

		public string Command
		{
			get;
			private set;
		}

		public bool Expires
		{
			get;
			private set;
		}

		public bool Expired
		{
			get
			{
				return this.Expires && DateTime.Now > this.Stamp;
			}
		}

		public double TimeLeft
		{
			get
			{
				double result;
				if (!this.Expires)
				{
					result = -1.0;
				}
				else
				{
					result = (this.Stamp - DateTime.Now).TotalSeconds;
				}
				return result;
			}
		}

		public Countdown(string command, double time = 0.0)
		{
			this.Command = command;
			this.Expires = (time > 0.0);
			this.Stamp = (this.Expires ? DateTime.Now.AddSeconds(time) : default(DateTime));
		}

		public Countdown(string command, DateTime stamp = default(DateTime))
		{
			this.Command = command;
			this.Expires = (stamp.Ticks > 0L);
			this.Stamp = stamp;
		}
	}
}
