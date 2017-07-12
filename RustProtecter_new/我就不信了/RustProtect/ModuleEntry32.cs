namespace RustProtect
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ModuleEntry32
    {
        public uint dwSize;
        public uint th32ModuleID;
        public uint th32ProcessID;
        public IntPtr GlblcntUsage;
        public uint ProccntUsage;
        public IntPtr modBaseAddr;
        public uint modBaseSize;
        public IntPtr hModule;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x100)]
        public byte[] szModule;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=260)]
        public byte[] szExePath;
    }
}

