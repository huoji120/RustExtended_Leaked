using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RustProtect
{
	public class Process32
	{
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

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern System.IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Process32First(System.IntPtr hSnapshot, ref ProcessEntry32 lppe);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Process32Next(System.IntPtr hSnapshot, ref ProcessEntry32 lppe);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern System.IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
		public static extern bool ReadProcessMemory(System.IntPtr hProcess, System.IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool WriteProcessMemory(System.IntPtr hProcess, System.IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Module32First(System.IntPtr hSnapshot, ref ModuleEntry32 lpme);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", SetLastError = true)]
		public static extern bool Module32Next(System.IntPtr hSnapshot, ref ModuleEntry32 lpme);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
		public static extern int CloseHandle(System.IntPtr hSnapshot);

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall)]
		public static extern uint GetCurrentProcessId();

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall)]
		public static extern System.IntPtr GetCurrentProcess();

		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint GetLogicalDriveStrings(uint bufferLength, [System.Runtime.InteropServices.Out] char[] buffer);

		[System.Runtime.InteropServices.DllImport("PSAPI.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint GetModuleFileNameEx(System.IntPtr hProcess, System.IntPtr hModule, [System.Runtime.InteropServices.Out] System.Text.StringBuilder lpBaseName, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] [System.Runtime.InteropServices.In] int nSize);

		[System.Runtime.InteropServices.DllImport("PSAPI.DLL")]
		public static extern uint GetProcessImageFileName(System.IntPtr hProcess, [System.Runtime.InteropServices.Out] System.Text.StringBuilder lpImageFileName, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] [System.Runtime.InteropServices.In] int nSize);

		[System.Runtime.InteropServices.DllImport("PSAPI.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int EnumProcessModules(System.IntPtr hProcess, [System.Runtime.InteropServices.Out] System.IntPtr lphModule, uint cb, out uint lpcbNeeded);

		[System.Runtime.InteropServices.DllImport("PSAPI.DLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public static extern uint GetModuleBaseName(System.IntPtr hProcess, System.IntPtr hModule, [System.Runtime.InteropServices.Out] System.Text.StringBuilder lpBaseName, uint nSize);

		public static System.Collections.Generic.List<ProcessEntry32> GetProcess32List()
		{
			System.Collections.Generic.List<ProcessEntry32> list = new System.Collections.Generic.List<ProcessEntry32>();
			System.IntPtr intPtr = Process32.CreateToolhelp32Snapshot(Process32.TH32CS_SNAPPROCESS, 0u);
			if (intPtr != System.IntPtr.Zero)
			{
				ProcessEntry32 processEntry = default(ProcessEntry32);
				processEntry.dwSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(processEntry);
				if (Process32.Process32First(intPtr, ref processEntry))
				{
					do
					{
						System.IntPtr intPtr2 = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)processEntry.dwSize);
						System.Runtime.InteropServices.Marshal.StructureToPtr(processEntry, intPtr2, true);
						ProcessEntry32 item = (ProcessEntry32)System.Runtime.InteropServices.Marshal.PtrToStructure(intPtr2, typeof(ProcessEntry32));
						System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr2);
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
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(260);
			System.IntPtr intPtr = Process32.OpenProcess(Process32.PROCESS_QUERY_INFORMATION | Process32.PROCESS_VM_READ, 0, lpProcess.th32ProcessID);
			Process32.GetModuleFileNameEx(intPtr, System.IntPtr.Zero, stringBuilder, 260);
			if (stringBuilder.Length == 0)
			{
				Process32.GetProcessImageFileName(intPtr, stringBuilder, 260);
			}
			Process32.CloseHandle(intPtr);
			string text = stringBuilder.ToString();
			if (text.Contains("\\Device\\HardDiskVolume", true))
			{
				string[] logicalDrives = System.IO.Directory.GetLogicalDrives();
				for (int i = 0; i < logicalDrives.Length; i++)
				{
					text = text.Replace("\\Device\\HarddiskVolume" + (i + 1) + "\\", logicalDrives[i]);
				}
			}
			return text;
		}

		public static bool GetProcessFileName(Process process, out string result)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(1024);
			result = null;
			System.IntPtr intPtr = Process32.OpenProcess(1040u, 0, (uint)process.Id);
			Process32.GetModuleFileNameEx(intPtr, System.IntPtr.Zero, stringBuilder, 1024);
			Process32.CloseHandle(intPtr);
			result = stringBuilder.ToString();
			return !string.IsNullOrEmpty(result) && System.IO.File.Exists(result);
		}

		public static bool GetProcessModules(Process process, out string[] modules)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			System.IntPtr hProcess = Process32.OpenProcess(1040u, 0, (uint)process.Id);
			System.IntPtr[] array = new System.IntPtr[1024];
			uint num = 0u;
			uint cb = (uint)(System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.IntPtr)) * array.Length);
			System.Runtime.InteropServices.GCHandle gCHandle = System.Runtime.InteropServices.GCHandle.Alloc(array, System.Runtime.InteropServices.GCHandleType.Pinned);
			System.IntPtr lphModule = gCHandle.AddrOfPinnedObject();
			if (Process32.EnumProcessModules(hProcess, lphModule, cb, out num) == 1)
			{
				int num2 = (int)((ulong)num / (ulong)((long)System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.IntPtr))));
				for (int i = 0; i < num2; i++)
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(1024);
					Process32.GetModuleFileNameEx(hProcess, array[i], stringBuilder, stringBuilder.Capacity);
					list.Add(stringBuilder.ToString());
				}
			}
			gCHandle.Free();
			modules = list.ToArray();
			return true;
		}

		public static byte[] ReadMemory(Process process, int address, int length, out int bytesRead)
		{
			byte[] array = new byte[length];
			bytesRead = 0;
			System.IntPtr intPtr = Process32.OpenProcess(Process32.PROCESS_ALL_ACCESS, 0, (uint)process.Id);
			if (intPtr != System.IntPtr.Zero)
			{
				Process32.ReadProcessMemory(intPtr, new System.IntPtr(address), array, length, out bytesRead);
				Process32.CloseHandle(intPtr);
			}
			return array;
		}

		public static byte[] ReadMemory(ProcessEntry32 process, int address, int length, out int bytesRead)
		{
			System.IntPtr intPtr = Process32.OpenProcess(Process32.PROCESS_ALL_ACCESS, 0, process.th32ProcessID);
			byte[] result;
			if (intPtr != System.IntPtr.Zero)
			{
				byte[] array = new byte[length];
				Process32.ReadProcessMemory(intPtr, new System.IntPtr(address), array, length, out bytesRead);
				Process32.CloseHandle(intPtr);
				result = array;
			}
			else
			{
				bytesRead = 0;
				result = new byte[0];
			}
			return result;
		}
	}
}
