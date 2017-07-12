using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RustExtended
{
	public class Banned
	{
		public static string SQL_INSERT_USER_BANNED = "REPLACE INTO `db_users_banned` (`steam_id`, `ip_address`, `date`, `period`, `reason`, `details`) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5});";

		public static string SQL_DELETE_USER_BANNED = "DELETE FROM `db_users_banned` WHERE `steam_id`={0};";

		public static string SQL_CLEAR_USER_BANLIST = "TRUNCATE `db_users_banned`;";

		private static string string_0 = "rust_banned.txt";

		public static Dictionary<ulong, UserBanned> Database;

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
				return Banned.Database.Count;
			}
		}

		public static void Initialize()
		{
			Banned.Initialized = false;
			Banned.SaveFilePath = Path.Combine(Core.SavePath, Banned.string_0);
			Banned.Database = new Dictionary<ulong, UserBanned>();
			if (Core.DatabaseType.Contains("FILE"))
			{
				Banned.Initialized = Banned.LoadAsTextFile();
			}
			if (Core.DatabaseType.Contains("MYSQL"))
			{
				Banned.Initialized = Banned.LoadAsDatabaseSQL();
			}
		}

		public static bool LoadAsTextFile()
		{
			Banned.Loaded = 0;
			bool result;
			if (!File.Exists(Banned.SaveFilePath))
			{
				result = false;
			}
			else
			{
				string text = File.ReadAllText(Banned.SaveFilePath);
				string[] array = text.Split(new string[]
				{
					"\r\n",
					"\n"
				}, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (text2.Trim().Length != 0 && text2.Contains("="))
					{
						string text3 = text2.Split(new string[]
						{
							"//"
						}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						if (text3.Length != 0)
						{
							string[] array3 = text3.Split(new char[]
							{
								'='
							});
							ulong key = 0uL;
							if (ulong.TryParse(array3[0], out key) && !Banned.Database.ContainsKey(key))
							{
								array3 = array3[1].Split(new string[]
								{
									"::"
								}, StringSplitOptions.None);
								if (array3.Length > 0)
								{
									array3[0].Trim();
								}
								string ipAddr = (array3.Length > 1) ? array3[1].Trim() : "";
								DateTime time = (array3.Length > 2) ? DateTime.Parse(array3[2].Trim()) : default(DateTime);
								DateTime period = (array3.Length > 3) ? DateTime.Parse(array3[3].Trim()) : default(DateTime);
								string reason = (array3.Length > 4) ? array3[4].Trim() : "No reason.";
								string details = (array3.Length > 5) ? array3[5].Trim() : "No details.";
								Banned.Database.Add(key, new UserBanned(ipAddr, time, period, reason, details));
								Banned.Loaded++;
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		public static int SaveAsTextFile()
		{
			using (StreamWriter streamWriter = File.CreateText(Banned.SaveFilePath))
			{
				foreach (ulong current in Banned.Database.Keys)
				{
					if (Banned.Database.ContainsKey(current))
					{
						streamWriter.WriteLine(string.Concat(new object[]
						{
							Users.Database[current].SteamID,
							"=",
							Users.Database[current].Username,
							"::",
							Banned.Database[current].IP,
							"::",
							Banned.Database[current].Time.ToString("yyyy-MM-dd HH:mm:ss"),
							"::",
							Banned.Database[current].Period.ToString("yyyy-MM-dd HH:mm:ss"),
							"::",
							Banned.Database[current].Reason,
							"::",
							Banned.Database[current].Details
						}));
					}
				}
			}
			return Banned.Database.Count;
		}

		public static bool LoadAsDatabaseSQL()
		{
			MySQL.Result result = MySQL.Query("SELECT * FROM `db_users_banned`;", false);
			if (result != null)
			{
				foreach (MySQL.Row current in result.Row)
				{
					ulong asUInt = current.Get("steam_id").AsUInt64;
					if (!Banned.Database.ContainsKey(asUInt))
					{
						Banned.Database.Add(asUInt, new UserBanned(current.Get("ip_address").AsString, current.Get("date").AsDateTime, current.Get("period").AsDateTime, current.Get("reason").AsString, ""));
					}
				}
			}
			return true;
		}

		public static int SaveAsDatabaseSQL()
		{
			int result;
			if (!Core.DatabaseType.Equals("MYSQL"))
			{
				result = 0;
			}
			else
			{
				using (Dictionary<ulong, UserBanned>.KeyCollection.Enumerator enumerator = Banned.Database.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ulong current = enumerator.Current;
						MySQL.Update(string.Format(Banned.SQL_INSERT_USER_BANNED, new object[]
						{
							current,
							Banned.Database[current].IP,
							Banned.Database[current].Time.ToString("yyyy-MM-dd HH:mm:ss"),
							Banned.Database[current].Period.ToString("yyyy-MM-dd HH:mm:ss"),
							MySQL.QuoteString(Banned.Database[current].Reason),
							MySQL.QuoteString(Banned.Database[current].Details)
						}));
					}
					goto IL_117;
				}
				IL_109:
				LibRust.Cycle();
				Thread.Sleep(100);
				IL_117:
				if (MySQL.Queued)
				{
					goto IL_109;
				}
				result = Banned.Database.Count;
			}
			return result;
		}

		public static UserBanned Get(ulong steam_id)
		{
			UserBanned result;
			if (!Banned.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				result = Banned.Database[steam_id];
			}
			return result;
		}

		public static bool Add(ulong steam_id, string reason = "", DateTime period = default(DateTime), string details = "")
		{
			bool result;
			if (!Users.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				Users.SetFlags(steam_id, UserFlags.banned, true);
				if (Banned.Database.ContainsKey(steam_id))
				{
					result = true;
				}
				else
				{
					if (reason == "")
					{
						reason = "No reason.";
					}
					if (details == "")
					{
						details = "No details.";
					}
					UserBanned userBanned = new UserBanned(Users.Database[steam_id].LastConnectIP, DateTime.Now, period, reason, details);
					Banned.Database.Add(steam_id, userBanned);
					if (Core.DatabaseType.Equals("FILE"))
					{
						Banned.SaveAsTextFile();
					}
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format(Banned.SQL_INSERT_USER_BANNED, new object[]
						{
							steam_id,
							userBanned.IP,
							userBanned.Time.ToString("yyyy-MM-dd HH:mm:ss"),
							userBanned.Period.ToString("yyyy-MM-dd HH:mm:ss"),
							MySQL.QuoteString(userBanned.Reason),
							MySQL.QuoteString(userBanned.Details)
						}));
					}
					result = true;
				}
			}
			return result;
		}

		public static bool Remove(ulong steam_id)
		{
			if (Users.Database.ContainsKey(steam_id))
			{
				Users.SetFlags(steam_id, UserFlags.banned, false);
				Blocklist.Remove(Users.Database[steam_id].LastConnectIP);
			}
			if (Banned.Database.ContainsKey(steam_id))
			{
				Banned.Database.Remove(steam_id);
			}
			if (Core.DatabaseType.Equals("FILE"))
			{
				Banned.SaveAsTextFile();
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format(Banned.SQL_DELETE_USER_BANNED, steam_id));
			}
			return true;
		}

		public static bool Clear()
		{
			foreach (ulong current in Users.Database.Keys)
			{
				if (Users.HasFlag(current, UserFlags.banned))
				{
					Users.Database[current].SetFlag(UserFlags.banned, false);
				}
				if (Banned.Database.ContainsKey(current))
				{
					Banned.Database.Remove(current);
					Blocklist.Remove(Users.Database[current].LastConnectIP);
				}
			}
			if (Core.DatabaseType.Equals("FILE"))
			{
				Banned.SaveAsTextFile();
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(Banned.SQL_CLEAR_USER_BANLIST);
			}
			return true;
		}
	}
}
