using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;

namespace RustExtended
{
	public class MOTDEvent : IDisposable
	{
		public bool Enabled;

		public string Title;

		public List<string> Messages;

		public List<string> Announce;

		private Timer timer_0;

		public int Interval
		{
			get
			{
				return (int)this.timer_0.Interval / 1000;
			}
			set
			{
				this.timer_0.Interval = (double)(value * 1000);
			}
		}

		public MOTDEvent(string title, int interval = 3600)
		{
			this.Title = title;
			this.Messages = new List<string>();
			this.Announce = new List<string>();
			this.timer_0 = new Timer();
			Timer timer = this.timer_0;
			ElapsedEventHandler value = new ElapsedEventHandler(this.method_0);
			timer.Elapsed += value;
			Timer timer2 = this.timer_0;
			ElapsedEventHandler value2 = new ElapsedEventHandler(this.method_1);
			timer2.Elapsed += value2;
			this.timer_0.AutoReset = true;
			this.Interval = interval;
		}

		public void Start()
		{
			this.timer_0.Enabled = (this.Enabled && this.timer_0.Interval >= 1.0);
			if (!this.timer_0.Enabled)
			{
				this.DoMessages();
				this.DoAnnounce();
			}
		}

		public void Stop()
		{
			this.timer_0.Enabled = false;
		}

		public void Dispose()
		{
			this.timer_0.Stop();
			this.timer_0.Dispose();
			GC.SuppressFinalize(this);
		}

		protected void DoMessages()
		{
			if (this.Messages.Count != 0 && this.Enabled)
			{
				foreach (string current in this.Messages)
				{
					Broadcast.MessageAll(Helper.ReplaceVariables(null, current, null, ""));
				}
			}
		}

		protected void DoAnnounce()
		{
			if (this.Announce.Count != 0 && this.Enabled)
			{
				foreach (string current in this.Announce)
				{
					Broadcast.NoticeAll("☢", Helper.ReplaceVariables(null, current, null, ""), null, 5f);
				}
			}
		}

		[CompilerGenerated]
		private void method_0(object sender, ElapsedEventArgs e)
		{
			this.DoMessages();
		}

		[CompilerGenerated]
		private void method_1(object sender, ElapsedEventArgs e)
		{
			this.DoAnnounce();
		}
	}
}
