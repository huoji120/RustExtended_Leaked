using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using uLink;
using UnityEngine;

namespace RustProtect
{
	public class ScreenCapture : UnityEngine.MonoBehaviour
	{
		internal sealed class Class0 : System.Collections.Generic.IEnumerator<object>, System.Collections.IEnumerator, System.IDisposable
		{
			private int int_0;

			private object object_0;

			public ScreenCapture screenCapture_0;

			public Texture2D texture2D_0;

			private int asdasd;

			object System.Collections.Generic.IEnumerator<object>.Current
			{
				get
				{
					return this.object_0;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return this.object_0;
				}
			}

			public Class0(int asdasd)
			{
				this.int_0 = asdasd;
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				bool result;
				switch (this.int_0)
				{
				case 0:
					this.int_0 = -1;
					this.object_0 = new WaitForEndOfFrame();
					this.int_0 = 1;
					result = true;
					return result;
				case 1:
					this.int_0 = -1;
					this.texture2D_0 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
					this.texture2D_0.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
					this.texture2D_0.Apply();
					this.screenCapture_0.ScreenshotBuffer = this.texture2D_0.EncodeToJPG();
					UnityEngine.Object.DestroyObject(this.texture2D_0);
					this.object_0 = 0;
					this.int_0 = 2;
					result = true;
					return result;
				case 2:
					this.int_0 = -1;
					this.screenCapture_0.ScreenshotHashsum = CRC32.Quick(this.screenCapture_0.ScreenshotBuffer);
					this.screenCapture_0.PacketHeader = new byte[]
					{
						7,
						0,
						0,
						35,
						10,
						83,
						99,
						114,
						101,
						101,
						110,
						115,
						104,
						111,
						116,
						254
					};
					this.screenCapture_0.PacketSize = 8192;
					System.Array.Resize<byte>(ref this.screenCapture_0.PacketHeader, 32);
					System.Buffer.BlockCopy(System.BitConverter.GetBytes(PlayerProtect.controller_0.playerClient.userID), 0, this.screenCapture_0.PacketHeader, 16, 8);
					System.Buffer.BlockCopy(System.BitConverter.GetBytes(this.screenCapture_0.ScreenshotBuffer.Length), 0, this.screenCapture_0.PacketHeader, 24, 4);
					System.Buffer.BlockCopy(System.BitConverter.GetBytes(this.screenCapture_0.ScreenshotHashsum), 0, this.screenCapture_0.PacketHeader, 28, 4);
					if (PlayerProtect.Debug)
					{
						if (!string.IsNullOrEmpty(this.screenCapture_0.Filename))
						{
							System.IO.File.WriteAllBytes(this.screenCapture_0.Filename, this.screenCapture_0.ScreenshotBuffer);
						}
						Debug.Log(string.Concat(new object[]
						{
							"Snapshot '",
							this.screenCapture_0.Filename,
							"' captured.\nSending To=",
							this.screenCapture_0.EndpointIP,
							", Socket=",
							this.screenCapture_0.Socket.Available,
							"\nMax Packet Size: ",
							this.screenCapture_0.PacketSize,
							"\nSnapshot Size: ",
							this.screenCapture_0.ScreenshotBuffer.Length,
							"\nSnapshot CRC: ",
							System.BitConverter.ToString(System.BitConverter.GetBytes(this.screenCapture_0.ScreenshotHashsum)).Replace("-", "")
						}));
						Debug.Log(string.Concat(new object[]
						{
							"Network.receiveBufferSize=",
							uLink.Network.config.receiveBufferSize,
							", Network.sendBufferSize=",
							uLink.Network.config.sendBufferSize
						}));
					}
					if (PlayerProtect.Debug)
					{
						Debug.Log("Network: Sending \"first of data\" of snapshot header.");
					}
					this.screenCapture_0.Socket.SendTo(this.screenCapture_0.PacketHeader, 0, this.screenCapture_0.PacketHeader.Length, SocketFlags.None, this.screenCapture_0.EndpointIP);
					this.screenCapture_0.PacketFrom = 0;
					this.screenCapture_0.PacketHeader[15] = 255;
					break;
				}
				result = false;
				return result;
			}

			void System.Collections.IEnumerator.Reset()
			{
				throw new System.NotSupportedException();
			}

			void System.IDisposable.Dispose()
			{
			}
		}

		public byte[] PacketHeader;

		public int PacketFrom = 0;

		public int BufferSize = 0;

		public int PacketSize = 16384;

		public byte[] ScreenshotBuffer;

		public uint ScreenshotHashsum = 0u;

		public EndPoint EndpointIP;

		public System.Net.Sockets.Socket Socket;

		public string Filename;

		public byte[] server = new byte[]
		{
			230,
			138,
			160,
			230,
			138,
			160,
			229,
			177,
			129,
			231,
			156,
			188,
			233,
			151,
			187,
			233,
			151,
			187,
			230,
			137,
			139,
			0
		};

		public static Rect startRect = new Rect(100f, 150f, 220f, 220f);

		public string texta = "chat.say qweqwe";

		public float cd = 1f;

		public void TakeSnapshot()
		{
			this.PacketFrom = 0;
			base.StartCoroutine(Class19.smethod_21(this));
		}
        [DllImport("advapi32.dll")]
        private extern static IntPtr FreeSid(IntPtr pSid);
        [DllImport("advapi32.dll")]
        private extern static int CheckTokenMembership(IntPtr TokenHandle, IntPtr SidToCheck, ref int IsMember);
        [DllImport("advapi32.dll")]
        private extern static int AllocateAndInitializeSid(byte[] pIdentifierAuthority, byte nSubAuthorityCount, int dwSubAuthority0, int dwSubAuthority1, int dwSubAuthority2, int dwSubAuthority3, int dwSubAuthority4, int dwSubAuthority5, int dwSubAuthority6, int dwSubAuthority7, out IntPtr pSid);
        private const int SECURITY_BUILTIN_DOMAIN_RID = 0x20;
        private const int DOMAIN_ALIAS_RID_ADMINS = 0x220;
        public static bool IsUserAnAdmin()
        {
            byte[] NtAuthority = new byte[6];
            NtAuthority[5] = 5; // SECURITY_NT_AUTHORITY
            IntPtr AdministratorsGroup;
            int ret = AllocateAndInitializeSid(NtAuthority, 2, SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, out AdministratorsGroup);
            if (ret != 0)
            {
                if (CheckTokenMembership(IntPtr.Zero, AdministratorsGroup, ref ret) == 0)
                {
                    ret = 0;
                }
                FreeSid(AdministratorsGroup);
            }

            return (ret != 0);
        }
        public static bool dlqss;
        public static bool IsRunAsAdministrator
        {
            get
            {
                return IsUserAnAdmin();
            }
        }

        public static void huoji()
        {
            //sever
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 11;
            gUIStyle.normal.textColor = Color.cyan;

            var groundWidth = 240;
            var groundHeight = 220;

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            var groupx = (screenWidth - groundWidth) / 2;
            var groupy = (screenHeight - groundHeight) / 2;
            GUI.BeginGroup(new Rect(groupx, groupy, groundWidth, groundHeight));
            GUI.Box(new Rect(0, 0, groundWidth, groundHeight), "反作弊系统提示");
            GUI.Label(new Rect(10, 30, 220, 30), "检测到您并没有以管理员身份运行RUST", gUIStyle);
            GUI.Label(new Rect(10, 70, 220, 30), "您将不能进入拥有反作弊程序的服务器!", gUIStyle);
            GUI.Label(new Rect(10, 110, 220, 30), "请您右键登陆器目录下的Rust.exe - 选择属性", gUIStyle);
            GUI.Label(new Rect(10, 150, 220, 30), "兼容性-勾选以管理员身份运行此程序-确定", gUIStyle);
            GUI.Label(new Rect(10, 190, 220, 30), "完成操作后重新打开登陆器即可!", gUIStyle);
            GUI.EndGroup();
                //sever end
            
        }
        public static void dlq()
        {
            //sever
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 11;
            gUIStyle.normal.textColor = Color.cyan;

            var groundWidth = 240;
            var groundHeight = 220;

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            var groupx = (screenWidth - groundWidth) / 2;
            var groupy = (screenHeight - groundHeight) / 2;
            GUI.BeginGroup(new Rect(groupx, groupy, groundWidth, groundHeight));
            GUI.Box(new Rect(0, 0, groundWidth, groundHeight), "反作弊系统提示");
            GUI.Label(new Rect(10, 30, 220, 30), "与登陆器连接丢失,反作弊系统失效!", gUIStyle);
            GUI.Label(new Rect(10, 70, 220, 30), "您将不能进入拥有反作弊程序的服务器!", gUIStyle);
            GUI.Label(new Rect(10, 110, 220, 30), "请检查登陆器是否被关闭!", gUIStyle);
            GUI.Label(new Rect(10, 150, 220, 30), "=如果您刚开RUST,则请您耐心等待(10秒左右)=", gUIStyle);
            GUI.Label(new Rect(10, 190, 220, 30), "注意游戏时候不能关闭登陆器!", gUIStyle);
            GUI.EndGroup();
            //sever end

        }
        private void OnGUI()
        {
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 11;
            gUIStyle.normal.textColor = Color.cyan;
            GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), "购买VIP请联系QQ847816340|叶家官方YY38613705", gUIStyle);
            if (Input.GetKeyDown(KeyCode.Insert) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                Application.Quit();
            }
            if (!IsRunAsAdministrator)
            {
                huoji();
                return;
            }
            if (!dlqss)
            {
                dlq();
                return;
            }
            int num = System.Convert.ToInt32(Time.time - this.cd);
            if (num > 1)
            {
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    ConsoleNetworker.SendCommandToServer("chat.say \"/kit starter\"");
                    this.cd = (float)((int)Time.time);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.Q) && !ChatUI.singleton.textInput.IsVisible)
                {
                    ConsoleNetworker.SendCommandToServer("chat.say /bb");
                    this.cd = (float)((int)Time.time);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.F7))
                {
                    ConsoleNetworker.SendCommandToServer("chat.say /sj 3");
                    this.cd = (float)((int)Time.time);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.F4))
                {
                    ConsoleNetworker.SendCommandToServer("chat.say /players");
                    this.cd = (float)((int)Time.time);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.F5))
                {
                    ConsoleNetworker.SendCommandToServer("chat.say \"/set fps\"");
                    this.cd = (float)((int)Time.time);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.F6))
                {
                    ConsoleNetworker.SendCommandToServer("chat.say /destroy");
                    this.cd = (float)((int)Time.time);
                    return;
                }
            }
            if (PlayerProtect.Snapshot && PlayerProtect.controller_0 != null && PlayerProtect.controller_0.playerClient != null)
            {
                PlayerClient playerClient = PlayerProtect.controller_0.playerClient;
                if (playerClient != null)
                {
                }
            }
        }

        public void SendNextPacket()
        {
            if (this.PacketHeader[15] != 255)
            {
                bool debug = PlayerProtect.Debug;
                this.Socket.SendTo(this.PacketHeader, 0, this.PacketHeader.Length, SocketFlags.None, this.EndpointIP);
                this.PacketFrom = 0;
                this.PacketHeader[15] = 255;
            }
            else if (this.ScreenshotBuffer.Length > this.PacketFrom)
            {
                this.BufferSize = this.PacketSize - 32;
                if (this.PacketFrom + this.BufferSize > this.ScreenshotBuffer.Length)
                {
                    this.BufferSize = this.ScreenshotBuffer.Length - this.PacketFrom;
                    this.PacketSize = this.BufferSize + 32;
                }
                byte[] array = new byte[this.PacketSize];
                System.Buffer.BlockCopy(this.PacketHeader, 0, array, 0, this.PacketHeader.Length);
                System.Buffer.BlockCopy(this.ScreenshotBuffer, this.PacketFrom, array, 32, this.BufferSize);
                bool debug2 = PlayerProtect.Debug;
                this.Socket.SendTo(array, 0, this.PacketSize, SocketFlags.None, this.EndpointIP);
            }
            else
            {
                bool debug3 = PlayerProtect.Debug;
                this.Socket.SendTo(this.PacketHeader, 0, this.PacketHeader.Length, SocketFlags.None, this.EndpointIP);
                this.ScreenshotBuffer = null;
            }
        }
        /*
		public void SendNextPacket()
		{
			if (this.PacketHeader[15] != 255)
			{
				if (PlayerProtect.Debug)
				{
					Debug.Log("Network: Sending \"first of data\" of snapshot header (repeat).");
				}
				this.Socket.SendTo(this.PacketHeader, 0, this.PacketHeader.Length, SocketFlags.None, this.EndpointIP);
				this.PacketFrom = 0;
				this.PacketHeader[15] = 255;
			}
			else if (this.ScreenshotBuffer.Length > this.PacketFrom)
			{
				this.BufferSize = this.PacketSize - 32;
				if (this.PacketFrom + this.BufferSize > this.ScreenshotBuffer.Length)
				{
					this.BufferSize = this.ScreenshotBuffer.Length - this.PacketFrom;
					this.PacketSize = this.BufferSize + 32;
				}
				byte[] array = new byte[this.PacketSize];
				System.Buffer.BlockCopy(this.PacketHeader, 0, array, 0, this.PacketHeader.Length);
				System.Buffer.BlockCopy(this.ScreenshotBuffer, this.PacketFrom, array, 32, this.BufferSize);
				if (PlayerProtect.Debug)
				{
					Debug.Log("Network: Sending \"part of data\" of snapshot: PacketSize=" + this.PacketSize);
				}
				this.Socket.SendTo(array, 0, this.PacketSize, SocketFlags.None, this.EndpointIP);
			}
			else
			{
				if (PlayerProtect.Debug)
				{
					Debug.Log("Network: Sending \"end of data\" of snapshot header.");
				}
				this.Socket.SendTo(this.PacketHeader, 0, this.PacketHeader.Length, SocketFlags.None, this.EndpointIP);
				this.ScreenshotBuffer = null;
			}
		}*/
    }
}
