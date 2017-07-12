using System;
using System.IO;

namespace ns2
{
	public static class Class8
	{
		public class Class9
		{
			private const int int_0 = 0;

			private const int int_1 = 1;

			private const int int_2 = 2;

			private const int int_3 = 3;

			private const int int_4 = 4;

			private const int int_5 = 5;

			private const int int_6 = 6;

			private const int int_7 = 7;

			private const int int_8 = 8;

			private const int int_9 = 9;

			private const int int_10 = 10;

			private const int int_11 = 11;

			private const int int_12 = 12;

			internal static readonly int[] int_13 = new int[]
			{
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				13,
				15,
				17,
				19,
				23,
				27,
				31,
				35,
				43,
				51,
				59,
				67,
				83,
				99,
				115,
				131,
				163,
				195,
				227,
				258
			};

			internal static readonly int[] int_14 = new int[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				5,
				5,
				5,
				5,
				0
			};

			internal static readonly int[] int_15 = new int[]
			{
				1,
				2,
				3,
				4,
				5,
				7,
				9,
				13,
				17,
				25,
				33,
				49,
				65,
				97,
				129,
				193,
				257,
				385,
				513,
				769,
				1025,
				1537,
				2049,
				3073,
				4097,
				6145,
				8193,
				12289,
				16385,
				24577
			};

			internal static readonly int[] int_16 = new int[]
			{
				0,
				0,
				0,
				0,
				1,
				1,
				2,
				2,
				3,
				3,
				4,
				4,
				5,
				5,
				6,
				6,
				7,
				7,
				8,
				8,
				9,
				9,
				10,
				10,
				11,
				11,
				12,
				12,
				13,
				13
			};

			internal int int_17;

			internal int int_18;

			internal int int_19;

			internal int int_20;

			internal int int_21;

			internal bool bool_0;

			internal Class8.Class10 class10_0;

			internal Class8.Class11 class11_0;

			internal Class8.Class13 class13_0;

			internal Class8.Class12 class12_0;

			internal Class8.Class12 class12_1;

			public Class9(byte[] bytes)
			{
				this.class10_0 = new Class8.Class10();
				this.class11_0 = new Class8.Class11();
				this.int_17 = 2;
				Class19.smethod_5(bytes.Length, 0, this.class10_0, bytes);
			}
		}

		public class Class10
		{
			internal byte[] byte_0;

			internal int int_0;

			internal int int_1;

			internal uint uint_0;

			internal int int_2;

			public int AvailableBits
			{
				get
				{
					return this.int_2;
				}
			}

			public int AvailableBytes
			{
				get
				{
					return this.int_1 - this.int_0 + (this.int_2 >> 3);
				}
			}

			public bool IsNeedingInput
			{
				get
				{
					return this.int_0 == this.int_1;
				}
			}
		}

		internal class Class11
		{
			private const int int_0 = 32768;

			private const int int_1 = 32767;

			internal byte[] byte_0 = new byte[32768];

			internal int int_2;

			internal int int_3;
		}

		internal class Class13
		{
			private const int int_0 = 0;

			private const int int_1 = 1;

			private const int int_2 = 2;

			private const int int_3 = 3;

			private const int int_4 = 4;

			private const int int_5 = 5;

			internal static readonly int[] int_6 = new int[]
			{
				3,
				3,
				11
			};

			internal static readonly int[] int_7 = new int[]
			{
				2,
				3,
				7
			};

			internal byte[] byte_0;

			internal byte[] byte_1;

			internal Class8.Class12 class12_0;

			internal int int_8;

			internal int int_9;

			internal int int_10;

			internal int int_11;

			internal int int_12;

			internal int int_13;

			internal byte byte_2;

			internal int int_14;

			internal static readonly int[] int_15 = new int[]
			{
				16,
				17,
				18,
				0,
				8,
				7,
				9,
				6,
				10,
				5,
				11,
				4,
				12,
				3,
				13,
				2,
				14,
				1,
				15
			};
		}

		public class Class12
		{
			private const int int_0 = 15;

			internal short[] short_0;

			public static readonly Class8.Class12 class12_0;

			public static readonly Class8.Class12 class12_1;

			static Class12()
			{
				byte[] array = new byte[288];
				int i = 0;
				while (i < 144)
				{
					array[i++] = 8;
				}
				while (i < 256)
				{
					array[i++] = 9;
				}
				while (i < 280)
				{
					array[i++] = 7;
				}
				while (i < 288)
				{
					array[i++] = 8;
				}
				Class8.Class12.class12_0 = new Class8.Class12(array);
				array = new byte[32];
				i = 0;
				while (i < 32)
				{
					array[i++] = 5;
				}
				Class8.Class12.class12_1 = new Class8.Class12(array);
			}

			public Class12(byte[] codeLengths)
			{
				Class19.smethod_18(codeLengths, this);
			}
		}

		internal class Class14
		{
			private const int int_0 = 4;

			private const int int_1 = 8;

			private const int int_2 = 16;

			private const int int_3 = 20;

			private const int int_4 = 28;

			private const int int_5 = 30;

			internal int int_6 = 16;

			internal long long_0;

			internal Class8.Class18 class18_0;

			internal Class8.Class17 class17_0;

			public long TotalOut
			{
				get
				{
					return this.long_0;
				}
			}

			public bool IsFinished
			{
				get
				{
					return this.int_6 == 30 && this.class18_0.int_1 == 0;
				}
			}

			public bool IsNeedingInput
			{
				get
				{
					return Class19.smethod_63(this.class17_0);
				}
			}

			public Class14()
			{
				this.class18_0 = new Class8.Class18();
				this.class17_0 = new Class8.Class17(this.class18_0);
			}
		}

		public class Class18
		{
			protected internal byte[] byte_0 = new byte[65536];

			internal int int_0;

			internal int int_1;

			internal uint uint_0;

			internal int int_2;

			public int BitCount
			{
				get
				{
					return this.int_2;
				}
			}

			public bool IsFlushed
			{
				get
				{
					return this.int_1 == 0;
				}
			}
		}

		public class Class17
		{
			private const int int_0 = 258;

			private const int int_1 = 3;

			private const int int_2 = 32768;

			private const int int_3 = 32767;

			private const int int_4 = 32768;

			private const int int_5 = 32767;

			private const int int_6 = 5;

			private const int int_7 = 262;

			private const int int_8 = 32506;

			private const int int_9 = 4096;

			internal int int_10;

			internal short[] short_0;

			internal short[] short_1;

			internal int int_11;

			internal int int_12;

			internal bool bool_0;

			internal int int_13;

			internal int int_14;

			internal int int_15;

			internal byte[] byte_0;

			internal byte[] byte_1;

			internal int int_16;

			internal int int_17;

			internal int int_18;

			internal Class8.Class18 class18_0;

			internal Class8.Class15 class15_0;

			public Class17(Class8.Class18 pending)
			{
				this.class18_0 = pending;
				this.class15_0 = new Class8.Class15(pending);
				this.byte_0 = new byte[65536];
				this.short_0 = new short[32768];
				this.short_1 = new short[32768];
				this.int_14 = 1;
				this.int_13 = 1;
			}
		}

		internal class Class15
		{
			public class Class16
			{
				public short[] short_0;

				public byte[] byte_0;

				public int int_0;

				public int int_1;

				internal short[] short_1;

				internal int[] int_2;

				internal int int_3;

				internal Class8.Class15 class15_0;

				public Class16(Class8.Class15 dh, int elems, int minCodes, int maxLength)
				{
					this.class15_0 = dh;
					this.int_0 = minCodes;
					this.int_3 = maxLength;
					this.short_0 = new short[elems];
					this.int_2 = new int[maxLength];
				}
			}

			private const int int_0 = 16384;

			private const int int_1 = 286;

			private const int int_2 = 30;

			private const int int_3 = 19;

			private const int int_4 = 16;

			private const int int_5 = 17;

			private const int int_6 = 18;

			private const int int_7 = 256;

			internal static readonly int[] int_8;

			internal static readonly byte[] byte_0;

			internal Class8.Class18 class18_0;

			internal Class8.Class15.Class16 class16_0;

			internal Class8.Class15.Class16 class16_1;

			internal Class8.Class15.Class16 class16_2;

			internal short[] short_0;

			internal byte[] byte_1;

			internal int int_9;

			internal int int_10;

			internal static readonly short[] short_1;

			internal static readonly byte[] byte_2;

			internal static readonly short[] short_2;

			internal static readonly byte[] byte_3;

			static Class15()
			{
				Class8.Class15.int_8 = new int[]
				{
					16,
					17,
					18,
					0,
					8,
					7,
					9,
					6,
					10,
					5,
					11,
					4,
					12,
					3,
					13,
					2,
					14,
					1,
					15
				};
				Class8.Class15.byte_0 = new byte[]
				{
					0,
					8,
					4,
					12,
					2,
					10,
					6,
					14,
					1,
					9,
					5,
					13,
					3,
					11,
					7,
					15
				};
				Class8.Class15.short_1 = new short[286];
				Class8.Class15.byte_2 = new byte[286];
				int i = 0;
				while (i < 144)
				{
					Class8.Class15.short_1[i] = Class19.smethod_42(48 + i << 8);
					Class8.Class15.byte_2[i++] = 8;
				}
				while (i < 256)
				{
					Class8.Class15.short_1[i] = Class19.smethod_42(256 + i << 7);
					Class8.Class15.byte_2[i++] = 9;
				}
				while (i < 280)
				{
					Class8.Class15.short_1[i] = Class19.smethod_42(-256 + i << 9);
					Class8.Class15.byte_2[i++] = 7;
				}
				while (i < 286)
				{
					Class8.Class15.short_1[i] = Class19.smethod_42(-88 + i << 8);
					Class8.Class15.byte_2[i++] = 8;
				}
				Class8.Class15.short_2 = new short[30];
				Class8.Class15.byte_3 = new byte[30];
				for (i = 0; i < 30; i++)
				{
					Class8.Class15.short_2[i] = Class19.smethod_42(i << 11);
					Class8.Class15.byte_3[i] = 5;
				}
			}

			public Class15(Class8.Class18 pending)
			{
				this.class18_0 = pending;
				this.class16_0 = new Class8.Class15.Class16(this, 286, 257, 15);
				this.class16_1 = new Class8.Class15.Class16(this, 30, 1, 15);
				this.class16_2 = new Class8.Class15.Class16(this, 19, 4, 7);
				this.short_0 = new short[16384];
				this.byte_1 = new byte[16384];
			}
		}

		internal class Stream0 : System.IO.MemoryStream
		{
			public Stream0()
			{
			}

			public Stream0(byte[] buffer) : base(buffer, false)
			{
			}
		}

		public static string string_0;
	}
}
