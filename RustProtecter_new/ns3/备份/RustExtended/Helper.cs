namespace RustExtended
{
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
    using System.Threading;

    public class Helper
    {
        public static string AssemblyNotVerified = "The assembly versions is incompatible, please update RustExtended.";
        public static string ChatLogFileName;
        public static StreamWriter ChatLogStream;
        private static long long_0 = 0L;
        [CompilerGenerated]
        private static Predicate<Assembly> predicate_0;
        [CompilerGenerated]
        private static Predicate<AssemblyName> predicate_1;
        [CompilerGenerated]
        private static Predicate<Assembly> predicate_2;
        [CompilerGenerated]
        private static Predicate<AssemblyName> predicate_3;
        public static Version RequireVersion = Assembly.GetExecutingAssembly().GetName().Version;
        public static string RustLogFileName;
        public static StreamWriter RustLogStream;
        public static string ServerDenyToStarted = "You can't run server with RustExtended on this IP with this port.\nPlease purchase this modification from a developer before to use.\nVisit web-site: http://www.rust-extended.ru/ for more details.";
        public static string ServSQLFileName;
        public static StreamWriter ServSQLStream;
        public static Dictionary<ulong, System.Collections.Generic.List<string>> userArmor = new Dictionary<ulong, System.Collections.Generic.List<string>>();

        public static bool AssemblyVerify()
        {
            if (predicate_0 == null)
            {
                predicate_0 = new Predicate<Assembly>(Helper.smethod_0);
            }
            Assembly objA = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>().Find(predicate_0);
            if (!object.Equals(objA, null))
            {
                if (predicate_1 == null)
                {
                    predicate_1 = new Predicate<AssemblyName>(Helper.smethod_1);
                }
                AssemblyName name = objA.GetReferencedAssemblies().ToList<AssemblyName>().Find(predicate_1);
                if (((Core.Version.Major == name.Version.Major) && (Core.Version.Minor == name.Version.Minor)) && (Core.Version.Build == name.Version.Build))
                {
                    if (predicate_2 == null)
                    {
                        predicate_2 = new Predicate<Assembly>(Helper.smethod_2);
                    }
                    objA = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>().Find(predicate_2);
                    if (object.Equals(objA, null))
                    {
                        return false;
                    }
                    if (predicate_3 == null)
                    {
                        predicate_3 = new Predicate<AssemblyName>(Helper.smethod_3);
                    }
                    if (objA.GetReferencedAssemblies().ToList<AssemblyName>().Find(predicate_3) == null)
                    {
                        LogWarning("WARNING: The assembly uLink.dll do not linked with RustExtended. Anti-cheat RustProtect could not enabled on server.", true);
                    }
                    return true;
                }
                string assemblyNotVerified = AssemblyNotVerified;
                AssemblyNotVerified = assemblyNotVerified + "\nIncompatible Assembly-CSharp.dll v" + name.Version.ToString(3) + ", required v" + RequireVersion.ToString(3);
            }
            return true;
        }

        public static bool AvatarLoad(ref Character character, NetUser netUser)
        {
            if (((character == null) || (netUser == null)) || (netUser.avatar == null))
            {
                return false;
            }
            if (netUser.avatar.HasVitals)
            {
                character.GetLocal<Metabolism>().LoadVitals(netUser.avatar.Vitals);
                character.takeDamage.LoadVitals(netUser.avatar.Vitals);
            }
            character.GetLocal<PlayerInventory>().LoadToAvatar(ref netUser.avatar);
            return true;
        }

        public static bool AvatarSave(ref Character character, NetUser netUser)
        {
            if (((character == null) || (netUser == null)) || (netUser.avatar == null))
            {
                return false;
            }
            using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler = RustProto.Avatar.Recycler())
            {
                RustProto.Avatar.Builder avatar = recycler.OpenBuilder();
                Character masterCharacter = character.masterCharacter;
                if (masterCharacter == null)
                {
                    masterCharacter = character;
                }
                avatar.SetPos(masterCharacter.origin);
                avatar.SetAng(masterCharacter.rotation);
                using (Recycler<Vitals, Vitals.Builder> recycler2 = Vitals.Recycler())
                {
                    Vitals.Builder vitals = recycler2.OpenBuilder();
                    character.GetLocal<Metabolism>().SaveVitals(ref vitals);
                    character.takeDamage.SaveVitals(ref vitals);
                    avatar.SetVitals(vitals);
                }
                character.GetLocal<PlayerInventory>().SaveToAvatar(ref avatar);
                character.netUser.avatar = avatar.Build();
                return true;
            }
        }

        [DllImport("USER32.DLL", CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
        private static extern IntPtr CallWindowProcW([In] byte[] byte_0, IntPtr intptr_0, int int_0, [In, Out] byte[] byte_1, IntPtr intptr_1);
        public static void ClearArmor(PlayerClient playerClient)
        {
            Inventory component = playerClient.controllable.GetComponent<Inventory>();
            for (int i = 0x24; i < 40; i++)
            {
                component.RemoveItem(i);
            }
        }

        public static bool CreateFileBackup(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                return false;
            }
            if (System.IO.File.Exists(filename + ".old.20"))
            {
                System.IO.File.Delete(filename + ".old.20");
            }
            for (int i = 0x13; i >= 0; i--)
            {
                if (System.IO.File.Exists(filename + ".old." + i))
                {
                    System.IO.File.Move(filename + ".old." + i, filename + ".old." + (i + 1));
                }
            }
            System.IO.File.Move(filename, filename + ".old.0");
            return true;
        }

        public static int DestroyStructure(StructureMaster master)
        {
            if (master == null)
            {
                return -1;
            }
            int count = master._structureComponents.Count;
            foreach (StructureComponent component in master._structureComponents)
            {
                TakeDamage.HurtSelf(component, 3.402823E+38f, null);
            }
            if (master._structureComponents.Count == 0)
            {
                NetCull.Destroy(master.gameObject);
            }
            return count;
        }

        public static bool DisconnectBySteamID(ulong steam_id)
        {
            bool flag = false;
            foreach (uLink.NetworkPlayer player in NetCull.connections)
            {
                NetUser localData = player.GetLocalData() as NetUser;
                if ((localData != null) && (localData.userID == steam_id))
                {
                    localData.Kick(NetError.ConnectionTimeout, true);
                    flag = true;
                }
            }
            return flag;
        }

        public static uint DisconnectByUsername(string username)
        {
            uint num = 0;
            foreach (uLink.NetworkPlayer player in NetCull.connections)
            {
                NetUser localData = player.GetLocalData() as NetUser;
                if ((localData != null) && (localData.displayName == username))
                {
                    localData.Kick(NetError.ConnectionTimeout, true);
                    num++;
                }
            }
            return num;
        }

        public static bool EquipArmor(PlayerClient playerClient, string itemName, [Optional, DefaultParameterValue(false)] bool replaceCurrent)
        {
            int slot = 0;
            Inventory component = playerClient.controllable.GetComponent<Inventory>();
            if (itemName.Contains("Helmet"))
            {
                slot = 0x24;
            }
            if (itemName.Contains("Vest"))
            {
                slot = 0x25;
            }
            if (itemName.Contains("Pants"))
            {
                slot = 0x26;
            }
            if (itemName.Contains("Boots"))
            {
                slot = 0x27;
            }
            if (slot <= 0)
            {
                return false;
            }
            if (replaceCurrent)
            {
                component.RemoveItem(slot);
            }
            component.AddItemAmount(DatablockDictionary.GetByName(itemName), 1, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, false, Inventory.Slot.KindFlags.Armor));
            return true;
        }

        public static void GenerateFile(string sourcefile, string targetfile)
        {
            if (System.IO.File.Exists(sourcefile))
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                string[] strArray = System.IO.File.ReadAllLines(sourcefile);
                if (strArray.Length != 0)
                {
                    System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
                    System.Collections.Generic.List<string> list3 = new System.Collections.Generic.List<string>();
                    System.Collections.Generic.List<string> list4 = new System.Collections.Generic.List<string>();
                    System.Collections.Generic.List<string> list5 = new System.Collections.Generic.List<string>();
                    foreach (string str in strArray)
                    {
                        int length = NetCull.connections.Length;
                        string item = str.Replace("%SERVER.HOSTNAME%", server.hostname).Replace("%SERVER.IP%", UnityEngine.MasterServer.ipAddress).Replace("%SERVER.PORT%", server.port.ToString()).Replace("%SERVER.STEAMID%", Rust.Steam.Server.SteamID.ToString()).Replace("%SERVER.STEAMGROUP%", Rust.Steam.Server.SteamGroup.ToString()).Replace("%SERVER.OFFICIAL%", Rust.Steam.Server.Official ? "YES" : "NO").Replace("%SERVER.MODDED%", Rust.Steam.Server.Modded ? "YES" : "NO").Replace("%SERVER.MAP%", server.map).Replace("%SERVER.PVP%", server.pvp ? "ON" : "OFF").Replace("%SERVER.MAXPLAYERS%", NetCull.maxConnections.ToString()).Replace("%SERVER.PLAYERS%", length.ToString()).Replace("%BANLIST.COUNT%", Banned.Count.ToString()).Replace("%CLANS.COUNT%", Clans.Count.ToString()).Replace("%CLANS.CREATE_COST%", Clans.CreateCost.ToString()).Replace("%CLANS.LEVELS.COUNT%", Clans.Levels.Count.ToString()).Replace("%ECONOMY%", Economy.Enabled ? "YES" : "NO").Replace("%ECONOMY.SIGN%", Economy.CurrencySign).Replace("%ECONOMY.CURRENCYSIGN%", Economy.CurrencySign).Replace("%ECONOMY.STARTBALANCE%", Economy.StartBalance.ToString()).Replace("%ECONOMY.COST_RABBIT%", Economy.CostRabbit.ToString()).Replace("%ECONOMY.COST_CHICKEN%", Economy.CostChicken.ToString()).Replace("%ECONOMY.COST_STAG%", Economy.CostStag.ToString()).Replace("%ECONOMY.COST_BOAR%", Economy.CostBoar.ToString()).Replace("%ECONOMY.COST_WOLF%", Economy.CostWolf.ToString()).Replace("%ECONOMY.COST_BEAR%", Economy.CostBear.ToString()).Replace("%ECONOMY.COST_MUTANTWOLF%", Economy.CostMutantWolf.ToString()).Replace("%ECONOMY.COST_MUTANTBEAR%", Economy.CostMutantBear.ToString()).Replace("%ECONOMY.FEEDEATH%", Economy.FeeDeath ? "YES" : "NO").Replace("%ECONOMY.FEEDEATH.PERCENT%", Economy.FeeDeathPercent.ToString()).Replace("%ECONOMY.FEESUICIDE%", Economy.FeeSuicide ? "YES" : "NO").Replace("%ECONOMY.FEESUICIDE.PERCENT%", Economy.FeeSuicidePercent.ToString()).Replace("%ECONOMY.FEEMURDER%", Economy.FeeMurder ? "YES" : "NO").Replace("%ECONOMY.FEEMURDER.PERCENT%", Economy.FeeMurderPercent.ToString()).Replace("%ECONOMY.PAYMURDER%", Economy.PayMurder ? "YES" : "NO").Replace("%ECONOMY.PAYMURDER.PERCENT%", Economy.PayMurderPercent.ToString()).Replace("%ECONOMY.PAYMURDER%", Economy.PayMurder ? "YES" : "NO").Replace("%SHOP%", Shop.Enabled ? "YES" : "NO").Replace("%SHOP.CAN_BUY%", Shop.CanBuy ? "YES" : "NO").Replace("%SHOP.CAN_SELL%", Shop.CanSell ? "YES" : "NO").Replace("%SHOP.TRADEZONEONLY%", Shop.TradeZoneOnly ? "YES" : "NO");
                        if (item.Contains("<USERLIST>"))
                        {
                            list2.Add(item);
                        }
                        if (item.Contains("</USERLIST>"))
                        {
                            list2.Add(item);
                            foreach (UserData data in Users.All)
                            {
                                foreach (string str3 in list2)
                                {
                                    if (!str3.Replace("<USERLIST>", "").Replace("</USERLIST>", "").IsEmpty())
                                    {
                                        string str4 = str3.Replace("%USER.COUNT%", Users.Count.ToString()).Replace("%USER.STEAM_ID%", data.SteamID.ToString()).Replace("%USER.USERNAME%", data.Username).Replace("%USER.PASSWORD%", data.Password).Replace("%USER.COMMENTS%", data.Comments).Replace("%USER.RANK%", data.Rank.ToString()).Replace("%USER.FLAGS%", data.Flags.ToString()).Replace("%USER.ZONE%", (data.Zone != null) ? data.Zone.Name : "").Replace("%USER.CLAN%", (data.Clan != null) ? data.Clan.Name : "").Replace("%USER.CLAN.ABBR%", (data.Clan != null) ? data.Clan.Abbr : "").Replace("%USER.CLAN.CREATED%", (data.Clan != null) ? data.Clan.Created.ToString("MM/dd/yyyy HH:mm:ss") : "").Replace("%USER.CLAN.LEVEL%", (data.Clan != null) ? data.Clan.Level.ToString() : "").Replace("%USER.CLAN.BALANCE%", (data.Clan != null) ? data.Clan.Balance.ToString() : "").Replace("%USER.CLAN.EXPERIENCE%", (data.Clan != null) ? data.Clan.Experience.ToString() : "").Replace("%USER.CLAN.EXP%", (data.Clan != null) ? data.Clan.Experience.ToString() : "").Replace("%USER.CLAN.MEMBERS.COUNT%", (data.Clan != null) ? data.Clan.Members.Count.ToString() : "").Replace("%USER.CLAN.MEMBERS.MAX%", (data.Clan != null) ? data.Clan.Level.MaxMembers.ToString() : "").Replace("%USER.CLAN.MOTD%", (data.Clan != null) ? data.Clan.MOTD : "").Replace("%USER.VIOLATIONS%", data.Violations.ToString()).Replace("%USER.VIOLATIONDATE%", data.ViolationDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%USER.FIRSTCONNECTDATE%", data.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%USER.LASTCONNECTDATE%", data.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%USER.FIRSTCONNECTIP%", data.FirstConnectIP).Replace("%USER.LASTCONNECTIP%", data.LastConnectIP).Replace("%USER.PREMIUMDATE%", data.PremiumDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%USER.POS%", data.Position.AsString()).Replace("%USER.POS.X%", data.Position.x.ToString()).Replace("%USER.POS.Y%", data.Position.y.ToString()).Replace("%USER.POS.Z%", data.Position.z.ToString()).Replace("%USER.PING%", (data.AveragePing > 0) ? data.AveragePing.ToString() : "");
                                        UserEconomy economy = Economy.Get(data.SteamID);
                                        if (economy != null)
                                        {
                                            ulong num = 0L;
                                            str4 = str4.Replace("%USER.BALANCE%", economy.Balance.ToString()).Replace("%USER.MONEY%", economy.Balance.ToString()).Replace("%USER.KILLED.ANIMALS%", economy.AnimalsKilled.ToString());
                                            num = (ulong) (0L + economy.AnimalsKilled);
                                            str4 = str4.Replace("%USER.KILLED.MUTANTS%", economy.MutantsKilled.ToString());
                                            num += (ulong)economy.MutantsKilled;
                                            str4 = str4.Replace("%USER.KILLED.NPC%", num.ToString()).Replace("%USER.KILLED.PLAYERS%", economy.PlayersKilled.ToString());
                                            num += (ulong)economy.PlayersKilled;
                                            str4 = str4.Replace("%USER.KILLED%", num.ToString()).Replace("%USER.DEATHS%", economy.Deaths.ToString());
                                        }
                                        list.Add(str4);
                                    }
                                }
                            }
                            list2.Clear();
                        }
                        else if (list2.Count > 0)
                        {
                            list2.Add(item);
                        }
                        else
                        {
                            if (item.Contains("<PLAYERLIST>"))
                            {
                                list3.Add(item);
                            }
                            if (item.Contains("</PLAYERLIST>"))
                            {
                                list3.Add(item);
                                foreach (PlayerClient client in PlayerClient.All)
                                {
                                    foreach (string str5 in list3)
                                    {
                                        if (!str5.Replace("<PLAYERLIST>", "").Replace("</PLAYERLIST>", "").IsEmpty())
                                        {
                                            string str6 = str5.Replace("%PLAYER.NUMBER%", client.netPlayer.id.ToString());
                                            UserData bySteamID = Users.GetBySteamID(client.userID);
                                            if (bySteamID != null)
                                            {
                                                str6 = str6.Replace("%PLAYER.STEAM_ID%", bySteamID.SteamID.ToString()).Replace("%PLAYER.USERNAME%", bySteamID.Username).Replace("%PLAYER.PASSWORD%", bySteamID.Password).Replace("%PLAYER.COMMENTS%", bySteamID.Comments).Replace("%PLAYER.RANK%", bySteamID.Rank.ToString()).Replace("%PLAYER.FLAGS%", bySteamID.Flags.ToString()).Replace("%PLAYER.ZONE%", (bySteamID.Zone != null) ? bySteamID.Zone.Name : "").Replace("%PLAYER.CLAN%", (bySteamID.Clan != null) ? bySteamID.Clan.Name : "").Replace("%PLAYER.CLAN.ABBR%", (bySteamID.Clan != null) ? bySteamID.Clan.Abbr : "").Replace("%PLAYER.CLAN.CREATED%", (bySteamID.Clan != null) ? bySteamID.Clan.Created.ToString("MM/dd/yyyy HH:mm:ss") : "").Replace("%PLAYER.CLAN.LEVEL%", (bySteamID.Clan != null) ? bySteamID.Clan.Level.ToString() : "").Replace("%PLAYER.CLAN.BALANCE%", (bySteamID.Clan != null) ? bySteamID.Clan.Balance.ToString() : "").Replace("%PLAYER.CLAN.EXPERIENCE%", (bySteamID.Clan != null) ? bySteamID.Clan.Experience.ToString() : "").Replace("%PLAYER.CLAN.EXP%", (bySteamID.Clan != null) ? bySteamID.Clan.Experience.ToString() : "").Replace("%PLAYER.CLAN.MEMBERS.COUNT%", (bySteamID.Clan != null) ? bySteamID.Clan.Members.Count.ToString() : "").Replace("%PLAYER.CLAN.MEMBERS.MAX%", (bySteamID.Clan != null) ? bySteamID.Clan.Level.MaxMembers.ToString() : "").Replace("%PLAYER.CLAN.MOTD%", (bySteamID.Clan != null) ? bySteamID.Clan.MOTD : "").Replace("%PLAYER.VIOLATIONS%", bySteamID.Violations.ToString()).Replace("%PLAYER.VIOLATIONDATE%", bySteamID.ViolationDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%PLAYER.FIRSTCONNECTDATE%", bySteamID.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%PLAYER.LASTCONNECTDATE%", bySteamID.FirstConnectDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%PLAYER.FIRSTCONNECTIP%", bySteamID.FirstConnectIP).Replace("%PLAYER.LASTCONNECTIP%", bySteamID.LastConnectIP).Replace("%PLAYER.PREMIUMDATE%", bySteamID.PremiumDate.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%PLAYER.POS%", bySteamID.Position.AsString()).Replace("%PLAYER.POS.X%", bySteamID.Position.x.ToString()).Replace("%PLAYER.POS.Y%", bySteamID.Position.y.ToString()).Replace("%PLAYER.POS.Z%", bySteamID.Position.z.ToString()).Replace("%PLAYER.PING%", (bySteamID.AveragePing > 0) ? bySteamID.AveragePing.ToString() : "");
                                            }
                                            UserEconomy economy2 = Economy.Get(client.userID);
                                            if (economy2 != null)
                                            {
                                                ulong num2 = 0L;
                                                str6 = str6.Replace("%PLAYER.BALANCE%", economy2.Balance.ToString()).Replace("%PLAYER.MONEY%", economy2.Balance.ToString()).Replace("%PLAYER.KILLED.ANIMALS%", economy2.AnimalsKilled.ToString());
                                                num2 = (ulong) (0L + economy2.AnimalsKilled);
                                                str6 = str6.Replace("%PLAYER.KILLED.MUTANTS%", economy2.MutantsKilled.ToString());
                                                num2 += (ulong)economy2.MutantsKilled;
                                                str6 = str6.Replace("%PLAYER.KILLED.NPC%", num2.ToString()).Replace("%PLAYER.KILLED.PLAYERS%", economy2.PlayersKilled.ToString());
                                                num2 += (ulong)economy2.PlayersKilled;
                                                str6 = str6.Replace("%PLAYER.KILLED%", num2.ToString()).Replace("%PLAYER.DEATHS%", economy2.Deaths.ToString());
                                            }
                                            list.Add(str6);
                                        }
                                    }
                                }
                                list3.Clear();
                            }
                            else if (list3.Count > 0)
                            {
                                list3.Add(item);
                            }
                            else
                            {
                                if (item.Contains("<CLANLIST>"))
                                {
                                    list5.Add(item);
                                }
                                if (item.Contains("</CLANLIST>"))
                                {
                                    list5.Add(item);
                                    using (Dictionary<uint, ClanData>.KeyCollection.Enumerator enumerator5 = Clans.Database.Keys.GetEnumerator())
                                    {
                                        while (enumerator5.MoveNext())
                                        {
                                            Predicate<ClanLevel> match = null;
                                            Class41 class2 = new Class41 {
                                                uint_0 = enumerator5.Current
                                            };
                                            foreach (string str7 in list5)
                                            {
                                                if (!str7.Replace("<CLANLIST>", "").Replace("</CLANLIST>", "").IsEmpty())
                                                {
                                                    string str8 = str7.Replace("%CLAN.ID%", class2.uint_0.ToHEX(true)).Replace("%CLAN.NAME%", Clans.Database[class2.uint_0].Name).Replace("%CLAN.ABBR%", Clans.Database[class2.uint_0].Abbr).Replace("%CLAN.CREATED%", Clans.Database[class2.uint_0].Created.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%CLAN.FLAGS%", Clans.Database[class2.uint_0].Flags.ToString()).Replace("%CLAN.BALANCE%", Clans.Database[class2.uint_0].Balance.ToString()).Replace("%CLAN.EXPERIENCE%", Clans.Database[class2.uint_0].Experience.ToString()).Replace("%CLAN.EXP%", Clans.Database[class2.uint_0].Experience.ToString()).Replace("%CLAN.TAX%", Clans.Database[class2.uint_0].Tax.ToString()).Replace("%CLAN.MOTD%", Clans.Database[class2.uint_0].MOTD).Replace("%CLAN.LOCATION%", Clans.Database[class2.uint_0].Location.AsString()).Replace("%CLAN.LOCATION.X%", Clans.Database[class2.uint_0].Location.x.ToString()).Replace("%CLAN.LOCATION.Y%", Clans.Database[class2.uint_0].Location.y.ToString()).Replace("%CLAN.LOCATION.Z%", Clans.Database[class2.uint_0].Location.z.ToString()).Replace("%CLAN.MEMBERS.MAX%", Clans.Database[class2.uint_0].Level.MaxMembers.ToString()).Replace("%CLAN.MEMBERS.COUNT%", Clans.Database[class2.uint_0].Members.Count.ToString()).Replace("%CLAN.MEMBERS.ONLINE%", Clans.Database[class2.uint_0].Online.ToString()).Replace("%CLAN.LEVEL%", Clans.Database[class2.uint_0].Level.Id.ToString()).Replace("%CLAN.LEVEL.TAX%", Clans.Database[class2.uint_0].Level.CurrencyTax.ToString()).Replace("%CLAN.LEVEL.CAN_MOTD%", Clans.Database[class2.uint_0].Level.FlagMotd ? "YES" : "NO").Replace("%CLAN.LEVEL.CAN_ABBR%", Clans.Database[class2.uint_0].Level.FlagAbbr ? "YES" : "NO").Replace("%CLAN.LEVEL.CAN_FFIRE%", Clans.Database[class2.uint_0].Level.FlagFFire ? "YES" : "NO").Replace("%CLAN.LEVEL.CAN_TAX%", Clans.Database[class2.uint_0].Level.FlagTax ? "YES" : "NO").Replace("%CLAN.LEVEL.CAN_HOUSE%", Clans.Database[class2.uint_0].Level.FlagHouse ? "YES" : "NO").Replace("%CLAN.LEVEL.CAN_DECLARE%", Clans.Database[class2.uint_0].Level.FlagDeclare ? "YES" : "NO").Replace("%CLAN.LEVEL.MAXMEMBERS%", Clans.Database[class2.uint_0].Level.MaxMembers.ToString()).Replace("%CLAN.LEVEL.BONUSCRAFTINGSPEED%", Clans.Database[class2.uint_0].Level.BonusCraftingSpeed.ToString()).Replace("%CLAN.LEVEL.BONUSGATHERINGANIMAL%", Clans.Database[class2.uint_0].Level.BonusGatheringAnimal.ToString()).Replace("%CLAN.LEVEL.BONUSGATHERINGROCK%", Clans.Database[class2.uint_0].Level.BonusGatheringRock.ToString()).Replace("%CLAN.LEVEL.BONUSGATHERINGWOOD%", Clans.Database[class2.uint_0].Level.BonusGatheringWood.ToString()).Replace("%CLAN.LEVEL.BONUSMEMBERSPAYMURDER%", Clans.Database[class2.uint_0].Level.BonusMembersPayMurder.ToString()).Replace("%CLAN.LEVEL.BONUSMEMBERSDEFENSE%", Clans.Database[class2.uint_0].Level.BonusMembersDefense.ToString()).Replace("%CLAN.LEVEL.BONUSMEMBERSDAMAGE%", Clans.Database[class2.uint_0].Level.BonusMembersDamage.ToString());
                                                    UserData data3 = Users.GetBySteamID(Clans.Database[class2.uint_0].LeaderID);
                                                    str8 = str8.Replace("%CLAN.LEADER%", (data3 != null) ? "YES" : "NO").Replace("%CLAN.LEADER.STEAM_ID%", (data3 == null) ? "" : data3.SteamID.ToString()).Replace("%CLAN.LEADER.USERNAME%", (data3 == null) ? "" : data3.Username).Replace("%CLAN.LEADER.FLAGS%", (data3 == null) ? "" : data3.Flags.ToString()).Replace("%CLAN.WAR.COUNT%", Clans.Database[class2.uint_0].Hostile.Count.ToString()).Replace("%CLAN.HOSTILE.COUNT%", Clans.Database[class2.uint_0].Hostile.Count.ToString());
                                                    if (match == null)
                                                    {
                                                        match = new Predicate<ClanLevel>(class2.method_0);
                                                    }
                                                    ClanLevel level = Clans.Levels.Find(match);
                                                    str8 = str8.Replace("%CLAN.NEXTLEVEL%", (level == null) ? "" : level.Id.ToString()).Replace("%CLAN.NEXTLEVEL.TAX%", (level == null) ? "" : level.CurrencyTax.ToString()).Replace("%CLAN.LEVEL.CAN_MOTD%", (level == null) ? "" : (level.FlagMotd ? "YES" : "NO")).Replace("%CLAN.LEVEL.CAN_ABBR%", (level == null) ? "" : (level.FlagAbbr ? "YES" : "NO")).Replace("%CLAN.LEVEL.CAN_FFIRE%", (level == null) ? "" : (level.FlagFFire ? "YES" : "NO")).Replace("%CLAN.LEVEL.CAN_TAX%", (level == null) ? "" : (level.FlagTax ? "YES" : "NO")).Replace("%CLAN.LEVEL.CAN_HOUSE%", (level == null) ? "" : (level.FlagHouse ? "YES" : "NO")).Replace("%CLAN.LEVEL.CAN_DECLARE%", (level == null) ? "" : (level.FlagDeclare ? "YES" : "NO")).Replace("%CLAN.NEXTLEVEL.MAXMEMBERS%", (level == null) ? "" : level.MaxMembers.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSCRAFTINGSPEED%", (level == null) ? "" : level.BonusCraftingSpeed.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSGATHERINGANIMAL%", (level == null) ? "" : level.BonusGatheringAnimal.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSGATHERINGROCK%", (level == null) ? "" : level.BonusGatheringRock.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSGATHERINGWOOD%", (level == null) ? "" : level.BonusGatheringWood.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSMEMBERSPAYMURDER%", (level == null) ? "" : level.BonusMembersPayMurder.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSMEMBERSDEFENSE%", (level == null) ? "" : level.BonusMembersDefense.ToString()).Replace("%CLAN.NEXTLEVEL.BONUSMEMBERSDAMAGE%", (level == null) ? "" : level.BonusMembersDamage.ToString());
                                                    list.Add(str8);
                                                }
                                            }
                                        }
                                    }
                                    list5.Clear();
                                }
                                else if (list5.Count > 0)
                                {
                                    list5.Add(item);
                                }
                                else
                                {
                                    if (item.Contains("<BANLIST>"))
                                    {
                                        list4.Add(item);
                                    }
                                    if (item.Contains("</BANLIST>"))
                                    {
                                        list4.Add(item);
                                        foreach (ulong num3 in Banned.Database.Keys)
                                        {
                                            foreach (string str9 in list4)
                                            {
                                                if (!str9.Replace("<BANLIST>", "").Replace("</BANLIST>", "").IsEmpty())
                                                {
                                                    string str10 = str9.Replace("%BANNED.STEAM_ID%", num3.ToString());
                                                    UserData data4 = Users.GetBySteamID(num3);
                                                    str10 = str10.Replace("%BANNED.USERNAME%", (data4 == null) ? "" : data4.Username).Replace("%BANNED.IP%", Banned.Database[num3].IP).Replace("%BANNED.DATE%", Banned.Database[num3].Time.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%BANNED.PERIOD%", Banned.Database[num3].Period.ToString("MM/dd/yyyy HH:mm:ss")).Replace("%BANNED.REASON%", Banned.Database[num3].Reason).Replace("%BANNED.DETAILS%", Banned.Database[num3].Details);
                                                    list.Add(str10);
                                                }
                                            }
                                        }
                                        list4.Clear();
                                    }
                                    else if (list4.Count > 0)
                                    {
                                        list4.Add(item);
                                    }
                                    else
                                    {
                                        list.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    using (StreamWriter writer = System.IO.File.CreateText(targetfile))
                    {
                        foreach (string str11 in list)
                        {
                            writer.WriteLine(str11);
                        }
                    }
                }
            }
        }

        public static System.Collections.Generic.List<string> GetAvailabledCommands(UserData userData)
        {
            int result = 0;
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            NetUser user = NetUser.FindByUserID(userData.SteamID);
            if (user != null)
            {
                foreach (string str in Core.Commands)
                {
                    Class42 class2 = new Class42();
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] strArray = str.Split(new char[] { '=' });
                        if (strArray.Length >= 2)
                        {
                            class2.string_0 = strArray[1].Replace("!", "").Trim();
                            if (!class2.string_0.IsEmpty() && !list.Exists(new Predicate<string>(class2.method_0)))
                            {
                                if (!userData.HasFlag(UserFlags.admin) && !user.admin)
                                {
                                    if ((int.TryParse(strArray[0], out result) && (result == userData.Rank)) && str.Contains("=!"))
                                    {
                                        list.Add(str.Replace("=!", "="));
                                    }
                                    if ((int.TryParse(strArray[0], out result) && (result <= userData.Rank)) && !str.Contains("=!"))
                                    {
                                        list.Add(str);
                                    }
                                }
                                else
                                {
                                    list.Add(str.Replace("=!", "="));
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        public static string GetChatTextColor(string color)
        {
            int num = color.Replace("#", "").Replace("$", "").ToInt32();
            if (num != 0)
            {
                return ("[COLOR#" + num.ToHEX(false) + "]");
            }
            return "";
        }

        public static System.Collections.Generic.List<string> GetConfigSections(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                return null;
            }
            System.Collections.Generic.List<string> list = System.IO.File.ReadAllLines(filename).ToList<string>();
            System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
            string item = null;
            for (int i = 0; i < list.Count; i++)
            {
                string str2 = list[i].Trim();
                if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("//"))
                {
                    if (str2.Contains("//"))
                    {
                        str2 = str2.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if ((!string.IsNullOrEmpty(str2) && str2.StartsWith("[")) && str2.EndsWith("]"))
                    {
                        item = str2.Substring(1, str2.Length - 2).Trim();
                        if (!list2.Contains(item))
                        {
                            list2.Add(item);
                        }
                    }
                }
            }
            return list2;
        }

        public static Dictionary<string, string> GetConfigValues(string filename, string section)
        {
            if (!System.IO.File.Exists(filename))
            {
                return null;
            }
            System.Collections.Generic.List<string> list = System.IO.File.ReadAllLines(filename).ToList<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string str = null;
            for (int i = 0; i < list.Count; i++)
            {
                string str2 = list[i].Trim();
                if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("//"))
                {
                    if (str2.Contains("//"))
                    {
                        str2 = str2.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(str2))
                    {
                        if (str2.StartsWith("[") && str2.EndsWith("]"))
                        {
                            str = str2.Substring(1, str2.Length - 2);
                        }
                        else if (((section == null) || str.Equals(section, StringComparison.CurrentCultureIgnoreCase)) && str2.Contains("="))
                        {
                            string[] strArray = str2.Split(new char[] { '=' });
                            strArray[0] = strArray[0].Trim();
                            strArray[1] = strArray[1].Trim();
                            if (!dictionary.ContainsKey(strArray[0]))
                            {
                                dictionary.Add(strArray[0].Trim(), strArray[1].Trim());
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        public static bool GetEquipedArmor(PlayerClient playerClient, out System.Collections.Generic.List<IInventoryItem> items)
        {
            Inventory component = playerClient.controllable.GetComponent<Inventory>();
            items = new System.Collections.Generic.List<IInventoryItem>();
            for (int i = 0; i < (component.slotCount - 1); i++)
            {
                IInventoryItem item;
                if (component.GetItem(i, out item))
                {
                    try
                    {
                        if (((i > 0x23) && (i < 40)) && (item != null))
                        {
                            items.Add(item);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return (items.Count<IInventoryItem>() > 0);
        }

        public static Ray GetEyesRay(Character character)
        {
            if (character == null)
            {
                return new Ray();
            }
            Vector3 position = character.transform.position;
            Vector3 direction = character.eyesRay.direction;
            position.y += character.stateFlags.crouch ? 1.1f : 1.6f;
            return new Ray(position, direction);
        }

        public static Ray GetEyesRay(Controllable controllable)
        {
            if (controllable == null)
            {
                return new Ray();
            }
            return GetEyesRay(controllable.character);
        }

        public static Ray GetEyesRay(NetUser player)
        {
            if (player == null)
            {
                return new Ray();
            }
            return GetEyesRay(player.playerClient);
        }

        public static Ray GetEyesRay(PlayerClient player)
        {
            if (player == null)
            {
                return new Ray();
            }
            return GetEyesRay(player.controllable);
        }

        public static string GetLastSaveFile()
        {
            FileInfo info = null;
            string autoSavePath = ServerSaveManager.autoSavePath;
            if (System.IO.File.Exists(autoSavePath))
            {
                info = new FileInfo(autoSavePath);
            }
            if ((info == null) || (info.Length == 0L))
            {
                for (int i = 0; i < 20; i++)
                {
                    autoSavePath = ServerSaveManager.autoSavePath + ".old." + i;
                    if (System.IO.File.Exists(autoSavePath) && (new FileInfo(autoSavePath).Length > 0L))
                    {
                        return autoSavePath;
                    }
                }
            }
            return null;
        }

        public static GameObject GetLineObject(Vector3 start, Vector3 end, out Vector3 point, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            RaycastHit hit;
            bool flag;
            MeshBatchInstance instance;
            point = Vector3.zero;
            if (!Facepunch.MeshBatch.MeshBatchPhysics.Linecast(start, end, out hit, layerMask, out flag, out instance))
            {
                return null;
            }
            IDMain main = flag ? instance.idMain : IDBase.GetMain(hit.collider);
            point = hit.point;
            if (main == null)
            {
                return hit.collider.gameObject;
            }
            return main.gameObject;
        }

        public static GameObject[] GetLineObjects(Ray ray, [Optional, DefaultParameterValue(300f)] float distance, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>();
            foreach (RaycastHit hit in Physics.RaycastAll(ray, distance, layerMask))
            {
                list.Add(IDBase.Get(hit.collider).idMain.gameObject);
            }
            return list.ToArray();
        }

        public static GameObject[] GetLineObjects(Vector3 start, Vector3 end, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            Vector3 vector = end - start;
            Ray ray = new Ray(start, vector.normalized);
            return GetLineObjects(ray, Vector3.Distance(start, end), layerMask);
        }

        public static GameObject GetLookObject(Character character, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            if (character == null)
            {
                return null;
            }
            Vector3 position = character.transform.position;
            Vector3 direction = character.eyesRay.direction;
            position.y += character.stateFlags.crouch ? 1f : 1.85f;
            return GetLookObject(new Ray(position, direction), 300f, -1);
        }

        public static GameObject GetLookObject(Controllable controllable, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            if (controllable == null)
            {
                return null;
            }
            return GetLookObject(controllable.character, -1);
        }

        public static GameObject GetLookObject(NetUser player, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            if (player == null)
            {
                return null;
            }
            return GetLookObject(player.playerClient, -1);
        }

        public static GameObject GetLookObject(PlayerClient player, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            if (player == null)
            {
                return null;
            }
            return GetLookObject(player.controllable, -1);
        }

        public static GameObject GetLookObject(Ray ray, [Optional, DefaultParameterValue(300f)] float distance, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            Vector3 zero = Vector3.zero;
            return GetLookObject(ray, out zero, distance, layerMask);
        }

        public static GameObject GetLookObject(Ray ray, out Vector3 point, [Optional, DefaultParameterValue(300f)] float distance, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            RaycastHit hit;
            bool flag;
            MeshBatchInstance instance;
            point = Vector3.zero;
            if (!Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray, out hit, distance, layerMask, out flag, out instance))
            {
                return null;
            }
            IDMain main = flag ? instance.idMain : IDBase.GetMain(hit.collider);
            point = hit.point;
            if (main == null)
            {
                return hit.collider.gameObject;
            }
            return main.gameObject;
        }

        public static Ray GetLookRay(Character character)
        {
            if (character == null)
            {
                return new Ray();
            }
            Vector3 position = character.transform.position;
            Vector3 direction = character.eyesRay.direction;
            position.y += character.stateFlags.crouch ? 0.85f : 1.65f;
            return new Ray(position, direction);
        }

        public static Ray GetLookRay(Controllable controllable)
        {
            if (controllable == null)
            {
                return new Ray();
            }
            return GetLookRay(controllable.character);
        }

        public static Ray GetLookRay(NetUser player)
        {
            if (player == null)
            {
                return new Ray();
            }
            return GetLookRay(player.playerClient);
        }

        public static Ray GetLookRay(PlayerClient player)
        {
            if (player == null)
            {
                return new Ray();
            }
            return GetLookRay(player.controllable);
        }

        public static byte[] GetMD5(string input)
        {
            return MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
        }

        public static NetUser GetNetUser(string Value)
        {
            PlayerClient playerClient = GetPlayerClient(Value);
            if (playerClient == null)
            {
                return null;
            }
            return playerClient.netUser;
        }

        public static JsonData GetPlayerBans(string steam_id)
        {
            WebRequest request = WebRequest.Create("http://api.steampowered.com/ISteamUser/GetPlayerBans/v1/?key=" + Core.SteamAPIKey + "&steamids=" + steam_id);
            request.Timeout = 0x1388;
            JsonData data = JsonMapper.ToObject(new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8).ReadToEnd());
            if (data["players"].Count > 0)
            {
                return data["players"][0];
            }
            return null;
        }

        public static PlayerClient GetPlayerClient(string Value)
        {
            PlayerClient client;
            ulong num;
            Class43 class2 = new Class43 {
                string_0 = Value.Replace("*", "")
            };
            if (ulong.TryParse(Value, out num) && PlayerClient.FindByUserID(num, out client))
            {
                return client;
            }
            class2.stringComparison_0 = Users.UniqueNames ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (Value.StartsWith("*") && Value.EndsWith("*"))
            {
                return PlayerClient.All.Find(new Predicate<PlayerClient>(class2.method_0));
            }
            if (Value.StartsWith("*"))
            {
                return PlayerClient.All.Find(new Predicate<PlayerClient>(class2.method_1));
            }
            if (Value.EndsWith("*"))
            {
                return PlayerClient.All.Find(new Predicate<PlayerClient>(class2.method_2));
            }
            return PlayerClient.All.Find(new Predicate<PlayerClient>(class2.method_3));
        }

        public static PlayerClient GetPlayerClient(ulong SteamID)
        {
            PlayerClient client;
            PlayerClient.FindByUserID(SteamID, out client);
            return client;
        }

        public static PlayerClient GetPlayerClient(uLink.NetworkPlayer player)
        {
            PlayerClient client;
            PlayerClient.Find(player, out client, false);
            return client;
        }

        public static int GetPlayerComponents(ulong userID)
        {
            int num = 0;
            foreach (StructureMaster master in StructureMaster.AllStructures)
            {
                if (master.ownerID == userID)
                {
                    num += master._structureComponents.Count;
                }
            }
            return num;
        }

        public static int GetPlayerObjects(ulong userID)
        {
            Class44 class2 = new Class44 {
                ulong_0 = userID
            };
            int num = Enumerable.Count<DeployableObject>(UnityEngine.Object.FindObjectsOfType<DeployableObject>(), new Func<DeployableObject, bool>(class2.method_0));
            foreach (StructureMaster master in StructureMaster.AllStructures)
            {
                if (master.ownerID == class2.ulong_0)
                {
                    num += master._structureComponents.Count;
                }
            }
            return num;
        }

        public static System.Collections.Generic.List<Vector3> GetPlayerSpawns(NetUser netUser, [Optional, DefaultParameterValue(true)] bool Valid)
        {
            return GetPlayerSpawns(netUser.userID, Valid);
        }

        public static System.Collections.Generic.List<Vector3> GetPlayerSpawns(PlayerClient player, [Optional, DefaultParameterValue(true)] bool Valid)
        {
            return GetPlayerSpawns(player.userID, Valid);
        }

        public static System.Collections.Generic.List<Vector3> GetPlayerSpawns(ulong userID, [Optional, DefaultParameterValue(true)] bool Valid)
        {
            System.Collections.Generic.List<Vector3> list = new System.Collections.Generic.List<Vector3>();
            foreach (DeployableObject obj2 in RustServerManagement.Get().playerSpawns)
            {
                if (obj2.ownerID == userID)
                {
                    DeployedRespawn component = obj2.GetComponent<DeployedRespawn>();
                    if ((component != null) && (!Valid || component.IsValidToSpawn()))
                    {
                        list.Add(component.GetSpawnPos() + new Vector3(0f, 0.5f, 0f));
                    }
                }
            }
            return list;
        }

        public static ulong GetProcessorId()
        {
            byte[] buffer4;
            int num;
            byte[] buffer = new byte[] { 
                0x55, 0x89, 0xe5, 0x57, 0x8b, 0x7d, 0x10, 0x6a, 1, 0x58, 0x53, 15, 0xa2, 0x89, 7, 0x89, 
                0x57, 4, 0x5b, 0x5f, 0x89, 0xec, 0x5d, 0xc2, 0x10, 0
             };
            byte[] buffer2 = new byte[] { 
                0x53, 0x48, 0xc7, 0xc0, 1, 0, 0, 0, 15, 0xa2, 0x41, 0x89, 0, 0x41, 0x89, 80, 
                4, 0x5b, 0xc3
             };
            byte[] buffer3 = new byte[8];
            if (IntPtr.Size == 8)
            {
                buffer4 = buffer2;
            }
            else
            {
                buffer4 = buffer;
            }
            IntPtr size = new IntPtr(buffer4.Length);
            if (!VirtualProtect(buffer4, size, 0x40, out num))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            size = new IntPtr(buffer3.Length);
            if (CallWindowProcW(buffer4, IntPtr.Zero, 0, buffer3, size) != IntPtr.Zero)
            {
                return BitConverter.ToUInt64(buffer3, 0);
            }
            return 0L;
        }

        [DllImport("KERNEL32.DLL")]
        private static extern uint GetSystemDefaultLCID();
        public static uint GetSystemLocaleID()
        {
            return GetSystemDefaultLCID();
        }

        public static int GiveItem(PlayerClient player, ItemDataBlock itemData, [Optional, DefaultParameterValue(1)] int quantity, [Optional, DefaultParameterValue(-1)] int modSlots)
        {
            PlayerInventory component = player.controllable.GetComponent<PlayerInventory>();
            Inventory.Slot.Preference slotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, itemData.IsSplittable(), Inventory.Slot.Kind.Belt);
            return GiveItem(component, itemData, slotPreference, quantity, modSlots);
        }

        public static int GiveItem(PlayerClient player, string itemName, [Optional, DefaultParameterValue(1)] int quantity, [Optional, DefaultParameterValue(-1)] int slots)
        {
            return GiveItem(player, DatablockDictionary.GetByName(itemName), quantity, slots);
        }

        public static int GiveItem(PlayerInventory inventory, ItemDataBlock itemData, Inventory.Slot.Preference slotPreference, [Optional, DefaultParameterValue(1)] int quantity, [Optional, DefaultParameterValue(-1)] int modSlots)
        {
            int num = 0;
            if ((itemData != null) && (inventory != null))
            {
                if (itemData.IsSplittable())
                {
                    return (num + (quantity - inventory.AddItemAmount(itemData, quantity, Inventory.AmountMode.Default, slotPreference)));
                }
                int maxEligableSlots = itemData.GetMaxEligableSlots();
                for (int i = 0; i < quantity; i++)
                {
                    IInventoryItem objA = inventory.AddItem(itemData, slotPreference, itemData._spawnUsesMax);
                    if (object.ReferenceEquals(objA, null))
                    {
                        return num;
                    }
                    num++;
                    if ((modSlots != -1) && (maxEligableSlots != 0))
                    {
                        IHeldItem item2 = objA as IHeldItem;
                        if (!object.ReferenceEquals(item2, null))
                        {
                            item2.SetTotalModSlotCount(Mathf.Min(modSlots, maxEligableSlots));
                        }
                    }
                }
            }
            return num;
        }

        public static RustExtended.IniFile IniFile(string filename)
        {
            return new RustExtended.IniFile(filename);
        }

        public static void Initialize()
        {
            if (!Directory.Exists(Core.LogsPath))
            {
                Directory.CreateDirectory(Core.LogsPath);
            }
            string lastSaveFile = GetLastSaveFile();
            if (lastSaveFile != null)
            {
                Debug.LogError("Bad save or not found " + ServerSaveManager.autoSavePath);
                Debug.LogError("Restored save from " + lastSaveFile);
                System.IO.File.Copy(lastSaveFile, ServerSaveManager.autoSavePath, true);
            }
            RustExtended.Method.Initialize();
            Core.BetaVersion = true;
            Core.ServerIP = "127.0.0.1";
            Core.ExternalIP = "123.123.123.123";
        }

        public static string Int32ToString(int[] values)
        {
            string str = "";
            foreach (int num in values)
            {
                if (num > 0)
                {
                    byte[] bytes = BitConverter.GetBytes(num);
                    str = str + Encoding.Unicode.GetString(bytes);
                }
            }
            return str.Trim(new char[1]);
        }

        public static System.Collections.Generic.List<IInventoryItem> InventoryGetItems(Inventory inventory)
        {
            System.Collections.Generic.List<IInventoryItem> list = new System.Collections.Generic.List<IInventoryItem>();
            Inventory.OccupiedIterator occupiedIterator = inventory.occupiedIterator;
            while (occupiedIterator.Next())
            {
                list.Add(occupiedIterator.item);
            }
            return list;
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

        public static void InventoryItemRemove(PlayerClient player, ItemDataBlock Item)
        {
            Inventory component = player.controllable.GetComponent<Inventory>();
            if (((component != null) && (Item != null)) && !Item.transferable)
            {
                for (IInventoryItem item = component.FindItem(Item); !object.ReferenceEquals(item, null); item = component.FindItem(Item))
                {
                    component.RemoveItem(item);
                }
            }
        }

        public static int InventoryItemRemove(Inventory inventory, ItemDataBlock datablock, int quantity)
        {
            int num = 0;
            while (num < quantity)
            {
                IInventoryItem item = inventory.FindItem(datablock);
                if (item == null)
                {
                    return num;
                }
                if (!item.datablock.IsSplittable())
                {
                    num++;
                    inventory.RemoveItem(item);
                }
                else
                {
                    int num2 = quantity - num;
                    if (item.uses > num2)
                    {
                        num += num2;
                        item.SetUses(item.uses - num2);
                        continue;
                    }
                    num += item.uses;
                    inventory.RemoveItem(item);
                }
            }
            return num;
        }

        public static void Log(string msg, [Optional, DefaultParameterValue(true)] bool inConsole)
        {
            if ((RustLogFile != RustLogFileName) && (RustLogStream != null))
            {
                RustLogStream.Close();
                RustLogStream = null;
            }
            if (RustLogStream == null)
            {
                RustLogStream = new StreamWriter(new FileStream(RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (RustLogStream != null)
            {
                RustLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
                RustLogStream.Flush();
                System.IO.File.SetLastWriteTime(RustLogFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.Print(msg, false);
            }
        }

        public static void LogChat(string msg, [Optional, DefaultParameterValue(false)] bool inConsole)
        {
            if ((ChatLogFile != ChatLogFileName) && (ChatLogStream != null))
            {
                ChatLogStream.Close();
                ChatLogStream = null;
            }
            if (ChatLogStream == null)
            {
                ChatLogStream = new StreamWriter(new FileStream(ChatLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (ChatLogStream != null)
            {
                ChatLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
                ChatLogStream.Flush();
                System.IO.File.SetLastWriteTime(ChatLogFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.Print(msg, false);
            }
        }

        public static void LogError(string msg, [Optional, DefaultParameterValue(true)] bool inConsole)
        {
            if ((RustLogFile != RustLogFileName) && (RustLogStream != null))
            {
                RustLogStream.Close();
                RustLogStream = null;
            }
            if (RustLogStream == null)
            {
                RustLogStream = new StreamWriter(new FileStream(RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (RustLogStream != null)
            {
                RustLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":[ERROR]: " + msg);
                RustLogStream.Flush();
                System.IO.File.SetLastWriteTime(RustLogFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.PrintError(msg, false);
            }
        }

        public static void LogSQL(string msg, [Optional, DefaultParameterValue(false)] bool inConsole)
        {
            if ((ServSQLFile != ServSQLFileName) && (ServSQLStream != null))
            {
                ServSQLStream.Close();
                ServSQLStream = null;
            }
            if (ServSQLStream == null)
            {
                ServSQLStream = new StreamWriter(new FileStream(ServSQLFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (ServSQLStream != null)
            {
                ServSQLStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
                ServSQLStream.Flush();
                System.IO.File.SetLastWriteTime(ServSQLFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.Print("MYSQL: " + msg, false);
            }
        }

        public static void LogSQLError(string msg, [Optional, DefaultParameterValue(true)] bool inConsole)
        {
            if ((ServSQLFile != ServSQLFileName) && (ServSQLStream != null))
            {
                ServSQLStream.Close();
                ServSQLStream = null;
            }
            if (ServSQLStream == null)
            {
                ServSQLStream = new StreamWriter(new FileStream(ServSQLFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (ServSQLStream != null)
            {
                ServSQLStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":[ERROR]: " + msg);
                ServSQLStream.Flush();
                System.IO.File.SetLastWriteTime(ServSQLFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.PrintError("MYSQL ERROR: " + msg, false);
            }
        }

        public static void LogSQLWarning(string msg, [Optional, DefaultParameterValue(true)] bool inConsole)
        {
            if ((ServSQLFile != ServSQLFileName) && (ServSQLStream != null))
            {
                ServSQLStream.Close();
                ServSQLStream = null;
            }
            if (ServSQLStream == null)
            {
                ServSQLStream = new StreamWriter(new FileStream(ServSQLFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (ServSQLStream != null)
            {
                ServSQLStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
                ServSQLStream.Flush();
                System.IO.File.SetLastWriteTime(ServSQLFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.PrintWarning("MYSQL: " + msg, false);
            }
        }

        public static void LogWarning(string msg, [Optional, DefaultParameterValue(true)] bool inConsole)
        {
            if ((RustLogFile != RustLogFileName) && (RustLogStream != null))
            {
                RustLogStream.Close();
                RustLogStream = null;
            }
            if (RustLogStream == null)
            {
                RustLogStream = new StreamWriter(new FileStream(RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            }
            if (RustLogStream != null)
            {
                RustLogStream.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
                RustLogStream.Flush();
                System.IO.File.SetLastWriteTime(RustLogFile, DateTime.Now);
            }
            if (inConsole)
            {
                ConsoleSystem.PrintWarning(msg, false);
            }
        }

        public static string NiceName(string input)
        {
            input = input.Replace("_A", "").Replace("A(Clone)", "").Replace("(Clone)", "");
            MatchCollection matchs = new Regex("([A-Z]*[^A-Z_]+)", RegexOptions.Compiled).Matches(input);
            string[] strArray = new string[matchs.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = matchs[i].Groups[0].Value.Trim();
            }
            return string.Join(" ", strArray);
        }

        public static string ObsceneText(string text)
        {
            string[] strArray = text.Split(new char[] { ' ' });
            for (int i = 0; i < strArray.Length; i++)
            {
                using (System.Collections.Generic.List<string>.Enumerator enumerator = Core.ForbiddenObscene.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                        if (strArray[i].ToUpper().Contains(current))
                        {
                            goto Label_004A;
                        }
                    }
                    continue;
                Label_004A:
                    strArray[i] = new string('*', strArray[i].Length);
                }
            }
            return string.Join(" ", strArray);
        }

        public static string QuoteSafe(string text)
        {
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Trim(new char[] { '"' });
            }
            return (text = "\"" + text.Replace("\"", "\\\"") + "\"");
        }

        public static int RemoveAllObjects(string name)
        {
            int num = 0;
            string str = null;
            string pattern = name.Replace("*", "");
            foreach (IDMain main in UnityEngine.Object.FindObjectsOfType<IDMain>())
            {
                str = NiceName(main.name);
                if (str.Equals(name, StringComparison.CurrentCultureIgnoreCase) && RemoveObject(main.gameObject, false))
                {
                    num++;
                }
                else if (name.Equals("ALL", StringComparison.CurrentCultureIgnoreCase) && RemoveObject(main.gameObject, false))
                {
                    num++;
                }
                else if ((name.Equals("NPC", StringComparison.CurrentCultureIgnoreCase) && (main.GetComponent<Character>() != null)) && RemoveObject(main.gameObject, false))
                {
                    num++;
                }
                else if ((name.Equals("RES", StringComparison.CurrentCultureIgnoreCase) && (main.GetComponent<ResourceTarget>() != null)) && RemoveObject(main.gameObject, false))
                {
                    num++;
                }
                else if ((name.StartsWith("*") && name.EndsWith("*")) && (str.Contains(pattern, true) && RemoveObject(main.gameObject, false)))
                {
                    num++;
                }
                else if ((name.StartsWith("*") && str.EndsWith(pattern, StringComparison.CurrentCultureIgnoreCase)) && RemoveObject(main.gameObject, false))
                {
                    num++;
                }
                else if ((name.EndsWith("*") && str.StartsWith(pattern, StringComparison.CurrentCultureIgnoreCase)) && RemoveObject(main.gameObject, false))
                {
                    num++;
                }
            }
            Log(string.Concat(new object[] { "Removed ", num, " object(s) with name \"", pattern, "\"." }), true);
            return num;
        }

        public static bool RemoveObject(GameObject obj, [Optional, DefaultParameterValue(false)] bool force)
        {
            if (obj.GetComponent<DeployableObject>() != null)
            {
                NetCull.Destroy(obj);
                return true;
            }
            if (obj.GetComponent<LootableObject>() != null)
            {
                NetCull.Destroy(obj);
                return true;
            }
            StructureComponent comp = obj.GetComponent<StructureComponent>();
            if (comp != null)
            {
                comp._master.RemoveComponent(comp);
                NetCull.Destroy(obj);
                return true;
            }
            BasicWildLifeAI component = obj.GetComponent<BasicWildLifeAI>();
            if (component != null)
            {
                int index = WildlifeManager.Data.lifeInstances.IndexOf(component);
                if ((index != -1) && (index < WildlifeManager.Data.lifeInstanceCount))
                {
                    WildlifeManager.Data.lifeInstances.RemoveAt(index);
                    WildlifeManager.Data.lifeInstanceCount--;
                    WildlifeManager.Data.thinkIterator = 0;
                    NetCull.Destroy(obj);
                    return true;
                }
                return false;
            }
            if (obj.GetComponent<ResourceTarget>() != null)
            {
                NetCull.Destroy(obj);
                return true;
            }
            if (force)
            {
                NetCull.Destroy(obj);
                return true;
            }
            return false;
        }

        public static string ReplaceVariables(NetUser netUser, string text, [Optional, DefaultParameterValue(null)] string varFrom, [Optional, DefaultParameterValue("")] string varTo)
        {
            if (!string.IsNullOrEmpty(varFrom) && text.Contains(varFrom))
            {
                text = text.Replace(varFrom, varTo);
            }
            if ((netUser != null) && text.Contains("%USERNAME%"))
            {
                text = text.Replace("%USERNAME%", netUser.displayName);
            }
            if ((netUser != null) && text.Contains("%STEAM_ID%"))
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

        [CompilerGenerated]
        private static bool smethod_0(Assembly assembly_0)
        {
            return (assembly_0.GetName().Name == "Assembly-CSharp");
        }

        [CompilerGenerated]
        private static bool smethod_1(AssemblyName assemblyName_0)
        {
            return (assemblyName_0.Name == "RustExtended");
        }

        [CompilerGenerated]
        private static bool smethod_2(Assembly assembly_0)
        {
            return (assembly_0.GetName().Name == "uLink");
        }

        [CompilerGenerated]
        private static bool smethod_3(AssemblyName assemblyName_0)
        {
            return (assemblyName_0.Name == "RustExtended");
        }

        public static string[] SplitQuotes(string input, [Optional, DefaultParameterValue(' ')] char separator)
        {
            input = input.Replace("\\\"", "&qute;");
            MatchCollection matchs = new Regex("\"([^\"]+)\"|'([^']+)'|([^" + separator + "]+)", RegexOptions.Compiled).Matches(input);
            string[] strArray = new string[matchs.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = matchs[i].Groups[0].Value.Trim(new char[] { ' ', '\t', '"' });
                strArray[i] = strArray[i].Replace("&qute;", "\"");
            }
            return strArray;
        }

        public static int[] StringToInt32(string value)
        {
            int[] array = new int[0];
            if (!string.IsNullOrEmpty(value))
            {
                byte[] bytes = Encoding.Unicode.GetBytes(value);
                Array.Resize<int>(ref array, (bytes.Length / 4) + 1);
                Array.Resize<byte>(ref bytes, array.Length * 4);
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = BitConverter.ToInt32(bytes, i * 4);
                }
            }
            return array;
        }

        public static DateTime StringToTime(string time, DateTime startTime = default(DateTime))
        {
            foreach (Match match in Regex.Matches(time, @"(\d+\s*(y|M|d|h|m|s))"))
            {
                if (match.Value.EndsWith("y"))
                {
                    startTime = startTime.AddYears(int.Parse(match.Value.Trim(new char[] { 'y' })));
                }
                if (match.Value.EndsWith("M"))
                {
                    startTime = startTime.AddMonths(int.Parse(match.Value.Trim(new char[] { 'M' })));
                }
                if (match.Value.EndsWith("d"))
                {
                    startTime = startTime.AddDays(double.Parse(match.Value.Trim(new char[] { 'd' })));
                }
                if (match.Value.EndsWith("h"))
                {
                    startTime = startTime.AddHours(double.Parse(match.Value.Trim(new char[] { 'h' })));
                }
                if (match.Value.EndsWith("m"))
                {
                    startTime = startTime.AddMinutes(double.Parse(match.Value.Trim(new char[] { 'm' })));
                }
                if (match.Value.EndsWith("s"))
                {
                    startTime = startTime.AddSeconds(double.Parse(match.Value.Trim(new char[] { 's' })));
                }
            }
            return startTime;
        }
        /*
        public static NetUser TeleportTo(NetUser netuser, Vector3 position)
        {
            Character character;
            if (Character.FindByUser(netuser.userID, out character))
            {
                if ((float.IsNaN(position.x) || float.IsNaN(position.y)) || float.IsNaN(position.z))
                {
                    return netuser;
                }
                if ((position == Vector3.zero) || object.Equals(character.transform.position, position))
                {
                    return netuser;
                }
                if (character.transform.position == position)
                {
                    return netuser;
                }
                float num = Vector3.Distance(character.transform.position, position);
                float y = Mathf.Round(num / 1000f);
                if (y > 5f)
                {
                    y = 5f;
                }
                position += new Vector3(0f, y, 0f);
                LogChat(string.Concat(new object[] { "User [", netuser.displayName, ":", netuser.userID, "] teleported from ", character.transform.position, " to ", position, " (distance: ", num, "m, lifted: ", y, "m)" }), false);
                RustServerManagement.Get().TeleportPlayerToWorld(netuser.networkPlayer, position);
                netuser.truthDetector.NoteTeleported(position, 0.0);
            }
            return netuser;
        }
        */
        public static void TeleportTo(NetUser netUser, Vector3 position)
        {
            Character character;
            if (((Character.FindByUser(netUser.userID, out character) && (netUser.playerClient != null)) && (((position != Vector3.zero) && !float.IsNaN(position.x)) && (!float.IsNaN(position.y) && !float.IsNaN(position.z)))) && ((netUser.playerClient.controllable != null) && (character.transform.position != position)))
            {
                float num = Vector3.Distance(character.transform.position, position);
                LogChat(string.Concat(new object[] { "User [", netUser.displayName, ":", netUser.userID, "] teleport from ", character.transform.position, " to ", position, " (distance: ", num, "m)" }), false);
                if ((Core.PlayerTeleportMethod > 0) && !netUser.admin)
                {
                    if (character.controller is HumanController)
                    {
                        character.controller.GetComponent<AvatarSaveRestore>().SaveAvatar();
                        netUser.truthDetector.NoteTeleported(position, 0.0);
                        IDLocalCharacter.DestroyCharacter(character);
                        smethod_176(character.eyesAngles, netUser, position);
                    }
                    else
                    {
                        Debug.LogError(string.Concat(new object[] { "Couldn't teleport user ", netUser, " to ", position }));
                    }
                }
                else
                {
                    float y = Mathf.Round(num / 1000f);
                    if (y > 2f)
                    {
                        y = 2f;
                    }
                    netUser.truthDetector.NoteTeleported(position + new Vector3(0f, y, 0f), 2.0);
                    ServerManagement._serverMan.networkView.RPC<Vector3>("UnstickMove", netUser.networkPlayer, position);
                }
            }
        }

        static void smethod_176(Angle2 angle2_0, NetUser netUser_0, Vector3 vector3_0)
        {
            Thread.Sleep((int)(NetCull.sendInterval * 1000.0) * 2);
            Character character = Character.SummonCharacter(netUser_0.networkPlayer, ":player_soldier", vector3_0, angle2_0);
            if (character != null && character.controller is HumanController)
            {
                character.controller.GetComponent<AvatarSaveRestore>().LoadAvatar();
                netUser_0.playerClient.lastKnownPosition = character.eyesOrigin;
                netUser_0.playerClient.hasLastKnownPosition = true;
                object[] args = new object[] { netUser_0.playerClient };
                Oxide.Main.Call("HandleSpawn", args);

            }
            else
            {
                Debug.LogError("Couldn't create character for user " + netUser_0);
            }
        }


        public static void UpgradePlayerWeapon(IWeaponItem Weapon)
        {
            Weapon.SetTotalModSlotCount(4);
            ItemModDataBlock byName = DatablockDictionary.GetByName("Silencer") as ItemModDataBlock;
            ItemModDataBlock block2 = DatablockDictionary.GetByName("Holo sight") as ItemModDataBlock;
            ItemModDataBlock block3 = DatablockDictionary.GetByName("Laser Sight") as ItemModDataBlock;
            ItemModDataBlock block4 = DatablockDictionary.GetByName("Flashlight Mod") as ItemModDataBlock;
            if (!Weapon.itemMods.Contains<ItemModDataBlock>(byName))
            {
                Weapon.AddMod(byName);
            }
            if (!Weapon.itemMods.Contains<ItemModDataBlock>(block2))
            {
                Weapon.AddMod(block2);
            }
            if (!Weapon.itemMods.Contains<ItemModDataBlock>(block3))
            {
                Weapon.AddMod(block3);
            }
            if (!Weapon.itemMods.Contains<ItemModDataBlock>(block4))
            {
                Weapon.AddMod(block4);
            }
        }

        public static bool UserAlreadyConnected(ulong steam_id, out NetUser netUser)
        {
            foreach (uLink.NetworkPlayer player in NetCull.connections)
            {
                object localData = player.GetLocalData();
                if (((localData is NetUser) && ((netUser = (NetUser) localData) != null)) && (netUser.userID == steam_id))
                {
                    return true;
                }
            }
            netUser = null;
            return false;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("KERNEL32.DLL", CharSet=CharSet.Unicode, SetLastError=true)]
        public static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);
        public static string[] WarpChatText(string input, [Optional, DefaultParameterValue(80)] int maxlength, [Optional, DefaultParameterValue("")] string prefix, [Optional, DefaultParameterValue("")] string suffix)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            if (input.Length > maxlength)
            {
                while (input.Length > maxlength)
                {
                    StringBuilder builder = new StringBuilder();
                    string str = "";
                    System.Collections.Generic.List<string> list2 = input.Split(new char[] { ' ' }).ToList<string>();
                    int startIndex = maxlength;
                    int length = input.Length - maxlength;
                    if (list2.Count == 1)
                    {
                        str = builder.Append(input.Substring(0, maxlength) + "-").ToString();
                    }
                    else
                    {
                        for (int i = 0; i < list2.Count; i++)
                        {
                            if (maxlength < (builder + list2[i]).Length)
                            {
                                break;
                            }
                            builder.Append(" " + list2[i]);
                        }
                        str = builder.ToString().Trim();
                        startIndex = str.Length + 1;
                        length = (input.Length - str.Length) - 1;
                    }
                    input = input.Substring(startIndex, length);
                    list.Add(prefix + str + suffix);
                }
            }
            list.Add(prefix + input + suffix);
            return list.ToArray();
        }

        public static string ChatLogFile
        {
            get
            {
                return Path.Combine(Core.LogsPath, "Chat" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
            }
        }

        public static uint NewSerial
        {
            get
            {
                return (uint) NewSerial64;
            }
        }

        public static ulong NewSerial64
        {
            get
            {
                return (ulong) (DateTime.Now.Ticks ^ (long_0 += 1L));
            }
        }

        public static string RustLogFile
        {
            get
            {
                return Path.Combine(Core.LogsPath, "Rust" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
            }
        }

        public static string ServSQLFile
        {
            get
            {
                return Path.Combine(Core.LogsPath, "MySQL" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
            }
        }

        [CompilerGenerated]
        private sealed class Class41
        {
            public uint uint_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.RequireLevel == Clans.Database[this.uint_0].Level.Id);
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
                return (deployableObject_0.ownerID == this.ulong_0);
            }
        }
    }
}

