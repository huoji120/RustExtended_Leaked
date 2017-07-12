namespace RustProtect
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ByteCollectionHelper
    {
        public static string GetString(this byte[] bytes, int offset)
        {
            string str = null;
            int num = -1;
            num = bytes.IndexOf(0, offset);
            if (num >= 0)
            {
                str = Encoding.ASCII.GetString(bytes, offset, num - offset);
            }
            return str;
        }

        public static int IndexOf(this byte[] bytes, byte value, [Optional, DefaultParameterValue(0)] int startIndex)
        {
            while (startIndex < bytes.Length)
            {
                if (bytes[startIndex++] == value)
                {
                    return (startIndex - 1);
                }
            }
            return -1;
        }

        public static T ToStruct<T>(this byte[] bytes, [Optional, DefaultParameterValue(0)] int start) where T: struct
        {
            T structure = Activator.CreateInstance<T>();
            int cb = Marshal.SizeOf(structure);
            IntPtr destination = Marshal.AllocHGlobal(cb);
            Marshal.Copy(bytes, start, destination, cb);
            structure = (T) Marshal.PtrToStructure(destination, structure.GetType());
            Marshal.FreeHGlobal(destination);
            return structure;
        }
    }
}

