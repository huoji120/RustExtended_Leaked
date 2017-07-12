namespace RustProtect
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryAssemblyEntry
    {
        public long Pointer;
        public uint Filesize;
        public string Filepath;
        public string TargetRuntime;
    }
}

