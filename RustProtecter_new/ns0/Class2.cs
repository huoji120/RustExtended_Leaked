using System;
using System.IO.Compression.Zlib;

namespace ns0
{
	internal sealed class Class2
	{
		private enum Enum2
		{
			const_0,
			const_1,
			const_2,
			const_3,
			const_4,
			const_5,
			const_6,
			const_7,
			const_8,
			const_9
		}

		private const int int_0 = 1440;

		internal static readonly int[] int_1 = new int[]
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

		private Class2.Enum2 enum2_0;

		internal int int_2;

		internal int int_3;

		internal int int_4;

		internal int[] int_5;

		internal int[] int_6 = new int[1];

		internal int[] int_7 = new int[1];

		internal Class4 class4_0 = new Class4();

		internal int int_8;

		internal ZlibCodec zlibCodec_0;

		internal int int_9;

		internal int int_10;

		internal int[] int_11;

		internal byte[] byte_0;

		internal int int_12;

		internal int int_13;

		internal int int_14;

		internal object object_0;

		internal uint uint_0;

		internal Class6 class6_0 = new Class6();

		internal Class2(ZlibCodec codec, object checkfn, int w)
		{
			this.zlibCodec_0 = codec;
			this.int_11 = new int[4320];
			this.byte_0 = new byte[w];
			this.int_12 = w;
			this.object_0 = checkfn;
			this.enum2_0 = Class2.Enum2.const_0;
			this.Reset();
		}

		internal uint Reset()
		{
			uint result = this.uint_0;
			this.enum2_0 = Class2.Enum2.const_0;
			this.int_9 = 0;
			this.int_10 = 0;
			this.int_14 = 0;
			this.int_13 = 0;
			if (this.object_0 != null)
			{
				this.zlibCodec_0.uint_0 = (this.uint_0 = Adler.Adler32(0u, null, 0, 0));
			}
			return result;
		}

		internal int method_0(int int_15)
		{
            int num;
            int nextIn = this.zlibCodec_0.NextIn;
            int availableBytesIn = this.zlibCodec_0.AvailableBytesIn;
            int num2 = this.int_10;
            int num3 = this.int_9;
            int destinationIndex = this.int_14;
            int num7 = (destinationIndex < this.int_13) ? ((this.int_13 - destinationIndex) - 1) : (this.int_12 - destinationIndex);
            goto Label_07FC;
            Label_0056:
            this.int_10 = num2;
            this.int_9 = num3;
            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
            this.zlibCodec_0.NextIn = nextIn;
            this.int_14 = destinationIndex;
            int_15 = this.class4_0.method_1(this, int_15);
            if (int_15 != 1)
            {
                return this.Flush(int_15);
            }
            int_15 = 0;
            nextIn = this.zlibCodec_0.NextIn;
            availableBytesIn = this.zlibCodec_0.AvailableBytesIn;
            num2 = this.int_10;
            num3 = this.int_9;
            destinationIndex = this.int_14;
            num7 = (destinationIndex < this.int_13) ? ((this.int_13 - destinationIndex) - 1) : (this.int_12 - destinationIndex);
            if (this.int_8 != 0)
            {
                this.enum2_0 = Enum2.const_7;
                goto Label_0E11;
            }
            this.enum2_0 = Enum2.const_0;
            goto Label_07FC;
            Label_02C8:
            num = this.int_3;
            if (this.int_4 < ((0x102 + (num & 0x1f)) + ((num >> 5) & 0x1f)))
            {
                num = this.int_6[0];
                while (num3 < num)
                {
                    if (availableBytesIn == 0)
                    {
                        this.int_10 = num2;
                        this.int_9 = num3;
                        this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                        this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                        this.zlibCodec_0.NextIn = nextIn;
                        this.int_14 = destinationIndex;
                        return this.Flush(int_15);
                    }
                    int_15 = 0;
                    availableBytesIn--;
                    num2 |= (this.zlibCodec_0.InputBuffer[nextIn++] & 0xff) << num3;
                    num3 += 8;
                }
                num = this.int_11[((this.int_7[0] + (num2 & Class3.int_0[num])) * 3) + 1];
                int num10 = this.int_11[((this.int_7[0] + (num2 & Class3.int_0[num])) * 3) + 2];
                if (num10 < 0x10)
                {
                    num2 = num2 >> num;
                    num3 -= num;
                    this.int_5[this.int_4++] = num10;
                }
                else
                {
                    int index = (num10 == 0x12) ? 7 : (num10 - 14);
                    int num9 = (num10 == 0x12) ? 11 : 3;
                    while (num3 < (num + index))
                    {
                        if (availableBytesIn == 0)
                        {
                            this.int_10 = num2;
                            this.int_9 = num3;
                            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                            this.zlibCodec_0.NextIn = nextIn;
                            this.int_14 = destinationIndex;
                            return this.Flush(int_15);
                        }
                        int_15 = 0;
                        availableBytesIn--;
                        num2 |= (this.zlibCodec_0.InputBuffer[nextIn++] & 0xff) << num3;
                        num3 += 8;
                    }
                    num2 = num2 >> num;
                    num3 -= num;
                    num9 += num2 & Class3.int_0[index];
                    num2 = num2 >> index;
                    num3 -= index;
                    index = this.int_4;
                    num = this.int_3;
                    if (((index + num9) > ((0x102 + (num & 0x1f)) + ((num >> 5) & 0x1f))) || ((num10 == 0x10) && (index < 1)))
                    {
                        this.int_5 = null;
                        this.enum2_0 = Enum2.const_9;
                        this.zlibCodec_0.Message = "invalid bit length repeat";
                        int_15 = -3;
                        this.int_10 = num2;
                        this.int_9 = num3;
                        this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                        this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                        this.zlibCodec_0.NextIn = nextIn;
                        this.int_14 = destinationIndex;
                        return this.Flush(-3);
                    }
                    num10 = (num10 == 0x10) ? this.int_5[index - 1] : 0;
                    do
                    {
                        this.int_5[index++] = num10;
                    }
                    while (--num9 != 0);
                    this.int_4 = index;
                }
                goto Label_02C8;
            }
            this.int_7[0] = -1;
            int[] numArray5 = new int[] { 9 };
            int[] numArray6 = new int[] { 6 };
            int[] numArray7 = new int[1];
            int[] numArray8 = new int[1];
            num = this.int_3;
            num = this.class6_0.method_2(0x101 + (num & 0x1f), 1 + ((num >> 5) & 0x1f), this.int_5, numArray5, numArray6, numArray7, numArray8, this.int_11, this.zlibCodec_0);
            if (num != 0)
            {
                if (num == -3)
                {
                    this.int_5 = null;
                    this.enum2_0 = Enum2.const_9;
                }
                int_15 = num;
                this.int_10 = num2;
                this.int_9 = num3;
                this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                this.zlibCodec_0.NextIn = nextIn;
                this.int_14 = destinationIndex;
                return this.Flush(int_15);
            }
            this.class4_0.method_0(numArray5[0], numArray6[0], this.int_11, numArray7[0], this.int_11, numArray8[0]);
            this.enum2_0 = Enum2.const_6;
            goto Label_0056;
            Label_0401:
            while (this.int_4 < (4 + (this.int_3 >> 10)))
            {
                while (num3 < 3)
                {
                    if (availableBytesIn == 0)
                    {
                        this.int_10 = num2;
                        this.int_9 = num3;
                        this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                        this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                        this.zlibCodec_0.NextIn = nextIn;
                        this.int_14 = destinationIndex;
                        return this.Flush(int_15);
                    }
                    int_15 = 0;
                    availableBytesIn--;
                    num2 |= (this.zlibCodec_0.InputBuffer[nextIn++] & 0xff) << num3;
                    num3 += 8;
                }
                this.int_5[int_1[this.int_4++]] = num2 & 7;
                num2 = num2 >> 3;
                num3 -= 3;
            }
            while (this.int_4 < 0x13)
            {
                this.int_5[int_1[this.int_4++]] = 0;
            }
            this.int_6[0] = 7;
            num = this.class6_0.method_1(this.int_5, this.int_6, this.int_7, this.int_11, this.zlibCodec_0);
            if (num != 0)
            {
                int_15 = num;
                if (int_15 == -3)
                {
                    this.int_5 = null;
                    this.enum2_0 = Enum2.const_9;
                }
                this.int_10 = num2;
                this.int_9 = num3;
                this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                this.zlibCodec_0.NextIn = nextIn;
                this.int_14 = destinationIndex;
                return this.Flush(int_15);
            }
            this.int_4 = 0;
            this.enum2_0 = Enum2.const_5;
            goto Label_02C8;
            Label_07FC:
            switch (this.enum2_0)
            {
                case Enum2.const_0:
                    while (num3 < 3)
                    {
                        if (availableBytesIn == 0)
                        {
                            this.int_10 = num2;
                            this.int_9 = num3;
                            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                            this.zlibCodec_0.NextIn = nextIn;
                            this.int_14 = destinationIndex;
                            return this.Flush(int_15);
                        }
                        int_15 = 0;
                        availableBytesIn--;
                        num2 |= (this.zlibCodec_0.InputBuffer[nextIn++] & 0xff) << num3;
                        num3 += 8;
                    }
                    num = num2 & 7;
                    this.int_8 = num & 1;
                    switch (((uint)(num >> 1)))
                    {
                        case 0:
                            num2 = num2 >> 3;
                            num3 -= 3;
                            num = num3 & 7;
                            num2 = num2 >> num;
                            num3 -= num;
                            this.enum2_0 = Enum2.const_1;
                            break;

                        case 1:
                            {
                                int[] numArray = new int[1];
                                int[] numArray2 = new int[1];
                                int[][] numArray3 = new int[1][];
                                int[][] numArray4 = new int[1][];
                                Class6.smethod_0(numArray, numArray2, numArray3, numArray4, this.zlibCodec_0);
                                this.class4_0.method_0(numArray[0], numArray2[0], numArray3[0], 0, numArray4[0], 0);
                                num2 = num2 >> 3;
                                num3 -= 3;
                                this.enum2_0 = Enum2.const_6;
                                break;
                            }
                        case 2:
                            num2 = num2 >> 3;
                            num3 -= 3;
                            this.enum2_0 = Enum2.const_3;
                            break;

                        case 3:
                            num2 = num2 >> 3;
                            num3 -= 3;
                            this.enum2_0 = Enum2.const_9;
                            this.zlibCodec_0.Message = "invalid block type";
                            int_15 = -3;
                            this.int_10 = num2;
                            this.int_9 = num3;
                            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                            this.zlibCodec_0.NextIn = nextIn;
                            this.int_14 = destinationIndex;
                            return this.Flush(-3);
                    }
                    goto Label_07FC;

                case Enum2.const_1:
                    while (num3 < 0x20)
                    {
                        if (availableBytesIn == 0)
                        {
                            this.int_10 = num2;
                            this.int_9 = num3;
                            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                            this.zlibCodec_0.NextIn = nextIn;
                            this.int_14 = destinationIndex;
                            return this.Flush(int_15);
                        }
                        int_15 = 0;
                        availableBytesIn--;
                        num2 |= (this.zlibCodec_0.InputBuffer[nextIn++] & 0xff) << num3;
                        num3 += 8;
                    }
                    if (((~num2 >> 0x10) & 0xffff) != (num2 & 0xffff))
                    {
                        this.enum2_0 = Enum2.const_9;
                        this.zlibCodec_0.Message = "invalid stored block lengths";
                        int_15 = -3;
                        this.int_10 = num2;
                        this.int_9 = num3;
                        this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                        this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                        this.zlibCodec_0.NextIn = nextIn;
                        this.int_14 = destinationIndex;
                        return this.Flush(-3);
                    }
                    this.int_2 = num2 & 0xffff;
                    num2 = num3 = 0;
                    this.enum2_0 = (this.int_2 != 0) ? Enum2.const_2 : ((this.int_8 != 0) ? Enum2.const_7 : Enum2.const_0);
                    goto Label_07FC;

                case Enum2.const_2:
                    if (availableBytesIn == 0)
                    {
                        this.int_10 = num2;
                        this.int_9 = num3;
                        this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                        this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                        this.zlibCodec_0.NextIn = nextIn;
                        this.int_14 = destinationIndex;
                        return this.Flush(int_15);
                    }
                    if (num7 == 0)
                    {
                        if ((destinationIndex == this.int_12) && (this.int_13 != 0))
                        {
                            destinationIndex = 0;
                            num7 = (0 < this.int_13) ? ((this.int_13 - destinationIndex) - 1) : (this.int_12 - destinationIndex);
                        }
                        if (num7 == 0)
                        {
                            this.int_14 = destinationIndex;
                            int_15 = this.Flush(int_15);
                            destinationIndex = this.int_14;
                            num7 = (destinationIndex < this.int_13) ? ((this.int_13 - destinationIndex) - 1) : (this.int_12 - destinationIndex);
                            if ((destinationIndex == this.int_12) && (this.int_13 != 0))
                            {
                                destinationIndex = 0;
                                num7 = (0 < this.int_13) ? ((this.int_13 - destinationIndex) - 1) : (this.int_12 - destinationIndex);
                            }
                            if (num7 == 0)
                            {
                                this.int_10 = num2;
                                this.int_9 = num3;
                                this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                                this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                                this.zlibCodec_0.NextIn = nextIn;
                                this.int_14 = destinationIndex;
                                return this.Flush(int_15);
                            }
                        }
                    }
                    int_15 = 0;
                    num = this.int_2;
                    if (num > availableBytesIn)
                    {
                        num = availableBytesIn;
                    }
                    if (num > num7)
                    {
                        num = num7;
                    }
                    Array.Copy(this.zlibCodec_0.InputBuffer, nextIn, this.byte_0, destinationIndex, num);
                    nextIn += num;
                    availableBytesIn -= num;
                    destinationIndex += num;
                    num7 -= num;
                    this.int_2 -= num;
                    if (this.int_2 == 0)
                    {
                        this.enum2_0 = (this.int_8 != 0) ? Enum2.const_7 : Enum2.const_0;
                    }
                    goto Label_07FC;

                case Enum2.const_3:
                    while (num3 < 14)
                    {
                        if (availableBytesIn == 0)
                        {
                            this.int_10 = num2;
                            this.int_9 = num3;
                            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                            this.zlibCodec_0.NextIn = nextIn;
                            this.int_14 = destinationIndex;
                            return this.Flush(int_15);
                        }
                        int_15 = 0;
                        availableBytesIn--;
                        num2 |= (this.zlibCodec_0.InputBuffer[nextIn++] & 0xff) << num3;
                        num3 += 8;
                    }
                    this.int_3 = num = num2 & 0x3fff;
                    if (((num & 0x1f) > 0x1d) || (((num >> 5) & 0x1f) > 0x1d))
                    {
                        this.enum2_0 = Enum2.const_9;
                        this.zlibCodec_0.Message = "too many length or distance symbols";
                        int_15 = -3;
                        this.int_10 = num2;
                        this.int_9 = num3;
                        this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                        this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                        this.zlibCodec_0.NextIn = nextIn;
                        this.int_14 = destinationIndex;
                        return this.Flush(-3);
                    }
                    num = (0x102 + (num & 0x1f)) + ((num >> 5) & 0x1f);
                    if ((this.int_5 != null) && (this.int_5.Length >= num))
                    {
                        Array.Clear(this.int_5, 0, num);
                    }
                    else
                    {
                        this.int_5 = new int[num];
                    }
                    num2 = num2 >> 14;
                    num3 -= 14;
                    this.int_4 = 0;
                    this.enum2_0 = Enum2.const_4;
                    goto Label_0401;

                case Enum2.const_4:
                    goto Label_0401;

                case Enum2.const_5:
                    goto Label_02C8;

                case Enum2.const_6:
                    goto Label_0056;

                case Enum2.const_7:
                    break;

                case Enum2.const_8:
                    goto Label_0EB8;

                case Enum2.const_9:
                    int_15 = -3;
                    this.int_10 = num2;
                    this.int_9 = num3;
                    this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                    this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                    this.zlibCodec_0.NextIn = nextIn;
                    this.int_14 = destinationIndex;
                    return this.Flush(-3);

                default:
                    int_15 = -2;
                    this.int_10 = num2;
                    this.int_9 = num3;
                    this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                    this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                    this.zlibCodec_0.NextIn = nextIn;
                    this.int_14 = destinationIndex;
                    return this.Flush(-2);
            }
            Label_0E11:
            this.int_14 = destinationIndex;
            int_15 = this.Flush(int_15);
            destinationIndex = this.int_14;
            num7 = (destinationIndex < this.int_13) ? ((this.int_13 - destinationIndex) - 1) : (this.int_12 - destinationIndex);
            if (this.int_13 != this.int_14)
            {
                this.int_10 = num2;
                this.int_9 = num3;
                this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
                this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
                this.zlibCodec_0.NextIn = nextIn;
                this.int_14 = destinationIndex;
                return this.Flush(int_15);
            }
            this.enum2_0 = Enum2.const_8;
            Label_0EB8:
            int_15 = 1;
            this.int_10 = num2;
            this.int_9 = num3;
            this.zlibCodec_0.AvailableBytesIn = availableBytesIn;
            this.zlibCodec_0.TotalBytesIn += nextIn - this.zlibCodec_0.NextIn;
            this.zlibCodec_0.NextIn = nextIn;
            this.int_14 = destinationIndex;
            return this.Flush(1);
        }

 

 
		internal void method_1()
		{
			this.Reset();
			this.byte_0 = null;
			this.int_11 = null;
		}

		internal void SetDictionary(byte[] byte_1, int int_15, int int_16)
		{
			Array.Copy(byte_1, int_15, this.byte_0, 0, int_16);
			this.int_14 = int_16;
			this.int_13 = int_16;
		}

		internal int method_2()
		{
			if (this.enum2_0 != Class2.Enum2.const_1)
			{
				return 0;
			}
			return 1;
		}

		internal int Flush(int int_15)
		{
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (i == 0)
				{
					num = ((this.int_13 <= this.int_14) ? this.int_14 : this.int_12) - this.int_13;
				}
				else
				{
					num = this.int_14 - this.int_13;
				}
				if (num == 0)
				{
					if (int_15 == -5)
					{
						int_15 = 0;
					}
					return int_15;
				}
				if (num > this.zlibCodec_0.AvailableBytesOut)
				{
					num = this.zlibCodec_0.AvailableBytesOut;
				}
				if (num != 0 && int_15 == -5)
				{
					int_15 = 0;
				}
				this.zlibCodec_0.AvailableBytesOut -= num;
				this.zlibCodec_0.TotalBytesOut += (long)num;
				if (this.object_0 != null)
				{
					this.zlibCodec_0.uint_0 = (this.uint_0 = Adler.Adler32(this.uint_0, this.byte_0, this.int_13, num));
				}
				Array.Copy(this.byte_0, this.int_13, this.zlibCodec_0.OutputBuffer, this.zlibCodec_0.NextOut, num);
				this.zlibCodec_0.NextOut += num;
				this.int_13 += num;
				if (this.int_13 == this.int_12 && i == 0)
				{
					this.int_13 = 0;
					if (this.int_14 == this.int_12)
					{
						this.int_14 = 0;
					}
				}
				else
				{
					i++;
				}
			}
			return int_15;
		}
	}
}
