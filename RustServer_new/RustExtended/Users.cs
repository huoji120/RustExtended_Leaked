using RustProto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace RustExtended
{
	public class Users
	{
		[CompilerGenerated]
		private sealed class Class50
		{
			public string[] string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0[0];
			}
		}

		[CompilerGenerated]
		private sealed class Class51
		{
			public MySQL.Row row_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.row_0.Get("command").AsString;
			}
		}

		[CompilerGenerated]
		private sealed class Class52
		{
			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class53
		{
			public Countdown countdown_0;

			public bool method_0(Countdown countdown_1)
			{
				return countdown_1.Command == this.countdown_0.Command;
			}
		}

		[CompilerGenerated]
		private sealed class Class54
		{
			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class55
		{
			public Countdown countdown_0;

			public bool method_0(Countdown countdown_1)
			{
				return countdown_1.Command == this.countdown_0.Command;
			}
		}

		public static string SQL_INSERT_USER_DATA = "REPLACE INTO `db_users` (`steam_id`, `username`, `password`, `comments`, `rank`, `flags`, `language`, `x`, `y`, `z`, `violations`, `violation_date`, `last_connect_ip`, `last_connect_date`, `first_connect_ip`, `first_connect_date`, `premium_date`) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, '{11}', '{12}', '{13}', '{14}', '{15}', '{16}');";

		public static string SQL_UPDATE_USER_DATA = "UPDATE `db_users` SET `username`={1}, `password`={2}, `comments`={3}, `rank`={4}, `flags`={5}, `language`={6}, `x`={7}, `y`={8}, `z`={9}, `violations`={10}, `violation_date`='{11}', `last_connect_ip`='{12}', `last_connect_date`='{13}', `first_connect_ip`='{14}', `first_connect_date`='{15}', `premium_date`='{16}' WHERE `steam_id`={0} LIMIT 1;";

		public static string SQL_UPDATE_USER_ELEM = "UPDATE `db_users` SET `{1}`={2} WHERE `steam_id`={0} LIMIT 1;";

		public static string SQL_DELETE_USER_DATA = "DELETE FROM `db_users` WHERE `steam_id`={0};";

		public static string SQL_INSERT_USER_SHARED = "REPLACE INTO `db_users_shared` (`owner_id`, `user_id`) VALUES ({0}, {1});";

		public static string SQL_DELETE_USER_SHARED = "DELETE FROM `db_users_shared` WHERE `owner_id`={0} AND `user_id`={1};";

		public static string SQL_CLEAR_USER_SHAREDS = "DELETE FROM `db_users_shared` WHERE `owner_id`={0};";

		public static string SQL_UPDATE_USER_PERSONAL = "UPDATE `db_users_personal` SET `quantity`={2} WHERE `user_id`={0} AND `item_name`='{1}';";

		public static string SQL_INSERT_USER_PERSONAL = "REPLACE INTO `db_users_personal` (`user_id`, `item_name`, `quantity`) VALUES ({0}, '{1}', {2});";

		public static string SQL_DELETE_USER_PERSONAL = "DELETE FROM `db_users_personal` WHERE `user_id`={0} AND `item_name`='{1}';";

		public static string SQL_CLEAR_USER_PERSONALS = "DELETE FROM `db_users_personal` WHERE `user_id`={0};";

		public static string SQL_INSERT_USER_COUNTDOWN = "REPLACE INTO `db_users_countdown` (`user_id`, `command`, `expires`) VALUES ({0}, '{1}', {2});";

		public static string SQL_DELETE_USER_COUNTDOWN = "DELETE FROM `db_users_countdown` WHERE `user_id`={0} AND `command`='{1}';";

		public static string SQL_CLEAR_USER_COUNTDOWNS = "DELETE FROM `db_users_countdown` WHERE `user_id`={0};";

		public static string SQL_INSERT_USER_BANNED = "REPLACE INTO `db_users_banned` (`steam_id`, `ip_address`, `date`, `period`, `reason`, `details`) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5});";

		public static string SQL_DELETE_USER_BANNED = "DELETE FROM `db_users_banned` WHERE `steam_id`={0};";

		public static string SQL_CLEAR_USER_BANLIST = "TRUNCATE `db_users_banned`;";

		public static bool VerifyNames = true;

		public static string VerifyChars = "0-9a-zA-Z. _-";

		public static bool UniqueNames = true;

		public static bool BindingNames = true;

		public static int DefaultRank = 0;

		public static int PremiumRank = 1;

		public static bool MD5Password = true;

		public static bool DisplayRank = true;

		public static int AutoAdminRank = 3;

		public static int PingLimit = 0;

		public static float NetworkTimeout = 0f;

		private static string string_0 = "rust_users.txt";

		public static Dictionary<ulong, RustProto.Avatar> Avatar;

		private static Dictionary<ulong, ulong> dictionary_0;

		public static Dictionary<ulong, UserData> Database;

		public static Dictionary<ulong, List<ulong>> Shared;

		public static Dictionary<ulong, List<Countdown>> Countdown;

		public static Dictionary<ulong, Dictionary<string, int>> Personal;

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static int int_0;

		[CompilerGenerated]
		private static Predicate<Countdown> predicate_0;

		[CompilerGenerated]
		private static Predicate<Countdown> predicate_1;

		private static Dictionary<string, int> asdasg;

		public static string SaveFilePath
		{
			get;
			private set;
		}

		public static bool Initialized
		{
			get;
			private set;
		}

		public static int Loaded
		{
			get;
			private set;
		}

		public static int Count
		{
			get
			{
				return Users.Database.Keys.Count;
			}
		}

		public static List<UserData> All
		{
			get
			{
				return Users.Database.Values.ToList<UserData>();
			}
		}

		public static void Initialize()
		{
			Users.SaveFilePath = Path.Combine(Core.SavePath, Users.string_0);
			Users.Avatar = new Dictionary<ulong, RustProto.Avatar>();
			Users.dictionary_0 = new Dictionary<ulong, ulong>();
			Users.Database = new Dictionary<ulong, UserData>();
			Users.Shared = new Dictionary<ulong, List<ulong>>();
			Users.Personal = new Dictionary<ulong, Dictionary<string, int>>();
			Users.Countdown = new Dictionary<ulong, List<Countdown>>();
			Users.Initialized = false;
			if (Core.DatabaseType.Contains("FILE", true))
			{
				Users.Initialized = Users.LoadAsTextFile();
			}
			if (Core.DatabaseType.Contains("MYSQL", true))
			{
				Users.Initialized = Users.LoadAsDatabaseSQL();
			}
		}

		public static ulong Hash(ulong steam_id)
		{
			ulong result;
			if (!Users.dictionary_0.ContainsKey(steam_id))
			{
				result = 0uL;
			}
			else
			{
				result = Users.dictionary_0[steam_id];
			}
			return result;
		}

		public static void HashUpdate(ulong steam_id)
		{
			if (Users.Database.ContainsKey(steam_id))
			{
				if (Users.dictionary_0.ContainsKey(steam_id))
				{
					Users.dictionary_0[steam_id] = Users.Database[steam_id].Hash;
				}
			}
		}

		public static bool LoadAsTextFile()
		{
			Users.Loaded = 0;
			bool result;
			if (!File.Exists(Users.SaveFilePath))
			{
				result = true;
			}
			else
			{
				string text = File.ReadAllText(Users.SaveFilePath);
				if (string.IsNullOrEmpty(text))
				{
					result = true;
				}
				else
				{
					string[] array = text.Split(new string[]
					{
						"\r\n",
						"\n"
					}, StringSplitOptions.RemoveEmptyEntries);
					string text2 = null;
					Version v = null;
					UserData userData = null;
					UserEconomy userEconomy = null;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text3 = array2[i];
						Predicate<Countdown> predicate = null;
						Users.Class50 @class = new Users.Class50();
						if (text3.StartsWith("[") && text3.EndsWith("]"))
						{
							if (!(v == null) && text2 != null)
							{
								userData = null;
								ulong num = 0uL;
								if (ulong.TryParse(text3.Substring(1, text3.Length - 2), out num))
								{
									if (Users.Database.ContainsKey(num))
									{
										userData = Users.Database[num];
									}
									else
									{
										userData = new UserData(0uL);
										userData.SteamID = num;
										Users.Database.Add(num, userData);
										Users.Loaded++;
									}
									if (Economy.Database.ContainsKey(num))
									{
										userEconomy = Economy.Database[num];
									}
									else
									{
										userEconomy = new UserEconomy(num, Economy.StartBalance);
										Economy.Database.Add(num, userEconomy);
									}
									if (!Users.Shared.ContainsKey(userData.SteamID))
									{
										Users.Shared.Add(userData.SteamID, new List<ulong>());
									}
									if (!Users.Personal.ContainsKey(userData.SteamID))
									{
										Users.Personal.Add(userData.SteamID, new Dictionary<string, int>());
									}
									if (!Users.Countdown.ContainsKey(userData.SteamID))
									{
										Users.Countdown.Add(userData.SteamID, new List<Countdown>());
									}
								}
							}
						}
						else
						{
							@class.string_0 = text3.Split(new char[]
							{
								'='
							});
							if (@class.string_0.Length >= 2)
							{
								@class.string_0[1] = @class.string_0[1].Trim();
								string key;
								if (userData == null)
								{
									if (@class.string_0[0].Equals("VERSION", StringComparison.OrdinalIgnoreCase))
									{
										v = new Version(@class.string_0[1]);
									}
									if (@class.string_0[0].Equals("TITLE", StringComparison.OrdinalIgnoreCase))
									{
										text2 = @class.string_0[1];
									}
									if (@class.string_0[0].Equals("TIME", StringComparison.OrdinalIgnoreCase))
									{
										Convert.ToUInt32(@class.string_0[1]);
									}
								}
								else if (!string.IsNullOrEmpty(@class.string_0[1]) && !(v == null) && text2 != null && (key = @class.string_0[0].ToUpper()) != null)
								{
									if (Users.asdasg == null)
									{
										Users.asdasg = new Dictionary<string, int>(22)
										{
											{
												"USERNAME",
												0
											},
											{
												"PASSWORD",
												1
											},
											{
												"COMMENTS",
												2
											},
											{
												"RANK",
												3
											},
											{
												"FLAGS",
												4
											},
											{
												"LANGUAGE",
												5
											},
											{
												"POSITION",
												6
											},
											{
												"VIOLATIONS",
												7
											},
											{
												"VIOLATIONDATE",
												8
											},
											{
												"LASTCONNECTIP",
												9
											},
											{
												"LASTCONNECTDATE",
												10
											},
											{
												"FIRSTCONNECTIP",
												11
											},
											{
												"FIRSTCONNECTDATE",
												12
											},
											{
												"PREMIUMDATE",
												13
											},
											{
												"SHARED",
												14
											},
											{
												"PERSONAL",
												15
											},
											{
												"COUNTDOWN",
												16
											},
											{
												"BALANCE",
												17
											},
											{
												"KILLEDPLAYERS",
												18
											},
											{
												"KILLEDMUTANTS",
												19
											},
											{
												"KILLEDANIMALS",
												20
											},
											{
												"DEATHS",
												21
											}
										};
									}
									int num2;
									if (Users.asdasg.TryGetValue(key, out num2))
									{
										switch (num2)
										{
										case 0:
											userData.Username = @class.string_0[1];
											break;
										case 1:
											userData.Password = @class.string_0[1];
											break;
										case 2:
											userData.Comments = @class.string_0[1];
											break;
										case 3:
											userData.Rank = Convert.ToInt32(@class.string_0[1]);
											break;
										case 4:
											userData.Flags = @class.string_0[1].ToEnum<UserFlags>();
											break;
										case 5:
											userData.Language = @class.string_0[1];
											break;
										case 6:
											@class.string_0 = @class.string_0[1].Split(new char[]
											{
												','
											});
											if (@class.string_0.Length > 0)
											{
												float.TryParse(@class.string_0[0].Trim(), out userData.Position.x);
											}
											if (@class.string_0.Length > 1)
											{
												float.TryParse(@class.string_0[1].Trim(), out userData.Position.y);
											}
											if (@class.string_0.Length > 2)
											{
												float.TryParse(@class.string_0[2].Trim(), out userData.Position.z);
											}
											break;
										case 7:
											userData.Violations = Convert.ToInt32(@class.string_0[1]);
											break;
										case 8:
											userData.ViolationDate = DateTime.Parse(@class.string_0[1]);
											break;
										case 9:
											userData.LastConnectIP = @class.string_0[1];
											break;
										case 10:
											userData.LastConnectDate = DateTime.Parse(@class.string_0[1]);
											break;
										case 11:
											userData.FirstConnectIP = @class.string_0[1];
											break;
										case 12:
											userData.FirstConnectDate = DateTime.Parse(@class.string_0[1]);
											break;
										case 13:
											userData.PremiumDate = DateTime.Parse(@class.string_0[1]);
											break;
										case 14:
										{
											ulong item = Convert.ToUInt64(@class.string_0[1]);
											if (!Users.Shared[userData.SteamID].Contains(item))
											{
												Users.Shared[userData.SteamID].Add(item);
											}
											break;
										}
										case 15:
											if (@class.string_0[1].Contains(","))
											{
												@class.string_0 = @class.string_0[1].Replace(", ", ",").Split(new char[]
												{
													','
												});
												if (!Users.Personal[userData.SteamID].ContainsKey(@class.string_0[0]))
												{
													Users.Personal[userData.SteamID].Add(@class.string_0[0], Convert.ToInt32(@class.string_0[1]));
												}
											}
											break;
										case 16:
											if (@class.string_0[1].Contains(","))
											{
												@class.string_0 = @class.string_0[1].Replace(", ", ",").Split(new char[]
												{
													','
												});
												List<Countdown> list = Users.Countdown[userData.SteamID];
												if (predicate == null)
												{
													predicate = new Predicate<Countdown>(@class.method_0);
												}
												if (!list.Exists(predicate))
												{
													DateTime stamp;
													int num3;
													if (DateTime.TryParse(@class.string_0[1], out stamp))
													{
														Users.Countdown[userData.SteamID].Add(new Countdown(@class.string_0[0], stamp));
													}
													else if (int.TryParse(@class.string_0[1], out num3))
													{
														Users.Countdown[userData.SteamID].Add(new Countdown(@class.string_0[0], (double)num3));
													}
													Countdown countdown = Users.Countdown[userData.SteamID].Last<Countdown>();
													if (countdown.Expired)
													{
														Users.CountdownRemove(userData.SteamID, countdown);
													}
												}
											}
											break;
										case 17:
											userEconomy.Balance = Convert.ToUInt64(@class.string_0[1]);
											break;
										case 18:
											userEconomy.PlayersKilled = Convert.ToInt32(@class.string_0[1]);
											break;
										case 19:
											userEconomy.MutantsKilled = Convert.ToInt32(@class.string_0[1]);
											break;
										case 20:
											userEconomy.AnimalsKilled = Convert.ToInt32(@class.string_0[1]);
											break;
										case 21:
											userEconomy.Deaths = Convert.ToInt32(@class.string_0[1]);
											break;
										}
									}
								}
							}
						}
					}
					List<ulong> list2 = new List<ulong>();
					foreach (ulong current in Users.Database.Keys)
					{
						if (string.IsNullOrEmpty(Users.Database[current].Username))
						{
							list2.Add(current);
						}
					}
					foreach (ulong current2 in list2)
					{
						Users.Database.Remove(current2);
						Economy.Database.Remove(current2);
						Users.Shared.Remove(current2);
						Users.Personal.Remove(current2);
						Users.Countdown.Remove(current2);
					}
					Users.Loaded -= list2.Count;
					foreach (ulong current3 in Users.Database.Keys)
					{
						if (Users.Database[current3].HasFlag(UserFlags.online) && NetUser.FindByUserID(current3) == null)
						{
							Users.Database[current3].SetFlag(UserFlags.online, false);
						}
						if (Users.Database[current3].HasFlag(UserFlags.nopvp))
						{
							List<Countdown> list3 = Users.Countdown[current3];
							if (Users.predicate_0 == null)
							{
								Users.predicate_0 = new Predicate<Countdown>(Users.smethod_0);
							}
							if (!list3.Exists(Users.predicate_0))
							{
								Users.Database[current3].SetFlag(UserFlags.nopvp, false);
							}
						}
					}
					Users.dictionary_0.Clear();
					foreach (ulong current4 in Users.Database.Keys)
					{
						Users.dictionary_0.Add(current4, Users.Database[current4].Hash);
					}
					Economy.Hashdata.Clear();
					foreach (ulong current5 in Economy.Database.Keys)
					{
						Economy.Hashdata.Add(current5, Economy.Database[current5].Hash);
					}
					result = true;
				}
			}
			return result;
		}

		public static int SaveAsTextFile()
		{
			using (StreamWriter streamWriter = File.CreateText(Users.SaveFilePath + ".new"))
			{
				streamWriter.WriteLine("TITLE=" + Core.ProductName);
				streamWriter.WriteLine("VERSION=" + Core.Version);
				streamWriter.WriteLine("TIME=" + (uint)Environment.TickCount);
				streamWriter.WriteLine();
				foreach (UserData current in Users.Database.Values)
				{
					streamWriter.WriteLine("[" + current.SteamID + "]");
					streamWriter.WriteLine("USERNAME=" + current.Username);
					streamWriter.WriteLine("PASSWORD=" + current.Password);
					streamWriter.WriteLine("COMMENTS=" + current.Comments);
					streamWriter.WriteLine("RANK=" + current.Rank);
					streamWriter.WriteLine("FLAGS=" + (current.Flags & ~UserFlags.online).ToString().Replace(", ", ","));
					streamWriter.WriteLine("LANGUAGE=" + current.Language);
					foreach (ulong current2 in Users.SharedList(current.SteamID))
					{
						streamWriter.WriteLine("SHARED=" + current2);
					}
					foreach (string current3 in Users.PersonalList(current.SteamID).Keys)
					{
						streamWriter.WriteLine(string.Concat(new object[]
						{
							"PERSONAL=",
							current3,
							",",
							Users.PersonalList(current.SteamID)[current3]
						}));
					}
					foreach (Countdown current4 in Users.CountdownList(current.SteamID))
					{
						if (!current4.Expired)
						{
							streamWriter.WriteLine("COUNTDOWN=" + current4.Command + "," + (current4.Expires ? current4.Stamp.ToString("MM/dd/yyyy HH:mm:ss") : "0"));
						}
					}
					streamWriter.WriteLine(string.Concat(new object[]
					{
						"POSITION=",
						current.Position.x,
						",",
						current.Position.y,
						",",
						current.Position.z
					}));
					streamWriter.WriteLine("VIOLATIONS=" + current.Violations);
					streamWriter.WriteLine("VIOLATIONDATE=" + current.ViolationDate.ToString("MM/dd/yyyy HH:mm:ss"));
					streamWriter.WriteLine("LASTCONNECTIP=" + current.LastConnectIP);
					streamWriter.WriteLine("LASTCONNECTDATE=" + current.LastConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
					streamWriter.WriteLine("FIRSTCONNECTIP=" + current.FirstConnectIP);
					streamWriter.WriteLine("FIRSTCONNECTDATE=" + current.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
					streamWriter.WriteLine("PREMIUMDATE=" + current.PremiumDate.ToString("MM/dd/yyyy HH:mm:ss"));
					if (Economy.Database.ContainsKey(current.SteamID))
					{
						streamWriter.WriteLine("BALANCE=" + Economy.Database[current.SteamID].Balance);
						streamWriter.WriteLine("KILLEDPLAYERS=" + Economy.Database[current.SteamID].PlayersKilled);
						streamWriter.WriteLine("KILLEDMUTANTS=" + Economy.Database[current.SteamID].MutantsKilled);
						streamWriter.WriteLine("KILLEDANIMALS=" + Economy.Database[current.SteamID].AnimalsKilled);
						streamWriter.WriteLine("DEATHS=" + Economy.Database[current.SteamID].Deaths);
					}
					streamWriter.WriteLine();
				}
			}
			Helper.CreateFileBackup(Users.SaveFilePath);
			File.Move(Users.SaveFilePath + ".new", Users.SaveFilePath);
			return Users.Database.Values.Count;
		}

		public static bool LoadAsDatabaseSQL()
		{
			Users.Loaded = 0;
			MySQL.Result result = MySQL.Query("SELECT * FROM `db_users`;", false);
			if (result != null)
			{
				foreach (MySQL.Row current in result.Row)
				{
					ulong asUInt = current.Get("steam_id").AsUInt64;
					if (!Users.Database.ContainsKey(asUInt) && asUInt != 0uL)
					{
						UserData userData = new UserData(0uL);
						userData.SteamID = asUInt;
						userData.Username = current.Get("username").AsString;
						userData.Password = current.Get("password").AsString;
						userData.Comments = current.Get("comments").AsString;
						userData.Rank = current.Get("rank").AsInt;
						userData.Flags = current.Get("flags").AsEnum<UserFlags>();
						userData.Language = current.Get("language").AsString;
						userData.Position.x = current.Get("x").AsFloat;
						userData.Position.y = current.Get("y").AsFloat;
						userData.Position.z = current.Get("z").AsFloat;
						userData.Violations = current.Get("violations").AsInt;
						userData.ViolationDate = current.Get("violation_date").AsDateTime;
						userData.LastConnectIP = current.Get("last_connect_ip").AsString;
						userData.LastConnectDate = current.Get("last_connect_date").AsDateTime;
						userData.FirstConnectIP = current.Get("first_connect_ip").AsString;
						userData.FirstConnectDate = current.Get("first_connect_date").AsDateTime;
						userData.PremiumDate = current.Get("premium_date").AsDateTime;
						if (!Users.Shared.ContainsKey(asUInt))
						{
							Users.Shared.Add(asUInt, new List<ulong>());
						}
						if (!Users.Personal.ContainsKey(asUInt))
						{
							Users.Personal.Add(asUInt, new Dictionary<string, int>());
						}
						if (!Users.Countdown.ContainsKey(asUInt))
						{
							Users.Countdown.Add(asUInt, new List<Countdown>());
						}
						bool flag = false;
						if (userData.HasFlag(UserFlags.online) && NetUser.FindByUserID(asUInt) == null)
						{
							flag = true;
							userData.SetFlag(UserFlags.online, false);
						}
						if (userData.HasFlag(UserFlags.nopvp))
						{
							List<Countdown> list = Users.Countdown[asUInt];
							if (Users.predicate_1 == null)
							{
								Users.predicate_1 = new Predicate<Countdown>(Users.smethod_1);
							}
							if (!list.Exists(Users.predicate_1))
							{
								flag = true;
								userData.SetFlag(UserFlags.nopvp, false);
							}
						}
						if (flag)
						{
							Users.SQL_Update(asUInt, "flags", userData.Flags.ToString().Replace(" ", ""));
						}
						Users.Database.Add(asUInt, userData);
						Economy.Database.Add(asUInt, new UserEconomy(asUInt, Economy.StartBalance));
						Users.Loaded++;
					}
				}
			}
			List<ulong> list2 = new List<ulong>();
			foreach (ulong current2 in Users.Database.Keys)
			{
				if (string.IsNullOrEmpty(Users.Database[current2].Username))
				{
					list2.Add(current2);
				}
			}
			foreach (ulong current3 in list2)
			{
				Users.Database.Remove(current3);
				Economy.Database.Remove(current3);
				Users.Shared.Remove(current3);
				Users.Personal.Remove(current3);
				Users.Countdown.Remove(current3);
				MySQL.Update(string.Format(Users.SQL_DELETE_USER_DATA, current3));
			}
			Users.Loaded -= list2.Count;
			Users.dictionary_0.Clear();
			foreach (ulong current4 in Users.Database.Keys)
			{
				Users.dictionary_0.Add(current4, Users.Database[current4].Hash);
			}
			MySQL.Result result2 = MySQL.Query("SELECT * FROM `db_users_economy`;", false);
			if (result2 != null)
			{
				foreach (MySQL.Row current5 in result2.Row)
				{
					ulong asUInt2 = current5.Get("user_id").AsUInt64;
					if (Users.Database.ContainsKey(asUInt2))
					{
						Economy.Database[asUInt2].Balance = current5.Get("balance").AsUInt64;
						Economy.Database[asUInt2].PlayersKilled = current5.Get("killed_players").AsInt;
						Economy.Database[asUInt2].MutantsKilled = current5.Get("killed_mutants").AsInt;
						Economy.Database[asUInt2].AnimalsKilled = current5.Get("killed_animals").AsInt;
						Economy.Database[asUInt2].Deaths = current5.Get("deaths").AsInt;
					}
				}
			}
			Economy.Hashdata.Clear();
			foreach (ulong current6 in Economy.Database.Keys)
			{
				Economy.Hashdata.Add(current6, Economy.Database[current6].Hash);
			}
			MySQL.Result result3 = MySQL.Query("SELECT * FROM `db_users_shared`;", false);
			if (result3 != null)
			{
				foreach (MySQL.Row current7 in result3.Row)
				{
					ulong asUInt3 = current7.Get("owner_id").AsUInt64;
					if (Users.Database.ContainsKey(asUInt3) && !Users.Shared[asUInt3].Contains(current7.Get("user_id").AsUInt64))
					{
						Users.Shared[asUInt3].Add(current7.Get("user_id").AsUInt64);
					}
				}
			}
			MySQL.Result result4 = MySQL.Query("SELECT * FROM `db_users_personal`;", false);
			if (result4 != null)
			{
				foreach (MySQL.Row current8 in result4.Row)
				{
					ulong asUInt4 = current8.Get("user_id").AsUInt64;
					if (Users.Database.ContainsKey(asUInt4) && !Users.Personal[asUInt4].ContainsKey(current8.Get("item_name").AsString))
					{
						Users.Personal[asUInt4].Add(current8.Get("item_name").AsString, current8.Get("quantity").AsInt);
					}
				}
			}
			MySQL.Result result5 = MySQL.Query("SELECT * FROM `db_users_countdown`;", false);
			if (result5 != null)
			{
				using (List<MySQL.Row>.Enumerator enumerator5 = result5.Row.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Predicate<Countdown> predicate = null;
						Users.Class51 @class = new Users.Class51();
						@class.row_0 = enumerator5.Current;
						ulong asUInt5 = @class.row_0.Get("user_id").AsUInt64;
						if (Users.Database.ContainsKey(asUInt5))
						{
							List<Countdown> list3 = Users.Countdown[asUInt5];
							if (predicate == null)
							{
								predicate = new Predicate<Countdown>(@class.method_0);
							}
							if (!list3.Exists(predicate))
							{
								if (@class.row_0.Get("expires").IsNull)
								{
									Users.Countdown[asUInt5].Add(new Countdown(@class.row_0.Get("command").AsString, 0.0));
								}
								else
								{
									Users.Countdown[asUInt5].Add(new Countdown(@class.row_0.Get("command").AsString, @class.row_0.Get("expires").AsDateTime));
								}
								Countdown countdown = Users.Countdown[asUInt5].Last<Countdown>();
								if (countdown.Expired)
								{
									Users.CountdownRemove(asUInt5, countdown);
								}
							}
						}
					}
				}
			}
			return true;
		}

		public static bool SaveAsDatabaseSQL()
		{
			bool result;
			if (!Core.DatabaseType.Equals("MYSQL"))
			{
				result = false;
			}
			else
			{
				using (Dictionary<ulong, UserData>.ValueCollection.Enumerator enumerator = Users.Database.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UserData current = enumerator.Current;
						MySQL.Update(string.Format(Users.SQL_INSERT_USER_DATA, new object[]
						{
							current.SteamID,
							MySQL.QuoteString(current.Username),
							MySQL.QuoteString(current.Password),
							MySQL.QuoteString(current.Comments),
							current.Rank,
							MySQL.QuoteString(current.Flags.ToString().Replace(" ", "")),
							MySQL.QuoteString(current.Language),
							current.Position.x,
							current.Position.y,
							current.Position.z,
							current.Violations,
							current.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss"),
							current.LastConnectIP,
							current.LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
							current.FirstConnectIP,
							current.FirstConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
							current.PremiumDate.ToString("yyyy-MM-dd HH:mm:ss")
						}));
						List<ulong> list = Users.SharedList(current.SteamID);
						Dictionary<string, int> dictionary = Users.PersonalList(current.SteamID);
						List<Countdown> list2 = Users.CountdownList(current.SteamID);
						foreach (ulong current2 in list)
						{
							MySQL.Update(string.Format(Users.SQL_INSERT_USER_SHARED, current.SteamID, current2));
						}
						foreach (string current3 in dictionary.Keys)
						{
							MySQL.Update(string.Format(Users.SQL_INSERT_USER_PERSONAL, current.SteamID, current3, dictionary[current3]));
						}
						foreach (Countdown current4 in list2)
						{
							MySQL.Update(string.Format(Users.SQL_INSERT_USER_COUNTDOWN, current.SteamID, current4.Command, current4.Expires ? MySQL.QuoteString(current4.Stamp.ToString("yyyy-MM-dd HH:mm:ss")) : "NULL"));
						}
						if (Economy.Database.ContainsKey(current.SteamID))
						{
							MySQL.Update(string.Format(Economy.SQL_INSERT_ECONOMY, new object[]
							{
								current.SteamID,
								Economy.Database[current.SteamID].Balance,
								Economy.Database[current.SteamID].PlayersKilled,
								Economy.Database[current.SteamID].MutantsKilled,
								Economy.Database[current.SteamID].AnimalsKilled,
								Economy.Database[current.SteamID].Deaths
							}));
						}
					}
					goto IL_401;
				}
				IL_3F3:
				LibRust.Cycle();
				Thread.Sleep(100);
				IL_401:
				if (MySQL.Queued)
				{
					goto IL_3F3;
				}
				result = true;
			}
			return result;
		}

		public static void SQL_Update(ulong user_id)
		{
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				if (Users.Database.ContainsKey(user_id))
				{
					MySQL.Update(string.Format(Users.SQL_UPDATE_USER_DATA, new object[]
					{
						Users.Database[user_id].SteamID,
						MySQL.QuoteString(Users.Database[user_id].Username),
						MySQL.QuoteString(Users.Database[user_id].Password),
						MySQL.QuoteString(Users.Database[user_id].Comments),
						Users.Database[user_id].Rank,
						MySQL.QuoteString(Users.Database[user_id].Flags.ToString().Replace(" ", "")),
						MySQL.QuoteString(Users.Database[user_id].Language),
						Users.Database[user_id].Position.x,
						Users.Database[user_id].Position.y,
						Users.Database[user_id].Position.z,
						Users.Database[user_id].Violations,
						Users.Database[user_id].ViolationDate.ToString("yyyy-MM-dd HH:mm:ss"),
						Users.Database[user_id].LastConnectIP,
						Users.Database[user_id].LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
						Users.Database[user_id].FirstConnectIP,
						Users.Database[user_id].FirstConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
						Users.Database[user_id].PremiumDate.ToString("yyyy-MM-dd HH:mm:ss")
					}));
				}
			}
		}

		public static void SQL_Update(ulong user_id, string key, object value)
		{
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				string text = null;
				if (value is int)
				{
					text = ((int)value).ToString();
				}
				if (value is long)
				{
					text = ((long)value).ToString();
				}
				if (value is uint)
				{
					text = ((uint)value).ToString();
				}
				if (value is ulong)
				{
					text = ((ulong)value).ToString();
				}
				if (value is float)
				{
					text = ((float)value).ToString();
				}
				if (value is double)
				{
					text = ((double)value).ToString();
				}
				if (value is string)
				{
					text = MySQL.QuoteString((string)value);
				}
				if (value is bool)
				{
					text = MySQL.QuoteString(((bool)value) ? "Yes" : "No");
				}
				if (value is DateTime)
				{
					text = MySQL.QuoteString(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
				}
				if (value is UserFlags)
				{
					text = MySQL.QuoteString(((UserFlags)value).ToString().Replace(" ", ""));
				}
				if (text != null)
				{
					MySQL.Update(string.Format(Users.SQL_UPDATE_USER_ELEM, user_id, key, text));
				}
			}
		}

		public static ulong SQL_SynchronizeUsers()
		{
			ulong result = 0uL;
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Result result2 = MySQL.Query("SELECT * FROM `db_users`;", true);
				if (result2 != null)
				{
					foreach (MySQL.Row current in result2.Row)
					{
						ulong asUInt = current.Get("steam_id").AsUInt64;
						if (!Users.dictionary_0.ContainsKey(asUInt))
						{
							Users.dictionary_0.Add(asUInt, 0uL);
						}
						if (!Users.Database.ContainsKey(asUInt))
						{
							UserData userData = new UserData(0uL);
							userData.SteamID = asUInt;
							userData.Username = current.Get("username").AsString;
							userData.Password = current.Get("password").AsString;
							userData.Comments = current.Get("comments").AsString;
							userData.Rank = current.Get("rank").AsInt;
							userData.Flags = current.Get("flags").AsString.ToEnum<UserFlags>();
							userData.Language = current.Get("language").AsString;
							userData.Position.x = current.Get("x").AsFloat;
							userData.Position.y = current.Get("y").AsFloat;
							userData.Position.z = current.Get("z").AsFloat;
							userData.Violations = current.Get("violations").AsInt;
							userData.ViolationDate = current.Get("violation_date").AsDateTime;
							userData.LastConnectIP = current.Get("last_connect_ip").AsString;
							userData.LastConnectDate = current.Get("last_connect_date").AsDateTime;
							userData.FirstConnectIP = current.Get("first_connect_ip").AsString;
							userData.FirstConnectDate = current.Get("first_connect_date").AsDateTime;
							userData.PremiumDate = current.Get("premium_date").AsDateTime;
							Users.Database.Add(asUInt, userData);
						}
						else if (Users.Database[asUInt].Hash != Users.dictionary_0[asUInt])
						{
							Users.SQL_Update(asUInt);
						}
						else
						{
							Users.Database[asUInt].Username = current.Get("username").AsString;
							Users.Database[asUInt].Password = current.Get("password").AsString;
							Users.Database[asUInt].Comments = current.Get("comments").AsString;
							Users.Database[asUInt].Rank = current.Get("rank").AsInt;
							Users.Database[asUInt].Flags = current.Get("flags").AsString.ToEnum<UserFlags>();
							Users.Database[asUInt].Language = current.Get("language").AsString;
							Users.Database[asUInt].Position.x = current.Get("x").AsFloat;
							Users.Database[asUInt].Position.y = current.Get("y").AsFloat;
							Users.Database[asUInt].Position.z = current.Get("z").AsFloat;
							Users.Database[asUInt].Violations = current.Get("violations").AsInt;
							Users.Database[asUInt].ViolationDate = current.Get("violation_date").AsDateTime;
							Users.Database[asUInt].LastConnectIP = current.Get("last_connect_ip").AsString;
							Users.Database[asUInt].LastConnectDate = current.Get("last_connect_date").AsDateTime;
							Users.Database[asUInt].FirstConnectIP = current.Get("first_connect_ip").AsString;
							Users.Database[asUInt].FirstConnectDate = current.Get("first_connect_date").AsDateTime;
							Users.Database[asUInt].PremiumDate = current.Get("premium_date").AsDateTime;
						}
						Users.dictionary_0[asUInt] = Users.Database[asUInt].Hash;
					}
				}
				MySQL.Result result3 = MySQL.Query("SELECT * FROM `db_users_economy`;", true);
				if (result3 != null)
				{
					foreach (MySQL.Row current2 in result3.Row)
					{
						ulong asUInt2 = current2.Get("user_id").AsUInt64;
						if (!Economy.Hashdata.ContainsKey(asUInt2))
						{
							Economy.Hashdata.Add(asUInt2, 0uL);
						}
						if (!Economy.Database.ContainsKey(asUInt2))
						{
							UserEconomy userEconomy = new UserEconomy(asUInt2, Economy.StartBalance);
							userEconomy.Balance = current2.Get("balance").AsUInt64;
							userEconomy.PlayersKilled = current2.Get("killed_players").AsInt;
							userEconomy.MutantsKilled = current2.Get("killed_mutants").AsInt;
							userEconomy.AnimalsKilled = current2.Get("killed_animals").AsInt;
							userEconomy.Deaths = current2.Get("deaths").AsInt;
							Economy.Database.Add(asUInt2, userEconomy);
						}
						else if (Economy.Database[asUInt2].Hash != Economy.Hashdata[asUInt2])
						{
							Economy.SQL_Update(asUInt2);
						}
						else
						{
							Economy.Database[asUInt2].Balance = current2.Get("balance").AsUInt64;
							Economy.Database[asUInt2].PlayersKilled = current2.Get("killed_players").AsInt;
							Economy.Database[asUInt2].MutantsKilled = current2.Get("killed_mutants").AsInt;
							Economy.Database[asUInt2].AnimalsKilled = current2.Get("killed_animals").AsInt;
							Economy.Database[asUInt2].Deaths = current2.Get("deaths").AsInt;
						}
						Economy.Hashdata[asUInt2] = Economy.Database[asUInt2].Hash;
					}
				}
			}
			return result;
		}

		public static bool SQL_UpdatePersonal(ulong user_id)
		{
			bool result;
			if (Users.Database.ContainsKey(user_id) && Core.DatabaseType.Equals("MYSQL"))
			{
				UserData userData = Users.Database[user_id];
				foreach (string current in Users.PersonalList(userData.SteamID).Keys)
				{
					MySQL.Update(string.Format(Users.SQL_UPDATE_USER_PERSONAL, user_id, current, Users.PersonalList(userData.SteamID)[current]));
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static UserData Add(ulong steam_id, string username, string password = "", string comments = "", int rank = 0, UserFlags flag = UserFlags.guest, string language = "", string connect_ip = "", DateTime connect_date = default(DateTime))
		{
			UserData userData = new UserData(0uL);
			userData.SteamID = steam_id;
			userData.Username = username;
			userData.Password = password;
			userData.Comments = comments;
			userData.Rank = rank;
			userData.Flags = flag;
			userData.Language = ((language == "") ? Core.Languages[0] : language);
			userData.Position = Vector3.zero;
			userData.Violations = 0;
			userData.ViolationDate = default(DateTime);
			userData.LastConnectIP = connect_ip;
			userData.LastConnectDate = connect_date;
			userData.FirstConnectIP = connect_ip;
			userData.FirstConnectDate = connect_date;
			userData.PremiumDate = default(DateTime);
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format(Users.SQL_INSERT_USER_DATA, new object[]
				{
					userData.SteamID,
					MySQL.QuoteString(userData.Username),
					MySQL.QuoteString(userData.Password),
					MySQL.QuoteString(userData.Comments),
					userData.Rank,
					MySQL.QuoteString(userData.Flags.ToString().Replace(" ", "")),
					MySQL.QuoteString(userData.Language),
					userData.Position.x,
					userData.Position.y,
					userData.Position.z,
					userData.Violations,
					userData.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss"),
					userData.LastConnectIP,
					userData.LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
					userData.FirstConnectIP,
					userData.FirstConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
					userData.PremiumDate.ToString("yyyy-MM-dd HH:mm:ss")
				}));
			}
			Users.Database.Add(userData.SteamID, userData);
			return userData;
		}

		public static UserData GetBySteamID(ulong steam_id)
		{
			UserData result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Database[steam_id];
			}
			return result;
		}

		public static UserData GetByUserName(string username)
		{
			UserData result;
			if (Users.Database != null && Users.Database.Count != 0)
			{
				StringComparison comparisonType = Users.UniqueNames ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
				foreach (UserData current in Users.Database.Values)
				{
					if (current.Username.Equals(username, comparisonType))
					{
						result = current;
						return result;
					}
				}
				result = null;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static UserData Find(string Value)
		{
			UserData result;
			if (Users.Database == null || Users.Database.Count == 0)
			{
				result = null;
			}
			else
			{
				string text = Value.Replace("*", "");
				StringComparison comparisonType = Users.UniqueNames ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
				ulong key;
				UserData userData;
				if (ulong.TryParse(Value, out key) && Users.Database.TryGetValue(key, out userData))
				{
					result = userData;
				}
				else
				{
					if (Value.StartsWith("*") && Value.EndsWith("*"))
					{
						foreach (UserData current in Users.Database.Values)
						{
							if (current.Username.Contains(text, true))
							{
								UserData userData2 = current;
								result = userData2;
								return result;
							}
						}
					}
					if (Value.StartsWith("*"))
					{
						foreach (UserData current2 in Users.Database.Values)
						{
							if (current2.Username.EndsWith(text, comparisonType))
							{
								UserData userData2 = current2;
								result = userData2;
								return result;
							}
						}
					}
					if (Value.EndsWith("*"))
					{
						foreach (UserData current3 in Users.Database.Values)
						{
							if (current3.Username.StartsWith(text, comparisonType))
							{
								UserData userData2 = current3;
								result = userData2;
								return result;
							}
						}
					}
					foreach (UserData current4 in Users.Database.Values)
					{
						if (current4.Username.Equals(text, comparisonType))
						{
							UserData userData2 = current4;
							result = userData2;
							return result;
						}
					}
					result = null;
				}
			}
			return result;
		}

		public static UserData Find(ulong userid)
		{
			UserData userData;
			UserData result;
			if (userid != 0uL && Users.Database.TryGetValue(userid, out userData))
			{
				result = userData;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static bool Delete(ulong steam_id)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_DELETE_USER_BANNED, steam_id));
					MySQL.Update(string.Format(Users.SQL_CLEAR_USER_COUNTDOWNS, steam_id));
					MySQL.Update(string.Format(Users.SQL_CLEAR_USER_PERSONALS, steam_id));
					MySQL.Update(string.Format(Users.SQL_CLEAR_USER_SHAREDS, steam_id));
					MySQL.Update(string.Format(Users.SQL_DELETE_USER_DATA, steam_id));
				}
				Economy.Delete(steam_id);
				Banned.Remove(steam_id);
				Users.Countdown.Remove(steam_id);
				Users.Personal.Remove(steam_id);
				Users.Shared.Remove(steam_id);
				Users.Database.Remove(steam_id);
				Users.dictionary_0.Remove(steam_id);
				result = true;
			}
			return result;
		}

		public static bool Delete(string username)
		{
			bool result = false;
			foreach (UserData current in Users.Database.Values)
			{
				if (current.Username.Equals(username))
				{
					result = Users.Delete(current.SteamID);
				}
			}
			return result;
		}

		public static bool Exists(ulong userid)
		{
			return Users.Database.ContainsKey(userid);
		}

		public static string GetUsername(ulong steam_id)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Database[steam_id].Username;
			}
			return result;
		}

		public static bool GetUsername(ulong steam_id, ref string Username)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				if (string.IsNullOrEmpty(Username))
				{
					Username = Users.Database[steam_id].Username;
				}
				int rank = Users.Database[steam_id].Rank;
				if (!Core.Ranks.ContainsKey(rank))
				{
					result = false;
				}
				else if (string.IsNullOrEmpty(Core.Ranks[rank]))
				{
					result = false;
				}
				else
				{
					Username = "[" + Core.Ranks[rank] + "] " + Username;
					result = true;
				}
			}
			return result;
		}

		public static string GetPassword(ulong steam_id)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Database[steam_id].Password;
			}
			return result;
		}

		public static string GetComments(ulong steam_id)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Database[steam_id].Comments;
			}
			return result;
		}

		public static int GetRank(ulong steam_id)
		{
			int result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = -1;
			}
			else
			{
				result = Users.Database[steam_id].Rank;
			}
			return result;
		}

		public static UserFlags GetFlags(ulong steam_id)
		{
			UserFlags result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = UserFlags.guest;
			}
			else
			{
				result = Users.Database[steam_id].Flags;
			}
			return result;
		}

		public static string GetLanguage(ulong steam_id)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = Core.Languages[0];
			}
			else
			{
				result = Users.Database[steam_id].Language;
			}
			return result;
		}

		public static string GetLastConnectIP(ulong steam_id)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Database[steam_id].LastConnectIP;
			}
			return result;
		}

		public static DateTime GetLastConnectDate(ulong steam_id)
		{
			DateTime result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = default(DateTime);
			}
			else
			{
				result = Users.Database[steam_id].LastConnectDate;
			}
			return result;
		}

		public static string GetFirstConnectIP(ulong steam_id)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Database[steam_id].FirstConnectIP;
			}
			return result;
		}

		public static DateTime GetFirstConnectDate(ulong steam_id)
		{
			DateTime result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = default(DateTime);
			}
			else
			{
				result = Users.Database[steam_id].FirstConnectDate;
			}
			return result;
		}

		public static bool IsOnline(ulong steam_id)
		{
			return Users.Database.ContainsKey(steam_id) && Users.Database[steam_id].HasFlag(UserFlags.online);
		}

		public static bool SetUsername(ulong steam_id, string username)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].Username = username;
				result = true;
			}
			return result;
		}

		public static bool SetPassword(ulong steam_id, string password)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].Password = password;
				result = true;
			}
			return result;
		}

		public static bool SetComments(ulong steam_id, string comments)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].Comments = comments;
				result = true;
			}
			return result;
		}

		public static bool SetRank(ulong steam_id, int rank)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].Rank = rank;
				result = true;
			}
			return result;
		}

		public static bool SetFlags(ulong steam_id, UserFlags flag, bool state = true)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].SetFlag(flag, state);
				result = true;
			}
			return result;
		}

		public static bool ToggleFlag(ulong steam_id, UserFlags flag)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].ToggleFlag(flag);
				result = true;
			}
			return result;
		}

		public static bool SetViolations(ulong steam_id, int violations)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].Violations = violations;
				result = true;
			}
			return result;
		}

		public static bool SetLastConnectIP(ulong steam_id, string ipAddress)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].LastConnectIP = ipAddress;
				result = true;
			}
			return result;
		}

		public static bool SetLastConnectDate(ulong steam_id, DateTime date)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].LastConnectDate = date;
				result = true;
			}
			return result;
		}

		public static bool SetFirstConnectIP(ulong steam_id, string ipAddress)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].FirstConnectIP = ipAddress;
				result = true;
			}
			return result;
		}

		public static bool SetFirstConnectDate(ulong steam_id, DateTime date)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].FirstConnectDate = date;
				result = true;
			}
			return result;
		}

		public static bool SetPremiumDate(ulong steam_id, DateTime date)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.Database[steam_id].PremiumDate = date;
				result = true;
			}
			return result;
		}

		public static bool ChangeID(ulong steam_id, ulong new_id)
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				string text = Core.SavePath + "\\userdata\\" + new_id;
				string text2 = Core.SavePath + "\\userdata\\" + steam_id;
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
				}
				if (Directory.Exists(text2))
				{
					Directory.Move(text2, text);
				}
				Users.Database.Add(new_id, Users.Database[steam_id]);
				Users.Database[new_id].SteamID = new_id;
				Users.Database.Remove(steam_id);
				Users.dictionary_0.Add(new_id, Users.dictionary_0[steam_id]);
				Users.dictionary_0.Remove(steam_id);
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format("UPDATE `db_users` SET `steam_id`={1} WHERE `steam_id`={0};", steam_id, new_id));
					MySQL.Update(string.Format("UPDATE `db_users_shared` SET `user_id`={1} WHERE `user_id`={0};", steam_id, new_id));
					MySQL.Update(string.Format("UPDATE `db_users_shared` SET `owner_id`={1} WHERE `owner_id`={0};", steam_id, new_id));
					MySQL.Update(string.Format("UPDATE `db_users_personal` SET `user_id`={1} WHERE `user_id`={0};", steam_id, new_id));
					MySQL.Update(string.Format("UPDATE `db_users_countdown` SET `user_id`={1} WHERE `user_id`={0};", steam_id, new_id));
					MySQL.Update(string.Format("UPDATE `db_punish_logs` SET `steam_id`={1} WHERE `steam_id`={0};", steam_id, new_id));
				}
				if (Clans.Enabled)
				{
					foreach (ClanData current in Clans.Database.Values)
					{
						if (current.LeaderID == steam_id)
						{
							current.LeaderID = new_id;
						}
					}
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format("UPDATE `db_clans` SET `leader_id`={1} WHERE `leader_id`={0};", steam_id, new_id));
					}
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format("UPDATE `db_clans_members` SET `user_id`={1} WHERE `user_id`={0};", steam_id, new_id));
					}
				}
				if (Economy.Enabled && Economy.Database.ContainsKey(steam_id))
				{
					Economy.Database.Add(new_id, Economy.Database[steam_id]);
					Economy.Database[new_id].SteamID = new_id;
					Economy.Database.Remove(steam_id);
					Economy.Hashdata.Add(new_id, Economy.Hashdata[steam_id]);
					Economy.Hashdata.Remove(steam_id);
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format("UPDATE `db_users_economy` SET `user_id`={1} WHERE `user_id`={0} LIMIT 1;", steam_id, new_id));
					}
				}
				if (Banned.Database.ContainsKey(steam_id))
				{
					Banned.Database.Add(new_id, Banned.Database[steam_id]);
					Banned.Database.Remove(steam_id);
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format("UPDATE `db_users_banned` SET `steam_id`={1} WHERE `steam_id`={0} LIMIT 1;", steam_id, new_id));
					}
				}
				foreach (StructureMaster current2 in StructureMaster.AllStructures)
				{
					if (current2.ownerID == steam_id)
					{
						current2.creatorID = new_id;
						current2.ownerID = new_id;
						current2.CacheCreator();
					}
				}
				DeployableObject[] array = UnityEngine.Object.FindObjectsOfType<DeployableObject>();
				for (int i = 0; i < array.Length; i++)
				{
					DeployableObject deployableObject = array[i];
					if (deployableObject.ownerID == steam_id)
					{
						deployableObject.creatorID = new_id;
						deployableObject.ownerID = new_id;
						deployableObject.CacheCreator();
					}
				}
				result = true;
			}
			return result;
		}

		public static List<ulong> SharedList(ulong steam_id)
		{
			if (!Users.Shared.ContainsKey(steam_id))
			{
				Users.Shared.Add(steam_id, new List<ulong>());
			}
			return Users.Shared[steam_id];
		}

		public static bool SharedGet(ulong steam_id, ulong user_id)
		{
			return Users.Shared.ContainsKey(steam_id) && Users.Shared[steam_id].Contains(user_id);
		}

		public static bool SharedAdd(ulong steam_id, ulong user_id)
		{
			if (!Users.Shared.ContainsKey(steam_id))
			{
				Users.Shared.Add(steam_id, new List<ulong>());
			}
			bool result;
			if (Users.Shared[steam_id].Contains(user_id))
			{
				result = false;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_INSERT_USER_SHARED, steam_id, user_id));
				}
				Users.Shared[steam_id].Add(user_id);
				result = true;
			}
			return result;
		}

		public static bool SharedRemove(ulong steam_id, ulong user_id)
		{
			if (!Users.Shared.ContainsKey(steam_id))
			{
				Users.Shared.Add(steam_id, new List<ulong>());
			}
			bool result;
			if (!Users.Shared[steam_id].Contains(user_id))
			{
				result = false;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_DELETE_USER_SHARED, steam_id, user_id));
				}
				Users.Shared[steam_id].Remove(user_id);
				result = true;
			}
			return result;
		}

		public static void SharedClear(ulong steam_id)
		{
			if (!Users.Shared.ContainsKey(steam_id))
			{
				Users.Shared.Add(steam_id, new List<ulong>());
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format(Users.SQL_CLEAR_USER_SHAREDS, steam_id));
			}
			Users.Shared[steam_id].Clear();
		}

		public static Dictionary<string, int> PersonalList(ulong steam_id)
		{
			if (!Users.Personal.ContainsKey(steam_id))
			{
				Users.Personal.Add(steam_id, new Dictionary<string, int>());
			}
			return Users.Personal[steam_id];
		}

		public static int PersonalGet(ulong steam_id, string item_name)
		{
			int result;
			if (!Users.Personal.ContainsKey(steam_id))
			{
				result = 0;
			}
			else if (!Users.Personal[steam_id].ContainsKey(item_name))
			{
				result = 0;
			}
			else
			{
				result = Users.Personal[steam_id][item_name];
			}
			return result;
		}

		public static bool PersonalAdd(ulong steam_id, string item_name, int quantity)
		{
			if (!Users.Personal.ContainsKey(steam_id))
			{
				Users.Personal.Add(steam_id, new Dictionary<string, int>());
			}
			bool result;
			if (Users.Personal[steam_id].ContainsKey(item_name))
			{
				Dictionary<string, int> dictionary;
				(dictionary = Users.Personal[steam_id])[item_name] = dictionary[item_name] + quantity;
				result = true;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_INSERT_USER_PERSONAL, steam_id, item_name, quantity));
				}
				Users.Personal[steam_id].Add(item_name, quantity);
				result = true;
			}
			return result;
		}

		public static bool PersonalRemove(ulong steam_id, string item_name)
		{
			if (!Users.Personal.ContainsKey(steam_id))
			{
				Users.Personal.Add(steam_id, new Dictionary<string, int>());
			}
			bool result;
			if (!Users.Personal[steam_id].ContainsKey(item_name))
			{
				result = false;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_DELETE_USER_PERSONAL, steam_id, item_name));
				}
				Users.Personal[steam_id].Remove(item_name);
				result = true;
			}
			return result;
		}

		public static void PersonalClear(ulong steam_id)
		{
			if (!Users.Personal.ContainsKey(steam_id))
			{
				Users.Personal.Add(steam_id, new Dictionary<string, int>());
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format(Users.SQL_CLEAR_USER_PERSONALS, steam_id));
			}
			Users.Personal[steam_id].Clear();
		}

		public static List<Countdown> CountdownList(ulong steam_id)
		{
			if (!Users.Countdown.ContainsKey(steam_id))
			{
				Users.Countdown.Add(steam_id, new List<Countdown>());
			}
			return Users.Countdown[steam_id];
		}

		public static Countdown CountdownGet(ulong steam_id, string command)
		{
			Users.Class52 @class = new Users.Class52();
			@class.string_0 = command;
			Countdown result;
			if (!Users.Countdown.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.Countdown[steam_id].Find(new Predicate<Countdown>(@class.method_0));
			}
			return result;
		}

		public static bool CountdownAdd(ulong steam_id, Countdown countdown)
		{
			Users.Class53 @class = new Users.Class53();
			@class.countdown_0 = countdown;
			if (!Users.Countdown.ContainsKey(steam_id))
			{
				Users.Countdown.Add(steam_id, new List<Countdown>());
			}
			bool result;
			if (Users.Countdown[steam_id].Exists(new Predicate<Countdown>(@class.method_0)))
			{
				result = false;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_INSERT_USER_COUNTDOWN, steam_id, @class.countdown_0.Command, @class.countdown_0.Expires ? MySQL.QuoteString(@class.countdown_0.Stamp.ToString("yyyy-MM-dd HH:mm:ss")) : "NULL"));
				}
				Users.Countdown[steam_id].Add(@class.countdown_0);
				result = true;
			}
			return result;
		}

		public static bool CountdownRemove(ulong steam_id, string command)
		{
			Users.Class54 @class = new Users.Class54();
			@class.string_0 = command;
			if (!Users.Countdown.ContainsKey(steam_id))
			{
				Users.Countdown.Add(steam_id, new List<Countdown>());
			}
			return Users.CountdownRemove(steam_id, Users.Countdown[steam_id].Find(new Predicate<Countdown>(@class.method_0)));
		}

		public static bool CountdownRemove(ulong steam_id, Countdown countdown)
		{
			Users.Class55 @class = new Users.Class55();
			@class.countdown_0 = countdown;
			if (!Users.Countdown.ContainsKey(steam_id))
			{
				Users.Countdown.Add(steam_id, new List<Countdown>());
			}
			bool result;
			if (@class.countdown_0 != null && Users.Countdown[steam_id].Exists(new Predicate<Countdown>(@class.method_0)))
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Users.SQL_DELETE_USER_COUNTDOWN, steam_id, @class.countdown_0.Command));
				}
				result = Users.Countdown[steam_id].Remove(@class.countdown_0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void CountdownsClear(ulong steam_id)
		{
			if (!Users.Countdown.ContainsKey(steam_id))
			{
				Users.Countdown.Add(steam_id, new List<Countdown>());
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format(Users.SQL_CLEAR_USER_COUNTDOWNS, steam_id));
			}
			Users.Countdown[steam_id].Clear();
		}

		public static bool HasRank(ulong steam_id, int rank)
		{
			return Users.Database.ContainsKey(steam_id) && Users.Database[steam_id].Rank == rank;
		}

		public static bool HasFlag(ulong steam_id, UserFlags flag)
		{
			return Users.Database.ContainsKey(steam_id) && Users.Database[steam_id].HasFlag(flag);
		}

		public static bool Details(ulong steam_id)
		{
			return Users.Database.ContainsKey(steam_id) && Users.Database[steam_id].HasFlag(UserFlags.details);
		}

		public static string NiceName(ulong steam_id, NamePrefix prefixes = NamePrefix.None)
		{
			string result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Users.NiceName(Users.Database[steam_id], prefixes);
			}
			return result;
		}

		public static string NiceName(UserData userdata, NamePrefix prefixes = NamePrefix.None)
		{
			string username = userdata.Username;
			Users.NiceName(userdata, ref username, prefixes);
			return username;
		}

		public static bool NiceName(ulong steam_id, ref string username, NamePrefix prefixes = NamePrefix.None)
		{
			return Users.Database.ContainsKey(steam_id) && Users.NiceName(Users.Database[steam_id], ref username, prefixes);
		}

		public static bool NiceName(UserData userdata, ref string username, NamePrefix prefixes = NamePrefix.None)
		{
			if (prefixes.Has(NamePrefix.Rank) && Core.Ranks.ContainsKey(userdata.Rank) && !string.IsNullOrEmpty(Core.Ranks[userdata.Rank]))
			{
				username = "[" + Core.Ranks[userdata.Rank] + "] " + username;
			}
			if (prefixes.Has(NamePrefix.Clan) && userdata.Clan != null && !userdata.Clan.Abbr.IsEmpty())
			{
				username = "<" + userdata.Clan.Abbr + "> " + username;
			}
			return true;
		}

		public static bool IsBanned(ulong steam_id)
		{
			return Users.Database.ContainsKey(steam_id) && (Banned.Database.ContainsKey(steam_id) || Users.HasFlag(steam_id, UserFlags.banned));
		}

		public static bool Ban(ulong steam_id, string reason = "", DateTime period = default(DateTime), string details = "")
		{
			return Banned.Add(steam_id, reason, period, details);
		}

		public static bool Unban(ulong steam_id)
		{
			return Banned.Remove(steam_id);
		}

		[CompilerGenerated]
		private static bool smethod_0(Countdown countdown_0)
		{
			return countdown_0.Command == "pvp";
		}

		[CompilerGenerated]
		private static bool smethod_1(Countdown countdown_0)
		{
			return countdown_0.Command == "pvp";
		}
	}
}
