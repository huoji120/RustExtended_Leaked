using System;
using System.Runtime.CompilerServices;

namespace System.IO
{
	public class BitStream
	{
		public bool Serialization;

		private long long_0;

		private long long_1;

		[CompilerGenerated]
		private Stream stream_0;

		public virtual Stream BaseStream
		{
			get;
			private set;
		}

		public long Length
		{
			get
			{
				return this.BaseStream.Length;
			}
		}

		public long ReadPosition
		{
			get
			{
				return this.long_1;
			}
			set
			{
				this.long_1 = value;
			}
		}

		public long WritePosition
		{
			get
			{
				return this.long_0;
			}
			set
			{
				this.long_0 = value;
			}
		}

		public BitStream(bool serialize = true)
		{
			this.BaseStream = new MemoryStream();
			this.Serialization = serialize;
		}

		public BitStream(byte[] bytes, bool serialize = true)
		{
			this.BaseStream = new MemoryStream(bytes);
			this.Serialization = serialize;
		}

		public BitStream(Stream input, bool serialize = true)
		{
			this.BaseStream = input;
			this.Serialization = serialize;
		}

		public void RegisterType<T>(BitStreamCodec.SerializerHandler serializer, BitStreamCodec.DeserializerHandler deserializer)
		{
			BitStreamCodec.RegisterCodec<T>(serializer, deserializer);
		}

		public void SetLength(long value)
		{
			this.BaseStream.SetLength(value);
		}

		public long Seek(long offset, SeekOrigin origin)
		{
			return this.BaseStream.Seek(offset, origin);
		}

		public byte[] GetBuffer(int offset = 0)
		{
			byte[] array = new byte[this.BaseStream.Length];
			this.BaseStream.Seek((long)offset, SeekOrigin.Begin);
			this.BaseStream.Read(array, 0, array.Length);
			this.BaseStream.Position = this.long_1;
			return array;
		}

		public void SetBuffer(byte[] bytes, int offset = 0)
		{
			this.BaseStream = new MemoryStream(bytes, offset, bytes.Length);
			this.long_0 = this.BaseStream.Position;
			this.long_1 = 0L;
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			this.BaseStream.Position = this.long_0;
			this.BaseStream.Write(buffer, offset, count);
			this.long_0 = this.BaseStream.Position;
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			this.BaseStream.Position = this.long_1;
			return this.BaseStream.Read(buffer, offset, count);
		}

		public void WriteBytes(byte[] bytes, int offset, int length)
		{
			this.BaseStream.Position = this.long_0;
			this.BaseStream.Write(bytes, offset, length);
			this.long_0 = this.BaseStream.Position;
		}

		public byte[] ReadBytes(long length = 0L)
		{
			if (length <= 0L)
			{
				length = this.BaseStream.Length - this.long_1;
			}
			byte[] array = new byte[length];
			this.BaseStream.Position = this.long_1;
			this.BaseStream.Read(array, 0, array.Length);
			this.long_1 = this.BaseStream.Position;
			return array;
		}

		public void Write<T>(object value)
		{
			BitStreamCodec codec = BitStreamCodec.GetCodec<T>();
			codec.Serializer(this, value, ref this.long_0);
		}

		public void WriteBoolean(bool value)
		{
			this.Write<bool>(value);
		}

		public void WriteChar(char value)
		{
			this.Write<char>(value);
		}

		public void WriteByte(byte value)
		{
			this.Write<byte>(value);
		}

		public void WriteSByte(sbyte value)
		{
			this.Write<sbyte>(value);
		}

		public void WriteInt16(short value)
		{
			this.Write<short>(value);
		}

		public void WriteUInt16(ushort value)
		{
			this.Write<ushort>(value);
		}

		public void WriteInt32(int value)
		{
			this.Write<int>(value);
		}

		public void WriteUInt32(uint value)
		{
			this.Write<uint>(value);
		}

		public void WriteInt64(long value)
		{
			this.Write<long>(value);
		}

		public void WriteUInt64(ulong value)
		{
			this.Write<ulong>(value);
		}

		public void WriteSingle(float value)
		{
			this.Write<float>(value);
		}

		public void WriteDouble(double value)
		{
			this.Write<double>(value);
		}

		public void WriteDecimal(decimal value)
		{
			this.Write<decimal>(value);
		}

		public void WriteTimeSpan(TimeSpan value)
		{
			this.Write<TimeSpan>(value);
		}

		public void WriteDateTime(DateTime value)
		{
			this.Write<DateTime>(value);
		}

		public void WriteString(string value)
		{
			this.Write<string>(value);
		}

		public T Read<T>()
		{
			BitStreamCodec codec = BitStreamCodec.GetCodec<T>();
			return (T)((object)codec.Deserializer(this, ref this.long_1));
		}

		public bool ReadBoolean()
		{
			return this.Read<bool>();
		}

		public char ReadChar()
		{
			return this.Read<char>();
		}

		public byte ReadByte()
		{
			return this.Read<byte>();
		}

		public sbyte ReadSByte()
		{
			return this.Read<sbyte>();
		}

		public short ReadInt16()
		{
			return this.Read<short>();
		}

		public ushort ReadUInt16()
		{
			return this.Read<ushort>();
		}

		public int ReadInt32()
		{
			return this.Read<int>();
		}

		public uint ReadUInt32()
		{
			return this.Read<uint>();
		}

		public long ReadInt64()
		{
			return this.Read<long>();
		}

		public ulong ReadUInt64()
		{
			return this.Read<ulong>();
		}

		public float ReadSingle()
		{
			return this.Read<float>();
		}

		public double ReadDouble()
		{
			return this.Read<double>();
		}

		public decimal ReadDecimal()
		{
			return this.Read<decimal>();
		}

		public TimeSpan ReadTimeSpan()
		{
			return this.Read<TimeSpan>();
		}

		public DateTime ReadDateTime()
		{
			return this.Read<DateTime>();
		}

		public string ReadString()
		{
			return this.Read<string>();
		}
	}
}
