using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RustExtended
{
	public class IniFile
	{
		private string string_0;

		[DllImport("KERNEL32.DLL")]
		private static extern long WritePrivateProfileString(string string_1, string string_2, string string_3, string string_4);

		[DllImport("KERNEL32.DLL")]
		private static extern int GetPrivateProfileString(string string_1, string string_2, string string_3, StringBuilder stringBuilder_0, int int_0, string string_4);

		public IniFile(string iniFile)
		{
			this.string_0 = iniFile;
		}

		public string Read(string section, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			IniFile.GetPrivateProfileString(section, key, "", stringBuilder, 255, this.string_0);
			return stringBuilder.ToString();
		}

		public void Write(string section, string key, string value)
		{
			IniFile.WritePrivateProfileString(section, key, value, this.string_0);
		}

		public void Delete(string section, string key)
		{
			IniFile.WritePrivateProfileString(section, key, null, this.string_0);
		}

		public void Delete(string section)
		{
			IniFile.WritePrivateProfileString(section, null, null, this.string_0);
		}
	}
}
