using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RustExtended
{
	public class Blocklist
	{
		private static string string_0 = "blocked_ip.txt";

		private static List<string> list_0 = new List<string>();

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static bool bool_0;

		public static string FilePath
		{
			get;
			private set;
		}

		public static int Count
		{
			get
			{
				return Blocklist.list_0.Count;
			}
		}

		public static bool Initialized
		{
			get;
			private set;
		}

		public static void Initialize()
		{
			Blocklist.FilePath = Path.Combine(Core.SavePath, Blocklist.string_0);
			if (Core.DatabaseType.Equals("FILE"))
			{
				Blocklist.Initialized = Blocklist.LoadAsTextFile();
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				Blocklist.Initialized = Blocklist.LoadAsDatabaseSQL();
			}
		}

		public static bool LoadAsTextFile()
		{
			if (!File.Exists(Blocklist.FilePath))
			{
				File.CreateText(Blocklist.FilePath).Close();
			}
			Blocklist.list_0 = File.ReadAllLines(Blocklist.FilePath).ToList<string>();
			return true;
		}

		public static int SaveAsTextFile()
		{
			File.WriteAllLines(Blocklist.FilePath, Blocklist.list_0.ToArray());
			return Blocklist.list_0.Count;
		}

		public static bool LoadAsDatabaseSQL()
		{
			MySQL.Result result = MySQL.Query("SELECT * FROM `db_blocked_ip`;", false);
			if (result != null)
			{
				foreach (MySQL.Row current in result.Row)
				{
					string asString = current.Get("ip_address").AsString;
					if (!Blocklist.list_0.Contains(asString))
					{
						Blocklist.list_0.Add(asString);
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
				using (List<string>.Enumerator enumerator = Blocklist.list_0.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						MySQL.Update(string.Format("REPLACE INTO `db_blocked_ip` (`ip_address`) VALUES ('{0}');", current));
					}
					goto IL_6C;
				}
				IL_5E:
				LibRust.Cycle();
				Thread.Sleep(100);
				IL_6C:
				if (MySQL.Queued)
				{
					goto IL_5E;
				}
				result = true;
			}
			return result;
		}

		public static bool Exists(string ipAddress)
		{
			return Blocklist.list_0.Contains(ipAddress);
		}

		public static void Add(string ipAddress)
		{
			if (!Blocklist.list_0.Contains(ipAddress))
			{
				Blocklist.list_0.Add(ipAddress);
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format("REPLACE INTO `db_blocked_ip` (`ip_address`) VALUES ('{0}');", ipAddress));
			}
			if (Core.DatabaseType.Equals("FILE"))
			{
				Blocklist.SaveAsTextFile();
			}
		}

		public static void Remove(string ipAddress)
		{
			if (Blocklist.list_0.Contains(ipAddress))
			{
				Blocklist.list_0.Remove(ipAddress);
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				MySQL.Update(string.Format("DELETE FROM `db_blocked_ip` WHERE `ip_address`='{0}';", ipAddress));
			}
			if (Core.DatabaseType.Equals("FILE"))
			{
				Blocklist.SaveAsTextFile();
			}
		}
	}
}
