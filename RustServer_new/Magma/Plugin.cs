using Magma.Events;
using RustExtended;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Magma
{
	public class Plugin
	{
		private string code;

		private ArrayList commands;

		private string path;

		private List<TimedEvent> timers;

		public string Code
		{
			get
			{
				return this.code;
			}
			set
			{
				this.code = value;
			}
		}

		public ArrayList Commands
		{
			get
			{
				return this.commands;
			}
			set
			{
				this.commands = value;
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}

		public Plugin(string path)
		{
			this.Path = path;
			this.timers = new List<TimedEvent>();
		}

		public bool CreateDir(string name)
		{
			bool result;
			if (name.Contains(".."))
			{
				result = false;
			}
			else
			{
				string str = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
				string text = Data.PATH + str + "\\" + name;
				if (Directory.Exists(text))
				{
					result = false;
				}
				else
				{
					Directory.CreateDirectory(text);
					result = true;
				}
			}
			return result;
		}

		public IniParser CreateIni(string name)
		{
			IniParser result;
			try
			{
				IniParser iniParser;
				if (name.Contains(".."))
				{
					iniParser = null;
					result = iniParser;
					return result;
				}
				string text = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
				string iniPath = string.Concat(new string[]
				{
					Data.PATH,
					text,
					"\\",
					name,
					".ini"
				});
				File.WriteAllText(iniPath, "");
				iniParser = new IniParser(iniPath);
				result = iniParser;
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			result = null;
			return result;
		}

		public TimedEvent CreateTimer(string name, int timeoutDelay)
		{
			TimedEvent timedEvent = this.GetTimer(name);
			TimedEvent result;
			if (timedEvent == null)
			{
				timedEvent = new TimedEvent(name, (double)timeoutDelay);
				timedEvent.OnFire += new TimedEvent.TimedEventFireDelegate(this.OnTimerCB);
				this.timers.Add(timedEvent);
				result = timedEvent;
			}
			else
			{
				result = timedEvent;
			}
			return result;
		}

		public TimedEvent CreateTimer(string name, int timeoutDelay, ParamsList args)
		{
			TimedEvent timedEvent = this.CreateTimer(name, timeoutDelay);
			timedEvent.Args = args;
			timedEvent.OnFire -= new TimedEvent.TimedEventFireDelegate(this.OnTimerCB);
			timedEvent.OnFireArgs += new TimedEvent.TimedEventFireArgsDelegate(this.OnTimerCBArgs);
			return timedEvent;
		}

		public void DeleteLog(string file)
		{
			string text = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
			string text2 = string.Concat(new string[]
			{
				Data.PATH,
				text,
				"\\",
				file,
				".ini"
			});
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
		}

		public string GetDate()
		{
			return DateTime.Now.ToShortDateString();
		}

		public IniParser GetIni(string name)
		{
			IniParser result;
			if (!name.Contains(".."))
			{
				string text = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
				string iniPath = string.Concat(new string[]
				{
					Data.PATH,
					text,
					"\\",
					name,
					".ini"
				});
				if (File.Exists(iniPath))
				{
					result = new IniParser(iniPath);
					return result;
				}
			}
			result = null;
			return result;
		}

		public List<IniParser> GetInis(string name)
		{
			string str = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
			string text = Data.PATH + str + "\\" + name;
			List<IniParser> list = new List<IniParser>();
			string[] files = Directory.GetFiles(text);
			for (int i = 0; i < files.Length; i++)
			{
				string iniPath = files[i];
				list.Add(new IniParser(iniPath));
			}
			return list;
		}

		public int GetTicks()
		{
			return Environment.TickCount;
		}

		public string GetTime()
		{
			return DateTime.Now.ToShortTimeString();
		}

		public TimedEvent GetTimer(string name)
		{
			TimedEvent result;
			foreach (TimedEvent current in this.timers)
			{
				if (current.Name == name)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public long GetTimestamp()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}

		public bool IniExists(string name)
		{
			string text = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
			return File.Exists(string.Concat(new string[]
			{
				Data.PATH,
				text,
				"\\",
				name,
				".ini"
			}));
		}

		private void Invoke(string name, params object[] obj)
		{
			try
			{
				PluginEngine.GetPluginEngine().Interpreter.Run(this.Code);
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Server", Server.GetServer());
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Data", Data.GetData());
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("DataStore", DataStore.GetInstance());
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Util", Util.GetUtil());
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Web", new Web());
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Time", this);
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("World", World.GetWorld());
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Plugin", this);
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("MySQL", typeof(MySQL));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("MySQLRow", typeof(MySQL.Row));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("MySQLField", typeof(MySQL.Field));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("MySQLResult", typeof(MySQL.Result));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("MySQLRecord", typeof(MySQL.Record));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Helper", typeof(Helper));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("IniFile", typeof(IniFile));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("UserFlags", typeof(UserFlags));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("UserData", typeof(UserData));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("UserBanned", typeof(UserBanned));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Countdown", typeof(Countdown));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Banned", typeof(Banned));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Users", typeof(Users));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Blocklist", typeof(Blocklist));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("UserEconomy", typeof(UserEconomy));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Economy", typeof(Economy));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("ClanData", typeof(ClanData));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("ClanLevel", typeof(ClanLevel));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("ClanMemberFlags", typeof(ClanMemberFlags));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Clans", typeof(Clans));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("ClanFlags", typeof(ClanFlags));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("WorldZone", typeof(WorldZone));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("ZoneFlags", typeof(ZoneFlags));
				PluginEngine.GetPluginEngine().Interpreter.SetParameter("Zones", typeof(Zones));
				if (obj != null)
				{
					PluginEngine.GetPluginEngine().Interpreter.CallFunction(name, obj);
				}
				else
				{
					PluginEngine.GetPluginEngine().Interpreter.CallFunction(name, new object[0]);
				}
			}
			catch (Exception ex)
			{
				Console.Write(string.Concat(new string[]
				{
					"Error invoking function : ",
					name,
					"\nFrom : ",
					this.path,
					"\n\n",
					ex.ToString()
				}));
			}
		}

		public void KillTimer(string name)
		{
			TimedEvent timer = this.GetTimer(name);
			if (timer != null)
			{
				timer.Stop();
				this.timers.Remove(timer);
			}
		}

		public void KillTimers()
		{
			foreach (TimedEvent current in this.timers)
			{
				current.Stop();
			}
			this.timers.Clear();
		}

		public void Log(string file, string text)
		{
			string text2 = System.IO.Path.GetFileName(this.Path).Replace(".js", "");
			File.AppendAllText(string.Concat(new string[]
			{
				Data.PATH,
				text2,
				"\\",
				file,
				".log"
			}), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + ": " + text + "\r\n");
		}

		public void OnBlueprintUse(Player p, BPUseEvent ae)
		{
			this.Invoke("On_BlueprintUse", new object[]
			{
				p,
				ae
			});
		}

		public void OnChat(Player player, ref ChatString text)
		{
			this.Invoke("On_Chat", new object[]
			{
				player,
				text
			});
		}

		public void OnCommand(Player player, string command, string[] args)
		{
			this.Invoke("On_Command", new object[]
			{
				player,
				command,
				args
			});
		}

		public void OnConsole(ref ConsoleSystem.Arg arg, bool external)
		{
			if (!external)
			{
				this.Invoke("On_Console", new object[]
				{
					Player.FindByPlayerClient(arg.argUser.playerClient),
					arg
				});
			}
			else
			{
				this.Invoke("On_Console", new object[]
				{
					null,
					arg
				});
			}
		}

		public void OnDoorUse(Player p, DoorEvent de)
		{
			this.Invoke("On_DoorUse", new object[]
			{
				p,
				de
			});
		}

		public void OnEntityDecay(DecayEvent de)
		{
			this.Invoke("On_EntityDecay", new object[]
			{
				de
			});
		}

		public void OnEntityDeployed(Player p, Entity e)
		{
			this.Invoke("On_EntityDeployed", new object[]
			{
				p,
				e
			});
		}

		public void OnEntityHurt(HurtEvent he)
		{
			this.Invoke("On_EntityHurt", new object[]
			{
				he
			});
		}

		public void OnItemsLoaded(ItemsBlocks items)
		{
			this.Invoke("On_ItemsLoaded", new object[]
			{
				items
			});
		}

		public void OnNPCHurt(HurtEvent he)
		{
			this.Invoke("On_NPCHurt", new object[]
			{
				he
			});
		}

		public void OnNPCKilled(DeathEvent de)
		{
			this.Invoke("On_NPCKilled", new object[]
			{
				de
			});
		}

		public void OnPlayerConnected(Player player)
		{
			this.Invoke("On_PlayerConnected", new object[]
			{
				player
			});
		}

		public void OnPlayerDisconnected(Player player)
		{
			this.Invoke("On_PlayerDisconnected", new object[]
			{
				player
			});
		}

		public void OnPlayerGathering(Player p, GatherEvent ge)
		{
			this.Invoke("On_PlayerGathering", new object[]
			{
				p,
				ge
			});
		}

		public void OnPlayerHurt(HurtEvent he)
		{
			this.Invoke("On_PlayerHurt", new object[]
			{
				he
			});
		}

		public void OnPlayerKilled(DeathEvent de)
		{
			this.Invoke("On_PlayerKilled", new object[]
			{
				de
			});
		}

		public void OnPlayerSpawn(Player p, SpawnEvent se)
		{
			this.Invoke("On_PlayerSpawning", new object[]
			{
				p,
				se
			});
		}

		public void OnPlayerSpawned(Player p, SpawnEvent se)
		{
			this.Invoke("On_PlayerSpawned", new object[]
			{
				p,
				se
			});
		}

		public void OnPluginInit()
		{
			this.Invoke("On_PluginInit", new object[0]);
		}

		public void OnServerInit()
		{
			this.Invoke("On_ServerInit", new object[0]);
		}

		public void OnServerShutdown()
		{
			this.Invoke("On_ServerShutdown", new object[0]);
		}

		public void OnTablesLoaded(Dictionary<string, LootSpawnList> lists)
		{
			this.Invoke("On_TablesLoaded", new object[]
			{
				lists
			});
		}

		public void OnTimerCB(string name)
		{
			if (this.Code.Contains(name + "Callback"))
			{
				this.Invoke(name + "Callback", new object[0]);
			}
		}

		public void OnTimerCBArgs(string name, ParamsList args)
		{
			if (this.Code.Contains(name + "Callback"))
			{
				this.Invoke(name + "Callback", new object[]
				{
					args
				});
			}
		}
	}
}
