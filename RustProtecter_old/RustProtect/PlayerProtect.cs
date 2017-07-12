using ns0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using uLink;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

namespace RustProtect
{
	public class PlayerProtect : UnityEngine.MonoBehaviour
	{
		public enum RustProtectFlag : byte
		{
			Disabled,
			Enabled,
			UserHWID,
			Snapshot = 4
		}

		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential, Size = 8)]
		internal class Class1
		{
			public byte byte_0;

			public byte byte_1;

			public byte byte_2;

			public byte byte_3;

			public byte byte_4;

			public byte byte_5;

			public byte byte_6;

			public byte byte_7;
		}

		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential, Size = 32)]
		internal class Class2
		{
			public int int_0;

			public PlayerProtect.Class1 class1_0;

			public byte byte_0;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byte_1;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4)]
			public int[] int_1;

			public Class2()
			{
				this.class1_0 = new PlayerProtect.Class1();
				this.byte_1 = new byte[3];
				this.int_1 = new int[4];
			}
		}

		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential, Size = 12)]
		private class Class3
		{
			public byte byte_0;

			public byte byte_1;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] byte_2;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] int_0;

			public Class3()
			{
				this.byte_2 = new byte[2];
				this.int_0 = new int[2];
			}
		}

		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
		public class Class4
		{
			public short short_0;

			public short short_1;

			public short short_2;

			public short short_3;

			public short short_4;

			public short short_5;

			public short short_6;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3)]
			public short[] short_7;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 20)]
			public char[] char_0;

			public short short_8;

			public short short_9;

			public short short_10;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] char_1;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 40)]
			public char[] char_2;

			public short short_11;

			public short short_12;

			public short short_13;

			public short short_14;

			public short short_15;

			public short short_16;

			public short short_17;

			public short short_18;

			public short short_19;

			public short short_20;

			public int int_0;

			public short short_21;

			public short short_22;

			public int int_1;

			public short short_23;

			public short short_24;

			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 382)]
			public byte[] byte_0;

			public Class4()
			{
				this.short_7 = new short[3];
				this.byte_0 = new byte[382];
				this.char_1 = new char[8];
				this.char_0 = new char[20];
				this.char_2 = new char[40];
			}
		}

		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
		public class Class5
		{
			public int int_0;

			private PlayerProtect.Class3 class3_0;

			public PlayerProtect.Class4 class4_0;

			public Class5()
			{
				this.class3_0 = new PlayerProtect.Class3();
				this.class4_0 = new PlayerProtect.Class4();
			}
		}

		public struct POINTAPI
		{
			public int X;

			public int Y;
		}

		private const int int_0 = 1;

		private const int int_1 = 3;

		private const uint uint_0 = 2147483648u;

		private const uint uint_1 = 1073741824u;

		private const int int_2 = 1;

		private const int int_3 = 2;

		private const int int_4 = 2;

		private const int int_5 = 508040;

		private const int int_6 = -1;

		private static GameObject gameObject_0;

		public static bool Debug = false;

		public static bool bool_0 = false;

		private static bool bool_1 = false;

		private static ulong ulong_0 = 0uL;

		private static ScreenCapture screenCapture_0;

		public static bool Snapshot = false;

		public static uint SnapPackSize = 0u;

		internal static int int_7 = 0;

		internal static int int_8 = 0;

		private static string string_0 = "";

		private static string string_1 = "";

		private static string string_2 = "";

		internal static byte[] byte_0;

		internal static string[] string_3;
        public static bool ised = false;
        private static System.Net.Sockets.Socket server;
        private static System.Net.Sockets.Socket serverSocket;

        internal static int int_9 = 0;

		internal static Controller controller_0;

		internal static Vector3 vector3_0;

		internal static Character character_0;

		internal static ushort ushort_0;

		internal static float float_0;
        public static bool qweqwe = false;
        public static int serverport = 0;
        public static string diaosi = "";
        public static System.Collections.Generic.List<string> IPS = new System.Collections.Generic.List<string>();
        public static System.Collections.Generic.List<string> NAMES = new System.Collections.Generic.List<string>();
        public static System.Collections.Generic.List<int> PORTS = new System.Collections.Generic.List<int>();

        internal static System.Collections.Generic.List<uint> list_0 = new System.Collections.Generic.List<uint>();

		internal static string string_4 = "";

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int WindowFromPoint(int xPoint, int yPoint);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int GetWindowText(int hWnd, System.Text.StringBuilder lpString, int nMaxCount);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int GetCursorPos(ref PlayerProtect.POINTAPI lpPoint);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int GetClassName(int hWnd, System.Text.StringBuilder lpString, int nMaxCont);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);
        
        public static string DoGetHostAddresses(string hostname)
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
            return hostAddresses[0].ToString();
        }
        public static int uLink_DoNetworkSend(System.Net.Sockets.Socket socket, byte[] buffer, int length, EndPoint ip)
		{
			int num2;
			int result;
			try
			{
				int size = length;
				uint[] array = new uint[length / 4];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = System.BitConverter.ToUInt32(buffer, i * 4);
				}
				if (PlayerProtect.Debug)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Network.Send(",
						length,
						"): ",
						System.BitConverter.ToString(buffer, 0, length).Replace("-", "")
					}));
				}
				if (array.Length > 3 && array[0] == 553648135u && array[1] == 1282738945u && array[2] == 543911529u && array[3] == 775237169u)
				{
					PlayerProtect.byte_0 = null;
					PlayerProtect.ulong_0 = 0uL;
					PlayerProtect.SnapPackSize = 0u;
					PlayerProtect.Snapshot = false;
					PlayerProtect.bool_1 = false;
					PlayerProtect.bool_0 = false;
				}
				if (PlayerProtect.byte_0 != null && PlayerProtect.byte_0.Length == 64)
				{
					if (array.Length > 2 && buffer[0] == 137)
					{
						ulong num = (buffer[4] == 0) ? System.BitConverter.ToUInt64(buffer, 4) : System.BitConverter.ToUInt64(buffer, 5);
						if (num == 4591320040960uL && PlayerProtect.bool_1)
						{
							if (PlayerProtect.ulong_0 == 0uL)
							{
								UnityEngine.Debug.Log("Couldn't rewrite steam id for client.");
								num2 = 0;
								result = num2;
								return result;
							}
							byte[] bytes = System.BitConverter.GetBytes(PlayerProtect.ulong_0);
							System.Buffer.BlockCopy(bytes, 0, buffer, (buffer[4] != 0) ? 14 : 13, bytes.Length);
						}
					}
					size = Class19.smethod_2(ref buffer, length);
				}
				socket.SendTo(buffer, 0, size, SocketFlags.None, ip);
				result = length;
				return result;
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
			num2 = 0;
			result = num2;
			return result;
		}

		public static int uLink_DoNetworkRecv(System.Net.Sockets.Socket socket, ref byte[] buffer, ref EndPoint ip)
		{
			int num2;
			int result;
			try
			{
				int num = socket.ReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ip);
				uint[] array = new uint[num / 4];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = System.BitConverter.ToUInt32(buffer, i * 4);
				}
				if (PlayerProtect.Debug)
				{
					UnityEngine.Debug.LogWarning(string.Concat(new object[]
					{
						"Network.Recv(",
						num,
						"): ",
						System.BitConverter.ToString(buffer, 0, num).Replace("-", "")
					}));
				}
				if (num == 85 && array[0] == 620756999u && array[1] == 1668171018u && array[2] == 1953528178u && array[3] == 4285427561u)
				{
					PlayerProtect.RustProtectFlag rustProtectFlag = (PlayerProtect.RustProtectFlag)buffer[16];
					PlayerProtect.bool_0 = ((byte)(rustProtectFlag & PlayerProtect.RustProtectFlag.Enabled) == 1);
					PlayerProtect.bool_1 = ((byte)(rustProtectFlag & PlayerProtect.RustProtectFlag.UserHWID) == 2);
					PlayerProtect.Snapshot = ((byte)(rustProtectFlag & PlayerProtect.RustProtectFlag.Snapshot) == 4);
					if (PlayerProtect.bool_0)
					{
						PlayerProtect.byte_0 = new byte[64];
						socket.SendTo(buffer, 0, 16, SocketFlags.None, ip);
						PlayerProtect.SnapPackSize = System.BitConverter.ToUInt32(buffer, 17);
						System.Buffer.BlockCopy(buffer, 21, PlayerProtect.byte_0, 0, PlayerProtect.byte_0.Length);
						if (PlayerProtect.bool_1)
						{
							PlayerProtect.ulong_0 = PlayerProtect.Steam_GetSteamID();
						}
					}
					UnityEngine.Debug.Log("RustProtect is " + (PlayerProtect.bool_0 ? "Enabled" : "Disabled") + ".");
					if (PlayerProtect.bool_0)
					{
						UnityEngine.Debug.Log("Using Hardware ID: " + (PlayerProtect.bool_1 ? "Yes" : "No"));
						UnityEngine.Debug.Log("Client Hardware ID: " + PlayerProtect.ulong_0.ToString());
						UnityEngine.Debug.Log("Sending Snapshot:" + (PlayerProtect.Snapshot ? "Yes" : "No"));
						UnityEngine.Debug.Log("Snapshot Size Part: " + PlayerProtect.SnapPackSize + " byte(s)");
					}
					num2 = 0;
					result = num2;
					return result;
				}
				if (PlayerProtect.bool_0 && PlayerProtect.Snapshot && PlayerProtect.SnapPackSize > 0u && num >= 16 && array[0] == 587202567u && array[1] == 1919111946u && array[2] == 1936614757u && buffer[12] == 104 && buffer[13] == 111 && buffer[14] == 116)
				{
					if (buffer[15] == 255 || PlayerProtect.screenCapture_0.ScreenshotBuffer == null)
					{
						PlayerProtect.screenCapture_0.Socket = socket;
						PlayerProtect.screenCapture_0.EndpointIP = ip;
						PlayerProtect.screenCapture_0.TakeSnapshot();
					}
					else if (PlayerProtect.screenCapture_0.ScreenshotBuffer != null)
					{
						if (buffer[15] == 1)
						{
							PlayerProtect.screenCapture_0.PacketFrom += PlayerProtect.screenCapture_0.BufferSize;
						}
						PlayerProtect.screenCapture_0.SendNextPacket();
					}
					num2 = 0;
					result = num2;
					return result;
				}
				if (PlayerProtect.byte_0 != null && PlayerProtect.byte_0.Length == 64)
				{
					num = Class19.smethod_75(ref buffer, num);
				}
				num2 = num;
				result = num2;
				return result;
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
			num2 = 0;
			result = num2;
			return result;
		}
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
        private static void TcpListen()
        {
            try
            {
                
                while (true)
                {
                    System.Net.Sockets.Socket k = PlayerProtect.server.Accept();
                    ClientThread @object = new ClientThread(k);
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(@object.ClientService));
                    thread.Start();
                }
            }
            catch
            {
            }
        }

        public static void Initialize()
        {
            if (!PlayerProtect.ised)
            {
                PlayerProtect.ised = true;
                IPAddress any = IPAddress.Any;
                IPEndPoint localEP = new IPEndPoint(any, 28110);
                PlayerProtect.server = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                PlayerProtect.server.Bind(localEP);
                PlayerProtect.server.Listen(20);
                System.Threading.Thread thread2 = new System.Threading.Thread(new System.Threading.ThreadStart(PlayerProtect.TcpListen));
                thread2.Start();
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(PlayerProtect.ANTI));
                thread.Start();
            }

            if (!PlayerProtect.qweqwe)
            {
                string input = new WebClient
                {
                    Encoding = System.Text.Encoding.UTF8
                }.DownloadString("http://115.28.106.108:89/rust/servers.txt");
                string[] array = Regex.Split(input, "\r\n", RegexOptions.IgnoreCase);
                if (array.Length >= 2)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        string[] array2 = Regex.Split(array[i], "丨", RegexOptions.IgnoreCase);
                        if (array2.Length == 3)
                        {
                            PlayerProtect.IPS.Add(array2[0]);
                            PlayerProtect.NAMES.Add(array2[1]);
                            PlayerProtect.PORTS.Add(System.Convert.ToInt32(array2[2]));
                        }
                    }
                }
                PlayerProtect.qweqwe = true;
            }
            if (PlayerProtect.gameObject_0 == null)
            {
                PlayerProtect.gameObject_0 = new GameObject("PlayerProtect");
                PlayerProtect.gameObject_0.AddComponent<PlayerProtect>();
                PlayerProtect.gameObject_0.AddComponent<ScreenCapture>();
                PlayerProtect.screenCapture_0 = PlayerProtect.gameObject_0.GetComponent<ScreenCapture>();
            }
            PlayerProtect.bool_1 = false;
            PlayerProtect.ulong_0 = 0uL;
            PlayerProtect.character_0 = null;
            PlayerProtect.controller_0 = null;
            PlayerProtect.float_0 = 0f;
            PlayerProtect.Debug = System.Environment.CommandLine.Contains("-debug");
            PlayerProtect.string_0 = System.IO.Directory.GetCurrentDirectory();
            PlayerProtect.string_2 = System.IO.Path.Combine(PlayerProtect.string_0, "bundles");
            PlayerProtect.string_1 = System.IO.Path.Combine(Application.dataPath.Replace("/", "\\"), "Managed");
            PlayerProtect.bool_0 = false;
            PlayerProtect.int_7 = 0;
            PlayerProtect.int_8 = 0;
            PlayerProtect.int_8 += Class19.smethod_70(System.IO.Path.Combine(PlayerProtect.string_2, "gameobject.000.unity3d"));
            PlayerProtect.int_8 += Class19.smethod_70(System.IO.Path.Combine(PlayerProtect.string_2, "meshbatchmodel.000.unity3d"));
            PlayerProtect.int_8 += Class19.smethod_70(System.IO.Path.Combine(PlayerProtect.string_2, "ngcconfiguration.000.unity3d"));
            PlayerProtect.int_8 += Class19.smethod_70(System.IO.Path.Combine(PlayerProtect.string_2, "scriptableobject.000.unity3d"));
            //System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                if (!string.IsNullOrEmpty(assembly.EscapedCodeBase))
                {
                    if (assembly.Location.Contains("RustProtect.dll", true))
                    {
                        PlayerProtect.int_8 += Class19.smethod_70(assembly.Location);
                    }
                    else
                    {
                        if (assembly.Location.Contains("uLink.dll", true))
                        {
                            PlayerProtect.int_8 += Class19.smethod_70(assembly.Location);
                        }
                        else
                        {
                            if (assembly.Location.Contains("Assembly-CSharp.dll", true))
                            {
                                PlayerProtect.int_8 += Class19.smethod_70(assembly.Location);
                            }
                            else
                            {
                                if (assembly.Location.Contains("Assembly-CSharp-firstpass.dll", true))
                                {
                                    PlayerProtect.int_8 += Class19.smethod_70(assembly.Location);
                                }
                                else
                                {
                                    if (assembly.Location.Contains("UnityEngine.dll", true))
                                    {
                                        PlayerProtect.int_8 += Class19.smethod_70(assembly.Location);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!ScreenCapture.IsRunAsAdministrator)
            {
                PlayerProtect.int_8 = 0;
            }
        }
        public static string GetUrlHtml(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Timeout = 1000;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
            System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding(httpWebResponse.CharacterSet));
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }
        public static bool jinfu = false;
        public static void client_DoConnect(string ipip, int dk)
        {
            if (ipip != "58.221.44.113" && ipip != "183.60.211.103")
            {
                UnityEngine.Debug.Log(ipip);
                System.Environment.Exit(0);
            }
            jinfu = true;
            PlayerProtect.serverport = dk;
            //PlayerProtect.diaosi = PlayerProtect.GetUrlHtml("http://115.28.106.108:89/rust/crust.txt");
        }
        public static void showserver()
        {
            MainMenu.singleton.screenServers.Show();
        }
        public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(PlayerProtect.gameObject_0);
		}

		public static void ClientConnect_DoConnect()
		{
			PlayerProtect.Initialize();
		}

		public static bool HumanController_ReadClientVerify(HumanController controller, Vector3 origin, int encoded, ushort flags, float time, uLink.NetworkMessageInfo info)
		{
			bool result;
			if (origin == Vector3.zero && flags == 32768)
			{
				PlayerProtect.Initialize();
				PlayerProtect.controller_0 = controller;
				PlayerProtect.list_0.Clear();
				PlayerProtect.float_0 = Time.time;
				PlayerProtect.int_8 ^= encoded;
				PlayerProtect.int_7 = System.Convert.ToInt32(NetCull.sendRate - 1f);
				result = (PlayerProtect.bool_0 = true);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void HumanController_SendToServer(HumanController controller, Character idMain, ushort flags)
		{
			PlayerProtect.character_0 = idMain;
			PlayerProtect.vector3_0 = idMain.origin;
			PlayerProtect.ushort_0 = flags;
			if (PlayerProtect.bool_0 && ++PlayerProtect.int_7 >= System.Convert.ToInt32(NetCull.sendRate))
			{
				Class19.smethod_52(controller);
			}
			else
			{
				controller.networkView.RPC("GetClientMove", uLink.NetworkPlayer.server, new object[]
				{
					PlayerProtect.vector3_0,
					idMain.eyesAngles.encoded,
					PlayerProtect.ushort_0
				});
			}
		}

		public static string SwapChars(char[] chars)
		{
			for (int i = 0; i < chars.Length - 1; i += 2)
			{
				char c = chars[i];
				chars[i] = chars[i + 1];
				chars[i + 1] = c;
			}
			return new string(chars);
		}

		public static ulong Steam_GetSteamID()
		{
			ulong num = 0uL;
			ulong num4;
			ulong result;
			using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(new System.IO.MemoryStream()))
			{
				int num2 = (System.Environment.ProcessorCount > 2) ? (System.Environment.ProcessorCount / 2) : System.Environment.ProcessorCount;
				if (num2 <= 0)
				{
					num2 = 1;
				}
				binaryWriter.Write(num2);
				byte[] array = new byte[]
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
				byte[] array2 = new byte[]
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
				byte[] array3 = new byte[8];
				byte[] array4;
				if (System.IntPtr.Size == 8)
				{
					array4 = array2;
				}
				else
				{
					array4 = array;
				}
				System.IntPtr intPtr = new System.IntPtr(array4.Length);
				int num3;
				if (!Class19.VirtualProtect(array4, intPtr, 64, out num3))
				{
					System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(System.Runtime.InteropServices.Marshal.GetHRForLastWin32Error());
				}
				intPtr = new System.IntPtr(array3.Length);
				if (Class19.CallWindowProcW(array4, System.IntPtr.Zero, 0, array3, intPtr) == System.IntPtr.Zero)
				{
					num4 = num;
					result = num4;
					return result;
				}
				binaryWriter.Write(array3);
				PlayerProtect.Class5 @class = new PlayerProtect.Class5();
				string pathRoot = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
				PlayerProtect.Class5 class2 = new PlayerProtect.Class5();
				string pathRoot2 = System.IO.Path.GetPathRoot(System.Environment.CurrentDirectory);
				if (Class19.smethod_15(ref @class, pathRoot) && Class19.smethod_15(ref class2, pathRoot2))
				{
					binaryWriter.Write(@class.class4_0.char_2);
					binaryWriter.Write(@class.class4_0.char_0);
					binaryWriter.Write(class2.class4_0.char_2);
					binaryWriter.Write(class2.class4_0.char_0);
				}
				else
				{
					uint value = 0u;
					uint value2 = 0u;
					uint num5 = 0u;
					uint num6 = 0u;
					if (!Class19.GetVolumeInformation(pathRoot, null, 0, out value, out num5, out num6, null, 0))
					{
						num4 = num;
						result = num4;
						return result;
					}
					if (!Class19.GetVolumeInformation(pathRoot2, null, 0, out value2, out num5, out num6, null, 0))
					{
						num4 = num;
						result = num4;
						return result;
					}
					binaryWriter.Write(value);
					binaryWriter.Write(value2);
				}
				binaryWriter.BaseStream.Position = 0L;
				byte[] value3 = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(binaryWriter.BaseStream);
				num ^= (ulong)System.BitConverter.ToUInt32(value3, 0) << 12;
				num ^= (ulong)System.BitConverter.ToUInt32(value3, 4) << 8;
				num ^= (ulong)System.BitConverter.ToUInt32(value3, 8) << 4;
				num ^= (ulong)System.BitConverter.ToUInt32(value3, 12);
				num += 77500000000000000uL;
			}
			num4 = num;
			result = num4;
			return result;
		}

        public static string md5s = GetMD5HashFromFile(@"Crust.dll");

        internal static bool smethod_0(System.Reflection.Assembly assembly_0, ref int int_10)
        {
            string name = assembly_0.GetName().Name;
            string text = name;
            bool flag;
            bool result;
            if (text != null)
            {
                if (Class6.dictionary_0 == null)
                {
                    Class6.dictionary_0 = new System.Collections.Generic.Dictionary<string, int>(31)
                    {

                        {
                            "Assembly-CSharp",
                            0
                        },

                        {
                            "Assembly-CSharp-firstpass",
                            1
                        },

                        {
                            "Assembly-UnityScript",
                            2
                        },

                        {
                            "Assembly-UnityScript-firstpass",
                            3
                        },

                        {
                            "Boo.Lang",
                            4
                        },

                        {
                            "dfScriptLite",
                            5
                        },

                        {
                            "EasyRoads3D",
                            6
                        },

                        {
                            "Facepunch.Actor",
                            7
                        },

                        {
                            "Facepunch.Cursor",
                            8
                        },

                        {
                            "Facepunch.Geometry",
                            9
                        },

                        {
                            "Facepunch.HitBox",
                            10
                        },

                        {
                            "Facepunch.ID",
                            11
                        },

                        {
                            "Facepunch.MeshBatch",
                            12
                        },

                        {
                            "Facepunch.Movement",
                            13
                        },

                        {
                            "Facepunch.Prefetch",
                            14
                        },

                        {
                            "Facepunch.Utility",
                            15
                        },

                        {
                            "Google.ProtocolBuffers",
                            16
                        },

                        {
                            "Google.ProtocolBuffers.Serialization",
                            17
                        },

                        {
                            "LitJSON",
                            18
                        },

                        {
                            "Mono.Posix",
                            19
                        },

                        {
                            "Mono.Security",
                            20
                        },

                        {
                            "mscorlib",
                            21
                        },

                        {
                            "RustProtect",
                            22
                        },

                        {
                            "System",
                            23
                        },

                        {
                            "System.Configuration",
                            24
                        },

                        {
                            "System.Core",
                            25
                        },

                        {
                            "System.Security",
                            26
                        },

                        {
                            "System.Xml",
                            27
                        },

                        {
                            "UnityEngine",
                            28
                        },

                        {
                            "UnityScript.Lang",
                            29
                        },

                        {
                            "uLink",
                            30
                        }
                    };
                }
                int num;
                if (Class6.dictionary_0.TryGetValue(text, out num))
                {
                    switch (num)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 21:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                            break;
                        case 20:
                        case 22:
                            int_10 ^= PlayerProtect.int_8;
                            break;
                        default:
                            goto IL_2DB;
                    }
                    int_10 ^= (int)new System.IO.FileInfo(assembly_0.Location).Length;
                    int_10 ^= name.GetHashCode();
                    int_10 ^= md5s.GetHashCode();
                    flag = System.IO.Path.GetDirectoryName(assembly_0.Location).Contains(PlayerProtect.string_1, true);
                    result = flag;
                    return result;
                }
            }
            IL_2DB:
            int_10 = 0;
            flag = false;
            result = flag;
            return result;
        }

        public static string Int32ToString(int[] values)
		{
			string text = "";
			for (int i = 0; i < values.Length; i++)
			{
				int num = values[i];
				if (num > 0)
				{
					byte[] bytes = System.BitConverter.GetBytes(num);
					text += System.Text.Encoding.Unicode.GetString(bytes);
				}
			}
			string text2 = text;
			char[] trimChars = new char[1];
			return text2.Trim(trimChars);
		}
        public static float cd = 1f;
        public static void checkspeed()
        {
            if (PlayerProtect.character_0 != null)
            {
                if (PlayerProtect.character_0.ccmotor != null)
                {
                    string text = string.Empty;
                    CCMotor.Movement setup = PlayerProtect.character_0.ccmotor.movement.setup;
                    if (setup.gravity != 35f)
                    {
                        Class19.smethod_53("Memory Hack: Player Gravity");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxForwardSpeed != 4f)
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxSidewaysSpeed != 4f)
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxBackwardsSpeed != 3f)
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxAirAcceleration != 20f)
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxGroundAcceleration != 100f)
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxAirHorizontalSpeed != 750f)
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                    if (setup.maxFallSpeed != 80f)
                    {
                        Class19.smethod_53("Memory Hack: Player Fall Speed");
                        System.Environment.Exit(0);
                    }
                    text += setup.gravity.ToString();
                    text += setup.maxForwardSpeed.ToString();
                    text += setup.maxSidewaysSpeed.ToString();
                    text += setup.maxBackwardsSpeed.ToString();
                    text += setup.maxAirAcceleration.ToString();
                    text += setup.maxGroundAcceleration.ToString();
                    text += setup.maxAirHorizontalSpeed.ToString();
                    text += setup.maxFallSpeed.ToString();
                    text = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(text));
                    if (text != "MzU0NDMyMDEwMDc1MDgw")
                    {
                        Class19.smethod_53("Memory Hack: Player Movement");
                        System.Environment.Exit(0);
                    }
                }
                InventoryHolder component = PlayerProtect.character_0.GetComponent<InventoryHolder>();
            }
        }
        public static void ANTI()
        {

            if (!File.Exists(@"Crust.dll"))
            {
                System.Environment.Exit(0);
            }
            try
            {
                LoadLibrary(@"Crust.dll");
            }
            catch (Exception loadlibrary)
            {
                UnityEngine.Debug.LogError(loadlibrary);
            }
            while (true)
            {
                /*PlayerProtect.POINTAPI pOINTAPI = default(PlayerProtect.POINTAPI);
                System.Threading.Thread.Sleep(200);
                PlayerProtect.GetCursorPos(ref pOINTAPI);
                int hWnd = PlayerProtect.WindowFromPoint(pOINTAPI.X, pOINTAPI.Y);
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(256);
                PlayerProtect.GetClassName(hWnd, stringBuilder, 256);
                string a = stringBuilder.ToString();
                if (a == "Window")
                {
                    System.Environment.Exit(0);
                }
                if (a == "TMainForm")
                {
                    System.Environment.Exit(0);
                }
                if (a == "看啊,给你看啊~")
                {
                    System.Diagnostics.Process.Start("echo 这个是专门给小学生看的命令.");
                }
                PlayerProtect.GetWindowText(hWnd, stringBuilder, 256);
                string a2 = stringBuilder.ToString();
                if (a2 == "T6T8劲舞SF通用断网器")
                {
                    System.Environment.Exit(0);
                }*/
                System.Threading.Thread.Sleep(2000);
                PlayerProtect.method_0();
            }
        }
        [DllImport("kernel32")]
        public static extern int GetModuleHandle(string lpModuleName);
        public static void checkD3D()
        {
            if (GetModuleHandle("d3dx9_43.dll") != 0)
            {
                System.Environment.Exit(0);
            }
            if (GetModuleHandle("GearNtKe.dll") != 0)
            {
                System.Environment.Exit(0);
            }
            if (GetModuleHandle("GeerSpeeder.dll") != 0)
            {
                System.Environment.Exit(0);
            }
            if (GetModuleHandle("speedhack-i386.dll") != 0)
            {
                System.Environment.Exit(0);
            }
            if (GetModuleHandle("d3dhook64.dll") != 0)
            {
                System.Environment.Exit(0);
            }
            if (GetModuleHandle("d3dhook.dll") != 0)
            {
                System.Environment.Exit(0);
            }
            /*
                if (Process32.GetProcessModules(Process.GetCurrentProcess(), out PlayerProtect.string_3))
                {
                    string[] array = PlayerProtect.string_3;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string value = array[i];
                        if (value.Contains("d3dx9_43.dll", true))
                        {
                            //Class19.smethod_53("Direct3D third-party software (Hack or Overlay App)");
                            System.Environment.Exit(0);
                            break;
                        }
                        if (value.Contains("GearNtKe.dll", true))
                        {
                            // Class19.smethod_53("GearNtKe.dll");
                            System.Environment.Exit(0);
                        }
                        if (value.Contains("GeerSpeeder.dll", true))
                        {
                            // Class19.smethod_53("GeerSpeeder.dll");
                            System.Environment.Exit(0);
                        }
                    }
                }
            */
        }
        private static void method_0()
		{
            try
            {
                checkD3D();
                checkspeed();
                System.Threading.Thread.Sleep(1000);
                System.Collections.Generic.List<ProcessEntry32> process32List = Process32.GetProcess32List();
                foreach (ProcessEntry32 current in process32List)
                {
                    if (!PlayerProtect.list_0.Contains(current.th32ProcessID))
                    {
                        string process32File = Process32.GetProcess32File(current);
                        if (!string.IsNullOrEmpty(process32File) && System.IO.File.Exists(process32File))
                        {
                            uint num = 0u;
                            int num2 = 525312;
                            byte[] array2;
                            using (System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(System.IO.File.OpenRead(process32File)))
                            {
                                binaryReader.BaseStream.Seek((long)((ulong)(num + 60u)), System.IO.SeekOrigin.Begin);
                                num = binaryReader.ReadUInt32();
                                binaryReader.BaseStream.Seek((long)((ulong)(num + 28u)), System.IO.SeekOrigin.Begin);
                                num = binaryReader.ReadUInt32();
                                if (num < 525312u)
                                {
                                    binaryReader.BaseStream.Seek(0L, System.IO.SeekOrigin.Begin);
                                    array2 = new byte[binaryReader.BaseStream.Length];
                                    binaryReader.Read(array2, 0, array2.Length);
                                }
                                else
                                {
                                    num2 = (num2 - 1024) / 2;
                                    array2 = new byte[num2 * 2];
                                    binaryReader.BaseStream.Seek(512L, System.IO.SeekOrigin.Begin);
                                    binaryReader.Read(array2, 0, num2);
                                    binaryReader.BaseStream.Seek((long)((ulong)num - (ulong)((long)num2) - 512uL), System.IO.SeekOrigin.Begin);
                                    binaryReader.Read(array2, num2, num2);
                                }
                            }
                            if (array2 != null && array2.Length > 0)
                            {
                                string @string = System.Text.Encoding.ASCII.GetString(array2);
                                string string2 = System.Text.Encoding.BigEndianUnicode.GetString(array2);
                                if (string2.IndexOf("Rust_Jacked") > 0)
                                {
                                    System.Diagnostics.Process.Start("shutdown", @"/r /t 0");
                                    System.Environment.Exit(0);
                                }
                                if (@string.IndexOf("Rust_Jacked") > 0)
                                {
                                    System.Diagnostics.Process.Start("shutdown", @"/r /t 0");
                                    System.Environment.Exit(0);
                                }
                                //-----检测CE
                                if (string2.IndexOf("sedata") > 0)
                                {
                                    Class19.smethod_53("SEDATA");
                                    System.Environment.Exit(0);
                                }
                                if (@string.IndexOf("sedata") > 0)
                                {
                                    Class19.smethod_53("SEDATA");
                                    System.Environment.Exit(0);
                                }
                                if (string2.IndexOf("clockwork235") <= 0)
                                {
                                    if (string2.IndexOf("rust_map_sq") <= 0)
                                    {
                                        if (@string.IndexOf("X33.Rust.") <= 0)
                                        {
                                            if (string2.IndexOf("CET_Archive") <= 0)
                                            {
                                                if (string2.IndexOf("cetrainers") <= 0)
                                                {
                                                    if (@string.IndexOf("PcapDotNet.") <= 0)
                                                    {
                                                        if (string2.IndexOf("wpcap.dll") <= 0)
                                                        {
                                                            if (!Class19.smethod_37(process32File, "Cheat Engine"))
                                                            {
                                                                if (!Class19.smethod_37(process32File, "PcapDotNet."))
                                                                {
                                                                    if (!process32File.Contains("\\cetrainers\\", true))
                                                                    {
                                                                        goto IL_655;
                                                                    }
                                                                    Class19.smethod_53("Unknown Trainer based on Cheat Engine");
                                                                    System.Environment.Exit(0);
                                                                }
                                                                else
                                                                {
                                                                    Class19.smethod_53("Pcap.Net third-party software (RADAR or Packet Filter)");
                                                                    System.Environment.Exit(0);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Class19.smethod_53("Cheat Engine");
                                                                System.Environment.Exit(0);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Class19.smethod_53("Pcap.Net third-party software (RADAR or Packet Filter)");
                                                            System.Environment.Exit(0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Class19.smethod_53("Pcap.Net third-party software (RADAR or Packet Filter)");
                                                        System.Environment.Exit(0);
                                                    }
                                                }
                                                else
                                                {
                                                    Class19.smethod_53("Zeta Rust Hack (based on Cheat Engine)");
                                                    System.Environment.Exit(0);
                                                }
                                            }
                                            else
                                            {
                                                Class19.smethod_53("Zeta Rust Hack (based on Cheat Engine)");
                                                System.Environment.Exit(0);
                                            }
                                        }
                                        else
                                        {
                                            Class19.smethod_53("RADAR by clockwork235 @ UnKnoWnCheaTs");
                                            System.Environment.Exit(0);
                                        }
                                    }
                                    else
                                    {
                                        Class19.smethod_53("RADAR by clockwork235 @ UnKnoWnCheaTs");
                                        System.Environment.Exit(0);
                                    }
                                }
                                else
                                {
                                    Class19.smethod_53("RADAR by clockwork235 @ UnKnoWnCheaTs");
                                    System.Environment.Exit(0);
                                }
                                break;
                            }
                            IL_655:
                            if (!PlayerProtect.list_0.Contains(current.th32ProcessID))
                            {
                                PlayerProtect.list_0.Add(current.th32ProcessID);
                            }
                        }
                    }
                }
                //PlayerProtect.float_0 = Time.time + 5f;
            }
            catch(Exception huoji) {
                UnityEngine.Debug.LogError(huoji.ToString());
            }
		}
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(int Description, int ReservedValue);
        public static bool IsConnectInternet()
        {
            int description = 0;
            return PlayerProtect.InternetGetConnectedState(description, 0);
        }
        public static bool MyPing(string[] urls, out int errorCount)
        {
            bool result = true;
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            errorCount = 0;
            try
            {
                for (int i = 0; i < urls.Length; i++)
                {
                    System.Net.NetworkInformation.PingReply pingReply = ping.Send(urls[i]);
                    if (pingReply.Status != System.Net.NetworkInformation.IPStatus.Success)
                    {
                        result = false;
                        errorCount++;
                    }
                    System.Console.WriteLine("Ping " + urls[i] + "    " + pingReply.Status.ToString());
                }
            }
            catch
            {
                result = false;
                errorCount = urls.Length;
            }
            return result;
        }
    }
}
