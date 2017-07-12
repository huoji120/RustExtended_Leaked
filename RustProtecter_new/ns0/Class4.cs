using System;
using System.IO.Compression.Zlib;

namespace ns0
{
	internal sealed class Class4
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

		internal int int_10;

		internal int int_11;

		internal int[] int_12;

		internal int int_13;

		internal int int_14;

		internal int int_15;

		internal int int_16;

		internal int int_17;

		internal byte byte_0;

		internal byte byte_1;

		internal int[] int_18;

		internal int int_19;

		internal int[] int_20;

		internal int int_21;

		internal Class4()
		{
		}

		internal void method_0(int int_22, int int_23, int[] int_24, int int_25, int[] int_26, int int_27)
		{
			this.int_10 = 0;
			this.byte_0 = (byte)int_22;
			this.byte_1 = (byte)int_23;
			this.int_18 = int_24;
			this.int_19 = int_25;
			this.int_20 = int_26;
			this.int_21 = int_27;
			this.int_12 = null;
		}

		internal int method_1(Class2 class2_0, int int_22)
		{
			ZlibCodec zlibCodec_ = class2_0.zlibCodec_0;
			int num = zlibCodec_.NextIn;
			int num2 = zlibCodec_.AvailableBytesIn;
			int num3 = class2_0.int_10;
			int i = class2_0.int_9;
			int num4 = class2_0.int_14;
			int num5 = (num4 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4);
			while (true)
			{
				int num6;
				switch (this.int_10)
				{
				case 0:
					if (num5 >= 258 && num2 >= 10)
					{
						class2_0.int_10 = num3;
						class2_0.int_9 = i;
						zlibCodec_.AvailableBytesIn = num2;
						zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
						zlibCodec_.NextIn = num;
						class2_0.int_14 = num4;
						int_22 = this.method_2((int)this.byte_0, (int)this.byte_1, this.int_18, this.int_19, this.int_20, this.int_21, class2_0, zlibCodec_);
						num = zlibCodec_.NextIn;
						num2 = zlibCodec_.AvailableBytesIn;
						num3 = class2_0.int_10;
						i = class2_0.int_9;
						num4 = class2_0.int_14;
						num5 = ((num4 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
						if (int_22 != 0)
						{
							this.int_10 = ((int_22 == 1) ? 7 : 9);
							continue;
						}
					}
					this.int_14 = (int)this.byte_0;
					this.int_12 = this.int_18;
					this.int_13 = this.int_19;
					this.int_10 = 1;
					goto IL_44A;
				case 1:
					goto IL_44A;
				case 2:
					num6 = this.int_16;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_795;
						}
						int_22 = 0;
						num2--;
						num3 |= (int)(zlibCodec_.InputBuffer[num++] & 255) << i;
						i += 8;
					}
					this.int_11 += (num3 & Class3.int_0[num6]);
					num3 >>= num6;
					i -= num6;
					this.int_14 = (int)this.byte_1;
					this.int_12 = this.int_20;
					this.int_13 = this.int_21;
					this.int_10 = 3;
					goto IL_2E3;
				case 3:
					goto IL_2E3;
				case 4:
					num6 = this.int_16;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_88C;
						}
						int_22 = 0;
						num2--;
						num3 |= (int)(zlibCodec_.InputBuffer[num++] & 255) << i;
						i += 8;
					}
					this.int_17 += (num3 & Class3.int_0[num6]);
					num3 >>= num6;
					i -= num6;
					this.int_10 = 5;
					goto IL_143;
				case 5:
					goto IL_143;
				case 6:
					if (num5 == 0)
					{
						if (num4 == class2_0.int_12 && class2_0.int_13 != 0)
						{
							num4 = 0;
							num5 = ((0 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
						}
						if (num5 == 0)
						{
							class2_0.int_14 = num4;
							int_22 = class2_0.Flush(int_22);
							num4 = class2_0.int_14;
							num5 = ((num4 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
							if (num4 == class2_0.int_12 && class2_0.int_13 != 0)
							{
								num4 = 0;
								num5 = ((0 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
							}
							if (num5 == 0)
							{
								goto IL_920;
							}
						}
					}
					int_22 = 0;
					class2_0.byte_0[num4++] = (byte)this.int_15;
					num5--;
					this.int_10 = 0;
					continue;
				case 7:
					goto IL_96A;
				case 8:
					goto IL_A1B;
				case 9:
					goto IL_A68;
				}
				break;
				IL_143:
				int j;
				for (j = num4 - this.int_17; j < 0; j += class2_0.int_12)
				{
				}
				while (this.int_11 != 0)
				{
					if (num5 == 0)
					{
						if (num4 == class2_0.int_12 && class2_0.int_13 != 0)
						{
							num4 = 0;
							num5 = ((0 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
						}
						if (num5 == 0)
						{
							class2_0.int_14 = num4;
							int_22 = class2_0.Flush(int_22);
							num4 = class2_0.int_14;
							num5 = ((num4 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
							if (num4 == class2_0.int_12 && class2_0.int_13 != 0)
							{
								num4 = 0;
								num5 = ((0 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4));
							}
							if (num5 == 0)
							{
								goto IL_8D6;
							}
						}
					}
					class2_0.byte_0[num4++] = class2_0.byte_0[j++];
					num5--;
					if (j == class2_0.int_12)
					{
						j = 0;
					}
					this.int_11--;
				}
				this.int_10 = 0;
				continue;
				IL_2E3:
				num6 = this.int_14;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_842;
					}
					int_22 = 0;
					num2--;
					num3 |= (int)(zlibCodec_.InputBuffer[num++] & 255) << i;
					i += 8;
				}
				int num7 = (this.int_13 + (num3 & Class3.int_0[num6])) * 3;
				num3 >>= this.int_12[num7 + 1];
				i -= this.int_12[num7 + 1];
				int num8 = this.int_12[num7];
				if ((num8 & 16) != 0)
				{
					this.int_16 = (num8 & 15);
					this.int_17 = this.int_12[num7 + 2];
					this.int_10 = 4;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.int_14 = num8;
					this.int_13 = num7 / 3 + this.int_12[num7 + 2];
					continue;
				}
				goto IL_7DF;
				IL_44A:
				num6 = this.int_14;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_74B;
					}
					int_22 = 0;
					num2--;
					num3 |= (int)(zlibCodec_.InputBuffer[num++] & 255) << i;
					i += 8;
				}
				num7 = (this.int_13 + (num3 & Class3.int_0[num6])) * 3;
				num3 >>= this.int_12[num7 + 1];
				i -= this.int_12[num7 + 1];
				num8 = this.int_12[num7];
				if (num8 == 0)
				{
					this.int_15 = this.int_12[num7 + 2];
					this.int_10 = 6;
				}
				else if ((num8 & 16) != 0)
				{
					this.int_16 = (num8 & 15);
					this.int_11 = this.int_12[num7 + 2];
					this.int_10 = 2;
				}
				else if ((num8 & 64) == 0)
				{
					this.int_14 = num8;
					this.int_13 = num7 / 3 + this.int_12[num7 + 2];
				}
				else
				{
					if ((num8 & 32) == 0)
					{
						goto IL_6E8;
					}
					this.int_10 = 7;
				}
			}
			int_22 = -2;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(-2);
			IL_6E8:
			this.int_10 = 9;
			zlibCodec_.Message = "invalid literal/length code";
			int_22 = -3;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(-3);
			IL_74B:
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(int_22);
			IL_795:
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(int_22);
			IL_7DF:
			this.int_10 = 9;
			zlibCodec_.Message = "invalid distance code";
			int_22 = -3;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(-3);
			IL_842:
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(int_22);
			IL_88C:
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(int_22);
			IL_8D6:
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(int_22);
			IL_920:
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(int_22);
			IL_96A:
			if (i > 7)
			{
				i -= 8;
				num2++;
				num--;
			}
			class2_0.int_14 = num4;
			int_22 = class2_0.Flush(int_22);
			num4 = class2_0.int_14;
			int arg_9BA_0 = (num4 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4);
			if (class2_0.int_13 != class2_0.int_14)
			{
				class2_0.int_10 = num3;
				class2_0.int_9 = i;
				zlibCodec_.AvailableBytesIn = num2;
				zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
				zlibCodec_.NextIn = num;
				class2_0.int_14 = num4;
				return class2_0.Flush(int_22);
			}
			this.int_10 = 8;
			IL_A1B:
			int_22 = 1;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(1);
			IL_A68:
			int_22 = -3;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_.AvailableBytesIn = num2;
			zlibCodec_.TotalBytesIn += (long)(num - zlibCodec_.NextIn);
			zlibCodec_.NextIn = num;
			class2_0.int_14 = num4;
			return class2_0.Flush(-3);
		}

		internal int method_2(int int_22, int int_23, int[] int_24, int int_25, int[] int_26, int int_27, Class2 class2_0, ZlibCodec zlibCodec_0)
		{
			int num = zlibCodec_0.NextIn;
			int num2 = zlibCodec_0.AvailableBytesIn;
			int num3 = class2_0.int_10;
			int i = class2_0.int_9;
			int num4 = class2_0.int_14;
			int num5 = (num4 < class2_0.int_13) ? (class2_0.int_13 - num4 - 1) : (class2_0.int_12 - num4);
			int num6 = Class3.int_0[int_22];
			int num7 = Class3.int_0[int_23];
			int num10;
			int num11;
			while (true)
			{
				if (i >= 20)
				{
					int num8 = num3 & num6;
					int num9 = (int_25 + num8) * 3;
					if ((num10 = int_24[num9]) == 0)
					{
						num3 >>= int_24[num9 + 1];
						i -= int_24[num9 + 1];
						class2_0.byte_0[num4++] = (byte)int_24[num9 + 2];
						num5--;
					}
					else
					{
						while (true)
						{
							num3 >>= int_24[num9 + 1];
							i -= int_24[num9 + 1];
							if ((num10 & 16) != 0)
							{
								break;
							}
							if ((num10 & 64) != 0)
							{
								goto IL_547;
							}
							num8 += int_24[num9 + 2];
							num8 += (num3 & Class3.int_0[num10]);
							num9 = (int_25 + num8) * 3;
							if ((num10 = int_24[num9]) == 0)
							{
								goto IL_3E8;
							}
						}
						num10 &= 15;
						num11 = int_24[num9 + 2] + (num3 & Class3.int_0[num10]);
						num3 >>= num10;
						for (i -= num10; i < 15; i += 8)
						{
							num2--;
							num3 |= (int)(zlibCodec_0.InputBuffer[num++] & 255) << i;
						}
						num8 = (num3 & num7);
						num9 = (int_27 + num8) * 3;
						num10 = int_26[num9];
						while (true)
						{
							num3 >>= int_26[num9 + 1];
							i -= int_26[num9 + 1];
							if ((num10 & 16) != 0)
							{
								break;
							}
							if ((num10 & 64) != 0)
							{
								goto IL_440;
							}
							num8 += int_26[num9 + 2];
							num8 += (num3 & Class3.int_0[num10]);
							num9 = (int_27 + num8) * 3;
							num10 = int_26[num9];
						}
						num10 &= 15;
						while (i < num10)
						{
							num2--;
							num3 |= (int)(zlibCodec_0.InputBuffer[num++] & 255) << i;
							i += 8;
						}
						int num12 = int_26[num9 + 2] + (num3 & Class3.int_0[num10]);
						num3 >>= num10;
						i -= num10;
						num5 -= num11;
						int num13;
						if (num4 >= num12)
						{
							num13 = num4 - num12;
							if (num4 - num13 > 0 && 2 > num4 - num13)
							{
								class2_0.byte_0[num4++] = class2_0.byte_0[num13++];
								class2_0.byte_0[num4++] = class2_0.byte_0[num13++];
								num11 -= 2;
							}
							else
							{
								Array.Copy(class2_0.byte_0, num13, class2_0.byte_0, num4, 2);
								num4 += 2;
								num13 += 2;
								num11 -= 2;
							}
						}
						else
						{
							num13 = num4 - num12;
							do
							{
								num13 += class2_0.int_12;
							}
							while (num13 < 0);
							num10 = class2_0.int_12 - num13;
							if (num11 > num10)
							{
								num11 -= num10;
								if (num4 - num13 > 0 && num10 > num4 - num13)
								{
									do
									{
										class2_0.byte_0[num4++] = class2_0.byte_0[num13++];
									}
									while (--num10 != 0);
								}
								else
								{
									Array.Copy(class2_0.byte_0, num13, class2_0.byte_0, num4, num10);
									num4 += num10;
									num13 += num10;
								}
								num13 = 0;
							}
						}
						if (num4 - num13 > 0 && num11 > num4 - num13)
						{
							do
							{
								class2_0.byte_0[num4++] = class2_0.byte_0[num13++];
							}
							while (--num11 != 0);
							goto IL_41D;
						}
						Array.Copy(class2_0.byte_0, num13, class2_0.byte_0, num4, num11);
						num4 += num11;
						num13 += num11;
						goto IL_41D;
						IL_3E8:
						num3 >>= int_24[num9 + 1];
						i -= int_24[num9 + 1];
						class2_0.byte_0[num4++] = (byte)int_24[num9 + 2];
						num5--;
					}
					IL_41D:
					if (num5 < 258 || num2 < 10)
					{
						goto IL_4CA;
					}
				}
				else
				{
					num2--;
					num3 |= (int)(zlibCodec_0.InputBuffer[num++] & 255) << i;
					i += 8;
				}
			}
			IL_440:
			zlibCodec_0.Message = "invalid distance code";
			num11 = zlibCodec_0.AvailableBytesIn - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_0.AvailableBytesIn = num2;
			zlibCodec_0.TotalBytesIn += (long)(num - zlibCodec_0.NextIn);
			zlibCodec_0.NextIn = num;
			class2_0.int_14 = num4;
			return -3;
			IL_4CA:
			num11 = zlibCodec_0.AvailableBytesIn - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_0.AvailableBytesIn = num2;
			zlibCodec_0.TotalBytesIn += (long)(num - zlibCodec_0.NextIn);
			zlibCodec_0.NextIn = num;
			class2_0.int_14 = num4;
			return 0;
			IL_547:
			if ((num10 & 32) != 0)
			{
				num11 = zlibCodec_0.AvailableBytesIn - num2;
				num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
				num2 += num11;
				num -= num11;
				i -= num11 << 3;
				class2_0.int_10 = num3;
				class2_0.int_9 = i;
				zlibCodec_0.AvailableBytesIn = num2;
				zlibCodec_0.TotalBytesIn += (long)(num - zlibCodec_0.NextIn);
				zlibCodec_0.NextIn = num;
				class2_0.int_14 = num4;
				return 1;
			}
			zlibCodec_0.Message = "invalid literal/length code";
			num11 = zlibCodec_0.AvailableBytesIn - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			class2_0.int_10 = num3;
			class2_0.int_9 = i;
			zlibCodec_0.AvailableBytesIn = num2;
			zlibCodec_0.TotalBytesIn += (long)(num - zlibCodec_0.NextIn);
			zlibCodec_0.NextIn = num;
			class2_0.int_14 = num4;
			return -3;
		}
	}
}
