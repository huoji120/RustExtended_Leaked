using Facepunch;
using Facepunch.Utility;
using RustExtended;
using System;
using System.IO;
using UnityEngine;

namespace Magma
{
	public class Bootstrap : Facepunch.MonoBehaviour
	{
		public static string Version = "1.1.5";

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void Start()
		{
			Helper.Log("Initialize Magma Plugins", true);
			string text = Path.Combine(Core.SavePath, "\\cfg\\Magma\\config.cfg");
			if (!File.Exists(text))
			{
				Data.PATH = Path.Combine(Core.SavePath, "plugins\\");
			}
			else
			{
				Data.PATH = new IniParser(text).GetSetting("Settings", "Directory");
			}
			Data.PATH = CommandLine.GetSwitch("-plugindir", Data.PATH);
			if (!Directory.Exists(Data.PATH))
			{
				Directory.CreateDirectory(Data.PATH);
			}
			PluginEngine.GetPluginEngine();
			Hooks.ServerStarted();
			Helper.Log("Loaded " + PluginEngine.GetPluginEngine().Plugins.Count + " Plugin(s) Total.", true);
		}
	}
}
