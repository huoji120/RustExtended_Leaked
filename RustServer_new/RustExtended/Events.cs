using Facepunch;
using Facepunch.Clocks.Counters;
using RustProto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;

namespace RustExtended
{
	public class Events : Facepunch.MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class Class35
		{
			public ulong ulong_0;

			public void method_0(object sender, ElapsedEventArgs e)
			{
				Events.EventSleeperAway(sender, this.ulong_0);
			}
		}

		[CompilerGenerated]
		private sealed class Class36
		{
			public NetUser netUser_0;

			public string string_0;

			public Vector3 vector3_0;

			public void method_0(object sender, ElapsedEventArgs e)
			{
				Events.Teleport_HomeWarp(sender, this.netUser_0, this.string_0, this.vector3_0);
			}
		}

		[CompilerGenerated]
		private sealed class Class37
		{
			public NetUser netUser_0;

			public string string_0;

			public ClanData clanData_0;

			public void method_0(object sender, ElapsedEventArgs e)
			{
				Events.Teleport_ClanWarp(sender, this.netUser_0, this.string_0, this.clanData_0);
			}
		}

		[CompilerGenerated]
		private sealed class Class38
		{
			public Character character_0;

			public NetUser netUser_0;

			public NetUser netUser_1;

			public string string_0;

			public void method_0(object sender, ElapsedEventArgs e)
			{
				Events.Teleport_PlayerTo(sender, this.netUser_0, this.netUser_1, this.string_0, this.character_0.transform.position);
			}
		}

		[CompilerGenerated]
		private sealed class Class39
		{
			public NetUser netUser_0;

			public string string_0;

			public void method_0(object sender, ElapsedEventArgs e)
			{
				Events.EventDisablePvP(this.netUser_0, this.string_0);
			}
		}

		[CompilerGenerated]
		private sealed class Class40
		{
			public NetUser netUser_0;

			public string string_0;

			public bool method_0(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.string_0;
			}
		}

		private static Events events_0 = new Events();

		public static List<EventTimer> Timer = new List<EventTimer>();

		public static List<MOTDEvent> Motd = new List<MOTDEvent>();

		public static DateTime EventTime_DoServerEvents = DateTime.Now;

		public static DateTime EventTime_DoProcessUsers = DateTime.Now;

		public static DateTime EventTime_DoAirdropEvent = DateTime.Now;

		private static bool bool_0 = false;

		private static bool bool_1 = false;

		private static bool bool_2 = false;

		private static bool bool_3 = false;

		private static DateTime dateTime_0 = default(DateTime);

		public static long AirdropLastTime = -1L;

		public static long AirdropNextTime = -1L;

		public static long AirdropNextHour = -1L;

		public static long AirdropNextDay = -1L;

		public static Events Singleton
		{
			get
			{
				return Events.events_0;
			}
		}

		private Events()
		{
		}

		public static void Initialize()
		{
		}

		public static void DoServerEvents()
		{
			if (!Events.bool_0)
			{
				Events.bool_0 = true;
				try
				{
					Core.GetSpawnersSpawns();
					if (Core.GenerateSource.Length > 0 && Core.GenerateOutput.Length > 0 && Core.GenerateSource.Length == Core.GenerateOutput.Length)
					{
						for (int i = 0; i < Core.GenerateSource.Length; i++)
						{
							Helper.GenerateFile(Core.GenerateSource[i], Core.GenerateOutput[i]);
						}
					}
					if (Truth.RustProtectChangeKey && Time.time - Truth.ProtectionUpdateTime > Truth.RustProtectChangeKeyInterval)
					{
						Truth.ProtectionUpdateTime = Time.time + 1f;
						int newSerial = (int)Helper.NewSerial;
						Truth.ProtectionKey ^= newSerial;
						Truth.ProtectionHash ^= newSerial;
						if (server.log > 2)
						{
							ConsoleSystem.Print("Protection Key Changed To=" + string.Format("0x{0:X8}", Truth.ProtectionKey) + ", New Hash=" + string.Format("0x{0:X8}", Truth.ProtectionHash), false);
						}
						foreach (PlayerClient current in PlayerClient.All)
						{
							Users.GetBySteamID(current.userID).ProtectTick = 0;
							Users.GetBySteamID(current.userID).ProtectTime = 0f;
						}
					}
					if (Core.CyclePvP)
					{
						if (server.pvp && (int)EnvironmentControlCenter.Singleton.GetTime() == Core.CyclePvPOff)
						{
							Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Disabled", null, null), null, 5f);
							server.pvp = false;
						}
						else if (!server.pvp && (int)EnvironmentControlCenter.Singleton.GetTime() == Core.CyclePvPOn)
						{
							Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Enabled", null, null), null, 5f);
							server.pvp = true;
						}
					}
					if (Core.CycleInstantCraft)
					{
						if (crafting.instant && (int)EnvironmentControlCenter.Singleton.GetTime() == Core.CycleInstantCraftOff)
						{
							Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.InstantCraft.Disabled", null, null), null, 5f);
							crafting.instant = false;
						}
						else if (!crafting.instant && (int)EnvironmentControlCenter.Singleton.GetTime() == Core.CycleInstantCraftOn)
						{
							Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.InstantCraft.Enabled", null, null), null, 5f);
							crafting.instant = true;
						}
					}
					if (Clans.Database != null)
					{
						uint[] array = Clans.Database.Keys.ToArray<uint>();
						uint[] array2 = array;
						for (int j = 0; j < array2.Length; j++)
						{
							uint key = array2[j];
							if (Clans.Database.ContainsKey(key))
							{
								ClanData clanData = Clans.Database[key];
								uint[] array3 = clanData.Hostile.Keys.ToArray<uint>();
								if (array3.Length > 0)
								{
									uint[] array4 = array3;
									for (int k = 0; k < array4.Length; k++)
									{
										uint key2 = array4[k];
										if (Clans.Database.ContainsKey(key2) && DateTime.Now > clanData.Hostile[key2])
										{
											ClanData clanData2 = Clans.Database[key2];
											string[] messagesClan = Config.GetMessagesClan("Command.Clan.Hostile.Ended", clanData, null, null);
											for (int l = 0; l < messagesClan.Length; l++)
											{
												string text = messagesClan[l];
												clanData.Message(text.Replace("%HOSTILE.CLAN.NAME%", clanData2.Name));
											}
											string[] messagesClan2 = Config.GetMessagesClan("Command.Clan.Hostile.Ended", clanData2, null, null);
											for (int m = 0; m < messagesClan2.Length; m++)
											{
												string text2 = messagesClan2[m];
												clanData2.Message(text2.Replace("%HOSTILE.CLAN.NAME%", clanData.Name));
											}
											clanData.Hostile.Remove(clanData2.ID);
											clanData.Penalty = Helper.StringToTime(Clans.ClanWarEndedPenalty, DateTime.Now);
											clanData2.Hostile.Remove(clanData.ID);
											clanData2.Penalty = Helper.StringToTime(Clans.ClanWarEndedPenalty, DateTime.Now);
											if (Core.DatabaseType.Equals("MYSQL"))
											{
												MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN_HOSTILE, clanData.ID));
												MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN_HOSTILE, clanData2.ID));
											}
										}
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
					Helper.LogWarning("WARNING: Server DoEvents restarted by exception.", true);
				}
				Events.bool_0 = false;
			}
		}

		public static void DoProcessUsers()
		{
			if (!Events.bool_1)
			{
				Events.bool_1 = true;
				foreach (UserData current in Users.All)
				{
					NetUser netUser = NetUser.FindByUserID(current.SteamID);
					if (netUser != null && !netUser.did_join)
					{
						netUser = null;
					}
					List<Countdown> list = new List<Countdown>();
					foreach (Countdown current2 in Users.CountdownList(current.SteamID))
					{
						if (current2.Expires)
						{
							if (current2.Expired)
							{
								list.Add(current2);
							}
							else if (current2.Command.Equals("pvp", StringComparison.OrdinalIgnoreCase) && current.HasFlag(UserFlags.nopvp) && Convert.ToInt32(current2.TimeLeft) < Core.CommandNoPVPCountdown)
							{
								current.SetFlag(UserFlags.nopvp, false);
								if (netUser != null)
								{
									Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.PvP.Enabled", netUser, null), 5f);
								}
								Broadcast.NoticeAll("☢", Config.GetMessage("Command.PvP.NoticeEnabled", null, current.Username), netUser, 5f);
							}
						}
					}
					foreach (Countdown current3 in list)
					{
						Users.CountdownRemove(current.SteamID, current3);
					}
					if (current.PremiumDate.Millisecond != 0 && current.PremiumDate < DateTime.Now)
					{
						Users.SetFlags(current.SteamID, UserFlags.premium, false);
						Users.SetRank(current.SteamID, Users.DefaultRank);
						Users.SetPremiumDate(current.SteamID, default(DateTime));
						Broadcast.Notice(netUser, "☢", Config.GetMessage("Player.Premium.Expired", null, null), 5f);
					}
					if (Core.OwnershipDestroyAutoDisable > 0 && Core.DestoryOwnership.ContainsKey(current.SteamID) && Core.DestoryOwnership[current.SteamID] < DateTime.Now)
					{
						Core.DestoryOwnership.Remove(current.SteamID);
						if (netUser != null)
						{
							Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Destroy.Disabled", null, null), 5f);
						}
					}
					Character character;
                    if (netUser != null && netUser.did_join && netUser.admin && Character.FindByUser(netUser.userID, out character))
					{
						Metabolism component = character.GetComponent<Metabolism>();
						if (component.GetCalorieLevel() < 3000f)
						{
							component.AddCalories(3000f - component.GetCalorieLevel());
						}
						if (component.GetRadLevel() > 0f)
						{
							component.AddAntiRad(component.GetRadLevel());
						}
					}
				}
				Events.bool_1 = false;
				if (Core.DatabaseType.Equals("MYSQL") && !Events.bool_3 && DateTime.Now.Subtract(Events.dateTime_0).TotalMilliseconds > Core.MySQL_SyncInterval)
				{
					if (Core.MySQL_LogLevel > 2u)
					{
						Helper.LogSQL("Thread \"ProcessUsers\": Synchronizing server data from MySQL database", false);
					}
					SystemTimestamp restart = SystemTimestamp.Restart;
					Events.bool_3 = true;
					Core.SQL_UpdateServer();
					if (Core.MySQL_Synchronize)
					{
						Users.SQL_SynchronizeUsers();
					}
					if (Core.MySQL_Synchronize)
					{
						Clans.SQL_SynchronizeClans();
					}
					Events.dateTime_0 = DateTime.Now;
					Events.bool_3 = false;
					restart.Stop();
					if (Core.MySQL_LogLevel > 2u)
					{
						Helper.LogSQL("Thread \"ProcessUsers\": Synchronized, is took " + restart.ElapsedSeconds.ToString("0.0000") + " second(s).", false);
					}
				}
			}
		}

		public static void DoAirdropEvent()
		{
			if (!Events.bool_2)
			{
				Events.bool_2 = true;
				int tickCount = Environment.TickCount;
				if (Core.Airdrop && Core.AirdropPlanes > 0 && NetCull.connections.Length >= airdrop.min_players)
				{
					bool flag = false;
					int num = (int)Math.Abs(EnvironmentControlCenter.Singleton.GetTime());
					if (Core.AirdropInterval && (long)Environment.TickCount >= Events.AirdropNextTime)
					{
						if (Events.AirdropLastTime != -1L)
						{
							flag = true;
						}
						Events.AirdropLastTime = (long)Environment.TickCount;
						Events.AirdropNextTime = Events.AirdropLastTime + (long)(Core.AirdropIntervalTime * 1000);
						if (server.log > 1)
						{
							Helper.Log("[Airdrop.Extended] A next call airdrop after " + Core.AirdropIntervalTime + " second(s).", true);
						}
					}
					if (Core.AirdropDropTime)
					{
						if (Events.AirdropNextHour == -1L && Core.AirdropDropTimeHours.Length > 0)
						{
							Events.AirdropNextDay = (long)(EnvironmentControlCenter.Singleton.sky.Cycle.Day + 1);
							if (Core.AirdropDropTimeHours.Length > 2)
							{
								Events.AirdropNextHour = (long)Core.AirdropDropTimeHours.Length.Random(0);
							}
							else if (Core.AirdropDropTimeHours.Length > 1)
							{
								Events.AirdropNextHour = (long)UnityEngine.Random.Range(Core.AirdropDropTimeHours[0], Core.AirdropDropTimeHours[1]);
							}
							else
							{
								Events.AirdropNextHour = (long)Core.AirdropDropTimeHours[0];
							}
							if (server.log > 1)
							{
								Helper.Log("[Airdrop.Extended] A next call airdrop set on " + Events.AirdropNextHour + " h.", true);
							}
						}
						else if (Events.AirdropNextHour == (long)num && (long)EnvironmentControlCenter.Singleton.sky.Cycle.Day >= Events.AirdropNextDay)
						{
							Events.AirdropNextHour = -1L;
							flag = true;
						}
					}
					if (flag)
					{
						if (Core.AirdropAnnounce)
						{
							Broadcast.MessageAll(Config.GetMessage("Airdrop.Incoming", null, null));
						}
						for (int i = 0; i < Core.AirdropPlanes; i++)
						{
							SupplyDropZone.CallAirDrop();
						}
					}
				}
				Events.bool_2 = false;
			}
		}

		public static void SleeperAway(ulong userID, int lifetime)
		{
			Events.Class35 @class = new Events.Class35();
			@class.ulong_0 = userID;
			EventTimer eventTimer = new EventTimer
			{
				Interval = (double)lifetime,
				AutoReset = false
			};
			eventTimer.Elapsed += new ElapsedEventHandler(@class.method_0);
			eventTimer.Start();
		}

		public static void EventSleeperAway(object obj, ulong userID)
		{
			if (obj != null && obj is EventTimer)
			{
				(obj as EventTimer).Dispose();
			}
			string username = Users.GetUsername(userID);
			RustProto.Avatar avatar = NetUser.LoadAvatar(userID);
			if (avatar != null && avatar.HasAwayEvent && avatar.AwayEvent.Type == AwayEvent.Types.AwayEventType.SLUMBER)
			{
				SleepingAvatar.TransientData transientData = (SleepingAvatar.TransientData)typeof(SleepingAvatar).GetMethod("Close", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
				{
					userID
				});
				if (transientData.exists)
				{
					Helper.Log(string.Concat(new object[]
					{
						"User Sleeping [",
						username,
						":",
						userID,
						"] is disappeared."
					}), true);
					transientData.AdjustIncomingAvatar(ref avatar);
					NetUser.SaveAvatar(userID, ref avatar);
				}
			}
		}

		public static EventTimer TimeEvent_HomeWarp(NetUser Sender, string Command, double time, Vector3 pos)
		{
			Events.Class36 @class = new Events.Class36();
			@class.netUser_0 = Sender;
			@class.string_0 = Command;
			@class.vector3_0 = pos;
			EventTimer result;
			if (time <= 0.0)
			{
				Events.Teleport_HomeWarp(null, @class.netUser_0, @class.string_0, @class.vector3_0);
				result = null;
			}
			else
			{
				EventTimer eventTimer = new EventTimer
				{
					Interval = time * 1000.0,
					AutoReset = false
				};
				eventTimer.Elapsed += new ElapsedEventHandler(@class.method_0);
				eventTimer.Sender = @class.netUser_0;
				eventTimer.Command = @class.string_0;
				eventTimer.Start();
				result = eventTimer;
			}
			return result;
		}

		public static void Teleport_HomeWarp(object obj, NetUser Sender, string command, Vector3 pos)
		{
			if (obj != null && obj is EventTimer)
			{
				(obj as EventTimer).Dispose();
			}
			if (Economy.Enabled && Core.CommandHomePayment > 0uL)
			{
				UserEconomy userEconomy = Economy.Get(Sender.userID);
				string newValue = Core.CommandHomePayment.ToString("N0") + Economy.CurrencySign;
				if (userEconomy.Balance < Core.CommandHomePayment)
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Home.NoEnoughCurrency", Sender, null).Replace("%PRICE%", newValue), 5f);
					return;
				}
				userEconomy.Balance -= Core.CommandHomePayment;
				string newValue2 = userEconomy.Balance.ToString("N0") + Economy.CurrencySign;
				Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", newValue2), null, 0f);
			}
			if (Core.CommandHomeCountdown > 0)
			{
				Users.CountdownAdd(Sender.userID, new Countdown(command, (double)Core.CommandHomeCountdown));
			}
			Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Home.Return", Sender, null), 5f);
			Helper.TeleportTo(Sender, pos);
		}

		public static EventTimer TimeEvent_ClanWarp(NetUser netUser, string Command, double time, ClanData clan)
		{
			Events.Class37 @class = new Events.Class37();
			@class.netUser_0 = netUser;
			@class.string_0 = Command;
			@class.clanData_0 = clan;
			EventTimer result;
			if (time <= 0.0)
			{
				Events.Teleport_ClanWarp(null, @class.netUser_0, @class.string_0, @class.clanData_0);
				result = null;
			}
			else
			{
				EventTimer eventTimer = new EventTimer
				{
					Interval = time * 1000.0,
					AutoReset = false
				};
				eventTimer.Elapsed += new ElapsedEventHandler(@class.method_0);
				eventTimer.Sender = @class.netUser_0;
				eventTimer.Command = @class.string_0;
				eventTimer.Start();
				result = eventTimer;
			}
			return result;
		}

		public static void Teleport_ClanWarp(object obj, NetUser netUser, string command, ClanData clan)
		{
			if (obj != null && obj is EventTimer)
			{
				(obj as EventTimer).Dispose();
			}
			Helper.TeleportTo(netUser, clan.Location);
			if (clan.Level.WarpCountdown > 0u)
			{
				Users.CountdownAdd(netUser.userID, new Countdown(command, clan.Level.WarpCountdown));
			}
			Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Clan.Warp.Warped", netUser, null), 5f);
		}

		public static EventTimer TimeEvent_TeleportTo(NetUser Sender, NetUser Target, string Command, double time)
		{
			Events.Class38 @class = new Events.Class38();
			@class.netUser_0 = Sender;
			@class.netUser_1 = Target;
			@class.string_0 = Command;
			EventTimer result;
			if (Core.CommandTeleportOutdoorsOnly)
			{
				Vector3 position = @class.netUser_1.playerClient.controllable.character.transform.position;
				Collider[] array = Physics.OverlapSphere(position, 1f, 271975425);
				for (int i = 0; i < array.Length; i++)
				{
					Collider component = array[i];
					IDMain main = IDBase.GetMain(component);
					if (!(main == null))
					{
						StructureMaster component2 = main.GetComponent<StructureMaster>();
						if (!(component2 == null) && component2.ownerID != @class.netUser_0.userID && component2.ownerID != @class.netUser_1.userID)
						{
							UserData bySteamID = Users.GetBySteamID(component2.ownerID);
							if (bySteamID == null || (!bySteamID.HasShared(@class.netUser_0.userID) && !bySteamID.HasShared(@class.netUser_1.userID)))
							{
								Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Teleport.NoTeleport", @class.netUser_0, @class.netUser_1.displayName), 5f);
								Broadcast.Notice(@class.netUser_1, "☢", Config.GetMessage("Command.Teleport.NotHere", @class.netUser_1, @class.netUser_0.displayName), 5f);
								result = null;
								return result;
							}
						}
					}
				}
			}
			Broadcast.Message(@class.netUser_0, Config.GetMessage("Command.Teleport.IsConfirm", @class.netUser_0, null).Replace("%USERNAME%", @class.netUser_1.displayName), null, 0f);
			Broadcast.Message(@class.netUser_1, Config.GetMessage("Command.Teleport.Confirmed", @class.netUser_1, null).Replace("%USERNAME%", @class.netUser_0.displayName), null, 0f);
            if (!Character.FindByUser(@class.netUser_1.userID, out @class.character_0))
			{
				result = null;
			}
			else if (time <= 0.0)
			{
				Events.Teleport_PlayerTo(null, @class.netUser_0, @class.netUser_1, @class.string_0, @class.character_0.transform.position);
				result = null;
			}
			else
			{
				EventTimer eventTimer = new EventTimer
				{
					Interval = time * 1000.0,
					AutoReset = false
				};
				eventTimer.Elapsed += new ElapsedEventHandler(@class.method_0);
				eventTimer.Sender = @class.netUser_0;
				eventTimer.Target = @class.netUser_1;
				eventTimer.Command = @class.string_0;
				Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Teleport.Timewait", @class.netUser_0, null).Replace("%TIME%", eventTimer.TimeLeft.ToString()), 5f);
				Broadcast.Notice(@class.netUser_1, "☢", Config.GetMessage("Command.Teleport.Timewait", @class.netUser_1, null).Replace("%TIME%", eventTimer.TimeLeft.ToString()), 5f);
				eventTimer.Start();
				result = eventTimer;
			}
			return result;
		}

		public static void Teleport_PlayerTo(object obj, NetUser Sender, NetUser Target, string command, Vector3 pos)
		{
			if (obj != null && obj is EventTimer)
			{
				(obj as EventTimer).Dispose();
			}
			if (Economy.Enabled && Core.CommandTeleportPayment > 0uL)
			{
				UserEconomy userEconomy = Economy.Get(Sender.userID);
				string newValue = Core.CommandTeleportPayment.ToString("N0") + Economy.CurrencySign;
				if (userEconomy.Balance < Core.CommandTeleportPayment)
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Teleport.NoEnoughCurrency", Sender, null).Replace("%PRICE%", newValue), 5f);
					return;
				}
				userEconomy.Balance -= Core.CommandTeleportPayment;
				string newValue2 = userEconomy.Balance.ToString("N0") + Economy.CurrencySign;
				Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", newValue2), null, 0f);
			}
			Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Teleport.TeleportOnPlayer", Sender, null).Replace("%USERNAME%", Target.displayName), 5f);
			Broadcast.Notice(Target, "☢", Config.GetMessage("Command.Teleport.TeleportedPlayer", Target, null).Replace("%USERNAME%", Sender.displayName), 5f);
			if (Core.CommandTeleportCountdown > 0)
			{
				Users.CountdownAdd(Sender.userID, new Countdown(command, (double)Core.CommandTeleportCountdown));
			}
			Helper.TeleportTo(Sender, pos);
		}

		public static bool DisablePvP(NetUser netUser, string Command, double time)
		{
			Events.Class39 @class = new Events.Class39();
			@class.netUser_0 = netUser;
			@class.string_0 = Command;
			bool result;
			if (@class.netUser_0 == null)
			{
				result = false;
			}
			else
			{
				EventTimer eventTimer = new EventTimer
				{
					Interval = time,
					AutoReset = false
				};
				eventTimer.Elapsed += new ElapsedEventHandler(@class.method_0);
				eventTimer.Sender = @class.netUser_0;
				eventTimer.Command = @class.string_0;
				eventTimer.Start();
				result = true;
			}
			return result;
		}

		public static void EventDisablePvP(NetUser netUser, string Command)
		{
			Events.Class40 @class = new Events.Class40();
			@class.netUser_0 = netUser;
			@class.string_0 = Command;
			EventTimer eventTimer = Events.Timer.Find(new Predicate<EventTimer>(@class.method_0));
			if (eventTimer != null)
			{
				eventTimer.Dispose();
				if (@class.netUser_0 != null)
				{
					Users.SetFlags(@class.netUser_0.userID, UserFlags.nopvp, true);
					int num = Core.CommandNoPVPDuration + Core.CommandNoPVPCountdown;
					if (num > 0)
					{
						Users.CountdownAdd(@class.netUser_0.userID, new Countdown(@class.string_0, (double)num));
					}
					TimeSpan timeSpan = TimeSpan.FromSeconds((double)Core.CommandNoPVPDuration);
					Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.PvP.Disabled", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds)), 5f);
					Broadcast.NoticeAll("☢", Config.GetMessage("Command.PvP.NoticeDisabled", @class.netUser_0, null), @class.netUser_0, 5f);
				}
			}
		}

		public static void EventServerShutdown(EventTimer sender, int ShutdownTime, ref int Timeleft)
		{
			if (Timeleft != 0)
			{
				if (Timeleft <= 5)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 1f);
				}
				else if (Timeleft == 10)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 5f);
				}
				else if (Timeleft == 30)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 10f);
				}
				else if (Timeleft == 60)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 10f);
				}
				else if (Timeleft == ShutdownTime)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.Shutdown", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 10f);
				}
			}
			if (Timeleft > 0)
			{
				Timeleft--;
			}
			else
			{
				try
				{
					if (sender != null)
					{
						sender.Stop();
					}
					AvatarSaveProc.SaveAll();
					ServerSaveManager.AutoSave();
					Process.GetCurrentProcess().Kill();
				}
				catch (Exception ex)
				{
					Helper.LogError(ex.ToString(), true);
				}
			}
		}

		public static void EventServerRestart(EventTimer sender, int ShutdownTime, ref int Timeleft)
		{
			if (Timeleft != 0)
			{
				if (Timeleft <= 5)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 1f);
				}
				else if (Timeleft == 10)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 5f);
				}
				else if (Timeleft == 30)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 10f);
				}
				else if (Timeleft == 60)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 10f);
				}
				else if (Timeleft == ShutdownTime)
				{
					Broadcast.NoticeAll("☢", Config.GetMessage("Server.Restart", null, null).Replace("%SECONDS%", Timeleft.ToString()), null, 10f);
				}
			}
			if (Timeleft > 0)
			{
				Timeleft--;
			}
			else
			{
				try
				{
					if (sender != null)
					{
						sender.Stop();
					}
					AvatarSaveProc.SaveAll();
					ServerSaveManager.AutoSave();
					string text = Environment.GetCommandLineArgs()[0];
					string arguments = string.Join(" ", Environment.GetCommandLineArgs()).Replace(text, "").Trim();
					Process.Start(text, arguments);
					Process.GetCurrentProcess().Kill();
				}
				catch (Exception ex)
				{
					Helper.LogError(ex.ToString(), true);
				}
			}
		}
	}
}
