namespace EveOPreview.UI
{
	partial class ConfigForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
			this.ConfigFilesGridView = new System.Windows.Forms.DataGridView();
			this.FileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CopyColumn = new System.Windows.Forms.DataGridViewButtonColumn();
			this.DeleteColumn = new System.Windows.Forms.DataGridViewButtonColumn();
			((System.ComponentModel.ISupportInitialize)(this.ConfigFilesGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// ConfigFilesGridView
			// 
			this.ConfigFilesGridView.AllowUserToAddRows = false;
			this.ConfigFilesGridView.AllowUserToDeleteRows = false;
			this.ConfigFilesGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.ConfigFilesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ConfigFilesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileColumn,
            this.NameColumn,
            this.CopyColumn,
            this.DeleteColumn});
			this.ConfigFilesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ConfigFilesGridView.Location = new System.Drawing.Point(0, 0);
			this.ConfigFilesGridView.Name = "ConfigFilesGridView";
			this.ConfigFilesGridView.RowHeadersVisible = false;
			this.ConfigFilesGridView.Size = new System.Drawing.Size(724, 320);
			this.ConfigFilesGridView.TabIndex = 0;
			this.ConfigFilesGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConfigFilesGridView_CellContentClick);
			this.ConfigFilesGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConfigFilesGridView_CellDoubleClick);
			this.ConfigFilesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConfigFilesGridView_CellValueChanged);
			// 
			// FileColumn
			// 
			this.FileColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.FileColumn.DataPropertyName = "Key";
			this.FileColumn.HeaderText = "File";
			this.FileColumn.Name = "FileColumn";
			this.FileColumn.ReadOnly = true;
			// 
			// NameColumn
			// 
			this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.NameColumn.DataPropertyName = "Value";
			this.NameColumn.HeaderText = "Name";
			this.NameColumn.Name = "NameColumn";
			// 
			// CopyColumn
			// 
			this.CopyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.CopyColumn.HeaderText = "Copy";
			this.CopyColumn.MinimumWidth = 50;
			this.CopyColumn.Name = "CopyColumn";
			this.CopyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.CopyColumn.Text = "Copy";
			this.CopyColumn.UseColumnTextForButtonValue = true;
			this.CopyColumn.Width = 50;
			// 
			// DeleteColumn
			// 
			this.DeleteColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.DeleteColumn.HeaderText = "Delete";
			this.DeleteColumn.MinimumWidth = 50;
			this.DeleteColumn.Name = "DeleteColumn";
			this.DeleteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.DeleteColumn.Text = "Delete";
			this.DeleteColumn.UseColumnTextForButtonValue = true;
			this.DeleteColumn.Width = 50;
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(724, 320);
			this.Controls.Add(this.ConfigFilesGridView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ConfigForm";
			this.Text = "Configuration Settings";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigForm_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.ConfigFilesGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView ConfigFilesGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn FileColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
		private System.Windows.Forms.DataGridViewButtonColumn CopyColumn;
		private System.Windows.Forms.DataGridViewButtonColumn DeleteColumn;
	}
}