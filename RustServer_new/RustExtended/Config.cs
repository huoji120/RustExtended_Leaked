using Facepunch.Utility;
using Rust.Steam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RustExtended
{
	public class Config
	{
		private struct Struct0
		{
			public string string_0;

			public string string_1;

			public List<string> list_0;
		}

		[CompilerGenerated]
		private sealed class Class13
		{
			public string[] string_0;

			public bool method_0(MOTDEvent motdevent_0)
			{
				return motdevent_0.Title.Equals(this.string_0[1]);
			}
		}

		[CompilerGenerated]
		private sealed class Class14
		{
			public Config.Class13 class13_0;

			public int int_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return clanLevel_0.Id == this.int_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class15
		{
			public string string_0;

			public string string_1;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1 == this.string_1;
			}
		}

		[CompilerGenerated]
		private sealed class Class16
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class17
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class18
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class19
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class20
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class21
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class22
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class23
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class24
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class25
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class26
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class27
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class28
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class29
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class30
		{
			public string string_0;

			public string string_1;

			public bool bool_0;

			public bool method_0(Config.Struct0 struct0_0)
			{
				return struct0_0.string_0 == this.string_0 && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		[CompilerGenerated]
		private sealed class Class31
		{
			public ClanData clanData_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return clanLevel_0.RequireLevel == this.clanData_0.Level.Id;
			}
		}

		[CompilerGenerated]
		private sealed class Class32
		{
			public ClanData clanData_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return clanLevel_0.RequireLevel == this.clanData_0.Level.Id;
			}
		}

		private static List<Config.Struct0> list_0 = new List<Config.Struct0>();

		private static Config config_0 = new Config();

		[CompilerGenerated]
		private static string string_0;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static bool bool_1;

		[CompilerGenerated]
		private static Dictionary<string, List<string>> dictionary_0;

		public static Config Singleton
		{
			get
			{
				return Config.config_0;
			}
		}

		public static string FilePath
		{
			get;
			private set;
		}

		public static bool Initialized
		{
			get;
			private set;
		}

		public static bool Loading
		{
			get;
			private set;
		}

		public static Dictionary<string, List<string>> LoadoutList
		{
			get;
			private set;
		}

		private Config()
		{
			Config.Loading = false;
			Config.Initialized = false;
		}

		public static void Initialize()
		{
			Config.Loading = true;
			Config.FilePath = Path.Combine(Core.SavePath, "cfg\\RustExtended");
			if (!Directory.Exists(Config.FilePath))
			{
				Directory.CreateDirectory(Config.FilePath);
			}
			Config.list_0.Clear();
			Core.Commands.Clear();
			Core.Ranks.Clear();
			Core.Kits.Clear();
			Core.ForbiddenUsername.Clear();
			Core.DestoryResources.Clear();
			if (Config.LoadoutList == null)
			{
				Config.LoadoutList = new Dictionary<string, List<string>>();
			}
			Config.LoadoutList.Clear();
			if (Events.Motd != null)
			{
				foreach (MOTDEvent current in Events.Motd)
				{
					current.Dispose();
				}
			}
			Events.Motd.Clear();
			if (Clans.Levels == null)
			{
				Clans.Levels = new List<ClanLevel>();
			}
			Clans.Levels.Clear();
			if (Clans.CraftExperience == null)
			{
				Clans.CraftExperience = new Dictionary<string, int>();
			}
			Clans.CraftExperience.Clear();
			Config.Initialized = Config.Load();
			if (Config.Initialized)
			{
				Config.Get("SERVER", "ServerName", ref Core.ServerName, true);
				Config.Get("SERVER", "Languages", ref Core.Languages, true);
				Config.Get("SERVER", "ronglu", ref Core.ronglu, true);
				Config.Get("SERVER", "dlq", ref Core.dlq, true);
				Config.Get("SERVER", "dlq.ip", ref Core.dlqip, true);
				Config.Get("SERVER", "dlq.user", ref Core.dlquser, true);
				Config.Get("SERVER", "dlq.password", ref Core.dlqpassword, true);
				Config.Get("SERVER", "dlq.port", ref Core.dlqport, true);
				Config.Get("SERVER", "dlq.db", ref Core.dlqdb, true);
				Config.Get("SERVER", "dlq.shop", ref Core.dlqshop, true);
				Config.Get("SERVER", "dlq1", ref Core.dlq1, true);
				Config.Get("SERVER", "xtf", ref Core.xtf, true);
				Config.Get("SERVER", "每日签到", ref Core.mrqd, true);
				Config.Get("SERVER", "xzjz", ref Core.xzjz, true);
				Config.Get("SERVER", "ltjz", ref Core.loutijz, true);
				Config.Get("SERVER", "Chat.juli", ref Core.Chatjuli, true);
				Config.Get("SERVER", "Errors.Threshold", ref Core.ErrorsThreshold, true);
				Config.Get("SERVER", "Errors.Shutdown", ref Core.ErrorsShutdown, true);
				Config.Get("SERVER", "Errors.Restart", ref Core.ErrorsRestart, true);
				Config.Get("SERVER", "Steam.APIKey", ref Core.SteamAPIKey, true);
				Config.Get("SERVER", "Steam.AuthUser", ref Core.SteamAuthUser, true);
				Config.Get("SERVER", "Steam.SetOfficial", ref Server.Official, true);
				Config.Get("SERVER", "Steam.SetModded", ref Server.Modded, true);
				Config.Get("SERVER", "Steam.FakeOnline", ref Core.SteamFakeOnline, true);
				Config.Get("SERVER", "Steam.Favourite", ref Core.SteamFavourite, true);
				Config.Get("SERVER", "SavePath", ref Core.SavePath, true);
				Config.Get("SERVER", "LogsPath", ref Core.LogsPath, true);
				Config.Get("SERVER", "Generate.Source", ref Core.GenerateSource, true);
				Config.Get("SERVER", "Generate.Output", ref Core.GenerateOutput, true);
				Config.Get("SERVER", "Override.Loots", ref Core.OverrideLoots, true);
				Config.Get("SERVER", "Override.Items", ref Core.OverrideItems, true);
				Config.Get("SERVER", "Override.Spawns", ref Core.OverrideSpawns, true);
				Config.Get("SERVER", "Override.Damage", ref Core.OverrideDamage, true);
				Config.Get("SERVER", "PremiumConnections", ref Core.PremiumConnections, true);
				Config.Get("SERVER", "DecayObjects", ref Core.DecayObjects, true);
				Config.Get("SERVER", "SleepersLifeTime", ref Core.SleepersLifeTime, true);
				Config.Get("SERVER", "ShutdownTime", ref Core.ShutdownTime, true);
				Config.Get("SERVER", "RestartTime", ref Core.RestartTime, true);
				Config.Get("SERVER", "AutoShutdown", ref Core.AutoShutdown, true);
				Config.Get("SERVER", "AutoRestart", ref Core.AutoRestart, true);
				Config.Get("SERVER", "MySQL.Host", ref Core.MySQL_Host, true);
				Config.Get("SERVER", "MySQL.Port", ref Core.MySQL_Port, true);
				Config.Get("SERVER", "MySQL.Username", ref Core.MySQL_Username, true);
				Config.Get("SERVER", "MySQL.Password", ref Core.MySQL_Password, true);
				Config.Get("SERVER", "MySQL.Database", ref Core.MySQL_Database, true);
				Config.Get("SERVER", "MySQL.Synchronize", ref Core.MySQL_Synchronize, true);
				Config.Get("SERVER", "MySQL.SyncInterval", ref Core.MySQL_SyncInterval, true);
				Config.Get("SERVER", "MySQL.LogLevel", ref Core.MySQL_LogLevel, true);
				Config.Get("SERVER", "MySQL.UTF8", ref Core.MySQL_UTF8, true);
				Config.Get("SERVER", "Database.Type", ref Core.DatabaseType, true);
				Config.Get("SERVER", "Users.VerifyNames", ref Users.VerifyNames, true);
				Config.Get("SERVER", "Users.VerifyChars", ref Users.VerifyChars, true);
				Config.Get("SERVER", "Users.UniqueNames", ref Users.UniqueNames, true);
				Config.Get("SERVER", "Users.BindingNames", ref Users.BindingNames, true);
				Config.Get("SERVER", "Users.DefaultRank", ref Users.DefaultRank, true);
				Config.Get("SERVER", "Users.PremiumRank", ref Users.PremiumRank, true);
				Config.Get("SERVER", "Users.AutoAdminRank", ref Users.AutoAdminRank, true);
				Config.Get("SERVER", "Users.DisplayRank", ref Users.DisplayRank, true);
				Config.Get("SERVER", "Users.PingLimit", ref Users.PingLimit, true);
				Config.Get("SERVER", "Users.Network.Timeout", ref Users.NetworkTimeout, true);
				Config.Get("SERVER", "Users.MD5Password", ref Users.MD5Password, true);
				Config.Get("SERVER", "Admin.Godmode", ref Core.AdminGodmode, true);
				Config.Get("SERVER", "Admin.InstantDestory", ref Core.AdminInstantDestory, true);
				Config.Get("SERVER", "Avatar.AutoSave.Interval", ref Core.AvatarAutoSaveInterval, true);
				Config.Get("SERVER", "Object.Lootable.Lifetime", ref Core.ObjectLootableLifetime, true);
				Config.Get("SERVER", "Whitelist.Enabled", ref Core.WhitelistEnabled, true);
				Config.Get("SERVER", "Whitelist.Refresh", ref Core.WhitelistRefresh, true);
				Config.Get("SERVER", "Whitelist.Interval", ref Core.WhitelistInterval, true);
				Config.Get("SERVER", "Truth.Punishment", ref Truth.Punishment, true);
				Config.Get("SERVER", "Truth.ReportRank", ref Truth.ReportRank, true);
				Config.Get("SERVER", "Truth.ViolationColor", ref Truth.ViolationColor, true);
				Config.Get("SERVER", "Truth.MaxViolations", ref Truth.MaxViolations, true);
				Config.Get("SERVER", "Truth.ViolationDetails", ref Truth.ViolationDetails, true);
				Config.Get("SERVER", "Truth.ViolationTimelife", ref Truth.ViolationTimelife, true);
				Config.Get("SERVER", "Truth.CheckAimbot", ref Truth.CheckAimbot, true);
				Config.Get("SERVER", "Truth.CheckWallhack", ref Truth.CheckWallhack, true);
				Config.Get("SERVER", "Truth.CheckJumphack", ref Truth.CheckJumphack, true);
				Config.Get("SERVER", "Truth.CheckFallhack", ref Truth.CheckFallhack, true);
				Config.Get("SERVER", "Truth.CheckSpeedhack", ref Truth.CheckSpeedhack, true);
				Config.Get("SERVER", "Truth.CheckShotRange", ref Truth.CheckShotRange, true);
				Config.Get("SERVER", "Truth.CheckShotEyes", ref Truth.CheckShotEyes, true);
				Config.Get("SERVER", "Truth.ShotAboveMaxDistance", ref Truth.ShotAboveMaxDistance, true);
				Config.Get("SERVER", "Truth.ShotThroughObject.Block", ref Truth.ShotThroughObjectBlock, true);
				Config.Get("SERVER", "Truth.ShotThroughObject.Punish", ref Truth.ShotThroughObjectPunish, true);
				Config.Get("SERVER", "Truth.HeadshotThreshold", ref Truth.HeadshotThreshold, true);
				Config.Get("SERVER", "Truth.MaxMovementSpeed", ref Truth.MaxMovementSpeed, true);
				Config.Get("SERVER", "Truth.MaxJumpingHeight", ref Truth.MaxJumpingHeight, true);
				Config.Get("SERVER", "Truth.MinFallingHeight", ref Truth.MinFallingHeight, true);
				Config.Get("SERVER", "Truth.MinShotRateByRange", ref Truth.MinShotRateByRange, true);
				Config.Get("SERVER", "Truth.HeadshotAimTime", ref Truth.HeadshotAimTime, true);
				Config.Get("SERVER", "Truth.Banned.BlockIP", ref Truth.BannedBlockIP, true);
				Config.Get("SERVER", "Truth.Banned.Period", ref Truth.BannedPeriod, true);
				Config.Get("SERVER", "Truth.Banned.ExcludeIP", ref Truth.BannedExcludeIP, true);
				Config.Get("SERVER", "RustProtect", ref Truth.RustProtect, true);
				Config.Get("SERVER", "RustProtect.SteamHWID", ref Truth.RustProtectSteamHWID, true);
				Config.Get("SERVER", "RustProtect.Key", ref Truth.RustProtectKey, true);
				Config.Get("SERVER", "RustProtect.Hash", ref Truth.RustProtectHash, true);
				Config.Get("SERVER", "RustProtect.ChangeKey", ref Truth.RustProtectChangeKey, true);
				Config.Get("SERVER", "RustProtect.ChangeKey.Interval", ref Truth.RustProtectChangeKeyInterval, true);
				Config.Get("SERVER", "RustProtect.MaxTicks", ref Truth.RustProtectMaxTicks, true);
				Config.Get("SERVER", "RustProtect.Snapshots", ref Truth.RustProtectSnapshots, true);
				Config.Get("SERVER", "RustProtect.Snapshots.MaxCount", ref Truth.RustProtectSnapshotsMaxCount, true);
				Config.Get("SERVER", "RustProtect.Snapshots.Interval", ref Truth.RustProtectSnapshotsInterval, true);
				Config.Get("SERVER", "RustProtect.Snapshots.PacketSize", ref Truth.RustProtectSnapshotsPacketSize, true);
				Config.Get("SERVER", "RustProtect.Snapshots.Path", ref Truth.RustProtectSnapshotsPath, true);
				Config.Get("SERVER", "Airdrop.Enabled", ref Core.Airdrop, true);
				Config.Get("SERVER", "Airdrop.Announce", ref Core.AirdropAnnounce, true);
				Config.Get("SERVER", "Airdrop.DropTime", ref Core.AirdropDropTime, true);
				Config.Get("SERVER", "Airdrop.DropTime.Hours", ref Core.AirdropDropTimeHours, true);
				Config.Get("SERVER", "Airdrop.Interval", ref Core.AirdropInterval, true);
				Config.Get("SERVER", "Airdrop.Interval.Time", ref Core.AirdropIntervalTime, true);
				Config.Get("SERVER", "Airdrop.Planes", ref Core.AirdropPlanes, true);
				Config.Get("SERVER", "Airdrop.Drops", ref Core.AirdropDrops, true);
				Config.Get("SERVER", "Cycle.PvP", ref Core.CyclePvP, true);
				Config.Get("SERVER", "Cycle.PvP.Off", ref Core.CyclePvPOff, true);
				Config.Get("SERVER", "Cycle.PvP.On", ref Core.CyclePvPOn, true);
				Config.Get("SERVER", "Cycle.InstantCraft", ref Core.CycleInstantCraft, true);
				Config.Get("SERVER", "Cycle.InstantCraft.Off", ref Core.CycleInstantCraftOff, true);
				Config.Get("SERVER", "Cycle.InstantCraft.On", ref Core.CycleInstantCraftOn, true);
				Config.Get("SERVER", "Announce.PlayerJoin", ref Core.AnnouncePlayerJoin, true);
				Config.Get("SERVER", "Announce.PlayerLeave", ref Core.AnnouncePlayerLeave, true);
				Config.Get("SERVER", "Announce.AdminConnect", ref Core.AnnounceAdminConnect, true);
				Config.Get("SERVER", "Announce.DeathNPC", ref Core.AnnounceDeathNPC, true);
				Config.Get("SERVER", "Announce.DeathSelf", ref Core.AnnounceDeathSelf, true);
				Config.Get("SERVER", "Announce.DeathMurder", ref Core.AnnounceDeathMurder, true);
				Config.Get("SERVER", "Announce.KillNotice", ref Core.AnnounceKillNotice, true);
				Config.Get("SERVER", "Notice.Connected.Player", ref Core.NoticeConnectedPlayer, true);
				Config.Get("SERVER", "Notice.Connected.Admin", ref Core.NoticeConnectedAdmin, true);
				Config.Get("SERVER", "Chat.Line.MaxLength", ref Core.ChatLineMaxLength, true);
				Config.Get("SERVER", "Chat.Time", ref Core.ChatTime, true);
				Config.Get("SERVER", "Chat.System.Color", ref Core.ChatSystemColor, true);
				Config.Get("SERVER", "Chat.Say.Icon", ref Core.ChatSayIcon, true);
				Config.Get("SERVER", "Chat.Say.Color", ref Core.ChatSayColor, true);
				Config.Get("SERVER", "Chat.Say.Distance", ref Core.ChatSayDistance, true);
				Config.Get("SERVER", "Chat.Yell.Key", ref Core.ChatYellKey, true);
				Config.Get("SERVER", "Chat.Yell.Icon", ref Core.ChatYellIcon, true);
				Config.Get("SERVER", "Chat.Yell.Color", ref Core.ChatYellColor, true);
				Config.Get("SERVER", "Chat.Yell.Distance", ref Core.ChatYellDistance, true);
				Config.Get("SERVER", "Chat.Whisper.Key", ref Core.ChatWhisperKey, true);
				Config.Get("SERVER", "Chat.Whisper.Icon", ref Core.ChatWhisperIcon, true);
				Config.Get("SERVER", "Chat.Whisper.Color", ref Core.ChatWhisperColor, true);
				Config.Get("SERVER", "Chat.Whisper.Distance", ref Core.ChatWhisperDistance, true);
				Config.Get("SERVER", "Chat.Clan.Key", ref Core.ChatClanKey, true);
				Config.Get("SERVER", "Chat.Clan.Icon", ref Core.ChatClanIcon, true);
				Config.Get("SERVER", "Chat.Clan.Color", ref Core.ChatClanColor, true);
				Config.Get("SERVER", "Chat.Command.Key", ref Core.ChatCommandKey, true);
				Config.Get("SERVER", "Chat.Divider", ref Core.ChatDivider, true);
				Config.Get("SERVER", "Chat.Console", ref Core.ChatConsole, true);
				Config.Get("SERVER", "Chat.Console.Name", ref Core.ChatConsoleName, true);
				Config.Get("SERVER", "Chat.Console.Color", ref Core.ChatConsoleColor, true);
				Config.Get("SERVER", "Chat.Display.Rank", ref Core.ChatDisplayRank, true);
				Config.Get("SERVER", "Chat.Display.Clan", ref Core.ChatDisplayClan, true);
				Config.Get("SERVER", "Chat.History.Private", ref Core.ChatHistoryPrivate, true);
				Config.Get("SERVER", "Chat.History.Commands", ref Core.ChatHistoryCommands, true);
				Config.Get("SERVER", "Chat.History.Stored", ref Core.ChatHistoryStored, true);
				Config.Get("SERVER", "Chat.History.Display", ref Core.ChatHistoryDisplay, true);
				Config.Get("SERVER", "Chat.MuteDefaultTime", ref Core.ChatMuteDefaultTime, true);
				Config.Get("SERVER", "Voice.Notification", ref Core.VoiceNotification, true);
				Config.Get("SERVER", "Voice.NotificationDelay", ref Core.VoiceNotificationDelay, true);
				Config.Get("SERVER", "Ownership.Destroy", ref Core.OwnershipDestroy, true);
				Config.Get("SERVER", "Ownership.Destroy.Instant", ref Core.OwnershipDestroyInstant, true);
				Config.Get("SERVER", "Ownership.Destroy.AutoDisable", ref Core.OwnershipDestroyAutoDisable, true);
				Config.Get("SERVER", "Ownership.Destroy.NoCarryWeight", ref Core.OwnershipDestroyNoCarryWeight, true);
				Config.Get("SERVER", "Ownership.Destroy.ReceiveResources", ref Core.OwnershipDestroyReceiveResources, true);
				Config.Get("SERVER", "Ownership.Protect.PremiumUser", ref Core.OwnershipProtectPremiumUser, true);
				Config.Get("SERVER", "Ownership.Protect.OfflineUser", ref Core.OwnershipProtectOfflineUser, true);
				Config.Get("SERVER", "Ownership.Protect.SharedUsers", ref Core.OwnershipProtectSharedUsers, true);
				Config.Get("SERVER", "Ownership.Protect.Container", ref Core.OwnershipProtectContainer, true);
				Config.Get("SERVER", "Ownership.Attacked.Announce", ref Core.OwnershipAttackedAnnounce, true);
				Config.Get("SERVER", "Ownership.Attacked.PremiumOnly", ref Core.OwnershipAttackedPremiumOnly, true);
				Config.Get("SERVER", "Ownership.NotOwner.DenyBuild", ref Core.OwnershipNotOwnerDenyBuild, true);
				Config.Get("SERVER", "Ownership.NotOwner.DenyDeploy", ref Core.OwnershipNotOwnerDenyDeploy, true);
				Config.Get("SERVER", "Ownership.Build.MaxComponents", ref Core.OwnershipBuildMaxComponents, true);
				Config.Get("SERVER", "Ownership.Build.MaxHeight", ref Core.OwnershipBuildMaxHeight, true);
				Config.Get("SERVER", "Ownership.Build.MaxLength", ref Core.OwnershipBuildMaxLength, true);
				Config.Get("SERVER", "Ownership.Build.MaxWidth", ref Core.OwnershipBuildMaxWidth, true);
				Config.Get("SERVER", "Ownership.MaxComponents", ref Core.OwnershipMaxComponents, true);
				Config.Get("SERVER", "Resources.AmountMultiplier.Wood", ref Core.ResourcesAmountMultiplierWood, true);
				Config.Get("SERVER", "Resources.AmountMultiplier.Rock", ref Core.ResourcesAmountMultiplierRock, true);
				Config.Get("SERVER", "Resources.AmountMultiplier.Flay", ref Core.ResourcesAmountMultiplierFlay, true);
				Config.Get("SERVER", "Resources.GatherMultiplier.Wood", ref Core.ResourcesGatherMultiplierWood, true);
				Config.Get("SERVER", "Resources.GatherMultiplier.Rock", ref Core.ResourcesGatherMultiplierRock, true);
				Config.Get("SERVER", "Resources.GatherMultiplier.Flay", ref Core.ResourcesGatherMultiplierFlay, true);
				Config.Get("SERVER", "Command.Transfer.Forbidden", ref Core.CommandTransferForbidden, true);
				Config.Get("SERVER", "Command.Home.Payment", ref Core.CommandHomePayment, true);
				Config.Get("SERVER", "Command.Home.Timewait", ref Core.CommandHomeTimewait, true);
				Config.Get("SERVER", "Command.Home.Countdown", ref Core.CommandHomeCountdown, true);
				Config.Get("SERVER", "Command.Home.OutdoorsOnly", ref Core.CommandHomeOutdoorsOnly, true);
				Config.Get("SERVER", "Command.Teleport.Payment", ref Core.CommandTeleportPayment, true);
				Config.Get("SERVER", "Command.Teleport.Timewait", ref Core.CommandTeleportTimewait, true);
				Config.Get("SERVER", "Command.Teleport.Countdown", ref Core.CommandTeleportCountdown, true);
				Config.Get("SERVER", "Command.Teleport.OutdoorsOnly", ref Core.CommandTeleportOutdoorsOnly, true);
				Config.Get("SERVER", "Command.NoPVP.Timewait", ref Core.CommandNoPVPTimewait, true);
				Config.Get("SERVER", "Command.NoPVP.Duration", ref Core.CommandNoPVPDuration, true);
				Config.Get("SERVER", "Command.NoPVP.Countdown", ref Core.CommandNoPVPCountdown, true);
				Config.Get("CLANS", "Enabled", ref Clans.Enabled, true);
				Config.Get("CLANS", "DefaultLevel", ref Clans.DefaultLevel, true);
				Config.Get("CLANS", "CreateCost", ref Clans.CreateCost, true);
				Config.Get("CLANS", "Experience.Multiplier", ref Clans.ExperienceMultiplier, true);
				Config.Get("CLANS", "Warp.OutdoorsOnly", ref Clans.WarpOutdoorsOnly, true);
				Config.Get("CLANS", "ClanWar.Death.Pay", ref Clans.ClanWarDeathPay, true);
				Config.Get("CLANS", "ClanWar.Death.Percent", ref Clans.ClanWarDeathPercent, true);
				Config.Get("CLANS", "ClanWar.Murder.Fee", ref Clans.ClanWarMurderFee, true);
				Config.Get("CLANS", "ClanWar.Murder.Percent", ref Clans.ClanWarMurderPercent, true);
				Config.Get("CLANS", "ClanWar.Declared.Gain.Percent", ref Clans.ClanWarDeclaredGainPercent, true);
				Config.Get("CLANS", "ClanWar.Declined.Lost.Percent", ref Clans.ClanWarDeclinedLostPercent, true);
				Config.Get("CLANS", "ClanWar.Declined.Penalty", ref Clans.ClanWarDeclinedPenalty, true);
				Config.Get("CLANS", "ClanWar.Accepted.Time", ref Clans.ClanWarAcceptedTime, true);
				Config.Get("CLANS", "ClanWar.Ended.Penalty", ref Clans.ClanWarEndedPenalty, true);
				if (Core.dlq && !DlqMysql.IsConnected)
				{
					if (DlqMysql.Connect(Core.dlqip, Core.dlquser, Core.dlqpassword, Core.dlqdb, Core.dlqport, null, (DlqMysql.ClientFlags)0uL))
					{
						Debug.Log("dlq Connected.");
					}
					else
					{
						Debug.Log("dlq MySql error.");
					}
				}
				if (Core.dlq1 && RustHook.dlqsv == null)
				{
					IPAddress any = IPAddress.Any;
					IPEndPoint localEP = new IPEndPoint(any, server.port + 2);
					RustHook.dlqsv = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					RustHook.dlqsv.Bind(localEP);
					RustHook.dlqsv.Listen(20);
					Thread thread = new Thread(new ThreadStart(RustHook.TcpListen));
					thread.Start();
				}
				if (Core.xzjz)
				{
					RustHook.dijiusers = new IniParser(Core.SavePath + "\\dijiusers.ini");
				}
				if (Core.xtf)
				{
					string text = File.ReadAllText(Core.SavePath + "\\xtf.txt", Encoding.ASCII);
					string[] array = text.Split(new string[]
					{
						"\r\n"
					}, StringSplitOptions.None);
					if (array.Length >= 2)
					{
						int num = 0;
						while (array.Length > num)
						{
							string[] array2 = array[num].Split(new char[]
							{
								','
							});
							if (array2.Length == 2)
							{
								int value = Convert.ToInt32(array2[1]);
								RustHook.xtf.Add(array2[0], value);
								Debug.Log(array2[0] + value.ToString());
							}
							num++;
						}
					}
				}
				if (Core.Languages.Length == 1)
				{
					Core.Languages = Core.Languages[0].ToUpper().Replace(" ", "").Split(new char[]
					{
						','
					});
				}
				if (Core.Languages.Length == 0)
				{
					Core.Languages = new string[]
					{
						"ENG"
					};
				}
				Core.DatabaseType = Core.DatabaseType.ToUpper();
				if (!Core.DatabaseType.Equals("FILE") && !Core.DatabaseType.Equals("MYSQL"))
				{
					ConsoleSystem.PrintError("RustExtended: Unknown Database Type \"" + Core.DatabaseType + "\"", false);
				}
				switch (Core.MySQL_LogLevel)
				{
				case 0u:
					MySQL.LogLevel = MySQL.LogLevelType.NONE;
					break;
				case 1u:
					MySQL.LogLevel = MySQL.LogLevelType.ERRORS;
					break;
				default:
					MySQL.LogLevel = MySQL.LogLevelType.ALL;
					break;
				}
				foreach (PlayerClient current2 in PlayerClient.All)
				{
					Users.GetBySteamID(current2.userID).ProtectTime = 0f;
					Users.GetBySteamID(current2.userID).ProtectTick = 0;
				}
				Truth.PunishAction = Truth.Punishment.ToUpper().Replace(" ", "").Split(new char[]
				{
					','
				});
				Truth.ProtectionHash = Truth.RustProtectHash.ToInt32();
				Truth.ProtectionKey = Truth.RustProtectKey.ToInt32();
				if (Truth.RustProtectSnapshotsPacketSize < 256u)
				{
					Truth.RustProtectSnapshotsPacketSize = 256u;
				}
				if (Truth.RustProtectSnapshotsPacketSize > 8192u)
				{
					Truth.RustProtectSnapshotsPacketSize = 8192u;
				}
				if (Truth.RustProtectMaxTicks < NetCull.sendRate)
				{
					Truth.RustProtectMaxTicks = (uint)NetCull.sendRate;
				}
				Truth.RustProtectSnapshotsPath = Truth.RustProtectSnapshotsPath.Trim(new char[]
				{
					'"',
					' '
				}).Replace("/", "\\");
				if (CommandLine.HasSwitch("-no-premium-connections"))
				{
					Core.PremiumConnections = 0;
				}
				if (Core.SleepersLifeTime < 0)
				{
					Core.SleepersLifeTime = 0;
				}
				if (Core.PremiumConnections > 0)
				{
					server.maxplayers += Core.PremiumConnections;
				}
				if (CommandLine.HasSwitch("-force-steam-auth"))
				{
					Core.SteamAuthUser = true;
				}
				if (Server.Official)
				{
				}
				if (Server.Modded)
				{
				}
				if (!Core.SteamAuthUser)
				{
				}
				if (Truth.RustProtect)
				{
					Helper.Log("---------------------------------------------------", true);
					Helper.Log("RUST ANTI Cheat System Strart!", true);
					Helper.Log("ACS Version: 0.1.", true);
					Helper.Log("---------------------------------------------------", true);
				}
				int num2 = (Core.SteamFakeOnline.Length > 0) ? Core.SteamFakeOnline[0] : 0;
				int val = (Core.SteamFakeOnline.Length > 1) ? Core.SteamFakeOnline[1] : num2;
				Core.SteamFakeOnline = new int[]
				{
					Math.Min(num2, val),
					Math.Max(num2, val)
				};
				Core.HasFakeOnline = (Core.SteamFakeOnline[1] > 0 && !CommandLine.HasSwitch("-ignore-fake-online"));
				if (Core.SteamFakeOnline[0] > server.maxplayers)
				{
					Core.SteamFakeOnline[0] = server.maxplayers;
				}
				if (Core.SteamFakeOnline[1] > server.maxplayers)
				{
					Core.SteamFakeOnline[1] = server.maxplayers;
				}
				Core.ChatDivider = Core.ChatDivider.Trim(new char[]
				{
					'"'
				});
				Core.ChatSayIcon = Core.ChatSayIcon.Trim(new char[]
				{
					'"'
				});
				Core.ChatSayColor = Core.ChatSayColor.ToUpper();
				Core.ChatClanIcon = Core.ChatClanIcon.Trim(new char[]
				{
					'"'
				});
				Core.ChatClanColor = Core.ChatClanColor.ToUpper();
				Core.ChatYellIcon = Core.ChatYellIcon.Trim(new char[]
				{
					'"'
				});
				Core.ChatYellColor = Core.ChatYellColor.ToUpper();
				Core.ChatWhisperIcon = Core.ChatWhisperIcon.Trim(new char[]
				{
					'"'
				});
				Core.ChatWhisperColor = Core.ChatWhisperColor.ToUpper();
				Core.ChatConsoleName = Core.ChatConsoleName.Trim(new char[]
				{
					'"'
				});
				Core.ChatConsoleColor = Core.ChatConsoleColor.ToUpper();
				Core.RankColor.Clear();
				foreach (int current3 in Core.Ranks.Keys)
				{
					string text2 = string.Format("Chat.Rank.{0}.Color", current3);
					if (!Core.RankColor.ContainsKey(current3) && Config.Get("SERVER", text2, ref text2, true))
					{
						Core.RankColor.Add(current3, text2);
					}
				}
				foreach (MOTDEvent current4 in Events.Motd)
				{
					current4.Start();
				}
				Config.Loading = false;
			}
		}

		public static bool Load()
		{
			Predicate<MOTDEvent> predicate = null;
			Config.Class13 @class = new Config.Class13();
			string[] files = Directory.GetFiles(Config.FilePath, "*.cfg");
			bool result;
			if (files.Length == 0)
			{
				ConsoleSystem.Log("No configuration files. Loaded defaults.");
				result = false;
			}
			else
			{
				string text = "-1";
				@class.string_0 = new string[0];
				List<string> list = new List<string>();
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					list.AddRange(File.ReadAllLines(path).ToList<string>());
				}
				for (int j = 0; j < list.Count; j++)
				{
					string text2 = list[j].Trim();
					if (!string.IsNullOrEmpty(text2) && !text2.StartsWith("//"))
					{
						if (text2.Contains("//"))
						{
							text2 = text2.Split(new string[]
							{
								"//"
							}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						}
						if (!string.IsNullOrEmpty(text2))
						{
							string a2;
							if (text2.ToUpper().StartsWith("[") && text2.ToUpper().EndsWith("]"))
							{
								@class.string_0 = Helper.SplitQuotes(text2.Substring(1, text2.Length - 2), ' ');
								@class.string_0[0] = @class.string_0[0].ToUpper();
								if (@class.string_0[0].StartsWith("MESSAGES.") || @class.string_0[0].StartsWith("BODYPART.") || @class.string_0[0].StartsWith("NAMES."))
								{
									@class.string_0 = @class.string_0[0].Split(new char[]
									{
										'.'
									});
								}
								if (@class.string_0[0] == "RANK" && @class.string_0.Length > 1)
								{
									int key = 0;
									string value = "";
									string[] array2 = new string[]
									{
										@class.string_0[1].Trim()
									};
									if (@class.string_0[1].Contains('.'))
									{
										array2 = @class.string_0[1].Split(new char[]
										{
											'.'
										});
										value = array2[1].Trim();
									}
									if (!int.TryParse(array2[0].Trim(), out key))
									{
										@class.string_0[0] = "NULL";
									}
									else
									{
										text = array2[0].Trim();
										if (Core.Ranks.ContainsKey(key))
										{
											@class.string_0[0] = "NULL";
										}
										else
										{
											Core.Ranks.Add(key, value);
										}
									}
								}
							}
							else if (text2.Contains("="))
							{
								string[] array3 = text2.Split(new char[]
								{
									'='
								});
								array3[0] = array3[0].Trim();
								array3[1] = array3[1].Trim();
								Config.Class14 class2 = new Config.Class14();
								class2.class13_0 = @class;
								string text3 = @class.string_0[0];
								switch (text3)
								{
								case "SERVER":
									Config.Add(@class.string_0[0], array3[0], array3[1]);
									break;
								case "OVERRIDE.ARMOR":
									Config.Add(@class.string_0[0], array3[0], array3[1]);
									break;
								case "OVERRIDE.DAMAGE":
									Config.Add(@class.string_0[0], array3[0], array3[1]);
									break;
								case "DESTROY.RESOURCES":
									Core.DestoryResources.Add(array3[0], array3[1]);
									break;
								case "MESSAGES":
								case "NAMES":
								case "BODYPART":
									if (@class.string_0.Length < 2)
									{
										Config.Add(@class.string_0[0], array3[0], array3[1]);
									}
									else
									{
										Config.Add(@class.string_0[0] + '.' + @class.string_0[1], array3[0], array3[1]);
									}
									break;
								case "RANK":
									Core.Commands.Add(string.Concat(new string[]
									{
										text,
										"=",
										array3[0].ToLower(),
										"=",
										array3[1]
									}));
									break;
								case "MOTD":
									if (@class.string_0.Length > 1 && !@class.string_0[1].IsEmpty())
									{
										List<MOTDEvent> motd = Events.Motd;
										if (predicate == null)
										{
											predicate = new Predicate<MOTDEvent>(@class.method_0);
										}
										MOTDEvent mOTDEvent = motd.Find(predicate);
										if (mOTDEvent == null)
										{
											mOTDEvent = new MOTDEvent(@class.string_0[1], 3600);
											Events.Motd.Add(mOTDEvent);
										}
										if (array3[0].Equals("Enabled", StringComparison.OrdinalIgnoreCase))
										{
											string a = array3[1].ToUpper().Trim();
											mOTDEvent.Enabled = (a == "TRUE" || a == "YES" || a == "ON" || a == "1");
										}
										else if (array3[0].Equals("Interval", StringComparison.OrdinalIgnoreCase))
										{
											int interval;
											int.TryParse(array3[1], out interval);
											mOTDEvent.Interval = interval;
										}
										else if (array3[0].Equals("Message", StringComparison.OrdinalIgnoreCase))
										{
											mOTDEvent.Messages.Add(array3[1]);
										}
										else if (array3[0].Equals("Announce", StringComparison.OrdinalIgnoreCase))
										{
											mOTDEvent.Announce.Add(array3[1]);
										}
									}
									break;
								case "KIT":
									if (@class.string_0.Length > 1)
									{
										string key2 = @class.string_0[1].ToLower();
										List<string> list2 = (List<string>)Core.Kits[key2];
										if (list2 == null)
										{
											list2 = new List<string>();
											Core.Kits.Add(key2, list2);
										}
										list2.Add(text2);
									}
									break;
								case "LOADOUT":
								{
									string key3 = @class.string_0[1].Trim().ToUpper();
									if (!Config.LoadoutList.ContainsKey(key3))
									{
										Config.LoadoutList.Add(key3, new List<string>());
									}
									Config.LoadoutList[key3].Add(text2);
									break;
								}
								case "ECONOMY":
								case "SHOP":
									Config.Add(@class.string_0[0], array3[0], array3[1]);
									break;
								case "SHOP.LIST":
									Config.Add(@class.string_0[0], "ENTRY", text2);
									break;
								case "SHOP.GROUP":
									Config.Add(@class.string_0[0], @class.string_0[1], text2);
									break;
								case "CLANS":
									Config.Add(@class.string_0[0], array3[0], array3[1]);
									break;
								case "CLAN.CRAFTING.EXPERIENCE":
									if (!string.IsNullOrEmpty(array3[0]) && !Clans.CraftExperience.ContainsKey(array3[0]))
									{
										int value2 = 0;
										if (int.TryParse(array3[1], out value2))
										{
											Clans.CraftExperience.Add(array3[0].Trim(), value2);
										}
									}
									break;
								case "CLANLEVEL":
									if (int.TryParse(@class.string_0[1], out class2.int_0))
									{
										ClanLevel clanLevel = Clans.Levels.Find(new Predicate<ClanLevel>(class2.method_0));
										if (clanLevel == null)
										{
											clanLevel = new ClanLevel(class2.int_0);
											Clans.Levels.Add(clanLevel);
										}
										text2 = array3[0].Trim().ToUpper();
										if (!array3[1].IsEmpty())
										{
											if (text2.Equals("REQUIRE.CURRENCY"))
											{
												ulong.TryParse(array3[1], out clanLevel.RequireCurrency);
											}
											else if (text2.Equals("REQUIRE.EXPERIENCE"))
											{
												ulong.TryParse(array3[1], out clanLevel.RequireExperience);
											}
											else if (text2.Equals("REQUIRE.LEVEL"))
											{
												int.TryParse(array3[1], out clanLevel.RequireLevel);
											}
											else if (text2.Equals("MAXIMUM.MEMBERS"))
											{
												int.TryParse(array3[1], out clanLevel.MaxMembers);
											}
											else if (text2.Equals("CURRENCY.TAX"))
											{
												uint.TryParse(array3[1], out clanLevel.CurrencyTax);
											}
											else if (text2.Equals("WARP.TIMEWAIT"))
											{
												uint.TryParse(array3[1], out clanLevel.WarpTimewait);
											}
											else if (text2.Equals("WARP.COUNTDOWN"))
											{
												uint.TryParse(array3[1], out clanLevel.WarpCountdown);
											}
											else if (text2.Equals("FLAG.MOTD"))
											{
												clanLevel.FlagMotd = array3[1].ToBool();
											}
											else if (text2.Equals("FLAG.ABBR"))
											{
												clanLevel.FlagAbbr = array3[1].ToBool();
											}
											else if (text2.Equals("FLAG.FFIRE"))
											{
												clanLevel.FlagFFire = array3[1].ToBool();
											}
											else if (text2.Equals("FLAG.TAX"))
											{
												clanLevel.FlagTax = array3[1].ToBool();
											}
											else if (text2.Equals("FLAG.HOUSE"))
											{
												clanLevel.FlagHouse = array3[1].ToBool();
											}
											else if (text2.Equals("FLAG.DECLARE"))
											{
												clanLevel.FlagDeclare = array3[1].ToBool();
											}
											else if (text2.Equals("BONUS.CRAFTING.SPEED"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusCraftingSpeed);
											}
											else if (text2.Equals("BONUS.GATHERING.WOOD"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusGatheringWood);
											}
											else if (text2.Equals("BONUS.GATHERING.ROCK"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusGatheringRock);
											}
											else if (text2.Equals("BONUS.GATHERING.ANIMAL"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusGatheringAnimal);
											}
											else if (text2.Equals("BONUS.MEMBERS.PAYMURDER"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusMembersPayMurder);
											}
											else if (text2.Equals("BONUS.MEMBERS.DEFENSE"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusMembersDefense);
											}
											else if (text2.Equals("BONUS.MEMBERS.DAMAGE"))
											{
												uint.TryParse(array3[1], out clanLevel.BonusMembersDamage);
											}
										}
									}
									break;
								}
							}
							else if ((a2 = @class.string_0[0]) != null)
							{
								if (!(a2 == "FORBIDDEN.NAME"))
								{
									if (a2 == "FORBIDDEN.OBSCENE")
									{
										Core.ForbiddenObscene.Add(text2.ToUpper());
									}
								}
								else
								{
									Core.ForbiddenUsername.Add(text2.ToUpper());
								}
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		private static void Add(string string_1, string string_2, string string_3)
		{
			Config.Class15 @class = new Config.Class15();
			@class.string_0 = string_1;
			@class.string_1 = string_2;
			Config.Struct0 item = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			if (item.Equals(default(Config.Struct0)))
			{
				item.string_0 = @class.string_0;
				item.string_1 = @class.string_1;
				item.list_0 = new List<string>();
				Config.list_0.Add(item);
			}
			item.list_0.Add(string_3);
		}

		public static bool Get(string section, string key, ref int result, bool caseinsensitive = true)
		{
			Config.Class16 @class = new Config.Class16();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			int num;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!int.TryParse(@struct.list_0[0], out num))
			{
				result2 = false;
			}
			else
			{
				result = num;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref int[] result, bool caseinsensitive = true)
		{
			Config.Class17 @class = new Config.Class17();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				string[] array = @struct.list_0[0].Split(new char[]
				{
					','
				});
				if (array.Length == 0)
				{
					result2 = false;
				}
				else
				{
					Array.Resize<int>(ref result, array.Length);
					for (int i = 0; i < result.Length; i++)
					{
						int.TryParse(array[i], out result[i]);
					}
					result2 = true;
				}
			}
			return result2;
		}

		public static bool Get(string section, string key, ref uint result, bool caseinsensitive = true)
		{
			Config.Class18 @class = new Config.Class18();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			uint num;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!uint.TryParse(@struct.list_0[0], out num))
			{
				result2 = false;
			}
			else
			{
				result = num;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref uint[] result, bool caseinsensitive = true)
		{
			Config.Class19 @class = new Config.Class19();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				string[] array = @struct.list_0[0].Split(new char[]
				{
					','
				});
				if (array.Length == 0)
				{
					result2 = false;
				}
				else
				{
					Array.Resize<uint>(ref result, array.Length);
					for (int i = 0; i < result.Length; i++)
					{
						uint.TryParse(array[i], out result[i]);
					}
					result2 = true;
				}
			}
			return result2;
		}

		public static bool Get(string section, string key, ref long result, bool caseinsensitive = true)
		{
			Config.Class20 @class = new Config.Class20();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			long num;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!long.TryParse(@struct.list_0[0], out num))
			{
				result2 = false;
			}
			else
			{
				result = num;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref long[] result, bool caseinsensitive = true)
		{
			Config.Class21 @class = new Config.Class21();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				string[] array = @struct.list_0[0].Split(new char[]
				{
					','
				});
				if (array.Length == 0)
				{
					result2 = false;
				}
				else
				{
					Array.Resize<long>(ref result, array.Length);
					for (int i = 0; i < result.Length; i++)
					{
						long.TryParse(array[i], out result[i]);
					}
					result2 = true;
				}
			}
			return result2;
		}

		public static bool Get(string section, string key, ref ulong result, bool caseinsensitive = true)
		{
			Config.Class22 @class = new Config.Class22();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			ulong num;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!ulong.TryParse(@struct.list_0[0], out num))
			{
				result2 = false;
			}
			else
			{
				result = num;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref ulong[] result, bool caseinsensitive = true)
		{
			Config.Class23 @class = new Config.Class23();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				string[] array = @struct.list_0[0].Split(new char[]
				{
					','
				});
				if (array.Length == 0)
				{
					result2 = false;
				}
				else
				{
					Array.Resize<ulong>(ref result, array.Length);
					for (int i = 0; i < result.Length; i++)
					{
						ulong.TryParse(array[i], out result[i]);
					}
					result2 = true;
				}
			}
			return result2;
		}

		public static bool Get(string section, string key, ref float result, bool caseinsensitive = true)
		{
			Config.Class24 @class = new Config.Class24();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			float num;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!float.TryParse(@struct.list_0[0], out num))
			{
				result2 = false;
			}
			else
			{
				result = num;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref float[] result, bool caseinsensitive = true)
		{
			Config.Class25 @class = new Config.Class25();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				float num = 0f;
				string[] array = @struct.list_0[0].Split(new char[]
				{
					','
				});
				if (array.Length == 0)
				{
					result2 = false;
				}
				else
				{
					for (int i = 0; i < result.Length; i++)
					{
						if (array.Length > i)
						{
							num = float.Parse(array[i].Trim());
						}
						result[i] = num;
					}
					result2 = true;
				}
			}
			return result2;
		}

		public static bool Get(string section, string key, ref double result, bool caseinsensitive = true)
		{
			Config.Class26 @class = new Config.Class26();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			double num;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!double.TryParse(@struct.list_0[0], out num))
			{
				result2 = false;
			}
			else
			{
				result = num;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref double[] result, bool caseinsensitive = true)
		{
			Config.Class27 @class = new Config.Class27();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				double num = 0.0;
				string[] array = @struct.list_0[0].Split(new char[]
				{
					','
				});
				if (array.Length == 0)
				{
					result2 = false;
				}
				else
				{
					for (int i = 0; i < result.Length; i++)
					{
						if (array.Length > i)
						{
							num = double.Parse(array[i].Trim());
						}
						result[i] = num;
					}
					result2 = true;
				}
			}
			return result2;
		}

		public static bool Get(string section, string key, ref bool result, bool caseinsensitive = true)
		{
			Config.Class28 @class = new Config.Class28();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			bool flag;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else if (!bool.TryParse(@struct.list_0[0], out flag))
			{
				result2 = false;
			}
			else
			{
				result = flag;
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref string result, bool caseinsensitive = true)
		{
			Config.Class29 @class = new Config.Class29();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				result = @struct.list_0[0];
				result2 = true;
			}
			return result2;
		}

		public static bool Get(string section, string key, ref string[] result, bool caseinsensitive = true)
		{
			Config.Class30 @class = new Config.Class30();
			@class.string_0 = section;
			@class.string_1 = key;
			@class.bool_0 = caseinsensitive;
			Config.Struct0 @struct = Config.list_0.Find(new Predicate<Config.Struct0>(@class.method_0));
			bool result2;
			if (@struct.Equals(default(Config.Struct0)))
			{
				result2 = false;
			}
			else
			{
				result = @struct.list_0.ToArray();
				result2 = true;
			}
			return result2;
		}

		public static string GetMessage(string msg, NetUser User = null, string Username = null)
		{
			string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
			string text = msg;
			Config.Get("MESSAGES." + str, msg, ref text, true);
			return Helper.ReplaceVariables(User, text, (Username == null) ? null : "%USERNAME%", Username);
		}

		public static string GetMessageCommand(string msg, string command = "", NetUser User = null)
		{
			string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
			string text = msg;
			Config.Get("MESSAGES." + str, msg, ref text, true);
			text = Helper.ReplaceVariables(User, text, null, "");
			if (text.Contains("%COMMAND%"))
			{
				text = text.Replace("%COMMAND%", command);
			}
			return text;
		}

		public static string[] GetMessages(string msg, NetUser User = null)
		{
			string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
			string[] array = new string[0];
			Config.Get("MESSAGES." + str, msg, ref array, true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Helper.ReplaceVariables(User, array[i], null, "");
			}
			return array;
		}

		public static string GetMessageDeath(string msg, NetUser Victim = null, string KillerName = null, string WeaponName = null)
		{
			string str = (Victim == null) ? Core.Languages[0] : Users.GetLanguage(Victim.userID);
			string[] array = new string[]
			{
				msg
			};
			Config.Get("MESSAGES." + str, msg, ref array, true);
			int num = UnityEngine.Random.Range(0, array.Length);
			string text = Helper.ReplaceVariables(Victim, array[num], null, "");
			if (Victim != null && text.Contains("%VICTIM%"))
			{
				text = text.Replace("%VICTIM%", Victim.displayName);
			}
			if (KillerName != null && text.Contains("%KILLER%"))
			{
				text = text.Replace("%KILLER%", KillerName);
			}
			if (WeaponName != null && text.Contains("%WEAPON%"))
			{
				text = text.Replace("%WEAPON%", WeaponName);
			}
			if (text.Contains("%POSX%"))
			{
				text = text.Replace("%POSX%", Victim.playerClient.lastKnownPosition.x.ToString());
			}
			if (text.Contains("%POSY%"))
			{
				text = text.Replace("%POSY%", Victim.playerClient.lastKnownPosition.y.ToString());
			}
			if (text.Contains("%POSZ%"))
			{
				text = text.Replace("%POSZ%", Victim.playerClient.lastKnownPosition.z.ToString());
			}
			if (text.Contains("%POS%"))
			{
				text = text.Replace("%POS%", Victim.playerClient.lastKnownPosition.ToString());
			}
			return text;
		}

		public static string GetMessageMurder(string msg, NetUser Killer = null, string VictimName = null, string WeaponName = null)
		{
			string str = (Killer == null) ? Core.Languages[0] : Users.GetLanguage(Killer.userID);
			string[] array = new string[]
			{
				msg
			};
			Config.Get("MESSAGES." + str, msg, ref array, true);
			int num = UnityEngine.Random.Range(0, array.Length);
			string text = Helper.ReplaceVariables(Killer, array[num], null, "");
			if (Killer != null && text.Contains("%KILLER%"))
			{
				text = text.Replace("%KILLER%", Killer.displayName);
			}
			if (VictimName != null && text.Contains("%VICTIM%"))
			{
				text = text.Replace("%VICTIM%", VictimName);
			}
			if (WeaponName != null && text.Contains("%WEAPON%"))
			{
				text = text.Replace("%WEAPON%", WeaponName);
			}
			if (text.Contains("%POSX%"))
			{
				text = text.Replace("%POSX%", Killer.playerClient.lastKnownPosition.x.ToString());
			}
			if (text.Contains("%POSY%"))
			{
				text = text.Replace("%POSY%", Killer.playerClient.lastKnownPosition.y.ToString());
			}
			if (text.Contains("%POSZ%"))
			{
				text = text.Replace("%POSZ%", Killer.playerClient.lastKnownPosition.z.ToString());
			}
			if (text.Contains("%POS%"))
			{
				text = text.Replace("%POS%", Killer.playerClient.lastKnownPosition.ToString());
			}
			return text;
		}

		public static string GetMessageObject(string msg, string VictimName = null, PlayerClient Killer = null, string WeaponName = null, UserData Owner = null)
		{
			string str = (Killer == null) ? Core.Languages[0] : Users.GetLanguage(Killer.userID);
			string text = msg;
			Config.Get("MESSAGES." + str, msg, ref text, true);
			if (text.Contains("%OWNERNAME%"))
			{
				text = text.Replace("%OWNERNAME%", (Owner != null) ? Owner.Username : "-");
			}
			if (text.Contains("%OWNER_ID%"))
			{
				text = text.Replace("%OWNER_ID%", (Owner != null) ? Owner.SteamID.ToString() : "-");
			}
			if (Killer != null && text.Contains("%USERNAME%"))
			{
				text = text.Replace("%USERNAME%", Killer.netUser.displayName);
			}
			if (Killer != null && text.Contains("%STEAM_ID%"))
			{
				text = text.Replace("%STEAM_ID%", Killer.netUser.userID.ToString());
			}
			if (VictimName != null && text.Contains("%OBJECT%"))
			{
				text = text.Replace("%OBJECT%", VictimName);
			}
			if (WeaponName != null && text.Contains("%WEAPON%"))
			{
				text = text.Replace("%WEAPON%", WeaponName);
			}
			return text;
		}

		public static string GetMessageTruth(string msg, NetUser User = null, string hackMethod = "", int Violations = 0, DateTime period = default(DateTime))
		{
			string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
			string text = msg;
			Config.Get("MESSAGES." + str, msg, ref text, true);
			text = Helper.ReplaceVariables(User, text, null, "");
			if (text.Contains("%BAN_PERIOD%"))
			{
				text = text.Replace("%BAN_PERIOD%", (period.Ticks > 0L) ? period.ToString("yyyy-MM-dd HH:mm:ss") : "--:--");
			}
			if (text.Contains("%VIOLATION_HACK%"))
			{
				text = text.Replace("%VIOLATION_HACK%", hackMethod);
			}
			if (text.Contains("%VIOLATION_NUM%"))
			{
				text = text.Replace("%VIOLATION_NUM%", Violations.ToString());
			}
			if (text.Contains("%VIOLATION_MAX%"))
			{
				text = text.Replace("%VIOLATION_MAX%", Truth.MaxViolations.ToString());
			}
			if (text.Contains("%POSX%"))
			{
				text = text.Replace("%POSX%", User.playerClient.lastKnownPosition.x.ToString());
			}
			if (text.Contains("%POSY%"))
			{
				text = text.Replace("%POSY%", User.playerClient.lastKnownPosition.y.ToString());
			}
			if (text.Contains("%POSZ%"))
			{
				text = text.Replace("%POSZ%", User.playerClient.lastKnownPosition.z.ToString());
			}
			if (text.Contains("%POS%"))
			{
				text = text.Replace("%POS%", User.playerClient.lastKnownPosition.ToString());
			}
			return text;
		}

		public static string GetMessageClan(string msg, ClanData clan = null, NetUser netUser = null, UserData dataUser = null)
		{
			Config.Class31 @class = new Config.Class31();
			@class.clanData_0 = clan;
			string str = (netUser == null) ? Core.Languages[0] : Users.GetLanguage(netUser.userID);
			string text = msg;
			Config.Get("MESSAGES." + str, msg, ref text, true);
			ClanLevel clanLevel = null;
			if (@class.clanData_0 != null)
			{
				clanLevel = Clans.Levels.Find(new Predicate<ClanLevel>(@class.method_0));
			}
			UserData userData = null;
			if (@class.clanData_0 != null)
			{
				userData = Users.GetBySteamID(@class.clanData_0.LeaderID);
			}
			text = Helper.ReplaceVariables(netUser, text, null, "");
			if (text.Contains("%CREATE_COST%"))
			{
				text = text.Replace("%CREATE_COST%", Clans.CreateCost.ToString("N0") + Economy.CurrencySign);
			}
			if (text.Contains("%CLANS.COUNT%"))
			{
				text = text.Replace("%CLANS.COUNT%", Clans.Database.Count.ToString());
			}
			if (dataUser != null)
			{
				if (text.Contains("%STEAM_ID%"))
				{
					text = text.Replace("%STEAM_ID%", dataUser.SteamID.ToString());
				}
				if (text.Contains("%USERNAME%"))
				{
					text = text.Replace("%USERNAME%", dataUser.Username);
				}
			}
			if (@class.clanData_0 != null)
			{
				if (text.Contains("%CLAN.ID%"))
				{
					text = text.Replace("%CLAN.ID%", @class.clanData_0.ID.ToString());
				}
				if (text.Contains("%CLAN.CREATED%"))
				{
					text = text.Replace("%CLAN.CREATED%", @class.clanData_0.Created.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				if (text.Contains("%CLAN.NAME%"))
				{
					text = text.Replace("%CLAN.NAME%", @class.clanData_0.Name);
				}
				if (text.Contains("%CLAN.ABBR%") && @class.clanData_0.Flags.Has(ClanFlags.can_abbr))
				{
					text = text.Replace("%CLAN.ABBR%", @class.clanData_0.Abbr);
				}
				if (text.Contains("%CLAN.MOTD%") && @class.clanData_0.Flags.Has(ClanFlags.can_motd))
				{
					text = text.Replace("%CLAN.MOTD%", @class.clanData_0.MOTD);
				}
				if (text.Contains("%CLAN.TAX%"))
				{
					text = text.Replace("%CLAN.TAX%", @class.clanData_0.Tax.ToString() + "%");
				}
				if (text.Contains("%CLAN.EXPERIENCE%"))
				{
					text = text.Replace("%CLAN.EXPERIENCE%", @class.clanData_0.Experience.ToString());
				}
				if (text.Contains("%CLAN.LOCATION%") && @class.clanData_0.Flags.Has(ClanFlags.can_warp))
				{
					text = text.Replace("%CLAN.LOCATION%", @class.clanData_0.Location.AsString());
				}
				if (text.Contains("%CLAN.ONLINE%"))
				{
					text = text.Replace("%CLAN.ONLINE%", @class.clanData_0.Online.ToString());
				}
				if (text.Contains("%CLAN.MEMBERS.COUNT%"))
				{
					text = text.Replace("%CLAN.MEMBERS.COUNT%", @class.clanData_0.Members.Count.ToString());
				}
			}
			if (userData != null)
			{
				if (text.Contains("%CLAN.LEADER.STEAM_ID%"))
				{
					text = text.Replace("%CLAN.LEADER.STEAM_ID%", userData.SteamID.ToString());
				}
				if (text.Contains("%CLAN.LEADER.USERNAME%"))
				{
					text = text.Replace("%CLAN.LEADER.USERNAME%", userData.Username);
				}
			}
			if (@class.clanData_0 != null && @class.clanData_0.Level != null)
			{
				if (text.Contains("%CLAN.LEVEL%"))
				{
					text = text.Replace("%CLAN.LEVEL%", @class.clanData_0.Level.Id.ToString());
				}
				if (text.Contains("%CLAN.MEMBERS.MAX%"))
				{
					text = text.Replace("%CLAN.MEMBERS.MAX%", @class.clanData_0.Level.MaxMembers.ToString());
				}
				if (text.Contains("%CLAN.WARP_TIMEOUT%"))
				{
					text = text.Replace("%CLAN.WARP_TIMEOUT%", @class.clanData_0.Level.WarpTimewait.ToString());
				}
				if (text.Contains("%CLAN.WARP_COUNTDOWN%"))
				{
					text = text.Replace("%CLAN.WARP_COUNTDOWN%", @class.clanData_0.Level.WarpCountdown.ToString());
				}
				if (text.Contains("%CLAN.BONUS.CRAFTINGSPEED%") && @class.clanData_0.Level.BonusCraftingSpeed > 0u)
				{
					text = text.Replace("%CLAN.BONUS.CRAFTINGSPEED%", @class.clanData_0.Level.BonusCraftingSpeed.ToString() + "%");
				}
				if (text.Contains("%CLAN.BONUS.GATHERINGWOOD%") && @class.clanData_0.Level.BonusGatheringWood > 0u)
				{
					text = text.Replace("%CLAN.BONUS.GATHERINGWOOD%", @class.clanData_0.Level.BonusGatheringWood.ToString() + "%");
				}
				if (text.Contains("%CLAN.BONUS.GATHERINGROCK%") && @class.clanData_0.Level.BonusGatheringRock > 0u)
				{
					text = text.Replace("%CLAN.BONUS.GATHERINGROCK%", @class.clanData_0.Level.BonusGatheringRock.ToString() + "%");
				}
				if (text.Contains("%CLAN.BONUS.GATHERINGANIMAL%") && @class.clanData_0.Level.BonusGatheringAnimal > 0u)
				{
					text = text.Replace("%CLAN.BONUS.GATHERINGANIMAL%", @class.clanData_0.Level.BonusGatheringAnimal.ToString() + "%");
				}
				if (text.Contains("%CLAN.BONUS.MEMBERS_DEFENSE%") && @class.clanData_0.Level.BonusMembersDefense > 0u)
				{
					text = text.Replace("%CLAN.BONUS.MEMBERS_DEFENSE%", @class.clanData_0.Level.BonusMembersDefense.ToString() + "%");
				}
				if (text.Contains("%CLAN.BONUS.MEMBERS_DAMAGE%") && @class.clanData_0.Level.BonusMembersDamage > 0u)
				{
					text = text.Replace("%CLAN.BONUS.MEMBERS_DAMAGE%", @class.clanData_0.Level.BonusMembersDamage.ToString() + "%");
				}
				if (text.Contains("%CLAN.BONUS.MEMBERS_PAYMURDER%") && @class.clanData_0.Level.BonusMembersPayMurder > 0u)
				{
					text = text.Replace("%CLAN.BONUS.MEMBERS_PAYMURDER%", @class.clanData_0.Level.BonusMembersPayMurder.ToString() + "%");
				}
			}
			if (@class.clanData_0 != null && clanLevel != null)
			{
				if (text.Contains("%CLAN.NEXT_LEVEL%"))
				{
					text = text.Replace("%CLAN.NEXT_LEVEL%", clanLevel.Id.ToString());
				}
				if (text.Contains("%CLAN.NEXT_CURRENCY%"))
				{
					text = text.Replace("%CLAN.NEXT_CURRENCY%", clanLevel.RequireCurrency.ToString());
				}
				if (text.Contains("%CLAN.NEXT_EXPERIENCE%"))
				{
					text = text.Replace("%CLAN.NEXT_EXPERIENCE%", clanLevel.RequireExperience.ToString());
				}
				if (text.Contains("%CLAN.NEXT_MAXMEMBERS%"))
				{
					text = text.Replace("%CLAN.NEXT_MAXMEMBERS%", clanLevel.MaxMembers.ToString());
				}
			}
			return text;
		}

		public static string[] GetMessagesClan(string msg, ClanData clan = null, NetUser netuser = null, UserData dataUser = null)
		{
			Config.Class32 @class = new Config.Class32();
			@class.clanData_0 = clan;
			string str = (netuser == null) ? Core.Languages[0] : Users.GetLanguage(netuser.userID);
			string[] array = new string[0];
			Config.Get("MESSAGES." + str, msg, ref array, true);
			ClanLevel clanLevel = null;
			if (@class.clanData_0 != null)
			{
				clanLevel = Clans.Levels.Find(new Predicate<ClanLevel>(@class.method_0));
			}
			UserData userData = null;
			if (@class.clanData_0 != null)
			{
				userData = Users.GetBySteamID(@class.clanData_0.LeaderID);
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Helper.ReplaceVariables(netuser, array[i], null, "");
				if (array[i].Contains("%CREATE_COST%"))
				{
					array[i] = array[i].Replace("%CREATE_COST%", Clans.CreateCost.ToString() + Economy.CurrencySign);
				}
				if (array[i].Contains("%CLANS.COUNT%"))
				{
					array[i] = array[i].Replace("%CLANS.COUNT%", Clans.Database.Count.ToString());
				}
				if (dataUser != null)
				{
					if (array[i].Contains("%USERNAME%"))
					{
						array[i] = array[i].Replace("%USERNAME%", dataUser.Username);
					}
					if (array[i].Contains("%STEAM_ID%"))
					{
						array[i] = array[i].Replace("%STEAM_ID%", dataUser.SteamID.ToString());
					}
				}
				if (@class.clanData_0 != null)
				{
					if (array[i].Contains("%CLAN.ID%"))
					{
						array[i] = array[i].Replace("%CLAN.ID%", @class.clanData_0.ID.ToString());
					}
					if (array[i].Contains("%CLAN.CREATED%"))
					{
						array[i] = array[i].Replace("%CLAN.CREATED%", @class.clanData_0.Created.ToString("yyyy-MM-dd HH:mm:ss"));
					}
					if (array[i].Contains("%CLAN.NAME%"))
					{
						array[i] = array[i].Replace("%CLAN.NAME%", @class.clanData_0.Name);
					}
					if (array[i].Contains("%CLAN.ABBR%") && @class.clanData_0.Flags.Has(ClanFlags.can_abbr))
					{
						array[i] = array[i].Replace("%CLAN.ABBR%", @class.clanData_0.Abbr);
					}
					if (array[i].Contains("%CLAN.MOTD%") && @class.clanData_0.Flags.Has(ClanFlags.can_motd))
					{
						array[i] = array[i].Replace("%CLAN.MOTD%", @class.clanData_0.MOTD);
					}
					if (array[i].Contains("%CLAN.BALANCE%") && Economy.Enabled)
					{
						array[i] = array[i].Replace("%CLAN.BALANCE%", @class.clanData_0.Balance.ToString() + Economy.CurrencySign);
					}
					if (array[i].Contains("%CLAN.TAX%"))
					{
						array[i] = array[i].Replace("%CLAN.TAX%", @class.clanData_0.Tax.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.EXPERIENCE%"))
					{
						array[i] = array[i].Replace("%CLAN.EXPERIENCE%", @class.clanData_0.Experience.ToString());
					}
					if (array[i].Contains("%CLAN.LOCATION%") && @class.clanData_0.Flags.Has(ClanFlags.can_warp))
					{
						array[i] = array[i].Replace("%CLAN.LOCATION%", @class.clanData_0.Location.AsString());
					}
					if (array[i].Contains("%CLAN.ONLINE%"))
					{
						array[i] = array[i].Replace("%CLAN.ONLINE%", @class.clanData_0.Online.ToString());
					}
					if (array[i].Contains("%CLAN.MEMBERS.COUNT%"))
					{
						array[i] = array[i].Replace("%CLAN.MEMBERS.COUNT%", @class.clanData_0.Members.Count.ToString());
					}
				}
				if (userData != null)
				{
					if (array[i].Contains("%CLAN.LEADER.STEAM_ID%"))
					{
						array[i] = array[i].Replace("%CLAN.LEADER.STEAM_ID%", userData.SteamID.ToString());
					}
					if (array[i].Contains("%CLAN.LEADER.USERNAME%"))
					{
						array[i] = array[i].Replace("%CLAN.LEADER.USERNAME%", userData.Username);
					}
				}
				if (@class.clanData_0 != null && @class.clanData_0.Level != null)
				{
					if (array[i].Contains("%CLAN.LEVEL%"))
					{
						array[i] = array[i].Replace("%CLAN.LEVEL%", @class.clanData_0.Level.Id.ToString());
					}
					if (array[i].Contains("%CLAN.MEMBERS.MAX%"))
					{
						array[i] = array[i].Replace("%CLAN.MEMBERS.MAX%", @class.clanData_0.Level.MaxMembers.ToString());
					}
					if (array[i].Contains("%CLAN.WARP_TIMEOUT%"))
					{
						array[i] = array[i].Replace("%CLAN.WARP_TIMEOUT%", @class.clanData_0.Level.WarpTimewait.ToString());
					}
					if (array[i].Contains("%CLAN.WARP_COUNTDOWN%"))
					{
						array[i] = array[i].Replace("%CLAN.WARP_COUNTDOWN%", @class.clanData_0.Level.WarpCountdown.ToString());
					}
					if (array[i].Contains("%CLAN.BONUS.CRAFTINGSPEED%") && @class.clanData_0.Level.BonusCraftingSpeed > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.CRAFTINGSPEED%", @class.clanData_0.Level.BonusCraftingSpeed.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.BONUS.GATHERINGWOOD%") && @class.clanData_0.Level.BonusGatheringWood > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.GATHERINGWOOD%", @class.clanData_0.Level.BonusGatheringWood.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.BONUS.GATHERINGROCK%") && @class.clanData_0.Level.BonusGatheringRock > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.GATHERINGROCK%", @class.clanData_0.Level.BonusGatheringRock.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.BONUS.GATHERINGANIMAL%") && @class.clanData_0.Level.BonusGatheringAnimal > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.GATHERINGANIMAL%", @class.clanData_0.Level.BonusGatheringAnimal.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.BONUS.MEMBERS_PAYMURDER%") && @class.clanData_0.Level.BonusMembersPayMurder > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.MEMBERS_PAYMURDER%", @class.clanData_0.Level.BonusMembersPayMurder.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.BONUS.MEMBERS_DEFENSE%") && @class.clanData_0.Level.BonusMembersDefense > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.MEMBERS_DEFENSE%", @class.clanData_0.Level.BonusMembersDefense.ToString() + "%");
					}
					if (array[i].Contains("%CLAN.BONUS.MEMBERS_DAMAGE%") && @class.clanData_0.Level.BonusMembersDamage > 0u)
					{
						array[i] = array[i].Replace("%CLAN.BONUS.MEMBERS_DAMAGE%", @class.clanData_0.Level.BonusMembersDamage.ToString() + "%");
					}
				}
				if (@class.clanData_0 != null && clanLevel != null)
				{
					if (array[i].Contains("%CLAN.NEXT_LEVEL%"))
					{
						array[i] = array[i].Replace("%CLAN.NEXT_LEVEL%", clanLevel.Id.ToString());
					}
					if (array[i].Contains("%CLAN.NEXT_CURRENCY%"))
					{
						array[i] = array[i].Replace("%CLAN.NEXT_CURRENCY%", clanLevel.RequireCurrency.ToString());
					}
					if (array[i].Contains("%CLAN.NEXT_EXPERIENCE%"))
					{
						array[i] = array[i].Replace("%CLAN.NEXT_EXPERIENCE%", clanLevel.RequireExperience.ToString());
					}
					if (array[i].Contains("%CLAN.NEXT_MAXMEMBERS%"))
					{
						array[i] = array[i].Replace("%CLAN.NEXT_MAXMEMBERS%", clanLevel.MaxMembers.ToString());
					}
				}
			}
			return array;
		}

		public static string GetMessageTeleport(string msg, NetUser User = null, WorldZone Zone = null, WorldZone Warp = null)
		{
			string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
			string text = msg;
			Config.Get("MESSAGES." + str, msg, ref text, true);
			text = Helper.ReplaceVariables(User, text, null, "");
			if (Warp == null && Zone != null)
			{
				Warp = Zone.WarpZone;
			}
			if (Zone != null && text.Contains("%ZONE%"))
			{
				text = text.Replace("%ZONE%", Zone.Name);
			}
			if (Warp != null && text.Contains("%INTO%"))
			{
				text = text.Replace("%INTO%", Zone.Name);
			}
			if (Zone != null && text.Contains("%SECONDS%"))
			{
				text = text.Replace("%SECONDS%", Zone.WarpTime.ToString());
			}
			return text;
		}
	}
}
