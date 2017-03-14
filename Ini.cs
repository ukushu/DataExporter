using System.Runtime.InteropServices;
using System.Text;

namespace BotAgent.DataExporter
{
    public class Ini
    {
        public string Path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        
        public Ini(string iniPath)
        {
            Path = iniPath;
        }

        /// <summary>
        /// Works with "Default" section
        /// </summary>
        public void Write(string key, string value)
        {
            WritePrivateProfileString("Default", key, value, Path);
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="section"></PARAM>
        /// section name
        /// <PARAM name="key"></PARAM>
        /// key Name
        /// <PARAM name="value"></PARAM>
        /// value Name
        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        /// <summary>
        /// Works with "Default" section
        /// </summary>
        public string Read(string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString("Default", key, "", temp, 255, Path);
            return temp.ToString();
        }

        /// <summary>
        /// Read Data value From the Ini File
        /// </summary>
        /// <PARAM name="section"></PARAM>
        /// <PARAM name="key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string Read(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, Path);
            return temp.ToString();
        }
    }
}