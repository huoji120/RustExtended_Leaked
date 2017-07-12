namespace RustExtended
{
    using Facepunch;
    using Facepunch.Clocks.Counters;
    using RustProto;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Timers;
    using UnityEngine;

    public class Events : Facepunch.MonoBehaviour
    {
        public static long AirdropLastTime = -1L;
        public static long AirdropNextDay = -1L;
        public static long AirdropNextHour = -1L;
        public static long AirdropNextTime = -1L;
        private static bool bool_0 = false;
        private static bool bool_1 = false;
        private static bool bool_2 = false;
        private static bool bool_3 = false;
        private static DateTime dateTime_0 = new DateTime();
        private static Events events_0 = new Events();
        public static DateTime EventTime_DoAirdropEvent = DateTime.Now;
        public static DateTime EventTime_DoProcessUsers = DateTime.Now;
        public static DateTime EventTime_DoServerEvents = DateTime.Now;
        public static System.Collections.Generic.List<MOTDEvent> Motd = new System.Collections.Generic.List<MOTDEvent>();
        public static System.Collections.Generic.List<EventTimer> Timer = new System.Collections.Generic.List<EventTimer>();

        private Events()
        {
        }

        public static bool DisablePvP(NetUser netUser, string Command, double time)
        {
            Class39 class2 = new Class39 {
                netUser_0 = netUser,
                string_0 = Command
            };
            if (class2.netUser_0 == null)
            {
                return false;
            }
            EventTimer timer = new EventTimer {
                Interval = time,
                AutoReset = false
            };
            timer.Elapsed += new ElapsedEventHandler(class2.method_0);
            timer.Sender = class2.netUser_0;
            timer.Command = class2.string_0;
            timer.Start();
            return true;
        }

        public static void DoAirdropEvent()
        {
            if (!bool_2)
            {
                bool_2 = true;
                int tickCount = Environment.TickCount;
                if ((Core.Airdrop && (Core.AirdropPlanes > 0)) && (NetCull.connections.Length >= airdrop.min_players))
                {
                    bool flag = false;
                    int num = (int) Math.Abs(EnvironmentControlCenter.Singleton.GetTime());
                    if (Core.AirdropInterval && (((ulong) Environment.TickCount) >= (ulong)AirdropNextTime))
                    {
                        if (AirdropLastTime != -1L)
                        {
                            flag = true;
                        }
                        AirdropLastTime = (long) ((ulong) Environment.TickCount);
                        AirdropNextTime = AirdropLastTime + (Core.AirdropIntervalTime * 0x3e8);
                        if (server.log > 1)
                        {
                            Helper.Log("[Airdrop.Extended] A next call airdrop after " + Core.AirdropIntervalTime + " second(s).", true);
                        }
                    }
                    if (Core.AirdropDropTime)
                    {
                        if ((AirdropNextHour == -1L) && (Core.AirdropDropTimeHours.Length > 0))
                        {
                            AirdropNextDay = EnvironmentControlCenter.Singleton.sky.Cycle.Day + 1;
                            if (Core.AirdropDropTimeHours.Length > 2)
                            {
                                AirdropNextHour = Core.AirdropDropTimeHours.Length.Random(0);
                            }
                            else if (Core.AirdropDropTimeHours.Length > 1)
                            {
                                AirdropNextHour = UnityEngine.Random.Range(Core.AirdropDropTimeHours[0], Core.AirdropDropTimeHours[1]);
                            }
                            else
                            {
                                AirdropNextHour = Core.AirdropDropTimeHours[0];
                            }
                            if (server.log > 1)
                            {
                                Helper.Log("[Airdrop.Extended] A next call airdrop set on " + AirdropNextHour + " h.", true);
                            }
                        }
                        else if ((AirdropNextHour == num) && (EnvironmentControlCenter.Singleton.sky.Cycle.Day >= AirdropNextDay))
                        {
                            AirdropNextHour = -1L;
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        if (Core.AirdropAnnounce)
                        {
                            Broadcast.MessageAll(Config.GetMessage("Airdrop.Incoming", null, null));
                        }
                        for (int i = 0; i < Core.AirdropPlanes; i++)
                        {
                            SupplyDropZone.CallAirDrop();
                        }
                    }
                }
                bool_2 = false;
            }
        }

        public static void DoProcessUsers()
        {
            if (!bool_1)
            {
                bool_1 = true;
                foreach (UserData data in Users.All)
                {
                    Character character;
                    NetUser player = NetUser.FindByUserID(data.SteamID);
                    if ((player != null) && !player.did_join)
                    {
                        player = null;
                    }
                    System.Collections.Generic.List<Countdown> list = new System.Collections.Generic.List<Countdown>();
                    foreach (Countdown countdown in Users.CountdownList(data.SteamID))
                    {
                        if (countdown.Expires)
                        {
                            if (countdown.Expired)
                            {
                                list.Add(countdown);
                            }
                            else if ((countdown.Command.Equals("pvp", StringComparison.OrdinalIgnoreCase) && data.HasFlag(UserFlags.nopvp)) && (Convert.ToInt32(countdown.TimeLeft) < Core.CommandNoPVPCountdown))
                            {
                                data.SetFlag(UserFlags.nopvp, false);
                                if (player != null)
                                {
                                    Broadcast.Notice(player, "☢", Config.GetMessage("Command.PvP.Enabled", player, null), 5f);
                                }
                                Broadcast.NoticeAll("☢", Config.GetMessage("Command.PvP.NoticeEnabled", null, data.Username), player, 5f);
                            }
                        }
                    }
                    foreach (Countdown countdown2 in list)
                    {
                        Users.CountdownRemove(data.SteamID, countdown2);
                    }
                    if ((data.PremiumDate.Millisecond != 0) && (data.PremiumDate < DateTime.Now))
                    {
                        Users.SetFlags(data.SteamID, UserFlags.premium, false);
                        Users.SetRank(data.SteamID, Users.DefaultRank);
                        DateTime date = new DateTime();
                        Users.SetPremiumDate(data.SteamID, date);
                        Broadcast.Notice(player, "☢", Config.GetMessage("Player.Premium.Expired", null, null), 5f);
                    }
                    if (((Core.OwnershipDestroyAutoDisable > 0) && Core.DestoryOwnership.ContainsKey(data.SteamID)) && (Core.DestoryOwnership[data.SteamID] < DateTime.Now))
                    {
                        Core.DestoryOwnership.Remove(data.SteamID);
                        if (player != null)
                        {
                            Broadcast.Notice(player, "☢", Config.GetMessage("Command.Destroy.Disabled", null, null), 5f);
                        }
                    }
                    if (((player != null) && player.did_join) && (player.admin && Character.FindByUser(player.userID, out character)))
                    {
                        Metabolism component = character.GetComponent<Metabolism>();
                        if (component.GetCalorieLevel() < 3000f)
                        {
                            component.AddCalories(3000f - component.GetCalorieLevel());
                        }
                        if (component.GetRadLevel() > 0f)
                        {
                            component.AddAntiRad(component.GetRadLevel());
                        }
                    }
                }
                bool_1 = false;
                if ((Core.DatabaseType.Equals("MYSQL") && !bool_3) && (DateTime.Now.Subtract(dateTime_0).TotalMilliseconds > Core.MySQL_SyncInterval))
                {
                    if (Core.MySQL_LogLevel > 2)
                    {
                        Helper.LogSQL("Thread \"ProcessUsers\": Synchronizing server data from MySQL database", false);
                    }
                    SystemTimestamp restart = SystemTimestamp.Restart;
                    bool_3 = true;
                    Core.SQL_UpdateServer();
                    if (Core.MySQL_Synchronize)
                    {
                        Users.SQL_SynchronizeUsers();
                    }
                    if (Core.MySQL_Synchronize)
                    {
                        Clans.SQL_SynchronizeClans();
                    }
                    dateTime_0 = DateTime.Now;
                    bool_3 = false;
                    restart.Stop();
                    if (Core.MySQL_LogLevel > 2)
                    {
                        Helper.LogSQL("Thread \"ProcessUsers\": Synchronized, is took " + restart.ElapsedSeconds.ToString("0.0000") + " second(s).", false);
                    }
                }
            }
        }

        public static void DoServerEvents()
        {
            if (!bool_0)
            {
                bool_0 = true;
                try
                {
                    Core.GetSpawnersSpawns();
                    if (((Core.GenerateSource.Length > 0) && (Core.GenerateOutput.Length > 0)) && (Core.GenerateSource.Length == Core.GenerateOutput.Length))
                    {
                        for (int i = 0; i < Core.GenerateSource.Length; i++)
                        {
                            Helper.GenerateFile(Core.GenerateSource[i], Core.GenerateOutput[i]);
                        }
                    }
                    if (Truth.RustProtectChangeKey && ((Time.time - Truth.ProtectionUpdateTime) > Truth.RustProtectChangeKeyInterval))
                    {
                        Truth.ProtectionUpdateTime = Time.time + 1f;
                        int newSerial = (int) Helper.NewSerial;
                        Truth.ProtectionKey ^= newSerial;
                        Truth.ProtectionHash ^= newSerial;
                        if (server.log > 2)
                        {
                            ConsoleSystem.Print("Protection Key Changed To=" + string.Format("0x{0:X8}", Truth.ProtectionKey) + ", New Hash=" + string.Format("0x{0:X8}", Truth.ProtectionHash), false);
                        }
                        foreach (PlayerClient client in PlayerClient.All)
                        {
                            Users.GetBySteamID(client.userID).ProtectTick = 0;
                            Users.GetBySteamID(client.userID).ProtectTime = 0f;
                        }
                    }
                    if (Core.CyclePvP)
                    {
                        if (server.pvp && (((int) EnvironmentControlCenter.Singleton.GetTime()) == Core.CyclePvPOff))
                        {
                            Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Disabled", null, null), null, 5f);
                            server.pvp = false;
                        }
                        else if (!server.pvp && (((int) EnvironmentControlCenter.Singleton.GetTime()) == Core.CyclePvPOn))
                        {
                            Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.PvP.Enabled", null, null), null, 5f);
                            server.pvp = true;
                        }
                    }
                    if (Core.CycleInstantCraft)
                    {
                        if (crafting.instant && (((int) EnvironmentControlCenter.Singleton.GetTime()) == Core.CycleInstantCraftOff))
                        {
                            Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.InstantCraft.Disabled", null, null), null, 5f);
                            crafting.instant = false;
                        }
                        else if (!crafting.instant && (((int) EnvironmentControlCenter.Singleton.GetTime()) == Core.CycleInstantCraftOn))
                        {
                            Broadcast.NoticeAll("☢", Config.GetMessage("Cycle.InstantCraft.Enabled", null, null), null, 5f);
                            crafting.instant = true;
                        }
                    }
                    if (Clans.Database != null)
                    {
                        foreach (uint num3 in ((ICollection<uint>) Clans.Database.Keys).ToArray<uint>())
                        {
                            if (Clans.Database.ContainsKey(num3))
                            {
                                ClanData clan = Clans.Database[num3];
                                uint[] numArray2 = ((ICollection<uint>) clan.Hostile.Keys).ToArray<uint>();
                                if (numArray2.Length > 0)
                                {
                                    foreach (uint num4 in numArray2)
                                    {
                                        if (Clans.Database.ContainsKey(num4) && (DateTime.Now > clan.Hostile[num4]))
                                        {
                                            ClanData data2 = Clans.Database[num4];
                                            foreach (string str in Config.GetMessagesClan("Command.Clan.Hostile.Ended", clan, null, null))
                                            {
                                                clan.Message(str.Replace("%HOSTILE.CLAN.NAME%", data2.Name));
                                            }
                                            foreach (string str2 in Config.GetMessagesClan("Command.Clan.Hostile.Ended", data2, null, null))
                                            {
                                                data2.Message(str2.Replace("%HOSTILE.CLAN.NAME%", clan.Name));
                                            }
                                            clan.Hostile.Remove(data2.ID);
                                            clan.Penalty = Helper.StringToTime(Clans.ClanWarEndedPenalty, DateTime.Now);
                                            data2.Hostile.Remove(clan.ID);
                                            data2.Penalty = Helper.StringToTime(Clans.ClanWarEndedPenalty, DateTime.Now);
                                            if (Core.DatabaseType.Equals("MYSQL"))
                                            {
                                                MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN_HOSTILE, clan.ID));
                                                MySQL.Update(string.Format(Clans.SQL_DELETE_CLAN_HOSTILE, data2.ID));
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
                    Helper.LogWarning("WARNING: Server DoEvents restarted by exception.", true);
                }
                bool_0 = false;
            }
        }

        public static void EventDisablePvP(NetUser netUser, string Command)
        {
            Class40 class2 = new Class40 {
                netUser_0 = netUser,
                string_0 = Command
            };
            EventTimer timer = Timer.Find(new Predicate<EventTimer>(class2.method_0));
            if (timer != null)
            {
                timer.Dispose();
                if (class2.netUser_0 != null)
                {
                    Users.SetFlags(class2.netUser_0.userID, UserFlags.nopvp, true);
                    int num = Core.CommandNoPVPDuration + Core.CommandNoPVPCountdown;
                    if (num > 0)
                    {
                        Users.CountdownAdd(class2.netUser_0.userID, new Countdown(class2.string_0, (double) num));
                    }
                    TimeSpan span = TimeSpan.FromSeconds((double) Core.CommandNoPVPDuration);
                    Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.PvP.Disabled", class2.netUser_0, null).Replace("%TIME%", string.Format("{0}:{1:D2}", span.Minutes, span.Seconds)), 5f);
                    Broadcast.NoticeAll("☢", Config.GetMessage("Command.PvP.NoticeDisabled", class2.netUser_0, null), class2.netUser_0, 5f);
                }
            }
        }

        public static void EventServerRestart(EventTimer sender, int ShutdownTime, ref int Timeleft)
        {
            if (Timeleft != 0)
            {
                if (Timeleft <= 5)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 1f);
                }
                else if (Timeleft == 10)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 5f);
                }
                else if (Timeleft == 30)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 10f);
                }
                else if (Timeleft == 60)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillRestart", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 10f);
                }
                else if (Timeleft == ShutdownTime)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.Restart", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 10f);
                }
            }
            if (Timeleft > 0)
            {
                Timeleft--;
            }
            else
            {
                try
                {
                    if (sender != null)
                    {
                        sender.Stop();
                    }
                    AvatarSaveProc.SaveAll();
                    ServerSaveManager.AutoSave();
                    string oldValue = Environment.GetCommandLineArgs()[0];
                    string arguments = string.Join(" ", Environment.GetCommandLineArgs()).Replace(oldValue, "").Trim();
                    Process.Start(oldValue, arguments);
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception exception)
                {
                    Helper.LogError(exception.ToString(), true);
                }
            }
        }

        public static void EventServerShutdown(EventTimer sender, int ShutdownTime, ref int Timeleft)
        {
            if (Timeleft != 0)
            {
                if (Timeleft <= 5)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 1f);
                }
                else if (Timeleft == 10)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 5f);
                }
                else if (Timeleft == 30)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 10f);
                }
                else if (Timeleft == 60)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.WillShutdown", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 10f);
                }
                else if (Timeleft == ShutdownTime)
                {
                    Broadcast.NoticeAll("☢", Config.GetMessage("Server.Shutdown", null, null).Replace("%SECONDS%", ((int) Timeleft).ToString()), null, 10f);
                }
            }
            if (Timeleft > 0)
            {
                Timeleft--;
            }
            else
            {
                try
                {
                    if (sender != null)
                    {
                        sender.Stop();
                    }
                    AvatarSaveProc.SaveAll();
                    ServerSaveManager.AutoSave();
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception exception)
                {
                    Helper.LogError(exception.ToString(), true);
                }
            }
        }

        public static void EventSleeperAway(object obj, ulong userID)
        {
            if ((obj != null) && (obj is EventTimer))
            {
                (obj as EventTimer).Dispose();
            }
            string username = Users.GetUsername(userID);
            RustProto.Avatar avatar = NetUser.LoadAvatar(userID);
            if (((avatar != null) && avatar.HasAwayEvent) && (avatar.AwayEvent.Type == AwayEvent.Types.AwayEventType.SLUMBER))
            {
                SleepingAvatar.TransientData data = (SleepingAvatar.TransientData) typeof(SleepingAvatar).GetMethod("Close", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { userID });
                if (data.exists)
                {
                    Helper.Log(string.Concat(new object[] { "User Sleeping [", username, ":", userID, "] is disappeared." }), true);
                    data.AdjustIncomingAvatar(ref avatar);
                    NetUser.SaveAvatar(userID, ref avatar);
                }
            }
        }

        public static void Initialize()
        {
        }

        public static void SleeperAway(ulong userID, int lifetime)
        {
            Class35 class2 = new Class35 {
                ulong_0 = userID
            };
            EventTimer timer = new EventTimer {
                Interval = lifetime,
                AutoReset = false
            };
            timer.Elapsed += new ElapsedEventHandler(class2.method_0);
            timer.Start();
        }

        public static void Teleport_ClanWarp(object obj, NetUser netUser, string command, ClanData clan)
        {
            if ((obj != null) && (obj is EventTimer))
            {
                (obj as EventTimer).Dispose();
            }
            Helper.TeleportTo(netUser, clan.Location);
            if (clan.Level.WarpCountdown > 0)
            {
                Users.CountdownAdd(netUser.userID, new Countdown(command, (double) clan.Level.WarpCountdown));
            }
            Broadcast.Notice(netUser, "☢", Config.GetMessage("Command.Clan.Warp.Warped", netUser, null), 5f);
        }

        public static void Teleport_HomeWarp(object obj, NetUser Sender, string command, Vector3 pos)
        {
            if ((obj != null) && (obj is EventTimer))
            {
                (obj as EventTimer).Dispose();
            }
            if (Economy.Enabled && (Core.CommandHomePayment > 0L))
            {
                UserEconomy economy = Economy.Get(Sender.userID);
                string newValue = Core.CommandHomePayment.ToString("N0") + Economy.CurrencySign;
                if (economy.Balance < Core.CommandHomePayment)
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Home.NoEnoughCurrency", Sender, null).Replace("%PRICE%", newValue), 5f);
                    return;
                }
                economy.Balance -= Core.CommandHomePayment;
                string str2 = economy.Balance.ToString("N0") + Economy.CurrencySign;
                Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", str2), null, 0f);
            }
            if (Core.CommandHomeCountdown > 0)
            {
                Users.CountdownAdd(Sender.userID, new Countdown(command, (double) Core.CommandHomeCountdown));
            }
            Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Home.Return", Sender, null), 5f);
            Helper.TeleportTo(Sender, pos);
        }

        public static void Teleport_PlayerTo(object obj, NetUser Sender, NetUser Target, string command, Vector3 pos)
        {
            if ((obj != null) && (obj is EventTimer))
            {
                (obj as EventTimer).Dispose();
            }
            if (Economy.Enabled && (Core.CommandTeleportPayment > 0L))
            {
                UserEconomy economy = Economy.Get(Sender.userID);
                string newValue = Core.CommandTeleportPayment.ToString("N0") + Economy.CurrencySign;
                if (economy.Balance < Core.CommandTeleportPayment)
                {
                    Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Teleport.NoEnoughCurrency", Sender, null).Replace("%PRICE%", newValue), 5f);
                    return;
                }
                economy.Balance -= Core.CommandTeleportPayment;
                string str2 = economy.Balance.ToString("N0") + Economy.CurrencySign;
                Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", str2), null, 0f);
            }
            Broadcast.Notice(Sender, "☢", Config.GetMessage("Command.Teleport.TeleportOnPlayer", Sender, null).Replace("%USERNAME%", Target.displayName), 5f);
            Broadcast.Notice(Target, "☢", Config.GetMessage("Command.Teleport.TeleportedPlayer", Target, null).Replace("%USERNAME%", Sender.displayName), 5f);
            if (Core.CommandTeleportCountdown > 0)
            {
                Users.CountdownAdd(Sender.userID, new Countdown(command, (double) Core.CommandTeleportCountdown));
            }
            Helper.TeleportTo(Sender, pos);
        }

        public static EventTimer TimeEvent_ClanWarp(NetUser netUser, string Command, double time, ClanData clan)
        {
            Class37 class2 = new Class37 {
                netUser_0 = netUser,
                string_0 = Command,
                clanData_0 = clan
            };
            if (time <= 0.0)
            {
                Teleport_ClanWarp(null, class2.netUser_0, class2.string_0, class2.clanData_0);
                return null;
            }
            EventTimer timer = new EventTimer {
                Interval = time * 1000.0,
                AutoReset = false
            };
            timer.Elapsed += new ElapsedEventHandler(class2.method_0);
            timer.Sender = class2.netUser_0;
            timer.Command = class2.string_0;
            timer.Start();
            return timer;
        }

        public static EventTimer TimeEvent_HomeWarp(NetUser Sender, string Command, double time, Vector3 pos)
        {
            Class36 class2 = new Class36 {
                netUser_0 = Sender,
                string_0 = Command,
                vector3_0 = pos
            };
            if (time <= 0.0)
            {
                Teleport_HomeWarp(null, class2.netUser_0, class2.string_0, class2.vector3_0);
                return null;
            }
            EventTimer timer = new EventTimer {
                Interval = time * 1000.0,
                AutoReset = false
            };
            timer.Elapsed += new ElapsedEventHandler(class2.method_0);
            timer.Sender = class2.netUser_0;
            timer.Command = class2.string_0;
            timer.Start();
            return timer;
        }

        public static EventTimer TimeEvent_TeleportTo(NetUser Sender, NetUser Target, string Command, double time)
        {
            Class38 class2 = new Class38 {
                netUser_0 = Sender,
                netUser_1 = Target,
                string_0 = Command
            };
            if (Core.CommandTeleportOutdoorsOnly)
            {
                foreach (Collider collider in Physics.OverlapSphere(class2.netUser_1.playerClient.controllable.character.transform.position, 1f, 0x10360401))
                {
                    IDMain main = IDBase.GetMain(collider);
                    if (main != null)
                    {
                        StructureMaster component = main.GetComponent<StructureMaster>();
                        if (((component != null) && (component.ownerID != class2.netUser_0.userID)) && (component.ownerID != class2.netUser_1.userID))
                        {
                            UserData bySteamID = Users.GetBySteamID(component.ownerID);
                            if ((bySteamID == null) || (!bySteamID.HasShared(class2.netUser_0.userID) && !bySteamID.HasShared(class2.netUser_1.userID)))
                            {
                                Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Teleport.NoTeleport", class2.netUser_0, class2.netUser_1.displayName), 5f);
                                Broadcast.Notice(class2.netUser_1, "☢", Config.GetMessage("Command.Teleport.NotHere", class2.netUser_1, class2.netUser_0.displayName), 5f);
                                return null;
                            }
                        }
                    }
                }
            }
            Broadcast.Message(class2.netUser_0, Config.GetMessage("Command.Teleport.IsConfirm", class2.netUser_0, null).Replace("%USERNAME%", class2.netUser_1.displayName), null, 0f);
            Broadcast.Message(class2.netUser_1, Config.GetMessage("Command.Teleport.Confirmed", class2.netUser_1, null).Replace("%USERNAME%", class2.netUser_0.displayName), null, 0f);
            if (!Character.FindByUser(class2.netUser_1.userID, out class2.character_0))
            {
                return null;
            }
            if (time <= 0.0)
            {
                Teleport_PlayerTo(null, class2.netUser_0, class2.netUser_1, class2.string_0, class2.character_0.transform.position);
                return null;
            }
            EventTimer timer = new EventTimer {
                Interval = time * 1000.0,
                AutoReset = false
            };
            timer.Elapsed += new ElapsedEventHandler(class2.method_0);
            timer.Sender = class2.netUser_0;
            timer.Target = class2.netUser_1;
            timer.Command = class2.string_0;
            Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessage("Command.Teleport.Timewait", class2.netUser_0, null).Replace("%TIME%", timer.TimeLeft.ToString()), 5f);
            Broadcast.Notice(class2.netUser_1, "☢", Config.GetMessage("Command.Teleport.Timewait", class2.netUser_1, null).Replace("%TIME%", timer.TimeLeft.ToString()), 5f);
            timer.Start();
            return timer;
        }

        public static Events Singleton
        {
            get
            {
                return events_0;
            }
        }

        [CompilerGenerated]
        private sealed class Class35
        {
            public ulong ulong_0;

            public void method_0(object sender, ElapsedEventArgs e)
            {
                Events.EventSleeperAway(sender, this.ulong_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class36
        {
            public NetUser netUser_0;
            public string string_0;
            public Vector3 vector3_0;

            public void method_0(object sender, ElapsedEventArgs e)
            {
                Events.Teleport_HomeWarp(sender, this.netUser_0, this.string_0, this.vector3_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class37
        {
            public ClanData clanData_0;
            public NetUser netUser_0;
            public string string_0;

            public void method_0(object sender, ElapsedEventArgs e)
            {
                Events.Teleport_ClanWarp(sender, this.netUser_0, this.string_0, this.clanData_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class38
        {
            public Character character_0;
            public NetUser netUser_0;
            public NetUser netUser_1;
            public string string_0;

            public void method_0(object sender, ElapsedEventArgs e)
            {
                Events.Teleport_PlayerTo(sender, this.netUser_0, this.netUser_1, this.string_0, this.character_0.transform.position);
            }
        }

        [CompilerGenerated]
        private sealed class Class39
        {
            public NetUser netUser_0;
            public string string_0;

            public void method_0(object sender, ElapsedEventArgs e)
            {
                Events.EventDisablePvP(this.netUser_0, this.string_0);
            }
        }

        [CompilerGenerated]
        private sealed class Class40
        {
            public NetUser netUser_0;
            public string string_0;

            public bool method_0(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.string_0));
            }
        }
    }
}

