using ns0;
using System;
using System.Text;

namespace System.IO.Compression.Zlib
{
	public class GZipStream : Stream
	{
		public DateTime? LastModified;

		private int int_0;

		internal Stream0 stream0_0;

		private bool bool_0;

		private bool bool_1;

		private string string_0;

		private string string_1;

		private int int_1;

		internal static readonly DateTime dateTime_0 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		internal static readonly Encoding encoding_0 = Encoding.GetEncoding("iso-8859-1");

		public string Comment
		{
			get
			{
				return this.string_1;
			}
			set
			{
				if (this.bool_0)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this.string_1 = value;
			}
		}

		public string FileName
		{
			get
			{
				return this.string_0;
			}
			set
			{
				if (this.bool_0)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this.string_0 = value;
				if (this.string_0 == null)
				{
					return;
				}
				if (this.string_0.IndexOf("/") != -1)
				{
					this.string_0 = this.string_0.Replace("/", "\\");
				}
				if (this.string_0.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this.string_0.IndexOf("\\") != -1)
				{
					this.string_0 = Path.GetFileName(this.string_0);
				}
			}
		}

		public int Crc32
		{
			get
			{
				return this.int_1;
			}
		}

		public virtual FlushType FlushMode
		{
			get
			{
				return this.stream0_0.flushType_0;
			}
			set
			{
				if (this.bool_0)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this.stream0_0.flushType_0 = value;
			}
		}

		public int BufferSize
		{
			get
			{
				return this.stream0_0.int_0;
			}
			set
			{
				if (this.bool_0)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				if (this.stream0_0.byte_0 != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this.stream0_0.int_0 = value;
			}
		}

		public virtual long TotalIn
		{
			get
			{
				return this.stream0_0.zlibCodec_0.TotalBytesIn;
			}
		}

		public virtual long TotalOut
		{
			get
			{
				return this.stream0_0.zlibCodec_0.TotalBytesOut;
			}
		}

		public override bool CanRead
		{
			get
			{
				if (this.bool_0)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this.stream0_0.stream_0.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				if (this.bool_0)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this.stream0_0.stream_0.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override long Position
		{
			get
			{
				if (this.stream0_0.enum5_0 == Stream0.Enum5.const_0)
				{
					return this.stream0_0.zlibCodec_0.TotalBytesOut + (long)this.int_0;
				}
				if (this.stream0_0.enum5_0 == Stream0.Enum5.const_1)
				{
					return this.stream0_0.zlibCodec_0.TotalBytesIn + (long)this.stream0_0.int_1;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this.stream0_0 = new Stream0(stream, mode, level, Enum4.const_2, leaveOpen);
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.bool_0)
				{
					if (disposing && this.stream0_0 != null)
					{
						this.stream0_0.Close();
						this.int_1 = this.stream0_0.Crc32;
					}
					this.bool_0 = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		public override void Flush()
		{
			if (this.bool_0)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this.stream0_0.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.bool_0)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this.stream0_0.Read(buffer, offset, count);
			if (!this.bool_1)
			{
				this.bool_1 = true;
				this.FileName = this.stream0_0.string_0;
				this.Comment = this.stream0_0.string_1;
			}
			return result;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.bool_0)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this.stream0_0.enum5_0 == Stream0.Enum5.const_2)
			{
				if (!this.stream0_0._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this.int_0 = this.method_0();
			}
			this.stream0_0.Write(buffer, offset, count);
		}

		private int method_0()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.encoding_0.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.encoding_0.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			byte[] arg_78_0 = array3;
			int expr_71 = 0;
			int num4 = expr_71 + 1;
			arg_78_0[expr_71] = 31;
			byte[] arg_86_0 = array3;
			int expr_7C = 1;
			num4 = expr_7C + 1;
			arg_86_0[expr_7C] = 139;
			byte[] arg_90_0 = array3;
			int expr_8A = 2;
			num4 = expr_8A + 1;
			arg_90_0[expr_8A] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num4++] = b;
			if (!this.LastModified.HasValue)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			int value = (int)(this.LastModified.Value - GZipStream.dateTime_0).TotalSeconds;
			Array.Copy(BitConverter.GetBytes(value), 0, array3, num4, 4);
			num4 += 4;
			array3[num4++] = 0;
			array3[num4++] = 255;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num4, num2 - 1);
				num4 += num2 - 1;
				array3[num4++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num4, num - 1);
				num4 += num - 1;
				array3[num4++] = 0;
			}
			this.stream0_0.stream_0.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream stream_ = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				Stream0.CompressString(s, stream_);
				result = memoryStream.ToArray();
			}
			return result;
		}

		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream stream_ = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				Stream0.CompressBuffer(b, stream_);
				result = memoryStream.ToArray();
			}
			return result;
		}

		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream stream_ = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = Stream0.UncompressString(compressed, stream_);
			}
			return result;
		}

		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream stream_ = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = Stream0.UncompressBuffer(compressed, stream_);
			}
			return result;
		}
	}
}
