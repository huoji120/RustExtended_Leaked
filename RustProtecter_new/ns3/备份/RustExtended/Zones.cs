namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Timers;
    using uLink;
    using UnityEngine;

    public class Zones
    {
        [CompilerGenerated]
        private static bool bool_0;
        public static Dictionary<string, WorldZone> Database;
        public static Vector3 JailPosition = Vector3.zero;
        public static System.Collections.Generic.List<GameObject> Markers = new System.Collections.Generic.List<GameObject>();
        private static string string_0 = "rust_zones.txt";
        [CompilerGenerated]
        private static string string_1;
        [CompilerGenerated]
        private static WorldZone worldZone_0;

        public static bool AtZone(WorldZone zone, NetUser netUser)
        {
            return AtZone(zone, netUser.playerClient.lastKnownPosition);
        }

        public static bool AtZone(WorldZone zone, PlayerClient player)
        {
            return AtZone(zone, player.lastKnownPosition);
        }

        public static bool AtZone(WorldZone zone, uLink.NetworkPlayer netPlayer)
        {
            PlayerClient client;
            if (!PlayerClient.Find(netPlayer, out client))
            {
                return false;
            }
            return AtZone(zone, client.lastKnownPosition);
        }

        public static bool AtZone(WorldZone zone, GameObject gameObject)
        {
            return AtZone(zone, gameObject.transform.position);
        }

        public static bool AtZone(WorldZone zone, Transform transform)
        {
            return AtZone(zone, transform.position);
        }

        public static bool AtZone(WorldZone zone, Vector3 position)
        {
            return AtZone(zone, position.x, position.y, position.z);
        }

        public static bool AtZone(System.Collections.Generic.List<Vector2> V, Vector2 P)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            while (num2 < V.Count)
            {
                num3 = num2 + 1;
                if (num3 == V.Count)
                {
                    num3 = 0;
                }
                if (((V[num2].y <= P.y) && (V[num3].y > P.y)) || ((V[num2].y > P.y) && (V[num3].y <= P.y)))
                {
                    double num4 = (P.y - V[num2].y) / (V[num3].y - V[num2].y);
                    if (P.x < (V[num2].x + (num4 * (V[num3].x - V[num2].x))))
                    {
                        num++;
                    }
                }
                num2++;
            }
            return ((num % 2) != 0);
        }

        public static bool AtZone(WorldZone zone, float x, float y, float z)
        {
            return AtZone(zone.Points, new Vector2(x, z));
        }

        public static bool BuildMark(Vector3 position)
        {
            if (LastZone == null)
            {
                return false;
            }
            LastZone.Points.Add(new Vector2(position.x, position.z));
            WorldZone zone = Get(position);
            if ((zone != null) && !zone.Internal.Contains(LastZone))
            {
                zone.Internal.Add(LastZone);
            }
            Vector3 ground = GetGround(position.x, position.z);
            Markers.Add(World.Spawn(";struct_metal_pillar", ground));
            Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 4f, 0f)));
            Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 8f, 0f)));
            Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 12f, 0f)));
            return true;
        }

        public static bool BuildNew(string zone_name)
        {
            if (LastZone != null)
            {
                return false;
            }
            string key = "z_" + zone_name.Trim().Replace(" ", "_").ToLower();
            if (Database.ContainsKey(key))
            {
                return false;
            }
            LastZone = new WorldZone(zone_name, 0);
            return true;
        }

        public static bool BuildSave()
        {
            if ((LastZone != null) && (LastZone.Points.Count >= 3))
            {
                string key = "z_" + LastZone.Name.Trim().Replace(" ", "_").ToLower();
                LastZone.Center = GetCentroid(LastZone.Points);
                Database.Add(key, LastZone);
                LastZone = null;
                return true;
            }
            return false;
        }

        public static bool Delete(WorldZone zone)
        {
            if ((zone != null) && Database.ContainsValue(zone))
            {
                Database.Remove(zone.Defname);
                return true;
            }
            return false;
        }

        public static WorldZone Find(string search_name)
        {
            string str = search_name.Trim(new char[] { '*' });
            foreach (string str2 in Database.Keys)
            {
                if (search_name.StartsWith("*") && str2.EndsWith(str))
                {
                    return Database[str2];
                }
                if (search_name.EndsWith("*") && str2.StartsWith(str))
                {
                    return Database[str2];
                }
                if (str2.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    return Database[str2];
                }
                if (search_name.StartsWith("*") && Database[str2].Name.EndsWith(str))
                {
                    return Database[str2];
                }
                if (search_name.EndsWith("*") && Database[str2].Name.StartsWith(str))
                {
                    return Database[str2];
                }
                if (Database[str2].Name.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    return Database[str2];
                }
            }
            return null;
        }

        public static WorldZone Get(NetUser netUser)
        {
            return Get(netUser.playerClient.lastKnownPosition);
        }

        public static WorldZone Get(PlayerClient player)
        {
            return Get(player.lastKnownPosition);
        }

        public static WorldZone Get(string defname)
        {
            if (Database.ContainsKey(defname))
            {
                return Database[defname];
            }
            return null;
        }

        public static WorldZone Get(uLink.NetworkPlayer netPlayer)
        {
            PlayerClient client;
            if (!PlayerClient.Find(netPlayer, out client))
            {
                return null;
            }
            return Get(client.lastKnownPosition);
        }

        public static WorldZone Get(Transform transform)
        {
            return Get(transform.position.x, transform.position.y, transform.position.z);
        }

        public static WorldZone Get(Vector3 position)
        {
            return Get(position.x, position.y, position.z);
        }

        public static WorldZone Get(float x, float y, float z)
        {
            using (Dictionary<string, WorldZone>.KeyCollection.Enumerator enumerator = Database.Keys.GetEnumerator())
            {
                string current;
                WorldZone zone;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (AtZone(Database[current], x, y, z))
                    {
                        goto Label_003B;
                    }
                }
                goto Label_0063;
            Label_003B:
                zone = Database[current];
                GetInternal(ref zone, x, y, z);
                return zone;
            }
        Label_0063:
            return null;
        }

        public static Vector2 GetCentroid(WorldZone zone)
        {
            return GetCentroid(zone.Points);
        }

        public static Vector2 GetCentroid(System.Collections.Generic.List<Vector2> points)
        {
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            for (int i = points.Count - 1; num4 < points.Count; i = num4++)
            {
                float num6 = (points[num4].x * points[i].y) - (points[i].x * points[num4].y);
                num2 += (points[num4].x + points[i].x) * num6;
                num3 += (points[num4].y + points[i].y) * num6;
                num += num6;
            }
            if (num == 0f)
            {
                return Vector2.zero;
            }
            num *= 3f;
            return new Vector2(num2 / num, num3 / num);
        }

        public static string GetDefname(WorldZone zone)
        {
            foreach (string str in Database.Keys)
            {
                if (Database[str] == zone)
                {
                    return str;
                }
            }
            return null;
        }

        private static Vector3 GetGround(float float_0, float float_1)
        {
            Vector3 origin = new Vector3(float_0, 2000f, float_1);
            Vector3 direction = new Vector3(0f, -1f, 0f);
            return Physics.RaycastAll(origin, direction)[0].point;
        }

        public static void GetInternal(ref WorldZone zone, float x, float y, float z)
        {
            foreach (WorldZone zone2 in zone.Internal)
            {
                if (AtZone(zone2, x, y, z))
                {
                    zone = zone2;
                    GetInternal(ref zone, x, y, z);
                }
            }
        }

        public static void HidePoints()
        {
            if ((Markers != null) && (Markers.Count != 0))
            {
                foreach (GameObject obj2 in Markers)
                {
                    NetCull.Destroy(obj2);
                }
                Markers.Clear();
            }
        }

        public static void Initialize()
        {
            Initialized = false;
            SaveFilePath = Path.Combine(Core.SavePath, string_0);
            Database = new Dictionary<string, WorldZone>();
            if (File.Exists(SaveFilePath))
            {
                LoadAsFile();
            }
            Initialized = true;
        }

        public static bool LoadAsFile()
        {
            if (Database == null)
            {
                return false;
            }
            Database.Clear();
            string[] strArray = File.ReadAllLines(SaveFilePath);
            WorldZone zone = null;
            Dictionary<WorldZone, string> dictionary = new Dictionary<WorldZone, string>();
            foreach (string str in strArray)
            {
                string[] strArray2;
                float num;
                float num2;
                float num3;
                if (str.StartsWith("[") && str.EndsWith("]"))
                {
                    zone = null;
                    if (!str.StartsWith("[ZONE ", StringComparison.OrdinalIgnoreCase))
                    {
                        ConsoleSystem.LogError("Invalid section \"" + str + "\" from zones.");
                    }
                    else
                    {
                        strArray2 = Helper.SplitQuotes(str.Trim(new char[] { '[', ']' }), ' ');
                        if (Database.ContainsKey(strArray2[1]))
                        {
                            zone = Database[strArray2[1]];
                        }
                        else
                        {
                            zone = new WorldZone(null, 0);
                            Database.Add(strArray2[1], zone);
                        }
                    }
                }
                else if (zone != null)
                {
                    strArray2 = str.Split(new char[] { '=' });
                    if (strArray2.Length >= 2)
                    {
                        strArray2[1] = strArray2[1].Trim();
                        if (!string.IsNullOrEmpty(strArray2[1]))
                        {
                            num = 0f;
                            num2 = 0f;
                            num3 = 0f;
                            switch (strArray2[0].ToUpper())
                            {
                                case "NAME":
                                    zone.Name = strArray2[1];
                                    break;

                                case "FLAGS":
                                    zone.Flags = strArray2[1].ToEnum<ZoneFlags>();
                                    break;

                                case "INTERNAL":
                                    if (!Database.ContainsKey(strArray2[1]))
                                    {
                                        WorldZone zone2 = new WorldZone(null, 0);
                                        Database.Add(strArray2[1], zone2);
                                        zone.Internal.Add(zone2);
                                    }
                                    break;

                                case "CENTER":
                                    goto Label_02A7;

                                case "WARP":
                                    strArray2 = strArray2[1].Split(new char[] { ',' });
                                    dictionary.Add(zone, strArray2[0].Trim());
                                    if (strArray2.Length > 1)
                                    {
                                        long.TryParse(strArray2[1], out zone.WarpTime);
                                    }
                                    break;

                                case "POINT":
                                    goto Label_033B;

                                case "SPAWN":
                                    goto Label_038F;

                                case "FORBIDDEN.COMMAND":
                                    zone.ForbiddenCommand = zone.ForbiddenCommand.Add<string>(strArray2[1]);
                                    break;

                                case "ENTER.NOTICE":
                                    zone.Notice_OnEnter = strArray2[1];
                                    break;

                                case "LEAVE.NOTICE":
                                    zone.Notice_OnLeave = strArray2[1];
                                    break;

                                case "ENTER.MESSAGE":
                                    zone.Message_OnEnter = zone.Message_OnEnter.Add<string>(strArray2[1]);
                                    break;

                                case "LEAVE.MESSAGE":
                                    zone.Message_OnLeave = zone.Message_OnLeave.Add<string>(strArray2[1]);
                                    break;
                            }
                        }
                    }
                }
                continue;
            Label_02A7:;
                strArray2 = strArray2[1].Split(new char[] { ',' });
                if (strArray2.Length > 0)
                {
                    float.TryParse(strArray2[0], out num);
                }
                if (strArray2.Length > 1)
                {
                    float.TryParse(strArray2[1], out num2);
                }
                zone.Center = new Vector2(num, num2);
                continue;
            Label_033B:;
                strArray2 = strArray2[1].Split(new char[] { ',' });
                if (strArray2.Length > 0)
                {
                    float.TryParse(strArray2[0], out num);
                }
                if (strArray2.Length > 1)
                {
                    float.TryParse(strArray2[1], out num2);
                }
                zone.Points.Add(new Vector2(num, num2));
                continue;
            Label_038F:;
                strArray2 = strArray2[1].Split(new char[] { ',' });
                if (strArray2.Length > 0)
                {
                    float.TryParse(strArray2[0], out num);
                }
                if (strArray2.Length > 1)
                {
                    float.TryParse(strArray2[1], out num2);
                }
                if (strArray2.Length > 2)
                {
                    float.TryParse(strArray2[2], out num3);
                }
                zone.Spawns.Add(new Vector3(num, num2, num3));
            }
            foreach (WorldZone zone3 in dictionary.Keys)
            {
                zone3.WarpZone = Get(dictionary[zone3]);
            }
            return true;
        }

        public static void OnPlayerMove(NetUser netUser, ref Vector3 newpos, ref TruthDetector.ActionTaken taken)
        {
            Predicate<EventTimer> match = null;
            ElapsedEventHandler handler = null;
            Class56 class2 = new Class56 {
                netUser_0 = netUser
            };
            if (((class2.netUser_0 != null) && (class2.netUser_0.playerClient != null)) && (class2.netUser_0.playerClient.controllable != null))
            {
                Vector3 position = class2.netUser_0.playerClient.controllable.character.transform.position;
                if ((position != newpos) && ((position.x != newpos.x) || (position.z != newpos.z)))
                {
                    class2.userData_0 = Users.GetBySteamID(class2.netUser_0.userID);
                    if (class2.userData_0 != null)
                    {
                        if (!class2.userData_0.HasFlag(UserFlags.onevent))
                        {
                            class2.userData_0.Position = newpos;
                        }
                        class2.worldZone_0 = Get(newpos);
                        if (class2.userData_0.Zone != class2.worldZone_0)
                        {
                            EventTimer timer = null;
                            if (class2.userData_0.Zone != null)
                            {
                                if (match == null)
                                {
                                    match = new Predicate<EventTimer>(class2.method_0);
                                }
                                timer = Events.Timer.Find(match);
                            }
                            if (timer != null)
                            {
                                Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessageTeleport("Player.WarpZone.Interrupt", class2.netUser_0, class2.userData_0.Zone, null), 2f);
                                timer.Dispose();
                            }
                            if (class2.userData_0.Zone != null)
                            {
                                if ((class2.userData_0.Zone.NoLeave && !class2.netUser_0.admin) && ((class2.worldZone_0 == null) || !class2.userData_0.Zone.Internal.Contains(class2.worldZone_0)))
                                {
                                    newpos = position;
                                    taken = TruthDetector.ActionTaken.Moved;
                                    return;
                                }
                                if (!string.IsNullOrEmpty(class2.userData_0.Zone.Notice_OnLeave))
                                {
                                    Broadcast.Notice(class2.netUser_0, "☢", class2.userData_0.Zone.Notice_OnLeave, 5f);
                                }
                                foreach (string str in class2.userData_0.Zone.Message_OnLeave)
                                {
                                    Broadcast.Message(class2.netUser_0, str, null, 0f);
                                }
                            }
                            if (class2.worldZone_0 != null)
                            {
                                if ((class2.worldZone_0.NoEnter && !class2.netUser_0.admin) && ((class2.userData_0.Zone == null) || !class2.worldZone_0.Internal.Contains(class2.userData_0.Zone)))
                                {
                                    newpos = position;
                                    taken = TruthDetector.ActionTaken.Moved;
                                    return;
                                }
                                if (!string.IsNullOrEmpty(class2.worldZone_0.Notice_OnEnter))
                                {
                                    Broadcast.Notice(class2.netUser_0, "☢", class2.worldZone_0.Notice_OnEnter, 5f);
                                }
                                foreach (string str2 in class2.worldZone_0.Message_OnEnter)
                                {
                                    Broadcast.Message(class2.netUser_0, str2, null, 0f);
                                }
                            }
                            class2.userData_0.Zone = class2.worldZone_0;
                            if (((class2.worldZone_0 != null) && (class2.worldZone_0.WarpZone != null)) && (class2.worldZone_0.WarpZone.Spawns.Count > 0))
                            {
                                if (class2.worldZone_0.WarpTime > 0L)
                                {
                                    timer = new EventTimer {
                                        Interval = class2.worldZone_0.WarpTime * 0x3e8L,
                                        AutoReset = false
                                    };
                                    if (handler == null)
                                    {
                                        handler = new ElapsedEventHandler(class2.method_1);
                                    }
                                    timer.Elapsed += handler;
                                    timer.Sender = class2.netUser_0;
                                    timer.Command = class2.worldZone_0.Defname;
                                    timer.Start();
                                    Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessageTeleport("Player.WarpZone.Start", class2.netUser_0, class2.worldZone_0, null), 2f);
                                }
                                else
                                {
                                    PlayerWarp(class2.netUser_0, class2.userData_0, class2.worldZone_0);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void PlayerWarp(NetUser netUser, UserData user, WorldZone moveZone)
        {
            Class57 class2 = new Class57 {
                netUser_0 = netUser,
                worldZone_0 = moveZone
            };
            EventTimer timer = Events.Timer.Find(new Predicate<EventTimer>(class2.method_0));
            if (timer != null)
            {
                timer.Dispose();
            }
            if (class2.netUser_0 != null)
            {
                int num = UnityEngine.Random.Range(0, class2.worldZone_0.WarpZone.Spawns.Count);
                Helper.TeleportTo(class2.netUser_0, class2.worldZone_0.WarpZone.Spawns[num]);
                Broadcast.Notice(class2.netUser_0, "☢", Config.GetMessageTeleport("Player.WarpZone.Teleported", class2.netUser_0, class2.worldZone_0, null), 2f);
            }
        }

        public static bool SaveAsFile()
        {
            if (Database == null)
            {
                return false;
            }
            using (StreamWriter writer = new StreamWriter(SaveFilePath))
            {
                foreach (string str in Database.Keys)
                {
                    writer.WriteLine("[ZONE " + str + "]");
                    writer.WriteLine("NAME=" + Database[str].Name);
                    if (Database[str].Flags != 0)
                    {
                        writer.WriteLine("FLAGS=" + Database[str].Flags);
                    }
                    foreach (WorldZone zone in Database[str].Internal)
                    {
                        writer.WriteLine("INTERNAL=" + zone.Defname);
                    }
                    if (Database[str].Center != Vector2.zero)
                    {
                        writer.WriteLine("CENTER=" + Database[str].Center.AsString().Replace(" ", ""));
                    }
                    if (Database[str].WarpZone != null)
                    {
                        writer.WriteLine(string.Concat(new object[] { "WARP=", Database[str].WarpZone.Defname, ",", Database[str].WarpTime }));
                    }
                    foreach (Vector2 vector in Database[str].Points)
                    {
                        writer.WriteLine("POINT=" + vector.AsString().Replace(" ", ""));
                    }
                    foreach (Vector3 vector2 in Database[str].Spawns)
                    {
                        writer.WriteLine("SPAWN=" + vector2.AsString().Replace(" ", ""));
                    }
                    foreach (string str2 in Database[str].ForbiddenCommand)
                    {
                        writer.WriteLine("FORBIDDEN.COMMAND=" + str2);
                    }
                    if (!string.IsNullOrEmpty(Database[str].Notice_OnEnter))
                    {
                        writer.WriteLine("ENTER.NOTICE=" + Database[str].Notice_OnEnter);
                    }
                    if (!string.IsNullOrEmpty(Database[str].Notice_OnLeave))
                    {
                        writer.WriteLine("LEAVE.NOTICE=" + Database[str].Notice_OnLeave);
                    }
                    foreach (string str3 in Database[str].Message_OnEnter)
                    {
                        writer.WriteLine("ENTER.MESSAGE=" + str3);
                    }
                    foreach (string str4 in Database[str].Message_OnLeave)
                    {
                        writer.WriteLine("ENTER.MESSAGE=" + str4);
                    }
                    writer.WriteLine();
                }
            }
            return true;
        }

        public static void ShowPoints(WorldZone zone)
        {
            foreach (Vector2 vector in zone.Points)
            {
                Vector3 ground = GetGround(vector.x, vector.y);
                Markers.Add(World.Spawn(";struct_metal_pillar", ground));
                Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 4f, 0f)));
                Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 8f, 0f)));
                Markers.Add(World.Spawn(";struct_metal_pillar", ground + new Vector3(0f, 12f, 0f)));
            }
        }

        public static Dictionary<string, WorldZone> All
        {
            get
            {
                return Database;
            }
        }

        public static int Count
        {
            get
            {
                return Database.Count;
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

        public static bool IsBuild
        {
            get
            {
                return (LastZone != null);
            }
        }

        public static WorldZone LastZone
        {
            [CompilerGenerated]
            get
            {
                return worldZone_0;
            }
            [CompilerGenerated]
            private set
            {
                worldZone_0 = value;
            }
        }

        public static string SaveFilePath
        {
            [CompilerGenerated]
            get
            {
                return string_1;
            }
            [CompilerGenerated]
            private set
            {
                string_1 = value;
            }
        }

        [CompilerGenerated]
        private sealed class Class56
        {
            public NetUser netUser_0;
            public UserData userData_0;
            public WorldZone worldZone_0;

            public bool method_0(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.userData_0.Zone.Defname));
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
                return ((eventTimer_0.Sender == this.netUser_0) && (eventTimer_0.Command == this.worldZone_0.Defname));
            }
        }
    }
}

