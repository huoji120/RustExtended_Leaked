using System;
using System.Collections.Generic;
using System.Text;

namespace RustExtended
{
	public static class ListHelper
	{
		public static string ToCommaString(this List<int> list)
		{
			string result;
			if (list.Count <= 0)
			{
				result = "";
			}
			else if (list.Count == 1)
			{
				result = list[0].ToString();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(list[0].ToString());
				for (int i = 1; i < list.Count; i++)
				{
					stringBuilder.Append("," + list[i].ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static string ToCommaString(this List<uint> list)
		{
			string result;
			if (list.Count <= 0)
			{
				result = "";
			}
			else if (list.Count == 1)
			{
				result = list[0].ToString();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(list[0].ToString());
				for (int i = 1; i < list.Count; i++)
				{
					stringBuilder.Append("," + list[i].ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static string ToCommaString(this List<long> list)
		{
			string result;
			if (list.Count <= 0)
			{
				result = "";
			}
			else if (list.Count == 1)
			{
				result = list[0].ToString();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(list[0].ToString());
				for (int i = 1; i < list.Count; i++)
				{
					stringBuilder.Append("," + list[i].ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static string ToCommaString(this List<ulong> list)
		{
			string result;
			if (list.Count <= 0)
			{
				result = "";
			}
			else if (list.Count == 1)
			{
				result = list[0].ToString();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(list[0].ToString());
				for (int i = 1; i < list.Count; i++)
				{
					stringBuilder.Append("," + list[i].ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
	}
}
