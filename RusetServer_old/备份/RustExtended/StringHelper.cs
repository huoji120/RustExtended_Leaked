namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class StringHelper
    {
        public static string Args(this string str, object arg0)
        {
            return string.Format(str, arg0);
        }

        public static string Args(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string Args(this string str, object arg0, object arg1)
        {
            return string.Format(str, arg0, arg1);
        }

        public static string Args(this string str, object arg0, object arg1, object arg2)
        {
            return string.Format(str, arg0, arg1, arg2);
        }

        public static string Capitalize(this string input, StringHelper.CapitalizeCase case_method = StringHelper.CapitalizeCase.First)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            input = input.ToLower();
            if (case_method == CapitalizeCase.All)
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
            }
            return (input.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + input.Substring(1, input.Length - 1));
        }

        public static List<int> Int32List(this string[] value)
        {
            List<int> list = new List<int>();
            foreach (string str in value)
            {
                int num;
                if (int.TryParse(str, out num) && !list.Contains(num))
                {
                    list.Add(num);
                }
            }
            return list;
        }

        public static List<long> Int64List(this string[] value)
        {
            List<long> list = new List<long>();
            foreach (string str in value)
            {
                long num;
                if (long.TryParse(str, out num) && !list.Contains(num))
                {
                    list.Add(num);
                }
            }
            return list;
        }

        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool ToBool(this string input)
        {
            input = input.Trim().ToUpper();
            if (((!(input == "ENABLED") && !(input == "ENABLE")) && (!(input == "TRUE") && !(input == "YES"))) && (!(input == "ON") && !(input == "Y")))
            {
                return (input == "1");
            }
            return true;
        }

        public static T ToEnum<T>(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                input = "0";
            }
            return (T) Enum.Parse(typeof(T), input, true);
        }

        public static int ToInt32(this string value)
        {
            try
            {
                if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(2);
                }
                try
                {
                    return int.Parse(value, NumberStyles.HexNumber);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }

        public static long ToInt64(this string value)
        {
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            try
            {
                return long.Parse(value, NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                return 0L;
            }
        }

        public static uint ToUInt32(this string value)
        {
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            try
            {
                return uint.Parse(value, NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static ulong ToUInt64(this string value)
        {
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            try
            {
                return ulong.Parse(value, NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                return 0L;
            }
        }

        public static List<uint> UInt32List(this string[] value)
        {
            List<uint> list = new List<uint>();
            foreach (string str in value)
            {
                uint num;
                if (uint.TryParse(str, out num) && !list.Contains(num))
                {
                    list.Add(num);
                }
            }
            return list;
        }

        public static List<ulong> UInt64List(this string[] value)
        {
            List<ulong> list = new List<ulong>();
            foreach (string str in value)
            {
                ulong num;
                if (ulong.TryParse(str, out num) && !list.Contains(num))
                {
                    list.Add(num);
                }
            }
            return list;
        }

        public enum CapitalizeCase
        {
            First,
            All
        }
    }
}

