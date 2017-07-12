namespace RustProtect
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class EnumHelper
    {
        public static bool Has<T>(this Enum flags, T value) where T: struct
        {
            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(flags) & num) == num);
        }

        public static T SetFlag<T>(this Enum flags, T value, [Optional, DefaultParameterValue(true)] bool state)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentException(Class3.smethod_10(0x13c));
            }
            if (state)
            {
                return (T) Enum.ToObject(typeof(T), (ulong) (Convert.ToUInt64(flags) | Convert.ToUInt64(value)));
            }
            return (T) Enum.ToObject(typeof(T), (ulong) (Convert.ToUInt64(flags) & ~Convert.ToUInt64(value)));
        }
    }
}

