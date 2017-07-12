using Facepunch.MeshBatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class Truth
	{
		public struct PlayerShotEyes
		{
			public Vector3 origin;

			public Angle2 angles;

			public uint count;
		}

		public enum HackMethod
		{
			None,
			AimedHack,
			SpeedHack,
			MoveHack,
			JumpHack,
			WallHack,
			FallHack,
			NetExploit,
			OtherHack
		}

		[CompilerGenerated]
		private sealed class Class48
		{
			public TruthDetector truthDetector_0;

			public bool method_0(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.truthDetector_0.netUser && eventTimer_0.Command == "home";
			}

			public bool method_1(EventTimer eventTimer_0)
			{
				return eventTimer_0.Sender == this.truthDetector_0.netUser && eventTimer_0.Command == "clan";
			}

			public bool method_2(EventTimer eventTimer_0)
			{
				bool result;
				if (eventTimer_0.Sender != this.truthDetector_0.netUser)
				{
					if (eventTimer_0.Target != this.truthDetector_0.netUser)
					{
						result = false;
						return result;
					}
				}
				result = (eventTimer_0.Command == "tp");
				return result;
			}
		}

		private static string string_0 = "INSERT INTO `db_punish_logs` (`steam_id`, `reason`, `details`) VALUES ({0}, {1}, {2});";

		private static string string_1 = "UPDATE `db_users` SET `violation_date`='{1}' WHERE `steam_id`={0} LIMIT 1;";

		public static string Punishment = "NOTICE";

		public static int ReportRank = 2;

		public static int MaxViolations = 5;

		public static bool ViolationDetails = false;

		public static int ViolationTimelife = 1440;

		public static string ViolationColor = "#FF3030";

		public static bool CheckAimbot = true;

		public static bool CheckWallhack = true;

		public static bool CheckJumphack = true;

		public static bool CheckFallhack = true;

		public static bool CheckSpeedhack = true;

		public static bool CheckShotRange = true;

		public static bool CheckShotEyes = true;

		public static float ShotAboveMaxDistance = 0f;

		public static bool ShotThroughObjectBlock = true;

		public static bool ShotThroughObjectPunish = false;

		public static int HeadshotThreshold = 200;

		public static float MaxMovementSpeed = 12f;

		public static float MaxJumpingHeight = 10f;

		public static float MinFallingHeight = 40f;

		public static float MinShotRateByRange = 1.5f;

		public static float HeadshotAimTime = 9f;

		public static bool BannedBlockIP = false;

		public static int BannedPeriod = 43200;

		public static string[] BannedExcludeIP = new string[]
		{
			"127.0.0.1"
		};

		public static bool RustProtect = false;

		public static string RustProtectKey = "0x00000000";

		public static bool RustProtectChangeKey = false;

		public static uint RustProtectChangeKeyInterval = 600u;

		public static bool RustProtectSteamHWID = false;

		public static string RustProtectHash = "0xFFFFFFFF";

		public static uint RustProtectMaxTicks = 5u;

		public static bool RustProtectSnapshots = false;

		public static uint RustProtectSnapshotsMaxCount = 10u;

		public static uint RustProtectSnapshotsInterval = 600u;

		public static uint RustProtectSnapshotsPacketSize = 1024u;

		public static string RustProtectSnapshotsPath = "serverdata/snapshot";

		public static Dictionary<NetUser, float> LastPacketTime = new Dictionary<NetUser, float>();

		public static Dictionary<ulong, SnapshotData> SnapshotsData = new Dictionary<ulong, SnapshotData>();

		public static byte[] RustProtectCode = new byte[]
		{
			240,
			76,
			195,
			15,
			241,
			188,
			91,
			219,
			149,
			168,
			22,
			28,
			150,
			190,
			125,
			106
		};

		public static byte[] RustProtectKick = new byte[]
		{
			167,
			231,
			44,
			93,
			42,
			70,
			134,
			168,
			65,
			220,
			110,
			66,
			209,
			253,
			253,
			134
		};

		public static Dictionary<NetUser, double> FallHeight = new Dictionary<NetUser, double>();

		public static Dictionary<NetUser, double> AirMovement = new Dictionary<NetUser, double>();

		public static Dictionary<NetUser, Truth.PlayerShotEyes> WeaponShotEyes = new Dictionary<NetUser, Truth.PlayerShotEyes>();

		public static List<ulong> Exclude = new List<ulong>();

		public static string[] PunishAction = new string[0];

		public static string PunishReason = "";

		public static string PunishDetails = "";

		public static Truth.HackMethod HackDetected = Truth.HackMethod.None;

		private static TruthDetector.ActionTaken actionTaken_0;

		public static float ProtectionUpdateTime = 0f;

		public static long ProtectionHash = Truth.RustProtectHash.ToInt32();

		public static int ProtectionKey = Truth.RustProtectKey.ToInt32();

		public static int Rate
		{
			get
			{
				return Convert.ToInt32((float)ServerRuntime.TargetFrameRate / NetCull.sendRate * 5f);
			}
		}

		public static TruthDetector.ActionTaken NoteMoved(TruthDetector detector, ref Vector3 pos, Angle2 ang, double time)
		{
			Truth.HackDetected = Truth.HackMethod.None;
			Truth.actionTaken_0 = 0;
			try
			{
				uint num = detector.notedTime;
				detector.notedTime = (uint)Environment.TickCount;
				if (num > 0u)
				{
					num = (uint)(Environment.TickCount - (int)num);
				}
				UserData bySteamID = Users.GetBySteamID(detector.netUser.userID);
				if (detector.prevSnap.time > 0.0)
				{
					double num2 = time - detector.prevSnap.time;
					if (detector.ignoreSeconds > 0.0)
					{
						if (time > detector.prevSnap.time)
						{
							detector.ignoreSeconds -= num2;
						}
					}
					else if (!detector.netUser.admin && !Truth.Exclude.Contains(detector.netUser.userID))
					{
						if (Truth.CheckWallhack && Truth.smethod_0(detector, detector.prevSnap.pos, ref pos))
						{
                            Truth.actionTaken_0 = (TruthDetector.ActionTaken)2;
						}
						else if (Truth.CheckSpeedhack && Truth.smethod_1(detector, detector.prevSnap.pos, pos, num2))
						{
                            Truth.actionTaken_0 = (TruthDetector.ActionTaken)2;
						}
						else if (Truth.smethod_2(detector, detector.prevSnap.pos, ref pos, num2))
						{
                            Truth.actionTaken_0 = (TruthDetector.ActionTaken)2;
						}
					}
				}
				if (Truth.actionTaken_0 == 0)
				{
					detector.prevSnap.pos = pos;
					detector.prevSnap.time = time;
					detector.Record();
					if (detector.violation > 0)
					{
						detector.violation--;
					}
					if (bySteamID != null && bySteamID.Violations > 0 && bySteamID.ViolationDate.Ticks > 0L)
					{
						DateTime dateTime = bySteamID.ViolationDate.AddMinutes((double)Truth.ViolationTimelife);
						if (dateTime < DateTime.Now)
						{
							bySteamID.ViolationDate = dateTime;
							bySteamID.Violations--;
						}
					}
				}
				else
				{
					bool arg_26A_0;
					if (truth.punish && num > 0u)
					{
						long arg_262_0 = (long)((ulong)num);
						uLink.NetworkPlayer networkPlayer = detector.netUser.networkPlayer;
						arg_26A_0 = (arg_262_0 <= (long)networkPlayer.averagePing);
					}
					else
					{
						arg_26A_0 = true;
					}
					if (!arg_26A_0)
					{
						Truth.actionTaken_0 = Truth.Punish(detector.netUser, bySteamID, Truth.HackDetected, false);
						Truth.PunishReason = "";
						Truth.PunishDetails = "";
					}
				}
			}
			catch (Exception ex)
			{
				Helper.LogError(ex.ToString(), true);
			}
			return Truth.actionTaken_0;
		}

		public static TruthDetector.ActionTaken Punish(NetUser netUser, UserData userData, Truth.HackMethod hackMethod, bool PunishBan = false)
		{
			if (server.log > 1 && Users.HasFlag(netUser.userID, UserFlags.admin))
			{
				if (hackMethod == Truth.HackMethod.AimedHack)
				{
					Broadcast.Message(netUser, string.Concat(new object[]
					{
						"Violation ",
						netUser.truthDetector.violation,
						"(+",
						100,
						") of ",
						truth.threshold
					}), "TRUTH", 0f);
				}
				else
				{
					Broadcast.Message(netUser, string.Concat(new object[]
					{
						"Violation ",
						netUser.truthDetector.violation,
						"(+",
						Truth.Rate,
						") of ",
						truth.threshold
					}), "TRUTH", 0f);
				}
			}
			string text;
			TruthDetector.ActionTaken result;
			switch (hackMethod)
			{
			case Truth.HackMethod.AimedHack:
				text = "'Aimbot Hack'";
				netUser.truthDetector.violation += truth.threshold;
				break;
			case Truth.HackMethod.SpeedHack:
				text = "'Speed Hack'";
				netUser.truthDetector.violation += Truth.Rate;
				break;
			case Truth.HackMethod.MoveHack:
				text = "'Move Hack'";
				netUser.truthDetector.violation += Truth.Rate;
				break;
			case Truth.HackMethod.JumpHack:
				text = "'Jump Hack'";
				netUser.truthDetector.violation += Truth.Rate;
				break;
			case Truth.HackMethod.WallHack:
				text = "'Wall Hack'";
				netUser.truthDetector.violation += Truth.Rate;
				break;
			case Truth.HackMethod.FallHack:
				text = "'Fall Hack'";
				netUser.truthDetector.violation += truth.threshold;
				break;
			case Truth.HackMethod.NetExploit:
				text = "'Network Exploit'";
				netUser.truthDetector.violation += truth.threshold;
				break;
			case Truth.HackMethod.OtherHack:
				text = "'Object Hack'";
				netUser.truthDetector.violation += Truth.Rate;
				break;
			default:
				result = 0;
				return result;
			}
			if (netUser.truthDetector.violation >= truth.threshold)
			{
				if (Truth.MaxViolations != -1 && userData != null)
				{
					userData.ViolationDate = DateTime.Now;
					userData.Violations++;
				}
				netUser.truthDetector.violation = 0;
				if (Truth.MaxViolations != -1 && (Truth.PunishAction.Contains("BAN") || PunishBan || Truth.MaxViolations <= 0 || userData.Violations >= Truth.MaxViolations))
				{
					Users.SetViolations(userData.SteamID, 0);
					DateTime period = default(DateTime);
					if (Truth.BannedPeriod > 0)
					{
						period = DateTime.Now.AddMinutes((double)Truth.BannedPeriod);
					}
					Truth.PunishReason = Config.GetMessageTruth("Truth.Logger.Banned", netUser, text, userData.Violations, default(DateTime));
					if (Truth.PunishDetails != "")
					{
						Helper.LogError(string.Concat(new object[]
						{
							"Violated [",
							netUser.displayName,
							":",
							netUser.userID,
							"]: ",
							Truth.PunishDetails
						}), Truth.ViolationDetails);
					}
					Helper.Log(Truth.PunishReason, true);
					if (Truth.ReportRank > 0)
					{
						Broadcast.MessageGM(Truth.PunishReason);
					}
					if (Core.DatabaseType.Equals("MYSQL"))
					{
						MySQL.Update(string.Format(Truth.string_0, userData.SteamID, MySQL.QuoteString(Truth.PunishReason), MySQL.QuoteString(Truth.PunishDetails)));
						MySQL.Update(string.Format(Truth.string_1, userData.SteamID, userData.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss")));
					}
					if (Truth.PunishAction.Contains("NOTICE"))
					{
						Broadcast.Message(Truth.ViolationColor, netUser, Config.GetMessageTruth("Truth.Violation.Banned", netUser, text, userData.Violations, period), null, 0f);
						Broadcast.MessageAll(Truth.ViolationColor, Config.GetMessageTruth("Truth.Punish.Banned", netUser, text, userData.Violations, default(DateTime)), netUser);
						Broadcast.MessageAll(Truth.ViolationColor, Truth.PunishDetails, null);
					}
					else
					{
						Broadcast.Message(Truth.ViolationColor, netUser, Config.GetMessageTruth("Truth.Violation.Banned", netUser, text, userData.Violations, period), null, 0f);
						Broadcast.Message(Truth.ViolationColor, netUser, Truth.PunishDetails, null, 0f);
					}
					if (Truth.BannedBlockIP && !Truth.BannedExcludeIP.Contains(userData.LastConnectIP))
					{
						Blocklist.Add(userData.LastConnectIP);
					}
					Users.Ban(netUser.userID, "Banned for using " + text + " by SERVER.", period, Truth.PunishDetails);
					netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    result = (TruthDetector.ActionTaken)1;
					return result;
				}
				Truth.PunishReason = Config.GetMessageTruth("Truth.Logger.Notice", netUser, text, userData.Violations, default(DateTime));
				if (Truth.PunishDetails != "")
				{
					Helper.LogError(string.Concat(new object[]
					{
						"Violated [",
						netUser.displayName,
						":",
						netUser.userID,
						"]: ",
						Truth.PunishDetails
					}), Truth.ViolationDetails);
				}
				Helper.Log(Truth.PunishReason, true);
				if (Truth.ReportRank > 0)
				{
					Broadcast.MessageGM(Truth.PunishReason);
				}
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Truth.string_0, userData.SteamID, MySQL.QuoteString(Truth.PunishReason), MySQL.QuoteString(Truth.PunishDetails)));
					MySQL.Update(string.Format(Truth.string_1, userData.SteamID, userData.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss")));
				}
				string messageTruth = Config.GetMessageTruth("Truth.Violation.Notice", netUser, text, userData.Violations, default(DateTime));
				string messageTruth2 = Config.GetMessageTruth("Truth.Punish.Notice", netUser, text, userData.Violations, default(DateTime));
				if (Truth.PunishAction.Contains("KILL"))
				{
					messageTruth = Config.GetMessageTruth("Truth.Violation.Killed", netUser, text, userData.Violations, default(DateTime));
					messageTruth2 = Config.GetMessageTruth("Truth.Punish.Killed", netUser, text, userData.Violations, default(DateTime));
				}
				if (Truth.PunishAction.Contains("KICK"))
				{
					messageTruth = Config.GetMessageTruth("Truth.Violation.Kicked", netUser, text, userData.Violations, default(DateTime));
					messageTruth2 = Config.GetMessageTruth("Truth.Punish.Kicked", netUser, text, userData.Violations, default(DateTime));
				}
				if (Truth.PunishAction.Contains("NOTICE"))
				{
					Broadcast.Message(Truth.ViolationColor, netUser, messageTruth, null, 0f);
					Broadcast.MessageAll(Truth.ViolationColor, messageTruth2, netUser);
					Broadcast.MessageAll(Truth.ViolationColor, Truth.PunishDetails, null);
				}
				if (Truth.PunishAction.Contains("KILL"))
				{
					TakeDamage.KillSelf(netUser.playerClient.controllable.character, null);
				}
				if (Truth.PunishAction.Contains("KICK"))
				{
					netUser.Kick(NetError.Facepunch_Kick_Violation, true);
				}
			}
			result = Truth.actionTaken_0;
			return result;
		}

		private static bool smethod_0(TruthDetector truthDetector_0, Vector3 vector3_0, ref Vector3 vector3_1)
		{
			Vector3 vector = vector3_1 - vector3_0;
			bool result;
			if (vector.magnitude == 0f)
			{
				result = false;
			}
			else
			{
				Ray ray = new Ray(vector3_0 + new Vector3(0f, 0.75f, 0f), vector.normalized);
				RaycastHit raycastHit;
				bool flag;
				MeshBatchInstance meshBatchInstance;
				if (!MeshBatchPhysics.SphereCast(ray, 0.1f, out raycastHit, vector.magnitude, 538444803, out flag, out meshBatchInstance))
				{
					result = false;
				}
				else
				{
					IDMain iDMain = flag ? meshBatchInstance.idMain : IDBase.GetMain(raycastHit.collider);
					GameObject gameObject = (iDMain != null) ? iDMain.gameObject : raycastHit.collider.gameObject;
					string text = gameObject.name.Trim();
					DeployableObject component = gameObject.GetComponent<DeployableObject>();
					StructureComponent component2 = gameObject.GetComponent<StructureComponent>();
					if (text == "")
					{
						text = "Mesh Texture";
					}
					else if (component != null)
					{
						text = Helper.NiceName(component.name);
						if (truthDetector_0.netUser.userID == component.ownerID)
						{
							result = false;
							return result;
						}
						if (Users.SharedGet(component.ownerID, truthDetector_0.netUser.userID))
						{
							result = false;
							return result;
						}
					}
					else if (component2 != null)
					{
						text = Helper.NiceName(component2.name);
						if (truthDetector_0.netUser.userID == component2._master.ownerID)
						{
							result = false;
							return result;
						}
						if (Users.SharedGet(component2._master.ownerID, truthDetector_0.netUser.userID))
						{
							result = false;
							return result;
						}
					}
					Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.WallHack", truthDetector_0.netUser, "", 0, default(DateTime));
					Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.NAME%", text);
					Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.POS%", raycastHit.point.AsString());
					Truth.HackDetected = Truth.HackMethod.WallHack;
					vector3_1 = Truth.MoveBack(truthDetector_0, vector3_0, vector3_1);
					result = true;
				}
			}
			return result;
		}

		private static bool smethod_1(TruthDetector truthDetector_0, Vector3 vector3_0, Vector3 vector3_1, double double_0)
		{
			Predicate<EventTimer> predicate = null;
			Predicate<EventTimer> predicate2 = null;
			Predicate<EventTimer> predicate3 = null;
			Truth.Class48 @class = new Truth.Class48();
			@class.truthDetector_0 = truthDetector_0;
			bool result;
			if (double_0 > 0.0)
			{
				double num = (double)new Vector2(vector3_1.x - vector3_0.x, vector3_1.z - vector3_0.z).magnitude;
				double num2 = num / double_0;
				if (num2 == 0.0)
				{
					result = false;
					return result;
				}
				List<EventTimer> timer = Events.Timer;
				if (predicate == null)
				{
					predicate = new Predicate<EventTimer>(@class.method_0);
				}
				EventTimer eventTimer = timer.Find(predicate);
				if (eventTimer != null)
				{
					eventTimer.Dispose();
					Broadcast.Notice(@class.truthDetector_0.netUser.networkPlayer, "☢", Config.GetMessageCommand("Command.Home.Interrupt", "", @class.truthDetector_0.netUser), 5f);
				}
				List<EventTimer> timer2 = Events.Timer;
				if (predicate2 == null)
				{
					predicate2 = new Predicate<EventTimer>(@class.method_1);
				}
				EventTimer eventTimer2 = timer2.Find(predicate2);
				if (eventTimer2 != null)
				{
					eventTimer2.Dispose();
					Broadcast.Notice(@class.truthDetector_0.netUser.networkPlayer, "☢", Config.GetMessageCommand("Command.Clan.Warp.Interrupt", "", @class.truthDetector_0.netUser), 5f);
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
						Broadcast.Notice(eventTimer3.Sender, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", eventTimer3.Sender), 5f);
					}
					if (eventTimer3.Target != null)
					{
						Broadcast.Notice(eventTimer3.Target, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", eventTimer3.Target), 5f);
					}
					eventTimer3.Dispose();
				}
				if (num2 > (double)Truth.MaxMovementSpeed)
				{
					if (server.log > 1 && Users.HasFlag(@class.truthDetector_0.netUser.userID, UserFlags.admin))
					{
						Broadcast.Message(@class.truthDetector_0.netUser, "[COLOR#D02F2F]MovementSpeed: " + num2.ToString("0.0") + " of maximum " + Truth.MaxMovementSpeed.ToString("0.0"), "DEBUG", 0f);
					}
					Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.SpeedHack", @class.truthDetector_0.netUser, "", 0, default(DateTime));
					Truth.PunishDetails = Truth.PunishDetails.Replace("%SPEED.MOVEMENT%", num2.ToString("0.00"));
					Truth.PunishDetails = Truth.PunishDetails.Replace("%SPEED.MAXIMUM%", Truth.MaxMovementSpeed.ToString("0.00"));
					Truth.HackDetected = Truth.HackMethod.SpeedHack;
					vector3_1 = Truth.MoveBack(@class.truthDetector_0, vector3_0, vector3_1);
					result = true;
					return result;
				}
				if (server.log > 2 && Users.HasFlag(@class.truthDetector_0.netUser.userID, UserFlags.admin) && num2 > 1.0)
				{
					Broadcast.Message(@class.truthDetector_0.netUser, "MovementSpeed: " + num2.ToString("0.0") + " of maximum " + Truth.MaxMovementSpeed.ToString("0.0"), "DEBUG", 0f);
				}
			}
			result = false;
			return result;
		}

		private static bool smethod_2(TruthDetector truthDetector_0, Vector3 vector3_0, ref Vector3 vector3_1, double double_0)
		{
			bool result;
			if (double_0 > 0.0)
			{
				double num = (double)(vector3_1.y - vector3_0.y) / double_0;
				UserData bySteamID = Users.GetBySteamID(truthDetector_0.netUser.userID);
				Character idMain = truthDetector_0.netUser.playerClient.controllable.idMain;
				if (!Truth.FallHeight.ContainsKey(truthDetector_0.netUser))
				{
					Truth.FallHeight.Add(truthDetector_0.netUser, 0.0);
				}
				if (!Truth.AirMovement.ContainsKey(truthDetector_0.netUser))
				{
					Truth.AirMovement.Add(truthDetector_0.netUser, 0.0);
				}
				if (idMain.stateFlags.airborne)
				{
					Truth.AirMovement[truthDetector_0.netUser] = 0.0;
				}
				if (Truth.CheckJumphack && num > 0.0 && idMain != null && idMain.stateFlags.airborne)
				{
					truthDetector_0.jumpHeight += num;
					if (truthDetector_0.jumpHeight <= (double)Truth.MaxJumpingHeight && num <= (double)(Truth.MaxJumpingHeight * 2f))
					{
						if (server.log > 2 && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin) && truthDetector_0.jumpHeight > 1.0)
						{
							Broadcast.Message(truthDetector_0.netUser, "JumpHeight: " + truthDetector_0.jumpHeight.ToString("0.0") + " of maximum " + Truth.MaxJumpingHeight.ToString("0.0"), "DEBUG", 0f);
						}
					}
					else
					{
						if (server.log > 1 && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin))
						{
							Broadcast.Message(truthDetector_0.netUser, "[COLOR#D02F2F]JumpHeight: " + truthDetector_0.jumpHeight.ToString("0.0") + " of maximum " + Truth.MaxJumpingHeight.ToString("0.0"), "DEBUG", 0f);
						}
						Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.JumpHack", truthDetector_0.netUser, "", 0, default(DateTime));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%JUMP.HEIGHT%", truthDetector_0.jumpHeight.ToString("0.00"));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%JUMP.MAXHEIGHT%", Truth.MaxJumpingHeight.ToString("0.00"));
						Truth.HackDetected = Truth.HackMethod.JumpHack;
						vector3_1 = Truth.MoveBack(truthDetector_0, vector3_0, vector3_1);
					}
					result = (Truth.HackDetected == Truth.HackMethod.JumpHack);
					return result;
				}
				if (Truth.CheckFallhack && num < 0.0 && idMain != null && idMain.stateFlags.airborne)
				{
					Dictionary<NetUser, double> fallHeight;
					NetUser netUser;
					(fallHeight = Truth.FallHeight)[netUser = truthDetector_0.netUser] = fallHeight[netUser] + -num;
					if (Truth.FallHeight[truthDetector_0.netUser] >= (double)Truth.MinFallingHeight && bySteamID != null && bySteamID.FallCheck != FallCheckState.damaged)
					{
						bySteamID.FallCheck = FallCheckState.check;
						if (server.log > 2 && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin) && Truth.FallHeight[truthDetector_0.netUser] > 1.0)
						{
							Broadcast.Message(truthDetector_0.netUser, "[COLOR#D02F2F]FallHeight: " + Truth.FallHeight[truthDetector_0.netUser].ToString("0.00") + " of minimum " + Truth.MinFallingHeight.ToString("0.0"), "DEBUG", 0f);
						}
					}
					else if (server.log > 2 && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin) && Truth.FallHeight[truthDetector_0.netUser] > 1.0)
					{
						Broadcast.Message(truthDetector_0.netUser, "FallHeight: " + Truth.FallHeight[truthDetector_0.netUser].ToString("0.00") + " of minimum " + Truth.MinFallingHeight.ToString("0.0"), "DEBUG", 0f);
					}
				}
				else if (!idMain.stateFlags.airborne)
				{
					if (Truth.CheckFallhack && Truth.FallHeight[truthDetector_0.netUser] >= (double)Truth.MinFallingHeight)
					{
						if (bySteamID != null && bySteamID.FallCheck == FallCheckState.check)
						{
							Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.FallHack", truthDetector_0.netUser, "", 0, default(DateTime));
							Truth.PunishDetails = Truth.PunishDetails.Replace("%FALL.HEIGHT%", Truth.FallHeight[truthDetector_0.netUser].ToString("0.00"));
							Truth.PunishDetails = Truth.PunishDetails.Replace("%FALL.MINHEIGHT%", Truth.MinFallingHeight.ToString("0.00"));
							Truth.HackDetected = Truth.HackMethod.FallHack;
						}
					}
					else if (MeshBatchPhysics.OverlapSphere(vector3_1, 0.5f, 538444803).Length == 0)
					{
						Dictionary<NetUser, double> airMovement;
						NetUser netUser2;
						(airMovement = Truth.AirMovement)[netUser2 = truthDetector_0.netUser] = airMovement[netUser2] + 1.0;
						if (Truth.AirMovement[truthDetector_0.netUser] > (double)NetCull.sendRate)
						{
							Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.MoveHack", truthDetector_0.netUser, "", 0, default(DateTime));
							Truth.AirMovement[truthDetector_0.netUser] = 0.0;
							Truth.HackDetected = Truth.HackMethod.MoveHack;
						}
					}
					bySteamID.FallCheck = FallCheckState.none;
					Truth.FallHeight[truthDetector_0.netUser] = (truthDetector_0.jumpHeight = 0.0);
					result = (Truth.HackDetected == Truth.HackMethod.FallHack || Truth.HackDetected == Truth.HackMethod.MoveHack);
					return result;
				}
			}
			result = false;
			return result;
		}

		public static Vector3 MoveBack(TruthDetector detector, Vector3 oldpos, Vector3 newpos)
		{
			Vector3 vector = oldpos;
			if (!Truth.smethod_3(vector, newpos) && detector.snapshots.length > 0)
			{
				do
				{
					vector = detector.snapshots.array[detector.snapshots.end].pos;
				}
				while (!Truth.smethod_3(vector, newpos) && detector.Rollback());
				detector.prevSnap = detector.snapshots.array[detector.snapshots.end];
			}
			return vector;
		}

		private static bool smethod_3(Vector3 vector3_0, Vector3 vector3_1)
		{
			float num = vector3_0.x - vector3_1.x;
			float num2 = vector3_0.y - vector3_1.y;
			float num3 = vector3_0.z - vector3_1.z;
			return num * num + num2 * num2 + num3 * num3 >= 0.25f;
		}

		public static bool Test_WeaponShot(Character Killer, GameObject hitObj, IBulletWeaponItem weapon, ItemRepresentation rep, Transform transform, Vector3 endPos, bool isHeadshot)
		{
			bool result;
			if (Killer == null || transform == null)
			{
				result = true;
			}
			else if (transform == null || Killer == null || Killer.netUser == null)
			{
				result = true;
			}
			else if (float.IsNaN(endPos.x) || float.IsNaN(endPos.y) || float.IsNaN(endPos.z))
			{
				result = true;
			}
			else
			{
				Character component = hitObj.GetComponent<Character>();
				NetUser netUser = (!(Killer != null) || !Killer.controllable) ? null : Killer.netUser;
				NetUser netUser2 = (!(component != null) || !component.controllable) ? null : component.netUser;
				Vector3 origin = Helper.GetEyesRay(Killer).origin;
				float num = Vector3.Distance(origin, endPos);
				float bulletRange = ((BulletWeaponDataBlock)rep.datablock).bulletRange;
				if (component == null)
				{
					if (num > bulletRange)
					{
						result = true;
					}
					else
					{
						Collider[] array = Physics.OverlapSphere(Killer.eyesRay.origin, 0.2f);
						for (int i = 0; i < array.Length; i++)
						{
							Collider collider = array[i];
							IDBase component2 = collider.gameObject.GetComponent<IDBase>();
							if (component2 != null && component2.idMain is StructureMaster)
							{
								result = true;
								return result;
							}
						}
						IDMain idMain = IDBase.GetMain(hitObj).idMain;
						if (idMain.GetComponent<StructureComponent>() == null && idMain.GetComponent<SleepingAvatar>() == null)
						{
							Ray lookRay = Helper.GetLookRay(Killer);
							Vector3 position = hitObj.transform.position;
							position.y += 0.1f;
							if (Physics.RaycastAll(lookRay, Vector3.Distance(lookRay.origin, position), -1).Length > 1)
							{
								result = true;
								return result;
							}
						}
						result = false;
					}
				}
				else if (!Truth.CheckAimbot || netUser.admin || component.dead)
				{
					result = false;
				}
				else
				{
					string newValue = Helper.NiceName((netUser2 != null) ? netUser2.displayName : component.name);
					if (!Truth.WeaponShotEyes.ContainsKey(netUser))
					{
						Truth.WeaponShotEyes.Add(netUser, new Truth.PlayerShotEyes
						{
							origin = Killer.eyesRay.origin,
							angles = Killer.eyesAngles,
							count = 0u
						});
					}
					float magnitude = (transform.position - endPos).magnitude;
					if (magnitude > 3f)
					{
						Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.JackedSilent", netUser, "", 0, default(DateTime));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", netUser.displayName);
						Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
						Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
						Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
						Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
						Truth.Punish(netUser, Users.GetBySteamID(netUser.userID), Truth.HackMethod.AimedHack, true);
						result = true;
					}
					else if (num < 1f)
					{
						result = false;
					}
					else
					{
						if (Truth.ShotThroughObjectBlock)
						{
							Vector3 vector;
							GameObject lineObject = Helper.GetLineObject(origin, endPos, out vector, 406721553);
							if (lineObject != null && (lineObject.GetComponent<StructureComponent>() != null || lineObject.GetComponent<BasicDoor>() != null))
							{
								Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.ShootBlocked", netUser, "", 0, default(DateTime));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", netUser.displayName);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT%", Helper.NiceName(lineObject.name));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.NAME%", Helper.NiceName(lineObject.name));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.POS%", lineObject.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%POINT%", vector.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
								if (!Killer.stateFlags.movement)
								{
									if (Truth.ShotThroughObjectPunish)
									{
										Truth.Punish(netUser, Users.GetBySteamID(netUser.userID), Truth.HackMethod.AimedHack, false);
									}
									else
									{
										Helper.LogError(string.Concat(new object[]
										{
											"Blocked [",
											netUser.displayName,
											":",
											netUser.userID,
											"]: ",
											Truth.PunishDetails
										}), true);
									}
									result = true;
									return result;
								}
								Vector3 pos = netUser.truthDetector.prevSnap.pos;
								pos.x = origin.x;
								pos.z = origin.z;
								if (Helper.GetLineObject(pos, endPos, out vector, 406721553) == lineObject)
								{
									Helper.LogError(string.Concat(new object[]
									{
										"Blocked [",
										netUser.displayName,
										":",
										netUser.userID,
										"]: ",
										Truth.PunishDetails
									}), true);
									result = true;
									return result;
								}
							}
						}
						uint num2 = (uint)(Environment.TickCount - (int)netUser.truthDetector.prevHitTime);
						if (num2 == (uint)Environment.TickCount)
						{
							num2 = 0u;
						}
						netUser.truthDetector.prevHitTime = (uint)Environment.TickCount;
						if (num2 > 100u && (ulong)num2 < (ulong)((long)Environment.TickCount))
						{
							float minShotRateByRange = Truth.MinShotRateByRange;
							float num3 = num2 / num;
							Config.Get("SERVER", "Truth.MinShotRateByRange." + weapon.datablock.name, ref minShotRateByRange, true);
							if (num3 < minShotRateByRange)
							{
								Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.HighFireRate", netUser, "", 0, default(DateTime));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", netUser.displayName);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%SHOTRATE%", num3.ToString("N2"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%MINRATE%", minShotRateByRange.ToString("N2"));
								Truth.Punish(netUser, Users.GetBySteamID(netUser.userID), Truth.HackMethod.AimedHack, false);
								result = true;
								return result;
							}
						}
						if (Truth.CheckShotRange && Math.Abs(num) > bulletRange)
						{
							Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.OverWeaponRange", netUser, "", 0, default(DateTime));
							Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", netUser.displayName);
							Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
							Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
							Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
							Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
							Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
							Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
							if (netUser2 != null)
							{
								bool punishBan = Truth.ShotAboveMaxDistance > 0f && num - bulletRange >= Truth.ShotAboveMaxDistance;
								Truth.Punish(netUser, Users.GetBySteamID(netUser.userID), Truth.HackMethod.AimedHack, punishBan);
							}
							else
							{
								Broadcast.MessageAll(Truth.ViolationColor, Truth.PunishDetails, null);
								Helper.LogError(string.Concat(new object[]
								{
									"Noticed [",
									netUser.displayName,
									":",
									netUser.userID,
									"]: ",
									Truth.PunishDetails
								}), true);
							}
							result = true;
						}
						else
						{
							float num4 = Truth.HeadshotAimTime * num;
							if (num2 > num4)
							{
								netUser.truthDetector.headshotHold = 0;
							}
							if (isHeadshot)
							{
								netUser.truthDetector.headshotHold += (int)num;
							}
							if (netUser.truthDetector.headshotHold >= Truth.HeadshotThreshold)
							{
								Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.ThresholdHeadshots", netUser, "", 0, default(DateTime));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", netUser.displayName);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
								Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
								Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
								Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
								Truth.Punish(netUser, Users.GetBySteamID(netUser.userID), Truth.HackMethod.AimedHack, false);
								result = true;
							}
							else
							{
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		public static void Test_WeaponShotEyes(Character character, Angle2 angle)
		{
			if (Truth.WeaponShotEyes.ContainsKey(character.netUser))
			{
				Truth.PlayerShotEyes value = Truth.WeaponShotEyes[character.netUser];
				if (!object.Equals(angle, value.angles))
				{
					Truth.WeaponShotEyes.Remove(character.netUser);
				}
				else
				{
					value.count += 1u;
					if (value.count >= NetCull.sendRate / 5f)
					{
						Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.NoRecoil", null, "", 0, default(DateTime));
						Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", character.netUser.displayName);
						Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", character.GetComponent<InventoryHolder>().itemRepresentation.datablock.name);
						Truth.Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), Truth.HackMethod.AimedHack, false);
						Truth.WeaponShotEyes.Remove(character.netUser);
					}
					else
					{
						Truth.WeaponShotEyes[character.netUser] = value;
					}
				}
			}
		}

		public static bool GetClientVerify(HumanController controller, ref Vector3 origin, long encoded, ushort flags, uLink.NetworkMessageInfo info)
		{
			bool result;
			if (Truth.RustProtect)
			{
				UserData bySteamID = Users.GetBySteamID(controller.netUser.userID);
				if (bySteamID == null)
				{
					string[] messages = Config.GetMessages("Truth.Protect.NoUserdata", controller.netUser);
					for (int i = 0; i < messages.Length; i++)
					{
						string text = messages[i];
						Broadcast.Message(controller.netUser, text, null, 0f);
					}
					controller.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
					result = false;
					return result;
				}
				if (origin == Vector3.zero && (info.sender.externalIP == "213.141.149.103" || controller.netUser.admin || Truth.Exclude.Contains(controller.netUser.userID)))
				{
					bySteamID.ProtectTick = 0;
					result = false;
					return result;
				}
				if (bySteamID.ProtectTime == 0f)
				{
					if (server.log > 2)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Protection Key Sended [",
							bySteamID.Username,
							":",
							bySteamID.SteamID,
							":",
							bySteamID.LastConnectIP,
							"]: ProtectKey=",
							string.Format("0x{0:X8}", Truth.ProtectionKey)
						}));
					}
					bySteamID.ProtectTick = 0;
					bySteamID.ProtectTime = Time.time;
					flags = 32768;
					float num = 0f;
					controller.networkView.RPC("ReadClientMove", info.sender, new object[]
					{
						Vector3.zero,
						Truth.ProtectionKey,
						32768,
						num
					});
					result = false;
					return result;
				}
				if (origin == Vector3.zero && flags == 32767)
				{
					if (server.log > 2)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Received Protect Data [",
							bySteamID.Username,
							":",
							bySteamID.SteamID,
							":",
							bySteamID.LastConnectIP,
							"]: Data=",
							string.Format("0x{0:X8}", encoded)
						}));
					}
                    //bySteamID.ProtectKickData = bySteamID.ProtectKickData.Add(encoded);
                    bySteamID.ProtectKickData.Add((int)encoded);
                    result = false;
					return result;
				}
				if (origin == Vector3.zero && flags == 32768)
				{
					bySteamID.ProtectTick = 0;
					bySteamID.ProtectTime = Time.time;
					if (bySteamID.ProtectKickData.Length > 0)
					{
						bySteamID.ProtectKickName = Helper.Int32ToString(bySteamID.ProtectKickData);
						bySteamID.ProtectKickData = new int[0];
					}
					if (server.log > 2)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Received Protect Data [",
							bySteamID.Username,
							":",
							bySteamID.SteamID,
							":",
							bySteamID.LastConnectIP,
							"]: Checksum=",
							string.Format("0x{0:X8}", encoded)
						}));
					}
					if (encoded != Truth.ProtectionHash && !controller.netUser.admin && Time.time > Truth.ProtectionUpdateTime && !Config.Loading)
					{
						string text3;
						if (bySteamID.ProtectKickName != "")
						{
							string[] messages2 = Config.GetMessages("Truth.Protect.CheatsFound", controller.netUser);
							for (int j = 0; j < messages2.Length; j++)
							{
								string text2 = messages2[j];
								Broadcast.Message(controller.netUser, text2, null, 0f);
							}
							text3 = "Detected a forbidden \"" + bySteamID.ProtectKickName + "\".";
						}
						else
						{
							string[] messages3 = Config.GetMessages("Truth.Protect.BrokenClient", controller.netUser);
							for (int k = 0; k < messages3.Length; k++)
							{
								string text4 = messages3[k];
								Broadcast.Message(controller.netUser, text4, null, 0f);
							}
							text3 = "Incorrect a CRC(" + string.Format("0x{0:X8}", encoded) + ") received from client.";
						}
						Helper.LogError(string.Concat(new object[]
						{
							"Protect Kick [",
							controller.netUser.displayName,
							":",
							controller.netUser.userID,
							"]: ",
							text3
						}), true);
						controller.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
					}
					if (bySteamID.ProtectKickName != "")
					{
						bySteamID.ProtectKickName = "";
					}
					result = false;
					return result;
				}
				if ((long)bySteamID.ProtectTick > (long)((ulong)Truth.RustProtectMaxTicks) && Time.time > Truth.ProtectionUpdateTime && !Config.Loading)
				{
					string[] messages4 = Config.GetMessages("Truth.Protect.NotProtected", controller.netUser);
					for (int l = 0; l < messages4.Length; l++)
					{
						string text5 = messages4[l];
						Broadcast.Message(controller.netUser, text5, null, 0f);
					}
					Helper.LogError(string.Concat(new object[]
					{
						"Protect Kick [",
						controller.netUser.displayName,
						":",
						controller.netUser.userID,
						"]: No packets from client protection for very long time."
					}), true);
					if (server.log > 2)
					{
						Helper.LogError(string.Concat(new object[]
						{
							"Kick Details: ProtectTick=",
							bySteamID.ProtectTick,
							", SendRate=",
							NetCull.sendRate,
							", Time=",
							Time.time,
							", Protection.UpdateTime=",
							Truth.ProtectionUpdateTime
						}), true);
					}
					controller.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
					result = false;
					return result;
				}
				if (server.log > 2)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Received Default Data [",
						bySteamID.Username,
						":",
						bySteamID.SteamID,
						":",
						bySteamID.LastConnectIP,
						"]: origin=",
						origin,
						", encoded=",
						string.Format("0x{0:X8}", encoded),
						", flags=",
						string.Format("0x{0:X8}", flags)
					}));
				}
				bySteamID.ProtectTick++;
			}
			result = true;
			return result;
		}
	}
}
