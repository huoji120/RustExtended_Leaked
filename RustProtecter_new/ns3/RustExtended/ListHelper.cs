namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class ListHelper
    {
        public static string ToCommaString(this List<int> list)
        {
            if (list.Count <= 0)
            {
                return "";
            }
            if (list.Count == 1)
            {
                int num2 = list[0];
                return num2.ToString();
            }
            int num3 = list[0];
            StringBuilder builder = new StringBuilder(num3.ToString());
            for (int i = 1; i < list.Count; i++)
            {
                builder.Append("," + list[i].ToString());
            }
            return builder.ToString();
        }

        public static string ToCommaString(this List<long> list)
        {
            if (list.Count <= 0)
            {
                return "";
            }
            if (list.Count == 1)
            {
                long num2 = list[0];
                return num2.ToString();
            }
            long num3 = list[0];
            StringBuilder builder = new StringBuilder(num3.ToString());
            for (int i = 1; i < list.Count; i++)
            {
                builder.Append("," + list[i].ToString());
            }
            return builder.ToString();
        }

        public static string ToCommaString(this List<uint> list)
        {
            if (list.Count <= 0)
            {
                return "";
            }
            if (list.Count == 1)
            {
                uint num2 = list[0];
                return num2.ToString();
            }
            uint num3 = list[0];
            StringBuilder builder = new StringBuilder(num3.ToString());
            for (int i = 1; i < list.Count; i++)
            {
                builder.Append("," + list[i].ToString());
            }
            return builder.ToString();
        }

        public static string ToCommaString(this List<ulong> list)
        {
            if (list.Count <= 0)
            {
                return "";
            }
            if (list.Count == 1)
            {
                ulong num2 = list[0];
                return num2.ToString();
            }
            ulong num3 = list[0];
            StringBuilder builder = new StringBuilder(num3.ToString());
            for (int i = 1; i < list.Count; i++)
            {
                builder.Append("," + list[i].ToString());
            }
            return builder.ToString();
        }
    }
}

