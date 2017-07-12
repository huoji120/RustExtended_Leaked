using System;
using System.IO.Compression.Zlib;

namespace ns0
{
	internal sealed class Class5
	{
		private enum Enum3
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
			const_9,
			const_10,
			const_11,
			const_12,
			const_13
		}

		private const int int_0 = 32;

		private const int int_1 = 8;

		private Class5.Enum3 enum3_0;

		internal ZlibCodec zlibCodec_0;

		internal int int_2;

		internal uint uint_0;

		internal uint uint_1;

		internal int int_3;

		private bool bool_0 = true;

		internal int int_4;

		internal Class2 class2_0;

		private static readonly byte[] byte_0 = new byte[]
		{
			0,
			0,
			255,
			255
		};

		internal bool HandleRfc1950HeaderBytes
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
			}
		}

		public Class5()
		{
		}

		public Class5(bool expectRfc1950HeaderBytes)
		{
			this.bool_0 = expectRfc1950HeaderBytes;
		}

		internal int Reset()
		{
			ZlibCodec arg_1C_0 = this.zlibCodec_0;
			this.zlibCodec_0.TotalBytesOut = 0L;
			arg_1C_0.TotalBytesIn = 0L;
			this.zlibCodec_0.Message = null;
			this.enum3_0 = (this.HandleRfc1950HeaderBytes ? Class5.Enum3.const_0 : Class5.Enum3.const_7);
			this.class2_0.Reset();
			return 0;
		}

		internal int method_0()
		{
			if (this.class2_0 != null)
			{
				this.class2_0.method_1();
			}
			this.class2_0 = null;
			return 0;
		}

		internal int Initialize(ZlibCodec zlibCodec_1, int int_5)
		{
			this.zlibCodec_0 = zlibCodec_1;
			this.zlibCodec_0.Message = null;
			this.class2_0 = null;
			if (int_5 >= 8 && int_5 <= 15)
			{
				this.int_4 = int_5;
				this.class2_0 = new Class2(zlibCodec_1, this.HandleRfc1950HeaderBytes ? this : null, 1 << int_5);
				this.Reset();
				return 0;
			}
			this.method_0();
			throw new ZlibException("Bad window size.");
		}

		internal int Inflate(FlushType flushType_0)
		{
			if (this.zlibCodec_0.InputBuffer == null)
			{
				throw new ZlibException("InputBuffer is null. ");
			}
			int num = 0;
			int num2 = -5;
			while (true)
			{
				switch (this.enum3_0)
				{
				case Class5.Enum3.const_0:
					if (this.zlibCodec_0.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this.zlibCodec_0.AvailableBytesIn--;
					this.zlibCodec_0.TotalBytesIn += 1L;
					if (((this.int_2 = (int)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++]) & 15) != 8)
					{
						this.enum3_0 = Class5.Enum3.const_13;
						this.zlibCodec_0.Message = string.Format("unknown compression method (0x{0:X2})", this.int_2);
						this.int_3 = 5;
						continue;
					}
					if ((this.int_2 >> 4) + 8 > this.int_4)
					{
						this.enum3_0 = Class5.Enum3.const_13;
						this.zlibCodec_0.Message = string.Format("invalid window size ({0})", (this.int_2 >> 4) + 8);
						this.int_3 = 5;
						continue;
					}
					this.enum3_0 = Class5.Enum3.const_1;
					continue;
				case Class5.Enum3.const_1:
				{
					if (this.zlibCodec_0.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this.zlibCodec_0.AvailableBytesIn--;
					this.zlibCodec_0.TotalBytesIn += 1L;
					int num3 = (int)(this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] & 255);
					if (((this.int_2 << 8) + num3) % 31 != 0)
					{
						this.enum3_0 = Class5.Enum3.const_13;
						this.zlibCodec_0.Message = "incorrect header check";
						this.int_3 = 5;
						continue;
					}
					this.enum3_0 = (((num3 & 32) == 0) ? Class5.Enum3.const_7 : Class5.Enum3.const_2);
					continue;
				}
				case Class5.Enum3.const_2:
					if (this.zlibCodec_0.AvailableBytesIn != 0)
					{
						num2 = num;
						this.zlibCodec_0.AvailableBytesIn--;
						this.zlibCodec_0.TotalBytesIn += 1L;
                            unchecked
                            {
                                this.uint_1 = (uint)((long)((long)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] << 24) & ((long)(ulong)-16777216));
                            }
						this.enum3_0 = Class5.Enum3.const_3;
						continue;
					}
					return num2;
				case Class5.Enum3.const_3:
					if (this.zlibCodec_0.AvailableBytesIn != 0)
					{
						num2 = num;
						this.zlibCodec_0.AvailableBytesIn--;
						this.zlibCodec_0.TotalBytesIn += 1L;
						this.uint_1 += (uint)((int)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] << 16 & 16711680);
						this.enum3_0 = Class5.Enum3.const_4;
						continue;
					}
					return num2;
				case Class5.Enum3.const_4:
					if (this.zlibCodec_0.AvailableBytesIn != 0)
					{
						num2 = num;
						this.zlibCodec_0.AvailableBytesIn--;
						this.zlibCodec_0.TotalBytesIn += 1L;
						this.uint_1 += (uint)((int)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] << 8 & 65280);
						this.enum3_0 = Class5.Enum3.const_5;
						continue;
					}
					return num2;
				case Class5.Enum3.const_5:
					goto IL_60B;
				case Class5.Enum3.const_6:
					goto IL_694;
				case Class5.Enum3.const_7:
					num2 = this.class2_0.method_0(num2);
					if (num2 == -3)
					{
						this.enum3_0 = Class5.Enum3.const_13;
						this.int_3 = 0;
						continue;
					}
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 != 1)
					{
						return num2;
					}
					num2 = num;
					this.uint_0 = this.class2_0.Reset();
					if (this.HandleRfc1950HeaderBytes)
					{
						this.enum3_0 = Class5.Enum3.const_8;
						continue;
					}
					goto IL_6B8;
				case Class5.Enum3.const_8:
					if (this.zlibCodec_0.AvailableBytesIn != 0)
					{
						num2 = num;
						this.zlibCodec_0.AvailableBytesIn--;
						this.zlibCodec_0.TotalBytesIn += 1L;
                            unchecked
                            {
                                this.uint_1 = (uint)((long)((long)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] << 24) & (long)((ulong)-16777216));
                            }
						this.enum3_0 = Class5.Enum3.const_9;
						continue;
					}
					return num2;
				case Class5.Enum3.const_9:
					if (this.zlibCodec_0.AvailableBytesIn != 0)
					{
						num2 = num;
						this.zlibCodec_0.AvailableBytesIn--;
						this.zlibCodec_0.TotalBytesIn += 1L;
						this.uint_1 += (uint)((int)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] << 16 & 16711680);
						this.enum3_0 = Class5.Enum3.const_10;
						continue;
					}
					return num2;
				case Class5.Enum3.const_10:
					if (this.zlibCodec_0.AvailableBytesIn != 0)
					{
						num2 = num;
						this.zlibCodec_0.AvailableBytesIn--;
						this.zlibCodec_0.TotalBytesIn += 1L;
						this.uint_1 += (uint)((int)this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] << 8 & 65280);
						this.enum3_0 = Class5.Enum3.const_11;
						continue;
					}
					return num2;
				case Class5.Enum3.const_11:
					if (this.zlibCodec_0.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this.zlibCodec_0.AvailableBytesIn--;
					this.zlibCodec_0.TotalBytesIn += 1L;
					this.uint_1 += (uint)(this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] & 255);
					if (this.uint_0 != this.uint_1)
					{
						this.enum3_0 = Class5.Enum3.const_13;
						this.zlibCodec_0.Message = "incorrect data check";
						this.int_3 = 5;
						continue;
					}
					goto IL_6CA;
				case Class5.Enum3.const_12:
					return 1;
				case Class5.Enum3.const_13:
					goto IL_6D4;
				}
				goto Block_20;
			}
			return num2;
			Block_20:
			throw new ZlibException("Stream error.");
			IL_60B:
			if (this.zlibCodec_0.AvailableBytesIn == 0)
			{
				return num2;
			}
			this.zlibCodec_0.AvailableBytesIn--;
			this.zlibCodec_0.TotalBytesIn += 1L;
			this.uint_1 += (uint)(this.zlibCodec_0.InputBuffer[this.zlibCodec_0.NextIn++] & 255);
			this.zlibCodec_0.uint_0 = this.uint_1;
			this.enum3_0 = Class5.Enum3.const_6;
			return 2;
			IL_694:
			this.enum3_0 = Class5.Enum3.const_13;
			this.zlibCodec_0.Message = "need dictionary";
			this.int_3 = 0;
			return -2;
			IL_6B8:
			this.enum3_0 = Class5.Enum3.const_12;
			return 1;
			IL_6CA:
			this.enum3_0 = Class5.Enum3.const_12;
			return 1;
			IL_6D4:
			throw new ZlibException(string.Format("Bad state ({0})", this.zlibCodec_0.Message));
		}

		internal int SetDictionary(byte[] byte_1)
		{
			int int_ = 0;
			int num = byte_1.Length;
			if (this.enum3_0 != Class5.Enum3.const_6)
			{
				throw new ZlibException("Stream error.");
			}
			if (Adler.Adler32(1u, byte_1, 0, byte_1.Length) != this.zlibCodec_0.uint_0)
			{
				return -3;
			}
			this.zlibCodec_0.uint_0 = Adler.Adler32(0u, null, 0, 0);
			if (num >= 1 << this.int_4)
			{
				num = (1 << this.int_4) - 1;
				int_ = byte_1.Length - num;
			}
			this.class2_0.SetDictionary(byte_1, int_, num);
			this.enum3_0 = Class5.Enum3.const_7;
			return 0;
		}

		internal int method_1()
		{
			if (this.enum3_0 != Class5.Enum3.const_13)
			{
				this.enum3_0 = Class5.Enum3.const_13;
				this.int_3 = 0;
			}
			int num;
			if ((num = this.zlibCodec_0.AvailableBytesIn) == 0)
			{
				return -5;
			}
			int num2 = this.zlibCodec_0.NextIn;
			int num3 = this.int_3;
			while (num != 0 && num3 < 4)
			{
				if (this.zlibCodec_0.InputBuffer[num2] == Class5.byte_0[num3])
				{
					num3++;
				}
				else if (this.zlibCodec_0.InputBuffer[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			this.zlibCodec_0.TotalBytesIn += (long)(num2 - this.zlibCodec_0.NextIn);
			this.zlibCodec_0.NextIn = num2;
			this.zlibCodec_0.AvailableBytesIn = num;
			this.int_3 = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long totalBytesIn = this.zlibCodec_0.TotalBytesIn;
			long totalBytesOut = this.zlibCodec_0.TotalBytesOut;
			this.Reset();
			this.zlibCodec_0.TotalBytesIn = totalBytesIn;
			this.zlibCodec_0.TotalBytesOut = totalBytesOut;
			this.enum3_0 = Class5.Enum3.const_7;
			return 0;
		}

		internal int method_2(ZlibCodec zlibCodec_1)
		{
			return this.class2_0.method_2();
		}
	}
}
