namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;

    public class SnapshotData
    {
        [CompilerGenerated]
        private byte[] byte_0;
        [CompilerGenerated]
        private int int_0;
        [CompilerGenerated]
        private int int_1;

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

        public byte[] Buffer
        {
            [CompilerGenerated]
            get
            {
                return this.byte_0;
            }
            [CompilerGenerated]
            private set
            {
                this.byte_0 = value;
            }
        }

        public uint Hashsum
        {
            get
            {
                return CRC32.Quick(this.Buffer);
            }
        }

        public int Length
        {
            [CompilerGenerated]
            get
            {
                return this.int_1;
            }
            [CompilerGenerated]
            private set
            {
                this.int_1 = value;
            }
        }

        public int Position
        {
            [CompilerGenerated]
            get
            {
                return this.int_0;
            }
            [CompilerGenerated]
            private set
            {
                this.int_0 = value;
            }
        }
    }
}

