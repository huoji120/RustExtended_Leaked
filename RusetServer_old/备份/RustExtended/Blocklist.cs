namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Blocklist
    {
        [CompilerGenerated]
        private static bool bool_0;
        private static System.Collections.Generic.List<string> list_0 = new System.Collections.Generic.List<string>();
        private static string string_0 = "blocked_ip.txt";
        [CompilerGenerated]
        private static string string_1;

        public static void Add(string ipAddress)
        {
            if (!list_0.Contains(ipAddress))
            {
                list_0.Add(ipAddress);
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format("REPLACE INTO `db_blocked_ip` (`ip_address`) VALUES ('{0}');", ipAddress));
            }
            if (Core.DatabaseType.Equals("FILE"))
            {
                SaveAsTextFile();
            }
        }

        public static bool Exists(string ipAddress)
        {
            return list_0.Contains(ipAddress);
        }

        public static void Initialize()
        {
            FilePath = Path.Combine(Core.SavePath, string_0);
            if (Core.DatabaseType.Equals("FILE"))
            {
                Initialized = LoadAsTextFile();
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                Initialized = LoadAsDatabaseSQL();
            }
        }

        public static bool LoadAsDatabaseSQL()
        {
            MySQL.Result result = MySQL.Query("SELECT * FROM `db_blocked_ip`;", false);
            if (result != null)
            {
                foreach (MySQL.Row row in result.Row)
                {
                    string asString = row.Get("ip_address").AsString;
                    if (!list_0.Contains(asString))
                    {
                        list_0.Add(asString);
                    }
                }
            }
            return true;
        }

        public static bool LoadAsTextFile()
        {
            if (!File.Exists(FilePath))
            {
                File.CreateText(FilePath).Close();
            }
            list_0 = File.ReadAllLines(FilePath).ToList<string>();
            return true;
        }

        public static void Remove(string ipAddress)
        {
            if (list_0.Contains(ipAddress))
            {
                list_0.Remove(ipAddress);
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format("DELETE FROM `db_blocked_ip` WHERE `ip_address`='{0}';", ipAddress));
            }
            if (Core.DatabaseType.Equals("FILE"))
            {
                SaveAsTextFile();
            }
        }

        public static bool SaveAsDatabaseSQL()
        {
            if (!Core.DatabaseType.Equals("MYSQL"))
            {
                return false;
            }
            foreach (string str in list_0)
            {
                MySQL.Update(string.Format("REPLACE INTO `db_blocked_ip` (`ip_address`) VALUES ('{0}');", str));
            }
            while (MySQL.Queued)
            {
                LibRust.Cycle();
                Thread.Sleep(100);
            }
            return true;
        }

        public static int SaveAsTextFile()
        {
            File.WriteAllLines(FilePath, list_0.ToArray());
            return list_0.Count;
        }

        public static int Count
        {
            get
            {
                return list_0.Count;
            }
        }

        public static string FilePath
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
    }
}

