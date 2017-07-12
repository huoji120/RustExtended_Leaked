using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class IniParser
{
	private struct SectionPair
	{
		public string Section;

		public string Key;
	}

	private string iniFilePath;

	private Hashtable keyPairs = new Hashtable();

	public string Name;

	private List<IniParser.SectionPair> tmpList = new List<IniParser.SectionPair>();

	public IniParser(string iniPath)
	{
		TextReader textReader = null;
		string text = null;
		this.iniFilePath = iniPath;
		this.Name = Path.GetFileNameWithoutExtension(iniPath);
		if (File.Exists(iniPath))
		{
			try
			{
				try
				{
					textReader = new StreamReader(iniPath);
					for (string text2 = textReader.ReadLine(); text2 != null; text2 = textReader.ReadLine())
					{
						text2 = text2.Trim();
						if (text2 != "")
						{
							if (text2.StartsWith("[") && text2.EndsWith("]"))
							{
								text = text2.Substring(1, text2.Length - 2);
							}
							else
							{
								string[] array = text2.Split(new char[]
								{
									'='
								}, 2);
								string value = null;
								if (text == null)
								{
									text = "ROOT";
								}
								IniParser.SectionPair sectionPair;
								sectionPair.Section = text;
								sectionPair.Key = array[0];
								if (array.Length > 1)
								{
									value = array[1];
								}
								this.keyPairs.Add(sectionPair, value);
								this.tmpList.Add(sectionPair);
							}
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return;
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
		}
		FileStream fileStream = new FileStream(iniPath, FileMode.Create);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.Write("");
		streamWriter.Close();
		fileStream.Close();
		throw new FileNotFoundException("Added " + iniPath);
	}

	public void AddSetting(string sectionName, string settingName)
	{
		this.AddSetting(sectionName, settingName, null);
	}

	public void AddSetting(string sectionName, string settingName, string settingValue)
	{
		IniParser.SectionPair sectionPair;
		sectionPair.Section = sectionName;
		sectionPair.Key = settingName;
		if (this.keyPairs.ContainsKey(sectionPair))
		{
			this.keyPairs.Remove(sectionPair);
		}
		if (this.tmpList.Contains(sectionPair))
		{
			this.tmpList.Remove(sectionPair);
		}
		this.keyPairs.Add(sectionPair, settingValue);
		this.tmpList.Add(sectionPair);
	}

	public int Count()
	{
		List<string> list = new List<string>();
		foreach (IniParser.SectionPair current in this.tmpList)
		{
			if (!list.Contains(current.Section))
			{
				list.Add(current.Section);
			}
		}
		return list.Count;
	}

	public void DeleteSetting(string sectionName, string settingName)
	{
		IniParser.SectionPair sectionPair;
		sectionPair.Section = sectionName;
		sectionPair.Key = settingName;
		if (this.keyPairs.ContainsKey(sectionPair))
		{
			this.keyPairs.Remove(sectionPair);
			this.tmpList.Remove(sectionPair);
		}
	}

	public string[] EnumSection(string sectionName)
	{
		List<string> list = new List<string>();
		foreach (IniParser.SectionPair current in this.tmpList)
		{
			if (current.Section == sectionName)
			{
				list.Add(current.Key);
			}
		}
		return list.ToArray();
	}

	public string GetSetting(string sectionName, string settingName)
	{
		IniParser.SectionPair sectionPair;
		sectionPair.Section = sectionName;
		sectionPair.Key = settingName;
		return (string)this.keyPairs[sectionPair];
	}

	public bool isCommandOn(string cmdName)
	{
		string setting = this.GetSetting("Commands", cmdName);
		return setting == null || setting == "true";
	}

	public void Save()
	{
		this.SaveSettings(this.iniFilePath);
	}

	public void SaveSettings(string newFilePath)
	{
		ArrayList arrayList = new ArrayList();
		string text = "";
		foreach (IniParser.SectionPair current in this.tmpList)
		{
			if (!arrayList.Contains(current.Section))
			{
				arrayList.Add(current.Section);
			}
		}
		foreach (string text2 in arrayList)
		{
			text = text + "[" + text2 + "]\r\n";
			foreach (IniParser.SectionPair current2 in this.tmpList)
			{
				if (current2.Section == text2)
				{
					string text3 = (string)this.keyPairs[current2];
					if (text3 != null)
					{
						text3 = "=" + text3;
					}
					text = text + current2.Key + text3 + "\r\n";
				}
			}
			text += "\r\n";
		}
		try
		{
			TextWriter textWriter = new StreamWriter(newFilePath);
			textWriter.Write(text);
			textWriter.Close();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void SetSetting(string sectionName, string settingName, string value)
	{
		IniParser.SectionPair sectionPair;
		sectionPair.Section = sectionName;
		sectionPair.Key = settingName;
		if (this.keyPairs.ContainsKey(sectionPair))
		{
			this.keyPairs[sectionPair] = value;
		}
	}
}
