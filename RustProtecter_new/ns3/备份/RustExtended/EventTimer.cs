namespace RustExtended
{
    using System;
    using System.Diagnostics;
    using System.Timers;
    using RustExtended;

    public class EventTimer : System.Timers.Timer
    {
        public string Command;
        public NetUser Sender;
        private Stopwatch stopwatch_0 = new Stopwatch();
        public NetUser Target;

        public new void Dispose()
        {
            RustExtended.Events.Timer.Remove(this);
            this.stopwatch_0.Stop();
            base.Dispose();
        }

        public new void Start()
        {
            RustExtended.Events.Timer.Add(this);
            this.stopwatch_0.Start();
            base.Start();
        }

        public double TimeLeft
        {
            get
            {
                this.stopwatch_0.Stop();
                long elapsedMilliseconds = this.stopwatch_0.ElapsedMilliseconds;
                this.stopwatch_0.Start();
                return Math.Round((double) ((base.Interval - elapsedMilliseconds) / 1000.0));
            }
        }
    }
}

