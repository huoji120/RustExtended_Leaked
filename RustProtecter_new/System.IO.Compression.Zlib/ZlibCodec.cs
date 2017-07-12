using ns0;
using System;
using System.Runtime.InteropServices;

namespace System.IO.Compression.Zlib
{
	[ClassInterface(ClassInterfaceType.AutoDispatch), ComVisible(true), Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	public sealed class ZlibCodec
	{
		public byte[] InputBuffer;

		public int NextIn;

		public int AvailableBytesIn;

		public long TotalBytesIn;

		public byte[] OutputBuffer;

		public int NextOut;

		public int AvailableBytesOut;

		public long TotalBytesOut;

		public string Message;

		internal Class0 class0_0;

		internal Class5 class5_0;

		internal uint uint_0;

		public CompressionLevel CompressLevel = CompressionLevel.Default;

		public int WindowBits = 15;

		public CompressionStrategy Strategy;

		public int Adler32
		{
			get
			{
				return (int)this.uint_0;
			}
		}

		public ZlibCodec()
		{
		}

		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				int num = this.InitializeDeflate();
				if (num != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				int num2 = this.InitializeInflate();
				if (num2 != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.class0_0 != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.class5_0 = new Class5(expectRfc1950Header);
			return this.class5_0.Initialize(this, windowBits);
		}

		public int Inflate(FlushType flush)
		{
			if (this.class5_0 == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.class5_0.Inflate(flush);
		}

		public int EndInflate()
		{
			if (this.class5_0 == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.class5_0.method_0();
			this.class5_0 = null;
			return result;
		}

		public int SyncInflate()
		{
			if (this.class5_0 == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.class5_0.method_1();
		}

		public int InitializeDeflate()
		{
			return this.method_0(true);
		}

		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this.method_0(true);
		}

		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this.method_0(wantRfc1950Header);
		}

		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this.method_0(true);
		}

		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this.method_0(wantRfc1950Header);
		}

		private int method_0(bool bool_0)
		{
			if (this.class5_0 != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.class0_0 = new Class0();
			this.class0_0.WantRfc1950HeaderBytes = bool_0;
			return this.class0_0.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		public int Deflate(FlushType flush)
		{
			if (this.class0_0 == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.class0_0.Deflate(flush);
		}

		public int EndDeflate()
		{
			if (this.class0_0 == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.class0_0 = null;
			return 0;
		}

		public void ResetDeflate()
		{
			if (this.class0_0 == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.class0_0.Reset();
		}

		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.class0_0 == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.class0_0.method_28(level, strategy);
		}

		public int SetDictionary(byte[] dictionary)
		{
			if (this.class5_0 != null)
			{
				return this.class5_0.SetDictionary(dictionary);
			}
			if (this.class0_0 == null)
			{
				throw new ZlibException("No Inflate or Deflate state!");
			}
			return this.class0_0.SetDictionary(dictionary);
		}

		internal void method_1()
		{
			int num = this.class0_0.int_21;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.class0_0.byte_0.Length > this.class0_0.int_20 && this.OutputBuffer.Length > this.NextOut && this.class0_0.byte_0.Length >= this.class0_0.int_20 + num && this.OutputBuffer.Length >= this.NextOut + num)
			{
				Array.Copy(this.class0_0.byte_0, this.class0_0.int_20, this.OutputBuffer, this.NextOut, num);
				this.NextOut += num;
				this.class0_0.int_20 += num;
				this.TotalBytesOut += (long)num;
				this.AvailableBytesOut -= num;
				this.class0_0.int_21 -= num;
				if (this.class0_0.int_21 == 0)
				{
					this.class0_0.int_20 = 0;
				}
				return;
			}
			throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.class0_0.byte_0.Length, this.class0_0.int_21));
		}

		internal int method_2(byte[] byte_0, int int_0, int int_1)
		{
			int num = this.AvailableBytesIn;
			if (num > int_1)
			{
				num = int_1;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.class0_0.WantRfc1950HeaderBytes)
			{
				this.uint_0 = Adler.Adler32(this.uint_0, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, byte_0, int_0, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}
	}
}
