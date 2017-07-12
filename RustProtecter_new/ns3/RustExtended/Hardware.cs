using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
namespace RustExtended
{
	public class Hardware
	{
		public enum ProcessorArchitecture
		{
			X86,
			X64 = 9,
			Arm = -1,
			Itanium = 6,
			Unknown = 65535
		}
		public struct SystemInfo
		{
			public Hardware.ProcessorArchitecture ProcessorArchitecture;
			public uint PageSize;
			public IntPtr MinimumApplicationAddress;
			public IntPtr MaximumApplicationAddress;
			public IntPtr ActiveProcessorMask;
			public uint NumberOfProcessors;
			public uint ProcessorType;
			public uint AllocationGranularity;
			public ushort ProcessorLevel;
			public ushort ProcessorRevision;
		}
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class Class0
		{
			public uint uint_0;
			public uint uint_1;
			public ulong ulong_0;
			public ulong ulong_1;
			public ulong ulong_2;
			public ulong ulong_3;
			public ulong ulong_4;
			public ulong ulong_5;
			public ulong ulong_6;
			public Class0()
			{
				this.uint_0 = (uint)Marshal.SizeOf(typeof(Hardware.Class0));
			}
		}
		[StructLayout(LayoutKind.Sequential, Size = 8)]
		private class Class1
		{
			public byte byte_0;
			public byte byte_1;
			public byte byte_2;
			public byte byte_3;
			public byte byte_4;
			public byte byte_5;
			public byte byte_6;
			public byte byte_7;
		}
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		private class Class2
		{
			public int int_0;
			public Hardware.Class1 class1_0;
			public byte byte_0;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byte_1;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public int[] int_1;
			public Class2()
			{
				this.class1_0 = new Hardware.Class1();
				this.byte_1 = new byte[3];
				this.int_1 = new int[4];
			}
		}
		[StructLayout(LayoutKind.Sequential, Size = 12)]
		private class Class3
		{
			public byte byte_0;
			public byte byte_1;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] byte_2;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] int_0;
			public Class3()
			{
				this.byte_2 = new byte[2];
				this.int_0 = new int[2];
			}
		}
		[StructLayout(LayoutKind.Sequential)]
		private class Class4
		{
			public short short_0;
			public short short_1;
			public short short_2;
			public short short_3;
			public short short_4;
			public short short_5;
			public short short_6;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public short[] short_7;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			public char[] char_0;
			public short short_8;
			public short short_9;
			public short short_10;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] char_1;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
			public char[] char_2;
			public short short_11;
			public short short_12;
			public short short_13;
			public short short_14;
			public short short_15;
			public short short_16;
			public short short_17;
			public short short_18;
			public short short_19;
			public short short_20;
			public int int_0;
			public short short_21;
			public short short_22;
			public int int_1;
			public short short_23;
			public short short_24;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 382)]
			public byte[] byte_0;
			public Class4()
			{
				this.short_7 = new short[3];
				this.byte_0 = new byte[382];
				this.char_1 = new char[8];
				this.char_0 = new char[20];
				this.char_2 = new char[40];
			}
		}
		[StructLayout(LayoutKind.Sequential)]
		private class Class5
		{
			public int int_0;
			public Hardware.Class3 class3_0;
			public Hardware.Class4 class4_0;
			public Class5()
			{
				this.class3_0 = new Hardware.Class3();
				this.class4_0 = new Hardware.Class4();
			}
		}
		private const int int_0 = 1;
		private const int int_1 = 3;
		private const uint uint_0 = 2147483648u;
		private const uint uint_1 = 1073741824u;
		private const int int_2 = 1;
		private const int int_3 = 2;
		private const int int_4 = 2;
		private const int int_5 = 508040;
		private const int int_6 = -1;
		public static string MachineID
		{
			get
			{
				byte[] array = Hardware.CalculateID();
				if (array != null && array.Length >= 1)
				{
					return BitConverter.ToString(array).Replace("-", "").ToLower();
				}
				return "";
			}
		}
		[DllImport("KERNEL32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern void GetSystemInfo(out Hardware.SystemInfo systemInfo_0);
		[DllImport("KERNEL32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GlobalMemoryStatusEx([In] [Out] Hardware.Class0 class0_0);
		[DllImport("KERNEL32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetVolumeInformation(string string_0, StringBuilder stringBuilder_0, int int_7, out uint uint_2, out uint uint_3, out uint uint_4, StringBuilder stringBuilder_1, uint uint_5);
		[DllImport("KERNEL32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetDiskFreeSpaceEx(string string_0, out ulong ulong_0, out ulong ulong_1, out ulong ulong_2);
		[DllImport("USER32.DLL", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr CallWindowProcW([In] byte[] byte_0, IntPtr intptr_0, int int_7, [In] [Out] byte[] byte_1, IntPtr intptr_1);
		[DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool VirtualProtect([In] byte[] byte_0, IntPtr intptr_0, int int_7, out int int_8);
		[DllImport("KERNEL32.DLL")]
		private static extern int CreateFile(string string_0, uint uint_2, int int_7, int int_8, int int_9, int int_10, int int_11);
		[DllImport("KERNEL32.DLL")]
		private static extern int DeviceIoControl(int int_7, int int_8, [In] [Out] Hardware.Class2 class2_0, int int_9, [In] [Out] Hardware.Class5 class5_0, int int_10, ref int int_11, int int_12);
		[DllImport("KERNEL32.DLL")]
		private static extern int CloseHandle(int int_7);
		private static string smethod_0(char[] char_0)
		{
			for (int i = 0; i < char_0.Length - 1; i += 2)
			{
				char c = char_0[i];
				char_0[i] = char_0[i + 1];
				char_0[i + 1] = c;
			}
			return new string(char_0);
		}
		private static bool smethod_1(string string_0, out Hardware.Class5 class5_0)
		{
			int num = 0;
			int num2 = 0;
			Hardware.Class2 @class = new Hardware.Class2();
			class5_0 = new Hardware.Class5();
			int num3;
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				num3 = Hardware.CreateFile("\\\\.\\Smartvsd", 0u, 0, 0, 1, 0, 0);
			}
			else
			{
				num3 = Hardware.CreateFile("\\\\.\\" + string_0[0] + ":", 3221225472u, 3, 0, 3, 0, 0);
			}
			if (num3 != -1)
			{
				@class.byte_0 = (byte)num;
				@class.int_0 = Marshal.SizeOf(class5_0);
				@class.class1_0.byte_5 = (byte)(160 | num << 4);
				@class.class1_0.byte_6 = 236;
				@class.class1_0.byte_1 = 1;
				@class.class1_0.byte_2 = 1;
				if (Hardware.DeviceIoControl(num3, 508040, @class, Marshal.SizeOf(@class), class5_0, Marshal.SizeOf(class5_0), ref num2, 0) != 0)
				{
					return true;
				}
			}
			return false;
		}
		private static bool smethod_2(byte byte_0, out Hardware.Class5 class5_0)
		{
			int num = 0;
			int num2 = 0;
			Hardware.Class2 @class = new Hardware.Class2();
			class5_0 = new Hardware.Class5();
			int num3;
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				num3 = Hardware.CreateFile("\\\\.\\Smartvsd", 0u, 0, 0, 1, 0, 0);
			}
			else
			{
				num3 = Hardware.CreateFile("\\\\.\\PhysicalDrive" + byte_0, 3221225472u, 3, 0, 3, 0, 0);
			}
			if (num3 != -1)
			{
				@class.byte_0 = (byte)num;
				@class.int_0 = Marshal.SizeOf(class5_0);
				@class.class1_0.byte_5 = (byte)(160 | num << 4);
				@class.class1_0.byte_6 = 236;
				@class.class1_0.byte_1 = 1;
				@class.class1_0.byte_2 = 1;
				if (Hardware.DeviceIoControl(num3, 508040, @class, Marshal.SizeOf(@class), class5_0, Marshal.SizeOf(class5_0), ref num2, 0) != 0)
				{
					return true;
				}
			}
			return false;
		}
		public static byte[] CalculateID()
		{
			byte[] array = null;
			using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream()))
			{
				int num = (Environment.ProcessorCount > 2) ? (Environment.ProcessorCount / 2) : Environment.ProcessorCount;
				if (num <= 0)
				{
					num = 1;
				}
				binaryWriter.Write(num);
				byte[] array2 = new byte[]
				{
					85,
					137,
					229,
					87,
					139,
					125,
					16,
					106,
					1,
					88,
					83,
					15,
					162,
					137,
					7,
					137,
					87,
					4,
					91,
					95,
					137,
					236,
					93,
					194,
					16,
					0
				};
				byte[] array3 = new byte[]
				{
					83,
					72,
					199,
					192,
					1,
					0,
					0,
					0,
					15,
					162,
					65,
					137,
					0,
					65,
					137,
					80,
					4,
					91,
					195
				};
				byte[] array4 = new byte[8];
				byte[] array5;
				if (IntPtr.Size == 8)
				{
					array5 = array3;
				}
				else
				{
					array5 = array2;
				}
				IntPtr intPtr = new IntPtr(array5.Length);
				int num2;
				if (!Hardware.VirtualProtect(array5, intPtr, 64, out num2))
				{
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
				intPtr = new IntPtr(array4.Length);
				if (Hardware.CallWindowProcW(array5, IntPtr.Zero, 0, array4, intPtr) == IntPtr.Zero)
				{
					byte[] result = array;
					return result;
				}
				binaryWriter.Write(array4);
				Hardware.Class5 @class = new Hardware.Class5();
				string pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
				Hardware.Class5 class2 = new Hardware.Class5();
				string pathRoot2 = Path.GetPathRoot(Environment.CurrentDirectory);
				if (Hardware.smethod_1(pathRoot, out @class) && Hardware.smethod_1(pathRoot2, out class2))
				{
					binaryWriter.Write(@class.class4_0.char_2);
					binaryWriter.Write(@class.class4_0.char_0);
					binaryWriter.Write(class2.class4_0.char_2);
					binaryWriter.Write(class2.class4_0.char_0);
				}
				else
				{
					uint value = 0u;
					uint value2 = 0u;
					uint num3 = 0u;
					uint num4 = 0u;
					if (!Hardware.GetVolumeInformation(pathRoot, null, 0, out value, out num3, out num4, null, 0u))
					{
						byte[] result = array;
						return result;
					}
					if (!Hardware.GetVolumeInformation(pathRoot2, null, 0, out value2, out num3, out num4, null, 0u))
					{
						byte[] result = array;
						return result;
					}
					binaryWriter.Write(value);
					binaryWriter.Write(value2);
				}
				binaryWriter.BaseStream.Position = 0L;
				array = new MD5CryptoServiceProvider().ComputeHash(binaryWriter.BaseStream);
			}
			return array;
		}
	}
}
