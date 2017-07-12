namespace RustProtect
{
    using NetLink;
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
    using UnityEngine;

    public class Protection : MonoBehaviour
    {
        private static bool bool_0;
        private static bool bool_1 = false;
        public static byte[] EncryptionKey = null;
        private static float float_0 = 0f;
        private static GameObject gameObject_0;
        private static Hardware hardware_0 = new Hardware();
        private static int int_0 = 0x6d6f;
        private static System.Collections.Generic.List<uint> list_0 = new System.Collections.Generic.List<uint>();
        private static System.Collections.Generic.List<string> list_1 = new System.Collections.Generic.List<string>();
        private static System.Collections.Generic.List<string> list_2 = new System.Collections.Generic.List<string>();
        private static NetLink.Network network_0;
        private static NetLink.Network.Packet packet_0;
        public static PlayerClient playerClient;
        private static Process process_0 = null;
        [CompilerGenerated]
        private static Protection protection_0;
        public static byte[] Screenshot = null;
        public static ulong Steam_ID;
        [CompilerGenerated]
        private static string string_0;
        private static string string_1 = Class3.smethod_10(0x153c);
        private static string[] string_2 = new string[0];
        private static string string_3;
        private static string string_4;
        private static Thread thread_0;
        private static Thread thread_1;
        private static ulong ulong_0 = ulong.MaxValue;
        public static string Username;
        private static VerifyFile[] verifyFile_0 = new VerifyFile[0];

        private void Awake()
        {
            Singleton = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            list_0.Clear();
            list_1.Clear();
            list_2.Clear();
            process_0 = Process.GetCurrentProcess();
            ApplicationPath = Path.GetDirectoryName(process_0.MainModule.FileName);
            ApplicationPath = string_0 + Path.DirectorySeparatorChar.ToString();
            thread_0 = new Thread(new ThreadStart(Protection.smethod_7));
            thread_0.Start();
            base.InvokeRepeating(Class3.smethod_10(0x290), 0f, 0.1f);
            base.InvokeRepeating(Class3.smethod_10(0x2a6), 0f, 1f);
            UnityEngine.Debug.Log(Class3.smethod_10(0x2ca));
        }

        public void DoNetwork()
        {
            if (((network_0 != null) && network_0.Connected) && (packet_0 = network_0.Receive(0L)).Received)
            {
                if (ProtectLoader.Debug)
                {
                    UnityEngine.Debug.Log(string.Concat(new object[] { Class3.smethod_10(0x36a), packet_0.Type, Class3.smethod_10(0x37e), packet_0.Received.ToString(), Class3.smethod_10(920), packet_0.Length, Class3.smethod_10(0x3ae), packet_0.RemainingBytes, Class3.smethod_10(970), packet_0.Flags, Class3.smethod_10(0x3dc) }));
                }
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
                        MessageType message = (MessageType) ((short) packet_0.Read<ushort>());
                        this.DoNetworkMessageData(packet_0, message);
                    }
                }
            }
        }

        public void DoNetworkMessageData(NetLink.Network.Packet packet, MessageType message)
        {
            if (ProtectLoader.Debug)
            {
                UnityEngine.Debug.Log(string.Concat(new object[] { Class3.smethod_10(0x418), network_0.RemoteEndPoint, Class3.smethod_10(0x456), message }));
            }
            if (message == MessageType.Checksum)
            {
                int num2 = packet_0.Read<int>(); //服务器返回要读取什么
                verifyFile_0 = new VerifyFile[num2];
                for (int i = 0; i < num2; i++)
                {
                    verifyFile_0[i] = new VerifyFile { Filename = packet_0.Read<string>(), Filesize = packet_0.Read<long>() };
                }
                thread_1 = new Thread(new ThreadStart(Protection.smethod_6));
                thread_1.Start();
            }
            else if (message == MessageType.Screenshot)
            {
                RustProtect.Snapshot.Singleton.CaptureSnapshot();
                base.InvokeRepeating(Class3.smethod_10(0x4a4), 0f, 0.1f);
            }
            else if (message == MessageType.OverrideItems)
            {
                int num4 = packet.Read<int>();
                for (int j = 0; j < num4; j++)
                {
                    try
                    {
                        BlueprintDataBlock block2;
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
                        if (packet.Read<bool>() && smethod_9(byName, out block2))
                        {
                            block2.numResultItem = packet.Read<int>();
                            block2.craftingDuration = packet.Read<float>();
                            block2.RequireWorkbench = packet.Read<bool>();
                            int num5 = packet.Read<int>();
                            System.Collections.Generic.List<BlueprintDataBlock.IngredientEntry> list = new System.Collections.Generic.List<BlueprintDataBlock.IngredientEntry>();
                            for (int k = 0; k < num5; k++)
                            {
                                BlueprintDataBlock.IngredientEntry item = new BlueprintDataBlock.IngredientEntry {
                                    amount = packet.Read<int>(),
                                    Ingredient = DatablockDictionary.GetByName(packet.Read<string>())
                                };
                                if ((item.amount > 0) && (item.Ingredient != null))
                                {
                                    list.Add(item);
                                }
                            }
                            block2.ingredients = list.ToArray();
                        }
                    }
                    catch (Exception)
                    {
                        UnityEngine.Debug.Log(Class3.smethod_10(0x460));
                        return;
                    }
                }
            }
        }

        public void DoScanningPlayer()
        {
            if (NetCull.isClientRunning && !bool_1)
            {
                bool_1 = true;
                if (((playerClient != null) && (playerClient.controllable != null)) && (playerClient.controllable.character != null))
                {
                    Character character = playerClient.controllable.character;
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
                        if (character.ccmotor.movement.setup.gravity != 35f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxFallSpeed != 80f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxForwardSpeed != 4f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxSidewaysSpeed != 4f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxBackwardsSpeed != 3f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxAirAcceleration != 20f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxGroundAcceleration != 100f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                        if (character.ccmotor.movement.setup.maxAirHorizontalSpeed != 750f)
                        {
                            smethod_0(Class3.smethod_10(0x4f8), playerClient.userName + Class3.smethod_10(0x550), true);
                        }
                    }
                    InventoryHolder component = character.GetComponent<InventoryHolder>();
                    if (((component != null) && (component.itemRepresentation != null)) && (component.inputItem.datablock is BulletWeaponDataBlock))
                    {
                        BulletWeaponDataBlock datablock = (BulletWeaponDataBlock) component.inputItem.datablock;
                        uint uniqueID = (uint) datablock.uniqueID;
                        string s = ((((((((((((string.Empty + datablock.bulletRange + Class3.smethod_10(0x5c0)) + datablock.reloadDuration + Class3.smethod_10(0x5c0)) + datablock.fireRate + Class3.smethod_10(0x5c0)) + datablock.fireRateSecondary + Class3.smethod_10(0x5c0)) + datablock.recoilYawMin + Class3.smethod_10(0x5c0)) + datablock.recoilYawMax + Class3.smethod_10(0x5c0)) + datablock.recoilPitchMin + Class3.smethod_10(0x5c0)) + datablock.recoilPitchMax + Class3.smethod_10(0x5c0)) + datablock.recoilDuration + Class3.smethod_10(0x5c0)) + datablock.aimSway + Class3.smethod_10(0x5c0)) + datablock.aimSwaySpeed + Class3.smethod_10(0x5c0)) + datablock.aimingRecoilSubtract + Class3.smethod_10(0x5c0)) + datablock.crouchRecoilSubtract;
                        ulong num2 = BitConverter.ToUInt64(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(s)), 0);
                        if (ProtectLoader.Debug)
                        {
                            object[] objArray1 = new object[] { Class3.smethod_10(0x5c6), datablock.name, Class3.smethod_10(0x5da), uniqueID, Class3.smethod_10(0x5e8), num2.ToString(Class3.smethod_10(0x5fe)), Class3.smethod_10(0x608), s, Class3.smethod_10(0x61a) };
                            string contents = string.Concat(objArray1);
                            File.AppendAllText(Path.Combine(Application.dataPath, Class3.smethod_10(0x624)), contents);
                        }
                        if ((uniqueID == 0x5edbe45e) && (num2 != 18345605656075165559L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0x40594e58) && (num2 != 10722295551327323350L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0xa737e2c9) && (num2 != 10546893518047341734L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0xb9f4fdc4) && (num2 != 16251289927229003782L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0xfc0ea92d) && (num2 != 0x3f87c80b4d43e85eL))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0x521f5a20) && (num2 != 14126027389018450226L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0x27d2154f) && (num2 != 0x6326a38eae13859fL))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0x9199f266) && (num2 != 9331602331646563999L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                        if ((uniqueID == 0x208aabfc) && (num2 != 17058071295959981111L))
                        {
                            smethod_0(Class3.smethod_10(0x654), playerClient.userName + Class3.smethod_10(0x6a8), true);
                        }
                    }
                }
                bool_1 = false;
            }
        }
        public static bool firstBB = false;
        public static bool lastBB = true;
        public void DoSnapshot()
        {
            if ((network_0.Connected && base.IsInvoking()) && (Screenshot != null))
            {
                firstBB = true;
                base.CancelInvoke(Class3.smethod_10(0x4a4));
                NetLink.Network.Packet packet = new NetLink.Network.Packet(NetLink.Network.PacketType.DataStream, NetLink.Network.PacketFlag.Compressed, null);
                packet.Write<ushort>(MessageType.Screenshot);
                packet.Write<string>(Steam_ID.ToString());
                packet.Write(Screenshot, 0, Screenshot.Length);
                network_0.Send(packet);
                Screenshot = null;
                lastBB = false;
            }
        }

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
                if (ProtectLoader.Debug)
                {
                    UnityEngine.Debug.Log(string.Concat(new object[] { Class3.smethod_10(0x13a6), url, Class3.smethod_10(840), port }));
                }
                Steam_ID = ClientConnect.Steam_GetSteamID();
                Username = Marshal.PtrToStringAnsi(ClientConnect.Steam_GetDisplayname());
                string_3 = hardware_0.String_0;
                string_4 = Application.systemLanguage.ToString();
                if (ProtectLoader.Debug)
                {
                    UnityEngine.Debug.Log(string.Concat(new object[] { Class3.smethod_10(0x13fe), Steam_ID, Class3.smethod_10(0x1416), Username, Class3.smethod_10(0x1430), string_4, Class3.smethod_10(0x144a), string_3 }));
                }
                EncryptionKey = new SHA512CryptoServiceProvider().ComputeHash(assembly_bytes);
                network_0 = new NetLink.Network(string_1, int_0);
                if (ProtectLoader.Debug)
                {
                    UnityEngine.Debug.Log(Class3.smethod_10(0x1468) + network_0.Connected.ToString());
                }
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
                    if (ProtectLoader.Debug)
                    {
                        UnityEngine.Debug.Log(Class3.smethod_10(0x1496) + packet_0.Received.ToString());
                    }
                    if ((packet_0.Received && (packet_0.Type == NetLink.Network.PacketType.Response)) && packet_0.Flags.Has<NetLink.Network.PacketFlag>(NetLink.Network.PacketFlag.Compressed))
                    {
                        bool_0 = packet_0.Read<bool>();
                        int num2 = packet_0.Read<int>();
                        verifyFile_0 = new VerifyFile[num2];
                        for (int i = 0; i < num2; i++)
                        {
                            verifyFile_0[i] = new VerifyFile { Filename = packet_0.Read<string>(), Filesize = packet_0.Read<long>() };
                            UnityEngine.Debug.Log(Class3.smethod_10(0x14c2) + packet_0.Read<string>());
                        }
                        if (ProtectLoader.Debug)
                        {
                            UnityEngine.Debug.Log(Class3.smethod_10(0x14c2) + num2);
                        }
                        gameObject_0 = new GameObject(typeof(Protection).FullName);
                        gameObject_0.AddComponent<Protection>();
                        gameObject_0.AddComponent<RustProtect.Snapshot>();
                        return;
                    }
                }
                network_0.Dispose();
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
                UnityEngine.Debug.Log(Class3.smethod_10(0x2fe));
            }
        }

        private void OnDestroy()
        {
            if (protection_0 == this)
            {
                Singleton = null;
            }
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

        private static long smethod_1(Stream stream_0, string string_5)
        {
            long num = -1L;
            byte[] buffer = new byte[0x1000];
            while (true)
            {
                if (stream_0.Read(buffer, 0, buffer.Length) > 0L)
                {
                    num = smethod_2(buffer, string_5);
                }
                if (num == -1L)
                {
                    return num;
                }
            }
        }

        private static long smethod_2(byte[] object_0, string string_5)
        {
            return smethod_4(object_0, (long) object_0.Length, string_5);
        }

        private static long smethod_3(byte[] object_0, object[] object_1)
        {
            return smethod_5(object_0, (long) object_0.Length, object_1);
        }

        private static long smethod_4(byte[] object_0, long long_0, string string_5)
        {
            int num = 0;
            if (long_0 > object_0.Length)
            {
                long_0 = object_0.Length;
            }
            for (int i = 0; i < long_0; i++)
            {
                if (object_0[i] == string_5[num])
                {
                    num++;
                }
                else
                {
                    num = 0;
                }
                if (num == string_5.Length)
                {
                    return (long) (i - string_5.Length);
                }
            }
            return -1L;
        }

        private static long smethod_5(byte[] object_0, long long_0, object[] object_1)
        {
            int[] numArray = new int[object_1.Length];
            if (long_0 > object_0.Length)
            {
                long_0 = object_0.Length;
            }
            for (int i = 0; i < long_0; i++)
            {
                for (int j = 0; j < numArray.Length; j++)
                {
                    byte huoji_1 = object_0[i];
                    byte huoji_2 = (byte) object_1[j];
                    if (huoji_1 == huoji_2)
                    {
                        numArray[j]++;
                    }
                    else
                    {
                        numArray[j] = 0;
                    }
                    if (numArray[j] == object_1[j].ToString().Length)
                    {
                        return (long) (i - object_1[j].ToString().Length);
                    }
                }
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
                    IntPtr processHandle = Process32.OpenProcess(Process32.PROCESS_QUERY_INFORMATION | Process32.PROCESS_VM_READ, 0, (uint) process_0.Id);
                    if (processHandle == IntPtr.Zero)
                    {
                        ulong_0 = ulong.MaxValue;
                    }
                    else
                    {
                        Dictionary<string, MemoryAssemblyEntry> dictionary = new Dictionary<string, MemoryAssemblyEntry>();
                        uint num2 = Process32.smethod_3(processHandle, (long) (process_0.MainModule.BaseAddress.ToInt32() + 0xa1f9cc));
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
                                    num5 = Process32.smethod_3(processHandle, (long) (num2 + num3));
                                    if (num5 == 0)
                                    {
                                        continue;
                                    }
                                    num6 = Process32.smethod_3(processHandle, (long) (num5 + 20));
                                    if (num6 == 0)
                                    {
                                        continue;
                                    }
                                    string path = Process32.ReadString(processHandle, (long) num6);
                                    if (!path.ToLower().EndsWith(Class3.smethod_10(0x714)) || !File.Exists(path))
                                    {
                                        continue;
                                    }
                                    if (ProtectLoader.Debug)
                                    {
                                        UnityEngine.Debug.Log(Class3.smethod_10(0x736) + num5.ToString(Class3.smethod_10(0x758)));
                                    }
                                    flag = true;
                                }
                                num6 = Process32.smethod_3(processHandle, (long) (num2 + num3));
                                uint num8 = Process32.smethod_3(processHandle, (long) (num6 + 12));
                                if (num8 > 0)
                                {
                                    uint num7 = Process32.smethod_3(processHandle, (long) (num6 + 8));
                                    uint num9 = Process32.smethod_3(processHandle, (long) (num6 + 20));
                                    uint num10 = Process32.smethod_3(processHandle, (long) (num6 + 0x20));
                                    if (((num7 > 0) && (num9 > 0)) && (num10 > 0))
                                    {
                                        MemoryAssemblyEntry entry2 = new MemoryAssemblyEntry {
                                            Pointer = num7,
                                            Filesize = num8,
                                            Filepath = Process32.ReadString(processHandle, (long) num9),
                                            TargetRuntime = Process32.ReadString(processHandle, (long) num10)
                                        };
                                        if (File.Exists(entry2.Filepath))
                                        {
                                            //原来的 rustprotect
                                            string str2 = entry2.Filepath.Replace(string_0, "");
                                            dictionary[str2] = entry2;
                                            num4++;
                                            if (ProtectLoader.Debug)
                                            {
                                                UnityEngine.Debug.Log(string.Concat(new object[] { Class3.smethod_10(0x760), entry2.Pointer.ToString(Class3.smethod_10(0x758)), Class3.smethod_10(0x78c), entry2.Filesize, Class3.smethod_10(0x7b8), str2, Class3.smethod_10(0x7e4), entry2.Filepath, Class3.smethod_10(0x810), entry2.TargetRuntime }));
                                            }
                                        }
                                    }
                                }
                                continue;
                            }
                            catch (Exception exception)
                            {
                                ulong_0 = ulong.MaxValue;
                                if (ProtectLoader.Debug)
                                {
                                    UnityEngine.Debug.LogError(exception.ToString());
                                }
                                return;
                            }
                        }
                        if (!flag)
                        {
                            ulong_0 = ulong.MaxValue;
                            if (ProtectLoader.Debug)
                            {
                                UnityEngine.Debug.LogError(Class3.smethod_10(0x848));
                            }
                        }
                        else
                        {
                            // verifyFile_0 是\rust\xxxx什么的
                            foreach (VerifyFile file in verifyFile_0)
                            {
                                //file是个文件名字 rustprotect.dll什么的
                                MemoryAssemblyEntry entry3; //定义一个entry3的变量
                                if (!File.Exists(file.Filename))
                                {
                                    break;
                                }
                                contents = contents + file.Filename + Class3.smethod_10(0x8a4);
                                string str4 = Path.GetFileName(file.Filename).Replace(Class3.smethod_10(0x8ac), Class3.smethod_10(0x8b2));
                                if (dictionary.TryGetValue(file.Filename, out entry3)) //输出长度
                                {
                                    
                                    if (file.Filesize != entry3.Filesize) //文件的长度不等于这个的长度就退出
                                    {
                                        break;
                                    }
                                    byte[] buffer = Process32.ReadBytes(processHandle, entry3.Pointer, (int)file.Filesize);
                                    
                                    if ((buffer == null) || (buffer.Length != entry3.Filesize))
                                    {
                                        break;
                                    }
                                    /*
                                    if (Path.GetFileName(file.Filename) == "RustProtect.dll")
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(buffer), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes("C:\\rust_Data\\Managed\\RustProtect.dll")), 0);
                                    }
                                    else
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(buffer), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }*/
                                    UnityEngine.Debug.LogError(Class3.smethod_10(0x848) + file.Filename);
                                    maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(buffer), 0);
                                    maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                }
                                else if ((str4.Contains(Class3.smethod_10(0x8b8)) && !(str4 == Class3.smethod_10(0x8ce))) && !(Path.GetExtension(str4).ToLower() != Class3.smethod_10(0x8f0)))
                                {
                                    UnityEngine.Debug.LogError(Class3.smethod_10(0x8fc) + str4);
                                }
                                else
                                {
                                    if (Path.GetFileName(file.Filename) == "RustProtect.dll")
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes("C:\\RustProtect.dll")), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }
                                    else
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes(file.Filename)), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }
                                    /*
                                    if (Path.GetFileName(file.Filename) == "RustProtect.dll")
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes("C:\\RustProtect.dll")), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);

                                    }
                                    else if (Path.GetFileName(file.Filename) == "UnityEngine.dll")
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes("C:\\UnityEngine.dll")), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }
                                    else if (Path.GetFileName(file.Filename) == "UnityEngine.Cloud.Analytics.dll")
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes("C:\\UnityEngine.Cloud.Analytics.dll")), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }
                                    else if (Path.GetFileName(file.Filename) == "UnityEngine.Cloud.Analytics.Util.dll")
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes("C:\\UnityEngine.Cloud.Analytics.Util.dll")), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }
                                    else
                                    {
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes(file.Filename)), 0);
                                        maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                    }*/

                                    //  maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(File.ReadAllBytes(file.Filename)), 0);
                                    //  maxValue ^= BitConverter.ToUInt64(provider.ComputeHash(Encoding.UTF8.GetBytes(file.Filename)), 0);
                                }
                            }
                            Process32.CloseHandle(processHandle);
                            ulong_0 = maxValue;
                            if (ProtectLoader.Debug)
                            {
                                contents = contents + Class3.smethod_10(0x93e) + ulong_0.ToString(Class3.smethod_10(0x5fe));
                                File.WriteAllText(Path.Combine(Application.dataPath, Class3.smethod_10(0x95c)), contents); //路径 + 内容
                                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                                {
                                    list.Add(assembly.GetName().Name);
                                }
                                File.WriteAllLines(Path.Combine(Application.dataPath, Class3.smethod_10(0x984)), list.ToArray());
                            }
                            if ((network_0 != null) && network_0.Connected)
                            {
                                if (!huojisb)
                                {
                                    foreach (Assembly sb in AppDomain.CurrentDomain.GetAssemblies())
                                    {
                                        list2.Add(sb);
                                    }
                                    huojisb = true;
                                }

                                Assembly[] assemblies = list2.ToArray<Assembly>();

                                //ulong_0 = 0x989E8DA176BBC246;
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

        private static void smethod_7()
        {
        Label_0001:
            if (!NetCull.isClientRunning || (thread_0 == null))
            {
                return;
            }
            Path.Combine(Application.dataPath, Class3.smethod_10(0x9e2));
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
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
            using (System.Collections.Generic.List<ProcessEntry32>.Enumerator enumerator = Process32.GetProcess32List().GetEnumerator())
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
                    reader.BaseStream.Seek((long) (num + 6), SeekOrigin.Begin);
                    short num2 = reader.ReadInt16();
                    if (num2 > 7)
                    {
                        num2 = 7;
                    }
                    int num3 = num + 0x108;
                    for (int i = 0; i < num2; i++)
                    {
                        reader.BaseStream.Seek((long) num3, SeekOrigin.Begin);
                        uint num5 = reader.ReadUInt32();
                        uint num6 = reader.ReadUInt32();
                        if (num5 > 0x20000)
                        {
                            num5 = 0x20000;
                        }
                        reader.BaseStream.Seek((long) num6, SeekOrigin.Begin);
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

        private static void smethod_8([Optional, DefaultParameterValue(null)] string string_5)
        {
            if ((string_5 != null) && (string_5.Length > 0))
            {
                ChatUI.AddLine(Class3.smethod_10(0x14ea), Class3.smethod_10(0x1508) + string_5 + Class3.smethod_10(0x1528));
            }
            NetCull.isMessageQueueRunning = true;
            NetCull.Disconnect();
        }

        private static bool smethod_9(UnityEngine.Object object_0, out BlueprintDataBlock blueprintDataBlock_0)
        {
            ItemDataBlock[] all = DatablockDictionary.All;
            for (int i = 0; i < all.Length; i++)
            {
                BlueprintDataBlock block = all[i] as BlueprintDataBlock;
                if ((block != null) && (block.resultItem == object_0))
                {
                    blueprintDataBlock_0 = block;
                    return true;
                }
            }
            blueprintDataBlock_0 = null;
            return false;
        }

        public static string ApplicationPath
        {
            [CompilerGenerated]
            get
            {
                // return string_0;
                return "C:\\RUST";
            }
            [CompilerGenerated]
            private set
            {
                string_0 = value;
                string_0 = "C:\\RUST";
            }
        }

        public static bool huojisb = false;
        static List<Assembly> list2 = new List<Assembly>();
        public static Assembly[] Assemblies
        {
            get
            {
                if (!huojisb)
                {
                    foreach (Assembly sb in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        list2.Add(sb);
                    }
                    huojisb = true;
                }
                return list2.ToArray<Assembly>();
                //return AppDomain.CurrentDomain.GetAssemblies().ToArray<Assembly>();
            }
        }

        public static Protection Singleton
        {
            [CompilerGenerated]
            get
            {
                return protection_0;
            }
            [CompilerGenerated]
            private set
            {
                protection_0 = value;
            }
        }

        public enum MessageType : short
        {
            Approve = 2,
            Checksum = 4,
            Connect = 1,
            Fileslist = 0x10,
            KickMessage = 8,
            OverrideItems = 0x100,
            Screenshot = 0x40,
            Unknown = 0,
            UpdateFile = 0x20
        }
    }
}

