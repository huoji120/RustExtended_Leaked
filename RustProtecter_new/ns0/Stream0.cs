using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Compression.Zlib;
using System.Text;

namespace ns0
{
	internal class Stream0 : Stream
	{
		internal enum Enum5
		{
			const_0,
			const_1,
			const_2
		}

		protected internal ZlibCodec zlibCodec_0;

		protected internal Stream0.Enum5 enum5_0 = Stream0.Enum5.const_2;

		protected internal FlushType flushType_0;

		protected internal Enum4 enum4_0;

		protected internal System.IO.Compression.Zlib.CompressionMode compressionMode_0;

		protected internal CompressionLevel compressionLevel_0;

		protected internal bool bool_0;

		protected internal byte[] byte_0;

		protected internal int int_0 = 16384;

		protected internal byte[] byte_1 = new byte[1];

		protected internal Stream stream_0;

		protected internal CompressionStrategy compressionStrategy_0;

		private CRC32 CRC32;

		protected internal string string_0;

		protected internal string string_1;

		protected internal DateTime dateTime_0;

		protected internal int int_1;

		private bool bool_1;

		internal int Crc32
		{
			get
			{
				if (this.CRC32 == null)
				{
					return 0;
				}
				return System.IO.Compression.CRC32.Crc32Result;
			}
		}

		protected internal bool _wantCompress
		{
			get
			{
				return this.compressionMode_0 == System.IO.Compression.Zlib.CompressionMode.Compress;
			}
		}

		private ZlibCodec z
		{
			get
			{
				if (this.zlibCodec_0 == null)
				{
					bool flag = this.enum4_0 == Enum4.const_0;
					this.zlibCodec_0 = new ZlibCodec();
					if (this.compressionMode_0 == System.IO.Compression.Zlib.CompressionMode.Decompress)
					{
						this.zlibCodec_0.InitializeInflate(flag);
					}
					else
					{
						this.zlibCodec_0.Strategy = this.compressionStrategy_0;
						this.zlibCodec_0.InitializeDeflate(this.compressionLevel_0, flag);
					}
				}
				return this.zlibCodec_0;
			}
		}

		private byte[] workingBuffer
		{
			get
			{
				if (this.byte_0 == null)
				{
					this.byte_0 = new byte[this.int_0];
				}
				return this.byte_0;
			}
		}

		public override bool CanRead
		{
			get
			{
				return this.stream_0.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.stream_0.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.stream_0.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return this.stream_0.Length;
			}
		}

		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public Stream0(Stream stream, System.IO.Compression.Zlib.CompressionMode compressionMode, CompressionLevel level, Enum4 flavor, bool leaveOpen)
		{
			this.flushType_0 = FlushType.None;
			this.stream_0 = stream;
			this.bool_0 = leaveOpen;
			this.compressionMode_0 = compressionMode;
			this.enum4_0 = flavor;
			this.compressionLevel_0 = level;
			if (flavor == Enum4.const_2)
			{
				this.CRC32 = new CRC32();
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.CRC32 != null)
			{
                System.IO.Compression.CRC32.SlurpBlock(buffer, offset, count);
			}
			if (this.enum5_0 == Stream0.Enum5.const_2)
			{
				this.enum5_0 = Stream0.Enum5.const_0;
			}
			else if (this.enum5_0 != Stream0.Enum5.const_0)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count == 0)
			{
				return;
			}
			this.z.InputBuffer = buffer;
			this.zlibCodec_0.NextIn = offset;
			this.zlibCodec_0.AvailableBytesIn = count;
			while (true)
			{
				this.zlibCodec_0.NextOut = 0;
				this.zlibCodec_0.OutputBuffer = this.workingBuffer;
				this.zlibCodec_0.AvailableBytesOut = this.byte_0.Length;
				int num = this._wantCompress ? this.zlibCodec_0.Deflate(this.flushType_0) : this.zlibCodec_0.Inflate(this.flushType_0);
				if (num != 0 && num != 1)
				{
					break;
				}
				this.stream_0.Write(this.byte_0, 0, this.byte_0.Length - this.zlibCodec_0.AvailableBytesOut);
				bool flag = this.zlibCodec_0.AvailableBytesIn == 0 && this.zlibCodec_0.AvailableBytesOut != 0;
				if (this.enum4_0 == Enum4.const_2 && !this._wantCompress)
				{
					flag = (this.zlibCodec_0.AvailableBytesIn == 8 && this.zlibCodec_0.AvailableBytesOut != 0);
				}
				if (flag)
				{
					return;
				}
			}
			throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this.zlibCodec_0.Message);
		}

		private void method_0()
		{
            if (this.zlibCodec_0 != null)
            {
                if (this.enum5_0 != Enum5.const_0)
                {
                    if ((this.enum5_0 == Enum5.const_1) && (this.enum4_0 == Enum4.const_2))
                    {
                        if (this._wantCompress)
                        {
                            throw new ZlibException("Reading with compression is not supported.");
                        }
                        if (this.zlibCodec_0.TotalBytesOut != 0L)
                        {
                            byte[] destinationArray = new byte[8];
                            if (this.zlibCodec_0.AvailableBytesIn < 8)
                            {
                                Array.Copy(this.zlibCodec_0.InputBuffer, this.zlibCodec_0.NextIn, destinationArray, 0, this.zlibCodec_0.AvailableBytesIn);
                                int count = 8 - this.zlibCodec_0.AvailableBytesIn;
                                int num5 = this.stream_0.Read(destinationArray, this.zlibCodec_0.AvailableBytesIn, count);
                                if (count != num5)
                                {
                                    throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this.zlibCodec_0.AvailableBytesIn + num5));
                                }
                            }
                            else
                            {
                                Array.Copy(this.zlibCodec_0.InputBuffer, this.zlibCodec_0.NextIn, destinationArray, 0, destinationArray.Length);
                            }
                            int num6 = BitConverter.ToInt32(destinationArray, 0);
                            int num7 = System.IO.Compression.CRC32.Crc32Result;
                            int num8 = BitConverter.ToInt32(destinationArray, 4);
                            int num9 = (int)(((ulong)this.zlibCodec_0.TotalBytesOut) & 0xffffffffL);
                            if (num7 != num6)
                            {
                                throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", num7, num6));
                            }
                            if (num9 != num8)
                            {
                                throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num9, num8));
                            }
                        }
                    }
                }
                else
                {
                    bool flag = false;
                    while (true)
                    {
                        this.zlibCodec_0.OutputBuffer = this.workingBuffer;
                        this.zlibCodec_0.NextOut = 0;
                        this.zlibCodec_0.AvailableBytesOut = this.byte_0.Length;
                        if (!this._wantCompress)
                        {
                        }
                        int num = this.zlibCodec_0.Deflate(FlushType.Finish);
                        if ((num != 1) && (num != 0))
                        {
                            string str = (this._wantCompress ? "de" : "in") + "flating";
                            if (this.zlibCodec_0.Message == null)
                            {
                                throw new ZlibException(string.Format("{0}: (rc = {1})", str, num));
                            }
                            throw new ZlibException(str + ": " + this.zlibCodec_0.Message);
                        }
                        if ((this.byte_0.Length - this.zlibCodec_0.AvailableBytesOut) > 0)
                        {
                            this.stream_0.Write(this.byte_0, 0, this.byte_0.Length - this.zlibCodec_0.AvailableBytesOut);
                        }
                        flag = (this.zlibCodec_0.AvailableBytesIn == 0) && (this.zlibCodec_0.AvailableBytesOut != 0);
                        if ((this.enum4_0 == Enum4.const_2) && !this._wantCompress)
                        {
                            flag = (this.zlibCodec_0.AvailableBytesIn == 8) && (this.zlibCodec_0.AvailableBytesOut != 0);
                        }
                        if (flag)
                        {
                            this.Flush();
                            if (this.enum4_0 == Enum4.const_2)
                            {
                                if (!this._wantCompress)
                                {
                                    throw new ZlibException("Writing with decompression is not supported.");
                                }
                                int num2 = System.IO.Compression.CRC32.Crc32Result;
                                this.stream_0.Write(BitConverter.GetBytes(num2), 0, 4);
                                int num3 = (int)(((ulong)System.IO.Compression.CRC32.TotalBytesRead) & 0xffffffffL);
                                this.stream_0.Write(BitConverter.GetBytes(num3), 0, 4);
                            }
                            return;
                        }
                    }
                }
            }
        }

		private void method_1()
		{
			if (this.z == null)
			{
				return;
			}
			if (this._wantCompress)
			{
				this.zlibCodec_0.EndDeflate();
			}
			else
			{
				this.zlibCodec_0.EndInflate();
			}
			this.zlibCodec_0 = null;
		}

		public override void Close()
		{
			if (this.stream_0 == null)
			{
				return;
			}
			try
			{
				this.method_0();
			}
			finally
			{
				this.method_1();
				if (!this.bool_0)
				{
					this.stream_0.Close();
				}
				this.stream_0 = null;
			}
		}

		public override void Flush()
		{
			this.stream_0.Flush();
		}

		public override long Seek(long offset, SeekOrigin loc)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			this.stream_0.SetLength(value);
		}

		private string method_2()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			while (true)
			{
				int num = this.stream_0.Read(this.byte_1, 0, 1);
				if (num != 1)
				{
					break;
				}
				if (this.byte_1[0] == 0)
				{
					flag = true;
				}
				else
				{
					list.Add(this.byte_1[0]);
				}
				if (flag)
				{
					goto IL_4C;
				}
			}
			throw new ZlibException("Unexpected EOF reading GZIP header.");
			IL_4C:
			byte[] array = list.ToArray();
			return System.IO.Compression.Zlib.GZipStream.encoding_0.GetString(array, 0, array.Length);
		}

		private int method_3()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = this.stream_0.Read(array, 0, array.Length);
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 != 10)
			{
				throw new ZlibException("Not a valid GZIP stream.");
			}
			if (array[0] == 31 && array[1] == 139)
			{
				if (array[2] == 8)
				{
					int num3 = BitConverter.ToInt32(array, 4);
					this.dateTime_0 = System.IO.Compression.Zlib.GZipStream.dateTime_0.AddSeconds((double)num3);
					num += num2;
					if ((array[3] & 4) == 4)
					{
						num2 = this.stream_0.Read(array, 0, 2);
						num += num2;
						short num4 = (short)((int)array[0] + (int)array[1] * 256);
						byte[] array2 = new byte[(int)num4];
						num2 = this.stream_0.Read(array2, 0, array2.Length);
						if (num2 != (int)num4)
						{
							throw new ZlibException("Unexpected end-of-file reading GZIP header.");
						}
						num += num2;
					}
					if ((array[3] & 8) == 8)
					{
						this.string_0 = this.method_2();
					}
					if ((array[3] & 16) == 16)
					{
						this.string_1 = this.method_2();
					}
					if ((array[3] & 2) == 2)
					{
						this.Read(this.byte_1, 0, 1);
					}
					return num;
				}
			}
			throw new ZlibException("Bad GZIP header.");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.enum5_0 == Stream0.Enum5.const_2)
			{
				if (!this.stream_0.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this.enum5_0 = Stream0.Enum5.const_1;
				this.z.AvailableBytesIn = 0;
				if (this.enum4_0 == Enum4.const_2)
				{
					this.int_1 = this.method_3();
					if (this.int_1 == 0)
					{
						return 0;
					}
				}
			}
			if (this.enum5_0 != Stream0.Enum5.const_1)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.bool_1 && this._wantCompress)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset < buffer.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.GetLength(0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.zlibCodec_0.OutputBuffer = buffer;
			this.zlibCodec_0.NextOut = offset;
			this.zlibCodec_0.AvailableBytesOut = count;
			this.zlibCodec_0.InputBuffer = this.workingBuffer;
			int num;
			while (true)
			{
				if (this.zlibCodec_0.AvailableBytesIn == 0 && !this.bool_1)
				{
					this.zlibCodec_0.NextIn = 0;
					this.zlibCodec_0.AvailableBytesIn = this.stream_0.Read(this.byte_0, 0, this.byte_0.Length);
					if (this.zlibCodec_0.AvailableBytesIn == 0)
					{
						this.bool_1 = true;
					}
				}
				num = (this._wantCompress ? this.zlibCodec_0.Deflate(this.flushType_0) : this.zlibCodec_0.Inflate(this.flushType_0));
				if (this.bool_1 && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_17;
				}
				if (((this.bool_1 || num == 1) && this.zlibCodec_0.AvailableBytesOut == count) || this.zlibCodec_0.AvailableBytesOut <= 0 || this.bool_1)
				{
					goto IL_234;
				}
				if (num != 0)
				{
					goto Block_21;
				}
			}
			return 0;
			Block_17:
			throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", this._wantCompress ? "de" : "in", num, this.zlibCodec_0.Message));
			Block_21:
			IL_234:
			if (this.zlibCodec_0.AvailableBytesOut > 0)
			{
				if (num != 0)
				{
				}
				if (this.bool_1 && this._wantCompress)
				{
					num = this.zlibCodec_0.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this.zlibCodec_0.Message));
					}
				}
			}
			num = count - this.zlibCodec_0.AvailableBytesOut;
			if (this.CRC32 != null)
			{
                System.IO.Compression.CRC32.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		public static void CompressString(string string_2, Stream stream_1)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(string_2);
			try
			{
				stream_1.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (stream_1 != null)
				{
					((IDisposable)stream_1).Dispose();
				}
			}
		}

		public static void CompressBuffer(byte[] byte_2, Stream stream_1)
		{
			try
			{
				stream_1.Write(byte_2, 0, byte_2.Length);
			}
			finally
			{
				if (stream_1 != null)
				{
					((IDisposable)stream_1).Dispose();
				}
			}
		}

		public static string UncompressString(byte[] byte_2, Stream stream_1)
		{
			byte[] array = new byte[1024];
			Encoding uTF = Encoding.UTF8;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = stream_1.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (stream_1 != null)
					{
						((IDisposable)stream_1).Dispose();
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(memoryStream, uTF);
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		public static byte[] UncompressBuffer(byte[] byte_2, Stream stream_1)
		{
			byte[] array = new byte[1024];
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = stream_1.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (stream_1 != null)
					{
						((IDisposable)stream_1).Dispose();
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}
	}
}
