using System;
using System.Collections.Generic;
using System.Globalization;

namespace RustExtended
{
	public static class StringHelper
	{
		public enum CapitalizeCase
		{
			First,
			All
		}

		public static bool IsEmpty(this string input)
		{
			return string.IsNullOrEmpty(input);
		}

		public static string Args(this string str, object arg0)
		{
			return string.Format(str, arg0);
		}

		public static string Args(this string str, object arg0, object arg1)
		{
			return string.Format(str, arg0, arg1);
		}

		public static string Args(this string str, object arg0, object arg1, object arg2)
		{
			return string.Format(str, arg0, arg1, arg2);
		}

		public static string Args(this string str, params object[] args)
		{
			return string.Format(str, args);
		}

		public static string Capitalize(this string input, StringHelper.CapitalizeCase case_method = StringHelper.CapitalizeCase.First)
		{
			string result;
			if (string.IsNullOrEmpty(input))
			{
				result = input;
			}
			else
			{
				input = input.ToLower();
				if (case_method == StringHelper.CapitalizeCase.All)
				{
					result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
				}
				else
				{
					result = input.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + input.Substring(1, input.Length - 1);
				}
			}
			return result;
		}

		public static int ToInt32(this string value)
		{
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				value = value.Substring(2);
			}
			int result;
			try
			{
				result = int.Parse(value, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				result = 0;
			}
			return result;
		}

		public static long ToInt64(this string value)
		{
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				value = value.Substring(2);
			}
			long result;
			try
			{
				result = long.Parse(value, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				result = 0L;
			}
			return result;
		}

		public static uint ToUInt32(this string value)
		{
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				value = value.Substring(2);
			}
			uint result;
			try
			{
				result = uint.Parse(value, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				result = 0u;
			}
			return result;
		}

		public static ulong ToUInt64(this string value)
		{
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				value = value.Substring(2);
			}
			ulong result;
			try
			{
				result = ulong.Parse(value, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				result = 0uL;
			}
			return result;
		}

		public static T ToEnum<T>(this string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				input = "0";
			}
			return (T)((object)Enum.Parse(typeof(T), input, true));
		}

		public static bool ToBool(this string input)
		{
			input = input.Trim().ToUpper();
			return input == "ENABLED" || input == "ENABLE" || input == "TRUE" || input == "YES" || input == "ON" || input == "Y" || input == "1";
		}

		public static List<int> Int32List(this string[] value)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < value.Length; i++)
			{
				string s = value[i];
				int item;
				if (int.TryParse(s, out item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<uint> UInt32List(this string[] value)
		{
			List<uint> list = new List<uint>();
			for (int i = 0; i < value.Length; i++)
			{
				string s = value[i];
				uint item;
				if (uint.TryParse(s, out item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<long> Int64List(this string[] value)
		{
			List<long> list = new List<long>();
			for (int i = 0; i < value.Length; i++)
			{
				string s = value[i];
				long item;
				if (long.TryParse(s, out item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<ulong> UInt64List(this string[] value)
		{
			List<ulong> list = new List<ulong>();
			for (int i = 0; i < value.Length; i++)
			{
				string s = value[i];
				ulong item;
				if (ulong.TryParse(s, out item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}
}
