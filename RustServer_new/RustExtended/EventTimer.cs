using System;
using System.Diagnostics;
using System.Timers;

namespace RustExtended
{
	public class EventTimer : Timer
	{
		public NetUser Sender;

		public NetUser Target;

		public string Command;

		private Stopwatch stopwatch_0 = new Stopwatch();

		public double TimeLeft
		{
			get
			{
				this.stopwatch_0.Stop();
				long elapsedMilliseconds = this.stopwatch_0.ElapsedMilliseconds;
				this.stopwatch_0.Start();
				return Math.Round((base.Interval - (double)elapsedMilliseconds) / 1000.0);
			}
		}

		public new void Start()
		{
			RustExtended.Events.Timer.Add(this);
			this.stopwatch_0.Start();
			base.Start();
		}

		public new void Dispose()
		{
			RustExtended.Events.Timer.Remove(this);
			this.stopwatch_0.Stop();
			base.Dispose();
		}
	}
}
