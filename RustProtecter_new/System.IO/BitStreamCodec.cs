using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.IO
{
	public class BitStreamCodec
	{
		public delegate void SerializerHandler(BitStream stream, object value, ref long position);

		public delegate object DeserializerHandler(BitStream stream, ref long position);

		private static readonly Dictionary<Type, BitStreamCodec> dictionary_0;

		private static byte byte_0;

		public BitStreamCodec.SerializerHandler Serializer;

		public BitStreamCodec.DeserializerHandler Deserializer;

		[CompilerGenerated]
		private Type type_0;

		[CompilerGenerated]
		private byte byte_1;

		public Type Type
		{
			get;
			private set;
		}

		public byte Code
		{
			get;
			private set;
		}

		static BitStreamCodec()
		{
			BitStreamCodec.dictionary_0 = new Dictionary<Type, BitStreamCodec>();
			BitStreamCodec.byte_0 = 16;
			BitStreamCodec.RegisterCodec<object>(BitStreamTypeCode.Undefined, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_0), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_1));
			BitStreamCodec.RegisterCodec<bool>(BitStreamTypeCode.Boolean, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_2), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_3));
			BitStreamCodec.RegisterCodec<char>(BitStreamTypeCode.Char, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_4), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_5));
			BitStreamCodec.RegisterCodec<byte>(BitStreamTypeCode.Byte, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_6), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_7));
			BitStreamCodec.RegisterCodec<sbyte>(BitStreamTypeCode.SByte, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_8), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_9));
			BitStreamCodec.RegisterCodec<short>(BitStreamTypeCode.Int16, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_10), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_11));
			BitStreamCodec.RegisterCodec<ushort>(BitStreamTypeCode.UInt16, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_12), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_13));
			BitStreamCodec.RegisterCodec<int>(BitStreamTypeCode.Int32, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_14), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_15));
			BitStreamCodec.RegisterCodec<uint>(BitStreamTypeCode.UInt32, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_16), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_17));
			BitStreamCodec.RegisterCodec<long>(BitStreamTypeCode.Int64, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_18), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_19));
			BitStreamCodec.RegisterCodec<ulong>(BitStreamTypeCode.UInt64, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_20), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_21));
			BitStreamCodec.RegisterCodec<float>(BitStreamTypeCode.Single, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_22), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_23));
			BitStreamCodec.RegisterCodec<double>(BitStreamTypeCode.Double, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_24), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_25));
			BitStreamCodec.RegisterCodec<decimal>(BitStreamTypeCode.Decimal, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_26), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_27));
			BitStreamCodec.RegisterCodec<TimeSpan>(BitStreamTypeCode.TimeSpan, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_28), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_29));
			BitStreamCodec.RegisterCodec<DateTime>(BitStreamTypeCode.DateTime, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_30), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_31));
			BitStreamCodec.RegisterCodec<string>(BitStreamTypeCode.String, new BitStreamCodec.SerializerHandler(BitStreamCodec.smethod_32), new BitStreamCodec.DeserializerHandler(BitStreamCodec.smethod_33));
		}

		public BitStreamCodec(Type type, byte bytecode, BitStreamCodec.SerializerHandler serializer, BitStreamCodec.DeserializerHandler deserializer)
		{
			this.Type = type;
			this.Code = bytecode;
			this.Serializer = serializer;
			this.Deserializer = deserializer;
		}

		public static BitStreamCodec GetCodec<T>()
		{
			return BitStreamCodec.GetCodec(typeof(T));
		}

		public static BitStreamCodec GetCodec(Type type)
		{
			BitStreamCodec result;
			if (!BitStreamCodec.dictionary_0.TryGetValue(type, out result))
			{
				return BitStreamCodec.dictionary_0[typeof(object)];
			}
			return result;
		}

		public static void RegisterCodec<T>(BitStreamTypeCode typeCode, BitStreamCodec.SerializerHandler serializer, BitStreamCodec.DeserializerHandler deserializer)
		{
			BitStreamCodec.RegisterCodec<T>((byte)typeCode, serializer, deserializer);
		}

		public static void RegisterCodec<T>(byte code, BitStreamCodec.SerializerHandler serializer, BitStreamCodec.DeserializerHandler deserializer)
		{
			BitStreamCodec.RegisterCodec(typeof(T), code, serializer, deserializer);
		}

		public static void RegisterCodec<T>(BitStreamCodec.SerializerHandler serializer, BitStreamCodec.DeserializerHandler deserializer)
		{
			if (!BitStreamCodec.dictionary_0.ContainsKey(typeof(T)) && BitStreamCodec.byte_0 < 128)
			{
				BitStreamCodec.RegisterCodec(typeof(T), BitStreamCodec.byte_0 += 1, serializer, deserializer);
				return;
			}
		}

		public static void RegisterCodec(Type type, byte code, BitStreamCodec.SerializerHandler serializer, BitStreamCodec.DeserializerHandler deserializer)
		{
			if (BitStreamCodec.dictionary_0.ContainsKey(type))
			{
				return;
			}
			BitStreamCodec.dictionary_0[type] = new BitStreamCodec(type, code, serializer, deserializer);
		}

		private static void smethod_0(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<object>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_1(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<object>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_2(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<bool>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_3(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<bool>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_4(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<char>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_5(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<char>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_6(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<byte>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_7(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<byte>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_8(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<sbyte>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_9(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<sbyte>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_10(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<short>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_11(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<short>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_12(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<ushort>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_13(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<ushort>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_14(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<int>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_15(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<int>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_16(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<uint>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_17(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<uint>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_18(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<long>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_19(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<long>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_20(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<ulong>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_21(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<ulong>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_22(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<float>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_23(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<float>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_24(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<double>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_25(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<double>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_26(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<decimal>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_27(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<decimal>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_28(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<TimeSpan>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_29(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<TimeSpan>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_30(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<DateTime>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_31(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<DateTime>().method_1(bitStream_0, ref long_0);
		}

		private static void smethod_32(BitStream bitStream_0, object object_0, ref long long_0)
		{
			BitStreamCodec.GetCodec<string>().method_0(bitStream_0, object_0, ref long_0);
		}

		private static object smethod_33(BitStream bitStream_0, ref long long_0)
		{
			return BitStreamCodec.GetCodec<string>().method_1(bitStream_0, ref long_0);
		}

		private void method_0(BitStream bitStream_0, object object_0, ref long long_0)
		{
			bitStream_0.BaseStream.Position = long_0;
			if (bitStream_0.Serialization)
			{
				bitStream_0.BaseStream.WriteByte(this.Code);
			}
			byte[] array;
			switch (this.Code)
			{
			case 1:
				array = BitConverter.GetBytes(Convert.ToBoolean(object_0));
				break;
			case 2:
				array = new byte[]
				{
					Convert.ToByte(object_0)
				};
				break;
			case 3:
			case 4:
				array = new byte[]
				{
					Convert.ToByte(object_0)
				};
				break;
			case 5:
				array = BitConverter.GetBytes(Convert.ToInt16(object_0));
				break;
			case 6:
				array = BitConverter.GetBytes(Convert.ToUInt16(object_0));
				break;
			case 7:
				array = BitConverter.GetBytes(Convert.ToInt32(object_0));
				break;
			case 8:
				array = BitConverter.GetBytes(Convert.ToUInt32(object_0));
				break;
			case 9:
				array = BitConverter.GetBytes(Convert.ToInt64(object_0));
				break;
			case 10:
				array = BitConverter.GetBytes(Convert.ToUInt64(object_0));
				break;
			case 11:
				array = BitConverter.GetBytes(Convert.ToSingle(object_0));
				break;
			case 12:
				array = BitConverter.GetBytes(Convert.ToDouble(object_0));
				break;
			case 13:
				array = Convert.ToDecimal(object_0).GetBytes();
				break;
			case 14:
				array = BitConverter.GetBytes(((TimeSpan)object_0).Ticks);
				break;
			case 15:
				array = BitConverter.GetBytes(((DateTime)object_0).ToBinary());
				break;
			case 16:
			{
				array = Encoding.UTF8.GetBytes((string)object_0);
				byte[] bytes = BitConverter.GetBytes(array.Length);
				bitStream_0.BaseStream.Write(bytes, 0, bytes.Length);
				break;
			}
			default:
				array = new byte[0];
				break;
			}
			bitStream_0.BaseStream.Write(array, 0, array.Length);
			long_0 = bitStream_0.BaseStream.Position;
		}

		private object method_1(BitStream bitStream_0, ref long long_0)
		{
			bitStream_0.BaseStream.Position = long_0;
			object result = new object();
			byte b = this.Code;
			if (bitStream_0.Serialization)
			{
				b = (byte)bitStream_0.BaseStream.ReadByte();
				if (b != this.Code)
				{
					return new object();
				}
			}
			switch (this.Code)
			{
			case 1:
			{
				byte[] array = new byte[1];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToBoolean(array, 0);
				break;
			}
			case 2:
				result = Convert.ToChar(bitStream_0.BaseStream.ReadByte());
				break;
			case 3:
				result = Convert.ToSByte(bitStream_0.BaseStream.ReadByte());
				break;
			case 4:
				result = Convert.ToByte(bitStream_0.BaseStream.ReadByte());
				break;
			case 5:
			{
				byte[] array = new byte[2];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToInt16(array, 0);
				break;
			}
			case 6:
			{
				byte[] array = new byte[2];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToUInt16(array, 0);
				break;
			}
			case 7:
			{
				byte[] array = new byte[4];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToInt32(array, 0);
				break;
			}
			case 8:
			{
				byte[] array = new byte[4];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToUInt32(array, 0);
				break;
			}
			case 9:
			{
				byte[] array = new byte[8];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToInt64(array, 0);
				break;
			}
			case 10:
			{
				byte[] array = new byte[8];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToUInt64(array, 0);
				break;
			}
			case 11:
			{
				byte[] array = new byte[4];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToSingle(array, 0);
				break;
			}
			case 12:
			{
				byte[] array = new byte[8];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = BitConverter.ToDouble(array, 0);
				break;
			}
			case 13:
			{
				byte[] array = new byte[16];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = array.ToDecimal();
				break;
			}
			case 14:
			{
				byte[] array = new byte[8];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = TimeSpan.FromTicks(BitConverter.ToInt64(array, 0));
				break;
			}
			case 15:
			{
				byte[] array = new byte[8];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = DateTime.FromBinary(BitConverter.ToInt64(array, 0));
				break;
			}
			case 16:
			{
				byte[] array = new byte[4];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				array = new byte[BitConverter.ToInt32(array, 0)];
				bitStream_0.BaseStream.Read(array, 0, array.Length);
				result = Encoding.UTF8.GetString(array);
				break;
			}
			}
			long_0 = bitStream_0.BaseStream.Position;
			return result;
		}
	}
}
