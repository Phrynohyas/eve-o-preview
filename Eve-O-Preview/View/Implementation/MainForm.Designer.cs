using System.Windows.Forms;

namespace EveOPreview.View
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
			System.Windows.Forms.ToolStripMenuItem RestoreWindowMenuItem;
			System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
			System.Windows.Forms.ToolStripMenuItem TitleMenuItem;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuItem;
			System.Windows.Forms.TabControl ContentTabControl;
			System.Windows.Forms.TabPage GeneralTabPage;
			System.Windows.Forms.Panel GeneralSettingsPanel;
			System.Windows.Forms.TabPage ThumbnailTabPage;
			System.Windows.Forms.Panel ThumbnailSettingsPanel;
			System.Windows.Forms.Label HeigthLabel;
			System.Windows.Forms.Label WidthLabel;
			System.Windows.Forms.Label OpacityLabel;
			System.Windows.Forms.Panel ZoomSettingsPanel;
			System.Windows.Forms.Label ZoomFactorLabel;
			System.Windows.Forms.Label ZoomAnchorLabel;
			System.Windows.Forms.TabPage OverlayTabPage;
			System.Windows.Forms.Panel OverlaySettingsPanel;
			System.Windows.Forms.TabPage ClientsTabPage;
			System.Windows.Forms.Panel ClientsPanel;
			System.Windows.Forms.Label ThumbnailsListLabel;
			System.Windows.Forms.TabPage AboutTabPage;
			System.Windows.Forms.Panel AboutPanel;
			System.Windows.Forms.Label DocumentationLinkLabel;
			System.Windows.Forms.Label DescriptionLabel;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.Label NameLabel;
			this.EnableClientLayoutTrackingCheckBox = new System.Windows.Forms.CheckBox();
			this.HideActiveClientThumbnailCheckBox = new System.Windows.Forms.CheckBox();
			this.ShowThumbnailsAlwaysOnTopCheckBox = new System.Windows.Forms.CheckBox();
			this.HideThumbnailsOnLostFocusCheckBox = new System.Windows.Forms.CheckBox();
			this.EnablePerClientThumbnailsLayoutsCheckBox = new System.Windows.Forms.CheckBox();
			this.MinimizeToTrayCheckBox = new System.Windows.Forms.CheckBox();
			this.ThumbnailsWidthNumericEdit = new System.Windows.Forms.NumericUpDown();
			this.ThumbnailsHeightNumericEdit = new System.Windows.Forms.NumericUpDown();
			this.ThumbnailOpacityTrackBar = new System.Windows.Forms.TrackBar();
			this.ZoomTabPage = new System.Windows.Forms.TabPage();
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
			this.HighlightColorLabel = new System.Windows.Forms.Label();
			this.ActiveClientHighlightColorButton = new System.Windows.Forms.Panel();
			this.EnableActiveClientHighlightCheckBox = new System.Windows.Forms.CheckBox();
			this.ShowThumbnailOverlaysCheckBox = new System.Windows.Forms.CheckBox();
			this.ShowThumbnailFramesCheckBox = new System.Windows.Forms.CheckBox();
			this.ThumbnailsList = new System.Windows.Forms.CheckedListBox();
			this.VersionLabel = new System.Windows.Forms.Label();
			this.DocumentationLink = new System.Windows.Forms.LinkLabel();
			this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			RestoreWindowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			TitleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			SeparatorMenuItem = new System.Windows.Forms.ToolStripSeparator();
			ContentTabControl = new System.Windows.Forms.TabControl();
			GeneralTabPage = new System.Windows.Forms.TabPage();
			GeneralSettingsPanel = new System.Windows.Forms.Panel();
			ThumbnailTabPage = new System.Windows.Forms.TabPage();
			ThumbnailSettingsPanel = new System.Windows.Forms.Panel();
			HeigthLabel = new System.Windows.Forms.Label();
			WidthLabel = new System.Windows.Forms.Label();
			OpacityLabel = new System.Windows.Forms.Label();
			ZoomSettingsPanel = new System.Windows.Forms.Panel();
			ZoomFactorLabel = new System.Windows.Forms.Label();
			ZoomAnchorLabel = new System.Windows.Forms.Label();
			OverlayTabPage = new System.Windows.Forms.TabPage();
			OverlaySettingsPanel = new System.Windows.Forms.Panel();
			ClientsTabPage = new System.Windows.Forms.TabPage();
			ClientsPanel = new System.Windows.Forms.Panel();
			ThumbnailsListLabel = new System.Windows.Forms.Label();
			AboutTabPage = new System.Windows.Forms.TabPage();
			AboutPanel = new System.Windows.Forms.Panel();
			DocumentationLinkLabel = new System.Windows.Forms.Label();
			DescriptionLabel = new System.Windows.Forms.Label();
			NameLabel = new System.Windows.Forms.Label();
			ContentTabControl.SuspendLayout();
			GeneralTabPage.SuspendLayout();
			GeneralSettingsPanel.SuspendLayout();
			ThumbnailTabPage.SuspendLayout();
			ThumbnailSettingsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailsWidthNumericEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailsHeightNumericEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailOpacityTrackBar)).BeginInit();
			this.ZoomTabPage.SuspendLayout();
			ZoomSettingsPanel.SuspendLayout();
			this.ZoomAnchorPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailZoomFactorNumericEdit)).BeginInit();
			OverlayTabPage.SuspendLayout();
			OverlaySettingsPanel.SuspendLayout();
			ClientsTabPage.SuspendLayout();
			ClientsPanel.SuspendLayout();
			AboutTabPage.SuspendLayout();
			AboutPanel.SuspendLayout();
			this.TrayMenu.SuspendLayout();
			this.SuspendLayout();
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
			// ContentTabControl
			// 
			ContentTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
			ContentTabControl.Controls.Add(GeneralTabPage);
			ContentTabControl.Controls.Add(ThumbnailTabPage);
			ContentTabControl.Controls.Add(this.ZoomTabPage);
			ContentTabControl.Controls.Add(OverlayTabPage);
			ContentTabControl.Controls.Add(ClientsTabPage);
			ContentTabControl.Controls.Add(AboutTabPage);
			ContentTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			ContentTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			ContentTabControl.ItemSize = new System.Drawing.Size(35, 120);
			ContentTabControl.Location = new System.Drawing.Point(0, 0);
			ContentTabControl.Multiline = true;
			ContentTabControl.Name = "ContentTabControl";
			ContentTabControl.SelectedIndex = 0;
			ContentTabControl.Size = new System.Drawing.Size(390, 218);
			ContentTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			ContentTabControl.TabIndex = 6;
			ContentTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ContentTabControl_DrawItem);
			// 
			// GeneralTabPage
			// 
			GeneralTabPage.BackColor = System.Drawing.SystemColors.Control;
			GeneralTabPage.Controls.Add(GeneralSettingsPanel);
			GeneralTabPage.Location = new System.Drawing.Point(124, 4);
			GeneralTabPage.Name = "GeneralTabPage";
			GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
			GeneralTabPage.Size = new System.Drawing.Size(262, 210);
			GeneralTabPage.TabIndex = 0;
			GeneralTabPage.Text = "General";
			// 
			// GeneralSettingsPanel
			// 
			GeneralSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			GeneralSettingsPanel.Controls.Add(this.EnableClientLayoutTrackingCheckBox);
			GeneralSettingsPanel.Controls.Add(this.HideActiveClientThumbnailCheckBox);
			GeneralSettingsPanel.Controls.Add(this.ShowThumbnailsAlwaysOnTopCheckBox);
			GeneralSettingsPanel.Controls.Add(this.HideThumbnailsOnLostFocusCheckBox);
			GeneralSettingsPanel.Controls.Add(this.EnablePerClientThumbnailsLayoutsCheckBox);
			GeneralSettingsPanel.Controls.Add(this.MinimizeToTrayCheckBox);
			GeneralSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			GeneralSettingsPanel.Location = new System.Drawing.Point(3, 3);
			GeneralSettingsPanel.Name = "GeneralSettingsPanel";
			GeneralSettingsPanel.Size = new System.Drawing.Size(256, 204);
			GeneralSettingsPanel.TabIndex = 18;
			// 
			// EnableClientLayoutTrackingCheckBox
			// 
			this.EnableClientLayoutTrackingCheckBox.AutoSize = true;
			this.EnableClientLayoutTrackingCheckBox.Location = new System.Drawing.Point(8, 31);
			this.EnableClientLayoutTrackingCheckBox.Name = "EnableClientLayoutTrackingCheckBox";
			this.EnableClientLayoutTrackingCheckBox.Size = new System.Drawing.Size(127, 17);
			this.EnableClientLayoutTrackingCheckBox.TabIndex = 19;
			this.EnableClientLayoutTrackingCheckBox.Text = "Track client locations";
			this.EnableClientLayoutTrackingCheckBox.UseVisualStyleBackColor = true;
			this.EnableClientLayoutTrackingCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// HideActiveClientThumbnailCheckBox
			// 
			this.HideActiveClientThumbnailCheckBox.AutoSize = true;
			this.HideActiveClientThumbnailCheckBox.Checked = true;
			this.HideActiveClientThumbnailCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HideActiveClientThumbnailCheckBox.Location = new System.Drawing.Point(8, 55);
			this.HideActiveClientThumbnailCheckBox.Name = "HideActiveClientThumbnailCheckBox";
			this.HideActiveClientThumbnailCheckBox.Size = new System.Drawing.Size(184, 17);
			this.HideActiveClientThumbnailCheckBox.TabIndex = 20;
			this.HideActiveClientThumbnailCheckBox.Text = "Hide preview of active EVE client";
			this.HideActiveClientThumbnailCheckBox.UseVisualStyleBackColor = true;
			this.HideActiveClientThumbnailCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// ShowThumbnailsAlwaysOnTopCheckBox
			// 
			this.ShowThumbnailsAlwaysOnTopCheckBox.AutoSize = true;
			this.ShowThumbnailsAlwaysOnTopCheckBox.Checked = true;
			this.ShowThumbnailsAlwaysOnTopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowThumbnailsAlwaysOnTopCheckBox.Location = new System.Drawing.Point(8, 79);
			this.ShowThumbnailsAlwaysOnTopCheckBox.Name = "ShowThumbnailsAlwaysOnTopCheckBox";
			this.ShowThumbnailsAlwaysOnTopCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowThumbnailsAlwaysOnTopCheckBox.Size = new System.Drawing.Size(137, 17);
			this.ShowThumbnailsAlwaysOnTopCheckBox.TabIndex = 21;
			this.ShowThumbnailsAlwaysOnTopCheckBox.Text = "Previews always on top";
			this.ShowThumbnailsAlwaysOnTopCheckBox.UseVisualStyleBackColor = true;
			this.ShowThumbnailsAlwaysOnTopCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// HideThumbnailsOnLostFocusCheckBox
			// 
			this.HideThumbnailsOnLostFocusCheckBox.AutoSize = true;
			this.HideThumbnailsOnLostFocusCheckBox.Checked = true;
			this.HideThumbnailsOnLostFocusCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HideThumbnailsOnLostFocusCheckBox.Location = new System.Drawing.Point(8, 103);
			this.HideThumbnailsOnLostFocusCheckBox.Name = "HideThumbnailsOnLostFocusCheckBox";
			this.HideThumbnailsOnLostFocusCheckBox.Size = new System.Drawing.Size(234, 17);
			this.HideThumbnailsOnLostFocusCheckBox.TabIndex = 22;
			this.HideThumbnailsOnLostFocusCheckBox.Text = "Hide previews when EVE client is not active";
			this.HideThumbnailsOnLostFocusCheckBox.UseVisualStyleBackColor = true;
			this.HideThumbnailsOnLostFocusCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// EnablePerClientThumbnailsLayoutsCheckBox
			// 
			this.EnablePerClientThumbnailsLayoutsCheckBox.AutoSize = true;
			this.EnablePerClientThumbnailsLayoutsCheckBox.Checked = true;
			this.EnablePerClientThumbnailsLayoutsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EnablePerClientThumbnailsLayoutsCheckBox.Location = new System.Drawing.Point(8, 127);
			this.EnablePerClientThumbnailsLayoutsCheckBox.Name = "EnablePerClientThumbnailsLayoutsCheckBox";
			this.EnablePerClientThumbnailsLayoutsCheckBox.Size = new System.Drawing.Size(185, 17);
			this.EnablePerClientThumbnailsLayoutsCheckBox.TabIndex = 23;
			this.EnablePerClientThumbnailsLayoutsCheckBox.Text = "Unique layout for each EVE client";
			this.EnablePerClientThumbnailsLayoutsCheckBox.UseVisualStyleBackColor = true;
			this.EnablePerClientThumbnailsLayoutsCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// MinimizeToTrayCheckBox
			// 
			this.MinimizeToTrayCheckBox.AutoSize = true;
			this.MinimizeToTrayCheckBox.Location = new System.Drawing.Point(8, 7);
			this.MinimizeToTrayCheckBox.Name = "MinimizeToTrayCheckBox";
			this.MinimizeToTrayCheckBox.Size = new System.Drawing.Size(139, 17);
			this.MinimizeToTrayCheckBox.TabIndex = 18;
			this.MinimizeToTrayCheckBox.Text = "Minimize to System Tray";
			this.MinimizeToTrayCheckBox.UseVisualStyleBackColor = true;
			this.MinimizeToTrayCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// ThumbnailTabPage
			// 
			ThumbnailTabPage.BackColor = System.Drawing.SystemColors.Control;
			ThumbnailTabPage.Controls.Add(ThumbnailSettingsPanel);
			ThumbnailTabPage.Location = new System.Drawing.Point(124, 4);
			ThumbnailTabPage.Name = "ThumbnailTabPage";
			ThumbnailTabPage.Padding = new System.Windows.Forms.Padding(3);
			ThumbnailTabPage.Size = new System.Drawing.Size(262, 210);
			ThumbnailTabPage.TabIndex = 1;
			ThumbnailTabPage.Text = "Thumbnail";
			// 
			// ThumbnailSettingsPanel
			// 
			ThumbnailSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			ThumbnailSettingsPanel.Controls.Add(HeigthLabel);
			ThumbnailSettingsPanel.Controls.Add(WidthLabel);
			ThumbnailSettingsPanel.Controls.Add(this.ThumbnailsWidthNumericEdit);
			ThumbnailSettingsPanel.Controls.Add(this.ThumbnailsHeightNumericEdit);
			ThumbnailSettingsPanel.Controls.Add(this.ThumbnailOpacityTrackBar);
			ThumbnailSettingsPanel.Controls.Add(OpacityLabel);
			ThumbnailSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			ThumbnailSettingsPanel.Location = new System.Drawing.Point(3, 3);
			ThumbnailSettingsPanel.Name = "ThumbnailSettingsPanel";
			ThumbnailSettingsPanel.Size = new System.Drawing.Size(256, 204);
			ThumbnailSettingsPanel.TabIndex = 19;
			// 
			// HeigthLabel
			// 
			HeigthLabel.AutoSize = true;
			HeigthLabel.Location = new System.Drawing.Point(8, 57);
			HeigthLabel.Name = "HeigthLabel";
			HeigthLabel.Size = new System.Drawing.Size(90, 13);
			HeigthLabel.TabIndex = 24;
			HeigthLabel.Text = "Thumbnail Heigth";
			// 
			// WidthLabel
			// 
			WidthLabel.AutoSize = true;
			WidthLabel.Location = new System.Drawing.Point(8, 33);
			WidthLabel.Name = "WidthLabel";
			WidthLabel.Size = new System.Drawing.Size(87, 13);
			WidthLabel.TabIndex = 23;
			WidthLabel.Text = "Thumbnail Width";
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
			this.ThumbnailsWidthNumericEdit.Location = new System.Drawing.Point(105, 31);
			this.ThumbnailsWidthNumericEdit.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.ThumbnailsWidthNumericEdit.Name = "ThumbnailsWidthNumericEdit";
			this.ThumbnailsWidthNumericEdit.Size = new System.Drawing.Size(48, 20);
			this.ThumbnailsWidthNumericEdit.TabIndex = 21;
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
			this.ThumbnailsHeightNumericEdit.Location = new System.Drawing.Point(105, 55);
			this.ThumbnailsHeightNumericEdit.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.ThumbnailsHeightNumericEdit.Name = "ThumbnailsHeightNumericEdit";
			this.ThumbnailsHeightNumericEdit.Size = new System.Drawing.Size(48, 20);
			this.ThumbnailsHeightNumericEdit.TabIndex = 22;
			this.ThumbnailsHeightNumericEdit.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
			this.ThumbnailsHeightNumericEdit.ValueChanged += new System.EventHandler(this.ThumbnailSizeChanged_Handler);
			// 
			// ThumbnailOpacityTrackBar
			// 
			this.ThumbnailOpacityTrackBar.AutoSize = false;
			this.ThumbnailOpacityTrackBar.LargeChange = 10;
			this.ThumbnailOpacityTrackBar.Location = new System.Drawing.Point(61, 6);
			this.ThumbnailOpacityTrackBar.Maximum = 100;
			this.ThumbnailOpacityTrackBar.Minimum = 20;
			this.ThumbnailOpacityTrackBar.Name = "ThumbnailOpacityTrackBar";
			this.ThumbnailOpacityTrackBar.Size = new System.Drawing.Size(191, 22);
			this.ThumbnailOpacityTrackBar.TabIndex = 20;
			this.ThumbnailOpacityTrackBar.TickFrequency = 10;
			this.ThumbnailOpacityTrackBar.Value = 20;
			this.ThumbnailOpacityTrackBar.ValueChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// OpacityLabel
			// 
			OpacityLabel.AutoSize = true;
			OpacityLabel.Location = new System.Drawing.Point(8, 9);
			OpacityLabel.Name = "OpacityLabel";
			OpacityLabel.Size = new System.Drawing.Size(43, 13);
			OpacityLabel.TabIndex = 19;
			OpacityLabel.Text = "Opacity";
			// 
			// ZoomTabPage
			// 
			this.ZoomTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.ZoomTabPage.Controls.Add(ZoomSettingsPanel);
			this.ZoomTabPage.Location = new System.Drawing.Point(124, 4);
			this.ZoomTabPage.Name = "ZoomTabPage";
			this.ZoomTabPage.Size = new System.Drawing.Size(262, 210);
			this.ZoomTabPage.TabIndex = 2;
			this.ZoomTabPage.Text = "Zoom";
			// 
			// ZoomSettingsPanel
			// 
			ZoomSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			ZoomSettingsPanel.Controls.Add(ZoomFactorLabel);
			ZoomSettingsPanel.Controls.Add(this.ZoomAnchorPanel);
			ZoomSettingsPanel.Controls.Add(ZoomAnchorLabel);
			ZoomSettingsPanel.Controls.Add(this.EnableThumbnailZoomCheckBox);
			ZoomSettingsPanel.Controls.Add(this.ThumbnailZoomFactorNumericEdit);
			ZoomSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			ZoomSettingsPanel.Location = new System.Drawing.Point(0, 0);
			ZoomSettingsPanel.Name = "ZoomSettingsPanel";
			ZoomSettingsPanel.Size = new System.Drawing.Size(262, 210);
			ZoomSettingsPanel.TabIndex = 36;
			// 
			// ZoomFactorLabel
			// 
			ZoomFactorLabel.AutoSize = true;
			ZoomFactorLabel.Location = new System.Drawing.Point(8, 33);
			ZoomFactorLabel.Name = "ZoomFactorLabel";
			ZoomFactorLabel.Size = new System.Drawing.Size(67, 13);
			ZoomFactorLabel.TabIndex = 39;
			ZoomFactorLabel.Text = "Zoom Factor";
			// 
			// ZoomAnchorPanel
			// 
			this.ZoomAnchorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorNWRadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorNRadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorNERadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorWRadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorSERadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorCRadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorSRadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorERadioButton);
			this.ZoomAnchorPanel.Controls.Add(this.ZoomAanchorSWRadioButton);
			this.ZoomAnchorPanel.Location = new System.Drawing.Point(81, 54);
			this.ZoomAnchorPanel.Name = "ZoomAnchorPanel";
			this.ZoomAnchorPanel.Size = new System.Drawing.Size(77, 73);
			this.ZoomAnchorPanel.TabIndex = 38;
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
			this.ZoomAanchorNRadioButton.Location = new System.Drawing.Point(31, 3);
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
			this.ZoomAanchorNERadioButton.Location = new System.Drawing.Point(59, 3);
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
			this.ZoomAanchorWRadioButton.Location = new System.Drawing.Point(3, 29);
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
			this.ZoomAanchorSERadioButton.Location = new System.Drawing.Point(59, 55);
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
			this.ZoomAanchorCRadioButton.Location = new System.Drawing.Point(31, 29);
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
			this.ZoomAanchorSRadioButton.Location = new System.Drawing.Point(31, 55);
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
			this.ZoomAanchorERadioButton.Location = new System.Drawing.Point(59, 29);
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
			this.ZoomAanchorSWRadioButton.Location = new System.Drawing.Point(3, 55);
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
			ZoomAnchorLabel.Location = new System.Drawing.Point(8, 57);
			ZoomAnchorLabel.Name = "ZoomAnchorLabel";
			ZoomAnchorLabel.Size = new System.Drawing.Size(41, 13);
			ZoomAnchorLabel.TabIndex = 40;
			ZoomAnchorLabel.Text = "Anchor";
			// 
			// EnableThumbnailZoomCheckBox
			// 
			this.EnableThumbnailZoomCheckBox.AutoSize = true;
			this.EnableThumbnailZoomCheckBox.Checked = true;
			this.EnableThumbnailZoomCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EnableThumbnailZoomCheckBox.Location = new System.Drawing.Point(8, 7);
			this.EnableThumbnailZoomCheckBox.Name = "EnableThumbnailZoomCheckBox";
			this.EnableThumbnailZoomCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.EnableThumbnailZoomCheckBox.Size = new System.Drawing.Size(98, 17);
			this.EnableThumbnailZoomCheckBox.TabIndex = 36;
			this.EnableThumbnailZoomCheckBox.Text = "Zoom on hover";
			this.EnableThumbnailZoomCheckBox.UseVisualStyleBackColor = true;
			this.EnableThumbnailZoomCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// ThumbnailZoomFactorNumericEdit
			// 
			this.ThumbnailZoomFactorNumericEdit.BackColor = System.Drawing.SystemColors.Window;
			this.ThumbnailZoomFactorNumericEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ThumbnailZoomFactorNumericEdit.Location = new System.Drawing.Point(81, 31);
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
			this.ThumbnailZoomFactorNumericEdit.TabIndex = 37;
			this.ThumbnailZoomFactorNumericEdit.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.ThumbnailZoomFactorNumericEdit.ValueChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// OverlayTabPage
			// 
			OverlayTabPage.BackColor = System.Drawing.SystemColors.Control;
			OverlayTabPage.Controls.Add(OverlaySettingsPanel);
			OverlayTabPage.Location = new System.Drawing.Point(124, 4);
			OverlayTabPage.Name = "OverlayTabPage";
			OverlayTabPage.Size = new System.Drawing.Size(262, 210);
			OverlayTabPage.TabIndex = 3;
			OverlayTabPage.Text = "Overlay";
			// 
			// OverlaySettingsPanel
			// 
			OverlaySettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			OverlaySettingsPanel.Controls.Add(this.HighlightColorLabel);
			OverlaySettingsPanel.Controls.Add(this.ActiveClientHighlightColorButton);
			OverlaySettingsPanel.Controls.Add(this.EnableActiveClientHighlightCheckBox);
			OverlaySettingsPanel.Controls.Add(this.ShowThumbnailOverlaysCheckBox);
			OverlaySettingsPanel.Controls.Add(this.ShowThumbnailFramesCheckBox);
			OverlaySettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			OverlaySettingsPanel.Location = new System.Drawing.Point(0, 0);
			OverlaySettingsPanel.Name = "OverlaySettingsPanel";
			OverlaySettingsPanel.Size = new System.Drawing.Size(262, 210);
			OverlaySettingsPanel.TabIndex = 25;
			// 
			// HighlightColorLabel
			// 
			this.HighlightColorLabel.AutoSize = true;
			this.HighlightColorLabel.Location = new System.Drawing.Point(5, 78);
			this.HighlightColorLabel.Name = "HighlightColorLabel";
			this.HighlightColorLabel.Size = new System.Drawing.Size(31, 13);
			this.HighlightColorLabel.TabIndex = 29;
			this.HighlightColorLabel.Text = "Color";
			// 
			// ActiveClientHighlightColorButton
			// 
			this.ActiveClientHighlightColorButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ActiveClientHighlightColorButton.Location = new System.Drawing.Point(42, 77);
			this.ActiveClientHighlightColorButton.Name = "ActiveClientHighlightColorButton";
			this.ActiveClientHighlightColorButton.Size = new System.Drawing.Size(93, 17);
			this.ActiveClientHighlightColorButton.TabIndex = 28;
			this.ActiveClientHighlightColorButton.Click += new System.EventHandler(this.ActiveClientHighlightColorButton_Click);
			// 
			// EnableActiveClientHighlightCheckBox
			// 
			this.EnableActiveClientHighlightCheckBox.AutoSize = true;
			this.EnableActiveClientHighlightCheckBox.Checked = true;
			this.EnableActiveClientHighlightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EnableActiveClientHighlightCheckBox.Location = new System.Drawing.Point(8, 55);
			this.EnableActiveClientHighlightCheckBox.Name = "EnableActiveClientHighlightCheckBox";
			this.EnableActiveClientHighlightCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.EnableActiveClientHighlightCheckBox.Size = new System.Drawing.Size(127, 17);
			this.EnableActiveClientHighlightCheckBox.TabIndex = 27;
			this.EnableActiveClientHighlightCheckBox.Text = "Highlight active client";
			this.EnableActiveClientHighlightCheckBox.UseVisualStyleBackColor = true;
			this.EnableActiveClientHighlightCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// ShowThumbnailOverlaysCheckBox
			// 
			this.ShowThumbnailOverlaysCheckBox.AutoSize = true;
			this.ShowThumbnailOverlaysCheckBox.Checked = true;
			this.ShowThumbnailOverlaysCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowThumbnailOverlaysCheckBox.Location = new System.Drawing.Point(8, 7);
			this.ShowThumbnailOverlaysCheckBox.Name = "ShowThumbnailOverlaysCheckBox";
			this.ShowThumbnailOverlaysCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowThumbnailOverlaysCheckBox.Size = new System.Drawing.Size(90, 17);
			this.ShowThumbnailOverlaysCheckBox.TabIndex = 25;
			this.ShowThumbnailOverlaysCheckBox.Text = "Show overlay";
			this.ShowThumbnailOverlaysCheckBox.UseVisualStyleBackColor = true;
			this.ShowThumbnailOverlaysCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// ShowThumbnailFramesCheckBox
			// 
			this.ShowThumbnailFramesCheckBox.AutoSize = true;
			this.ShowThumbnailFramesCheckBox.Checked = true;
			this.ShowThumbnailFramesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowThumbnailFramesCheckBox.Location = new System.Drawing.Point(8, 31);
			this.ShowThumbnailFramesCheckBox.Name = "ShowThumbnailFramesCheckBox";
			this.ShowThumbnailFramesCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowThumbnailFramesCheckBox.Size = new System.Drawing.Size(87, 17);
			this.ShowThumbnailFramesCheckBox.TabIndex = 26;
			this.ShowThumbnailFramesCheckBox.Text = "Show frames";
			this.ShowThumbnailFramesCheckBox.UseVisualStyleBackColor = true;
			this.ShowThumbnailFramesCheckBox.CheckedChanged += new System.EventHandler(this.OptionChanged_Handler);
			// 
			// ClientsTabPage
			// 
			ClientsTabPage.BackColor = System.Drawing.SystemColors.Control;
			ClientsTabPage.Controls.Add(ClientsPanel);
			ClientsTabPage.Location = new System.Drawing.Point(124, 4);
			ClientsTabPage.Name = "ClientsTabPage";
			ClientsTabPage.Size = new System.Drawing.Size(262, 210);
			ClientsTabPage.TabIndex = 4;
			ClientsTabPage.Text = "Active Clients";
			// 
			// ClientsPanel
			// 
			ClientsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			ClientsPanel.Controls.Add(this.ThumbnailsList);
			ClientsPanel.Controls.Add(ThumbnailsListLabel);
			ClientsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			ClientsPanel.Location = new System.Drawing.Point(0, 0);
			ClientsPanel.Name = "ClientsPanel";
			ClientsPanel.Size = new System.Drawing.Size(262, 210);
			ClientsPanel.TabIndex = 32;
			// 
			// ThumbnailsList
			// 
			this.ThumbnailsList.BackColor = System.Drawing.SystemColors.Window;
			this.ThumbnailsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ThumbnailsList.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ThumbnailsList.FormattingEnabled = true;
			this.ThumbnailsList.IntegralHeight = false;
			this.ThumbnailsList.Location = new System.Drawing.Point(0, 28);
			this.ThumbnailsList.Name = "ThumbnailsList";
			this.ThumbnailsList.Size = new System.Drawing.Size(260, 180);
			this.ThumbnailsList.TabIndex = 34;
			this.ThumbnailsList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ThumbnailsList_ItemCheck_Handler);
			// 
			// ThumbnailsListLabel
			// 
			ThumbnailsListLabel.AutoSize = true;
			ThumbnailsListLabel.Location = new System.Drawing.Point(8, 9);
			ThumbnailsListLabel.Name = "ThumbnailsListLabel";
			ThumbnailsListLabel.Size = new System.Drawing.Size(162, 13);
			ThumbnailsListLabel.TabIndex = 33;
			ThumbnailsListLabel.Text = "Thumbnails (check to force hide)";
			// 
			// AboutTabPage
			// 
			AboutTabPage.BackColor = System.Drawing.SystemColors.Control;
			AboutTabPage.Controls.Add(AboutPanel);
			AboutTabPage.Location = new System.Drawing.Point(124, 4);
			AboutTabPage.Name = "AboutTabPage";
			AboutTabPage.Size = new System.Drawing.Size(262, 210);
			AboutTabPage.TabIndex = 5;
			AboutTabPage.Text = "About";
			// 
			// AboutPanel
			// 
			AboutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			AboutPanel.Controls.Add(DocumentationLinkLabel);
			AboutPanel.Controls.Add(DescriptionLabel);
			AboutPanel.Controls.Add(this.VersionLabel);
			AboutPanel.Controls.Add(NameLabel);
			AboutPanel.Controls.Add(this.DocumentationLink);
			AboutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			AboutPanel.Location = new System.Drawing.Point(0, 0);
			AboutPanel.Name = "AboutPanel";
			AboutPanel.Size = new System.Drawing.Size(262, 210);
			AboutPanel.TabIndex = 2;
			// 
			// DocumentationLinkLabel
			// 
			DocumentationLinkLabel.AutoSize = true;
			DocumentationLinkLabel.Location = new System.Drawing.Point(0, 157);
			DocumentationLinkLabel.Name = "DocumentationLinkLabel";
			DocumentationLinkLabel.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
			DocumentationLinkLabel.Size = new System.Drawing.Size(222, 19);
			DocumentationLinkLabel.TabIndex = 6;
			DocumentationLinkLabel.Text = "For more information visit our forum thread:";
			// 
			// DescriptionLabel
			// 
			DescriptionLabel.Location = new System.Drawing.Point(0, 41);
			DescriptionLabel.Name = "DescriptionLabel";
			DescriptionLabel.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
			DescriptionLabel.Size = new System.Drawing.Size(261, 116);
			DescriptionLabel.TabIndex = 5;
			DescriptionLabel.Text = resources.GetString("DescriptionLabel.Text");
			// 
			// VersionLabel
			// 
			this.VersionLabel.AutoSize = true;
			this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.VersionLabel.Location = new System.Drawing.Point(133, 9);
			this.VersionLabel.Name = "VersionLabel";
			this.VersionLabel.Size = new System.Drawing.Size(49, 20);
			this.VersionLabel.TabIndex = 4;
			this.VersionLabel.Text = "1.0.0";
			// 
			// NameLabel
			// 
			NameLabel.AutoSize = true;
			NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			NameLabel.Location = new System.Drawing.Point(4, 9);
			NameLabel.Name = "NameLabel";
			NameLabel.Size = new System.Drawing.Size(130, 20);
			NameLabel.TabIndex = 3;
			NameLabel.Text = "EVE-O Preview";
			// 
			// DocumentationLink
			// 
			this.DocumentationLink.Location = new System.Drawing.Point(0, 173);
			this.DocumentationLink.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
			this.DocumentationLink.Name = "DocumentationLink";
			this.DocumentationLink.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
			this.DocumentationLink.Size = new System.Drawing.Size(262, 33);
			this.DocumentationLink.TabIndex = 2;
			this.DocumentationLink.TabStop = true;
			this.DocumentationLink.Text = "to be set from prresenter to be set from prresenter to be set from prresenter to " +
    "be set from prresenter";
			this.DocumentationLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DocumentationLinkClicked_Handler);
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
			this.ClientSize = new System.Drawing.Size(390, 218);
			this.Controls.Add(ContentTabControl);
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
			ContentTabControl.ResumeLayout(false);
			GeneralTabPage.ResumeLayout(false);
			GeneralSettingsPanel.ResumeLayout(false);
			GeneralSettingsPanel.PerformLayout();
			ThumbnailTabPage.ResumeLayout(false);
			ThumbnailSettingsPanel.ResumeLayout(false);
			ThumbnailSettingsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailsWidthNumericEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailsHeightNumericEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailOpacityTrackBar)).EndInit();
			this.ZoomTabPage.ResumeLayout(false);
			ZoomSettingsPanel.ResumeLayout(false);
			ZoomSettingsPanel.PerformLayout();
			this.ZoomAnchorPanel.ResumeLayout(false);
			this.ZoomAnchorPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ThumbnailZoomFactorNumericEdit)).EndInit();
			OverlayTabPage.ResumeLayout(false);
			OverlaySettingsPanel.ResumeLayout(false);
			OverlaySettingsPanel.PerformLayout();
			ClientsTabPage.ResumeLayout(false);
			ClientsPanel.ResumeLayout(false);
			ClientsPanel.PerformLayout();
			AboutTabPage.ResumeLayout(false);
			AboutPanel.ResumeLayout(false);
			AboutPanel.PerformLayout();
			this.TrayMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private NotifyIcon NotifyIcon;
		private ContextMenuStrip TrayMenu;
		private TabPage ZoomTabPage;
		private CheckBox EnableClientLayoutTrackingCheckBox;
		private CheckBox HideActiveClientThumbnailCheckBox;
		private CheckBox ShowThumbnailsAlwaysOnTopCheckBox;
		private CheckBox HideThumbnailsOnLostFocusCheckBox;
		private CheckBox EnablePerClientThumbnailsLayoutsCheckBox;
		private CheckBox MinimizeToTrayCheckBox;
		private NumericUpDown ThumbnailsWidthNumericEdit;
		private NumericUpDown ThumbnailsHeightNumericEdit;
		private TrackBar ThumbnailOpacityTrackBar;
		private Panel ZoomAnchorPanel;
		private RadioButton ZoomAanchorNWRadioButton;
		private RadioButton ZoomAanchorNRadioButton;
		private RadioButton ZoomAanchorNERadioButton;
		private RadioButton ZoomAanchorWRadioButton;
		private RadioButton ZoomAanchorSERadioButton;
		private RadioButton ZoomAanchorCRadioButton;
		private RadioButton ZoomAanchorSRadioButton;
		private RadioButton ZoomAanchorERadioButton;
		private RadioButton ZoomAanchorSWRadioButton;
		private CheckBox EnableThumbnailZoomCheckBox;
		private NumericUpDown ThumbnailZoomFactorNumericEdit;
		private Label HighlightColorLabel;
		private Panel ActiveClientHighlightColorButton;
		private CheckBox EnableActiveClientHighlightCheckBox;
		private CheckBox ShowThumbnailOverlaysCheckBox;
		private CheckBox ShowThumbnailFramesCheckBox;
		private CheckedListBox ThumbnailsList;
		private LinkLabel DocumentationLink;
		private Label VersionLabel;
	}
}