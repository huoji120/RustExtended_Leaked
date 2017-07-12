namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Timers;

    public class MOTDEvent : IDisposable
    {
        public List<string> Announce;
        public bool Enabled;
        public List<string> Messages;
        private Timer timer_0;
        public string Title;

        public MOTDEvent(string title, [Optional, DefaultParameterValue(0xe10)] int interval)
        {
            ElapsedEventHandler handler = null;
            ElapsedEventHandler handler2 = null;
            this.Title = title;
            this.Messages = new List<string>();
            this.Announce = new List<string>();
            this.timer_0 = new Timer();
            handler = new ElapsedEventHandler(this.method_0);
            this.timer_0.Elapsed += handler;
            handler2 = new ElapsedEventHandler(this.method_1);
            this.timer_0.Elapsed += handler2;
            this.timer_0.AutoReset = true;
            this.Interval = interval;
        }

        public void Dispose()
        {
            this.timer_0.Stop();
            this.timer_0.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void DoAnnounce()
        {
            if ((this.Announce.Count != 0) && this.Enabled)
            {
                foreach (string str in this.Announce)
                {
                    Broadcast.NoticeAll("☢", Helper.ReplaceVariables(null, str, null, ""), null, 5f);
                }
            }
        }

        protected void DoMessages()
        {
            if ((this.Messages.Count != 0) && this.Enabled)
            {
                foreach (string str in this.Messages)
                {
                    Broadcast.MessageAll(Helper.ReplaceVariables(null, str, null, ""));
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

        public void Start()
        {
            this.timer_0.Enabled = this.Enabled && (this.timer_0.Interval >= 1.0);
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

        public int Interval
        {
            get
            {
                return (((int) this.timer_0.Interval) / 0x3e8);
            }
            set
            {
                this.timer_0.Interval = value * 0x3e8;
            }
        }
    }
}

