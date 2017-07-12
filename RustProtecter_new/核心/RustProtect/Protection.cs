using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using NetLink;
using UnityEngine;


namespace RustProtect
{
    public class Protection : MonoBehaviour
	{
		public enum MessageType : short
		{
			Unknown,
			Connect,
			Approve,
			Checksum = 4,
			KickMessage = 8,
			Fileslist = 16,
			UpdateFile = 32,
			Screenshot = 64,
			OverrideItems = 256
		}

		private static GameObject gameObject_0;

		[CompilerGenerated]
		private static Protection protection_0;

		[CompilerGenerated]
		private static string string_0;

		private static string string_1 = Class3.smethod_10(5436);

		private static int int_0 = 28015;

		private static bool bool_0;

		private static float float_0 = 0f;

		private static bool bool_1 = false;

		private static List<uint> list_0 = new List<uint>();

		private static List<string> list_1 = new List<string>();

		private static List<string> list_2 = new List<string>();

		private static string[] string_2 = new string[0];

		private static VerifyFile[] verifyFile_0 = new VerifyFile[0];

		private static ulong ulong_0 = 18446744073709551615uL;

		private static Process process_0 = null;

		public static byte[] EncryptionKey = null;

		private static Hardware hardware_0 = new Hardware();

		private static Thread thread_0;

		private static Thread thread_1;

		private static NetLink.Network.Packet packet_0;

		private static NetLink.Network network_0;

		public static byte[] Screenshot = null;

		public static PlayerClient playerClient;

		public static ulong Steam_ID;

		public static string Username;

		private static string string_3;

		private static string string_4;

		public static Protection Singleton
		{
			get;
			private set;
		}

		public static string ApplicationPath
		{
			get;
			private set;
		}

		public static Assembly[] Assemblies
		{
			get
			{
				return EnumerableToArray.ToArray<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
			}
		}

		private void Awake()
		{
			Protection.Singleton = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Protection.list_0.Clear();
			Protection.list_1.Clear();
			Protection.list_2.Clear();
			Protection.process_0 = Process.GetCurrentProcess();
			Protection.ApplicationPath = Path.GetDirectoryName(Protection.process_0.MainModule.FileName);
			Protection.ApplicationPath = Protection.string_0 + Path.DirectorySeparatorChar.ToString();
			Protection.thread_0 = new Thread(new ThreadStart(Protection.smethod_7));
			Protection.thread_0.Start();
			base.InvokeRepeating(Class3.smethod_10(656), 0f, 0.1f);
			base.InvokeRepeating(Class3.smethod_10(678), 0f, 1f);
            UnityEngine.Debug.Log(Class3.smethod_10(714));
		}

		private void OnDestroy()
		{
			if (Protection.protection_0 == this)
			{
				Protection.Singleton = null;
			}
		}

        private void LateUpdate()
        {
            if ((NetCull.isClientRunning && (network_0 != null)) && network_0.Connected)
            {
                if ((playerClient == null) && (PlayerClient.GetLocalPlayer() != null))
                {
                    playerClient = PlayerClient.GetLocalPlayer();
                    UnityEngine.Debug.Log(string.Concat(new object[] { Class3.smethod_10(820), playerClient.userName, Class3.smethod_10(840), playerClient.userID, Class3.smethod_10(0x34e) }));
                    NetLink.Network.Packet packet = new NetLink.Network.Packet(NetLink.Network.PacketType.Firstpass, NetLink.Network.PacketFlag.Compressed, null);
                    packet.Write<ushort>(MessageType.Approve);
                    packet.Write<string>(SystemInfo.operatingSystem);
                    packet.Write<string>(string_4);
                    network_0.Send(packet);
                    thread_0 = new Thread(new ThreadStart(Protection.smethod_7));
                    thread_0.Start();
                    base.InvokeRepeating(Class3.smethod_10(0x290), 0f, 0.1f);
                    base.InvokeRepeating(Class3.smethod_10(0x2a6), 0f, 1f);
                }
            }
            else
            {
                base.CancelInvoke();
                smethod_8(null);
                if (network_0 != null)
                {
                    network_0.Dispose();
                }
                UnityEngine.Object.DestroyImmediate(this);
            }
        }

        public void DoNetwork()
        {
            if (((network_0 != null) && network_0.Connected) && (packet_0 = network_0.Receive(0L)).Received)
            {
                if (((verifyFile_0.Length != 0) || (packet_0.Type == NetLink.Network.PacketType.Response)) && ((packet_0.Type != NetLink.Network.PacketType.Response) || packet_0.Flags.Has<NetLink.Network.PacketFlag>(NetLink.Network.PacketFlag.Compressed)))
                {
                    NetLink.Network.PacketType type = packet_0.Type;
                    switch (type)
                    {
                        case NetLink.Network.PacketType.Pingpong:
                            network_0.SendPacket(NetLink.Network.PacketType.Pingpong, NetLink.Network.PacketFlag.None, null);
                            return;

                        case NetLink.Network.PacketType.Disconnect:
                            smethod_8(Class3.smethod_10(0x3e2));
                            network_0.Dispose();
                            UnityEngine.Object.DestroyImmediate(base.gameObject);
                            return;
                    }
                    if ((type == NetLink.Network.PacketType.DataStream) && packet_0.Flags.Has<NetLink.Network.PacketFlag>(NetLink.Network.PacketFlag.Compressed))
                    {
                        MessageType message = (MessageType)((short)packet_0.Read<ushort>());
                        this.DoNetworkMessageData(packet_0, message);
                    }
                }
            }
        }

        public void DoNetworkMessageData(NetLink.Network.Packet packet, Protection.MessageType message)
		{
			if (ProtectLoader.Debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					Class3.smethod_10(1048),
					Protection.network_0.RemoteEndPoint,
					Class3.smethod_10(1110),
					message
				}));
			}
			if (message == Protection.MessageType.Checksum)
			{
				int num = Protection.packet_0.Read<int>();
				Protection.verifyFile_0 = new VerifyFile[num];
				for (int i = 0; i < num; i++)
				{
					Protection.verifyFile_0[i] = new VerifyFile
					{
						Filename = Protection.packet_0.Read<string>(),
						Filesize = Protection.packet_0.Read<long>()
					};
				}
				Protection.thread_1 = new Thread(new ThreadStart(Protection.smethod_6));
				Protection.thread_1.Start();
				return;
			}
			if (message != Protection.MessageType.Screenshot)
			{
				if (message == Protection.MessageType.OverrideItems)
				{
					int num2 = packet.Read<int>();
					for (int j = 0; j < num2; j++)
					{
						try
						{
							ItemDataBlock byName = DatablockDictionary.GetByName(packet.Read<string>());
							if (byName == null)
							{
								throw new Exception();
							}
							byName.itemDescriptionOverride = packet.Read<string>();
							byName.isResearchable = packet.Read<bool>();
							byName.isRecycleable = packet.Read<bool>();
							byName.isRepairable = packet.Read<bool>();
							byName._splittable = packet.Read<bool>();
							byName.doesLoseCondition = packet.Read<bool>();
							byName._maxCondition = packet.Read<float>();
							byName._minUsesForDisplay = packet.Read<int>();
							byName._spawnUsesMin = packet.Read<int>();
							byName._spawnUsesMax = packet.Read<int>();
							BlueprintDataBlock blueprintDataBlock = null;
							if (packet.Read<bool>() && Protection.smethod_9(byName, out blueprintDataBlock))
							{
								blueprintDataBlock.numResultItem = packet.Read<int>();
								blueprintDataBlock.craftingDuration = packet.Read<float>();
								blueprintDataBlock.RequireWorkbench = packet.Read<bool>();
								int num3 = packet.Read<int>();
								List<BlueprintDataBlock.IngredientEntry> list = new List<BlueprintDataBlock.IngredientEntry>();
								for (int k = 0; k < num3; k++)
								{
									BlueprintDataBlock.IngredientEntry ingredientEntry = new BlueprintDataBlock.IngredientEntry();
									ingredientEntry.amount = packet.Read<int>();
									ingredientEntry.Ingredient = DatablockDictionary.GetByName(packet.Read<string>());
									if (ingredientEntry.amount > 0 && ingredientEntry.Ingredient != null)
									{
										list.Add(ingredientEntry);
									}
								}
								blueprintDataBlock.ingredients = list.ToArray();
							}
						}
						catch (Exception)
						{
							UnityEngine.Debug.Log("ErrOr: " + Class3.smethod_10(1120));
							return;
						}
					}
					return;
				}
				return;
			}
			Snapshot.Singleton.CaptureSnapshot();
			base.InvokeRepeating(Class3.smethod_10(1188), 0f, 0.1f);
		}


        private static void smethod_0(string string_5, [Optional, DefaultParameterValue("")] string string_6, [Optional, DefaultParameterValue(false)] bool bool_2)
        {
            if (((network_0 != null) && network_0.Connected) && (Time.time > float_0))
            {
                float_0 = Time.time + 1f;
                if (string.IsNullOrEmpty(string_6))
                {
                    string_6 = "";
                }
                if (string.IsNullOrEmpty(string_5))
                {
                    string_5 = Class3.smethod_10(0x4bc);
                }
                NetLink.Network.Packet packet = new NetLink.Network.Packet(NetLink.Network.PacketType.DataStream, NetLink.Network.PacketFlag.Compressed, null);
                packet.Write<ushort>(MessageType.KickMessage);
                packet.Write<bool>(bool_2);
                packet.Write<string>(string_6);
                packet.Write<string>(string_5);
                network_0.Send(packet);
            }
        }




        public void DoScanningPlayer()
		{
			if (NetCull.isClientRunning && !Protection.bool_1)
			{
				Protection.bool_1 = true;
				if (Protection.playerClient != null && Protection.playerClient.controllable != null && Protection.playerClient.controllable.character != null)
				{
					Character character = Protection.playerClient.controllable.character;
					if (character.ccmotor != null)
					{
						if (render.fov < 60)
						{
							render.fov = 60;
						}
						if (render.fov > 80)
						{
							render.fov = 80;
						}
						CCMotor.Movement expr_AA = character.ccmotor.movement.setup;
						if (expr_AA.gravity != 35f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxFallSpeed != 80f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxForwardSpeed != 4f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxSidewaysSpeed != 4f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxBackwardsSpeed != 3f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxAirAcceleration != 20f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxGroundAcceleration != 100f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
						if (expr_AA.maxAirHorizontalSpeed != 750f)
						{
							Protection.smethod_0(Class3.smethod_10(1272), Protection.playerClient.userName + Class3.smethod_10(1360), true);
						}
					}
					InventoryHolder component = character.GetComponent<InventoryHolder>();
					if (component != null && component.itemRepresentation != null && component.inputItem.datablock is BulletWeaponDataBlock)
					{
						BulletWeaponDataBlock bulletWeaponDataBlock = (BulletWeaponDataBlock)component.inputItem.datablock;
						uint uniqueID = (uint)bulletWeaponDataBlock.uniqueID;
						string text = string.Empty;
						text = text + bulletWeaponDataBlock.bulletRange + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.reloadDuration + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.fireRate + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.fireRateSecondary + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.recoilYawMin + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.recoilYawMax + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.recoilPitchMin + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.recoilPitchMax + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.recoilDuration + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.aimSway + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.aimSwaySpeed + Class3.smethod_10(1472);
						text = text + bulletWeaponDataBlock.aimingRecoilSubtract + Class3.smethod_10(1472);
						text += bulletWeaponDataBlock.crouchRecoilSubtract;
						ulong num = BitConverter.ToUInt64(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(text)), 0);
						if (ProtectLoader.Debug)
						{
							string contents = string.Concat(new object[]
							{
								Class3.smethod_10(1478),
								bulletWeaponDataBlock.name,
								Class3.smethod_10(1498),
								uniqueID,
								Class3.smethod_10(1512),
								num.ToString(Class3.smethod_10(1534)),
								Class3.smethod_10(1544),
								text,
								Class3.smethod_10(1562)
							});
							File.AppendAllText(Path.Combine(Application.dataPath, Class3.smethod_10(1572)), contents);
						}
						if (uniqueID == 1591469150u && num != 18345605656075165559uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 1079594584u && num != 10722295551327323350uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 2805457609u && num != 10546893518047341734uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 3119840708u && num != 16251289927229003782uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 4228819245u && num != 4577847497111890014uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 1377786400u && num != 14126027389018450226uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 668079439u && num != 7144577692071069087uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 2442785382u && num != 9331602331646563999uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
						if (uniqueID == 545958908u && num != 17058071295959981111uL)
						{
							Protection.smethod_0(Class3.smethod_10(1620), Protection.playerClient.userName + Class3.smethod_10(1704), true);
						}
					}
				}
				Protection.bool_1 = false;
			}
		}

        public void DoSnapshot()
        {
            if ((network_0.Connected && base.IsInvoking()) && (Screenshot != null))
            {
                base.CancelInvoke(Class3.smethod_10(0x4a4));
                NetLink.Network.Packet packet = new NetLink.Network.Packet(NetLink.Network.PacketType.DataStream, NetLink.Network.PacketFlag.Compressed, null);
                packet.Write<ushort>(MessageType.Screenshot);
                packet.Write<string>(Steam_ID.ToString());
                packet.Write(Screenshot, 0, Screenshot.Length);
                network_0.Send(packet);
                Screenshot = null;
            }
        }


        private static long smethod_1(Stream stream_0, string string_5)
		{
			long num = -1L;
			byte[] array = new byte[4096];
			do
			{
				if ((long)stream_0.Read(array, 0, array.Length) > 0L)
				{
					num = Protection.smethod_2(array, string_5);
				}
			}
			while (num != -1L);
			return num;
		}

        private static long smethod_2(byte[] object_0, string string_5)
        {
            return smethod_4(object_0, (long)object_0.Length, string_5);
        }

        private static long smethod_3(byte[] object_0, object[] object_1)
        {
            return smethod_5(object_0, (long)object_0.Length, object_1);
        }
        private static long smethod_4(byte[] object_0, long long_0, string string_5)
		{
			int num = 0;
			if (long_0 > (long)object_0.Length)
			{
				long_0 = (long)object_0.Length;
			}
			int num2 = 0;
			while ((long)num2 < long_0)
			{
				if (object_0[num2] == string_5[num])
				{
					num++;
				}
				else
				{
					num = 0;
				}
				if (num == string_5.Length)
				{
					return (long)(num2 - string_5.Length);
				}
				num2++;
			}
			return -1L;
		}

        private static long smethod_5(byte[] object_0, long long_0, object[] object_1)
        {
            int[] array = new int[object_1.Length];
            if (long_0 > (long)object_0.Length)
            {
                long_0 = (long)object_0.Length;
            }
            int num = 0;
            while ((long)num < long_0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    int huoji_1 = object_0[num];
                    int huoji_2 = (int)(object_1[i]);
                    if (huoji_1 == huoji_2)
                    {
                        array[i]++;
                    }
                    else
                    {
                        array[i] = 0;
                    }
                    if (array[i] == object_1[i].ToString().Length)
                    {
                        return (long)(num - object_1[i].ToString().Length);
                    }
                }
                num++;
            }
            return -1L;
        }



        private static void smethod_6()
        {
            if (verifyFile_0.Length != 0)
            {
                try
                {
                    string contents = "";
                    ulong maxValue = ulong.MaxValue;
                    MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                    IntPtr processHandle = Process32.OpenProcess(Process32.PROCESS_QUERY_INFORMATION | Process32.PROCESS_VM_READ, 0, (uint)process_0.Id);
                    if (processHandle == IntPtr.Zero)
                    {
                        ulong_0 = ulong.MaxValue;
                    }
                    else
                    {
                        Dictionary<string, MemoryAssemblyEntry> dictionary = new Dictionary<string, MemoryAssemblyEntry>();
                        uint num2 = Process32.smethod_3(processHandle, (long)(process_0.MainModule.BaseAddress.ToInt32() + 0xa1f9cc));
                        uint num3 = 0;
                        bool flag = false;
                        int num4 = 0;
                        uint num5 = 0;
                        uint num6 = num2;
                        while (num3 < 0x800)
                        {
                            if (num4 >= Assemblies.Length)
                            {
                                break;
                            }
                            try
                            {
                                num3 += 4;
                                if (!flag)
                                {
                                    num5 = Process32.smethod_3(processHandle, (long)(num2 + num3));
                                    if (num5 == 0)
                                    {
                                        continue;
                                    }
                                    num6 = Process32.smethod_3(processHandle, (long)(num5 + 20));
                                    if (num6 == 0)
                                    {
                                        continue;
                                    }
                                    string path = Process32.ReadString(processHandle, (long)num6);
                                    if (!path.ToLower().EndsWith(Class3.smethod_10(0x714)) || !File.Exists(path))
                                    {
                                        continue;
                                    }
                                    flag = true;
                                }
                                num6 = Process32.smethod_3(processHandle, (long)(num2 + num3));
                                uint num8 = Process32.smethod_3(processHandle, (long)(num6 + 12));
                                if (num8 > 0)
                                {
                                    uint num7 = Process32.smethod_3(processHandle, (long)(num6 + 8));
                                    uint num9 = Process32.smethod_3(processHandle, (long)(num6 + 20));
                                    uint num10 = Process32.smethod_3(processHandle, (long)(num6 + 0x20));
                                    if (((num7 > 0) && (num9 > 0)) && (num10 > 0))
                                    {
                                        MemoryAssemblyEntry entry2 = new MemoryAssemblyEntry
                                        {
                                            Pointer = (long)num7,
                                            Filesize = num8,
                                            Filepath = Process32.ReadString(processHandle, (long)num9),
                                            TargetRuntime = Process32.ReadString(processHandle, (long)num10)
                                        };
                                        if (File.Exists(entry2.Filepath))
                                        {
                                            string str2 = entry2.Filepath.Replace(string_0, "");
                                            dictionary[str2] = entry2;
                                            num4++;
                                        }
                                    }
                                }
                                continue;
                            }
                            catch (Exception exception)
                            {
                                ulong_0 = ulong.MaxValue;
                                return;
                            }
                        }
                        if (!flag)
                        {
                            ulong_0 = ulong.MaxValue;
                        }
                        else
                        {
                            foreach (VerifyFile file in verifyFile_0)
                            {
                                MemoryAssemblyEntry entry3;
                                if (!File.Exists(file.Filename))
                                {
                                    break;
                                }
                                contents = contents + file.Filename + Class3.smethod_10(0x8a4);
                                string str4 = Path.GetFileName(file.Filename).Replace(Class3.smethod_10(0x8ac), Class3.smethod_10(0x8b2));
                                if (dictionary.TryGetValue(file.Filename, out entry3))
                                {
                                    if (file.Filesize != entry3.Filesize)
                                    {
                                        break;
                                    }
                                    byte[] buffer = Process32.ReadBytes(processHandle, entry3.Pointer, (int)entry3.Filesize);
                                    if ((buffer == null) || (buffer.Length != entry3.Filesize))
                                    {
                                        break;
                                    }
                                    maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(buffer), 0);
                                    maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                }
                                else if ((str4.Contains(Class3.smethod_10(0x8b8)) && !(str4 == Class3.smethod_10(0x8ce))) && !(Path.GetExtension(str4).ToLower() != Class3.smethod_10(0x8f0)))
                                {
                                    UnityEngine.Debug.LogError(Class3.smethod_10(0x8fc) + str4);
                                }
                                else
                                {
                                    maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes(file.Filename)), 0);
                                    maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                }
                            }
                            Process32.CloseHandle(processHandle);
                            ulong_0 = maxValue;
                            if (ProtectLoader.Debug)
                            {
                                contents = contents + Class3.smethod_10(0x93e) + ulong_0.ToString(Class3.smethod_10(0x5fe));
                                File.WriteAllText(Path.Combine(Application.dataPath, Class3.smethod_10(0x95c)), contents);
                                List<string> list = new List<string>();
                                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                                {
                                    list.Add(assembly.GetName().Name);
                                }
                                File.WriteAllLines(Path.Combine(Application.dataPath, Class3.smethod_10(0x984)), list.ToArray());
                            }
                            if ((network_0 != null) && network_0.Connected)
                            {
                                Assembly[] assemblies = Assemblies;
                                NetLink.Network.Packet packet = new NetLink.Network.Packet(NetLink.Network.PacketType.DataStream, NetLink.Network.PacketFlag.Compressed, null);
                                packet.Write<ushort>(MessageType.Checksum);
                                packet.Write<ulong>(ulong_0);
                                packet.Write<int>(assemblies.Length);
                                foreach (Assembly assembly2 in assemblies)
                                {
                                    string str5 = (assembly2.EscapedCodeBase == null) ? "" : assembly2.Location;
                                    packet.Write<string>(assembly2.GetName().Name);
                                    packet.Write<string>(str5);
                                }
                                packet.Write<int>(string_2.Length);
                                foreach (string str6 in string_2)
                                {
                                    packet.Write<string>(str6);
                                }
                                network_0.Send(packet);
                            }
                            thread_1 = null;
                            Thread.Sleep(10);
                        }
                    }
                }
                catch (Exception exception2)
                {
                    UnityEngine.Debug.LogError(Class3.smethod_10(0x9b4) + exception2.Message);
                }
            }
        }


        /*
		private static void smethod_6()
		{
			if (Protection.verifyFile_0.Length == 0)
			{
				return;
			}
			try
			{
				string text = "";
				ulong num = 18446744073709551615uL;
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				IntPtr intPtr = Process32.OpenProcess(Process32.PROCESS_QUERY_INFORMATION | Process32.PROCESS_VM_READ, 0, (uint)Protection.process_0.Id);
				if (intPtr == IntPtr.Zero)
				{
					Protection.ulong_0 = 18446744073709551615uL;
				}
				else
				{
					Dictionary<string, MemoryAssemblyEntry> dictionary = new Dictionary<string, MemoryAssemblyEntry>();
					uint num2 = Process32.smethod_3(intPtr, (long)(Protection.process_0.MainModule.BaseAddress.ToInt32() + 10615244));
					uint num3 = 0u;
					bool flag = false;
					int num4 = 0;
					while (num3 < 2048u && num4 < Protection.Assemblies.Length)
					{
						try
						{
							num3 += 4u;
							uint num6;
							if (!flag)
							{
								uint num5 = Process32.smethod_3(intPtr, (long)((ulong)(num2 + num3)));
								if (num5 == 0u)
								{
									continue;
								}
								num6 = Process32.smethod_3(intPtr, (long)((ulong)(num5 + 20u)));
								if (num6 == 0u)
								{
									continue;
								}
								string text2 = Process32.ReadString(intPtr, (long)((ulong)num6));
								if (!text2.ToLower().EndsWith(Class3.smethod_10(1812)) || !File.Exists(text2))
								{
									continue;
								}
								if (ProtectLoader.Debug)
								{
									UnityEngine.Debug.Log(Class3.smethod_10(1846) + num5.ToString(Class3.smethod_10(1880)));
								}
								flag = true;
							}
							num6 = Process32.smethod_3(intPtr, (long)((ulong)(num2 + num3)));
							uint num7 = Process32.smethod_3(intPtr, (long)((ulong)(num6 + 12u)));
							if (num7 > 0u)
							{
								uint num8 = Process32.smethod_3(intPtr, (long)((ulong)(num6 + 8u)));
								uint num9 = Process32.smethod_3(intPtr, (long)((ulong)(num6 + 20u)));
								uint num10 = Process32.smethod_3(intPtr, (long)((ulong)(num6 + 32u)));
								if (num8 > 0u && num9 > 0u && num10 > 0u)
								{
									MemoryAssemblyEntry memoryAssemblyEntry = new MemoryAssemblyEntry
									{
										Pointer = (long)((ulong)num8),
										Filesize = num7,
										Filepath = Process32.ReadString(intPtr, (long)((ulong)num9)),
										TargetRuntime = Process32.ReadString(intPtr, (long)((ulong)num10))
									};
									if (File.Exists(memoryAssemblyEntry.Filepath))
									{
										string text3 = memoryAssemblyEntry.Filepath.Replace(Protection.string_0, "");
										dictionary[text3] = memoryAssemblyEntry;
										num4++;
										if (ProtectLoader.Debug)
										{
											UnityEngine.Debug.Log(string.Concat(new object[]
											{
												Class3.smethod_10(1888),
												memoryAssemblyEntry.Pointer.ToString(Class3.smethod_10(1880)),
												Class3.smethod_10(1932),
												memoryAssemblyEntry.Filesize,
												Class3.smethod_10(1976),
												text3,
												Class3.smethod_10(2020),
												memoryAssemblyEntry.Filepath,
												Class3.smethod_10(2064),
												memoryAssemblyEntry.TargetRuntime
											}));
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							Protection.ulong_0 = 18446744073709551615uL;
							if (ProtectLoader.Debug)
							{
								UnityEngine.Debug.LogError(ex.ToString());
							}
							return;
						}
					}
					if (!flag)
					{
						Protection.ulong_0 = 18446744073709551615uL;
						if (ProtectLoader.Debug)
						{
							UnityEngine.Debug.LogError(Class3.smethod_10(2120));
						}
					}
					else
					{
						VerifyFile[] array = Protection.verifyFile_0;
						for (int i = 0; i < array.Length; i++)
						{
							VerifyFile verifyFile = array[i];
							if (!File.Exists(verifyFile.Filename))
							{
								break;
							}
							text = text + verifyFile.Filename + Class3.smethod_10(2212);
							string text4 = Path.GetFileName(verifyFile.Filename).Replace(Class3.smethod_10(2220), Class3.smethod_10(2226));
							MemoryAssemblyEntry memoryAssemblyEntry2;
							if (dictionary.TryGetValue(verifyFile.Filename, out memoryAssemblyEntry2))
							{
								if (verifyFile.Filesize != (long)((ulong)memoryAssemblyEntry2.Filesize))
								{
									break;
								}
								byte[] array2 = Process32.ReadBytes(intPtr, memoryAssemblyEntry2.Pointer, (int)memoryAssemblyEntry2.Filesize);
								if (array2 == null || (long)array2.Length != (long)((ulong)memoryAssemblyEntry2.Filesize))
								{
									break;
								}
								num ^= BitConverter.ToUInt64(mD5CryptoServiceProvider.ComputeHash(array2), 0);
								num ^= BitConverter.ToUInt64(mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(verifyFile.Filename)), 0);
							}
							else if (text4.Contains(Class3.smethod_10(2232)) && !(text4 == Class3.smethod_10(2254)) && !(Path.GetExtension(text4).ToLower() != Class3.smethod_10(2288)))
							{
								UnityEngine.Debug.LogError(Class3.smethod_10(2300) + text4);
							}
							else
							{
								num ^= BitConverter.ToUInt64(mD5CryptoServiceProvider.ComputeHash(File.ReadAllBytes(verifyFile.Filename)), 0);
								num ^= BitConverter.ToUInt64(mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(verifyFile.Filename)), 0);
							}
						}
						Process32.CloseHandle(intPtr);
						Protection.ulong_0 = num;
						if (ProtectLoader.Debug)
						{
							text = text + Class3.smethod_10(2366) + Protection.ulong_0.ToString(Class3.smethod_10(1534));
							File.WriteAllText(Path.Combine(Application.dataPath, Class3.smethod_10(2396)), text);
							List<string> list = new List<string>();
							Assembly[] array3 = AppDomain.CurrentDomain.GetAssemblies();
							for (int i = 0; i < array3.Length; i++)
							{
								Assembly assembly = array3[i];
								list.Add(assembly.GetName().Name);
							}
							File.WriteAllLines(Path.Combine(Application.dataPath, Class3.smethod_10(2436)), list.ToArray());
						}
						if (Protection.network_0 != null && Protection.network_0.Connected)
						{
							Assembly[] assemblies = Protection.Assemblies;
                            NetLink.Network.Packet packet = new NetLink.Network.Packet(16, 1, null);
							packet.Write<ushort>(Protection.MessageType.Checksum);
							packet.Write<ulong>(Protection.ulong_0);
							packet.Write<int>(assemblies.Length);
							Assembly[] array3 = assemblies;
							for (int i = 0; i < array3.Length; i++)
							{
								Assembly assembly2 = array3[i];
								string text5 = (assembly2.EscapedCodeBase == null) ? "" : assembly2.Location;
								packet.Write<string>(assembly2.GetName().Name);
								packet.Write<string>(text5);
							}
							packet.Write<int>(Protection.string_2.Length);
							string[] array4 = Protection.string_2;
							for (int i = 0; i < array4.Length; i++)
							{
								string text6 = array4[i];
								packet.Write<string>(text6);
							}
							Protection.network_0.Send(packet);
						}
						Protection.thread_1 = null;
						Thread.Sleep(10);
					}
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogError(Class3.smethod_10(2484) + ex2.Message);
			}
		}*/

        private static void smethod_7()
        {
            Label_0001:
            if (!NetCull.isClientRunning || (thread_0 == null))
            {
                return;
            }
            Path.Combine(Application.dataPath, Class3.smethod_10(0x9e2));
            List<string> list = new List<string>();
            foreach (ProcessModule module in process_0.Modules)
            {
                list.Add(module.FileName);
            }
            string_2 = list.ToArray();
            if (ProtectLoader.Debug)
            {
                File.WriteAllText(Path.Combine(Application.dataPath, Class3.smethod_10(0xa12)), string.Join(Class3.smethod_10(0x8a4), string_2));
            }
            foreach (string str in string_2)
            {
                if (!list_2.Contains(str))
                {
                    if (str.Contains(Class3.smethod_10(0xa3a), true))
                    {
                        smethod_0(Class3.smethod_10(0xa56), playerClient.userName + Class3.smethod_10(0xabc), true);
                        break;
                    }
                    if (new FileInfo(str).Length < 0x80000L)
                    {
                        byte[] buffer = File.ReadAllBytes(str);
                        string[] textArray1 = new string[] { Class3.smethod_10(0xb3a), Class3.smethod_10(0xb5c) };
                        if (smethod_3(buffer, textArray1) != -1L)
                        {
                            smethod_0(Class3.smethod_10(0xb74), playerClient.userName + Class3.smethod_10(0xbc6), true);
                            break;
                        }
                        string[] textArray2 = new string[] { Class3.smethod_10(0xc30), Class3.smethod_10(0xc42), Class3.smethod_10(0xc70) };
                        if (smethod_3(buffer, textArray2) != -1L)
                        {
                            smethod_0(Class3.smethod_10(0xca2), playerClient.userName + Class3.smethod_10(0xce2), true);
                            break;
                        }
                        if (smethod_2(buffer, Class3.smethod_10(0xa3a)) != -1L)
                        {
                            smethod_0(Class3.smethod_10(0xa56), playerClient.userName + Class3.smethod_10(0xabc), true);
                            break;
                        }
                    }
                    list_2.Add(str);
                }
            }
            using (List<ProcessEntry32>.Enumerator enumerator = Process32.GetProcess32List().GetEnumerator())
            {
                ProcessEntry32 current;
                string str2;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (!list_0.Contains(current.th32ProcessID))
                    {
                        str2 = Process32.GetProcess32File(current);
                        if ((!string.IsNullOrEmpty(str2) && File.Exists(str2)) && !list_1.Contains(str2))
                        {
                            goto Label_02C6;
                        }
                    }
                }
                goto Label_0723;
                Label_02C6:
                if (ProtectLoader.Debug)
                {
                    File.AppendAllText(Path.Combine(Application.dataPath, Class3.smethod_10(0xd3a)), string.Concat(new object[] { Class3.smethod_10(0xd66), current.th32ProcessID, Class3.smethod_10(0x456), str2, Class3.smethod_10(0x8a4) }));
                }
                if ((str2.Contains(Class3.smethod_10(0xd6c), true) || str2.Contains(Class3.smethod_10(0xd88), true)) || str2.Contains(Class3.smethod_10(0xda2), true))
                {
                    goto Label_06EA;
                }
                if (str2.Contains(Class3.smethod_10(0xe74), true))
                {
                    smethod_0(Class3.smethod_10(0xe8e), playerClient.userName + Class3.smethod_10(0xef4), true);
                    goto Label_0723;
                }
                MemoryStream stream = new MemoryStream();
                using (BinaryReader reader = new BinaryReader(File.OpenRead(str2)))
                {
                    reader.BaseStream.Seek(60L, SeekOrigin.Begin);
                    int num = reader.ReadInt32();
                    reader.BaseStream.Seek((long)(num + 6), SeekOrigin.Begin);
                    short num2 = reader.ReadInt16();
                    if (num2 > 7)
                    {
                        num2 = 7;
                    }
                    int num3 = num + 0x108;
                    for (int i = 0; i < num2; i++)
                    {
                        reader.BaseStream.Seek((long)num3, SeekOrigin.Begin);
                        uint num5 = reader.ReadUInt32();
                        uint num6 = reader.ReadUInt32();
                        if (num5 > 0x20000)
                        {
                            num5 = 0x20000;
                        }
                        reader.BaseStream.Seek((long)num6, SeekOrigin.Begin);
                        byte[] buffer2 = new byte[num5];
                        reader.Read(buffer2, 0, buffer2.Length);
                        stream.Write(buffer2, 0, buffer2.Length);
                        num3 += 40;
                    }
                }
                if (stream.Length > 0L)
                {
                    byte[] bytes = stream.ToArray();
                    if (ProtectLoader.Debug)
                    {
                        File.WriteAllBytes(Path.Combine(Application.dataPath, Class3.smethod_10(0xf6e) + Path.GetFileName(str2) + Class3.smethod_10(0xf84)), bytes);
                    }
                    string str3 = Encoding.ASCII.GetString(bytes);
                    string str4 = Encoding.Unicode.GetString(bytes);
                    string str5 = Encoding.BigEndianUnicode.GetString(bytes);
                    if (((str5.IndexOf(Class3.smethod_10(0xf90)) <= 0) && (str5.IndexOf(Class3.smethod_10(0xfac)) <= 0)) && (str3.IndexOf(Class3.smethod_10(0xfc6)) <= 0))
                    {
                        if ((str5.IndexOf(Class3.smethod_10(0x109c)) <= 0) && (str5.IndexOf(Class3.smethod_10(0x10b6)) <= 0))
                        {
                            if ((str3.IndexOf(Class3.smethod_10(0xe74)) <= 0) && (str5.IndexOf(Class3.smethod_10(0x11a2)) <= 0))
                            {
                                if (((str3.IndexOf(Class3.smethod_10(0x11b8)) <= 0) && (str4.IndexOf(Class3.smethod_10(0x11b8)) <= 0)) && (str4.IndexOf(Class3.smethod_10(0x11b8)) <= 0))
                                {
                                    goto Label_06AA;
                                }
                                smethod_0(Class3.smethod_10(0xe8e), playerClient.userName + Class3.smethod_10(0xef4), true);
                            }
                            else
                            {
                                smethod_0(Class3.smethod_10(0xe8e), playerClient.userName + Class3.smethod_10(0xef4), true);
                            }
                        }
                        else
                        {
                            smethod_0(Class3.smethod_10(0x10ce), playerClient.userName + Class3.smethod_10(0x112e), true);
                        }
                    }
                    else
                    {
                        smethod_0(Class3.smethod_10(0xfdc), playerClient.userName + Class3.smethod_10(0x1032), true);
                    }
                    goto Label_0723;
                }
                Label_06AA:
                if (!list_1.Contains(str2))
                {
                    list_1.Add(str2);
                }
                if (!list_0.Contains(current.th32ProcessID))
                {
                    list_0.Add(current.th32ProcessID);
                }
                goto Label_0723;
                Label_06EA:
                smethod_0(Class3.smethod_10(0xdbe), playerClient.userName + Class3.smethod_10(0xe18), true);
            }
            Label_0723:
            Thread.Sleep(250);
            goto Label_0001;
        }





        /*public static void Initialize(string url, int port, byte[] assembly_bytes)
		{
			Protection.string_1 = url;
			Protection.int_0 = port;
			if (!Process32.IsRunAsAdministrator)
			{
				UnityEngine.Debug.LogError(Class3.smethod_10(4556));
				UnityEngine.Debug.LogError(Class3.smethod_10(4744));
				UnityEngine.Debug.LogError(Class3.smethod_10(4886));
				UnityEngine.Debug.LogError(Class3.smethod_10(4556));
				return;
			}
			if (ProtectLoader.Debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					Class3.smethod_10(5030),
					url,
					Class3.smethod_10(840),
					port
				}));
			}
			Protection.Steam_ID = ClientConnect.Steam_GetSteamID();
			Protection.Username = Marshal.PtrToStringAnsi(ClientConnect.Steam_GetDisplayname());
			Protection.string_3 = Protection.hardware_0.String_0;
			Protection.string_4 = Application.systemLanguage.ToString();
			if (ProtectLoader.Debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					Class3.smethod_10(5118),
					Protection.Steam_ID,
					Class3.smethod_10(5142),
					Protection.Username,
					Class3.smethod_10(5168),
					Protection.string_4,
					Class3.smethod_10(5194),
					Protection.string_3
				}));
			}
			Protection.EncryptionKey = new SHA512CryptoServiceProvider().ComputeHash(assembly_bytes);
			Protection.network_0 = new NetLink.Network(Protection.string_1, Protection.int_0);
			if (ProtectLoader.Debug)
			{
				UnityEngine.Debug.Log(Class3.smethod_10(5224) + Protection.network_0.Connected.ToString());
			}
			if (Protection.network_0.Connected && Protection.string_3 != null && Protection.string_3.Length == 32)
			{
				Assembly[] assemblies = Protection.Assemblies;
				NetLink.Network.Packet packet = new NetLink.Network.Packet(2, 1, null);
				packet.Write<ushort>(Protection.MessageType.Connect);
				packet.Write<ulong>(Protection.Steam_ID);
				packet.Write<string>(Protection.Username);
				packet.Write<string>(Protection.string_3);
				packet.Write<int>(assemblies.Length);
				Assembly[] array = assemblies;
				for (int i = 0; i < array.Length; i++)
				{
					Assembly assembly = array[i];
					packet.Write<string>(assembly.GetName().Name);
				}
				Protection.network_0.Send(packet);
				Protection.packet_0 = Protection.network_0.Receive(2000L);
				if (ProtectLoader.Debug)
				{
					UnityEngine.Debug.Log(Class3.smethod_10(5270) + Protection.packet_0.Received.ToString());
				}
				if (Protection.packet_0.Received && packet_0.Type == 4 && Protection.packet_0.Flags.Has(1))
				{
					Protection.bool_0 = Protection.packet_0.Read<bool>();
					int num = Protection.packet_0.Read<int>();
					Protection.verifyFile_0 = new VerifyFile[num];
					for (int j = 0; j < num; j++)
					{
						Protection.verifyFile_0[j] = new VerifyFile
						{
							Filename = Protection.packet_0.Read<string>(),
							Filesize = Protection.packet_0.Read<long>()
						};
					}
					if (ProtectLoader.Debug)
					{
						UnityEngine.Debug.Log(Class3.smethod_10(5314) + num);
					}
					Protection.gameObject_0 = new GameObject(typeof(Protection).FullName);
					Protection.gameObject_0.AddComponent<Protection>();
					Protection.gameObject_0.AddComponent<Snapshot>();
					return;
				}
			}
			Protection.network_0.Dispose();
		}*/
        public static void Initialize(string url, int port, byte[] assembly_bytes)
        {
            string_1 = url;
            int_0 = port;
            if (!Process32.IsRunAsAdministrator)
            {
                UnityEngine.Debug.LogError(Class3.smethod_10(0x11cc));
                UnityEngine.Debug.LogError(Class3.smethod_10(0x1288));
                UnityEngine.Debug.LogError(Class3.smethod_10(0x1316));
                UnityEngine.Debug.LogError(Class3.smethod_10(0x11cc));
            }
            else
            {
                Steam_ID = ClientConnect.Steam_GetSteamID();
                Username = Marshal.PtrToStringAnsi(ClientConnect.Steam_GetDisplayname());
                string_3 = hardware_0.String_0;
                string_4 = Application.systemLanguage.ToString();
                EncryptionKey = new SHA512CryptoServiceProvider().ComputeHash(assembly_bytes);
                network_0 = new NetLink.Network(string_1, int_0);
                if ((network_0.Connected && (string_3 != null)) && (string_3.Length == 0x20))
                {
                    Assembly[] assemblies = Assemblies;
                    NetLink.Network.Packet packet = new NetLink.Network.Packet(NetLink.Network.PacketType.Firstpass, NetLink.Network.PacketFlag.Compressed, null);
                    packet.Write<ushort>(MessageType.Connect);
                    packet.Write<ulong>(Steam_ID);
                    packet.Write<string>(Username);
                    packet.Write<string>(string_3);
                    packet.Write<int>(assemblies.Length);
                    foreach (Assembly assembly in assemblies)
                    {
                        packet.Write<string>(assembly.GetName().Name);
                    }
                    network_0.Send(packet);
                    packet_0 = network_0.Receive(0x7d0L);
                    if ((packet_0.Received && (packet_0.Type == NetLink.Network.PacketType.Response)) && packet_0.Flags.Has<NetLink.Network.PacketFlag>(NetLink.Network.PacketFlag.Compressed))
                    {
                        bool_0 = packet_0.Read<bool>();
                        int num2 = packet_0.Read<int>();
                        verifyFile_0 = new VerifyFile[num2];
                        for (int i = 0; i < num2; i++)
                        {
                            verifyFile_0[i] = new VerifyFile { Filename = packet_0.Read<string>(), Filesize = packet_0.Read<long>() };
                        }
                        gameObject_0 = new GameObject(typeof(Protection).FullName);
                        gameObject_0.AddComponent<Protection>();
                        gameObject_0.AddComponent<Snapshot>();
                        return;
                    }
                }
                network_0.Dispose();
            }
        }



        private static void smethod_8(string string_5 = null)
		{
			if (string_5 != null && string_5.Length > 0)
			{
				ChatUI.AddLine(Class3.smethod_10(5354), Class3.smethod_10(5384) + string_5 + Class3.smethod_10(5416));
			}
			NetCull.isMessageQueueRunning = true;
			NetCull.Disconnect();
		}

		private static bool smethod_9(UnityEngine.Object object_0, out BlueprintDataBlock blueprintDataBlock_0)
		{
			ItemDataBlock[] all = DatablockDictionary.All;
			for (int i = 0; i < all.Length; i++)
			{
				BlueprintDataBlock blueprintDataBlock = all[i] as BlueprintDataBlock;
				if (blueprintDataBlock != null && blueprintDataBlock.resultItem == object_0)
				{
					blueprintDataBlock_0 = blueprintDataBlock;
					return true;
				}
			}
			blueprintDataBlock_0 = null;
			return false;
		}
	}
}
