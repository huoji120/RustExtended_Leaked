using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

internal class Class3
{
	internal class Attribute0 : Attribute
	{
		internal class Class4<NyHorW72D1l5d0uLde>
		{
		}

		[Class3.Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
		public Attribute0(object object_0)
		{
		}
	}

	internal class Class5
	{
		[Class3.Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
		internal static void ce4DmfsmSrOT856tDgfrkMb()
		{
			if (!(Class3.Class5.smethod_0(Convert.ToBase64String(Class3.assembly_0.GetName().GetPublicKeyToken()), Class3.smethod_10(5458)) != Class3.smethod_10(5464)))
			{
				return;
			}
			while (true)
			{
				Class3.Class5.ce4DmfsmSrOT856tDgfrkMb();
			}
		}

		[Class3.Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
		internal static string smethod_0(string string_0, string string_1)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(string_0);
			byte[] array = bytes;
			byte[] key = new byte[]
			{
				82,
				102,
				104,
				110,
				32,
				77,
				24,
				34,
				118,
				181,
				51,
				17,
				18,
				51,
				12,
				109,
				10,
				32,
				77,
				24,
				34,
				158,
				161,
				41,
				97,
				28,
				118,
				181,
				5,
				25,
				1,
				88
			};
			byte[] iV = Class3.smethod_8(Encoding.Unicode.GetBytes(string_1));
			MemoryStream memoryStream = new MemoryStream();
			SymmetricAlgorithm symmetricAlgorithm = Class3.smethod_6();
			symmetricAlgorithm.Key = key;
			symmetricAlgorithm.IV = iV;
			CryptoStream cryptoStream = new CryptoStream(memoryStream, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.Close();
			return Convert.ToBase64String(memoryStream.ToArray());
		}
	}

	private static Assembly assembly_0;

	private static uint[] uint_0;

	private static byte[] byte_0;

	private static byte[] byte_1;

	private static string[] string_0;

	private static int[] int_0;

	private static bool bool_0;

	private static byte[] byte_2;

	private static int int_1;

	private static SortedList sortedList_0;

	private static bool bool_1;

	private static IntPtr intptr_0;

	private static long long_0;

	private static byte[] byte_3;

	private static IntPtr intptr_1;

	private static bool bool_2;

	private static int int_2;

	static Class3()
	{
		Class3.assembly_0 = typeof(Class3).Assembly;
		Class3.uint_0 = new uint[]
		{
			3614090360u,
			3905402710u,
			606105819u,
			3250441966u,
			4118548399u,
			1200080426u,
			2821735955u,
			4249261313u,
			1770035416u,
			2336552879u,
			4294925233u,
			2304563134u,
			1804603682u,
			4254626195u,
			2792965006u,
			1236535329u,
			4129170786u,
			3225465664u,
			643717713u,
			3921069994u,
			3593408605u,
			38016083u,
			3634488961u,
			3889429448u,
			568446438u,
			3275163606u,
			4107603335u,
			1163531501u,
			2850285829u,
			4243563512u,
			1735328473u,
			2368359562u,
			4294588738u,
			2272392833u,
			1839030562u,
			4259657740u,
			2763975236u,
			1272893353u,
			4139469664u,
			3200236656u,
			681279174u,
			3936430074u,
			3572445317u,
			76029189u,
			3654602809u,
			3873151461u,
			530742520u,
			3299628645u,
			4096336452u,
			1126891415u,
			2878612391u,
			4237533241u,
			1700485571u,
			2399980690u,
			4293915773u,
			2240044497u,
			1873313359u,
			4264355552u,
			2734768916u,
			1309151649u,
			4149444226u,
			3174756917u,
			718787259u,
			3951481745u
		};
		Class3.bool_1 = false;
		Class3.bool_2 = false;
		Class3.byte_0 = new byte[0];
		Class3.byte_2 = new byte[0];
		Class3.byte_3 = new byte[0];
		Class3.byte_1 = new byte[0];
		Class3.intptr_1 = IntPtr.Zero;
		Class3.intptr_0 = IntPtr.Zero;
		Class3.string_0 = new string[0];
		Class3.int_0 = new int[0];
		Class3.int_2 = 1;
		Class3.bool_0 = false;
		Class3.sortedList_0 = new SortedList();
		Class3.int_1 = 0;
		Class3.long_0 = 0L;
	}

	private void aWdHwaGPYEwVj()
	{
	}

    internal static byte[] smethod_0(byte[] object_0)
	{
        uint[] array = new uint[16];
		int num = 448 - object_0.Length * 8 % 512;
		uint num2 = (uint)((num + 512) % 512);
		if (num2 == 0u)
		{
			num2 = 512u;
		}
		uint num3 = (uint)((long)object_0.Length + (long)((ulong)(num2 / 8u)) + 8L);
		ulong num4 = (ulong)((long)object_0.Length * 8L);
		byte[] array2 = new byte[num3];
		for (int i = 0; i < object_0.Length; i++)
		{
			array2[i] = (byte)(object_0[i]);
		}
		byte[] expr_77_cp_0 = array2;
		int expr_77_cp_1 = object_0.Length;
		expr_77_cp_0[expr_77_cp_1] |= 128;
		for (int j = 8; j > 0; j--)
		{
			array2[(int)(checked((IntPtr)(unchecked((ulong)num3 - (ulong)((long)j)))))] = (byte)(num4 >> (8 - j) * 8 & 255uL);
		}
		uint num5 = (uint)(array2.Length * 8 / 32);
		uint num6 = 1732584193u;
		uint num7 = 4023233417u;
		uint num8 = 2562383102u;
		uint num9 = 271733878u;
		for (uint num10 = 0u; num10 < num5 / 16u; num10 += 1u)
		{
			uint num11 = num10 << 6;
			for (uint num12 = 0u; num12 < 61u; num12 += 4u)
			{
				array[(int)((UIntPtr)(num12 >> 2))] = (uint)((int)array2[(int)((UIntPtr)(num11 + (num12 + 3u)))] << 24 | (int)array2[(int)((UIntPtr)(num11 + (num12 + 2u)))] << 16 | (int)array2[(int)((UIntPtr)(num11 + (num12 + 1u)))] << 8 | (int)array2[(int)((UIntPtr)(num11 + num12))]);
			}
			uint num13 = num6;
			uint num14 = num7;
			uint num15 = num8;
			uint num16 = num9;


            Class3.smethod_1(ref num6, num7, num8, num9, 0u, 7, 1u, array);
			Class3.smethod_1(ref num9, num6, num7, num8, 1u, 12, 2u, array);
			Class3.smethod_1(ref num8, num9, num6, num7, 2u, 17, 3u, array);
			Class3.smethod_1(ref num7, num8, num9, num6, 3u, 22, 4u, array);
			Class3.smethod_1(ref num6, num7, num8, num9, 4u, 7, 5u, array);
			Class3.smethod_1(ref num9, num6, num7, num8, 5u, 12, 6u, array);
			Class3.smethod_1(ref num8, num9, num6, num7, 6u, 17, 7u, array);
			Class3.smethod_1(ref num7, num8, num9, num6, 7u, 22, 8u, array);
			Class3.smethod_1(ref num6, num7, num8, num9, 8u, 7, 9u, array);
			Class3.smethod_1(ref num9, num6, num7, num8, 9u, 12, 10u, array);
			Class3.smethod_1(ref num8, num9, num6, num7, 10u, 17, 11u, array);
			Class3.smethod_1(ref num7, num8, num9, num6, 11u, 22, 12u, array);
			Class3.smethod_1(ref num6, num7, num8, num9, 12u, 7, 13u, array);
			Class3.smethod_1(ref num9, num6, num7, num8, 13u, 12, 14u, array);
			Class3.smethod_1(ref num8, num9, num6, num7, 14u, 17, 15u, array);
			Class3.smethod_1(ref num7, num8, num9, num6, 15u, 22, 16u, array);
			Class3.smethod_2(ref num6, num7, num8, num9, 1u, 5, 17u, array);
			Class3.smethod_2(ref num9, num6, num7, num8, 6u, 9, 18u, array);
			Class3.smethod_2(ref num8, num9, num6, num7, 11u, 14, 19u, array);
			Class3.smethod_2(ref num7, num8, num9, num6, 0u, 20, 20u, array);
			Class3.smethod_2(ref num6, num7, num8, num9, 5u, 5, 21u, array);
			Class3.smethod_2(ref num9, num6, num7, num8, 10u, 9, 22u, array);
			Class3.smethod_2(ref num8, num9, num6, num7, 15u, 14, 23u, array);
			Class3.smethod_2(ref num7, num8, num9, num6, 4u, 20, 24u, array);
			Class3.smethod_2(ref num6, num7, num8, num9, 9u, 5, 25u, array);
			Class3.smethod_2(ref num9, num6, num7, num8, 14u, 9, 26u, array);
			Class3.smethod_2(ref num8, num9, num6, num7, 3u, 14, 27u, array);
			Class3.smethod_2(ref num7, num8, num9, num6, 8u, 20, 28u, array);
			Class3.smethod_2(ref num6, num7, num8, num9, 13u, 5, 29u, array);
			Class3.smethod_2(ref num9, num6, num7, num8, 2u, 9, 30u, array);
			Class3.smethod_2(ref num8, num9, num6, num7, 7u, 14, 31u, array);
			Class3.smethod_2(ref num7, num8, num9, num6, 12u, 20, 32u, array);
			Class3.smethod_3(ref num6, num7, num8, num9, 5u, 4, 33u, array);
			Class3.smethod_3(ref num9, num6, num7, num8, 8u, 11, 34u, array);
			Class3.smethod_3(ref num8, num9, num6, num7, 11u, 16, 35u, array);
			Class3.smethod_3(ref num7, num8, num9, num6, 14u, 23, 36u, array);
			Class3.smethod_3(ref num6, num7, num8, num9, 1u, 4, 37u, array);
			Class3.smethod_3(ref num9, num6, num7, num8, 4u, 11, 38u, array);
			Class3.smethod_3(ref num8, num9, num6, num7, 7u, 16, 39u, array);
			Class3.smethod_3(ref num7, num8, num9, num6, 10u, 23, 40u, array);
			Class3.smethod_3(ref num6, num7, num8, num9, 13u, 4, 41u, array);
			Class3.smethod_3(ref num9, num6, num7, num8, 0u, 11, 42u, array);
			Class3.smethod_3(ref num8, num9, num6, num7, 3u, 16, 43u, array);
			Class3.smethod_3(ref num7, num8, num9, num6, 6u, 23, 44u, array);
			Class3.smethod_3(ref num6, num7, num8, num9, 9u, 4, 45u, array);
			Class3.smethod_3(ref num9, num6, num7, num8, 12u, 11, 46u, array);
			Class3.smethod_3(ref num8, num9, num6, num7, 15u, 16, 47u, array);
			Class3.smethod_3(ref num7, num8, num9, num6, 2u, 23, 48u, array);
			Class3.ywnZmcpthG(ref num6, num7, num8, num9, 0u, 6, 49u, array);
			Class3.ywnZmcpthG(ref num9, num6, num7, num8, 7u, 10, 50u, array);
			Class3.ywnZmcpthG(ref num8, num9, num6, num7, 14u, 15, 51u, array);
			Class3.ywnZmcpthG(ref num7, num8, num9, num6, 5u, 21, 52u, array);
			Class3.ywnZmcpthG(ref num6, num7, num8, num9, 12u, 6, 53u, array);
			Class3.ywnZmcpthG(ref num9, num6, num7, num8, 3u, 10, 54u, array);
			Class3.ywnZmcpthG(ref num8, num9, num6, num7, 10u, 15, 55u, array);
			Class3.ywnZmcpthG(ref num7, num8, num9, num6, 1u, 21, 56u, array);
			Class3.ywnZmcpthG(ref num6, num7, num8, num9, 8u, 6, 57u, array);
			Class3.ywnZmcpthG(ref num9, num6, num7, num8, 15u, 10, 58u, array);
			Class3.ywnZmcpthG(ref num8, num9, num6, num7, 6u, 15, 59u, array);
			Class3.ywnZmcpthG(ref num7, num8, num9, num6, 13u, 21, 60u, array);
			Class3.ywnZmcpthG(ref num6, num7, num8, num9, 4u, 6, 61u, array);
			Class3.ywnZmcpthG(ref num9, num6, num7, num8, 11u, 10, 62u, array);
			Class3.ywnZmcpthG(ref num8, num9, num6, num7, 2u, 15, 63u, array);
			Class3.ywnZmcpthG(ref num7, num8, num9, num6, 9u, 21, 64u, array);
			num6 += num13;
			num7 += num14;
			num8 += num15;
			num9 += num16;
		}
		byte[] array3 = new byte[16];
		Array.Copy(BitConverter.GetBytes(num6), 0, array3, 0, 4);
		Array.Copy(BitConverter.GetBytes(num7), 0, array3, 4, 4);
		Array.Copy(BitConverter.GetBytes(num8), 0, array3, 8, 4);
		Array.Copy(BitConverter.GetBytes(num9), 0, array3, 12, 4);
		return array3;
	}

	private static void smethod_1(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] object_0)
	{
	    uint_1 = uint_2 + Class3.smethod_4(uint_1 + ((uint_2 & uint_3) | (~uint_2 & uint_4)) + (uint)(object_0[(int)((UIntPtr)uint_5)]) + Class3.uint_0[(int)((UIntPtr)(uint_6 - 1u))], ushort_0);
	}

	private static void smethod_2(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] object_0)
	{
		uint_1 = uint_2 + Class3.smethod_4(uint_1 + ((uint_2 & uint_4) | (uint_3 & ~uint_4)) + (uint)(object_0[(int)((UIntPtr)uint_5)]) + Class3.uint_0[(int)((UIntPtr)(uint_6 - 1u))], ushort_0);
	}

	private static void smethod_3(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] object_0)
	{
		uint_1 = uint_2 + Class3.smethod_4(uint_1 + (uint_2 ^ uint_3 ^ uint_4) + (uint)(object_0[(int)((UIntPtr)uint_5)]) + Class3.uint_0[(int)((UIntPtr)(uint_6 - 1u))], ushort_0);
	}

	private static void ywnZmcpthG(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] object_0)
	{
		uint_1 = uint_2 + Class3.smethod_4(uint_1 + (uint_3 ^ (uint_2 | ~uint_4)) + (uint)(object_0[(int)((UIntPtr)uint_5)]) + Class3.uint_0[(int)((UIntPtr)(uint_6 - 1u))], ushort_0);
	}

	private static uint smethod_4(uint uint_1, ushort ushort_0)
	{
		return uint_1 >> (int)(32 - ushort_0) | uint_1 << (int)ushort_0;
	}

	internal static bool smethod_5()
	{
		if (!Class3.bool_1)
		{
			Class3.smethod_7();
			Class3.bool_1 = true;
		}
		return Class3.bool_2;
	}

	internal static SymmetricAlgorithm smethod_6()
	{
		SymmetricAlgorithm result = null;
		if (Class3.smethod_5())
		{
			try
			{
				result = (SymmetricAlgorithm)Activator.CreateInstance("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Security.Cryptography.AesCryptoServiceProvider").Unwrap();
				return result;
			}
			catch
			{
				result = new RijndaelManaged();
				return result;
			}
		}
		try
		{
			result = new RijndaelManaged();
		}
		catch
		{
			result = (SymmetricAlgorithm)Activator.CreateInstance("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Security.Cryptography.AesCryptoServiceProvider").Unwrap();
		}
		return result;
	}

	internal static void smethod_7()
	{
		try
		{
			new RijndaelManaged();
			Class3.bool_2 = false;
		}
		catch
		{
			Class3.bool_2 = true;
		}
	}

	internal static byte[] smethod_8(byte[] byte_4)
	{
		if (!Class3.smethod_5())
		{
			return new MD5CryptoServiceProvider().ComputeHash(byte_4);
		}
		return Class3.smethod_0(byte_4);
	}

	private static uint smethod_9(uint uint_1)
	{
		return (uint)"{11111-22222-10009-11112}".Length;
	}

	[Class3.Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
    internal static string smethod_10(int int_3)
    {
        if (byte_3.Length == 0)
        {
            BinaryReader reader = new BinaryReader(assembly_0.GetManifestResourceStream("060Hjua3VcYYofB8kc.B8mJIhm13YQC35FDEm"))
            {
                BaseStream = { Position = 0L }
            };
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            byte[] buffer = reader.ReadBytes((int)reader.BaseStream.Length);
            reader.Close();
            byte[] buffer5 = new byte[0x20];
            buffer5[0] = 0x95;
            buffer5[0] = 0x69;
            buffer5[0] = 0x88;
            buffer5[0] = (byte)(0xa3 + 0x16);
            buffer5[1] = (byte)(0xdf - 0x4a);
            buffer5[1] = 0x7c;
            buffer5[1] = 12;
            buffer5[2] = 0xa4;
            buffer5[2] = (byte)(230 - 0x4c);
            buffer5[2] = (byte)(0x91 - 0x30);
            buffer5[2] = (byte)(0xbf - 0x3f);
            buffer5[2] = (byte)(0x87 + 0x2e);
            buffer5[3] = (byte)(0xf3 - 0x51);
            buffer5[3] = (byte)(0xdd - 0x49);
            buffer5[3] = (byte)(50 + 0x7a);
            buffer5[3] = (byte)(0xa1 - 0x35);
            buffer5[3] = 0x6d;
            buffer5[3] = (byte)(0x2c - 4);
            buffer5[4] = (byte)(0xe1 - 0x4b);
            buffer5[4] = 0xc9;
            buffer5[4] = (byte)(0x3f + 0x5d);
            buffer5[4] = 0x37;
            buffer5[5] = 0x70;
            buffer5[5] = 0xa6;
            buffer5[5] = 0x99;
            buffer5[5] = (byte)(0x9f - 0x35);
            buffer5[5] = 0xc0;
            buffer5[6] = 0x71;
            buffer5[6] = (byte)(0xb9 - 0x3d);
            buffer5[6] = (byte)(4 + 0x59);
            buffer5[6] = (byte)(140 - 0x2e);
            buffer5[6] = 0x52;
            buffer5[6] = (byte)(0x6b + 0x65);
            buffer5[7] = (byte)(0xdb - 0x49);
            buffer5[7] = 150;
            buffer5[7] = 0x6f;
            buffer5[7] = (byte)(0x94 - 0x47);
            buffer5[8] = 0x8a;
            buffer5[8] = 100;
            buffer5[8] = (byte)(0xbc - 0x3e);
            buffer5[8] = (byte)(0x94 + 0x1c);
            buffer5[9] = (byte)(0x58 + 0x77);
            buffer5[9] = 0x8b;
            buffer5[9] = (byte)(0x5d + 2);
            buffer5[9] = 0x86;
            buffer5[10] = (byte)(0xde - 0x4a);
            buffer5[10] = (byte)(11 + 0x1b);
            buffer5[10] = 0x9a;
            buffer5[10] = 0xc6;
            buffer5[11] = (byte)(0x6b + 7);
            buffer5[11] = (byte)(0x13 + 0x39);
            buffer5[11] = 0x87;
            buffer5[11] = (byte)(0xb8 - 0x3d);
            buffer5[11] = 0xda;
            buffer5[12] = (byte)(0xa4 - 0x36);
            buffer5[12] = 180;
            buffer5[12] = 0x8b;
            buffer5[12] = 0x9a;
            buffer5[13] = 0x47;
            buffer5[13] = 0x6a;
            buffer5[13] = (byte)(0x26 + 0x40);
            buffer5[13] = 0xb6;
            buffer5[14] = (byte)(0x1f + 0x65);
            buffer5[14] = 0x9e;
            buffer5[14] = (byte)(0x23 + 0x10);
            buffer5[14] = 0x1c;
            buffer5[15] = (byte)(0x5c + 0x67);
            buffer5[15] = (byte)(0xab - 0x39);
            buffer5[15] = 0xcd;
            buffer5[0x10] = 0x65;
            buffer5[0x10] = 0x91;
            buffer5[0x10] = (byte)(0x68 - 0x2b);
            buffer5[0x11] = (byte)(0xab - 0x39);
            buffer5[0x11] = 0x57;
            buffer5[0x11] = 0xad;
            buffer5[0x11] = (byte)(0x9a - 0x33);
            buffer5[0x11] = 0x34;
            buffer5[0x12] = (byte)(0x86 - 0x2c);
            buffer5[0x12] = 0x8e;
            buffer5[0x12] = (byte)(0xb9 + 1);
            buffer5[0x13] = 0x4b;
            buffer5[0x13] = 0x37;
            buffer5[0x13] = (byte)(0x16 + 0x3a);
            buffer5[0x13] = 0xba;
            buffer5[0x13] = 0xc6;
            buffer5[0x13] = 0x7f;
            buffer5[20] = (byte)(0x7e - 0x2a);
            buffer5[20] = (byte)(0x1a + 0x4a);
            buffer5[20] = (byte)(0xbb - 0x3e);
            buffer5[20] = 70;
            buffer5[20] = (byte)(0x48 + 0x36);
            buffer5[0x15] = (byte)(0x77 + 0x1b);
            buffer5[0x15] = (byte)(0xb1 - 0x3b);
            buffer5[0x15] = 0x73;
            buffer5[0x15] = 0xab;
            buffer5[0x16] = (byte)(6 + 0x38);
            buffer5[0x16] = (byte)(0xd8 - 0x48);
            buffer5[0x16] = 0xd3;
            buffer5[0x17] = (byte)(0xce - 0x44);
            buffer5[0x17] = 0x6f;
            buffer5[0x17] = (byte)(0x40 + 0x60);
            buffer5[0x17] = (byte)(0xb7 + 0x39);
            buffer5[0x18] = (byte)(0xd7 - 0x47);
            buffer5[0x18] = 130;
            buffer5[0x18] = (byte)(0x51 - 0x2e);
            buffer5[0x19] = 0x89;
            buffer5[0x19] = 0x80;
            buffer5[0x19] = 0x5e;
            buffer5[0x19] = 0x84;
            buffer5[0x19] = (byte)(0x29 + 0x72);
            buffer5[0x1a] = (byte)(0x7e - 0x2a);
            buffer5[0x1a] = 0x2c;
            buffer5[0x1a] = (byte)(0x76 + 0x36);
            buffer5[0x1a] = 0x16;
            buffer5[0x1b] = 0x4b;
            buffer5[0x1b] = 0x7e;
            buffer5[0x1b] = 0x8d;
            buffer5[0x1b] = 0x90;
            buffer5[0x1b] = 0x9a;
            buffer5[0x1b] = 90;
            buffer5[0x1c] = 0x9e;
            buffer5[0x1c] = 0x84;
            buffer5[0x1c] = 0x61;
            buffer5[0x1d] = 0x7c;
            buffer5[0x1d] = (byte)(0xfb - 0x53);
            buffer5[0x1d] = (byte)(0x1a + 0x51);
            buffer5[0x1d] = (byte)(0xac + 0x22);
            buffer5[30] = (byte)(0xe5 - 0x4c);
            buffer5[30] = (byte)(160 - 0x35);
            buffer5[30] = (byte)(130 - 0x2b);
            buffer5[30] = (byte)(0xc4 - 0x53);
            buffer5[0x1f] = (byte)(0x77 + 0x72);
            buffer5[0x1f] = 0x98;
            buffer5[0x1f] = 210;
            buffer5[0x1f] = (byte)(100 - 0x60);
            byte[] rgbKey = buffer5;
            byte[] buffer6 = new byte[0x10];
            buffer6[0] = 0x95;
            buffer6[0] = (byte)(30 + 0x67);
            buffer6[0] = 0x63;
            buffer6[0] = 0x95;
            buffer6[0] = (byte)(0x6f - 0x30);
            buffer6[1] = 0x81;
            buffer6[1] = (byte)(0xe3 - 0x4b);
            buffer6[1] = 0x80;
            buffer6[1] = (byte)(0x13 + 0x1c);
            buffer6[1] = 0x74;
            buffer6[1] = 4;
            buffer6[2] = 0x94;
            buffer6[2] = (byte)(50 + 0x7a);
            buffer6[2] = (byte)(0x22 + 0x3e);
            buffer6[2] = 0x33;
            buffer6[2] = (byte)(0x61 - 0x58);
            buffer6[3] = (byte)(0x39 + 0x5b);
            buffer6[3] = (byte)(0xd9 - 0x48);
            buffer6[3] = (byte)(0xde - 0x4a);
            buffer6[3] = 0x9a;
            buffer6[3] = 0x98;
            buffer6[3] = 0xae;
            buffer6[4] = (byte)(100 + 0x55);
            buffer6[4] = 0x5f;
            buffer6[4] = (byte)(0xa9 - 0x38);
            buffer6[4] = 0x7c;
            buffer6[4] = (byte)(4 + 0x59);
            buffer6[4] = (byte)(0x49 - 0x2b);
            buffer6[5] = 0x7c;
            buffer6[5] = 0x69;
            buffer6[5] = 0x7a;
            buffer6[5] = (byte)(0x70 + 90);
            buffer6[6] = (byte)(0x2a + 0x6c);
            buffer6[6] = 0x6f;
            buffer6[6] = 0x7a;
            buffer6[6] = (byte)(0 + 0x54);
            buffer6[6] = 0x60;
            buffer6[7] = 0x48;
            buffer6[7] = (byte)(0xf8 - 0x52);
            buffer6[7] = (byte)(0x4f + 60);
            buffer6[7] = 0x94;
            buffer6[7] = (byte)(0x6d - 2);
            buffer6[8] = (byte)(0x39 + 0x48);
            buffer6[8] = (byte)(0x4d + 11);
            buffer6[8] = 0x6c;
            buffer6[8] = (byte)(15 + 0);
            buffer6[8] = 0x51;
            buffer6[8] = 130;
            buffer6[9] = (byte)(0xca - 0x43);
            buffer6[9] = 0x7b;
            buffer6[9] = 0x8a;
            buffer6[9] = (byte)(0x26 + 0);
            buffer6[9] = 0x4d;
            buffer6[10] = (byte)(0x99 - 0x33);
            buffer6[10] = 0x67;
            buffer6[10] = 0x44;
            buffer6[11] = 0x8b;
            buffer6[11] = 0x5c;
            buffer6[11] = 0xc5;
            buffer6[12] = 0x60;
            buffer6[12] = 0x84;
            buffer6[12] = (byte)(0xec - 0x4e);
            buffer6[12] = 0x6c;
            buffer6[12] = (byte)(0x6b + 0x24);
            buffer6[12] = (byte)(0xba - 0x47);
            buffer6[13] = 0xd8;
            buffer6[13] = (byte)(0x1c + 0x3f);
            buffer6[13] = (byte)(0x97 - 50);
            buffer6[13] = (byte)(40 + 0x5c);
            buffer6[14] = 0x6a;
            buffer6[14] = (byte)(0xab - 0x39);
            buffer6[14] = 0x57;
            buffer6[15] = (byte)(0xea - 0x4e);
            buffer6[15] = 0xa8;
            buffer6[15] = 0x80;
            buffer6[15] = (byte)(0x49 - 0x10);
            byte[] array = buffer6;
            Array.Reverse(array);
            for (int i = 0; i < array.Length; i++)
            {
                rgbKey[i] = (byte)(rgbKey[i] ^ array[i]);
            }
            if (int_3 == -1)
            {
                SymmetricAlgorithm algorithm = smethod_6();
                algorithm.Mode = CipherMode.CBC;
                ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, array);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                byte_3 = stream.ToArray();
                stream.Close();
                stream2.Close();
                buffer = byte_3;
            }
            int num = buffer.Length % 4;
            int num9 = buffer.Length / 4;
            byte[] buffer3 = new byte[buffer.Length];
            int num17 = rgbKey.Length / 4;
            uint num6 = 0;
            uint num7 = 0;
            uint num5 = 0;
            if (num > 0)
            {
                num9++;
            }
            uint index = 0;
            for (int j = 0; j < num9; j++)
            {
                int num18 = j % num17;
                int num3 = j * 4;
                index = (uint)(num18 * 4);
                num7 = (uint)((((rgbKey[(int)((IntPtr)(index + 3))] << 0x18) | (rgbKey[(int)((IntPtr)(index + 2))] << 0x10)) | (rgbKey[(int)((IntPtr)(index + 1))] << 8)) | rgbKey[index]);
                uint num12 = 0xff;
                int num13 = 0;
                if ((j == (num9 - 1)) && (num > 0))
                {
                    num5 = 0;
                    for (int k = 0; k < num; k++)
                    {
                        if (k > 0)
                        {
                            num5 = num5 << 8;
                        }
                        num5 |= buffer[buffer.Length - (1 + k)];
                    }
                    num6 += num7;
                }
                else
                {
                    index = (uint)num3;
                    num5 = (uint)((((buffer[(int)((IntPtr)(index + 3))] << 0x18) | (buffer[(int)((IntPtr)(index + 2))] << 0x10)) | (buffer[(int)((IntPtr)(index + 1))] << 8)) | buffer[index]);
                    num6 += num7;
                }
                uint num19 = num6;
                num19 ^= num19 << 0x19;
                num19 += 0xb5d60c64;
                num19 ^= num19 << 2;
                num19 += 0xa07935c6;
                num19 ^= num19 >> 15;
                num19 += 0x36d9f688;
                num19 = ((uint)(((0x7f808fc8 << 11) - 0x7f808fc8) ^ -1602669114)) + num19;
                num6 += num19;
                if ((j == (num9 - 1)) && (num > 0))
                {
                    uint num11 = num6 ^ num5;
                    for (int m = 0; m < num; m++)
                    {
                        if (m > 0)
                        {
                            num12 = num12 << 8;
                            num13 += 8;
                        }
                        buffer3[num3 + m] = (byte)((num11 & num12) >> num13);
                    }
                }
                else
                {
                    uint num16 = num6 ^ num5;
                    buffer3[num3] = (byte)(num16 & 0xff);
                    buffer3[num3 + 1] = (byte)((num16 & 0xff00) >> 8);
                    buffer3[num3 + 2] = (byte)((num16 & 0xff0000) >> 0x10);
                    buffer3[num3 + 3] = (byte)((num16 & -16777216) >> 0x18);
                }
            }
            byte_3 = buffer3;
        }
        int count = BitConverter.ToInt32(byte_3, int_3);
        try
        {
            return Encoding.Unicode.GetString(byte_3, int_3 + 4, count);
        }
        catch
        {
        }
        return "";
    }
    [Attribute0(typeof(Attribute0.Class4<object>[]))]
   
    /*
	internal static string smethod_10(int int_3)
	{
		if (Class3.byte_3.Length == 0)
		{
			BinaryReader binaryReader = new BinaryReader(Class3.assembly_0.GetManifestResourceStream("060Hjua3VcYYofB8kc.B8mJIhm13YQC35FDEm"));
			binaryReader.BaseStream.Position = 0L;
			RSACryptoServiceProvider.UseMachineKeyStore = true;
			byte[] array = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
			binaryReader.Close();
			byte[] array2 = new byte[32];
			array2[0] = 149;
			array2[0] = 105;
			array2[0] = 136;
			array2[0] = 163 + 22;
			array2[1] = 223 - 74;
			array2[1] = 124;
			array2[1] = 12;
			array2[2] = 164;
			array2[2] = 230 - 76;
			array2[2] = 145 - 48;
			array2[2] = 191 - 63;
			array2[2] = 135 + 46;
			array2[3] = 243 - 81;
			array2[3] = 221 - 73;
			array2[3] = 50 + 122;
			array2[3] = 161 - 53;
			array2[3] = 109;
			array2[3] = 44 - 4;
			array2[4] = 225 - 75;
			array2[4] = 201;
			array2[4] = 63 + 93;
			array2[4] = 55;
			array2[5] = 112;
			array2[5] = 166;
			array2[5] = 153;
			array2[5] = 159 - 53;
			array2[5] = 192;
			array2[6] = 113;
			array2[6] = 185 - 61;
			array2[6] = 4 + 89;
			array2[6] = 140 - 46;
			array2[6] = 82;
			array2[6] = 107 + 101;
			array2[7] = 219 - 73;
			array2[7] = 150;
			array2[7] = 111;
			array2[7] = 148 - 71;
			array2[8] = 138;
			array2[8] = 100;
			array2[8] = 188 - 62;
			array2[8] = 148 + 28;
			array2[9] = 88 + 119;
			array2[9] = 139;
			array2[9] = 93 + 2;
			array2[9] = 134;
			array2[10] = 222 - 74;
			array2[10] = 11 + 27;
			array2[10] = 154;
			array2[10] = 198;
			array2[11] = 107 + 7;
			array2[11] = 19 + 57;
			array2[11] = 135;
			array2[11] = 184 - 61;
			array2[11] = 218;
			array2[12] = 164 - 54;
			array2[12] = 180;
			array2[12] = 139;
			array2[12] = 154;
			array2[13] = 71;
			array2[13] = 106;
			array2[13] = 38 + 64;
			array2[13] = 182;
			array2[14] = 31 + 101;
			array2[14] = 158;
			array2[14] = 35 + 16;
			array2[14] = 28;
			array2[15] = 92 + 103;
			array2[15] = 171 - 57;
			array2[15] = 205;
			array2[16] = 101;
			array2[16] = 145;
			array2[16] = 104 - 43;
			array2[17] = 171 - 57;
			array2[17] = 87;
			array2[17] = 173;
			array2[17] = 154 - 51;
			array2[17] = 52;
			array2[18] = 134 - 44;
			array2[18] = 142;
			array2[18] = 185 + 1;
			array2[19] = 75;
			array2[19] = 55;
			array2[19] = 22 + 58;
			array2[19] = 186;
			array2[19] = 198;
			array2[19] = 127;
			array2[20] = 126 - 42;
			array2[20] = 26 + 74;
			array2[20] = 187 - 62;
			array2[20] = 70;
			array2[20] = 72 + 54;
			array2[21] = 119 + 27;
			array2[21] = 177 - 59;
			array2[21] = 115;
			array2[21] = 171;
			array2[22] = 6 + 56;
			array2[22] = 216 - 72;
			array2[22] = 211;
			array2[23] = 206 - 68;
			array2[23] = 111;
			array2[23] = 64 + 96;
			array2[23] = 183 + 57;
			array2[24] = 215 - 71;
			array2[24] = 130;
			array2[24] = 81 - 46;
			array2[25] = 137;
			array2[25] = 128;
			array2[25] = 94;
			array2[25] = 132;
			array2[25] = 41 + 114;
			array2[26] = 126 - 42;
			array2[26] = 44;
			array2[26] = 118 + 54;
			array2[26] = 22;
			array2[27] = 75;
			array2[27] = 126;
			array2[27] = 141;
			array2[27] = 144;
			array2[27] = 154;
			array2[27] = 90;
			array2[28] = 158;
			array2[28] = 132;
			array2[28] = 97;
			array2[29] = 124;
			array2[29] = 251 - 83;
			array2[29] = 26 + 81;
			array2[29] = 172 + 34;
			array2[30] = 229 - 76;
			array2[30] = 160 - 53;
			array2[30] = 130 - 43;
			array2[30] = 196 - 83;
			array2[31] = 119 + 114;
			array2[31] = 152;
			array2[31] = 210;
			array2[31] = 100 - 96;
			byte[] array3 = array2;
			byte[] array4 = new byte[16];
			array4[0] = 149;
			array4[0] = 30 + 103;
			array4[0] = 99;
			array4[0] = 149;
			array4[0] = 111 - 48;
			array4[1] = 129;
			array4[1] = 227 - 75;
			array4[1] = 128;
			array4[1] = 19 + 28;
			array4[1] = 116;
			array4[1] = 4;
			array4[2] = 148;
			array4[2] = 50 + 122;
			array4[2] = 34 + 62;
			array4[2] = 51;
			array4[2] = 97 - 88;
			array4[3] = 57 + 91;
			array4[3] = 217 - 72;
			array4[3] = 222 - 74;
			array4[3] = 154;
			array4[3] = 152;
			array4[3] = 174;
			array4[4] = 100 + 85;
			array4[4] = 95;
			array4[4] = 169 - 56;
			array4[4] = 124;
			array4[4] = 4 + 89;
			array4[4] = 73 - 43;
			array4[5] = 124;
			array4[5] = 105;
			array4[5] = 122;
			array4[5] = 112 + 90;
			array4[6] = 42 + 108;
			array4[6] = 111;
			array4[6] = 122;
			array4[6] = 0 + 84;
			array4[6] = 96;
			array4[7] = 72;
			array4[7] = 248 - 82;
			array4[7] = 79 + 60;
			array4[7] = 148;
			array4[7] = 109 - 2;
			array4[8] = 57 + 72;
			array4[8] = 77 + 11;
			array4[8] = 108;
			array4[8] = 15 + 0;
			array4[8] = 81;
			array4[8] = 130;
			array4[9] = 202 - 67;
			array4[9] = 123;
			array4[9] = 138;
			array4[9] = 38 + 0;
			array4[9] = 77;
			array4[10] = 153 - 51;
			array4[10] = 103;
			array4[10] = 68;
			array4[11] = 139;
			array4[11] = 92;
			array4[11] = 197;
			array4[12] = 96;
			array4[12] = 132;
			array4[12] = 236 - 78;
			array4[12] = 108;
			array4[12] = 107 + 36;
			array4[12] = 186 - 71;
			array4[13] = 216;
			array4[13] = 28 + 63;
			array4[13] = 151 - 50;
			array4[13] = 40 + 92;
			array4[14] = 106;
			array4[14] = 171 - 57;
			array4[14] = 87;
			array4[15] = 234 - 78;
			array4[15] = 168;
			array4[15] = 128;
			array4[15] = 73 - 16;
			byte[] array5 = array4;
			Array.Reverse(array5);
			for (int i = 0; i < array5.Length; i++)
			{
				array3[i] ^= array5[i];
			}
			if (int_3 == -1)
			{
				SymmetricAlgorithm symmetricAlgorithm = Class3.smethod_6();
				symmetricAlgorithm.Mode = CipherMode.CBC;
				ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(array3, array5);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				Class3.byte_3 = memoryStream.ToArray();
				memoryStream.Close();
				cryptoStream.Close();
				array = Class3.byte_3;
			}
			int num = array.Length % 4;
			int num2 = array.Length / 4;
			byte[] array6 = new byte[array.Length];
			int num3 = array3.Length / 4;
			uint num4 = 0u;
			if (num > 0)
			{
				num2++;
			}
			for (int j = 0; j < num2; j++)
			{
				int num5 = j % num3;
				int num6 = j * 4;
				uint num7 = (uint)(num5 * 4);
				uint num8 = (uint)((int)array3[(int)((UIntPtr)(num7 + 3u))] << 24 | (int)array3[(int)((UIntPtr)(num7 + 2u))] << 16 | (int)array3[(int)((UIntPtr)(num7 + 1u))] << 8 | (int)array3[(int)((UIntPtr)num7)]);
				uint num9 = 255u;
				int num10 = 0;
				uint num11;
				if (j == num2 - 1 && num > 0)
				{
					num11 = 0u;
					for (int k = 0; k < num; k++)
					{
						if (k > 0)
						{
							num11 <<= 8;
						}
						num11 |= (uint)array[array.Length - (1 + k)];
					}
					num4 += num8;
				}
				else
				{
					num7 = (uint)num6;
					num11 = (uint)((int)array[(int)((UIntPtr)(num7 + 3u))] << 24 | (int)array[(int)((UIntPtr)(num7 + 2u))] << 16 | (int)array[(int)((UIntPtr)(num7 + 1u))] << 8 | (int)array[(int)((UIntPtr)num7)]);
					num4 += num8;
				}
				uint arg_ECD_0 = num4;
				uint num12 = num4;
				num12 ^= num12 << 25;
				num12 += 3050703972u;
				num12 ^= num12 << 2;
				num12 += 2692298182u;
				num12 ^= num12 >> 15;
				num12 += 920254088u;
				num12 = ((0x7f808fc8 << 11) - 0x7f808fc8 ^ 2692298182u) + num12;
				num4 = arg_ECD_0 + num12;
				if (j == num2 - 1 && num > 0)
				{
					uint num13 = num4 ^ num11;
					for (int l = 0; l < num; l++)
					{
						if (l > 0)
						{
							num9 <<= 8;
							num10 += 8;
						}
						array6[num6 + l] = (byte)((num13 & num9) >> num10);
					}
				}
				else
				{
					uint num14 = num4 ^ num11;
					array6[num6] = (byte)(num14 & 255u);
					array6[num6 + 1] = (byte)((num14 & 65280u) >> 8);
					array6[num6 + 2] = (byte)((num14 & 16711680u) >> 16);
					array6[num6 + 3] = (byte)((num14 & 4278190080u) >> 24);
				}
			}
			Class3.byte_3 = array6;
		}
		int count = BitConverter.ToInt32(Class3.byte_3, int_3);
		try
		{
			return Encoding.Unicode.GetString(Class3.byte_3, int_3 + 4, count);
		}
		catch
		{
		}
		return "";
	}
    */

	internal static string smethod_11(string string_1)
	{
		"{11111-22222-50001-00000}".Trim();
		byte[] array = Convert.FromBase64String(string_1);
		return Encoding.Unicode.GetString(array, 0, array.Length);
	}

	[Class3.Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
	private static byte[] smethod_12(string string_1)
	{
		byte[] array;
		using (FileStream fileStream = new FileStream(string_1, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			int num = 0;
			long length = fileStream.Length;
			int i = (int)length;
			array = new byte[i];
			while (i > 0)
			{
				int num2 = fileStream.Read(array, num, i);
				num += num2;
				i -= num2;
			}
		}
		return array;
	}

	[Class3.Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
	private static byte[] smethod_13(byte[] byte_4)
	{
		MemoryStream memoryStream = new MemoryStream();
		SymmetricAlgorithm symmetricAlgorithm = Class3.smethod_6();
		symmetricAlgorithm.Key = new byte[]
		{
			180,
			198,
			156,
			73,
			110,
			30,
			81,
			2,
			175,
			124,
			15,
			40,
			110,
			219,
			117,
			14,
			246,
			19,
			238,
			66,
			31,
			221,
			179,
			93,
			6,
			141,
			82,
			236,
			55,
			10,
			81,
			82
		};
		symmetricAlgorithm.IV = new byte[]
		{
			36,
			116,
			191,
			129,
			173,
			123,
			97,
			230,
			48,
			52,
			34,
			64,
			164,
			191,
			95,
			121
		};
		CryptoStream cryptoStream = new CryptoStream(memoryStream, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(byte_4, 0, byte_4.Length);
		cryptoStream.Close();
		return memoryStream.ToArray();
	}

	private byte[] method_0()
	{
		return null;
	}

	private byte[] method_1()
	{
		return null;
	}

	private byte[] method_2()
	{
		string text = "{11111-22222-20001-00001}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	private byte[] method_3()
	{
		string text = "{11111-22222-20001-00002}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	private byte[] method_4()
	{
		string text = "{11111-22222-30001-00001}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	private byte[] method_5()
	{
		string text = "{11111-22222-30001-00002}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	internal byte[] method_6()
	{
		string text = "{11111-22222-40001-00001}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	internal byte[] method_7()
	{
		string text = "{11111-22222-40001-00002}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	internal byte[] method_8()
	{
		string text = "{11111-22222-50001-00001}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}

	internal byte[] method_9()
	{
		string text = "{11111-22222-50001-00002}";
		if (text.Length > 0)
		{
			return new byte[]
			{
				1,
				2
			};
		}
		return new byte[]
		{
			1,
			2
		};
	}
}
