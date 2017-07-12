using NetLink;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.IO;

namespace RustProtect
{
	public class ProtectLoader
	{
		public static NetLink.Network NetClient;

		public static NetLink.Network.Packet NetPacket;

		public static Network.NetEncryption NetCrypt;

		public static byte[] EncryptionKey;

		public static ItemDataBlock[] DefaultItems = EnumerableToArray.ToArray(DatablockDictionary.All);

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static bool bool_1;

		[CompilerGenerated]
		private static bool bool_2;

		[CompilerGenerated]
		private static bool bool_3;

		public static bool Enabled
		{
			get;
			private set;
		}

		public static bool Encryption
		{
			get;
			private set;
		}

		public static bool Debug
		{
			get;
			private set;
		}

		public static bool Netlog
		{
			get;
			private set;
		}

		public static void OnConnect(string url, int port)
		{
			try
			{
				ProtectLoader.Enabled = false;
				ProtectLoader.NetCrypt = null;
				ProtectLoader.EncryptionKey = null;
				ProtectLoader.Debug = Environment.CommandLine.Contains("-debug");
				ProtectLoader.Netlog = Environment.CommandLine.Contains("-netlog");
				ProtectLoader.NetClient = new NetLink.Network(url, port);
				if (ProtectLoader.NetClient.Connected)
				{
					ProtectLoader.NetClient.SendPacket(NetLink.Network.PacketType.Firstpass, NetLink.Network.PacketFlag.None, null);
					ProtectLoader.NetPacket = ProtectLoader.NetClient.Receive(2000L);
					if (ProtectLoader.NetPacket.Received && ProtectLoader.NetPacket.Type == NetLink.Network.PacketType.Response && (byte)(ProtectLoader.NetPacket.Flags & NetLink.Network.PacketFlag.Compressed) == 1)
					{
						ProtectLoader.Enabled = ProtectLoader.NetPacket.Read<bool>();
						if (ProtectLoader.NetPacket.Length > 0L && ProtectLoader.Enabled)
						{
							ProtectLoader.Encryption = ProtectLoader.NetPacket.Read<bool>();
							byte[] array;
							string file = ProtectLoader.NetPacket.GetFile(out array);
                            if (file.Equals("RustProtect.Core", StringComparison.CurrentCultureIgnoreCase))
							{
								try
								{
									if (array != null && array.Length > 0 && Method.Initialize(Assembly.Load(array)))
									{

                                        object[] args = new object[]
										{
											url,
											port,
											array
										};
										Method.Invoke("RustProtect.Protection.Initialize", args);
										if (ProtectLoader.Encryption)
										{
											ProtectLoader.EncryptionKey = Method.Invoke("RustProtect.Protection.EncryptionKey").AsByteArray;
                                        }
										if (ProtectLoader.EncryptionKey != null && ProtectLoader.EncryptionKey.Length == 64)
										{
											ProtectLoader.NetCrypt = new Network.NetEncryption(EncryptionKey);
                                        }
                                        ProtectLoader.Enabled = true;
									}
								}
								catch (Exception)
								{
									ProtectLoader.Enabled = false;
								}
							}
						}
					}
					ProtectLoader.NetClient.Dispose();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}
	}
}
