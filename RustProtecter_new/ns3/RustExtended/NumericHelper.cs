namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class NumericHelper
    {
        public static string ToHEX(this int value, [Optional, DefaultParameterValue(true)] bool AsString)
        {
            return ((AsString ? "0x" : "") + string.Format("{0:X8}", value));
        }

        public static string ToHEX(this long value, [Optional, DefaultParameterValue(true)] bool AsString)
        {
            return ((AsString ? "0x" : "") + string.Format("{0:X16}", value));
        }

        public static string ToHEX(this uint value, [Optional, DefaultParameterValue(true)] bool AsString)
        {
            return ((AsString ? "0x" : "") + string.Format("{0:X8}", value));
        }

        public static string ToHEX(this ulong value, [Optional, DefaultParameterValue(true)] bool AsString)
        {
            return ((AsString ? "0x" : "") + string.Format("{0:X16}", value));
        }
    }
}

