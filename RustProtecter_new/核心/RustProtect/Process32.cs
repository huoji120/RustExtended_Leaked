using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RustProtect
{
	public class Process32
	{
		public struct LUID
		{
			public uint LowPart;

			public int HighPart;
		}

		public struct LUID_AND_ATTRIBUTES
		{
			public Process32.LUID Luid;

			public uint Attributes;
		}

		public enum TOKEN_INFORMATION_CLASS
		{
			TokenUser = 1,
			TokenGroups,
			TokenPrivileges,
			TokenOwner,
			TokenPrimaryGroup,
			TokenDefaultDacl,
			TokenSource,
			TokenType,
			TokenImpersonationLevel,
			TokenStatistics,
			TokenRestrictedSids,
			TokenSessionId,
			TokenGroupsAndPrivileges,
			TokenSessionReference,
			TokenSandBoxInert,
			TokenAuditPolicy,
			TokenOrigin,
			TokenElevationType,
			TokenLinkedToken,
			TokenElevation,
			TokenHasRestrictions,
			TokenAccessInformation,
			TokenVirtualizationAllowed,
			TokenVirtualizationEnabled,
			TokenIntegrityLevel,
			TokenUIAccess,
			TokenMandatoryPolicy,
			TokenLogonSid,
			MaxTokenInfoClass
		}

		public enum TOKEN_ELEVATION_TYPE
		{
			TokenElevationTypeDefault = 1,
			TokenElevationTypeFull,
			TokenElevationTypeLimited
		}

		public struct TOKEN_PRIVILEGES
		{
			public uint PrivilegeCount;

			public Process32.LUID Luid;

			public uint Attributes;
		}

		public static uint TH32CS_SNAPHEAPLIST = 1u;

		public static uint TH32CS_SNAPPROCESS = 2u;

		public static uint TH32CS_SNAPTHREAD = 4u;

		public static uint TH32CS_SNAPMODULE = 8u;

		public static uint TH32CS_SNAPMODULE32 = 16u;

		public static uint TH32CS_SNAPALL = 15u;

		public static uint TH32CS_INHERIT = 2147483648u;

		public static uint PROCESS_ALL_ACCESS = 2035711u;

		public static uint PROCESS_TERMINATE = 1u;

		public static uint PROCESS_CREATE_THREAD = 2u;

		public static uint PROCESS_VM_OPERATION = 8u;

		public static uint PROCESS_VM_READ = 16u;

		public static uint PROCESS_VM_WRITE = 32u;

		public static uint PROCESS_DUP_HANDLE = 64u;

		public static uint PROCESS_CREATE_PROCESS = 128u;

		public static uint PROCESS_SET_QUOTA = 256u;

		public static uint PROCESS_SET_INFORMATION = 512u;

		public static uint PROCESS_QUERY_INFORMATION = 1024u;

		public static uint PROCESS_SUSPEND_RESUME = 2048u;

		public static uint PROCESS_QUERY_LIMITED_INFORMATION = 4096u;

		public static uint SYNCHRONIZE = 1048576u;

		public static uint STANDARD_RIGHTS_REQUIRED = 983040u;

		public static uint STANDARD_RIGHTS_READ = 131072u;

		public static uint TOKEN_ASSIGN_PRIMARY = 1u;

		public static uint TOKEN_DUPLICATE = 2u;

		public static uint TOKEN_IMPERSONATE = 4u;

		public static uint TOKEN_QUERY = 8u;

		public static uint TOKEN_QUERY_SOURCE = 16u;

		public static uint TOKEN_ADJUST_PRIVILEGES = 32u;

		public static uint TOKEN_ADJUST_GROUPS = 64u;

		public static uint TOKEN_ADJUST_DEFAULT = 128u;

		public static uint TOKEN_ADJUST_SESSIONID = 256u;

		public static uint TOKEN_READ = 131080u;

		public static uint TOKEN_ALL_ACCESS = 983551u;

		public const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1u;

		public const uint SE_PRIVILEGE_ENABLED = 2u;

		public const uint SE_PRIVILEGE_REMOVED = 4u;

		public const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648u;

		public const string SE_DEBUG_NAME = "SeDebugPrivilege";

		public static bool IsUACEnabled
		{
			get
			{
				return Registry.LocalMachine.OpenSubKey(Class3.smethod_10(398), false).GetValue(Class3.smethod_10(516)).Equals(1);
			}
		}

		public static bool IsRunAsAdministrator
		{
			get
			{
				return Process32.IsUserAnAdmin();
			}
		}

		[DllImport("shell32.dll", SetLastError = true)]
		public static extern bool IsUserAnAdmin();

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool GetTokenInformation(IntPtr TokenHandle, Process32.TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, int TokenInformationLength, out int ReturnLength);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool PrivilegeCheck(IntPtr ClientToken, IntPtr RequiredPrivileges, ref int pfResult);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out Process32.LUID lpLuid);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref Process32.TOKEN_PRIVILEGES NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Process32First(IntPtr hSnapshot, ref ProcessEntry32 lppe);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Process32Next(IntPtr hSnapshot, ref ProcessEntry32 lppe);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

		[DllImport("KERNEL32.DLL")]
		public static extern bool ReadProcessMemory(IntPtr handle, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool WriteProcessMemory(IntPtr handle, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Module32First(IntPtr handle, ref ModuleEntry32 entry);

		[DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Module32Next(IntPtr handle, ref ModuleEntry32 entry);

		[DllImport("KERNEL32.DLL")]
		public static extern int CloseHandle(IntPtr handle);

		[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall)]
		public static extern uint GetCurrentProcessId();

		[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [MarshalAs(UnmanagedType.U4)] [In] int nSize);

		[DllImport("psapi.dll")]
		public static extern uint GetProcessImageFileName(IntPtr hProcess, [Out] StringBuilder lpImageFileName, [MarshalAs(UnmanagedType.U4)] [In] int nSize);

		[DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, out uint lpcbNeeded);

		[DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

		public static bool SetPrivilege(string privilege, bool enabled = true)
		{
			IntPtr tokenHandle;
			Process32.LUID luid;
			if (Process32.OpenProcessToken(Process32.GetCurrentProcess(), Process32.TOKEN_ADJUST_PRIVILEGES | Process32.TOKEN_QUERY, out tokenHandle) && Process32.LookupPrivilegeValue(null, privilege, out luid))
			{
				Process32.TOKEN_PRIVILEGES tOKEN_PRIVILEGES;
				tOKEN_PRIVILEGES.PrivilegeCount = 1u;
				tOKEN_PRIVILEGES.Luid = luid;
				tOKEN_PRIVILEGES.Attributes = 2u;
				return Process32.AdjustTokenPrivileges(tokenHandle, false, ref tOKEN_PRIVILEGES, 1024, IntPtr.Zero, IntPtr.Zero);
			}
			return false;
		}

		public static List<ProcessEntry32> GetProcess32List()
		{
			Process32.SetPrivilege(Class3.smethod_10(538), true);
			Process.EnterDebugMode();
			List<ProcessEntry32> list = new List<ProcessEntry32>();
			IntPtr intPtr = Process32.CreateToolhelp32Snapshot(Process32.TH32CS_SNAPPROCESS, 0u);
			if (intPtr != IntPtr.Zero)
			{
				ProcessEntry32 processEntry = default(ProcessEntry32);
				processEntry.dwSize = (uint)Marshal.SizeOf(processEntry);
				if (Process32.Process32First(intPtr, ref processEntry))
				{
					do
					{
						IntPtr intPtr2 = Marshal.AllocHGlobal((int)processEntry.dwSize);
						Marshal.StructureToPtr(processEntry, intPtr2, true);
						ProcessEntry32 item = (ProcessEntry32)Marshal.PtrToStructure(intPtr2, typeof(ProcessEntry32));
						Marshal.FreeHGlobal(intPtr2);
						list.Add(item);
					}
					while (Process32.Process32Next(intPtr, ref processEntry));
				}
				Process32.CloseHandle(intPtr);
			}
			return list;
		}

		public static string GetProcess32File(ProcessEntry32 lpProcess)
		{
			int nSize = 1024;
			StringBuilder stringBuilder = new StringBuilder(1024);
			IntPtr intPtr = Process32.OpenProcess(Process32.PROCESS_QUERY_INFORMATION | Process32.PROCESS_VM_READ, 0, lpProcess.th32ProcessID);
			Process32.GetModuleFileNameEx(intPtr, IntPtr.Zero, stringBuilder, 1024);
			if (stringBuilder.Length == 0)
			{
				Process32.GetProcessImageFileName(intPtr, stringBuilder, nSize);
			}
			Process32.CloseHandle(intPtr);
			return stringBuilder.ToString();
		}

		public static byte[] ReadMemory(Process process, int address, int length, out int bytesRead)
		{
			return Process32.ReadMemory((uint)process.Id, address, length, out bytesRead);
		}

		public static byte[] ReadMemory(ProcessEntry32 process, int address, int length, out int bytesRead)
		{
			return Process32.ReadMemory(process.th32ProcessID, address, length, out bytesRead);
		}

		public static byte[] ReadMemory(uint pid, int address, int length, out int bytesRead)
		{
			IntPtr intPtr = Process32.OpenProcess(Process32.PROCESS_ALL_ACCESS, 0, pid);
			if (intPtr != IntPtr.Zero)
			{
				byte[] array = new byte[length];
				Process32.ReadProcessMemory(intPtr, new IntPtr(address), array, length, out bytesRead);
				Process32.CloseHandle(intPtr);
				return array;
			}
			bytesRead = 0;
			return new byte[0];
		}

		public static byte ReadByte(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[1];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 1, out num);
			return array[0];
		}

		public static byte[] ReadBytes(IntPtr processHandle, long lpBaseAddress, int _Size)
		{
			int num = 0;
			byte[] array = new byte[_Size];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, _Size, out num);
			if (num == 0)
			{
				return new byte[0];
			}
			return array;
		}

		public static bool ReadBoolean(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[1];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 1, out num);
			return Convert.ToBoolean(array[0]);
		}

		public static short smethod_0(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[2];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 2, out num);
			return BitConverter.ToInt16(array, 0);
		}

		public static ushort smethod_1(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[2];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 2, out num);
			return BitConverter.ToUInt16(array, 0);
		}

		public static int smethod_2(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[4];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 4, out num);
			return BitConverter.ToInt32(array, 0);
		}

		public static uint smethod_3(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[4];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 4, out num);
			return BitConverter.ToUInt32(array, 0);
		}

		public static long smethod_4(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[8];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 8, out num);
			return BitConverter.ToInt64(array, 0);
		}

		public static ulong smethod_5(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[8];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 8, out num);
			return BitConverter.ToUInt64(array, 0);
		}

		public static float ReadFloat(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[4];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 4, out num);
			return BitConverter.ToSingle(array, 0);
		}

		public static double ReadDouble(IntPtr processHandle, long lpBaseAddress)
		{
			int num = 0;
			byte[] array = new byte[8];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, 8, out num);
			return (double)BitConverter.ToSingle(array, 0);
		}

		public static string ReadString(IntPtr processHandle, long lpBaseAddress, int size)
		{
			int num = 0;
			byte[] array = new byte[size];
			Process32.ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), array, size, out num);
			int num2 = array.IndexOf(0, 0);
			if (num2 < 0)
			{
				num2 = array.Length;
			}
			return Encoding.UTF8.GetString(array, 0, num2);
		}

		public static string ReadString(IntPtr processHandle, long lpBaseAddress)
		{
			return Process32.ReadString(processHandle, lpBaseAddress, 260);
		}
	}
}
