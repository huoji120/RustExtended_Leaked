using System;

namespace RustExtended
{
	public static class NumericHelper
	{
		public static string ToHEX(this int value, bool AsString = true)
		{
			return (AsString ? "0x" : "") + string.Format("{0:X8}", value);
		}

		public static string ToHEX(this uint value, bool AsString = true)
		{
			return (AsString ? "0x" : "") + string.Format("{0:X8}", value);
		}

		public static string ToHEX(this long value, bool AsString = true)
		{
			return (AsString ? "0x" : "") + string.Format("{0:X16}", value);
		}

		public static string ToHEX(this ulong value, bool AsString = true)
		{
			return (AsString ? "0x" : "") + string.Format("{0:X16}", value);
		}
	}
}
