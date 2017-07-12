using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
namespace ns1
{
	internal static class Class6
	{
		internal class Class7
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
			private static readonly int[] int_13 = new int[]
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
			private static readonly int[] int_14 = new int[]
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
			private static readonly int[] int_15 = new int[]
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
			private static readonly int[] int_16 = new int[]
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
			private int int_17;
			private int int_18;
			private int int_19;
			private int int_20;
			private int int_21;
			private bool bool_0;
			private Class6.Class8 class8_0;
			private Class6.Class9 class9_0;
			private Class6.Class11 class11_0;
			private Class6.Class10 class10_0;
			private Class6.Class10 class10_1;
			public Class7(byte[] bytes)
			{
				this.class8_0 = new Class6.Class8();
				this.class9_0 = new Class6.Class9();
				this.int_17 = 2;
				this.class8_0.method_5(bytes, 0, bytes.Length);
			}
			private bool method_0()
			{
				int i = this.class9_0.method_5();
				while (i >= 258)
				{
					int num;
					switch (this.int_17)
					{
					case 7:
						while (((num = this.class10_0.method_1(this.class8_0)) & -256) == 0)
						{
							this.class9_0.method_0(num);
							if (--i < 258)
							{
								return true;
							}
						}
						if (num >= 257)
						{
							this.int_19 = Class6.Class7.int_13[num - 257];
							this.int_18 = Class6.Class7.int_14[num - 257];
							goto IL_9E;
						}
						if (num < 0)
						{
							return false;
						}
						this.class10_1 = null;
						this.class10_0 = null;
						this.int_17 = 2;
						return true;
					case 8:
						goto IL_9E;
					case 9:
						goto IL_EE;
					case 10:
						break;
					default:
						continue;
					}
					IL_121:
					if (this.int_18 > 0)
					{
						this.int_17 = 10;
						int num2 = this.class8_0.method_0(this.int_18);
						if (num2 < 0)
						{
							return false;
						}
						this.class8_0.method_1(this.int_18);
						this.int_20 += num2;
					}
					this.class9_0.method_2(this.int_19, this.int_20);
					i -= this.int_19;
					this.int_17 = 7;
					continue;
					IL_EE:
					num = this.class10_1.method_1(this.class8_0);
					if (num >= 0)
					{
						this.int_20 = Class6.Class7.int_15[num];
						this.int_18 = Class6.Class7.int_16[num];
						goto IL_121;
					}
					return false;
					IL_9E:
					if (this.int_18 > 0)
					{
						this.int_17 = 8;
						int num3 = this.class8_0.method_0(this.int_18);
						if (num3 < 0)
						{
							return false;
						}
						this.class8_0.method_1(this.int_18);
						this.int_19 += num3;
					}
					this.int_17 = 9;
					goto IL_EE;
				}
				return true;
			}
			private bool method_1()
			{
				switch (this.int_17)
				{
				case 2:
				{
					if (this.bool_0)
					{
						this.int_17 = 12;
						return false;
					}
					int num = this.class8_0.method_0(3);
					if (num < 0)
					{
						return false;
					}
					this.class8_0.method_1(3);
					if ((num & 1) != 0)
					{
						this.bool_0 = true;
					}
					switch (num >> 1)
					{
					case 0:
						this.class8_0.method_2();
						this.int_17 = 3;
						break;
					case 1:
						this.class10_0 = Class6.Class10.class10_0;
						this.class10_1 = Class6.Class10.class10_1;
						this.int_17 = 7;
						break;
					case 2:
						this.class11_0 = new Class6.Class11();
						this.int_17 = 6;
						break;
					}
					return true;
				}
				case 3:
					if ((this.int_21 = this.class8_0.method_0(16)) < 0)
					{
						return false;
					}
					this.class8_0.method_1(16);
					this.int_17 = 4;
					break;
				case 4:
					break;
				case 5:
					goto IL_137;
				case 6:
					if (!this.class11_0.method_0(this.class8_0))
					{
						return false;
					}
					this.class10_0 = this.class11_0.method_1();
					this.class10_1 = this.class11_0.method_2();
					this.int_17 = 7;
					goto IL_1BB;
				case 7:
				case 8:
				case 9:
				case 10:
					goto IL_1BB;
				case 11:
					return false;
				case 12:
					return false;
				default:
					return false;
				}
				int num2 = this.class8_0.method_0(16);
				if (num2 < 0)
				{
					return false;
				}
				this.class8_0.method_1(16);
				this.int_17 = 5;
				IL_137:
				int num3 = this.class9_0.method_3(this.class8_0, this.int_21);
				this.int_21 -= num3;
				if (this.int_21 == 0)
				{
					this.int_17 = 2;
					return true;
				}
				return !this.class8_0.IsNeedingInput;
				IL_1BB:
				return this.method_0();
			}
			public int method_2(byte[] byte_0, int int_22, int int_23)
			{
				int num = 0;
				while (true)
				{
					if (this.int_17 != 11)
					{
						int num2 = this.class9_0.method_7(byte_0, int_22, int_23);
						int_22 += num2;
						num += num2;
						int_23 -= num2;
						if (int_23 == 0)
						{
							return num;
						}
					}
					if (!this.method_1())
					{
						if (this.class9_0.method_6() <= 0)
						{
							break;
						}
						if (this.int_17 == 11)
						{
							break;
						}
					}
				}
				return num;
			}
		}
		internal class Class8
		{
			private byte[] byte_0;
			private int int_0;
			private int int_1;
			private uint uint_0;
			private int int_2;
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
			public int method_0(int int_3)
			{
				if (this.int_2 < int_3)
				{
					if (this.int_0 == this.int_1)
					{
						return -1;
					}
					this.uint_0 |= (uint)((uint)((int)(this.byte_0[this.int_0++] & 255) | (int)(this.byte_0[this.int_0++] & 255) << 8) << this.int_2);
					this.int_2 += 16;
				}
				return (int)((ulong)this.uint_0 & (ulong)((long)((1 << int_3) - 1)));
			}
			public void method_1(int int_3)
			{
				this.uint_0 >>= int_3;
				this.int_2 -= int_3;
			}
			public void method_2()
			{
				this.uint_0 >>= (this.int_2 & 7);
				this.int_2 &= -8;
			}
			public int method_3(byte[] byte_1, int int_3, int int_4)
			{
				int num = 0;
				while (this.int_2 > 0 && int_4 > 0)
				{
					byte_1[int_3++] = (byte)this.uint_0;
					this.uint_0 >>= 8;
					this.int_2 -= 8;
					int_4--;
					num++;
				}
				if (int_4 == 0)
				{
					return num;
				}
				int num2 = this.int_1 - this.int_0;
				if (int_4 > num2)
				{
					int_4 = num2;
				}
				Array.Copy(this.byte_0, this.int_0, byte_1, int_3, int_4);
				this.int_0 += int_4;
				if ((this.int_0 - this.int_1 & 1) != 0)
				{
					this.uint_0 = (uint)(this.byte_0[this.int_0++] & 255);
					this.int_2 = 8;
				}
				return num + int_4;
			}
			public void method_4()
			{
				this.int_2 = 0;
				this.int_1 = 0;
				this.int_0 = 0;
				this.uint_0 = 0u;
			}
			public void method_5(byte[] byte_1, int int_3, int int_4)
			{
				if (this.int_0 < this.int_1)
				{
					throw new InvalidOperationException();
				}
				int num = int_3 + int_4;
				if (0 <= int_3 && int_3 <= num && num <= byte_1.Length)
				{
					if ((int_4 & 1) != 0)
					{
						this.uint_0 |= (uint)((uint)(byte_1[int_3++] & 255) << this.int_2);
						this.int_2 += 8;
					}
					this.byte_0 = byte_1;
					this.int_0 = int_3;
					this.int_1 = num;
					return;
				}
				throw new ArgumentOutOfRangeException();
			}
		}
		internal class Class9
		{
			private const int int_0 = 32768;
			private const int int_1 = 32767;
			private byte[] byte_0 = new byte[32768];
			private int int_2;
			private int int_3;
			public void method_0(int int_4)
			{
				if (this.int_3++ == 32768)
				{
					throw new InvalidOperationException();
				}
				this.byte_0[this.int_2++] = (byte)int_4;
				this.int_2 &= 32767;
			}
			private void method_1(int int_4, int int_5, int int_6)
			{
				while (int_5-- > 0)
				{
					this.byte_0[this.int_2++] = this.byte_0[int_4++];
					this.int_2 &= 32767;
					int_4 &= 32767;
				}
			}
			public void method_2(int int_4, int int_5)
			{
				if ((this.int_3 += int_4) > 32768)
				{
					throw new InvalidOperationException();
				}
				int num = this.int_2 - int_5 & 32767;
				int num2 = 32768 - int_4;
				if (num > num2 || this.int_2 >= num2)
				{
					this.method_1(num, int_4, int_5);
					return;
				}
				if (int_4 <= int_5)
				{
					Array.Copy(this.byte_0, num, this.byte_0, this.int_2, int_4);
					this.int_2 += int_4;
					return;
				}
				while (int_4-- > 0)
				{
					this.byte_0[this.int_2++] = this.byte_0[num++];
				}
			}
			public int method_3(Class6.Class8 class8_0, int int_4)
			{
				int_4 = Math.Min(Math.Min(int_4, 32768 - this.int_3), class8_0.AvailableBytes);
				int num = 32768 - this.int_2;
				int num2;
				if (int_4 > num)
				{
					num2 = class8_0.method_3(this.byte_0, this.int_2, num);
					if (num2 == num)
					{
						num2 += class8_0.method_3(this.byte_0, 0, int_4 - num);
					}
				}
				else
				{
					num2 = class8_0.method_3(this.byte_0, this.int_2, int_4);
				}
				this.int_2 = (this.int_2 + num2 & 32767);
				this.int_3 += num2;
				return num2;
			}
			public void method_4(byte[] byte_1, int int_4, int int_5)
			{
				if (this.int_3 > 0)
				{
					throw new InvalidOperationException();
				}
				if (int_5 > 32768)
				{
					int_4 += int_5 - 32768;
					int_5 = 32768;
				}
				Array.Copy(byte_1, int_4, this.byte_0, 0, int_5);
				this.int_2 = (int_5 & 32767);
			}
			public int method_5()
			{
				return 32768 - this.int_3;
			}
			public int method_6()
			{
				return this.int_3;
			}
			public int method_7(byte[] byte_1, int int_4, int int_5)
			{
				int num = this.int_2;
				if (int_5 > this.int_3)
				{
					int_5 = this.int_3;
				}
				else
				{
					num = (this.int_2 - this.int_3 + int_5 & 32767);
				}
				int num2 = int_5;
				int num3 = int_5 - num;
				if (num3 > 0)
				{
					Array.Copy(this.byte_0, 32768 - num3, byte_1, int_4, num3);
					int_4 += num3;
					int_5 = num;
				}
				Array.Copy(this.byte_0, num - int_5, byte_1, int_4, int_5);
				this.int_3 -= num2;
				if (this.int_3 < 0)
				{
					throw new InvalidOperationException();
				}
				return num2;
			}
			public void method_8()
			{
				this.int_2 = 0;
				this.int_3 = 0;
			}
		}
		internal class Class10
		{
			private const int int_0 = 15;
			private short[] short_0;
			public static readonly Class6.Class10 class10_0;
			public static readonly Class6.Class10 class10_1;
			static Class10()
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
				Class6.Class10.class10_0 = new Class6.Class10(array);
				array = new byte[32];
				i = 0;
				while (i < 32)
				{
					array[i++] = 5;
				}
				Class6.Class10.class10_1 = new Class6.Class10(array);
			}
			public Class10(byte[] codeLengths)
			{
				this.method_0(codeLengths);
			}
			private void method_0(byte[] byte_0)
			{
				int[] array = new int[16];
				int[] array2 = new int[16];
				for (int i = 0; i < byte_0.Length; i++)
				{
					int num = (int)byte_0[i];
					if (num > 0)
					{
						array[num]++;
					}
				}
				int num2 = 0;
				int num3 = 512;
				for (int j = 1; j <= 15; j++)
				{
					array2[j] = num2;
					num2 += array[j] << 16 - j;
					if (j >= 10)
					{
						int num4 = array2[j] & 130944;
						int num5 = num2 & 130944;
						num3 += num5 - num4 >> 16 - j;
					}
				}
				this.short_0 = new short[num3];
				int num6 = 512;
				for (int k = 15; k >= 10; k--)
				{
					int num7 = num2 & 130944;
					num2 -= array[k] << 16 - k;
					int num8 = num2 & 130944;
					for (int l = num8; l < num7; l += 128)
					{
						this.short_0[(int)Class6.Class13.smethod_0(l)] = (short)(-num6 << 4 | k);
						num6 += 1 << k - 9;
					}
				}
				for (int m = 0; m < byte_0.Length; m++)
				{
					int num9 = (int)byte_0[m];
					if (num9 != 0)
					{
						num2 = array2[num9];
						int num10 = (int)Class6.Class13.smethod_0(num2);
						if (num9 <= 9)
						{
							do
							{
								this.short_0[num10] = (short)(m << 4 | num9);
								num10 += 1 << num9;
							}
							while (num10 < 512);
						}
						else
						{
							int num11 = (int)this.short_0[num10 & 511];
							int num12 = 1 << (num11 & 15);
							num11 = -(num11 >> 4);
							do
							{
								this.short_0[num11 | num10 >> 9] = (short)(m << 4 | num9);
								num10 += 1 << num9;
							}
							while (num10 < num12);
						}
						array2[num9] = num2 + (1 << 16 - num9);
					}
				}
			}
			public int method_1(Class6.Class8 class8_0)
			{
				int num;
				if ((num = class8_0.method_0(9)) >= 0)
				{
					int num2;
					if ((num2 = (int)this.short_0[num]) >= 0)
					{
						class8_0.method_1(num2 & 15);
						return num2 >> 4;
					}
					int num3 = -(num2 >> 4);
					int int_ = num2 & 15;
					if ((num = class8_0.method_0(int_)) >= 0)
					{
						num2 = (int)this.short_0[num3 | num >> 9];
						class8_0.method_1(num2 & 15);
						return num2 >> 4;
					}
					int availableBits = class8_0.AvailableBits;
					num = class8_0.method_0(availableBits);
					num2 = (int)this.short_0[num3 | num >> 9];
					if ((num2 & 15) <= availableBits)
					{
						class8_0.method_1(num2 & 15);
						return num2 >> 4;
					}
					return -1;
				}
				else
				{
					int availableBits2 = class8_0.AvailableBits;
					num = class8_0.method_0(availableBits2);
					int num2 = (int)this.short_0[num];
					if (num2 >= 0 && (num2 & 15) <= availableBits2)
					{
						class8_0.method_1(num2 & 15);
						return num2 >> 4;
					}
					return -1;
				}
			}
		}
		internal class Class11
		{
			private const int int_0 = 0;
			private const int int_1 = 1;
			private const int int_2 = 2;
			private const int int_3 = 3;
			private const int int_4 = 4;
			private const int int_5 = 5;
			private static readonly int[] int_6 = new int[]
			{
				3,
				3,
				11
			};
			private static readonly int[] int_7 = new int[]
			{
				2,
				3,
				7
			};
			private byte[] byte_0;
			private byte[] byte_1;
			private Class6.Class10 class10_0;
			private int int_8;
			private int int_9;
			private int int_10;
			private int int_11;
			private int int_12;
			private int int_13;
			private byte byte_2;
			private int int_14;
			private static readonly int[] int_15 = new int[]
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
			public bool method_0(Class6.Class8 class8_0)
			{
				while (true)
				{
					switch (this.int_8)
					{
					case 0:
						this.int_9 = class8_0.method_0(5);
						if (this.int_9 >= 0)
						{
							this.int_9 += 257;
							class8_0.method_1(5);
							this.int_8 = 1;
							goto IL_1DD;
						}
						return false;
					case 1:
						goto IL_1DD;
					case 2:
						goto IL_18F;
					case 3:
						goto IL_156;
					case 4:
						break;
					case 5:
						goto IL_2C;
					default:
						continue;
					}
					IL_E1:
					int num;
					while (((num = this.class10_0.method_1(class8_0)) & -16) == 0)
					{
						this.byte_1[this.int_14++] = (this.byte_2 = (byte)num);
						if (this.int_14 == this.int_12)
						{
							return true;
						}
					}
					if (num >= 0)
					{
						if (num >= 17)
						{
							this.byte_2 = 0;
						}
						this.int_13 = num - 16;
						this.int_8 = 5;
						goto IL_2C;
					}
					return false;
					IL_156:
					while (this.int_14 < this.int_11)
					{
						int num2 = class8_0.method_0(3);
						if (num2 < 0)
						{
							return false;
						}
						class8_0.method_1(3);
						this.byte_0[Class6.Class11.int_15[this.int_14]] = (byte)num2;
						this.int_14++;
					}
					this.class10_0 = new Class6.Class10(this.byte_0);
					this.byte_0 = null;
					this.int_14 = 0;
					this.int_8 = 4;
					goto IL_E1;
					IL_2C:
					int num3 = Class6.Class11.int_7[this.int_13];
					int num4 = class8_0.method_0(num3);
					if (num4 < 0)
					{
						return false;
					}
					class8_0.method_1(num3);
					num4 += Class6.Class11.int_6[this.int_13];
					while (num4-- > 0)
					{
						this.byte_1[this.int_14++] = this.byte_2;
					}
					if (this.int_14 == this.int_12)
					{
						break;
					}
					this.int_8 = 4;
					continue;
					IL_18F:
					this.int_11 = class8_0.method_0(4);
					if (this.int_11 >= 0)
					{
						this.int_11 += 4;
						class8_0.method_1(4);
						this.byte_0 = new byte[19];
						this.int_14 = 0;
						this.int_8 = 3;
						goto IL_156;
					}
					return false;
					IL_1DD:
					this.int_10 = class8_0.method_0(5);
					if (this.int_10 >= 0)
					{
						this.int_10++;
						class8_0.method_1(5);
						this.int_12 = this.int_9 + this.int_10;
						this.byte_1 = new byte[this.int_12];
						this.int_8 = 2;
						goto IL_18F;
					}
					return false;
				}
				return true;
			}
			public Class6.Class10 method_1()
			{
				byte[] array = new byte[this.int_9];
				Array.Copy(this.byte_1, 0, array, 0, this.int_9);
				return new Class6.Class10(array);
			}
			public Class6.Class10 method_2()
			{
				byte[] array = new byte[this.int_10];
				Array.Copy(this.byte_1, this.int_9, array, 0, this.int_10);
				return new Class6.Class10(array);
			}
		}
		internal class Class12
		{
			private const int int_0 = 4;
			private const int int_1 = 8;
			private const int int_2 = 16;
			private const int int_3 = 20;
			private const int int_4 = 28;
			private const int int_5 = 30;
			private int int_6 = 16;
			private long long_0;
			private Class6.Class16 class16_0;
			private Class6.Class15 class15_0;
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
					return this.int_6 == 30 && this.class16_0.IsFlushed;
				}
			}
			public bool IsNeedingInput
			{
				get
				{
					return this.class15_0.method_8();
				}
			}
			public Class12()
			{
				this.class16_0 = new Class6.Class16();
				this.class15_0 = new Class6.Class15(this.class16_0);
			}
			public void method_0()
			{
				this.int_6 |= 12;
			}
			public void method_1(byte[] byte_0)
			{
				this.class15_0.method_7(byte_0);
			}
			public int method_2(byte[] byte_0)
			{
				int num = 0;
				int num2 = byte_0.Length;
				int num3 = num2;
				while (true)
				{
					int num4 = this.class16_0.method_4(byte_0, num, num2);
					num += num4;
					this.long_0 += (long)num4;
					num2 -= num4;
					if (num2 == 0 || this.int_6 == 30)
					{
						goto IL_E3;
					}
					if (!this.class15_0.method_6((this.int_6 & 4) != 0, (this.int_6 & 8) != 0))
					{
						if (this.int_6 == 16)
						{
							break;
						}
						if (this.int_6 == 20)
						{
							for (int i = 8 + (-this.class16_0.BitCount & 7); i > 0; i -= 10)
							{
								this.class16_0.method_3(2, 10);
							}
							this.int_6 = 16;
						}
						else
						{
							if (this.int_6 == 28)
							{
								this.class16_0.method_2();
								this.int_6 = 30;
							}
						}
					}
				}
				return num3 - num2;
				IL_E3:
				return num3 - num2;
			}
		}
		internal class Class13
		{
			public class Class14
			{
				public short[] short_0;
				public byte[] byte_0;
				public int int_0;
				public int int_1;
				private short[] short_1;
				private int[] int_2;
				private int int_3;
				private Class6.Class13 class13_0;
				public Class14(Class6.Class13 dh, int elems, int minCodes, int maxLength)
				{
					this.class13_0 = dh;
					this.int_0 = minCodes;
					this.int_3 = maxLength;
					this.short_0 = new short[elems];
					this.int_2 = new int[maxLength];
				}
				public void method_0(int int_4)
				{
					this.class13_0.class16_0.method_3((int)this.short_1[int_4] & 65535, (int)this.byte_0[int_4]);
				}
				public void method_1(short[] short_2, byte[] byte_1)
				{
					this.short_1 = short_2;
					this.byte_0 = byte_1;
				}
				public void method_2()
				{
					int[] array = new int[this.int_3];
					int num = 0;
					this.short_1 = new short[this.short_0.Length];
					for (int i = 0; i < this.int_3; i++)
					{
						array[i] = num;
						num += this.int_2[i] << 15 - i;
					}
					for (int j = 0; j < this.int_1; j++)
					{
						int num2 = (int)this.byte_0[j];
						if (num2 > 0)
						{
							this.short_1[j] = Class6.Class13.smethod_0(array[num2 - 1]);
							array[num2 - 1] += 1 << 16 - num2;
						}
					}
				}
				private void method_3(int[] int_4)
				{
					this.byte_0 = new byte[this.short_0.Length];
					int num = int_4.Length / 2;
					int num2 = (num + 1) / 2;
					int num3 = 0;
					for (int i = 0; i < this.int_3; i++)
					{
						this.int_2[i] = 0;
					}
					int[] array = new int[num];
					array[num - 1] = 0;
					for (int j = num - 1; j >= 0; j--)
					{
						if (int_4[2 * j + 1] != -1)
						{
							int num4 = array[j] + 1;
							if (num4 > this.int_3)
							{
								num4 = this.int_3;
								num3++;
							}
							array[int_4[2 * j]] = (array[int_4[2 * j + 1]] = num4);
						}
						else
						{
							int num5 = array[j];
							this.int_2[num5 - 1]++;
							this.byte_0[int_4[2 * j]] = (byte)array[j];
						}
					}
					if (num3 == 0)
					{
						return;
					}
					int num6 = this.int_3 - 1;
					while (true)
					{
						if (this.int_2[--num6] != 0)
						{
							do
							{
								this.int_2[num6]--;
								this.int_2[++num6]++;
								num3 -= 1 << this.int_3 - 1 - num6;
							}
							while (num3 > 0 && num6 < this.int_3 - 1);
							if (num3 <= 0)
							{
								break;
							}
						}
					}
					this.int_2[this.int_3 - 1] += num3;
					this.int_2[this.int_3 - 2] -= num3;
					int num7 = 2 * num2;
					for (int num8 = this.int_3; num8 != 0; num8--)
					{
						int k = this.int_2[num8 - 1];
						while (k > 0)
						{
							int num9 = 2 * int_4[num7++];
							if (int_4[num9 + 1] == -1)
							{
								this.byte_0[int_4[num9]] = (byte)num8;
								k--;
							}
						}
					}
				}
				public void method_4()
				{
					int num = this.short_0.Length;
					int[] array = new int[num];
					int i = 0;
					int num2 = 0;
					for (int j = 0; j < num; j++)
					{
						int num3 = (int)this.short_0[j];
						if (num3 != 0)
						{
							int num4 = i++;
							int num5;
							while (num4 > 0 && (int)this.short_0[array[num5 = (num4 - 1) / 2]] > num3)
							{
								array[num4] = array[num5];
								num4 = num5;
							}
							array[num4] = j;
							num2 = j;
						}
					}
					while (i < 2)
					{
						int num6 = (num2 < 2) ? (++num2) : 0;
						array[i++] = num6;
					}
					this.int_1 = Math.Max(num2 + 1, this.int_0);
					int num7 = i;
					int[] array2 = new int[4 * i - 2];
					int[] array3 = new int[2 * i - 1];
					int num8 = num7;
					for (int k = 0; k < i; k++)
					{
						int num9 = array[k];
						array2[2 * k] = num9;
						array2[2 * k + 1] = -1;
						array3[k] = (int)this.short_0[num9] << 8;
						array[k] = k;
					}
					do
					{
						int num10 = array[0];
						int num11 = array[--i];
						int num12 = 0;
						int l;
						for (l = 1; l < i; l = l * 2 + 1)
						{
							if (l + 1 < i && array3[array[l]] > array3[array[l + 1]])
							{
								l++;
							}
							array[num12] = array[l];
							num12 = l;
						}
						int num13 = array3[num11];
						while ((l = num12) > 0 && array3[array[num12 = (l - 1) / 2]] > num13)
						{
							array[l] = array[num12];
						}
						array[l] = num11;
						int num14 = array[0];
						num11 = num8++;
						array2[2 * num11] = num10;
						array2[2 * num11 + 1] = num14;
						int num15 = Math.Min(array3[num10] & 255, array3[num14] & 255);
						num13 = (array3[num11] = array3[num10] + array3[num14] - num15 + 1);
						num12 = 0;
						for (l = 1; l < i; l = num12 * 2 + 1)
						{
							if (l + 1 < i && array3[array[l]] > array3[array[l + 1]])
							{
								l++;
							}
							array[num12] = array[l];
							num12 = l;
						}
						while ((l = num12) > 0 && array3[array[num12 = (l - 1) / 2]] > num13)
						{
							array[l] = array[num12];
						}
						array[l] = num11;
					}
					while (i > 1);
					this.method_3(array2);
				}
				public int method_5()
				{
					int num = 0;
					for (int i = 0; i < this.short_0.Length; i++)
					{
						num += (int)(this.short_0[i] * (short)this.byte_0[i]);
					}
					return num;
				}
				public void method_6(Class6.Class13.Class14 class14_0)
				{
					int num = -1;
					int i = 0;
					while (i < this.int_1)
					{
						int num2 = 1;
						int num3 = (int)this.byte_0[i];
						int num4;
						int num5;
						if (num3 == 0)
						{
							num4 = 138;
							num5 = 3;
						}
						else
						{
							num4 = 6;
							num5 = 3;
							if (num != num3)
							{
								short[] expr_3B_cp_0 = class14_0.short_0;
								int expr_3B_cp_1 = num3;
								expr_3B_cp_0[expr_3B_cp_1] += 1;
								num2 = 0;
							}
						}
						num = num3;
						i++;
						while (i < this.int_1)
						{
							if (num != (int)this.byte_0[i])
							{
								break;
							}
							i++;
							if (++num2 >= num4)
							{
								break;
							}
						}
						if (num2 < num5)
						{
							short[] expr_8C_cp_0 = class14_0.short_0;
							int expr_8C_cp_1 = num;
							expr_8C_cp_0[expr_8C_cp_1] += (short)num2;
						}
						else
						{
							if (num != 0)
							{
								short[] expr_AD_cp_0 = class14_0.short_0;
								int expr_AD_cp_1 = 16;
								expr_AD_cp_0[expr_AD_cp_1] += 1;
							}
							else
							{
								if (num2 <= 10)
								{
									short[] expr_CF_cp_0 = class14_0.short_0;
									int expr_CF_cp_1 = 17;
									expr_CF_cp_0[expr_CF_cp_1] += 1;
								}
								else
								{
									short[] expr_EC_cp_0 = class14_0.short_0;
									int expr_EC_cp_1 = 18;
									expr_EC_cp_0[expr_EC_cp_1] += 1;
								}
							}
						}
					}
				}
				public void method_7(Class6.Class13.Class14 class14_0)
				{
					int num = -1;
					int i = 0;
					while (i < this.int_1)
					{
						int num2 = 1;
						int num3 = (int)this.byte_0[i];
						int num4;
						int num5;
						if (num3 == 0)
						{
							num4 = 138;
							num5 = 3;
						}
						else
						{
							num4 = 6;
							num5 = 3;
							if (num != num3)
							{
								class14_0.method_0(num3);
								num2 = 0;
							}
						}
						num = num3;
						i++;
						while (i < this.int_1)
						{
							if (num != (int)this.byte_0[i])
							{
								break;
							}
							i++;
							if (++num2 >= num4)
							{
								break;
							}
						}
						if (num2 < num5)
						{
							while (num2-- > 0)
							{
								class14_0.method_0(num);
							}
						}
						else
						{
							if (num != 0)
							{
								class14_0.method_0(16);
								this.class13_0.class16_0.method_3(num2 - 3, 2);
							}
							else
							{
								if (num2 <= 10)
								{
									class14_0.method_0(17);
									this.class13_0.class16_0.method_3(num2 - 3, 3);
								}
								else
								{
									class14_0.method_0(18);
									this.class13_0.class16_0.method_3(num2 - 11, 7);
								}
							}
						}
					}
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
			private static readonly int[] int_8;
			private static readonly byte[] byte_0;
			private Class6.Class16 class16_0;
			private Class6.Class13.Class14 class14_0;
			private Class6.Class13.Class14 class14_1;
			private Class6.Class13.Class14 class14_2;
			private short[] short_0;
			private byte[] byte_1;
			private int int_9;
			private int int_10;
			private static readonly short[] short_1;
			private static readonly byte[] byte_2;
			private static readonly short[] short_2;
			private static readonly byte[] byte_3;
			public static short smethod_0(int int_11)
			{
				return (short)((int)Class6.Class13.byte_0[int_11 & 15] << 12 | (int)Class6.Class13.byte_0[int_11 >> 4 & 15] << 8 | (int)Class6.Class13.byte_0[int_11 >> 8 & 15] << 4 | (int)Class6.Class13.byte_0[int_11 >> 12]);
			}
			static Class13()
			{
				Class6.Class13.int_8 = new int[]
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
				Class6.Class13.byte_0 = new byte[]
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
				Class6.Class13.short_1 = new short[286];
				Class6.Class13.byte_2 = new byte[286];
				int i = 0;
				while (i < 144)
				{
					Class6.Class13.short_1[i] = Class6.Class13.smethod_0(48 + i << 8);
					Class6.Class13.byte_2[i++] = 8;
				}
				while (i < 256)
				{
					Class6.Class13.short_1[i] = Class6.Class13.smethod_0(256 + i << 7);
					Class6.Class13.byte_2[i++] = 9;
				}
				while (i < 280)
				{
					Class6.Class13.short_1[i] = Class6.Class13.smethod_0(-256 + i << 9);
					Class6.Class13.byte_2[i++] = 7;
				}
				while (i < 286)
				{
					Class6.Class13.short_1[i] = Class6.Class13.smethod_0(-88 + i << 8);
					Class6.Class13.byte_2[i++] = 8;
				}
				Class6.Class13.short_2 = new short[30];
				Class6.Class13.byte_3 = new byte[30];
				for (i = 0; i < 30; i++)
				{
					Class6.Class13.short_2[i] = Class6.Class13.smethod_0(i << 11);
					Class6.Class13.byte_3[i] = 5;
				}
			}
			public Class13(Class6.Class16 pending)
			{
				this.class16_0 = pending;
				this.class14_0 = new Class6.Class13.Class14(this, 286, 257, 15);
				this.class14_1 = new Class6.Class13.Class14(this, 30, 1, 15);
				this.class14_2 = new Class6.Class13.Class14(this, 19, 4, 7);
				this.short_0 = new short[16384];
				this.byte_1 = new byte[16384];
			}
			public void method_0()
			{
				this.int_9 = 0;
				this.int_10 = 0;
			}
			private int method_1(int int_11)
			{
				if (int_11 == 255)
				{
					return 285;
				}
				int num = 257;
				while (int_11 >= 8)
				{
					num += 4;
					int_11 >>= 1;
				}
				return num + int_11;
			}
			private int method_2(int int_11)
			{
				int num = 0;
				while (int_11 >= 4)
				{
					num += 2;
					int_11 >>= 1;
				}
				return num + int_11;
			}
			public void method_3(int int_11)
			{
				this.class14_2.method_2();
				this.class14_0.method_2();
				this.class14_1.method_2();
				this.class16_0.method_3(this.class14_0.int_1 - 257, 5);
				this.class16_0.method_3(this.class14_1.int_1 - 1, 5);
				this.class16_0.method_3(int_11 - 4, 4);
				for (int i = 0; i < int_11; i++)
				{
					this.class16_0.method_3((int)this.class14_2.byte_0[Class6.Class13.int_8[i]], 3);
				}
				this.class14_0.method_7(this.class14_2);
				this.class14_1.method_7(this.class14_2);
			}
			public void method_4()
			{
				for (int i = 0; i < this.int_9; i++)
				{
					int num = (int)(this.byte_1[i] & 255);
					int num2 = (int)this.short_0[i];
					if (num2-- != 0)
					{
						int num3 = this.method_1(num);
						this.class14_0.method_0(num3);
						int num4 = (num3 - 261) / 4;
						if (num4 > 0 && num4 <= 5)
						{
							this.class16_0.method_3(num & (1 << num4) - 1, num4);
						}
						int num5 = this.method_2(num2);
						this.class14_1.method_0(num5);
						num4 = num5 / 2 - 1;
						if (num4 > 0)
						{
							this.class16_0.method_3(num2 & (1 << num4) - 1, num4);
						}
					}
					else
					{
						this.class14_0.method_0(num);
					}
				}
				this.class14_0.method_0(256);
			}
			public void method_5(byte[] byte_4, int int_11, int int_12, bool bool_0)
			{
				this.class16_0.method_3(bool_0 ? 1 : 0, 3);
				this.class16_0.method_2();
				this.class16_0.method_0(int_12);
				this.class16_0.method_0(~int_12);
				this.class16_0.method_1(byte_4, int_11, int_12);
				this.method_0();
			}
			public void method_6(byte[] byte_4, int int_11, int int_12, bool bool_0)
			{
				short[] expr_15_cp_0 = this.class14_0.short_0;
				int expr_15_cp_1 = 256;
				expr_15_cp_0[expr_15_cp_1] += 1;
				this.class14_0.method_4();
				this.class14_1.method_4();
				this.class14_0.method_6(this.class14_2);
				this.class14_1.method_6(this.class14_2);
				this.class14_2.method_4();
				int num = 4;
				for (int i = 18; i > num; i--)
				{
					if (this.class14_2.byte_0[Class6.Class13.int_8[i]] > 0)
					{
						num = i + 1;
					}
				}
				int num2 = 14 + num * 3 + this.class14_2.method_5() + this.class14_0.method_5() + this.class14_1.method_5() + this.int_10;
				int num3 = this.int_10;
				for (int j = 0; j < 286; j++)
				{
					num3 += (int)(this.class14_0.short_0[j] * (short)Class6.Class13.byte_2[j]);
				}
				for (int k = 0; k < 30; k++)
				{
					num3 += (int)(this.class14_1.short_0[k] * (short)Class6.Class13.byte_3[k]);
				}
				if (num2 >= num3)
				{
					num2 = num3;
				}
				if (int_11 >= 0 && int_12 + 4 < num2 >> 3)
				{
					this.method_5(byte_4, int_11, int_12, bool_0);
					return;
				}
				if (num2 == num3)
				{
					this.class16_0.method_3(2 + (bool_0 ? 1 : 0), 3);
					this.class14_0.method_1(Class6.Class13.short_1, Class6.Class13.byte_2);
					this.class14_1.method_1(Class6.Class13.short_2, Class6.Class13.byte_3);
					this.method_4();
					this.method_0();
					return;
				}
				this.class16_0.method_3(4 + (bool_0 ? 1 : 0), 3);
				this.method_3(num);
				this.method_4();
				this.method_0();
			}
			public bool method_7()
			{
				return this.int_9 >= 16384;
			}
			public bool method_8(int int_11)
			{
				this.short_0[this.int_9] = 0;
				this.byte_1[this.int_9++] = (byte)int_11;
				short[] expr_39_cp_0 = this.class14_0.short_0;
				expr_39_cp_0[int_11] += 1;
				return this.method_7();
			}
			public bool method_9(int int_11, int int_12)
			{
				this.short_0[this.int_9] = (short)int_11;
				this.byte_1[this.int_9++] = (byte)(int_12 - 3);
				int num = this.method_1(int_12 - 3);
				short[] expr_46_cp_0 = this.class14_0.short_0;
				int expr_46_cp_1 = num;
				expr_46_cp_0[expr_46_cp_1] += 1;
				if (num >= 265 && num < 285)
				{
					this.int_10 += (num - 261) / 4;
				}
				int num2 = this.method_2(int_11 - 1);
				short[] expr_95_cp_0 = this.class14_1.short_0;
				int expr_95_cp_1 = num2;
				expr_95_cp_0[expr_95_cp_1] += 1;
				if (num2 >= 4)
				{
					this.int_10 += num2 / 2 - 1;
				}
				return this.method_7();
			}
		}
		internal class Class15
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
			private int int_10;
			private short[] short_0;
			private short[] short_1;
			private int int_11;
			private int int_12;
			private bool bool_0;
			private int int_13;
			private int int_14;
			private int int_15;
			private byte[] byte_0;
			private byte[] byte_1;
			private int int_16;
			private int int_17;
			private int int_18;
			private Class6.Class16 class16_0;
			private Class6.Class13 class13_0;
			public Class15(Class6.Class16 pending)
			{
				this.class16_0 = pending;
				this.class13_0 = new Class6.Class13(pending);
				this.byte_0 = new byte[65536];
				this.short_0 = new short[32768];
				this.short_1 = new short[32768];
				this.int_14 = 1;
				this.int_13 = 1;
			}
			private void method_0()
			{
				this.int_10 = ((int)this.byte_0[this.int_14] << 5 ^ (int)this.byte_0[this.int_14 + 1]);
			}
			private int method_1()
			{
				int num = (this.int_10 << 5 ^ (int)this.byte_0[this.int_14 + 2]) & 32767;
				short num2 = this.short_1[this.int_14 & 32767] = this.short_0[num];
				this.short_0[num] = (short)this.int_14;
				this.int_10 = num;
				return (int)num2 & 65535;
			}
			private void method_2()
			{
				Array.Copy(this.byte_0, 32768, this.byte_0, 0, 32768);
				this.int_11 -= 32768;
				this.int_14 -= 32768;
				this.int_13 -= 32768;
				for (int i = 0; i < 32768; i++)
				{
					int num = (int)this.short_0[i] & 65535;
					this.short_0[i] = (short)((num >= 32768) ? (num - 32768) : 0);
				}
				for (int j = 0; j < 32768; j++)
				{
					int num2 = (int)this.short_1[j] & 65535;
					this.short_1[j] = (short)((num2 >= 32768) ? (num2 - 32768) : 0);
				}
			}
			public void method_3()
			{
				if (this.int_14 >= 65274)
				{
					this.method_2();
				}
				while (this.int_15 < 262 && this.int_17 < this.int_18)
				{
					int num = 65536 - this.int_15 - this.int_14;
					if (num > this.int_18 - this.int_17)
					{
						num = this.int_18 - this.int_17;
					}
					Array.Copy(this.byte_1, this.int_17, this.byte_0, this.int_14 + this.int_15, num);
					this.int_17 += num;
					this.int_16 += num;
					this.int_15 += num;
				}
				if (this.int_15 >= 3)
				{
					this.method_0();
				}
			}
			private bool method_4(int int_19)
			{
				int num = 128;
				int num2 = 128;
				short[] array = this.short_1;
				int num3 = this.int_14;
				int num4 = this.int_14 + this.int_12;
				int num5 = Math.Max(this.int_12, 2);
				int num6 = Math.Max(this.int_14 - 32506, 0);
				int num7 = this.int_14 + 258 - 1;
				byte b = this.byte_0[num4 - 1];
				byte b2 = this.byte_0[num4];
				if (num5 >= 8)
				{
					num >>= 2;
				}
				if (num2 > this.int_15)
				{
					num2 = this.int_15;
				}
				do
				{
					if (this.byte_0[int_19 + num5] == b2 && this.byte_0[int_19 + num5 - 1] == b && this.byte_0[int_19] == this.byte_0[num3] && this.byte_0[int_19 + 1] == this.byte_0[num3 + 1])
					{
						int num8 = int_19 + 2;
						num3 += 2;
						while (this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && this.byte_0[++num3] == this.byte_0[++num8] && num3 < num7)
						{
						}
						if (num3 > num4)
						{
							this.int_11 = int_19;
							num4 = num3;
							num5 = num3 - this.int_14;
							if (num5 >= num2)
							{
								break;
							}
							b = this.byte_0[num4 - 1];
							b2 = this.byte_0[num4];
						}
						num3 = this.int_14;
					}
					if ((int_19 = ((int)array[int_19 & 32767] & 65535)) <= num6)
					{
						break;
					}
				}
				while (--num != 0);
				this.int_12 = Math.Min(num5, this.int_15);
				return this.int_12 >= 3;
			}
			private bool method_5(bool bool_1, bool bool_2)
			{
				if (this.int_15 < 262 && !bool_1)
				{
					return false;
				}
				while (this.int_15 >= 262 || bool_1)
				{
					if (this.int_15 == 0)
					{
						if (this.bool_0)
						{
							this.class13_0.method_8((int)(this.byte_0[this.int_14 - 1] & 255));
						}
						this.bool_0 = false;
						this.class13_0.method_6(this.byte_0, this.int_13, this.int_14 - this.int_13, bool_2);
						this.int_13 = this.int_14;
						return false;
					}
					if (this.int_14 >= 65274)
					{
						this.method_2();
					}
					int num = this.int_11;
					int num2 = this.int_12;
					if (this.int_15 >= 3)
					{
						int num3 = this.method_1();
						if (num3 != 0 && this.int_14 - num3 <= 32506 && this.method_4(num3) && this.int_12 <= 5 && this.int_12 == 3 && this.int_14 - this.int_11 > 4096)
						{
							this.int_12 = 2;
						}
					}
					if (num2 >= 3 && this.int_12 <= num2)
					{
						this.class13_0.method_9(this.int_14 - 1 - num, num2);
						num2 -= 2;
						do
						{
							this.int_14++;
							this.int_15--;
							if (this.int_15 >= 3)
							{
								this.method_1();
							}
						}
						while (--num2 > 0);
						this.int_14++;
						this.int_15--;
						this.bool_0 = false;
						this.int_12 = 2;
					}
					else
					{
						if (this.bool_0)
						{
							this.class13_0.method_8((int)(this.byte_0[this.int_14 - 1] & 255));
						}
						this.bool_0 = true;
						this.int_14++;
						this.int_15--;
					}
					if (this.class13_0.method_7())
					{
						int num4 = this.int_14 - this.int_13;
						if (this.bool_0)
						{
							num4--;
						}
						bool flag = bool_2 && this.int_15 == 0 && !this.bool_0;
						this.class13_0.method_6(this.byte_0, this.int_13, num4, flag);
						this.int_13 += num4;
						return !flag;
					}
				}
				return true;
			}
			public bool method_6(bool bool_1, bool bool_2)
			{
				bool flag;
				do
				{
					this.method_3();
					bool bool_3 = bool_1 && this.int_17 == this.int_18;
					flag = this.method_5(bool_3, bool_2);
					if (!this.class16_0.IsFlushed)
					{
						break;
					}
				}
				while (flag);
				return flag;
			}
			public void method_7(byte[] byte_2)
			{
				this.byte_1 = byte_2;
				this.int_17 = 0;
				this.int_18 = byte_2.Length;
			}
			public bool method_8()
			{
				return this.int_18 == this.int_17;
			}
		}
		internal class Class16
		{
			protected byte[] byte_0 = new byte[65536];
			private int int_0;
			private int int_1;
			private uint uint_0;
			private int int_2;
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
			public void method_0(int int_3)
			{
				this.byte_0[this.int_1++] = (byte)int_3;
				this.byte_0[this.int_1++] = (byte)(int_3 >> 8);
			}
			public void method_1(byte[] byte_1, int int_3, int int_4)
			{
				Array.Copy(byte_1, int_3, this.byte_0, this.int_1, int_4);
				this.int_1 += int_4;
			}
			public void method_2()
			{
				if (this.int_2 > 0)
				{
					this.byte_0[this.int_1++] = (byte)this.uint_0;
					if (this.int_2 > 8)
					{
						this.byte_0[this.int_1++] = (byte)(this.uint_0 >> 8);
					}
				}
				this.uint_0 = 0u;
				this.int_2 = 0;
			}
			public void method_3(int int_3, int int_4)
			{
				this.uint_0 |= (uint)((uint)int_3 << this.int_2);
				this.int_2 += int_4;
				if (this.int_2 >= 16)
				{
					this.byte_0[this.int_1++] = (byte)this.uint_0;
					this.byte_0[this.int_1++] = (byte)(this.uint_0 >> 8);
					this.uint_0 >>= 16;
					this.int_2 -= 16;
				}
			}
			public int method_4(byte[] byte_1, int int_3, int int_4)
			{
				if (this.int_2 >= 8)
				{
					this.byte_0[this.int_1++] = (byte)this.uint_0;
					this.uint_0 >>= 8;
					this.int_2 -= 8;
				}
				if (int_4 > this.int_1 - this.int_0)
				{
					int_4 = this.int_1 - this.int_0;
					Array.Copy(this.byte_0, this.int_0, byte_1, int_3, int_4);
					this.int_0 = 0;
					this.int_1 = 0;
				}
				else
				{
					Array.Copy(this.byte_0, this.int_0, byte_1, int_3, int_4);
					this.int_0 += int_4;
				}
				return int_4;
			}
		}
		internal class Stream0 : MemoryStream
		{
			public void method_0(int int_0)
			{
				this.WriteByte((byte)(int_0 & 255));
				this.WriteByte((byte)(int_0 >> 8 & 255));
			}
			public void method_1(int int_0)
			{
				this.method_0(int_0);
				this.method_0(int_0 >> 16);
			}
			public int method_2()
			{
				return this.ReadByte() | this.ReadByte() << 8;
			}
			public int method_3()
			{
				return this.method_2() | this.method_2() << 16;
			}
			public Stream0()
			{
			}
			public Stream0(byte[] buffer) : base(buffer, false)
			{
			}
		}
		public static string string_0;
		private static bool smethod_0(Assembly assembly_0, Assembly assembly_1)
		{
			byte[] publicKey = assembly_0.GetName().GetPublicKey();
			byte[] publicKey2 = assembly_1.GetName().GetPublicKey();
			if (publicKey2 == null != (publicKey == null))
			{
				return false;
			}
			if (publicKey2 != null)
			{
				for (int i = 0; i < publicKey2.Length; i++)
				{
					if (publicKey2[i] != publicKey[i])
					{
						return false;
					}
				}
			}
			return true;
		}
		private static ICryptoTransform smethod_1(byte[] byte_0, byte[] byte_1, bool bool_0)
		{
			ICryptoTransform result;
			using (SymmetricAlgorithm symmetricAlgorithm = new RijndaelManaged())
			{
				result = (bool_0 ? symmetricAlgorithm.CreateDecryptor(byte_0, byte_1) : symmetricAlgorithm.CreateEncryptor(byte_0, byte_1));
			}
			return result;
		}
		private static ICryptoTransform smethod_2(byte[] byte_0, byte[] byte_1, bool bool_0)
		{
			ICryptoTransform result;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				result = (bool_0 ? dESCryptoServiceProvider.CreateDecryptor(byte_0, byte_1) : dESCryptoServiceProvider.CreateEncryptor(byte_0, byte_1));
			}
			return result;
		}
		public static byte[] smethod_3(byte[] byte_0)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (callingAssembly != executingAssembly && !Class6.smethod_0(executingAssembly, callingAssembly))
			{
				return null;
			}
			Class6.Stream0 stream = new Class6.Stream0(byte_0);
			byte[] array = new byte[0];
			int num = stream.method_3();
			if (num == 67324752)
			{
				short num2 = (short)stream.method_2();
				int num3 = stream.method_2();
				int num4 = stream.method_2();
				if (num == 67324752 && num2 == 20 && num3 == 0)
				{
					if (num4 == 8)
					{
						stream.method_3();
						stream.method_3();
						stream.method_3();
						int num5 = stream.method_3();
						int num6 = stream.method_2();
						int num7 = stream.method_2();
						if (num6 > 0)
						{
							byte[] buffer = new byte[num6];
							stream.Read(buffer, 0, num6);
						}
						if (num7 > 0)
						{
							byte[] buffer2 = new byte[num7];
							stream.Read(buffer2, 0, num7);
						}
						byte[] array2 = new byte[stream.Length - stream.Position];
						stream.Read(array2, 0, array2.Length);
						Class6.Class7 @class = new Class6.Class7(array2);
						array = new byte[num5];
						@class.method_2(array, 0, array.Length);
						goto IL_279;
					}
				}
				throw new FormatException("Wrong Header Signature");
			}
			int num8 = num >> 24;
			num -= num8 << 24;
			if (num == 8223355)
			{
				if (num8 == 1)
				{
					int num9 = stream.method_3();
					array = new byte[num9];
					int num11;
					for (int i = 0; i < num9; i += num11)
					{
						int num10 = stream.method_3();
						num11 = stream.method_3();
						byte[] array3 = new byte[num10];
						stream.Read(array3, 0, array3.Length);
						Class6.Class7 class2 = new Class6.Class7(array3);
						class2.method_2(array, i, num11);
					}
				}
				if (num8 == 2)
				{
					byte[] byte_ = new byte[]
					{
						157,
						222,
						104,
						52,
						113,
						83,
						218,
						165
					};
					byte[] byte_2 = new byte[]
					{
						3,
						114,
						130,
						12,
						179,
						169,
						217,
						238
					};
					using (ICryptoTransform cryptoTransform = Class6.smethod_2(byte_, byte_2, true))
					{
						byte[] byte_3 = cryptoTransform.TransformFinalBlock(byte_0, 4, byte_0.Length - 4);
						array = Class6.smethod_3(byte_3);
					}
				}
				if (num8 != 3)
				{
					goto IL_279;
				}
				byte[] byte_4 = new byte[]
				{
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1
				};
				byte[] byte_5 = new byte[]
				{
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2
				};
				using (ICryptoTransform cryptoTransform2 = Class6.smethod_1(byte_4, byte_5, true))
				{
					byte[] byte_6 = cryptoTransform2.TransformFinalBlock(byte_0, 4, byte_0.Length - 4);
					array = Class6.smethod_3(byte_6);
					goto IL_279;
				}
			}
			throw new FormatException("Unknown Header");
			IL_279:
			stream.Close();
			stream = null;
			return array;
		}
		public static byte[] smethod_4(byte[] byte_0)
		{
			return Class6.smethod_7(byte_0, 1, null, null);
		}
		public static byte[] smethod_5(byte[] byte_0, byte[] byte_1, byte[] byte_2)
		{
			return Class6.smethod_7(byte_0, 2, byte_1, byte_2);
		}
		public static byte[] smethod_6(byte[] byte_0, byte[] byte_1, byte[] byte_2)
		{
			return Class6.smethod_7(byte_0, 3, byte_1, byte_2);
		}
		private static byte[] smethod_7(byte[] byte_0, int int_0, byte[] byte_1, byte[] byte_2)
		{
			byte[] result;
			try
			{
				Class6.Stream0 stream = new Class6.Stream0();
				if (int_0 == 0)
				{
					Class6.Class12 @class = new Class6.Class12();
					DateTime now = DateTime.Now;
					long num = (long)((ulong)((now.Year - 1980 & 127) << 25 | now.Month << 21 | now.Day << 16 | now.Hour << 11 | now.Minute << 5 | (int)((uint)now.Second >> 1)));
					uint[] array = new uint[]
					{
						0u,
						1996959894u,
						3993919788u,
						2567524794u,
						124634137u,
						1886057615u,
						3915621685u,
						2657392035u,
						249268274u,
						2044508324u,
						3772115230u,
						2547177864u,
						162941995u,
						2125561021u,
						3887607047u,
						2428444049u,
						498536548u,
						1789927666u,
						4089016648u,
						2227061214u,
						450548861u,
						1843258603u,
						4107580753u,
						2211677639u,
						325883990u,
						1684777152u,
						4251122042u,
						2321926636u,
						335633487u,
						1661365465u,
						4195302755u,
						2366115317u,
						997073096u,
						1281953886u,
						3579855332u,
						2724688242u,
						1006888145u,
						1258607687u,
						3524101629u,
						2768942443u,
						901097722u,
						1119000684u,
						3686517206u,
						2898065728u,
						853044451u,
						1172266101u,
						3705015759u,
						2882616665u,
						651767980u,
						1373503546u,
						3369554304u,
						3218104598u,
						565507253u,
						1454621731u,
						3485111705u,
						3099436303u,
						671266974u,
						1594198024u,
						3322730930u,
						2970347812u,
						795835527u,
						1483230225u,
						3244367275u,
						3060149565u,
						1994146192u,
						31158534u,
						2563907772u,
						4023717930u,
						1907459465u,
						112637215u,
						2680153253u,
						3904427059u,
						2013776290u,
						251722036u,
						2517215374u,
						3775830040u,
						2137656763u,
						141376813u,
						2439277719u,
						3865271297u,
						1802195444u,
						476864866u,
						2238001368u,
						4066508878u,
						1812370925u,
						453092731u,
						2181625025u,
						4111451223u,
						1706088902u,
						314042704u,
						2344532202u,
						4240017532u,
						1658658271u,
						366619977u,
						2362670323u,
						4224994405u,
						1303535960u,
						984961486u,
						2747007092u,
						3569037538u,
						1256170817u,
						1037604311u,
						2765210733u,
						3554079995u,
						1131014506u,
						879679996u,
						2909243462u,
						3663771856u,
						1141124467u,
						855842277u,
						2852801631u,
						3708648649u,
						1342533948u,
						654459306u,
						3188396048u,
						3373015174u,
						1466479909u,
						544179635u,
						3110523913u,
						3462522015u,
						1591671054u,
						702138776u,
						2966460450u,
						3352799412u,
						1504918807u,
						783551873u,
						3082640443u,
						3233442989u,
						3988292384u,
						2596254646u,
						62317068u,
						1957810842u,
						3939845945u,
						2647816111u,
						81470997u,
						1943803523u,
						3814918930u,
						2489596804u,
						225274430u,
						2053790376u,
						3826175755u,
						2466906013u,
						167816743u,
						2097651377u,
						4027552580u,
						2265490386u,
						503444072u,
						1762050814u,
						4150417245u,
						2154129355u,
						426522225u,
						1852507879u,
						4275313526u,
						2312317920u,
						282753626u,
						1742555852u,
						4189708143u,
						2394877945u,
						397917763u,
						1622183637u,
						3604390888u,
						2714866558u,
						953729732u,
						1340076626u,
						3518719985u,
						2797360999u,
						1068828381u,
						1219638859u,
						3624741850u,
						2936675148u,
						906185462u,
						1090812512u,
						3747672003u,
						2825379669u,
						829329135u,
						1181335161u,
						3412177804u,
						3160834842u,
						628085408u,
						1382605366u,
						3423369109u,
						3138078467u,
						570562233u,
						1426400815u,
						3317316542u,
						2998733608u,
						733239954u,
						1555261956u,
						3268935591u,
						3050360625u,
						752459403u,
						1541320221u,
						2607071920u,
						3965973030u,
						1969922972u,
						40735498u,
						2617837225u,
						3943577151u,
						1913087877u,
						83908371u,
						2512341634u,
						3803740692u,
						2075208622u,
						213261112u,
						2463272603u,
						3855990285u,
						2094854071u,
						198958881u,
						2262029012u,
						4057260610u,
						1759359992u,
						534414190u,
						2176718541u,
						4139329115u,
						1873836001u,
						414664567u,
						2282248934u,
						4279200368u,
						1711684554u,
						285281116u,
						2405801727u,
						4167216745u,
						1634467795u,
						376229701u,
						2685067896u,
						3608007406u,
						1308918612u,
						956543938u,
						2808555105u,
						3495958263u,
						1231636301u,
						1047427035u,
						2932959818u,
						3654703836u,
						1088359270u,
						936918000u,
						2847714899u,
						3736837829u,
						1202900863u,
						817233897u,
						3183342108u,
						3401237130u,
						1404277552u,
						615818150u,
						3134207493u,
						3453421203u,
						1423857449u,
						601450431u,
						3009837614u,
						3294710456u,
						1567103746u,
						711928724u,
						3020668471u,
						3272380065u,
						1510334235u,
						755167117u
					};
					uint num2 = 4294967295u;
					uint num3 = 4294967295u;
					int num4 = 0;
					int num5 = byte_0.Length;
					while (--num5 >= 0)
					{
						num3 = (array[(int)((UIntPtr)((num3 ^ (uint)byte_0[num4++]) & 255u))] ^ num3 >> 8);
					}
					num3 ^= num2;
					stream.method_1(67324752);
					stream.method_0(20);
					stream.method_0(0);
					stream.method_0(8);
					stream.method_1((int)num);
					stream.method_1((int)num3);
					long position = stream.Position;
					stream.method_1(0);
					stream.method_1(byte_0.Length);
					byte[] bytes = Encoding.UTF8.GetBytes("{data}");
					stream.method_0(bytes.Length);
					stream.method_0(0);
					stream.Write(bytes, 0, bytes.Length);
					@class.method_1(byte_0);
					while (!@class.IsNeedingInput)
					{
						byte[] array2 = new byte[512];
						int num6 = @class.method_2(array2);
						if (num6 <= 0)
						{
							break;
						}
						stream.Write(array2, 0, num6);
					}
					@class.method_0();
					while (!@class.IsFinished)
					{
						byte[] array3 = new byte[512];
						int num7 = @class.method_2(array3);
						if (num7 <= 0)
						{
							break;
						}
						stream.Write(array3, 0, num7);
					}
					long totalOut = @class.TotalOut;
					stream.method_1(33639248);
					stream.method_0(20);
					stream.method_0(20);
					stream.method_0(0);
					stream.method_0(8);
					stream.method_1((int)num);
					stream.method_1((int)num3);
					stream.method_1((int)totalOut);
					stream.method_1(byte_0.Length);
					stream.method_0(bytes.Length);
					stream.method_0(0);
					stream.method_0(0);
					stream.method_0(0);
					stream.method_0(0);
					stream.method_1(0);
					stream.method_1(0);
					stream.Write(bytes, 0, bytes.Length);
					stream.method_1(101010256);
					stream.method_0(0);
					stream.method_0(0);
					stream.method_0(1);
					stream.method_0(1);
					stream.method_1(46 + bytes.Length);
					stream.method_1((int)((long)(30 + bytes.Length) + totalOut));
					stream.method_0(0);
					stream.Seek(position, SeekOrigin.Begin);
					stream.method_1((int)totalOut);
				}
				else
				{
					if (int_0 == 1)
					{
						stream.method_1(25000571);
						stream.method_1(byte_0.Length);
						byte[] array4;
						for (int i = 0; i < byte_0.Length; i += array4.Length)
						{
							array4 = new byte[Math.Min(2097151, byte_0.Length - i)];
							Buffer.BlockCopy(byte_0, i, array4, 0, array4.Length);
							long position2 = stream.Position;
							stream.method_1(0);
							stream.method_1(array4.Length);
							Class6.Class12 class2 = new Class6.Class12();
							class2.method_1(array4);
							while (!class2.IsNeedingInput)
							{
								byte[] array5 = new byte[512];
								int num8 = class2.method_2(array5);
								if (num8 <= 0)
								{
									break;
								}
								stream.Write(array5, 0, num8);
							}
							class2.method_0();
							while (!class2.IsFinished)
							{
								byte[] array6 = new byte[512];
								int num9 = class2.method_2(array6);
								if (num9 <= 0)
								{
									break;
								}
								stream.Write(array6, 0, num9);
							}
							long position3 = stream.Position;
							stream.Position = position2;
							stream.method_1((int)class2.TotalOut);
							stream.Position = position3;
						}
					}
					else
					{
						if (int_0 == 2)
						{
							stream.method_1(41777787);
							byte[] array7 = Class6.smethod_7(byte_0, 1, null, null);
							using (ICryptoTransform cryptoTransform = Class6.smethod_2(byte_1, byte_2, false))
							{
								byte[] array8 = cryptoTransform.TransformFinalBlock(array7, 0, array7.Length);
								stream.Write(array8, 0, array8.Length);
								goto IL_44E;
							}
						}
						if (int_0 == 3)
						{
							stream.method_1(58555003);
							byte[] array9 = Class6.smethod_7(byte_0, 1, null, null);
							using (ICryptoTransform cryptoTransform2 = Class6.smethod_1(byte_1, byte_2, false))
							{
								byte[] array10 = cryptoTransform2.TransformFinalBlock(array9, 0, array9.Length);
								stream.Write(array10, 0, array10.Length);
							}
						}
					}
				}
				IL_44E:
				stream.Flush();
				stream.Close();
				result = stream.ToArray();
			}
			catch (Exception ex)
			{
				Class6.string_0 = "ERR 2003: " + ex.Message;
				throw;
			}
			return result;
		}
	}
}
