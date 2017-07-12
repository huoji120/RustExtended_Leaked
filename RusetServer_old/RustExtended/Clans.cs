namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Clans
    {
        [CompilerGenerated]
        private static bool bool_0;
        public static string ClanWarAcceptedTime = "14d";
        public static bool ClanWarDeathPay = true;
        public static uint ClanWarDeathPercent = 10;
        public static uint ClanWarDeclaredGainPercent = 20;
        public static uint ClanWarDeclinedLostPercent = 0x19;
        public static string ClanWarDeclinedPenalty = "7d";
        public static string ClanWarEndedPenalty = "7d";
        public static bool ClanWarMurderFee = true;
        public static uint ClanWarMurderPercent = 10;
        public static Dictionary<string, int> CraftExperience;
        public static uint CreateCost = 0x3e8;
        public static Dictionary<uint, ClanData> Database;
        public static int DefaultLevel = 0;
        private static Dictionary<uint, ulong> dictionary_0;
        public static bool Enabled = true;
        public static float ExperienceMultiplier = 1f;
        [CompilerGenerated]
        private static int int_0;
        public static System.Collections.Generic.List<ClanLevel> Levels;
        public static string SQL_DELETE_CLAN = "DELETE FROM `db_clans` WHERE `id`={0} LIMIT 1;";
        public static string SQL_DELETE_CLAN_HOSTILE = "DELETE FROM `db_clans_hostile` WHERE `clan_id`={0} LIMIT 1;";
        public static string SQL_DELETE_MEMBER = "DELETE FROM `db_clans_members` WHERE `user_id`={0} LIMIT 1;";
        public static string SQL_INSERT_CLAN = "REPLACE INTO `db_clans` (`id`,`created`,`name`,`abbrev`,`leader_id`,`flags`,`balance`,`tax`,`level`,`experience`,`location`,`motd`,`penalty`) VALUES ({0},'{1}','{2}','{3}',{4},'{5}',{6},{7},{8},{9},'{10}','{11}','{12}');";
        public static string SQL_INSERT_CLAN_HOSTILE = "REPLACE INTO `db_clans_hostile` (`clan_id`,`hostile_id`,`ending`) VALUES ({0},{1},'{2}');";
        public static string SQL_INSERT_MEMBER = "REPLACE INTO `db_clans_members` (`user_id`, `clan_id`, `privileges`) VALUES ({0}, {1}, '{2}');";
        private static string string_0 = "rust_clans.txt";
        [CompilerGenerated]
        private static string string_1;
        public static bool WarpOutdoorsOnly = false;

        public static bool AcceptsWar(ClanData declared_clan, ClanData accepted_clan)
        {
            if (((declared_clan != null) && (accepted_clan != null)) && (declared_clan != accepted_clan))
            {
                DateTime time = Helper.StringToTime(ClanWarAcceptedTime, DateTime.Now);
                if (time <= DateTime.Now)
                {
                    time = time.AddDays(14.0);
                }
                if (!accepted_clan.Hostile.ContainsKey(declared_clan.ID))
                {
                    accepted_clan.Hostile.Add(declared_clan.ID, time);
                    Broadcast.MessageClan(accepted_clan, Config.GetMessageClan("Command.Clan.Hostile.Accepted", declared_clan, null, null));
                    if (Core.DatabaseType.Equals("MYSQL"))
                    {
                        DateTime time2 = accepted_clan.Hostile[declared_clan.ID];
                        MySQL.Update(string.Format(SQL_INSERT_CLAN_HOSTILE, accepted_clan.ID, declared_clan.ID, time2.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                }
                if (!declared_clan.Hostile.ContainsKey(accepted_clan.ID))
                {
                    declared_clan.Hostile.Add(accepted_clan.ID, time);
                    Broadcast.MessageClan(declared_clan, Config.GetMessageClan("Command.Clan.Hostile.Declared", accepted_clan, null, null));
                    if (Core.DatabaseType.Equals("MYSQL"))
                    {
                        DateTime time3 = declared_clan.Hostile[accepted_clan.ID];
                        MySQL.Update(string.Format(SQL_INSERT_CLAN_HOSTILE, declared_clan.ID, accepted_clan.ID, time3.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                }
                return true;
            }
            return false;
        }

        public static ClanData Create(string name, ulong leader_id, DateTime created)
        {
            ClanData data = new ClanData(Helper.NewSerial, name, "", leader_id, created);
            if (data == null)
            {
                return null;
            }
            Database.Add(data.ID, data);
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                SQL_Update(data, false);
            }
            return data;
        }

        public static bool DeclineWar(ClanData declared_clan, ClanData declined_clan)
        {
            if (((declared_clan != null) && (declined_clan != null)) && (declared_clan != declined_clan))
            {
                string[] strArray = Config.GetMessagesClan("Command.Clan.Hostile.DeclinedTo", declared_clan, null, null);
                string[] strArray2 = Config.GetMessagesClan("Command.Clan.Hostile.DeclinedFrom", declined_clan, null, null);
                foreach (string str in strArray)
                {
                    if (!str.Contains("%CLAN."))
                    {
                        Broadcast.MessageClan(declined_clan, str);
                    }
                }
                foreach (string str2 in strArray2)
                {
                    if (!str2.Contains("%CLAN."))
                    {
                        Broadcast.MessageClan(declared_clan, str2);
                    }
                }
                ulong num = (declined_clan.Balance * ClanWarDeclaredGainPercent) / ((ulong) 100L);
                ulong num2 = (declined_clan.Balance * ClanWarDeclinedLostPercent) / ((ulong) 100L);
                ulong num3 = (declined_clan.Experience * ClanWarDeclaredGainPercent) / ((ulong) 100L);
                ulong num4 = (declined_clan.Experience * ClanWarDeclinedLostPercent) / ((ulong) 100L);
                declared_clan.Balance += num;
                declared_clan.Experience += num3;
                declined_clan.Balance -= num2;
                declined_clan.Experience -= num4;
                declined_clan.Penalty = Helper.StringToTime(ClanWarDeclinedPenalty, DateTime.Now);
                return true;
            }
            return false;
        }

        public static ClanData Find(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                ClanData data;
                using (Dictionary<uint, ClanData>.KeyCollection.Enumerator enumerator = Database.Keys.GetEnumerator())
                {
                    uint current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (Database[current].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            return Database[current];
                        }
                        if (Database[current].Abbr.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            goto Label_0071;
                        }
                    }
                    goto Label_008F;
                Label_0071:
                    data = Database[current];
                }
                return data;
            }
        Label_008F:
            return null;
        }

        public static ClanData Find(ulong leader_id)
        {
            foreach (uint num in Database.Keys)
            {
                if (Database[num].LeaderID == leader_id)
                {
                    return Database[num];
                }
            }
            return null;
        }

        public static ClanData Get(uint id)
        {
            if (Database.ContainsKey(id))
            {
                return Database[id];
            }
            return null;
        }

        public static void Initialize()
        {
            SaveFilePath = Path.Combine(Core.SavePath, string_0);
            Database = new Dictionary<uint, ClanData>();
            dictionary_0 = new Dictionary<uint, ulong>();
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
            Predicate<ClanLevel> match = null;
            Class1 class3 = new Class1();
            Loaded = 0;
            ClanData data = null;
            UserData key = null;
            class3.row_0 = null;
            MySQL.Result result = MySQL.Query("SELECT * FROM `db_clans`;", false);
            MySQL.Result result2 = MySQL.Query("SELECT * FROM `db_clans_members`;", false);
            MySQL.Result result3 = MySQL.Query("SELECT * FROM `db_clans_hostile`;", false);
            if ((result2 != null) && (result != null))
            {
                foreach (MySQL.Row row in result2.Row)
                {
                    Class2 class2 = new Class2 {
                        class1_0 = class3
                    };
                    ulong num = row.Get("user_id").AsUInt64;
                    class2.uint_0 = row.Get("clan_id").AsUInt;
                    key = Users.GetBySteamID(num);
                    if ((key != null) && (class2.uint_0 != 0))
                    {
                        if ((class3.row_0 == null) || (class3.row_0.Get("id").AsUInt != class2.uint_0))
                        {
                            class3.row_0 = result.Row.Find(new Predicate<MySQL.Row>(class2.method_0));
                        }
                        if (class3.row_0 == null)
                        {
                            MySQL.Query(string.Format(SQL_DELETE_MEMBER, num), false);
                        }
                        else
                        {
                            ClanMemberFlags flags = row.Get("privileges").AsEnum<ClanMemberFlags>();
                            if ((data == null) || (data.ID != class2.uint_0))
                            {
                                data = Get(class2.uint_0);
                            }
                            if (data == null)
                            {
                                DateTime created = new DateTime();
                                data = new ClanData(class2.uint_0, null, null, 0L, created) {
                                    Name = class3.row_0.Get("name").AsString,
                                    Abbr = class3.row_0.Get("abbrev").AsString,
                                    LeaderID = class3.row_0.Get("leader_id").AsUInt64,
                                    Created = class3.row_0.Get("created").AsDateTime,
                                    Flags = class3.row_0.Get("flags").AsEnum<ClanFlags>(),
                                    Balance = class3.row_0.Get("balance").AsUInt64,
                                    Tax = class3.row_0.Get("tax").AsUInt
                                };
                                if (match == null)
                                {
                                    match = new Predicate<ClanLevel>(class3.method_0);
                                }
                                data.SetLevel(Levels.Find(match));
                                data.Experience = class3.row_0.Get("experience").AsUInt64;
                                string[] strArray = class3.row_0.Get("location").AsString.Split(new char[] { ',' });
                                if (strArray.Length > 0)
                                {
                                    float.TryParse(strArray[0], out data.Location.x);
                                }
                                if (strArray.Length > 1)
                                {
                                    float.TryParse(strArray[1], out data.Location.y);
                                }
                                if (strArray.Length > 2)
                                {
                                    float.TryParse(strArray[2], out data.Location.z);
                                }
                                data.MOTD = class3.row_0.Get("motd").AsString;
                                data.Penalty = class3.row_0.Get("penalty").AsDateTime;
                                if (result3 != null)
                                {
                                    foreach (MySQL.Row row2 in result3.Row)
                                    {
                                        if (row2.Get("clan_id").AsUInt == class2.uint_0)
                                        {
                                            data.Hostile.Add(row2.Get("hostile_id").AsUInt, row2.Get("ending").AsDateTime);
                                        }
                                    }
                                }
                                Database.Add(data.ID, data);
                                Loaded++;
                            }
                            data.Members.Add(key, flags);
                            key.Clan = data;
                        }
                    }
                    else
                    {
                        MySQL.Query(string.Format(SQL_DELETE_MEMBER, num), false);
                    }
                }
            }
            dictionary_0.Clear();
            foreach (uint num2 in Database.Keys)
            {
                dictionary_0.Add(num2, Database[num2].Hash);
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
            string str = File.ReadAllText(SaveFilePath);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            string[] strArray = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string str2 = null;
            Version version = null;
            ClanData data = null;
            foreach (string str3 in strArray)
            {
                UserData bySteamID;
                int num3;
                Predicate<ClanLevel> match = null;
                Class0 class2 = new Class0();
                if (str3.StartsWith("[") && str3.EndsWith("]"))
                {
                    data = null;
                    if ((version != null) && (str2 != null))
                    {
                        uint key = str3.Substring(1, str3.Length - 2).ToUInt32();
                        if (key != 0)
                        {
                            if (Database.ContainsKey(key))
                            {
                                data = Database[key];
                            }
                            else
                            {
                                key = Helper.NewSerial;
                                DateTime created = new DateTime();
                                data = new ClanData(key, null, null, 0L, created);
                                Database.Add(key, data);
                                Loaded++;
                            }
                        }
                    }
                }
                else
                {
                    class2.string_0 = str3.Split(new char[] { '=' });
                    if (class2.string_0.Length >= 2)
                    {
                        if (data == null)
                        {
                            if (class2.string_0[0].Equals("VERSION", StringComparison.OrdinalIgnoreCase))
                            {
                                version = new Version(class2.string_0[1]);
                            }
                            if (class2.string_0[0].Equals("TITLE", StringComparison.OrdinalIgnoreCase))
                            {
                                str2 = class2.string_0[1];
                            }
                            if (class2.string_0[0].Equals("TIME", StringComparison.OrdinalIgnoreCase))
                            {
                                Convert.ToUInt32(class2.string_0[1]);
                            }
                        }
                        else
                        {
                            switch (class2.string_0[0].ToUpper())
                            {
                                case "NAME":
                                    data.Name = class2.string_0[1].Trim();
                                    break;

                                case "ABBREV":
                                    data.Abbr = class2.string_0[1].Trim();
                                    break;

                                case "LEADER":
                                    data.LeaderID = ulong.Parse(class2.string_0[1]);
                                    break;

                                case "CREATED":
                                    data.Created = DateTime.Parse(class2.string_0[1]);
                                    break;

                                case "FLAGS":
                                    data.Flags = class2.string_0[1].ToEnum<ClanFlags>();
                                    break;

                                case "BALANCE":
                                    data.Balance = ulong.Parse(class2.string_0[1]);
                                    break;

                                case "TAX":
                                    data.Tax = uint.Parse(class2.string_0[1]);
                                    break;

                                case "LEVEL":
                                    if (match == null)
                                    {
                                        match = new Predicate<ClanLevel>(class2.method_0);
                                    }
                                    data.SetLevel(Levels.Find(match));
                                    break;

                                case "EXPERIENCE":
                                    data.Experience = ulong.Parse(class2.string_0[1]);
                                    break;

                                case "LOCATION":
                                    class2.string_0 = class2.string_0[1].Split(new char[] { ',' });
                                    if (class2.string_0.Length > 0)
                                    {
                                        float.TryParse(class2.string_0[0].Trim(), out data.Location.x);
                                    }
                                    if (class2.string_0.Length > 1)
                                    {
                                        float.TryParse(class2.string_0[1].Trim(), out data.Location.y);
                                    }
                                    if (class2.string_0.Length > 2)
                                    {
                                        float.TryParse(class2.string_0[2].Trim(), out data.Location.z);
                                    }
                                    break;

                                case "MOTD":
                                    data.MOTD = class2.string_0[1].Trim();
                                    break;

                                case "PENALTY":
                                    data.Penalty = DateTime.Parse(class2.string_0[1]);
                                    break;

                                case "HOSTILE":
                                    class2.string_0 = class2.string_0[1].Split(new char[] { ',' });
                                    if (class2.string_0.Length >= 2)
                                    {
                                        data.Hostile.Add(class2.string_0[0].ToUInt32(), DateTime.Parse(class2.string_0[1]));
                                    }
                                    break;

                                case "MEMBER":
                                    class2.string_0 = class2.string_0[1].Split(new char[] { ',' });
                                    bySteamID = Users.GetBySteamID(ulong.Parse(class2.string_0[0]));
                                    if (bySteamID == null)
                                    {
                                        break;
                                    }
                                    num3 = 1;
                                    goto Label_05A2;
                            }
                        }
                    }
                }
                continue;
            Label_0586:
                class2.string_0[num3 - 1] = class2.string_0[num3];
                num3++;
            Label_05A2:
                if (num3 < class2.string_0.Length)
                {
                    goto Label_0586;
                }
                Array.Resize<string>(ref class2.string_0, class2.string_0.Length - 1);
                ClanMemberFlags flags = string.Join(",", class2.string_0).ToEnum<ClanMemberFlags>();
                bySteamID.Clan = data;
                data.Members.Add(bySteamID, flags);
            }
            return true;
        }

        public static bool MemberJoin(ClanData clanData, UserData userData)
        {
            if (((clanData == null) || (userData == null)) || clanData.Members.ContainsKey(userData))
            {
                return false;
            }
            ClanMemberFlags flags = 0;
            if (clanData.LeaderID == userData.SteamID)
            {
                flags |= ClanMemberFlags.management | ClanMemberFlags.dismiss | ClanMemberFlags.invite;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_INSERT_MEMBER, userData.SteamID, clanData.ID, flags.ToString().Replace(" ", "")));
            }
            clanData.Members.Add(userData, flags);
            userData.Clan = clanData;
            NetUser player = NetUser.FindByUserID(userData.SteamID);
            if (player != null)
            {
                Broadcast.Message(player, Config.GetMessageClan("Command.Clan.PlayerJoined", clanData, null, userData), null, 0f);
            }
            return true;
        }

        public static bool MemberLeave(ClanData clanData, UserData userData)
        {
            if (((clanData == null) || (userData == null)) || (userData.Clan != clanData))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_MEMBER, userData.SteamID));
            }
            if (!clanData.Members.ContainsKey(userData))
            {
                return false;
            }
            clanData.Members.Remove(userData);
            userData.Clan = null;
            NetUser player = NetUser.FindByUserID(userData.SteamID);
            if (player != null)
            {
                Broadcast.Message(player, Config.GetMessageClan("Command.Clan.PlayerLeaved", clanData, null, userData), null, 0f);
            }
            return true;
        }

        public static void Remove(ClanData clandata)
        {
            if (clandata != null)
            {
                Remove(clandata.ID);
            }
        }

        public static void Remove(string name)
        {
            ClanData clandata = Find(name);
            if (clandata != null)
            {
                Remove(clandata);
            }
        }

        public static void Remove(uint id)
        {
            if (Database.ContainsKey(id))
            {
                if (Core.DatabaseType.Equals("MYSQL"))
                {
                    foreach (UserData data in Database[id].Members.Keys)
                    {
                        MySQL.Update(string.Format(SQL_DELETE_MEMBER, data.SteamID));
                    }
                    foreach (uint num in Database[id].Hostile.Keys)
                    {
                        MySQL.Update(string.Format(SQL_DELETE_CLAN_HOSTILE, id));
                        MySQL.Update(string.Format(SQL_DELETE_CLAN_HOSTILE, num));
                    }
                    MySQL.Update(string.Format(SQL_DELETE_CLAN, id));
                }
                Database[id].Hostile.Clear();
                Database[id].Members.Clear();
                Database.Remove(id);
            }
        }

        public static bool SaveAsDatabaseSQL()
        {
            if (!Core.DatabaseType.Equals("MYSQL"))
            {
                return false;
            }
            foreach (ClanData data in Database.Values)
            {
                SQL_Update(data, true);
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
            using (StreamWriter writer = File.CreateText(SaveFilePath + ".new"))
            {
                writer.WriteLine("TITLE=" + Core.ProductName);
                writer.WriteLine("VERSION=" + Core.Version);
                writer.WriteLine("TIME=" + ((uint) Environment.TickCount));
                writer.WriteLine();
                foreach (uint num in Database.Keys)
                {
                    writer.WriteLine("[" + num.ToHEX(true) + "]");
                    writer.WriteLine("NAME=" + Database[num].Name);
                    writer.WriteLine("ABBREV=" + Database[num].Abbr);
                    writer.WriteLine("LEADER=" + Database[num].LeaderID);
                    writer.WriteLine("CREATED=" + Database[num].Created.ToString("MM/dd/yyyy HH:mm:ss"));
                    writer.WriteLine("FLAGS=" + Database[num].Flags.ToString().Replace(" ", ""));
                    writer.WriteLine("BALANCE=" + Database[num].Balance.ToString());
                    writer.WriteLine("TAX=" + Database[num].Tax.ToString());
                    writer.WriteLine("LEVEL=" + Database[num].Level.Id);
                    writer.WriteLine("EXPERIENCE=" + Database[num].Experience);
                    writer.WriteLine(string.Concat(new object[] { "LOCATION=", Database[num].Location.x, ",", Database[num].Location.y, ",", Database[num].Location.z }));
                    writer.WriteLine("MOTD=" + Database[num].MOTD);
                    writer.WriteLine("PENALTY=" + Database[num].Penalty.ToString("MM/dd/yyyy HH:mm:ss"));
                    if (Database[num].Hostile.Count > 0)
                    {
                        foreach (uint num2 in Database[num].Hostile.Keys)
                        {
                            writer.WriteLine("HOSTILE=" + num2.ToHEX(true) + "," + Database[num].Hostile[num2].ToString("MM/dd/yyyy HH:mm:ss"));
                        }
                    }
                    foreach (UserData data in Database[num].Members.Keys)
                    {
                        writer.WriteLine(string.Concat(new object[] { "MEMBER=", data.SteamID, ",", Database[num].Members[data].ToString().Replace(" ", "") }));
                    }
                    writer.WriteLine();
                }
            }
            Helper.CreateFileBackup(SaveFilePath);
            File.Move(SaveFilePath + ".new", SaveFilePath);
            return Database.Count;
        }

        public static void SQL_SynchronizeClans()
        {
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Result result = MySQL.Query("SELECT * FROM `db_clans`;", true);
                if (result != null)
                {
                    using (System.Collections.Generic.List<MySQL.Row>.Enumerator enumerator = result.Row.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Predicate<ClanLevel> match = null;
                            Class3 class2 = new Class3 {
                                row_0 = enumerator.Current
                            };
                            ClanData clandata = null;
                            uint asUInt = class2.row_0.Get("id").AsUInt;
                            if (Database.ContainsKey(asUInt))
                            {
                                clandata = Get(asUInt);
                            }
                            else
                            {
                                DateTime created = new DateTime();
                                Database.Add(asUInt, clandata = new ClanData(asUInt, null, null, 0L, created));
                            }
                            if (!dictionary_0.ContainsKey(asUInt))
                            {
                                dictionary_0.Add(asUInt, 0L);
                            }
                            if (Database[clandata.ID].Hash == dictionary_0[clandata.ID])
                            {
                                clandata.Name = class2.row_0.Get("name").AsString;
                                clandata.Abbr = class2.row_0.Get("abbrev").AsString;
                                clandata.LeaderID = class2.row_0.Get("leader_id").AsUInt64;
                                clandata.Created = class2.row_0.Get("created").AsDateTime;
                                clandata.Flags = class2.row_0.Get("flags").AsEnum<ClanFlags>();
                                clandata.Balance = class2.row_0.Get("balance").AsUInt64;
                                clandata.Tax = class2.row_0.Get("tax").AsUInt;
                                if (match == null)
                                {
                                    match = new Predicate<ClanLevel>(class2.method_0);
                                }
                                clandata.SetLevel(Levels.Find(match));
                                clandata.Experience = class2.row_0.Get("experience").AsUInt64;
                                string[] strArray = class2.row_0.Get("location").AsString.Replace(", ", ",").Split(new char[] { ',' });
                                if (strArray.Length > 0)
                                {
                                    float.TryParse(strArray[0].Trim(), out clandata.Location.x);
                                }
                                if (strArray.Length > 1)
                                {
                                    float.TryParse(strArray[1].Trim(), out clandata.Location.y);
                                }
                                if (strArray.Length > 2)
                                {
                                    float.TryParse(strArray[2].Trim(), out clandata.Location.z);
                                }
                                clandata.MOTD = class2.row_0.Get("motd").AsString;
                                clandata.Penalty = class2.row_0.Get("penalty").AsDateTime;
                            }
                            else
                            {
                                SQL_Update(clandata, false);
                            }
                            dictionary_0[asUInt] = Database[asUInt].Hash;
                        }
                    }
                }
            }
        }

        public static void SQL_Update(ClanData clandata, [Optional, DefaultParameterValue(false)] bool updateHostile)
        {
            MySQL.Update(string.Format(SQL_INSERT_CLAN, new object[] { clandata.ID, clandata.Created.ToString("yyyy-MM-dd HH:mm:ss"), clandata.Name, clandata.Abbr, clandata.LeaderID, clandata.Flags.ToString().Replace(" ", ""), clandata.Balance, clandata.Tax, clandata.Level.Id, clandata.Experience, string.Concat(new object[] { clandata.Location.x, ",", clandata.Location.y, ",", clandata.Location.z }), clandata.MOTD, clandata.Penalty.ToString("yyyy-MM-dd HH:mm:ss") }));
            foreach (UserData data in clandata.Members.Keys)
            {
                MySQL.Update(string.Format(SQL_INSERT_MEMBER, data.SteamID, clandata.ID, clandata.Members[data].ToString().Replace(" ", "")));
            }
            if (updateHostile)
            {
                foreach (uint num in clandata.Hostile.Keys)
                {
                    DateTime time = clandata.Hostile[num];
                    MySQL.Update(string.Format(SQL_INSERT_CLAN_HOSTILE, clandata.ID, num, time.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }
            if (!dictionary_0.ContainsKey(clandata.ID))
            {
                dictionary_0.Add(clandata.ID, 0L);
            }
            dictionary_0[clandata.ID] = Database[clandata.ID].Hash;
        }

        public static void TransferAccept(ClanData clanData, UserData userData)
        {
            NetUser user = NetUser.FindByUserID(clanData.LeaderID);
            if (user != null)
            {
                Broadcast.MessageClan(user, clanData, Config.GetMessageClan("Command.Clan.Transfer.QueryAnswerY", clanData, null, userData));
            }
            clanData.LeaderID = userData.SteamID;
            clanData.Members[userData] = ClanMemberFlags.management | ClanMemberFlags.dismiss | ClanMemberFlags.invite;
            Broadcast.MessageClan(clanData, Config.GetMessageClan("Command.Clan.Transfer.Success", clanData, null, userData));
        }

        public static void TransferDecline(ClanData clanData, UserData userData)
        {
            NetUser user = NetUser.FindByUserID(clanData.LeaderID);
            if (user != null)
            {
                Broadcast.MessageClan(user, clanData, Config.GetMessageClan("Command.Clan.Transfer.QueryAnswerN", clanData, null, userData));
            }
        }

        public static ClanData[] All
        {
            get
            {
                return ((ICollection<ClanData>) Database.Values).ToArray<ClanData>();
            }
        }

        public static int Count
        {
            get
            {
                if (Database == null)
                {
                    return 0;
                }
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

        [CompilerGenerated]
        private sealed class Class0
        {
            public string[] string_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.Id == uint.Parse(this.string_0[1]));
            }
        }

        [CompilerGenerated]
        private sealed class Class1
        {
            public MySQL.Row row_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.Id == this.row_0.Get("level").AsUInt);
            }
        }

        [CompilerGenerated]
        private sealed class Class2
        {
            public Clans.Class1 class1_0;
            public uint uint_0;

            public bool method_0(MySQL.Row row_0)
            {
                return (row_0.Get("id").AsUInt == this.uint_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class3
        {
            public MySQL.Row row_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.Id == this.row_0.Get("level").AsUInt);
            }
        }
    }
}

