using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DoDLevelLoader.Models
{
    public class DoDSetting
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string DoDPath { get; set; }

        public static void SaveTo(string settingINIPath)
        {
            WritePrivateProfileString("General", "DoDPath", DoDPath, settingINIPath);
        }

        public static void Read(string settingINIPath)
        {
            StringBuilder builder = new StringBuilder();
            GetPrivateProfileString("General", "DoDPath", null, builder, 1024, settingINIPath);
            DoDPath = builder.ToString();
        }
    }
}
