namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public class Users
    {
        public static int AutoAdminRank = 3;
        public static Dictionary<ulong, RustProto.Avatar> Avatar;
        public static bool BindingNames = true;
        [CompilerGenerated]
        private static bool bool_0;
        public static Dictionary<ulong, System.Collections.Generic.List<RustExtended.Countdown>> Countdown;
        public static Dictionary<ulong, UserData> Database;
        public static int DefaultRank = 0;
        private static Dictionary<ulong, ulong> dictionary_0;
        public static bool DisplayRank = true;
        [CompilerGenerated]
        private static int int_0;
        public static bool MD5Password = true;
        public static float NetworkTimeout = 0f;
        public static Dictionary<ulong, Dictionary<string, int>> Personal;
        public static int PingLimit = 0;
        [CompilerGenerated]
        private static Predicate<RustExtended.Countdown> predicate_0;
        [CompilerGenerated]
        private static Predicate<RustExtended.Countdown> predicate_1;
        public static int PremiumRank = 1;
        public static Dictionary<ulong, System.Collections.Generic.List<ulong>> Shared;
        public static string SQL_CLEAR_USER_BANLIST = "TRUNCATE `db_users_banned`;";
        public static string SQL_CLEAR_USER_COUNTDOWNS = "DELETE FROM `db_users_countdown` WHERE `user_id`={0};";
        public static string SQL_CLEAR_USER_PERSONALS = "DELETE FROM `db_users_personal` WHERE `user_id`={0};";
        public static string SQL_CLEAR_USER_SHAREDS = "DELETE FROM `db_users_shared` WHERE `owner_id`={0};";
        public static string SQL_DELETE_USER_BANNED = "DELETE FROM `db_users_banned` WHERE `steam_id`={0};";
        public static string SQL_DELETE_USER_COUNTDOWN = "DELETE FROM `db_users_countdown` WHERE `user_id`={0} AND `command`='{1}';";
        public static string SQL_DELETE_USER_DATA = "DELETE FROM `db_users` WHERE `steam_id`={0};";
        public static string SQL_DELETE_USER_PERSONAL = "DELETE FROM `db_users_personal` WHERE `user_id`={0} AND `item_name`='{1}';";
        public static string SQL_DELETE_USER_SHARED = "DELETE FROM `db_users_shared` WHERE `owner_id`={0} AND `user_id`={1};";
        public static string SQL_INSERT_USER_BANNED = "REPLACE INTO `db_users_banned` (`steam_id`, `ip_address`, `date`, `period`, `reason`, `details`) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5});";
        public static string SQL_INSERT_USER_COUNTDOWN = "REPLACE INTO `db_users_countdown` (`user_id`, `command`, `expires`) VALUES ({0}, '{1}', {2});";
        public static string SQL_INSERT_USER_DATA = "REPLACE INTO `db_users` (`steam_id`, `username`, `password`, `comments`, `rank`, `flags`, `language`, `x`, `y`, `z`, `violations`, `violation_date`, `last_connect_ip`, `last_connect_date`, `first_connect_ip`, `first_connect_date`, `premium_date`) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, '{11}', '{12}', '{13}', '{14}', '{15}', '{16}');";
        public static string SQL_INSERT_USER_PERSONAL = "REPLACE INTO `db_users_personal` (`user_id`, `item_name`, `quantity`) VALUES ({0}, '{1}', {2});";
        public static string SQL_INSERT_USER_SHARED = "REPLACE INTO `db_users_shared` (`owner_id`, `user_id`) VALUES ({0}, {1});";
        public static string SQL_UPDATE_USER_DATA = "UPDATE `db_users` SET `username`={1}, `password`={2}, `comments`={3}, `rank`={4}, `flags`={5}, `language`={6}, `x`={7}, `y`={8}, `z`={9}, `violations`={10}, `violation_date`='{11}', `last_connect_ip`='{12}', `last_connect_date`='{13}', `first_connect_ip`='{14}', `first_connect_date`='{15}', `premium_date`='{16}' WHERE `steam_id`={0} LIMIT 1;";
        public static string SQL_UPDATE_USER_ELEM = "UPDATE `db_users` SET `{1}`={2} WHERE `steam_id`={0} LIMIT 1;";
        public static string SQL_UPDATE_USER_PERSONAL = "UPDATE `db_users_personal` SET `quantity`={2} WHERE `user_id`={0} AND `item_name`='{1}';";
        private static string string_0 = "rust_users.txt";
        [CompilerGenerated]
        private static string string_1;
        public static bool UniqueNames = true;
        public static string VerifyChars = "0-9a-zA-Z. _-";
        public static bool VerifyNames = true;

        public static UserData Add(ulong steam_id, string username, string password, string comments, int rank,  UserFlags flag,  string language,  string connect_ip, DateTime connect_date)
        {
            UserData data = new UserData(0L) {
                SteamID = steam_id,
                Username = username,
                Password = password,
                Comments = comments,
                Rank = rank,
                Flags = flag,
                Language = (language == "") ? Core.Languages[0] : language,
                Position = Vector3.zero,
                Violations = 0,
                ViolationDate = new DateTime(),
                LastConnectIP = connect_ip,
                LastConnectDate = connect_date,
                FirstConnectIP = connect_ip,
                FirstConnectDate = connect_date,
                PremiumDate = new DateTime()
            };
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_INSERT_USER_DATA, new object[] { 
                    data.SteamID, MySQL.QuoteString(data.Username), MySQL.QuoteString(data.Password), MySQL.QuoteString(data.Comments), data.Rank, MySQL.QuoteString(data.Flags.ToString().Replace(" ", "")), MySQL.QuoteString(data.Language), data.Position.x, data.Position.y, data.Position.z, data.Violations, data.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss"), data.LastConnectIP, data.LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"), data.FirstConnectIP, data.FirstConnectDate.ToString("yyyy-MM-dd HH:mm:ss"), 
                    data.PremiumDate.ToString("yyyy-MM-dd HH:mm:ss")
                 }));
            }
            Database.Add(data.SteamID, data);
            return data;
        }

        public static bool Ban(ulong steam_id, string reason,  DateTime period, string details)
        {
            return Banned.Add(steam_id, reason, period, details);
        }

        public static bool ChangeID(ulong steam_id, ulong new_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            string path = Core.SavePath + @"\userdata\" + new_id;
            string str2 = Core.SavePath + @"\userdata\" + steam_id;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            if (Directory.Exists(str2))
            {
                Directory.Move(str2, path);
            }
            Database.Add(new_id, Database[steam_id]);
            Database[new_id].SteamID = new_id;
            Database.Remove(steam_id);
            dictionary_0.Add(new_id, dictionary_0[steam_id]);
            dictionary_0.Remove(steam_id);
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
                foreach (ClanData data in Clans.Database.Values)
                {
                    if (data.LeaderID == steam_id)
                    {
                        data.LeaderID = new_id;
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
            foreach (StructureMaster master in StructureMaster.AllStructures)
            {
                if (master.ownerID == steam_id)
                {
                    master.creatorID = new_id;
                    master.ownerID = new_id;
                    master.CacheCreator();
                }
            }
            foreach (DeployableObject obj2 in UnityEngine.Object.FindObjectsOfType<DeployableObject>())
            {
                if (obj2.ownerID == steam_id)
                {
                    obj2.creatorID = new_id;
                    obj2.ownerID = new_id;
                    obj2.CacheCreator();
                }
            }
            return true;
        }

        public static bool CountdownAdd(ulong steam_id, RustExtended.Countdown countdown)
        {
            Class53 class2 = new Class53 {
                countdown_0 = countdown
            };
            if (!Countdown.ContainsKey(steam_id))
            {
                Countdown.Add(steam_id, new System.Collections.Generic.List<RustExtended.Countdown>());
            }
            if (Countdown[steam_id].Exists(new Predicate<RustExtended.Countdown>(class2.method_0)))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_INSERT_USER_COUNTDOWN, steam_id, class2.countdown_0.Command, class2.countdown_0.Expires ? MySQL.QuoteString(class2.countdown_0.Stamp.ToString("yyyy-MM-dd HH:mm:ss")) : "NULL"));
            }
            Countdown[steam_id].Add(class2.countdown_0);
            return true;
        }

        public static RustExtended.Countdown CountdownGet(ulong steam_id, string command)
        {
            Class52 class2 = new Class52 {
                string_0 = command
            };
            if (!Countdown.ContainsKey(steam_id))
            {
                return null;
            }
            return Countdown[steam_id].Find(new Predicate<RustExtended.Countdown>(class2.method_0));
        }

        public static System.Collections.Generic.List<RustExtended.Countdown> CountdownList(ulong steam_id)
        {
            if (!Countdown.ContainsKey(steam_id))
            {
                Countdown.Add(steam_id, new System.Collections.Generic.List<RustExtended.Countdown>());
            }
            return Countdown[steam_id];
        }

        public static bool CountdownRemove(ulong steam_id, RustExtended.Countdown countdown)
        {
            Class55 class2 = new Class55 {
                countdown_0 = countdown
            };
            if (!Countdown.ContainsKey(steam_id))
            {
                Countdown.Add(steam_id, new System.Collections.Generic.List<RustExtended.Countdown>());
            }
            if ((class2.countdown_0 == null) || !Countdown[steam_id].Exists(new Predicate<RustExtended.Countdown>(class2.method_0)))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_USER_COUNTDOWN, steam_id, class2.countdown_0.Command));
            }
            return Countdown[steam_id].Remove(class2.countdown_0);
        }

        public static bool CountdownRemove(ulong steam_id, string command)
        {
            Class54 class2 = new Class54 {
                string_0 = command
            };
            if (!Countdown.ContainsKey(steam_id))
            {
                Countdown.Add(steam_id, new System.Collections.Generic.List<RustExtended.Countdown>());
            }
            return CountdownRemove(steam_id, Countdown[steam_id].Find(new Predicate<RustExtended.Countdown>(class2.method_0)));
        }

        public static void CountdownsClear(ulong steam_id)
        {
            if (!Countdown.ContainsKey(steam_id))
            {
                Countdown.Add(steam_id, new System.Collections.Generic.List<RustExtended.Countdown>());
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_CLEAR_USER_COUNTDOWNS, steam_id));
            }
            Countdown[steam_id].Clear();
        }

        public static bool Delete(string username)
        {
            bool flag = false;
            foreach (UserData data in Database.Values)
            {
                if (data.Username.Equals(username))
                {
                    flag = Delete(data.SteamID);
                }
            }
            return flag;
        }

        public static bool Delete(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_USER_BANNED, steam_id));
                MySQL.Update(string.Format(SQL_CLEAR_USER_COUNTDOWNS, steam_id));
                MySQL.Update(string.Format(SQL_CLEAR_USER_PERSONALS, steam_id));
                MySQL.Update(string.Format(SQL_CLEAR_USER_SHAREDS, steam_id));
                MySQL.Update(string.Format(SQL_DELETE_USER_DATA, steam_id));
            }
            Economy.Delete(steam_id);
            Banned.Remove(steam_id);
            Countdown.Remove(steam_id);
            Personal.Remove(steam_id);
            Shared.Remove(steam_id);
            Database.Remove(steam_id);
            dictionary_0.Remove(steam_id);
            return true;
        }

        public static bool Details(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            return Database[steam_id].HasFlag(UserFlags.details);
        }

        public static bool Exists(ulong userid)
        {
            return Database.ContainsKey(userid);
        }

        public static UserData Find(string Value)
        {
            if ((Database != null) && (Database.Count != 0))
            {
                UserData data;
                ulong num;
                string pattern = Value.Replace("*", "");
                StringComparison comparisonType = UniqueNames ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                if (ulong.TryParse(Value, out num) && Database.TryGetValue(num, out data))
                {
                    return data;
                }
                if (Value.StartsWith("*") && Value.EndsWith("*"))
                {
                    foreach (UserData data2 in Database.Values)
                    {
                        if (data2.Username.Contains(pattern, true))
                        {
                            return data2;
                        }
                    }
                }
                if (Value.StartsWith("*"))
                {
                    foreach (UserData data3 in Database.Values)
                    {
                        if (data3.Username.EndsWith(pattern, comparisonType))
                        {
                            return data3;
                        }
                    }
                }
                if (Value.EndsWith("*"))
                {
                    foreach (UserData data4 in Database.Values)
                    {
                        if (data4.Username.StartsWith(pattern, comparisonType))
                        {
                            return data4;
                        }
                    }
                }
                foreach (UserData data5 in Database.Values)
                {
                    if (data5.Username.Equals(pattern, comparisonType))
                    {
                        return data5;
                    }
                }
            }
            return null;
        }

        public static UserData Find(ulong userid)
        {
            UserData data;
            if ((userid != 0L) && Database.TryGetValue(userid, out data))
            {
                return data;
            }
            return null;
        }

        public static UserData GetBySteamID(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id];
        }

        public static UserData GetByUserName(string username)
        {
            if ((Database != null) && (Database.Count != 0))
            {
                StringComparison comparisonType = UniqueNames ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                foreach (UserData data in Database.Values)
                {
                    if (data.Username.Equals(username, comparisonType))
                    {
                        return data;
                    }
                }
            }
            return null;
        }

        public static string GetComments(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id].Comments;
        }

        public static DateTime GetFirstConnectDate(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return new DateTime();
            }
            return Database[steam_id].FirstConnectDate;
        }

        public static string GetFirstConnectIP(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id].FirstConnectIP;
        }

        public static UserFlags GetFlags(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return UserFlags.guest;
            }
            return Database[steam_id].Flags;
        }

        public static string GetLanguage(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return Core.Languages[0];
            }
            return Database[steam_id].Language;
        }

        public static DateTime GetLastConnectDate(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return new DateTime();
            }
            return Database[steam_id].LastConnectDate;
        }

        public static string GetLastConnectIP(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id].LastConnectIP;
        }

        public static string GetPassword(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id].Password;
        }

        public static int GetRank(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return -1;
            }
            return Database[steam_id].Rank;
        }

        public static string GetUsername(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return Database[steam_id].Username;
        }

        public static bool GetUsername(ulong steam_id, ref string Username)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Username))
            {
                Username = Database[steam_id].Username;
            }
            int rank = Database[steam_id].Rank;
            if (!Core.Ranks.ContainsKey(rank))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Core.Ranks[rank]))
            {
                return false;
            }
            Username = "[" + Core.Ranks[rank] + "] " + Username;
            return true;
        }

        public static bool HasFlag(ulong steam_id, UserFlags flag)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            return Database[steam_id].HasFlag(flag);
        }

        public static ulong Hash(ulong steam_id)
        {
            if (!dictionary_0.ContainsKey(steam_id))
            {
                return 0L;
            }
            return dictionary_0[steam_id];
        }

        public static void HashUpdate(ulong steam_id)
        {
            if (Database.ContainsKey(steam_id) && dictionary_0.ContainsKey(steam_id))
            {
                dictionary_0[steam_id] = Database[steam_id].Hash;
            }
        }

        public static bool HasRank(ulong steam_id, int rank)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            return (Database[steam_id].Rank == rank);
        }

        public static void Initialize()
        {
            SaveFilePath = Path.Combine(Core.SavePath, string_0);
            Avatar = new Dictionary<ulong, RustProto.Avatar>();
            dictionary_0 = new Dictionary<ulong, ulong>();
            Database = new Dictionary<ulong, UserData>();
            Shared = new Dictionary<ulong, System.Collections.Generic.List<ulong>>();
            Personal = new Dictionary<ulong, Dictionary<string, int>>();
            Countdown = new Dictionary<ulong, System.Collections.Generic.List<RustExtended.Countdown>>();
            Initialized = false;
            if (Core.DatabaseType.Contains("FILE", true))
            {
                Initialized = LoadAsTextFile();
            }
            if (Core.DatabaseType.Contains("MYSQL", true))
            {
                Initialized = LoadAsDatabaseSQL();
            }
        }

        public static bool IsBanned(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            if (!Banned.Database.ContainsKey(steam_id))
            {
                return HasFlag(steam_id, UserFlags.banned);
            }
            return true;
        }

        public static bool IsOnline(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            return Database[steam_id].HasFlag(UserFlags.online);
        }

        public static bool LoadAsDatabaseSQL()
        {
            Loaded = 0;
            UserData data = null;
            MySQL.Result result = MySQL.Query("SELECT * FROM `db_users`;", false);
            if (result != null)
            {
                foreach (MySQL.Row row in result.Row)
                {
                    ulong key = row.Get("steam_id").AsUInt64;
                    if (!Database.ContainsKey(key) && (key != 0L))
                    {
                        data = new UserData(0L) {
                            SteamID = key,
                            Username = row.Get("username").AsString,
                            Password = row.Get("password").AsString,
                            Comments = row.Get("comments").AsString,
                            Rank = row.Get("rank").AsInt,
                            Flags = row.Get("flags").AsEnum<UserFlags>(),
                            Language = row.Get("language").AsString
                        };
                        data.Position.x = row.Get("x").AsFloat;
                        data.Position.y = row.Get("y").AsFloat;
                        data.Position.z = row.Get("z").AsFloat;
                        data.Violations = row.Get("violations").AsInt;
                        data.ViolationDate = row.Get("violation_date").AsDateTime;
                        data.LastConnectIP = row.Get("last_connect_ip").AsString;
                        data.LastConnectDate = row.Get("last_connect_date").AsDateTime;
                        data.FirstConnectIP = row.Get("first_connect_ip").AsString;
                        data.FirstConnectDate = row.Get("first_connect_date").AsDateTime;
                        data.PremiumDate = row.Get("premium_date").AsDateTime;
                        if (!Shared.ContainsKey(key))
                        {
                            Shared.Add(key, new System.Collections.Generic.List<ulong>());
                        }
                        if (!Personal.ContainsKey(key))
                        {
                            Personal.Add(key, new Dictionary<string, int>());
                        }
                        if (!Countdown.ContainsKey(key))
                        {
                            Countdown.Add(key, new System.Collections.Generic.List<RustExtended.Countdown>());
                        }
                        bool flag = false;
                        if (data.HasFlag(UserFlags.online) && (NetUser.FindByUserID(key) == null))
                        {
                            flag = true;
                            data.SetFlag(UserFlags.online, false);
                        }
                        if (data.HasFlag(UserFlags.nopvp))
                        {
                            if (predicate_1 == null)
                            {
                                predicate_1 = new Predicate<RustExtended.Countdown>(Users.smethod_1);
                            }
                            if (!Countdown[key].Exists(predicate_1))
                            {
                                flag = true;
                                data.SetFlag(UserFlags.nopvp, false);
                            }
                        }
                        if (flag)
                        {
                            SQL_Update(key, "flags", data.Flags.ToString().Replace(" ", ""));
                        }
                        Database.Add(key, data);
                        Economy.Database.Add(key, new UserEconomy(key, Economy.StartBalance));
                        Loaded++;
                    }
                }
            }
            System.Collections.Generic.List<ulong> list = new System.Collections.Generic.List<ulong>();
            foreach (ulong num2 in Database.Keys)
            {
                if ((num2.ToString().Length != 0x11) || string.IsNullOrEmpty(Database[num2].Username))
                {
                    list.Add(num2);
                }
            }
            foreach (ulong num3 in list)
            {
                Database.Remove(num3);
                Economy.Database.Remove(num3);
                Shared.Remove(num3);
                Personal.Remove(num3);
                Countdown.Remove(num3);
                MySQL.Update(string.Format(SQL_DELETE_USER_DATA, num3));
            }
            Loaded -= list.Count;
            dictionary_0.Clear();
            foreach (ulong num4 in Database.Keys)
            {
                dictionary_0.Add(num4, Database[num4].Hash);
            }
            MySQL.Result result2 = MySQL.Query("SELECT * FROM `db_users_economy`;", false);
            if (result2 != null)
            {
                foreach (MySQL.Row row2 in result2.Row)
                {
                    ulong num5 = row2.Get("user_id").AsUInt64;
                    if (Database.ContainsKey(num5))
                    {
                        Economy.Database[num5].Balance = row2.Get("balance").AsUInt64;
                        Economy.Database[num5].PlayersKilled = row2.Get("killed_players").AsInt;
                        Economy.Database[num5].MutantsKilled = row2.Get("killed_mutants").AsInt;
                        Economy.Database[num5].AnimalsKilled = row2.Get("killed_animals").AsInt;
                        Economy.Database[num5].Deaths = row2.Get("deaths").AsInt;
                    }
                }
            }
            Economy.Hashdata.Clear();
            foreach (ulong num6 in Economy.Database.Keys)
            {
                Economy.Hashdata.Add(num6, Economy.Database[num6].Hash);
            }
            MySQL.Result result3 = MySQL.Query("SELECT * FROM `db_users_shared`;", false);
            if (result3 != null)
            {
                foreach (MySQL.Row row3 in result3.Row)
                {
                    ulong num7 = row3.Get("owner_id").AsUInt64;
                    if (Database.ContainsKey(num7) && !Shared[num7].Contains(row3.Get("user_id").AsUInt64))
                    {
                        Shared[num7].Add(row3.Get("user_id").AsUInt64);
                    }
                }
            }
            MySQL.Result result4 = MySQL.Query("SELECT * FROM `db_users_personal`;", false);
            if (result4 != null)
            {
                foreach (MySQL.Row row4 in result4.Row)
                {
                    ulong num8 = row4.Get("user_id").AsUInt64;
                    if (Database.ContainsKey(num8) && !Personal[num8].ContainsKey(row4.Get("item_name").AsString))
                    {
                        Personal[num8].Add(row4.Get("item_name").AsString, row4.Get("quantity").AsInt);
                    }
                }
            }
            MySQL.Result result5 = MySQL.Query("SELECT * FROM `db_users_countdown`;", false);
            if (result5 != null)
            {
                using (System.Collections.Generic.List<MySQL.Row>.Enumerator enumerator9 = result5.Row.GetEnumerator())
                {
                    while (enumerator9.MoveNext())
                    {
                        Predicate<RustExtended.Countdown> match = null;
                        Class51 class2 = new Class51 {
                            row_0 = enumerator9.Current
                        };
                        ulong num9 = class2.row_0.Get("user_id").AsUInt64;
                        if (Database.ContainsKey(num9))
                        {
                            if (match == null)
                            {
                                match = new Predicate<RustExtended.Countdown>(class2.method_0);
                            }
                            if (!Countdown[num9].Exists(match))
                            {
                                if (class2.row_0.Get("expires").IsNull)
                                {
                                    Countdown[num9].Add(new RustExtended.Countdown(class2.row_0.Get("command").AsString, 0.0));
                                }
                                else
                                {
                                    Countdown[num9].Add(new RustExtended.Countdown(class2.row_0.Get("command").AsString, class2.row_0.Get("expires").AsDateTime));
                                }
                                RustExtended.Countdown countdown = Countdown[num9].Last<RustExtended.Countdown>();
                                if (countdown.Expired)
                                {
                                    CountdownRemove(num9, countdown);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static bool LoadAsTextFile()
        {
            Loaded = 0;
            if (File.Exists(SaveFilePath))
            {
                string str = File.ReadAllText(SaveFilePath);
                if (string.IsNullOrEmpty(str))
                {
                    return true;
                }
                string[] strArray = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string str2 = null;
                Version version = null;
                UserData data = null;
                UserEconomy economy = null;
                foreach (string str3 in strArray)
                {
                    Predicate<RustExtended.Countdown> match = null;
                    Class50 class2 = new Class50();
                    if (str3.StartsWith("[") && str3.EndsWith("]"))
                    {
                        if ((version != null) && (str2 != null))
                        {
                            data = null;
                            ulong result = 0L;
                            if (ulong.TryParse(str3.Substring(1, str3.Length - 2), out result))
                            {
                                if (Database.ContainsKey(result))
                                {
                                    data = Database[result];
                                }
                                else
                                {
                                    data = new UserData(0L) {
                                        SteamID = result
                                    };
                                    Database.Add(result, data);
                                    Loaded++;
                                }
                                if (Economy.Database.ContainsKey(result))
                                {
                                    economy = Economy.Database[result];
                                }
                                else
                                {
                                    economy = new UserEconomy(result, Economy.StartBalance);
                                    Economy.Database.Add(result, economy);
                                }
                                if (!Shared.ContainsKey(data.SteamID))
                                {
                                    Shared.Add(data.SteamID, new System.Collections.Generic.List<ulong>());
                                }
                                if (!Personal.ContainsKey(data.SteamID))
                                {
                                    Personal.Add(data.SteamID, new Dictionary<string, int>());
                                }
                                if (!Countdown.ContainsKey(data.SteamID))
                                {
                                    Countdown.Add(data.SteamID, new System.Collections.Generic.List<RustExtended.Countdown>());
                                }
                            }
                        }
                    }
                    else
                    {
                        class2.string_0 = str3.Split(new char[] { '=' });
                        if (class2.string_0.Length >= 2)
                        {
                            class2.string_0[1] = class2.string_0[1].Trim();
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
                            else if ((!string.IsNullOrEmpty(class2.string_0[1]) && (version != null)) && (str2 != null))
                            {
                                switch (class2.string_0[0].ToUpper())
                                {
                                    case "USERNAME":
                                        data.Username = class2.string_0[1];
                                        break;

                                    case "PASSWORD":
                                        data.Password = class2.string_0[1];
                                        break;

                                    case "COMMENTS":
                                        data.Comments = class2.string_0[1];
                                        break;

                                    case "RANK":
                                        data.Rank = Convert.ToInt32(class2.string_0[1]);
                                        break;

                                    case "FLAGS":
                                        data.Flags = class2.string_0[1].ToEnum<UserFlags>();
                                        break;

                                    case "LANGUAGE":
                                        data.Language = class2.string_0[1];
                                        break;

                                    case "POSITION":
                                        goto Label_04F7;

                                    case "VIOLATIONS":
                                        data.Violations = Convert.ToInt32(class2.string_0[1]);
                                        break;

                                    case "VIOLATIONDATE":
                                        data.ViolationDate = DateTime.Parse(class2.string_0[1]);
                                        break;

                                    case "LASTCONNECTIP":
                                        data.LastConnectIP = class2.string_0[1];
                                        break;

                                    case "LASTCONNECTDATE":
                                        data.LastConnectDate = DateTime.Parse(class2.string_0[1]);
                                        break;

                                    case "FIRSTCONNECTIP":
                                        data.FirstConnectIP = class2.string_0[1];
                                        break;

                                    case "FIRSTCONNECTDATE":
                                        data.FirstConnectDate = DateTime.Parse(class2.string_0[1]);
                                        break;

                                    case "PREMIUMDATE":
                                        data.PremiumDate = DateTime.Parse(class2.string_0[1]);
                                        break;

                                    case "SHARED":
                                    {
                                        ulong item = Convert.ToUInt64(class2.string_0[1]);
                                        if (!Shared[data.SteamID].Contains(item))
                                        {
                                            Shared[data.SteamID].Add(item);
                                        }
                                        break;
                                    }
                                    case "PERSONAL":
                                        if (class2.string_0[1].Contains(","))
                                        {
                                            class2.string_0 = class2.string_0[1].Replace(", ", ",").Split(new char[] { ',' });
                                            if (!Personal[data.SteamID].ContainsKey(class2.string_0[0]))
                                            {
                                                Personal[data.SteamID].Add(class2.string_0[0], Convert.ToInt32(class2.string_0[1]));
                                            }
                                        }
                                        break;

                                    case "COUNTDOWN":
                                        goto Label_0740;

                                    case "BALANCE":
                                        economy.Balance = Convert.ToUInt64(class2.string_0[1]);
                                        break;

                                    case "KILLEDPLAYERS":
                                        economy.PlayersKilled = Convert.ToInt32(class2.string_0[1]);
                                        break;

                                    case "KILLEDMUTANTS":
                                        economy.MutantsKilled = Convert.ToInt32(class2.string_0[1]);
                                        break;

                                    case "KILLEDANIMALS":
                                        economy.AnimalsKilled = Convert.ToInt32(class2.string_0[1]);
                                        break;

                                    case "DEATHS":
                                        economy.Deaths = Convert.ToInt32(class2.string_0[1]);
                                        break;
                                }
                            }
                        }
                    }
                    continue;
                Label_04F7:;
                    class2.string_0 = class2.string_0[1].Split(new char[] { ',' });
                    if (class2.string_0.Length > 0)
                    {
                        float.TryParse(class2.string_0[0].Trim(), out data.Position.x);
                    }
                    if (class2.string_0.Length > 1)
                    {
                        float.TryParse(class2.string_0[1].Trim(), out data.Position.y);
                    }
                    if (class2.string_0.Length > 2)
                    {
                        float.TryParse(class2.string_0[2].Trim(), out data.Position.z);
                    }
                    continue;
                Label_0740:
                    if (class2.string_0[1].Contains(","))
                    {
                        class2.string_0 = class2.string_0[1].Replace(", ", ",").Split(new char[] { ',' });
                        if (match == null)
                        {
                            match = new Predicate<RustExtended.Countdown>(class2.method_0);
                        }
                        if (!Countdown[data.SteamID].Exists(match))
                        {
                            DateTime time;
                            if (DateTime.TryParse(class2.string_0[1], out time))
                            {
                                Countdown[data.SteamID].Add(new RustExtended.Countdown(class2.string_0[0], time));
                            }
                            else
                            {
                                int num3;
                                if (int.TryParse(class2.string_0[1], out num3))
                                {
                                    Countdown[data.SteamID].Add(new RustExtended.Countdown(class2.string_0[0], (double) num3));
                                }
                            }
                            RustExtended.Countdown countdown = Countdown[data.SteamID].Last<RustExtended.Countdown>();
                            if (countdown.Expired)
                            {
                                CountdownRemove(data.SteamID, countdown);
                            }
                        }
                    }
                }
                System.Collections.Generic.List<ulong> list = new System.Collections.Generic.List<ulong>();
                foreach (ulong num4 in Database.Keys)
                {
                    if ((num4.ToString().Length != 0x11) || string.IsNullOrEmpty(Database[num4].Username))
                    {
                        list.Add(num4);
                    }
                }
                foreach (ulong num5 in list)
                {
                    Database.Remove(num5);
                    Economy.Database.Remove(num5);
                    Shared.Remove(num5);
                    Personal.Remove(num5);
                    Countdown.Remove(num5);
                }
                Loaded -= list.Count;
                foreach (ulong num6 in Database.Keys)
                {
                    if (Database[num6].HasFlag(UserFlags.online) && (NetUser.FindByUserID(num6) == null))
                    {
                        Database[num6].SetFlag(UserFlags.online, false);
                    }
                    if (Database[num6].HasFlag(UserFlags.nopvp))
                    {
                        if (predicate_0 == null)
                        {
                            predicate_0 = new Predicate<RustExtended.Countdown>(Users.smethod_0);
                        }
                        if (!Countdown[num6].Exists(predicate_0))
                        {
                            Database[num6].SetFlag(UserFlags.nopvp, false);
                        }
                    }
                }
                dictionary_0.Clear();
                foreach (ulong num7 in Database.Keys)
                {
                    dictionary_0.Add(num7, Database[num7].Hash);
                }
                Economy.Hashdata.Clear();
                foreach (ulong num8 in Economy.Database.Keys)
                {
                    Economy.Hashdata.Add(num8, Economy.Database[num8].Hash);
                }
            }
            return true;
        }

        public static string NiceName(UserData userdata, NamePrefix prefixes)
        {
            string username = userdata.Username;
            NiceName(userdata, ref username, prefixes);
            return username;
        }

        public static string NiceName(ulong steam_id, NamePrefix prefixes)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return null;
            }
            return NiceName(Database[steam_id], prefixes);
        }

        public static bool NiceName(UserData userdata, ref string username,NamePrefix prefixes)
        {
            if ((prefixes.Has<NamePrefix>((NamePrefix.None | NamePrefix.Rank)) && Core.Ranks.ContainsKey(userdata.Rank)) && !string.IsNullOrEmpty(Core.Ranks[userdata.Rank]))
            {
                username = "[" + Core.Ranks[userdata.Rank] + "] " + username;
            }
            if ((prefixes.Has<NamePrefix>(NamePrefix.Clan) && (userdata.Clan != null)) && !userdata.Clan.Abbr.IsEmpty())
            {
                username = "<" + userdata.Clan.Abbr + "> " + username;
            }
            return true;
        }

        public static bool NiceName(ulong steam_id, ref string username, NamePrefix prefixes)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            return NiceName(Database[steam_id], ref username, prefixes);
        }

        public static bool PersonalAdd(ulong steam_id, string item_name, int quantity)
        {
            if (!Personal.ContainsKey(steam_id))
            {
                Personal.Add(steam_id, new Dictionary<string, int>());
            }
            if (Personal[steam_id].ContainsKey(item_name))
            {
                Dictionary<string, int> dictionary;
                string str;
                (dictionary = Personal[steam_id])[str = item_name] = dictionary[str] + quantity;
                return true;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_INSERT_USER_PERSONAL, steam_id, item_name, quantity));
            }
            Personal[steam_id].Add(item_name, quantity);
            return true;
        }

        public static void PersonalClear(ulong steam_id)
        {
            if (!Personal.ContainsKey(steam_id))
            {
                Personal.Add(steam_id, new Dictionary<string, int>());
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_CLEAR_USER_PERSONALS, steam_id));
            }
            Personal[steam_id].Clear();
        }

        public static int PersonalGet(ulong steam_id, string item_name)
        {
            if (!Personal.ContainsKey(steam_id))
            {
                return 0;
            }
            if (!Personal[steam_id].ContainsKey(item_name))
            {
                return 0;
            }
            return Personal[steam_id][item_name];
        }

        public static Dictionary<string, int> PersonalList(ulong steam_id)
        {
            if (!Personal.ContainsKey(steam_id))
            {
                Personal.Add(steam_id, new Dictionary<string, int>());
            }
            return Personal[steam_id];
        }

        public static bool PersonalRemove(ulong steam_id, string item_name)
        {
            if (!Personal.ContainsKey(steam_id))
            {
                Personal.Add(steam_id, new Dictionary<string, int>());
            }
            if (!Personal[steam_id].ContainsKey(item_name))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_USER_PERSONAL, steam_id, item_name));
            }
            Personal[steam_id].Remove(item_name);
            return true;
        }

        public static bool SaveAsDatabaseSQL()
        {
            if (!Core.DatabaseType.Equals("MYSQL"))
            {
                return false;
            }
            foreach (UserData data in Database.Values)
            {
                MySQL.Update(string.Format(SQL_INSERT_USER_DATA, new object[] { 
                    data.SteamID, MySQL.QuoteString(data.Username), MySQL.QuoteString(data.Password), MySQL.QuoteString(data.Comments), data.Rank, MySQL.QuoteString(data.Flags.ToString().Replace(" ", "")), MySQL.QuoteString(data.Language), data.Position.x, data.Position.y, data.Position.z, data.Violations, data.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss"), data.LastConnectIP, data.LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"), data.FirstConnectIP, data.FirstConnectDate.ToString("yyyy-MM-dd HH:mm:ss"), 
                    data.PremiumDate.ToString("yyyy-MM-dd HH:mm:ss")
                 }));
                System.Collections.Generic.List<ulong> list = SharedList(data.SteamID);
                Dictionary<string, int> dictionary = PersonalList(data.SteamID);
                System.Collections.Generic.List<RustExtended.Countdown> list2 = CountdownList(data.SteamID);
                foreach (ulong num in list)
                {
                    MySQL.Update(string.Format(SQL_INSERT_USER_SHARED, data.SteamID, num));
                }
                foreach (string str in dictionary.Keys)
                {
                    MySQL.Update(string.Format(SQL_INSERT_USER_PERSONAL, data.SteamID, str, dictionary[str]));
                }
                foreach (RustExtended.Countdown countdown in list2)
                {
                    MySQL.Update(string.Format(SQL_INSERT_USER_COUNTDOWN, data.SteamID, countdown.Command, countdown.Expires ? MySQL.QuoteString(countdown.Stamp.ToString("yyyy-MM-dd HH:mm:ss")) : "NULL"));
                }
                if (Economy.Database.ContainsKey(data.SteamID))
                {
                    MySQL.Update(string.Format(Economy.SQL_INSERT_ECONOMY, new object[] { data.SteamID, Economy.Database[data.SteamID].Balance, Economy.Database[data.SteamID].PlayersKilled, Economy.Database[data.SteamID].MutantsKilled, Economy.Database[data.SteamID].AnimalsKilled, Economy.Database[data.SteamID].Deaths }));
                }
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
                foreach (UserData data in Database.Values)
                {
                    writer.WriteLine("[" + data.SteamID + "]");
                    writer.WriteLine("USERNAME=" + data.Username);
                    writer.WriteLine("PASSWORD=" + data.Password);
                    writer.WriteLine("COMMENTS=" + data.Comments);
                    writer.WriteLine("RANK=" + data.Rank);
                    writer.WriteLine("FLAGS=" + ((data.Flags & ~UserFlags.online)).ToString().Replace(", ", ","));
                    writer.WriteLine("LANGUAGE=" + data.Language);
                    foreach (ulong num in SharedList(data.SteamID))
                    {
                        writer.WriteLine("SHARED=" + num);
                    }
                    foreach (string str in PersonalList(data.SteamID).Keys)
                    {
                        writer.WriteLine(string.Concat(new object[] { "PERSONAL=", str, ",", PersonalList(data.SteamID)[str] }));
                    }
                    foreach (RustExtended.Countdown countdown in CountdownList(data.SteamID))
                    {
                        if (!countdown.Expired)
                        {
                            writer.WriteLine("COUNTDOWN=" + countdown.Command + "," + (countdown.Expires ? countdown.Stamp.ToString("MM/dd/yyyy HH:mm:ss") : "0"));
                        }
                    }
                    writer.WriteLine(string.Concat(new object[] { "POSITION=", data.Position.x, ",", data.Position.y, ",", data.Position.z }));
                    writer.WriteLine("VIOLATIONS=" + data.Violations);
                    writer.WriteLine("VIOLATIONDATE=" + data.ViolationDate.ToString("MM/dd/yyyy HH:mm:ss"));
                    writer.WriteLine("LASTCONNECTIP=" + data.LastConnectIP);
                    writer.WriteLine("LASTCONNECTDATE=" + data.LastConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
                    writer.WriteLine("FIRSTCONNECTIP=" + data.FirstConnectIP);
                    writer.WriteLine("FIRSTCONNECTDATE=" + data.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss"));
                    writer.WriteLine("PREMIUMDATE=" + data.PremiumDate.ToString("MM/dd/yyyy HH:mm:ss"));
                    if (Economy.Database.ContainsKey(data.SteamID))
                    {
                        writer.WriteLine("BALANCE=" + Economy.Database[data.SteamID].Balance);
                        writer.WriteLine("KILLEDPLAYERS=" + Economy.Database[data.SteamID].PlayersKilled);
                        writer.WriteLine("KILLEDMUTANTS=" + Economy.Database[data.SteamID].MutantsKilled);
                        writer.WriteLine("KILLEDANIMALS=" + Economy.Database[data.SteamID].AnimalsKilled);
                        writer.WriteLine("DEATHS=" + Economy.Database[data.SteamID].Deaths);
                    }
                    writer.WriteLine();
                }
            }
            Helper.CreateFileBackup(SaveFilePath);
            File.Move(SaveFilePath + ".new", SaveFilePath);
            return Database.Values.Count;
        }

        public static bool SetComments(ulong steam_id, string comments)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].Comments = comments;
            return true;
        }

        public static bool SetFirstConnectDate(ulong steam_id, DateTime date)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].FirstConnectDate = date;
            return true;
        }

        public static bool SetFirstConnectIP(ulong steam_id, string ipAddress)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].FirstConnectIP = ipAddress;
            return true;
        }

        public static bool SetFlags(ulong steam_id, UserFlags flag, [Optional, DefaultParameterValue(true)] bool state)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].SetFlag(flag, state);
            return true;
        }

        public static bool SetLastConnectDate(ulong steam_id, DateTime date)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].LastConnectDate = date;
            return true;
        }

        public static bool SetLastConnectIP(ulong steam_id, string ipAddress)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].LastConnectIP = ipAddress;
            return true;
        }

        public static bool SetPassword(ulong steam_id, string password)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].Password = password;
            return true;
        }

        public static bool SetPremiumDate(ulong steam_id, DateTime date)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].PremiumDate = date;
            return true;
        }

        public static bool SetRank(ulong steam_id, int rank)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].Rank = rank;
            return true;
        }

        public static bool SetUsername(ulong steam_id, string username)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].Username = username;
            return true;
        }

        public static bool SetViolations(ulong steam_id, int violations)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].Violations = violations;
            return true;
        }

        public static bool SharedAdd(ulong steam_id, ulong user_id)
        {
            if (!Shared.ContainsKey(steam_id))
            {
                Shared.Add(steam_id, new System.Collections.Generic.List<ulong>());
            }
            if (Shared[steam_id].Contains(user_id))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_INSERT_USER_SHARED, steam_id, user_id));
            }
            Shared[steam_id].Add(user_id);
            return true;
        }

        public static void SharedClear(ulong steam_id)
        {
            if (!Shared.ContainsKey(steam_id))
            {
                Shared.Add(steam_id, new System.Collections.Generic.List<ulong>());
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_CLEAR_USER_SHAREDS, steam_id));
            }
            Shared[steam_id].Clear();
        }

        public static bool SharedGet(ulong steam_id, ulong user_id)
        {
            if (!Shared.ContainsKey(steam_id))
            {
                return false;
            }
            return Shared[steam_id].Contains(user_id);
        }

        public static System.Collections.Generic.List<ulong> SharedList(ulong steam_id)
        {
            if (!Shared.ContainsKey(steam_id))
            {
                Shared.Add(steam_id, new System.Collections.Generic.List<ulong>());
            }
            return Shared[steam_id];
        }

        public static bool SharedRemove(ulong steam_id, ulong user_id)
        {
            if (!Shared.ContainsKey(steam_id))
            {
                Shared.Add(steam_id, new System.Collections.Generic.List<ulong>());
            }
            if (!Shared[steam_id].Contains(user_id))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_USER_SHARED, steam_id, user_id));
            }
            Shared[steam_id].Remove(user_id);
            return true;
        }

        [CompilerGenerated]
        private static bool smethod_0(RustExtended.Countdown countdown_0)
        {
            return (countdown_0.Command == "pvp");
        }

        [CompilerGenerated]
        private static bool smethod_1(RustExtended.Countdown countdown_0)
        {
            return (countdown_0.Command == "pvp");
        }

        public static ulong SQL_SynchronizeUsers()
        {
            ulong num = 0L;
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Result result = MySQL.Query("SELECT * FROM `db_users`;", true);
                if (result != null)
                {
                    foreach (MySQL.Row row in result.Row)
                    {
                        ulong key = row.Get("steam_id").AsUInt64;
                        if (!dictionary_0.ContainsKey(key))
                        {
                            dictionary_0.Add(key, 0L);
                        }
                        if (!Database.ContainsKey(key))
                        {
                            UserData data = new UserData(0L) {
                                SteamID = key,
                                Username = row.Get("username").AsString,
                                Password = row.Get("password").AsString,
                                Comments = row.Get("comments").AsString,
                                Rank = row.Get("rank").AsInt,
                                Flags = row.Get("flags").AsString.ToEnum<UserFlags>(),
                                Language = row.Get("language").AsString
                            };
                            data.Position.x = row.Get("x").AsFloat;
                            data.Position.y = row.Get("y").AsFloat;
                            data.Position.z = row.Get("z").AsFloat;
                            data.Violations = row.Get("violations").AsInt;
                            data.ViolationDate = row.Get("violation_date").AsDateTime;
                            data.LastConnectIP = row.Get("last_connect_ip").AsString;
                            data.LastConnectDate = row.Get("last_connect_date").AsDateTime;
                            data.FirstConnectIP = row.Get("first_connect_ip").AsString;
                            data.FirstConnectDate = row.Get("first_connect_date").AsDateTime;
                            data.PremiumDate = row.Get("premium_date").AsDateTime;
                            Database.Add(key, data);
                        }
                        else if (Database[key].Hash != dictionary_0[key])
                        {
                            SQL_Update(key);
                        }
                        else
                        {
                            Database[key].Username = row.Get("username").AsString;
                            Database[key].Password = row.Get("password").AsString;
                            Database[key].Comments = row.Get("comments").AsString;
                            Database[key].Rank = row.Get("rank").AsInt;
                            Database[key].Flags = row.Get("flags").AsString.ToEnum<UserFlags>();
                            Database[key].Language = row.Get("language").AsString;
                            Database[key].Position.x = row.Get("x").AsFloat;
                            Database[key].Position.y = row.Get("y").AsFloat;
                            Database[key].Position.z = row.Get("z").AsFloat;
                            Database[key].Violations = row.Get("violations").AsInt;
                            Database[key].ViolationDate = row.Get("violation_date").AsDateTime;
                            Database[key].LastConnectIP = row.Get("last_connect_ip").AsString;
                            Database[key].LastConnectDate = row.Get("last_connect_date").AsDateTime;
                            Database[key].FirstConnectIP = row.Get("first_connect_ip").AsString;
                            Database[key].FirstConnectDate = row.Get("first_connect_date").AsDateTime;
                            Database[key].PremiumDate = row.Get("premium_date").AsDateTime;
                        }
                        dictionary_0[key] = Database[key].Hash;
                    }
                }
                MySQL.Result result2 = MySQL.Query("SELECT * FROM `db_users_economy`;", true);
                if (result2 == null)
                {
                    return num;
                }
                foreach (MySQL.Row row2 in result2.Row)
                {
                    ulong num3 = row2.Get("user_id").AsUInt64;
                    if (!Economy.Hashdata.ContainsKey(num3))
                    {
                        Economy.Hashdata.Add(num3, 0L);
                    }
                    if (!Economy.Database.ContainsKey(num3))
                    {
                        UserEconomy economy = new UserEconomy(num3, Economy.StartBalance) {
                            Balance = row2.Get("balance").AsUInt64,
                            PlayersKilled = row2.Get("killed_players").AsInt,
                            MutantsKilled = row2.Get("killed_mutants").AsInt,
                            AnimalsKilled = row2.Get("killed_animals").AsInt,
                            Deaths = row2.Get("deaths").AsInt
                        };
                        Economy.Database.Add(num3, economy);
                    }
                    else if (Economy.Database[num3].Hash != Economy.Hashdata[num3])
                    {
                        Economy.SQL_Update(num3);
                    }
                    else
                    {
                        Economy.Database[num3].Balance = row2.Get("balance").AsUInt64;
                        Economy.Database[num3].PlayersKilled = row2.Get("killed_players").AsInt;
                        Economy.Database[num3].MutantsKilled = row2.Get("killed_mutants").AsInt;
                        Economy.Database[num3].AnimalsKilled = row2.Get("killed_animals").AsInt;
                        Economy.Database[num3].Deaths = row2.Get("deaths").AsInt;
                    }
                    Economy.Hashdata[num3] = Economy.Database[num3].Hash;
                }
            }
            return num;
        }

        public static void SQL_Update(ulong user_id)
        {
            if (Core.DatabaseType.Equals("MYSQL") && Database.ContainsKey(user_id))
            {
                MySQL.Update(string.Format(SQL_UPDATE_USER_DATA, new object[] { 
                    Database[user_id].SteamID, MySQL.QuoteString(Database[user_id].Username), MySQL.QuoteString(Database[user_id].Password), MySQL.QuoteString(Database[user_id].Comments), Database[user_id].Rank, MySQL.QuoteString(Database[user_id].Flags.ToString().Replace(" ", "")), MySQL.QuoteString(Database[user_id].Language), Database[user_id].Position.x, Database[user_id].Position.y, Database[user_id].Position.z, Database[user_id].Violations, Database[user_id].ViolationDate.ToString("yyyy-MM-dd HH:mm:ss"), Database[user_id].LastConnectIP, Database[user_id].LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"), Database[user_id].FirstConnectIP, Database[user_id].FirstConnectDate.ToString("yyyy-MM-dd HH:mm:ss"), 
                    Database[user_id].PremiumDate.ToString("yyyy-MM-dd HH:mm:ss")
                 }));
            }
        }

        public static void SQL_Update(ulong user_id, string key, object value)
        {
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                string str = null;
                if (value is int)
                {
                    str = ((int) value).ToString();
                }
                if (value is long)
                {
                    str = ((long) value).ToString();
                }
                if (value is uint)
                {
                    str = ((uint) value).ToString();
                }
                if (value is ulong)
                {
                    str = ((ulong) value).ToString();
                }
                if (value is float)
                {
                    str = ((float) value).ToString();
                }
                if (value is double)
                {
                    str = ((double) value).ToString();
                }
                if (value is string)
                {
                    str = MySQL.QuoteString((string) value);
                }
                if (value is bool)
                {
                    str = MySQL.QuoteString(((bool) value) ? "Yes" : "No");
                }
                if (value is DateTime)
                {
                    str = MySQL.QuoteString(((DateTime) value).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (value is UserFlags)
                {
                    str = MySQL.QuoteString(((UserFlags) value).ToString().Replace(" ", ""));
                }
                if (str != null)
                {
                    MySQL.Update(string.Format(SQL_UPDATE_USER_ELEM, user_id, key, str));
                }
            }
        }

        public static bool SQL_UpdatePersonal(ulong user_id)
        {
            if (!Database.ContainsKey(user_id) || !Core.DatabaseType.Equals("MYSQL"))
            {
                return false;
            }
            UserData data = Database[user_id];
            foreach (string str in PersonalList(data.SteamID).Keys)
            {
                MySQL.Update(string.Format(SQL_UPDATE_USER_PERSONAL, user_id, str, PersonalList(data.SteamID)[str]));
            }
            return true;
        }

        public static bool ToggleFlag(ulong steam_id, UserFlags flag)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            Database[steam_id].ToggleFlag(flag);
            return true;
        }

        public static bool Unban(ulong steam_id)
        {
            return Banned.Remove(steam_id);
        }

        public static System.Collections.Generic.List<UserData> All
        {
            get
            {
                return Database.Values.ToList<UserData>();
            }
        }

        public static int Count
        {
            get
            {
                return Database.Keys.Count;
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
        private sealed class Class50
        {
            public string[] string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0[0]);
            }
        }

        [CompilerGenerated]
        private sealed class Class51
        {
            public MySQL.Row row_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.row_0.Get("command").AsString);
            }
        }

        [CompilerGenerated]
        private sealed class Class52
        {
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class53
        {
            public Countdown countdown_0;

            public bool method_0(Countdown countdown_1)
            {
                return (countdown_1.Command == this.countdown_0.Command);
            }
        }

        [CompilerGenerated]
        private sealed class Class54
        {
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class55
        {
            public Countdown countdown_0;

            public bool method_0(Countdown countdown_1)
            {
                return (countdown_1.Command == this.countdown_0.Command);
            }
        }
    }
}

