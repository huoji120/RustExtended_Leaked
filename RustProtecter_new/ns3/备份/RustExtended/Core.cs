namespace RustExtended
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    public class Core
    {
        public static bool AdminGodmode = true;
        public static bool AdminInstantDestory = true;
        public static bool Airdrop = true;
        public static bool AirdropAnnounce = true;
        public static int[] AirdropDrops = new int[] { 1, 3 };
        public static bool AirdropDropTime = false;
        public static int[] AirdropDropTimeHours = new int[] { 13, 0x13 };
        public static bool AirdropInterval = false;
        public static int AirdropIntervalTime = 0xe10;
        public static int AirdropPlanes = 1;
        public static bool AnnounceAdminConnect = false;
        public static bool AnnounceDeathMurder = true;
        public static bool AnnounceDeathNPC = true;
        public static bool AnnounceDeathSelf = true;
        public static bool AnnounceInvisConnect = false;
        public static bool AnnounceKillNotice = false;
        public static bool AnnouncePlayerJoin = true;
        public static bool AnnouncePlayerLeave = true;
        public static bool AssemblyVerifed = false;
        public static ulong AutoRestart = 0L;
        public static ulong AutoShutdown = 0L;
        public static uint AvatarAutoSaveInterval = 0x6d6;
        public static bool BetaVersion = false;
        public static string ChatClanColor = "#7FFF7F";
        public static string ChatClanIcon = "CLAN";
        public static string ChatClanKey = ".";
        public static string ChatCommandKey = "/";
        public static bool ChatConsole = false;
        public static string ChatConsoleColor = "#FFFFFF";
        public static string ChatConsoleName = "SERVER CONSOLE";
        public static bool ChatDisplayClan = false;
        public static bool ChatDisplayRank = false;
        public static string ChatDivider = " | ";
        public static bool ChatHistoryCommands = true;
        public static int ChatHistoryDisplay = 0x19;
        public static bool ChatHistoryPrivate = true;
        public static int ChatHistoryStored = 100;
        public static int ChatLineMaxLength = 0;
        public static int ChatMuteDefaultTime = 0x708;
        public static Dictionary<ulong, UserQuery> ChatQuery = new Dictionary<ulong, UserQuery>();
        public static string ChatSayColor = "#FFFFFF";
        public static int ChatSayDistance = 200;
        public static string ChatSayIcon = "CHAT";
        public static string ChatSystemColor = "#7FFFFF";
        public static string ChatWhisperColor = "#FF7FFF";
        public static int ChatWhisperDistance = 50;
        public static string ChatWhisperIcon = "WHISPER";
        public static string ChatWhisperKey = "@";
        public static string ChatYellColor = "#FFFF7F";
        public static int ChatYellDistance = 0;
        public static string ChatYellIcon = "YELL";
        public static string ChatYellKey = "!";
        public static int CommandHomeCountdown = 600;
        public static bool CommandHomeOutdoorsOnly = false;
        public static ulong CommandHomePayment = 0L;
        public static int CommandHomeTimewait = 30;
        public static int CommandNoPVPCountdown = 0xbb8;
        public static int CommandNoPVPDuration = 600;
        public static int CommandNoPVPTimewait = 10;
        public static System.Collections.Generic.List<string> Commands = new System.Collections.Generic.List<string>();
        public static int CommandTeleportCountdown = 600;
        public static bool CommandTeleportOutdoorsOnly = false;
        public static ulong CommandTeleportPayment = 0L;
        public static int CommandTeleportTimewait = 30;
        public static string[] CommandTransferForbidden = new string[0];
        public static bool CycleInstantCraft = false;
        public static int CycleInstantCraftOff = 0;
        public static int CycleInstantCraftOn = 6;
        public static bool CyclePvP = false;
        public static int CyclePvPOff = 0x17;
        public static int CyclePvPOn = 7;
        public static string DatabaseType = "FILE";
        public static string DataPath = "rust_server_Data";
        public static bool DecayObjects = true;
        public static Dictionary<ulong, DateTime> DestoryOwnership = new Dictionary<ulong, DateTime>();
        public static Dictionary<string, string> DestoryResources = new Dictionary<string, string>();
        public static bool ErrorsRestart = false;
        public static bool ErrorsShutdown = false;
        public static long ErrorsThreshold = 100L;
        public static string ExternalIP = "";
        public static System.Collections.Generic.List<string> ForbiddenObscene = new System.Collections.Generic.List<string>();
        public static System.Collections.Generic.List<string> ForbiddenUsername = new System.Collections.Generic.List<string>();
        public static string[] GenerateOutput = new string[0];
        public static string[] GenerateSource = new string[0];
        public static GenericSpawner[] GenericSpawners;
        public static int GenericSpawnsCount = 0;
        public static int GenericSpawnsTotal = 0;
        public static bool HasFakeOnline = false;
        public static bool HasShutdown = false;
        public static Dictionary<ulong, System.Collections.Generic.List<HistoryRecord>> History = new Dictionary<ulong, System.Collections.Generic.List<HistoryRecord>>();
        public static bool Initialized = false;
        public static Hashtable Kits = new Hashtable();
        public static string[] Languages = new string[] { "ENG", "RUS" };
        public static System.Collections.Generic.List<LoadoutEntry> Loadout = new System.Collections.Generic.List<LoadoutEntry>();
        public static bool LoadoutInitialized = false;
        public static string LogsPath = "logs";
        public static string MagmaVersion = Magma.Bootstrap.Version;
        public static string MySQL_Database = "server_rust";
        public static string MySQL_Host = "127.0.0.1";
        public static bool MySQL_Initialized = false;
        public static uint MySQL_LogLevel = 1;
        public static string MySQL_Password = "";
        public static uint MySQL_Port = 0xcea;
        public static bool MySQL_Synchronize = false;
        public static uint MySQL_SyncInterval = 0x1388;
        public static string MySQL_Username = "root";
        public static bool MySQL_UTF8 = true;
        public static bool NoticeConnectedAdmin = true;
        public static bool NoticeConnectedPlayer = true;
        public static float ObjectLootableLifetime = 1800f;
        public static bool OverrideDamage = false;
        public static bool OverrideItems = false;
        public static bool OverrideLoots = false;
        public static bool OverrideSpawns = false;
        public static bool OwnershipAttackedAnnounce = true;
        public static bool OwnershipAttackedPremiumOnly = false;
        public static int OwnershipBuildMaxComponents = 0;
        public static int OwnershipBuildMaxHeight = 0;
        public static int OwnershipBuildMaxLength = 0;
        public static int OwnershipBuildMaxWidth = 0;
        public static bool OwnershipDestroy = false;
        public static int OwnershipDestroyAutoDisable = 30;
        public static bool OwnershipDestroyInstant = false;
        public static bool OwnershipDestroyNoCarryWeight = true;
        public static bool OwnershipDestroyReceiveResources = false;
        public static int OwnershipMaxComponents = 0;
        public static bool OwnershipNotOwnerDenyBuild = false;
        public static string[] OwnershipNotOwnerDenyDeploy = new string[0];
        public static string[] OwnershipProtectContainer = new string[0];
        public static bool OwnershipProtectOfflineUser = false;
        public static bool OwnershipProtectPremiumUser = false;
        public static bool OwnershipProtectSharedUsers = false;
        public static bool PlayersFreezed = false;
        public static int PremiumConnections = 10;
        public static string ProductName = Assembly.GetExecutingAssembly().GetName().Name;
        public static Dictionary<int, string> RankColor = new Dictionary<int, string>();
        public static Dictionary<int, string> Ranks = new Dictionary<int, string>();
        public static Dictionary<ulong, NetUser> Reply = new Dictionary<ulong, NetUser>();
        public static float ResourcesAmountMultiplierFlay = 1f;
        public static float ResourcesAmountMultiplierRock = 1f;
        public static float ResourcesAmountMultiplierWood = 1f;
        public static float ResourcesGatherMultiplierFlay = 1f;
        public static float ResourcesGatherMultiplierRock = 1f;
        public static float ResourcesGatherMultiplierWood = 1f;
        public static int RestartTime = 120;
        public static string RootPath = "";
        public static string SavePath = "serverdata";
        public static string ServerIP = "";
        public static string ServerName = server.hostname;
        public static int ShutdownTime = 120;
        public static int SleepersLifeTime = 300;
        public static string SQL_SERVER_SET = "REPLACE INTO `db_server` (`name`, `value`) VALUES('{0}', {1});";
        public static string SteamAPIKey = "A029EB87E56667E9F8082AA051E501E8";
        public static bool SteamAuthUser = true;
        public static int[] SteamFakeOnline = new int[2];
        public static string[] SteamFavourite = new string[0];
        public static System.Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        public static bool VoiceNotification = true;
        public static int VoiceNotificationDelay = 0x5dc;
        public static bool WhitelistEnabled = false;
        public static int WhitelistInterval = 600;
        public static bool WhitelistRefresh = false;
        public static uint PlayerTeleportMethod = 0u;
        public static void GetSpawnersSpawns()
        {
            if ((GenericSpawners != null) && (GenericSpawners.Length != 0))
            {
                int num = 0;
                foreach (GenericSpawner spawner in GenericSpawners)
                {
                    foreach (GenericSpawnerSpawnList.GenericSpawnInstance instance in spawner._spawnList)
                    {
                        num += instance.spawned.Count;
                    }
                }
                GenericSpawnsCount = num;
            }
        }

        public static bool Initialize()
        {
            Initialized = AssemblyVerifed;
            if (!Initialized)
            {
                return false;
            }
            Helper.Log("RustExtended Initialization", true);
            World.Initialize();
            Zones.Initialize();
            Economy.Initialize();
            Helper.Log("RustExtended Economy " + (Economy.Enabled ? "Enabled" : "Disabled") + ".", true);
            Helper.Log("RustExtended Shopping " + (Shop.Enabled ? "Enabled" : "Disabled") + ".", true);
            Users.Initialize();
            Banned.Initialize();
            Clans.Initialize();
            Blocklist.Initialize();
            LoadoutInitialized = InitializeLoadout();
            if (Config.Initialized)
            {
                Helper.Log("  - " + PremiumConnections + " Allocated Premium Connection(s)", true);
                Helper.Log("  - " + Commands.Count + " Total Command(s)", true);
                Helper.Log("  - " + Ranks.Count + " Total Rank(s)", true);
                Helper.Log("  - " + Kits.Count + " Total Kit(s)", true);
                Helper.Log("  - " + ForbiddenUsername.Count + " Total Forbidden Name(s)", true);
                Helper.Log("  - " + ForbiddenObscene.Count + " Total Obscene(s)", true);
                Helper.Log("  - " + Events.Motd.Count + " Total Message Event(s)", true);
                Helper.Log("  - " + Clans.Levels.Count + " Total Clan Level(s)", true);
            }
            if (LoadoutInitialized)
            {
                Helper.Log("  - " + Loadout.Count + " Loadout", true);
            }
            if (Zones.Initialized)
            {
                Helper.Log("  - " + Zones.Count + " Total Zone(s)", true);
            }
            if (Users.Initialized)
            {
                Helper.Log("  - " + Users.Count + " Total User(s)", true);
            }
            if (Banned.Initialized)
            {
                Helper.Log("  - " + Banned.Count + " Banned User(s)", true);
            }
            if (Clans.Initialized)
            {
                Helper.Log("  - " + Clans.Count + " Total Clan(s)", true);
            }
            if (Economy.Enabled && Shop.Initialized)
            {
                Helper.Log("  - " + Shop.GroupCount + " Total Shop Group(s)", true);
                Helper.Log("  - " + Shop.ItemCount + " Total Shop Item(s)", true);
            }
            if (Blocklist.Initialized)
            {
                Helper.Log("  - " + Blocklist.Count + " Total Blocked IP", true);
            }
            if (Override.LootsFileCreated)
            {
                Helper.Log(" Loots file has been created.", true);
            }
            else if (OverrideLoots && Override.LootsInitialized)
            {
                Helper.Log("  - " + Override.LootsCount + " Overridden Loot(s)", true);
            }
            if (Override.ItemsFileCreated)
            {
                Helper.Log(" Items file has been created.", true);
            }
            else if (OverrideItems && Override.ItemsInitialized)
            {
                Helper.Log("  - " + Override.ItemsCount + " Overridden Item(s)", true);
            }
            Events.Initialize();
            GenericSpawners = UnityEngine.Object.FindObjectsOfType<GenericSpawner>();
            for (int i = 0; i < GenericSpawners.Length; i++)
            {
                int num2 = 0;
                switch (i)
                {
                    case 0x2b:
                        GenericSpawners[i].transform.position = new Vector3(6019f, 428.8f, -2296.3f);
                        break;

                    case 0x2c:
                        GenericSpawners[i].transform.position = new Vector3(5819f, 428.8f, -1896.3f);
                        break;
                }
                foreach (GenericSpawnerSpawnList.GenericSpawnInstance instance in GenericSpawners[i]._spawnList)
                {
                    GenericSpawnsTotal += instance.targetPopulation;
                    num2 += instance.targetPopulation;
                }
                Helper.Log(string.Format("[GenericSpawner #{0}] Position={1}, Radius={2}, Spawns={3}({4}), Think Delay={5}", new object[] { i, GenericSpawners[i].transform.position.AsString(), GenericSpawners[i].radius, GenericSpawners[i]._spawnList.Count, num2, GenericSpawners[i].thinkDelay }), false);
            }
            return Initialized;
        }

        public static bool InitializeLoadout()
        {
            Loadout.Clear();
            foreach (string str in Config.LoadoutList.Keys)
            {
                LoadoutEntry entry = new LoadoutEntry();
                Loadout.Add(entry);
                entry.Name = str;
                foreach (string str2 in Config.LoadoutList[str])
                {
                    ItemDataBlock block3;
                    string[] strArray = str2.Split(new char[] { '=' });
                    if (strArray.Length < 2)
                    {
                        continue;
                    }
                    strArray[0] = strArray[0].Trim();
                    strArray[1] = strArray[1].Trim(new char[] { '"', ' ' });
                    if (strArray[0].Equals("RANK", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
                        if (!string.IsNullOrEmpty(strArray[1]))
                        {
                            foreach (string str3 in strArray[1].Split(new char[] { ',' }))
                            {
                                int num;
                                if (int.TryParse(str3, out num) && !list.Contains(num))
                                {
                                    list.Add(num);
                                }
                            }
                        }
                        entry.Ranks = list.ToArray();
                        continue;
                    }
                    string[] strArray2 = strArray[1].Split(new char[] { ',' });
                    string name = strArray2[0];
                    int result = 1;
                    if (strArray2.Length > 1)
                    {
                        int.TryParse(strArray2[1], out result);
                    }
                    if (result < 1)
                    {
                        result = 1;
                    }
                    int num3 = -1;
                    if (strArray2.Length > 2)
                    {
                        int.TryParse(strArray2[2], out num3);
                    }
                    if (result < 1)
                    {
                        num3 = -1;
                    }
                    if (!strArray[0].Equals("NOCRAFTING", StringComparison.OrdinalIgnoreCase))
                    {
                        goto Label_029A;
                    }
                    string str5 = name.Trim(new char[] { '*', ' ' });
                    foreach (ItemDataBlock block in DatablockDictionary.All)
                    {
                        if (block is BlueprintDataBlock)
                        {
                            if (string.IsNullOrEmpty(str5))
                            {
                                entry.NoCrafting.Add((BlueprintDataBlock) block);
                            }
                            else if (((!name.StartsWith("*") || block.name.EndsWith(str5)) && (!name.EndsWith("*") || block.name.StartsWith(str5))) && block.name.Equals(str5, StringComparison.OrdinalIgnoreCase))
                            {
                                entry.NoCrafting.Add((BlueprintDataBlock)block);
                                str5 = null;
                                goto Label_025D;
                            }
                        }
                    }
                    goto Label_0272;
                Label_025D:

                Label_0272:
                    if (!string.IsNullOrEmpty(str5))
                    {
                        Helper.LogError("Loadout item \"" + name + "\" is not a blueprint.", true);
                    }
                    continue;
                Label_029A:
                    if (!strArray[0].Equals("BLUEPRINT", StringComparison.OrdinalIgnoreCase))
                    {
                        goto Label_03A5;
                    }
                    string str6 = name.Trim(new char[] { '*', ' ' });
                    foreach (ItemDataBlock block2 in DatablockDictionary.All)
                    {
                        if (block2 is BlueprintDataBlock)
                        {
                            if (string.IsNullOrEmpty(str6))
                            {
                                entry.Blueprints.Add((BlueprintDataBlock) block2);
                            }
                            else if (((!name.StartsWith("*") || block2.name.EndsWith(str6)) && (!name.EndsWith("*") || block2.name.StartsWith(str6))) && block2.name.Equals(str6, StringComparison.OrdinalIgnoreCase))
                            {
                                entry.Blueprints.Add((BlueprintDataBlock)block2);
                                str6 = null;
                                goto Label_0368;
                            }
                        }
                    }
                    goto Label_037D;
                Label_0368:

                Label_037D:
                    if (!string.IsNullOrEmpty(str6))
                    {
                        Helper.LogError("Loadout item \"" + name + "\" is not a blueprint.", true);
                    }
                    continue;
                Label_03A5:
                    block3 = DatablockDictionary.GetByName(name);
                    if (block3 == null)
                    {
                        Helper.LogError("Loadout item \"" + name + "\" not exists in datablock dictionary.", true);
                    }
                    else
                    {
                        LoadoutItem item = new LoadoutItem {
                            ItemBlock = block3,
                            Quantity = result,
                            ModSlots = num3
                        };
                        if (strArray[0].Equals("LOADOUTITEM", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, block3.IsSplittable(), Inventory.Slot.Kind.Belt);
                            entry.LoadoutItems.Add(item);
                        }
                        if (strArray[0].Equals("LOADOUTITEM.BELT", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, block3.IsSplittable(), Inventory.Slot.Kind.Default);
                            entry.LoadoutItems.Add(item);
                        }
                        if (strArray[0].Equals("LOADOUTITEM.ARMOR", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, block3.IsSplittable(), Inventory.Slot.Kind.Default);
                            entry.LoadoutItems.Add(item);
                        }
                        if (strArray[0].Equals("REQUIREMENT", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, block3.IsSplittable(), Inventory.Slot.Kind.Belt);
                            if (block3.category == ItemDataBlock.ItemCategory.Survival)
                            {
                                item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, block3.IsSplittable(), Inventory.Slot.KindFlags.Default);
                            }
                            if (block3.category == ItemDataBlock.ItemCategory.Weapons)
                            {
                                item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, block3.IsSplittable(), Inventory.Slot.KindFlags.Default);
                            }
                            if (block3.category == ItemDataBlock.ItemCategory.Armor)
                            {
                                item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, block3.IsSplittable(), Inventory.Slot.KindFlags.Default);
                            }
                            entry.Requirements.Add(item);
                        }
                    }
                }
            }
            return true;
        }

        public static void SQL_UpdateServer()
        {
            MySQL.Result result = MySQL.Query("SELECT * FROM `db_server`;", true);
            if (result != null)
            {
                foreach (MySQL.Row row in result.Row)
                {
                    if (row.Get("name").AsString.Equals("rcon_command", StringComparison.OrdinalIgnoreCase) && !row.Get("value").IsNull)
                    {
                        ConsoleSystem.Arg arg = new ConsoleSystem.Arg(row.Get("value").AsString);
                        if (!ConsoleSystem.RunCommand(ref arg, true))
                        {
                            ConsoleSystem.PrintError(arg.Reply, false);
                        }
                        MySQL.Update(string.Format(SQL_SERVER_SET, row.Get("name").AsString, "NULL"));
                    }
                }
            }
            MySQL.Update(string.Format(SQL_SERVER_SET, "time_update", MySQL.QuoteString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
        }

        public static bool Debug
        {
            get
            {
                return extended.debug;
            }
            set
            {
                extended.debug = value;
            }
        }
    }
}

