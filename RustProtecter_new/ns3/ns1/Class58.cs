namespace ns1
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Class58
    {
        public static string string_0;

        private static bool smethod_0(Assembly assembly_0, Assembly assembly_1)
        {
            byte[] publicKey = assembly_0.GetName().GetPublicKey();
            byte[] buffer2 = assembly_1.GetName().GetPublicKey();
            if ((buffer2 == null) != (publicKey == null))
            {
                return false;
            }
            if (buffer2 != null)
            {
                for (int i = 0; i < buffer2.Length; i++)
                {
                    if (buffer2[i] != publicKey[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static ICryptoTransform smethod_1(byte[] byte_0, byte[] byte_1, bool bool_0)
        {
            using (SymmetricAlgorithm algorithm = new RijndaelManaged())
            {
                return (bool_0 ? algorithm.CreateDecryptor(byte_0, byte_1) : algorithm.CreateEncryptor(byte_0, byte_1));
            }
        }

        private static ICryptoTransform smethod_2(byte[] byte_0, byte[] byte_1, bool bool_0)
        {
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                return (bool_0 ? provider.CreateDecryptor(byte_0, byte_1) : provider.CreateEncryptor(byte_0, byte_1));
            }
        }

        public static byte[] smethod_3(byte[] byte_0)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            if ((callingAssembly != executingAssembly) && !smethod_0(executingAssembly, callingAssembly))
            {
                return null;
            }
            Stream0 stream = new Stream0(byte_0);
            byte[] buffer = new byte[0];
            int num = stream.method_3();
            if (num == 0x4034b50)
            {
                short num2 = (short) stream.method_2();
                int num3 = stream.method_2();
                int num4 = stream.method_2();
                if ((((num != 0x4034b50) || (num2 != 20)) || (num3 != 0)) || (num4 != 8))
                {
                    throw new FormatException("Wrong Header Signature");
                }
                stream.method_3();
                stream.method_3();
                stream.method_3();
                int num5 = stream.method_3();
                int count = stream.method_2();
                int num7 = stream.method_2();
                if (count > 0)
                {
                    byte[] buffer2 = new byte[count];
                    stream.Read(buffer2, 0, count);
                }
                if (num7 > 0)
                {
                    byte[] buffer3 = new byte[num7];
                    stream.Read(buffer3, 0, num7);
                }
                byte[] buffer4 = new byte[stream.Length - stream.Position];
                stream.Read(buffer4, 0, buffer4.Length);
                Class59 class2 = new Class59(buffer4);
                buffer = new byte[num5];
                class2.method_2(buffer, 0, buffer.Length);
                buffer4 = null;
            }
            else
            {
                int num8 = num >> 0x18;
                num -= num8 << 0x18;
                if (num == 0x7d7a7b)
                {
                    switch (num8)
                    {
                        case 1:
                        {
                            int num12;
                            int num9 = stream.method_3();
                            buffer = new byte[num9];
                            for (int i = 0; i < num9; i += num12)
                            {
                                int num11 = stream.method_3();
                                num12 = stream.method_3();
                                byte[] buffer5 = new byte[num11];
                                stream.Read(buffer5, 0, buffer5.Length);
                                new Class59(buffer5).method_2(buffer, i, num12);
                            }
                            break;
                        }
                        case 2:
                        {
                            byte[] buffer6 = new byte[] { 9, 0x4e, 5, 0x95, 0x23, 0x3b, 50, 0x2b };
                            byte[] buffer7 = new byte[] { 0xb2, 0x57, 0x4a, 0x60, 0xa4, 0xff, 0x2e, 0x18 };
                            using (ICryptoTransform transform = smethod_2(buffer6, buffer7, true))
                            {
                                buffer = smethod_3(transform.TransformFinalBlock(byte_0, 4, byte_0.Length - 4));
                            }
                            break;
                        }
                    }
                    if (num8 != 3)
                    {
                        goto Label_0279;
                    }
                    byte[] buffer9 = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                    byte[] buffer10 = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
                    using (ICryptoTransform transform2 = smethod_1(buffer9, buffer10, true))
                    {
                        buffer = smethod_3(transform2.TransformFinalBlock(byte_0, 4, byte_0.Length - 4));
                        goto Label_0279;
                    }
                }
                throw new FormatException("Unknown Header");
            }
        Label_0279:
            stream.Close();
            stream = null;
            return buffer;
        }

        public static byte[] smethod_4(byte[] byte_0)
        {
            return smethod_7(byte_0, 1, null, null);
        }

        public static byte[] smethod_5(byte[] byte_0, byte[] byte_1, byte[] byte_2)
        {
            return smethod_7(byte_0, 2, byte_1, byte_2);
        }

        public static byte[] smethod_6(byte[] byte_0, byte[] byte_1, byte[] byte_2)
        {
            return smethod_7(byte_0, 3, byte_1, byte_2);
        }

        private static byte[] smethod_7(byte[] byte_0, int int_0, byte[] byte_1, byte[] byte_2)
        {
            byte[] buffer11;
            try
            {
                Stream0 stream = new Stream0();
                if (int_0 == 0)
                {
                    Class64 class2 = new Class64();
                    DateTime now = DateTime.Now;
                    long num = (long) ((ulong) ((((((((now.Year - 0x7bc) & 0x7f) << 0x19) | (now.Month << 0x15)) | (now.Day << 0x10)) | (now.Hour << 11)) | (now.Minute << 5)) | (now.Second >> 1)));
                    uint[] numArray = new uint[] { 
                        0, 0x77073096, 0xee0e612c, 0x990951ba, 0x76dc419, 0x706af48f, 0xe963a535, 0x9e6495a3, 0xedb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988, 0x9b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91, 
                        0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7, 0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5, 
                        0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b, 0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59, 
                        0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924, 0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d, 
                        0x76dc4190, 0x1db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x6b6b51f, 0x9fbfe4a5, 0xe8b8d433, 0x7807c9a2, 0xf00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x86d3d2d, 0x91646c97, 0xe6635c01, 
                        0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65, 
                        0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9, 
                        0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f, 0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad, 
                        0xedb88320, 0x9abfb3b6, 0x3b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x4db2615, 0x73dc1683, 0xe3630b12, 0x94643b84, 0xd6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0xa00ae27, 0x7d079eb1, 
                        0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5, 
                        0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b, 0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79, 
                        0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236, 0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d, 
                        0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x26d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x5005713, 0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0xcb61b38, 0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0xbdbdf21, 
                        0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777, 0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45, 
                        0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9, 
                        0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
                     };
                    uint maxValue = uint.MaxValue;
                    uint num3 = uint.MaxValue;
                    int num4 = 0;
                    int length = byte_0.Length;
                    while (--length >= 0)
                    {
                        num3 = numArray[(int) ((IntPtr) ((num3 ^ byte_0[num4++]) & 0xff))] ^ (num3 >> 8);
                    }
                    num3 ^= maxValue;
                    stream.method_1(0x4034b50);
                    stream.method_0(20);
                    stream.method_0(0);
                    stream.method_0(8);
                    stream.method_1((int) num);
                    stream.method_1((int) num3);
                    long position = stream.Position;
                    stream.method_1(0);
                    stream.method_1(byte_0.Length);
                    byte[] bytes = Encoding.UTF8.GetBytes("{data}");
                    stream.method_0(bytes.Length);
                    stream.method_0(0);
                    stream.Write(bytes, 0, bytes.Length);
                    class2.method_1(byte_0);
                    while (!class2.IsNeedingInput)
                    {
                        byte[] buffer2 = new byte[0x200];
                        int count = class2.method_2(buffer2);
                        if (count <= 0)
                        {
                            break;
                        }
                        stream.Write(buffer2, 0, count);
                    }
                    class2.method_0();
                    while (!class2.IsFinished)
                    {
                        byte[] buffer3 = new byte[0x200];
                        int num8 = class2.method_2(buffer3);
                        if (num8 <= 0)
                        {
                            break;
                        }
                        stream.Write(buffer3, 0, num8);
                    }
                    long totalOut = class2.TotalOut;
                    stream.method_1(0x2014b50);
                    stream.method_0(20);
                    stream.method_0(20);
                    stream.method_0(0);
                    stream.method_0(8);
                    stream.method_1((int) num);
                    stream.method_1((int) num3);
                    stream.method_1((int) totalOut);
                    stream.method_1(byte_0.Length);
                    stream.method_0(bytes.Length);
                    stream.method_0(0);
                    stream.method_0(0);
                    stream.method_0(0);
                    stream.method_0(0);
                    stream.method_1(0);
                    stream.method_1(0);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.method_1(0x6054b50);
                    stream.method_0(0);
                    stream.method_0(0);
                    stream.method_0(1);
                    stream.method_0(1);
                    stream.method_1(0x2e + bytes.Length);
                    stream.method_1((30 + bytes.Length) + ((int) totalOut));
                    stream.method_0(0);
                    stream.Seek(position, SeekOrigin.Begin);
                    stream.method_1((int) totalOut);
                }
                else if (int_0 == 1)
                {
                    byte[] buffer4;
                    stream.method_1(0x17d7a7b);
                    stream.method_1(byte_0.Length);
                    for (int i = 0; i < byte_0.Length; i += buffer4.Length)
                    {
                        buffer4 = new byte[Math.Min(0x1fffff, byte_0.Length - i)];
                        Buffer.BlockCopy(byte_0, i, buffer4, 0, buffer4.Length);
                        long num11 = stream.Position;
                        stream.method_1(0);
                        stream.method_1(buffer4.Length);
                        Class64 class3 = new Class64();
                        class3.method_1(buffer4);
                        while (!class3.IsNeedingInput)
                        {
                            byte[] buffer5 = new byte[0x200];
                            int num12 = class3.method_2(buffer5);
                            if (num12 <= 0)
                            {
                                break;
                            }
                            stream.Write(buffer5, 0, num12);
                        }
                        class3.method_0();
                        while (!class3.IsFinished)
                        {
                            byte[] buffer6 = new byte[0x200];
                            int num13 = class3.method_2(buffer6);
                            if (num13 <= 0)
                            {
                                break;
                            }
                            stream.Write(buffer6, 0, num13);
                        }
                        long num14 = stream.Position;
                        stream.Position = num11;
                        stream.method_1((int) class3.TotalOut);
                        stream.Position = num14;
                    }
                }
                else
                {
                    if (int_0 == 2)
                    {
                        stream.method_1(0x27d7a7b);
                        byte[] inputBuffer = smethod_7(byte_0, 1, null, null);
                        using (ICryptoTransform transform = smethod_2(byte_1, byte_2, false))
                        {
                            byte[] buffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                            stream.Write(buffer, 0, buffer.Length);
                            goto Label_044E;
                        }
                    }
                    if (int_0 == 3)
                    {
                        stream.method_1(0x37d7a7b);
                        byte[] buffer9 = smethod_7(byte_0, 1, null, null);
                        using (ICryptoTransform transform2 = smethod_1(byte_1, byte_2, false))
                        {
                            byte[] buffer10 = transform2.TransformFinalBlock(buffer9, 0, buffer9.Length);
                            stream.Write(buffer10, 0, buffer10.Length);
                        }
                    }
                }
            Label_044E:
                stream.Flush();
                stream.Close();
                buffer11 = stream.ToArray();
            }
            catch (Exception exception)
            {
                string_0 = "ERR 2003: " + exception.Message;
                throw;
            }
            return buffer11;
        }

        internal class Class59
        {
            private bool bool_0;
            private Class58.Class60 class60_0 = new Class58.Class60();
            private Class58.Class61 class61_0 = new Class58.Class61();
            private Class58.Class62 class62_0;
            private Class58.Class62 class62_1;
            private Class58.Class63 class63_0;
            private const int int_0 = 0;
            private const int int_1 = 1;
            private const int int_10 = 10;
            private const int int_11 = 11;
            private const int int_12 = 12;
            private static readonly int[] int_13 = new int[] { 
                3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 0x11, 0x13, 0x17, 0x1b, 0x1f, 
                0x23, 0x2b, 0x33, 0x3b, 0x43, 0x53, 0x63, 0x73, 0x83, 0xa3, 0xc3, 0xe3, 0x102
             };
            private static readonly int[] int_14 = new int[] { 
                0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 
                3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0
             };
            private static readonly int[] int_15 = new int[] { 
                1, 2, 3, 4, 5, 7, 9, 13, 0x11, 0x19, 0x21, 0x31, 0x41, 0x61, 0x81, 0xc1, 
                0x101, 0x181, 0x201, 0x301, 0x401, 0x601, 0x801, 0xc01, 0x1001, 0x1801, 0x2001, 0x3001, 0x4001, 0x6001
             };
            private static readonly int[] int_16 = new int[] { 
                0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 
                7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13
             };
            private int int_17 = 2;
            private int int_18;
            private int int_19;
            private const int int_2 = 2;
            private int int_20;
            private int int_21;
            private const int int_3 = 3;
            private const int int_4 = 4;
            private const int int_5 = 5;
            private const int int_6 = 6;
            private const int int_7 = 7;
            private const int int_8 = 8;
            private const int int_9 = 9;

            public Class59(byte[] bytes)
            {
                this.class60_0.method_5(bytes, 0, bytes.Length);
            }

            private bool method_0()
            {
                int num = this.class61_0.method_4();
                while (num >= 0x102)
                {
                    int num2;
                    switch (this.int_17)
                    {
                        case 7:
                            goto Label_0052;

                        case 8:
                            goto Label_009E;

                        case 9:
                            goto Label_00EE;

                        case 10:
                            goto Label_0121;

                        default:
                        {
                            continue;
                        }
                    }
                Label_0037:
                    this.class61_0.Write(num2);
                    if (--num < 0x102)
                    {
                        return true;
                    }
                Label_0052:
                    if (((num2 = this.class62_0.method_1(this.class60_0)) & -256) == 0)
                    {
                        goto Label_0037;
                    }
                    if (num2 < 0x101)
                    {
                        if (num2 < 0)
                        {
                            return false;
                        }
                        this.class62_1 = null;
                        this.class62_0 = null;
                        this.int_17 = 2;
                        return true;
                    }
                    this.int_19 = int_13[num2 - 0x101];
                    this.int_18 = int_14[num2 - 0x101];
                Label_009E:
                    if (this.int_18 > 0)
                    {
                        this.int_17 = 8;
                        int num3 = this.class60_0.method_0(this.int_18);
                        if (num3 < 0)
                        {
                            return false;
                        }
                        this.class60_0.method_1(this.int_18);
                        this.int_19 += num3;
                    }
                    this.int_17 = 9;
                Label_00EE:
                    num2 = this.class62_1.method_1(this.class60_0);
                    if (num2 < 0)
                    {
                        return false;
                    }
                    this.int_20 = int_15[num2];
                    this.int_18 = int_16[num2];
                Label_0121:
                    if (this.int_18 > 0)
                    {
                        this.int_17 = 10;
                        int num4 = this.class60_0.method_0(this.int_18);
                        if (num4 < 0)
                        {
                            return false;
                        }
                        this.class60_0.method_1(this.int_18);
                        this.int_20 += num4;
                    }
                    this.class61_0.method_1(this.int_19, this.int_20);
                    num -= this.int_19;
                    this.int_17 = 7;
                }
                return true;
            }

            private bool method_1()
            {
                int num3;
                switch (this.int_17)
                {
                    case 2:
                        if (!this.bool_0)
                        {
                            int num = this.class60_0.method_0(3);
                            if (num < 0)
                            {
                                return false;
                            }
                            this.class60_0.method_1(3);
                            if ((num & 1) != 0)
                            {
                                this.bool_0 = true;
                            }
                            switch ((num >> 1))
                            {
                                case 0:
                                    this.class60_0.method_2();
                                    this.int_17 = 3;
                                    goto Label_00DC;

                                case 1:
                                    this.class62_0 = Class58.Class62.class62_0;
                                    this.class62_1 = Class58.Class62.class62_1;
                                    this.int_17 = 7;
                                    goto Label_00DC;

                                case 2:
                                    this.class63_0 = new Class58.Class63();
                                    this.int_17 = 6;
                                    goto Label_00DC;
                            }
                            break;
                        }
                        this.int_17 = 12;
                        return false;

                    case 3:
                        this.int_21 = this.class60_0.method_0(0x10);
                        if (this.int_21 >= 0)
                        {
                            this.class60_0.method_1(0x10);
                            this.int_17 = 4;
                            goto Label_010F;
                        }
                        return false;

                    case 4:
                        goto Label_010F;

                    case 5:
                        goto Label_0137;

                    case 6:
                        if (this.class63_0.method_0(this.class60_0))
                        {
                            this.class62_0 = this.class63_0.method_1();
                            this.class62_1 = this.class63_0.method_2();
                            this.int_17 = 7;
                            goto Label_01BB;
                        }
                        return false;

                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        goto Label_01BB;

                    case 12:
                        return false;

                    default:
                        return false;
                }
            Label_00DC:
                return true;
            Label_010F:
                if (this.class60_0.method_0(0x10) < 0)
                {
                    return false;
                }
                this.class60_0.method_1(0x10);
                this.int_17 = 5;
            Label_0137:
                num3 = this.class61_0.method_2(this.class60_0, this.int_21);
                this.int_21 -= num3;
                if (this.int_21 == 0)
                {
                    this.int_17 = 2;
                    return true;
                }
                return !this.class60_0.IsNeedingInput;
            Label_01BB:
                return this.method_0();
            }

            public int method_2(byte[] byte_0, int int_22, int int_23)
            {
                int num = 0;
                goto Label_0048;
            Label_0004:
                if (!this.method_1() && ((this.class61_0.method_5() <= 0) || (this.int_17 == 11)))
                {
                    return num;
                }
            Label_0048:
                if (this.int_17 == 11)
                {
                    goto Label_0004;
                }
                int num2 = this.class61_0.method_6(byte_0, int_22, int_23);
                int_22 += num2;
                num += num2;
                int_23 -= num2;
                if (int_23 != 0)
                {
                    goto Label_0004;
                }
                return num;
            }
        }

        internal class Class60
        {
            private byte[] byte_0;
            private int int_0;
            private int int_1;
            private int int_2;
            private uint uint_0;

            public int method_0(int int_3)
            {
                if (this.int_2 < int_3)
                {
                    if (this.int_0 == this.int_1)
                    {
                        return -1;
                    }
                    this.uint_0 |= (uint) (((this.byte_0[this.int_0++] & 0xff) | ((this.byte_0[this.int_0++] & 0xff) << 8)) << this.int_2);
                    this.int_2 += 0x10;
                }
                return (((int) this.uint_0) & ((((int) 1) << int_3) - 1));
            }

            public void method_1(int int_3)
            {
                this.uint_0 = this.uint_0 >> int_3;
                this.int_2 -= int_3;
            }

            public void method_2()
            {
                this.uint_0 = this.uint_0 >> (this.int_2 & 7);
                this.int_2 &= -8;
            }

            public int method_3(byte[] byte_1, int int_3, int int_4)
            {
                int num = 0;
                while (this.int_2 > 0)
                {
                    if (int_4 <= 0)
                    {
                        break;
                    }
                    byte_1[int_3++] = (byte) this.uint_0;
                    this.uint_0 = this.uint_0 >> 8;
                    this.int_2 -= 8;
                    int_4--;
                    num++;
                }
                if (int_4 == 0)
                {
                    return num;
                }
                int num2 = this.int_1 - this.int_0;
                if (int_4 > num2)
                {
                    int_4 = num2;
                }
                Array.Copy(this.byte_0, this.int_0, byte_1, int_3, int_4);
                this.int_0 += int_4;
                if (((this.int_0 - this.int_1) & 1) != 0)
                {
                    this.uint_0 = (uint) (this.byte_0[this.int_0++] & 0xff);
                    this.int_2 = 8;
                }
                return (num + int_4);
            }

            public void method_4()
            {
                this.int_2 = 0;
                this.int_1 = 0;
                this.int_0 = 0;
                this.uint_0 = 0;
            }

            public void method_5(byte[] byte_1, int int_3, int int_4)
            {
                if (this.int_0 < this.int_1)
                {
                    throw new InvalidOperationException();
                }
                int num = int_3 + int_4;
                if (((0 > int_3) || (int_3 > num)) || (num > byte_1.Length))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if ((int_4 & 1) != 0)
                {
                    this.uint_0 |= (uint) ((byte_1[int_3++] & 0xff) << this.int_2);
                    this.int_2 += 8;
                }
                this.byte_0 = byte_1;
                this.int_0 = int_3;
                this.int_1 = num;
            }

            public int AvailableBits
            {
                get
                {
                    return this.int_2;
                }
            }

            public int AvailableBytes
            {
                get
                {
                    return ((this.int_1 - this.int_0) + (this.int_2 >> 3));
                }
            }

            public bool IsNeedingInput
            {
                get
                {
                    return (this.int_0 == this.int_1);
                }
            }
        }

        internal class Class61
        {
            private byte[] byte_0 = new byte[0x8000];
            private const int int_0 = 0x8000;
            private const int int_1 = 0x7fff;
            private int int_2;
            private int int_3;

            private void method_0(int int_4, int int_5, int int_6)
            {
                while (int_5-- > 0)
                {
                    this.byte_0[this.int_2++] = this.byte_0[int_4++];
                    this.int_2 &= 0x7fff;
                    int_4 &= 0x7fff;
                }
            }

            public void method_1(int int_4, int int_5)
            {
                this.int_3 += int_4;
                if (this.int_3 > 0x8000)
                {
                    throw new InvalidOperationException();
                }
                int num = (this.int_2 - int_5) & 0x7fff;
                int num2 = 0x8000 - int_4;
                if ((num > num2) || (this.int_2 >= num2))
                {
                    this.method_0(num, int_4, int_5);
                }
                else if (int_4 > int_5)
                {
                    while (int_4-- > 0)
                    {
                        this.byte_0[this.int_2++] = this.byte_0[num++];
                    }
                }
                else
                {
                    Array.Copy(this.byte_0, num, this.byte_0, this.int_2, int_4);
                    this.int_2 += int_4;
                }
            }

            public int method_2(Class58.Class60 class60_0, int int_4)
            {
                int num;
                int_4 = Math.Min(Math.Min(int_4, 0x8000 - this.int_3), class60_0.AvailableBytes);
                int num2 = 0x8000 - this.int_2;
                if (int_4 > num2)
                {
                    num = class60_0.method_3(this.byte_0, this.int_2, num2);
                    if (num == num2)
                    {
                        num += class60_0.method_3(this.byte_0, 0, int_4 - num2);
                    }
                }
                else
                {
                    num = class60_0.method_3(this.byte_0, this.int_2, int_4);
                }
                this.int_2 = (this.int_2 + num) & 0x7fff;
                this.int_3 += num;
                return num;
            }

            public void method_3(byte[] byte_1, int int_4, int int_5)
            {
                if (this.int_3 > 0)
                {
                    throw new InvalidOperationException();
                }
                if (int_5 > 0x8000)
                {
                    int_4 += int_5 - 0x8000;
                    int_5 = 0x8000;
                }
                Array.Copy(byte_1, int_4, this.byte_0, 0, int_5);
                this.int_2 = int_5 & 0x7fff;
            }

            public int method_4()
            {
                return (0x8000 - this.int_3);
            }

            public int method_5()
            {
                return this.int_3;
            }

            public int method_6(byte[] byte_1, int int_4, int int_5)
            {
                int num = this.int_2;
                if (int_5 > this.int_3)
                {
                    int_5 = this.int_3;
                }
                else
                {
                    num = ((this.int_2 - this.int_3) + int_5) & 0x7fff;
                }
                int num2 = int_5;
                int length = int_5 - num;
                if (length > 0)
                {
                    Array.Copy(this.byte_0, 0x8000 - length, byte_1, int_4, length);
                    int_4 += length;
                    int_5 = num;
                }
                Array.Copy(this.byte_0, num - int_5, byte_1, int_4, int_5);
                this.int_3 -= num2;
                if (this.int_3 < 0)
                {
                    throw new InvalidOperationException();
                }
                return num2;
            }

            public void method_7()
            {
                this.int_2 = 0;
                this.int_3 = 0;
            }

            public void Write(int int_4)
            {
                if (this.int_3++ == 0x8000)
                {
                    throw new InvalidOperationException();
                }
                this.byte_0[this.int_2++] = (byte) int_4;
                this.int_2 &= 0x7fff;
            }
        }

        internal class Class62
        {
            public static readonly Class58.Class62 class62_0;
            public static readonly Class58.Class62 class62_1;
            private const int int_0 = 15;
            private short[] short_0;

            static Class62()
            {
                byte[] codeLengths = new byte[0x120];
                int num = 0;
                while (num < 0x90)
                {
                    codeLengths[num++] = 8;
                }
                while (num < 0x100)
                {
                    codeLengths[num++] = 9;
                }
                while (num < 280)
                {
                    codeLengths[num++] = 7;
                }
                while (num < 0x120)
                {
                    codeLengths[num++] = 8;
                }
                class62_0 = new Class58.Class62(codeLengths);
                codeLengths = new byte[0x20];
                num = 0;
                while (num < 0x20)
                {
                    codeLengths[num++] = 5;
                }
                class62_1 = new Class58.Class62(codeLengths);
            }

            public Class62(byte[] codeLengths)
            {
                this.method_0(codeLengths);
            }

            private void method_0(byte[] byte_0)
            {
                int[] numArray = new int[0x10];
                int[] numArray2 = new int[0x10];
                for (int i = 0; i < byte_0.Length; i++)
                {
                    int index = byte_0[i];
                    if (index > 0)
                    {
                        numArray[index]++;
                    }
                }
                int num3 = 0;
                int num4 = 0x200;
                for (int j = 1; j <= 15; j++)
                {
                    numArray2[j] = num3;
                    num3 += numArray[j] << (0x10 - j);
                    if (j >= 10)
                    {
                        int num6 = numArray2[j] & 0x1ff80;
                        int num7 = num3 & 0x1ff80;
                        num4 += (num7 - num6) >> (0x10 - j);
                    }
                }
                this.short_0 = new short[num4];
                int num8 = 0x200;
                for (int k = 15; k >= 10; k--)
                {
                    int num10 = num3 & 0x1ff80;
                    num3 -= numArray[k] << (0x10 - k);
                    int num11 = num3 & 0x1ff80;
                    for (int n = num11; n < num10; n += 0x80)
                    {
                        this.short_0[Class58.Class65.smethod_0(n)] = (short) ((-num8 << 4) | k);
                        num8 += ((int) 1) << (k - 9);
                    }
                }
                for (int m = 0; m < byte_0.Length; m++)
                {
                    int num14 = byte_0[m];
                    if (num14 != 0)
                    {
                        num3 = numArray2[num14];
                        int num15 = Class58.Class65.smethod_0(num3);
                        if (num14 <= 9)
                        {
                            do
                            {
                                this.short_0[num15] = (short) ((m << 4) | num14);
                                num15 += ((int) 1) << num14;
                            }
                            while (num15 < 0x200);
                        }
                        else
                        {
                            int num16 = this.short_0[num15 & 0x1ff];
                            int num17 = ((int) 1) << (num16 & 15);
                            num16 = -(num16 >> 4);
                            do
                            {
                                this.short_0[num16 | (num15 >> 9)] = (short) ((m << 4) | num14);
                                num15 += ((int) 1) << num14;
                            }
                            while (num15 < num17);
                        }
                        numArray2[num14] = num3 + (((int) 1) << (0x10 - num14));
                    }
                }
            }

            public int method_1(Class58.Class60 class60_0)
            {
                int num2;
                int index = class60_0.method_0(9);
                if (index >= 0)
                {
                    num2 = this.short_0[index];
                    if (num2 >= 0)
                    {
                        class60_0.method_1(num2 & 15);
                        return (num2 >> 4);
                    }
                    int num3 = -(num2 >> 4);
                    int num4 = num2 & 15;
                    index = class60_0.method_0(num4);
                    if (index >= 0)
                    {
                        num2 = this.short_0[num3 | (index >> 9)];
                        class60_0.method_1(num2 & 15);
                        return (num2 >> 4);
                    }
                    int num5 = class60_0.AvailableBits;
                    index = class60_0.method_0(num5);
                    num2 = this.short_0[num3 | (index >> 9)];
                    if ((num2 & 15) <= num5)
                    {
                        class60_0.method_1(num2 & 15);
                        return (num2 >> 4);
                    }
                    return -1;
                }
                int availableBits = class60_0.AvailableBits;
                index = class60_0.method_0(availableBits);
                num2 = this.short_0[index];
                if ((num2 >= 0) && ((num2 & 15) <= availableBits))
                {
                    class60_0.method_1(num2 & 15);
                    return (num2 >> 4);
                }
                return -1;
            }
        }

        internal class Class63
        {
            private byte[] byte_0;
            private byte[] byte_1;
            private byte byte_2;
            private Class58.Class62 class62_0;
            private const int int_0 = 0;
            private const int int_1 = 1;
            private int int_10;
            private int int_11;
            private int int_12;
            private int int_13;
            private int int_14;
            private static readonly int[] int_15 = new int[] { 
                0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 
                14, 1, 15
             };
            private const int int_2 = 2;
            private const int int_3 = 3;
            private const int int_4 = 4;
            private const int int_5 = 5;
            private static readonly int[] int_6 = new int[] { 3, 3, 11 };
            private static readonly int[] int_7 = new int[] { 2, 3, 7 };
            private int int_8;
            private int int_9;

            public bool method_0(Class58.Class60 class60_0)
            {
                int num2;
                int num3;
            Label_0000:
                switch (this.int_8)
                {
                    case 0:
                        this.int_9 = class60_0.method_0(5);
                        if (this.int_9 < 0)
                        {
                            return false;
                        }
                        this.int_9 += 0x101;
                        class60_0.method_1(5);
                        this.int_8 = 1;
                        goto Label_01DD;

                    case 1:
                        goto Label_01DD;

                    case 2:
                        goto Label_018F;

                    case 3:
                        goto Label_0156;

                    case 4:
                        goto Label_00E1;

                    case 5:
                        break;

                    default:
                        goto Label_0000;
                }
            Label_002C:
                num3 = int_7[this.int_13];
                int num4 = class60_0.method_0(num3);
                if (num4 < 0)
                {
                    return false;
                }
                class60_0.method_1(num3);
                num4 += int_6[this.int_13];
                while (num4-- > 0)
                {
                    this.byte_1[this.int_14++] = this.byte_2;
                }
                if (this.int_14 == this.int_12)
                {
                    return true;
                }
                this.int_8 = 4;
                goto Label_0000;
            Label_00E1:
                while (((num2 = this.class62_0.method_1(class60_0)) & -16) == 0)
                {
                    this.byte_1[this.int_14++] = this.byte_2 = (byte) num2;
                    if (this.int_14 == this.int_12)
                    {
                        return true;
                    }
                }
                if (num2 < 0)
                {
                    return false;
                }
                if (num2 >= 0x11)
                {
                    this.byte_2 = 0;
                }
                this.int_13 = num2 - 0x10;
                this.int_8 = 5;
                goto Label_002C;
            Label_0156:
                while (this.int_14 < this.int_11)
                {
                    int num = class60_0.method_0(3);
                    if (num < 0)
                    {
                        return false;
                    }
                    class60_0.method_1(3);
                    this.byte_0[int_15[this.int_14]] = (byte) num;
                    this.int_14++;
                }
                this.class62_0 = new Class58.Class62(this.byte_0);
                this.byte_0 = null;
                this.int_14 = 0;
                this.int_8 = 4;
                goto Label_00E1;
            Label_018F:
                this.int_11 = class60_0.method_0(4);
                if (this.int_11 < 0)
                {
                    return false;
                }
                this.int_11 += 4;
                class60_0.method_1(4);
                this.byte_0 = new byte[0x13];
                this.int_14 = 0;
                this.int_8 = 3;
                goto Label_0156;
            Label_01DD:
                this.int_10 = class60_0.method_0(5);
                if (this.int_10 < 0)
                {
                    return false;
                }
                this.int_10++;
                class60_0.method_1(5);
                this.int_12 = this.int_9 + this.int_10;
                this.byte_1 = new byte[this.int_12];
                this.int_8 = 2;
                goto Label_018F;
            }

            public Class58.Class62 method_1()
            {
                byte[] destinationArray = new byte[this.int_9];
                Array.Copy(this.byte_1, 0, destinationArray, 0, this.int_9);
                return new Class58.Class62(destinationArray);
            }

            public Class58.Class62 method_2()
            {
                byte[] destinationArray = new byte[this.int_10];
                Array.Copy(this.byte_1, this.int_9, destinationArray, 0, this.int_10);
                return new Class58.Class62(destinationArray);
            }
        }

        internal class Class64
        {
            private Class58.Class67 class67_0;
            private Class58.Class68 class68_0 = new Class58.Class68();
            private const int int_0 = 4;
            private const int int_1 = 8;
            private const int int_2 = 0x10;
            private const int int_3 = 20;
            private const int int_4 = 0x1c;
            private const int int_5 = 30;
            private int int_6 = 0x10;
            private long long_0;

            public Class64()
            {
                this.class67_0 = new Class58.Class67(this.class68_0);
            }

            public void method_0()
            {
                this.int_6 |= 12;
            }

            public void method_1(byte[] byte_0)
            {
                this.class67_0.method_7(byte_0);
            }

            public int method_2(byte[] byte_0)
            {
                int num4;
                int num = 0;
                int length = byte_0.Length;
                int num3 = length;
            Label_00B1:
                num4 = this.class68_0.Flush(byte_0, num, length);
                num += num4;
                this.long_0 += num4;
                length -= num4;
                if ((length != 0) && (this.int_6 != 30))
                {
                    if (!this.class67_0.method_6((this.int_6 & 4) != 0, (this.int_6 & 8) != 0))
                    {
                        if (this.int_6 == 0x10)
                        {
                            return (num3 - length);
                        }
                        if (this.int_6 == 20)
                        {
                            for (int i = 8 + (-this.class68_0.BitCount & 7); i > 0; i -= 10)
                            {
                                this.class68_0.method_3(2, 10);
                            }
                            this.int_6 = 0x10;
                        }
                        else if (this.int_6 == 0x1c)
                        {
                            this.class68_0.method_2();
                            this.int_6 = 30;
                        }
                    }
                    goto Label_00B1;
                }
                return (num3 - length);
            }

            public bool IsFinished
            {
                get
                {
                    return ((this.int_6 == 30) && this.class68_0.IsFlushed);
                }
            }

            public bool IsNeedingInput
            {
                get
                {
                    return this.class67_0.method_8();
                }
            }

            public long TotalOut
            {
                get
                {
                    return this.long_0;
                }
            }
        }

        internal class Class65
        {
            private static readonly byte[] byte_0 = new byte[] { 0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15 };
            private byte[] byte_1;
            private static readonly byte[] byte_2 = new byte[0x11e];
            private static readonly byte[] byte_3;
            private Class66 class66_0;
            private Class66 class66_1;
            private Class66 class66_2;
            private Class58.Class68 class68_0;
            private const int int_0 = 0x4000;
            private const int int_1 = 0x11e;
            private int int_10;
            private const int int_2 = 30;
            private const int int_3 = 0x13;
            private const int int_4 = 0x10;
            private const int int_5 = 0x11;
            private const int int_6 = 0x12;
            private const int int_7 = 0x100;
            private static readonly int[] int_8 = new int[] { 
                0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 
                14, 1, 15
             };
            private int int_9;
            private short[] short_0;
            private static readonly short[] short_1 = new short[0x11e];
            private static readonly short[] short_2;

            static Class65()
            {
                int index = 0;
                while (index < 0x90)
                {
                    short_1[index] = smethod_0((0x30 + index) << 8);
                    byte_2[index++] = 8;
                }
                while (index < 0x100)
                {
                    short_1[index] = smethod_0((0x100 + index) << 7);
                    byte_2[index++] = 9;
                }
                while (index < 280)
                {
                    short_1[index] = smethod_0((-256 + index) << 9);
                    byte_2[index++] = 7;
                }
                while (index < 0x11e)
                {
                    short_1[index] = smethod_0((-88 + index) << 8);
                    byte_2[index++] = 8;
                }
                short_2 = new short[30];
                byte_3 = new byte[30];
                for (index = 0; index < 30; index++)
                {
                    short_2[index] = smethod_0(index << 11);
                    byte_3[index] = 5;
                }
            }

            public Class65(Class58.Class68 pending)
            {
                this.class68_0 = pending;
                this.class66_0 = new Class66(this, 0x11e, 0x101, 15);
                this.class66_1 = new Class66(this, 30, 1, 15);
                this.class66_2 = new Class66(this, 0x13, 4, 7);
                this.short_0 = new short[0x4000];
                this.byte_1 = new byte[0x4000];
            }

            public void Init()
            {
                this.int_9 = 0;
                this.int_10 = 0;
            }

            private int method_0(int int_11)
            {
                if (int_11 == 0xff)
                {
                    return 0x11d;
                }
                int num = 0x101;
                while (int_11 >= 8)
                {
                    num += 4;
                    int_11 = int_11 >> 1;
                }
                return (num + int_11);
            }

            private int method_1(int int_11)
            {
                int num = 0;
                while (int_11 >= 4)
                {
                    num += 2;
                    int_11 = int_11 >> 1;
                }
                return (num + int_11);
            }

            public void method_2(int int_11)
            {
                this.class66_2.method_2();
                this.class66_0.method_2();
                this.class66_1.method_2();
                this.class68_0.method_3(this.class66_0.int_1 - 0x101, 5);
                this.class68_0.method_3(this.class66_1.int_1 - 1, 5);
                this.class68_0.method_3(int_11 - 4, 4);
                for (int i = 0; i < int_11; i++)
                {
                    this.class68_0.method_3(this.class66_2.byte_0[int_8[i]], 3);
                }
                this.class66_0.method_7(this.class66_2);
                this.class66_1.method_7(this.class66_2);
            }

            public void method_3()
            {
                for (int i = 0; i < this.int_9; i++)
                {
                    int num2 = this.byte_1[i] & 0xff;
                    int num3 = this.short_0[i];
                    if (num3-- != 0)
                    {
                        int num4 = this.method_0(num2);
                        this.class66_0.method_0(num4);
                        int num5 = (num4 - 0x105) / 4;
                        if ((num5 > 0) && (num5 <= 5))
                        {
                            this.class68_0.method_3(num2 & ((((int) 1) << num5) - 1), num5);
                        }
                        int num6 = this.method_1(num3);
                        this.class66_1.method_0(num6);
                        num5 = (num6 / 2) - 1;
                        if (num5 > 0)
                        {
                            this.class68_0.method_3(num3 & ((((int) 1) << num5) - 1), num5);
                        }
                    }
                    else
                    {
                        this.class66_0.method_0(num2);
                    }
                }
                this.class66_0.method_0(0x100);
            }

            public void method_4(byte[] byte_4, int int_11, int int_12, bool bool_0)
            {
                this.class68_0.method_3(bool_0 ? 1 : 0, 3);
                this.class68_0.method_2();
                this.class68_0.method_0(int_12);
                this.class68_0.method_0(~int_12);
                this.class68_0.method_1(byte_4, int_11, int_12);
                this.Init();
            }

            public void method_5(byte[] byte_4, int int_11, int int_12, bool bool_0)
            {
                this.class66_0.short_0[0x100] = (short) (this.class66_0.short_0[0x100] + 1);
                this.class66_0.method_4();
                this.class66_1.method_4();
                this.class66_0.method_6(this.class66_2);
                this.class66_1.method_6(this.class66_2);
                this.class66_2.method_4();
                int num = 4;
                for (int i = 0x12; i > num; i--)
                {
                    if (this.class66_2.byte_0[int_8[i]] > 0)
                    {
                        num = i + 1;
                    }
                }
                int num3 = ((((14 + (num * 3)) + this.class66_2.method_5()) + this.class66_0.method_5()) + this.class66_1.method_5()) + this.int_10;
                int num4 = this.int_10;
                for (int j = 0; j < 0x11e; j++)
                {
                    num4 += this.class66_0.short_0[j] * byte_2[j];
                }
                for (int k = 0; k < 30; k++)
                {
                    num4 += this.class66_1.short_0[k] * byte_3[k];
                }
                if (num3 >= num4)
                {
                    num3 = num4;
                }
                if ((int_11 >= 0) && ((int_12 + 4) < (num3 >> 3)))
                {
                    this.method_4(byte_4, int_11, int_12, bool_0);
                }
                else if (num3 == num4)
                {
                    this.class68_0.method_3(2 + (bool_0 ? 1 : 0), 3);
                    this.class66_0.method_1(short_1, byte_2);
                    this.class66_1.method_1(short_2, byte_3);
                    this.method_3();
                    this.Init();
                }
                else
                {
                    this.class68_0.method_3(4 + (bool_0 ? 1 : 0), 3);
                    this.method_2(num);
                    this.method_3();
                    this.Init();
                }
            }

            public bool method_6()
            {
                return (this.int_9 >= 0x4000);
            }

            public bool method_7(int int_11)
            {
                this.short_0[this.int_9] = 0;
                this.byte_1[this.int_9++] = (byte) int_11;
                this.class66_0.short_0[int_11] = (short) (this.class66_0.short_0[int_11] + 1);
                return this.method_6();
            }

            public bool method_8(int int_11, int int_12)
            {
                this.short_0[this.int_9] = (short) int_11;
                this.byte_1[this.int_9++] = (byte) (int_12 - 3);
                int index = this.method_0(int_12 - 3);
                this.class66_0.short_0[index] = (short) (this.class66_0.short_0[index] + 1);
                if ((index >= 0x109) && (index < 0x11d))
                {
                    this.int_10 += (index - 0x105) / 4;
                }
                int num2 = this.method_1(int_11 - 1);
                this.class66_1.short_0[num2] = (short) (this.class66_1.short_0[num2] + 1);
                if (num2 >= 4)
                {
                    this.int_10 += (num2 / 2) - 1;
                }
                return this.method_6();
            }

            public static short smethod_0(int int_11)
            {
                return (short) ((((byte_0[int_11 & 15] << 12) | (byte_0[(int_11 >> 4) & 15] << 8)) | (byte_0[(int_11 >> 8) & 15] << 4)) | byte_0[int_11 >> 12]);
            }

            public class Class66
            {
                public byte[] byte_0;
                private Class58.Class65 class65_0;
                public int int_0;
                public int int_1;
                private int[] int_2;
                private int int_3;
                public short[] short_0;
                private short[] short_1;

                public Class66(Class58.Class65 dh, int elems, int minCodes, int maxLength)
                {
                    this.class65_0 = dh;
                    this.int_0 = minCodes;
                    this.int_3 = maxLength;
                    this.short_0 = new short[elems];
                    this.int_2 = new int[maxLength];
                }

                public void method_0(int int_4)
                {
                    this.class65_0.class68_0.method_3(this.short_1[int_4] & 0xffff, this.byte_0[int_4]);
                }

                public void method_1(short[] short_2, byte[] byte_1)
                {
                    this.short_1 = short_2;
                    this.byte_0 = byte_1;
                }

                public void method_2()
                {
                    int[] numArray = new int[this.int_3];
                    int num = 0;
                    this.short_1 = new short[this.short_0.Length];
                    for (int i = 0; i < this.int_3; i++)
                    {
                        numArray[i] = num;
                        num += this.int_2[i] << (15 - i);
                    }
                    for (int j = 0; j < this.int_1; j++)
                    {
                        int num4 = this.byte_0[j];
                        if (num4 > 0)
                        {
                            this.short_1[j] = Class58.Class65.smethod_0(numArray[num4 - 1]);
                            numArray[num4 - 1] += ((int) 1) << (0x10 - num4);
                        }
                    }
                }

                private void method_3(int[] int_4)
                {
                    this.byte_0 = new byte[this.short_0.Length];
                    int num = int_4.Length / 2;
                    int num2 = (num + 1) / 2;
                    int num3 = 0;
                    for (int i = 0; i < this.int_3; i++)
                    {
                        this.int_2[i] = 0;
                    }
                    int[] numArray = new int[num];
                    numArray[num - 1] = 0;
                    for (int j = num - 1; j >= 0; j--)
                    {
                        if (int_4[(2 * j) + 1] != -1)
                        {
                            int num6 = numArray[j] + 1;
                            if (num6 > this.int_3)
                            {
                                num6 = this.int_3;
                                num3++;
                            }
                            numArray[int_4[2 * j]] = numArray[int_4[(2 * j) + 1]] = num6;
                        }
                        else
                        {
                            int num7 = numArray[j];
                            this.int_2[num7 - 1]++;
                            this.byte_0[int_4[2 * j]] = (byte) numArray[j];
                        }
                    }
                    if (num3 != 0)
                    {
                        int index = this.int_3 - 1;
                        while (true)
                        {
                            while (this.int_2[--index] == 0)
                            {
                            }
                            do
                            {
                                this.int_2[index]--;
                                this.int_2[++index]++;
                                num3 -= ((int) 1) << ((this.int_3 - 1) - index);
                            }
                            while ((num3 > 0) && (index < (this.int_3 - 1)));
                            if (num3 <= 0)
                            {
                                this.int_2[this.int_3 - 1] += num3;
                                this.int_2[this.int_3 - 2] -= num3;
                                int num9 = 2 * num2;
                                for (int k = this.int_3; k != 0; k--)
                                {
                                    int num11 = this.int_2[k - 1];
                                    while (num11 > 0)
                                    {
                                        int num12 = 2 * int_4[num9++];
                                        if (int_4[num12 + 1] == -1)
                                        {
                                            this.byte_0[int_4[num12]] = (byte) k;
                                            num11--;
                                        }
                                    }
                                }
                                return;
                            }
                        }
                    }
                }

                public void method_4()
                {
                    int length = this.short_0.Length;
                    int[] numArray = new int[length];
                    int num2 = 0;
                    int num3 = 0;
                    for (int i = 0; i < length; i++)
                    {
                        int num5 = this.short_0[i];
                        if (num5 != 0)
                        {
                            int index = num2++;
                            while (index > 0)
                            {
                                int num7;
                                if (this.short_0[numArray[num7 = (index - 1) / 2]] <= num5)
                                {
                                    break;
                                }
                                numArray[index] = numArray[num7];
                                index = num7;
                            }
                            numArray[index] = i;
                            num3 = i;
                        }
                    }
                    while (num2 < 2)
                    {
                        int num8 = (num3 < 2) ? ++num3 : 0;
                        numArray[num2++] = num8;
                    }
                    this.int_1 = Math.Max(num3 + 1, this.int_0);
                    int num9 = num2;
                    int[] numArray2 = new int[(4 * num2) - 2];
                    int[] numArray3 = new int[(2 * num2) - 1];
                    int num10 = num9;
                    for (int j = 0; j < num2; j++)
                    {
                        int num12 = numArray[j];
                        numArray2[2 * j] = num12;
                        numArray2[(2 * j) + 1] = -1;
                        numArray3[j] = this.short_0[num12] << 8;
                        numArray[j] = j;
                    }
                    while (true)
                    {
                        int num13 = numArray[0];
                        int num14 = numArray[--num2];
                        int num15 = 0;
                        int num16 = 1;
                        while (num16 < num2)
                        {
                            if (((num16 + 1) < num2) && (numArray3[numArray[num16]] > numArray3[numArray[num16 + 1]]))
                            {
                                num16++;
                            }
                            numArray[num15] = numArray[num16];
                            num15 = num16;
                            num16 = (num16 * 2) + 1;
                        }
                        int num17 = numArray3[num14];
                        while ((num16 = num15) > 0)
                        {
                            if (numArray3[numArray[num15 = (num16 - 1) / 2]] <= num17)
                            {
                                break;
                            }
                            numArray[num16] = numArray[num15];
                        }
                        numArray[num16] = num14;
                        int num18 = numArray[0];
                        num14 = num10++;
                        numArray2[2 * num14] = num13;
                        numArray2[(2 * num14) + 1] = num18;
                        int num19 = Math.Min((int) (numArray3[num13] & 0xff), (int) (numArray3[num18] & 0xff));
                        numArray3[num14] = num17 = ((numArray3[num13] + numArray3[num18]) - num19) + 1;
                        num15 = 0;
                        num16 = 1;
                        while (num16 < num2)
                        {
                            if (((num16 + 1) < num2) && (numArray3[numArray[num16]] > numArray3[numArray[num16 + 1]]))
                            {
                                num16++;
                            }
                            numArray[num15] = numArray[num16];
                            num15 = num16;
                            num16 = (num15 * 2) + 1;
                        }
                        while ((num16 = num15) > 0)
                        {
                            if (numArray3[numArray[num15 = (num16 - 1) / 2]] <= num17)
                            {
                                break;
                            }
                            numArray[num16] = numArray[num15];
                        }
                        numArray[num16] = num14;
                        if (num2 <= 1)
                        {
                            this.method_3(numArray2);
                            return;
                        }
                    }
                }

                public int method_5()
                {
                    int num = 0;
                    for (int i = 0; i < this.short_0.Length; i++)
                    {
                        num += this.short_0[i] * this.byte_0[i];
                    }
                    return num;
                }

                public void method_6(Class58.Class65.Class66 class66_0)
                {
                    int index = -1;
                    int num5 = 0;
                    while (num5 < this.int_1)
                    {
                        int num;
                        int num2;
                        int num3 = 1;
                        int num6 = this.byte_0[num5];
                        if (num6 == 0)
                        {
                            num = 0x8a;
                            num2 = 3;
                        }
                        else
                        {
                            num = 6;
                            num2 = 3;
                            if (index != num6)
                            {
                                class66_0.short_0[num6] = (short) (class66_0.short_0[num6] + 1);
                                num3 = 0;
                            }
                        }
                        index = num6;
                        num5++;
                        while (num5 < this.int_1)
                        {
                            if (index != this.byte_0[num5])
                            {
                                break;
                            }
                            num5++;
                            if (++num3 >= num)
                            {
                                break;
                            }
                        }
                        if (num3 < num2)
                        {
                            class66_0.short_0[index] = (short) (class66_0.short_0[index] + ((short) num3));
                        }
                        else
                        {
                            if (index != 0)
                            {
                                class66_0.short_0[0x10] = (short) (class66_0.short_0[0x10] + 1);
                                continue;
                            }
                            if (num3 <= 10)
                            {
                                class66_0.short_0[0x11] = (short) (class66_0.short_0[0x11] + 1);
                                continue;
                            }
                            class66_0.short_0[0x12] = (short) (class66_0.short_0[0x12] + 1);
                        }
                    }
                }

                public void method_7(Class58.Class65.Class66 class66_0)
                {
                    int num4 = -1;
                    int index = 0;
                    while (index < this.int_1)
                    {
                        int num;
                        int num2;
                        int num3 = 1;
                        int num6 = this.byte_0[index];
                        if (num6 == 0)
                        {
                            num = 0x8a;
                            num2 = 3;
                        }
                        else
                        {
                            num = 6;
                            num2 = 3;
                            if (num4 != num6)
                            {
                                class66_0.method_0(num6);
                                num3 = 0;
                            }
                        }
                        num4 = num6;
                        index++;
                        while (index < this.int_1)
                        {
                            if (num4 != this.byte_0[index])
                            {
                                break;
                            }
                            index++;
                            if (++num3 >= num)
                            {
                                break;
                            }
                        }
                        if (num3 < num2)
                        {
                            while (num3-- > 0)
                            {
                                class66_0.method_0(num4);
                            }
                        }
                        else if (num4 != 0)
                        {
                            class66_0.method_0(0x10);
                            this.class65_0.class68_0.method_3(num3 - 3, 2);
                        }
                        else
                        {
                            if (num3 <= 10)
                            {
                                class66_0.method_0(0x11);
                                this.class65_0.class68_0.method_3(num3 - 3, 3);
                                continue;
                            }
                            class66_0.method_0(0x12);
                            this.class65_0.class68_0.method_3(num3 - 11, 7);
                        }
                    }
                }
            }
        }

        internal class Class67
        {
            private bool bool_0;
            private byte[] byte_0;
            private byte[] byte_1;
            private Class58.Class65 class65_0;
            private Class58.Class68 class68_0;
            private const int int_0 = 0x102;
            private const int int_1 = 3;
            private int int_10;
            private int int_11;
            private int int_12;
            private int int_13;
            private int int_14;
            private int int_15;
            private int int_16;
            private int int_17;
            private int int_18;
            private const int int_2 = 0x8000;
            private const int int_3 = 0x7fff;
            private const int int_4 = 0x8000;
            private const int int_5 = 0x7fff;
            private const int int_6 = 5;
            private const int int_7 = 0x106;
            private const int int_8 = 0x7efa;
            private const int int_9 = 0x1000;
            private short[] short_0;
            private short[] short_1;

            public Class67(Class58.Class68 pending)
            {
                this.class68_0 = pending;
                this.class65_0 = new Class58.Class65(pending);
                this.byte_0 = new byte[0x10000];
                this.short_0 = new short[0x8000];
                this.short_1 = new short[0x8000];
                this.int_14 = 1;
                this.int_13 = 1;
            }

            private void method_0()
            {
                this.int_10 = (this.byte_0[this.int_14] << 5) ^ this.byte_0[this.int_14 + 1];
            }

            private int method_1()
            {
                short num;
                int index = ((this.int_10 << 5) ^ this.byte_0[this.int_14 + 2]) & 0x7fff;
                this.short_1[this.int_14 & 0x7fff] = num = this.short_0[index];
                this.short_0[index] = (short) this.int_14;
                this.int_10 = index;
                return (num & 0xffff);
            }

            private void method_2()
            {
                Array.Copy(this.byte_0, 0x8000, this.byte_0, 0, 0x8000);
                this.int_11 -= 0x8000;
                this.int_14 -= 0x8000;
                this.int_13 -= 0x8000;
                for (int i = 0; i < 0x8000; i++)
                {
                    int num2 = this.short_0[i] & 0xffff;
                    this.short_0[i] = (num2 >= 0x8000) ? ((short) (num2 - 0x8000)) : ((short) 0);
                }
                for (int j = 0; j < 0x8000; j++)
                {
                    int num4 = this.short_1[j] & 0xffff;
                    this.short_1[j] = (num4 >= 0x8000) ? ((short) (num4 - 0x8000)) : ((short) 0);
                }
            }

            public void method_3()
            {
                if (this.int_14 >= 0xfefa)
                {
                    this.method_2();
                }
                while (this.int_15 < 0x106)
                {
                    if (this.int_17 >= this.int_18)
                    {
                        break;
                    }
                    int length = (0x10000 - this.int_15) - this.int_14;
                    if (length > (this.int_18 - this.int_17))
                    {
                        length = this.int_18 - this.int_17;
                    }
                    Array.Copy(this.byte_1, this.int_17, this.byte_0, this.int_14 + this.int_15, length);
                    this.int_17 += length;
                    this.int_16 += length;
                    this.int_15 += length;
                }
                if (this.int_15 >= 3)
                {
                    this.method_0();
                }
            }

            private bool method_4(int int_19)
            {
                int num = 0x80;
                int num2 = 0x80;
                short[] numArray = this.short_1;
                int index = this.int_14;
                int num5 = this.int_14 + this.int_12;
                int num6 = Math.Max(this.int_12, 2);
                int num7 = Math.Max(this.int_14 - 0x7efa, 0);
                int num8 = (this.int_14 + 0x102) - 1;
                byte num9 = this.byte_0[num5 - 1];
                byte num10 = this.byte_0[num5];
                if (num6 >= 8)
                {
                    num = num >> 2;
                }
                if (num2 > this.int_15)
                {
                    num2 = this.int_15;
                }
                do
                {
                    if ((this.byte_0[int_19 + num6] == num10) && (((this.byte_0[(int_19 + num6) - 1] == num9) && (this.byte_0[int_19] == this.byte_0[index])) && (this.byte_0[int_19 + 1] == this.byte_0[index + 1])))
                    {
                        int num4 = int_19 + 2;
                        index += 2;
                        while (this.byte_0[++index] == this.byte_0[++num4])
                        {
                            if ((((this.byte_0[++index] != this.byte_0[++num4]) || (this.byte_0[++index] != this.byte_0[++num4])) || ((this.byte_0[++index] != this.byte_0[++num4]) || (this.byte_0[++index] != this.byte_0[++num4]))) || (((this.byte_0[++index] != this.byte_0[++num4]) || (this.byte_0[++index] != this.byte_0[++num4])) || ((this.byte_0[++index] != this.byte_0[++num4]) || (index >= num8))))
                            {
                                break;
                            }
                        }
                        if (index > num5)
                        {
                            this.int_11 = int_19;
                            num5 = index;
                            num6 = index - this.int_14;
                            if (num6 >= num2)
                            {
                                break;
                            }
                            num9 = this.byte_0[num5 - 1];
                            num10 = this.byte_0[num5];
                        }
                        index = this.int_14;
                    }
                }
                while (((int_19 = numArray[int_19 & 0x7fff] & 0xffff) > num7) && (--num != 0));
                this.int_12 = Math.Min(num6, this.int_15);
                return (this.int_12 >= 3);
            }

            private bool method_5(bool bool_1, bool bool_2)
            {
                if ((this.int_15 >= 0x106) || bool_1)
                {
                    goto Label_0195;
                }
                return false;
            Label_017E:
                if (this.class65_0.method_6())
                {
                    int num4 = this.int_14 - this.int_13;
                    if (this.bool_0)
                    {
                        num4--;
                    }
                    bool flag = (bool_2 && (this.int_15 == 0)) && !this.bool_0;
                    this.class65_0.method_5(this.byte_0, this.int_13, num4, flag);
                    this.int_13 += num4;
                    return !flag;
                }
            Label_0195:
                if ((this.int_15 >= 0x106) || bool_1)
                {
                    if (this.int_15 == 0)
                    {
                        if (this.bool_0)
                        {
                            this.class65_0.method_7(this.byte_0[this.int_14 - 1] & 0xff);
                        }
                        this.bool_0 = false;
                        this.class65_0.method_5(this.byte_0, this.int_13, this.int_14 - this.int_13, bool_2);
                        this.int_13 = this.int_14;
                        return false;
                    }
                    if (this.int_14 >= 0xfefa)
                    {
                        this.method_2();
                    }
                    int num = this.int_11;
                    int num2 = this.int_12;
                    if (this.int_15 >= 3)
                    {
                        int num3 = this.method_1();
                        if ((((num3 != 0) && ((this.int_14 - num3) <= 0x7efa)) && (this.method_4(num3) && (this.int_12 <= 5))) && ((this.int_12 == 3) && ((this.int_14 - this.int_11) > 0x1000)))
                        {
                            this.int_12 = 2;
                        }
                    }
                    if ((num2 < 3) || (this.int_12 > num2))
                    {
                        if (this.bool_0)
                        {
                            this.class65_0.method_7(this.byte_0[this.int_14 - 1] & 0xff);
                        }
                        this.bool_0 = true;
                        this.int_14++;
                        this.int_15--;
                        goto Label_017E;
                    }
                    this.class65_0.method_8((this.int_14 - 1) - num, num2);
                    num2 -= 2;
                Label_00DF:
                    this.int_14++;
                    this.int_15--;
                    if (this.int_15 >= 3)
                    {
                        this.method_1();
                    }
                    if (--num2 <= 0)
                    {
                        this.int_14++;
                        this.int_15--;
                        this.bool_0 = false;
                        this.int_12 = 2;
                    }
                    else
                    {
                        goto Label_00DF;
                    }
                    goto Label_017E;
                }
                return true;
            }

            public bool method_6(bool bool_1, bool bool_2)
            {
                // This item is obfuscated and can not be translated.
                bool flag;
                do
                {
                    this.method_3();
                    if (!bool_1)
                    {
                    }
                    bool flag2 = false;
                    flag = this.method_5(flag2, bool_2);
                }
                while (this.class68_0.IsFlushed && flag);
                return flag;
            }

            public void method_7(byte[] byte_2)
            {
                this.byte_1 = byte_2;
                this.int_17 = 0;
                this.int_18 = byte_2.Length;
            }

            public bool method_8()
            {
                return (this.int_18 == this.int_17);
            }
        }

        internal class Class68
        {
            protected byte[] byte_0 = new byte[0x10000];
            private int int_0;
            private int int_1;
            private int int_2;
            private uint uint_0;

            public int Flush(byte[] byte_1, int int_3, int int_4)
            {
                if (this.int_2 >= 8)
                {
                    this.byte_0[this.int_1++] = (byte) this.uint_0;
                    this.uint_0 = this.uint_0 >> 8;
                    this.int_2 -= 8;
                }
                if (int_4 > (this.int_1 - this.int_0))
                {
                    int_4 = this.int_1 - this.int_0;
                    Array.Copy(this.byte_0, this.int_0, byte_1, int_3, int_4);
                    this.int_0 = 0;
                    this.int_1 = 0;
                    return int_4;
                }
                Array.Copy(this.byte_0, this.int_0, byte_1, int_3, int_4);
                this.int_0 += int_4;
                return int_4;
            }

            public void method_0(int int_3)
            {
                this.byte_0[this.int_1++] = (byte) int_3;
                this.byte_0[this.int_1++] = (byte) (int_3 >> 8);
            }

            public void method_1(byte[] byte_1, int int_3, int int_4)
            {
                Array.Copy(byte_1, int_3, this.byte_0, this.int_1, int_4);
                this.int_1 += int_4;
            }

            public void method_2()
            {
                if (this.int_2 > 0)
                {
                    this.byte_0[this.int_1++] = (byte) this.uint_0;
                    if (this.int_2 > 8)
                    {
                        this.byte_0[this.int_1++] = (byte) (this.uint_0 >> 8);
                    }
                }
                this.uint_0 = 0;
                this.int_2 = 0;
            }

            public void method_3(int int_3, int int_4)
            {
                this.uint_0 |= (uint) (int_3 << this.int_2);
                this.int_2 += int_4;
                if (this.int_2 >= 0x10)
                {
                    this.byte_0[this.int_1++] = (byte) this.uint_0;
                    this.byte_0[this.int_1++] = (byte) (this.uint_0 >> 8);
                    this.uint_0 = this.uint_0 >> 0x10;
                    this.int_2 -= 0x10;
                }
            }

            public int BitCount
            {
                get
                {
                    return this.int_2;
                }
            }

            public bool IsFlushed
            {
                get
                {
                    return (this.int_1 == 0);
                }
            }
        }

        internal class Stream0 : MemoryStream
        {
            public Stream0()
            {
            }

            public Stream0(byte[] buffer) : base(buffer, false)
            {
            }

            public void method_0(int int_0)
            {
                this.WriteByte((byte) (int_0 & 0xff));
                this.WriteByte((byte) ((int_0 >> 8) & 0xff));
            }

            public void method_1(int int_0)
            {
                this.method_0(int_0);
                this.method_0(int_0 >> 0x10);
            }

            public int method_2()
            {
                return (this.ReadByte() | (this.ReadByte() << 8));
            }

            public int method_3()
            {
                return (this.method_2() | (this.method_2() << 0x10));
            }
        }
    }
}

