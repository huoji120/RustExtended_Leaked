namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Banned
    {
        [CompilerGenerated]
        private static bool bool_0;
        public static Dictionary<ulong, UserBanned> Database;
        [CompilerGenerated]
        private static int int_0;
        public static string SQL_CLEAR_USER_BANLIST = "TRUNCATE `db_users_banned`;";
        public static string SQL_DELETE_USER_BANNED = "DELETE FROM `db_users_banned` WHERE `steam_id`={0};";
        public static string SQL_INSERT_USER_BANNED = "REPLACE INTO `db_users_banned` (`steam_id`, `ip_address`, `date`, `period`, `reason`, `details`) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5});";
        private static string string_0 = "rust_banned.txt";
        [CompilerGenerated]
        private static string string_1;

        public static bool Add(ulong steam_id, string reason = "", DateTime period = default(DateTime), string details = "")
        {
            if (!Users.Database.ContainsKey(steam_id))
            {
                return false;
            }
            Users.SetFlags(steam_id, UserFlags.banned, true);
            if (!Database.ContainsKey(steam_id))
            {
                if (reason == "")
                {
                    reason = "No reason.";
                }
                if (details == "")
                {
                    details = "No details.";
                }
                UserBanned banned = new UserBanned(Users.Database[steam_id].LastConnectIP, DateTime.Now, period, reason, details);
                Database.Add(steam_id, banned);
                if (Core.DatabaseType.Equals("FILE"))
                {
                    SaveAsTextFile();
                }
                if (Core.DatabaseType.Equals("MYSQL"))
                {
                    MySQL.Update(string.Format(SQL_INSERT_USER_BANNED, new object[] { steam_id, banned.IP, banned.Time.ToString("yyyy-MM-dd HH:mm:ss"), banned.Period.ToString("yyyy-MM-dd HH:mm:ss"), MySQL.QuoteString(banned.Reason), MySQL.QuoteString(banned.Details) }));
                }
            }
            return true;
        }

        public static bool Clear()
        {
            foreach (ulong num in Users.Database.Keys)
            {
                if (Users.HasFlag(num, UserFlags.banned))
                {
                    Users.Database[num].SetFlag(UserFlags.banned, false);
                }
                if (Database.ContainsKey(num))
                {
                    Database.Remove(num);
                    Blocklist.Remove(Users.Database[num].LastConnectIP);
                }
            }
            if (Core.DatabaseType.Equals("FILE"))
            {
                SaveAsTextFile();
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(SQL_CLEAR_USER_BANLIST);
            }
            return true;
        }

        public static UserBanned Get(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id];
        }

        public static void Initialize()
        {
            Initialized = false;
            SaveFilePath = Path.Combine(Core.SavePath, string_0);
            Database = new Dictionary<ulong, UserBanned>();
            if (Core.DatabaseType.Contains("FILE"))
            {
                Initialized = LoadAsTextFile();
            }
            if (Core.DatabaseType.Contains("MYSQL"))
            {
                Initialized = LoadAsDatabaseSQL();
            }
        }

        public static bool LoadAsDatabaseSQL()
        {
            MySQL.Result result = MySQL.Query("SELECT * FROM `db_users_banned`;", false);
            if (result != null)
            {
                foreach (MySQL.Row row in result.Row)
                {
                    ulong key = row.Get("steam_id").AsUInt64;
                    if (!Database.ContainsKey(key))
                    {
                        Database.Add(key, new UserBanned(row.Get("ip_address").AsString, row.Get("date").AsDateTime, row.Get("period").AsDateTime, row.Get("reason").AsString, ""));
                    }
                }
            }
            return true;
        }

        public static bool LoadAsTextFile()
        {
            Loaded = 0;
            if (!File.Exists(SaveFilePath))
            {
                return false;
            }
            foreach (string str2 in File.ReadAllText(SaveFilePath).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if ((str2.Trim().Length != 0) && str2.Contains("="))
                {
                    string str3 = str2.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    if (str3.Length != 0)
                    {
                        string[] strArray2 = str3.Split(new char[] { '=' });
                        ulong result = 0L;
                        if (ulong.TryParse(strArray2[0], out result) && !Database.ContainsKey(result))
                        {
                            strArray2 = strArray2[1].Split(new string[] { "::" }, StringSplitOptions.None);
                            if (strArray2.Length > 0)
                            {
                                strArray2[0].Trim();
                            }
                            string ipAddr = (strArray2.Length > 1) ? strArray2[1].Trim() : "";
                            DateTime time = (strArray2.Length > 2) ? DateTime.Parse(strArray2[2].Trim()) : new DateTime();
                            DateTime period = (strArray2.Length > 3) ? DateTime.Parse(strArray2[3].Trim()) : new DateTime();
                            string reason = (strArray2.Length > 4) ? strArray2[4].Trim() : "No reason.";
                            string details = (strArray2.Length > 5) ? strArray2[5].Trim() : "No details.";
                            Database.Add(result, new UserBanned(ipAddr, time, period, reason, details));
                            Loaded++;
                        }
                    }
                }
            }
            return true;
        }

        public static bool Remove(ulong steam_id)
        {
            if (Users.Database.ContainsKey(steam_id))
            {
                Users.SetFlags(steam_id, UserFlags.banned, false);
                Blocklist.Remove(Users.Database[steam_id].LastConnectIP);
            }
            if (Database.ContainsKey(steam_id))
            {
                Database.Remove(steam_id);
            }
            if (Core.DatabaseType.Equals("FILE"))
            {
                SaveAsTextFile();
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_USER_BANNED, steam_id));
            }
            return true;
        }

        public static int SaveAsDatabaseSQL()
        {
            if (!Core.DatabaseType.Equals("MYSQL"))
            {
                return 0;
            }
            foreach (ulong num in Database.Keys)
            {
                MySQL.Update(string.Format(SQL_INSERT_USER_BANNED, new object[] { num, Database[num].IP, Database[num].Time.ToString("yyyy-MM-dd HH:mm:ss"), Database[num].Period.ToString("yyyy-MM-dd HH:mm:ss"), MySQL.QuoteString(Database[num].Reason), MySQL.QuoteString(Database[num].Details) }));
            }
            while (MySQL.Queued)
            {
                LibRust.Cycle();
                Thread.Sleep(100);
            }
            return Database.Count;
        }

        public static int SaveAsTextFile()
        {
            using (StreamWriter writer = File.CreateText(SaveFilePath))
            {
                foreach (ulong num in Database.Keys)
                {
                    if (Database.ContainsKey(num))
                    {
                        writer.WriteLine(string.Concat(new object[] { Users.Database[num].SteamID, "=", Users.Database[num].Username, "::", Database[num].IP, "::", Database[num].Time.ToString("yyyy-MM-dd HH:mm:ss"), "::", Database[num].Period.ToString("yyyy-MM-dd HH:mm:ss"), "::", Database[num].Reason, "::", Database[num].Details }));
                    }
                }
            }
            return Database.Count;
        }

        public static int Count
        {
            get
            {
                return Database.Count;
            }
        }

        public static bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return bool_0;
            }
            [CompilerGenerated]
            private set
            {
                bool_0 = value;
            }
        }

        public static int Loaded
        {
            [CompilerGenerated]
            get
            {
                return int_0;
            }
            [CompilerGenerated]
            private set
            {
                int_0 = value;
            }
        }

        public static string SaveFilePath
        {
            [CompilerGenerated]
            get
            {
                return string_1;
            }
            [CompilerGenerated]
            private set
            {
                string_1 = value;
            }
        }
    }
}

