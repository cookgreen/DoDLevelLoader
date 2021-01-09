using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoDLevelLoader
{
    public partial class frmMain : Form
    {
        private List<DoDLevel> Levels;
        private const string DOD_LEVEL_DIR = "base/level/";
        private BackgroundWorker worker;
        private delegate void ListViewAddItemDelegate(ListView lsv, ListViewItem item);
        private delegate void ListViewAddImageDelegate(ListView lsv, Image image);
        private ListViewAddItemDelegate listViewAddItemDelegate;
        private ListViewAddImageDelegate listViewAddImageDelegate;

        public frmMain()
        {
            InitializeComponent();

            listViewAddItemDelegate = new ListViewAddItemDelegate(ListViewAddItemMethod);
            listViewAddImageDelegate = new ListViewAddImageDelegate(ListViewAddImageMethod);

            RefreshLevels();
        }

        private void ListViewAddItemMethod(ListView lsv, ListViewItem item)
        {
            if (lsv.InvokeRequired)
            {
                lsv.Invoke(listViewAddItemDelegate, lsv, item);
            }
            else
            {
                lsv.Items.Add(item);
            }
        }

        private void ListViewAddImageMethod(ListView lsv, Image image)
        {
            if (lsv.InvokeRequired)
            {
                lsv.Invoke(listViewAddImageDelegate, lsv, image);
            }
            else
            {
                lsv.LargeImageList.Images.Add(image);
            }
        }


        private void mnuChangeDoDPath_Click(object sender, EventArgs e)
        {
            frmPathSelector pathSelectorWin = new frmPathSelector();
            if (pathSelectorWin.ShowDialog() == DialogResult.OK)
            {
                DoDSetting.DoDPath = pathSelectorWin.SelectedPath;
                DoDSetting.SaveTo(Path.Combine(Environment.CurrentDirectory, "setting.ini").Replace("\\", "/"));
                RefreshLevels();
            }
        }

        private void RefreshLevels()
        {
            levelList.Items.Clear();

            levelList.LargeImageList = new ImageList();
            levelList.LargeImageList.ImageSize = new Size(128, 72);
            btnOK.Enabled = false;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Levels = new List<DoDLevel>();
            string dodExePath = DoDSetting.DoDPath;

            if (string.IsNullOrEmpty(dodExePath))
            {
                return;
            }

            FileInfo fi = new FileInfo(dodExePath);
            var dodDir = fi.Directory;
            var dodLevelDir = Path.Combine(dodDir.FullName, DOD_LEVEL_DIR);

            if (Directory.Exists(dodLevelDir))
            {
                DirectoryInfo di = new DirectoryInfo(dodLevelDir);
                foreach (var subDi in di.EnumerateDirectories())
                {
                    var foundedHabFiles = subDi.EnumerateFiles().Where(o => o.Extension == ".hab");
                    if (foundedHabFiles.Count() > 0)
                    {
                        string habFile = DOD_LEVEL_DIR + subDi.Name + "/" + foundedHabFiles.First().Name;
                        string brefingImageFullPath = dodLevelDir + "/" + subDi.Name;
                        DoDLevel level = new DoDLevel(subDi.Name, habFile, Path.Combine(dodDir.FullName, DOD_LEVEL_DIR), brefingImageFullPath);
                        Levels.Add(level);

                        ListViewAddImageMethod(levelList, level.BrefingImage);

                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = levelList.LargeImageList.Images.Count - 1;

                        lvi.Text = subDi.Name;
                        lvi.SubItems.Add(DOD_LEVEL_DIR + subDi.Name + "/" + foundedHabFiles.First().Name);

                        ListViewAddItemMethod(levelList, lvi);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (File.Exists(DoDSetting.DoDPath) &&
                !string.IsNullOrEmpty(DoDSetting.DoDPath) &&
                (new FileInfo(DoDSetting.DoDPath)).Name == "darkestofdays.exe" &&
                levelList.SelectedIndices.Count == 1)
            {
                var lvi = levelList.SelectedItems[0];
                string levelRelativePath = lvi.SubItems[1].Text;

                Process dodProcess = new Process();
                dodProcess.StartInfo.FileName = DoDSetting.DoDPath;
                dodProcess.StartInfo.Arguments = levelRelativePath;
                dodProcess.Start();
            }
            else
            {
                if (!File.Exists(DoDSetting.DoDPath) ||
                    string.IsNullOrEmpty(DoDSetting.DoDPath) ||
                   (new FileInfo(DoDSetting.DoDPath)).Name != "darkestofdays.exe")
                {
                    mnuChangeDoDPath_Click(sender, e);
                }
                else if (levelList.SelectedItems.Count == 0)
                {
                    MessageBox.Show(
                        "Please select a level to start!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void levelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (levelList.SelectedIndices.Count > 0)
            {
                btnOK.Enabled = true;
            }
        }
    }
}
