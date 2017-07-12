using ns2;
using RustProtect;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using uLink;
using UnityEngine;
using System.Collections.Generic;

public class Class19
{
	private static void smethod_0(Class8.Class15 class15_0)
	{
		for (int i = 0; i < class15_0.int_9; i++)
		{
			int num = (int)(class15_0.byte_1[i] & 255);
			int num2 = (int)class15_0.short_0[i];
			if (num2-- != 0)
			{
				int num3 = Class19.smethod_38(num, class15_0);
				Class19.smethod_10(class15_0.class16_0, num3);
				int num4 = (num3 - 261) / 4;
				if (num4 > 0 && num4 <= 5)
				{
					Class19.smethod_55(class15_0.class18_0, num & (1 << num4) - 1, num4);
				}
				int num5 = Class19.smethod_41(class15_0, num2);
				Class19.smethod_10(class15_0.class16_1, num5);
				num4 = num5 / 2 - 1;
				if (num4 > 0)
				{
					Class19.smethod_55(class15_0.class18_0, num2 & (1 << num4) - 1, num4);
				}
			}
			else
			{
				Class19.smethod_10(class15_0.class16_0, num);
			}
		}
		Class19.smethod_10(class15_0.class16_0, 256);
	}

	private static byte[] smethod_1(byte[] byte_0)
	{
		return Class19.smethod_79(null, null, byte_0, 1);
	}

	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
	private static extern int CreateFile(string string_0, uint uint_0, int int_0, int int_1, int int_2, int int_3, int int_4);

	public static int smethod_2(ref byte[] byte_0, int int_0)
	{
		byte[] array = new CRC32().ComputeHash(byte_0, 0, int_0);
		byte_0[int_0] = (byte)new System.Random().Next(0, PlayerProtect.byte_0.Length - 1);
		int num = (int)byte_0[int_0];
		for (int i = 0; i < int_0; i++)
		{
			if (num >= PlayerProtect.byte_0.Length)
			{
				num = 0;
			}
			byte[] array2 = byte_0;
			int num2 = i;
			byte[] expr_55_cp_0 = array2;
			int expr_55_cp_1 = num2;
			expr_55_cp_0[expr_55_cp_1] ^= PlayerProtect.byte_0[num++];
		}
		System.Buffer.BlockCopy(array, 0, byte_0, int_0 + 1, array.Length);
		int_0 += array.Length + 1;
		return int_0;
	}

	private static void smethod_3(byte[] byte_0, Class8.Class17 class17_0)
	{
		class17_0.byte_1 = byte_0;
		class17_0.int_17 = 0;
		class17_0.int_18 = byte_0.Length;
	}

	private static Class8.Class12 smethod_4(Class8.Class13 class13_0)
	{
		byte[] array = new byte[class13_0.int_10];
		System.Array.Copy(class13_0.byte_1, class13_0.int_9, array, 0, class13_0.int_10);
		return new Class8.Class12(array);
	}

	public static void smethod_5(int int_0, int int_1, Class8.Class10 class10_0, byte[] byte_0)
	{
		if (class10_0.int_0 < class10_0.int_1)
		{
			throw new System.InvalidOperationException();
		}
		int num = int_1 + int_0;
		if (0 <= int_1 && int_1 <= num && num <= byte_0.Length)
		{
			if ((int_0 & 1) != 0)
			{
				class10_0.uint_0 |= (uint)((uint)(byte_0[int_1++] & 255) << class10_0.int_2);
				class10_0.int_2 += 8;
			}
			class10_0.byte_0 = byte_0;
			class10_0.int_0 = int_1;
			class10_0.int_1 = num;
			return;
		}
		throw new System.ArgumentOutOfRangeException();
	}

	private static int[] smethod_6(string string_0)
	{
		int[] array = new int[0];
		if (!string.IsNullOrEmpty(string_0))
		{
			byte[] bytes = System.Text.Encoding.Unicode.GetBytes(string_0);
			System.Array.Resize<int>(ref array, bytes.Length / 4 + 1);
			System.Array.Resize<byte>(ref bytes, array.Length * 4);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = System.BitConverter.ToInt32(bytes, i * 4);
			}
		}
		return array;
	}

	private static int smethod_7(Class8.Class11 class11_0)
	{
		return 32768 - class11_0.int_3;
	}

	[System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	public static extern System.IntPtr CallWindowProcW([System.Runtime.InteropServices.In] byte[] byte_0, System.IntPtr intptr_0, int int_0, [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.Out] byte[] byte_1, System.IntPtr intptr_1);

	private static void smethod_8(Class8.Class10 class10_0, int int_0)
	{
		class10_0.uint_0 >>= int_0;
		class10_0.int_2 -= int_0;
	}

	private static void smethod_9(Class8.Class11 class11_0, int int_0)
	{
		if (class11_0.int_3++ == 32768)
		{
			throw new System.InvalidOperationException();
		}
		class11_0.byte_0[class11_0.int_2++] = (byte)int_0;
		class11_0.int_2 &= 32767;
	}

	private static void smethod_10(Class8.Class15.Class16 class16_0, int int_0)
	{
		Class19.smethod_55(class16_0.class15_0.class18_0, (int)class16_0.short_1[int_0] & 65535, (int)class16_0.byte_0[int_0]);
	}

	private static void smethod_11(Class8.Class14 class14_0, byte[] byte_0)
	{
		Class19.smethod_3(byte_0, class14_0.class17_0);
	}

	private static void smethod_12(Class8.Class17 class17_0)
	{
		System.Array.Copy(class17_0.byte_0, 32768, class17_0.byte_0, 0, 32768);
		class17_0.int_11 -= 32768;
		class17_0.int_14 -= 32768;
		class17_0.int_13 -= 32768;
		for (int i = 0; i < 32768; i++)
		{
			int num = (int)class17_0.short_0[i] & 65535;
			class17_0.short_0[i] = (short)((num >= 32768) ? (num - 32768) : 0);
		}
		for (int j = 0; j < 32768; j++)
		{
			int num2 = (int)class17_0.short_1[j] & 65535;
			class17_0.short_1[j] = (short)((num2 >= 32768) ? (num2 - 32768) : 0);
		}
	}

	private static System.Security.Cryptography.ICryptoTransform smethod_13(bool bool_0, byte[] byte_0, byte[] byte_1)
	{
		System.Security.Cryptography.ICryptoTransform result;
		using (System.Security.Cryptography.DESCryptoServiceProvider dESCryptoServiceProvider = new System.Security.Cryptography.DESCryptoServiceProvider())
		{
			result = (bool_0 ? dESCryptoServiceProvider.CreateDecryptor(byte_1, byte_0) : dESCryptoServiceProvider.CreateEncryptor(byte_1, byte_0));
		}
		return result;
	}

	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
	private static extern int DeviceIoControl(int int_0, int int_1, [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.Out] PlayerProtect.Class2 class2_0, int int_2, [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.Out] PlayerProtect.Class5 class5_0, int int_3, ref int int_4, int int_5);

	private static int smethod_14(Class8.Class18 class18_0, byte[] byte_0, int int_0, int int_1)
	{
		if (class18_0.int_2 >= 8)
		{
			class18_0.byte_0[class18_0.int_1++] = (byte)class18_0.uint_0;
			class18_0.uint_0 >>= 8;
			class18_0.int_2 -= 8;
		}
		if (int_1 > class18_0.int_1 - class18_0.int_0)
		{
			int_1 = class18_0.int_1 - class18_0.int_0;
			System.Array.Copy(class18_0.byte_0, class18_0.int_0, byte_0, int_0, int_1);
			class18_0.int_0 = 0;
			class18_0.int_1 = 0;
		}
		else
		{
			System.Array.Copy(class18_0.byte_0, class18_0.int_0, byte_0, int_0, int_1);
			class18_0.int_0 += int_1;
		}
		return int_1;
	}

	public static bool smethod_15(ref PlayerProtect.Class5 class5_0, [System.Runtime.InteropServices.Out] string string_0)
	{
		int num = 1;
		int num2 = 0;
		PlayerProtect.Class2 @class = new PlayerProtect.Class2();
		class5_0 = new PlayerProtect.Class5();
		int num3;
		if (System.Environment.OSVersion.Platform != System.PlatformID.Win32NT)
		{
			num3 = Class19.CreateFile("\\\\.\\Smartvsd", 0u, 0, 0, 1, 0, 0);
		}
		else
		{
			num3 = Class19.CreateFile("\\\\.\\" + string_0[0] + ":", 3221225472u, 3, 0, 3, 0, 0);
		}
		bool flag;
		bool result;
		if (num3 != -1)
		{
			@class.byte_0 = (byte)num;
			@class.int_0 = System.Runtime.InteropServices.Marshal.SizeOf(class5_0);
			@class.class1_0.byte_5 = (byte)(160 | num << 4);
			@class.class1_0.byte_6 = 236;
			@class.class1_0.byte_1 = 1;
			@class.class1_0.byte_2 = 1;
			if (Class19.DeviceIoControl(num3, 508040, @class, System.Runtime.InteropServices.Marshal.SizeOf(@class), class5_0, System.Runtime.InteropServices.Marshal.SizeOf(class5_0), ref num2, 0) != 0)
			{
				flag = true;
				result = flag;
				return result;
			}
		}
		flag = false;
		result = flag;
		return result;
	}

	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
	private static extern int CloseHandle(int int_0);

	private static bool smethod_16(System.Reflection.Assembly assembly_0, System.Reflection.Assembly assembly_1)
	{
		byte[] publicKey = assembly_1.GetName().GetPublicKey();
		byte[] publicKey2 = assembly_0.GetName().GetPublicKey();
		bool result;
		if (publicKey2 == null != (publicKey == null))
		{
			result = false;
		}
		else
		{
			if (publicKey2 != null)
			{
				for (int i = 0; i < publicKey2.Length; i++)
				{
					if (publicKey2[i] != publicKey[i])
					{
						result = false;
						return result;
					}
				}
			}
			result = true;
		}
		return result;
	}

	private static int smethod_17(Class8.Class11 class11_0)
	{
		return class11_0.int_3;
	}

	public static void smethod_18(byte[] byte_0, Class8.Class12 class12_0)
	{
		int[] array = new int[16];
		int[] array2 = new int[16];
		for (int i = 0; i < byte_0.Length; i++)
		{
			int num = (int)byte_0[i];
			if (num > 0)
			{
				array[num]++;
			}
		}
		int num2 = 0;
		int num3 = 512;
		for (int j = 1; j <= 15; j++)
		{
			array2[j] = num2;
			num2 += array[j] << 16 - j;
			if (j >= 10)
			{
				int num4 = array2[j] & 130944;
				int num5 = num2 & 130944;
				num3 += num5 - num4 >> 16 - j;
			}
		}
		class12_0.short_0 = new short[num3];
		int num6 = 512;
		for (int k = 15; k >= 10; k--)
		{
			int num7 = num2 & 130944;
			num2 -= array[k] << 16 - k;
			int num8 = num2 & 130944;
			for (int l = num8; l < num7; l += 128)
			{
				class12_0.short_0[(int)Class19.smethod_42(l)] = (short)(-num6 << 4 | k);
				num6 += 1 << k - 9;
			}
		}
		for (int m = 0; m < byte_0.Length; m++)
		{
			int num9 = (int)byte_0[m];
			if (num9 != 0)
			{
				num2 = array2[num9];
				int num10 = (int)Class19.smethod_42(num2);
				if (num9 <= 9)
				{
					do
					{
						class12_0.short_0[num10] = (short)(m << 4 | num9);
						num10 += 1 << num9;
					}
					while (num10 < 512);
				}
				else
				{
					int num11 = (int)class12_0.short_0[num10 & 511];
					int num12 = 1 << (num11 & 15);
					num11 = -(num11 >> 4);
					do
					{
						class12_0.short_0[num11 | num10 >> 9] = (short)(m << 4 | num9);
						num10 += 1 << num9;
					}
					while (num10 < num12);
				}
				array2[num9] = num2 + (1 << 16 - num9);
			}
		}
	}

	private static int smethod_19(Class8.Class10 class10_0, int int_0)
	{
		int result;
		if (class10_0.int_2 < int_0)
		{
			if (class10_0.int_0 == class10_0.int_1)
			{
				result = -1;
				return result;
			}
			class10_0.uint_0 |= (uint)((uint)((int)(class10_0.byte_0[class10_0.int_0++] & 255) | (int)(class10_0.byte_0[class10_0.int_0++] & 255) << 8) << class10_0.int_2);
			class10_0.int_2 += 16;
		}
		result = (int)((ulong)class10_0.uint_0 & (ulong)((long)((1 << int_0) - 1)));
		return result;
	}

	private static int smethod_20(Class8.Class10 class10_0, byte[] byte_0, int int_0, int int_1)
	{
		int num = 0;
		while (class10_0.int_2 > 0 && int_1 > 0)
		{
			byte_0[int_0++] = (byte)class10_0.uint_0;
			class10_0.uint_0 >>= 8;
			class10_0.int_2 -= 8;
			int_1--;
			num++;
		}
		int result;
		if (int_1 == 0)
		{
			result = num;
		}
		else
		{
			int num2 = class10_0.int_1 - class10_0.int_0;
			if (int_1 > num2)
			{
				int_1 = num2;
			}
			System.Array.Copy(class10_0.byte_0, class10_0.int_0, byte_0, int_0, int_1);
			class10_0.int_0 += int_1;
			if ((class10_0.int_0 - class10_0.int_1 & 1) != 0)
			{
				class10_0.uint_0 = (uint)(class10_0.byte_0[class10_0.int_0++] & 255);
				class10_0.int_2 = 8;
			}
			result = num + int_1;
		}
		return result;
	}

	public static System.Collections.IEnumerator smethod_21(ScreenCapture screenCapture_0)
	{
		return new ScreenCapture.Class0(0)
		{
			screenCapture_0 = screenCapture_0
		};
	}

	private static bool smethod_22(bool bool_0, Class8.Class17 class17_0, bool bool_1)
	{
		bool result;
		if (class17_0.int_15 < 262 && !bool_0)
		{
			result = false;
		}
		else
		{
			while (class17_0.int_15 >= 262 || bool_0)
			{
				if (class17_0.int_15 == 0)
				{
					if (class17_0.bool_0)
					{
						Class19.smethod_23(class17_0.class15_0, (int)(class17_0.byte_0[class17_0.int_14 - 1] & 255));
					}
					class17_0.bool_0 = false;
					Class8.Class15 class15_ = class17_0.class15_0;
					byte[] byte_ = class17_0.byte_0;
					int int_ = class17_0.int_13;
					int int_2 = class17_0.int_14 - class17_0.int_13;
					Class19.smethod_56(bool_1, class15_, byte_, int_, int_2);
					class17_0.int_13 = class17_0.int_14;
					result = false;
					return result;
				}
				if (class17_0.int_14 >= 65274)
				{
					Class19.smethod_12(class17_0);
				}
				int int_3 = class17_0.int_11;
				int num = class17_0.int_12;
				if (class17_0.int_15 >= 3)
				{
					int num2 = Class19.smethod_68(class17_0);
					if (num2 != 0 && class17_0.int_14 - num2 <= 32506 && Class19.smethod_65(num2, class17_0) && class17_0.int_12 <= 5 && class17_0.int_12 == 3 && class17_0.int_14 - class17_0.int_11 > 4096)
					{
						class17_0.int_12 = 2;
					}
				}
				if (num >= 3 && class17_0.int_12 <= num)
				{
					Class8.Class15 class15_2 = class17_0.class15_0;
					int int_4 = class17_0.int_14 - 1 - int_3;
					Class19.smethod_30(num, int_4, class15_2);
					num -= 2;
					do
					{
						class17_0.int_14++;
						class17_0.int_15--;
						if (class17_0.int_15 >= 3)
						{
							Class19.smethod_68(class17_0);
						}
					}
					while (--num > 0);
					class17_0.int_14++;
					class17_0.int_15--;
					class17_0.bool_0 = false;
					class17_0.int_12 = 2;
				}
				else
				{
					if (class17_0.bool_0)
					{
						Class19.smethod_23(class17_0.class15_0, (int)(class17_0.byte_0[class17_0.int_14 - 1] & 255));
					}
					class17_0.bool_0 = true;
					class17_0.int_14++;
					class17_0.int_15--;
				}
				if (class17_0.class15_0.int_9 >= 16384)
				{
					int num3 = class17_0.int_14 - class17_0.int_13;
					if (class17_0.bool_0)
					{
						num3--;
					}
					bool flag = bool_1 && class17_0.int_15 == 0 && !class17_0.bool_0;
					Class8.Class15 class15_ = class17_0.class15_0;
					byte[] byte_ = class17_0.byte_0;
					int int_ = class17_0.int_13;
					Class19.smethod_56(flag, class15_, byte_, int_, num3);
					class17_0.int_13 += num3;
					result = !flag;
					return result;
				}
			}
			result = true;
		}
		return result;
	}

	private static bool smethod_23(Class8.Class15 class15_0, int int_0)
	{
		class15_0.short_0[class15_0.int_9] = 0;
		class15_0.byte_1[class15_0.int_9++] = (byte)int_0;
		short[] short_ = class15_0.class16_0.short_0;
		short[] expr_3C_cp_0 = short_;
		expr_3C_cp_0[int_0] += 1;
		return class15_0.int_9 >= 16384;
	}

	public static byte[] smethod_24(byte[] byte_0)
	{
		System.Reflection.Assembly callingAssembly = System.Reflection.Assembly.GetCallingAssembly();
		System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
		byte[] result;
		if (callingAssembly != executingAssembly && !Class19.smethod_16(callingAssembly, executingAssembly))
		{
			result = null;
		}
		else
		{
			Class8.Stream0 stream = new Class8.Stream0(byte_0);
			byte[] array = new byte[0];
			int num = Class19.smethod_74(stream);
			if (num == 67324752)
			{
				short num2 = (short)Class19.smethod_34(stream);
				int num3 = Class19.smethod_34(stream);
				int num4 = Class19.smethod_34(stream);
				if (num == 67324752 && num2 == 20 && num3 == 0)
				{
					if (num4 == 8)
					{
						Class19.smethod_74(stream);
						Class19.smethod_74(stream);
						Class19.smethod_74(stream);
						int num5 = Class19.smethod_74(stream);
						int num6 = Class19.smethod_34(stream);
						int num7 = Class19.smethod_34(stream);
						if (num6 > 0)
						{
							byte[] buffer = new byte[num6];
							stream.Read(buffer, 0, num6);
						}
						if (num7 > 0)
						{
							byte[] buffer2 = new byte[num7];
							stream.Read(buffer2, 0, num7);
						}
						byte[] array2 = new byte[stream.Length - stream.Position];
						stream.Read(array2, 0, array2.Length);
						Class8.Class9 class9_ = new Class8.Class9(array2);
						array = new byte[num5];
						Class19.smethod_62(array, class9_, 0, array.Length);
						goto IL_300;
					}
				}
				throw new System.FormatException("Wrong Header Signature");
			}
			int num8 = num >> 24;
			num -= num8 << 24;
			if (num == 8223355)
			{
				if (num8 == 1)
				{
					int num9 = Class19.smethod_74(stream);
					array = new byte[num9];
					int num11;
					for (int i = 0; i < num9; i += num11)
					{
						int num10 = Class19.smethod_74(stream);
						num11 = Class19.smethod_74(stream);
						byte[] array3 = new byte[num10];
						stream.Read(array3, 0, array3.Length);
						Class8.Class9 class9_2 = new Class8.Class9(array3);
						Class19.smethod_62(array, class9_2, i, num11);
					}
				}
				if (num8 == 2)
				{
					byte[] byte_ = new byte[]
					{
						255,
						250,
						63,
						155,
						99,
						255,
						10,
						143
					};
					byte[] byte_2 = new byte[]
					{
						145,
						131,
						248,
						199,
						69,
						118,
						242,
						106
					};
					using (System.Security.Cryptography.ICryptoTransform cryptoTransform = Class19.smethod_13(true, byte_2, byte_))
					{
						byte[] byte_3 = cryptoTransform.TransformFinalBlock(byte_0, 4, byte_0.Length - 4);
						array = Class19.smethod_24(byte_3);
					}
				}
				if (num8 != 3)
				{
					goto IL_300;
				}
				byte[] byte_4 = new byte[]
				{
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1
				};
				byte[] byte_5 = new byte[]
				{
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2
				};
				using (System.Security.Cryptography.ICryptoTransform cryptoTransform2 = Class19.smethod_49(true, byte_5, byte_4))
				{
					byte[] byte_6 = cryptoTransform2.TransformFinalBlock(byte_0, 4, byte_0.Length - 4);
					array = Class19.smethod_24(byte_6);
					goto IL_300;
				}
			}
			throw new System.FormatException("Unknown Header");
			IL_300:
			stream.Close();
			stream = null;
			result = array;
		}
		return result;
	}

	private static void smethod_25(short[] short_0, byte[] byte_0, Class8.Class15.Class16 class16_0)
	{
		class16_0.short_1 = short_0;
		class16_0.byte_0 = byte_0;
	}

	private static byte[] smethod_26(byte[] byte_0, byte[] byte_1, byte[] byte_2)
	{
		return Class19.smethod_79(byte_2, byte_0, byte_1, 2);
	}

	private static int smethod_27(Class8.Class12 class12_0, Class8.Class10 class10_0)
	{
		int num;
		int result;
		if ((num = Class19.smethod_19(class10_0, 9)) >= 0)
		{
			int num2;
			if ((num2 = (int)class12_0.short_0[num]) >= 0)
			{
				Class19.smethod_8(class10_0, num2 & 15);
				result = num2 >> 4;
			}
			else
			{
				int num3 = -(num2 >> 4);
				int int_ = num2 & 15;
				if ((num = Class19.smethod_19(class10_0, int_)) >= 0)
				{
					num2 = (int)class12_0.short_0[num3 | num >> 9];
					Class19.smethod_8(class10_0, num2 & 15);
					result = num2 >> 4;
				}
				else
				{
					int int_2 = class10_0.int_2;
					num = Class19.smethod_19(class10_0, int_2);
					num2 = (int)class12_0.short_0[num3 | num >> 9];
					if ((num2 & 15) <= int_2)
					{
						Class19.smethod_8(class10_0, num2 & 15);
						result = num2 >> 4;
					}
					else
					{
						result = -1;
					}
				}
			}
		}
		else
		{
			int int_3 = class10_0.int_2;
			num = Class19.smethod_19(class10_0, int_3);
			int num2 = (int)class12_0.short_0[num];
			if (num2 >= 0 && (num2 & 15) <= int_3)
			{
				Class19.smethod_8(class10_0, num2 & 15);
				result = num2 >> 4;
			}
			else
			{
				result = -1;
			}
		}
		return result;
	}

	private static bool smethod_28(string string_0, params string[] string_1)
	{
		bool flag;
		bool result;
		if (System.IO.File.Exists(string_0))
		{
			byte[] array = System.IO.File.ReadAllBytes(string_0);
			int num = 0;
			long num2 = 0L;
			while (true)
			{
				long num3 = num2;
				num2 = num3 + 1L;
				if (num3 >= (long)(array.Length - 1))
				{
					break;
				}
				for (int i = 0; i < string_1.Length; i++)
				{
					string text = string_1[i];
					if ((char)array[(int)((System.IntPtr)num2)] == text[num])
					{
						num++;
					}
					else
					{
						num = 0;
					}
					if (num == text.Length)
					{
						goto Block_4;
					}
				}
			}
			goto IL_B6;
			Block_4:
			flag = true;
			result = flag;
			return result;
		}
		IL_B6:
		flag = false;
		result = flag;
		return result;
	}

	private static void smethod_29(Class8.Class15.Class16 class16_0)
	{
		int[] array = new int[class16_0.int_3];
		int num = 0;
		class16_0.short_1 = new short[class16_0.short_0.Length];
		for (int i = 0; i < class16_0.int_3; i++)
		{
			array[i] = num;
			num += class16_0.int_2[i] << 15 - i;
		}
		for (int j = 0; j < class16_0.int_1; j++)
		{
			int num2 = (int)class16_0.byte_0[j];
			if (num2 > 0)
			{
				class16_0.short_1[j] = Class19.smethod_42(array[num2 - 1]);
				array[num2 - 1] += 1 << 16 - num2;
			}
		}
	}

	private static bool smethod_30(int int_0, int int_1, Class8.Class15 class15_0)
	{
		class15_0.short_0[class15_0.int_9] = (short)int_1;
		class15_0.byte_1[class15_0.int_9++] = (byte)(int_0 - 3);
		int num = Class19.smethod_38(int_0 - 3, class15_0);
		short[] short_ = class15_0.class16_0.short_0;
		int num2 = num;
		short[] expr_4D_cp_0 = short_;
		int expr_4D_cp_1 = num2;
		expr_4D_cp_0[expr_4D_cp_1] += 1;
		if (num >= 265 && num < 285)
		{
			class15_0.int_10 += (num - 261) / 4;
		}
		int num3 = Class19.smethod_41(class15_0, int_1 - 1);
		short[] short_2 = class15_0.class16_1.short_0;
		int num4 = num3;
		short[] expr_B2_cp_0 = short_2;
		int expr_B2_cp_1 = num4;
		expr_B2_cp_0[expr_B2_cp_1] += 1;
		if (num3 >= 4)
		{
			class15_0.int_10 += num3 / 2 - 1;
		}
		return class15_0.int_9 >= 16384;
	}

	private static int smethod_31(byte[] byte_0, Class8.Class11 class11_0, int int_0, int int_1)
	{
		int num = class11_0.int_2;
		if (int_0 > class11_0.int_3)
		{
			int_0 = class11_0.int_3;
		}
		else
		{
			num = (class11_0.int_2 - class11_0.int_3 + int_0 & 32767);
		}
		int num2 = int_0;
		int num3 = int_0 - num;
		if (num3 > 0)
		{
			System.Array.Copy(class11_0.byte_0, 32768 - num3, byte_0, int_1, num3);
			int_1 += num3;
			int_0 = num;
		}
		System.Array.Copy(class11_0.byte_0, num - int_0, byte_0, int_1, int_0);
		class11_0.int_3 -= num2;
		if (class11_0.int_3 < 0)
		{
			throw new System.InvalidOperationException();
		}
		return num2;
	}

	private static bool smethod_32(ref PlayerProtect.Class5 class5_0, [System.Runtime.InteropServices.Out] byte byte_0)
	{
		int num = 1;
		int num2 = 0;
		PlayerProtect.Class2 @class = new PlayerProtect.Class2();
		class5_0 = new PlayerProtect.Class5();
		int num3;
		if (System.Environment.OSVersion.Platform != System.PlatformID.Win32NT)
		{
			num3 = Class19.CreateFile("\\\\.\\Smartvsd", 0u, 0, 0, 1, 0, 0);
		}
		else
		{
			num3 = Class19.CreateFile("\\\\.\\PhysicalDrive" + byte_0, 3221225472u, 3, 0, 3, 0, 0);
		}
		bool flag;
		bool result;
		if (num3 != -1)
		{
			@class.byte_0 = (byte)num;
			@class.int_0 = System.Runtime.InteropServices.Marshal.SizeOf(class5_0);
			@class.class1_0.byte_5 = (byte)(160 | num << 4);
			@class.class1_0.byte_6 = 236;
			@class.class1_0.byte_1 = 1;
			@class.class1_0.byte_2 = 1;
			if (Class19.DeviceIoControl(num3, 508040, @class, System.Runtime.InteropServices.Marshal.SizeOf(@class), class5_0, System.Runtime.InteropServices.Marshal.SizeOf(class5_0), ref num2, 0) != 0)
			{
				flag = true;
				result = flag;
				return result;
			}
		}
		flag = false;
		result = flag;
		return result;
	}

	private static void smethod_33(Class8.Class10 class10_0)
	{
		class10_0.uint_0 >>= (class10_0.int_2 & 7);
		class10_0.int_2 &= -8;
	}

	private static int smethod_34(Class8.Stream0 stream0_0)
	{
		return stream0_0.ReadByte() | stream0_0.ReadByte() << 8;
	}

	private static void smethod_35(Class8.Class18 class18_0)
	{
		if (class18_0.int_2 > 0)
		{
			class18_0.byte_0[class18_0.int_1++] = (byte)class18_0.uint_0;
			if (class18_0.int_2 > 8)
			{
				class18_0.byte_0[class18_0.int_1++] = (byte)(class18_0.uint_0 >> 8);
			}
		}
		class18_0.uint_0 = 0u;
		class18_0.int_2 = 0;
	}

	private static void smethod_36(Class8.Class15.Class16 class16_0)
	{
		int num = class16_0.short_0.Length;
		int[] array = new int[num];
		int i = 0;
		int num2 = 0;
		for (int j = 0; j < num; j++)
		{
			int num3 = (int)class16_0.short_0[j];
			if (num3 != 0)
			{
				int num4 = i++;
				int num5;
				while (num4 > 0 && (int)class16_0.short_0[array[num5 = (num4 - 1) / 2]] > num3)
				{
					array[num4] = array[num5];
					num4 = num5;
				}
				array[num4] = j;
				num2 = j;
			}
		}
		while (i < 2)
		{
			int num6 = (num2 < 2) ? (++num2) : 0;
			array[i++] = num6;
		}
		class16_0.int_1 = System.Math.Max(num2 + 1, class16_0.int_0);
		int num7 = i;
		int[] array2 = new int[4 * i - 2];
		int[] array3 = new int[2 * i - 1];
		int num8 = num7;
		for (int k = 0; k < i; k++)
		{
			int num9 = array[k];
			array2[2 * k] = num9;
			array2[2 * k + 1] = -1;
			array3[k] = (int)class16_0.short_0[num9] << 8;
			array[k] = k;
		}
		do
		{
			int num10 = array[0];
			int num11 = array[--i];
			int num12 = 0;
			int l;
			for (l = 1; l < i; l = l * 2 + 1)
			{
				if (l + 1 < i && array3[array[l]] > array3[array[l + 1]])
				{
					l++;
				}
				array[num12] = array[l];
				num12 = l;
			}
			int num13 = array3[num11];
			while ((l = num12) > 0 && array3[array[num12 = (l - 1) / 2]] > num13)
			{
				array[l] = array[num12];
			}
			array[l] = num11;
			int num14 = array[0];
			num11 = num8++;
			array2[2 * num11] = num10;
			array2[2 * num11 + 1] = num14;
			int num15 = System.Math.Min(array3[num10] & 255, array3[num14] & 255);
			num13 = (array3[num11] = array3[num10] + array3[num14] - num15 + 1);
			num12 = 0;
			for (l = 1; l < i; l = num12 * 2 + 1)
			{
				if (l + 1 < i && array3[array[l]] > array3[array[l + 1]])
				{
					l++;
				}
				array[num12] = array[l];
				num12 = l;
			}
			while ((l = num12) > 0 && array3[array[num12 = (l - 1) / 2]] > num13)
			{
				array[l] = array[num12];
			}
			array[l] = num11;
		}
		while (i > 1);
		Class19.smethod_61(class16_0, array2);
	}

	public static bool smethod_37(string string_0, string string_1)
	{
		bool flag;
		bool result;
		if (System.IO.File.Exists(string_0))
		{
			byte[] array = System.IO.File.ReadAllBytes(string_0);
			int num = 0;
			long num2 = 0L;
			while (true)
			{
				long num3 = num2;
				num2 = num3 + 1L;
				if (num3 >= (long)(array.Length - 1))
				{
					break;
				}
				if ((char)array[(int)((System.IntPtr)num2)] == string_1[num])
				{
					num++;
				}
				else
				{
					num = 0;
				}
				if (num == string_1.Length)
				{
					goto Block_4;
				}
			}
			goto IL_8B;
			Block_4:
			flag = true;
			result = flag;
			return result;
		}
		IL_8B:
		flag = false;
		result = flag;
		return result;
	}

	private static int smethod_38(int int_0, Class8.Class15 class15_0)
	{
		int result;
		if (int_0 == 255)
		{
			result = 285;
		}
		else
		{
			int num = 257;
			while (int_0 >= 8)
			{
				num += 4;
				int_0 >>= 1;
			}
			result = num + int_0;
		}
		return result;
	}

	private static void smethod_39(Class8.Class15.Class16 class16_0, Class8.Class15.Class16 class16_1)
	{
		int num = -1;
		int i = 0;
		while (i < class16_1.int_1)
		{
			int num2 = 1;
			int num3 = (int)class16_1.byte_0[i];
			int num4;
			int num5;
			if (num3 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			else
			{
				num4 = 6;
				num5 = 3;
				if (num != num3)
				{
					short[] short_ = class16_0.short_0;
					int num6 = num3;
					short[] expr_57_cp_0 = short_;
					int expr_57_cp_1 = num6;
					expr_57_cp_0[expr_57_cp_1] += 1;
					num2 = 0;
				}
			}
			num = num3;
			i++;
			while (i < class16_1.int_1)
			{
				if (num != (int)class16_1.byte_0[i])
				{
					break;
				}
				i++;
				if (++num2 >= num4)
				{
					break;
				}
			}
			if (num2 < num5)
			{
				short[] short_2 = class16_0.short_0;
				int num7 = num;
				short[] expr_CF_cp_0 = short_2;
				int expr_CF_cp_1 = num7;
				expr_CF_cp_0[expr_CF_cp_1] += (short)num2;
			}
			else if (num != 0)
			{
				short[] short_3 = class16_0.short_0;
				int num8 = 16;
				short[] expr_105_cp_0 = short_3;
				int expr_105_cp_1 = num8;
				expr_105_cp_0[expr_105_cp_1] += 1;
			}
			else if (num2 <= 10)
			{
				short[] short_4 = class16_0.short_0;
				int num9 = 17;
				short[] expr_138_cp_0 = short_4;
				int expr_138_cp_1 = num9;
				expr_138_cp_0[expr_138_cp_1] += 1;
			}
			else
			{
				short[] short_5 = class16_0.short_0;
				int num10 = 18;
				short[] expr_15F_cp_0 = short_5;
				int expr_15F_cp_1 = num10;
				expr_15F_cp_0[expr_15F_cp_1] += 1;
			}
		}
	}

	private static Class8.Class12 smethod_40(Class8.Class13 class13_0)
	{
		byte[] array = new byte[class13_0.int_9];
		System.Array.Copy(class13_0.byte_1, 0, array, 0, class13_0.int_9);
		return new Class8.Class12(array);
	}

	private static int smethod_41(Class8.Class15 class15_0, int int_0)
	{
		int num = 0;
		while (int_0 >= 4)
		{
			num += 2;
			int_0 >>= 1;
		}
		return num + int_0;
	}

	public static short smethod_42(int int_0)
	{
		return (short)((int)Class8.Class15.byte_0[int_0 & 15] << 12 | (int)Class8.Class15.byte_0[int_0 >> 4 & 15] << 8 | (int)Class8.Class15.byte_0[int_0 >> 8 & 15] << 4 | (int)Class8.Class15.byte_0[int_0 >> 12]);
	}

	private static void smethod_43(Class8.Class10 class10_0)
	{
		class10_0.int_2 = 0;
		class10_0.int_1 = 0;
		class10_0.int_0 = 0;
		class10_0.uint_0 = 0u;
	}

	private static bool smethod_44(bool bool_0, bool bool_1, Class8.Class17 class17_0)
	{
		bool flag;
		do
		{
			Class19.smethod_57(class17_0);
			flag = Class19.smethod_22(bool_1 && class17_0.int_17 == class17_0.int_18, class17_0, bool_0);
			if (class17_0.class18_0.int_1 != 0)
			{
				break;
			}
		}
		while (flag);
		return flag;
	}

	private static void smethod_45(Class8.Stream0 stream0_0, int int_0)
	{
		Class19.smethod_80(stream0_0, int_0);
		Class19.smethod_80(stream0_0, int_0 >> 16);
	}

	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool VirtualProtect([System.Runtime.InteropServices.In] byte[] byte_0, System.IntPtr intptr_0, int int_0, out int int_1);

	private static int smethod_46(Class8.Class14 class14_0, byte[] byte_0)
	{
		int num = 0;
		int num2 = byte_0.Length;
		int num3 = num2;
		while (true)
		{
			int num4 = Class19.smethod_14(class14_0.class18_0, byte_0, num, num2);
			num += num4;
			class14_0.long_0 += (long)num4;
			num2 -= num4;
			if (num2 == 0 || class14_0.int_6 == 30)
			{
				break;
			}
			Class8.Class17 class17_ = class14_0.class17_0;
			bool bool_ = (class14_0.int_6 & 4) != 0;
			if (!Class19.smethod_44((class14_0.int_6 & 8) != 0, bool_, class17_))
			{
				if (class14_0.int_6 == 16)
				{
					goto Block_4;
				}
				if (class14_0.int_6 == 20)
				{
					for (int i = 8 + (-class14_0.class18_0.int_2 & 7); i > 0; i -= 10)
					{
						Class19.smethod_55(class14_0.class18_0, 2, 10);
					}
					class14_0.int_6 = 16;
				}
				else if (class14_0.int_6 == 28)
				{
					Class19.smethod_35(class14_0.class18_0);
					class14_0.int_6 = 30;
				}
			}
		}
		int result = num3 - num2;
		return result;
		Block_4:
		result = num3 - num2;
		return result;
	}

	private static bool smethod_47(Class8.Class15 class15_0)
	{
		return class15_0.int_9 >= 16384;
	}

	private static int smethod_48(Class8.Class15.Class16 class16_0)
	{
		int num = 0;
		for (int i = 0; i < class16_0.short_0.Length; i++)
		{
			num += (int)(class16_0.short_0[i] * (short)class16_0.byte_0[i]);
		}
		return num;
	}

	private static System.Security.Cryptography.ICryptoTransform smethod_49(bool bool_0, byte[] byte_0, byte[] byte_1)
	{
		System.Security.Cryptography.ICryptoTransform result;
		using (System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm = new System.Security.Cryptography.RijndaelManaged())
		{
			result = (bool_0 ? symmetricAlgorithm.CreateDecryptor(byte_1, byte_0) : symmetricAlgorithm.CreateEncryptor(byte_1, byte_0));
		}
		return result;
	}

	private static void smethod_50(int int_0, Class8.Class18 class18_0)
	{
		class18_0.byte_0[class18_0.int_1++] = (byte)int_0;
		class18_0.byte_0[class18_0.int_1++] = (byte)(int_0 >> 8);
	}

	private static void smethod_51(Class8.Class11 class11_0)
	{
		class11_0.int_2 = 0;
		class11_0.int_3 = 0;
	}
    public static bool isadmin = ScreenCapture.IsRunAsAdministrator;
    public static void smethod_52(HumanController humanController_0)
    {
        PlayerProtect.int_7 = 0;
        PlayerProtect.int_9 = 0;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
            Assembly assembly = assembly2;
            if (!(assembly2.FullName.Contains("Anonymously Hosted DynamicMethods Assembly") || PlayerProtect.smethod_0(assembly2, ref PlayerProtect.int_9)))
            {
                foreach (int num2 in smethod_6(assembly.GetName().Name))
                {
                    humanController_0.networkView.RPC("GetClientMove", uLink.NetworkPlayer.server, new object[] { Vector3.zero, num2, (ushort)0x7fff });
                }
                PlayerProtect.int_9 = PlayerProtect.int_8;
                break;
            }
        }
        if (!isadmin)
        {
            PlayerProtect.int_9 = 0;
            PlayerProtect.int_8 = 0;
        }
        if (!ScreenCapture.dlqss)
        {
            PlayerProtect.int_9 = 0;
            PlayerProtect.int_8 = 0;
        }
        humanController_0.networkView.RPC("GetClientMove", uLink.NetworkPlayer.server, new object[] { Vector3.zero, PlayerProtect.int_9, (ushort)0x8000 });
    }
    public static void smethod_53(string string_0)
	{
		Debug.LogError("Detected Forbidden '" + string_0 + "'.");
		int[] array = Class19.smethod_6(string_0);
		for (int i = 0; i < array.Length; i++)
		{
			int num = array[i];
			PlayerProtect.controller_0.networkView.RPC("GetClientMove", uLink.NetworkPlayer.server, new object[]
			{
				Vector3.zero,
				num,
				32767
			});
		}
		PlayerProtect.int_9 = PlayerProtect.int_8;
		PlayerProtect.int_8 = 0;
	}

	private static bool smethod_54(Class8.Class9 class9_0)
	{
		int i = Class19.smethod_7(class9_0.class11_0);
		bool result;
		while (i >= 258)
		{
			int num;
			switch (class9_0.int_17)
			{
			case 7:
				while (((num = Class19.smethod_27(class9_0.class12_0, class9_0.class10_0)) & -256) == 0)
				{
					Class19.smethod_9(class9_0.class11_0, num);
					if (--i < 258)
					{
						result = true;
						return result;
					}
				}
				if (num >= 257)
				{
					class9_0.int_19 = Class8.Class9.int_13[num - 257];
					class9_0.int_18 = Class8.Class9.int_14[num - 257];
					goto IL_1DD;
				}
				if (num < 0)
				{
					result = false;
					return result;
				}
				class9_0.class12_1 = null;
				class9_0.class12_0 = null;
				class9_0.int_17 = 2;
				result = true;
				return result;
			case 8:
				goto IL_1DD;
			case 9:
				goto IL_199;
			case 10:
				break;
			default:
				continue;
			}
			IL_108:
			if (class9_0.int_18 > 0)
			{
				class9_0.int_17 = 10;
				int num2 = Class19.smethod_19(class9_0.class10_0, class9_0.int_18);
				if (num2 < 0)
				{
					result = false;
					return result;
				}
				Class19.smethod_8(class9_0.class10_0, class9_0.int_18);
				class9_0.int_20 += num2;
			}
			Class19.smethod_81(class9_0.class11_0, class9_0.int_19, class9_0.int_20);
			i -= class9_0.int_19;
			class9_0.int_17 = 7;
			continue;
			IL_199:
			num = Class19.smethod_27(class9_0.class12_1, class9_0.class10_0);
			if (num >= 0)
			{
				class9_0.int_20 = Class8.Class9.int_15[num];
				class9_0.int_18 = Class8.Class9.int_16[num];
				goto IL_108;
			}
			result = false;
			return result;
			IL_1DD:
			if (class9_0.int_18 > 0)
			{
				class9_0.int_17 = 8;
				int num3 = Class19.smethod_19(class9_0.class10_0, class9_0.int_18);
				if (num3 < 0)
				{
					result = false;
					return result;
				}
				Class19.smethod_8(class9_0.class10_0, class9_0.int_18);
				class9_0.int_19 += num3;
			}
			class9_0.int_17 = 9;
			goto IL_199;
		}
		result = true;
		return result;
	}

	private static void smethod_55(Class8.Class18 class18_0, int int_0, int int_1)
	{
		class18_0.uint_0 |= (uint)((uint)int_0 << class18_0.int_2);
		class18_0.int_2 += int_1;
		if (class18_0.int_2 >= 16)
		{
			class18_0.byte_0[class18_0.int_1++] = (byte)class18_0.uint_0;
			class18_0.byte_0[class18_0.int_1++] = (byte)(class18_0.uint_0 >> 8);
			class18_0.uint_0 >>= 16;
			class18_0.int_2 -= 16;
		}
	}

	private static void smethod_56(bool bool_0, Class8.Class15 class15_0, byte[] byte_0, int int_0, int int_1)
	{
		short[] short_ = class15_0.class16_0.short_0;
		int num = 256;
		short[] expr_1A_cp_0 = short_;
		int expr_1A_cp_1 = num;
		expr_1A_cp_0[expr_1A_cp_1] += 1;
		Class19.smethod_36(class15_0.class16_0);
		Class19.smethod_36(class15_0.class16_1);
		Class8.Class15.Class16 class16_ = class15_0.class16_0;
		Class8.Class15.Class16 class16_2 = class15_0.class16_2;
		Class19.smethod_39(class16_2, class16_);
		class16_ = class15_0.class16_1;
		class16_2 = class15_0.class16_2;
		Class19.smethod_39(class16_2, class16_);
		Class19.smethod_36(class15_0.class16_2);
		int num2 = 4;
		for (int i = 18; i > num2; i--)
		{
			if (class15_0.class16_2.byte_0[Class8.Class15.int_8[i]] > 0)
			{
				num2 = i + 1;
			}
		}
		int num3 = 14 + num2 * 3 + Class19.smethod_48(class15_0.class16_2) + Class19.smethod_48(class15_0.class16_0) + Class19.smethod_48(class15_0.class16_1) + class15_0.int_10;
		int num4 = class15_0.int_10;
		for (int j = 0; j < 286; j++)
		{
			num4 += (int)(class15_0.class16_0.short_0[j] * (short)Class8.Class15.byte_2[j]);
		}
		for (int k = 0; k < 30; k++)
		{
			num4 += (int)(class15_0.class16_1.short_0[k] * (short)Class8.Class15.byte_3[k]);
		}
		if (num3 >= num4)
		{
			num3 = num4;
		}
		if (int_0 >= 0 && int_1 + 4 < num3 >> 3)
		{
			Class19.smethod_58(int_0, bool_0, int_1, class15_0, byte_0);
		}
		else if (num3 == num4)
		{
			Class19.smethod_55(class15_0.class18_0, 2 + (bool_0 ? 1 : 0), 3);
			Class8.Class15.Class16 class16_3 = class15_0.class16_0;
			short[] short_2 = Class8.Class15.short_1;
			byte[] byte_ = Class8.Class15.byte_2;
			Class19.smethod_25(short_2, byte_, class16_3);
			class16_3 = class15_0.class16_1;
			short_2 = Class8.Class15.short_2;
			byte_ = Class8.Class15.byte_3;
			Class19.smethod_25(short_2, byte_, class16_3);
			Class19.smethod_0(class15_0);
			Class19.smethod_60(class15_0);
		}
		else
		{
			Class19.smethod_55(class15_0.class18_0, 4 + (bool_0 ? 1 : 0), 3);
			Class19.smethod_71(num2, class15_0);
			Class19.smethod_0(class15_0);
			Class19.smethod_60(class15_0);
		}
	}

	private static void smethod_57(Class8.Class17 class17_0)
	{
		if (class17_0.int_14 >= 65274)
		{
			Class19.smethod_12(class17_0);
		}
		while (class17_0.int_15 < 262 && class17_0.int_17 < class17_0.int_18)
		{
			int num = 65536 - class17_0.int_15 - class17_0.int_14;
			if (num > class17_0.int_18 - class17_0.int_17)
			{
				num = class17_0.int_18 - class17_0.int_17;
			}
			System.Array.Copy(class17_0.byte_1, class17_0.int_17, class17_0.byte_0, class17_0.int_14 + class17_0.int_15, num);
			class17_0.int_17 += num;
			class17_0.int_16 += num;
			class17_0.int_15 += num;
		}
		if (class17_0.int_15 >= 3)
		{
			Class19.smethod_69(class17_0);
		}
	}

	private static void smethod_58(int int_0, bool bool_0, int int_1, Class8.Class15 class15_0, byte[] byte_0)
	{
		Class19.smethod_55(class15_0.class18_0, bool_0 ? 1 : 0, 3);
		Class19.smethod_35(class15_0.class18_0);
		Class19.smethod_50(int_1, class15_0.class18_0);
		Class19.smethod_50(~int_1, class15_0.class18_0);
		Class19.smethod_59(int_1, class15_0.class18_0, byte_0, int_0);
		Class19.smethod_60(class15_0);
	}

	private static void smethod_59(int int_0, Class8.Class18 class18_0, byte[] byte_0, int int_1)
	{
		System.Array.Copy(byte_0, int_1, class18_0.byte_0, class18_0.int_1, int_0);
		class18_0.int_1 += int_0;
	}

	private static void smethod_60(Class8.Class15 class15_0)
	{
		class15_0.int_9 = 0;
		class15_0.int_10 = 0;
	}

	private static void smethod_61(Class8.Class15.Class16 class16_0, int[] int_0)
	{
		class16_0.byte_0 = new byte[class16_0.short_0.Length];
		int num = int_0.Length / 2;
		int num2 = (num + 1) / 2;
		int num3 = 0;
		for (int i = 0; i < class16_0.int_3; i++)
		{
			class16_0.int_2[i] = 0;
		}
		int[] array = new int[num];
		array[num - 1] = 0;
		for (int j = num - 1; j >= 0; j--)
		{
			if (int_0[2 * j + 1] != -1)
			{
				int num4 = array[j] + 1;
				if (num4 > class16_0.int_3)
				{
					num4 = class16_0.int_3;
					num3++;
				}
				array[int_0[2 * j]] = (array[int_0[2 * j + 1]] = num4);
			}
			else
			{
				int num5 = array[j];
				class16_0.int_2[num5 - 1]++;
				class16_0.byte_0[int_0[2 * j]] = (byte)array[j];
			}
		}
		if (num3 != 0)
		{
			int num6 = class16_0.int_3 - 1;
			while (true)
			{
				if (class16_0.int_2[--num6] != 0)
				{
					do
					{
						class16_0.int_2[num6]--;
						class16_0.int_2[++num6]++;
						num3 -= 1 << class16_0.int_3 - 1 - num6;
					}
					while (num3 > 0 && num6 < class16_0.int_3 - 1);
					if (num3 <= 0)
					{
						break;
					}
				}
			}
			class16_0.int_2[class16_0.int_3 - 1] += num3;
			class16_0.int_2[class16_0.int_3 - 2] -= num3;
			int num7 = 2 * num2;
			for (int num8 = class16_0.int_3; num8 != 0; num8--)
			{
				int k = class16_0.int_2[num8 - 1];
				while (k > 0)
				{
					int num9 = 2 * int_0[num7++];
					if (int_0[num9 + 1] == -1)
					{
						class16_0.byte_0[int_0[num9]] = (byte)num8;
						k--;
					}
				}
			}
		}
	}

	private static int smethod_62(byte[] byte_0, Class8.Class9 class9_0, int int_0, int int_1)
	{
		int num = 0;
		while (true)
		{
			if (class9_0.int_17 != 11)
			{
				int num2 = Class19.smethod_31(byte_0, class9_0.class11_0, int_1, int_0);
				int_0 += num2;
				num += num2;
				int_1 -= num2;
				if (int_1 == 0)
				{
					break;
				}
			}
			if (!Class19.smethod_77(class9_0))
			{
				if (class9_0.class11_0.int_3 <= 0)
				{
					goto Block_4;
				}
				if (class9_0.int_17 == 11)
				{
					goto Block_5;
				}
			}
		}
		int result = num;
		return result;
		Block_4:
		Block_5:
		result = num;
		return result;
	}

	public static bool smethod_63(Class8.Class17 class17_0)
	{
		return class17_0.int_18 == class17_0.int_17;
	}

	private static bool smethod_64(Class8.Class13 class13_0, Class8.Class10 class10_0)
	{
		while (true)
		{
			switch (class13_0.int_8)
			{
			case 0:
				class13_0.int_9 = Class19.smethod_19(class10_0, 5);
				if (class13_0.int_9 >= 0)
				{
					class13_0.int_9 += 257;
					Class19.smethod_8(class10_0, 5);
					class13_0.int_8 = 1;
					goto IL_2B9;
				}
				goto IL_73;
			case 1:
				goto IL_2B9;
			case 2:
				goto IL_261;
			case 3:
				goto IL_138;
			case 4:
				goto IL_96;
			case 5:
				break;
			default:
				continue;
			}
			IL_1C4:
			int int_ = Class8.Class13.int_7[class13_0.int_13];
			int num = Class19.smethod_19(class10_0, int_);
			if (num < 0)
			{
				goto Block_9;
			}
			Class19.smethod_8(class10_0, int_);
			num += Class8.Class13.int_6[class13_0.int_13];
			while (num-- > 0)
			{
				class13_0.byte_1[class13_0.int_14++] = class13_0.byte_2;
			}
			if (class13_0.int_14 == class13_0.int_12)
			{
				goto Block_11;
			}
			class13_0.int_8 = 4;
			continue;
			IL_96:
			int num2;
			while (((num2 = Class19.smethod_27(class13_0.class12_0, class10_0)) & -16) == 0)
			{
				class13_0.byte_1[class13_0.int_14++] = (class13_0.byte_2 = (byte)num2);
				if (class13_0.int_14 == class13_0.int_12)
				{
					goto Block_3;
				}
			}
			if (num2 >= 0)
			{
				if (num2 >= 17)
				{
					class13_0.byte_2 = 0;
				}
				class13_0.int_13 = num2 - 16;
				class13_0.int_8 = 5;
				goto IL_1C4;
			}
			goto IL_130;
			IL_138:
			while (class13_0.int_14 < class13_0.int_11)
			{
				int num3 = Class19.smethod_19(class10_0, 3);
				if (num3 < 0)
				{
					goto Block_7;
				}
				Class19.smethod_8(class10_0, 3);
				class13_0.byte_0[Class8.Class13.int_15[class13_0.int_14]] = (byte)num3;
				class13_0.int_14++;
			}
			class13_0.class12_0 = new Class8.Class12(class13_0.byte_0);
			class13_0.byte_0 = null;
			class13_0.int_14 = 0;
			class13_0.int_8 = 4;
			goto IL_96;
			IL_261:
			class13_0.int_11 = Class19.smethod_19(class10_0, 4);
			if (class13_0.int_11 >= 0)
			{
				class13_0.int_11 += 4;
				Class19.smethod_8(class10_0, 4);
				class13_0.byte_0 = new byte[19];
				class13_0.int_14 = 0;
				class13_0.int_8 = 3;
				goto IL_138;
			}
			goto IL_2B4;
			IL_2B9:
			class13_0.int_10 = Class19.smethod_19(class10_0, 5);
			if (class13_0.int_10 >= 0)
			{
				class13_0.int_10++;
				Class19.smethod_8(class10_0, 5);
				class13_0.int_12 = class13_0.int_9 + class13_0.int_10;
				class13_0.byte_1 = new byte[class13_0.int_12];
				class13_0.int_8 = 2;
				goto IL_261;
			}
			goto IL_31C;
		}
		IL_73:
		bool result = false;
		return result;
		Block_3:
		result = true;
		return result;
		IL_130:
		result = false;
		return result;
		Block_7:
		result = false;
		return result;
		Block_9:
		result = false;
		return result;
		Block_11:
		result = true;
		return result;
		IL_2B4:
		result = false;
		return result;
		IL_31C:
		result = false;
		return result;
	}

	private static bool smethod_65(int int_0, Class8.Class17 class17_0)
	{
		int num = 128;
		int num2 = 128;
		short[] short_ = class17_0.short_1;
		int num3 = class17_0.int_14;
		int num4 = class17_0.int_14 + class17_0.int_12;
		int num5 = System.Math.Max(class17_0.int_12, 2);
		int num6 = System.Math.Max(class17_0.int_14 - 32506, 0);
		int num7 = class17_0.int_14 + 258 - 1;
		byte b = class17_0.byte_0[num4 - 1];
		byte b2 = class17_0.byte_0[num4];
		if (num5 >= 8)
		{
			num >>= 2;
		}
		if (num2 > class17_0.int_15)
		{
			num2 = class17_0.int_15;
		}
		do
		{
			if (class17_0.byte_0[int_0 + num5] == b2 && class17_0.byte_0[int_0 + num5 - 1] == b && class17_0.byte_0[int_0] == class17_0.byte_0[num3] && class17_0.byte_0[int_0 + 1] == class17_0.byte_0[num3 + 1])
			{
				int num8 = int_0 + 2;
				num3 += 2;
				while (class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && class17_0.byte_0[++num3] == class17_0.byte_0[++num8] && num3 < num7)
				{
				}
				if (num3 > num4)
				{
					class17_0.int_11 = int_0;
					num4 = num3;
					num5 = num3 - class17_0.int_14;
					if (num5 >= num2)
					{
						break;
					}
					b = class17_0.byte_0[num4 - 1];
					b2 = class17_0.byte_0[num4];
				}
				num3 = class17_0.int_14;
			}
			if ((int_0 = ((int)short_[int_0 & 32767] & 65535)) <= num6)
			{
				break;
			}
		}
		while (--num != 0);
		class17_0.int_12 = System.Math.Min(num5, class17_0.int_15);
		return class17_0.int_12 >= 3;
	}

	private static int smethod_66(Class8.Class11 class11_0, Class8.Class10 class10_0, int int_0)
	{
		int_0 = System.Math.Min(System.Math.Min(int_0, 32768 - class11_0.int_3), class10_0.AvailableBytes);
		int num = 32768 - class11_0.int_2;
		int num2;
		if (int_0 > num)
		{
			num2 = Class19.smethod_20(class10_0, class11_0.byte_0, class11_0.int_2, num);
			if (num2 == num)
			{
				num2 += Class19.smethod_20(class10_0, class11_0.byte_0, 0, int_0 - num);
			}
		}
		else
		{
			num2 = Class19.smethod_20(class10_0, class11_0.byte_0, class11_0.int_2, int_0);
		}
		class11_0.int_2 = (class11_0.int_2 + num2 & 32767);
		class11_0.int_3 += num2;
		return num2;
	}

	private static void smethod_67(Class8.Class14 class14_0)
	{
		class14_0.int_6 |= 12;
	}

	private static int smethod_68(Class8.Class17 class17_0)
	{
		int num = (class17_0.int_10 << 5 ^ (int)class17_0.byte_0[class17_0.int_14 + 2]) & 32767;
		short num2 = class17_0.short_1[class17_0.int_14 & 32767] = class17_0.short_0[num];
		class17_0.short_0[num] = (short)class17_0.int_14;
		class17_0.int_10 = num;
		return (int)num2 & 65535;
	}

	private static void smethod_69(Class8.Class17 class17_0)
	{
		class17_0.int_10 = ((int)class17_0.byte_0[class17_0.int_14] << 5 ^ (int)class17_0.byte_0[class17_0.int_14 + 1]);
	}

	public static int smethod_70(string string_0)
	{
		int num;
		int result;
		if (System.IO.File.Exists(string_0))
		{
			using (System.IO.Stream stream = System.IO.File.Open(string_0, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
			{
				num = System.BitConverter.ToInt32(System.Security.Cryptography.HashAlgorithm.Create().ComputeHash(stream), 0);
				result = num;
				return result;
			}
		}
		Debug.LogWarning("Couldn't find file " + string_0 + " for verify.");
		num = 0;
		result = num;
		return result;
	}

	private static void smethod_71(int int_0, Class8.Class15 class15_0)
	{
		Class19.smethod_29(class15_0.class16_2);
		Class19.smethod_29(class15_0.class16_0);
		Class19.smethod_29(class15_0.class16_1);
		Class19.smethod_55(class15_0.class18_0, class15_0.class16_0.int_1 - 257, 5);
		Class19.smethod_55(class15_0.class18_0, class15_0.class16_1.int_1 - 1, 5);
		Class19.smethod_55(class15_0.class18_0, int_0 - 4, 4);
		for (int i = 0; i < int_0; i++)
		{
			Class19.smethod_55(class15_0.class18_0, (int)class15_0.class16_2.byte_0[Class8.Class15.int_8[i]], 3);
		}
		Class19.smethod_72(class15_0.class16_0, class15_0.class16_2);
		Class19.smethod_72(class15_0.class16_1, class15_0.class16_2);
	}

	private static void smethod_72(Class8.Class15.Class16 class16_0, Class8.Class15.Class16 class16_1)
	{
		int num = -1;
		int i = 0;
		while (i < class16_0.int_1)
		{
			int num2 = 1;
			int num3 = (int)class16_0.byte_0[i];
			int num4;
			int num5;
			if (num3 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			else
			{
				num4 = 6;
				num5 = 3;
				if (num != num3)
				{
					Class19.smethod_10(class16_1, num3);
					num2 = 0;
				}
			}
			num = num3;
			i++;
			while (i < class16_0.int_1)
			{
				if (num != (int)class16_0.byte_0[i])
				{
					break;
				}
				i++;
				if (++num2 >= num4)
				{
					break;
				}
			}
			if (num2 < num5)
			{
				while (num2-- > 0)
				{
					Class19.smethod_10(class16_1, num);
				}
			}
			else if (num != 0)
			{
				Class19.smethod_10(class16_1, 16);
				Class19.smethod_55(class16_0.class15_0.class18_0, num2 - 3, 2);
			}
			else if (num2 <= 10)
			{
				Class19.smethod_10(class16_1, 17);
				Class19.smethod_55(class16_0.class15_0.class18_0, num2 - 3, 3);
			}
			else
			{
				Class19.smethod_10(class16_1, 18);
				Class19.smethod_55(class16_0.class15_0.class18_0, num2 - 11, 7);
			}
		}
	}

	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", CharSet = CharSet.Auto)]
	public static extern bool GetVolumeInformation(string string_0, System.Text.StringBuilder stringBuilder_0, int int_0, out uint uint_0, out uint uint_1, out uint uint_2, System.Text.StringBuilder stringBuilder_1, int int_1);

	private static void smethod_73(byte[] byte_0, int int_0, int int_1, Class8.Class11 class11_0)
	{
		if (class11_0.int_3 > 0)
		{
			throw new System.InvalidOperationException();
		}
		if (int_1 > 32768)
		{
			int_0 += int_1 - 32768;
			int_1 = 32768;
		}
		System.Array.Copy(byte_0, int_0, class11_0.byte_0, 0, int_1);
		class11_0.int_2 = (int_1 & 32767);
	}

	private static int smethod_74(Class8.Stream0 stream0_0)
	{
		return Class19.smethod_34(stream0_0) | Class19.smethod_34(stream0_0) << 16;
	}

	public static int smethod_75(ref byte[] byte_0, int int_0)
	{
		int num = System.BitConverter.ToInt32(byte_0, int_0 - 4);
		int num2 = (int)byte_0[int_0 - 5];
		for (int i = 0; i < int_0; i++)
		{
			if (num2 >= PlayerProtect.byte_0.Length)
			{
				num2 = 0;
			}
			byte[] array = byte_0;
			int num3 = i;
			byte[] expr_3A_cp_0 = array;
			int expr_3A_cp_1 = num3;
			expr_3A_cp_0[expr_3A_cp_1] ^= PlayerProtect.byte_0[num2++];
		}
		int num4 = System.BitConverter.ToInt32(new CRC32().ComputeHash(byte_0, 0, int_0 - 5), 0);
		if (num4 == num)
		{
			int_0 -= 5;
		}
		else
		{
			if (PlayerProtect.Debug)
			{
				Debug.LogWarning("Network.Recv(" + int_0 + "): Invalid Packet.");
			}
			int_0 = 0;
		}
		return int_0;
	}

	private static void smethod_76(Class8.Class11 class11_0, int int_0, int int_1, int int_2)
	{
		while (int_1-- > 0)
		{
			class11_0.byte_0[class11_0.int_2++] = class11_0.byte_0[int_0++];
			class11_0.int_2 &= 32767;
			int_0 &= 32767;
		}
	}

	private static bool smethod_77(Class8.Class9 class9_0)
	{
		bool result;
		switch (class9_0.int_17)
		{
		case 2:
		{
			if (class9_0.bool_0)
			{
				class9_0.int_17 = 12;
				result = false;
				return result;
			}
			int num = Class19.smethod_19(class9_0.class10_0, 3);
			if (num < 0)
			{
				result = false;
				return result;
			}
			Class19.smethod_8(class9_0.class10_0, 3);
			if ((num & 1) != 0)
			{
				class9_0.bool_0 = true;
			}
			switch (num >> 1)
			{
			case 0:
				Class19.smethod_33(class9_0.class10_0);
				class9_0.int_17 = 3;
				break;
			case 1:
				class9_0.class12_0 = Class8.Class12.class12_0;
				class9_0.class12_1 = Class8.Class12.class12_1;
				class9_0.int_17 = 7;
				break;
			case 2:
				class9_0.class13_0 = new Class8.Class13();
				class9_0.int_17 = 6;
				break;
			}
			result = true;
			return result;
		}
		case 3:
			if ((class9_0.int_21 = Class19.smethod_19(class9_0.class10_0, 16)) < 0)
			{
				result = false;
				return result;
			}
			Class19.smethod_8(class9_0.class10_0, 16);
			class9_0.int_17 = 4;
			break;
		case 4:
			break;
		case 5:
			goto IL_1F6;
		case 6:
			if (!Class19.smethod_64(class9_0.class13_0, class9_0.class10_0))
			{
				result = false;
				return result;
			}
			class9_0.class12_0 = Class19.smethod_40(class9_0.class13_0);
			class9_0.class12_1 = Class19.smethod_4(class9_0.class13_0);
			class9_0.int_17 = 7;
			goto IL_24B;
		case 7:
		case 8:
		case 9:
		case 10:
			goto IL_24B;
		case 11:
			result = false;
			return result;
		case 12:
			result = false;
			return result;
		default:
			result = false;
			return result;
		}
		int num2 = Class19.smethod_19(class9_0.class10_0, 16);
		if (num2 < 0)
		{
			result = false;
			return result;
		}
		Class19.smethod_8(class9_0.class10_0, 16);
		class9_0.int_17 = 5;
		IL_1F6:
		int num3 = Class19.smethod_66(class9_0.class11_0, class9_0.class10_0, class9_0.int_21);
		class9_0.int_21 -= num3;
		if (class9_0.int_21 == 0)
		{
			class9_0.int_17 = 2;
			result = true;
			return result;
		}
		result = !class9_0.class10_0.IsNeedingInput;
		return result;
		IL_24B:
		result = Class19.smethod_54(class9_0);
		return result;
	}

	private static byte[] smethod_78(byte[] byte_0, byte[] byte_1, byte[] byte_2)
	{
		return Class19.smethod_79(byte_0, byte_2, byte_1, 3);
	}

	private static byte[] smethod_79(byte[] byte_0, byte[] byte_1, byte[] byte_2, int int_0)
	{
		byte[] result;
		try
		{
			Class8.Stream0 stream = new Class8.Stream0();
			if (int_0 == 0)
			{
				Class8.Class14 @class = new Class8.Class14();
				System.DateTime now = System.DateTime.Now;
				long num = (long)((now.Year - 1980 & 127) << 25 | now.Month << 21 | now.Day << 16 | now.Hour << 11 | now.Minute << 5 | (int)((uint)now.Second >> 1));
				uint[] array = new uint[]
				{
					0u,
					1996959894u,
					3993919788u,
					2567524794u,
					124634137u,
					1886057615u,
					3915621685u,
					2657392035u,
					249268274u,
					2044508324u,
					3772115230u,
					2547177864u,
					162941995u,
					2125561021u,
					3887607047u,
					2428444049u,
					498536548u,
					1789927666u,
					4089016648u,
					2227061214u,
					450548861u,
					1843258603u,
					4107580753u,
					2211677639u,
					325883990u,
					1684777152u,
					4251122042u,
					2321926636u,
					335633487u,
					1661365465u,
					4195302755u,
					2366115317u,
					997073096u,
					1281953886u,
					3579855332u,
					2724688242u,
					1006888145u,
					1258607687u,
					3524101629u,
					2768942443u,
					901097722u,
					1119000684u,
					3686517206u,
					2898065728u,
					853044451u,
					1172266101u,
					3705015759u,
					2882616665u,
					651767980u,
					1373503546u,
					3369554304u,
					3218104598u,
					565507253u,
					1454621731u,
					3485111705u,
					3099436303u,
					671266974u,
					1594198024u,
					3322730930u,
					2970347812u,
					795835527u,
					1483230225u,
					3244367275u,
					3060149565u,
					1994146192u,
					31158534u,
					2563907772u,
					4023717930u,
					1907459465u,
					112637215u,
					2680153253u,
					3904427059u,
					2013776290u,
					251722036u,
					2517215374u,
					3775830040u,
					2137656763u,
					141376813u,
					2439277719u,
					3865271297u,
					1802195444u,
					476864866u,
					2238001368u,
					4066508878u,
					1812370925u,
					453092731u,
					2181625025u,
					4111451223u,
					1706088902u,
					314042704u,
					2344532202u,
					4240017532u,
					1658658271u,
					366619977u,
					2362670323u,
					4224994405u,
					1303535960u,
					984961486u,
					2747007092u,
					3569037538u,
					1256170817u,
					1037604311u,
					2765210733u,
					3554079995u,
					1131014506u,
					879679996u,
					2909243462u,
					3663771856u,
					1141124467u,
					855842277u,
					2852801631u,
					3708648649u,
					1342533948u,
					654459306u,
					3188396048u,
					3373015174u,
					1466479909u,
					544179635u,
					3110523913u,
					3462522015u,
					1591671054u,
					702138776u,
					2966460450u,
					3352799412u,
					1504918807u,
					783551873u,
					3082640443u,
					3233442989u,
					3988292384u,
					2596254646u,
					62317068u,
					1957810842u,
					3939845945u,
					2647816111u,
					81470997u,
					1943803523u,
					3814918930u,
					2489596804u,
					225274430u,
					2053790376u,
					3826175755u,
					2466906013u,
					167816743u,
					2097651377u,
					4027552580u,
					2265490386u,
					503444072u,
					1762050814u,
					4150417245u,
					2154129355u,
					426522225u,
					1852507879u,
					4275313526u,
					2312317920u,
					282753626u,
					1742555852u,
					4189708143u,
					2394877945u,
					397917763u,
					1622183637u,
					3604390888u,
					2714866558u,
					953729732u,
					1340076626u,
					3518719985u,
					2797360999u,
					1068828381u,
					1219638859u,
					3624741850u,
					2936675148u,
					906185462u,
					1090812512u,
					3747672003u,
					2825379669u,
					829329135u,
					1181335161u,
					3412177804u,
					3160834842u,
					628085408u,
					1382605366u,
					3423369109u,
					3138078467u,
					570562233u,
					1426400815u,
					3317316542u,
					2998733608u,
					733239954u,
					1555261956u,
					3268935591u,
					3050360625u,
					752459403u,
					1541320221u,
					2607071920u,
					3965973030u,
					1969922972u,
					40735498u,
					2617837225u,
					3943577151u,
					1913087877u,
					83908371u,
					2512341634u,
					3803740692u,
					2075208622u,
					213261112u,
					2463272603u,
					3855990285u,
					2094854071u,
					198958881u,
					2262029012u,
					4057260610u,
					1759359992u,
					534414190u,
					2176718541u,
					4139329115u,
					1873836001u,
					414664567u,
					2282248934u,
					4279200368u,
					1711684554u,
					285281116u,
					2405801727u,
					4167216745u,
					1634467795u,
					376229701u,
					2685067896u,
					3608007406u,
					1308918612u,
					956543938u,
					2808555105u,
					3495958263u,
					1231636301u,
					1047427035u,
					2932959818u,
					3654703836u,
					1088359270u,
					936918000u,
					2847714899u,
					3736837829u,
					1202900863u,
					817233897u,
					3183342108u,
					3401237130u,
					1404277552u,
					615818150u,
					3134207493u,
					3453421203u,
					1423857449u,
					601450431u,
					3009837614u,
					3294710456u,
					1567103746u,
					711928724u,
					3020668471u,
					3272380065u,
					1510334235u,
					755167117u
				};
				uint num2 = 4294967295u;
				uint num3 = 4294967295u;
				int num4 = 0;
				int num5 = byte_2.Length;
				while (--num5 >= 0)
				{
					num3 = (array[(int)((uint)((System.UIntPtr)((num3 ^ (uint)byte_2[num4++]) & 255u)))] ^ num3 >> 8);
				}
				num3 ^= num2;
				Class19.smethod_45(stream, 67324752);
				Class19.smethod_80(stream, 20);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 8);
				Class19.smethod_45(stream, (int)num);
				Class19.smethod_45(stream, (int)num3);
				long position = stream.Position;
				Class19.smethod_45(stream, 0);
				Class19.smethod_45(stream, byte_2.Length);
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes("{data}");
				Class19.smethod_80(stream, bytes.Length);
				Class19.smethod_80(stream, 0);
				stream.Write(bytes, 0, bytes.Length);
				Class19.smethod_11(@class, byte_2);
				while (!@class.IsNeedingInput)
				{
					byte[] array2 = new byte[512];
					int num6 = Class19.smethod_46(@class, array2);
					if (num6 <= 0)
					{
						break;
					}
					stream.Write(array2, 0, num6);
				}
				@class.int_6 |= 12;
				while (!@class.IsFinished)
				{
					byte[] array3 = new byte[512];
					int num7 = Class19.smethod_46(@class, array3);
					if (num7 <= 0)
					{
						break;
					}
					stream.Write(array3, 0, num7);
				}
				long long_ = @class.long_0;
				Class19.smethod_45(stream, 33639248);
				Class19.smethod_80(stream, 20);
				Class19.smethod_80(stream, 20);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 8);
				Class19.smethod_45(stream, (int)num);
				Class19.smethod_45(stream, (int)num3);
				Class19.smethod_45(stream, (int)long_);
				Class19.smethod_45(stream, byte_2.Length);
				Class19.smethod_80(stream, bytes.Length);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 0);
				Class19.smethod_45(stream, 0);
				Class19.smethod_45(stream, 0);
				stream.Write(bytes, 0, bytes.Length);
				Class19.smethod_45(stream, 101010256);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 0);
				Class19.smethod_80(stream, 1);
				Class19.smethod_80(stream, 1);
				Class19.smethod_45(stream, 46 + bytes.Length);
				Class19.smethod_45(stream, (int)((long)(30 + bytes.Length) + long_));
				Class19.smethod_80(stream, 0);
				stream.Seek(position, System.IO.SeekOrigin.Begin);
				Class19.smethod_45(stream, (int)long_);
			}
			else if (int_0 == 1)
			{
				Class19.smethod_45(stream, 25000571);
				Class19.smethod_45(stream, byte_2.Length);
				byte[] array4;
				for (int i = 0; i < byte_2.Length; i += array4.Length)
				{
					array4 = new byte[System.Math.Min(2097151, byte_2.Length - i)];
					System.Buffer.BlockCopy(byte_2, i, array4, 0, array4.Length);
					long position2 = stream.Position;
					Class19.smethod_45(stream, 0);
					Class19.smethod_45(stream, array4.Length);
					Class8.Class14 class2 = new Class8.Class14();
					Class19.smethod_11(class2, array4);
					while (!class2.IsNeedingInput)
					{
						byte[] array5 = new byte[512];
						int num8 = Class19.smethod_46(class2, array5);
						if (num8 <= 0)
						{
							break;
						}
						stream.Write(array5, 0, num8);
					}
					class2.int_6 |= 12;
					while (!class2.IsFinished)
					{
						byte[] array6 = new byte[512];
						int num9 = Class19.smethod_46(class2, array6);
						if (num9 <= 0)
						{
							break;
						}
						stream.Write(array6, 0, num9);
					}
					long position3 = stream.Position;
					stream.Position = position2;
					Class19.smethod_45(stream, (int)class2.long_0);
					stream.Position = position3;
				}
			}
			else
			{
				if (int_0 == 2)
				{
					Class19.smethod_45(stream, 41777787);
					byte[] array7 = Class19.smethod_79(null, null, byte_2, 1);
					using (System.Security.Cryptography.ICryptoTransform cryptoTransform = Class19.smethod_13(false, byte_0, byte_1))
					{
						byte[] array8 = cryptoTransform.TransformFinalBlock(array7, 0, array7.Length);
						stream.Write(array8, 0, array8.Length);
						goto IL_544;
					}
				}
				if (int_0 == 3)
				{
					Class19.smethod_45(stream, 58555003);
					byte[] array9 = Class19.smethod_79(null, null, byte_2, 1);
					using (System.Security.Cryptography.ICryptoTransform cryptoTransform2 = Class19.smethod_49(false, byte_0, byte_1))
					{
						byte[] array10 = cryptoTransform2.TransformFinalBlock(array9, 0, array9.Length);
						stream.Write(array10, 0, array10.Length);
					}
				}
			}
			IL_544:
			stream.Flush();
			stream.Close();
			result = stream.ToArray();
		}
		catch (System.Exception ex)
		{
			Class8.string_0 = "ERR 2003: " + ex.Message;
			throw;
		}
		return result;
	}

	private static void smethod_80(Class8.Stream0 stream0_0, int int_0)
	{
		stream0_0.WriteByte((byte)(int_0 & 255));
		stream0_0.WriteByte((byte)(int_0 >> 8 & 255));
	}

	private static void smethod_81(Class8.Class11 class11_0, int int_0, int int_1)
	{
		if ((class11_0.int_3 += int_0) > 32768)
		{
			throw new System.InvalidOperationException();
		}
		int num = class11_0.int_2 - int_1 & 32767;
		int num2 = 32768 - int_0;
		if (num > num2 || class11_0.int_2 >= num2)
		{
			Class19.smethod_76(class11_0, num, int_0, int_1);
		}
		else if (int_0 <= int_1)
		{
			System.Array.Copy(class11_0.byte_0, num, class11_0.byte_0, class11_0.int_2, int_0);
			class11_0.int_2 += int_0;
		}
		else
		{
			while (int_0-- > 0)
			{
				class11_0.byte_0[class11_0.int_2++] = class11_0.byte_0[num++];
			}
		}
	}
}
