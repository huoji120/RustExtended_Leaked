using System;

namespace NetLink
{
	public static class EnumHelper
	{
		public static bool Has<T>(this Enum flags, T value) where T : struct
		{
			ulong num = Convert.ToUInt64(flags);
			ulong num2 = Convert.ToUInt64(value);
			return (num & num2) == num2;
		}

		public static T SetFlag<T>(this Enum flags, T value, bool state = true)
		{
			if (!Enum.IsDefined(typeof(T), value))
			{
				throw new ArgumentException("Enum value and flags types don't match.");
			}
			if (state)
			{
				return (T)((object)Enum.ToObject(typeof(T), Convert.ToUInt64(flags) | Convert.ToUInt64(value)));
			}
			return (T)((object)Enum.ToObject(typeof(T), Convert.ToUInt64(flags) & ~Convert.ToUInt64(value)));
		}
	}
}
