using System;
using System.Runtime.CompilerServices;

namespace RustExtended
{
	public class SnapshotData
	{
		[CompilerGenerated]
		private byte[] byte_0;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		public byte[] Buffer
		{
			get;
			private set;
		}

		public int Position
		{
			get;
			private set;
		}

		public int Length
		{
			get;
			private set;
		}

		public uint Hashsum
		{
			get
			{
				return CRC32.Quick(this.Buffer);
			}
		}

		public SnapshotData(uint BufferSize)
		{
			this.Buffer = new byte[BufferSize];
			this.Length = this.Buffer.Length;
			this.Position = 0;
		}

		public void Append(byte[] buffer, int offset, int count)
		{
			System.Buffer.BlockCopy(buffer, offset, this.Buffer, this.Position, count);
			this.Position += count;
		}
	}
}
