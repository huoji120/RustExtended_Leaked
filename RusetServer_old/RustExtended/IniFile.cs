namespace RustExtended
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class IniFile
    {
        private string string_0;

        public IniFile(string iniFile)
        {
            this.string_0 = iniFile;
        }

        public void Delete(string section)
        {
            WritePrivateProfileString(section, null, null, this.string_0);
        }

        public void Delete(string section, string key)
        {
            WritePrivateProfileString(section, key, null, this.string_0);
        }

        [DllImport("KERNEL32.DLL")]
        private static extern int GetPrivateProfileString(string string_1, string string_2, string string_3, StringBuilder stringBuilder_0, int int_0, string string_4);
        public string Read(string section, string key)
        {
            StringBuilder builder = new StringBuilder(0xff);
            GetPrivateProfileString(section, key, "", builder, 0xff, this.string_0);
            return builder.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.string_0);
        }

        [DllImport("KERNEL32.DLL")]
        private static extern long WritePrivateProfileString(string string_1, string string_2, string string_3, string string_4);
    }
}

