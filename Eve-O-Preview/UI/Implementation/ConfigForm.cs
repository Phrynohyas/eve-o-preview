using System;
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

		public Dictionary<string, string> ConfigFiles {
			set
			{
				this.ConfigFilesGridView.DataSource = new BindingList<KeyValuePair<string, string>>(value.ToList());
			}
		}

		public Action<string, string> RenameConfigFile { get; set; }
		public Action<string> CopyConfigFile { get; set; }
		public Action<string> DeleteConfigFile { get; set; }
		public Action UpdateConfigListing { get; set; }

		private void ConfigFilesGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.ConfigFilesGridView.CurrentCell = this.ConfigFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if(e.ColumnIndex == 3)
			{
				this.ConfigFilesGridView.CurrentCell.ReadOnly = false;
			}

			Debug.WriteLine("Column " + e.ColumnIndex + " and Row " + e.RowIndex + " Readonly: "+ this.ConfigFilesGridView.CurrentCell.ReadOnly);

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

				string name = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
				string file = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
				string oldName = (string)this.ConfigFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

				if (name != oldName && name != this._lastRename)
				{
					this._lastRename = name;
					this.RenameConfigFile?.Invoke(file, name);

				}
			}
		}
	}
}
