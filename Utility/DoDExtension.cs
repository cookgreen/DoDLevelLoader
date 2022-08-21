using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDLevelLoader
{
    public class DoDExtension
    {
        public static bool CheckDoDPath(string dodExePath)
        {
            if (string.IsNullOrEmpty(dodExePath) || !File.Exists(dodExePath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
