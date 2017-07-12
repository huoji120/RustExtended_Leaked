using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

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
                    return "117.78.40.149";
					//return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_1));
				}
			}
			public string ServerIP
			{
				get
				{
                    return "117.78.40.149";
					//return Encoding.ASCII.GetString(Convert.FromBase64String(this.string_2));
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
		public static string RootPath = "";
		public static string DataPath = "rust_server_Data";
		public static string SavePath = "serverdata";
		public static string LogsPath = "logs";
		public static string ConfigFile = "cfg/server.cfg";
		public static string ProductName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string SoftwareNotVerified = "Erro : Our server was temporarily out of the error.Please contact QQ1296564236 Or wait to 5 min.thanks :( ";
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
                Loader.Log("Server external IP: " + "bbs.wghostk.com", true);
                Loader.AuthList = new string[]
				{
					"cmV2aXNpb25zLnR4dA==",
					"UnVzdEV4dGVuZGVkLkJldGE=",
					"UnVzdEV4dGVuZGVkLlJlbGVhc2U=",
					"aHR0cDovLzEyNy4wLjAuMTo4OC8=",
                    "aHR0cDovLzEyNy4wLjAuMTo4OC8="
				};
               
				Loader.HardwareID = Hardware.MachineID;
				if (string.IsNullOrEmpty(Loader.HardwareID))
				{
					throw new ArgumentException("This machine not can have hardware ID.");
				}
				Loader.ServerIP = (string.IsNullOrEmpty(server.ip) ? "127.0.0.1" : server.ip);
				Loader.ServerHost = "123.123.123.123" + ":" + server.port;
				Loader.Log("Server started on " + Loader.ServerHost, true);
				Loader.Log("ID: " + Loader.HardwareID, true);
				/*for (int i = 0; i < Loader.AuthList.Length; i++)
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
                */
				if (Loader.ServerIP == "127.0.0.1")
				{
                    Loader.ServerIP = "123.123.123.123";
				}
				Loader.Log("Server external IP: " + Loader.ExternalIP, true);
                
                //AppDomain.CurrentDomain.Load("E:\\ak.txt");
                /*
				foreach (string current in Loader.list_0)
				{
					Loader.Log("Receiving authentication data from server # " + LasthostId, true);
					byte[] array = Loader.ReceiveWebFile(current, Loader.AuthList[0]);
					if (array.Length > 5 && Encoding.ASCII.GetString(array, 0, 5) != "<!DOC")
					{
						if (array.Length > 24 || !Regex.IsMatch(Loader.ServerIP, "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$"))
						{ 
							byte[] rawAssembly = Loader.ReceiveWebFile(current, extended.beta ? Loader.AuthList[1] : Loader.AuthList[2]);
                            // File.WriteAllBytes(@"c:\test.dll", rawAssembly);
							try
							{
								AppDomain.CurrentDomain.Load(rawAssembly);
                                //AppDomain.CurrentDomain.Load("E:\\ak.txt");
								goto IL_30E;
							}
							catch (Exception)
							{
								Loader.LasthostId += 1u;
								continue;
							}
							goto IL_2E9;
							IL_30E:
							File.WriteAllBytes(Path.Combine(Loader.RootPath, Loader.AuthList[0]), array);
							Loader.AuthList[0] = new FileInfo(Assembly.GetExecutingAssembly().Location).Length.ToString();
							Loader.AuthList[extended.beta ? 1 : 2] = current;
							Loader.AuthList[extended.beta ? 2 : 1] = string.Empty;
							break;
						}
						throw new ArgumentException(Loader.SoftwareNotVerified);
					}
					IL_2E9:
					Loader.LasthostId += 1u;
				}
				string b = new FileInfo(Assembly.GetExecutingAssembly().Location).Length.ToString();
				string value = Convert.ToBase64String(Encoding.ASCII.GetBytes(Loader.AuthList[extended.beta ? 1 : 2]));
                
                if (Loader.AuthList[0] != b || Loader.AuthList.Contains(value))
				{
					throw new ArgumentException("Sorry, authentication server now not is available.\nPlease try again after few minutes.");
				}*/
                //AppDomain.CurrentDomain.Load("Z:\\Z.dll");
                Loader.Log("OK,Now We Got huoji So Lets Go!.", true);
                //System.Diagnostics.Process.Start("http://www.wghostk.com/");    
                Loader.Log("------------------------.", true);
                Loader.Log("    Cracked BY HUOJI    .", true);
                Loader.Log("       QQ1296564236     .", true);
                Loader.Log("email:1296564236@qq.com .", true);
                Loader.Log("website:www.wghostk.com .", true);
                Loader.Log("------------------------.", true);
                Loader.Log("We are cracker and hacker.",true);
                Loader.Log("Please Wait 5 Seconds ...", true);

                try
                {
                    RustExtended.Bootstrap.Initialize();
                    // AppDomain.CurrentDomain.Load("RustExtended.Core");
                }
                catch (Exception e)
                {
                    Loader.Log(e.ToString(), true);
                }
                
                //Method.Invoke("RustExtended.Bootstrap.Initialize");
            }
			catch (Exception ex)
			{
				ConsoleSystem.LogError(ex.Message.ToString());
				Thread.Sleep(5000);
				Process.GetCurrentProcess().Kill();
			}
		}
        /*
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
        */
        public static string getdns(string hostname)
        {
            try
            {
                IPAddress ip;
                if (IPAddress.TryParse(hostname, out ip))
                    return ip.ToString();
                else
                    return Dns.GetHostEntry(hostname).AddressList[0].ToString();
            }
            catch (Exception)
            {
                throw new Exception("IP Address Error");
            }
        }
        public static string GetUrlHtml(string url)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse hwrs = (HttpWebResponse)hwr.GetResponse();
            Stream stream = hwrs.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(hwrs.CharacterSet));
            string html = sr.ReadToEnd();
            sr.Close();
            return html;
        }
		public static string GetExternalIP()
		{
			string[] array = Loader.list_0.ToArray();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				try
				{
					Loader.Log("Connecting to authentication server # www.wghostk.com", true);
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
					return matchCollection[0].Value.Trim();
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
			return string.Empty;
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
