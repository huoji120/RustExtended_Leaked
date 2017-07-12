namespace RustProtect
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    public class Hardware
    {
        private static byte[] byte_0 = null;
        private static byte[] byte_1 = null;
        private static byte[] byte_2 = null;
        private static byte[] byte_3 = null;
        private static string string_0 = DateTime.Now.ToString(Class3.smethod_10(0x124));

        public Hardware()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                if (ProtectLoader.Debug)
                {
                    Debug.Log(Class3.smethod_10(12));
                }
                string[] strArray = smethod_2(BiosFirmwareTableProvider.RSMB);
                if (strArray.Length != 0)
                {
                    byte[] buffer3 = smethod_0(BiosFirmwareTableProvider.RSMB, strArray[0]);
                    int length = buffer3.Length;
                    if (length != 0)
                    {
                        byte[] buffer2;
                        int num;
                        if (length > 0x400)
                        {
                            length = 0x400;
                        }
                        writer.Write(buffer3, 0, length);
                        if (ProtectLoader.Debug)
                        {
                            Debug.Log(Class3.smethod_10(0x44));
                        }
                        byte[] buffer4 = new byte[] { 
                            0x55, 0x89, 0xe5, 0x57, 0x8b, 0x7d, 0x10, 0x6a, 1, 0x58, 0x53, 15, 0xa2, 0x89, 7, 0x89, 
                            0x57, 4, 0x5b, 0x5f, 0x89, 0xec, 0x5d, 0xc2, 0x10, 0
                         };
                        byte[] buffer = new byte[] { 
                            0x53, 0x48, 0xc7, 0xc0, 1, 0, 0, 0, 15, 0xa2, 0x41, 0x89, 0, 0x41, 0x89, 80, 
                            4, 0x5b, 0xc3
                         };
                        byte[] buffer5 = new byte[8];
                        if (IntPtr.Size == 8)
                        {
                            buffer2 = buffer;
                        }
                        else
                        {
                            buffer2 = buffer4;
                        }
                        IntPtr ptr = new IntPtr(buffer2.Length);
                        if (!Class1.VirtualProtect(buffer2, ptr, 0x40, out num))
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }
                        ptr = new IntPtr(buffer5.Length);
                        if (Class1.CallWindowProcW(buffer2, IntPtr.Zero, 0, buffer5, ptr) != IntPtr.Zero)
                        {
                            if (ProtectLoader.Debug)
                            {
                                Debug.Log(Class3.smethod_10(0x76));
                            }
                            writer.Write(SystemInfo.deviceName);
                            writer.Write(SystemInfo.deviceModel);
                            writer.Write(SystemInfo.graphicsDeviceName);
                            writer.Write(SystemInfo.graphicsDeviceID);
                            writer.Write(SystemInfo.graphicsDeviceVendorID);
                            writer.Write(SystemInfo.graphicsMemorySize);
                            if (ProtectLoader.Debug)
                            {
                                Debug.Log(Class3.smethod_10(0xa6));
                            }
                            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
                            byte_0 = new MD5CryptoServiceProvider().ComputeHash(writer.BaseStream);
                            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
                            byte_1 = new SHA1CryptoServiceProvider().ComputeHash(writer.BaseStream);
                            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
                            byte_2 = new SHA256CryptoServiceProvider().ComputeHash(writer.BaseStream);
                            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
                            byte_3 = new SHA512CryptoServiceProvider().ComputeHash(writer.BaseStream);
                            if (ProtectLoader.Debug)
                            {
                                Debug.Log(Class3.smethod_10(0xd6));
                            }
                        }
                    }
                }
            }
        }

        private uint method_0(uint uint_0, uint uint_1, uint uint_2, uint uint_3)
        {
            return ((((uint_0 << 0x10) | (uint_3 << 14)) | (uint_1 << 2)) | uint_2);
        }

        private string method_1(char[] char_0)
        {
            for (int i = 0; i <= (char_0.Length - 2); i += 2)
            {
                char ch = char_0[i];
                char_0[i] = char_0[i + 1];
                char_0[i + 1] = ch;
            }
            return new string(char_0);
        }

        private string method_2(byte[] byte_4)
        {
            for (int i = 0; i <= (byte_4.Length - 2); i += 2)
            {
                byte num2 = byte_4[i];
                byte_4[i] = byte_4[i + 1];
                byte_4[i + 1] = num2;
            }
            return Encoding.ASCII.GetString(byte_4);
        }

        private string method_3(string string_1)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < string_1.Length; i += 4)
            {
                builder.Append(Convert.ToChar(Convert.ToByte(string_1.Substring(i + 2, 2), 0x10)));
                builder.Append(Convert.ToChar(Convert.ToByte(string_1.Substring(i, 2), 0x10)));
            }
            char[] trimChars = new char[2];
            trimChars[1] = ' ';
            return builder.ToString().Trim(trimChars);
        }

        private PhysicalDisk method_4(string string_1)
        {
            PhysicalDisk disk = new PhysicalDisk {
                Extents = new VOLUMEDISKEXTENTS(),
                Adapter = new STORAGE_ADAPTER_DESCRIPTOR(),
                DeviceID = new STORAGE_DEVICE_ID_DESCRIPTOR(),
                Device = new STORAGE_DEVICE_DESCRIPTOR(),
                Version = new GETVERSIONOUTPARAMS(),
                Params = new SENDCMDOUTPARAMS(),
                SerialNumber = string.Empty,
                Firmware = string.Empty,
                Model = string.Empty
            };
            char[] trimChars = new char[] { '\\' };
            SafeFileHandle handle = Class1.CreateFileW(Class3.smethod_10(0) + Path.GetPathRoot(string_1).TrimEnd(trimChars), 0xc0000000, 3, IntPtr.Zero, 3, 0x80, IntPtr.Zero);
            try
            {
                uint num = 0;
                int cb = Marshal.SizeOf(disk.Extents);
                IntPtr ptr = Marshal.AllocHGlobal(cb);
                Struct0 struct2 = new Struct0();
                uint num3 = this.method_0(0x56, 0, 0, 0);
                if (Class1.DeviceIoControl_1(handle, num3, IntPtr.Zero, 0, ptr, cb, ref num, 0))
                {
                    disk.Extents = (VOLUMEDISKEXTENTS) Marshal.PtrToStructure(ptr, typeof(VOLUMEDISKEXTENTS));
                    disk.Number = Convert.ToByte(disk.Extents.diskNumber);
                }
                cb = Marshal.SizeOf(disk.Adapter);
                ptr = Marshal.AllocHGlobal(cb);
                struct2.uint_0 = 1;
                struct2.uint_1 = 0;
                if (Class1.DeviceIoControl(handle, 0x2d1400, struct2, Marshal.SizeOf(struct2), ptr, cb, ref num, 0))
                {
                    disk.Adapter = (STORAGE_ADAPTER_DESCRIPTOR) Marshal.PtrToStructure(ptr, typeof(STORAGE_ADAPTER_DESCRIPTOR));
                }
                cb = Marshal.SizeOf(disk.DeviceID);
                ptr = Marshal.AllocHGlobal(cb);
                struct2.uint_0 = 2;
                struct2.uint_1 = 0;
                if (Class1.DeviceIoControl(handle, 0x2d1400, struct2, Marshal.SizeOf(struct2), ptr, cb, ref num, 0))
                {
                    disk.DeviceID = (STORAGE_DEVICE_ID_DESCRIPTOR) Marshal.PtrToStructure(ptr, typeof(STORAGE_DEVICE_ID_DESCRIPTOR));
                }
                cb = Marshal.SizeOf(disk.Device);
                ptr = Marshal.AllocHGlobal(cb);
                struct2.uint_0 = 0;
                struct2.uint_1 = 0;
                if (Class1.DeviceIoControl(handle, 0x2d1400, struct2, Marshal.SizeOf(struct2), ptr, cb, ref num, 0))
                {
                    disk.Device = (STORAGE_DEVICE_DESCRIPTOR) Marshal.PtrToStructure(ptr, typeof(STORAGE_DEVICE_DESCRIPTOR));
                    string str2 = Encoding.ASCII.GetString(disk.Device.RawDeviceProperties);
                    int num4 = Marshal.SizeOf(disk.Device) - disk.Device.RawDeviceProperties.Length;
                    if (disk.Device.ProductIdOffset > 0)
                    {
                        char[] chArray2 = new char[2];
                        chArray2[1] = ' ';
                        disk.Model = str2.Substring(disk.Device.ProductIdOffset - num4, 20).Trim(chArray2);
                    }
                    if (disk.Device.ProductRevisionOffset > 0)
                    {
                        char[] chArray3 = new char[2];
                        chArray3[1] = ' ';
                        disk.Firmware = str2.Substring(disk.Device.ProductRevisionOffset - num4, 8).Trim(chArray3);
                    }
                    if (disk.Device.SerialNumberOffset > 0)
                    {
                        char[] chArray4 = new char[2];
                        chArray4[1] = ' ';
                        disk.SerialNumber = str2.Substring(disk.Device.SerialNumberOffset - num4, 40).Trim(chArray4);
                    }
                    if ((disk.SerialNumber != null) && (disk.SerialNumber.Length == 40))
                    {
                        disk.SerialNumber = this.method_3(disk.SerialNumber);
                    }
                    disk.RemovableMedia = Convert.ToBoolean(disk.Device.RemovableMedia);
                }
                cb = Marshal.SizeOf(disk.Version);
                ptr = Marshal.AllocHGlobal(cb);
                if (Class1.DeviceIoControl_1(handle, 0x74080, IntPtr.Zero, 0, ptr, cb, ref num, 0))
                {
                    disk.Version = (GETVERSIONOUTPARAMS) Marshal.PtrToStructure(ptr, typeof(GETVERSIONOUTPARAMS));
                    if ((disk.Version.fCapabilities & 4L) > 0L)
                    {
                        SENDCMDINPARAMS sendcmdinparams = new SENDCMDINPARAMS();
                        sendcmdinparams.DriveRegs.Command = 0xec;
                        sendcmdinparams.DriveNumber = disk.Number;
                        sendcmdinparams.BufferSize = 0x200;
                        if (Class1.DeviceIoControl(handle, 0x7c088, sendcmdinparams, Marshal.SizeOf(sendcmdinparams), ptr, cb, ref num, 0))
                        {
                            disk.Params = (SENDCMDOUTPARAMS) Marshal.PtrToStructure(ptr, typeof(SENDCMDOUTPARAMS));
                            disk.Model = this.method_2(disk.Params.IDS.ModelNumber).Trim();
                            disk.Firmware = this.method_2(disk.Params.IDS.FirmwareRevision).Trim();
                            disk.SerialNumber = this.method_2(disk.Params.IDS.SerialNumber).Trim();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (handle != null)
                {
                    handle.Dispose();
                }
            }
            return disk;
        }

        private static byte[] smethod_0(BiosFirmwareTableProvider biosFirmwareTableProvider_0, string string_1)
        {
            int num = (((string_1[3] << 0x18) | (string_1[2] << 0x10)) | (string_1[1] << 8)) | string_1[0];
            return smethod_1(biosFirmwareTableProvider_0, num);
        }

        private static byte[] smethod_1(BiosFirmwareTableProvider biosFirmwareTableProvider_0, int int_0)
        {
            byte[] destination = new byte[0];
            try
            {
                int cb = Class1.GetSystemFirmwareTable(biosFirmwareTableProvider_0, int_0, IntPtr.Zero, 0);
                if (cb <= 0)
                {
                    return destination;
                }
                IntPtr ptr = Marshal.AllocHGlobal(cb);
                Class1.GetSystemFirmwareTable(biosFirmwareTableProvider_0, int_0, ptr, cb);
                if (Marshal.GetLastWin32Error() == 0)
                {
                    destination = new byte[cb];
                    Marshal.Copy(ptr, destination, 0, cb);
                }
                Marshal.FreeHGlobal(ptr);
            }
            catch
            {
            }
            return destination;
        }

        private static string[] smethod_2(BiosFirmwareTableProvider biosFirmwareTableProvider_0)
        {
            string[] strArray = new string[0];
            try
            {
                int cb = Class1.EnumSystemFirmwareTables(biosFirmwareTableProvider_0, IntPtr.Zero, 0);
                if (cb <= 0)
                {
                    return strArray;
                }
                byte[] destination = new byte[cb];
                IntPtr ptr = Marshal.AllocHGlobal(cb);
                Class1.EnumSystemFirmwareTables(biosFirmwareTableProvider_0, ptr, cb);
                if (Marshal.GetLastWin32Error() == 0)
                {
                    strArray = new string[cb / 4];
                    Marshal.Copy(ptr, destination, 0, cb);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray[i] = Encoding.ASCII.GetString(destination, 4 * i, 4);
                    }
                }
                Marshal.FreeHGlobal(ptr);
            }
            catch
            {
            }
            return strArray;
        }

        private static string[] smethod_3(byte[] byte_4, SMBIOSTableEntry smbiostableEntry_0, ref int int_0)
        {
            int num;
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            int_0 += smbiostableEntry_0.header.length;
            do
            {
                string item = byte_4.GetString(int_0);
                list.Add(item);
                int_0 += item.Length;
                num = int_0 + 1;
                int_0 = num;
            }
            while (byte_4[num] != 0);
            int_0++;
            return list.ToArray();
        }

        public byte[] MD5
        {
            get
            {
                if ((byte_0 != null) && (byte_0.Length >= 0x10))
                {
                    return byte_0;
                }
                return new byte[0];
            }
        }

        public byte[] SHA1
        {
            get
            {
                if ((byte_1 != null) && (byte_1.Length >= 0x10))
                {
                    return byte_1;
                }
                return new byte[0];
            }
        }

        public byte[] SHA256
        {
            get
            {
                if ((byte_2 != null) && (byte_2.Length >= 0x10))
                {
                    return byte_2;
                }
                return new byte[0];
            }
        }

        public string SHA256String
        {
            get
            {
                if ((byte_2 != null) && (byte_2.Length >= 0x10))
                {
                    return BitConverter.ToString(byte_2, 0).Replace(Class3.smethod_10(0x11e), "").ToLower();
                }
                return "";
            }
        }

        public byte[] SHA512
        {
            get
            {
                if ((byte_3 != null) && (byte_3.Length >= 0x10))
                {
                    return byte_3;
                }
                return new byte[0];
            }
        }

        public string SHA512String
        {
            get
            {
                if ((byte_3 != null) && (byte_3.Length >= 0x10))
                {
                    return BitConverter.ToString(byte_3, 0).Replace(Class3.smethod_10(0x11e), "").ToLower();
                }
                return "";
            }
        }

        public string String_0
        {
            get
            {
                if ((byte_0 != null) && (byte_0.Length >= 0x10))
                {
                    return BitConverter.ToString(byte_0, 0).Replace(Class3.smethod_10(0x11e), "").ToLower();
                }
                return "";
            }
        }

        public string String_1
        {
            get
            {
                if ((byte_1 != null) && (byte_1.Length >= 0x10))
                {
                    return BitConverter.ToString(byte_1, 0).Replace(Class3.smethod_10(0x11e), "").ToLower();
                }
                return "";
            }
        }

        public enum BiosFirmwareTableProvider
        {
            ACPI = 0x41435049,
            FIRM = 0x4649524d,
            RSMB = 0x52534d42
        }

        private static class Class1
        {
            [DllImport("USER32.dll", CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
            public static extern IntPtr CallWindowProcW([In] byte[] byte_0, IntPtr intptr_0, int int_0, [In, Out] byte[] byte_1, IntPtr intptr_1);
            [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall)]
            public static extern int CloseHandle(int int_0);
            [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall, SetLastError=true)]
            public static extern SafeFileHandle CreateFileW([MarshalAs(UnmanagedType.LPWStr)] string string_0, uint uint_0, uint uint_1, IntPtr intptr_0, uint uint_2, uint uint_3, IntPtr intptr_1);
            [DllImport("KERNEL32.DLL", SetLastError=true)]
            public static extern bool DeviceIoControl(SafeFileHandle safeFileHandle_0, uint uint_0, [In, MarshalAs(UnmanagedType.AsAny)] object object_0, int int_0, [Out] IntPtr intptr_0, [Out] int int_1, ref uint uint_1, int int_2);
            [DllImport("KERNEL32.DLL", EntryPoint="DeviceIoControl", SetLastError=true)]
            public static extern bool DeviceIoControl_1(SafeFileHandle safeFileHandle_0, uint uint_0, IntPtr intptr_0, int int_0, [Out] IntPtr intptr_1, [Out] int int_1, ref uint uint_1, int int_2);
            [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall, SetLastError=true)]
            public static extern bool EnumDisplayDevices(string string_0, uint uint_0, ref Hardware.DISPLAY_DEVICE display_DEVICE_0, uint uint_1);
            [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall, SetLastError=true)]
            public static extern int EnumSystemFirmwareTables(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0, IntPtr intptr_0, int int_0);
            [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall, SetLastError=true)]
            public static extern int GetSystemFirmwareTable(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0, int int_0, IntPtr intptr_0, int int_1);
            [DllImport("KERNEL32.DLL", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Auto, SetLastError=true)]
            public static extern bool GetVolumeNameForVolumeMountPoint(string string_0, StringBuilder stringBuilder_0, uint uint_0);
            [DllImport("KERNEL32.DLL", CharSet=CharSet.Unicode, SetLastError=true)]
            public static extern bool VirtualProtect([In] byte[] byte_0, IntPtr intptr_0, int int_0, out int int_1);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int structSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public Hardware.DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
            public string DeviceKey;
        }

        public enum DisplayDeviceStateFlags
        {
            AttachedToDesktop = 1,
            Disconnect = 0x2000000,
            MirroringDriver = 8,
            ModesPruned = 0x8000000,
            MultiDriver = 2,
            PrimaryDevice = 4,
            Remote = 0x4000000,
            Removable = 0x20,
            VGACompatible = 0x10
        }

        [StructLayout(LayoutKind.Sequential, Size=12)]
        public struct DRIVERSTATUS
        {
            public byte DriveError;
            public byte byte_0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public int[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GETVERSIONOUTPARAMS
        {
            public byte bVersion;
            public byte bRevision;
            public byte bReserved;
            public byte bIDEDeviceMap;
            public int fCapabilities;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public int[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GStruct0
        {
            public Hardware.SMBIOSTableHeader header;
            public byte vendor;
            public byte version;
            public ushort startingSegment;
            public byte releaseDate;
            public byte biosRomSize;
            public ulong characteristics;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public byte[] extensionBytes;
        }

        [StructLayout(LayoutKind.Sequential, Size=8)]
        public struct IDEREGS
        {
            public byte Features;
            public byte SectorCount;
            public byte SectorNumber;
            public byte CylinderLow;
            public byte CylinderHigh;
            public byte DriveHead;
            public byte Command;
            public byte Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IDSECTOR
        {
            public short GenConfig;
            public short NumberCylinders;
            public short Reserved;
            public short NumberHeads;
            public short BytesPerTrack;
            public short BytesPerSector;
            public short SectorsPerTrack;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
            public short[] VendorUnique;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=20)]
            public byte[] SerialNumber;
            public short BufferClass;
            public short BufferSize;
            public short ECCSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public byte[] FirmwareRevision;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=40)]
            public byte[] ModelNumber;
            public short MoreVendorUnique;
            public short DoubleWordIO;
            public short Capabilities;
            public short Reserved1;
            public short short_0;
            public short short_1;
            public short BS;
            public short NumberCurrentCyls;
            public short NumberCurrentHeads;
            public short NumberCurrentSectorsPerTrack;
            public int CurrentSectorCapacity;
            public short MultipleSectorCapacity;
            public short MultipleSectorStuff;
            public int TotalAddressableSectors;
            public short SingleWordDMA;
            public short MultiWordDMA;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x17e)]
            public byte[] bReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PhysicalDisk
        {
            public byte Number;
            public string Model;
            public string Firmware;
            public string SerialNumber;
            public bool RemovableMedia;
            public Hardware.VOLUMEDISKEXTENTS Extents;
            public Hardware.STORAGE_DEVICE_DESCRIPTOR Device;
            public Hardware.STORAGE_DEVICE_ID_DESCRIPTOR DeviceID;
            public Hardware.STORAGE_ADAPTER_DESCRIPTOR Adapter;
            public Hardware.GETVERSIONOUTPARAMS Version;
            public Hardware.SENDCMDOUTPARAMS Params;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawSMBIOSData
        {
            public byte Used20CallingMethod;
            public byte MajorVersion;
            public byte MinorVersion;
            public byte DmiRevision;
            public uint Length;
            public Hardware.GStruct0 BiosInfo;
            public Hardware.SMBIOSTableSystemInfo SystemInfo;
            public Hardware.SMBIOSTableBaseBoardInfo BaseBoardInfo;
            public Hardware.SMBIOSTableEnclosureInfo EnclosureInfo;
            public Hardware.SMBIOSTableProcessorInfo ProcessorInfo;
            public Hardware.SMBIOSTableCacheInfo CacheInfo;
        }

        [StructLayout(LayoutKind.Sequential, Size=0x20)]
        public struct SENDCMDINPARAMS
        {
            public int BufferSize;
            public Hardware.IDEREGS DriveRegs;
            public byte DriveNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public int[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SENDCMDOUTPARAMS
        {
            public uint cBufferSize;
            public Hardware.DRIVERSTATUS Status;
            public Hardware.IDSECTOR IDS;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableBaseBoardInfo
        {
            public Hardware.SMBIOSTableHeader header;
            public byte manufacturer;
            public byte productName;
            public byte version;
            public byte serialNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableCacheInfo
        {
            public Hardware.SMBIOSTableHeader header;
            public byte socketDesignation;
            public long cacheConfiguration;
            public ushort maximumCacheSize;
            public ushort installedSize;
            public ushort supportedSRAMType;
            public ushort currentSRAMType;
            public byte cacheSpeed;
            public byte errorCorrectionType;
            public byte systemCacheType;
            public byte associativity;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableEnclosureInfo
        {
            public Hardware.SMBIOSTableHeader header;
            public byte manufacturer;
            public byte type;
            public byte version;
            public byte serialNumber;
            public byte assetTagNumber;
            public byte bootUpState;
            public byte powerSupplyState;
            public byte thermalState;
            public byte securityStatus;
            public long OEM_Defined;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableEntry
        {
            public Hardware.SMBIOSTableHeader header;
            public uint index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableHeader
        {
            public Hardware.SMBIOSTableType type;
            public byte length;
            public ushort Handle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableProcessorInfo
        {
            public Hardware.SMBIOSTableHeader header;
            public byte socketDesignation;
            public byte processorType;
            public byte processorFamily;
            public byte processorManufacturer;
            public ulong processorID;
            public byte processorVersion;
            public byte processorVoltage;
            public ushort externalClock;
            public ushort maxSpeed;
            public ushort currentSpeed;
            public byte status;
            public byte processorUpgrade;
            public ushort L1CacheHandler;
            public ushort L2CacheHandler;
            public ushort L3CacheHandler;
            public byte serialNumber;
            public byte assetTag;
            public byte partNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMBIOSTableSystemInfo
        {
            public Hardware.SMBIOSTableHeader header;
            public byte manufacturer;
            public byte productName;
            public byte version;
            public byte serialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x10)]
            public byte[] UUID;
            public byte wakeUpType;
        }

        public enum SMBIOSTableType : sbyte
        {
            BaseBoardInformation = 2,
            BIOSInformation = 0,
            BIOSLanguageInformation = 13,
            CacheInformation = 7,
            const_11 = 11,
            EnclosureInformation = 3,
            EndofTable = 0x7f,
            GroupAssociations = 14,
            MemoryArrayMappedAddress = 0x13,
            MemoryControllerInformation = 5,
            MemoryDevice = 0x11,
            MemoryDeviceMappedAddress = 20,
            MemoryErrorInformation = 0x12,
            MemoryModuleInformation = 6,
            OnBoardDevicesInformation = 10,
            PhysicalMemoryArray = 0x10,
            PortConnectorInformation = 8,
            ProcessorInformation = 4,
            SystemConfigurationOptions = 12,
            SystemEventLog = 15,
            SystemInformation = 1,
            SystemSlotsInformation = 9
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_ADAPTER_DESCRIPTOR
        {
            public uint Version;
            public uint Size;
            public uint MaximumTransferLength;
            public uint MaximumPhysicalPages;
            public uint AlignmentMask;
            public byte AdapterUsesPio;
            public byte AdapterScansDown;
            public byte CommandQueueing;
            public byte AcceleratedTransfer;
            public Hardware.STORAGE_BUS_TYPE BusType;
            public ushort BusMajorVersion;
            public ushort BusMinorVersion;
            public byte SrbType;
            public byte AddressType;
        }

        public enum STORAGE_BUS_TYPE : byte
        {
            BusTypeAta = 3,
            BusTypeAtapi = 2,
            BusTypeFibre = 6,
            BusTypeFileBackedVirtual = 15,
            BusTypeiScsi = 9,
            BusTypeMax = 0x10,
            BusTypeMaxReserved = 0x7f,
            BusTypeMmc = 13,
            BusTypeSas = 10,
            BusTypeSata = 11,
            BusTypeScsi = 1,
            BusTypeSd = 12,
            BusTypeSsa = 5,
            BusTypeUnknown = 0,
            BusTypeUsb = 7,
            BusTypeVirtual = 14,
            const_4 = 4,
            const_8 = 8
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DEVICE_DESCRIPTOR
        {
            public int Version;
            public int Size;
            public byte DeviceType;
            public byte DeviceTypeModifier;
            public byte RemovableMedia;
            public byte CommandQueueing;
            public int VendorIdOffset;
            public int ProductIdOffset;
            public int ProductRevisionOffset;
            public int SerialNumberOffset;
            public Hardware.STORAGE_BUS_TYPE BusType;
            public int RawPropertiesLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x2800)]
            public byte[] RawDeviceProperties;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DEVICE_ID_DESCRIPTOR
        {
            public int Version;
            public int Size;
            public int NumberOfIdentifiers;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x2800)]
            public byte[] Identifiers;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct0
        {
            public uint uint_0;
            public uint uint_1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
            public byte[] byte_0;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct VOLUMEDISKEXTENTS
        {
            [FieldOffset(8)]
            public uint diskNumber;
            [FieldOffset(0x18)]
            public long extentLength;
            [FieldOffset(0)]
            public uint numberOfDiskExtents;
            [FieldOffset(0x10)]
            public long startingOffset;
        }
    }
}

