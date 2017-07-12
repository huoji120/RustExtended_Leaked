using ns0;
using System;

namespace System.IO.Compression.Zlib
{
	public class ZlibStream : Stream
	{
		internal Stream0 stream0_0;

		private bool bool_0;

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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
				}
				return this.stream0_0.stream_0.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				if (this.stream0_0.enum5_0 == Stream0.Enum5.const_0)
				{
					return this.stream0_0.zlibCodec_0.TotalBytesOut;
				}
				if (this.stream0_0.enum5_0 == Stream0.Enum5.const_1)
				{
					return this.stream0_0.zlibCodec_0.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this.stream0_0 = new Stream0(stream, mode, level, Enum4.const_0, leaveOpen);
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
				throw new ObjectDisposedException("ZlibStream");
			}
			this.stream0_0.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.bool_0)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this.stream0_0.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.bool_0)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this.stream0_0.Write(buffer, offset, count);
		}

		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream stream_ = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
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
				Stream stream_ = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
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
				Stream stream_ = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = Stream0.UncompressString(compressed, stream_);
			}
			return result;
		}

		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream stream_ = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = Stream0.UncompressBuffer(compressed, stream_);
			}
			return result;
		}
	}
}
