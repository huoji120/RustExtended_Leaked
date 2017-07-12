using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RustProtect
{
	public static class ByteCollectionHelper
	{
		public static int IndexOf(this byte[] bytes, byte value, int startIndex = 0)
		{
			while (startIndex < bytes.Length)
			{
				if (bytes[startIndex++] == value)
				{
					return startIndex - 1;
				}
			}
			return -1;
		}

		public static string GetString(this byte[] bytes, int offset)
		{
			string result = null;
			int num = bytes.IndexOf(0, offset);
			if (num >= 0)
			{
				result = Encoding.ASCII.GetString(bytes, offset, num - offset);
			}
			return result;
		}

        public static T ToStruct<T>(this byte[] bytes, [Optional, DefaultParameterValue(0)] int start) where T : struct
        {
            T structure = Activator.CreateInstance<T>();
            int cb = Marshal.SizeOf(structure);
            IntPtr destination = Marshal.AllocHGlobal(cb);
            Marshal.Copy(bytes, start, destination, cb);
            structure = (T)Marshal.PtrToStructure(destination, structure.GetType());
            Marshal.FreeHGlobal(destination);
            return structure;
        }
    }
}
