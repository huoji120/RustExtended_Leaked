using System;
using System.Runtime.InteropServices;

namespace RustProtect
{
	public struct ProcessEntry32
	{
		private const int int_0 = 260;

		public uint dwSize;

		public uint cntUsage;

		public uint th32ProcessID;

		public System.IntPtr th32DefaultHeapID;

		public uint th32ModuleID;

		public uint cntThreads;

		public uint th32ParentProcessID;

		public int pcPriClassBase;

		public uint dwFlags;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szExeFile;
	}
}
