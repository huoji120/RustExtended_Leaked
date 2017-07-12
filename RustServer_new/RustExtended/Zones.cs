using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Timers;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class Zones
	{
		[CompilerGenerated]
		private sealed class Class56
		{
			public UserData userData_0;

			public WorldZone worldZone_0;

			public NetUser netUser_0;

			public bool method_0(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.userData_0.Zone.Defname;
			}

			public void method_1(object sender, ElapsedEventArgs e)
			{
				Zones.PlayerWarp(this.netUser_0, this.userData_0, this.worldZone_0);
			}
		}

		[CompilerGenerated]
		private sealed class Class57
		{
			public NetUser netUser_0;

			public WorldZone worldZone_0;

			public bool method_0(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.netUser_0 && eventTimer_0.Command == this.worldZone_0.Defname;
			}
		}

		private static string string_0 = "rust_zones.txt";

		public static Dictionary<string, WorldZone> Database;

		public static List<GameObject> Markers = new List<GameObject>();

		public static Vector3 JailPosition = Vector3.zero;

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static WorldZone worldZone_0;

		public static string SaveFilePath
		{
			get;
			private set;
		}

		public static bool Initialized
		{
			get;
			private set;
		}

		public static int Count
		{
			get
			{
				return Zones.Database.Count;
			}
		}

		public static WorldZone LastZone
		{
			get;
			private set;
		}

		public static Dictionary<string, WorldZone> All
		{
			get
			{
				return Zones.Database;
			}
		}

		public static bool IsBuild
		{
			get
			{
				return Zones.LastZone != null;
			}
		}

		public static void Initialize()
		{
			Zones.Initialized = false;
			Zones.SaveFilePath = Path.Combine(Core.SavePath, Zones.string_0);
			Zones.Database = new Dictionary<string, WorldZone>();
			if (File.Exists(Zones.SaveFilePath))
			{
				Zones.LoadAsFile();
			}
			Zones.Initialized = true;
		}

		public static bool LoadAsFile()
		{
			bool result;
			if (Zones.Database == null)
			{
				result = false;
			}
			else
			{
				Zones.Database.Clear();
				string[] array = File.ReadAllLines(Zones.SaveFilePath);
				WorldZone worldZone = null;
				Dictionary<WorldZone, string> dictionary = new Dictionary<WorldZone, string>();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (text.StartsWith("[") && text.EndsWith("]"))
					{
						worldZone = null;
						if (!text.StartsWith("[ZONE ", StringComparison.OrdinalIgnoreCase))
						{
							ConsoleSystem.LogError("Invalid section \"" + text + "\" from zones.");
						}
						else
						{
							string[] array3 = Helper.SplitQuotes(text.Trim(new char[]
							{
								'[',
								']'
							}), ' ');
							if (Zones.Database.ContainsKey(array3[1]))
							{
								worldZone = Zones.Database[array3[1]];
							}
							else
							{
								worldZone = new WorldZone(null, (ZoneFlags)0);
								Zones.Database.Add(array3[1], worldZone);
							}
						}
					}
					else if (worldZone != null)
					{
						string[] array3 = text.Split(new char[]
						{
							'='
						});
						if (array3.Length >= 2)
						{
							array3[1] = array3[1].Trim();
							if (!string.IsNullOrEmpty(array3[1]))
							{
								float x = 0f;
								float y = 0f;
								float z = 0f;
								string text2 = array3[0].ToUpper();
								switch (text2)
								{
								case "NAME":
									worldZone.Name = array3[1];
									break;
								case "FLAGS":
									worldZone.Flags = array3[1].ToEnum<ZoneFlags>();
									break;
								case "INTERNAL":
									if (!Zones.Database.ContainsKey(array3[1]))
									{
										WorldZone worldZone2 = new WorldZone(null, (ZoneFlags)0);
										Zones.Database.Add(array3[1], worldZone2);
										worldZone.Internal.Add(worldZone2);
									}
									break;
								case "CENTER":
									array3 = array3[1].Split(new char[]
									{
										','
									});
									if (array3.Length > 0)
									{
										float.TryParse(array3[0], out x);
									}
									if (array3.Length > 1)
									{
										float.TryParse(array3[1], out y);
									}
									worldZone.Center = new Vector2(x, y);
									break;
								case "WARP":
									array3 = array3[1].Split(new char[]
									{
										','
									});
									dictionary.Add(worldZone, array3[0].Trim());
									if (array3.Length > 1)
									{
										long.TryParse(array3[1], out worldZone.WarpTime);
									}
									break;
								case "POINT":
									array3 = array3[1].Split(new char[]
									{
										','
									});
									if (array3.Length > 0)
									{
										float.TryParse(array3[0], out x);
									}
									if (array3.Length > 1)
									{
										float.TryParse(array3[1], out y);
									}
									worldZone.Points.Add(new Vector2(x, y));
									break;
								case "SPAWN":
									array3 = array3[1].Split(new char[]
									{
										','
									});
									if (array3.Length > 0)
									{
										float.TryParse(array3[0], out x);
									}
									if (array3.Length > 1)
									{
										float.TryParse(array3[1], out y);
									}
									if (array3.Length > 2)
									{
										float.TryParse(array3[2], out z);
									}
									worldZone.Spawns.Add(new Vector3(x, y, z));
									break;
								case "FORBIDDEN.COMMAND":
									worldZone.ForbiddenCommand = worldZone.ForbiddenCommand.Add(array3[1]);
									break;
								case "ENTER.NOTICE":
									worldZone.Notice_OnEnter = array3[1];
									break;
								case "LEAVE.NOTICE":
									worldZone.Notice_OnLeave = array3[1];
									break;
								case "ENTER.MESSAGE":
									worldZone.Message_OnEnter = worldZone.Message_OnEnter.Add(array3[1]);
									break;
								case "LEAVE.MESSAGE":
									worldZone.Message_OnLeave = worldZone.Message_OnLeave.Add(array3[1]);
									break;
								}
							}
						}
					}
				}
				foreach (WorldZone current in dictionary.Keys)
				{
					current.WarpZone = Zones.Get(dictionary[current]);
				}
				result = true;
			}
			return result;
		}

		public static bool SaveAsFile()
		{
			bool result;
			if (Zones.Database == null)
			{
				result = false;
			}
			else
			{
				using (StreamWriter streamWriter = new StreamWriter(Zones.SaveFilePath))
				{
					foreach (string current in Zones.Database.Keys)
					{
						streamWriter.WriteLine("[ZONE " + current + "]");
						streamWriter.WriteLine("NAME=" + Zones.Database[current].Name);
						if (Zones.Database[current].Flags != (ZoneFlags)0)
						{
							streamWriter.WriteLine("FLAGS=" + Zones.Database[current].Flags);
						}
						foreach (WorldZone current2 in Zones.Database[current].Internal)
						{
							streamWriter.WriteLine("INTERNAL=" + current2.Defname);
						}
						if (Zones.Database[current].Center != Vector2.zero)
						{
							streamWriter.WriteLine("CENTER=" + Zones.Database[current].Center.AsString().Replace(" ", ""));
						}
						if (Zones.Database[current].WarpZone != null)
						{
							streamWriter.WriteLine(string.Concat(new object[]
							{
								"WARP=",
								Zones.Database[current].WarpZone.Defname,
								",",
								Zones.Database[current].WarpTime
							}));
						}
						foreach (Vector2 current3 in Zones.Database[current].Points)
						{
							streamWriter.WriteLine("POINT=" + current3.AsString().Replace(" ", ""));
						}
						foreach (Vector3 current4 in Zones.Database[current].Spawns)
						{
							streamWriter.WriteLine("SPAWN=" + current4.AsString().Replace(" ", ""));
						}
						string[] forbiddenCommand = Zones.Database[current].ForbiddenCommand;
						for (int i = 0; i < forbiddenCommand.Length; i++)
						{
							string str = forbiddenCommand[i];
							streamWriter.WriteLine("FORBIDDEN.COMMAND=" + str);
						}
						if (!string.IsNullOrEmpty(Zones.Database[current].Notice_OnEnter))
						{
							streamWriter.WriteLine("ENTER.NOTICE=" + Zones.Database[current].Notice_OnEnter);
						}
						if (!string.IsNullOrEmpty(Zones.Database[current].Notice_OnLeave))
						{
							streamWriter.WriteLine("LEAVE.NOTICE=" + Zones.Database[current].Notice_OnLeave);
						}
						string[] message_OnEnter = Zones.Database[current].Message_OnEnter;
						for (int j = 0; j < message_OnEnter.Length; j++)
						{
							string str2 = message_OnEnter[j];
							streamWriter.WriteLine("ENTER.MESSAGE=" + str2);
						}
						string[] message_OnLeave = Zones.Database[current].Message_OnLeave;
						for (int k = 0; k < message_OnLeave.Length; k++)
						{
							string str3 = message_OnLeave[k];
							streamWriter.WriteLine("ENTER.MESSAGE=" + str3);
						}
						streamWriter.WriteLine();
					}
				}
				result = true;
			}
			return result;
		}

		public static WorldZone Get(string defname)
		{
			WorldZone result;
			if (Zones.Database.ContainsKey(defname))
			{
				result = Zones.Database[defname];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static string GetDefname(WorldZone zone)
		{
			string result;
			foreach (string current in Zones.Database.Keys)
			{
				if (Zones.Database[current] == zone)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static bool BuildNew(string zone_name)
		{
			bool result;
			if (Zones.LastZone != null)
			{
				result = false;
			}
			else
			{
				string key = "z_" + zone_name.Trim().Replace(" ", "_").ToLower();
				if (Zones.Database.ContainsKey(key))
				{
					result = false;
				}
				else
				{
					Zones.LastZone = new WorldZone(zone_name, (ZoneFlags)0);
					result = true;
				}
			}
			return result;
		}

		public static bool BuildMark(Vector3 position)
		{
			bool result;
			if (Zones.LastZone == null)
			{
				result = false;
			}
			else
			{
				Zones.LastZone.Points.Add(new Vector2(position.x, position.z));
				WorldZone worldZone = Zones.Get(position);
				if (worldZone != null && !worldZone.Internal.Contains(Zones.LastZone))
				{
					worldZone.Internal.Add(Zones.LastZone);
				}
				Vector3 ground = Zones.GetGround(position.x, position.z);
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground));
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 4f, 0f)));
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 8f, 0f)));
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 12f, 0f)));
				result = true;
			}
			return result;
		}

		public static bool BuildSave()
		{
			bool result;
			if (Zones.LastZone != null && Zones.LastZone.Points.Count >= 3)
			{
				string key = "z_" + Zones.LastZone.Name.Trim().Replace(" ", "_").ToLower();
				Zones.LastZone.Center = Zones.GetCentroid(Zones.LastZone.Points);
				Zones.Database.Add(key, Zones.LastZone);
				Zones.LastZone = null;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static WorldZone Find(string search_name)
		{
			string value = search_name.Trim(new char[]
			{
				'*'
			});
			WorldZone result;
			foreach (string current in Zones.Database.Keys)
			{
				if (search_name.StartsWith("*") && current.EndsWith(value))
				{
					WorldZone worldZone = Zones.Database[current];
					result = worldZone;
					return result;
				}
				if (search_name.EndsWith("*") && current.StartsWith(value))
				{
					WorldZone worldZone = Zones.Database[current];
					result = worldZone;
					return result;
				}
				if (current.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					WorldZone worldZone = Zones.Database[current];
					result = worldZone;
					return result;
				}
				if (search_name.StartsWith("*") && Zones.Database[current].Name.EndsWith(value))
				{
					WorldZone worldZone = Zones.Database[current];
					result = worldZone;
					return result;
				}
				if (search_name.EndsWith("*") && Zones.Database[current].Name.StartsWith(value))
				{
					WorldZone worldZone = Zones.Database[current];
					result = worldZone;
					return result;
				}
				if (Zones.Database[current].Name.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					WorldZone worldZone = Zones.Database[current];
					result = worldZone;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static bool Delete(WorldZone zone)
		{
			bool result;
			if (zone != null && Zones.Database.ContainsValue(zone))
			{
				Zones.Database.Remove(zone.Defname);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private static Vector3 GetGround(float float_0, float float_1)
		{
			Vector3 origin = new Vector3(float_0, 2000f, float_1);
			Vector3 direction = new Vector3(0f, -1f, 0f);
			return Physics.RaycastAll(origin, direction)[0].point;
		}

		public static void ShowPoints(WorldZone zone)
		{
			foreach (Vector2 current in zone.Points)
			{
				Vector3 ground = Zones.GetGround(current.x, current.y);
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground));
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 4f, 0f)));
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 8f, 0f)));
				Zones.Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 12f, 0f)));
			}
		}

		public static void HidePoints()
		{
			if (Zones.Markers != null && Zones.Markers.Count != 0)
			{
				foreach (GameObject current in Zones.Markers)
				{
					NetCull.Destroy(current);
				}
				Zones.Markers.Clear();
			}
		}

		public static WorldZone Get(uLink.NetworkPlayer netPlayer)
		{
			PlayerClient playerClient;
			WorldZone result;
			if (!PlayerClient.Find(netPlayer, out playerClient))
			{
				result = null;
			}
			else
			{
				result = Zones.Get(playerClient.lastKnownPosition);
			}
			return result;
		}

		public static WorldZone Get(PlayerClient player)
		{
			return Zones.Get(player.lastKnownPosition);
		}

		public static WorldZone Get(NetUser netUser)
		{
			return Zones.Get(netUser.playerClient.lastKnownPosition);
		}

		public static WorldZone Get(Transform transform)
		{
			return Zones.Get(transform.position.x, transform.position.y, transform.position.z);
		}

		public static WorldZone Get(Vector3 position)
		{
			return Zones.Get(position.x, position.y, position.z);
		}

		public static WorldZone Get(float x, float y, float z)
		{
			WorldZone result;
			foreach (string current in Zones.Database.Keys)
			{
				if (Zones.AtZone(Zones.Database[current], x, y, z))
				{
					WorldZone worldZone = Zones.Database[current];
					Zones.GetInternal(ref worldZone, x, y, z);
					result = worldZone;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static void GetInternal(ref WorldZone zone, float x, float y, float z)
		{
			foreach (WorldZone current in zone.Internal)
			{
				if (Zones.AtZone(current, x, y, z))
				{
					zone = current;
					Zones.GetInternal(ref zone, x, y, z);
				}
			}
		}

		public static bool AtZone(WorldZone zone, uLink.NetworkPlayer netPlayer)
		{
			PlayerClient playerClient;
			return PlayerClient.Find(netPlayer, out playerClient) && Zones.AtZone(zone, playerClient.lastKnownPosition);
		}

		public static bool AtZone(WorldZone zone, PlayerClient player)
		{
			return Zones.AtZone(zone, player.lastKnownPosition);
		}

		public static bool AtZone(WorldZone zone, NetUser netUser)
		{
			return Zones.AtZone(zone, netUser.playerClient.lastKnownPosition);
		}

		public static bool AtZone(WorldZone zone, GameObject gameObject)
		{
			return Zones.AtZone(zone, gameObject.transform.position);
		}

		public static bool AtZone(WorldZone zone, Transform transform)
		{
			return Zones.AtZone(zone, transform.position);
		}

		public static bool AtZone(WorldZone zone, Vector3 position)
		{
			return Zones.AtZone(zone, position.x, position.y, position.z);
		}

		public static bool AtZone(WorldZone zone, float x, float y, float z)
		{
			return Zones.AtZone(zone.Points, new Vector2(x, z));
		}

		public static bool AtZone(List<Vector2> V, Vector2 P)
		{
			int num = 0;
			for (int i = 0; i < V.Count; i++)
			{
				int num2 = i + 1;
				if (num2 == V.Count)
				{
					num2 = 0;
				}
				if ((V[i].y <= P.y && V[num2].y > P.y) || (V[i].y > P.y && V[num2].y <= P.y))
				{
					double num3 = (double)((P.y - V[i].y) / (V[num2].y - V[i].y));
					if ((double)P.x < (double)V[i].x + num3 * (double)(V[num2].x - V[i].x))
					{
						num++;
					}
				}
			}
			return num % 2 != 0;
		}

		public static Vector2 GetCentroid(WorldZone zone)
		{
			return Zones.GetCentroid(zone.Points);
		}

		public static Vector2 GetCentroid(List<Vector2> points)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int i = 0;
			int index = points.Count - 1;
			while (i < points.Count)
			{
				float num4 = points[i].x * points[index].y - points[index].x * points[i].y;
				num2 += (points[i].x + points[index].x) * num4;
				num3 += (points[i].y + points[index].y) * num4;
				num += num4;
				index = i++;
			}
			Vector2 result;
			if (num == 0f)
			{
				result = Vector2.zero;
			}
			else
			{
				num *= 3f;
				result = new Vector2(num2 / num, num3 / num);
			}
			return result;
		}

		public static void OnPlayerMove(NetUser netUser, ref Vector3 newpos, ref TruthDetector.ActionTaken taken)
		{
			Predicate<EventTimer> predicate = null;
			ElapsedEventHandler elapsedEventHandler = null;
			Zones.Class56 @class = new Zones.Class56();
			@class.netUser_0 = netUser;
			if (@class.netUser_0 != null && !(@class.netUser_0.playerClient == null) && !(@class.netUser_0.playerClient.controllable == null))
			{
				Vector3 position = @class.netUser_0.playerClient.controllable.character.transform.position;
				if (!(position == newpos))
				{
					if (position.x != newpos.x || position.z != newpos.z)
					{
						@class.userData_0 = Users.GetBySteamID(@class.netUser_0.userID);
						if (@class.userData_0 != null)
						{
							if (!@class.userData_0.HasFlag(UserFlags.onevent))
							{
								@class.userData_0.Position = newpos;
							}
							@class.worldZone_0 = Zones.Get(newpos);
							if (@class.userData_0.Zone != @class.worldZone_0)
							{
								EventTimer eventTimer = null;
								if (@class.userData_0.Zone != null)
								{
									List<EventTimer> timer = Events.Timer;
									if (predicate == null)
									{
										predicate = new Predicate<EventTimer>(@class.method_0);
									}
									eventTimer = timer.Find(predicate);
								}
								if (eventTimer != null)
								{
									Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageTeleport("Player.WarpZone.Interrupt", @class.netUser_0, @class.userData_0.Zone, null), 2f);
									eventTimer.Dispose();
								}
								if (@class.userData_0.Zone != null)
								{
									if (@class.userData_0.Zone.NoLeave && !@class.netUser_0.admin && (@class.worldZone_0 == null || !@class.userData_0.Zone.Internal.Contains(@class.worldZone_0)))
									{
										newpos = position;
                                        taken = (TruthDetector.ActionTaken)2;
										return;
									}
									if (!string.IsNullOrEmpty(@class.userData_0.Zone.Notice_OnLeave))
									{
										Broadcast.Notice(@class.netUser_0, "☢", @class.userData_0.Zone.Notice_OnLeave, 5f);
									}
									string[] message_OnLeave = @class.userData_0.Zone.Message_OnLeave;
									for (int i = 0; i < message_OnLeave.Length; i++)
									{
										string text = message_OnLeave[i];
										Broadcast.Message(@class.netUser_0, text, null, 0f);
									}
								}
								if (@class.worldZone_0 != null)
								{
									if (@class.worldZone_0.NoEnter && !@class.netUser_0.admin && (@class.userData_0.Zone == null || !@class.worldZone_0.Internal.Contains(@class.userData_0.Zone)))
									{
										newpos = position;
                                        taken = (TruthDetector.ActionTaken)2;
										return;
									}
									if (!string.IsNullOrEmpty(@class.worldZone_0.Notice_OnEnter))
									{
										Broadcast.Notice(@class.netUser_0, "☢", @class.worldZone_0.Notice_OnEnter, 5f);
									}
									string[] message_OnEnter = @class.worldZone_0.Message_OnEnter;
									for (int j = 0; j < message_OnEnter.Length; j++)
									{
										string text2 = message_OnEnter[j];
										Broadcast.Message(@class.netUser_0, text2, null, 0f);
									}
								}
								@class.userData_0.Zone = @class.worldZone_0;
								if (@class.worldZone_0 != null && @class.worldZone_0.WarpZone != null && @class.worldZone_0.WarpZone.Spawns.Count > 0)
								{
									if (@class.worldZone_0.WarpTime > 0L)
									{
										eventTimer = new EventTimer
										{
											Interval = (double)(@class.worldZone_0.WarpTime * 1000L),
											AutoReset = false
										};
										Timer timer2 = eventTimer;
										if (elapsedEventHandler == null)
										{
											elapsedEventHandler = new ElapsedEventHandler(@class.method_1);
										}
										timer2.Elapsed += elapsedEventHandler;
										eventTimer.Sender = @class.netUser_0;
										eventTimer.Command = @class.worldZone_0.Defname;
										eventTimer.Start();
										Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageTeleport("Player.WarpZone.Start", @class.netUser_0, @class.worldZone_0, null), 2f);
									}
									else
									{
										Zones.PlayerWarp(@class.netUser_0, @class.userData_0, @class.worldZone_0);
									}
								}
							}
						}
					}
				}
			}
		}

		public static void PlayerWarp(NetUser netUser, UserData user, WorldZone moveZone)
		{
			Zones.Class57 @class = new Zones.Class57();
			@class.netUser_0 = netUser;
			@class.worldZone_0 = moveZone;
			EventTimer eventTimer = Events.Timer.Find(new Predicate<EventTimer>(@class.method_0));
			if (eventTimer != null)
			{
				eventTimer.Dispose();
			}
			if (@class.netUser_0 != null)
			{
				int index = UnityEngine.Random.Range(0, @class.worldZone_0.WarpZone.Spawns.Count);
				Helper.TeleportTo(@class.netUser_0, @class.worldZone_0.WarpZone.Spawns[index]);
				Broadcast.Notice(@class.netUser_0, "☢", Config.GetMessageTeleport("Player.WarpZone.Teleported", @class.netUser_0, @class.worldZone_0, null), 2f);
			}
		}
	}
}
