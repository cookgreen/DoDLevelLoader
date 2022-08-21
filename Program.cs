using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoDLevelLoader.Models;
using DoDLevelLoader.Forms;

namespace DoDLevelLoader
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DoDSetting.Read(Path.Combine(Environment.CurrentDirectory, "setting.ini"));
            frmDoDLoader dodLoaderWin = new frmDoDLoader();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(dodLoaderWin);
        }
    }
}
