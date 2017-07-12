namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Countdown
    {
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

        public string Command { get; private set; }

        public bool Expired
        {
            get
            {
                return (this.Expires && (DateTime.Now > this.Stamp));
            }
        }

        public bool Expires { get; private set; }

        public DateTime Stamp { get; private set; }

        public double TimeLeft
        {
            get
            {
                if (!this.Expires)
                {
                    return -1.0;
                }
                TimeSpan span = (TimeSpan) (this.Stamp - DateTime.Now);
                return span.TotalSeconds;
            }
        }
    }
}

