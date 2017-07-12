using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using JSON;

namespace RustProtect
{
	public class Hardware
	{
		private static class Class1
		{
			[DllImport("USER32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			public static extern IntPtr CallWindowProcW([In] byte[] byte_0, IntPtr intptr_0, int int_0, [In] [Out] byte[] byte_1, IntPtr intptr_1);

			[DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool VirtualProtect([In] byte[] byte_0, IntPtr intptr_0, int int_0, out int int_1);

			[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
			public static extern SafeFileHandle CreateFileW([MarshalAs(UnmanagedType.LPWStr)] string string_0, uint uint_0, uint uint_1, IntPtr intptr_0, uint uint_2, uint uint_3, IntPtr intptr_1);

			[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool GetVolumeNameForVolumeMountPoint(string string_0, StringBuilder stringBuilder_0, uint uint_0);

            [DllImport("KERNEL32.DLL", SetLastError = true)]
            public static extern bool DeviceIoControl(SafeFileHandle safeFileHandle_0, uint uint_0, [In, MarshalAs(UnmanagedType.AsAny)] object object_0, int int_0, [Out] IntPtr intptr_0, [Out] int int_1, ref uint uint_1, int int_2);


            [DllImport("KERNEL32.DLL", EntryPoint = "DeviceIoControl", SetLastError = true)]
			public static extern bool DeviceIoControl_1(SafeFileHandle safeFileHandle_0, uint uint_0, IntPtr intptr_0, int int_0, [Out] IntPtr intptr_1, [Out] int int_1, ref uint uint_1, int int_2);

			[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall)]
			public static extern int CloseHandle(int int_0);

			[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
			public static extern int EnumSystemFirmwareTables(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0, IntPtr intptr_0, int int_0);

			[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
			public static extern int GetSystemFirmwareTable(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0, int int_0, IntPtr intptr_0, int int_1);

			[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
			public static extern bool EnumDisplayDevices(string string_0, uint uint_0, ref Hardware.DISPLAY_DEVICE display_DEVICE_0, uint uint_1);
		}

		public enum STORAGE_BUS_TYPE : byte
		{
			BusTypeUnknown,
			BusTypeScsi,
			BusTypeAtapi,
			BusTypeAta,
			const_4,
			BusTypeSsa,
			BusTypeFibre,
			BusTypeUsb,
			const_8,
			BusTypeiScsi,
			BusTypeSas,
			BusTypeSata,
			BusTypeSd,
			BusTypeMmc,
			BusTypeVirtual,
			BusTypeFileBackedVirtual,
			BusTypeMax,
			BusTypeMaxReserved = 127
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct VOLUMEDISKEXTENTS
		{
			[FieldOffset(0)]
			public uint numberOfDiskExtents;

			[FieldOffset(8)]
			public uint diskNumber;

			[FieldOffset(16)]
			public long startingOffset;

			[FieldOffset(24)]
			public long extentLength;
		}

		private struct Struct0
		{
			public uint uint_0;

			public uint uint_1;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public byte[] byte_0;
		}

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

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10240)]
			public byte[] RawDeviceProperties;
		}

		public struct STORAGE_DEVICE_ID_DESCRIPTOR
		{
			public int Version;

			public int Size;

			public int NumberOfIdentifiers;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10240)]
			public byte[] Identifiers;
		}

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

		public struct GETVERSIONOUTPARAMS
		{
			public byte bVersion;

			public byte bRevision;

			public byte bReserved;

			public byte bIDEDeviceMap;

			public int fCapabilities;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public int[] dwReserved;
		}

		[StructLayout(LayoutKind.Sequential, Size = 8)]
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

		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct SENDCMDINPARAMS
		{
			public int BufferSize;

			public Hardware.IDEREGS DriveRegs;

			public byte DriveNumber;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] bReserved;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public int[] dwReserved;
		}

		[StructLayout(LayoutKind.Sequential, Size = 12)]
		public struct DRIVERSTATUS
		{
			public byte DriveError;

			public byte byte_0;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] bReserved;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] dwReserved;
		}

		public struct IDSECTOR
		{
			public short GenConfig;

			public short NumberCylinders;

			public short Reserved;

			public short NumberHeads;

			public short BytesPerTrack;

			public short BytesPerSector;

			public short SectorsPerTrack;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public short[] VendorUnique;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			public byte[] SerialNumber;

			public short BufferClass;

			public short BufferSize;

			public short ECCSize;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] FirmwareRevision;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
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

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 382)]
			public byte[] bReserved;
		}

		public struct SENDCMDOUTPARAMS
		{
			public uint cBufferSize;

			public Hardware.DRIVERSTATUS Status;

			public Hardware.IDSECTOR IDS;
		}

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

		public enum BiosFirmwareTableProvider
		{
			ACPI = 1094930505,
			FIRM = 1179210317,
			RSMB = 1381190978
		}

		public enum SMBIOSTableType : sbyte
		{
			BIOSInformation,
			SystemInformation,
			BaseBoardInformation,
			EnclosureInformation,
			ProcessorInformation,
			MemoryControllerInformation,
			MemoryModuleInformation,
			CacheInformation,
			PortConnectorInformation,
			SystemSlotsInformation,
			OnBoardDevicesInformation,
			const_11,
			SystemConfigurationOptions,
			BIOSLanguageInformation,
			GroupAssociations,
			SystemEventLog,
			PhysicalMemoryArray,
			MemoryDevice,
			MemoryErrorInformation,
			MemoryArrayMappedAddress,
			MemoryDeviceMappedAddress,
			EndofTable = 127
		}

		public struct SMBIOSTableHeader
		{
			public Hardware.SMBIOSTableType type;

			public byte length;

			public ushort Handle;
		}

		public struct SMBIOSTableEntry
		{
			public Hardware.SMBIOSTableHeader header;

			public uint index;
		}

		public struct GStruct0
		{
			public Hardware.SMBIOSTableHeader header;

			public byte vendor;

			public byte version;

			public ushort startingSegment;

			public byte releaseDate;

			public byte biosRomSize;

			public ulong characteristics;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] extensionBytes;
		}

		public struct SMBIOSTableSystemInfo
		{
			public Hardware.SMBIOSTableHeader header;

			public byte manufacturer;

			public byte productName;

			public byte version;

			public byte serialNumber;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] UUID;

			public byte wakeUpType;
		}

		public struct SMBIOSTableBaseBoardInfo
		{
			public Hardware.SMBIOSTableHeader header;

			public byte manufacturer;

			public byte productName;

			public byte version;

			public byte serialNumber;
		}

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

		public enum DisplayDeviceStateFlags
		{
			AttachedToDesktop = 1,
			MultiDriver,
			PrimaryDevice = 4,
			MirroringDriver = 8,
			VGACompatible = 16,
			Removable = 32,
			ModesPruned = 134217728,
			Remote = 67108864,
			Disconnect = 33554432
		}

		public struct DISPLAY_DEVICE
		{
			[MarshalAs(UnmanagedType.U4)]
			public int structSize;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceString;

			[MarshalAs(UnmanagedType.U4)]
			public Hardware.DisplayDeviceStateFlags StateFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceID;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceKey;
		}

		private static byte[] byte_0 = null;

		private static byte[] byte_1 = null;

		private static byte[] byte_2 = null;

		private static byte[] byte_3 = null;

		private static string string_0 = DateTime.Now.ToString(Class3.smethod_10(292));

		public byte[] MD5
		{
			get
			{
				if (Hardware.byte_0 != null && Hardware.byte_0.Length >= 16)
				{
					return Hardware.byte_0;
				}
				return new byte[0];
			}
		}

		public byte[] SHA1
		{
			get
			{
				if (Hardware.byte_1 != null && Hardware.byte_1.Length >= 16)
				{
					return Hardware.byte_1;
				}
				return new byte[0];
			}
		}

		public byte[] SHA256
		{
			get
			{
				if (Hardware.byte_2 != null && Hardware.byte_2.Length >= 16)
				{
					return Hardware.byte_2;
				}
				return new byte[0];
			}
		}

		public byte[] SHA512
		{
			get
			{
				if (Hardware.byte_3 != null && Hardware.byte_3.Length >= 16)
				{
					return Hardware.byte_3;
				}
				return new byte[0];
			}
		}

		public string String_0
		{
			get
			{
				if (Hardware.byte_0 != null && Hardware.byte_0.Length >= 16)
				{
					return BitConverter.ToString(Hardware.byte_0, 0).Replace(Class3.smethod_10(286), "").ToLower();
				}
				return "";
			}
		}

		public string String_1
		{
			get
			{
				if (Hardware.byte_1 != null && Hardware.byte_1.Length >= 16)
				{
					return BitConverter.ToString(Hardware.byte_1, 0).Replace(Class3.smethod_10(286), "").ToLower();
				}
				return "";
			}
		}

		public string SHA256String
		{
			get
			{
				if (Hardware.byte_2 != null && Hardware.byte_2.Length >= 16)
				{
					return BitConverter.ToString(Hardware.byte_2, 0).Replace(Class3.smethod_10(286), "").ToLower();
				}
				return "";
			}
		}

		public string SHA512String
		{
			get
			{
				if (Hardware.byte_3 != null && Hardware.byte_3.Length >= 16)
				{
					return BitConverter.ToString(Hardware.byte_3, 0).Replace(Class3.smethod_10(286), "").ToLower();
				}
				return "";
			}
		}

		private uint method_0(uint uint_0, uint uint_1, uint uint_2, uint uint_3)
		{
			return uint_0 << 16 | uint_3 << 14 | uint_1 << 2 | uint_2;
		}

		private string method_1(char[] char_0)
		{
			for (int i = 0; i <= char_0.Length - 2; i += 2)
			{
				char c = char_0[i];
				char_0[i] = char_0[i + 1];
				char_0[i + 1] = c;
			}
			return new string(char_0);
		}

		private string method_2(byte[] byte_4)
		{
			for (int i = 0; i <= byte_4.Length - 2; i += 2)
			{
				byte b = byte_4[i];
				byte_4[i] = byte_4[i + 1];
				byte_4[i + 1] = b;
			}
			return Encoding.ASCII.GetString(byte_4);
		}

		private string method_3(string string_1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < string_1.Length; i += 4)
			{
				stringBuilder.Append(Convert.ToChar(Convert.ToByte(string_1.Substring(i + 2, 2), 16)));
				stringBuilder.Append(Convert.ToChar(Convert.ToByte(string_1.Substring(i, 2), 16)));
			}
			return stringBuilder.ToString().Trim(new char[]
			{
				'\0',
				' '
			});
		}
        /*
		private Hardware.PhysicalDisk method_4(string string_1)
		{
			Hardware.PhysicalDisk physicalDisk = default(Hardware.PhysicalDisk);
			physicalDisk.Extents = default(Hardware.VOLUMEDISKEXTENTS);
			physicalDisk.Adapter = default(Hardware.STORAGE_ADAPTER_DESCRIPTOR);
			physicalDisk.DeviceID = default(Hardware.STORAGE_DEVICE_ID_DESCRIPTOR);
			physicalDisk.Device = default(Hardware.STORAGE_DEVICE_DESCRIPTOR);
			physicalDisk.Version = default(Hardware.GETVERSIONOUTPARAMS);
			physicalDisk.Params = default(Hardware.SENDCMDOUTPARAMS);
			physicalDisk.SerialNumber = string.Empty;
			physicalDisk.Firmware = string.Empty;
			physicalDisk.Model = string.Empty;
			string str = Path.GetPathRoot(string_1).TrimEnd(new char[]
			{
				'\\'
			});
			using (SafeFileHandle safeFileHandle = Hardware.Class1.CreateFileW(Class3.smethod_10(0) + str, 3221225472u, 3u, IntPtr.Zero, 3u, 128u, IntPtr.Zero))
			{
				try
				{
					uint num = 0u;
					int num2 = Marshal.SizeOf(physicalDisk.Extents);
					IntPtr intPtr = Marshal.AllocHGlobal(num2);
					Hardware.Struct0 @struct = default(Hardware.Struct0);
					uint uint_ = this.method_0(86u, 0u, 0u, 0u);
					if (Hardware.Class1.DeviceIoControl_1(safeFileHandle, uint_, IntPtr.Zero, 0, intPtr, num2, ref num, 0))
					{
						physicalDisk.Extents = (Hardware.VOLUMEDISKEXTENTS)Marshal.PtrToStructure(intPtr, typeof(Hardware.VOLUMEDISKEXTENTS));
						physicalDisk.Number = Convert.ToByte(physicalDisk.Extents.diskNumber);
					}
					num2 = Marshal.SizeOf(physicalDisk.Adapter);
					intPtr = Marshal.AllocHGlobal(num2);
					@struct.uint_0 = 1u;
					@struct.uint_1 = 0u;
					if (Hardware.Class1.DeviceIoControl(safeFileHandle, 2954240u, @struct, Marshal.SizeOf(@struct), intPtr, num2, ref num, 0))
					{
						physicalDisk.Adapter = (Hardware.STORAGE_ADAPTER_DESCRIPTOR)Marshal.PtrToStructure(intPtr, typeof(Hardware.STORAGE_ADAPTER_DESCRIPTOR));
					}
					num2 = Marshal.SizeOf(physicalDisk.DeviceID);
					intPtr = Marshal.AllocHGlobal(num2);
					@struct.uint_0 = 2u;
					@struct.uint_1 = 0u;
					if (Hardware.Class1.DeviceIoControl(safeFileHandle, 2954240u, @struct, Marshal.SizeOf(@struct), intPtr, num2, ref num, 0))
					{
						physicalDisk.DeviceID = (Hardware.STORAGE_DEVICE_ID_DESCRIPTOR)Marshal.PtrToStructure(intPtr, typeof(Hardware.STORAGE_DEVICE_ID_DESCRIPTOR));
					}
					num2 = Marshal.SizeOf(physicalDisk.Device);
					intPtr = Marshal.AllocHGlobal(num2);
					@struct.uint_0 = 0u;
					@struct.uint_1 = 0u;
					if (Hardware.Class1.DeviceIoControl(safeFileHandle, 2954240u, @struct, Marshal.SizeOf(@struct), intPtr, num2, ref num, 0))
					{
						physicalDisk.Device = (Hardware.STORAGE_DEVICE_DESCRIPTOR)Marshal.PtrToStructure(intPtr, typeof(Hardware.STORAGE_DEVICE_DESCRIPTOR));
						string @string = Encoding.ASCII.GetString(physicalDisk.Device.RawDeviceProperties);
						int num3 = Marshal.SizeOf(physicalDisk.Device) - physicalDisk.Device.RawDeviceProperties.Length;
						if (physicalDisk.Device.ProductIdOffset > 0)
						{
							physicalDisk.Model = @string.Substring(physicalDisk.Device.ProductIdOffset - num3, 20).Trim(new char[]
							{
								'\0',
								' '
							});
						}
						if (physicalDisk.Device.ProductRevisionOffset > 0)
						{
							physicalDisk.Firmware = @string.Substring(physicalDisk.Device.ProductRevisionOffset - num3, 8).Trim(new char[]
							{
								'\0',
								' '
							});
						}
						if (physicalDisk.Device.SerialNumberOffset > 0)
						{
							physicalDisk.SerialNumber = @string.Substring(physicalDisk.Device.SerialNumberOffset - num3, 40).Trim(new char[]
							{
								'\0',
								' '
							});
						}
						if (physicalDisk.SerialNumber != null && physicalDisk.SerialNumber.Length == 40)
						{
							physicalDisk.SerialNumber = this.method_3(physicalDisk.SerialNumber);
						}
						physicalDisk.RemovableMedia = Convert.ToBoolean(physicalDisk.Device.RemovableMedia);
					}
					num2 = Marshal.SizeOf(physicalDisk.Version);
					intPtr = Marshal.AllocHGlobal(num2);
					if (Hardware.Class1.DeviceIoControl_1(safeFileHandle, 475264u, IntPtr.Zero, 0, intPtr, num2, ref num, 0))
					{
						physicalDisk.Version = (Hardware.GETVERSIONOUTPARAMS)Marshal.PtrToStructure(intPtr, typeof(Hardware.GETVERSIONOUTPARAMS));
						if (((long)physicalDisk.Version.fCapabilities & 4L) > 0L)
						{
							Hardware.SENDCMDINPARAMS sENDCMDINPARAMS = default(Hardware.SENDCMDINPARAMS);
							sENDCMDINPARAMS.DriveRegs.Command = 236;
							sENDCMDINPARAMS.DriveNumber = physicalDisk.Number;
							sENDCMDINPARAMS.BufferSize = 512;
							if (Hardware.Class1.DeviceIoControl(safeFileHandle, 508040u, sENDCMDINPARAMS, Marshal.SizeOf(sENDCMDINPARAMS), intPtr, num2, ref num, 0))
							{
								physicalDisk.Params = (Hardware.SENDCMDOUTPARAMS)Marshal.PtrToStructure(intPtr, typeof(Hardware.SENDCMDOUTPARAMS));
								physicalDisk.Model = this.method_2(physicalDisk.Params.IDS.ModelNumber).Trim();
								physicalDisk.Firmware = this.method_2(physicalDisk.Params.IDS.FirmwareRevision).Trim();
								physicalDisk.SerialNumber = this.method_2(physicalDisk.Params.IDS.SerialNumber).Trim();
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return physicalDisk;
		}*/
        private PhysicalDisk method_4(string string_1)
        {
            PhysicalDisk disk = new PhysicalDisk
            {
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
                    disk.Extents = (VOLUMEDISKEXTENTS)Marshal.PtrToStructure(ptr, typeof(VOLUMEDISKEXTENTS));
                    disk.Number = Convert.ToByte(disk.Extents.diskNumber);
                }
                cb = Marshal.SizeOf(disk.Adapter);
                ptr = Marshal.AllocHGlobal(cb);
                struct2.uint_0 = 1;
                struct2.uint_1 = 0;
                if (Class1.DeviceIoControl(handle, 0x2d1400, struct2, Marshal.SizeOf(struct2), ptr, cb, ref num, 0))
                {
                    disk.Adapter = (STORAGE_ADAPTER_DESCRIPTOR)Marshal.PtrToStructure(ptr, typeof(STORAGE_ADAPTER_DESCRIPTOR));
                }
                cb = Marshal.SizeOf(disk.DeviceID);
                ptr = Marshal.AllocHGlobal(cb);
                struct2.uint_0 = 2;
                struct2.uint_1 = 0;
                if (Class1.DeviceIoControl(handle, 0x2d1400, struct2, Marshal.SizeOf(struct2), ptr, cb, ref num, 0))
                {
                    disk.DeviceID = (STORAGE_DEVICE_ID_DESCRIPTOR)Marshal.PtrToStructure(ptr, typeof(STORAGE_DEVICE_ID_DESCRIPTOR));
                }
                cb = Marshal.SizeOf(disk.Device);
                ptr = Marshal.AllocHGlobal(cb);
                struct2.uint_0 = 0;
                struct2.uint_1 = 0;
                if (Class1.DeviceIoControl(handle, 0x2d1400, struct2, Marshal.SizeOf(struct2), ptr, cb, ref num, 0))
                {
                    disk.Device = (STORAGE_DEVICE_DESCRIPTOR)Marshal.PtrToStructure(ptr, typeof(STORAGE_DEVICE_DESCRIPTOR));
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
                    disk.Version = (GETVERSIONOUTPARAMS)Marshal.PtrToStructure(ptr, typeof(GETVERSIONOUTPARAMS));
                    if ((disk.Version.fCapabilities & 4L) > 0L)
                    {
                        SENDCMDINPARAMS sendcmdinparams = new SENDCMDINPARAMS();
                        sendcmdinparams.DriveRegs.Command = 0xec;
                        sendcmdinparams.DriveNumber = disk.Number;
                        sendcmdinparams.BufferSize = 0x200;
                        if (Class1.DeviceIoControl(handle, 0x7c088, sendcmdinparams, Marshal.SizeOf(sendcmdinparams), ptr, cb, ref num, 0))
                        {
                            disk.Params = (SENDCMDOUTPARAMS)Marshal.PtrToStructure(ptr, typeof(SENDCMDOUTPARAMS));
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




        private static byte[] smethod_0(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0, string string_1)
		{
			int int_ = (int)((int)string_1[3] << 24 | (int)string_1[2] << 16 | (int)string_1[1] << 8 | string_1[0]);
			return Hardware.smethod_1(biosFirmwareTableProvider_0, int_);
		}

		private static byte[] smethod_1(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0, int int_0)
		{
			byte[] array = new byte[0];
			try
			{
				int systemFirmwareTable = Hardware.Class1.GetSystemFirmwareTable(biosFirmwareTableProvider_0, int_0, IntPtr.Zero, 0);
				if (systemFirmwareTable > 0)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(systemFirmwareTable);
					Hardware.Class1.GetSystemFirmwareTable(biosFirmwareTableProvider_0, int_0, intPtr, systemFirmwareTable);
					if (Marshal.GetLastWin32Error() == 0)
					{
						array = new byte[systemFirmwareTable];
						Marshal.Copy(intPtr, array, 0, systemFirmwareTable);
					}
					Marshal.FreeHGlobal(intPtr);
				}
			}
			catch
			{
			}
			return array;
		}

		private static string[] smethod_2(Hardware.BiosFirmwareTableProvider biosFirmwareTableProvider_0)
		{
			string[] array = new string[0];
			try
			{
				int num = Hardware.Class1.EnumSystemFirmwareTables(biosFirmwareTableProvider_0, IntPtr.Zero, 0);
				if (num > 0)
				{
					byte[] array2 = new byte[num];
					IntPtr intPtr = Marshal.AllocHGlobal(num);
					Hardware.Class1.EnumSystemFirmwareTables(biosFirmwareTableProvider_0, intPtr, num);
					if (Marshal.GetLastWin32Error() == 0)
					{
						array = new string[num / 4];
						Marshal.Copy(intPtr, array2, 0, num);
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = Encoding.ASCII.GetString(array2, 4 * i, 4);
						}
					}
					Marshal.FreeHGlobal(intPtr);
				}
			}
			catch
			{
			}
			return array;
		}

		private static string[] smethod_3(byte[] byte_4, Hardware.SMBIOSTableEntry smbiostableEntry_0, ref int int_0)
		{
			List<string> list = new List<string>();
			int_0 += (int)smbiostableEntry_0.header.length;
			int num;
			do
			{
				string @string = byte_4.GetString(int_0);
				list.Add(@string);
				int_0 += @string.Length;
				num = int_0 + 1;
				int_0 = num;
			}
			while (byte_4[num] != 0);
			int_0++;
			return list.ToArray();
		}

		public Hardware()
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream()))
			{
				if (ProtectLoader.Debug)
				{
					UnityEngine.Debug.Log(Class3.smethod_10(12));
				}
				string[] array = Hardware.smethod_2(Hardware.BiosFirmwareTableProvider.RSMB);
				if (array.Length != 0)
				{
					byte[] array2 = Hardware.smethod_0(Hardware.BiosFirmwareTableProvider.RSMB, array[0]);
					int num = array2.Length;
					if (num != 0)
					{
						if (num > 1024)
						{
							num = 1024;
						}
						binaryWriter.Write(array2, 0, num);
						if (ProtectLoader.Debug)
						{
							UnityEngine.Debug.Log(Class3.smethod_10(68));
						}
						byte[] array3 = new byte[]
						{
							85,
							137,
							229,
							87,
							139,
							125,
							16,
							106,
							1,
							88,
							83,
							15,
							162,
							137,
							7,
							137,
							87,
							4,
							91,
							95,
							137,
							236,
							93,
							194,
							16,
							0
						};
						byte[] array4 = new byte[]
						{
							83,
							72,
							199,
							192,
							1,
							0,
							0,
							0,
							15,
							162,
							65,
							137,
							0,
							65,
							137,
							80,
							4,
							91,
							195
						};
						byte[] array5 = new byte[8];
						byte[] array6;
						if (IntPtr.Size == 8)
						{
							array6 = array4;
						}
						else
						{
							array6 = array3;
						}
						IntPtr intPtr = new IntPtr(array6.Length);
						int num2;
						if (!Hardware.Class1.VirtualProtect(array6, intPtr, 64, out num2))
						{
							Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
						}
						intPtr = new IntPtr(array5.Length);
						if (!(Hardware.Class1.CallWindowProcW(array6, IntPtr.Zero, 0, array5, intPtr) == IntPtr.Zero))
						{
							if (ProtectLoader.Debug)
							{
								UnityEngine.Debug.Log(Class3.smethod_10(118));
							}
							binaryWriter.Write(SystemInfo.deviceName);
							binaryWriter.Write(SystemInfo.deviceModel);
							binaryWriter.Write(SystemInfo.graphicsDeviceName);
							binaryWriter.Write(SystemInfo.graphicsDeviceID);
							binaryWriter.Write(SystemInfo.graphicsDeviceVendorID);
							binaryWriter.Write(SystemInfo.graphicsMemorySize);
							if (ProtectLoader.Debug)
							{
								UnityEngine.Debug.Log(Class3.smethod_10(166));
							}
							binaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
							Hardware.byte_0 = new MD5CryptoServiceProvider().ComputeHash(binaryWriter.BaseStream);
							binaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
							Hardware.byte_1 = new SHA1CryptoServiceProvider().ComputeHash(binaryWriter.BaseStream);
							binaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
							Hardware.byte_2 = new SHA256CryptoServiceProvider().ComputeHash(binaryWriter.BaseStream);
							binaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
							Hardware.byte_3 = new SHA512CryptoServiceProvider().ComputeHash(binaryWriter.BaseStream);
							if (ProtectLoader.Debug)
							{
								UnityEngine.Debug.Log(Class3.smethod_10(214));
							}
						}
					}
				}
			}
		}
	}
}
