namespace RustProtect
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Process32
    {
        public static uint PROCESS_ALL_ACCESS = 0x1f0fff;
        public static uint PROCESS_CREATE_PROCESS = 0x80;
        public static uint PROCESS_CREATE_THREAD = 2;
        public static uint PROCESS_DUP_HANDLE = 0x40;
        public static uint PROCESS_QUERY_INFORMATION = 0x400;
        public static uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        public static uint PROCESS_SET_INFORMATION = 0x200;
        public static uint PROCESS_SET_QUOTA = 0x100;
        public static uint PROCESS_SUSPEND_RESUME = 0x800;
        public static uint PROCESS_TERMINATE = 1;
        public static uint PROCESS_VM_OPERATION = 8;
        public static uint PROCESS_VM_READ = 0x10;
        public static uint PROCESS_VM_WRITE = 0x20;
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";
        public const uint SE_PRIVILEGE_ENABLED = 2;
        public const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1;
        public const uint SE_PRIVILEGE_REMOVED = 4;
        public const uint SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;
        public static uint STANDARD_RIGHTS_READ = 0x20000;
        public static uint STANDARD_RIGHTS_REQUIRED = 0xf0000;
        public static uint SYNCHRONIZE = 0x100000;
        public static uint TH32CS_INHERIT = 0x80000000;
        public static uint TH32CS_SNAPALL = 15;
        public static uint TH32CS_SNAPHEAPLIST = 1;
        public static uint TH32CS_SNAPMODULE = 8;
        public static uint TH32CS_SNAPMODULE32 = 0x10;
        public static uint TH32CS_SNAPPROCESS = 2;
        public static uint TH32CS_SNAPTHREAD = 4;
        public static uint TOKEN_ADJUST_DEFAULT = 0x80;
        public static uint TOKEN_ADJUST_GROUPS = 0x40;
        public static uint TOKEN_ADJUST_PRIVILEGES = 0x20;
        public static uint TOKEN_ADJUST_SESSIONID = 0x100;
        public static uint TOKEN_ALL_ACCESS = 0xf01ff;
        public static uint TOKEN_ASSIGN_PRIMARY = 1;
        public static uint TOKEN_DUPLICATE = 2;
        public static uint TOKEN_IMPERSONATE = 4;
        public static uint TOKEN_QUERY = 8;
        public static uint TOKEN_QUERY_SOURCE = 0x10;
        public static uint TOKEN_READ = 0x20008;

        [DllImport("advapi32.dll", SetLastError=true)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);
        [DllImport("KERNEL32.DLL")]
        public static extern int CloseHandle(IntPtr handle);
        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);
        [DllImport("psapi.dll", CallingConvention=CallingConvention.StdCall, SetLastError=true)]
        public static extern int EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, out uint lpcbNeeded);
        [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall)]
        public static extern IntPtr GetCurrentProcess();
        [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall)]
        public static extern uint GetCurrentProcessId();
        [DllImport("psapi.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode)]
        public static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);
        [DllImport("psapi.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In, MarshalAs(UnmanagedType.U4)] int nSize);
        public static string GetProcess32File(ProcessEntry32 lpProcess)
        {
            int nSize = 0x400;
            StringBuilder lpBaseName = new StringBuilder(0x400);
            IntPtr hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, 0, lpProcess.th32ProcessID);
            GetModuleFileNameEx(hProcess, IntPtr.Zero, lpBaseName, 0x400);
            if (lpBaseName.Length == 0)
            {
                GetProcessImageFileName(hProcess, lpBaseName, nSize);
            }
            CloseHandle(hProcess);
            return lpBaseName.ToString();
        }

        public static System.Collections.Generic.List<ProcessEntry32> GetProcess32List()
        {
            SetPrivilege(Class3.smethod_10(0x21a), true);
            Process.EnterDebugMode();
            System.Collections.Generic.List<ProcessEntry32> list = new System.Collections.Generic.List<ProcessEntry32>();
            IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (hSnapshot != IntPtr.Zero)
            {
                ProcessEntry32 entry;
                entry = new ProcessEntry32 {
                    dwSize = (uint) Marshal.SizeOf(entry)
                };
                if (Process32First(hSnapshot, ref entry))
                {
                    do
                    {
                        IntPtr ptr = Marshal.AllocHGlobal((int) entry.dwSize);
                        Marshal.StructureToPtr(entry, ptr, true);
                        ProcessEntry32 item = (ProcessEntry32) Marshal.PtrToStructure(ptr, typeof(ProcessEntry32));
                        Marshal.FreeHGlobal(ptr);
                        list.Add(item);
                    }
                    while (Process32Next(hSnapshot, ref entry));
                }
                CloseHandle(hSnapshot);
            }
            return list;
        }

        [DllImport("psapi.dll")]
        public static extern uint GetProcessImageFileName(IntPtr hProcess, [Out] StringBuilder lpImageFileName, [In, MarshalAs(UnmanagedType.U4)] int nSize);
        [DllImport("advapi32.dll", SetLastError=true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, int TokenInformationLength, out int ReturnLength);
        [DllImport("shell32.dll", SetLastError=true)]
        public static extern bool IsUserAnAdmin();
        [DllImport("advapi32.dll", SetLastError=true)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);
        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern bool Module32First(IntPtr handle, ref ModuleEntry32 entry);
        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern bool Module32Next(IntPtr handle, ref ModuleEntry32 entry);
        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("advapi32.dll", SetLastError=true)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("advapi32.dll", SetLastError=true)]
        public static extern bool PrivilegeCheck(IntPtr ClientToken, IntPtr RequiredPrivileges, ref int pfResult);
        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern bool Process32First(IntPtr hSnapshot, ref ProcessEntry32 lppe);
        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern bool Process32Next(IntPtr hSnapshot, ref ProcessEntry32 lppe);
        public static bool ReadBoolean(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[1];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 1, out lpNumberOfBytesRead);
            return Convert.ToBoolean(lpBuffer[0]);
        }

        public static byte ReadByte(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[1];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 1, out lpNumberOfBytesRead);
            return lpBuffer[0];
        }

        public static byte[] ReadBytes(IntPtr processHandle, long lpBaseAddress, int _Size)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[_Size];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, _Size, out lpNumberOfBytesRead);
            if (lpNumberOfBytesRead == 0)
            {
                return new byte[0];
            }
            return lpBuffer;
        }

        public static double ReadDouble(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[8];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 8, out lpNumberOfBytesRead);
            return (double) BitConverter.ToSingle(lpBuffer, 0);
        }

        public static float ReadFloat(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[4];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 4, out lpNumberOfBytesRead);
            return BitConverter.ToSingle(lpBuffer, 0);
        }

        public static byte[] ReadMemory(ProcessEntry32 process, int address, int length, out int bytesRead)
        {
            return ReadMemory(process.th32ProcessID, address, length, out bytesRead);
        }

        public static byte[] ReadMemory(Process process, int address, int length, out int bytesRead)
        {
            return ReadMemory((uint) process.Id, address, length, out bytesRead);
        }

        public static byte[] ReadMemory(uint pid, int address, int length, out int bytesRead)
        {
            IntPtr handle = OpenProcess(PROCESS_ALL_ACCESS, 0, pid);
            if (handle != IntPtr.Zero)
            {
                byte[] lpBuffer = new byte[length];
                ReadProcessMemory(handle, new IntPtr(address), lpBuffer, length, out bytesRead);
                CloseHandle(handle);
                return lpBuffer;
            }
            bytesRead = 0;
            return new byte[0];
        }

        [DllImport("KERNEL32.DLL")]
        public static extern bool ReadProcessMemory(IntPtr handle, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        public static string ReadString(IntPtr processHandle, long lpBaseAddress)
        {
            return ReadString(processHandle, lpBaseAddress, 260);
        }

        public static string ReadString(IntPtr processHandle, long lpBaseAddress, int size)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[size];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, size, out lpNumberOfBytesRead);
            int count = lpBuffer.IndexOf(0, 0);
            if (count < 0)
            {
                count = lpBuffer.Length;
            }
            return Encoding.UTF8.GetString(lpBuffer, 0, count);
        }

        public static bool SetPrivilege(string privilege, [Optional, DefaultParameterValue(true)] bool enabled)
        {
            IntPtr ptr;
            LUID luid;
            if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out ptr) && LookupPrivilegeValue(null, privilege, out luid))
            {
                TOKEN_PRIVILEGES token_privileges;
                token_privileges.PrivilegeCount = 1;
                token_privileges.Luid = luid;
                token_privileges.Attributes = 2;
                return AdjustTokenPrivileges(ptr, false, ref token_privileges, 0x400, IntPtr.Zero, IntPtr.Zero);
            }
            return false;
        }

        public static short smethod_0(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[2];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 2, out lpNumberOfBytesRead);
            return BitConverter.ToInt16(lpBuffer, 0);
        }

        public static ushort smethod_1(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[2];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 2, out lpNumberOfBytesRead);
            return BitConverter.ToUInt16(lpBuffer, 0);
        }

        public static int smethod_2(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[4];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 4, out lpNumberOfBytesRead);
            return BitConverter.ToInt32(lpBuffer, 0);
        }

        public static uint smethod_3(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[4];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 4, out lpNumberOfBytesRead);
            return BitConverter.ToUInt32(lpBuffer, 0);
        }

        public static long smethod_4(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[8];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 8, out lpNumberOfBytesRead);
            return BitConverter.ToInt64(lpBuffer, 0);
        }

        public static ulong smethod_5(IntPtr processHandle, long lpBaseAddress)
        {
            int lpNumberOfBytesRead = 0;
            byte[] lpBuffer = new byte[8];
            ReadProcessMemory(processHandle, new IntPtr(lpBaseAddress), lpBuffer, 8, out lpNumberOfBytesRead);
            return BitConverter.ToUInt64(lpBuffer, 0);
        }

        [DllImport("KERNEL32.DLL", SetLastError=true)]
        public static extern bool WriteProcessMemory(IntPtr handle, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        public static bool IsRunAsAdministrator
        {
            get
            {
                return IsUserAnAdmin();
            }
        }

        public static bool IsUACEnabled
        {
            get
            {
                return Microsoft.Win32.Registry.LocalMachine.OpenSubKey(Class3.smethod_10(0x18e), false).GetValue(Class3.smethod_10(0x204)).Equals(1);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public Process32.LUID Luid;
            public uint Attributes;
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull = 2,
            TokenElevationTypeLimited = 3
        }

        public enum TOKEN_INFORMATION_CLASS
        {
            MaxTokenInfoClass = 0x1d,
            TokenAccessInformation = 0x16,
            TokenAuditPolicy = 0x10,
            TokenDefaultDacl = 6,
            TokenElevation = 20,
            TokenElevationType = 0x12,
            TokenGroups = 2,
            TokenGroupsAndPrivileges = 13,
            TokenHasRestrictions = 0x15,
            TokenImpersonationLevel = 9,
            TokenIntegrityLevel = 0x19,
            TokenLinkedToken = 0x13,
            TokenLogonSid = 0x1c,
            TokenMandatoryPolicy = 0x1b,
            TokenOrigin = 0x11,
            TokenOwner = 4,
            TokenPrimaryGroup = 5,
            TokenPrivileges = 3,
            TokenRestrictedSids = 11,
            TokenSandBoxInert = 15,
            TokenSessionId = 12,
            TokenSessionReference = 14,
            TokenSource = 7,
            TokenStatistics = 10,
            TokenType = 8,
            TokenUIAccess = 0x1a,
            TokenUser = 1,
            TokenVirtualizationAllowed = 0x17,
            TokenVirtualizationEnabled = 0x18
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            public Process32.LUID Luid;
            public uint Attributes;
        }
    }
}

