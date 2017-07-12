namespace Magma
{
    using Facepunch;
    using Facepunch.Utility;
    using RustExtended;
    using System;
    using System.IO;
    using UnityEngine;

    public class Bootstrap : Facepunch.MonoBehaviour
    {
        public static string Version = "1.1.5";

        public void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        public static void Start()
        {
            Helper.Log("Initialize Magma Plugins", true);
            string path = Path.Combine(Core.SavePath, @"\cfg\Magma\config.cfg");
            if (!File.Exists(path))
            {
                Magma.Data.PATH = Path.Combine(Core.SavePath, @"plugins\");
            }
            else
            {
                Magma.Data.PATH = new IniParser(path).GetSetting("Settings", "Directory");
            }
            Magma.Data.PATH = CommandLine.GetSwitch("-plugindir", Magma.Data.PATH);
            if (!Directory.Exists(Magma.Data.PATH))
            {
                Directory.CreateDirectory(Magma.Data.PATH);
            }
            PluginEngine.GetPluginEngine();
            Magma.Hooks.ServerStarted();
            Helper.Log("Loaded " + PluginEngine.GetPluginEngine().Plugins.Count + " Plugin(s) Total.", true);
        }
    }
}

