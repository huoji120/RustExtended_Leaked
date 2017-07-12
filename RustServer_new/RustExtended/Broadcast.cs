using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class Broadcast
	{
		public static Dictionary<uLink.NetworkPlayer, DateTime> Timewait = new Dictionary<uLink.NetworkPlayer, DateTime>();

		public static void Chat(string sender, string text, NetUser exclude = null)
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
						foreach (PlayerClient current in PlayerClient.All)
						{
							if (current.netUser != exclude)
							{
								ConsoleNetworker.SendClientCommand(current.netPlayer, "chat.add " + sender + " " + text);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
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
					text = Regex.Replace(text, "(\\[COLOR\\s*\\S*])|(\\[/COLOR\\s*\\S*])", "", RegexOptions.IgnoreCase).Trim();
					Helper.LogChat(string.Concat(new string[]
					{
						"[PM] \"",
						sender.displayName,
						"\" for \"",
						client.displayName,
						"\" say ",
						text
					}), false);
					string[] array = Helper.WarpChatText(Helper.ObsceneText(text), Core.ChatLineMaxLength, "", "");
					for (int i = 0; i < array.Length; i++)
					{
						ConsoleNetworker.SendClientCommand(sender.networkPlayer, "chat.add " + str + " " + Helper.QuoteSafe(chatTextColor + array[i]));
						ConsoleNetworker.SendClientCommand(client.networkPlayer, "chat.add " + str2 + " " + Helper.QuoteSafe(chatTextColor + array[i]));
					}
					if (Core.ChatHistoryPrivate)
					{
						if (!Core.History.ContainsKey(sender.userID))
						{
							Core.History.Add(sender.userID, new List<HistoryRecord>());
						}
						if (Core.History[sender.userID].Count > Core.ChatHistoryStored)
						{
							Core.History[sender.userID].RemoveAt(0);
						}
						Core.History[sender.userID].Add(default(HistoryRecord).Init(Config.GetMessage("Command.PM.To", null, null) + " " + client.displayName, text));
						if (!Core.History.ContainsKey(client.userID))
						{
							Core.History.Add(client.userID, new List<HistoryRecord>());
						}
						if (Core.History[client.userID].Count > Core.ChatHistoryStored)
						{
							Core.History[client.userID].RemoveAt(0);
						}
						Core.History[client.userID].Add(default(HistoryRecord).Init(Config.GetMessage("Command.PM.From", null, null) + " " + sender.displayName, text));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageGM(string text)
		{
			try
			{
				if (!text.IsEmpty())
				{
					text = "chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(text);
					foreach (PlayerClient current in PlayerClient.All)
					{
						if (Users.GetRank(current.userID) >= Truth.ReportRank)
						{
							ConsoleNetworker.SendClientCommand(current.netPlayer, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageClan(ClanData clan, string text)
		{
			try
			{
				if (!text.IsEmpty())
				{
					if (clan != null)
					{
						string text2 = Core.ChatClanIcon;
						if (text2 == "")
						{
							text2 = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
						}
						text = "chat.add " + Helper.QuoteSafe(text2) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[]
						{
							'"'
						}));
						foreach (UserData current in clan.Members.Keys)
						{
							PlayerClient playerClient;
							if (PlayerClient.FindByUserID(current.SteamID, out playerClient))
							{
								ConsoleNetworker.SendClientCommand(playerClient.netPlayer, text);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageClan(ClanData clan, string text, NetUser AsUser = null)
		{
			try
			{
				if (!text.IsEmpty())
				{
					if (clan != null)
					{
						string text2 = Core.ChatClanIcon;
						if (text2 == "")
						{
							text2 = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
						}
						if (AsUser != null)
						{
							text2 = AsUser.displayName + Core.ChatDivider + text2;
						}
						text = "chat.add " + Helper.QuoteSafe(text2) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[]
						{
							'"'
						}));
						foreach (UserData current in clan.Members.Keys)
						{
							PlayerClient playerClient;
							if (PlayerClient.FindByUserID(current.SteamID, out playerClient))
							{
								ConsoleNetworker.SendClientCommand(playerClient.netPlayer, text);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageClan(NetUser user, string text)
		{
			try
			{
				if (!text.IsEmpty())
				{
					if (user == null)
					{
						ConsoleSystem.Print(text, false);
					}
					else
					{
						ClanData clan = Users.GetBySteamID(user.userID).Clan;
						if (clan != null)
						{
							string text2 = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
							text = "chat.add " + Helper.QuoteSafe(text2) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[]
							{
								'"'
							}));
							ConsoleNetworker.SendClientCommand(user.networkPlayer, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageClan(NetUser user, ClanData clan, string text)
		{
			try
			{
				if (!text.IsEmpty())
				{
					string text2 = "<Undefined>";
					if (user == null)
					{
						if (clan != null)
						{
							text2 = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
						}
						ConsoleSystem.Print(text2 + ": " + text, false);
					}
					else
					{
						if (clan == null)
						{
							clan = Users.GetBySteamID(user.userID).Clan;
						}
						if (clan != null)
						{
							text2 = "<" + (clan.Abbr.IsEmpty() ? clan.Name : clan.Abbr) + ">";
							text = "chat.add " + Helper.QuoteSafe(text2) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatClanColor) + text.Trim(new char[]
							{
								'"'
							}));
							ConsoleNetworker.SendClientCommand(user.networkPlayer, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void Message(NetUser player, string text, string sender = null, float timewait = 0f)
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
						Broadcast.Message(player.networkPlayer, text, sender, timewait);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void Message(string color, NetUser player, string text, string sender = null, float timewait = 0f)
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
						Broadcast.Message(color, player.networkPlayer, text, sender, timewait);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void Message(uLink.NetworkPlayer player, string text, string sender = null, float timewait = 0f)
		{
			if (!text.IsEmpty())
			{
				if (!Broadcast.Timewait.ContainsKey(player) || !(Broadcast.Timewait[player] > DateTime.Now))
				{
					if (timewait > 0f)
					{
						if (!Broadcast.Timewait.ContainsKey(player))
						{
							Broadcast.Timewait.Add(player, DateTime.Now.AddMilliseconds((double)(1000f * timewait)));
						}
						else
						{
							Broadcast.Timewait[player] = DateTime.Now.AddMilliseconds((double)(1000f * timewait));
						}
					}
					if (string.IsNullOrEmpty(sender))
					{
						sender = Core.ServerName;
					}
					text = Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[]
					{
						'"'
					});
					ConsoleNetworker.SendClientCommand(player, "chat.add " + Helper.QuoteSafe(sender) + " " + Helper.QuoteSafe(text));
				}
			}
		}

		public static void Message(string color, uLink.NetworkPlayer player, string text, string sender = null, float timewait = 0f)
		{
			if (!text.IsEmpty())
			{
				if (!Broadcast.Timewait.ContainsKey(player) || !(Broadcast.Timewait[player] > DateTime.Now))
				{
					if (timewait > 0f)
					{
						if (!Broadcast.Timewait.ContainsKey(player))
						{
							Broadcast.Timewait.Add(player, DateTime.Now.AddMilliseconds((double)(1000f * timewait)));
						}
						else
						{
							Broadcast.Timewait[player] = DateTime.Now.AddMilliseconds((double)(1000f * timewait));
						}
					}
					if (string.IsNullOrEmpty(sender))
					{
						sender = Core.ServerName;
					}
					text = Helper.GetChatTextColor(color) + text.Trim(new char[]
					{
						'"'
					});
					ConsoleNetworker.SendClientCommand(player, "chat.add " + Helper.QuoteSafe(sender) + " " + Helper.QuoteSafe(text));
				}
			}
		}

		public static void MessageAll(string text)
		{
			try
			{
				if (!text.IsEmpty())
				{
					text = Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[]
					{
						'"'
					});
					ConsoleNetworker.Broadcast("chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(text));
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageAll(string text, string sender)
		{
			try
			{
				if (!text.IsEmpty())
				{
					text = Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[]
					{
						'"'
					});
					ConsoleNetworker.Broadcast("chat.add " + Helper.QuoteSafe(sender) + " " + Helper.QuoteSafe(text));
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageAll(string text, NetUser exclude)
		{
			try
			{
				if (!text.IsEmpty())
				{
					text = "chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(Core.ChatSystemColor) + text.Trim(new char[]
					{
						'"'
					}));
					foreach (PlayerClient current in PlayerClient.All)
					{
						if (current.netUser != exclude)
						{
							ConsoleNetworker.SendClientCommand(current.netPlayer, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void MessageAll(string color, string text, NetUser exclude)
		{
			try
			{
				if (!text.IsEmpty())
				{
					text = "chat.add " + Helper.QuoteSafe(Core.ServerName) + " " + Helper.QuoteSafe(Helper.GetChatTextColor(color) + text.Trim(new char[]
					{
						'"'
					}));
					foreach (PlayerClient current in PlayerClient.All)
					{
						if (current.netUser != exclude)
						{
							ConsoleNetworker.SendClientCommand(current.netPlayer, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void Notice(NetUser player, string icon, string text, float duration = 5f)
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
						Broadcast.Notice(player.networkPlayer, icon, text, duration);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void Notice(uLink.NetworkPlayer player, string icon, string text, float duration = 5f)
		{
			try
			{
				if (!text.IsEmpty())
				{
					ConsoleNetworker.SendClientCommand(player, string.Concat(new object[]
					{
						"notice.popup \"",
						duration,
						"\" \"",
						icon,
						"\" ",
						Helper.QuoteSafe(text)
					}));
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}

		public static void NoticeAll(string icon, string text, NetUser sender = null, float duration = 5f)
		{
			try
			{
				if (!text.IsEmpty())
				{
					text = string.Concat(new object[]
					{
						"notice.popup \"",
						duration,
						"\" ",
						icon,
						"\" ",
						Helper.QuoteSafe(text)
					});
					foreach (PlayerClient current in PlayerClient.All)
					{
						if (current.netUser != sender)
						{
							ConsoleNetworker.SendClientCommand(current.netPlayer, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR: " + ex.Message);
			}
		}
	}
}
