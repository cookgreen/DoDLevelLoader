﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoDLevelLoader
{
	public partial class frmPathSelector : Form
	{
		public string SelectedPath { get; set; }

		public frmPathSelector()
		{
			InitializeComponent();
			txtPath.Text = DoDSetting.DoDPath;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (File.Exists(txtPath.Text))
			{
				DialogResult = DialogResult.OK;
				Close();
			}
			else
			{
				DialogResult = DialogResult.Cancel;
				MessageBox.Show("Invalid DoD Path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnChoose_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Darkest of Days Execuable|darkestofdays.exe";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				SelectedPath = dialog.FileName;
				txtPath.Text = SelectedPath;
			}
		}
	}
}
