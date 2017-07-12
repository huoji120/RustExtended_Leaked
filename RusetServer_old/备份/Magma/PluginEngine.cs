namespace Magma
{
    using Jint;
    using Jint.Expressions;
    using RustExtended;
    using System;
    using System.Collections;
    using System.IO;

    public class PluginEngine
    {
        private string[] filters = new string[] { "system.io", "system.xml" };
        private JintEngine interpreter = new JintEngine();
        private static PluginEngine PE;
        private ArrayList plugins = new ArrayList();

        private PluginEngine()
        {
        }

        public bool FilterPlugin(string script)
        {
            string str = script.ToLower();
            foreach (string str2 in this.filters)
            {
                if (str.Contains(str2))
                {
                    Console.WriteLine("Script cannot contain: " + str2);
                    return false;
                }
            }
            return true;
        }

        public static PluginEngine GetPluginEngine()
        {
            if (PE == null)
            {
                PE = new PluginEngine();
                PE.Init();
            }
            return PE;
        }

        public void Init()
        {
            this.ReloadPlugins(null);
        }

        public void LoadPlugins(Player p)
        {
            Magma.Hooks.ResetHooks();
            this.ParsePlugin();
            foreach (Plugin plugin in this.plugins)
            {
                try
                {
                    this.interpreter.Run(plugin.Code);
                    foreach (Statement statement in JintEngine.Compile(plugin.Code, false).Statements)
                    {
                        if (statement.GetType() == typeof(FunctionDeclarationStatement))
                        {
                            FunctionDeclarationStatement statement2 = (FunctionDeclarationStatement) statement;
                            if (statement2 != null)
                            {
                                if (statement2.Name == "On_ServerInit")
                                {
                                    Magma.Hooks.OnServerInit += new Magma.Hooks.ServerInitDelegate(plugin.OnServerInit);
                                }
                                else if (statement2.Name == "On_PluginInit")
                                {
                                    Magma.Hooks.OnPluginInit += new Magma.Hooks.PluginInitHandlerDelegate(plugin.OnPluginInit);
                                }
                                else if (statement2.Name == "On_ServerShutdown")
                                {
                                    Magma.Hooks.OnServerShutdown += new Magma.Hooks.ServerShutdownDelegate(plugin.OnServerShutdown);
                                }
                                else if (statement2.Name == "On_ItemsLoaded")
                                {
                                    Magma.Hooks.OnItemsLoaded += new Magma.Hooks.ItemsDatablocksLoaded(plugin.OnItemsLoaded);
                                }
                                else if (statement2.Name == "On_TablesLoaded")
                                {
                                    Magma.Hooks.OnTablesLoaded += new Magma.Hooks.LootTablesLoaded(plugin.OnTablesLoaded);
                                }
                                else if (statement2.Name == "On_Chat")
                                {
                                    Magma.Hooks.OnChat += new Magma.Hooks.ChatHandlerDelegate(plugin.OnChat);
                                }
                                else if (statement2.Name == "On_Console")
                                {
                                    Magma.Hooks.OnConsoleReceived += new Magma.Hooks.ConsoleHandlerDelegate(plugin.OnConsole);
                                }
                                else if (statement2.Name == "On_Command")
                                {
                                    Magma.Hooks.OnCommand += new Magma.Hooks.CommandHandlerDelegate(plugin.OnCommand);
                                }
                                else if (statement2.Name == "On_PlayerConnected")
                                {
                                    Magma.Hooks.OnPlayerConnected += new Magma.Hooks.ConnectionHandlerDelegate(plugin.OnPlayerConnected);
                                }
                                else if (statement2.Name == "On_PlayerDisconnected")
                                {
                                    Magma.Hooks.OnPlayerDisconnected += new Magma.Hooks.DisconnectionHandlerDelegate(plugin.OnPlayerDisconnected);
                                }
                                else if (statement2.Name == "On_PlayerKilled")
                                {
                                    Magma.Hooks.OnPlayerKilled += new Magma.Hooks.KillHandlerDelegate(plugin.OnPlayerKilled);
                                }
                                else if (statement2.Name == "On_PlayerHurt")
                                {
                                    Magma.Hooks.OnPlayerHurt += new Magma.Hooks.HurtHandlerDelegate(plugin.OnPlayerHurt);
                                }
                                else if (statement2.Name == "On_PlayerSpawning")
                                {
                                    Magma.Hooks.OnPlayerSpawning += new Magma.Hooks.PlayerSpawnHandlerDelegate(plugin.OnPlayerSpawn);
                                }
                                else if (statement2.Name == "On_PlayerSpawned")
                                {
                                    Magma.Hooks.OnPlayerSpawned += new Magma.Hooks.PlayerSpawnHandlerDelegate(plugin.OnPlayerSpawned);
                                }
                                else if (statement2.Name == "On_PlayerGathering")
                                {
                                    Magma.Hooks.OnPlayerGathering += new Magma.Hooks.PlayerGatheringHandlerDelegate(plugin.OnPlayerGathering);
                                }
                                else if (statement2.Name == "On_EntityHurt")
                                {
                                    Magma.Hooks.OnEntityHurt += new Magma.Hooks.EntityHurtDelegate(plugin.OnEntityHurt);
                                }
                                else if (statement2.Name == "On_EntityDecay")
                                {
                                    Magma.Hooks.OnEntityDecay += new Magma.Hooks.EntityDecayDelegate(plugin.OnEntityDecay);
                                }
                                else if (statement2.Name == "On_EntityDeployed")
                                {
                                    Magma.Hooks.OnEntityDeployed += new Magma.Hooks.EntityDeployedDelegate(plugin.OnEntityDeployed);
                                }
                                else if (statement2.Name == "On_NPCHurt")
                                {
                                    Magma.Hooks.OnNPCHurt += new Magma.Hooks.HurtHandlerDelegate(plugin.OnNPCHurt);
                                }
                                else if (statement2.Name == "On_NPCKilled")
                                {
                                    Magma.Hooks.OnNPCKilled += new Magma.Hooks.KillHandlerDelegate(plugin.OnNPCKilled);
                                }
                                else if (statement2.Name == "On_BlueprintUse")
                                {
                                    Magma.Hooks.OnBlueprintUse += new Magma.Hooks.BlueprintUseHandlerDelagate(plugin.OnBlueprintUse);
                                }
                                else if (statement2.Name == "On_DoorUse")
                                {
                                    Magma.Hooks.OnDoorUse += new Magma.Hooks.DoorOpenHandlerDelegate(plugin.OnDoorUse);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    string arg = "Can't load plugin : " + plugin.Path.Remove(0, plugin.Path.LastIndexOf(@"\") + 1);
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
            int index = 0;
        Label_001A:
            if (index >= directories.Length)
            {
                return;
            }
            string path = directories[index];
            string str2 = "";
            foreach (string str3 in Directory.GetFiles(path))
            {
                if (Path.GetFileName(str3).Contains(".js") && Path.GetFileName(str3).Contains(Path.GetFileName(path)))
                {
                    str2 = str3;
                }
            }
            if (!(str2 != ""))
            {
                goto Label_03E0;
            }
            string[] strArray = File.ReadAllLines(str2);
            string script = "";
            string[] strArray5 = strArray;
            int num3 = 0;
        Label_009D:
            if (num3 < strArray5.Length)
            {
                string str5 = strArray5[num3];
                string str6 = str5.Replace("toLowerCase(", "Data.ToLower(").Replace("GetStaticField(", "Util.GetStaticField(").Replace("SetStaticField(", "Util.SetStaticField(").Replace("InvokeStatic(", "Util.InvokeStatic(").Replace("IsNull(", "Util.IsNull(").Replace("Datastore", "DataStore");
                try
                {
                    if (str6.Contains("new "))
                    {
                        string[] strArray2 = str6.Split(new string[] { "new " }, StringSplitOptions.None);
                        if ((!strArray2[0].Contains("\"") && !strArray2[0].Contains("'")) || (!strArray2[1].Contains("\"") && !strArray2[1].Contains("'")))
                        {
                            if (str6.Contains("];"))
                            {
                                string str7 = str6.Split(new string[] { "new " }, StringSplitOptions.None)[1].Split(new string[] { "];" }, StringSplitOptions.None)[0];
                                str6 = str6.Replace("new " + str7, "").Replace("];", "");
                                string str8 = str7.Split(new char[] { '[' })[1];
                                str7 = str7.Split(new char[] { '[' })[0];
                                string str9 = str6;
                                str6 = str9 + "Util.CreateArrayInstance('" + str7 + "', " + str8 + ");";
                            }
                            else
                            {
                                string str10 = str6.Split(new string[] { "new " }, StringSplitOptions.None)[1].Split(new string[] { ");" }, StringSplitOptions.None)[0];
                                str6 = str6.Replace("new " + str10, "").Replace(");", "");
                                string str11 = str10.Split(new char[] { '(' })[1];
                                str10 = str10.Split(new char[] { '(' })[0];
                                str6 = str6 + "Util.CreateInstance('" + str10 + "'";
                                if (str11 != "")
                                {
                                    str6 = str6 + ", " + str11;
                                }
                                str6 = str6 + ");";
                            }
                        }
                        else
                        {
                            script = script + str6 + "\r\n";
                            goto Label_03EB;
                        }
                    }
                    script = script + str6 + "\r\n";
                }
                catch (Exception)
                {
                    Helper.LogError("Magma : Couln't create instance at line -> " + str5, true);
                }
                goto Label_03EB;
            }
            if (this.FilterPlugin(script))
            {
                Helper.Log(" Plugin: " + str2, true);
                Plugin plugin = new Plugin(str2) {
                    Code = script
                };
                this.plugins.Add(plugin);
            }
            else
            {
                Helper.LogError("PERMISSION DENIED. Failed to load " + str2 + " due to restrictions on the API", true);
            }
        Label_03E0:
            index++;
            goto Label_001A;
        Label_03EB:
            num3++;
            goto Label_009D;
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
            Magma.Hooks.PluginInit();
        }

        public void Secure()
        {
            this.interpreter.AllowClr(true);
        }

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
    }
}

