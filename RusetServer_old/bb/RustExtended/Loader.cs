using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace RustExtended
{
	public class Loader
	{
		private struct Struct0
		{
			private string string_0;

			private string string_1;

			private string string_2;

			private string string_3;

			private string string_4;

			private string string_5;

			private string string_6;

			private string string_7;

			public string Agent
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_0));
				}
			}

			public string Hostname
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_1));
				}
			}

			public string ServerIP
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_2));
				}
			}

			public string ServerPort
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_3));
				}
			}

			public string MachineID
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_5));
				}
			}

			public string Timestamp
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_6));
				}
			}

			public string Filename
			{
				get
				{
					return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_7));
				}
			}

			public string AsString
			{
				get
				{
					return this.ToString();
				}
			}

			public Struct0(string agent, string hostname, string ip, int port, string ext_ip, string machineid, DateTime date, string filename)
			{
				this.string_0 = Convert.ToBase64String(Encoding.ASCII.GetBytes(agent));
				this.string_1 = Convert.ToBase64String(Encoding.ASCII.GetBytes(hostname));
				this.string_2 = Convert.ToBase64String(Encoding.ASCII.GetBytes(ip));
				this.string_3 = Convert.ToBase64String(Encoding.ASCII.GetBytes(port.ToString()));
				this.string_4 = Convert.ToBase64String(Encoding.ASCII.GetBytes(ext_ip.ToString()));
				this.string_5 = Convert.ToBase64String(Encoding.ASCII.GetBytes(machineid.ToString()));
				this.string_6 = Convert.ToBase64String(Encoding.ASCII.GetBytes(date.ToString("yyyy-MM-dd:mm")));
				this.string_7 = Convert.ToBase64String(Encoding.ASCII.GetBytes(filename));
			}

			public override string ToString()
			{
				return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", new object[]
				{
					Convert.ToChar(this.string_0.Length),
					this.string_0,
					Convert.ToChar(this.string_1.Length),
					this.string_1,
					Convert.ToChar(this.string_2.Length),
					this.string_2,
					Convert.ToChar(this.string_3.Length),
					this.string_3,
					Convert.ToChar(this.string_4.Length),
					this.string_4,
					Convert.ToChar(this.string_6.Length),
					this.string_6,
					Convert.ToChar(this.string_7.Length),
					this.string_7,
					Convert.ToChar(this.string_5.Length),
					this.string_5
				});
			}
		}

		public class WebClientRequest : WebClient
		{
			[CompilerGenerated]
			private int int_0;

			public int Timeout
			{
				get;
				set;
			}

			protected override WebRequest GetWebRequest(Uri address)
			{
				WebRequest webRequest = base.GetWebRequest(address);
				webRequest.Timeout = this.Timeout;
				return webRequest;
			}
		}

		private class ClientThread
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
				if (this.client.Receive(array) != 0)
				{
					UnityEngine.Debug.Log("123123123");
					Loader.Log("11111", true);
				}
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
						if (array2.Length >= 1)
						{
							if (!(array2[0] == "0"))
							{
								if (array2[0] == "1")
								{
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

		public static string RootPath = "";

		public static string DataPath = "rust_server_Data";

		public static string SavePath = "serverdata";

		public static string LogsPath = "logs";

		public static string ConfigFile = "cfg/server.cfg";

		public static string ProductName = Assembly.GetExecutingAssembly().GetName().Name;

		public static string SoftwareNotVerified = "You can't run server with RustExtended on this IP with this port.\nPlease purchase this modification from a developer before to use.\nVisit web-site: http://www.rust-extended.ru/ for more details.";

		private static List<string> list_0 = new List<string>();

		public static uint LasthostId = 0u;

		[CompilerGenerated]
		private static string string_0;

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static string string_2;

		[CompilerGenerated]
		private static string string_3;

		[CompilerGenerated]
		private static string[] string_4;

		private static System.Net.Sockets.Socket dlqsv;

		private static System.Net.Sockets.Socket serverSocket;

		public static string qweqwe;

		public static string RustLogFile
		{
			get
			{
				return Path.Combine(Loader.LogsPath, "Rust" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
			}
		}

		public static string ChatLogFile
		{
			get
			{
				return Path.Combine(Loader.LogsPath, "Chat" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
			}
		}

		public static string ServSQLFile
		{
			get
			{
				return Path.Combine(Loader.LogsPath, "MySQL" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log");
			}
		}

		public static string ServerIP
		{
			get;
			private set;
		}

		public static string ServerHost
		{
			get;
			private set;
		}

		public static string ExternalIP
		{
			get;
			private set;
		}

		public static string HardwareID
		{
			get;
			private set;
		}

		public static string[] AuthList
		{
			get;
			private set;
		}

		public static void Initialize()
		{
			try
			{
				Loader.RootPath = Directory.GetCurrentDirectory();
				Loader.DataPath = Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location));
				Loader.SavePath = Path.GetDirectoryName(ServerSaveManager.autoSavePath);
				Loader.LogsPath = Path.Combine(Loader.SavePath, Loader.LogsPath);
				if (!Directory.Exists(Loader.DataPath))
				{
					Directory.CreateDirectory(Loader.DataPath);
				}
				if (!Directory.Exists(Loader.SavePath))
				{
					Directory.CreateDirectory(Loader.SavePath);
				}
				if (!Directory.Exists(Loader.LogsPath))
				{
					Directory.CreateDirectory(Loader.LogsPath);
				}
				Loader.AuthList = new string[]
				{
					"cmV2aXNpb25zLnR4dA==",
					"UnVzdEV4dGVuZGVkLkJldGE=",
					"UnVzdEV4dGVuZGVkLlJlbGVhc2U=",
					"aHR0cDovLzEyNC4yMjguOTEuMjEyOjI4MDAyLw==",
					"aHR0cDovLzEyNC4yMjguOTEuMjEyOjI4MDAyLw=="
				};
				Loader.HardwareID = Hardware.MachineID;
				if (string.IsNullOrEmpty(Loader.HardwareID))
				{
					throw new ArgumentException("This machine not can have hardware ID.");
				}
				Loader.ServerIP = (string.IsNullOrEmpty(server.ip) ? "127.0.0.1" : server.ip);
				Loader.ServerHost = Loader.ServerIP + ":" + server.port;
				for (int i = 0; i < Loader.AuthList.Length; i++)
				{
					Loader.AuthList[i] = Encoding.ASCII.GetString(Convert.FromBase64String(Loader.AuthList[i]));
				}
				for (int j = 3; j < Loader.AuthList.Length; j++)
				{
					Loader.list_0.Add(Loader.AuthList[j]);
				}
				Loader.ExternalIP = Loader.GetExternalIP();
				if (!Regex.IsMatch(Loader.ExternalIP, "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$"))
				{
					throw new ArgumentException("No available servers, check your internet connection.");
				}
				if (Loader.ServerIP == "127.0.0.1")
				{
					Loader.ServerIP = Loader.ExternalIP;
				}
				foreach (string current in Loader.list_0)
				{
					byte[] array = Loader.ReceiveWebFile(current, Loader.AuthList[0]);
					if (array.Length > 5 && Encoding.ASCII.GetString(array, 0, 5) != "<!DOC")
					{
						if (array.Length > 24 || !Regex.IsMatch(Loader.ServerIP, "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$"))
						{
							byte[] rawAssembly = Loader.ReceiveWebFile(current, extended.beta ? Loader.AuthList[1] : Loader.AuthList[2]);
							try
							{
                                //File.WriteAllBytes(@"C:\piyan.dll", rawAssembly);
                                AppDomain.CurrentDomain.Load(rawAssembly);
							}
							catch (Exception)
							{
								Loader.LasthostId += 1u;
								continue;
							}
							Loader.AuthList[0] = new FileInfo(Assembly.GetExecutingAssembly().Location).Length.ToString();
							Loader.AuthList[extended.beta ? 1 : 2] = current;
							Loader.AuthList[extended.beta ? 2 : 1] = string.Empty;
							break;
						}
						throw new ArgumentException(Loader.SoftwareNotVerified);
					}
					else
					{
						Loader.LasthostId += 1u;
					}
				}
				string b = new FileInfo(Assembly.GetExecutingAssembly().Location).Length.ToString();
				string value = Convert.ToBase64String(Encoding.ASCII.GetBytes(Loader.AuthList[extended.beta ? 1 : 2]));
				if (Loader.AuthList[0] != b || Loader.AuthList.Contains(value))
				{
					throw new ArgumentException("Sorry, authentication server now not is available.\nPlease try again after few minutes.");
				}
				Method.Initialize();
				Method.Invoke("RustExtended.Bootstrap.Initialize");
			}
			catch (Exception ex)
			{
				ConsoleSystem.LogError(ex.Message.ToString());
				Thread.Sleep(5000);
				Process.GetCurrentProcess().Kill();
			}
		}

		private static void TcpListen()
		{
			try
			{
				while (true)
				{
					System.Net.Sockets.Socket k = Loader.dlqsv.Accept();
					Loader.ClientThread @object = new Loader.ClientThread(k);
					Thread thread = new Thread(new ThreadStart(@object.ClientService));
					thread.Start();
				}
			}
			catch
			{
			}
		}

		public static byte[] ReceiveWebFile(string uri, string filename)
		{
			byte[] result;
			using (Loader.WebClientRequest webClientRequest = new Loader.WebClientRequest())
			{
				try
				{
					Loader.Struct0 @struct = new Loader.Struct0("RustExtended.Loader", server.hostname, Loader.ServerIP, server.port, Loader.ExternalIP, Loader.HardwareID, DateTime.UtcNow, filename);
					NameValueCollection nameValueCollection = new NameValueCollection();
					nameValueCollection.Add("data", Encryption.Encrypt(@struct.AsString));
					webClientRequest.Headers[HttpRequestHeader.UserAgent] = @struct.Agent;
					webClientRequest.Timeout = 5000;
					result = webClientRequest.UploadValues(uri, "POST", nameValueCollection);
				}
				catch (Exception ex)
				{
					Console.CursorTop--;
					Loader.LogError("Receiving Error: " + ex.Message, true);
					result = new byte[0];
				}
			}
			return result;
		}

		public static string GetExternalIP()
		{
			string[] array = Loader.list_0.ToArray();
			string[] array2 = array;
			string result;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				try
				{
					WebRequest webRequest = WebRequest.Create(text);
					webRequest.Timeout = 5000;
					string text2 = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();
					if (text2 == null || text2.Length == 0)
					{
						throw new Exception("No response.");
					}
					MatchCollection matchCollection = Regex.Matches(text2, "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}");
					if (matchCollection.Count == 0)
					{
						throw new Exception("Bad response.");
					}
					result = matchCollection[0].Value.Trim();
					return result;
				}
				catch (Exception ex)
				{
					Console.CursorTop--;
					Loader.LogError(string.Concat(new object[]
					{
						"Connecting to authentication server #",
						Loader.LasthostId,
						" - Failed: ",
						ex.Message
					}), true);
					Loader.list_0.Remove(text);
				}
			}
			result = string.Empty;
			return result;
		}

		public static void Log(string msg, bool inConsole = true)
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

		public static void LogWarning(string msg, bool inConsole = true)
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
				ConsoleSystem.PrintWarning(msg, false);
			}
		}

		public static void LogError(string msg, bool inConsole = true)
		{
			using (FileStream fileStream = new FileStream(Loader.RustLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "[ERROR]: " + msg);
				}
			}
			File.SetLastWriteTime(Loader.RustLogFile, DateTime.Now);
			if (inConsole)
			{
				ConsoleSystem.PrintError(msg, false);
			}
		}
	}
}
