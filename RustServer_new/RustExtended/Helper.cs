using Facepunch.MeshBatch;
using LitJson;
using Rust.Steam;
using RustProto;
using RustProto.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class Helper
	{
		[CompilerGenerated]
		private sealed class Class41
		{
			public uint uint_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return clanLevel_0.RequireLevel == Clans.Database[this.uint_0].Level.Id;
			}
		}

		[CompilerGenerated]
		private sealed class Class42
		{
			public string string_0;

			public bool method_0(string string_1)
			{
				return string_1.Contains("=" + this.string_0 + "=");
			}
		}

		[CompilerGenerated]
		private sealed class Class43
		{
			public string string_0;

			public StringComparison stringComparison_0;

			public bool method_0(PlayerClient playerClient_0)
			{
				return playerClient_0.netUser.displayName.Contains(this.string_0);
			}

			public bool method_1(PlayerClient playerClient_0)
			{
				return playerClient_0.netUser.displayName.EndsWith(this.string_0, this.stringComparison_0);
			}

			public bool method_2(PlayerClient playerClient_0)
			{
				return playerClient_0.netUser.displayName.StartsWith(this.string_0, this.stringComparison_0);
			}

			public bool method_3(PlayerClient playerClient_0)
			{
				return playerClient_0.netUser.displayName.Equals(this.string_0, this.stringComparison_0);
			}
		}

		[CompilerGenerated]
		private sealed class Class44
		{
			public ulong ulong_0;

			public bool method_0(DeployableObject deployableObject_0)
			{
				return deployableObject_0.ownerID == this.ulong_0;
			}
		}

		public static Version RequireVersion = Assembly.GetExecutingAssembly().GetName().Version;

		public static string RustLogFileName;

		public static StreamWriter RustLogStream;

		public static string ChatLogFileName;

		public static StreamWriter ChatLogStream;

		public static string ServSQLFileName;

		public static StreamWriter ServSQLStream;

		public static Dictionary<ulong, List<string>> userArmor = new Dictionary<ulong, List<string>>();

		public static string AssemblyNotVerified = "The assembly versions is incompatible, please update RustExtended.";

		public static string ServerDenyToStarted = "You can't run server with RustExtended on this IP with this port.\nPlease purchase this modification from a developer before to use.\nVisit web-site: http://www.rust-extended.ru/ for more details.";

		private static long long_0 = 0L;

		[CompilerGenerated]
		private static Predicate<Assembly> predicate_0;

		[CompilerGenerated]
		private static Predicate<AssemblyName> predicate_1;

		[CompilerGenerated]
		private static Predicate<Assembly> predicate_2;

		[CompilerGenerated]
		private static Predicate<AssemblyName> predicate_3;

		public static string RustLogFile
		{
			get
			{
				return Path.Combine(Core.LogsPath, "Rust" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
			}
		}

		public static string ChatLogFile
		{
			get
			{
				return Path.Combine(Core.LogsPath, "Chat" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
			}
		}

		public static string ServSQLFile
		{
			get
			{
				return Path.Combine(Core.LogsPath, "MySQL" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
			}
		}

		public static uint NewSerial
		{
			get
			{
				return (uint)Helper.NewSerial64;
			}
		}

		public static ulong NewSerial64
		{
			get
			{
				return (ulong)(DateTime.Now.Ticks ^ (Helper.long_0 += 1L));
			}
		}

		[DllImport("KERNEL32.DLL")]
		private static extern uint GetSystemDefaultLCID();

		public static void Initialize()
		{
			if (!Directory.Exists(Core.LogsPath))
			{
				Directory.CreateDirectory(Core.LogsPath);
			}
			string lastSaveFile = Helper.GetLastSaveFile();
			if (lastSaveFile != null)
			{
				Debug.LogError("Bad save or not found " + ServerSaveManager.autoSavePath);
				Debug.LogError("Restored save from " + lastSaveFile);
				File.Copy(lastSaveFile, ServerSaveManager.autoSavePath, true);
			}
			Method.Initialize();
			Core.BetaVersion = Method.Invoke("extended.beta").AsBoolean;
			Core.ServerIP = Method.Invoke("RustExtended.Loader.ServerIP").AsString;
			Core.ExternalIP = Method.Invoke("RustExtended.Loader.ExternalIP").AsString;
		}

		public static bool AssemblyVerify()
		{
			List<Assembly> list = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
			if (Helper.predicate_0 == null)
			{
				Helper.predicate_0 = new Predicate<Assembly>(Helper.smethod_0);
			}
			Assembly assembly = list.Find(Helper.predicate_0);
			bool result;
			if (object.Equals(assembly, null))
			{
				result = false;
			}
			else
			{
				List<AssemblyName> list2 = assembly.GetReferencedAssemblies().ToList<AssemblyName>();
				if (Helper.predicate_1 == null)
				{
					Helper.predicate_1 = new Predicate<AssemblyName>(Helper.smethod_1);
				}
				AssemblyName assemblyName = list2.Find(Helper.predicate_1);
				if (Core.Version.Major == assemblyName.Version.Major && Core.Version.Minor == assemblyName.Version.Minor)
				{
					if (Core.Version.Build == assemblyName.Version.Build)
					{
						List<Assembly> list3 = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
						if (Helper.predicate_2 == null)
						{
							Helper.predicate_2 = new Predicate<Assembly>(Helper.smethod_2);
						}
						assembly = list3.Find(Helper.predicate_2);
						if (object.Equals(assembly, null))
						{
							result = false;
							return result;
						}
						List<AssemblyName> list4 = assembly.GetReferencedAssemblies().ToList<AssemblyName>();
						if (Helper.predicate_3 == null)
						{
							Helper.predicate_3 = new Predicate<AssemblyName>(Helper.smethod_3);
						}
						if (list4.Find(Helper.predicate_3) == null)
						{
							Helper.LogWarning("WARNING: The assembly uLink.dll do not linked with RustExtended. Anti-cheat RustProtect could not enabled on server.", true);
						}
						result = true;
						return result;
					}
				}
				string assemblyNotVerified = Helper.AssemblyNotVerified;
				Helper.AssemblyNotVerified = string.Concat(new string[]
				{
					assemblyNotVerified,
					"\nIncompatible Assembly-CSharp.dll v",
					assemblyName.Version.ToString(3),
					", required v",
					Helper.RequireVersion.ToString(3)
				});
				result = false;
			}
			return result;
		}

		public static string GetLastSaveFile()
		{
			FileInfo fileInfo = null;
			string text = ServerSaveManager.autoSavePath;
			if (File.Exists(text))
			{
				fileInfo = new FileInfo(text);
			}
			string result;
			if (fileInfo == null || fileInfo.Length == 0L)
			{
				for (int i = 0; i < 20; i++)
				{
					text = ServerSaveManager.autoSavePath + ".old." + i;
					if (File.Exists(text) && new FileInfo(text).Length > 0L)
					{
						result = text;
						return result;
					}
				}
			}
			result = null;
			return result;
		}

		public static uint GetSystemLocaleID()
		{
			return Helper.GetSystemDefaultLCID();
		}

		public static List<string> GetConfigSections(string filename)
		{
			List<string> result;
			if (!File.Exists(filename))
			{
				result = null;
			}
			else
			{
				List<string> list = File.ReadAllLines(filename).ToList<string>();
				List<string> list2 = new List<string>();
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i].Trim();
					if (!string.IsNullOrEmpty(text) && !text.StartsWith("//"))
					{
						if (text.Contains("//"))
						{
							text = text.Split(new string[]
							{
								"//"
							}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						}
						if (!string.IsNullOrEmpty(text) && text.StartsWith("[") && text.EndsWith("]"))
						{
							string item = text.Substring(1, text.Length - 2).Trim();
							if (!list2.Contains(item))
							{
								list2.Add(item);
							}
						}
					}
				}
				result = list2;
			}
			return result;
		}

		public static Dictionary<string, string> GetConfigValues(string filename, string section)
		{
			Dictionary<string, string> result;
			if (!File.Exists(filename))
			{
				result = null;
			}
			else
			{
				List<string> list = File.ReadAllLines(filename).ToList<string>();
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				string text = null;
				for (int i = 0; i < list.Count; i++)
				{
					string text2 = list[i].Trim();
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
							if (text2.StartsWith("[") && text2.EndsWith("]"))
							{
								text = text2.Substring(1, text2.Length - 2);
							}
							else if ((section == null || text.Equals(section, StringComparison.CurrentCultureIgnoreCase)) && text2.Contains("="))
							{
								string[] array = text2.Split(new char[]
								{
									'='
								});
								array[0] = array[0].Trim();
								array[1] = array[1].Trim();
								if (!dictionary.ContainsKey(array[0]))
								{
									dictionary.Add(array[0].Trim(), array[1].Trim());
								}
							}
						}
					}
				}
				result = dictionary;
			}
			return result;
		}

		public static IniFile IniFile(string filename)
		{
			return new IniFile(filename);
		}

		public static void Log(string msg, bool inConsole = true)
		{
			if (Helper.RustLogFile != Helper.RustLogFileName && Helper.RustLogStream != null)
			{
				Helper.RustLogStream.Close();
				Helper.RustLogStream = null;
			}
			if (Helper.RustLogStream == null)
			{
				Helper.RustLogStream = new StreamWriter(new FileStream(Helper.RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.RustLogStream != null)
			{
				Helper.RustLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
				Helper.RustLogStream.Flush();
				File.SetLastWriteTime(Helper.RustLogFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.Print(msg, false);
			}
		}

		public static void LogWarning(string msg, bool inConsole = true)
		{
			if (Helper.RustLogFile != Helper.RustLogFileName && Helper.RustLogStream != null)
			{
				Helper.RustLogStream.Close();
				Helper.RustLogStream = null;
			}
			if (Helper.RustLogStream == null)
			{
				Helper.RustLogStream = new StreamWriter(new FileStream(Helper.RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.RustLogStream != null)
			{
				Helper.RustLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
				Helper.RustLogStream.Flush();
				File.SetLastWriteTime(Helper.RustLogFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.PrintWarning(msg, false);
			}
		}

		public static void LogError(string msg, bool inConsole = true)
		{
			if (Helper.RustLogFile != Helper.RustLogFileName && Helper.RustLogStream != null)
			{
				Helper.RustLogStream.Close();
				Helper.RustLogStream = null;
			}
			if (Helper.RustLogStream == null)
			{
				Helper.RustLogStream = new StreamWriter(new FileStream(Helper.RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.RustLogStream != null)
			{
				Helper.RustLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":[ERROR]: " + msg);
				Helper.RustLogStream.Flush();
				File.SetLastWriteTime(Helper.RustLogFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.PrintError(msg, false);
			}
		}

		public static void LogChat(string msg, bool inConsole = false)
		{
			if (Helper.ChatLogFile != Helper.ChatLogFileName && Helper.ChatLogStream != null)
			{
				Helper.ChatLogStream.Close();
				Helper.ChatLogStream = null;
			}
			if (Helper.ChatLogStream == null)
			{
				Helper.ChatLogStream = new StreamWriter(new FileStream(Helper.ChatLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.ChatLogStream != null)
			{
				Helper.ChatLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
				Helper.ChatLogStream.Flush();
				File.SetLastWriteTime(Helper.ChatLogFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.Print(msg, false);
			}
		}

		public static void LogSQL(string msg, bool inConsole = false)
		{
			if (Helper.ServSQLFile != Helper.ServSQLFileName && Helper.ServSQLStream != null)
			{
				Helper.ServSQLStream.Close();
				Helper.ServSQLStream = null;
			}
			if (Helper.ServSQLStream == null)
			{
				Helper.ServSQLStream = new StreamWriter(new FileStream(Helper.ServSQLFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.ServSQLStream != null)
			{
				Helper.ServSQLStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
				Helper.ServSQLStream.Flush();
				File.SetLastWriteTime(Helper.ServSQLFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.Print("MYSQL: " + msg, false);
			}
		}

		public static void LogSQLWarning(string msg, bool inConsole = true)
		{
			if (Helper.ServSQLFile != Helper.ServSQLFileName && Helper.ServSQLStream != null)
			{
				Helper.ServSQLStream.Close();
				Helper.ServSQLStream = null;
			}
			if (Helper.ServSQLStream == null)
			{
				Helper.ServSQLStream = new StreamWriter(new FileStream(Helper.ServSQLFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.ServSQLStream != null)
			{
				Helper.ServSQLStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
				Helper.ServSQLStream.Flush();
				File.SetLastWriteTime(Helper.ServSQLFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.PrintWarning("MYSQL: " + msg, false);
			}
		}

		public static void LogSQLError(string msg, bool inConsole = true)
		{
			if (Helper.ServSQLFile != Helper.ServSQLFileName && Helper.ServSQLStream != null)
			{
				Helper.ServSQLStream.Close();
				Helper.ServSQLStream = null;
			}
			if (Helper.ServSQLStream == null)
			{
				Helper.ServSQLStream = new StreamWriter(new FileStream(Helper.ServSQLFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
			}
			if (Helper.ServSQLStream != null)
			{
				Helper.ServSQLStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":[ERROR]: " + msg);
				Helper.ServSQLStream.Flush();
				File.SetLastWriteTime(Helper.ServSQLFile, DateTime.Now);
			}
			if (inConsole)
			{
				ConsoleSystem.PrintError("MYSQL ERROR: " + msg, false);
			}
		}

		public static void GenerateFile(string sourcefile, string targetfile)
		{
			if (File.Exists(sourcefile))
			{
				List<string> list = new List<string>();
				string[] array = File.ReadAllLines(sourcefile);
				if (array.Length != 0)
				{
					List<string> list2 = new List<string>();
					List<string> list3 = new List<string>();
					List<string> list4 = new List<string>();
					List<string> list5 = new List<string>();
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text = array2[i];
						string text2 = text.Replace("%SERVER.HOSTNAME%", server.hostname);
						text2 = text2.Replace("%SERVER.IP%", UnityEngine.MasterServer.ipAddress);
						text2 = text2.Replace("%SERVER.PORT%", server.port.ToString());
						text2 = text2.Replace("%SERVER.STEAMID%", Server.SteamID.ToString());
						text2 = text2.Replace("%SERVER.STEAMGROUP%", Server.SteamGroup.ToString());
						text2 = text2.Replace("%SERVER.OFFICIAL%", Server.Official ? "YES" : "NO");
						text2 = text2.Replace("%SERVER.MODDED%", Server.Modded ? "YES" : "NO");
						text2 = text2.Replace("%SERVER.MAP%", server.map);
						text2 = text2.Replace("%SERVER.PVP%", server.pvp ? "ON" : "OFF");
						text2 = text2.Replace("%SERVER.MAXPLAYERS%", NetCull.maxConnections.ToString());
						text2 = text2.Replace("%SERVER.PLAYERS%", NetCull.connections.Length.ToString());
						text2 = text2.Replace("%BANLIST.COUNT%", Banned.Count.ToString());
						text2 = text2.Replace("%CLANS.COUNT%", Clans.Count.ToString());
						text2 = text2.Replace("%CLANS.CREATE_COST%", Clans.CreateCost.ToString());
						text2 = text2.Replace("%CLANS.LEVELS.COUNT%", Clans.Levels.Count.ToString());
						text2 = text2.Replace("%ECONOMY%", Economy.Enabled ? "YES" : "NO");
						text2 = text2.Replace("%ECONOMY.SIGN%", Economy.CurrencySign);
						text2 = text2.Replace("%ECONOMY.CURRENCYSIGN%", Economy.CurrencySign);
						text2 = text2.Replace("%ECONOMY.STARTBALANCE%", Economy.StartBalance.ToString());
						text2 = text2.Replace("%ECONOMY.COST_RABBIT%", Economy.CostRabbit.ToString());
						text2 = text2.Replace("%ECONOMY.COST_CHICKEN%", Economy.CostChicken.ToString());
						text2 = text2.Replace("%ECONOMY.COST_STAG%", Economy.CostStag.ToString());
						text2 = text2.Replace("%ECONOMY.COST_BOAR%", Economy.CostBoar.ToString());
						text2 = text2.Replace("%ECONOMY.COST_WOLF%", Economy.CostWolf.ToString());
						text2 = text2.Replace("%ECONOMY.COST_BEAR%", Economy.CostBear.ToString());
						text2 = text2.Replace("%ECONOMY.COST_MUTANTWOLF%", Economy.CostMutantWolf.ToString());
						text2 = text2.Replace("%ECONOMY.COST_MUTANTBEAR%", Economy.CostMutantBear.ToString());
						text2 = text2.Replace("%ECONOMY.FEEDEATH%", Economy.FeeDeath ? "YES" : "NO");
						text2 = text2.Replace("%ECONOMY.FEEDEATH.PERCENT%", Economy.FeeDeathPercent.ToString());
						text2 = text2.Replace("%ECONOMY.FEESUICIDE%", Economy.FeeSuicide ? "YES" : "NO");
						text2 = text2.Replace("%ECONOMY.FEESUICIDE.PERCENT%", Economy.FeeSuicidePercent.ToString());
						text2 = text2.Replace("%ECONOMY.FEEMURDER%", Economy.FeeMurder ? "YES" : "NO");
						text2 = text2.Replace("%ECONOMY.FEEMURDER.PERCENT%", Economy.FeeMurderPercent.ToString());
						text2 = text2.Replace("%ECONOMY.PAYMURDER%", Economy.PayMurder ? "YES" : "NO");
						text2 = text2.Replace("%ECONOMY.PAYMURDER.PERCENT%", Economy.PayMurderPercent.ToString());
						text2 = text2.Replace("%ECONOMY.PAYMURDER%", Economy.PayMurder ? "YES" : "NO");
						text2 = text2.Replace("%SHOP%", Shop.Enabled ? "YES" : "NO");
						text2 = text2.Replace("%SHOP.CAN_BUY%", Shop.CanBuy ? "YES" : "NO");
						text2 = text2.Replace("%SHOP.CAN_SELL%", Shop.CanSell ? "YES" : "NO");
						text2 = text2.Replace("%SHOP.TRADEZONEONLY%", Shop.TradeZoneOnly ? "YES" : "NO");
						if (text2.Contains("<USERLIST>"))
						{
							list2.Add(text2);
						}
						if (text2.Contains("</USERLIST>"))
						{
							list2.Add(text2);
							foreach (UserData current in Users.All)
							{
								foreach (string current2 in list2)
								{
									if (!current2.Replace("<USERLIST>", "").Replace("</USERLIST>", "").IsEmpty())
									{
										string text3 = current2.Replace("%USER.COUNT%", Users.Count.ToString());
										text3 = text3.Replace("%USER.STEAM_ID%", current.SteamID.ToString());
										text3 = text3.Replace("%USER.USERNAME%", current.Username);
										text3 = text3.Replace("%USER.PASSWORD%", current.Password);
										text3 = text3.Replace("%USER.COMMENTS%", current.Comments);
										text3 = text3.Replace("%USER.RANK%", current.Rank.ToString());
										text3 = text3.Replace("%USER.FLAGS%", current.Flags.ToString());
										text3 = text3.Replace("%USER.ZONE%", (current.Zone != null) ? current.Zone.Name : "");
										text3 = text3.Replace("%USER.CLAN%", (current.Clan != null) ? current.Clan.Name : "");
										text3 = text3.Replace("%USER.CLAN.ABBR%", (current.Clan != null) ? current.Clan.Abbr : "");
										text3 = text3.Replace("%USER.CLAN.CREATED%", (current.Clan != null) ? current.Clan.Created.ToString("MM/dd/yyyy HH:mm:ss") : "");
										text3 = text3.Replace("%USER.CLAN.LEVEL%", (current.Clan != null) ? current.Clan.Level.ToString() : "");
										text3 = text3.Replace("%USER.CLAN.BALANCE%", (current.Clan != null) ? current.Clan.Balance.ToString() : "");
										text3 = text3.Replace("%USER.CLAN.EXPERIENCE%", (current.Clan != null) ? current.Clan.Experience.ToString() : "");
										text3 = text3.Replace("%USER.CLAN.EXP%", (current.Clan != null) ? current.Clan.Experience.ToString() : "");
										text3 = text3.Replace("%USER.CLAN.MEMBERS.COUNT%", (current.Clan != null) ? current.Clan.Members.Count.ToString() : "");
										text3 = text3.Replace("%USER.CLAN.MEMBERS.MAX%", (current.Clan != null) ? current.Clan.Level.MaxMembers.ToString() : "");
										text3 = text3.Replace("%USER.CLAN.MOTD%", (current.Clan != null) ? current.Clan.MOTD : "");
										text3 = text3.Replace("%USER.VIOLATIONS%", current.Violations.ToString());
										text3 = text3.Replace("%USER.VIOLATIONDATE%", current.ViolationDate.ToString("MM/dd/yyyy HH:mm:ss"));
										text3 = text3.Replace("%USER.FIRSTCONNECTDATE%", current.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
										text3 = text3.Replace("%USER.LASTCONNECTDATE%", current.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
										text3 = text3.Replace("%USER.FIRSTCONNECTIP%", current.FirstConnectIP);
										text3 = text3.Replace("%USER.LASTCONNECTIP%", current.LastConnectIP);
										text3 = text3.Replace("%USER.PREMIUMDATE%", current.PremiumDate.ToString("MM/dd/yyyy HH:mm:ss"));
										text3 = text3.Replace("%USER.POS%", current.Position.AsString());
										text3 = text3.Replace("%USER.POS.X%", current.Position.x.ToString());
										text3 = text3.Replace("%USER.POS.Y%", current.Position.y.ToString());
										text3 = text3.Replace("%USER.POS.Z%", current.Position.z.ToString());
										text3 = text3.Replace("%USER.PING%", (current.AveragePing > 0) ? current.AveragePing.ToString() : "");
										UserEconomy userEconomy = Economy.Get(current.SteamID);
										if (userEconomy != null)
										{
											text3 = text3.Replace("%USER.BALANCE%", userEconomy.Balance.ToString()).Replace("%USER.MONEY%", userEconomy.Balance.ToString());
											text3 = text3.Replace("%USER.KILLED.ANIMALS%", userEconomy.AnimalsKilled.ToString());
											ulong num = (ulong)((long)userEconomy.AnimalsKilled);
											text3 = text3.Replace("%USER.KILLED.MUTANTS%", userEconomy.MutantsKilled.ToString());
											num += (ulong)((long)userEconomy.MutantsKilled);
											text3 = text3.Replace("%USER.KILLED.NPC%", num.ToString());
											text3 = text3.Replace("%USER.KILLED.PLAYERS%", userEconomy.PlayersKilled.ToString());
											text3 = text3.Replace("%USER.KILLED%", (num + (ulong)((long)userEconomy.PlayersKilled)).ToString());
											text3 = text3.Replace("%USER.DEATHS%", userEconomy.Deaths.ToString());
										}
										list.Add(text3);
									}
								}
							}
							list2.Clear();
						}
						else if (list2.Count > 0)
						{
							list2.Add(text2);
						}
						else
						{
							if (text2.Contains("<PLAYERLIST>"))
							{
								list3.Add(text2);
							}
							if (text2.Contains("</PLAYERLIST>"))
							{
								list3.Add(text2);
								foreach (PlayerClient current3 in PlayerClient.All)
								{
									foreach (string current4 in list3)
									{
										if (!current4.Replace("<PLAYERLIST>", "").Replace("</PLAYERLIST>", "").IsEmpty())
										{
											string text4 = current4.Replace("%PLAYER.NUMBER%", current3.netPlayer.id.ToString());
											UserData bySteamID = Users.GetBySteamID(current3.userID);
											if (bySteamID != null)
											{
												text4 = text4.Replace("%PLAYER.STEAM_ID%", bySteamID.SteamID.ToString());
												text4 = text4.Replace("%PLAYER.USERNAME%", bySteamID.Username);
												text4 = text4.Replace("%PLAYER.PASSWORD%", bySteamID.Password);
												text4 = text4.Replace("%PLAYER.COMMENTS%", bySteamID.Comments);
												text4 = text4.Replace("%PLAYER.RANK%", bySteamID.Rank.ToString());
												text4 = text4.Replace("%PLAYER.FLAGS%", bySteamID.Flags.ToString());
												text4 = text4.Replace("%PLAYER.ZONE%", (bySteamID.Zone != null) ? bySteamID.Zone.Name : "");
												text4 = text4.Replace("%PLAYER.CLAN%", (bySteamID.Clan != null) ? bySteamID.Clan.Name : "");
												text4 = text4.Replace("%PLAYER.CLAN.ABBR%", (bySteamID.Clan != null) ? bySteamID.Clan.Abbr : "");
												text4 = text4.Replace("%PLAYER.CLAN.CREATED%", (bySteamID.Clan != null) ? bySteamID.Clan.Created.ToString("MM/dd/yyyy HH:mm:ss") : "");
												text4 = text4.Replace("%PLAYER.CLAN.LEVEL%", (bySteamID.Clan != null) ? bySteamID.Clan.Level.ToString() : "");
												text4 = text4.Replace("%PLAYER.CLAN.BALANCE%", (bySteamID.Clan != null) ? bySteamID.Clan.Balance.ToString() : "");
												text4 = text4.Replace("%PLAYER.CLAN.EXPERIENCE%", (bySteamID.Clan != null) ? bySteamID.Clan.Experience.ToString() : "");
												text4 = text4.Replace("%PLAYER.CLAN.EXP%", (bySteamID.Clan != null) ? bySteamID.Clan.Experience.ToString() : "");
												text4 = text4.Replace("%PLAYER.CLAN.MEMBERS.COUNT%", (bySteamID.Clan != null) ? bySteamID.Clan.Members.Count.ToString() : "");
												text4 = text4.Replace("%PLAYER.CLAN.MEMBERS.MAX%", (bySteamID.Clan != null) ? bySteamID.Clan.Level.MaxMembers.ToString() : "");
												text4 = text4.Replace("%PLAYER.CLAN.MOTD%", (bySteamID.Clan != null) ? bySteamID.Clan.MOTD : "");
												text4 = text4.Replace("%PLAYER.VIOLATIONS%", bySteamID.Violations.ToString());
												text4 = text4.Replace("%PLAYER.VIOLATIONDATE%", bySteamID.ViolationDate.ToString("MM/dd/yyyy HH:mm:ss"));
												text4 = text4.Replace("%PLAYER.FIRSTCONNECTDATE%", bySteamID.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
												text4 = text4.Replace("%PLAYER.LASTCONNECTDATE%", bySteamID.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
												text4 = text4.Replace("%PLAYER.FIRSTCONNECTIP%", bySteamID.FirstConnectIP);
												text4 = text4.Replace("%PLAYER.LASTCONNECTIP%", bySteamID.LastConnectIP);
												text4 = text4.Replace("%PLAYER.PREMIUMDATE%", bySteamID.PremiumDate.ToString("MM/dd/yyyy HH:mm:ss"));
												text4 = text4.Replace("%PLAYER.POS%", bySteamID.Position.AsString());
												text4 = text4.Replace("%PLAYER.POS.X%", bySteamID.Position.x.ToString());
												text4 = text4.Replace("%PLAYER.POS.Y%", bySteamID.Position.y.ToString());
												text4 = text4.Replace("%PLAYER.POS.Z%", bySteamID.Position.z.ToString());
												text4 = text4.Replace("%PLAYER.PING%", (bySteamID.AveragePing > 0) ? bySteamID.AveragePing.ToString() : "");
											}
											UserEconomy userEconomy2 = Economy.Get(current3.userID);
											if (userEconomy2 != null)
											{
												text4 = text4.Replace("%PLAYER.BALANCE%", userEconomy2.Balance.ToString()).Replace("%PLAYER.MONEY%", userEconomy2.Balance.ToString());
												text4 = text4.Replace("%PLAYER.KILLED.ANIMALS%", userEconomy2.AnimalsKilled.ToString());
												ulong num2 = (ulong)((long)userEconomy2.AnimalsKilled);
												text4 = text4.Replace("%PLAYER.KILLED.MUTANTS%", userEconomy2.MutantsKilled.ToString());
												num2 += (ulong)((long)userEconomy2.MutantsKilled);
												text4 = text4.Replace("%PLAYER.KILLED.NPC%", num2.ToString());
												text4 = text4.Replace("%PLAYER.KILLED.PLAYERS%", userEconomy2.PlayersKilled.ToString());
												text4 = text4.Replace("%PLAYER.KILLED%", (num2 + (ulong)((long)userEconomy2.PlayersKilled)).ToString());
												text4 = text4.Replace("%PLAYER.DEATHS%", userEconomy2.Deaths.ToString());
											}
											list.Add(text4);
										}
									}
								}
								list3.Clear();
							}
							else if (list3.Count > 0)
							{
								list3.Add(text2);
							}
							else
							{
								if (text2.Contains("<CLANLIST>"))
								{
									list5.Add(text2);
								}
								if (text2.Contains("</CLANLIST>"))
								{
									list5.Add(text2);
									using (Dictionary<uint, ClanData>.KeyCollection.Enumerator enumerator4 = Clans.Database.Keys.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											Predicate<ClanLevel> predicate = null;
											Helper.Class41 @class = new Helper.Class41();
											@class.uint_0 = enumerator4.Current;
											foreach (string current5 in list5)
											{
												if (!current5.Replace("<CLANLIST>", "").Replace("</CLANLIST>", "").IsEmpty())
												{
													string text5 = current5.Replace("%CLAN.ID%", @class.uint_0.ToHEX(true));
													text5 = text5.Replace("%CLAN.NAME%", Clans.Database[@class.uint_0].Name);
													text5 = text5.Replace("%CLAN.ABBR%", Clans.Database[@class.uint_0].Abbr);
													text5 = text5.Replace("%CLAN.CREATED%", Clans.Database[@class.uint_0].Created.ToString("MM/dd/yyyy HH:mm:ss"));
													text5 = text5.Replace("%CLAN.FLAGS%", Clans.Database[@class.uint_0].Flags.ToString());
													text5 = text5.Replace("%CLAN.BALANCE%", Clans.Database[@class.uint_0].Balance.ToString());
													text5 = text5.Replace("%CLAN.EXPERIENCE%", Clans.Database[@class.uint_0].Experience.ToString());
													text5 = text5.Replace("%CLAN.EXP%", Clans.Database[@class.uint_0].Experience.ToString());
													text5 = text5.Replace("%CLAN.TAX%", Clans.Database[@class.uint_0].Tax.ToString());
													text5 = text5.Replace("%CLAN.MOTD%", Clans.Database[@class.uint_0].MOTD);
													text5 = text5.Replace("%CLAN.LOCATION%", Clans.Database[@class.uint_0].Location.AsString());
													text5 = text5.Replace("%CLAN.LOCATION.X%", Clans.Database[@class.uint_0].Location.x.ToString());
													text5 = text5.Replace("%CLAN.LOCATION.Y%", Clans.Database[@class.uint_0].Location.y.ToString());
													text5 = text5.Replace("%CLAN.LOCATION.Z%", Clans.Database[@class.uint_0].Location.z.ToString());
													text5 = text5.Replace("%CLAN.MEMBERS.MAX%", Clans.Database[@class.uint_0].Level.MaxMembers.ToString());
													text5 = text5.Replace("%CLAN.MEMBERS.COUNT%", Clans.Database[@class.uint_0].Members.Count.ToString());
													text5 = text5.Replace("%CLAN.MEMBERS.ONLINE%", Clans.Database[@class.uint_0].Online.ToString());
													text5 = text5.Replace("%CLAN.LEVEL%", Clans.Database[@class.uint_0].Level.Id.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.TAX%", Clans.Database[@class.uint_0].Level.CurrencyTax.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.CAN_MOTD%", Clans.Database[@class.uint_0].Level.FlagMotd ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEVEL.CAN_ABBR%", Clans.Database[@class.uint_0].Level.FlagAbbr ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEVEL.CAN_FFIRE%", Clans.Database[@class.uint_0].Level.FlagFFire ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEVEL.CAN_TAX%", Clans.Database[@class.uint_0].Level.FlagTax ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEVEL.CAN_HOUSE%", Clans.Database[@class.uint_0].Level.FlagHouse ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEVEL.CAN_DECLARE%", Clans.Database[@class.uint_0].Level.FlagDeclare ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEVEL.MAXMEMBERS%", Clans.Database[@class.uint_0].Level.MaxMembers.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSCRAFTINGSPEED%", Clans.Database[@class.uint_0].Level.BonusCraftingSpeed.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSGATHERINGANIMAL%", Clans.Database[@class.uint_0].Level.BonusGatheringAnimal.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSGATHERINGROCK%", Clans.Database[@class.uint_0].Level.BonusGatheringRock.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSGATHERINGWOOD%", Clans.Database[@class.uint_0].Level.BonusGatheringWood.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSMEMBERSPAYMURDER%", Clans.Database[@class.uint_0].Level.BonusMembersPayMurder.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSMEMBERSDEFENSE%", Clans.Database[@class.uint_0].Level.BonusMembersDefense.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.BONUSMEMBERSDAMAGE%", Clans.Database[@class.uint_0].Level.BonusMembersDamage.ToString());
													UserData bySteamID2 = Users.GetBySteamID(Clans.Database[@class.uint_0].LeaderID);
													text5 = text5.Replace("%CLAN.LEADER%", (bySteamID2 != null) ? "YES" : "NO");
													text5 = text5.Replace("%CLAN.LEADER.STEAM_ID%", (bySteamID2 == null) ? "" : bySteamID2.SteamID.ToString());
													text5 = text5.Replace("%CLAN.LEADER.USERNAME%", (bySteamID2 == null) ? "" : bySteamID2.Username);
													text5 = text5.Replace("%CLAN.LEADER.FLAGS%", (bySteamID2 == null) ? "" : bySteamID2.Flags.ToString());
													text5 = text5.Replace("%CLAN.WAR.COUNT%", Clans.Database[@class.uint_0].Hostile.Count.ToString());
													text5 = text5.Replace("%CLAN.HOSTILE.COUNT%", Clans.Database[@class.uint_0].Hostile.Count.ToString());
													List<ClanLevel> levels = Clans.Levels;
													if (predicate == null)
													{
														predicate = new Predicate<ClanLevel>(@class.method_0);
													}
													ClanLevel clanLevel = levels.Find(predicate);
													text5 = text5.Replace("%CLAN.NEXTLEVEL%", (clanLevel == null) ? "" : clanLevel.Id.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.TAX%", (clanLevel == null) ? "" : clanLevel.CurrencyTax.ToString());
													text5 = text5.Replace("%CLAN.LEVEL.CAN_MOTD%", (clanLevel == null) ? "" : (clanLevel.FlagMotd ? "YES" : "NO"));
													text5 = text5.Replace("%CLAN.LEVEL.CAN_ABBR%", (clanLevel == null) ? "" : (clanLevel.FlagAbbr ? "YES" : "NO"));
													text5 = text5.Replace("%CLAN.LEVEL.CAN_FFIRE%", (clanLevel == null) ? "" : (clanLevel.FlagFFire ? "YES" : "NO"));
													text5 = text5.Replace("%CLAN.LEVEL.CAN_TAX%", (clanLevel == null) ? "" : (clanLevel.FlagTax ? "YES" : "NO"));
													text5 = text5.Replace("%CLAN.LEVEL.CAN_HOUSE%", (clanLevel == null) ? "" : (clanLevel.FlagHouse ? "YES" : "NO"));
													text5 = text5.Replace("%CLAN.LEVEL.CAN_DECLARE%", (clanLevel == null) ? "" : (clanLevel.FlagDeclare ? "YES" : "NO"));
													text5 = text5.Replace("%CLAN.NEXTLEVEL.MAXMEMBERS%", (clanLevel == null) ? "" : clanLevel.MaxMembers.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSCRAFTINGSPEED%", (clanLevel == null) ? "" : clanLevel.BonusCraftingSpeed.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSGATHERINGANIMAL%", (clanLevel == null) ? "" : clanLevel.BonusGatheringAnimal.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSGATHERINGROCK%", (clanLevel == null) ? "" : clanLevel.BonusGatheringRock.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSGATHERINGWOOD%", (clanLevel == null) ? "" : clanLevel.BonusGatheringWood.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSMEMBERSPAYMURDER%", (clanLevel == null) ? "" : clanLevel.BonusMembersPayMurder.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSMEMBERSDEFENSE%", (clanLevel == null) ? "" : clanLevel.BonusMembersDefense.ToString());
													text5 = text5.Replace("%CLAN.NEXTLEVEL.BONUSMEMBERSDAMAGE%", (clanLevel == null) ? "" : clanLevel.BonusMembersDamage.ToString());
													list.Add(text5);
												}
											}
										}
									}
									list5.Clear();
								}
								else if (list5.Count > 0)
								{
									list5.Add(text2);
								}
								else
								{
									if (text2.Contains("<BANLIST>"))
									{
										list4.Add(text2);
									}
									if (text2.Contains("</BANLIST>"))
									{
										list4.Add(text2);
										foreach (ulong current6 in Banned.Database.Keys)
										{
											foreach (string current7 in list4)
											{
												if (!current7.Replace("<BANLIST>", "").Replace("</BANLIST>", "").IsEmpty())
												{
													string text6 = current7.Replace("%BANNED.STEAM_ID%", current6.ToString());
													UserData bySteamID3 = Users.GetBySteamID(current6);
													text6 = text6.Replace("%BANNED.USERNAME%", (bySteamID3 == null) ? "" : bySteamID3.Username);
													text6 = text6.Replace("%BANNED.IP%", Banned.Database[current6].IP);
													text6 = text6.Replace("%BANNED.DATE%", Banned.Database[current6].Time.ToString("MM/dd/yyyy HH:mm:ss"));
													text6 = text6.Replace("%BANNED.PERIOD%", Banned.Database[current6].Period.ToString("MM/dd/yyyy HH:mm:ss"));
													text6 = text6.Replace("%BANNED.REASON%", Banned.Database[current6].Reason);
													text6 = text6.Replace("%BANNED.DETAILS%", Banned.Database[current6].Details);
													list.Add(text6);
												}
											}
										}
										list4.Clear();
									}
									else if (list4.Count > 0)
									{
										list4.Add(text2);
									}
									else
									{
										list.Add(text2);
									}
								}
							}
						}
					}
					using (StreamWriter streamWriter = File.CreateText(targetfile))
					{
						foreach (string current8 in list)
						{
							streamWriter.WriteLine(current8);
						}
					}
				}
			}
		}

		public static string GetChatTextColor(string color)
		{
			int num = color.Replace("#", "").Replace("$", "").ToInt32();
			string result;
			if (num != 0)
			{
				result = "[COLOR#" + num.ToHEX(false) + "]";
			}
			else
			{
				result = "";
			}
			return result;
		}

		public static string ObsceneText(string text)
		{
			string[] array = text.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				foreach (string current in Core.ForbiddenObscene)
				{
					if (array[i].ToUpper().Contains(current))
					{
						array[i] = new string('*', array[i].Length);
						break;
					}
				}
			}
			return string.Join(" ", array);
		}

		public static string QuoteSafe(string text)
		{
			if (text.StartsWith("\"") && text.EndsWith("\""))
			{
				text = text.Trim(new char[]
				{
					'"'
				});
			}
			return text = "\"" + text.Replace("\"", "\\\"") + "\"";
		}

		public static string[] SplitQuotes(string input, char separator = ' ')
		{
			input = input.Replace("\\\"", "&qute;");
			MatchCollection matchCollection = new Regex("\"([^\"]+)\"|'([^']+)'|([^" + separator + "]+)", RegexOptions.Compiled).Matches(input);
			string[] array = new string[matchCollection.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = matchCollection[i].Groups[0].Value.Trim(new char[]
				{
					' ',
					'\t',
					'"'
				});
				array[i] = array[i].Replace("&qute;", "\"");
			}
			return array;
		}

		public static string qushijian()
		{
			return DateTime.Now.ToString("yyyyMMdd");
		}

		public static void runcmd(string fangfa)
		{
			ConsoleSystem.Run(fangfa, false);
		}

		public static string NiceName(string input)
		{
			input = input.Replace("_A", "").Replace("A(Clone)", "").Replace("(Clone)", "");
			MatchCollection matchCollection = new Regex("([A-Z]*[^A-Z_]+)", RegexOptions.Compiled).Matches(input);
			string[] array = new string[matchCollection.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = matchCollection[i].Groups[0].Value.Trim();
			}
			return string.Join(" ", array);
		}

		public static string[] WarpChatText(string input, int maxlength = 80, string prefix = "", string suffix = "")
		{
			List<string> list = new List<string>();
			if (input.Length > maxlength)
			{
				while (input.Length > maxlength)
				{
					StringBuilder stringBuilder = new StringBuilder();
					List<string> list2 = input.Split(new char[]
					{
						' '
					}).ToList<string>();
					int startIndex = maxlength;
					int length = input.Length - maxlength;
					string text;
					if (list2.Count == 1)
					{
						text = stringBuilder.Append(input.Substring(0, maxlength) + "-").ToString();
					}
					else
					{
						int num = 0;
						while (num < list2.Count && maxlength >= (stringBuilder + list2[num]).Length)
						{
							stringBuilder.Append(" " + list2[num]);
							num++;
						}
						text = stringBuilder.ToString().Trim();
						startIndex = text.Length + 1;
						length = input.Length - text.Length - 1;
					}
					input = input.Substring(startIndex, length);
					list.Add(prefix + text + suffix);
				}
			}
			list.Add(prefix + input + suffix);
			return list.ToArray();
		}

		public static DateTime StringToTime(string time, DateTime startTime = default(DateTime))
		{
			foreach (Match match in Regex.Matches(time, "(\\d+\\s*(y|M|d|h|m|s))"))
			{
				if (match.Value.EndsWith("y"))
				{
					startTime = startTime.AddYears(int.Parse(match.Value.Trim(new char[]
					{
						'y'
					})));
				}
				if (match.Value.EndsWith("M"))
				{
					startTime = startTime.AddMonths(int.Parse(match.Value.Trim(new char[]
					{
						'M'
					})));
				}
				if (match.Value.EndsWith("d"))
				{
					startTime = startTime.AddDays(double.Parse(match.Value.Trim(new char[]
					{
						'd'
					})));
				}
				if (match.Value.EndsWith("h"))
				{
					startTime = startTime.AddHours(double.Parse(match.Value.Trim(new char[]
					{
						'h'
					})));
				}
				if (match.Value.EndsWith("m"))
				{
					startTime = startTime.AddMinutes(double.Parse(match.Value.Trim(new char[]
					{
						'm'
					})));
				}
				if (match.Value.EndsWith("s"))
				{
					startTime = startTime.AddSeconds(double.Parse(match.Value.Trim(new char[]
					{
						's'
					})));
				}
			}
			return startTime;
		}

		public static int[] StringToInt32(string value)
		{
			int[] array = new int[0];
			if (!string.IsNullOrEmpty(value))
			{
				byte[] bytes = Encoding.Unicode.GetBytes(value);
				Array.Resize<int>(ref array, bytes.Length / 4 + 1);
				Array.Resize<byte>(ref bytes, array.Length * 4);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = BitConverter.ToInt32(bytes, i * 4);
				}
			}
			return array;
		}

		public static string Int32ToString(int[] values)
		{
			string text = "";
			for (int i = 0; i < values.Length; i++)
			{
				int num = values[i];
				if (num > 0)
				{
					byte[] bytes = BitConverter.GetBytes(num);
					text += Encoding.Unicode.GetString(bytes);
				}
			}
			string text2 = text;
			char[] trimChars = new char[1];
			return text2.Trim(trimChars);
		}

		public static byte[] GetMD5(string input)
		{
			MD5 mD = MD5.Create();
			return mD.ComputeHash(Encoding.ASCII.GetBytes(input));
		}

		[DllImport("USER32.DLL", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr CallWindowProcW([In] byte[] byte_0, IntPtr intptr_0, int int_0, [In] [Out] byte[] byte_1, IntPtr intptr_1);

		[DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);

		public static ulong GetProcessorId()
		{
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
			if (IntPtr.Size == 8)
			{
				array4 = array2;
			}
			else
			{
				array4 = array;
			}
			IntPtr intPtr = new IntPtr(array4.Length);
			int num;
			if (!Helper.VirtualProtect(array4, intPtr, 64, out num))
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			intPtr = new IntPtr(array3.Length);
			ulong result;
			if (Helper.CallWindowProcW(array4, IntPtr.Zero, 0, array3, intPtr) != IntPtr.Zero)
			{
				result = BitConverter.ToUInt64(array3, 0);
			}
			else
			{
				result = 0uL;
			}
			return result;
		}

		public static string ReplaceVariables(NetUser netUser, string text, string varFrom = null, string varTo = "")
		{
			if (!string.IsNullOrEmpty(varFrom) && text.Contains(varFrom))
			{
				text = text.Replace(varFrom, varTo);
			}
			if (netUser != null && text.Contains("%USERNAME%"))
			{
				text = text.Replace("%USERNAME%", netUser.displayName);
			}
			if (netUser != null && text.Contains("%STEAM_ID%"))
			{
				text = text.Replace("%STEAM_ID%", netUser.userID.ToString());
			}
			if (text.Contains("%CORE_VERSION%"))
			{
				text = text.Replace("%CORE_VERSION%", Core.Version.ToString());
			}
			if (text.Contains("%MAXPLAYERS%"))
			{
				text = text.Replace("%MAXPLAYERS%", (NetCull.maxConnections - Core.PremiumConnections).ToString());
			}
			if (text.Contains("%SERVERNAME%"))
			{
				text = text.Replace("%SERVERNAME%", Core.ServerName);
			}
			if (text.Contains("%ONLINE%"))
			{
				text = text.Replace("%ONLINE%", PlayerClient.All.Count.ToString());
			}
			return text;
		}

		public static bool CreateFileBackup(string filename)
		{
			bool result;
			if (!File.Exists(filename))
			{
				result = false;
			}
			else
			{
				if (File.Exists(filename + ".old.20"))
				{
					File.Delete(filename + ".old.20");
				}
				for (int i = 19; i >= 0; i--)
				{
					if (File.Exists(filename + ".old." + i))
					{
						File.Move(filename + ".old." + i, filename + ".old." + (i + 1));
					}
				}
				File.Move(filename, filename + ".old.0");
				result = true;
			}
			return result;
		}

		public static List<string> GetAvailabledCommands(UserData userData)
		{
			int num = 0;
			List<string> list = new List<string>();
			NetUser netUser = NetUser.FindByUserID(userData.SteamID);
			List<string> result;
			if (netUser == null)
			{
				result = list;
			}
			else
			{
				foreach (string current in Core.Commands)
				{
					Helper.Class42 @class = new Helper.Class42();
					if (!string.IsNullOrEmpty(current))
					{
						string[] array = current.Split(new char[]
						{
							'='
						});
						if (array.Length >= 2)
						{
							@class.string_0 = array[1].Replace("!", "").Trim();
							if (!@class.string_0.IsEmpty() && !list.Exists(new Predicate<string>(@class.method_0)))
							{
								if (!userData.HasFlag(UserFlags.admin) && !netUser.admin)
								{
									if (int.TryParse(array[0], out num) && num == userData.Rank && current.Contains("=!"))
									{
										list.Add(current.Replace("=!", "="));
									}
									if (int.TryParse(array[0], out num) && num <= userData.Rank && !current.Contains("=!"))
									{
										list.Add(current);
									}
								}
								else
								{
									list.Add(current.Replace("=!", "="));
								}
							}
						}
					}
				}
				result = list;
			}
			return result;
		}

		public static bool AvatarSave(ref Character character, NetUser netUser)
		{
			bool result;
			if (character != null && netUser != null && netUser.avatar != null)
			{
				bool flag;
				using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler = RustProto.Avatar.Recycler())
				{
					RustProto.Avatar.Builder builder = recycler.OpenBuilder();
					Character character2 = character.masterCharacter;
					if (character2 == null)
					{
						character2 = character;
					}
					builder.SetPos(character2.origin);
					builder.SetAng(character2.rotation);
					using (Recycler<Vitals, Vitals.Builder> recycler2 = Vitals.Recycler())
					{
						Vitals.Builder vitals = recycler2.OpenBuilder();
						character.GetLocal<Metabolism>().SaveVitals(ref vitals);
						character.takeDamage.SaveVitals(ref vitals);
						builder.SetVitals(vitals);
					}
					character.GetLocal<PlayerInventory>().SaveToAvatar(ref builder);
					character.netUser.avatar = builder.Build();
					flag = true;
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool AvatarLoad(ref Character character, NetUser netUser)
		{
			bool result;
			if (character != null && netUser != null && netUser.avatar != null)
			{
				if (netUser.avatar.HasVitals)
				{
					character.GetLocal<Metabolism>().LoadVitals(netUser.avatar.Vitals);
					character.takeDamage.LoadVitals(netUser.avatar.Vitals);
				}
				character.GetLocal<PlayerInventory>().LoadToAvatar(ref netUser.avatar);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool UserAlreadyConnected(ulong steam_id, out NetUser netUser)
		{
			uLink.NetworkPlayer[] connections = NetCull.connections;
			bool result;
			for (int i = 0; i < connections.Length; i++)
			{
				uLink.NetworkPlayer networkPlayer = connections[i];
				object localData = networkPlayer.GetLocalData();
				if (localData is NetUser)
				{
					NetUser netUser2;
					netUser = (netUser2 = (NetUser)localData);
					if (netUser2 != null && netUser.userID == steam_id)
					{
						result = true;
						return result;
					}
				}
			}
			netUser = null;
			result = false;
			return result;
		}

		public static bool DisconnectBySteamID(ulong steam_id)
		{
			bool result = false;
			uLink.NetworkPlayer[] connections = NetCull.connections;
			for (int i = 0; i < connections.Length; i++)
			{
				uLink.NetworkPlayer networkPlayer = connections[i];
				NetUser netUser = networkPlayer.GetLocalData() as NetUser;
				if (netUser != null && netUser.userID == steam_id)
				{
					netUser.Kick(NetError.ConnectionTimeout, true);
					result = true;
				}
			}
			return result;
		}

		public static uint DisconnectByUsername(string username)
		{
			uint num = 0u;
			uLink.NetworkPlayer[] connections = NetCull.connections;
			for (int i = 0; i < connections.Length; i++)
			{
				uLink.NetworkPlayer networkPlayer = connections[i];
				NetUser netUser = networkPlayer.GetLocalData() as NetUser;
				if (netUser != null && netUser.displayName == username)
				{
					netUser.Kick(NetError.ConnectionTimeout, true);
					num += 1u;
				}
			}
			return num;
		}

		public static JsonData GetPlayerBans(string steam_id)
		{
			string requestUriString = "http://api.steampowered.com/ISteamUser/GetPlayerBans/v1/?key=" + Core.SteamAPIKey + "&steamids=" + steam_id;
			WebRequest webRequest = WebRequest.Create(requestUriString);
			webRequest.Timeout = 5000;
			StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream(), Encoding.UTF8);
			JsonData jsonData = JsonMapper.ToObject(streamReader.ReadToEnd());
			JsonData result;
            if (jsonData["players"].Count > 0)
			{
                result = jsonData["players"][0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static NetUser TeleportTo(NetUser netuser, Vector3 position)
		{
			Character character;
			NetUser result;
            if (!Character.FindByUser(netuser.userID, out character))
			{
				result = netuser;
			}
			else if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z))
			{
				result = netuser;
			}
			else if (position == Vector3.zero || object.Equals(character.transform.position, position))
			{
				result = netuser;
			}
			else if (character.transform.position == position)
			{
				result = netuser;
			}
			else
			{
				float num = Vector3.Distance(character.transform.position, position);
				float num2 = Mathf.Round(num / 1000f);
				if (num2 > 5f)
				{
					num2 = 5f;
				}
				position += new Vector3(0f, num2, 0f);
				Helper.LogChat(string.Concat(new object[]
				{
					"User [",
					netuser.displayName,
					":",
					netuser.userID,
					"] teleported from ",
					character.transform.position,
					" to ",
					position,
					" (distance: ",
					num,
					"m, lifted: ",
					num2,
					"m)"
				}), false);
				RustServerManagement.Get().TeleportPlayerToWorld(netuser.networkPlayer, position);
				netuser.truthDetector.NoteTeleported(position, 0.0);
				result = netuser;
			}
			return result;
		}

		public static List<Vector3> GetPlayerSpawns(NetUser netUser, bool Valid = true)
		{
			return Helper.GetPlayerSpawns(netUser.userID, Valid);
		}

		public static List<Vector3> GetPlayerSpawns(PlayerClient player, bool Valid = true)
		{
			return Helper.GetPlayerSpawns(player.userID, Valid);
		}

		public static List<Vector3> GetPlayerSpawns(ulong userID, bool Valid = true)
		{
			List<Vector3> list = new List<Vector3>();
			RustServerManagement rustServerManagement = RustServerManagement.Get();
			foreach (DeployableObject current in rustServerManagement.playerSpawns)
			{
				if (current.ownerID == userID)
				{
					DeployedRespawn component = current.GetComponent<DeployedRespawn>();
					if (!(component == null) && (!Valid || component.IsValidToSpawn()))
					{
						list.Add(component.GetSpawnPos() + new Vector3(0f, 0.5f, 0f));
					}
				}
			}
			return list;
		}

		public static PlayerClient GetPlayerClient(uLink.NetworkPlayer player)
		{
			PlayerClient result;
			PlayerClient.Find(player, out result, false);
			return result;
		}

		public static PlayerClient GetPlayerClient(ulong SteamID)
		{
			PlayerClient result;
			PlayerClient.FindByUserID(SteamID, out result);
			return result;
		}

		public static PlayerClient GetPlayerClient(string Value)
		{
			Helper.Class43 @class = new Helper.Class43();
			@class.string_0 = Value.Replace("*", "");
			ulong userID;
			PlayerClient playerClient;
			PlayerClient result;
			if (ulong.TryParse(Value, out userID) && PlayerClient.FindByUserID(userID, out playerClient))
			{
				result = playerClient;
			}
			else
			{
				@class.stringComparison_0 = (Users.UniqueNames ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
				if (Value.StartsWith("*") && Value.EndsWith("*"))
				{
					result = PlayerClient.All.Find(new Predicate<PlayerClient>(@class.method_0));
				}
				else if (Value.StartsWith("*"))
				{
					result = PlayerClient.All.Find(new Predicate<PlayerClient>(@class.method_1));
				}
				else if (Value.EndsWith("*"))
				{
					result = PlayerClient.All.Find(new Predicate<PlayerClient>(@class.method_2));
				}
				else
				{
					result = PlayerClient.All.Find(new Predicate<PlayerClient>(@class.method_3));
				}
			}
			return result;
		}

		public static NetUser GetNetUser(string Value)
		{
			PlayerClient playerClient = Helper.GetPlayerClient(Value);
			NetUser result;
			if (!(playerClient != null))
			{
				result = null;
			}
			else
			{
				result = playerClient.netUser;
			}
			return result;
		}

		public static int GiveItem(PlayerClient player, string itemName, int quantity = 1, int slots = -1)
		{
			return Helper.GiveItem(player, DatablockDictionary.GetByName(itemName), quantity, slots);
		}

		public static int GiveItem(PlayerClient player, ItemDataBlock itemData, int quantity = 1, int modSlots = -1)
		{
			PlayerInventory component = player.controllable.GetComponent<PlayerInventory>();
			Inventory.Slot.Preference slotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, itemData.IsSplittable(), Inventory.Slot.Kind.Belt);
			return Helper.GiveItem(component, itemData, slotPreference, quantity, modSlots);
		}

		public static int GiveItem(PlayerInventory inventory, ItemDataBlock itemData, Inventory.Slot.Preference slotPreference, int quantity = 1, int modSlots = -1)
		{
			int num = 0;
			int result;
			if (!(itemData == null) && !(inventory == null))
			{
				if (itemData.IsSplittable())
				{
					num += quantity - inventory.AddItemAmount(itemData, quantity, Inventory.AmountMode.Default, slotPreference);
				}
				else
				{
					int maxEligableSlots = (int)itemData.GetMaxEligableSlots();
					for (int i = 0; i < quantity; i++)
					{
						IInventoryItem inventoryItem = inventory.AddItem(itemData, slotPreference, itemData._spawnUsesMax);
						if (object.ReferenceEquals(inventoryItem, null))
						{
							break;
						}
						num++;
						if (modSlots != -1 && maxEligableSlots != 0)
						{
							IHeldItem heldItem = inventoryItem as IHeldItem;
							if (!object.ReferenceEquals(heldItem, null))
							{
								heldItem.SetTotalModSlotCount(Mathf.Min(modSlots, maxEligableSlots));
							}
						}
					}
				}
				result = num;
			}
			else
			{
				result = num;
			}
			return result;
		}

		public static bool GetEquipedArmor(PlayerClient playerClient, out List<IInventoryItem> items)
		{
			Inventory component = playerClient.controllable.GetComponent<Inventory>();
			items = new List<IInventoryItem>();
			for (int i = 0; i < component.slotCount - 1; i++)
			{
				IInventoryItem inventoryItem;
				if (component.GetItem(i, out inventoryItem))
				{
					try
					{
						if (i > 35 && i < 40 && inventoryItem != null)
						{
							items.Add(inventoryItem);
						}
					}
					catch
					{
					}
				}
			}
			return items.Count<IInventoryItem>() > 0;
		}

		public static bool EquipArmor(PlayerClient playerClient, string itemName, bool replaceCurrent = false)
		{
			int num = 0;
			Inventory component = playerClient.controllable.GetComponent<Inventory>();
			if (itemName.Contains("Helmet"))
			{
				num = 36;
			}
			if (itemName.Contains("Vest"))
			{
				num = 37;
			}
			if (itemName.Contains("Pants"))
			{
				num = 38;
			}
			if (itemName.Contains("Boots"))
			{
				num = 39;
			}
			bool result;
			if (num > 0)
			{
				if (replaceCurrent)
				{
					component.RemoveItem(num);
				}
				component.AddItemAmount(DatablockDictionary.GetByName(itemName), 1, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, false, Inventory.Slot.KindFlags.Armor));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void ClearArmor(PlayerClient playerClient)
		{
			Inventory component = playerClient.controllable.GetComponent<Inventory>();
			for (int i = 36; i < 40; i++)
			{
				component.RemoveItem(i);
			}
		}

		public static int InventoryItemCount(Inventory inventory, ItemDataBlock datablock)
		{
			int num = 0;
			Inventory.OccupiedIterator occupiedIterator = inventory.occupiedIterator;
			while (occupiedIterator.Next())
			{
				if (occupiedIterator.item.datablock == datablock)
				{
					if (occupiedIterator.item.datablock.IsSplittable())
					{
						num += occupiedIterator.item.uses;
					}
					else
					{
						num++;
					}
				}
			}
			return num;
		}

		public static List<IInventoryItem> InventoryGetItems(Inventory inventory)
		{
			List<IInventoryItem> list = new List<IInventoryItem>();
			Inventory.OccupiedIterator occupiedIterator = inventory.occupiedIterator;
			while (occupiedIterator.Next())
			{
				list.Add(occupiedIterator.item);
			}
			return list;
		}

		public static int InventoryItemRemove(Inventory inventory, ItemDataBlock datablock, int quantity)
		{
			int i = 0;
			while (i < quantity)
			{
				IInventoryItem inventoryItem = inventory.FindItem(datablock);
				if (inventoryItem == null)
				{
					break;
				}
				if (!inventoryItem.datablock.IsSplittable())
				{
					i++;
					inventory.RemoveItem(inventoryItem);
				}
				else
				{
					int num = quantity - i;
					if (inventoryItem.uses > num)
					{
						i += num;
						inventoryItem.SetUses(inventoryItem.uses - num);
					}
					else
					{
						i += inventoryItem.uses;
						inventory.RemoveItem(inventoryItem);
					}
				}
			}
			return i;
		}

		public static void InventoryItemRemove(PlayerClient player, ItemDataBlock Item)
		{
			Inventory component = player.controllable.GetComponent<Inventory>();
			if (!(component == null) && !(Item == null) && !Item.transferable)
			{
				IInventoryItem inventoryItem = component.FindItem(Item);
				while (!object.ReferenceEquals(inventoryItem, null))
				{
					component.RemoveItem(inventoryItem);
					inventoryItem = component.FindItem(Item);
				}
			}
		}

		public static int GetPlayerObjects(ulong userID)
		{
			Helper.Class44 @class = new Helper.Class44();
			@class.ulong_0 = userID;
			int num = UnityEngine.Object.FindObjectsOfType<DeployableObject>().Count(new Func<DeployableObject, bool>(@class.method_0));
			foreach (StructureMaster current in StructureMaster.AllStructures)
			{
				if (current.ownerID == @class.ulong_0)
				{
					num += current._structureComponents.Count;
				}
			}
			return num;
		}

		public static int GetPlayerComponents(ulong userID)
		{
			int num = 0;
			foreach (StructureMaster current in StructureMaster.AllStructures)
			{
				if (current.ownerID == userID)
				{
					num += current._structureComponents.Count;
				}
			}
			return num;
		}

		public static void UpgradePlayerWeapon(IWeaponItem Weapon)
		{
			Weapon.SetTotalModSlotCount(4);
			ItemModDataBlock itemModDataBlock = DatablockDictionary.GetByName("Silencer") as ItemModDataBlock;
			ItemModDataBlock itemModDataBlock2 = DatablockDictionary.GetByName("Holo sight") as ItemModDataBlock;
			ItemModDataBlock itemModDataBlock3 = DatablockDictionary.GetByName("Laser Sight") as ItemModDataBlock;
			ItemModDataBlock itemModDataBlock4 = DatablockDictionary.GetByName("Flashlight Mod") as ItemModDataBlock;
			if (!Weapon.itemMods.Contains(itemModDataBlock))
			{
				Weapon.AddMod(itemModDataBlock);
			}
			if (!Weapon.itemMods.Contains(itemModDataBlock2))
			{
				Weapon.AddMod(itemModDataBlock2);
			}
			if (!Weapon.itemMods.Contains(itemModDataBlock3))
			{
				Weapon.AddMod(itemModDataBlock3);
			}
			if (!Weapon.itemMods.Contains(itemModDataBlock4))
			{
				Weapon.AddMod(itemModDataBlock4);
			}
		}

		public static Ray GetEyesRay(NetUser player)
		{
			Ray result;
			if (player == null)
			{
				result = default(Ray);
			}
			else
			{
				result = Helper.GetEyesRay(player.playerClient);
			}
			return result;
		}

		public static Ray GetEyesRay(PlayerClient player)
		{
			Ray result;
			if (player == null)
			{
				result = default(Ray);
			}
			else
			{
				result = Helper.GetEyesRay(player.controllable);
			}
			return result;
		}

		public static Ray GetEyesRay(Controllable controllable)
		{
			Ray result;
			if (controllable == null)
			{
				result = default(Ray);
			}
			else
			{
				result = Helper.GetEyesRay(controllable.character);
			}
			return result;
		}

		public static Ray GetEyesRay(Character character)
		{
			Ray result;
			if (character == null)
			{
				result = default(Ray);
			}
			else
			{
				Vector3 position = character.transform.position;
				Vector3 direction = character.eyesRay.direction;
				position.y += (character.stateFlags.crouch ? 1.1f : 1.6f);
				result = new Ray(position, direction);
			}
			return result;
		}

		public static Ray GetLookRay(NetUser player)
		{
			Ray result;
			if (player == null)
			{
				result = default(Ray);
			}
			else
			{
				result = Helper.GetLookRay(player.playerClient);
			}
			return result;
		}

		public static Ray GetLookRay(PlayerClient player)
		{
			Ray result;
			if (player == null)
			{
				result = default(Ray);
			}
			else
			{
				result = Helper.GetLookRay(player.controllable);
			}
			return result;
		}

		public static Ray GetLookRay(Controllable controllable)
		{
			Ray result;
			if (controllable == null)
			{
				result = default(Ray);
			}
			else
			{
				result = Helper.GetLookRay(controllable.character);
			}
			return result;
		}

		public static Ray GetLookRay(Character character)
		{
			Ray result;
			if (character == null)
			{
				result = default(Ray);
			}
			else
			{
				Vector3 position = character.transform.position;
				Vector3 direction = character.eyesRay.direction;
				position.y += (character.stateFlags.crouch ? 0.85f : 1.65f);
				result = new Ray(position, direction);
			}
			return result;
		}

		public static int DestroyStructure(StructureMaster master)
		{
			int result;
			if (master == null)
			{
				result = -1;
			}
			else
			{
				int count = master._structureComponents.Count;
				int num = 0;
				bool xzjz = Core.xzjz;
				ulong ownerID = master.ownerID;
				bool flag = RustHook.diji.ContainsKey(ownerID);
				foreach (StructureComponent current in master._structureComponents)
				{
					TakeDamage.HurtSelf(current, 3.40282347E+38f, null);
					if (current.name.IndexOf("Foundation") != -1 && flag && xzjz)
					{
						num++;
					}
				}
				if (master._structureComponents.Count == 0)
				{
					num = 1;
					NetCull.Destroy(master.gameObject);
				}
				if (flag && xzjz)
				{
					string setting = RustHook.dijiusers.GetSetting(ownerID.ToString(), "diji");
					int num2 = 1;
					if (setting != "")
					{
						num2 = Convert.ToInt32(setting);
					}
					int num3 = num2 - num;
					if (num3 <= 0)
					{
						num3 = 0;
					}
					RustHook.dijiusers.AddSetting(ownerID.ToString(), "diji", num3.ToString());
				}
				result = count;
			}
			return result;
		}

		public static int RemoveAllObjects(string name)
		{
			int num = 0;
			string text = name.Replace("*", "");
			IDMain[] array = UnityEngine.Object.FindObjectsOfType<IDMain>();
			for (int i = 0; i < array.Length; i++)
			{
				IDMain iDMain = array[i];
				string text2 = Helper.NiceName(iDMain.name);
				if (text2.Equals(name, StringComparison.CurrentCultureIgnoreCase) && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
				else if (name.Equals("ALL", StringComparison.CurrentCultureIgnoreCase) && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
				else if (name.Equals("NPC", StringComparison.CurrentCultureIgnoreCase) && iDMain.GetComponent<Character>() != null && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
				else if (name.Equals("RES", StringComparison.CurrentCultureIgnoreCase) && iDMain.GetComponent<ResourceTarget>() != null && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
				else if (name.StartsWith("*") && name.EndsWith("*") && text2.Contains(text, true) && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
				else if (name.StartsWith("*") && text2.EndsWith(text, StringComparison.CurrentCultureIgnoreCase) && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
				else if (name.EndsWith("*") && text2.StartsWith(text, StringComparison.CurrentCultureIgnoreCase) && Helper.RemoveObject(iDMain.gameObject, false))
				{
					num++;
				}
			}
			Helper.Log(string.Concat(new object[]
			{
				"Removed ",
				num,
				" object(s) with name \"",
				text,
				"\"."
			}), true);
			return num;
		}

		public static bool RemoveObject(GameObject obj, bool force = false)
		{
			DeployableObject component = obj.GetComponent<DeployableObject>();
			bool result;
			if (component != null)
			{
				NetCull.Destroy(obj);
				result = true;
			}
			else
			{
				LootableObject component2 = obj.GetComponent<LootableObject>();
				if (component2 != null)
				{
					NetCull.Destroy(obj);
					result = true;
				}
				else
				{
					StructureComponent component3 = obj.GetComponent<StructureComponent>();
					if (component3 != null)
					{
						component3._master.RemoveComponent(component3);
						NetCull.Destroy(obj);
						result = true;
					}
					else
					{
						BasicWildLifeAI component4 = obj.GetComponent<BasicWildLifeAI>();
						if (component4 != null)
						{
							int num = WildlifeManager.Data.lifeInstances.IndexOf(component4);
							if (num != -1 && num < WildlifeManager.Data.lifeInstanceCount)
							{
								WildlifeManager.Data.lifeInstances.RemoveAt(num);
								WildlifeManager.Data.lifeInstanceCount--;
								WildlifeManager.Data.thinkIterator = 0;
								NetCull.Destroy(obj);
								result = true;
							}
							else
							{
								result = false;
							}
						}
						else
						{
							ResourceTarget component5 = obj.GetComponent<ResourceTarget>();
							if (component5 != null)
							{
								NetCull.Destroy(obj);
								result = true;
							}
							else if (force)
							{
								NetCull.Destroy(obj);
								result = true;
							}
							else
							{
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		public static GameObject GetLookObject(NetUser player, int layerMask = -1)
		{
			GameObject result;
			if (player == null)
			{
				result = null;
			}
			else
			{
				result = Helper.GetLookObject(player.playerClient, -1);
			}
			return result;
		}

		public static GameObject GetLookObject(PlayerClient player, int layerMask = -1)
		{
			GameObject result;
			if (player == null)
			{
				result = null;
			}
			else
			{
				result = Helper.GetLookObject(player.controllable, -1);
			}
			return result;
		}

		public static GameObject GetLookObject(Controllable controllable, int layerMask = -1)
		{
			GameObject result;
			if (controllable == null)
			{
				result = null;
			}
			else
			{
				result = Helper.GetLookObject(controllable.character, -1);
			}
			return result;
		}

		public static GameObject GetLookObject(Character character, int layerMask = -1)
		{
			GameObject result;
			if (character == null)
			{
				result = null;
			}
			else
			{
				Vector3 position = character.transform.position;
				Vector3 direction = character.eyesRay.direction;
				position.y += (character.stateFlags.crouch ? 1f : 1.85f);
				result = Helper.GetLookObject(new Ray(position, direction), 300f, -1);
			}
			return result;
		}

		public static GameObject GetLookObject(Ray ray, float distance = 300f, int layerMask = -1)
		{
			Vector3 zero = Vector3.zero;
			return Helper.GetLookObject(ray, out zero, distance, layerMask);
		}

		public static GameObject GetLookObject(Ray ray, out Vector3 point, float distance = 300f, int layerMask = -1)
		{
			point = Vector3.zero;
			RaycastHit raycastHit;
			bool flag;
			MeshBatchInstance meshBatchInstance;
			GameObject result;
            if (!MeshBatchPhysics.Raycast(ray, out raycastHit, distance, layerMask, out flag, out meshBatchInstance))
			{
				result = null;
			}
			else
			{
				IDMain iDMain = flag ? meshBatchInstance.idMain : IDBase.GetMain(raycastHit.collider);
				point = raycastHit.point;
				if (!(iDMain != null))
				{
					result = raycastHit.collider.gameObject;
				}
				else
				{
					result = iDMain.gameObject;
				}
			}
			return result;
		}

		public static GameObject GetLineObject(Vector3 start, Vector3 end, out Vector3 point, int layerMask = -1)
		{
			point = Vector3.zero;
			RaycastHit raycastHit;
			bool flag;
			MeshBatchInstance meshBatchInstance;
			GameObject result;
            if (!MeshBatchPhysics.Linecast(start, end, out raycastHit, layerMask, out flag, out meshBatchInstance))
			{
				result = null;
			}
			else
			{
				IDMain iDMain = flag ? meshBatchInstance.idMain : IDBase.GetMain(raycastHit.collider);
				point = raycastHit.point;
				if (!(iDMain != null))
				{
					result = raycastHit.collider.gameObject;
				}
				else
				{
					result = iDMain.gameObject;
				}
			}
			return result;
		}

		public static GameObject[] GetLineObjects(Ray ray, float distance = 300f, int layerMask = -1)
		{
			List<GameObject> list = new List<GameObject>();
			RaycastHit[] array = Physics.RaycastAll(ray, distance, layerMask);
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				list.Add(IDBase.Get(raycastHit.collider).idMain.gameObject);
			}
			return list.ToArray();
		}

		public static GameObject[] GetLineObjects(Vector3 start, Vector3 end, int layerMask = -1)
		{
			Ray ray = new Ray(start, (end - start).normalized);
			return Helper.GetLineObjects(ray, Vector3.Distance(start, end), layerMask);
		}

		[CompilerGenerated]
		private static bool smethod_0(Assembly assembly_0)
		{
			return assembly_0.GetName().Name == "Assembly-CSharp";
		}

		[CompilerGenerated]
		private static bool smethod_1(AssemblyName assemblyName_0)
		{
			return assemblyName_0.Name == "RustExtended";
		}

		[CompilerGenerated]
		private static bool smethod_2(Assembly assembly_0)
		{
			return assembly_0.GetName().Name == "uLink";
		}

		[CompilerGenerated]
		private static bool smethod_3(AssemblyName assemblyName_0)
		{
			return assemblyName_0.Name == "RustExtended";
		}
	}
}
