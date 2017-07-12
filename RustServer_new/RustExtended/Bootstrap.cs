using Facepunch;
using Facepunch.Utility;
using Magma;
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

namespace RustExtended
{
	public class Bootstrap : Facepunch.MonoBehaviour
	{
		public static GameObject Singleton;

		public static bool Initialized = false;

		public static uint LastTickCount = 0u;

		public static uint UpdateTime = 0u;

		protected static bool REV_Beta;

		protected static string REV_HWID;

		protected static string REV_SrvIP;

		protected static string REV_ExtIP;

		protected static string REV_Length;

		protected static string REV_AuthURI;

		protected static string[] REV_Servs;

		protected static float REV_NextTime;

		public static float NextPacketsTimeStamp = 0f;

		public static uint SendPacketsPerSecond = 0u;

		public static uint RecvPacketsPerSecond = 0u;

		public static uint SendPacketCounter = 0u;

		public static uint RecvPacketCounter = 0u;

		public static FileStream OutputLogStream;

		public static string OutputLogFile = "output_log.txt";

		public static string OutputLogString = string.Empty;

		public static long OutputLogOffset = 0L;

		public static long OutputLogErrors = 0L;

		public static float LastErrorTime = 0f;

		public static void Initialize()
		{
			try
			{
				serv.servData.SteamID = 0uL;
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
				if (Core.ServerIP == "127.0.0.1" || Core.ExternalIP == "127.0.0.1")
				{
					throw new ArgumentException(Helper.ServerDenyToStarted);
				}
				if (NetCull.config.localIP != "" && NetCull.config.localIP != Core.ServerIP)
				{
					throw new ArgumentException(Helper.ServerDenyToStarted);
				}
				if (!Core.BetaVersion)
				{
					Helper.Log("= HUOJIÈÙÓþ³öÆ· =", true);
				}
				Core.AssemblyVerifed = Helper.AssemblyVerify();
				if (!Core.AssemblyVerifed)
				{
					throw new ArgumentException(Helper.AssemblyNotVerified);
				}
				Config.Initialize();
				if (Environment.CommandLine.Contains("-debug"))
				{
					Core.Debug = true;
				}
				Bootstrap.Singleton = new GameObject(typeof(Bootstrap).FullName);
				Bootstrap.Singleton.AddComponent<Bootstrap>();
				Bootstrap.Singleton.AddComponent<Magma.Bootstrap>();
				Main.Init();
				Main.Call("ServerStart", null);
				Bootstrap.OutputLogFile = CommandLine.GetSwitch("-logfile", Path.Combine(Core.DataPath, Bootstrap.OutputLogFile));
				Bootstrap.OutputLogStream = File.Open(Bootstrap.OutputLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
					if (!MySQL.Connect(Core.MySQL_Host, Core.MySQL_Username, Core.MySQL_Password, Core.MySQL_Database, Core.MySQL_Port, null, (MySQL.ClientFlags)0uL))
					{
						throw new ArgumentException("MYSQL ERROR: " + MySQL.Error());
					}
					Helper.LogSQL(string.Format("Connected to \"{0}\" on port {1}, version: {2}", Core.MySQL_Host, Core.MySQL_Port, MySQL.ServerVersion), true);
					List<string> list = MySQL.Databases(null);
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
			catch (Exception ex)
			{
				ConsoleSystem.LogError(ex.Message.ToString());
				Thread.Sleep(5000);
				Process.GetCurrentProcess().Kill();
			}
		}

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.InvokeRepeating("CheckActivityConnections", 0f, 0.5f);
		}

		public void Update()
		{
			if (Core.Initialized && NetCull.isServerRunning)
			{
				if (Bootstrap.LastTickCount > 0u)
				{
					Bootstrap.UpdateTime = (uint)(Environment.TickCount - (int)Bootstrap.LastTickCount);
				}
				if (Bootstrap.UpdateTime > 35u)
				{
					RustHook.RustSteamServer_UpdateServerTitle();
				}
				Bootstrap.LastTickCount = (uint)Environment.TickCount;
				if (Time.time > Bootstrap.NextPacketsTimeStamp)
				{
					Bootstrap.NextPacketsTimeStamp = Time.time + 1f;
					Bootstrap.SendPacketsPerSecond = Bootstrap.SendPacketCounter;
					Bootstrap.SendPacketCounter = 0u;
					Bootstrap.RecvPacketsPerSecond = Bootstrap.RecvPacketCounter;
					Bootstrap.RecvPacketCounter = 0u;
				}
				if (Time.time > Bootstrap.REV_NextTime)
				{
					try
					{
						Bootstrap.REV_NextTime = Time.time + 10f;
						string a = NetCull.config.localIP.Trim();
						Bootstrap.REV_Beta = Method.Invoke("extended.beta").AsBoolean;
						Bootstrap.REV_HWID = Method.Invoke("RustExtended.Loader.HardwareID").AsString;
						Bootstrap.REV_Servs = Method.Invoke("RustExtended.Loader.AuthList").AsStrings;
						Bootstrap.REV_SrvIP = Method.Invoke("RustExtended.Loader.ServerIP").AsString;
						Bootstrap.REV_ExtIP = Method.Invoke("RustExtended.Loader.ExternalIP").AsString;
						Bootstrap.REV_AuthURI = "";
						Bootstrap.REV_Length = Path.Combine(Core.DataPath, "Managed\\RustExtended.dll");
						Bootstrap.REV_Length = (File.Exists(Bootstrap.REV_Length) ? new FileInfo(Bootstrap.REV_Length).Length.ToString() : null);
						if (!string.IsNullOrEmpty(Bootstrap.REV_Length) && Bootstrap.REV_Servs.Length >= 4 && !(Bootstrap.REV_Servs[0] != Bootstrap.REV_Length))
						{
							Bootstrap.REV_AuthURI = Bootstrap.REV_Servs[Bootstrap.REV_Beta ? 1 : 2];
						}
						else
						{
							Core.AssemblyVerifed = false;
						}
						if (a != "" && NetCull.config.localIP != Bootstrap.REV_SrvIP)
						{
							Core.AssemblyVerifed = false;
						}
						if (!Core.AssemblyVerifed || string.IsNullOrEmpty(Bootstrap.REV_HWID) || string.IsNullOrEmpty(Bootstrap.REV_SrvIP) || string.IsNullOrEmpty(Bootstrap.REV_ExtIP) || !Bootstrap.REV_Servs.Contains(Bootstrap.REV_AuthURI))
						{
							Helper.LogWarning(Helper.ServerDenyToStarted, true);
							Thread.Sleep(5000);
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
				if (Bootstrap.OutputLogStream != null)
				{
					Bootstrap.OutputLogString = string.Empty;
					if (Bootstrap.OutputLogOffset > 0L && Bootstrap.OutputLogOffset < Bootstrap.OutputLogStream.Length)
					{
						int num = (int)(Bootstrap.OutputLogStream.Length - Bootstrap.OutputLogOffset);
						byte[] array = new byte[num];
						Bootstrap.OutputLogStream.Seek(Bootstrap.OutputLogOffset, SeekOrigin.Begin);
						Bootstrap.OutputLogStream.Read(array, 0, num);
						Bootstrap.OutputLogOffset = Bootstrap.OutputLogStream.Length;
						Bootstrap.OutputLogString = Encoding.UTF8.GetString(array);
					}
					else if (Bootstrap.OutputLogOffset == 0L)
					{
						Bootstrap.OutputLogOffset = Bootstrap.OutputLogStream.Seek(0L, SeekOrigin.End);
					}
				}
				if (Bootstrap.OutputLogString != string.Empty && Bootstrap.OutputLogString.Contains("Failed to create agent because"))
				{
					Bootstrap.OutputLogString = string.Empty;
					int num2 = this.method_0();
					if (num2 > 0)
					{
						Helper.Log("WildlifeAI: Successfully removed " + num2 + " creatures having errors of NavMesh agent.", false);
					}
				}
				else if (Bootstrap.OutputLogString != string.Empty && Bootstrap.OutputLogString.Contains("only be called on an active agent"))
				{
					Bootstrap.OutputLogString = string.Empty;
					int num3 = this.method_0();
					if (num3 > 0)
					{
						Helper.Log("WildlifeAI: Successfully removed " + num3 + " creatures having errors of NavMesh agent.", false);
					}
				}
				if (!Core.HasShutdown && Bootstrap.OutputLogString != string.Empty && (Core.ErrorsShutdown || Core.ErrorsRestart))
				{
					if (!Bootstrap.OutputLogString.StartsWith("System.NullReferenceException:") && !Bootstrap.OutputLogString.StartsWith("NullReferenceException:"))
					{
						if (!Bootstrap.OutputLogString.StartsWith("System.InvalidCastException:") && !Bootstrap.OutputLogString.StartsWith("ArgumentException:"))
						{
							if (!Bootstrap.OutputLogString.StartsWith("Error:") && !Bootstrap.OutputLogString.StartsWith("Invalid parameter because it was infinity or nan"))
							{
								if (Bootstrap.OutputLogErrors > 0L && Bootstrap.LastErrorTime + 1f > Time.time)
								{
									Bootstrap.LastErrorTime = Time.time;
									Bootstrap.OutputLogErrors -= 1L;
								}
							}
							else
							{
								Bootstrap.OutputLogErrors += 1L;
								Bootstrap.LastErrorTime = Time.time;
							}
						}
						else
						{
							Bootstrap.OutputLogErrors += 1L;
							Bootstrap.LastErrorTime = Time.time;
						}
					}
					else
					{
						Bootstrap.OutputLogErrors += 1L;
						Bootstrap.LastErrorTime = Time.time;
					}
					if (Bootstrap.OutputLogErrors > Core.ErrorsThreshold)
					{
						if (!Core.HasShutdown && Core.ErrorsShutdown)
						{
							Core.HasShutdown = true;
							int num4 = 0;
							Helper.LogWarning("Server has shutdown, because very a lot of errors.", true);
							Events.EventServerShutdown(null, 0, ref num4);
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
				if (Core.AutoShutdown > 0uL && !Core.HasShutdown && Time.time > Core.AutoShutdown * 60uL)
				{
					Commands.Shutdown(null, "serv.shutdown", new string[0]);
				}
				if (Core.AutoRestart > 0uL && !Core.HasShutdown && Time.time > Core.AutoRestart * 60uL)
				{
					Commands.Restart(null, "serv.restart", new string[0]);
				}
			}
		}

		private int method_0()
		{
			int num = 0;
			BasicWildLifeAI[] array = UnityEngine.Object.FindObjectsOfType<BasicWildLifeAI>();
			BasicWildLifeAI[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				BasicWildLifeAI basicWildLifeAI = array2[i];
				TakeDamage component = basicWildLifeAI.GetComponent<TakeDamage>();
				if (!(component == null) && component.alive)
				{
					NavMeshMovement component2 = basicWildLifeAI.GetComponent<NavMeshMovement>();
					if (component2 == null || component2._agent == null || component2._agent.pathStatus == NavMeshPathStatus.PathInvalid)
					{
						int num2 = WildlifeManager.Data.lifeInstances.IndexOf(basicWildLifeAI);
						if (num2 != -1 && num2 < WildlifeManager.Data.lifeInstanceCount)
						{
							if (component2 != null)
							{
								Helper.Log(string.Concat(new object[]
								{
									"WildLifeAI: Creature '",
									Helper.NiceName(basicWildLifeAI.gameObject.name),
									" at ",
									basicWildLifeAI.gameObject.transform.position,
									"' has been removed because creature without mesh agent."
								}), false);
							}
							else
							{
								Helper.Log(string.Concat(new object[]
								{
									"WildLifeAI: Creature '",
									Helper.NiceName(basicWildLifeAI.gameObject.name),
									" at ",
									basicWildLifeAI.gameObject.transform.position,
									"' has been removed because mesh agent have invalid path."
								}), false);
							}
							WildlifeManager.Data.lifeInstances.RemoveAt(num2);
							WildlifeManager.Data.lifeInstanceCount--;
							WildlifeManager.Data.thinkIterator = 0;
							NetCull.Destroy(basicWildLifeAI.gameObject);
							num++;
						}
					}
				}
			}
			return num;
		}

		public void CheckActivityConnections()
		{
			if (NetCull.isServerRunning && Users.NetworkTimeout > 0f && Bootstrap.UpdateTime / 1000u < Users.NetworkTimeout)
			{
				uLink.NetworkPlayer[] connections = NetCull.connections;
				uLink.NetworkPlayer[] array = connections;
				for (int i = 0; i < array.Length; i++)
				{
					uLink.NetworkPlayer networkPlayer = array[i];
					NetUser netUser = NetUser.Find(networkPlayer);
					Character character = null;
                    if (netUser != null && Truth.LastPacketTime.ContainsKey(netUser) && Character.FindByUser(netUser.userID, out character))
					{
						float num = Time.time - Truth.LastPacketTime[netUser];
						if (Truth.LastPacketTime[netUser] > 0f && num > Users.NetworkTimeout)
						{
							Helper.LogWarning(string.Concat(new object[]
							{
								"Kicked by Server [",
								netUser.displayName,
								":",
								netUser.userID,
								"]: No receiving packets from client ",
								num.ToString("F2"),
								" second(s)."
							}), true);
							netUser.Kick(NetError.ConnectionTimeout, true);
						}
					}
				}
			}
		}
	}
}
