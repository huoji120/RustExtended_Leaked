namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using uLink;
    using UnityEngine;

    public class Broadcast
    {
        public static Dictionary<uLink.NetworkPlayer, DateTime> Timewait = new Dictionary<uLink.NetworkPlayer, DateTime>();

        public static void Chat(string sender, string text, [Optional, DefaultParameterValue(null)] NetUser exclude)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    if (string.IsNullOrEmpty(sender))
                    {
                        sender = Core.ServerName;
                    }
                    text = Helper.QuoteSafe(text);
                    sender = Helper.QuoteSafe(sender);
                    if (exclude == null)
                    {
                        ConsoleNetworker.Broadcast("chat.add " + sender + " " + text);
                    }
                    else
                    {
                        foreach (PlayerClient client in PlayerClient.All)
                        {
                            if (client.netUser != exclude)
                            {
                                ConsoleNetworker.SendClientCommand(client.netPlayer, "chat.add " + sender + " " + text);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void ChatPM(NetUser sender, NetUser client, string text)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    string str = Helper.QuoteSafe(Config.GetMessage("Command.PM.To", null, null) + " " + client.displayName);
                    string str2 = Helper.QuoteSafe(Config.GetMessage("Command.PM.From", null, null) + " " + sender.displayName);
                    string chatTextColor = Helper.GetChatTextColor(Core.ChatWhisperColor);
                    text = Regex.Replace(text, @"(\[COLOR\s*\S*])|(\[/COLOR\s*\S*])", "", RegexOptions.IgnoreCase).Trim();
                    Helper.LogChat("[PM] \"" + sender.displayName + "\" for \"" + client.displayName + "\" say " + text, false);
                    string[] strArray = Helper.WarpChatText(Helper.ObsceneText(text), Core.ChatLineMaxLength, "", "");
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        ConsoleNetworker.SendClientCommand(sender.networkPlayer, "chat.add " + str + " " + Helper.QuoteSafe(chatTextColor + strArray[i]));
                        ConsoleNetworker.SendClientCommand(client.networkPlayer, "chat.add " + str2 + " " + Helper.QuoteSafe(chatTextColor + strArray[i]));
                    }
                    if (Core.ChatHistoryPrivate)
                    {
                        if (!Core.History.ContainsKey(sender.userID))
                        {
                            Core.History.Add(sender.userID, new System.Collections.Generic.List<HistoryRecord>());
                        }
                        if (Core.History[sender.userID].Count > Core.ChatHistoryStored)
                        {
                            Core.History[sender.userID].RemoveAt(0);
                        }
                        HistoryRecord record2 = new HistoryRecord();
                        Core.History[sender.userID].Add(record2.Init(Config.GetMessage("Command.PM.To", null, null) + " " + client.displayName, text));
                        if (!Core.History.ContainsKey(client.userID))
                        {
                            Core.History.Add(client.userID, new System.Collections.Generic.List<HistoryRecord>());
                        }
                        if (Core.History[client.userID].Count > Core.ChatHistoryStored)
                        {
                            Core.History[client.userID].RemoveAt(0);
                        }
                        HistoryRecord record4 = new HistoryRecord();
                        Core.History[client.userID].Add(record4.Init(Config.GetMessage("Command.PM.From", null, null) + " " + sender.displayName, text));
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void Message(NetUser player, string text, [Optional, DefaultParameterValue(null)] string sender, [Optional, DefaultParameterValue(0f)] float timewait)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    if (player == null)
                    {
                        ConsoleSystem.Print(text, false);
                    }
                    else
                    {
                        Message(player.networkPlayer, text, sender, timewait);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void Message(uLink.NetworkPlayer player, string text, [Optional, DefaultParameterValue(null)] string sender, [Optional, DefaultParameterValue(0f)] float timewait)
        {
            if (!text.IsEmpty() && (!Timewait.ContainsKey(player) || (Timewait[player] <= DateTime.Now)))
            {
                if (timewait > 0f)
                {
                    if (!Timewait.ContainsKey(player))
                    {
                        Timewait.Add(player, DateTime.Now.AddMilliseconds((double) (1000f * timewait)));
                    }
                    else
                    {
                        Timewait[player] = DateTime.Now.AddMilliseconds((double) (1000f * timewait));
                    }
                }
                if (string.IsNullOrEmpty(sender))
                {
                    sender = Core.ServerName;
                }
                text = Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[] { '"' });
                ConsoleNetworker.SendClientCommand(player, "chat.add " + Helper.QuoteSafe(sender) + " " + Helper.QuoteSafe(text));
            }
        }

        public static void Message(string color, NetUser player, string text, [Optional, DefaultParameterValue(null)] string sender, [Optional, DefaultParameterValue(0f)] float timewait)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    if (player == null)
                    {
                        ConsoleSystem.Print(text, false);
                    }
                    else
                    {
                        Message(color, player.networkPlayer, text, sender, timewait);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void Message(string color, uLink.NetworkPlayer player, string text, [Optional, DefaultParameterValue(null)] string sender, [Optional, DefaultParameterValue(0f)] float timewait)
        {
            if (!text.IsEmpty() && (!Timewait.ContainsKey(player) || (Timewait[player] <= DateTime.Now)))
            {
                if (timewait > 0f)
                {
                    if (!Timewait.ContainsKey(player))
                    {
                        Timewait.Add(player, DateTime.Now.AddMilliseconds((double) (1000f * timewait)));
                    }
                    else
                    {
                        Timewait[player] = DateTime.Now.AddMilliseconds((double) (1000f * timewait));
                    }
                }
                if (string.IsNullOrEmpty(sender))
                {
                    sender = Core.ServerName;
                }
                text = Helper.GetChatTextColor(color) + text.Trim(new char[] { '"' });
                ConsoleNetworker.SendClientCommand(player, "chat.add " + Helper.QuoteSafe(sender) + " " + Helper.QuoteSafe(text));
            }
        }

        public static void MessageAll(string text)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    text = Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[] { '"' });
                    ConsoleNetworker.Broadcast("chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(text));
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageAll(string text, NetUser exclude)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    text = "chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[] { '"' }));
                    foreach (PlayerClient client in PlayerClient.All)
                    {
                        if (client.netUser != exclude)
                        {
                            ConsoleNetworker.SendClientCommand(client.netPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageAll(string text, string sender)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    text = Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[] { '"' });
                    ConsoleNetworker.Broadcast("chat.add " + Helper.QuoteSafe(sender) + " " + Helper.QuoteSafe(text));
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageAll(string color, string text, NetUser exclude)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    text = "chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(color) + text.Trim(new char[] { '"' }));
                    foreach (PlayerClient client in PlayerClient.All)
                    {
                        if (client.netUser != exclude)
                        {
                            ConsoleNetworker.SendClientCommand(client.netPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageClan(NetUser user, string text)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    string str = "<Undefined>";
                    if (user == null)
                    {
                        ConsoleSystem.Print(text, false);
                    }
                    else
                    {
                        ClanData clan = Users.GetBySteamID(user.userID).Clan;
                        if (clan != null)
                        {
                            str = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
                            text = "chat.add " + Helper.QuoteSafe(str) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[] { '"' }));
                            ConsoleNetworker.SendClientCommand(user.networkPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageClan(ClanData clan, string text)
        {
            try
            {
                if (!text.IsEmpty() && (clan != null))
                {
                    string chatClanIcon = Core.ChatClanIcon;
                    if (chatClanIcon == "")
                    {
                        chatClanIcon = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
                    }
                    text = "chat.add " + Helper.QuoteSafe(chatClanIcon) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[] { '"' }));
                    foreach (UserData data in clan.Members.Keys)
                    {
                        PlayerClient client;
                        if (PlayerClient.FindByUserID(data.SteamID, out client))
                        {
                            ConsoleNetworker.SendClientCommand(client.netPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageClan(NetUser user, ClanData clan, string text)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    string str = "<Undefined>";
                    if (user == null)
                    {
                        if (clan != null)
                        {
                            str = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
                        }
                        ConsoleSystem.Print(str + ": " + text, false);
                    }
                    else
                    {
                        if (clan == null)
                        {
                            clan = Users.GetBySteamID(user.userID).Clan;
                        }
                        if (clan != null)
                        {
                            str = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
                            text = "chat.add " + Helper.QuoteSafe(str) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[] { '"' }));
                            ConsoleNetworker.SendClientCommand(user.networkPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageClan(ClanData clan, string text, [Optional, DefaultParameterValue(null)] NetUser AsUser)
        {
            try
            {
                if (!text.IsEmpty() && (clan != null))
                {
                    string chatClanIcon = Core.ChatClanIcon;
                    if (chatClanIcon == "")
                    {
                        chatClanIcon = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
                    }
                    if (AsUser != null)
                    {
                        chatClanIcon = AsUser.displayName + Core.ChatDivider + chatClanIcon;
                    }
                    text = "chat.add " + Helper.QuoteSafe(chatClanIcon) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[] { '"' }));
                    foreach (UserData data in clan.Members.Keys)
                    {
                        PlayerClient client;
                        if (PlayerClient.FindByUserID(data.SteamID, out client))
                        {
                            ConsoleNetworker.SendClientCommand(client.netPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void MessageGM(string text)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    text = "chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(text);
                    foreach (PlayerClient client in PlayerClient.All)
                    {
                        if (Users.GetRank(client.userID) >= Truth.ReportRank)
                        {
                            ConsoleNetworker.SendClientCommand(client.netPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void Notice(NetUser player, string icon, string text, [Optional, DefaultParameterValue(5f)] float duration)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    if (player == null)
                    {
                        ConsoleSystem.Print(text, false);
                    }
                    else
                    {
                        Notice(player.networkPlayer, icon, text, duration);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void Notice(uLink.NetworkPlayer player, string icon, string text, [Optional, DefaultParameterValue(5f)] float duration)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    ConsoleNetworker.SendClientCommand(player, string.Concat(new object[] { "notice.popup \"", duration, "\" \"", icon, "\" ", Helper.QuoteSafe(text) }));
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }

        public static void NoticeAll(string icon, string text, [Optional, DefaultParameterValue(null)] NetUser sender, [Optional, DefaultParameterValue(5f)] float duration)
        {
            try
            {
                if (!text.IsEmpty())
                {
                    text = string.Concat(new object[] { "notice.popup \"", duration, "\" ", icon, "\" ", Helper.QuoteSafe(text) });
                    foreach (PlayerClient client in PlayerClient.All)
                    {
                        if (client.netUser != sender)
                        {
                            ConsoleNetworker.SendClientCommand(client.netPlayer, text);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("ERROR: " + exception.Message);
            }
        }
    }
}

