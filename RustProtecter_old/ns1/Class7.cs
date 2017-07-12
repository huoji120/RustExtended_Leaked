using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ns1
{
	internal class Class7
	{
		private static readonly string string_0;

		private static readonly string string_1;

		private static readonly byte[] byte_0;

		private static readonly System.Collections.Generic.Dictionary<int, string> dictionary_0;

		private static readonly bool bool_0;

		private static readonly int int_0;

		public static string smethod_0(int int_1)
		{
			int_1 -= Class7.int_0;
			string result;
			if (Class7.bool_0)
			{
				string text;
				Class7.dictionary_0.TryGetValue(int_1, out text);
				if (text != null)
				{
					result = text;
					return result;
				}
			}
			int index = int_1;
			int num = (int)Class7.byte_0[index++];
			int num2;
			if ((num & 128) == 0)
			{
				num2 = num;
				if (num2 == 0)
				{
					result = string.Empty;
					return result;
				}
			}
			else if ((num & 64) == 0)
			{
				num2 = ((num & 63) << 8) + (int)Class7.byte_0[index++];
			}
			else
			{
				num2 = ((num & 31) << 24) + ((int)Class7.byte_0[index++] << 16) + ((int)Class7.byte_0[index++] << 8) + (int)Class7.byte_0[index++];
			}
			string text3;
			try
			{
				byte[] array = System.Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(Class7.byte_0, index, num2));
				string text2 = string.Intern(System.Text.Encoding.UTF8.GetString(array, 0, array.Length));
				if (Class7.bool_0)
				{
					try
					{
						Class7.dictionary_0.Add(int_1, text2);
					}
					catch
					{
					}
				}
				text3 = text2;
			}
			catch
			{
				text3 = null;
			}
			result = text3;
			return result;
		}

		static Class7()
		{
			Class7.string_0 = "0";
			Class7.string_1 = "160";
			Class7.byte_0 = null;
			Class7.bool_0 = false;
			Class7.int_0 = 0;
			if (Class7.string_0 == "1")
			{
				Class7.bool_0 = true;
				Class7.dictionary_0 = new System.Collections.Generic.Dictionary<int, string>();
			}
			Class7.int_0 = System.Convert.ToInt32(Class7.string_1);
			System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			using (System.IO.Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("{7e792eae-11c6-48ed-9f5d-c2f96dff9090}"))
			{
				int num = System.Convert.ToInt32(manifestResourceStream.Length);
				byte[] buffer = new byte[num];
				manifestResourceStream.Read(buffer, 0, num);
				Class7.byte_0 = Class19.smethod_24(buffer);
				manifestResourceStream.Close();
			}
		}
	}
}
