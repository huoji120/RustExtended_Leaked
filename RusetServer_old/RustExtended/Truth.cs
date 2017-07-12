namespace RustExtended
{
    using Facepunch.MeshBatch;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using uLink;
    using UnityEngine;

    public class Truth
    {
        private static TruthDetector.ActionTaken actionTaken_0;
        public static Dictionary<NetUser, double> AirMovement = new Dictionary<NetUser, double>();
        public static bool BannedBlockIP = false;
        public static string[] BannedExcludeIP = new string[] { "127.0.0.1" };
        public static int BannedPeriod = 0xa8c0;
        public static bool CheckAimbot = true;
        public static bool CheckFallhack = true;
        public static bool CheckJumphack = true;
        public static bool CheckShotEyes = true;
        public static bool CheckShotRange = true;
        public static bool CheckSpeedhack = true;
        public static bool CheckWallhack = true;
        public static System.Collections.Generic.List<ulong> Exclude = new System.Collections.Generic.List<ulong>();
        public static Dictionary<NetUser, double> FallHeight = new Dictionary<NetUser, double>();
        public static HackMethod HackDetected = HackMethod.None;
        public static float HeadshotAimTime = 9f;
        public static int HeadshotThreshold = 200;
        public static Dictionary<NetUser, float> LastPacketTime = new Dictionary<NetUser, float>();
        public static float MaxJumpingHeight = 10f;
        public static float MaxMovementSpeed = 12f;
        public static int MaxViolations = 5;
        public static float MinFallingHeight = 40f;
        public static float MinShotRateByRange = 1.5f;
        public static int ProtectionHash = RustProtectHash.ToInt32();
        public static int ProtectionKey = RustProtectKey.ToInt32();
        public static float ProtectionUpdateTime = 0f;
        public static string[] PunishAction = new string[0];
        public static string PunishDetails = "";
        public static string Punishment = "NOTICE";
        public static string PunishReason = "";
        public static int ReportRank = 2;
        public static bool RustProtect = false;
        public static bool RustProtectChangeKey = false;
        public static uint RustProtectChangeKeyInterval = 600;
        public static byte[] RustProtectCode = new byte[] { 240, 0x4c, 0xc3, 15, 0xf1, 0xbc, 0x5b, 0xdb, 0x95, 0xa8, 0x16, 0x1c, 150, 190, 0x7d, 0x6a };
        public static string RustProtectHash = "0xFFFFFFFF";
        public static string RustProtectKey = "0x00000000";
        public static byte[] RustProtectKick = new byte[] { 0xa7, 0xe7, 0x2c, 0x5d, 0x2a, 70, 0x86, 0xa8, 0x41, 220, 110, 0x42, 0xd1, 0xfd, 0xfd, 0x86 };
        public static uint RustProtectMaxTicks = 5;
        public static bool RustProtectSnapshots = false;
        public static uint RustProtectSnapshotsInterval = 600;
        public static uint RustProtectSnapshotsMaxCount = 10;
        public static uint RustProtectSnapshotsPacketSize = 0x400;
        public static string RustProtectSnapshotsPath = "serverdata/snapshot";
        public static bool RustProtectSteamHWID = false;
        public static float ShotAboveMaxDistance = 0f;
        public static bool ShotThroughObjectBlock = true;
        public static bool ShotThroughObjectPunish = false;
        public static Dictionary<ulong, SnapshotData> SnapshotsData = new Dictionary<ulong, SnapshotData>();
        private static string string_0 = "INSERT INTO `db_punish_logs` (`steam_id`, `reason`, `details`) VALUES ({0}, {1}, {2});";
        private static string string_1 = "UPDATE `db_users` SET `violation_date`='{1}' WHERE `steam_id`={0} LIMIT 1;";
        public static string ViolationColor = "#FF3030";
        public static bool ViolationDetails = false;
        public static int ViolationTimelife = 0x5a0;
        public static Dictionary<NetUser, PlayerShotEyes> WeaponShotEyes = new Dictionary<NetUser, PlayerShotEyes>();

        public static bool GetClientVerify(HumanController controller, ref Vector3 origin, int encoded, ushort flags, uLink.NetworkMessageInfo info)
        {
            if (RustProtect)
            {
                UserData bySteamID = Users.GetBySteamID(controller.netUser.userID);
                if (bySteamID == null)
                {
                    foreach (string str in Config.GetMessages("Truth.Protect.NoUserdata", controller.netUser))
                    {
                        Broadcast.Message(controller.netUser, str, null, 0f);
                    }
                    controller.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    return false;
                }
                if ((origin == Vector3.zero) && (((info.sender.externalIP == "213.141.149.103") || controller.netUser.admin) || Exclude.Contains(controller.netUser.userID)))
                {
                    bySteamID.ProtectTick = 0;
                    return false;
                }
                if (bySteamID.ProtectTime == 0f)
                {
                    if (server.log > 2)
                    {
                        Debug.Log(string.Concat(new object[] { "Protection Key Sended [", bySteamID.Username, ":", bySteamID.SteamID, ":", bySteamID.LastConnectIP, "]: ProtectKey=", string.Format("0x{0:X8}", ProtectionKey) }));
                    }
                    bySteamID.ProtectTick = 0;
                    bySteamID.ProtectTime = Time.time;
                    flags = 0x8000;
                    float num = 0f;
                    controller.networkView.RPC("ReadClientMove", info.sender, new object[] { Vector3.zero, ProtectionKey, (ushort) 0x8000, num });
                    return false;
                }
                if ((origin == Vector3.zero) && (flags == 0x7fff))
                {
                    if (server.log > 2)
                    {
                        Debug.Log(string.Concat(new object[] { "Received Protect Data [", bySteamID.Username, ":", bySteamID.SteamID, ":", bySteamID.LastConnectIP, "]: Data=", string.Format("0x{0:X8}", encoded) }));
                    }
                    bySteamID.ProtectKickData = bySteamID.ProtectKickData.Add<int>(encoded);
                    return false;
                }
                if ((origin == Vector3.zero) && (flags == 0x8000))
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
                        Debug.Log(string.Concat(new object[] { "Received Protect Data [", bySteamID.Username, ":", bySteamID.SteamID, ":", bySteamID.LastConnectIP, "]: Checksum=", string.Format("0x{0:X8}", encoded) }));
                    }
                    if (((encoded != ProtectionHash) && !controller.netUser.admin) && ((Time.time > ProtectionUpdateTime) && !Config.Loading))
                    {
                        string str2 = "Unknown Kick Reason.";
                        if (bySteamID.ProtectKickName != "")
                        {
                            foreach (string str3 in Config.GetMessages("Truth.Protect.CheatsFound", controller.netUser))
                            {
                                Broadcast.Message(controller.netUser, str3, null, 0f);
                            }
                            str2 = "Detected a forbidden \"" + bySteamID.ProtectKickName + "\".";
                        }
                        else
                        {
                            foreach (string str4 in Config.GetMessages("Truth.Protect.BrokenClient", controller.netUser))
                            {
                                Broadcast.Message(controller.netUser, str4, null, 0f);
                            }
                            str2 = "Incorrect a CRC(" + string.Format("0x{0:X8}", encoded) + ") received from client.";
                        }
                        Helper.LogError(string.Concat(new object[] { "Protect Kick [", controller.netUser.displayName, ":", controller.netUser.userID, "]: ", str2 }), true);
                        controller.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    }
                    if (bySteamID.ProtectKickName != "")
                    {
                        bySteamID.ProtectKickName = "";
                    }
                    return false;
                }
                if (((bySteamID.ProtectTick > RustProtectMaxTicks) && (Time.time > ProtectionUpdateTime)) && !Config.Loading)
                {
                    foreach (string str5 in Config.GetMessages("Truth.Protect.NotProtected", controller.netUser))
                    {
                        Broadcast.Message(controller.netUser, str5, null, 0f);
                    }
                    Helper.LogError(string.Concat(new object[] { "Protect Kick [", controller.netUser.displayName, ":", controller.netUser.userID, "]: No packets from client protection for very long time." }), true);
                    if (server.log > 2)
                    {
                        Helper.LogError(string.Concat(new object[] { "Kick Details: ProtectTick=", bySteamID.ProtectTick, ", SendRate=", NetCull.sendRate, ", Time=", Time.time, ", Protection.UpdateTime=", ProtectionUpdateTime }), true);
                    }
                    controller.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    return false;
                }
                if (server.log > 2)
                {
                    Debug.Log(string.Concat(new object[] { "Received Default Data [", bySteamID.Username, ":", bySteamID.SteamID, ":", bySteamID.LastConnectIP, "]: origin=", (Vector3) origin, ", encoded=", string.Format("0x{0:X8}", encoded), ", flags=", string.Format("0x{0:X8}", flags) }));
                }
                bySteamID.ProtectTick++;
            }
            return true;
        }

        public static Vector3 MoveBack(TruthDetector detector, Vector3 oldpos, Vector3 newpos)
        {
            Vector3 pos = oldpos;
            if (!smethod_3(pos, newpos) && (detector.snapshots.length > 0))
            {
                do
                {
                    pos = detector.snapshots.array[detector.snapshots.end].pos;
                }
                while (!smethod_3(pos, newpos) && detector.Rollback());
                detector.prevSnap = detector.snapshots.array[detector.snapshots.end];
            }
            return pos;
        }

        public static TruthDetector.ActionTaken NoteMoved(TruthDetector detector, ref Vector3 pos, Angle2 ang, double time)
        {
            UserData userData = null;
            HackDetected = HackMethod.None;
            actionTaken_0 = TruthDetector.ActionTaken.None;
            try
            {
                uint notedTime = detector.notedTime;
                detector.notedTime = (uint) Environment.TickCount;
                if (notedTime > 0)
                {
                    notedTime = ((uint) Environment.TickCount) - notedTime;
                }
                userData = Users.GetBySteamID(detector.netUser.userID);
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
                    else if (!detector.netUser.admin && !Exclude.Contains(detector.netUser.userID))
                    {
                        if (CheckWallhack && smethod_0(detector, detector.prevSnap.pos, ref pos))
                        {
                            actionTaken_0 = TruthDetector.ActionTaken.Moved;
                        }
                        else if (CheckSpeedhack && smethod_1(detector, detector.prevSnap.pos, pos, num2))
                        {
                            actionTaken_0 = TruthDetector.ActionTaken.Moved;
                        }
                        else if (smethod_2(detector, detector.prevSnap.pos, ref pos, num2))
                        {
                            actionTaken_0 = TruthDetector.ActionTaken.Moved;
                        }
                    }
                }
                if (actionTaken_0 == TruthDetector.ActionTaken.None)
                {
                    detector.prevSnap.pos = pos;
                    detector.prevSnap.time = time;
                    detector.Record();
                    if (detector.violation > 0)
                    {
                        detector.violation--;
                    }
                    if (((userData != null) && (userData.Violations > 0)) && (userData.ViolationDate.Ticks > 0L))
                    {
                        DateTime time2 = userData.ViolationDate.AddMinutes((double) ViolationTimelife);
                        if (time2 < DateTime.Now)
                        {
                            userData.ViolationDate = time2;
                            userData.Violations--;
                        }
                    }
                }
                else if ((truth.punish && (notedTime > 0)) && (notedTime > detector.netUser.networkPlayer.averagePing))
                {
                    actionTaken_0 = Punish(detector.netUser, userData, HackDetected, false);
                    PunishReason = "";
                    PunishDetails = "";
                }
            }
            catch (Exception exception)
            {
                Helper.LogError(exception.ToString(), true);
            }
            return actionTaken_0;
        }

        public static TruthDetector.ActionTaken Punish(NetUser netUser, UserData userData, HackMethod hackMethod, [Optional, DefaultParameterValue(false)] bool PunishBan)
        {
            string str = "";
            if ((server.log > 1) && Users.HasFlag(netUser.userID, UserFlags.admin))
            {
                if (hackMethod == HackMethod.AimedHack)
                {
                    Broadcast.Message(netUser, string.Concat(new object[] { "Violation ", netUser.truthDetector.violation, "(+", 100, ") of ", truth.threshold }), "TRUTH", 0f);
                }
                else
                {
                    Broadcast.Message(netUser, string.Concat(new object[] { "Violation ", netUser.truthDetector.violation, "(+", Rate, ") of ", truth.threshold }), "TRUTH", 0f);
                }
            }
            switch (hackMethod)
            {
                case HackMethod.AimedHack:
                    str = "'Aimbot Hack'";
                    netUser.truthDetector.violation += truth.threshold;
                    break;

                case HackMethod.SpeedHack:
                    str = "'Speed Hack'";
                    netUser.truthDetector.violation += Rate;
                    break;

                case HackMethod.MoveHack:
                    str = "'Move Hack'";
                    netUser.truthDetector.violation += Rate;
                    break;

                case HackMethod.JumpHack:
                    str = "'Jump Hack'";
                    netUser.truthDetector.violation += Rate;
                    break;

                case HackMethod.WallHack:
                    str = "'Wall Hack'";
                    netUser.truthDetector.violation += Rate;
                    break;

                case HackMethod.FallHack:
                    str = "'Fall Hack'";
                    netUser.truthDetector.violation += truth.threshold;
                    break;

                case HackMethod.NetExploit:
                    str = "'Network Exploit'";
                    netUser.truthDetector.violation += truth.threshold;
                    break;

                case HackMethod.OtherHack:
                    str = "'Object Hack'";
                    netUser.truthDetector.violation += Rate;
                    break;

                default:
                    return TruthDetector.ActionTaken.None;
            }
            if (netUser.truthDetector.violation >= truth.threshold)
            {
                if ((MaxViolations != -1) && (userData != null))
                {
                    userData.ViolationDate = DateTime.Now;
                    userData.Violations++;
                }
                netUser.truthDetector.violation = 0;
                if ((MaxViolations != -1) && ((PunishAction.Contains<string>("BAN") || PunishBan) || ((MaxViolations <= 0) || (userData.Violations >= MaxViolations))))
                {
                    Users.SetViolations(userData.SteamID, 0);
                    DateTime period = new DateTime();
                    if (BannedPeriod > 0)
                    {
                        period = DateTime.Now.AddMinutes((double) BannedPeriod);
                    }
                    PunishReason = Config.GetMessageTruth("Truth.Logger.Banned", netUser, str, userData.Violations, new DateTime());
                    if (PunishDetails != "")
                    {
                        Helper.LogError(string.Concat(new object[] { "Violated [", netUser.displayName, ":", netUser.userID, "]: ", PunishDetails }), ViolationDetails);
                    }
                    Helper.Log(PunishReason, true);
                    if (ReportRank > 0)
                    {
                        Broadcast.MessageGM(PunishReason);
                    }
                    if (Core.DatabaseType.Equals("MYSQL"))
                    {
                        MySQL.Update(string.Format(string_0, userData.SteamID, MySQL.QuoteString(PunishReason), MySQL.QuoteString(PunishDetails)));
                        MySQL.Update(string.Format(string_1, userData.SteamID, userData.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    if (PunishAction.Contains<string>("NOTICE"))
                    {
                        Broadcast.Message(ViolationColor, netUser, Config.GetMessageTruth("Truth.Violation.Banned", netUser, str, userData.Violations, period), null, 0f);
                        Broadcast.MessageAll(ViolationColor, Config.GetMessageTruth("Truth.Punish.Banned", netUser, str, userData.Violations, new DateTime()), netUser);
                        Broadcast.MessageAll(ViolationColor, PunishDetails, null);
                    }
                    else
                    {
                        Broadcast.Message(ViolationColor, netUser, Config.GetMessageTruth("Truth.Violation.Banned", netUser, str, userData.Violations, period), null, 0f);
                        Broadcast.Message(ViolationColor, netUser, PunishDetails, null, 0f);
                    }
                    if (BannedBlockIP && !BannedExcludeIP.Contains<string>(userData.LastConnectIP))
                    {
                        Blocklist.Add(userData.LastConnectIP);
                    }
                    Users.Ban(netUser.userID, "Banned for using " + str + " by SERVER.", period, PunishDetails);
                    netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    return TruthDetector.ActionTaken.Kicked;
                }
                PunishReason = Config.GetMessageTruth("Truth.Logger.Notice", netUser, str, userData.Violations, new DateTime());
                if (PunishDetails != "")
                {
                    Helper.LogError(string.Concat(new object[] { "Violated [", netUser.displayName, ":", netUser.userID, "]: ", PunishDetails }), ViolationDetails);
                }
                Helper.Log(PunishReason, true);
                if (ReportRank > 0)
                {
                    Broadcast.MessageGM(PunishReason);
                }
                if (Core.DatabaseType.Equals("MYSQL"))
                {
                    MySQL.Update(string.Format(string_0, userData.SteamID, MySQL.QuoteString(PunishReason), MySQL.QuoteString(PunishDetails)));
                    MySQL.Update(string.Format(string_1, userData.SteamID, userData.ViolationDate.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                string text = Config.GetMessageTruth("Truth.Violation.Notice", netUser, str, userData.Violations, new DateTime());
                string str3 = Config.GetMessageTruth("Truth.Punish.Notice", netUser, str, userData.Violations, new DateTime());
                if (PunishAction.Contains<string>("KILL"))
                {
                    text = Config.GetMessageTruth("Truth.Violation.Killed", netUser, str, userData.Violations, new DateTime());
                    str3 = Config.GetMessageTruth("Truth.Punish.Killed", netUser, str, userData.Violations, new DateTime());
                }
                if (PunishAction.Contains<string>("KICK"))
                {
                    text = Config.GetMessageTruth("Truth.Violation.Kicked", netUser, str, userData.Violations, new DateTime());
                    str3 = Config.GetMessageTruth("Truth.Punish.Kicked", netUser, str, userData.Violations, new DateTime());
                }
                if (PunishAction.Contains<string>("NOTICE"))
                {
                    Broadcast.Message(ViolationColor, netUser, text, null, 0f);
                    Broadcast.MessageAll(ViolationColor, str3, netUser);
                    Broadcast.MessageAll(ViolationColor, PunishDetails, null);
                }
                if (PunishAction.Contains<string>("KILL"))
                {
                    TakeDamage.KillSelf(netUser.playerClient.controllable.character, null);
                }
                if (PunishAction.Contains<string>("KICK"))
                {
                    netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                }
            }
            return actionTaken_0;
        }

        private static bool smethod_0(TruthDetector truthDetector_0, Vector3 vector3_0, ref Vector3 vector3_1)
        {
            RaycastHit hit;
            bool flag;
            MeshBatchInstance instance;
            Vector3 vector = vector3_1 - vector3_0;
            if (vector.magnitude == 0f)
            {
                return false;
            }
            Ray ray = new Ray(vector3_0 + new Vector3(0f, 0.75f, 0f), vector.normalized);
            if (!Facepunch.MeshBatch.MeshBatchPhysics.SphereCast(ray, 0.1f, out hit, vector.magnitude, 0x20180403, out flag, out instance))
            {
                return false;
            }
            IDMain main = flag ? instance.idMain : IDBase.GetMain(hit.collider);
            GameObject obj2 = (main != null) ? main.gameObject : hit.collider.gameObject;
            string newValue = obj2.name.Trim();
            DeployableObject obj3 = obj2.GetComponent<DeployableObject>();
            StructureComponent component = obj2.GetComponent<StructureComponent>();
            if (newValue == "")
            {
                newValue = "Mesh Texture";
            }
            else if (obj3 != null)
            {
                newValue = Helper.NiceName(obj3.name);
                if (truthDetector_0.netUser.userID == obj3.ownerID)
                {
                    return false;
                }
                if (Users.SharedGet(obj3.ownerID, truthDetector_0.netUser.userID))
                {
                    return false;
                }
            }
            else if (component != null)
            {
                newValue = Helper.NiceName(component.name);
                if (truthDetector_0.netUser.userID == component._master.ownerID)
                {
                    return false;
                }
                if (Users.SharedGet(component._master.ownerID, truthDetector_0.netUser.userID))
                {
                    return false;
                }
            }
            PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.WallHack", truthDetector_0.netUser, "", 0, new DateTime());
            PunishDetails = PunishDetails.Replace("%OBJECT.NAME%", newValue);
            PunishDetails = PunishDetails.Replace("%OBJECT.POS%", hit.point.AsString());
            HackDetected = HackMethod.WallHack;
            vector3_1 = MoveBack(truthDetector_0, vector3_0, vector3_1);
            return true;
        }

        private static bool smethod_1(TruthDetector truthDetector_0, Vector3 vector3_0, Vector3 vector3_1, double double_0)
        {
            Predicate<EventTimer> match = null;
            Predicate<EventTimer> predicate2 = null;
            Predicate<EventTimer> predicate3 = null;
            Class48 class2 = new Class48 {
                truthDetector_0 = truthDetector_0
            };
            if (double_0 > 0.0)
            {
                Vector2 vector = new Vector2(vector3_1.x - vector3_0.x, vector3_1.z - vector3_0.z);
                double magnitude = vector.magnitude;
                double num2 = magnitude / double_0;
                if (num2 == 0.0)
                {
                    return false;
                }
                if (match == null)
                {
                    match = new Predicate<EventTimer>(class2.method_0);
                }
                EventTimer timer = Events.Timer.Find(match);
                if (timer != null)
                {
                    timer.Dispose();
                    Broadcast.Notice(class2.truthDetector_0.netUser.networkPlayer, "☢", Config.GetMessageCommand("Command.Home.Interrupt", "", class2.truthDetector_0.netUser), 5f);
                }
                if (predicate2 == null)
                {
                    predicate2 = new Predicate<EventTimer>(class2.method_1);
                }
                EventTimer timer2 = Events.Timer.Find(predicate2);
                if (timer2 != null)
                {
                    timer2.Dispose();
                    Broadcast.Notice(class2.truthDetector_0.netUser.networkPlayer, "☢", Config.GetMessageCommand("Command.Clan.Warp.Interrupt", "", class2.truthDetector_0.netUser), 5f);
                }
                if (predicate3 == null)
                {
                    predicate3 = new Predicate<EventTimer>(class2.method_2);
                }
                EventTimer timer3 = Events.Timer.Find(predicate3);
                if (timer3 != null)
                {
                    if (timer3.Sender != null)
                    {
                        Broadcast.Notice(timer3.Sender, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", timer3.Sender), 5f);
                    }
                    if (timer3.Target != null)
                    {
                        Broadcast.Notice(timer3.Target, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", timer3.Target), 5f);
                    }
                    timer3.Dispose();
                }
                if (num2 > MaxMovementSpeed)
                {
                    if ((server.log > 1) && Users.HasFlag(class2.truthDetector_0.netUser.userID, UserFlags.admin))
                    {
                        Broadcast.Message(class2.truthDetector_0.netUser, "[COLOR#D02F2F]MovementSpeed: " + num2.ToString("0.0") + " of maximum " + MaxMovementSpeed.ToString("0.0"), "DEBUG", 0f);
                    }
                    PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.SpeedHack", class2.truthDetector_0.netUser, "", 0, new DateTime());
                    PunishDetails = PunishDetails.Replace("%SPEED.MOVEMENT%", num2.ToString("0.00"));
                    PunishDetails = PunishDetails.Replace("%SPEED.MAXIMUM%", MaxMovementSpeed.ToString("0.00"));
                    HackDetected = HackMethod.SpeedHack;
                    vector3_1 = MoveBack(class2.truthDetector_0, vector3_0, vector3_1);
                    return true;
                }
                if (((server.log > 2) && Users.HasFlag(class2.truthDetector_0.netUser.userID, UserFlags.admin)) && (num2 > 1.0))
                {
                    Broadcast.Message(class2.truthDetector_0.netUser, "MovementSpeed: " + num2.ToString("0.0") + " of maximum " + MaxMovementSpeed.ToString("0.0"), "DEBUG", 0f);
                }
            }
            return false;
        }

        private static bool smethod_2(TruthDetector truthDetector_0, Vector3 vector3_0, ref Vector3 vector3_1, double double_0)
        {
            if (double_0 > 0.0)
            {
                double num = ((double) (vector3_1.y - vector3_0.y)) / double_0;
                UserData bySteamID = Users.GetBySteamID(truthDetector_0.netUser.userID);
                Character idMain = truthDetector_0.netUser.playerClient.controllable.idMain;
                if (!FallHeight.ContainsKey(truthDetector_0.netUser))
                {
                    FallHeight.Add(truthDetector_0.netUser, 0.0);
                }
                if (!AirMovement.ContainsKey(truthDetector_0.netUser))
                {
                    AirMovement.Add(truthDetector_0.netUser, 0.0);
                }
                if (idMain.stateFlags.airborne)
                {
                    AirMovement[truthDetector_0.netUser] = 0.0;
                }
                if ((CheckJumphack && (num > 0.0)) && ((idMain != null) && idMain.stateFlags.airborne))
                {
                    truthDetector_0.jumpHeight += num;
                    if ((truthDetector_0.jumpHeight <= MaxJumpingHeight) && (num <= (MaxJumpingHeight * 2f)))
                    {
                        if (((server.log > 2) && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin)) && (truthDetector_0.jumpHeight > 1.0))
                        {
                            Broadcast.Message(truthDetector_0.netUser, "JumpHeight: " + truthDetector_0.jumpHeight.ToString("0.0") + " of maximum " + MaxJumpingHeight.ToString("0.0"), "DEBUG", 0f);
                        }
                    }
                    else
                    {
                        if ((server.log > 1) && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin))
                        {
                            Broadcast.Message(truthDetector_0.netUser, "[COLOR#D02F2F]JumpHeight: " + truthDetector_0.jumpHeight.ToString("0.0") + " of maximum " + MaxJumpingHeight.ToString("0.0"), "DEBUG", 0f);
                        }
                        PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.JumpHack", truthDetector_0.netUser, "", 0, new DateTime());
                        PunishDetails = PunishDetails.Replace("%JUMP.HEIGHT%", truthDetector_0.jumpHeight.ToString("0.00"));
                        PunishDetails = PunishDetails.Replace("%JUMP.MAXHEIGHT%", MaxJumpingHeight.ToString("0.00"));
                        HackDetected = HackMethod.JumpHack;
                        vector3_1 = MoveBack(truthDetector_0, vector3_0, vector3_1);
                    }
                    return (HackDetected == HackMethod.JumpHack);
                }
                if ((CheckFallhack && (num < 0.0)) && ((idMain != null) && idMain.stateFlags.airborne))
                {
                    Dictionary<NetUser, double> dictionary;
                    NetUser user;
                    (dictionary = FallHeight)[user = truthDetector_0.netUser] = dictionary[user] + (num = -num);
                    if (((FallHeight[truthDetector_0.netUser] >= MinFallingHeight) && (bySteamID != null)) && (bySteamID.FallCheck != FallCheckState.damaged))
                    {
                        bySteamID.FallCheck = FallCheckState.check;
                        if (((server.log > 2) && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin)) && (FallHeight[truthDetector_0.netUser] > 1.0))
                        {
                            Broadcast.Message(truthDetector_0.netUser, "[COLOR#D02F2F]FallHeight: " + FallHeight[truthDetector_0.netUser].ToString("0.00") + " of minimum " + MinFallingHeight.ToString("0.0"), "DEBUG", 0f);
                        }
                    }
                    else if (((server.log > 2) && Users.HasFlag(truthDetector_0.netUser.userID, UserFlags.admin)) && (FallHeight[truthDetector_0.netUser] > 1.0))
                    {
                        Broadcast.Message(truthDetector_0.netUser, "FallHeight: " + FallHeight[truthDetector_0.netUser].ToString("0.00") + " of minimum " + MinFallingHeight.ToString("0.0"), "DEBUG", 0f);
                    }
                }
                else if (!idMain.stateFlags.airborne)
                {
                    if (CheckFallhack && (FallHeight[truthDetector_0.netUser] >= MinFallingHeight))
                    {
                        if ((bySteamID != null) && (bySteamID.FallCheck == FallCheckState.check))
                        {
                            PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.FallHack", truthDetector_0.netUser, "", 0, new DateTime());
                            PunishDetails = PunishDetails.Replace("%FALL.HEIGHT%", FallHeight[truthDetector_0.netUser].ToString("0.00"));
                            PunishDetails = PunishDetails.Replace("%FALL.MINHEIGHT%", MinFallingHeight.ToString("0.00"));
                            HackDetected = HackMethod.FallHack;
                        }
                    }
                    else if (Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(vector3_1, 0.5f, 0x20180403).Length == 0)
                    {
                        Dictionary<NetUser, double> dictionary2;
                        NetUser user2;
                        (dictionary2 = AirMovement)[user2 = truthDetector_0.netUser] = dictionary2[user2] + 1.0;
                        if (AirMovement[truthDetector_0.netUser] > NetCull.sendRate)
                        {
                            PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.MoveHack", truthDetector_0.netUser, "", 0, new DateTime());
                            AirMovement[truthDetector_0.netUser] = 0.0;
                            HackDetected = HackMethod.MoveHack;
                        }
                    }
                    bySteamID.FallCheck = FallCheckState.none;
                    FallHeight[truthDetector_0.netUser] = truthDetector_0.jumpHeight = 0.0;
                    if (HackDetected != HackMethod.FallHack)
                    {
                        return (HackDetected == HackMethod.MoveHack);
                    }
                    return true;
                }
            }
            return false;
        }

        private static bool smethod_3(Vector3 vector3_0, Vector3 vector3_1)
        {
            float num = vector3_0.x - vector3_1.x;
            float num2 = vector3_0.y - vector3_1.y;
            float num3 = vector3_0.z - vector3_1.z;
            return ((((num * num) + (num2 * num2)) + (num3 * num3)) >= 0.25f);
        }

        public static bool Test_WeaponShot(Character Killer, GameObject hitObj, IBulletWeaponItem weapon, ItemRepresentation rep, Transform transform, Vector3 endPos, bool isHeadshot)
        {
            if ((Killer == null) || (transform == null))
            {
                return true;
            }
            if (((transform == null) || (Killer == null)) || (Killer.netUser == null))
            {
                return true;
            }
            if ((float.IsNaN(endPos.x) || float.IsNaN(endPos.y)) || float.IsNaN(endPos.z))
            {
                return true;
            }
            Character component = hitObj.GetComponent<Character>();
            NetUser key = ((Killer == null) || (Killer.controllable == null)) ? null : Killer.netUser;
            NetUser user2 = ((component == null) || (component.controllable == null)) ? null : component.netUser;
            Vector3 origin = Helper.GetEyesRay(Killer).origin;
            float num = Vector3.Distance(origin, endPos);
            float bulletRange = ((BulletWeaponDataBlock) rep.datablock).bulletRange;
            if (component == null)
            {
                if (num > bulletRange)
                {
                    return true;
                }
                foreach (Collider collider in Physics.OverlapSphere(Killer.eyesRay.origin, 0.2f))
                {
                    IDBase base2 = collider.gameObject.GetComponent<IDBase>();
                    if ((base2 != null) && (base2.idMain is StructureMaster))
                    {
                        return true;
                    }
                }
                IDMain idMain = IDBase.GetMain(hitObj).idMain;
                if ((idMain.GetComponent<StructureComponent>() == null) && (idMain.GetComponent<SleepingAvatar>() == null))
                {
                    Ray lookRay = Helper.GetLookRay(Killer);
                    Vector3 position = hitObj.transform.position;
                    position.y += 0.1f;
                    if (Physics.RaycastAll(lookRay, Vector3.Distance(lookRay.origin, position), -1).Length > 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            if ((CheckAimbot && !key.admin) && !component.dead)
            {
                string newValue = Helper.NiceName((user2 != null) ? user2.displayName : component.name);
                if (!WeaponShotEyes.ContainsKey(key))
                {
                    PlayerShotEyes eyes = new PlayerShotEyes {
                        origin = Killer.eyesRay.origin,
                        angles = Killer.eyesAngles,
                        count = 0
                    };
                    WeaponShotEyes.Add(key, eyes);
                }
                Vector3 vector5 = transform.position - endPos;
                if (vector5.magnitude > 3f)
                {
                    PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.JackedSilent", key, "", 0, new DateTime());
                    PunishDetails = PunishDetails.Replace("%KILLER.NAME%", key.displayName);
                    PunishDetails = PunishDetails.Replace("%VICTIM.NAME%", newValue);
                    PunishDetails = PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
                    PunishDetails = PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
                    PunishDetails = PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
                    PunishDetails = PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
                    Punish(key, Users.GetBySteamID(key.userID), HackMethod.AimedHack, true);
                    return true;
                }
                if (num < 1f)
                {
                    return false;
                }
                if (ShotThroughObjectBlock)
                {
                    Vector3 vector3;
                    GameObject obj2 = Helper.GetLineObject(origin, endPos, out vector3, 0x183e1411);
                    if ((obj2 != null) && ((obj2.GetComponent<StructureComponent>() != null) || (obj2.GetComponent<BasicDoor>() != null)))
                    {
                        PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.ShootBlocked", key, "", 0, new DateTime());
                        PunishDetails = PunishDetails.Replace("%KILLER.NAME%", key.displayName);
                        PunishDetails = PunishDetails.Replace("%VICTIM.NAME%", newValue);
                        PunishDetails = PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
                        PunishDetails = PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
                        PunishDetails = PunishDetails.Replace("%OBJECT%", Helper.NiceName(obj2.name));
                        PunishDetails = PunishDetails.Replace("%OBJECT.NAME%", Helper.NiceName(obj2.name));
                        PunishDetails = PunishDetails.Replace("%OBJECT.POS%", obj2.transform.position.AsString());
                        PunishDetails = PunishDetails.Replace("%POINT%", vector3.AsString());
                        PunishDetails = PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
                        PunishDetails = PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
                        PunishDetails = PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
                        if (!Killer.stateFlags.movement)
                        {
                            if (ShotThroughObjectPunish)
                            {
                                Punish(key, Users.GetBySteamID(key.userID), HackMethod.AimedHack, false);
                            }
                            else
                            {
                                Helper.LogError(string.Concat(new object[] { "Blocked [", key.displayName, ":", key.userID, "]: ", PunishDetails }), true);
                            }
                            return true;
                        }
                        Vector3 pos = key.truthDetector.prevSnap.pos;
                        pos.x = origin.x;
                        pos.z = origin.z;
                        if (Helper.GetLineObject(pos, endPos, out vector3, 0x183e1411) == obj2)
                        {
                            Helper.LogError(string.Concat(new object[] { "Blocked [", key.displayName, ":", key.userID, "]: ", PunishDetails }), true);
                            return true;
                        }
                    }
                }
                uint num4 = ((uint) Environment.TickCount) - key.truthDetector.prevHitTime;
                if (num4 == Environment.TickCount)
                {
                    num4 = 0;
                }
                key.truthDetector.prevHitTime = (uint) Environment.TickCount;
                if ((num4 > 100) && (num4 < Environment.TickCount))
                {
                    float minShotRateByRange = MinShotRateByRange;
                    float num6 = ((float) num4) / num;
                    Config.Get("SERVER", "Truth.MinShotRateByRange." + weapon.datablock.name, ref minShotRateByRange, true);
                    if (num6 < minShotRateByRange)
                    {
                        PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.HighFireRate", key, "", 0, new DateTime());
                        PunishDetails = PunishDetails.Replace("%KILLER.NAME%", key.displayName);
                        PunishDetails = PunishDetails.Replace("%VICTIM.NAME%", newValue);
                        PunishDetails = PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
                        PunishDetails = PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
                        PunishDetails = PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
                        PunishDetails = PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
                        PunishDetails = PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
                        PunishDetails = PunishDetails.Replace("%SHOTRATE%", num6.ToString("N2"));
                        PunishDetails = PunishDetails.Replace("%MINRATE%", minShotRateByRange.ToString("N2"));
                        Punish(key, Users.GetBySteamID(key.userID), HackMethod.AimedHack, false);
                        return true;
                    }
                }
                if (CheckShotRange && (Math.Abs(num) > bulletRange))
                {
                    PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.OverWeaponRange", key, "", 0, new DateTime());
                    PunishDetails = PunishDetails.Replace("%KILLER.NAME%", key.displayName);
                    PunishDetails = PunishDetails.Replace("%VICTIM.NAME%", newValue);
                    PunishDetails = PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
                    PunishDetails = PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
                    PunishDetails = PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
                    PunishDetails = PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
                    PunishDetails = PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
                    if (user2 != null)
                    {
                        bool punishBan = (ShotAboveMaxDistance > 0f) && ((num - bulletRange) >= ShotAboveMaxDistance);
                        Punish(key, Users.GetBySteamID(key.userID), HackMethod.AimedHack, punishBan);
                    }
                    else
                    {
                        Broadcast.MessageAll(ViolationColor, PunishDetails, null);
                        Helper.LogError(string.Concat(new object[] { "Noticed [", key.displayName, ":", key.userID, "]: ", PunishDetails }), true);
                    }
                    return true;
                }
                float num7 = HeadshotAimTime * num;
                if (num4 > num7)
                {
                    key.truthDetector.headshotHold = 0;
                }
                if (isHeadshot)
                {
                    key.truthDetector.headshotHold += (int) num;
                }
                if (key.truthDetector.headshotHold >= HeadshotThreshold)
                {
                    PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.ThresholdHeadshots", key, "", 0, new DateTime());
                    PunishDetails = PunishDetails.Replace("%KILLER.NAME%", key.displayName);
                    PunishDetails = PunishDetails.Replace("%VICTIM.NAME%", newValue);
                    PunishDetails = PunishDetails.Replace("%KILLER.POS%", Killer.transform.position.AsString());
                    PunishDetails = PunishDetails.Replace("%VICTIM.POS%", component.transform.position.AsString());
                    PunishDetails = PunishDetails.Replace("%DISTANCE%", Math.Abs(num).ToString("N1"));
                    PunishDetails = PunishDetails.Replace("%WEAPON.RANGE%", bulletRange.ToString("N1"));
                    PunishDetails = PunishDetails.Replace("%WEAPON%", weapon.datablock.name);
                    Punish(key, Users.GetBySteamID(key.userID), HackMethod.AimedHack, false);
                    return true;
                }
            }
            return false;
        }

        public static void Test_WeaponShotEyes(Character character, Angle2 angle)
        {
            if (WeaponShotEyes.ContainsKey(character.netUser))
            {
                PlayerShotEyes eyes = WeaponShotEyes[character.netUser];
                if (!object.Equals(angle, eyes.angles))
                {
                    WeaponShotEyes.Remove(character.netUser);
                }
                else
                {
                    eyes.count++;
                    if (eyes.count >= (NetCull.sendRate / 5f))
                    {
                        PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.NoRecoil", null, "", 0, new DateTime());
                        PunishDetails = PunishDetails.Replace("%KILLER.NAME%", character.netUser.displayName);
                        PunishDetails = PunishDetails.Replace("%WEAPON%", character.GetComponent<InventoryHolder>().itemRepresentation.datablock.name);
                        Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), HackMethod.AimedHack, false);
                        WeaponShotEyes.Remove(character.netUser);
                    }
                    else
                    {
                        WeaponShotEyes[character.netUser] = eyes;
                    }
                }
            }
        }

        public static int Rate
        {
            get
            {
                return Convert.ToInt32((float) ((((float) ServerRuntime.TargetFrameRate) / NetCull.sendRate) * 5f));
            }
        }

        [CompilerGenerated]
        private sealed class Class48
        {
            public TruthDetector truthDetector_0;

            public bool method_0(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.truthDetector_0.netUser) && (eventTimer_0.Command == "home"));
            }

            public bool method_1(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.truthDetector_0.netUser) && (eventTimer_0.Command == "clan"));
            }

            public bool method_2(EventTimer eventTimer_0)
            {
                if ((eventTimer_0.Sender != this.truthDetector_0.netUser) && (eventTimer_0.Target != this.truthDetector_0.netUser))
                {
                    return false;
                }
                return (eventTimer_0.Command == "tp");
            }
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

        [StructLayout(LayoutKind.Sequential)]
        public struct PlayerShotEyes
        {
            public Vector3 origin;
            public Angle2 angles;
            public uint count;
        }
    }
}

