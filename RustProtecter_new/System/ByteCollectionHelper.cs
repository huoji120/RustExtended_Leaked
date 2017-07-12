using System;
using System.Linq;

namespace System
{
	public static class ByteCollectionHelper
	{
		public static decimal ToDecimal(this byte[] bytes)
		{
			if (bytes.Count<byte>() != 16)
			{
				throw new Exception("A decimal must be created from exactly 16 bytes");
			}
			int[] array = new int[4];
			for (int i = 0; i <= 15; i += 4)
			{
				array[i / 4] = BitConverter.ToInt32(bytes, i);
			}
			return new decimal(array);
		}
	}
}
