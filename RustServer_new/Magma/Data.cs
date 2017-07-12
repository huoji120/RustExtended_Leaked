using Facepunch.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Magma
{
	public class Data
	{
		public List<string> chat_history = new List<string>();

		public List<string> chat_history_username = new List<string>();

		private static Data data;

		public static Hashtable inifiles = new Hashtable();

		public Hashtable magma_shared_data = new Hashtable();

		public static string PATH;

		[Obsolete("Replaced with DataStore.Add", false)]
		public void AddTableValue(string tablename, object key, object val)
		{
			Hashtable hashtable = (Hashtable)DataStore.GetInstance().datastore[tablename];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				DataStore.GetInstance().datastore.Add(tablename, hashtable);
			}
			if (hashtable.ContainsKey(key))
			{
				hashtable[key] = val;
			}
			else
			{
				hashtable.Add(key, val);
			}
		}

		public string GetConfigValue(string config, string section, string key)
		{
			IniParser iniParser = (IniParser)Data.inifiles[config.ToLower()];
			string result;
			if (iniParser == null)
			{
				result = "Config does not exist";
			}
			else
			{
				result = iniParser.GetSetting(section, key);
			}
			return result;
		}

		public static Data GetData()
		{
			if (Data.data == null)
			{
				Data.data = new Data();
			}
			return Data.data;
		}

		public IniParser GetRPPConfig()
		{
			IniParser result;
			if (Data.inifiles.ContainsKey("rust++"))
			{
				result = (IniParser)Data.inifiles["rust++"];
			}
			else
			{
				result = null;
			}
			return result;
		}

		[Obsolete("Replaced with DataStore.Get", false)]
		public object GetTableValue(string tablename, object key)
		{
			Hashtable hashtable = (Hashtable)DataStore.GetInstance().datastore[tablename];
			object result;
			if (hashtable == null)
			{
				result = null;
			}
			else
			{
				result = hashtable[key];
			}
			return result;
		}

		public void Init()
		{
			this.Load();
		}

		public void Load()
		{
			Data.inifiles.Clear();
			string[] directories = Directory.GetDirectories(Data.PATH);
			for (int i = 0; i < directories.Length; i++)
			{
				string path = directories[i];
				string text = "";
				string[] files = Directory.GetFiles(path);
				for (int j = 0; j < files.Length; j++)
				{
					string text2 = files[j];
					if (Path.GetFileName(text2).Contains(".cfg") && Path.GetFileName(text2).Contains(Path.GetFileName(path)))
					{
						text = text2;
					}
				}
				if (text != "")
				{
					string text3 = Path.GetFileName(text).Replace(".cfg", "").ToLower();
					Data.inifiles.Add(text3, new IniParser(text));
					Console.WriteLine("Loaded Config: " + text3);
				}
			}
		}

		public void OverrideConfig(string config, string section, string key, string value)
		{
			IniParser iniParser = (IniParser)Data.inifiles[config.ToLower()];
			if (iniParser != null)
			{
				iniParser.SetSetting(section, key, value);
			}
		}

		public string[] SplitQuoteStrings(string str)
		{
			return Facepunch.Utility.String.SplitQuotesStrings(str);
		}

		public int StrLen(string str)
		{
			return str.Length;
		}

		public string Substring(string str, int from, int to)
		{
			return str.Substring(from, to);
		}

		public int ToInt(string num)
		{
			return int.Parse(num);
		}

		public string ToLower(string str)
		{
			return str.ToLower();
		}

		public string ToUpper(string str)
		{
			return str.ToUpper();
		}
	}
}
