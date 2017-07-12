using Magma;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RustExtended
{
	public class Core
	{
		public static string ProductName;

		public static Version Version;

		public static string MagmaVersion;

		public static bool AssemblyVerifed;

		public static bool BetaVersion;

		public static bool Initialized;

		public static bool HasShutdown;

		public static bool MySQL_Initialized;

		public static string dlquser;

		public static string dlqpassword;

		public static uint dlqport;

		public static string dlqdb;

		public static string dlqip;

		public static bool dlqshop;

		public static int ronglu;

		public static bool loutijz;

		public static bool mrqd;

		public static bool xzjz;

		public static bool Chatjuli;

		public static bool xtf;

		public static string ServerIP;

		public static string ExternalIP;

		public static string ServerName;

		public static string SteamAPIKey;

		public static bool SteamAuthUser;

		public static bool HasFakeOnline;

		public static int[] SteamFakeOnline;

		public static string[] SteamFavourite;

		public static string[] Languages;

		public static long ErrorsThreshold;

		public static bool ErrorsShutdown;

		public static bool ErrorsRestart;

		public static string RootPath;

		public static string DataPath;

		public static string SavePath;

		public static string LogsPath;

		public static string[] GenerateSource;

		public static string[] GenerateOutput;

		public static string MySQL_Host;

		public static uint MySQL_Port;

		public static string MySQL_Username;

		public static string MySQL_Password;

		public static string MySQL_Database;

		public static bool MySQL_Synchronize;

		public static uint MySQL_SyncInterval;

		public static uint MySQL_LogLevel;

		public static bool MySQL_UTF8;

		public static string DatabaseType;

		public static bool OverrideLoots;

		public static bool OverrideItems;

		public static bool OverrideSpawns;

		public static bool OverrideDamage;

		public static int PremiumConnections;

		public static int SleepersLifeTime;

		public static bool DecayObjects;

		public static int ChatTime;

		public static int ShutdownTime;

		public static int RestartTime;

		public static ulong AutoShutdown;

		public static ulong AutoRestart;

		public static bool AdminGodmode;

		public static bool AdminInstantDestory;

		public static uint AvatarAutoSaveInterval;

		public static float ObjectLootableLifetime;

		public static bool WhitelistEnabled;

		public static bool WhitelistRefresh;

		public static int WhitelistInterval;

		public static bool Airdrop;

		public static bool AirdropAnnounce;

		public static bool AirdropDropTime;

		public static int[] AirdropDropTimeHours;

		public static bool AirdropInterval;

		public static int AirdropIntervalTime;

		public static int AirdropPlanes;

		public static int[] AirdropDrops;

		public static bool AnnouncePlayerJoin;

		public static bool AnnouncePlayerLeave;

		public static bool AnnounceAdminConnect;

		public static bool AnnounceInvisConnect;

		public static bool AnnounceDeathNPC;

		public static bool AnnounceDeathSelf;

		public static bool AnnounceDeathMurder;

		public static bool AnnounceKillNotice;

		public static bool NoticeConnectedPlayer;

		public static bool NoticeConnectedAdmin;

		public static bool CyclePvP;

		public static bool dlq;

		public static bool dlq1;

		public static int CyclePvPOff;

		public static int CyclePvPOn;

		public static bool CycleInstantCraft;

		public static int CycleInstantCraftOff;

		public static int CycleInstantCraftOn;

		public static int ChatLineMaxLength;

		public static string ChatSystemColor;

		public static string ChatSayIcon;

		public static string ChatSayColor;

		public static int ChatSayDistance;

		public static string ChatYellKey;

		public static string ChatYellIcon;

		public static string ChatYellColor;

		public static int ChatYellDistance;

		public static string ChatWhisperKey;

		public static string ChatWhisperIcon;

		public static string ChatWhisperColor;

		public static int ChatWhisperDistance;

		public static string ChatClanKey;

		public static string ChatClanColor;

		public static string ChatClanIcon;

		public static string ChatCommandKey;

		public static string ChatDivider;

		public static bool ChatConsole;

		public static string ChatConsoleName;

		public static string ChatConsoleColor;

		public static bool ChatDisplayRank;

		public static bool ChatDisplayClan;

		public static bool ChatHistoryPrivate;

		public static bool ChatHistoryCommands;

		public static int ChatHistoryStored;

		public static int ChatHistoryDisplay;

		public static int ChatMuteDefaultTime;

		public static bool VoiceNotification;

		public static int VoiceNotificationDelay;

		public static bool OwnershipDestroy;

		public static bool OwnershipDestroyInstant;

		public static int OwnershipDestroyAutoDisable;

		public static bool OwnershipDestroyNoCarryWeight;

		public static bool OwnershipDestroyReceiveResources;

		public static bool OwnershipProtectPremiumUser;

		public static bool OwnershipProtectOfflineUser;

		public static bool OwnershipProtectSharedUsers;

		public static string[] OwnershipProtectContainer;

		public static bool OwnershipAttackedAnnounce;

		public static bool OwnershipAttackedPremiumOnly;

		public static bool OwnershipNotOwnerDenyBuild;

		public static string[] OwnershipNotOwnerDenyDeploy;

		public static int OwnershipBuildMaxComponents;

		public static int OwnershipBuildMaxHeight;

		public static int OwnershipBuildMaxLength;

		public static int OwnershipBuildMaxWidth;

		public static int OwnershipMaxComponents;

		public static float ResourcesAmountMultiplierWood;

		public static float ResourcesAmountMultiplierRock;

		public static float ResourcesAmountMultiplierFlay;

		public static float ResourcesGatherMultiplierWood;

		public static float ResourcesGatherMultiplierRock;

		public static float ResourcesGatherMultiplierFlay;

		public static string[] CommandTransferForbidden;

		public static ulong CommandHomePayment;

		public static int CommandHomeTimewait;

		public static int CommandHomeCountdown;

		public static bool CommandHomeOutdoorsOnly;

		public static ulong CommandTeleportPayment;

		public static int CommandTeleportTimewait;

		public static int CommandTeleportCountdown;

		public static bool CommandTeleportOutdoorsOnly;

		public static int CommandNoPVPTimewait;

		public static int CommandNoPVPDuration;

		public static int CommandNoPVPCountdown;

		public static List<string> Commands;

		public static List<string> ForbiddenUsername;

		public static List<string> ForbiddenObscene;

		public static Dictionary<ulong, UserQuery> ChatQuery;

		public static Dictionary<ulong, List<HistoryRecord>> History;

		public static Dictionary<int, string> Ranks;

		public static Dictionary<int, string> RankColor;

		public static Dictionary<ulong, NetUser> Reply;

		public static Hashtable Kits;

		public static Dictionary<ulong, DateTime> DestoryOwnership;

		public static Dictionary<string, string> DestoryResources;

		public static bool PlayersFreezed;

		public static bool LoadoutInitialized;

		public static List<LoadoutEntry> Loadout;

		public static GenericSpawner[] GenericSpawners;

		public static int GenericSpawnsCount;

		public static int GenericSpawnsTotal;

		public static string SQL_SERVER_SET;

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

		public static bool Initialize()
		{
			Core.Initialized = Core.AssemblyVerifed;
			bool result;
			if (!Core.Initialized)
			{
				result = false;
			}
			else
			{
				Helper.Log("RustExtended Initialization", true);
				World.Initialize();
				Zones.Initialize();
				Economy.Initialize();
				Users.Initialize();
				Banned.Initialize();
				Clans.Initialize();
				Blocklist.Initialize();
				Core.LoadoutInitialized = Core.InitializeLoadout();
				if (Spawns.Singleton != null)
				{
					Spawns.Singleton.Initialize();
				}
				if (Config.Initialized)
				{
					Helper.Log("  - " + Core.PremiumConnections + " Allocated Premium Connection(s)", true);
					Helper.Log("  - " + Core.Commands.Count + " Total Command(s)", true);
					Helper.Log("  - " + Core.Ranks.Count + " Total Rank(s)", true);
					Helper.Log("  - " + Core.Kits.Count + " Total Kit(s)", true);
					Helper.Log("  - " + Core.ForbiddenUsername.Count + " Total Forbidden Name(s)", true);
					Helper.Log("  - " + Core.ForbiddenObscene.Count + " Total Obscene(s)", true);
					Helper.Log("  - " + Events.Motd.Count + " Total Message Event(s)", true);
					Helper.Log("  - " + Clans.Levels.Count + " Total Clan Level(s)", true);
				}
				if (Core.LoadoutInitialized)
				{
				}
				if (Zones.Initialized)
				{
				}
				if (Users.Initialized)
				{
				}
				if (Banned.Initialized)
				{
				}
				if (Clans.Initialized)
				{
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
				if (!Override.LootsFileCreated)
				{
					if (Core.OverrideLoots && Override.LootsInitialized)
					{
					}
				}
				if (!Override.ItemsFileCreated)
				{
					if (Core.OverrideItems && Override.ItemsInitialized)
					{
					}
				}
				if (Spawns.Initialized)
				{
					Helper.Log("  - " + Spawns.TotalGeneric + " Generic Spawner(s)", true);
					Helper.Log("  - " + Spawns.TotalLootable + " Lootable Spawner(s)", true);
				}
				Events.Initialize();
				result = Core.Initialized;
			}
			return result;
		}

		public static void GetSpawnersSpawns()
		{
			if (Core.GenericSpawners != null && Core.GenericSpawners.Length != 0)
			{
				int num = 0;
				GenericSpawner[] genericSpawners = Core.GenericSpawners;
				for (int i = 0; i < genericSpawners.Length; i++)
				{
					GenericSpawner genericSpawner = genericSpawners[i];
					foreach (GenericSpawnerSpawnList.GenericSpawnInstance current in genericSpawner._spawnList)
					{
						num += current.spawned.Count;
					}
				}
				Core.GenericSpawnsCount = num;
			}
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
                    ItemDataBlock block;
                    ItemDataBlock block2;
                    ItemDataBlock block4;
                    string[] strArray = str2.Split(new char[] { '=' });
                    if (strArray.Length < 2)
                    {
                        continue;
                    }
                    strArray[0] = strArray[0].Trim();
                    strArray[1] = strArray[1].Trim(new char[] { '"', ' ' });
                    if (strArray[0].Equals("RANK", StringComparison.OrdinalIgnoreCase))
                    {
                        List<int> list = new List<int>();
                        if (!string.IsNullOrEmpty(strArray[1]))
                        {
                            foreach (string str3 in strArray[1].Split(new char[] { ',' }))
                            {
                                int num;
                                if (!(!int.TryParse(str3, out num) || list.Contains(num)))
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
                        goto Label_0346;
                    }
                    string str5 = name.Trim(new char[] { '*', ' ' });
                    foreach (ItemDataBlock block3 in DatablockDictionary.All)
                    {
                        if (block3 is BlueprintDataBlock)
                        {
                            if (string.IsNullOrEmpty(str5))
                            {
                                entry.NoCrafting.Add((BlueprintDataBlock)block3);
                            }
                            else if (((!name.StartsWith("*") || block3.name.EndsWith(str5)) && (!name.EndsWith("*") || block3.name.StartsWith(str5))) && block3.name.Equals(str5, StringComparison.OrdinalIgnoreCase))
                            {
                                block2 = block3;
                                goto Label_0303;
                            }
                        }
                    }
                    goto Label_031A;
                Label_0303:
                    entry.NoCrafting.Add((BlueprintDataBlock)block2);
                    str5 = null;
                Label_031A:
                    if (!string.IsNullOrEmpty(str5))
                    {
                        Helper.LogError("Loadout item \"" + name + "\" is not a blueprint.", true);
                    }
                    continue;
                Label_0346:
                    if (!strArray[0].Equals("BLUEPRINT", StringComparison.OrdinalIgnoreCase))
                    {
                        goto Label_0492;
                    }
                    string str6 = name.Trim(new char[] { '*', ' ' });
                    foreach (ItemDataBlock block5 in DatablockDictionary.All)
                    {
                        if (block5 is BlueprintDataBlock)
                        {
                            if (string.IsNullOrEmpty(str6))
                            {
                                entry.Blueprints.Add((BlueprintDataBlock)block5);
                            }
                            else if (((!name.StartsWith("*") || block5.name.EndsWith(str6)) && (!name.EndsWith("*") || block5.name.StartsWith(str6))) && block5.name.Equals(str6, StringComparison.OrdinalIgnoreCase))
                            {
                                block4 = block5;
                                goto Label_044F;
                            }
                        }
                    }
                    goto Label_0466;
                Label_044F:
                    entry.Blueprints.Add((BlueprintDataBlock)block4);
                    str6 = null;
                Label_0466:
                    if (!string.IsNullOrEmpty(str6))
                    {
                        Helper.LogError("Loadout item \"" + name + "\" is not a blueprint.", true);
                    }
                    continue;
                Label_0492:
                    block = DatablockDictionary.GetByName(name);
                    if (block == null)
                    {
                        Helper.LogError("Loadout item \"" + name + "\" not exists in datablock dictionary.", true);
                    }
                    else
                    {
                        LoadoutItem item = new LoadoutItem
                        {
                            ItemBlock = block,
                            Quantity = result,
                            ModSlots = num3
                        };
                        if (strArray[0].Equals("LOADOUTITEM", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, block.IsSplittable(), Inventory.Slot.Kind.Belt);
                            entry.LoadoutItems.Add(item);
                        }
                        if (strArray[0].Equals("LOADOUTITEM.BELT", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, block.IsSplittable(), Inventory.Slot.Kind.Default);
                            entry.LoadoutItems.Add(item);
                        }
                        if (strArray[0].Equals("LOADOUTITEM.ARMOR", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, block.IsSplittable(), Inventory.Slot.Kind.Default);
                            entry.LoadoutItems.Add(item);
                        }
                        if (strArray[0].Equals("REQUIREMENT", StringComparison.OrdinalIgnoreCase))
                        {
                            item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, block.IsSplittable(), Inventory.Slot.Kind.Belt);
                            if (block.category == ItemDataBlock.ItemCategory.Survival)
                            {
                                item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, block.IsSplittable(), Inventory.Slot.KindFlags.Default);
                            }
                            if (block.category == ItemDataBlock.ItemCategory.Weapons)
                            {
                                item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, block.IsSplittable(), Inventory.Slot.KindFlags.Default);
                            }
                            if (block.category == ItemDataBlock.ItemCategory.Armor)
                            {
                                item.SlotReference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, block.IsSplittable(), Inventory.Slot.KindFlags.Default);
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
				foreach (MySQL.Row current in result.Row)
				{
					if (current.Get("name").AsString.Equals("rcon_command", StringComparison.OrdinalIgnoreCase) && !current.Get("value").IsNull)
					{
						ConsoleSystem.Arg arg = new ConsoleSystem.Arg(current.Get("value").AsString);
						if (!ConsoleSystem.RunCommand(ref arg, true))
						{
							ConsoleSystem.PrintError(arg.Reply, false);
						}
						MySQL.Update(string.Format(Core.SQL_SERVER_SET, current.Get("name").AsString, "NULL"));
					}
				}
			}
			MySQL.Update(string.Format(Core.SQL_SERVER_SET, "time_update", MySQL.QuoteString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
		}

		static Core()
		{
			Core.ProductName = Assembly.GetExecutingAssembly().GetName().Name;
			Core.Version = Assembly.GetExecutingAssembly().GetName().Version;
			Core.MagmaVersion = Magma.Bootstrap.Version;
			Core.AssemblyVerifed = false;
			Core.BetaVersion = false;
			Core.Initialized = false;
			Core.HasShutdown = false;
			Core.MySQL_Initialized = false;
			Core.dlquser = "root";
			Core.dlqpassword = "root";
			Core.dlqport = 3306u;
			Core.dlqdb = "dlq";
			Core.dlqip = "127.0.0.1";
			Core.dlqshop = false;
			Core.ronglu = 30;
			Core.loutijz = false;
			Core.mrqd = false;
			Core.xzjz = false;
			Core.Chatjuli = false;
			Core.xtf = false;
			Core.ServerIP = "";
			Core.ExternalIP = "";
			Core.ServerName = server.hostname;
			Core.SteamAPIKey = "A029EB87E56667E9F8082AA051E501E8";
			Core.SteamAuthUser = true;
			Core.HasFakeOnline = false;
			int[] steamFakeOnline = new int[2];
			Core.SteamFakeOnline = steamFakeOnline;
			Core.SteamFavourite = new string[0];
			Core.Languages = new string[]
			{
				"ENG",
				"RUS"
			};
			Core.ErrorsThreshold = 100L;
			Core.ErrorsShutdown = false;
			Core.ErrorsRestart = false;
			Core.RootPath = "";
			Core.DataPath = "rust_server_Data";
			Core.SavePath = "serverdata";
			Core.LogsPath = "logs";
			Core.GenerateSource = new string[0];
			Core.GenerateOutput = new string[0];
			Core.MySQL_Host = "127.0.0.1";
			Core.MySQL_Port = 3306u;
			Core.MySQL_Username = "root";
			Core.MySQL_Password = "";
			Core.MySQL_Database = "server_rust";
			Core.MySQL_Synchronize = false;
			Core.MySQL_SyncInterval = 5000u;
			Core.MySQL_LogLevel = 1u;
			Core.MySQL_UTF8 = true;
			Core.DatabaseType = "FILE";
			Core.dlq = false;
			Core.dlq1 = false;
			Core.xtf = false;
			Core.mrqd = false;
			Core.OverrideLoots = false;
			Core.OverrideItems = false;
			Core.OverrideSpawns = false;
			Core.OverrideDamage = false;
			Core.PremiumConnections = 10;
			Core.SleepersLifeTime = 300;
			Core.DecayObjects = true;
			Core.ShutdownTime = 120;
			Core.RestartTime = 120;
			Core.AutoShutdown = 0uL;
			Core.AutoRestart = 0uL;
			Core.AdminGodmode = true;
			Core.AdminInstantDestory = true;
			Core.AvatarAutoSaveInterval = 1750u;
			Core.ObjectLootableLifetime = 1800f;
			Core.WhitelistEnabled = false;
			Core.WhitelistRefresh = false;
			Core.WhitelistInterval = 600;
			Core.Airdrop = true;
			Core.AirdropAnnounce = true;
			Core.AirdropDropTime = false;
			Core.AirdropDropTimeHours = new int[]
			{
				13,
				19
			};
			Core.AirdropInterval = false;
			Core.AirdropIntervalTime = 3600;
			Core.AirdropPlanes = 1;
			Core.AirdropDrops = new int[]
			{
				1,
				3
			};
			Core.AnnouncePlayerJoin = true;
			Core.AnnouncePlayerLeave = true;
			Core.AnnounceAdminConnect = false;
			Core.AnnounceInvisConnect = false;
			Core.AnnounceDeathNPC = true;
			Core.AnnounceDeathSelf = true;
			Core.AnnounceDeathMurder = true;
			Core.AnnounceKillNotice = false;
			Core.NoticeConnectedPlayer = true;
			Core.NoticeConnectedAdmin = true;
			Core.CyclePvP = false;
			Core.CyclePvPOff = 23;
			Core.CyclePvPOn = 7;
			Core.CycleInstantCraft = false;
			Core.CycleInstantCraftOff = 0;
			Core.CycleInstantCraftOn = 6;
			Core.ChatLineMaxLength = 0;
			Core.ChatTime = 1;
			Core.ChatSystemColor = "#7FFFFF";
			Core.ChatSayIcon = "CHAT";
			Core.ChatSayColor = "#FFFFFF";
			Core.ChatSayDistance = 200;
			Core.ChatYellKey = "!";
			Core.ChatYellIcon = "YELL";
			Core.ChatYellColor = "#FFFF7F";
			Core.ChatYellDistance = 0;
			Core.ChatWhisperKey = "@";
			Core.ChatWhisperIcon = "WHISPER";
			Core.ChatWhisperColor = "#FF7FFF";
			Core.ChatWhisperDistance = 50;
			Core.ChatClanKey = ".";
			Core.ChatClanColor = "#7FFF7F";
			Core.ChatClanIcon = "CLAN";
			Core.ChatCommandKey = "/";
			Core.ChatDivider = " | ";
			Core.ChatConsole = false;
			Core.ChatConsoleName = "SERVER CONSOLE";
			Core.ChatConsoleColor = "#FFFFFF";
			Core.ChatDisplayRank = false;
			Core.ChatDisplayClan = false;
			Core.ChatHistoryPrivate = true;
			Core.ChatHistoryCommands = true;
			Core.ChatHistoryStored = 100;
			Core.ChatHistoryDisplay = 25;
			Core.ChatMuteDefaultTime = 1800;
			Core.VoiceNotification = true;
			Core.VoiceNotificationDelay = 1500;
			Core.OwnershipDestroy = false;
			Core.OwnershipDestroyInstant = false;
			Core.OwnershipDestroyAutoDisable = 30;
			Core.OwnershipDestroyNoCarryWeight = true;
			Core.OwnershipDestroyReceiveResources = false;
			Core.OwnershipProtectPremiumUser = false;
			Core.OwnershipProtectOfflineUser = false;
			Core.OwnershipProtectSharedUsers = false;
			Core.OwnershipProtectContainer = new string[0];
			Core.OwnershipAttackedAnnounce = true;
			Core.OwnershipAttackedPremiumOnly = false;
			Core.OwnershipNotOwnerDenyBuild = false;
			Core.OwnershipNotOwnerDenyDeploy = new string[0];
			Core.OwnershipBuildMaxComponents = 0;
			Core.OwnershipBuildMaxHeight = 0;
			Core.OwnershipBuildMaxLength = 0;
			Core.OwnershipBuildMaxWidth = 0;
			Core.OwnershipMaxComponents = 0;
			Core.ResourcesAmountMultiplierWood = 1f;
			Core.ResourcesAmountMultiplierRock = 1f;
			Core.ResourcesAmountMultiplierFlay = 1f;
			Core.ResourcesGatherMultiplierWood = 1f;
			Core.ResourcesGatherMultiplierRock = 1f;
			Core.ResourcesGatherMultiplierFlay = 1f;
			Core.CommandTransferForbidden = new string[0];
			Core.CommandHomePayment = 0uL;
			Core.CommandHomeTimewait = 30;
			Core.CommandHomeCountdown = 600;
			Core.CommandHomeOutdoorsOnly = false;
			Core.CommandTeleportPayment = 0uL;
			Core.CommandTeleportTimewait = 30;
			Core.CommandTeleportCountdown = 600;
			Core.CommandTeleportOutdoorsOnly = false;
			Core.CommandNoPVPTimewait = 10;
			Core.CommandNoPVPDuration = 600;
			Core.CommandNoPVPCountdown = 3000;
			Core.Commands = new List<string>();
			Core.ForbiddenUsername = new List<string>();
			Core.ForbiddenObscene = new List<string>();
			Core.ChatQuery = new Dictionary<ulong, UserQuery>();
			Core.History = new Dictionary<ulong, List<HistoryRecord>>();
			Core.Ranks = new Dictionary<int, string>();
			Core.RankColor = new Dictionary<int, string>();
			Core.Reply = new Dictionary<ulong, NetUser>();
			Core.Kits = new Hashtable();
			Core.DestoryOwnership = new Dictionary<ulong, DateTime>();
			Core.DestoryResources = new Dictionary<string, string>();
			Core.PlayersFreezed = false;
			Core.LoadoutInitialized = false;
			Core.Loadout = new List<LoadoutEntry>();
			Core.GenericSpawnsCount = 0;
			Core.GenericSpawnsTotal = 0;
			Core.SQL_SERVER_SET = "REPLACE INTO `db_server` (`name`, `value`) VALUES('{0}', {1});";
		}
	}
}
