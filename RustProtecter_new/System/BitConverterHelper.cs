using System;
using System.Collections.Generic;

namespace System
{
	public static class BitConverterHelper
	{
		public static byte[] GetBytes(this decimal value)
		{
			List<byte> list = new List<byte>();
			int[] bits = decimal.GetBits(value);
			for (int i = 0; i < bits.Length; i++)
			{
				int value2 = bits[i];
				list.AddRange(BitConverter.GetBytes(value2));
			}
			return list.ToArray();
		}
	}
}
