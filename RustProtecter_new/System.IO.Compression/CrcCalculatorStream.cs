using System;

namespace System.IO.Compression
{
	public class CrcCalculatorStream : Stream, IDisposable
	{
		private static readonly long long_0 = -99L;

		internal Stream stream_0;

		private static CRC32 crc32;

		private long long_1 = -99L;

		private bool bool_0;

		public long TotalBytesSlurped
		{
			get
			{
				return CRC32.TotalBytesRead;
			}
		}

		public int Crc
		{
			get
			{
				return CRC32.Crc32Result;
			}
		}

		public bool LeaveOpen
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
				return false;
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
				if (this.long_1 == CrcCalculatorStream.long_0)
				{
					return this.stream_0.Length;
				}
				return this.long_1;
			}
		}

		public override long Position
		{
			get
			{
				return CRC32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.long_0, stream, null)
		{
		}

		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.long_0, stream, null)
		{
		}

		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			this.stream_0 = stream;
			crc32 = (crc32 ?? new CRC32());
			this.long_1 = length;
			this.bool_0 = leaveOpen;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this.long_1 != CrcCalculatorStream.long_0)
			{
				if (CRC32.TotalBytesRead >= long_1)
				{
					return 0;
				}
				long num = this.long_1 - CRC32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this.stream_0.Read(buffer, offset, count2);
			if (num2 > 0)
			{
                CRC32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
                CRC32.SlurpBlock(buffer, offset, count);
			}
			this.stream_0.Write(buffer, offset, count);
		}

		public override void Flush()
		{
			this.stream_0.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		void IDisposable.Dispose()
		{
			this.Close();
		}

		public override void Close()
		{
			base.Close();
			if (!this.bool_0)
			{
				this.stream_0.Close();
			}
		}
	}
}
