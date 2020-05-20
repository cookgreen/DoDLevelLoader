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
		private const string DOD_LEVEL_DIR = "base/level/";
		public frmMain()
		{
			InitializeComponent();
			RefreshLevels();
		}

		private void mnuChangeDoDPath_Click(object sender, EventArgs e)
		{
			frmPathSelector pathSelectorWin = new frmPathSelector();
			if (pathSelectorWin.ShowDialog() == DialogResult.OK)
			{
				DoDSetting.DoDPath = pathSelectorWin.SelectedPath;
				DoDSetting.SaveTo(Path.Combine(Environment.CurrentDirectory, "setting.ini"));
				RefreshLevels();
			}
		}

		private void RefreshLevels()
		{
			levelList.Items.Clear();
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
						ListViewItem lvi = new ListViewItem();
						lvi.Text = subDi.Name;
						lvi.SubItems.Add(DOD_LEVEL_DIR + subDi.Name + "/" + foundedHabFiles.First().Name);
						levelList.Items.Add(lvi);
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
				var lvi = levelList.SelectedItems[levelList.SelectedIndices[0]];
				string levelRelativePath = lvi.SubItems[0].Text;

				Process dodProcess = new Process();
				dodProcess.StartInfo.FileName = DoDSetting.DoDPath;
				dodProcess.StartInfo.Arguments = levelRelativePath;
				dodProcess.Start();

				Hide();
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
