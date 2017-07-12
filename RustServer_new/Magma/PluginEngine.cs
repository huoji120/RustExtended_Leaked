using Jint;
using Jint.Expressions;
using RustExtended;
using System;
using System.Collections;
using System.IO;

namespace Magma
{
	public class PluginEngine
	{
		private string[] filters = new string[]
		{
			"system.io",
			"system.xml"
		};

		private JintEngine interpreter = new JintEngine();

		private static PluginEngine PE;

		private ArrayList plugins = new ArrayList();

		public JintEngine Interpreter
		{
			get
			{
				return this.interpreter;
			}
			set
			{
				this.interpreter = value;
			}
		}

		public ArrayList Plugins
		{
			get
			{
				return this.plugins;
			}
			set
			{
				this.plugins = value;
			}
		}

		private PluginEngine()
		{
		}

		public bool FilterPlugin(string script)
		{
			string text = script.ToLower();
			string[] array = this.filters;
			bool result;
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				if (text.Contains(text2))
				{
					Console.WriteLine("Script cannot contain: " + text2);
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}

		public static PluginEngine GetPluginEngine()
		{
			if (PluginEngine.PE == null)
			{
				PluginEngine.PE = new PluginEngine();
				PluginEngine.PE.Init();
			}
			return PluginEngine.PE;
		}

		public void Init()
		{
			this.ReloadPlugins(null);
		}

		public void LoadPlugins(Player p)
		{
			Hooks.ResetHooks();
			this.ParsePlugin();
			foreach (Plugin plugin in this.plugins)
			{
				try
				{
					this.interpreter.Run(plugin.Code);
					foreach (Statement current in JintEngine.Compile(plugin.Code, false).Statements)
					{
						if (current.GetType() == typeof(FunctionDeclarationStatement))
						{
							FunctionDeclarationStatement functionDeclarationStatement = (FunctionDeclarationStatement)current;
							if (functionDeclarationStatement != null)
							{
								if (functionDeclarationStatement.Name == "On_ServerInit")
								{
									Hooks.OnServerInit += new Hooks.ServerInitDelegate(plugin.OnServerInit);
								}
								else if (functionDeclarationStatement.Name == "On_PluginInit")
								{
									Hooks.OnPluginInit += new Hooks.PluginInitHandlerDelegate(plugin.OnPluginInit);
								}
								else if (functionDeclarationStatement.Name == "On_ServerShutdown")
								{
									Hooks.OnServerShutdown += new Hooks.ServerShutdownDelegate(plugin.OnServerShutdown);
								}
								else if (functionDeclarationStatement.Name == "On_ItemsLoaded")
								{
									Hooks.OnItemsLoaded += new Hooks.ItemsDatablocksLoaded(plugin.OnItemsLoaded);
								}
								else if (functionDeclarationStatement.Name == "On_TablesLoaded")
								{
									Hooks.OnTablesLoaded += new Hooks.LootTablesLoaded(plugin.OnTablesLoaded);
								}
								else if (functionDeclarationStatement.Name == "On_Chat")
								{
									Hooks.OnChat += new Hooks.ChatHandlerDelegate(plugin.OnChat);
								}
								else if (functionDeclarationStatement.Name == "On_Console")
								{
									Hooks.OnConsoleReceived += new Hooks.ConsoleHandlerDelegate(plugin.OnConsole);
								}
								else if (functionDeclarationStatement.Name == "On_Command")
								{
									Hooks.OnCommand += new Hooks.CommandHandlerDelegate(plugin.OnCommand);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerConnected")
								{
									Hooks.OnPlayerConnected += new Hooks.ConnectionHandlerDelegate(plugin.OnPlayerConnected);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerDisconnected")
								{
									Hooks.OnPlayerDisconnected += new Hooks.DisconnectionHandlerDelegate(plugin.OnPlayerDisconnected);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerKilled")
								{
									Hooks.OnPlayerKilled += new Hooks.KillHandlerDelegate(plugin.OnPlayerKilled);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerHurt")
								{
									Hooks.OnPlayerHurt += new Hooks.HurtHandlerDelegate(plugin.OnPlayerHurt);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerSpawning")
								{
									Hooks.OnPlayerSpawning += new Hooks.PlayerSpawnHandlerDelegate(plugin.OnPlayerSpawn);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerSpawned")
								{
									Hooks.OnPlayerSpawned += new Hooks.PlayerSpawnHandlerDelegate(plugin.OnPlayerSpawned);
								}
								else if (functionDeclarationStatement.Name == "On_PlayerGathering")
								{
									Hooks.OnPlayerGathering += new Hooks.PlayerGatheringHandlerDelegate(plugin.OnPlayerGathering);
								}
								else if (functionDeclarationStatement.Name == "On_EntityHurt")
								{
									Hooks.OnEntityHurt += new Hooks.EntityHurtDelegate(plugin.OnEntityHurt);
								}
								else if (functionDeclarationStatement.Name == "On_EntityDecay")
								{
									Hooks.OnEntityDecay += new Hooks.EntityDecayDelegate(plugin.OnEntityDecay);
								}
								else if (functionDeclarationStatement.Name == "On_EntityDeployed")
								{
									Hooks.OnEntityDeployed += new Hooks.EntityDeployedDelegate(plugin.OnEntityDeployed);
								}
								else if (functionDeclarationStatement.Name == "On_NPCHurt")
								{
									Hooks.OnNPCHurt += new Hooks.HurtHandlerDelegate(plugin.OnNPCHurt);
								}
								else if (functionDeclarationStatement.Name == "On_NPCKilled")
								{
									Hooks.OnNPCKilled += new Hooks.KillHandlerDelegate(plugin.OnNPCKilled);
								}
								else if (functionDeclarationStatement.Name == "On_BlueprintUse")
								{
									Hooks.OnBlueprintUse += new Hooks.BlueprintUseHandlerDelagate(plugin.OnBlueprintUse);
								}
								else if (functionDeclarationStatement.Name == "On_DoorUse")
								{
									Hooks.OnDoorUse += new Hooks.DoorOpenHandlerDelegate(plugin.OnDoorUse);
								}
							}
						}
					}
				}
				catch (Exception)
				{
					string arg = "Can't load plugin : " + plugin.Path.Remove(0, plugin.Path.LastIndexOf("\\") + 1);
					if (p != null)
					{
						p.Message(arg);
					}
					else
					{
						Server.GetServer().Broadcast(arg);
					}
				}
			}
		}

		public void ParsePlugin()
		{
			this.plugins.Clear();
			string[] directories = Directory.GetDirectories(Util.GetMagmaFolder());
			for (int i = 0; i < directories.Length; i++)
			{
				string path = directories[i];
				string text = "";
				string[] files = Directory.GetFiles(path);
				for (int j = 0; j < files.Length; j++)
				{
					string text2 = files[j];
					if (Path.GetFileName(text2).Contains(".js") && Path.GetFileName(text2).Contains(Path.GetFileName(path)))
					{
						text = text2;
					}
				}
				if (text != "")
				{
					string[] array = File.ReadAllLines(text);
					string text3 = "";
					string[] array2 = array;
					for (int k = 0; k < array2.Length; k++)
					{
						string text4 = array2[k];
						string text5 = text4.Replace("toLowerCase(", "Data.ToLower(").Replace("GetStaticField(", "Util.GetStaticField(").Replace("SetStaticField(", "Util.SetStaticField(").Replace("InvokeStatic(", "Util.InvokeStatic(").Replace("IsNull(", "Util.IsNull(").Replace("Datastore", "DataStore");
						try
						{
							if (text5.Contains("new "))
							{
								string[] array3 = text5.Split(new string[]
								{
									"new "
								}, StringSplitOptions.None);
								if ((array3[0].Contains("\"") || array3[0].Contains("'")) && (array3[1].Contains("\"") || array3[1].Contains("'")))
								{
									text3 = text3 + text5 + "\r\n";
									goto IL_3C6;
								}
								if (text5.Contains("];"))
								{
									string text6 = text5.Split(new string[]
									{
										"new "
									}, StringSplitOptions.None)[1].Split(new string[]
									{
										"];"
									}, StringSplitOptions.None)[0];
									text5 = text5.Replace("new " + text6, "").Replace("];", "");
									string text7 = text6.Split(new char[]
									{
										'['
									})[1];
									text6 = text6.Split(new char[]
									{
										'['
									})[0];
									string text8 = text5;
									text5 = string.Concat(new string[]
									{
										text8,
										"Util.CreateArrayInstance('",
										text6,
										"', ",
										text7,
										");"
									});
								}
								else
								{
									string text9 = text5.Split(new string[]
									{
										"new "
									}, StringSplitOptions.None)[1].Split(new string[]
									{
										");"
									}, StringSplitOptions.None)[0];
									text5 = text5.Replace("new " + text9, "").Replace(");", "");
									string text10 = text9.Split(new char[]
									{
										'('
									})[1];
									text9 = text9.Split(new char[]
									{
										'('
									})[0];
									text5 = text5 + "Util.CreateInstance('" + text9 + "'";
									if (text10 != "")
									{
										text5 = text5 + ", " + text10;
									}
									text5 += ");";
								}
							}
							text3 = text3 + text5 + "\r\n";
						}
						catch (Exception)
						{
							Helper.LogError("Magma : Couln't create instance at line -> " + text4, true);
						}
						IL_3C6:;
					}
					if (this.FilterPlugin(text3))
					{
						Plugin value = new Plugin(text)
						{
							Code = text3
						};
						this.plugins.Add(value);
					}
					else
					{
						Helper.LogError("PERMISSION DENIED. Failed to load " + text + " due to restrictions on the API", true);
					}
				}
			}
		}

		public void ReloadPlugins(Player p)
		{
			this.Secure();
			foreach (Plugin plugin in this.plugins)
			{
				plugin.KillTimers();
			}
			this.LoadPlugins(p);
			Data.GetData().Load();
			Hooks.PluginInit();
		}

		public void Secure()
		{
			this.interpreter.AllowClr(true);
		}
	}
}
