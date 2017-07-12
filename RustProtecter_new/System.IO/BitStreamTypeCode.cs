using System;

namespace System.IO
{
	public enum BitStreamTypeCode : byte
	{
		Undefined,
		Boolean,
		Char,
		SByte,
		Byte,
		Int16,
		UInt16,
		Int32,
		UInt32,
		Int64,
		UInt64,
		Single,
		Double,
		Decimal,
		TimeSpan,
		DateTime,
		String,
		ArrayType = 128,
		ArrayTypeMax = 255,
		MaxValue = 255
	}
}
