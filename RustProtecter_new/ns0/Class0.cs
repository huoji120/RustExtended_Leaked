using System;
using System.IO.Compression.Zlib;

namespace ns0
{
	internal sealed class Class0
	{
		internal delegate Enum0 Delegate0(FlushType flush);

		internal class Class1
		{
			internal int int_0;

			internal int int_1;

			internal int int_2;

			internal int int_3;

			internal Enum1 enum1_0;

			private static readonly Class0.Class1[] class1_0;

			private Class1(int goodLength, int maxLazy, int niceLength, int maxChainLength, Enum1 flavor)
			{
				this.int_0 = goodLength;
				this.int_1 = maxLazy;
				this.int_2 = niceLength;
				this.int_3 = maxChainLength;
				this.enum1_0 = flavor;
			}

			public static Class0.Class1 smethod_0(CompressionLevel compressionLevel_0)
			{
				return Class0.Class1.class1_0[(int)compressionLevel_0];
			}

			static Class1()
			{
				Class0.Class1.class1_0 = new Class0.Class1[]
				{
					new Class0.Class1(0, 0, 0, 0, Enum1.const_0),
					new Class0.Class1(4, 4, 8, 4, Enum1.const_1),
					new Class0.Class1(4, 5, 16, 8, Enum1.const_1),
					new Class0.Class1(4, 6, 32, 32, Enum1.const_1),
					new Class0.Class1(4, 4, 16, 16, Enum1.const_2),
					new Class0.Class1(8, 16, 32, 32, Enum1.const_2),
					new Class0.Class1(8, 16, 128, 128, Enum1.const_2),
					new Class0.Class1(8, 32, 128, 256, Enum1.const_2),
					new Class0.Class1(32, 128, 258, 1024, Enum1.const_2),
					new Class0.Class1(32, 258, 258, 4096, Enum1.const_2)
				};
			}
		}

		private static readonly int int_0 = 9;

		private static readonly int int_1 = 8;

		private Class0.Delegate0 delegate0_0;

		private static readonly string[] string_0 = new string[]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		private static readonly int int_2 = 32;

		private static readonly int int_3 = 42;

		private static readonly int int_4 = 113;

		private static readonly int int_5 = 666;

		private static readonly int int_6 = 8;

		private static readonly int int_7 = 0;

		private static readonly int int_8 = 1;

		private static readonly int int_9 = 2;

		private static readonly int int_10 = 0;

		private static readonly int int_11 = 1;

		private static readonly int int_12 = 2;

		private static readonly int int_13 = 16;

		private static readonly int int_14 = 3;

		private static readonly int int_15 = 258;

		private static readonly int int_16 = Class0.int_15 + Class0.int_14 + 1;

		private static readonly int int_17 = 2 * Class9.int_5 + 1;

		private static readonly int int_18 = 256;

		internal ZlibCodec zlibCodec_0;

		internal int int_19;

		internal byte[] byte_0;

		internal int int_20;

		internal int int_21;

		internal sbyte sbyte_0;

		internal int int_22;

		internal int int_23;

		internal int int_24;

		internal int int_25;

		internal byte[] byte_1;

		internal int int_26;

		internal short[] short_0;

		internal short[] short_1;

		internal int int_27;

		internal int int_28;

		internal int int_29;

		internal int int_30;

		internal int int_31;

		internal int int_32;

		private Class0.Class1 class1_0;

		internal int int_33;

		internal int int_34;

		internal int int_35;

		internal int int_36;

		internal int int_37;

		internal int int_38;

		internal int int_39;

		internal CompressionLevel compressionLevel_0;

		internal CompressionStrategy compressionStrategy_0;

		internal short[] short_2;

		internal short[] short_3;

		internal short[] short_4;

		internal Class7 class7_0 = new Class7();

		internal Class7 class7_1 = new Class7();

		internal Class7 class7_2 = new Class7();

		internal short[] short_5 = new short[Class9.int_0 + 1];

		internal int[] int_40 = new int[2 * Class9.int_5 + 1];

		internal int int_41;

		internal int int_42;

		internal sbyte[] sbyte_1 = new sbyte[2 * Class9.int_5 + 1];

		internal int int_43;

		internal int int_44;

		internal int int_45;

		internal int int_46;

		internal int int_47;

		internal int int_48;

		internal int int_49;

		internal int int_50;

		internal short short_6;

		internal int int_51;

		private bool bool_0;

		private bool bool_1 = true;

		internal bool WantRfc1950HeaderBytes
		{
			get
			{
				return this.bool_1;
			}
			set
			{
				this.bool_1 = value;
			}
		}

		internal Class0()
		{
			this.short_2 = new short[Class0.int_17 * 2];
			this.short_3 = new short[(2 * Class9.int_2 + 1) * 2];
			this.short_4 = new short[(2 * Class9.int_1 + 1) * 2];
		}

		private void method_0()
		{
			this.int_26 = 2 * this.int_23;
			Array.Clear(this.short_1, 0, this.int_28);
			this.class1_0 = Class0.Class1.smethod_0(this.compressionLevel_0);
			this.method_27();
			this.int_36 = 0;
			this.int_32 = 0;
			this.int_38 = 0;
			this.int_33 = (this.int_39 = Class0.int_14 - 1);
			this.int_35 = 0;
			this.int_27 = 0;
		}

		private void method_1()
		{
			this.class7_0.short_0 = this.short_2;
			this.class7_0.class10_0 = Class10.class10_0;
			this.class7_1.short_0 = this.short_3;
			this.class7_1.class10_0 = Class10.class10_1;
			this.class7_2.short_0 = this.short_4;
			this.class7_2.class10_0 = Class10.class10_2;
			this.short_6 = 0;
			this.int_51 = 0;
			this.int_50 = 8;
			this.method_2();
		}

		internal void method_2()
		{
			for (int i = 0; i < Class9.int_5; i++)
			{
				this.short_2[i * 2] = 0;
			}
			for (int j = 0; j < Class9.int_2; j++)
			{
				this.short_3[j * 2] = 0;
			}
			for (int k = 0; k < Class9.int_1; k++)
			{
				this.short_4[k * 2] = 0;
			}
			this.short_2[Class0.int_18 * 2] = 1;
			this.int_48 = 0;
			this.int_47 = 0;
			this.int_49 = 0;
			this.int_45 = 0;
		}

		internal void method_3(short[] short_7, int int_52)
		{
			int num = this.int_40[int_52];
			for (int i = int_52 << 1; i <= this.int_41; i <<= 1)
			{
				if (i < this.int_41 && Class0.smethod_0(short_7, this.int_40[i + 1], this.int_40[i], this.sbyte_1))
				{
					i++;
				}
				if (Class0.smethod_0(short_7, num, this.int_40[i], this.sbyte_1))
				{
					break;
				}
				this.int_40[int_52] = this.int_40[i];
				int_52 = i;
			}
			this.int_40[int_52] = num;
		}

		internal static bool smethod_0(short[] short_7, int int_52, int int_53, sbyte[] sbyte_2)
		{
			short num = short_7[int_52 * 2];
			short num2 = short_7[int_53 * 2];
			return num < num2 || (num == num2 && sbyte_2[int_52] <= sbyte_2[int_53]);
		}

		internal void method_4(short[] short_7, int int_52)
		{
			int num = -1;
			int num2 = (int)short_7[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			short_7[(int_52 + 1) * 2 + 1] = 32767;
			for (int i = 0; i <= int_52; i++)
			{
				int num6 = num2;
				num2 = (int)short_7[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.short_4[num6 * 2] = (short)((int)this.short_4[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] expr_87_cp_0 = this.short_4;
							int expr_87_cp_1 = num6 * 2;
							expr_87_cp_0[expr_87_cp_1] += 1;
						}
						short[] expr_A7_cp_0 = this.short_4;
						int expr_A7_cp_1 = Class9.int_7 * 2;
						expr_A7_cp_0[expr_A7_cp_1] += 1;
					}
					else if (num3 <= 10)
					{
						short[] expr_CF_cp_0 = this.short_4;
						int expr_CF_cp_1 = Class9.int_8 * 2;
						expr_CF_cp_0[expr_CF_cp_1] += 1;
					}
					else
					{
						short[] expr_F1_cp_0 = this.short_4;
						int expr_F1_cp_1 = Class9.int_9 * 2;
						expr_F1_cp_0[expr_F1_cp_1] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		internal int method_5()
		{
			this.method_4(this.short_2, this.class7_0.int_7);
			this.method_4(this.short_3, this.class7_1.int_7);
			this.class7_2.method_1(this);
			int num = Class9.int_1 - 1;
			while (num >= 3 && this.short_4[(int)(Class7.sbyte_0[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.int_47 += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		internal void method_6(int int_52, int int_53, int int_54)
		{
			this.method_10(int_52 - 257, 5);
			this.method_10(int_53 - 1, 5);
			this.method_10(int_54 - 4, 4);
			for (int i = 0; i < int_54; i++)
			{
				this.method_10((int)this.short_4[(int)(Class7.sbyte_0[i] * 2 + 1)], 3);
			}
			this.method_7(this.short_2, int_52 - 1);
			this.method_7(this.short_3, int_53 - 1);
		}

		internal void method_7(short[] short_7, int int_52)
		{
			int num = -1;
			int num2 = (int)short_7[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= int_52; i++)
			{
				int num6 = num2;
				num2 = (int)short_7[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.method_9(num6, this.short_4);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.method_9(num6, this.short_4);
							num3--;
						}
						this.method_9(Class9.int_7, this.short_4);
						this.method_10(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.method_9(Class9.int_8, this.short_4);
						this.method_10(num3 - 3, 3);
					}
					else
					{
						this.method_9(Class9.int_9, this.short_4);
						this.method_10(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		private void method_8(byte[] byte_2, int int_52, int int_53)
		{
			Array.Copy(byte_2, int_52, this.byte_0, this.int_21, int_53);
			this.int_21 += int_53;
		}

		internal void method_9(int int_52, short[] short_7)
		{
			int num = int_52 * 2;
			this.method_10((int)short_7[num] & 65535, (int)short_7[num + 1] & 65535);
		}

		internal void method_10(int int_52, int int_53)
		{
			if (this.int_51 > Class0.int_13 - int_53)
			{
				this.short_6 |= (short)(int_52 << this.int_51 & 65535);
				this.byte_0[this.int_21++] = (byte)this.short_6;
				this.byte_0[this.int_21++] = (byte)(this.short_6 >> 8);
				this.short_6 = (short)((uint)int_52 >> Class0.int_13 - this.int_51);
				this.int_51 += int_53 - Class0.int_13;
				return;
			}
			this.short_6 |= (short)(int_52 << this.int_51 & 65535);
			this.int_51 += int_53;
		}

		internal void method_11()
		{
			this.method_10(Class0.int_8 << 1, 3);
			this.method_9(Class0.int_18, Class10.short_0);
			this.method_15();
			if (1 + this.int_50 + 10 - this.int_51 < 9)
			{
				this.method_10(Class0.int_8 << 1, 3);
				this.method_9(Class0.int_18, Class10.short_0);
				this.method_15();
			}
			this.int_50 = 7;
		}

		internal bool method_12(int int_52, int int_53)
		{
			this.byte_0[this.int_46 + this.int_45 * 2] = (byte)((uint)int_52 >> 8);
			this.byte_0[this.int_46 + this.int_45 * 2 + 1] = (byte)int_52;
			this.byte_0[this.int_43 + this.int_45] = (byte)int_53;
			this.int_45++;
			if (int_52 == 0)
			{
				short[] expr_69_cp_0 = this.short_2;
				int expr_69_cp_1 = int_53 * 2;
				expr_69_cp_0[expr_69_cp_1] += 1;
			}
			else
			{
				this.int_49++;
				int_52--;
				short[] expr_A8_cp_0 = this.short_2;
				int expr_A8_cp_1 = ((int)Class7.sbyte_2[int_53] + Class9.int_3 + 1) * 2;
				expr_A8_cp_0[expr_A8_cp_1] += 1;
				short[] expr_C9_cp_0 = this.short_3;
				int expr_C9_cp_1 = Class7.smethod_0(int_52) * 2;
				expr_C9_cp_0[expr_C9_cp_1] += 1;
			}
			if ((this.int_45 & 8191) == 0 && this.compressionLevel_0 > CompressionLevel.Level2)
			{
				int num = this.int_45 << 3;
				int num2 = this.int_36 - this.int_32;
				for (int i = 0; i < Class9.int_2; i++)
				{
					num = (int)((long)num + (long)this.short_3[i * 2] * (5L + (long)Class7.int_3[i]));
				}
				num >>= 3;
				if (this.int_49 < this.int_45 / 2 && num < num2 / 2)
				{
					return true;
				}
			}
			return this.int_45 == this.int_44 - 1 || this.int_45 == this.int_44;
		}

		internal void method_13(short[] short_7, short[] short_8)
		{
			int num = 0;
			if (this.int_45 != 0)
			{
				do
				{
					int num2 = this.int_46 + num * 2;
					int num3 = ((int)this.byte_0[num2] << 8 & 65280) | (int)(this.byte_0[num2 + 1] & 255);
					int num4 = (int)(this.byte_0[this.int_43 + num] & 255);
					num++;
					if (num3 == 0)
					{
						this.method_9(num4, short_7);
					}
					else
					{
						int num5 = (int)Class7.sbyte_2[num4];
						this.method_9(num5 + Class9.int_3 + 1, short_7);
						int num6 = Class7.int_2[num5];
						if (num6 != 0)
						{
							num4 -= Class7.int_5[num5];
							this.method_10(num4, num6);
						}
						num3--;
						num5 = Class7.smethod_0(num3);
						this.method_9(num5, short_8);
						num6 = Class7.int_3[num5];
						if (num6 != 0)
						{
							num3 -= Class7.int_6[num5];
							this.method_10(num3, num6);
						}
					}
				}
				while (num < this.int_45);
			}
			this.method_9(Class0.int_18, short_7);
			this.int_50 = (int)short_7[Class0.int_18 * 2 + 1];
		}

		internal void method_14()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.short_2[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.short_2[i * 2];
				i++;
			}
			while (i < Class9.int_3)
			{
				num2 += (int)this.short_2[i * 2];
				i++;
			}
			this.sbyte_0 = (sbyte)((num2 > num >> 2) ? Class0.int_10 : Class0.int_11);
		}

		internal void method_15()
		{
			if (this.int_51 == 16)
			{
				this.byte_0[this.int_21++] = (byte)this.short_6;
				this.byte_0[this.int_21++] = (byte)(this.short_6 >> 8);
				this.short_6 = 0;
				this.int_51 = 0;
				return;
			}
			if (this.int_51 >= 8)
			{
				this.byte_0[this.int_21++] = (byte)this.short_6;
				this.short_6 = (short)(this.short_6 >> 8);
				this.int_51 -= 8;
			}
		}

		internal void method_16()
		{
			if (this.int_51 > 8)
			{
				this.byte_0[this.int_21++] = (byte)this.short_6;
				this.byte_0[this.int_21++] = (byte)(this.short_6 >> 8);
			}
			else if (this.int_51 > 0)
			{
				this.byte_0[this.int_21++] = (byte)this.short_6;
			}
			this.short_6 = 0;
			this.int_51 = 0;
		}

		internal void method_17(int int_52, int int_53, bool bool_2)
		{
			this.method_16();
			this.int_50 = 8;
			if (bool_2)
			{
				this.byte_0[this.int_21++] = (byte)int_53;
				this.byte_0[this.int_21++] = (byte)(int_53 >> 8);
				this.byte_0[this.int_21++] = (byte)(~(byte)int_53);
				this.byte_0[this.int_21++] = (byte)(~int_53 >> 8);
			}
			this.method_8(this.byte_1, int_52, int_53);
		}

		internal void method_18(bool bool_2)
		{
			this.method_21((this.int_32 >= 0) ? this.int_32 : -1, this.int_36 - this.int_32, bool_2);
			this.int_32 = this.int_36;
			this.zlibCodec_0.method_1();
		}

		internal Enum0 method_19(FlushType flushType_0)
		{
			int num = 65535;
			if (65535 > this.byte_0.Length - 5)
			{
				num = this.byte_0.Length - 5;
			}
			while (true)
			{
				if (this.int_38 <= 1)
				{
					this.method_22();
					if (this.int_38 == 0 && flushType_0 == FlushType.None)
					{
						return Enum0.const_0;
					}
					if (this.int_38 == 0)
					{
						goto IL_E9;
					}
				}
				this.int_36 += this.int_38;
				this.int_38 = 0;
				int num2 = this.int_32 + num;
				if (this.int_36 == 0 || this.int_36 >= num2)
				{
					this.int_38 = this.int_36 - num2;
					this.int_36 = num2;
					this.method_18(false);
					if (this.zlibCodec_0.AvailableBytesOut == 0)
					{
						return Enum0.const_0;
					}
				}
				if (this.int_36 - this.int_32 >= this.int_23 - Class0.int_16)
				{
					this.method_18(false);
					if (this.zlibCodec_0.AvailableBytesOut == 0)
					{
						break;
					}
				}
			}
			return Enum0.const_0;
			IL_E9:
			this.method_18(flushType_0 == FlushType.Finish);
			if (this.zlibCodec_0.AvailableBytesOut == 0)
			{
				if (flushType_0 != FlushType.Finish)
				{
					return Enum0.const_0;
				}
				return Enum0.const_2;
			}
			else
			{
				if (flushType_0 != FlushType.Finish)
				{
					return Enum0.const_1;
				}
				return Enum0.const_3;
			}
			return Enum0.const_0;
		}

		internal void method_20(int int_52, int int_53, bool bool_2)
		{
			this.method_10((Class0.int_7 << 1) + (bool_2 ? 1 : 0), 3);
			this.method_17(int_52, int_53, true);
		}

		internal void method_21(int int_52, int int_53, bool bool_2)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.compressionLevel_0 > CompressionLevel.None)
			{
				if ((int)this.sbyte_0 == Class0.int_12)
				{
					this.method_14();
				}
				this.class7_0.method_1(this);
				this.class7_1.method_1(this);
				num = this.method_5();
				num2 = this.int_47 + 3 + 7 >> 3;
				num3 = this.int_48 + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = int_53 + 5);
			}
			if (int_53 + 4 <= num2 && int_52 != -1)
			{
				this.method_20(int_52, int_53, bool_2);
			}
			else if (num3 == num2)
			{
				this.method_10((Class0.int_8 << 1) + (bool_2 ? 1 : 0), 3);
				this.method_13(Class10.short_0, Class10.short_1);
			}
			else
			{
				this.method_10((Class0.int_9 << 1) + (bool_2 ? 1 : 0), 3);
				this.method_6(this.class7_0.int_7 + 1, this.class7_1.int_7 + 1, num + 1);
				this.method_13(this.short_2, this.short_3);
			}
			this.method_2();
			if (bool_2)
			{
				this.method_16();
			}
		}

		private void method_22()
		{
			do
			{
				int num = this.int_26 - this.int_38 - this.int_36;
				int num2;
				if (num == 0 && this.int_36 == 0 && this.int_38 == 0)
				{
					num = this.int_23;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.int_36 >= this.int_23 + this.int_23 - Class0.int_16)
				{
					Array.Copy(this.byte_1, this.int_23, this.byte_1, 0, this.int_23);
					this.int_37 -= this.int_23;
					this.int_36 -= this.int_23;
					this.int_32 -= this.int_23;
					num2 = this.int_28;
					int num3 = num2;
					do
					{
						int num4 = (int)this.short_1[--num3] & 65535;
						this.short_1[num3] = (short)((num4 < this.int_23) ? 0 : (num4 - this.int_23));
					}
					while (--num2 != 0);
					num2 = this.int_23;
					num3 = num2;
					do
					{
						int num4 = (int)this.short_0[--num3] & 65535;
						this.short_0[num3] = (short)((num4 < this.int_23) ? 0 : (num4 - this.int_23));
					}
					while (--num2 != 0);
					num += this.int_23;
				}
				if (this.zlibCodec_0.AvailableBytesIn == 0)
				{
					return;
				}
				num2 = this.zlibCodec_0.method_2(this.byte_1, this.int_36 + this.int_38, num);
				this.int_38 += num2;
				if (this.int_38 >= Class0.int_14)
				{
					this.int_27 = (int)(this.byte_1[this.int_36] & 255);
					this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[this.int_36 + 1] & 255)) & this.int_30);
				}
				if (this.int_38 >= Class0.int_16)
				{
					break;
				}
			}
			while (this.zlibCodec_0.AvailableBytesIn != 0);
		}

		internal Enum0 method_23(FlushType flushType_0)
		{
			int num = 0;
			while (true)
			{
				if (this.int_38 < Class0.int_16)
				{
					this.method_22();
					if (this.int_38 < Class0.int_16 && flushType_0 == FlushType.None)
					{
						return Enum0.const_0;
					}
					if (this.int_38 == 0)
					{
						goto IL_2F0;
					}
				}
				if (this.int_38 >= Class0.int_14)
				{
					this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[this.int_36 + (Class0.int_14 - 1)] & 255)) & this.int_30);
					num = ((int)this.short_1[this.int_27] & 65535);
					this.short_0[this.int_36 & this.int_25] = this.short_1[this.int_27];
					this.short_1[this.int_27] = (short)this.int_36;
				}
				if ((long)num != 0L && (this.int_36 - num & 65535) <= this.int_23 - Class0.int_16 && this.compressionStrategy_0 != CompressionStrategy.HuffmanOnly)
				{
					this.int_33 = this.method_25(num);
				}
				bool flag;
				if (this.int_33 >= Class0.int_14)
				{
					flag = this.method_12(this.int_36 - this.int_37, this.int_33 - Class0.int_14);
					this.int_38 -= this.int_33;
					if (this.int_33 <= this.class1_0.int_1 && this.int_38 >= Class0.int_14)
					{
						this.int_33--;
						do
						{
							this.int_36++;
							this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[this.int_36 + (Class0.int_14 - 1)] & 255)) & this.int_30);
							num = ((int)this.short_1[this.int_27] & 65535);
							this.short_0[this.int_36 & this.int_25] = this.short_1[this.int_27];
							this.short_1[this.int_27] = (short)this.int_36;
						}
						while (--this.int_33 != 0);
						this.int_36++;
					}
					else
					{
						this.int_36 += this.int_33;
						this.int_33 = 0;
						this.int_27 = (int)(this.byte_1[this.int_36] & 255);
						this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[this.int_36 + 1] & 255)) & this.int_30);
					}
				}
				else
				{
					flag = this.method_12(0, (int)(this.byte_1[this.int_36] & 255));
					this.int_38--;
					this.int_36++;
				}
				if (flag)
				{
					this.method_18(false);
					if (this.zlibCodec_0.AvailableBytesOut == 0)
					{
						break;
					}
				}
			}
			return Enum0.const_0;
			IL_2F0:
			this.method_18(flushType_0 == FlushType.Finish);
			if (this.zlibCodec_0.AvailableBytesOut == 0)
			{
				if (flushType_0 == FlushType.Finish)
				{
					return Enum0.const_2;
				}
				return Enum0.const_0;
			}
			else
			{
				if (flushType_0 != FlushType.Finish)
				{
					return Enum0.const_1;
				}
				return Enum0.const_3;
			}
		}

		internal Enum0 method_24(FlushType flushType_0)
		{
			int num = 0;
			while (true)
			{
				if (this.int_38 < Class0.int_16)
				{
					this.method_22();
					if (this.int_38 < Class0.int_16 && flushType_0 == FlushType.None)
					{
						return Enum0.const_0;
					}
					if (this.int_38 == 0)
					{
						goto IL_385;
					}
				}
				if (this.int_38 >= Class0.int_14)
				{
					this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[this.int_36 + (Class0.int_14 - 1)] & 255)) & this.int_30);
					num = ((int)this.short_1[this.int_27] & 65535);
					this.short_0[this.int_36 & this.int_25] = this.short_1[this.int_27];
					this.short_1[this.int_27] = (short)this.int_36;
				}
				this.int_39 = this.int_33;
				this.int_34 = this.int_37;
				this.int_33 = Class0.int_14 - 1;
				if (num != 0 && this.int_39 < this.class1_0.int_1 && (this.int_36 - num & 65535) <= this.int_23 - Class0.int_16)
				{
					if (this.compressionStrategy_0 != CompressionStrategy.HuffmanOnly)
					{
						this.int_33 = this.method_25(num);
					}
					if (this.int_33 <= 5 && (this.compressionStrategy_0 == CompressionStrategy.Filtered || (this.int_33 == Class0.int_14 && this.int_36 - this.int_37 > 4096)))
					{
						this.int_33 = Class0.int_14 - 1;
					}
				}
				if (this.int_39 >= Class0.int_14 && this.int_33 <= this.int_39)
				{
					int num2 = this.int_36 + this.int_38 - Class0.int_14;
					bool flag = this.method_12(this.int_36 - 1 - this.int_34, this.int_39 - Class0.int_14);
					this.int_38 -= this.int_39 - 1;
					this.int_39 -= 2;
					do
					{
						if (++this.int_36 <= num2)
						{
							this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[this.int_36 + (Class0.int_14 - 1)] & 255)) & this.int_30);
							num = ((int)this.short_1[this.int_27] & 65535);
							this.short_0[this.int_36 & this.int_25] = this.short_1[this.int_27];
							this.short_1[this.int_27] = (short)this.int_36;
						}
					}
					while (--this.int_39 != 0);
					this.int_35 = 0;
					this.int_33 = Class0.int_14 - 1;
					this.int_36++;
					if (flag)
					{
						this.method_18(false);
						if (this.zlibCodec_0.AvailableBytesOut == 0)
						{
							break;
						}
					}
				}
				else if (this.int_35 != 0)
				{
					if (this.method_12(0, (int)(this.byte_1[this.int_36 - 1] & 255)))
					{
						this.method_18(false);
					}
					this.int_36++;
					this.int_38--;
					if (this.zlibCodec_0.AvailableBytesOut == 0)
					{
						return Enum0.const_0;
					}
				}
				else
				{
					this.int_35 = 1;
					this.int_36++;
					this.int_38--;
				}
			}
			return Enum0.const_0;
			IL_385:
			if (this.int_35 != 0)
			{
				bool flag = this.method_12(0, (int)(this.byte_1[this.int_36 - 1] & 255));
				this.int_35 = 0;
			}
			this.method_18(flushType_0 == FlushType.Finish);
			if (this.zlibCodec_0.AvailableBytesOut == 0)
			{
				if (flushType_0 == FlushType.Finish)
				{
					return Enum0.const_2;
				}
				return Enum0.const_0;
			}
			else
			{
				if (flushType_0 != FlushType.Finish)
				{
					return Enum0.const_1;
				}
				return Enum0.const_3;
			}
		}

		internal int method_25(int int_52)
		{
			int num = this.class1_0.int_3;
			int num2 = this.int_36;
			int num3 = this.int_39;
			int num4 = (this.int_36 > this.int_23 - Class0.int_16) ? (this.int_36 - (this.int_23 - Class0.int_16)) : 0;
			int num5 = this.class1_0.int_2;
			int num6 = this.int_25;
			int num7 = this.int_36 + Class0.int_15;
			byte b = this.byte_1[num2 + num3 - 1];
			byte b2 = this.byte_1[num2 + num3];
			if (this.int_39 >= this.class1_0.int_0)
			{
				num >>= 2;
			}
			if (num5 > this.int_38)
			{
				num5 = this.int_38;
			}
			do
			{
				int num8 = int_52;
				if (this.byte_1[num8 + num3] == b2 && this.byte_1[num8 + num3 - 1] == b && this.byte_1[num8] == this.byte_1[num2] && this.byte_1[++num8] == this.byte_1[num2 + 1])
				{
					num2 += 2;
					num8++;
					while (this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && this.byte_1[++num2] == this.byte_1[++num8] && num2 < num7)
					{
					}
					int num9 = Class0.int_15 - (num7 - num2);
					num2 = num7 - Class0.int_15;
					if (num9 > num3)
					{
						this.int_37 = int_52;
						num3 = num9;
						if (num9 >= num5)
						{
							break;
						}
						b = this.byte_1[num2 + num3 - 1];
						b2 = this.byte_1[num2 + num3];
					}
				}
				if ((int_52 = ((int)this.short_0[int_52 & num6] & 65535)) <= num4)
				{
					break;
				}
			}
			while (--num != 0);
			if (num3 <= this.int_38)
			{
				return num3;
			}
			return this.int_38;
		}

		internal int Initialize(ZlibCodec zlibCodec_1, CompressionLevel compressionLevel_1)
		{
			return this.Initialize(zlibCodec_1, compressionLevel_1, 15);
		}

		internal int Initialize(ZlibCodec zlibCodec_1, CompressionLevel compressionLevel_1, int int_52)
		{
			return this.Initialize(zlibCodec_1, compressionLevel_1, int_52, Class0.int_1, CompressionStrategy.Default);
		}

		internal int Initialize(ZlibCodec zlibCodec_1, CompressionLevel compressionLevel_1, int int_52, CompressionStrategy compressionStrategy_1)
		{
			return this.Initialize(zlibCodec_1, compressionLevel_1, int_52, Class0.int_1, compressionStrategy_1);
		}

		internal int Initialize(ZlibCodec zlibCodec_1, CompressionLevel compressionLevel_1, int int_52, int int_53, CompressionStrategy compressionStrategy_1)
		{
			this.zlibCodec_0 = zlibCodec_1;
			this.zlibCodec_0.Message = null;
			if (int_52 < 9 || int_52 > 15)
			{
				throw new ZlibException("windowBits must be in the range 9..15.");
			}
			if (int_53 < 1 || int_53 > Class0.int_0)
			{
				throw new ZlibException(string.Format("memLevel must be in the range 1.. {0}", Class0.int_0));
			}
			this.zlibCodec_0.class0_0 = this;
			this.int_24 = int_52;
			this.int_23 = 1 << this.int_24;
			this.int_25 = this.int_23 - 1;
			this.int_29 = int_53 + 7;
			this.int_28 = 1 << this.int_29;
			this.int_30 = this.int_28 - 1;
			this.int_31 = (this.int_29 + Class0.int_14 - 1) / Class0.int_14;
			this.byte_1 = new byte[this.int_23 * 2];
			this.short_0 = new short[this.int_23];
			this.short_1 = new short[this.int_28];
			this.int_44 = 1 << int_53 + 6;
			this.byte_0 = new byte[this.int_44 * 4];
			this.int_46 = this.int_44;
			this.int_43 = 3 * this.int_44;
			this.compressionLevel_0 = compressionLevel_1;
			this.compressionStrategy_0 = compressionStrategy_1;
			this.Reset();
			return 0;
		}

		internal void Reset()
		{
			ZlibCodec arg_1C_0 = this.zlibCodec_0;
			this.zlibCodec_0.TotalBytesOut = 0L;
			arg_1C_0.TotalBytesIn = 0L;
			this.zlibCodec_0.Message = null;
			this.int_21 = 0;
			this.int_20 = 0;
			this.bool_0 = false;
			this.int_19 = (this.WantRfc1950HeaderBytes ? Class0.int_3 : Class0.int_4);
			this.zlibCodec_0.uint_0 = Adler.Adler32(0u, null, 0, 0);
			this.int_22 = 0;
			this.method_1();
			this.method_0();
		}

		internal int method_26()
		{
			if (this.int_19 != Class0.int_3 && this.int_19 != Class0.int_4 && this.int_19 != Class0.int_5)
			{
				return -2;
			}
			this.byte_0 = null;
			this.short_1 = null;
			this.short_0 = null;
			this.byte_1 = null;
			if (this.int_19 != Class0.int_4)
			{
				return 0;
			}
			return -3;
		}

		private void method_27()
		{
			switch (this.class1_0.enum1_0)
			{
			case Enum1.const_0:
				this.delegate0_0 = new Class0.Delegate0(this.method_19);
				return;
			case Enum1.const_1:
				this.delegate0_0 = new Class0.Delegate0(this.method_23);
				return;
			case Enum1.const_2:
				this.delegate0_0 = new Class0.Delegate0(this.method_24);
				return;
			default:
				return;
			}
		}

		internal int method_28(CompressionLevel compressionLevel_1, CompressionStrategy compressionStrategy_1)
		{
			int result = 0;
			if (this.compressionLevel_0 != compressionLevel_1)
			{
				Class0.Class1 @class = Class0.Class1.smethod_0(compressionLevel_1);
				if (@class.enum1_0 != this.class1_0.enum1_0 && this.zlibCodec_0.TotalBytesIn != 0L)
				{
					result = this.zlibCodec_0.Deflate(FlushType.Partial);
				}
				this.compressionLevel_0 = compressionLevel_1;
				this.class1_0 = @class;
				this.method_27();
			}
			this.compressionStrategy_0 = compressionStrategy_1;
			return result;
		}

		internal int SetDictionary(byte[] byte_2)
		{
			int num = byte_2.Length;
			int sourceIndex = 0;
			if (byte_2 != null)
			{
				if (this.int_19 == Class0.int_3)
				{
					this.zlibCodec_0.uint_0 = Adler.Adler32(this.zlibCodec_0.uint_0, byte_2, 0, byte_2.Length);
					if (num < Class0.int_14)
					{
						return 0;
					}
					if (num > this.int_23 - Class0.int_16)
					{
						num = this.int_23 - Class0.int_16;
						sourceIndex = byte_2.Length - num;
					}
					Array.Copy(byte_2, sourceIndex, this.byte_1, 0, num);
					this.int_36 = num;
					this.int_32 = num;
					this.int_27 = (int)(this.byte_1[0] & 255);
					this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[1] & 255)) & this.int_30);
					for (int i = 0; i <= num - Class0.int_14; i++)
					{
						this.int_27 = ((this.int_27 << this.int_31 ^ (int)(this.byte_1[i + (Class0.int_14 - 1)] & 255)) & this.int_30);
						this.short_0[i & this.int_25] = this.short_1[this.int_27];
						this.short_1[this.int_27] = (short)i;
					}
					return 0;
				}
			}
			throw new ZlibException("Stream error.");
		}

		internal int Deflate(FlushType flushType_0)
		{
			if (this.zlibCodec_0.OutputBuffer != null && (this.zlibCodec_0.InputBuffer != null || this.zlibCodec_0.AvailableBytesIn == 0))
			{
				if (this.int_19 != Class0.int_5 || flushType_0 == FlushType.Finish)
				{
					if (this.zlibCodec_0.AvailableBytesOut == 0)
					{
						this.zlibCodec_0.Message = Class0.string_0[7];
						throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
					}
					int num = this.int_22;
					this.int_22 = (int)flushType_0;
					if (this.int_19 == Class0.int_3)
					{
						int num2 = Class0.int_6 + (this.int_24 - 8 << 4) << 8;
						int num3 = (this.compressionLevel_0 - CompressionLevel.BestSpeed & 255) >> 1;
						if (num3 > 3)
						{
							num3 = 3;
						}
						num2 |= num3 << 6;
						if (this.int_36 != 0)
						{
							num2 |= Class0.int_2;
						}
						num2 += 31 - num2 % 31;
						this.int_19 = Class0.int_4;
						this.byte_0[this.int_21++] = (byte)(num2 >> 8);
						this.byte_0[this.int_21++] = (byte)num2;
						if (this.int_36 != 0)
						{
							this.byte_0[this.int_21++] = (byte)((this.zlibCodec_0.uint_0 & 4278190080u) >> 24);
							this.byte_0[this.int_21++] = (byte)((this.zlibCodec_0.uint_0 & 16711680u) >> 16);
							this.byte_0[this.int_21++] = (byte)((this.zlibCodec_0.uint_0 & 65280u) >> 8);
							this.byte_0[this.int_21++] = (byte)(this.zlibCodec_0.uint_0 & 255u);
						}
						this.zlibCodec_0.uint_0 = Adler.Adler32(0u, null, 0, 0);
					}
					if (this.int_21 != 0)
					{
						this.zlibCodec_0.method_1();
						if (this.zlibCodec_0.AvailableBytesOut == 0)
						{
							this.int_22 = -1;
							return 0;
						}
					}
					else if (this.zlibCodec_0.AvailableBytesIn == 0 && flushType_0 <= (FlushType)num && flushType_0 != FlushType.Finish)
					{
						return 0;
					}
					if (this.int_19 == Class0.int_5 && this.zlibCodec_0.AvailableBytesIn != 0)
					{
						this.zlibCodec_0.Message = Class0.string_0[7];
						throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
					}
					if (this.zlibCodec_0.AvailableBytesIn != 0 || this.int_38 != 0 || (flushType_0 != FlushType.None && this.int_19 != Class0.int_5))
					{
						Enum0 @enum = this.delegate0_0(flushType_0);
						if (@enum == Enum0.const_2 || @enum == Enum0.const_3)
						{
							this.int_19 = Class0.int_5;
						}
						if (@enum != Enum0.const_0)
						{
							if (@enum != Enum0.const_2)
							{
								if (@enum != Enum0.const_1)
								{
									goto IL_31F;
								}
								if (flushType_0 == FlushType.Partial)
								{
									this.method_11();
								}
								else
								{
									this.method_20(0, 0, false);
									if (flushType_0 == FlushType.Full)
									{
										for (int i = 0; i < this.int_28; i++)
										{
											this.short_1[i] = 0;
										}
									}
								}
								this.zlibCodec_0.method_1();
								if (this.zlibCodec_0.AvailableBytesOut == 0)
								{
									this.int_22 = -1;
									return 0;
								}
								goto IL_31F;
							}
						}
						if (this.zlibCodec_0.AvailableBytesOut == 0)
						{
							this.int_22 = -1;
						}
						return 0;
					}
					IL_31F:
					if (flushType_0 != FlushType.Finish)
					{
						return 0;
					}
					if (!this.WantRfc1950HeaderBytes || this.bool_0)
					{
						return 1;
					}
					this.byte_0[this.int_21++] = (byte)((this.zlibCodec_0.uint_0 & 4278190080u) >> 24);
					this.byte_0[this.int_21++] = (byte)((this.zlibCodec_0.uint_0 & 16711680u) >> 16);
					this.byte_0[this.int_21++] = (byte)((this.zlibCodec_0.uint_0 & 65280u) >> 8);
					this.byte_0[this.int_21++] = (byte)(this.zlibCodec_0.uint_0 & 255u);
					this.zlibCodec_0.method_1();
					this.bool_0 = true;
					if (this.int_21 == 0)
					{
						return 1;
					}
					return 0;
				}
			}
			this.zlibCodec_0.Message = Class0.string_0[4];
			throw new ZlibException(string.Format("Something is fishy. [{0}]", this.zlibCodec_0.Message));
		}
	}
}
