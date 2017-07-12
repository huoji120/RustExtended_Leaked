using System;
using System.Runtime.InteropServices;

namespace RustProtect
{
	public struct ModuleEntry32
	{
		private const int int_0 = 256;

		public uint dwSize;

		public uint th32ModuleID;

		public uint th32ProcessID;

		public System.IntPtr GlblcntUsage;

		public uint ProccntUsage;

		public System.IntPtr modBaseAddr;

		public uint modBaseSize;

		public System.IntPtr hModule;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szModule;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szExePath;
	}
}
