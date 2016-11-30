using System.Windows.Forms;

namespace EveOPreview.UI
{
	partial class MainForm
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

		/// <summary>s
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label OpacityLabel;
            System.Windows.Forms.ToolStripMenuItem RestoreWindowMenuItem;
            System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
            System.Windows.Forms.Panel MainOptionsPanel;
            System.Windows.Forms.Panel ThumbnailSizePanel;
            System.Windows.Forms.Label HeigthLabel;
            System.Windows.Forms.Label WidthLabel;
            System.Windows.Forms.Panel ZoomOptionsPanel;
            System.Windows.Forms.Label ZoomFactorLabel;
            System.Windows.Forms.Label ZoomAnchorLabel;
            System.Windows.Forms.Panel ThumbnailListPanel;
            System.Windows.Forms.Label ThumbnailsListLabel;
            System.Windows.Forms.ToolStripMenuItem TitleMenuItem;
            System.Windows.Forms.ToolStripSeparator SeparatorMenuItem;
            System.Windows.Forms.Panel OverlayOptionsPanel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ThumbnailOpacityTrackBar = new System.Windows.Forms.TrackBar();
            this.MinimizeToTrayCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableClientLayoutTrackingCheckBox = new System.Windows.Forms.CheckBox();
            this.HideActiveClientThumbnailCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowThumbnailsAlwaysOnTopCheckBox = new System.Windows.Forms.CheckBox();
            this.HideThumbnailsOnLostFocusCheckBox = new System.Windows.Forms.CheckBox();
            this.EnablePerClientThumbnailsLayoutsCheckBox = new System.Windows.Forms.CheckBox();
            this.ThumbnailsWidthNumericEdit = new System.Windows.Forms.NumericUpDown();
            this.ThumbnailsHeightNumericEdit = new System.Windows.Forms.NumericUpDown();
            this.ZoomAnchorPanel = new System.Windows.Forms.Panel();
            this.ZoomAanchorNWRadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorNRadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorNERadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorWRadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorSERadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorCRadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorSRadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorERadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomAanchorSWRadioButton = new System.Windows.Forms.RadioButton();
            this.EnableThumbnailZoomCheckBox = new System.Windows.Forms.CheckBox();
            this.ThumbnailZoomFactorNumericEdit = new System.Windows.Forms.NumericUpDown();
            this.ThumbnailsList = new System.Windows.Forms.CheckedListBox();
            this.HighlightColorLabel = new System.Windows.Forms.Label();
            this.ActiveClientHighlightColorButton = new System.Windows.Forms.Panel();
            this.EnableActiveClientHighlightCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowThumbnailOverlaysCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowThumbnailFramesCheckBox = new System.Windows.Forms.CheckBox();
            this.ForumLinkLabel = new System.Windows.Forms.LinkLabel();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            OpacityLabel = new System.Windows.Forms.Label();
            RestoreWindowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MainOptionsPanel = new System.Windows.Forms.Panel();
            ThumbnailSizePanel = new System.Windows.Forms.Panel();
            HeigthLabel = new System.Windows.Forms.Label();
            WidthLabel = new System.Windows.Forms.Label();
            ZoomOptionsPanel = new System.Windows.Forms.Panel();
            ZoomFactorLabel = new System.Windows.Forms.Label();
            ZoomAnchorLabel = new System.Windows.Forms.Label();
            ThumbnailListPanel = new System.Windows.Forms.Panel();
            ThumbnailsListLabel = new System.Windows.Forms.Label();
            TitleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SeparatorMenuItem = new System.Windows.Forms.ToolStripSeparator();
            OverlayOptionsPanel = new System.Windows.Forms.Panel();
            MainOptionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailOpacityTrackBar)).BeginInit();
            ThumbnailSizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailsWidthNumericEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailsHeightNumericEdit)).BeginInit();
            ZoomOptionsPanel.SuspendLayout();
            this.ZoomAnchorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailZoomFactorNumericEdit)).BeginInit();
            ThumbnailListPanel.SuspendLayout();
            OverlayOptionsPanel.SuspendLayout();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpacityLabel
            // 
            OpacityLabel.AutoSize = true;
            OpacityLabel.Location = new System.Drawing.Point(0, 23);
            OpacityLabel.Name = "OpacityLabel";
            OpacityLabel.Size = new System.Drawing.Size(43, 13);
            OpacityLabel.TabIndex = 0;
            OpacityLabel.Text = "Opacity";
            // 
            // RestoreWindowMenuItem
            // 
            RestoreWindowMenuItem.Name = "RestoreWindowMenuItem";
            RestoreWindowMenuItem.Size = new System.Drawing.Size(151, 22);
            RestoreWindowMenuItem.Text = "Restore";
            RestoreWindowMenuItem.Click += new System.EventHandler(this.RestoreMainForm_Handler);
            // 
            // ExitMenuItem
            // 
            ExitMenuItem.Name = "ExitMenuItem";
            ExitMenuItem.Size = new System.Drawing.Size(151, 22);
            ExitMenuItem.Text = "Exit";
            ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick_Handler);
            // 
            // MainOptionsPanel
            // 
            MainOptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            MainOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            MainOptionsPanel.Controls.Add(this.ThumbnailOpacityTrackBar);
            MainOptionsPanel.Controls.Add(this.MinimizeToTrayCheckBox);
            MainOptionsPanel.Controls.Add(this.EnableClientLayoutTrackingCheckBox);
            MainOptionsPanel.Controls.Add(this.HideActiveClientThumbnailCheckBox);
            MainOptionsPanel.Controls.Add(this.ShowThumbnailsAlwaysOnTopCheckBox);
            MainOptionsPanel.Controls.Add(this.HideThumbnailsOnLostFocusCheckBox);
            MainOptionsPanel.Controls.Add(this.EnablePerClientThumbnailsLayoutsCheckBox);
            MainOptionsPanel.Controls.Add(OpacityLabel);
            MainOptionsPanel.Location = new System.Drawing.Point(3, 3);
            MainOptionsPanel.Name = "MainOptionsPanel";
            MainOptionsPanel.Size = new System.Drawing.Size(264, 164);
            MainOptionsPanel.TabIndex = 0;
            // 
            // ThumbnailOpacityTrackBar
            // 
            this.ThumbnailOpacityTrackBar.AutoSize = false;
            this.ThumbnailOpacityTrackBar.LargeChange = 10;
            this.ThumbnailOpacityTrackBar.Location = new System.Drawing.Point(51, 22);
            this.ThumbnailOpacityTrackBar.Maximum = 100;
            this.ThumbnailOpacityTrackBar.Minimum = 20;
            this.ThumbnailOpacityTrackBar.Name = "ThumbnailOpacityTrackBar";
            this.ThumbnailOpacityTrackBar.Size = new System.Drawing.Size(208, 22);
            this.ThumbnailOpacityTrackBar.TabIndex = 1;
            this.ThumbnailOpacityTrackBar.TickFrequency = 10;
            this.ThumbnailOpacityTrackBar.Value = 20;
            this.ThumbnailOpacityTrackBar.ValueChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // MinimizeToTrayCheckBox
            // 
            this.MinimizeToTrayCheckBox.AutoSize = true;
            this.MinimizeToTrayCheckBox.Location = new System.Drawing.Point(3, 3);
            this.MinimizeToTrayCheckBox.Name = "MinimizeToTrayCheckBox";
            this.MinimizeToTrayCheckBox.Size = new System.Drawing.Size(139, 17);
            this.MinimizeToTrayCheckBox.TabIndex = 0;
            this.MinimizeToTrayCheckBox.Text = "Minimize to System Tray";
            this.MinimizeToTrayCheckBox.UseVisualStyleBackColor = true;
            this.MinimizeToTrayCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // EnableClientLayoutTrackingCheckBox
            // 
            this.EnableClientLayoutTrackingCheckBox.AutoSize = true;
            this.EnableClientLayoutTrackingCheckBox.Location = new System.Drawing.Point(3, 50);
            this.EnableClientLayoutTrackingCheckBox.Name = "EnableClientLayoutTrackingCheckBox";
            this.EnableClientLayoutTrackingCheckBox.Size = new System.Drawing.Size(127, 17);
            this.EnableClientLayoutTrackingCheckBox.TabIndex = 2;
            this.EnableClientLayoutTrackingCheckBox.Text = "Track client locations";
            this.EnableClientLayoutTrackingCheckBox.UseVisualStyleBackColor = true;
            this.EnableClientLayoutTrackingCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // HideActiveClientThumbnailCheckBox
            // 
            this.HideActiveClientThumbnailCheckBox.AutoSize = true;
            this.HideActiveClientThumbnailCheckBox.Checked = true;
            this.HideActiveClientThumbnailCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HideActiveClientThumbnailCheckBox.Location = new System.Drawing.Point(3, 73);
            this.HideActiveClientThumbnailCheckBox.Name = "HideActiveClientThumbnailCheckBox";
            this.HideActiveClientThumbnailCheckBox.Size = new System.Drawing.Size(184, 17);
            this.HideActiveClientThumbnailCheckBox.TabIndex = 3;
            this.HideActiveClientThumbnailCheckBox.Text = "Hide preview of active EVE client";
            this.HideActiveClientThumbnailCheckBox.UseVisualStyleBackColor = true;
            this.HideActiveClientThumbnailCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ShowThumbnailsAlwaysOnTopCheckBox
            // 
            this.ShowThumbnailsAlwaysOnTopCheckBox.AutoSize = true;
            this.ShowThumbnailsAlwaysOnTopCheckBox.Checked = true;
            this.ShowThumbnailsAlwaysOnTopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowThumbnailsAlwaysOnTopCheckBox.Location = new System.Drawing.Point(3, 96);
            this.ShowThumbnailsAlwaysOnTopCheckBox.Name = "ShowThumbnailsAlwaysOnTopCheckBox";
            this.ShowThumbnailsAlwaysOnTopCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowThumbnailsAlwaysOnTopCheckBox.Size = new System.Drawing.Size(137, 17);
            this.ShowThumbnailsAlwaysOnTopCheckBox.TabIndex = 4;
            this.ShowThumbnailsAlwaysOnTopCheckBox.Text = "Previews always on top";
            this.ShowThumbnailsAlwaysOnTopCheckBox.UseVisualStyleBackColor = true;
            this.ShowThumbnailsAlwaysOnTopCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // HideThumbnailsOnLostFocusCheckBox
            // 
            this.HideThumbnailsOnLostFocusCheckBox.AutoSize = true;
            this.HideThumbnailsOnLostFocusCheckBox.Checked = true;
            this.HideThumbnailsOnLostFocusCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HideThumbnailsOnLostFocusCheckBox.Location = new System.Drawing.Point(3, 119);
            this.HideThumbnailsOnLostFocusCheckBox.Name = "HideThumbnailsOnLostFocusCheckBox";
            this.HideThumbnailsOnLostFocusCheckBox.Size = new System.Drawing.Size(234, 17);
            this.HideThumbnailsOnLostFocusCheckBox.TabIndex = 5;
            this.HideThumbnailsOnLostFocusCheckBox.Text = "Hide previews when EVE client is not active";
            this.HideThumbnailsOnLostFocusCheckBox.UseVisualStyleBackColor = true;
            this.HideThumbnailsOnLostFocusCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // EnablePerClientThumbnailsLayoutsCheckBox
            // 
            this.EnablePerClientThumbnailsLayoutsCheckBox.AutoSize = true;
            this.EnablePerClientThumbnailsLayoutsCheckBox.Checked = true;
            this.EnablePerClientThumbnailsLayoutsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnablePerClientThumbnailsLayoutsCheckBox.Location = new System.Drawing.Point(3, 142);
            this.EnablePerClientThumbnailsLayoutsCheckBox.Name = "EnablePerClientThumbnailsLayoutsCheckBox";
            this.EnablePerClientThumbnailsLayoutsCheckBox.Size = new System.Drawing.Size(185, 17);
            this.EnablePerClientThumbnailsLayoutsCheckBox.TabIndex = 6;
            this.EnablePerClientThumbnailsLayoutsCheckBox.Text = "Unique layout for each EVE client";
            this.EnablePerClientThumbnailsLayoutsCheckBox.UseVisualStyleBackColor = true;
            this.EnablePerClientThumbnailsLayoutsCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ThumbnailSizePanel
            // 
            ThumbnailSizePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            ThumbnailSizePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ThumbnailSizePanel.Controls.Add(HeigthLabel);
            ThumbnailSizePanel.Controls.Add(WidthLabel);
            ThumbnailSizePanel.Controls.Add(this.ThumbnailsWidthNumericEdit);
            ThumbnailSizePanel.Controls.Add(this.ThumbnailsHeightNumericEdit);
            ThumbnailSizePanel.Location = new System.Drawing.Point(3, 173);
            ThumbnailSizePanel.Name = "ThumbnailSizePanel";
            ThumbnailSizePanel.Size = new System.Drawing.Size(264, 28);
            ThumbnailSizePanel.TabIndex = 1;
            // 
            // HeigthLabel
            // 
            HeigthLabel.AutoSize = true;
            HeigthLabel.Location = new System.Drawing.Point(178, 5);
            HeigthLabel.Name = "HeigthLabel";
            HeigthLabel.Size = new System.Drawing.Size(38, 13);
            HeigthLabel.TabIndex = 14;
            HeigthLabel.Text = "Heigth";
            // 
            // WidthLabel
            // 
            WidthLabel.AutoSize = true;
            WidthLabel.Location = new System.Drawing.Point(0, 5);
            WidthLabel.Name = "WidthLabel";
            WidthLabel.Size = new System.Drawing.Size(124, 13);
            WidthLabel.TabIndex = 13;
            WidthLabel.Text = "Default Thumbnail Width";
            // 
            // ThumbnailsWidthNumericEdit
            // 
            this.ThumbnailsWidthNumericEdit.BackColor = System.Drawing.SystemColors.Window;
            this.ThumbnailsWidthNumericEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ThumbnailsWidthNumericEdit.CausesValidation = false;
            this.ThumbnailsWidthNumericEdit.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ThumbnailsWidthNumericEdit.Location = new System.Drawing.Point(124, 3);
            this.ThumbnailsWidthNumericEdit.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.ThumbnailsWidthNumericEdit.Name = "ThumbnailsWidthNumericEdit";
            this.ThumbnailsWidthNumericEdit.Size = new System.Drawing.Size(48, 20);
            this.ThumbnailsWidthNumericEdit.TabIndex = 0;
            this.ThumbnailsWidthNumericEdit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ThumbnailsWidthNumericEdit.ValueChanged += new System.EventHandler(this.ThumbnailSizeChanged_Handler);
            // 
            // ThumbnailsHeightNumericEdit
            // 
            this.ThumbnailsHeightNumericEdit.BackColor = System.Drawing.SystemColors.Window;
            this.ThumbnailsHeightNumericEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ThumbnailsHeightNumericEdit.CausesValidation = false;
            this.ThumbnailsHeightNumericEdit.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ThumbnailsHeightNumericEdit.Location = new System.Drawing.Point(217, 3);
            this.ThumbnailsHeightNumericEdit.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.ThumbnailsHeightNumericEdit.Name = "ThumbnailsHeightNumericEdit";
            this.ThumbnailsHeightNumericEdit.Size = new System.Drawing.Size(42, 20);
            this.ThumbnailsHeightNumericEdit.TabIndex = 1;
            this.ThumbnailsHeightNumericEdit.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.ThumbnailsHeightNumericEdit.ValueChanged += new System.EventHandler(this.ThumbnailSizeChanged_Handler);
            // 
            // ZoomOptionsPanel
            // 
            ZoomOptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            ZoomOptionsPanel.BackColor = System.Drawing.SystemColors.Control;
            ZoomOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ZoomOptionsPanel.Controls.Add(ZoomFactorLabel);
            ZoomOptionsPanel.Controls.Add(this.ZoomAnchorPanel);
            ZoomOptionsPanel.Controls.Add(ZoomAnchorLabel);
            ZoomOptionsPanel.Controls.Add(this.EnableThumbnailZoomCheckBox);
            ZoomOptionsPanel.Controls.Add(this.ThumbnailZoomFactorNumericEdit);
            ZoomOptionsPanel.Location = new System.Drawing.Point(3, 207);
            ZoomOptionsPanel.Name = "ZoomOptionsPanel";
            ZoomOptionsPanel.Size = new System.Drawing.Size(264, 82);
            ZoomOptionsPanel.TabIndex = 2;
            // 
            // ZoomFactorLabel
            // 
            ZoomFactorLabel.AutoSize = true;
            ZoomFactorLabel.Location = new System.Drawing.Point(0, 43);
            ZoomFactorLabel.Name = "ZoomFactorLabel";
            ZoomFactorLabel.Size = new System.Drawing.Size(67, 13);
            ZoomFactorLabel.TabIndex = 29;
            ZoomFactorLabel.Text = "Zoom Factor";
            // 
            // ZoomAnchorPanel
            // 
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorNWRadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorNRadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorNERadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorWRadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorSERadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorCRadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorSRadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorERadioButton);
            this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorSWRadioButton);
            this.ZoomAnchorPanel.Location = new System.Drawing.Point(199, 20);
            this.ZoomAnchorPanel.Name = "ZoomAnchorPanel";
            this.ZoomAnchorPanel.Size = new System.Drawing.Size(60, 57);
            this.ZoomAnchorPanel.TabIndex = 2;
            // 
            // ZoomAanchorNWRadioButton
            // 
            this.ZoomAanchorNWRadioButton.AutoSize = true;
            this.ZoomAanchorNWRadioButton.Location = new System.Drawing.Point(3, 3);
            this.ZoomAanchorNWRadioButton.Name = "ZoomAanchorNWRadioButton";
            this.ZoomAanchorNWRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorNWRadioButton.TabIndex = 0;
            this.ZoomAanchorNWRadioButton.TabStop = true;
            this.ZoomAanchorNWRadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorNWRadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorNRadioButton
            // 
            this.ZoomAanchorNRadioButton.AutoSize = true;
            this.ZoomAanchorNRadioButton.Location = new System.Drawing.Point(23, 3);
            this.ZoomAanchorNRadioButton.Name = "ZoomAanchorNRadioButton";
            this.ZoomAanchorNRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorNRadioButton.TabIndex = 1;
            this.ZoomAanchorNRadioButton.TabStop = true;
            this.ZoomAanchorNRadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorNRadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorNERadioButton
            // 
            this.ZoomAanchorNERadioButton.AutoSize = true;
            this.ZoomAanchorNERadioButton.Location = new System.Drawing.Point(43, 3);
            this.ZoomAanchorNERadioButton.Name = "ZoomAanchorNERadioButton";
            this.ZoomAanchorNERadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorNERadioButton.TabIndex = 2;
            this.ZoomAanchorNERadioButton.TabStop = true;
            this.ZoomAanchorNERadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorNERadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorWRadioButton
            // 
            this.ZoomAanchorWRadioButton.AutoSize = true;
            this.ZoomAanchorWRadioButton.Location = new System.Drawing.Point(3, 22);
            this.ZoomAanchorWRadioButton.Name = "ZoomAanchorWRadioButton";
            this.ZoomAanchorWRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorWRadioButton.TabIndex = 3;
            this.ZoomAanchorWRadioButton.TabStop = true;
            this.ZoomAanchorWRadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorWRadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorSERadioButton
            // 
            this.ZoomAanchorSERadioButton.AutoSize = true;
            this.ZoomAanchorSERadioButton.Location = new System.Drawing.Point(43, 41);
            this.ZoomAanchorSERadioButton.Name = "ZoomAanchorSERadioButton";
            this.ZoomAanchorSERadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorSERadioButton.TabIndex = 8;
            this.ZoomAanchorSERadioButton.TabStop = true;
            this.ZoomAanchorSERadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorSERadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorCRadioButton
            // 
            this.ZoomAanchorCRadioButton.AutoSize = true;
            this.ZoomAanchorCRadioButton.Location = new System.Drawing.Point(23, 22);
            this.ZoomAanchorCRadioButton.Name = "ZoomAanchorCRadioButton";
            this.ZoomAanchorCRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorCRadioButton.TabIndex = 4;
            this.ZoomAanchorCRadioButton.TabStop = true;
            this.ZoomAanchorCRadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorCRadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorSRadioButton
            // 
            this.ZoomAanchorSRadioButton.AutoSize = true;
            this.ZoomAanchorSRadioButton.Location = new System.Drawing.Point(23, 41);
            this.ZoomAanchorSRadioButton.Name = "ZoomAanchorSRadioButton";
            this.ZoomAanchorSRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorSRadioButton.TabIndex = 7;
            this.ZoomAanchorSRadioButton.TabStop = true;
            this.ZoomAanchorSRadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorSRadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorERadioButton
            // 
            this.ZoomAanchorERadioButton.AutoSize = true;
            this.ZoomAanchorERadioButton.Location = new System.Drawing.Point(43, 22);
            this.ZoomAanchorERadioButton.Name = "ZoomAanchorERadioButton";
            this.ZoomAanchorERadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorERadioButton.TabIndex = 5;
            this.ZoomAanchorERadioButton.TabStop = true;
            this.ZoomAanchorERadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorERadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAanchorSWRadioButton
            // 
            this.ZoomAanchorSWRadioButton.AutoSize = true;
            this.ZoomAanchorSWRadioButton.Location = new System.Drawing.Point(3, 41);
            this.ZoomAanchorSWRadioButton.Name = "ZoomAanchorSWRadioButton";
            this.ZoomAanchorSWRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ZoomAanchorSWRadioButton.TabIndex = 6;
            this.ZoomAanchorSWRadioButton.TabStop = true;
            this.ZoomAanchorSWRadioButton.UseVisualStyleBackColor = true;
            this.ZoomAanchorSWRadioButton.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ZoomAnchorLabel
            // 
            ZoomAnchorLabel.AutoSize = true;
            ZoomAnchorLabel.Location = new System.Drawing.Point(150, 43);
            ZoomAnchorLabel.Name = "ZoomAnchorLabel";
            ZoomAnchorLabel.Size = new System.Drawing.Size(41, 13);
            ZoomAnchorLabel.TabIndex = 30;
            ZoomAnchorLabel.Text = "Anchor";
            // 
            // EnableThumbnailZoomCheckBox
            // 
            this.EnableThumbnailZoomCheckBox.AutoSize = true;
            this.EnableThumbnailZoomCheckBox.Checked = true;
            this.EnableThumbnailZoomCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableThumbnailZoomCheckBox.Location = new System.Drawing.Point(3, 5);
            this.EnableThumbnailZoomCheckBox.Name = "EnableThumbnailZoomCheckBox";
            this.EnableThumbnailZoomCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.EnableThumbnailZoomCheckBox.Size = new System.Drawing.Size(98, 17);
            this.EnableThumbnailZoomCheckBox.TabIndex = 0;
            this.EnableThumbnailZoomCheckBox.Text = "Zoom on hover";
            this.EnableThumbnailZoomCheckBox.UseVisualStyleBackColor = true;
            this.EnableThumbnailZoomCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ThumbnailZoomFactorNumericEdit
            // 
            this.ThumbnailZoomFactorNumericEdit.BackColor = System.Drawing.SystemColors.Window;
            this.ThumbnailZoomFactorNumericEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ThumbnailZoomFactorNumericEdit.Location = new System.Drawing.Point(94, 41);
            this.ThumbnailZoomFactorNumericEdit.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ThumbnailZoomFactorNumericEdit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ThumbnailZoomFactorNumericEdit.Name = "ThumbnailZoomFactorNumericEdit";
            this.ThumbnailZoomFactorNumericEdit.Size = new System.Drawing.Size(38, 20);
            this.ThumbnailZoomFactorNumericEdit.TabIndex = 1;
            this.ThumbnailZoomFactorNumericEdit.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ThumbnailZoomFactorNumericEdit.ValueChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ThumbnailListPanel
            // 
            ThumbnailListPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            ThumbnailListPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ThumbnailListPanel.Controls.Add(this.ThumbnailsList);
            ThumbnailListPanel.Controls.Add(ThumbnailsListLabel);
            ThumbnailListPanel.Location = new System.Drawing.Point(3, 350);
            ThumbnailListPanel.Name = "ThumbnailListPanel";
            ThumbnailListPanel.Size = new System.Drawing.Size(264, 125);
            ThumbnailListPanel.TabIndex = 5;
            // 
            // ThumbnailsList
            // 
            this.ThumbnailsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ThumbnailsList.BackColor = System.Drawing.SystemColors.Window;
            this.ThumbnailsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ThumbnailsList.FormattingEnabled = true;
            this.ThumbnailsList.IntegralHeight = false;
            this.ThumbnailsList.Location = new System.Drawing.Point(3, 18);
            this.ThumbnailsList.Name = "ThumbnailsList";
            this.ThumbnailsList.Size = new System.Drawing.Size(256, 100);
            this.ThumbnailsList.TabIndex = 0;
            this.ThumbnailsList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ThumbnailsList_ItemCheck_Handler);
            // 
            // ThumbnailsListLabel
            // 
            ThumbnailsListLabel.AutoSize = true;
            ThumbnailsListLabel.Location = new System.Drawing.Point(0, 0);
            ThumbnailsListLabel.Name = "ThumbnailsListLabel";
            ThumbnailsListLabel.Size = new System.Drawing.Size(162, 13);
            ThumbnailsListLabel.TabIndex = 29;
            ThumbnailsListLabel.Text = "Thumbnails (check to force hide)";
            // 
            // TitleMenuItem
            // 
            TitleMenuItem.Enabled = false;
            TitleMenuItem.Name = "TitleMenuItem";
            TitleMenuItem.Size = new System.Drawing.Size(151, 22);
            TitleMenuItem.Text = "EVE-O Preview";
            // 
            // SeparatorMenuItem
            // 
            SeparatorMenuItem.Name = "SeparatorMenuItem";
            SeparatorMenuItem.Size = new System.Drawing.Size(148, 6);
            // 
            // OverlayOptionsPanel
            // 
            OverlayOptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            OverlayOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            OverlayOptionsPanel.Controls.Add(this.HighlightColorLabel);
            OverlayOptionsPanel.Controls.Add(this.ActiveClientHighlightColorButton);
            OverlayOptionsPanel.Controls.Add(this.EnableActiveClientHighlightCheckBox);
            OverlayOptionsPanel.Controls.Add(this.ShowThumbnailOverlaysCheckBox);
            OverlayOptionsPanel.Controls.Add(this.ShowThumbnailFramesCheckBox);
            OverlayOptionsPanel.Location = new System.Drawing.Point(3, 295);
            OverlayOptionsPanel.Name = "OverlayOptionsPanel";
            OverlayOptionsPanel.Size = new System.Drawing.Size(264, 49);
            OverlayOptionsPanel.TabIndex = 3;
            // 
            // HighlightColorLabel
            // 
            this.HighlightColorLabel.AutoSize = true;
            this.HighlightColorLabel.Location = new System.Drawing.Point(150, 28);
            this.HighlightColorLabel.Name = "HighlightColorLabel";
            this.HighlightColorLabel.Size = new System.Drawing.Size(31, 13);
            this.HighlightColorLabel.TabIndex = 19;
            this.HighlightColorLabel.Text = "Color";
            // 
            // ActiveClientHighlightColorButton
            // 
            this.ActiveClientHighlightColorButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActiveClientHighlightColorButton.Location = new System.Drawing.Point(199, 27);
            this.ActiveClientHighlightColorButton.Name = "ActiveClientHighlightColorButton";
            this.ActiveClientHighlightColorButton.Size = new System.Drawing.Size(60, 17);
            this.ActiveClientHighlightColorButton.TabIndex = 3;
            this.ActiveClientHighlightColorButton.Click += new System.EventHandler(this.ActiveClientHighlightColorButton_Click);
            // 
            // EnableActiveClientHighlightCheckBox
            // 
            this.EnableActiveClientHighlightCheckBox.AutoSize = true;
            this.EnableActiveClientHighlightCheckBox.Checked = true;
            this.EnableActiveClientHighlightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableActiveClientHighlightCheckBox.Location = new System.Drawing.Point(3, 27);
            this.EnableActiveClientHighlightCheckBox.Name = "EnableActiveClientHighlightCheckBox";
            this.EnableActiveClientHighlightCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.EnableActiveClientHighlightCheckBox.Size = new System.Drawing.Size(127, 17);
            this.EnableActiveClientHighlightCheckBox.TabIndex = 2;
            this.EnableActiveClientHighlightCheckBox.Text = "Highlight active client";
            this.EnableActiveClientHighlightCheckBox.UseVisualStyleBackColor = true;
            this.EnableActiveClientHighlightCheckBox.CheckStateChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ShowThumbnailOverlaysCheckBox
            // 
            this.ShowThumbnailOverlaysCheckBox.AutoSize = true;
            this.ShowThumbnailOverlaysCheckBox.Checked = true;
            this.ShowThumbnailOverlaysCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowThumbnailOverlaysCheckBox.Location = new System.Drawing.Point(3, 4);
            this.ShowThumbnailOverlaysCheckBox.Name = "ShowThumbnailOverlaysCheckBox";
            this.ShowThumbnailOverlaysCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowThumbnailOverlaysCheckBox.Size = new System.Drawing.Size(90, 17);
            this.ShowThumbnailOverlaysCheckBox.TabIndex = 0;
            this.ShowThumbnailOverlaysCheckBox.Text = "Show overlay";
            this.ShowThumbnailOverlaysCheckBox.UseVisualStyleBackColor = true;
            this.ShowThumbnailOverlaysCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ShowThumbnailFramesCheckBox
            // 
            this.ShowThumbnailFramesCheckBox.AutoSize = true;
            this.ShowThumbnailFramesCheckBox.Checked = true;
            this.ShowThumbnailFramesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowThumbnailFramesCheckBox.Location = new System.Drawing.Point(153, 4);
            this.ShowThumbnailFramesCheckBox.Name = "ShowThumbnailFramesCheckBox";
            this.ShowThumbnailFramesCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowThumbnailFramesCheckBox.Size = new System.Drawing.Size(87, 17);
            this.ShowThumbnailFramesCheckBox.TabIndex = 1;
            this.ShowThumbnailFramesCheckBox.Text = "Show frames";
            this.ShowThumbnailFramesCheckBox.UseVisualStyleBackColor = true;
            this.ShowThumbnailFramesCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
            // 
            // ForumLinkLabel
            // 
            this.ForumLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForumLinkLabel.Location = new System.Drawing.Point(4, 478);
            this.ForumLinkLabel.Name = "ForumLinkLabel";
            this.ForumLinkLabel.Size = new System.Drawing.Size(264, 33);
            this.ForumLinkLabel.TabIndex = 0;
            this.ForumLinkLabel.TabStop = true;
            this.ForumLinkLabel.Text = "to be set from prresenter to be set from prresenter to be set from prresenter to " +
    "be set from prresenter";
            this.ForumLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ForumLinkLabelClicked_Handler);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.TrayMenu;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "EVE-O Preview";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RestoreMainForm_Handler);
            // 
            // TrayMenu
            // 
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            TitleMenuItem,
            RestoreWindowMenuItem,
            SeparatorMenuItem,
            ExitMenuItem});
            this.TrayMenu.Name = "contextMenuStrip1";
            this.TrayMenu.Size = new System.Drawing.Size(152, 76);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(270, 511);
            this.Controls.Add(OverlayOptionsPanel);
            this.Controls.Add(MainOptionsPanel);
            this.Controls.Add(ThumbnailSizePanel);
            this.Controls.Add(ZoomOptionsPanel);
            this.Controls.Add(ThumbnailListPanel);
            this.Controls.Add(this.ForumLinkLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EVE-O Preview";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormClosing_Handler);
            this.Load += new System.EventHandler(this.MainFormResize_Handler);
            this.Resize += new System.EventHandler(this.MainFormResize_Handler);
            MainOptionsPanel.ResumeLayout(false);
            MainOptionsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailOpacityTrackBar)).EndInit();
            ThumbnailSizePanel.ResumeLayout(false);
            ThumbnailSizePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailsWidthNumericEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailsHeightNumericEdit)).EndInit();
            ZoomOptionsPanel.ResumeLayout(false);
            ZoomOptionsPanel.PerformLayout();
            this.ZoomAnchorPanel.ResumeLayout(false);
            this.ZoomAnchorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailZoomFactorNumericEdit)).EndInit();
            ThumbnailListPanel.ResumeLayout(false);
            ThumbnailListPanel.PerformLayout();
            OverlayOptionsPanel.ResumeLayout(false);
            OverlayOptionsPanel.PerformLayout();
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
		private LinkLabel ForumLinkLabel;
		private NumericUpDown ThumbnailsWidthNumericEdit;
		private NumericUpDown ThumbnailsHeightNumericEdit;
		private CheckBox EnableThumbnailZoomCheckBox;
		private RadioButton ZoomAanchorNWRadioButton;
		private RadioButton ZoomAanchorNRadioButton;
		private RadioButton ZoomAanchorNERadioButton;
		private RadioButton ZoomAanchorWRadioButton;
		private RadioButton ZoomAanchorCRadioButton;
		private RadioButton ZoomAanchorERadioButton;
		private RadioButton ZoomAanchorSWRadioButton;
		private RadioButton ZoomAanchorSRadioButton;
		private RadioButton ZoomAanchorSERadioButton;
		private NumericUpDown ThumbnailZoomFactorNumericEdit;
		private Panel ZoomAnchorPanel;
		private CheckedListBox ThumbnailsList;
		private NotifyIcon NotifyIcon;
		private ContextMenuStrip TrayMenu;
		private CheckBox EnableClientLayoutTrackingCheckBox;
		private CheckBox HideActiveClientThumbnailCheckBox;
		private CheckBox ShowThumbnailsAlwaysOnTopCheckBox;
		private CheckBox HideThumbnailsOnLostFocusCheckBox;
		private CheckBox EnablePerClientThumbnailsLayoutsCheckBox;
		private CheckBox ShowThumbnailOverlaysCheckBox;
		private CheckBox ShowThumbnailFramesCheckBox;
		private CheckBox MinimizeToTrayCheckBox;
		private TrackBar ThumbnailOpacityTrackBar;
		private CheckBox EnableActiveClientHighlightCheckBox;
		private Label HighlightColorLabel;
		private Panel ActiveClientHighlightColorButton;
	}
}