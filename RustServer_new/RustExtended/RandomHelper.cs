using System;

namespace RustExtended
{
	public static class RandomHelper
	{
		private static Random random_0 = new Random();

		public static long NextLong(this Random random, long min, long max)
		{
			if (max <= min)
			{
				throw new ArgumentOutOfRangeException("max", "Maximum should be be more than minimum.");
			}
			ulong num = (ulong)(max - min);
			ulong num2;
			do
			{
				byte[] array = new byte[8];
				random.NextBytes(array);
				num2 = (ulong)BitConverter.ToInt64(array, 0);
			}
			while (num2 > 18446744073709551615uL - (18446744073709551615uL % num + 1uL) % num);
			return (long)(num2 % num + (ulong)min);
		}

		public static int Random(this int value)
		{
			return value.Random(-2147483648);
		}

		public static int Random(this int value, int min)
		{
			return value.Random(min, value);
		}

		public static int Random(this int value, int min, int max)
		{
			return RandomHelper.random_0.Next(min, max);
		}

		public static uint Random(this uint value)
		{
			return value.Random(0u);
		}

		public static uint Random(this uint value, uint min)
		{
			return value.Random(min, value);
		}

		public static uint Random(this uint value, uint min, uint max)
		{
			return (uint)RandomHelper.random_0.Next((int)min, (int)max);
		}

		public static long Random(this long value)
		{
			return value.Random(-2147483648L);
		}

		public static long Random(this long value, long min)
		{
			return value.Random(min, value);
		}

		public static long Random(this long value, long min, long max)
		{
			return RandomHelper.random_0.NextLong(min, max);
		}

		public static ulong Random(this ulong value)
		{
			return value.Random(0uL);
		}

		public static ulong Random(this ulong value, ulong min)
		{
			return value.Random(min, value);
		}

		public static ulong Random(this ulong value, ulong min, ulong max)
		{
			return (ulong)RandomHelper.random_0.NextLong((long)min, (long)max);
		}

		public static double Random(this double value)
		{
			return value.Random(-1.7976931348623157E+308);
		}

		public static double Random(this double value, double min)
		{
			return value.Random(min, value);
		}

		public static double Random(this double value, double min, double max)
		{
			return RandomHelper.random_0.NextDouble() * (max - min) + min;
		}
	}
}
