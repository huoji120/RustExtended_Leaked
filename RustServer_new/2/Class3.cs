using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

internal class Class3
{
    private static Assembly assembly_0 = typeof(Class3).Assembly;
    private static bool bool_0 = false;
    private static bool bool_1 = false;
    private static bool bool_2 = false;
    private static byte[] byte_0 = new byte[0];
    private static byte[] byte_1 = new byte[0];
    private static byte[] byte_2 = new byte[0];
    private static byte[] byte_3 = new byte[0];
    private static int[] int_0 = new int[0];
    private static int int_1 = 0;
    private static int int_2 = 1;
    private static IntPtr intptr_0 = IntPtr.Zero;
    private static IntPtr intptr_1 = IntPtr.Zero;
    private static long long_0 = 0L;
    private static SortedList sortedList_0 = new SortedList();
    private static string[] string_0 = new string[0];
    private static uint[] uint_0 = new uint[] { 
        0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee, 0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501, 0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be, 0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821, 
        0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa, 0xd62f105d, 0x2441453, 0xd8a1e681, 0xe7d3fbc8, 0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed, 0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a, 
        0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c, 0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70, 0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x4881d05, 0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665, 
        0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039, 0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1, 0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1, 0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
     };

    private void aWdHwaGPYEwVj()
    {
    }

    private byte[] method_0()
    {
        return null;
    }

    private byte[] method_1()
    {
        return null;
    }

    private byte[] method_2()
    {
        string str = "{11111-22222-20001-00001}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    private byte[] method_3()
    {
        string str = "{11111-22222-20001-00002}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    private byte[] method_4()
    {
        string str = "{11111-22222-30001-00001}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    private byte[] method_5()
    {
        string str = "{11111-22222-30001-00002}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    internal byte[] method_6()
    {
        string str = "{11111-22222-40001-00001}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    internal byte[] method_7()
    {
        string str = "{11111-22222-40001-00002}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    internal byte[] method_8()
    {
        string str = "{11111-22222-50001-00001}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    internal byte[] method_9()
    {
        string str = "{11111-22222-50001-00002}";
        if (str.Length > 0)
        {
            return new byte[] { 1, 2 };
        }
        return new byte[] { 1, 2 };
    }

    internal static byte[] smethod_0(object object_0)
    {
        uint[] numArray = new uint[0x10];
        int num5 = 0x1c0 - ((object_0.Length * 8) % 0x200);
        uint num2 = (uint) ((num5 + 0x200) % 0x200);
        if (num2 == 0)
        {
            num2 = 0x200;
        }
        uint num4 = (uint) ((object_0.Length + (num2 / 8)) + ((ulong) 8L));
        ulong num3 = (ulong) (object_0.Length * 8L);
        byte[] buffer = new byte[num4];
        for (int i = 0; i < object_0.Length; i++)
        {
            buffer[i] = (byte) object_0[i];
        }
        buffer[object_0.Length] = (byte) (buffer[object_0.Length] | 0x80);
        for (int j = 8; j > 0; j--)
        {
            buffer[(int) ((IntPtr) (num4 - j))] = (byte) ((num3 >> ((8 - j) * 8)) & ((ulong) 0xffL));
        }
        uint num = (uint) ((buffer.Length * 8) / 0x20);
        uint num8 = 0x67452301;
        uint num9 = 0xefcdab89;
        uint num10 = 0x98badcfe;
        uint num11 = 0x10325476;
        for (uint k = 0; k < (num / 0x10); k++)
        {
            uint num13 = k << 6;
            for (uint m = 0; m < 0x3d; m += 4)
            {
                numArray[m >> 2] = (uint) ((((buffer[(int) ((IntPtr) (num13 + (m + 3)))] << 0x18) | (buffer[(int) ((IntPtr) (num13 + (m + 2)))] << 0x10)) | (buffer[(int) ((IntPtr) (num13 + (m + 1)))] << 8)) | buffer[num13 + m]);
            }
            uint num15 = num8;
            uint num16 = num9;
            uint num17 = num10;
            uint num18 = num11;
            smethod_1(ref num8, num9, num10, num11, 0, 7, 1, numArray);
            smethod_1(ref num11, num8, num9, num10, 1, 12, 2, numArray);
            smethod_1(ref num10, num11, num8, num9, 2, 0x11, 3, numArray);
            smethod_1(ref num9, num10, num11, num8, 3, 0x16, 4, numArray);
            smethod_1(ref num8, num9, num10, num11, 4, 7, 5, numArray);
            smethod_1(ref num11, num8, num9, num10, 5, 12, 6, numArray);
            smethod_1(ref num10, num11, num8, num9, 6, 0x11, 7, numArray);
            smethod_1(ref num9, num10, num11, num8, 7, 0x16, 8, numArray);
            smethod_1(ref num8, num9, num10, num11, 8, 7, 9, numArray);
            smethod_1(ref num11, num8, num9, num10, 9, 12, 10, numArray);
            smethod_1(ref num10, num11, num8, num9, 10, 0x11, 11, numArray);
            smethod_1(ref num9, num10, num11, num8, 11, 0x16, 12, numArray);
            smethod_1(ref num8, num9, num10, num11, 12, 7, 13, numArray);
            smethod_1(ref num11, num8, num9, num10, 13, 12, 14, numArray);
            smethod_1(ref num10, num11, num8, num9, 14, 0x11, 15, numArray);
            smethod_1(ref num9, num10, num11, num8, 15, 0x16, 0x10, numArray);
            smethod_2(ref num8, num9, num10, num11, 1, 5, 0x11, numArray);
            smethod_2(ref num11, num8, num9, num10, 6, 9, 0x12, numArray);
            smethod_2(ref num10, num11, num8, num9, 11, 14, 0x13, numArray);
            smethod_2(ref num9, num10, num11, num8, 0, 20, 20, numArray);
            smethod_2(ref num8, num9, num10, num11, 5, 5, 0x15, numArray);
            smethod_2(ref num11, num8, num9, num10, 10, 9, 0x16, numArray);
            smethod_2(ref num10, num11, num8, num9, 15, 14, 0x17, numArray);
            smethod_2(ref num9, num10, num11, num8, 4, 20, 0x18, numArray);
            smethod_2(ref num8, num9, num10, num11, 9, 5, 0x19, numArray);
            smethod_2(ref num11, num8, num9, num10, 14, 9, 0x1a, numArray);
            smethod_2(ref num10, num11, num8, num9, 3, 14, 0x1b, numArray);
            smethod_2(ref num9, num10, num11, num8, 8, 20, 0x1c, numArray);
            smethod_2(ref num8, num9, num10, num11, 13, 5, 0x1d, numArray);
            smethod_2(ref num11, num8, num9, num10, 2, 9, 30, numArray);
            smethod_2(ref num10, num11, num8, num9, 7, 14, 0x1f, numArray);
            smethod_2(ref num9, num10, num11, num8, 12, 20, 0x20, numArray);
            smethod_3(ref num8, num9, num10, num11, 5, 4, 0x21, numArray);
            smethod_3(ref num11, num8, num9, num10, 8, 11, 0x22, numArray);
            smethod_3(ref num10, num11, num8, num9, 11, 0x10, 0x23, numArray);
            smethod_3(ref num9, num10, num11, num8, 14, 0x17, 0x24, numArray);
            smethod_3(ref num8, num9, num10, num11, 1, 4, 0x25, numArray);
            smethod_3(ref num11, num8, num9, num10, 4, 11, 0x26, numArray);
            smethod_3(ref num10, num11, num8, num9, 7, 0x10, 0x27, numArray);
            smethod_3(ref num9, num10, num11, num8, 10, 0x17, 40, numArray);
            smethod_3(ref num8, num9, num10, num11, 13, 4, 0x29, numArray);
            smethod_3(ref num11, num8, num9, num10, 0, 11, 0x2a, numArray);
            smethod_3(ref num10, num11, num8, num9, 3, 0x10, 0x2b, numArray);
            smethod_3(ref num9, num10, num11, num8, 6, 0x17, 0x2c, numArray);
            smethod_3(ref num8, num9, num10, num11, 9, 4, 0x2d, numArray);
            smethod_3(ref num11, num8, num9, num10, 12, 11, 0x2e, numArray);
            smethod_3(ref num10, num11, num8, num9, 15, 0x10, 0x2f, numArray);
            smethod_3(ref num9, num10, num11, num8, 2, 0x17, 0x30, numArray);
            ywnZmcpthG(ref num8, num9, num10, num11, 0, 6, 0x31, numArray);
            ywnZmcpthG(ref num11, num8, num9, num10, 7, 10, 50, numArray);
            ywnZmcpthG(ref num10, num11, num8, num9, 14, 15, 0x33, numArray);
            ywnZmcpthG(ref num9, num10, num11, num8, 5, 0x15, 0x34, numArray);
            ywnZmcpthG(ref num8, num9, num10, num11, 12, 6, 0x35, numArray);
            ywnZmcpthG(ref num11, num8, num9, num10, 3, 10, 0x36, numArray);
            ywnZmcpthG(ref num10, num11, num8, num9, 10, 15, 0x37, numArray);
            ywnZmcpthG(ref num9, num10, num11, num8, 1, 0x15, 0x38, numArray);
            ywnZmcpthG(ref num8, num9, num10, num11, 8, 6, 0x39, numArray);
            ywnZmcpthG(ref num11, num8, num9, num10, 15, 10, 0x3a, numArray);
            ywnZmcpthG(ref num10, num11, num8, num9, 6, 15, 0x3b, numArray);
            ywnZmcpthG(ref num9, num10, num11, num8, 13, 0x15, 60, numArray);
            ywnZmcpthG(ref num8, num9, num10, num11, 4, 6, 0x3d, numArray);
            ywnZmcpthG(ref num11, num8, num9, num10, 11, 10, 0x3e, numArray);
            ywnZmcpthG(ref num10, num11, num8, num9, 2, 15, 0x3f, numArray);
            ywnZmcpthG(ref num9, num10, num11, num8, 9, 0x15, 0x40, numArray);
            num8 += num15;
            num9 += num16;
            num10 += num17;
            num11 += num18;
        }
        byte[] destinationArray = new byte[0x10];
        Array.Copy(BitConverter.GetBytes(num8), 0, destinationArray, 0, 4);
        Array.Copy(BitConverter.GetBytes(num9), 0, destinationArray, 4, 4);
        Array.Copy(BitConverter.GetBytes(num10), 0, destinationArray, 8, 4);
        Array.Copy(BitConverter.GetBytes(num11), 0, destinationArray, 12, 4);
        return destinationArray;
    }

    private static void smethod_1(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, object object_0)
    {
        uint_1 = uint_2 + smethod_4(((uint_1 + ((uint_2 & uint_3) | (~uint_2 & uint_4))) + object_0[uint_5]) + uint_0[(int) ((IntPtr) (uint_6 - 1))], ushort_0);
    }

    [Attribute0(typeof(Attribute0.Class4<object>[]))]
    internal static string smethod_10(int int_3)
    {
        if (byte_3.Length == 0)
        {
            BinaryReader reader = new BinaryReader(assembly_0.GetManifestResourceStream("060Hjua3VcYYofB8kc.B8mJIhm13YQC35FDEm")) {
                BaseStream = { Position = 0L }
            };
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            byte[] buffer = reader.ReadBytes((int) reader.BaseStream.Length);
            reader.Close();
            byte[] buffer5 = new byte[0x20];
            buffer5[0] = 0x95;
            buffer5[0] = 0x69;
            buffer5[0] = 0x88;
            buffer5[0] = (byte) (0xa3 + 0x16);
            buffer5[1] = (byte) (0xdf - 0x4a);
            buffer5[1] = 0x7c;
            buffer5[1] = 12;
            buffer5[2] = 0xa4;
            buffer5[2] = (byte) (230 - 0x4c);
            buffer5[2] = (byte) (0x91 - 0x30);
            buffer5[2] = (byte) (0xbf - 0x3f);
            buffer5[2] = (byte) (0x87 + 0x2e);
            buffer5[3] = (byte) (0xf3 - 0x51);
            buffer5[3] = (byte) (0xdd - 0x49);
            buffer5[3] = (byte) (50 + 0x7a);
            buffer5[3] = (byte) (0xa1 - 0x35);
            buffer5[3] = 0x6d;
            buffer5[3] = (byte) (0x2c - 4);
            buffer5[4] = (byte) (0xe1 - 0x4b);
            buffer5[4] = 0xc9;
            buffer5[4] = (byte) (0x3f + 0x5d);
            buffer5[4] = 0x37;
            buffer5[5] = 0x70;
            buffer5[5] = 0xa6;
            buffer5[5] = 0x99;
            buffer5[5] = (byte) (0x9f - 0x35);
            buffer5[5] = 0xc0;
            buffer5[6] = 0x71;
            buffer5[6] = (byte) (0xb9 - 0x3d);
            buffer5[6] = (byte) (4 + 0x59);
            buffer5[6] = (byte) (140 - 0x2e);
            buffer5[6] = 0x52;
            buffer5[6] = (byte) (0x6b + 0x65);
            buffer5[7] = (byte) (0xdb - 0x49);
            buffer5[7] = 150;
            buffer5[7] = 0x6f;
            buffer5[7] = (byte) (0x94 - 0x47);
            buffer5[8] = 0x8a;
            buffer5[8] = 100;
            buffer5[8] = (byte) (0xbc - 0x3e);
            buffer5[8] = (byte) (0x94 + 0x1c);
            buffer5[9] = (byte) (0x58 + 0x77);
            buffer5[9] = 0x8b;
            buffer5[9] = (byte) (0x5d + 2);
            buffer5[9] = 0x86;
            buffer5[10] = (byte) (0xde - 0x4a);
            buffer5[10] = (byte) (11 + 0x1b);
            buffer5[10] = 0x9a;
            buffer5[10] = 0xc6;
            buffer5[11] = (byte) (0x6b + 7);
            buffer5[11] = (byte) (0x13 + 0x39);
            buffer5[11] = 0x87;
            buffer5[11] = (byte) (0xb8 - 0x3d);
            buffer5[11] = 0xda;
            buffer5[12] = (byte) (0xa4 - 0x36);
            buffer5[12] = 180;
            buffer5[12] = 0x8b;
            buffer5[12] = 0x9a;
            buffer5[13] = 0x47;
            buffer5[13] = 0x6a;
            buffer5[13] = (byte) (0x26 + 0x40);
            buffer5[13] = 0xb6;
            buffer5[14] = (byte) (0x1f + 0x65);
            buffer5[14] = 0x9e;
            buffer5[14] = (byte) (0x23 + 0x10);
            buffer5[14] = 0x1c;
            buffer5[15] = (byte) (0x5c + 0x67);
            buffer5[15] = (byte) (0xab - 0x39);
            buffer5[15] = 0xcd;
            buffer5[0x10] = 0x65;
            buffer5[0x10] = 0x91;
            buffer5[0x10] = (byte) (0x68 - 0x2b);
            buffer5[0x11] = (byte) (0xab - 0x39);
            buffer5[0x11] = 0x57;
            buffer5[0x11] = 0xad;
            buffer5[0x11] = (byte) (0x9a - 0x33);
            buffer5[0x11] = 0x34;
            buffer5[0x12] = (byte) (0x86 - 0x2c);
            buffer5[0x12] = 0x8e;
            buffer5[0x12] = (byte) (0xb9 + 1);
            buffer5[0x13] = 0x4b;
            buffer5[0x13] = 0x37;
            buffer5[0x13] = (byte) (0x16 + 0x3a);
            buffer5[0x13] = 0xba;
            buffer5[0x13] = 0xc6;
            buffer5[0x13] = 0x7f;
            buffer5[20] = (byte) (0x7e - 0x2a);
            buffer5[20] = (byte) (0x1a + 0x4a);
            buffer5[20] = (byte) (0xbb - 0x3e);
            buffer5[20] = 70;
            buffer5[20] = (byte) (0x48 + 0x36);
            buffer5[0x15] = (byte) (0x77 + 0x1b);
            buffer5[0x15] = (byte) (0xb1 - 0x3b);
            buffer5[0x15] = 0x73;
            buffer5[0x15] = 0xab;
            buffer5[0x16] = (byte) (6 + 0x38);
            buffer5[0x16] = (byte) (0xd8 - 0x48);
            buffer5[0x16] = 0xd3;
            buffer5[0x17] = (byte) (0xce - 0x44);
            buffer5[0x17] = 0x6f;
            buffer5[0x17] = (byte) (0x40 + 0x60);
            buffer5[0x17] = (byte) (0xb7 + 0x39);
            buffer5[0x18] = (byte) (0xd7 - 0x47);
            buffer5[0x18] = 130;
            buffer5[0x18] = (byte) (0x51 - 0x2e);
            buffer5[0x19] = 0x89;
            buffer5[0x19] = 0x80;
            buffer5[0x19] = 0x5e;
            buffer5[0x19] = 0x84;
            buffer5[0x19] = (byte) (0x29 + 0x72);
            buffer5[0x1a] = (byte) (0x7e - 0x2a);
            buffer5[0x1a] = 0x2c;
            buffer5[0x1a] = (byte) (0x76 + 0x36);
            buffer5[0x1a] = 0x16;
            buffer5[0x1b] = 0x4b;
            buffer5[0x1b] = 0x7e;
            buffer5[0x1b] = 0x8d;
            buffer5[0x1b] = 0x90;
            buffer5[0x1b] = 0x9a;
            buffer5[0x1b] = 90;
            buffer5[0x1c] = 0x9e;
            buffer5[0x1c] = 0x84;
            buffer5[0x1c] = 0x61;
            buffer5[0x1d] = 0x7c;
            buffer5[0x1d] = (byte) (0xfb - 0x53);
            buffer5[0x1d] = (byte) (0x1a + 0x51);
            buffer5[0x1d] = (byte) (0xac + 0x22);
            buffer5[30] = (byte) (0xe5 - 0x4c);
            buffer5[30] = (byte) (160 - 0x35);
            buffer5[30] = (byte) (130 - 0x2b);
            buffer5[30] = (byte) (0xc4 - 0x53);
            buffer5[0x1f] = (byte) (0x77 + 0x72);
            buffer5[0x1f] = 0x98;
            buffer5[0x1f] = 210;
            buffer5[0x1f] = (byte) (100 - 0x60);
            byte[] rgbKey = buffer5;
            byte[] buffer6 = new byte[0x10];
            buffer6[0] = 0x95;
            buffer6[0] = (byte) (30 + 0x67);
            buffer6[0] = 0x63;
            buffer6[0] = 0x95;
            buffer6[0] = (byte) (0x6f - 0x30);
            buffer6[1] = 0x81;
            buffer6[1] = (byte) (0xe3 - 0x4b);
            buffer6[1] = 0x80;
            buffer6[1] = (byte) (0x13 + 0x1c);
            buffer6[1] = 0x74;
            buffer6[1] = 4;
            buffer6[2] = 0x94;
            buffer6[2] = (byte) (50 + 0x7a);
            buffer6[2] = (byte) (0x22 + 0x3e);
            buffer6[2] = 0x33;
            buffer6[2] = (byte) (0x61 - 0x58);
            buffer6[3] = (byte) (0x39 + 0x5b);
            buffer6[3] = (byte) (0xd9 - 0x48);
            buffer6[3] = (byte) (0xde - 0x4a);
            buffer6[3] = 0x9a;
            buffer6[3] = 0x98;
            buffer6[3] = 0xae;
            buffer6[4] = (byte) (100 + 0x55);
            buffer6[4] = 0x5f;
            buffer6[4] = (byte) (0xa9 - 0x38);
            buffer6[4] = 0x7c;
            buffer6[4] = (byte) (4 + 0x59);
            buffer6[4] = (byte) (0x49 - 0x2b);
            buffer6[5] = 0x7c;
            buffer6[5] = 0x69;
            buffer6[5] = 0x7a;
            buffer6[5] = (byte) (0x70 + 90);
            buffer6[6] = (byte) (0x2a + 0x6c);
            buffer6[6] = 0x6f;
            buffer6[6] = 0x7a;
            buffer6[6] = (byte) (0 + 0x54);
            buffer6[6] = 0x60;
            buffer6[7] = 0x48;
            buffer6[7] = (byte) (0xf8 - 0x52);
            buffer6[7] = (byte) (0x4f + 60);
            buffer6[7] = 0x94;
            buffer6[7] = (byte) (0x6d - 2);
            buffer6[8] = (byte) (0x39 + 0x48);
            buffer6[8] = (byte) (0x4d + 11);
            buffer6[8] = 0x6c;
            buffer6[8] = (byte) (15 + 0);
            buffer6[8] = 0x51;
            buffer6[8] = 130;
            buffer6[9] = (byte) (0xca - 0x43);
            buffer6[9] = 0x7b;
            buffer6[9] = 0x8a;
            buffer6[9] = (byte) (0x26 + 0);
            buffer6[9] = 0x4d;
            buffer6[10] = (byte) (0x99 - 0x33);
            buffer6[10] = 0x67;
            buffer6[10] = 0x44;
            buffer6[11] = 0x8b;
            buffer6[11] = 0x5c;
            buffer6[11] = 0xc5;
            buffer6[12] = 0x60;
            buffer6[12] = 0x84;
            buffer6[12] = (byte) (0xec - 0x4e);
            buffer6[12] = 0x6c;
            buffer6[12] = (byte) (0x6b + 0x24);
            buffer6[12] = (byte) (0xba - 0x47);
            buffer6[13] = 0xd8;
            buffer6[13] = (byte) (0x1c + 0x3f);
            buffer6[13] = (byte) (0x97 - 50);
            buffer6[13] = (byte) (40 + 0x5c);
            buffer6[14] = 0x6a;
            buffer6[14] = (byte) (0xab - 0x39);
            buffer6[14] = 0x57;
            buffer6[15] = (byte) (0xea - 0x4e);
            buffer6[15] = 0xa8;
            buffer6[15] = 0x80;
            buffer6[15] = (byte) (0x49 - 0x10);
            byte[] array = buffer6;
            Array.Reverse(array);
            for (int i = 0; i < array.Length; i++)
            {
                rgbKey[i] = (byte) (rgbKey[i] ^ array[i]);
            }
            if (int_3 == -1)
            {
                SymmetricAlgorithm algorithm = smethod_6();
                algorithm.Mode = CipherMode.CBC;
                ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, array);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                byte_3 = stream.ToArray();
                stream.Close();
                stream2.Close();
                buffer = byte_3;
            }
            int num = buffer.Length % 4;
            int num9 = buffer.Length / 4;
            byte[] buffer3 = new byte[buffer.Length];
            int num17 = rgbKey.Length / 4;
            uint num6 = 0;
            uint num7 = 0;
            uint num5 = 0;
            if (num > 0)
            {
                num9++;
            }
            uint index = 0;
            for (int j = 0; j < num9; j++)
            {
                int num18 = j % num17;
                int num3 = j * 4;
                index = (uint) (num18 * 4);
                num7 = (uint) ((((rgbKey[(int) ((IntPtr) (index + 3))] << 0x18) | (rgbKey[(int) ((IntPtr) (index + 2))] << 0x10)) | (rgbKey[(int) ((IntPtr) (index + 1))] << 8)) | rgbKey[index]);
                uint num12 = 0xff;
                int num13 = 0;
                if ((j == (num9 - 1)) && (num > 0))
                {
                    num5 = 0;
                    for (int k = 0; k < num; k++)
                    {
                        if (k > 0)
                        {
                            num5 = num5 << 8;
                        }
                        num5 |= buffer[buffer.Length - (1 + k)];
                    }
                    num6 += num7;
                }
                else
                {
                    index = (uint) num3;
                    num5 = (uint) ((((buffer[(int) ((IntPtr) (index + 3))] << 0x18) | (buffer[(int) ((IntPtr) (index + 2))] << 0x10)) | (buffer[(int) ((IntPtr) (index + 1))] << 8)) | buffer[index]);
                    num6 += num7;
                }
                uint num19 = num6;
                num19 ^= num19 << 0x19;
                num19 += 0xb5d60c64;
                num19 ^= num19 << 2;
                num19 += 0xa07935c6;
                num19 ^= num19 >> 15;
                num19 += 0x36d9f688;
                num19 = ((uint) (((0x7f808fc8 << 11) - 0x7f808fc8) ^ -1602669114)) + num19;
                num6 += num19;
                if ((j == (num9 - 1)) && (num > 0))
                {
                    uint num11 = num6 ^ num5;
                    for (int m = 0; m < num; m++)
                    {
                        if (m > 0)
                        {
                            num12 = num12 << 8;
                            num13 += 8;
                        }
                        buffer3[num3 + m] = (byte) ((num11 & num12) >> num13);
                    }
                }
                else
                {
                    uint num16 = num6 ^ num5;
                    buffer3[num3] = (byte) (num16 & 0xff);
                    buffer3[num3 + 1] = (byte) ((num16 & 0xff00) >> 8);
                    buffer3[num3 + 2] = (byte) ((num16 & 0xff0000) >> 0x10);
                    buffer3[num3 + 3] = (byte) ((num16 & -16777216) >> 0x18);
                }
            }
            byte_3 = buffer3;
        }
        int count = BitConverter.ToInt32(byte_3, int_3);
        try
        {
            return Encoding.Unicode.GetString(byte_3, int_3 + 4, count);
        }
        catch
        {
        }
        return "";
    }

    [Attribute0(typeof(Attribute0.Class4<object>[]))]
    internal static string smethod_11(string string_1)
    {
        "{11111-22222-50001-00000}".Trim();
        byte[] bytes = Convert.FromBase64String(string_1);
        return Encoding.Unicode.GetString(bytes, 0, bytes.Length);
    }

    [Attribute0(typeof(Attribute0.Class4<object>[]))]
    private static byte[] smethod_12(string string_1)
    {
        byte[] buffer;
        using (FileStream stream = new FileStream(string_1, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            int offset = 0;
            int length = (int) stream.Length;
            buffer = new byte[length];
            while (length > 0)
            {
                int num4 = stream.Read(buffer, offset, length);
                offset += num4;
                length -= num4;
            }
        }
        return buffer;
    }

    [Attribute0(typeof(Attribute0.Class4<object>[]))]
    private static byte[] smethod_13(byte[] byte_4)
    {
        MemoryStream stream = new MemoryStream();
        SymmetricAlgorithm algorithm = smethod_6();
        algorithm.Key = new byte[] { 
            180, 0xc6, 0x9c, 0x49, 110, 30, 0x51, 2, 0xaf, 0x7c, 15, 40, 110, 0xdb, 0x75, 14, 
            0xf6, 0x13, 0xee, 0x42, 0x1f, 0xdd, 0xb3, 0x5d, 6, 0x8d, 0x52, 0xec, 0x37, 10, 0x51, 0x52
         };
        algorithm.IV = new byte[] { 0x24, 0x74, 0xbf, 0x81, 0xad, 0x7b, 0x61, 230, 0x30, 0x34, 0x22, 0x40, 0xa4, 0xbf, 0x5f, 0x79 };
        CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Write);
        stream2.Write(byte_4, 0, byte_4.Length);
        stream2.Close();
        return stream.ToArray();
    }

    private static void smethod_2(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, object object_0)
    {
        uint_1 = uint_2 + smethod_4(((uint_1 + ((uint_2 & uint_4) | (uint_3 & ~uint_4))) + object_0[uint_5]) + uint_0[(int) ((IntPtr) (uint_6 - 1))], ushort_0);
    }

    private static void smethod_3(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, object object_0)
    {
        uint_1 = uint_2 + smethod_4(((uint_1 + ((uint_2 ^ uint_3) ^ uint_4)) + object_0[uint_5]) + uint_0[(int) ((IntPtr) (uint_6 - 1))], ushort_0);
    }

    private static uint smethod_4(uint uint_1, ushort ushort_0)
    {
        return ((uint_1 >> (0x20 - ushort_0)) | (uint_1 << ushort_0));
    }

    internal static bool smethod_5()
    {
        if (!bool_1)
        {
            smethod_7();
            bool_1 = true;
        }
        return bool_2;
    }

    internal static SymmetricAlgorithm smethod_6()
    {
        if (smethod_5())
        {
            try
            {
                return (SymmetricAlgorithm) Activator.CreateInstance("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Security.Cryptography.AesCryptoServiceProvider").Unwrap();
            }
            catch
            {
                return new RijndaelManaged();
            }
        }
        try
        {
            return new RijndaelManaged();
        }
        catch
        {
            return (SymmetricAlgorithm) Activator.CreateInstance("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Security.Cryptography.AesCryptoServiceProvider").Unwrap();
        }
    }

    internal static void smethod_7()
    {
        try
        {
            new RijndaelManaged();
            bool_2 = false;
        }
        catch
        {
            bool_2 = true;
        }
    }

    internal static byte[] smethod_8(byte[] byte_4)
    {
        if (!smethod_5())
        {
            return new MD5CryptoServiceProvider().ComputeHash(byte_4);
        }
        return smethod_0(byte_4);
    }

    private static uint smethod_9(uint uint_1)
    {
        return (uint) "{11111-22222-10009-11112}".Length;
    }

    private static void ywnZmcpthG(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, object object_0)
    {
        uint_1 = uint_2 + smethod_4(((uint_1 + (uint_3 ^ (uint_2 | ~uint_4))) + object_0[uint_5]) + uint_0[(int) ((IntPtr) (uint_6 - 1))], ushort_0);
    }

    internal class Attribute0 : Attribute
    {
        [Attribute0(typeof(Class4<object>[]))]
        public Attribute0(object object_0)
        {
        }

        internal class Class4<NyHorW72D1l5d0uLde>
        {
        }
    }

    internal class Class5
    {
        [Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
        internal static void ce4DmfsmSrOT856tDgfrkMb()
        {
            if (smethod_0(Convert.ToBase64String(Class3.assembly_0.GetName().GetPublicKeyToken()), Class3.smethod_10(0x1552)) != Class3.smethod_10(0x1558))
            {
                while (true)
                {
                    ce4DmfsmSrOT856tDgfrkMb();
                }
            }
        }

        [Attribute0(typeof(Class3.Attribute0.Class4<object>[]))]
        internal static string smethod_0(string string_0, string string_1)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(string_0);
            byte[] buffer3 = new byte[] { 
                0x52, 0x66, 0x68, 110, 0x20, 0x4d, 0x18, 0x22, 0x76, 0xb5, 0x33, 0x11, 0x12, 0x33, 12, 0x6d, 
                10, 0x20, 0x4d, 0x18, 0x22, 0x9e, 0xa1, 0x29, 0x61, 0x1c, 0x76, 0xb5, 5, 0x19, 1, 0x58
             };
            byte[] buffer4 = Class3.smethod_8(Encoding.Unicode.GetBytes(string_1));
            MemoryStream stream = new MemoryStream();
            SymmetricAlgorithm algorithm = Class3.smethod_6();
            algorithm.Key = buffer3;
            algorithm.IV = buffer4;
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.Close();
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}

