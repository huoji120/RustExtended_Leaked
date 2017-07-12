namespace RustExtended
{
    using Facepunch;
    using Facepunch.Utility;
    using Oxide;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using uLink;
    using UnityEngine;

    public class Bootstrap : Facepunch.MonoBehaviour
    {
        public static bool Initialized = false;
        public static float LastErrorTime = 0f;
        public static uint LastTickCount = 0;
        public static float NextPacketsTimeStamp = 0f;
        public static long OutputLogErrors = 0L;
        public static string OutputLogFile = "output_log.txt";
        public static long OutputLogOffset = 0L;
        public static FileStream OutputLogStream;
        public static string OutputLogString = string.Empty;
        public static uint RecvPacketCounter = 0;
        public static uint RecvPacketsPerSecond = 0;
        protected static string REV_AuthURI;
        protected static bool REV_Beta;
        protected static string REV_ExtIP;
        protected static string REV_HWID;
        protected static string REV_Length;
        protected static float REV_NextTime;
        protected static string[] REV_Servs;
        protected static string REV_SrvIP;
        public static uint SendPacketCounter = 0;
        public static uint SendPacketsPerSecond = 0;
        public static GameObject Singleton;
        public static uint UpdateTime = 0;

        public void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            base.InvokeRepeating("CheckActivityConnections", 0f, 0.5f);
        }

        public void CheckActivityConnections()
        {
            if ((NetCull.isServerRunning && (Users.NetworkTimeout > 0f)) && ((UpdateTime / 0x3e8) < Users.NetworkTimeout))
            {
                foreach (uLink.NetworkPlayer player in NetCull.connections)
                {
                    NetUser key = NetUser.Find(player);
                    Character character = null;
                    if (((key != null) && Truth.LastPacketTime.ContainsKey(key)) && Character.FindByUser(key.userID, out character))
                    {
                        float num = Time.time - Truth.LastPacketTime[key];
                        if ((Truth.LastPacketTime[key] > 0f) && (num > Users.NetworkTimeout))
                        {
                            Helper.LogWarning(string.Concat(new object[] { "Kicked by Server [", key.displayName, ":", key.userID, "]: No receiving packets from client ", num.ToString("F2"), " second(s)." }), true);
                            key.Kick(NetError.ConnectionTimeout, true);
                        }
                    }
                }
            }
        }

        public static void Initialize()
        {
            try
            {
                serv.servData.SteamID = 0L;
                serv.servData.Username = "SERVER";
                serv.servData.Flags = UserFlags.admin;
                serv.servData.FirstConnectIP = "127.0.0.1";
                serv.servData.LastConnectIP = "127.0.0.1";
                Core.RootPath = Directory.GetCurrentDirectory();
                Core.DataPath = Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location));
                Core.SavePath = Path.GetDirectoryName(ServerSaveManager.autoSavePath);
                Core.LogsPath = Path.Combine(Core.SavePath, Core.LogsPath);
                if (!Directory.Exists(Core.DataPath))
                {
                    Directory.CreateDirectory(Core.DataPath);
                }
                if (!Directory.Exists(Core.SavePath))
                {
                    Directory.CreateDirectory(Core.SavePath);
                }
                if (!Directory.Exists(Core.LogsPath))
                {
                    Directory.CreateDirectory(Core.LogsPath);
                }
                
                Helper.Initialize();
                Loader.Log("Really Initialize", true);
                if ((Core.ServerIP == "127.0.0.1") || (Core.ExternalIP == "127.0.0.1"))
                {
                    throw new ArgumentException(Helper.ServerDenyToStarted);
                }
                if ((NetCull.config.localIP != "") && (NetCull.config.localIP != Core.ServerIP))
                {
                    throw new ArgumentException(Helper.ServerDenyToStarted);
                }
                Loader.Log("ALL OK", true);

                if (!Core.BetaVersion)
                {
                    Helper.Log("RustExtended Version " + Core.Version.ToString() + " (RELEASE)", true);
                }
                else
                {
                    Helper.LogWarning("RustExtended Version " + Core.Version.ToString() + " (BETA)", true);
                }
                Core.AssemblyVerifed = Helper.AssemblyVerify();
                if (!Core.AssemblyVerifed)
                {
                    throw new ArgumentException(Helper.AssemblyNotVerified);
                }
                
                if (!Core.AssemblyVerifed)
                {
                    UnityEngine.Debug.Log("AssemblyVerified.All Good");
                }
                Helper.Log("All assembly versions has successfully verified.", true);
                Config.Initialize();
                if (Environment.CommandLine.Contains("-debug"))
                {
                    Core.Debug = true;
                }
                Singleton = new GameObject(typeof(Bootstrap).FullName);
                Singleton.AddComponent<Bootstrap>();
                Singleton.AddComponent<Bootstrap>();
                Main.Init();
                Main.Call("ServerStart", null);
                OutputLogFile = CommandLine.GetSwitch("-logfile", Path.Combine(Core.DataPath, OutputLogFile));
                OutputLogStream = File.Open(OutputLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (Core.DatabaseType.Equals("MYSQL"))
                {
                    if (!MySQL.Initialize())
                    {
                        throw new ArgumentException("MYSQL ERROR: Version " + MySQL.Version + " of library \"libmysql.dll\" is not supported.");
                    }
                    Helper.LogSQL("Client library version " + MySQL.Version, true);
                    if (Core.MySQL_UTF8)
                    {
                        MySQL.Charset = "utf8";
                    }
                    else
                    {
                        MySQL.Charset = "windows-1251";
                    }
                    if (!MySQL.Connect(Core.MySQL_Host, Core.MySQL_Username, Core.MySQL_Password, Core.MySQL_Database, Core.MySQL_Port, null, (MySQL.ClientFlags) 0L))
                    {
                        throw new ArgumentException("MYSQL ERROR: " + MySQL.Error());
                    }
                    Helper.LogSQL(string.Format("Connected to \"{0}\" on port {1}, version: {2}", Core.MySQL_Host, Core.MySQL_Port, MySQL.ServerVersion), true);
                    System.Collections.Generic.List<string> list = MySQL.Databases(null);
                    if (list == null)
                    {
                        throw new ArgumentException("MYSQL ERROR: Cannot receive list of database.");
                    }
                    if (!list.Contains(Core.MySQL_Database))
                    {
                        throw new ArgumentException("MYSQL ERROR: Database \"" + Core.MySQL_Database + "\" not exists.");
                    }
                    MySQL.Update(string.Format(Core.SQL_SERVER_SET, "time_startup", MySQL.QuoteString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                }
            }
            catch (Exception exception)
            {
                ConsoleSystem.LogError(exception.Message.ToString());
                Thread.Sleep(0x1388);
                Process.GetCurrentProcess().Kill();
            }
        }

        private int method_0()
        {
            int num = 0;
            foreach (BasicWildLifeAI eai in UnityEngine.Object.FindObjectsOfType<BasicWildLifeAI>())
            {
                TakeDamage component = eai.GetComponent<TakeDamage>();
                if ((component != null) && component.alive)
                {
                    NavMeshMovement movement = eai.GetComponent<NavMeshMovement>();
                    if (((movement == null) || (movement._agent == null)) || (movement._agent.pathStatus == NavMeshPathStatus.PathInvalid))
                    {
                        int index = WildlifeManager.Data.lifeInstances.IndexOf(eai);
                        if ((index != -1) && (index < WildlifeManager.Data.lifeInstanceCount))
                        {
                            if (movement != null)
                            {
                                Helper.Log(string.Concat(new object[] { "WildLifeAI: Creature '", Helper.NiceName(eai.gameObject.name), " at ", eai.gameObject.transform.position, "' has been removed because creature without mesh agent." }), false);
                            }
                            else
                            {
                                Helper.Log(string.Concat(new object[] { "WildLifeAI: Creature '", Helper.NiceName(eai.gameObject.name), " at ", eai.gameObject.transform.position, "' has been removed because mesh agent have invalid path." }), false);
                            }
                            WildlifeManager.Data.lifeInstances.RemoveAt(index);
                            WildlifeManager.Data.lifeInstanceCount--;
                            WildlifeManager.Data.thinkIterator = 0;
                            NetCull.Destroy(eai.gameObject);
                            num++;
                        }
                    }
                }
            }
            return num;
        }

        public void Update()
        {
            if (Core.Initialized && NetCull.isServerRunning)
            {
                if (LastTickCount > 0)
                {
                    UpdateTime = ((uint) Environment.TickCount) - LastTickCount;
                }
                if (UpdateTime > 0x23)
                {
                    RustHook.RustSteamServer_UpdateServerTitle();
                }
                LastTickCount = (uint) Environment.TickCount;
                if (Time.time > NextPacketsTimeStamp)
                {
                    NextPacketsTimeStamp = Time.time + 1f;
                    SendPacketsPerSecond = SendPacketCounter;
                    SendPacketCounter = 0;
                    RecvPacketsPerSecond = RecvPacketCounter;
                    RecvPacketCounter = 0;
                }
                if (Time.time > REV_NextTime)
                {
                    try
                    {
                        REV_NextTime = Time.time + 10f;
                        string str = NetCull.config.localIP.Trim();
                        REV_Beta = RustExtended.Method.Invoke("extended.beta").AsBoolean;
                        REV_HWID = RustExtended.Method.Invoke("RustExtended.Loader.HardwareID").AsString;
                        REV_Servs = RustExtended.Method.Invoke("RustExtended.Loader.AuthList").AsStrings;
                        REV_SrvIP = RustExtended.Method.Invoke("RustExtended.Loader.ServerIP").AsString;
                        REV_ExtIP = RustExtended.Method.Invoke("RustExtended.Loader.ExternalIP").AsString;
                        REV_AuthURI = "";
                        REV_Length = Path.Combine(Core.DataPath, @"Managed\RustExtended.dll");
                        REV_Length = File.Exists(REV_Length) ? new FileInfo(REV_Length).Length.ToString() : null;
                        if ((!string.IsNullOrEmpty(REV_Length) && (REV_Servs.Length >= 4)) && !(REV_Servs[0] != REV_Length))
                        {
                            REV_AuthURI = REV_Servs[REV_Beta ? 1 : 2];
                        }
                        else
                        {
                            Core.AssemblyVerifed = false;
                        }
                        if ((str != "") && (NetCull.config.localIP != REV_SrvIP))
                        {
                            Core.AssemblyVerifed = false;
                        }
                        if ((!Core.AssemblyVerifed || string.IsNullOrEmpty(REV_HWID)) || ((string.IsNullOrEmpty(REV_SrvIP) || string.IsNullOrEmpty(REV_ExtIP)) || !REV_Servs.Contains<string>(REV_AuthURI)))
                        {
                            Helper.LogWarning(Helper.ServerDenyToStarted, true);
                            Thread.Sleep(0x1388);
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (DateTime.Now.Subtract(Events.EventTime_DoServerEvents).TotalMilliseconds > 1000.0)
                {
                    Events.EventTime_DoServerEvents = DateTime.Now;
                    new Thread(new ThreadStart(Events.DoServerEvents)).Start();
                }
                if (DateTime.Now.Subtract(Events.EventTime_DoProcessUsers).TotalMilliseconds > 1000.0)
                {
                    Events.EventTime_DoProcessUsers = DateTime.Now;
                    new Thread(new ThreadStart(Events.DoProcessUsers)).Start();
                }
                if (DateTime.Now.Subtract(Events.EventTime_DoAirdropEvent).TotalMilliseconds > 500.0)
                {
                    Events.EventTime_DoAirdropEvent = DateTime.Now;
                    new Thread(new ThreadStart(Events.DoAirdropEvent)).Start();
                }
                if (OutputLogStream != null)
                {
                    OutputLogString = string.Empty;
                    if ((OutputLogOffset > 0L) && (OutputLogOffset < OutputLogStream.Length))
                    {
                        int count = (int) (OutputLogStream.Length - OutputLogOffset);
                        byte[] buffer = new byte[count];
                        OutputLogStream.Seek(OutputLogOffset, SeekOrigin.Begin);
                        OutputLogStream.Read(buffer, 0, count);
                        OutputLogOffset = OutputLogStream.Length;
                        OutputLogString = Encoding.UTF8.GetString(buffer);
                    }
                    else if (OutputLogOffset == 0L)
                    {
                        OutputLogOffset = OutputLogStream.Seek(0L, SeekOrigin.End);
                    }
                }
                if ((OutputLogString != string.Empty) && OutputLogString.Contains("Failed to create agent because"))
                {
                    OutputLogString = string.Empty;
                    int num2 = this.method_0();
                    if (num2 > 0)
                    {
                        Helper.Log("WildlifeAI: Successfully removed " + num2 + " creatures having errors of NavMesh agent.", false);
                    }
                }
                else if ((OutputLogString != string.Empty) && OutputLogString.Contains("only be called on an active agent"))
                {
                    OutputLogString = string.Empty;
                    int num3 = this.method_0();
                    if (num3 > 0)
                    {
                        Helper.Log("WildlifeAI: Successfully removed " + num3 + " creatures having errors of NavMesh agent.", false);
                    }
                }
                if ((!Core.HasShutdown && (OutputLogString != string.Empty)) && (Core.ErrorsShutdown || Core.ErrorsRestart))
                {
                    if (!OutputLogString.StartsWith("System.NullReferenceException:") && !OutputLogString.StartsWith("NullReferenceException:"))
                    {
                        if (!OutputLogString.StartsWith("System.InvalidCastException:") && !OutputLogString.StartsWith("ArgumentException:"))
                        {
                            if (!OutputLogString.StartsWith("Error:") && !OutputLogString.StartsWith("Invalid parameter because it was infinity or nan"))
                            {
                                if ((OutputLogErrors > 0L) && ((LastErrorTime + 1f) > Time.time))
                                {
                                    LastErrorTime = Time.time;
                                    OutputLogErrors -= 1L;
                                }
                            }
                            else
                            {
                                OutputLogErrors += 1L;
                                LastErrorTime = Time.time;
                            }
                        }
                        else
                        {
                            OutputLogErrors += 1L;
                            LastErrorTime = Time.time;
                        }
                    }
                    else
                    {
                        OutputLogErrors += 1L;
                        LastErrorTime = Time.time;
                    }
                    if (OutputLogErrors > Core.ErrorsThreshold)
                    {
                        if (!Core.HasShutdown && Core.ErrorsShutdown)
                        {
                            Core.HasShutdown = true;
                            int timeleft = 0;
                            Helper.LogWarning("Server has shutdown, because very a lot of errors.", true);
                            Events.EventServerShutdown(null, 0, ref timeleft);
                        }
                        if (!Core.HasShutdown && Core.ErrorsRestart)
                        {
                            Core.HasShutdown = true;
                            int num5 = 0;
                            Helper.LogWarning("Server has restarted, because very a lot of errors.", true);
                            Events.EventServerRestart(null, 0, ref num5);
                        }
                    }
                }
                if (((Core.AutoShutdown > 0L) && !Core.HasShutdown) && (Time.time > (Core.AutoShutdown * ((ulong) 60L))))
                {
                    Commands.Shutdown(null, "serv.shutdown", new string[0]);
                }
                if (((Core.AutoRestart > 0L) && !Core.HasShutdown) && (Time.time > (Core.AutoRestart * ((ulong) 60L))))
                {
                    Commands.Restart(null, "serv.restart", new string[0]);
                }
            }
        }
    }
}

