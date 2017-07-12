using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RustExtended
{
	public class Clans
	{
		[CompilerGenerated]
		private sealed class Class0
		{
			public string[] string_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return (long)clanLevel_0.Id == (long)((ulong)uint.Parse(this.string_0[1]));
			}
		}

		[CompilerGenerated]
		private sealed class Class1
		{
			public MySQL.Row row_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return (long)clanLevel_0.Id == (long)((ulong)this.row_0.Get("level").AsUInt);
			}
		}

		[CompilerGenerated]
		private sealed class Class2
		{
			public Clans.Class1 class1_0;

			public uint uint_0;

			public bool method_0(MySQL.Row row_0)
			{
				return row_0.Get("id").AsUInt == this.uint_0;
			}
		}

		[CompilerGenerated]
		private sealed class Class3
		{
			public MySQL.Row row_0;

			public bool method_0(ClanLevel clanLevel_0)
			{
				return (long)clanLevel_0.Id == (long)((ulong)this.row_0.Get("level").AsUInt);
			}
		}

		public static string SQL_INSERT_CLAN = "REPLACE INTO `db_clans` (`id`,`created`,`name`,`abbrev`,`leader_id`,`flags`,`balance`,`tax`,`level`,`experience`,`location`,`motd`,`penalty`) VALUES ({0},'{1}','{2}','{3}',{4},'{5}',{6},{7},{8},{9},'{10}','{11}','{12}');";

		public static string SQL_DELETE_CLAN = "DELETE FROM `db_clans` WHERE `id`={0} LIMIT 1;";

		public static string SQL_INSERT_CLAN_HOSTILE = "REPLACE INTO `db_clans_hostile` (`clan_id`,`hostile_id`,`ending`) VALUES ({0},{1},'{2}');";

		public static string SQL_DELETE_CLAN_HOSTILE = "DELETE FROM `db_clans_hostile` WHERE `clan_id`={0} LIMIT 1;";

		public static string SQL_INSERT_MEMBER = "REPLACE INTO `db_clans_members` (`user_id`, `clan_id`, `privileges`) VALUES ({0}, {1}, '{2}');";

		public static string SQL_DELETE_MEMBER = "DELETE FROM `db_clans_members` WHERE `user_id`={0} LIMIT 1;";

		private static string string_0 = "rust_clans.txt";

		public static bool Enabled = true;

		public static int DefaultLevel = 0;

		public static uint CreateCost = 1000u;

		public static float ExperienceMultiplier = 1f;

		public static bool WarpOutdoorsOnly = false;

		public static bool ClanWarDeathPay = true;

		public static uint ClanWarDeathPercent = 10u;

		public static bool ClanWarMurderFee = true;

		public static uint ClanWarMurderPercent = 10u;

		public static uint ClanWarDeclaredGainPercent = 20u;

		public static uint ClanWarDeclinedLostPercent = 25u;

		public static string ClanWarDeclinedPenalty = "7d";

		public static string ClanWarAcceptedTime = "14d";

		public static string ClanWarEndedPenalty = "7d";

		private static Dictionary<uint, ulong> dictionary_0;

		public static Dictionary<uint, ClanData> Database;

		public static List<ClanLevel> Levels;

		public static Dictionary<string, int> CraftExperience;

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static int int_0;

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
				int result;
				if (Clans.Database == null)
				{
					result = 0;
				}
				else
				{
					result = Clans.Database.Count;
				}
				return result;
			}
		}

		public static ClanData[] All
		{
			get
			{
				return Clans.Database.Values.ToArray<ClanData>();
			}
		}

		public static void Initialize()
		{
			Clans.SaveFilePath = Path.Combine(Core.SavePath, Clans.string_0);
			Clans.Database = new Dictionary<uint, ClanData>();
			Clans.dictionary_0 = new Dictionary<uint, ulong>();
			if (Core.DatabaseType.Contains("FILE"))
			{
				Clans.Initialized = Clans.LoadAsTextFile();
			}
			if (Core.DatabaseType.Contains("MYSQL"))
			{
				Clans.Initialized = Clans.LoadAsDatabaseSQL();
			}
		}

		public static bool LoadAsTextFile()
		{
			Clans.Loaded = 0;
			bool result;
			if (!File.Exists(Clans.SaveFilePath))
			{
				result = false;
			}
			else
			{
				string text = File.ReadAllText(Clans.SaveFilePath);
				if (string.IsNullOrEmpty(text))
				{
					result = false;
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
					ClanData clanData = null;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text3 = array2[i];
						Predicate<ClanLevel> predicate = null;
						Clans.Class0 @class = new Clans.Class0();
						if (text3.StartsWith("[") && text3.EndsWith("]"))
						{
							clanData = null;
							if (!(v == null) && text2 != null)
							{
								uint num = text3.Substring(1, text3.Length - 2).ToUInt32();
								if (num != 0u)
								{
									if (Clans.Database.ContainsKey(num))
									{
										clanData = Clans.Database[num];
									}
									else
									{
										num = Helper.NewSerial;
										clanData = new ClanData(num, null, null, 0uL, default(DateTime));
										Clans.Database.Add(num, clanData);
										Clans.Loaded++;
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
								if (clanData == null)
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
								else
								{
									string text4 = @class.string_0[0].ToUpper();
									switch (text4)
									{
									case "NAME":
										clanData.Name = @class.string_0[1].Trim();
										break;
									case "ABBREV":
										clanData.Abbr = @class.string_0[1].Trim();
										break;
									case "LEADER":
										clanData.LeaderID = ulong.Parse(@class.string_0[1]);
										break;
									case "CREATED":
										clanData.Created = DateTime.Parse(@class.string_0[1]);
										break;
									case "FLAGS":
										clanData.Flags = @class.string_0[1].ToEnum<ClanFlags>();
										break;
									case "BALANCE":
										clanData.Balance = ulong.Parse(@class.string_0[1]);
										break;
									case "TAX":
										clanData.Tax = uint.Parse(@class.string_0[1]);
										break;
									case "LEVEL":
									{
										ClanData clanData2 = clanData;
										List<ClanLevel> levels = Clans.Levels;
										if (predicate == null)
										{
											predicate = new Predicate<ClanLevel>(@class.method_0);
										}
										clanData2.SetLevel(levels.Find(predicate));
										break;
									}
									case "EXPERIENCE":
										clanData.Experience = ulong.Parse(@class.string_0[1]);
										break;
									case "LOCATION":
										@class.string_0 = @class.string_0[1].Split(new char[]
										{
											','
										});
										if (@class.string_0.Length > 0)
										{
											float.TryParse(@class.string_0[0].Trim(), out clanData.Location.x);
										}
										if (@class.string_0.Length > 1)
										{
											float.TryParse(@class.string_0[1].Trim(), out clanData.Location.y);
										}
										if (@class.string_0.Length > 2)
										{
											float.TryParse(@class.string_0[2].Trim(), out clanData.Location.z);
										}
										break;
									case "MOTD":
										clanData.MOTD = @class.string_0[1].Trim();
										break;
									case "PENALTY":
										clanData.Penalty = DateTime.Parse(@class.string_0[1]);
										break;
									case "HOSTILE":
										@class.string_0 = @class.string_0[1].Split(new char[]
										{
											','
										});
										if (@class.string_0.Length >= 2)
										{
											clanData.Hostile.Add(@class.string_0[0].ToUInt32(), DateTime.Parse(@class.string_0[1]));
										}
										break;
									case "MEMBER":
									{
										@class.string_0 = @class.string_0[1].Split(new char[]
										{
											','
										});
										ulong steam_id = ulong.Parse(@class.string_0[0]);
										UserData bySteamID = Users.GetBySteamID(steam_id);
										if (bySteamID != null)
										{
											for (int j = 1; j < @class.string_0.Length; j++)
											{
												@class.string_0[j - 1] = @class.string_0[j];
											}
											Array.Resize<string>(ref @class.string_0, @class.string_0.Length - 1);
											ClanMemberFlags value = string.Join(",", @class.string_0).ToEnum<ClanMemberFlags>();
											bySteamID.Clan = clanData;
											clanData.Members.Add(bySteamID, value);
										}
										break;
									}
									}
								}
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		public static int SaveAsTextFile()
		{
			using (StreamWriter streamWriter = File.CreateText(Clans.SaveFilePath + ".new"))
			{
				streamWriter.WriteLine("TITLE=" + Core.ProductName);
				streamWriter.WriteLine("VERSION=" + Core.Version);
				streamWriter.WriteLine("TIME=" + (uint)Environment.TickCount);
				streamWriter.WriteLine();
				foreach (uint current in Clans.Database.Keys)
				{
					streamWriter.WriteLine("[" + current.ToHEX(true) + "]");
					streamWriter.WriteLine("NAME=" + Clans.Database[current].Name);
					streamWriter.WriteLine("ABBREV=" + Clans.Database[current].Abbr);
					streamWriter.WriteLine("LEADER=" + Clans.Database[current].LeaderID);
					streamWriter.WriteLine("CREATED=" + Clans.Database[current].Created.ToString("MM/dd/yyyy HH:mm:ss"));
					streamWriter.WriteLine("FLAGS=" + Clans.Database[current].Flags.ToString().Replace(" ", ""));
					streamWriter.WriteLine("BALANCE=" + Clans.Database[current].Balance.ToString());
					streamWriter.WriteLine("TAX=" + Clans.Database[current].Tax.ToString());
					streamWriter.WriteLine("LEVEL=" + Clans.Database[current].Level.Id);
					streamWriter.WriteLine("EXPERIENCE=" + Clans.Database[current].Experience);
					streamWriter.WriteLine(string.Concat(new object[]
					{
						"LOCATION=",
						Clans.Database[current].Location.x,
						",",
						Clans.Database[current].Location.y,
						",",
						Clans.Database[current].Location.z
					}));
					streamWriter.WriteLine("MOTD=" + Clans.Database[current].MOTD);
					streamWriter.WriteLine("PENALTY=" + Clans.Database[current].Penalty.ToString("MM/dd/yyyy HH:mm:ss"));
					if (Clans.Database[current].Hostile.Count > 0)
					{
						foreach (uint current2 in Clans.Database[current].Hostile.Keys)
						{
							streamWriter.WriteLine("HOSTILE=" + current2.ToHEX(true) + "," + Clans.Database[current].Hostile[current2].ToString("MM/dd/yyyy HH:mm:ss"));
						}
					}
					foreach (UserData current3 in Clans.Database[current].Members.Keys)
					{
						streamWriter.WriteLine(string.Concat(new object[]
						{
							"MEMBER=",
							current3.SteamID,
							",",
							Clans.Database[current].Members[current3].ToString().Replace(" ", "")
						}));
					}
					streamWriter.WriteLine();
				}
			}
			Helper.CreateFileBackup(Clans.SaveFilePath);
			File.Move(Clans.SaveFilePath + ".new", Clans.SaveFilePath);
			return Clans.Database.Count;
		}

		public static bool LoadAsDatabaseSQL()
		{
			Predicate<ClanLevel> predicate = null;
			Clans.Class1 @class = new Clans.Class1();
			Clans.Loaded = 0;
			ClanData clanData = null;
			UserData userData = null;
			@class.row_0 = null;
			MySQL.Result result = MySQL.Query("SELECT * FROM `db_clans`;", false);
			MySQL.Result result2 = MySQL.Query("SELECT * FROM `db_clans_members`;", false);
			MySQL.Result result3 = MySQL.Query("SELECT * FROM `db_clans_hostile`;", false);
			if (result2 != null && result != null)
			{
				foreach (MySQL.Row current in result2.Row)
				{
					Clans.Class2 class2 = new Clans.Class2();
					class2.class1_0 = @class;
					ulong asUInt = current.Get("user_id").AsUInt64;
					class2.uint_0 = current.Get("clan_id").AsUInt;
					userData = Users.GetBySteamID(asUInt);
					if (userData != null && class2.uint_0 != 0u)
					{
						if (@class.row_0 == null || @class.row_0.Get("id").AsUInt != class2.uint_0)
						{
							@class.row_0 = result.Row.Find(new Predicate<MySQL.Row>(class2.method_0));
						}
						if (@class.row_0 == null)
						{
							MySQL.Query(string.Format(Clans.SQL_DELETE_MEMBER, asUInt), false);
						}
						else
						{
							ClanMemberFlags value = current.Get("privileges").AsEnum<ClanMemberFlags>();
							if (clanData == null || clanData.ID != class2.uint_0)
							{
								clanData = Clans.Get(class2.uint_0);
							}
							if (clanData == null)
							{
								clanData = new ClanData(class2.uint_0, null, null, 0uL, default(DateTime));
								clanData.Name = @class.row_0.Get("name").AsString;
								clanData.Abbr = @class.row_0.Get("abbrev").AsString;
								clanData.LeaderID = @class.row_0.Get("leader_id").AsUInt64;
								clanData.Created = @class.row_0.Get("created").AsDateTime;
								clanData.Flags = @class.row_0.Get("flags").AsEnum<ClanFlags>();
								clanData.Balance = @class.row_0.Get("balance").AsUInt64;
								clanData.Tax = @class.row_0.Get("tax").AsUInt;
								ClanData clanData2 = clanData;
								List<ClanLevel> levels = Clans.Levels;
								if (predicate == null)
								{
									predicate = new Predicate<ClanLevel>(@class.method_0);
								}
								clanData2.SetLevel(levels.Find(predicate));
								clanData.Experience = @class.row_0.Get("experience").AsUInt64;
								string[] array = @class.row_0.Get("location").AsString.Split(new char[]
								{
									','
								});
								if (array.Length > 0)
								{
									float.TryParse(array[0], out clanData.Location.x);
								}
								if (array.Length > 1)
								{
									float.TryParse(array[1], out clanData.Location.y);
								}
								if (array.Length > 2)
								{
									float.TryParse(array[2], out clanData.Location.z);
								}
								clanData.MOTD = @class.row_0.Get("motd").AsString;
								clanData.Penalty = @class.row_0.Get("penalty").AsDateTime;
								if (result3 != null)
								{
									foreach (MySQL.Row current2 in result3.Row)
									{
										if (current2.Get("clan_id").AsUInt == class2.uint_0)
										{
											clanData.Hostile.Add(current2.Get("hostile_id").AsUInt, current2.Get("ending").AsDateTime);
										}
									}
								}
								Clans.Database.Add(clanData.ID, clanData);
								Clans.Loaded++;
							}
							clanData.Members.Add(userData, value);
							userData.Clan = clanData;
						}
					}
					else
					{
						MySQL.Query(string.Format(Clans.SQL_DELETE_MEMBER, asUInt), false);
					}
				}
			}
			Clans.dictionary_0.Clear();
			foreach (uint current3 in Clans.Database.Keys)
			{
				Clans.dictionary_0.Add(current3, Clans.Database[current3].Hash);
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
				using (Dictionary<uint, ClanData>.ValueCollection.Enumerator enumerator = Clans.Database.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClanData current = enumerator.Current;
						Clans.SQL_Update(current, true);
					}
					goto IL_68;
				}
				IL_5A:
				LibRust.Cycle();
				Thread.Sleep(100);
				IL_68:
				if (MySQL.Queued)
				{
					goto IL_5A;
				}
				result = true;
			}
			return result;
		}

		public static void SQL_Update(ClanData clandata, bool updateHostile = false)
		{
			MySQL.Update(string.Format(Clans.SQL_INSERT_CLAN, new object[]
			{
				clandata.ID,
				clandata.Created.ToString("yyyy-MM-dd HH:mm:ss"),
				clandata.Name,
				clandata.Abbr,
				clandata.LeaderID,
				clandata.Flags.ToString().Replace(" ", ""),
				clandata.Balance,
				clandata.Tax,
				clandata.Level.Id,
				clandata.Experience,
				string.Concat(new object[]
				{
					clandata.Location.x,
					",",
					clandata.Location.y,
					",",
					clandata.Location.z
				}),
				clandata.MOTD,
				clandata.Penalty.ToString("yyyy-MM-dd HH:mm:ss")
			}));
			foreach (UserData current in clandata.Members.Keys)
			{
				MySQL.Update(string.Format(Clans.SQL_INSERT_MEMBER, current.SteamID, clandata.ID, clandata.Members[current].ToString().Replace(" ", "")));
			}
			if (updateHostile)
			{
				foreach (uint current2 in clandata.Hostile.Keys)
				{
					MySQL.Update(string.Format(Clans.SQL_INSERT_CLAN_HOSTILE, clandata.ID, current2, clandata.Hostile[current2].ToString("yyyy-MM-dd HH:mm:ss")));
				}
			}
			if (!Clans.dictionary_0.ContainsKey(clandata.ID))
			{
				Clans.dictionary_0.Add(clandata.ID, 0uL);
			}
			Clans.dictionary_0[clandata.ID] = Clans.Database[clandata.ID].Hash;
		}

		public static void SQL_SynchronizeClans()
		{
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Result result = MySQL.Query("SELECT * FROM `db_clans`;", true);
				if (result != null)
				{
					using (List<MySQL.Row>.Enumerator enumerator = result.Row.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Predicate<ClanLevel> predicate = null;
							Clans.Class3 @class = new Clans.Class3();
							@class.row_0 = enumerator.Current;
							uint asUInt = @class.row_0.Get("id").AsUInt;
							ClanData clanData;
							if (Clans.Database.ContainsKey(asUInt))
							{
								clanData = Clans.Get(asUInt);
							}
							else
							{
								Clans.Database.Add(asUInt, clanData = new ClanData(asUInt, null, null, 0uL, default(DateTime)));
							}
							if (!Clans.dictionary_0.ContainsKey(asUInt))
							{
								Clans.dictionary_0.Add(asUInt, 0uL);
							}
							if (Clans.Database[clanData.ID].Hash == Clans.dictionary_0[clanData.ID])
							{
								clanData.Name = @class.row_0.Get("name").AsString;
								clanData.Abbr = @class.row_0.Get("abbrev").AsString;
								clanData.LeaderID = @class.row_0.Get("leader_id").AsUInt64;
								clanData.Created = @class.row_0.Get("created").AsDateTime;
								clanData.Flags = @class.row_0.Get("flags").AsEnum<ClanFlags>();
								clanData.Balance = @class.row_0.Get("balance").AsUInt64;
								clanData.Tax = @class.row_0.Get("tax").AsUInt;
								ClanData clanData2 = clanData;
								List<ClanLevel> levels = Clans.Levels;
								if (predicate == null)
								{
									predicate = new Predicate<ClanLevel>(@class.method_0);
								}
								clanData2.SetLevel(levels.Find(predicate));
								clanData.Experience = @class.row_0.Get("experience").AsUInt64;
								string[] array = @class.row_0.Get("location").AsString.Replace(", ", ",").Split(new char[]
								{
									','
								});
								if (array.Length > 0)
								{
									float.TryParse(array[0].Trim(), out clanData.Location.x);
								}
								if (array.Length > 1)
								{
									float.TryParse(array[1].Trim(), out clanData.Location.y);
								}
								if (array.Length > 2)
								{
									float.TryParse(array[2].Trim(), out clanData.Location.z);
								}
								clanData.MOTD = @class.row_0.Get("motd").AsString;
								clanData.Penalty = @class.row_0.Get("penalty").AsDateTime;
							}
							else
							{
								Clans.SQL_Update(clanData, false);
							}
							Clans.dictionary_0[asUInt] = Clans.Database[asUInt].Hash;
						}
					}
				}
			}
		}

		public static ClanData Create(string name, ulong leader_id, DateTime created)
		{
			ClanData clanData = new ClanData(Helper.NewSerial, name, "", leader_id, created);
			ClanData result;
			if (clanData == null)
			{
				result = null;
			}
			else
			{
				Clans.Database.Add(clanData.ID, clanData);
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					Clans.SQL_Update(clanData, false);
				}
				result = clanData;
			}
			return result;
		}

		public static void Remove(ClanData clandata)
		{
			if (clandata != null)
			{
				Clans.Remove(clandata.ID);
			}
		}

		public static void Remove(string name)
		{
			ClanData clanData = Clans.Find(name);
			if (clanData != null)
			{
				Clans.Remove(clanData);
			}
		}

		public static void Remove(uint id)
		{
			if (Clans.Database.ContainsKey(id))
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					foreach (UserData current in Clans.Database[id].Members.Keys)
					{
						MySQL.Update(string.Format(Clans.SQL_DELETE_MEMBER, current.SteamID));
					}
					foreach (uint current2 in Clans.Database[id].Hostile.Keys)
					{
						MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN_HOSTILE, id));
						MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN_HOSTILE, current2));
					}
					MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN, id));
				}
				Clans.Database[id].Hostile.Clear();
				Clans.Database[id].Members.Clear();
				Clans.Database.Remove(id);
			}
		}

		public static ClanData Find(string name)
		{
			ClanData result;
			if (!string.IsNullOrEmpty(name))
			{
				foreach (uint current in Clans.Database.Keys)
				{
					if (Clans.Database[current].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						ClanData clanData = Clans.Database[current];
						result = clanData;
						return result;
					}
					if (Clans.Database[current].Abbr.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						ClanData clanData = Clans.Database[current];
						result = clanData;
						return result;
					}
				}
			}
			result = null;
			return result;
		}

		public static ClanData Find(ulong leader_id)
		{
			ClanData result;
			foreach (uint current in Clans.Database.Keys)
			{
				if (Clans.Database[current].LeaderID == leader_id)
				{
					result = Clans.Database[current];
					return result;
				}
			}
			result = null;
			return result;
		}

		public static ClanData Get(uint id)
		{
			ClanData result;
			if (Clans.Database.ContainsKey(id))
			{
				result = Clans.Database[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static bool MemberJoin(ClanData clanData, UserData userData)
		{
			bool result;
			if (clanData != null && userData != null && !clanData.Members.ContainsKey(userData))
			{
				ClanMemberFlags clanMemberFlags = (ClanMemberFlags)0;
				if (clanData.LeaderID == userData.SteamID)
				{
					clanMemberFlags |= (ClanMemberFlags.invite | ClanMemberFlags.dismiss | ClanMemberFlags.management);
				}
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Clans.SQL_INSERT_MEMBER, userData.SteamID, clanData.ID, clanMemberFlags.ToString().Replace(" ", "")));
				}
				clanData.Members.Add(userData, clanMemberFlags);
				userData.Clan = clanData;
				NetUser netUser = NetUser.FindByUserID(userData.SteamID);
				if (netUser != null)
				{
					Broadcast.Message(netUser, Config.GetMessageClan("Command.Clan.PlayerJoined", clanData, null, userData), null, 0f);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool MemberLeave(ClanData clanData, UserData userData)
		{
			bool result;
			if (clanData != null && userData != null)
			{
				if (userData.Clan == clanData)
				{
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format(Clans.SQL_DELETE_MEMBER, userData.SteamID));
					}
					if (!clanData.Members.ContainsKey(userData))
					{
						result = false;
						return result;
					}
					clanData.Members.Remove(userData);
					userData.Clan = null;
					NetUser netUser = NetUser.FindByUserID(userData.SteamID);
					if (netUser != null)
					{
						Broadcast.Message(netUser, Config.GetMessageClan("Command.Clan.PlayerLeaved", clanData, null, userData), null, 0f);
					}
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static bool AcceptsWar(ClanData declared_clan, ClanData accepted_clan)
		{
			bool result;
			if (declared_clan != null && accepted_clan != null)
			{
				if (declared_clan != accepted_clan)
				{
					DateTime dateTime = Helper.StringToTime(Clans.ClanWarAcceptedTime, DateTime.Now);
					if (dateTime <= DateTime.Now)
					{
						dateTime = dateTime.AddDays(14.0);
					}
					if (!accepted_clan.Hostile.ContainsKey(declared_clan.ID))
					{
						accepted_clan.Hostile.Add(declared_clan.ID, dateTime);
						Broadcast.MessageClan(accepted_clan, Config.GetMessageClan("Command.Clan.Hostile.Accepted", declared_clan, null, null));
						if (Core.DatabaseType.Equals("MYSQL"))
						{
							MySQL.Update(string.Format(Clans.SQL_INSERT_CLAN_HOSTILE, accepted_clan.ID, declared_clan.ID, accepted_clan.Hostile[declared_clan.ID].ToString("yyyy-MM-dd HH:mm:ss")));
						}
					}
					if (!declared_clan.Hostile.ContainsKey(accepted_clan.ID))
					{
						declared_clan.Hostile.Add(accepted_clan.ID, dateTime);
						Broadcast.MessageClan(declared_clan, Config.GetMessageClan("Command.Clan.Hostile.Declared", accepted_clan, null, null));
						if (Core.DatabaseType.Equals("MYSQL"))
						{
							MySQL.Update(string.Format(Clans.SQL_INSERT_CLAN_HOSTILE, declared_clan.ID, accepted_clan.ID, declared_clan.Hostile[accepted_clan.ID].ToString("yyyy-MM-dd HH:mm:ss")));
						}
					}
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static bool DeclineWar(ClanData declared_clan, ClanData declined_clan)
		{
			bool result;
			if (declared_clan != null && declined_clan != null)
			{
				if (declared_clan != declined_clan)
				{
					string[] messagesClan = Config.GetMessagesClan("Command.Clan.Hostile.DeclinedTo", declared_clan, null, null);
					string[] messagesClan2 = Config.GetMessagesClan("Command.Clan.Hostile.DeclinedFrom", declined_clan, null, null);
					string[] array = messagesClan;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (!text.Contains("%CLAN."))
						{
							Broadcast.MessageClan(declined_clan, text);
						}
					}
					string[] array2 = messagesClan2;
					for (int j = 0; j < array2.Length; j++)
					{
						string text2 = array2[j];
						if (!text2.Contains("%CLAN."))
						{
							Broadcast.MessageClan(declared_clan, text2);
						}
					}
					ulong num = declined_clan.Balance * (ulong)Clans.ClanWarDeclaredGainPercent / 100uL;
					ulong num2 = declined_clan.Balance * (ulong)Clans.ClanWarDeclinedLostPercent / 100uL;
					ulong num3 = declined_clan.Experience * (ulong)Clans.ClanWarDeclaredGainPercent / 100uL;
					ulong num4 = declined_clan.Experience * (ulong)Clans.ClanWarDeclinedLostPercent / 100uL;
					declared_clan.Balance += num;
					declared_clan.Experience += num3;
					declined_clan.Balance -= num2;
					declined_clan.Experience -= num4;
					declined_clan.Penalty = Helper.StringToTime(Clans.ClanWarDeclinedPenalty, DateTime.Now);
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static void TransferAccept(ClanData clanData, UserData userData)
		{
			NetUser netUser = NetUser.FindByUserID(clanData.LeaderID);
			if (netUser != null)
			{
				Broadcast.MessageClan(netUser, clanData, Config.GetMessageClan("Command.Clan.Transfer.QueryAnswerY", clanData, null, userData));
			}
			clanData.LeaderID = userData.SteamID;
			clanData.Members[userData] = (ClanMemberFlags.invite | ClanMemberFlags.dismiss | ClanMemberFlags.management);
			Broadcast.MessageClan(clanData, Config.GetMessageClan("Command.Clan.Transfer.Success", clanData, null, userData));
		}

		public static void TransferDecline(ClanData clanData, UserData userData)
		{
			NetUser netUser = NetUser.FindByUserID(clanData.LeaderID);
			if (netUser != null)
			{
				Broadcast.MessageClan(netUser, clanData, Config.GetMessageClan("Command.Clan.Transfer.QueryAnswerN", clanData, null, userData));
			}
		}
	}
}
