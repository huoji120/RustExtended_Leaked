using System;
using System.Runtime.InteropServices;

namespace RustProtect
{
	public static class EnumHelper
	{
		public static bool Has<T>(this Enum flags, T value) where T : struct
		{
			ulong arg_13_0 = Convert.ToUInt64(flags);
			ulong num = Convert.ToUInt64(value);
			return (arg_13_0 & num) == num;
		}

        public static T SetFlag<T>(this Enum flags, T value, [Optional, DefaultParameterValue(true)] bool state)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentException(Class3.smethod_10(0x13c));
            }
            if (state)
            {
                return (T)Enum.ToObject(typeof(T), (ulong)(Convert.ToUInt64(flags) | Convert.ToUInt64(value)));
            }
            return (T)Enum.ToObject(typeof(T), (ulong)(Convert.ToUInt64(flags) & ~Convert.ToUInt64(value)));
        }
    }
}
