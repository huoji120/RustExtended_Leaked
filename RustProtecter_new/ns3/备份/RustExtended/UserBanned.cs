namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class UserBanned
    {
        public UserBanned(string ipAddr, DateTime time, DateTime period, [Optional, DefaultParameterValue("")] string reason, [Optional, DefaultParameterValue("")] string details)
        {
            this.IP = ipAddr;
            this.Time = time;
            this.Period = period;
            this.Reason = reason;
            this.Details = details;
        }

        public string Details { get; private set; }

        public bool Expired
        {
            get
            {
                return ((this.Period.Ticks > 0L) && (this.Period < DateTime.Now));
            }
        }

        public string IP { get; private set; }

        public DateTime Period { get; private set; }

        public string Reason { get; private set; }

        public DateTime Time { get; private set; }
    }
}

