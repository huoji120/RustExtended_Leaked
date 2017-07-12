namespace RustProtect
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct VerifyFile
    {
        public string Filename;
        public long Filesize;
    }
}

