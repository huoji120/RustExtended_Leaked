namespace RustExtended
{
    using Facepunch.Utility;
    using Rust.Steam;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Config
    {
        [CompilerGenerated]
        private static bool bool_0;
        [CompilerGenerated]
        private static bool bool_1;
        private static Config config_0 = new Config();
        [CompilerGenerated]
        private static Dictionary<string, System.Collections.Generic.List<string>> dictionary_0;
        private static System.Collections.Generic.List<Struct0> list_0 = new System.Collections.Generic.List<Struct0>();
        [CompilerGenerated]
        private static string string_0;

        private Config()
        {
            Loading = false;
            Initialized = false;
        }

        private static void Add(string string_1, string string_2, string string_3)
        {
            Class15 class2 = new Class15 {
                string_0 = string_1,
                string_1 = string_2
            };
            Struct0 item = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (item.Equals(new Struct0()))
            {
                item.string_0 = class2.string_0;
                item.string_1 = class2.string_1;
                item.list_0 = new System.Collections.Generic.List<string>();
                list_0.Add(item);
            }
            item.list_0.Add(string_3);
        }

        public static bool Get(string section, string key, ref int[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class17 class2 = new Class17 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            string[] strArray = struct2.list_0[0].Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                return false;
            }
            Array.Resize<int>(ref result, strArray.Length);
            for (int i = 0; i < result.Length; i++)
            {
                int.TryParse(strArray[i], out result[i]);
            }
            return true;
        }

        public static bool Get(string section, string key, ref long[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class21 class2 = new Class21 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            string[] strArray = struct2.list_0[0].Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                return false;
            }
            Array.Resize<long>(ref result, strArray.Length);
            for (int i = 0; i < result.Length; i++)
            {
                long.TryParse(strArray[i], out result[i]);
            }
            return true;
        }

        public static bool Get(string section, string key, ref float[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class25 class2 = new Class25 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            float num = 0f;
            string[] strArray = struct2.list_0[0].Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < result.Length; i++)
            {
                if (strArray.Length > i)
                {
                    num = float.Parse(strArray[i].Trim());
                }
                result[i] = num;
            }
            return true;
        }

        public static bool Get(string section, string key, ref uint[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class19 class2 = new Class19 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            string[] strArray = struct2.list_0[0].Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                return false;
            }
            Array.Resize<uint>(ref result, strArray.Length);
            for (int i = 0; i < result.Length; i++)
            {
                uint.TryParse(strArray[i], out result[i]);
            }
            return true;
        }

        public static bool Get(string section, string key, ref ulong[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class23 class2 = new Class23 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            string[] strArray = struct2.list_0[0].Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                return false;
            }
            Array.Resize<ulong>(ref result, strArray.Length);
            for (int i = 0; i < result.Length; i++)
            {
                ulong.TryParse(strArray[i], out result[i]);
            }
            return true;
        }

        public static bool Get(string section, string key, ref bool result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            bool flag;
            Class28 class2 = new Class28 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!bool.TryParse(struct2.list_0[0], out flag))
            {
                return false;
            }
            result = flag;
            return true;
        }

        public static bool Get(string section, string key, ref double result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            double num;
            Class26 class2 = new Class26 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!double.TryParse(struct2.list_0[0], out num))
            {
                return false;
            }
            result = num;
            return true;
        }

        public static bool Get(string section, string key, ref int result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            int num;
            Class16 class2 = new Class16 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!int.TryParse(struct2.list_0[0], out num))
            {
                return false;
            }
            result = num;
            return true;
        }

        public static bool Get(string section, string key, ref long result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            long num;
            Class20 class2 = new Class20 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!long.TryParse(struct2.list_0[0], out num))
            {
                return false;
            }
            result = num;
            return true;
        }

        public static bool Get(string section, string key, ref float result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            float num;
            Class24 class2 = new Class24 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!float.TryParse(struct2.list_0[0], out num))
            {
                return false;
            }
            result = num;
            return true;
        }

        public static bool Get(string section, string key, ref string result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class29 class2 = new Class29 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            result = struct2.list_0[0];
            return true;
        }

        public static bool Get(string section, string key, ref uint result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            uint num;
            Class18 class2 = new Class18 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!uint.TryParse(struct2.list_0[0], out num))
            {
                return false;
            }
            result = num;
            return true;
        }

        public static bool Get(string section, string key, ref ulong result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            ulong num;
            Class22 class2 = new Class22 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            if (!ulong.TryParse(struct2.list_0[0], out num))
            {
                return false;
            }
            result = num;
            return true;
        }

        public static bool Get(string section, string key, ref double[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class27 class2 = new Class27 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            double num = 0.0;
            string[] strArray = struct2.list_0[0].Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < result.Length; i++)
            {
                if (strArray.Length > i)
                {
                    num = double.Parse(strArray[i].Trim());
                }
                result[i] = num;
            }
            return true;
        }

        public static bool Get(string section, string key, ref string[] result, [Optional, DefaultParameterValue(true)] bool caseinsensitive)
        {
            Class30 class2 = new Class30 {
                string_0 = section,
                string_1 = key,
                bool_0 = caseinsensitive
            };
            Struct0 struct2 = list_0.Find(new Predicate<Struct0>(class2.method_0));
            if (struct2.Equals(new Struct0()))
            {
                return false;
            }
            result = struct2.list_0.ToArray();
            return true;
        }

        public static string GetMessage(string msg, [Optional, DefaultParameterValue(null)] NetUser User, [Optional, DefaultParameterValue(null)] string Username)
        {
            string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
            string result = msg;
            Get("MESSAGES." + str, msg, ref result, true);
            return Helper.ReplaceVariables(User, result, (Username == null) ? null : "%USERNAME%", Username);
        }

        public static string GetMessageClan(string msg, [Optional, DefaultParameterValue(null)] ClanData clan, [Optional, DefaultParameterValue(null)] NetUser netUser, [Optional, DefaultParameterValue(null)] UserData dataUser)
        {
            Class31 class2 = new Class31 {
                clanData_0 = clan
            };
            string str = (netUser == null) ? Core.Languages[0] : Users.GetLanguage(netUser.userID);
            string result = msg;
            Get("MESSAGES." + str, msg, ref result, true);
            ClanLevel level = null;
            if (class2.clanData_0 != null)
            {
                level = Clans.Levels.Find(new Predicate<ClanLevel>(class2.method_0));
            }
            UserData bySteamID = null;
            if (class2.clanData_0 != null)
            {
                bySteamID = Users.GetBySteamID(class2.clanData_0.LeaderID);
            }
            result = Helper.ReplaceVariables(netUser, result, null, "");
            if (result.Contains("%CREATE_COST%"))
            {
                result = result.Replace("%CREATE_COST%", Clans.CreateCost.ToString("N0") + Economy.CurrencySign);
            }
            if (result.Contains("%CLANS.COUNT%"))
            {
                result = result.Replace("%CLANS.COUNT%", Clans.Database.Count.ToString());
            }
            if (dataUser != null)
            {
                if (result.Contains("%STEAM_ID%"))
                {
                    result = result.Replace("%STEAM_ID%", dataUser.SteamID.ToString());
                }
                if (result.Contains("%USERNAME%"))
                {
                    result = result.Replace("%USERNAME%", dataUser.Username);
                }
            }
            if (class2.clanData_0 != null)
            {
                if (result.Contains("%CLAN.ID%"))
                {
                    result = result.Replace("%CLAN.ID%", class2.clanData_0.ID.ToString());
                }
                if (result.Contains("%CLAN.CREATED%"))
                {
                    result = result.Replace("%CLAN.CREATED%", class2.clanData_0.Created.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (result.Contains("%CLAN.NAME%"))
                {
                    result = result.Replace("%CLAN.NAME%", class2.clanData_0.Name);
                }
                if (result.Contains("%CLAN.ABBR%") && class2.clanData_0.Flags.Has<ClanFlags>(ClanFlags.can_abbr))
                {
                    result = result.Replace("%CLAN.ABBR%", class2.clanData_0.Abbr);
                }
                if (result.Contains("%CLAN.MOTD%") && class2.clanData_0.Flags.Has<ClanFlags>(ClanFlags.can_motd))
                {
                    result = result.Replace("%CLAN.MOTD%", class2.clanData_0.MOTD);
                }
                if (result.Contains("%CLAN.TAX%"))
                {
                    result = result.Replace("%CLAN.TAX%", class2.clanData_0.Tax.ToString() + "%");
                }
                if (result.Contains("%CLAN.EXPERIENCE%"))
                {
                    result = result.Replace("%CLAN.EXPERIENCE%", class2.clanData_0.Experience.ToString());
                }
                if (result.Contains("%CLAN.LOCATION%") && class2.clanData_0.Flags.Has<ClanFlags>(ClanFlags.can_warp))
                {
                    result = result.Replace("%CLAN.LOCATION%", class2.clanData_0.Location.AsString());
                }
                if (result.Contains("%CLAN.ONLINE%"))
                {
                    result = result.Replace("%CLAN.ONLINE%", class2.clanData_0.Online.ToString());
                }
                if (result.Contains("%CLAN.MEMBERS.COUNT%"))
                {
                    result = result.Replace("%CLAN.MEMBERS.COUNT%", class2.clanData_0.Members.Count.ToString());
                }
            }
            if (bySteamID != null)
            {
                if (result.Contains("%CLAN.LEADER.STEAM_ID%"))
                {
                    result = result.Replace("%CLAN.LEADER.STEAM_ID%", bySteamID.SteamID.ToString());
                }
                if (result.Contains("%CLAN.LEADER.USERNAME%"))
                {
                    result = result.Replace("%CLAN.LEADER.USERNAME%", bySteamID.Username);
                }
            }
            if ((class2.clanData_0 != null) && (class2.clanData_0.Level != null))
            {
                if (result.Contains("%CLAN.LEVEL%"))
                {
                    result = result.Replace("%CLAN.LEVEL%", class2.clanData_0.Level.Id.ToString());
                }
                if (result.Contains("%CLAN.MEMBERS.MAX%"))
                {
                    result = result.Replace("%CLAN.MEMBERS.MAX%", class2.clanData_0.Level.MaxMembers.ToString());
                }
                if (result.Contains("%CLAN.WARP_TIMEOUT%"))
                {
                    result = result.Replace("%CLAN.WARP_TIMEOUT%", class2.clanData_0.Level.WarpTimewait.ToString());
                }
                if (result.Contains("%CLAN.WARP_COUNTDOWN%"))
                {
                    result = result.Replace("%CLAN.WARP_COUNTDOWN%", class2.clanData_0.Level.WarpCountdown.ToString());
                }
                if (result.Contains("%CLAN.BONUS.CRAFTINGSPEED%") && (class2.clanData_0.Level.BonusCraftingSpeed > 0))
                {
                    result = result.Replace("%CLAN.BONUS.CRAFTINGSPEED%", class2.clanData_0.Level.BonusCraftingSpeed.ToString() + "%");
                }
                if (result.Contains("%CLAN.BONUS.GATHERINGWOOD%") && (class2.clanData_0.Level.BonusGatheringWood > 0))
                {
                    result = result.Replace("%CLAN.BONUS.GATHERINGWOOD%", class2.clanData_0.Level.BonusGatheringWood.ToString() + "%");
                }
                if (result.Contains("%CLAN.BONUS.GATHERINGROCK%") && (class2.clanData_0.Level.BonusGatheringRock > 0))
                {
                    result = result.Replace("%CLAN.BONUS.GATHERINGROCK%", class2.clanData_0.Level.BonusGatheringRock.ToString() + "%");
                }
                if (result.Contains("%CLAN.BONUS.GATHERINGANIMAL%") && (class2.clanData_0.Level.BonusGatheringAnimal > 0))
                {
                    result = result.Replace("%CLAN.BONUS.GATHERINGANIMAL%", class2.clanData_0.Level.BonusGatheringAnimal.ToString() + "%");
                }
                if (result.Contains("%CLAN.BONUS.MEMBERS_DEFENSE%") && (class2.clanData_0.Level.BonusMembersDefense > 0))
                {
                    result = result.Replace("%CLAN.BONUS.MEMBERS_DEFENSE%", class2.clanData_0.Level.BonusMembersDefense.ToString() + "%");
                }
                if (result.Contains("%CLAN.BONUS.MEMBERS_DAMAGE%") && (class2.clanData_0.Level.BonusMembersDamage > 0))
                {
                    result = result.Replace("%CLAN.BONUS.MEMBERS_DAMAGE%", class2.clanData_0.Level.BonusMembersDamage.ToString() + "%");
                }
                if (result.Contains("%CLAN.BONUS.MEMBERS_PAYMURDER%") && (class2.clanData_0.Level.BonusMembersPayMurder > 0))
                {
                    result = result.Replace("%CLAN.BONUS.MEMBERS_PAYMURDER%", class2.clanData_0.Level.BonusMembersPayMurder.ToString() + "%");
                }
            }
            if ((class2.clanData_0 != null) && (level != null))
            {
                if (result.Contains("%CLAN.NEXT_LEVEL%"))
                {
                    result = result.Replace("%CLAN.NEXT_LEVEL%", level.Id.ToString());
                }
                if (result.Contains("%CLAN.NEXT_CURRENCY%"))
                {
                    result = result.Replace("%CLAN.NEXT_CURRENCY%", level.RequireCurrency.ToString());
                }
                if (result.Contains("%CLAN.NEXT_EXPERIENCE%"))
                {
                    result = result.Replace("%CLAN.NEXT_EXPERIENCE%", level.RequireExperience.ToString());
                }
                if (result.Contains("%CLAN.NEXT_MAXMEMBERS%"))
                {
                    result = result.Replace("%CLAN.NEXT_MAXMEMBERS%", level.MaxMembers.ToString());
                }
            }
            return result;
        }

        public static string GetMessageCommand(string msg, [Optional, DefaultParameterValue("")] string command, [Optional, DefaultParameterValue(null)] NetUser User)
        {
            string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
            string result = msg;
            Get("MESSAGES." + str, msg, ref result, true);
            result = Helper.ReplaceVariables(User, result, null, "");
            if (result.Contains("%COMMAND%"))
            {
                result = result.Replace("%COMMAND%", command);
            }
            return result;
        }

        public static string GetMessageDeath(string msg, [Optional, DefaultParameterValue(null)] NetUser Victim, [Optional, DefaultParameterValue(null)] string KillerName, [Optional, DefaultParameterValue(null)] string WeaponName)
        {
            string str = (Victim == null) ? Core.Languages[0] : Users.GetLanguage(Victim.userID);
            string[] result = new string[] { msg };
            Get("MESSAGES." + str, msg, ref result, true);
            int index = UnityEngine.Random.Range(0, result.Length);
            string str2 = Helper.ReplaceVariables(Victim, result[index], null, "");
            if ((Victim != null) && str2.Contains("%VICTIM%"))
            {
                str2 = str2.Replace("%VICTIM%", Victim.displayName);
            }
            if ((KillerName != null) && str2.Contains("%KILLER%"))
            {
                str2 = str2.Replace("%KILLER%", KillerName);
            }
            if ((WeaponName != null) && str2.Contains("%WEAPON%"))
            {
                str2 = str2.Replace("%WEAPON%", WeaponName);
            }
            if (str2.Contains("%POSX%"))
            {
                str2 = str2.Replace("%POSX%", Victim.playerClient.lastKnownPosition.x.ToString());
            }
            if (str2.Contains("%POSY%"))
            {
                str2 = str2.Replace("%POSY%", Victim.playerClient.lastKnownPosition.y.ToString());
            }
            if (str2.Contains("%POSZ%"))
            {
                str2 = str2.Replace("%POSZ%", Victim.playerClient.lastKnownPosition.z.ToString());
            }
            if (str2.Contains("%POS%"))
            {
                str2 = str2.Replace("%POS%", Victim.playerClient.lastKnownPosition.ToString());
            }
            return str2;
        }

        public static string GetMessageMurder(string msg, [Optional, DefaultParameterValue(null)] NetUser Killer, [Optional, DefaultParameterValue(null)] string VictimName, [Optional, DefaultParameterValue(null)] string WeaponName)
        {
            string str = (Killer == null) ? Core.Languages[0] : Users.GetLanguage(Killer.userID);
            string[] result = new string[] { msg };
            Get("MESSAGES." + str, msg, ref result, true);
            int index = UnityEngine.Random.Range(0, result.Length);
            string str2 = Helper.ReplaceVariables(Killer, result[index], null, "");
            if ((Killer != null) && str2.Contains("%KILLER%"))
            {
                str2 = str2.Replace("%KILLER%", Killer.displayName);
            }
            if ((VictimName != null) && str2.Contains("%VICTIM%"))
            {
                str2 = str2.Replace("%VICTIM%", VictimName);
            }
            if ((WeaponName != null) && str2.Contains("%WEAPON%"))
            {
                str2 = str2.Replace("%WEAPON%", WeaponName);
            }
            if (str2.Contains("%POSX%"))
            {
                str2 = str2.Replace("%POSX%", Killer.playerClient.lastKnownPosition.x.ToString());
            }
            if (str2.Contains("%POSY%"))
            {
                str2 = str2.Replace("%POSY%", Killer.playerClient.lastKnownPosition.y.ToString());
            }
            if (str2.Contains("%POSZ%"))
            {
                str2 = str2.Replace("%POSZ%", Killer.playerClient.lastKnownPosition.z.ToString());
            }
            if (str2.Contains("%POS%"))
            {
                str2 = str2.Replace("%POS%", Killer.playerClient.lastKnownPosition.ToString());
            }
            return str2;
        }

        public static string GetMessageObject(string msg, [Optional, DefaultParameterValue(null)] string VictimName, [Optional, DefaultParameterValue(null)] PlayerClient Killer, [Optional, DefaultParameterValue(null)] string WeaponName, [Optional, DefaultParameterValue(null)] UserData Owner)
        {
            string str = (Killer == null) ? Core.Languages[0] : Users.GetLanguage(Killer.userID);
            string result = msg;
            Get("MESSAGES." + str, msg, ref result, true);
            if (result.Contains("%OWNERNAME%"))
            {
                result = result.Replace("%OWNERNAME%", (Owner != null) ? Owner.Username : "-");
            }
            if (result.Contains("%OWNER_ID%"))
            {
                result = result.Replace("%OWNER_ID%", (Owner != null) ? Owner.SteamID.ToString() : "-");
            }
            if ((Killer != null) && result.Contains("%USERNAME%"))
            {
                result = result.Replace("%USERNAME%", Killer.netUser.displayName);
            }
            if ((Killer != null) && result.Contains("%STEAM_ID%"))
            {
                result = result.Replace("%STEAM_ID%", Killer.netUser.userID.ToString());
            }
            if ((VictimName != null) && result.Contains("%OBJECT%"))
            {
                result = result.Replace("%OBJECT%", VictimName);
            }
            if ((WeaponName != null) && result.Contains("%WEAPON%"))
            {
                result = result.Replace("%WEAPON%", WeaponName);
            }
            return result;
        }

        public static string[] GetMessages(string msg, [Optional, DefaultParameterValue(null)] NetUser User)
        {
            string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
            string[] result = new string[0];
            Get("MESSAGES." + str, msg, ref result, true);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Helper.ReplaceVariables(User, result[i], null, "");
            }
            return result;
        }

        public static string[] GetMessagesClan(string msg, [Optional, DefaultParameterValue(null)] ClanData clan, [Optional, DefaultParameterValue(null)] NetUser netuser, [Optional, DefaultParameterValue(null)] UserData dataUser)
        {
            Class32 class2 = new Class32 {
                clanData_0 = clan
            };
            string str = (netuser == null) ? Core.Languages[0] : Users.GetLanguage(netuser.userID);
            string[] result = new string[0];
            Get("MESSAGES." + str, msg, ref result, true);
            ClanLevel level = null;
            if (class2.clanData_0 != null)
            {
                level = Clans.Levels.Find(new Predicate<ClanLevel>(class2.method_0));
            }
            UserData bySteamID = null;
            if (class2.clanData_0 != null)
            {
                bySteamID = Users.GetBySteamID(class2.clanData_0.LeaderID);
            }
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Helper.ReplaceVariables(netuser, result[i], null, "");
                if (result[i].Contains("%CREATE_COST%"))
                {
                    result[i] = result[i].Replace("%CREATE_COST%", Clans.CreateCost.ToString() + Economy.CurrencySign);
                }
                if (result[i].Contains("%CLANS.COUNT%"))
                {
                    result[i] = result[i].Replace("%CLANS.COUNT%", Clans.Database.Count.ToString());
                }
                if (dataUser != null)
                {
                    if (result[i].Contains("%USERNAME%"))
                    {
                        result[i] = result[i].Replace("%USERNAME%", dataUser.Username);
                    }
                    if (result[i].Contains("%STEAM_ID%"))
                    {
                        result[i] = result[i].Replace("%STEAM_ID%", dataUser.SteamID.ToString());
                    }
                }
                if (class2.clanData_0 != null)
                {
                    if (result[i].Contains("%CLAN.ID%"))
                    {
                        result[i] = result[i].Replace("%CLAN.ID%", class2.clanData_0.ID.ToString());
                    }
                    if (result[i].Contains("%CLAN.CREATED%"))
                    {
                        result[i] = result[i].Replace("%CLAN.CREATED%", class2.clanData_0.Created.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    if (result[i].Contains("%CLAN.NAME%"))
                    {
                        result[i] = result[i].Replace("%CLAN.NAME%", class2.clanData_0.Name);
                    }
                    if (result[i].Contains("%CLAN.ABBR%") && class2.clanData_0.Flags.Has<ClanFlags>(ClanFlags.can_abbr))
                    {
                        result[i] = result[i].Replace("%CLAN.ABBR%", class2.clanData_0.Abbr);
                    }
                    if (result[i].Contains("%CLAN.MOTD%") && class2.clanData_0.Flags.Has<ClanFlags>(ClanFlags.can_motd))
                    {
                        result[i] = result[i].Replace("%CLAN.MOTD%", class2.clanData_0.MOTD);
                    }
                    if (result[i].Contains("%CLAN.BALANCE%") && Economy.Enabled)
                    {
                        result[i] = result[i].Replace("%CLAN.BALANCE%", class2.clanData_0.Balance.ToString() + Economy.CurrencySign);
                    }
                    if (result[i].Contains("%CLAN.TAX%"))
                    {
                        result[i] = result[i].Replace("%CLAN.TAX%", class2.clanData_0.Tax.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.EXPERIENCE%"))
                    {
                        result[i] = result[i].Replace("%CLAN.EXPERIENCE%", class2.clanData_0.Experience.ToString());
                    }
                    if (result[i].Contains("%CLAN.LOCATION%") && class2.clanData_0.Flags.Has<ClanFlags>(ClanFlags.can_warp))
                    {
                        result[i] = result[i].Replace("%CLAN.LOCATION%", class2.clanData_0.Location.AsString());
                    }
                    if (result[i].Contains("%CLAN.ONLINE%"))
                    {
                        result[i] = result[i].Replace("%CLAN.ONLINE%", class2.clanData_0.Online.ToString());
                    }
                    if (result[i].Contains("%CLAN.MEMBERS.COUNT%"))
                    {
                        result[i] = result[i].Replace("%CLAN.MEMBERS.COUNT%", class2.clanData_0.Members.Count.ToString());
                    }
                }
                if (bySteamID != null)
                {
                    if (result[i].Contains("%CLAN.LEADER.STEAM_ID%"))
                    {
                        result[i] = result[i].Replace("%CLAN.LEADER.STEAM_ID%", bySteamID.SteamID.ToString());
                    }
                    if (result[i].Contains("%CLAN.LEADER.USERNAME%"))
                    {
                        result[i] = result[i].Replace("%CLAN.LEADER.USERNAME%", bySteamID.Username);
                    }
                }
                if ((class2.clanData_0 != null) && (class2.clanData_0.Level != null))
                {
                    if (result[i].Contains("%CLAN.LEVEL%"))
                    {
                        result[i] = result[i].Replace("%CLAN.LEVEL%", class2.clanData_0.Level.Id.ToString());
                    }
                    if (result[i].Contains("%CLAN.MEMBERS.MAX%"))
                    {
                        result[i] = result[i].Replace("%CLAN.MEMBERS.MAX%", class2.clanData_0.Level.MaxMembers.ToString());
                    }
                    if (result[i].Contains("%CLAN.WARP_TIMEOUT%"))
                    {
                        result[i] = result[i].Replace("%CLAN.WARP_TIMEOUT%", class2.clanData_0.Level.WarpTimewait.ToString());
                    }
                    if (result[i].Contains("%CLAN.WARP_COUNTDOWN%"))
                    {
                        result[i] = result[i].Replace("%CLAN.WARP_COUNTDOWN%", class2.clanData_0.Level.WarpCountdown.ToString());
                    }
                    if (result[i].Contains("%CLAN.BONUS.CRAFTINGSPEED%") && (class2.clanData_0.Level.BonusCraftingSpeed > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.CRAFTINGSPEED%", class2.clanData_0.Level.BonusCraftingSpeed.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.BONUS.GATHERINGWOOD%") && (class2.clanData_0.Level.BonusGatheringWood > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.GATHERINGWOOD%", class2.clanData_0.Level.BonusGatheringWood.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.BONUS.GATHERINGROCK%") && (class2.clanData_0.Level.BonusGatheringRock > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.GATHERINGROCK%", class2.clanData_0.Level.BonusGatheringRock.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.BONUS.GATHERINGANIMAL%") && (class2.clanData_0.Level.BonusGatheringAnimal > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.GATHERINGANIMAL%", class2.clanData_0.Level.BonusGatheringAnimal.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.BONUS.MEMBERS_PAYMURDER%") && (class2.clanData_0.Level.BonusMembersPayMurder > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.MEMBERS_PAYMURDER%", class2.clanData_0.Level.BonusMembersPayMurder.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.BONUS.MEMBERS_DEFENSE%") && (class2.clanData_0.Level.BonusMembersDefense > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.MEMBERS_DEFENSE%", class2.clanData_0.Level.BonusMembersDefense.ToString() + "%");
                    }
                    if (result[i].Contains("%CLAN.BONUS.MEMBERS_DAMAGE%") && (class2.clanData_0.Level.BonusMembersDamage > 0))
                    {
                        result[i] = result[i].Replace("%CLAN.BONUS.MEMBERS_DAMAGE%", class2.clanData_0.Level.BonusMembersDamage.ToString() + "%");
                    }
                }
                if ((class2.clanData_0 != null) && (level != null))
                {
                    if (result[i].Contains("%CLAN.NEXT_LEVEL%"))
                    {
                        result[i] = result[i].Replace("%CLAN.NEXT_LEVEL%", level.Id.ToString());
                    }
                    if (result[i].Contains("%CLAN.NEXT_CURRENCY%"))
                    {
                        result[i] = result[i].Replace("%CLAN.NEXT_CURRENCY%", level.RequireCurrency.ToString());
                    }
                    if (result[i].Contains("%CLAN.NEXT_EXPERIENCE%"))
                    {
                        result[i] = result[i].Replace("%CLAN.NEXT_EXPERIENCE%", level.RequireExperience.ToString());
                    }
                    if (result[i].Contains("%CLAN.NEXT_MAXMEMBERS%"))
                    {
                        result[i] = result[i].Replace("%CLAN.NEXT_MAXMEMBERS%", level.MaxMembers.ToString());
                    }
                }
            }
            return result;
        }

        public static string GetMessageTeleport(string msg, [Optional, DefaultParameterValue(null)] NetUser User, [Optional, DefaultParameterValue(null)] WorldZone Zone, [Optional, DefaultParameterValue(null)] WorldZone Warp)
        {
            string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
            string result = msg;
            Get("MESSAGES." + str, msg, ref result, true);
            result = Helper.ReplaceVariables(User, result, null, "");
            if ((Warp == null) && (Zone != null))
            {
                Warp = Zone.WarpZone;
            }
            if ((Zone != null) && result.Contains("%ZONE%"))
            {
                result = result.Replace("%ZONE%", Zone.Name);
            }
            if ((Warp != null) && result.Contains("%INTO%"))
            {
                result = result.Replace("%INTO%", Zone.Name);
            }
            if ((Zone != null) && result.Contains("%SECONDS%"))
            {
                result = result.Replace("%SECONDS%", Zone.WarpTime.ToString());
            }
            return result;
        }

        public static string GetMessageTruth(string msg, NetUser User, string hackMethod, int Violations, DateTime period)
        {
            string str = (User == null) ? Core.Languages[0] : Users.GetLanguage(User.userID);
            string result = msg;
            Get("MESSAGES." + str, msg, ref result, true);
            result = Helper.ReplaceVariables(User, result, null, "");
            if (result.Contains("%BAN_PERIOD%"))
            {
                result = result.Replace("%BAN_PERIOD%", (period.Ticks > 0L) ? period.ToString("yyyy-MM-dd HH:mm:ss") : "--:--");
            }
            if (result.Contains("%VIOLATION_HACK%"))
            {
                result = result.Replace("%VIOLATION_HACK%", hackMethod);
            }
            if (result.Contains("%VIOLATION_NUM%"))
            {
                result = result.Replace("%VIOLATION_NUM%", Violations.ToString());
            }
            if (result.Contains("%VIOLATION_MAX%"))
            {
                result = result.Replace("%VIOLATION_MAX%", Truth.MaxViolations.ToString());
            }
            if (result.Contains("%POSX%"))
            {
                result = result.Replace("%POSX%", User.playerClient.lastKnownPosition.x.ToString());
            }
            if (result.Contains("%POSY%"))
            {
                result = result.Replace("%POSY%", User.playerClient.lastKnownPosition.y.ToString());
            }
            if (result.Contains("%POSZ%"))
            {
                result = result.Replace("%POSZ%", User.playerClient.lastKnownPosition.z.ToString());
            }
            if (result.Contains("%POS%"))
            {
                result = result.Replace("%POS%", User.playerClient.lastKnownPosition.ToString());
            }
            return result;
        }

        public static void Initialize()
        {
            Loading = true;
            FilePath = Path.Combine(Core.SavePath, @"cfg\RustExtended");
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            list_0.Clear();
            Core.Commands.Clear();
            Core.Ranks.Clear();
            Core.Kits.Clear();
            Core.ForbiddenUsername.Clear();
            Core.DestoryResources.Clear();
            if (LoadoutList == null)
            {
                LoadoutList = new Dictionary<string, System.Collections.Generic.List<string>>();
            }
            LoadoutList.Clear();
            if (Events.Motd != null)
            {
                foreach (MOTDEvent event2 in Events.Motd)
                {
                    event2.Dispose();
                }
            }
            Events.Motd.Clear();
            if (Clans.Levels == null)
            {
                Clans.Levels = new System.Collections.Generic.List<ClanLevel>();
            }
            Clans.Levels.Clear();
            if (Clans.CraftExperience == null)
            {
                Clans.CraftExperience = new Dictionary<string, int>();
            }
            Clans.CraftExperience.Clear();
            Initialized = Load();
            if (Initialized)
            {
                Get("SERVER", "ServerName", ref Core.ServerName, true);
                Get("SERVER", "Languages", ref Core.Languages, true);
                Get("SERVER", "Errors.Threshold", ref Core.ErrorsThreshold, true);
                Get("SERVER", "Errors.Shutdown", ref Core.ErrorsShutdown, true);
                Get("SERVER", "Errors.Restart", ref Core.ErrorsRestart, true);
                Get("SERVER", "Steam.APIKey", ref Core.SteamAPIKey, true);
                Get("SERVER", "Steam.AuthUser", ref Core.SteamAuthUser, true);
                Get("SERVER", "Steam.SetOfficial", ref Rust.Steam.Server.Official, true);
                Get("SERVER", "Steam.SetModded", ref Rust.Steam.Server.Modded, true);
                Get("SERVER", "Steam.FakeOnline", ref Core.SteamFakeOnline, true);
                Get("SERVER", "Steam.Favourite", ref Core.SteamFavourite, true);
                Get("SERVER", "SavePath", ref Core.SavePath, true);
                Get("SERVER", "LogsPath", ref Core.LogsPath, true);
                Get("SERVER", "Generate.Source", ref Core.GenerateSource, true);
                Get("SERVER", "Generate.Output", ref Core.GenerateOutput, true);
                Get("SERVER", "Override.Loots", ref Core.OverrideLoots, true);
                Get("SERVER", "Override.Items", ref Core.OverrideItems, true);
                Get("SERVER", "Override.Spawns", ref Core.OverrideSpawns, true);
                Get("SERVER", "Override.Damage", ref Core.OverrideDamage, true);
                Get("SERVER", "PremiumConnections", ref Core.PremiumConnections, true);
                Get("SERVER", "DecayObjects", ref Core.DecayObjects, true);
                Get("SERVER", "SleepersLifeTime", ref Core.SleepersLifeTime, true);
                Get("SERVER", "ShutdownTime", ref Core.ShutdownTime, true);
                Get("SERVER", "RestartTime", ref Core.RestartTime, true);
                Get("SERVER", "AutoShutdown", ref Core.AutoShutdown, true);
                Get("SERVER", "AutoRestart", ref Core.AutoRestart, true);
                Get("SERVER", "MySQL.Host", ref Core.MySQL_Host, true);
                Get("SERVER", "MySQL.Port", ref Core.MySQL_Port, true);
                Get("SERVER", "MySQL.Username", ref Core.MySQL_Username, true);
                Get("SERVER", "MySQL.Password", ref Core.MySQL_Password, true);
                Get("SERVER", "MySQL.Database", ref Core.MySQL_Database, true);
                Get("SERVER", "MySQL.Synchronize", ref Core.MySQL_Synchronize, true);
                Get("SERVER", "MySQL.SyncInterval", ref Core.MySQL_SyncInterval, true);
                Get("SERVER", "MySQL.LogLevel", ref Core.MySQL_LogLevel, true);
                Get("SERVER", "MySQL.UTF8", ref Core.MySQL_UTF8, true);
                Get("SERVER", "Database.Type", ref Core.DatabaseType, true);
                Get("SERVER", "Users.VerifyNames", ref Users.VerifyNames, true);
                Get("SERVER", "Users.VerifyChars", ref Users.VerifyChars, true);
                Get("SERVER", "Users.UniqueNames", ref Users.UniqueNames, true);
                Get("SERVER", "Users.BindingNames", ref Users.BindingNames, true);
                Get("SERVER", "Users.DefaultRank", ref Users.DefaultRank, true);
                Get("SERVER", "Users.PremiumRank", ref Users.PremiumRank, true);
                Get("SERVER", "Users.AutoAdminRank", ref Users.AutoAdminRank, true);
                Get("SERVER", "Users.DisplayRank", ref Users.DisplayRank, true);
                Get("SERVER", "Users.PingLimit", ref Users.PingLimit, true);
                Get("SERVER", "Users.Network.Timeout", ref Users.NetworkTimeout, true);
                Get("SERVER", "Users.MD5Password", ref Users.MD5Password, true);
                Get("SERVER", "Admin.Godmode", ref Core.AdminGodmode, true);
                Get("SERVER", "Admin.InstantDestory", ref Core.AdminInstantDestory, true);
                Get("SERVER", "Avatar.AutoSave.Interval", ref Core.AvatarAutoSaveInterval, true);
                Get("SERVER", "Object.Lootable.Lifetime", ref Core.ObjectLootableLifetime, true);
                Get("SERVER", "Whitelist.Enabled", ref Core.WhitelistEnabled, true);
                Get("SERVER", "Whitelist.Refresh", ref Core.WhitelistRefresh, true);
                Get("SERVER", "Whitelist.Interval", ref Core.WhitelistInterval, true);
                Get("SERVER", "Truth.Punishment", ref Truth.Punishment, true);
                Get("SERVER", "Truth.ReportRank", ref Truth.ReportRank, true);
                Get("SERVER", "Truth.ViolationColor", ref Truth.ViolationColor, true);
                Get("SERVER", "Truth.MaxViolations", ref Truth.MaxViolations, true);
                Get("SERVER", "Truth.ViolationDetails", ref Truth.ViolationDetails, true);
                Get("SERVER", "Truth.ViolationTimelife", ref Truth.ViolationTimelife, true);
                Get("SERVER", "Truth.CheckAimbot", ref Truth.CheckAimbot, true);
                Get("SERVER", "Truth.CheckWallhack", ref Truth.CheckWallhack, true);
                Get("SERVER", "Truth.CheckJumphack", ref Truth.CheckJumphack, true);
                Get("SERVER", "Truth.CheckFallhack", ref Truth.CheckFallhack, true);
                Get("SERVER", "Truth.CheckSpeedhack", ref Truth.CheckSpeedhack, true);
                Get("SERVER", "Truth.CheckShotRange", ref Truth.CheckShotRange, true);
                Get("SERVER", "Truth.CheckShotEyes", ref Truth.CheckShotEyes, true);
                Get("SERVER", "Truth.ShotAboveMaxDistance", ref Truth.ShotAboveMaxDistance, true);
                Get("SERVER", "Truth.ShotThroughObject.Block", ref Truth.ShotThroughObjectBlock, true);
                Get("SERVER", "Truth.ShotThroughObject.Punish", ref Truth.ShotThroughObjectPunish, true);
                Get("SERVER", "Truth.HeadshotThreshold", ref Truth.HeadshotThreshold, true);
                Get("SERVER", "Truth.MaxMovementSpeed", ref Truth.MaxMovementSpeed, true);
                Get("SERVER", "Truth.MaxJumpingHeight", ref Truth.MaxJumpingHeight, true);
                Get("SERVER", "Truth.MinFallingHeight", ref Truth.MinFallingHeight, true);
                Get("SERVER", "Truth.MinShotRateByRange", ref Truth.MinShotRateByRange, true);
                Get("SERVER", "Truth.HeadshotAimTime", ref Truth.HeadshotAimTime, true);
                Get("SERVER", "Truth.Banned.BlockIP", ref Truth.BannedBlockIP, true);
                Get("SERVER", "Truth.Banned.Period", ref Truth.BannedPeriod, true);
                Get("SERVER", "Truth.Banned.ExcludeIP", ref Truth.BannedExcludeIP, true);
                Get("SERVER", "RustProtect", ref Truth.RustProtect, true);
                Get("SERVER", "RustProtect.SteamHWID", ref Truth.RustProtectSteamHWID, true);
                Get("SERVER", "RustProtect.Key", ref Truth.RustProtectKey, true);
                Get("SERVER", "RustProtect.Hash", ref Truth.RustProtectHash, true);
                Get("SERVER", "RustProtect.ChangeKey", ref Truth.RustProtectChangeKey, true);
                Get("SERVER", "RustProtect.ChangeKey.Interval", ref Truth.RustProtectChangeKeyInterval, true);
                Get("SERVER", "RustProtect.MaxTicks", ref Truth.RustProtectMaxTicks, true);
                Get("SERVER", "RustProtect.Snapshots", ref Truth.RustProtectSnapshots, true);
                Get("SERVER", "RustProtect.Snapshots.MaxCount", ref Truth.RustProtectSnapshotsMaxCount, true);
                Get("SERVER", "RustProtect.Snapshots.Interval", ref Truth.RustProtectSnapshotsInterval, true);
                Get("SERVER", "RustProtect.Snapshots.PacketSize", ref Truth.RustProtectSnapshotsPacketSize, true);
                Get("SERVER", "RustProtect.Snapshots.Path", ref Truth.RustProtectSnapshotsPath, true);
                Get("SERVER", "Airdrop.Enabled", ref Core.Airdrop, true);
                Get("SERVER", "Airdrop.Announce", ref Core.AirdropAnnounce, true);
                Get("SERVER", "Airdrop.DropTime", ref Core.AirdropDropTime, true);
                Get("SERVER", "Airdrop.DropTime.Hours", ref Core.AirdropDropTimeHours, true);
                Get("SERVER", "Airdrop.Interval", ref Core.AirdropInterval, true);
                Get("SERVER", "Airdrop.Interval.Time", ref Core.AirdropIntervalTime, true);
                Get("SERVER", "Airdrop.Planes", ref Core.AirdropPlanes, true);
                Get("SERVER", "Airdrop.Drops", ref Core.AirdropDrops, true);
                Get("SERVER", "Cycle.PvP", ref Core.CyclePvP, true);
                Get("SERVER", "Cycle.PvP.Off", ref Core.CyclePvPOff, true);
                Get("SERVER", "Cycle.PvP.On", ref Core.CyclePvPOn, true);
                Get("SERVER", "Cycle.InstantCraft", ref Core.CycleInstantCraft, true);
                Get("SERVER", "Cycle.InstantCraft.Off", ref Core.CycleInstantCraftOff, true);
                Get("SERVER", "Cycle.InstantCraft.On", ref Core.CycleInstantCraftOn, true);
                Get("SERVER", "Announce.PlayerJoin", ref Core.AnnouncePlayerJoin, true);
                Get("SERVER", "Announce.PlayerLeave", ref Core.AnnouncePlayerLeave, true);
                Get("SERVER", "Announce.AdminConnect", ref Core.AnnounceAdminConnect, true);
                Get("SERVER", "Announce.DeathNPC", ref Core.AnnounceDeathNPC, true);
                Get("SERVER", "Announce.DeathSelf", ref Core.AnnounceDeathSelf, true);
                Get("SERVER", "Announce.DeathMurder", ref Core.AnnounceDeathMurder, true);
                Get("SERVER", "Announce.KillNotice", ref Core.AnnounceKillNotice, true);
                Get("SERVER", "Notice.Connected.Player", ref Core.NoticeConnectedPlayer, true);
                Get("SERVER", "Notice.Connected.Admin", ref Core.NoticeConnectedAdmin, true);
                Get("SERVER", "Chat.Line.MaxLength", ref Core.ChatLineMaxLength, true);
                Get("SERVER", "Chat.System.Color", ref Core.ChatSystemColor, true);
                Get("SERVER", "Chat.Say.Icon", ref Core.ChatSayIcon, true);
                Get("SERVER", "Chat.Say.Color", ref Core.ChatSayColor, true);
                Get("SERVER", "Chat.Say.Distance", ref Core.ChatSayDistance, true);
                Get("SERVER", "Chat.Yell.Key", ref Core.ChatYellKey, true);
                Get("SERVER", "Chat.Yell.Icon", ref Core.ChatYellIcon, true);
                Get("SERVER", "Chat.Yell.Color", ref Core.ChatYellColor, true);
                Get("SERVER", "Chat.Yell.Distance", ref Core.ChatYellDistance, true);
                Get("SERVER", "Chat.Whisper.Key", ref Core.ChatWhisperKey, true);
                Get("SERVER", "Chat.Whisper.Icon", ref Core.ChatWhisperIcon, true);
                Get("SERVER", "Chat.Whisper.Color", ref Core.ChatWhisperColor, true);
                Get("SERVER", "Chat.Whisper.Distance", ref Core.ChatWhisperDistance, true);
                Get("SERVER", "Chat.Clan.Key", ref Core.ChatClanKey, true);
                Get("SERVER", "Chat.Clan.Icon", ref Core.ChatClanIcon, true);
                Get("SERVER", "Chat.Clan.Color", ref Core.ChatClanColor, true);
                Get("SERVER", "Chat.Command.Key", ref Core.ChatCommandKey, true);
                Get("SERVER", "Chat.Divider", ref Core.ChatDivider, true);
                Get("SERVER", "Chat.Console", ref Core.ChatConsole, true);
                Get("SERVER", "Chat.Console.Name", ref Core.ChatConsoleName, true);
                Get("SERVER", "Chat.Console.Color", ref Core.ChatConsoleColor, true);
                Get("SERVER", "Chat.Display.Rank", ref Core.ChatDisplayRank, true);
                Get("SERVER", "Chat.Display.Clan", ref Core.ChatDisplayClan, true);
                Get("SERVER", "Chat.History.Private", ref Core.ChatHistoryPrivate, true);
                Get("SERVER", "Chat.History.Commands", ref Core.ChatHistoryCommands, true);
                Get("SERVER", "Chat.History.Stored", ref Core.ChatHistoryStored, true);
                Get("SERVER", "Chat.History.Display", ref Core.ChatHistoryDisplay, true);
                Get("SERVER", "Chat.MuteDefaultTime", ref Core.ChatMuteDefaultTime, true);
                Get("SERVER", "Voice.Notification", ref Core.VoiceNotification, true);
                Get("SERVER", "Voice.NotificationDelay", ref Core.VoiceNotificationDelay, true);
                Get("SERVER", "Ownership.Destroy", ref Core.OwnershipDestroy, true);
                Get("SERVER", "Ownership.Destroy.Instant", ref Core.OwnershipDestroyInstant, true);
                Get("SERVER", "Ownership.Destroy.AutoDisable", ref Core.OwnershipDestroyAutoDisable, true);
                Get("SERVER", "Ownership.Destroy.NoCarryWeight", ref Core.OwnershipDestroyNoCarryWeight, true);
                Get("SERVER", "Ownership.Destroy.ReceiveResources", ref Core.OwnershipDestroyReceiveResources, true);
                Get("SERVER", "Ownership.Protect.PremiumUser", ref Core.OwnershipProtectPremiumUser, true);
                Get("SERVER", "Ownership.Protect.OfflineUser", ref Core.OwnershipProtectOfflineUser, true);
                Get("SERVER", "Ownership.Protect.SharedUsers", ref Core.OwnershipProtectSharedUsers, true);
                Get("SERVER", "Ownership.Protect.Container", ref Core.OwnershipProtectContainer, true);
                Get("SERVER", "Ownership.Attacked.Announce", ref Core.OwnershipAttackedAnnounce, true);
                Get("SERVER", "Ownership.Attacked.PremiumOnly", ref Core.OwnershipAttackedPremiumOnly, true);
                Get("SERVER", "Ownership.NotOwner.DenyBuild", ref Core.OwnershipNotOwnerDenyBuild, true);
                Get("SERVER", "Ownership.NotOwner.DenyDeploy", ref Core.OwnershipNotOwnerDenyDeploy, true);
                Get("SERVER", "Ownership.Build.MaxComponents", ref Core.OwnershipBuildMaxComponents, true);
                Get("SERVER", "Ownership.Build.MaxHeight", ref Core.OwnershipBuildMaxHeight, true);
                Get("SERVER", "Ownership.Build.MaxLength", ref Core.OwnershipBuildMaxLength, true);
                Get("SERVER", "Ownership.Build.MaxWidth", ref Core.OwnershipBuildMaxWidth, true);
                Get("SERVER", "Ownership.MaxComponents", ref Core.OwnershipMaxComponents, true);
                Get("SERVER", "Resources.AmountMultiplier.Wood", ref Core.ResourcesAmountMultiplierWood, true);
                Get("SERVER", "Resources.AmountMultiplier.Rock", ref Core.ResourcesAmountMultiplierRock, true);
                Get("SERVER", "Resources.AmountMultiplier.Flay", ref Core.ResourcesAmountMultiplierFlay, true);
                Get("SERVER", "Resources.GatherMultiplier.Wood", ref Core.ResourcesGatherMultiplierWood, true);
                Get("SERVER", "Resources.GatherMultiplier.Rock", ref Core.ResourcesGatherMultiplierRock, true);
                Get("SERVER", "Resources.GatherMultiplier.Flay", ref Core.ResourcesGatherMultiplierFlay, true);
                Get("SERVER", "Command.Transfer.Forbidden", ref Core.CommandTransferForbidden, true);
                Get("SERVER", "Command.Home.Payment", ref Core.CommandHomePayment, true);
                Get("SERVER", "Command.Home.Timewait", ref Core.CommandHomeTimewait, true);
                Get("SERVER", "Command.Home.Countdown", ref Core.CommandHomeCountdown, true);
                Get("SERVER", "Command.Home.OutdoorsOnly", ref Core.CommandHomeOutdoorsOnly, true);
                Get("SERVER", "Command.Teleport.Payment", ref Core.CommandTeleportPayment, true);
                Get("SERVER", "Command.Teleport.Timewait", ref Core.CommandTeleportTimewait, true);
                Get("SERVER", "Command.Teleport.Countdown", ref Core.CommandTeleportCountdown, true);
                Get("SERVER", "Command.Teleport.OutdoorsOnly", ref Core.CommandTeleportOutdoorsOnly, true);
                Get("SERVER", "Command.NoPVP.Timewait", ref Core.CommandNoPVPTimewait, true);
                Get("SERVER", "Command.NoPVP.Duration", ref Core.CommandNoPVPDuration, true);
                Get("SERVER", "Command.NoPVP.Countdown", ref Core.CommandNoPVPCountdown, true);
                Get("CLANS", "Enabled", ref Clans.Enabled, true);
                Get("CLANS", "DefaultLevel", ref Clans.DefaultLevel, true);
                Get("CLANS", "CreateCost", ref Clans.CreateCost, true);
                Get("CLANS", "Experience.Multiplier", ref Clans.ExperienceMultiplier, true);
                Get("CLANS", "Warp.OutdoorsOnly", ref Clans.WarpOutdoorsOnly, true);
                Get("CLANS", "ClanWar.Death.Pay", ref Clans.ClanWarDeathPay, true);
                Get("CLANS", "ClanWar.Death.Percent", ref Clans.ClanWarDeathPercent, true);
                Get("CLANS", "ClanWar.Murder.Fee", ref Clans.ClanWarMurderFee, true);
                Get("CLANS", "ClanWar.Murder.Percent", ref Clans.ClanWarMurderPercent, true);
                Get("CLANS", "ClanWar.Declared.Gain.Percent", ref Clans.ClanWarDeclaredGainPercent, true);
                Get("CLANS", "ClanWar.Declined.Lost.Percent", ref Clans.ClanWarDeclinedLostPercent, true);
                Get("CLANS", "ClanWar.Declined.Penalty", ref Clans.ClanWarDeclinedPenalty, true);
                Get("CLANS", "ClanWar.Accepted.Time", ref Clans.ClanWarAcceptedTime, true);
                Get("CLANS", "ClanWar.Ended.Penalty", ref Clans.ClanWarEndedPenalty, true);
                if (Core.Languages.Length == 1)
                {
                    Core.Languages = Core.Languages[0].ToUpper().Replace(" ", "").Split(new char[] { ',' });
                }
                if (Core.Languages.Length == 0)
                {
                    Core.Languages = new string[] { "ENG" };
                }
                Core.DatabaseType = Core.DatabaseType.ToUpper();
                if (!Core.DatabaseType.Equals("FILE") && !Core.DatabaseType.Equals("MYSQL"))
                {
                    ConsoleSystem.PrintError("RustExtended: Unknown Database Type \"" + Core.DatabaseType + "\"", false);
                }
                switch (Core.MySQL_LogLevel)
                {
                    case 0:
                        MySQL.LogLevel = MySQL.LogLevelType.NONE;
                        break;

                    case 1:
                        MySQL.LogLevel = MySQL.LogLevelType.ERRORS;
                        break;

                    default:
                        MySQL.LogLevel = MySQL.LogLevelType.ALL;
                        break;
                }
                foreach (PlayerClient client in PlayerClient.All)
                {
                    Users.GetBySteamID(client.userID).ProtectTime = 0f;
                    Users.GetBySteamID(client.userID).ProtectTick = 0;
                }
                Truth.PunishAction = Truth.Punishment.ToUpper().Replace(" ", "").Split(new char[] { ',' });
                Truth.ProtectionHash = Truth.RustProtectHash.ToInt32();
                Truth.ProtectionKey = Truth.RustProtectKey.ToInt32();
                if (Truth.RustProtectSnapshotsPacketSize < 0x100)
                {
                    Truth.RustProtectSnapshotsPacketSize = 0x100;
                }
                if (Truth.RustProtectSnapshotsPacketSize > 0x2000)
                {
                    Truth.RustProtectSnapshotsPacketSize = 0x2000;
                }
                if (Truth.RustProtectMaxTicks < NetCull.sendRate)
                {
                    Truth.RustProtectMaxTicks = (uint) NetCull.sendRate;
                }
                Truth.RustProtectSnapshotsPath = Truth.RustProtectSnapshotsPath.Trim(new char[] { '"', ' ' }).Replace("/", @"\");
                if (CommandLine.HasSwitch("-no-premium-connections"))
                {
                    Core.PremiumConnections = 0;
                }
                if (Core.SleepersLifeTime < 0)
                {
                    Core.SleepersLifeTime = 0;
                }
                if (Core.PremiumConnections > 0)
                {
                    server.maxplayers += Core.PremiumConnections;
                }
                if (CommandLine.HasSwitch("-force-steam-auth"))
                {
                    Core.SteamAuthUser = true;
                }
                if (Rust.Steam.Server.Official)
                {
                    Helper.Log("This server running in official mode.", true);
                }
                if (Rust.Steam.Server.Modded)
                {
                    Helper.Log("This server has been marked as modified.", true);
                }
                if (!Core.SteamAuthUser)
                {
                    Helper.LogWarning("This server running without steam authentication.", true);
                }
                if (Truth.RustProtect)
                {
                    Helper.Log("---------------------------------------------------", true);
                    Helper.Log("Support for clients protection RustProtect enabled.", true);
                    Helper.Log("Do not forget give a RustProtect files for players.", true);
                    Helper.Log("---------------------------------------------------", true);
                }
                int num = (Core.SteamFakeOnline.Length > 0) ? Core.SteamFakeOnline[0] : 0;
                int num2 = (Core.SteamFakeOnline.Length > 1) ? Core.SteamFakeOnline[1] : num;
                Core.SteamFakeOnline = new int[] { Math.Min(num, num2), Math.Max(num, num2) };
                Core.HasFakeOnline = (Core.SteamFakeOnline[1] > 0) && !CommandLine.HasSwitch("-ignore-fake-online");
                if (Core.SteamFakeOnline[0] > server.maxplayers)
                {
                    Core.SteamFakeOnline[0] = server.maxplayers;
                }
                if (Core.SteamFakeOnline[1] > server.maxplayers)
                {
                    Core.SteamFakeOnline[1] = server.maxplayers;
                }
                Core.ChatDivider = Core.ChatDivider.Trim(new char[] { '"' });
                Core.ChatSayIcon = Core.ChatSayIcon.Trim(new char[] { '"' });
                Core.ChatSayColor = Core.ChatSayColor.ToUpper();
                Core.ChatClanIcon = Core.ChatClanIcon.Trim(new char[] { '"' });
                Core.ChatClanColor = Core.ChatClanColor.ToUpper();
                Core.ChatYellIcon = Core.ChatYellIcon.Trim(new char[] { '"' });
                Core.ChatYellColor = Core.ChatYellColor.ToUpper();
                Core.ChatWhisperIcon = Core.ChatWhisperIcon.Trim(new char[] { '"' });
                Core.ChatWhisperColor = Core.ChatWhisperColor.ToUpper();
                Core.ChatConsoleName = Core.ChatConsoleName.Trim(new char[] { '"' });
                Core.ChatConsoleColor = Core.ChatConsoleColor.ToUpper();
                Core.RankColor.Clear();
                foreach (int num3 in Core.Ranks.Keys)
                {
                    string key = string.Format("Chat.Rank.{0}.Color", num3);
                    if (!Core.RankColor.ContainsKey(num3) && Get("SERVER", key, ref key, true))
                    {
                        Core.RankColor.Add(num3, key);
                    }
                }
                foreach (MOTDEvent event3 in Events.Motd)
                {
                    event3.Start();
                }
                Loading = false;
            }
        }

        public static bool Load()
        {
            Predicate<MOTDEvent> match = null;
            Class13 class3 = new Class13();
            string[] files = Directory.GetFiles(FilePath, "*.cfg");
            if (files.Length == 0)
            {
                ConsoleSystem.Log("No configuration files. Loaded defaults.");
                return false;
            }
            string str = "-1";
            class3.string_0 = new string[0];
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            foreach (string str2 in files)
            {
                list.AddRange(File.ReadAllLines(str2).ToList<string>());
                ConsoleSystem.Print("Loaded " + str2.Replace(@"\", "/"), false);
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] strArray2;
                string str9;
                string str3 = list[i].Trim();
                if (!string.IsNullOrEmpty(str3) && !str3.StartsWith("//"))
                {
                    if (str3.Contains("//"))
                    {
                        str3 = str3.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(str3))
                    {
                        if (str3.ToUpper().StartsWith("[") && str3.ToUpper().EndsWith("]"))
                        {
                            class3.string_0 = Helper.SplitQuotes(str3.Substring(1, str3.Length - 2), ' ');
                            class3.string_0[0] = class3.string_0[0].ToUpper();
                            if ((class3.string_0[0].StartsWith("MESSAGES.") || class3.string_0[0].StartsWith("BODYPART.")) || class3.string_0[0].StartsWith("NAMES."))
                            {
                                class3.string_0 = class3.string_0[0].Split(new char[] { '.' });
                            }
                            if ((class3.string_0[0] == "RANK") && (class3.string_0.Length > 1))
                            {
                                int result = 0;
                                string str4 = "";
                                string[] strArray3 = new string[] { class3.string_0[1].Trim() };
                                if (class3.string_0[1].Contains<char>('.'))
                                {
                                    strArray3 = class3.string_0[1].Split(new char[] { '.' });
                                    str4 = strArray3[1].Trim();
                                }
                                if (!int.TryParse(strArray3[0].Trim(), out result))
                                {
                                    class3.string_0[0] = "NULL";
                                }
                                else
                                {
                                    str = strArray3[0].Trim();
                                    if (Core.Ranks.ContainsKey(result))
                                    {
                                        class3.string_0[0] = "NULL";
                                    }
                                    else
                                    {
                                        Core.Ranks.Add(result, str4);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!str3.Contains("="))
                            {
                                goto Label_0B30;
                            }
                            strArray2 = str3.Split(new char[] { '=' });
                            strArray2[0] = strArray2[0].Trim();
                            strArray2[1] = strArray2[1].Trim();
                            Class14 class2 = new Class14 {
                                class13_0 = class3
                            };
                            switch (class3.string_0[0])
                            {
                                case "SERVER":
                                    Add(class3.string_0[0], strArray2[0], strArray2[1]);
                                    break;

                                case "OVERRIDE.ARMOR":
                                    Add(class3.string_0[0], strArray2[0], strArray2[1]);
                                    break;

                                case "OVERRIDE.DAMAGE":
                                    Add(class3.string_0[0], strArray2[0], strArray2[1]);
                                    break;

                                case "DESTROY.RESOURCES":
                                    Core.DestoryResources.Add(strArray2[0], strArray2[1]);
                                    break;

                                case "MESSAGES":
                                case "NAMES":
                                case "BODYPART":
                                    if (class3.string_0.Length >= 2)
                                    {
                                        goto Label_0504;
                                    }
                                    Add(class3.string_0[0], strArray2[0], strArray2[1]);
                                    break;

                                case "RANK":
                                    Core.Commands.Add(str + "=" + strArray2[0].ToLower() + "=" + strArray2[1]);
                                    break;

                                case "MOTD":
                                    if ((class3.string_0.Length > 1) && !class3.string_0[1].IsEmpty())
                                    {
                                        if (match == null)
                                        {
                                            match = new Predicate<MOTDEvent>(class3.method_0);
                                        }
                                        MOTDEvent item = Events.Motd.Find(match);
                                        if (item == null)
                                        {
                                            item = new MOTDEvent(class3.string_0[1], 0xe10);
                                            Events.Motd.Add(item);
                                        }
                                        if (strArray2[0].Equals("Enabled", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string str5 = strArray2[1].ToUpper().Trim();
                                            item.Enabled = (((str5 == "TRUE") || (str5 == "YES")) || (str5 == "ON")) || (str5 == "1");
                                        }
                                        else if (strArray2[0].Equals("Interval", StringComparison.OrdinalIgnoreCase))
                                        {
                                            int num3;
                                            int.TryParse(strArray2[1], out num3);
                                            item.Interval = num3;
                                        }
                                        else if (strArray2[0].Equals("Message", StringComparison.OrdinalIgnoreCase))
                                        {
                                            item.Messages.Add(strArray2[1]);
                                        }
                                        else if (strArray2[0].Equals("Announce", StringComparison.OrdinalIgnoreCase))
                                        {
                                            item.Announce.Add(strArray2[1]);
                                        }
                                    }
                                    break;

                                case "KIT":
                                    if (class3.string_0.Length > 1)
                                    {
                                        string key = class3.string_0[1].ToLower();
                                        System.Collections.Generic.List<string> list2 = (System.Collections.Generic.List<string>) Core.Kits[key];
                                        if (list2 == null)
                                        {
                                            list2 = new System.Collections.Generic.List<string>();
                                            Core.Kits.Add(key, list2);
                                        }
                                        list2.Add(str3);
                                    }
                                    break;

                                case "LOADOUT":
                                {
                                    string str7 = class3.string_0[1].Trim().ToUpper();
                                    if (!LoadoutList.ContainsKey(str7))
                                    {
                                        LoadoutList.Add(str7, new System.Collections.Generic.List<string>());
                                    }
                                    LoadoutList[str7].Add(str3);
                                    break;
                                }
                                case "ECONOMY":
                                case "SHOP":
                                    Add(class3.string_0[0], strArray2[0], strArray2[1]);
                                    break;

                                case "SHOP.LIST":
                                    Add(class3.string_0[0], "ENTRY", str3);
                                    break;

                                case "SHOP.GROUP":
                                    Add(class3.string_0[0], class3.string_0[1], str3);
                                    break;

                                case "CLANS":
                                    Add(class3.string_0[0], strArray2[0], strArray2[1]);
                                    break;

                                case "CLAN.CRAFTING.EXPERIENCE":
                                    if (!string.IsNullOrEmpty(strArray2[0]) && !Clans.CraftExperience.ContainsKey(strArray2[0]))
                                    {
                                        int num4 = 0;
                                        if (int.TryParse(strArray2[1], out num4))
                                        {
                                            Clans.CraftExperience.Add(strArray2[0].Trim(), num4);
                                        }
                                    }
                                    break;

                                case "CLANLEVEL":
                                    if (int.TryParse(class3.string_0[1], out class2.int_0))
                                    {
                                        ClanLevel level = Clans.Levels.Find(new Predicate<ClanLevel>(class2.method_0));
                                        if (level == null)
                                        {
                                            level = new ClanLevel(class2.int_0);
                                            Clans.Levels.Add(level);
                                        }
                                        str3 = strArray2[0].Trim().ToUpper();
                                        if (!strArray2[1].IsEmpty())
                                        {
                                            if (str3.Equals("REQUIRE.CURRENCY"))
                                            {
                                                ulong.TryParse(strArray2[1], out level.RequireCurrency);
                                            }
                                            else if (str3.Equals("REQUIRE.EXPERIENCE"))
                                            {
                                                ulong.TryParse(strArray2[1], out level.RequireExperience);
                                            }
                                            else if (str3.Equals("REQUIRE.LEVEL"))
                                            {
                                                int.TryParse(strArray2[1], out level.RequireLevel);
                                            }
                                            else if (str3.Equals("MAXIMUM.MEMBERS"))
                                            {
                                                int.TryParse(strArray2[1], out level.MaxMembers);
                                            }
                                            else if (str3.Equals("CURRENCY.TAX"))
                                            {
                                                uint.TryParse(strArray2[1], out level.CurrencyTax);
                                            }
                                            else if (str3.Equals("WARP.TIMEWAIT"))
                                            {
                                                uint.TryParse(strArray2[1], out level.WarpTimewait);
                                            }
                                            else if (str3.Equals("WARP.COUNTDOWN"))
                                            {
                                                uint.TryParse(strArray2[1], out level.WarpCountdown);
                                            }
                                            else if (str3.Equals("FLAG.MOTD"))
                                            {
                                                level.FlagMotd = strArray2[1].ToBool();
                                            }
                                            else if (str3.Equals("FLAG.ABBR"))
                                            {
                                                level.FlagAbbr = strArray2[1].ToBool();
                                            }
                                            else if (str3.Equals("FLAG.FFIRE"))
                                            {
                                                level.FlagFFire = strArray2[1].ToBool();
                                            }
                                            else if (str3.Equals("FLAG.TAX"))
                                            {
                                                level.FlagTax = strArray2[1].ToBool();
                                            }
                                            else if (str3.Equals("FLAG.HOUSE"))
                                            {
                                                level.FlagHouse = strArray2[1].ToBool();
                                            }
                                            else if (str3.Equals("FLAG.DECLARE"))
                                            {
                                                level.FlagDeclare = strArray2[1].ToBool();
                                            }
                                            else if (str3.Equals("BONUS.CRAFTING.SPEED"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusCraftingSpeed);
                                            }
                                            else if (str3.Equals("BONUS.GATHERING.WOOD"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusGatheringWood);
                                            }
                                            else if (str3.Equals("BONUS.GATHERING.ROCK"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusGatheringRock);
                                            }
                                            else if (str3.Equals("BONUS.GATHERING.ANIMAL"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusGatheringAnimal);
                                            }
                                            else if (str3.Equals("BONUS.MEMBERS.PAYMURDER"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusMembersPayMurder);
                                            }
                                            else if (str3.Equals("BONUS.MEMBERS.DEFENSE"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusMembersDefense);
                                            }
                                            else if (str3.Equals("BONUS.MEMBERS.DAMAGE"))
                                            {
                                                uint.TryParse(strArray2[1], out level.BonusMembersDamage);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                continue;
            Label_0504:
                Add(class3.string_0[0] + '.' + class3.string_0[1], strArray2[0], strArray2[1]);
                continue;
            Label_0B30:
                if ((str9 = class3.string_0[0]) != null)
                {
                    if (!(str9 == "FORBIDDEN.NAME"))
                    {
                        if (str9 == "FORBIDDEN.OBSCENE")
                        {
                            Core.ForbiddenObscene.Add(str3.ToUpper());
                        }
                    }
                    else
                    {
                        Core.ForbiddenUsername.Add(str3.ToUpper());
                    }
                }
            }
            return true;
        }

        public static string FilePath
        {
            [CompilerGenerated]
            get
            {
                return string_0;
            }
            [CompilerGenerated]
            private set
            {
                string_0 = value;
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

        public static bool Loading
        {
            [CompilerGenerated]
            get
            {
                return bool_1;
            }
            [CompilerGenerated]
            private set
            {
                bool_1 = value;
            }
        }

        public static Dictionary<string, System.Collections.Generic.List<string>> LoadoutList
        {
            [CompilerGenerated]
            get
            {
                return dictionary_0;
            }
            [CompilerGenerated]
            private set
            {
                dictionary_0 = value;
            }
        }

        public static Config Singleton
        {
            get
            {
                return config_0;
            }
        }

        [CompilerGenerated]
        private sealed class Class13
        {
            public string[] string_0;

            public bool method_0(MOTDEvent motdevent_0)
            {
                return motdevent_0.Title.Equals(this.string_0[1]);
            }
        }

        [CompilerGenerated]
        private sealed class Class14
        {
            public Config.Class13 class13_0;
            public int int_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.Id == this.int_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class15
        {
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && (struct0_0.string_1 == this.string_1));
            }
        }

        [CompilerGenerated]
        private sealed class Class16
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class17
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class18
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class19
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class20
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class21
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class22
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class23
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class24
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class25
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class26
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class27
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class28
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class29
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class30
        {
            public bool bool_0;
            public string string_0;
            public string string_1;

            public bool method_0(Config.Struct0 struct0_0)
            {
                return ((struct0_0.string_0 == this.string_0) && struct0_0.string_1.Equals(this.string_1, this.bool_0 ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        [CompilerGenerated]
        private sealed class Class31
        {
            public ClanData clanData_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.RequireLevel == this.clanData_0.Level.Id);
            }
        }

        [CompilerGenerated]
        private sealed class Class32
        {
            public ClanData clanData_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.RequireLevel == this.clanData_0.Level.Id);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct0
        {
            public string string_0;
            public string string_1;
            public System.Collections.Generic.List<string> list_0;
        }
    }
}

