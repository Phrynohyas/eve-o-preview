﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveOPreview.UI
{
	public partial class ConfigForm : Form, IConfigView
	{
		public ConfigForm(Dictionary<string, string> configFiles)
		{
			InitializeComponent();

			this.ConfigFiles = configFiles;


		}

		public Dictionary<string, string> ConfigFiles
		{
			set
			{
				this.ConfigFilesGridView.DataSource = new BindingList<KeyValuePair<string, string>>(value.ToList());
			}
		}

		public Action<string, string> RenameConfigFile { get; set; }
		public Action<string, string> RenameConfigFilename { get; set; }
		public Action<string> CopyConfigFile { get; set; }
		public Action<string> DeleteConfigFile { get; set; }
		public Action UpdateConfigListing { get; set; }
		public Action DialogClosed { get; set; }

		private void ConfigFilesGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.ConfigFilesGridView.CurrentCell = this.ConfigFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (e.ColumnIndex == 3 || e.ColumnIndex == 2)
			{
				this.ConfigFilesGridView.CurrentCell.ReadOnly = false;
			}

			if (!this.ConfigFilesGridView.CurrentCell.ReadOnly)
			{
				this.ConfigFilesGridView.BeginEdit(true);
			}


		}

		private string _lastRename = "";

		private void ConfigFilesGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 3 && e.RowIndex >= 0)
			{

				string name = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[3].EditedFormattedValue;
				string file = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[2].Value;
				string oldName = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[3].Value;

				if (name != oldName && name != this._lastRename)
				{
					this._lastRename = name;
					this.RenameConfigFile?.Invoke(file, name);

				}
			}else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
			{

				string newFile = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[2].EditedFormattedValue;
				string file = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[2].Value;

				if (newFile != file && newFile != this._lastRename)
				{
					this._lastRename = newFile;
					this.RenameConfigFilename?.Invoke(file, newFile);

				}
			}
		}

		private void ConfigFilesGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			Debug.WriteLine("Column " + e.ColumnIndex + " and Row " + e.RowIndex);
			if (e.ColumnIndex == 0 && e.RowIndex >= 0) //copy
			{
				string file = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[2].Value;
				this.CopyConfigFile(file);
				Debug.WriteLine("COPY");
			}
			else if (e.ColumnIndex == 1 && e.RowIndex >= 0) //delete
			{
				string file = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[2].Value;
				this.DeleteConfigFile(file);
				Debug.WriteLine("DELETE");
			}
		}

		private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.DialogClosed?.Invoke();
			this.Dispose();
		}
	}
}