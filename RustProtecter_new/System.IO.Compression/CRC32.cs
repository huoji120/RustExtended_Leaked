using System;

namespace System.IO.Compression
{
	public class CRC32
	{
		private const int int_0 = 8192;

		private uint uint_0;

		private static long long_0;

		private static bool bool_0;

		private static uint[] uint_1;

		private static uint uint_2 = 4294967295u;

		public static long TotalBytesRead
		{
			get
			{
				return long_0;
			}
		}

		public static int Crc32Result
		{
			get
			{
				return (int)(~(int)uint_2);
			}
		}

		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			//long_0 = 0L;
			int i = input.Read(array, 0, 8192);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			long_0 += i;
			while (i > 0)
			{
                SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				long_0 += (long)i;
			}
			return (int)(~(int)uint_2);
		}

		public int ComputeCrc32(int W, byte B)
		{
			return this.method_0((uint)W, B);
		}

		internal int method_0(uint uint_3, byte byte_0)
		{
			return (int)(uint_1[(int)((UIntPtr)((uint_3 ^ (uint)byte_0) & 255u))] ^ uint_3 >> 8);
		}

		public static void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (bool_0)
				{
					uint num2 = uint_2 >> 24 ^ (uint)b;
					uint_2 = (uint_2 << 8 ^ uint_1[(int)((UIntPtr)num2)]);
				}
				else
				{
					uint num3 = (uint_2 & 255u) ^ (uint)b;
					uint_2 = (uint_2 >> 8 ^ uint_1[(int)((UIntPtr)num3)]);
				}
			}
			long_0 += (long)count;
		}

		public void UpdateCRC(byte b)
		{
			if (bool_0)
			{
				uint num = uint_2 >> 24 ^ (uint)b;
				uint_2 = (uint_2 << 8 ^ uint_1[(int)((UIntPtr)num)]);
				return;
			}
			uint num2 = (uint_2 & 255u) ^ (uint)b;
			uint_2 = (uint_2 >> 8 ^ uint_1[(int)((UIntPtr)num2)]);
		}

		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (bool_0)
				{
					uint num = uint_2 >> 24 ^ (uint)b;
					uint_2 = (uint_2 << 8 ^ uint_1[(int)((UIntPtr)num)]);
				}
				else
				{
					uint num2 = (uint_2 & 255u) ^ (uint)b;
					uint_2 = (uint_2 >> 8 ^ uint_1[(int)((UIntPtr)num2)]);
				}
			}
		}

		private static uint smethod_0(uint uint_3)
		{
			uint num = (uint_3 & 1431655765u) << 1 | (uint_3 >> 1 & 1431655765u);
			num = ((num & 858993459u) << 2 | (num >> 2 & 858993459u));
			num = ((num & 252645135u) << 4 | (num >> 4 & 252645135u));
			return num << 24 | (num & 65280u) << 8 | (num >> 8 & 65280u) | num >> 24;
		}

		private static byte smethod_1(byte byte_0)
		{
			uint num = (uint)byte_0 * 131586u;
			uint num2 = num & 17055760u;
			uint num3 = num << 2 & 17055760u << 1;
			return (byte)(16781313u * (num2 + num3) >> 24);
		}

		private void method_1()
		{
			uint_1 = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1u) == 1u)
					{
						num = (num >> 1 ^ this.uint_0);
					}
					else
					{
						num >>= 1;
					}
				}
				if (bool_0)
				{
					uint_1[(int)CRC32.smethod_1(b)] = CRC32.smethod_0(num);
				}
				else
				{
					uint_1[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		private uint method_2(uint[] uint_3, uint uint_4)
		{
			uint num = 0u;
			int num2 = 0;
			while (uint_4 != 0u)
			{
				if ((uint_4 & 1u) == 1u)
				{
					num ^= uint_3[num2];
				}
				uint_4 >>= 1;
				num2++;
			}
			return num;
		}

		private void method_3(uint[] uint_3, uint[] uint_4)
		{
			for (int i = 0; i < 32; i++)
			{
				uint_3[i] = this.method_2(uint_4, uint_4[i]);
			}
		}

		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~uint_2;
			array2[0] = this.uint_0;
			uint num2 = 1u;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.method_3(array, array2);
			this.method_3(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.method_3(array, array2);
				if ((num3 & 1u) == 1u)
				{
					num = this.method_2(array, num);
				}
				num3 >>= 1;
				if (num3 == 0u)
				{
					break;
				}
				this.method_3(array2, array);
				if ((num3 & 1u) == 1u)
				{
					num = this.method_2(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0u);
			num ^= (uint)crc;
			uint_2 = ~num;
		}

		public CRC32() : this(false)
		{
		}

		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		public CRC32(int polynomial, bool reverseBits)
		{
			bool_0 = reverseBits;
			this.uint_0 = (uint)polynomial;
			this.method_1();
		}

		public void Reset()
		{
			uint_2 = 4294967295u;
		}
	}
}
