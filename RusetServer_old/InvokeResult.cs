using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct InvokeResult
{
	private static object Result;
	private static Type ResultType;
	public Type Type
	{
		get
		{
			return InvokeResult.ResultType;
		}
	}
	public bool IsNull
	{
		get
		{
			return object.Equals(InvokeResult.Result, null);
		}
	}
	public DateTime AsDateTime
	{
		get
		{
			return Convert.ToDateTime(InvokeResult.Result);
		}
	}
	public decimal AsDecimal
	{
		get
		{
			return Convert.ToDecimal(InvokeResult.Result);
		}
	}
	public bool AsBoolean
	{
		get
		{
			return Convert.ToBoolean(InvokeResult.Result);
		}
	}
	public string AsString
	{
		get
		{
			return Convert.ToString(InvokeResult.Result);
		}
	}
	public double AsDouble
	{
		get
		{
			return Convert.ToDouble(InvokeResult.Result);
		}
	}
	public float AsSingle
	{
		get
		{
			return Convert.ToSingle(InvokeResult.Result);
		}
	}
	public float AsFloat
	{
		get
		{
			return Convert.ToSingle(InvokeResult.Result);
		}
	}
	public ulong AsUInt64
	{
		get
		{
			return Convert.ToUInt64(InvokeResult.Result);
		}
	}
	public long AsInt64
	{
		get
		{
			return Convert.ToInt64(InvokeResult.Result);
		}
	}
	public uint AsUInt
	{
		get
		{
			return Convert.ToUInt32(InvokeResult.Result);
		}
	}
	public int AsInt
	{
		get
		{
			return Convert.ToInt32(InvokeResult.Result);
		}
	}
	public sbyte AsSByte
	{
		get
		{
			return Convert.ToSByte(InvokeResult.Result);
		}
	}
	public byte AsByte
	{
		get
		{
			return Convert.ToByte(InvokeResult.Result);
		}
	}
	public char AsChar
	{
		get
		{
			return Convert.ToChar(InvokeResult.Result);
		}
	}
	public object AsObject
	{
		get
		{
			return InvokeResult.Result;
		}
	}
	public string[] AsStrings
	{
		get
		{
			if (!this.IsNull && InvokeResult.Result is string[])
			{
				return InvokeResult.Result as string[];
			}
			return null;
		}
	}
	public byte[] AsBytes
	{
		get
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, InvokeResult.Result);
				result = memoryStream.ToArray();
			}
			return result;
		}
	}
	public InvokeResult(object value, Type type)
	{
		InvokeResult.Result = value;
		InvokeResult.ResultType = type;
	}
	public T AsType<T>()
	{
		return (T)((object)InvokeResult.Result);
	}
}
