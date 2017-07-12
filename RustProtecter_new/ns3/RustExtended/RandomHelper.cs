namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;

    public static class RandomHelper
    {
        private static System.Random random_0 = new System.Random();

        public static long NextLong(this System.Random random, long min, long max)
        {
            ulong num2;
            if (max <= min)
            {
                throw new ArgumentOutOfRangeException("max", "Maximum should be be more than minimum.");
            }
            ulong num = (ulong) (max - min);
            do
            {
                byte[] buffer = new byte[8];
                random.NextBytes(buffer);
                num2 = (ulong) BitConverter.ToInt64(buffer, 0);
            }
            while (num2 > (ulong.MaxValue - (((ulong.MaxValue % num) + ((ulong) 1L)) % num)));
            return (((long) (num2 % num)) + min);
        }

        public static double Random(this double value)
        {
            return value.Random(double.MinValue);
        }

        public static int Random(this int value)
        {
            return value.Random(-2147483648);
        }

        public static long Random(this long value)
        {
            return value.Random(-2147483648L);
        }

        public static uint Random(this uint value)
        {
            return value.Random(0);
        }

        public static ulong Random(this ulong value)
        {
            return value.Random(0L);
        }

        public static double Random(this double value, double min)
        {
            return value.Random(min, value);
        }

        public static int Random(this int value, int min)
        {
            return value.Random(min, value);
        }

        public static long Random(this long value, long min)
        {
            return value.Random(min, value);
        }

        public static uint Random(this uint value, uint min)
        {
            return value.Random(min, value);
        }

        public static ulong Random(this ulong value, ulong min)
        {
            return value.Random(min, value);
        }

        public static double Random(this double value, double min, double max)
        {
            return ((random_0.NextDouble() * (max - min)) + min);
        }

        public static int Random(this int value, int min, int max)
        {
            return random_0.Next(min, max);
        }

        public static long Random(this long value, long min, long max)
        {
            return random_0.NextLong(min, max);
        }

        public static uint Random(this uint value, uint min, uint max)
        {
            return (uint) random_0.Next((int) min, (int) max);
        }

        public static ulong Random(this ulong value, ulong min, ulong max)
        {
            return (ulong) random_0.NextLong(((long) min), ((long) max));
        }
    }
}

