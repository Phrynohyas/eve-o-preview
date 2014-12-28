using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace PreviewToy
{
    partial class PreviewToyHandler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewToyHandler));
            this.option_hide_active = new System.Windows.Forms.CheckBox();
            this.option_hide_all_if_not_right_type = new System.Windows.Forms.CheckBox();
            this.option_unique_layout = new System.Windows.Forms.CheckBox();
            this.option_sync_size = new System.Windows.Forms.CheckBox();
            this.option_always_on_top = new System.Windows.Forms.CheckBox();
            this.option_show_thumbnail_frames = new System.Windows.Forms.CheckBox();
            this.forum_url = new System.Windows.Forms.LinkLabel();
            this.option_sync_size_x = new System.Windows.Forms.TextBox();
            this.option_sync_size_y = new System.Windows.Forms.TextBox();
            this.option_zoom_on_hover = new System.Windows.Forms.CheckBox();
            this.option_show_overlay = new System.Windows.Forms.CheckBox();
            this.option_zoom_anchor_NW = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_N = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_NE = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_W = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_C = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_E = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_SW = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_S = new System.Windows.Forms.RadioButton();
            this.option_zoom_anchor_SE = new System.Windows.Forms.RadioButton();
            this.option_zoom_factor = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.opacity_bar = new System.Windows.Forms.HScrollBar();
            this.opacity_label = new System.Windows.Forms.Label();
            this.option_track_client_windows = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.zoom_anchor_lable = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.previews_check_listbox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.aero_status_label = new System.Windows.Forms.Label();
            this.previewToyMainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewToyMainBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // option_hide_active
            // 
            this.option_hide_active.AutoSize = true;
            this.option_hide_active.Checked = true;
            this.option_hide_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_hide_active.Location = new System.Drawing.Point(3, 58);
            this.option_hide_active.Name = "option_hide_active";
            this.option_hide_active.Size = new System.Drawing.Size(184, 17);
            this.option_hide_active.TabIndex = 1;
            this.option_hide_active.Text = "Hide preview of active EVE client";
            this.option_hide_active.UseVisualStyleBackColor = true;
            this.option_hide_active.CheckedChanged += new System.EventHandler(this.option_hide_active_CheckedChanged);
            // 
            // option_hide_all_if_not_right_type
            // 
            this.option_hide_all_if_not_right_type.AutoSize = true;
            this.option_hide_all_if_not_right_type.Checked = true;
            this.option_hide_all_if_not_right_type.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_hide_all_if_not_right_type.Location = new System.Drawing.Point(3, 104);
            this.option_hide_all_if_not_right_type.Name = "option_hide_all_if_not_right_type";
            this.option_hide_all_if_not_right_type.Size = new System.Drawing.Size(242, 17);
            this.option_hide_all_if_not_right_type.TabIndex = 2;
            this.option_hide_all_if_not_right_type.Text = "Hide previews if active window not EVE client";
            this.option_hide_all_if_not_right_type.UseVisualStyleBackColor = true;
            this.option_hide_all_if_not_right_type.CheckedChanged += new System.EventHandler(this.option_hide_all_if_noneve_CheckedChanged);
            // 
            // option_unique_layout
            // 
            this.option_unique_layout.AutoSize = true;
            this.option_unique_layout.Checked = true;
            this.option_unique_layout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_unique_layout.Location = new System.Drawing.Point(3, 127);
            this.option_unique_layout.Name = "option_unique_layout";
            this.option_unique_layout.Size = new System.Drawing.Size(185, 17);
            this.option_unique_layout.TabIndex = 3;
            this.option_unique_layout.Text = "Unique layout for each EVE client";
            this.option_unique_layout.UseVisualStyleBackColor = true;
            this.option_unique_layout.CheckedChanged += new System.EventHandler(this.option_unique_layout_CheckedChanged);
            // 
            // option_sync_size
            // 
            this.option_sync_size.AutoSize = true;
            this.option_sync_size.Checked = true;
            this.option_sync_size.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_sync_size.Location = new System.Drawing.Point(1, 3);
            this.option_sync_size.Name = "option_sync_size";
            this.option_sync_size.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_sync_size.Size = new System.Drawing.Size(108, 17);
            this.option_sync_size.TabIndex = 4;
            this.option_sync_size.Text = "Syncronize resize";
            this.option_sync_size.UseVisualStyleBackColor = true;
            this.option_sync_size.CheckedChanged += new System.EventHandler(this.option_sync_size_CheckedChanged);
            // 
            // option_always_on_top
            // 
            this.option_always_on_top.AutoSize = true;
            this.option_always_on_top.Checked = true;
            this.option_always_on_top.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_always_on_top.Location = new System.Drawing.Point(3, 81);
            this.option_always_on_top.Name = "option_always_on_top";
            this.option_always_on_top.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_always_on_top.Size = new System.Drawing.Size(137, 17);
            this.option_always_on_top.TabIndex = 8;
            this.option_always_on_top.Text = "Previews always on top";
            this.option_always_on_top.UseVisualStyleBackColor = true;
            this.option_always_on_top.CheckedChanged += new System.EventHandler(this.option_always_on_top_CheckedChanged);
            // 
            // option_show_thumbnail_frames
            // 
            this.option_show_thumbnail_frames.AutoSize = true;
            this.option_show_thumbnail_frames.Checked = true;
            this.option_show_thumbnail_frames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_show_thumbnail_frames.Location = new System.Drawing.Point(99, 250);
            this.option_show_thumbnail_frames.Name = "option_show_thumbnail_frames";
            this.option_show_thumbnail_frames.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_show_thumbnail_frames.Size = new System.Drawing.Size(127, 17);
            this.option_show_thumbnail_frames.TabIndex = 9;
            this.option_show_thumbnail_frames.Text = "Show preview frames";
            this.option_show_thumbnail_frames.UseVisualStyleBackColor = true;
            this.option_show_thumbnail_frames.CheckedChanged += new System.EventHandler(this.option_show_thumbnail_frames_CheckedChanged);
            // 
            // forum_url
            // 
            this.forum_url.AutoSize = true;
            this.forum_url.Location = new System.Drawing.Point(3, 403);
            this.forum_url.Name = "forum_url";
            this.forum_url.Size = new System.Drawing.Size(216, 13);
            this.forum_url.TabIndex = 10;
            this.forum_url.TabStop = true;
            this.forum_url.Text = "https://bitbucket.org/ulph/eve-o-preview-git";
            this.forum_url.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // option_sync_size_x
            // 
            this.option_sync_size_x.BackColor = System.Drawing.SystemColors.Window;
            this.option_sync_size_x.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.option_sync_size_x.Location = new System.Drawing.Point(137, 3);
            this.option_sync_size_x.Name = "option_sync_size_x";
            this.option_sync_size_x.Size = new System.Drawing.Size(42, 20);
            this.option_sync_size_x.TabIndex = 11;
            this.option_sync_size_x.TextChanged += new System.EventHandler(this.option_sync_size_x_TextChanged);
            // 
            // option_sync_size_y
            // 
            this.option_sync_size_y.BackColor = System.Drawing.SystemColors.Window;
            this.option_sync_size_y.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.option_sync_size_y.Location = new System.Drawing.Point(196, 3);
            this.option_sync_size_y.Name = "option_sync_size_y";
            this.option_sync_size_y.Size = new System.Drawing.Size(42, 20);
            this.option_sync_size_y.TabIndex = 12;
            this.option_sync_size_y.TextChanged += new System.EventHandler(this.option_sync_size_y_TextChanged);
            // 
            // option_zoom_on_hover
            // 
            this.option_zoom_on_hover.AutoSize = true;
            this.option_zoom_on_hover.Checked = true;
            this.option_zoom_on_hover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_zoom_on_hover.Location = new System.Drawing.Point(1, 5);
            this.option_zoom_on_hover.Name = "option_zoom_on_hover";
            this.option_zoom_on_hover.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_zoom_on_hover.Size = new System.Drawing.Size(98, 17);
            this.option_zoom_on_hover.TabIndex = 13;
            this.option_zoom_on_hover.Text = "Zoom on hover";
            this.option_zoom_on_hover.UseVisualStyleBackColor = true;
            this.option_zoom_on_hover.CheckedChanged += new System.EventHandler(this.option_zoom_on_hover_CheckedChanged);
            // 
            // option_show_overlay
            // 
            this.option_show_overlay.AutoSize = true;
            this.option_show_overlay.Checked = true;
            this.option_show_overlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_show_overlay.Location = new System.Drawing.Point(3, 250);
            this.option_show_overlay.Name = "option_show_overlay";
            this.option_show_overlay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_show_overlay.Size = new System.Drawing.Size(90, 17);
            this.option_show_overlay.TabIndex = 14;
            this.option_show_overlay.Text = "Show overlay";
            this.option_show_overlay.UseVisualStyleBackColor = true;
            this.option_show_overlay.CheckedChanged += new System.EventHandler(this.option_show_overlay_CheckedChanged);
            // 
            // option_zoom_anchor_NW
            // 
            this.option_zoom_anchor_NW.AutoSize = true;
            this.option_zoom_anchor_NW.Location = new System.Drawing.Point(3, 3);
            this.option_zoom_anchor_NW.Name = "option_zoom_anchor_NW";
            this.option_zoom_anchor_NW.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_NW.TabIndex = 15;
            this.option_zoom_anchor_NW.TabStop = true;
            this.option_zoom_anchor_NW.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_NW.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_N
            // 
            this.option_zoom_anchor_N.AutoSize = true;
            this.option_zoom_anchor_N.Location = new System.Drawing.Point(23, 3);
            this.option_zoom_anchor_N.Name = "option_zoom_anchor_N";
            this.option_zoom_anchor_N.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_N.TabIndex = 16;
            this.option_zoom_anchor_N.TabStop = true;
            this.option_zoom_anchor_N.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_N.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_NE
            // 
            this.option_zoom_anchor_NE.AutoSize = true;
            this.option_zoom_anchor_NE.Location = new System.Drawing.Point(43, 3);
            this.option_zoom_anchor_NE.Name = "option_zoom_anchor_NE";
            this.option_zoom_anchor_NE.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_NE.TabIndex = 17;
            this.option_zoom_anchor_NE.TabStop = true;
            this.option_zoom_anchor_NE.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_NE.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_W
            // 
            this.option_zoom_anchor_W.AutoSize = true;
            this.option_zoom_anchor_W.Location = new System.Drawing.Point(3, 22);
            this.option_zoom_anchor_W.Name = "option_zoom_anchor_W";
            this.option_zoom_anchor_W.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_W.TabIndex = 18;
            this.option_zoom_anchor_W.TabStop = true;
            this.option_zoom_anchor_W.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_W.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_C
            // 
            this.option_zoom_anchor_C.AutoSize = true;
            this.option_zoom_anchor_C.Location = new System.Drawing.Point(23, 22);
            this.option_zoom_anchor_C.Name = "option_zoom_anchor_C";
            this.option_zoom_anchor_C.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_C.TabIndex = 19;
            this.option_zoom_anchor_C.TabStop = true;
            this.option_zoom_anchor_C.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_C.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_E
            // 
            this.option_zoom_anchor_E.AutoSize = true;
            this.option_zoom_anchor_E.Location = new System.Drawing.Point(43, 22);
            this.option_zoom_anchor_E.Name = "option_zoom_anchor_E";
            this.option_zoom_anchor_E.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_E.TabIndex = 20;
            this.option_zoom_anchor_E.TabStop = true;
            this.option_zoom_anchor_E.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_E.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_SW
            // 
            this.option_zoom_anchor_SW.AutoSize = true;
            this.option_zoom_anchor_SW.Location = new System.Drawing.Point(3, 41);
            this.option_zoom_anchor_SW.Name = "option_zoom_anchor_SW";
            this.option_zoom_anchor_SW.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_SW.TabIndex = 21;
            this.option_zoom_anchor_SW.TabStop = true;
            this.option_zoom_anchor_SW.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_SW.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_S
            // 
            this.option_zoom_anchor_S.AutoSize = true;
            this.option_zoom_anchor_S.Location = new System.Drawing.Point(23, 41);
            this.option_zoom_anchor_S.Name = "option_zoom_anchor_S";
            this.option_zoom_anchor_S.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_S.TabIndex = 22;
            this.option_zoom_anchor_S.TabStop = true;
            this.option_zoom_anchor_S.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_S.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_anchor_SE
            // 
            this.option_zoom_anchor_SE.AutoSize = true;
            this.option_zoom_anchor_SE.Location = new System.Drawing.Point(43, 41);
            this.option_zoom_anchor_SE.Name = "option_zoom_anchor_SE";
            this.option_zoom_anchor_SE.Size = new System.Drawing.Size(14, 13);
            this.option_zoom_anchor_SE.TabIndex = 23;
            this.option_zoom_anchor_SE.TabStop = true;
            this.option_zoom_anchor_SE.UseVisualStyleBackColor = true;
            this.option_zoom_anchor_SE.CheckedChanged += new System.EventHandler(this.option_zoom_anchor_X_CheckedChanged);
            // 
            // option_zoom_factor
            // 
            this.option_zoom_factor.BackColor = System.Drawing.SystemColors.Window;
            this.option_zoom_factor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.option_zoom_factor.Location = new System.Drawing.Point(9, 28);
            this.option_zoom_factor.Name = "option_zoom_factor";
            this.option_zoom_factor.Size = new System.Drawing.Size(28, 20);
            this.option_zoom_factor.TabIndex = 24;
            this.option_zoom_factor.TextChanged += new System.EventHandler(this.option_zoom_factor_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.panel6);
            this.flowLayoutPanel1.Controls.Add(this.option_track_client_windows);
            this.flowLayoutPanel1.Controls.Add(this.option_hide_active);
            this.flowLayoutPanel1.Controls.Add(this.option_always_on_top);
            this.flowLayoutPanel1.Controls.Add(this.option_hide_all_if_not_right_type);
            this.flowLayoutPanel1.Controls.Add(this.option_unique_layout);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.option_show_overlay);
            this.flowLayoutPanel1.Controls.Add(this.option_show_thumbnail_frames);
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Controls.Add(this.forum_url);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(252, 423);
            this.flowLayoutPanel1.TabIndex = 25;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.opacity_bar);
            this.panel6.Controls.Add(this.opacity_label);
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(246, 26);
            this.panel6.TabIndex = 33;
            // 
            // opacity_bar
            // 
            this.opacity_bar.Location = new System.Drawing.Point(48, 1);
            this.opacity_bar.Maximum = 120;
            this.opacity_bar.Name = "opacity_bar";
            this.opacity_bar.Size = new System.Drawing.Size(195, 23);
            this.opacity_bar.TabIndex = 1;
            this.opacity_bar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.opacity_bar_Scroll);
            // 
            // opacity_label
            // 
            this.opacity_label.AutoSize = true;
            this.opacity_label.Location = new System.Drawing.Point(2, 5);
            this.opacity_label.Name = "opacity_label";
            this.opacity_label.Size = new System.Drawing.Size(43, 13);
            this.opacity_label.TabIndex = 0;
            this.opacity_label.Text = "Opacity";
            // 
            // option_track_client_windows
            // 
            this.option_track_client_windows.AutoSize = true;
            this.option_track_client_windows.Location = new System.Drawing.Point(3, 35);
            this.option_track_client_windows.Name = "option_track_client_windows";
            this.option_track_client_windows.Size = new System.Drawing.Size(127, 17);
            this.option_track_client_windows.TabIndex = 32;
            this.option_track_client_windows.Text = "Track client locations";
            this.option_track_client_windows.UseVisualStyleBackColor = true;
            this.option_track_client_windows.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.option_sync_size);
            this.panel1.Controls.Add(this.option_sync_size_x);
            this.panel1.Controls.Add(this.option_sync_size_y);
            this.panel1.Location = new System.Drawing.Point(3, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 26);
            this.panel1.TabIndex = 26;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.zoom_anchor_lable);
            this.panel2.Controls.Add(this.option_zoom_on_hover);
            this.panel2.Controls.Add(this.option_zoom_factor);
            this.panel2.Location = new System.Drawing.Point(3, 182);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 62);
            this.panel2.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Factor";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.option_zoom_anchor_NW);
            this.panel3.Controls.Add(this.option_zoom_anchor_N);
            this.panel3.Controls.Add(this.option_zoom_anchor_NE);
            this.panel3.Controls.Add(this.option_zoom_anchor_W);
            this.panel3.Controls.Add(this.option_zoom_anchor_SE);
            this.panel3.Controls.Add(this.option_zoom_anchor_C);
            this.panel3.Controls.Add(this.option_zoom_anchor_S);
            this.panel3.Controls.Add(this.option_zoom_anchor_E);
            this.panel3.Controls.Add(this.option_zoom_anchor_SW);
            this.panel3.Location = new System.Drawing.Point(182, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(60, 57);
            this.panel3.TabIndex = 28;
            // 
            // zoom_anchor_lable
            // 
            this.zoom_anchor_lable.AutoSize = true;
            this.zoom_anchor_lable.Location = new System.Drawing.Point(134, 31);
            this.zoom_anchor_lable.Name = "zoom_anchor_lable";
            this.zoom_anchor_lable.Size = new System.Drawing.Size(41, 13);
            this.zoom_anchor_lable.TabIndex = 30;
            this.zoom_anchor_lable.Text = "Anchor";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.previews_check_listbox);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Location = new System.Drawing.Point(3, 273);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(246, 100);
            this.panel5.TabIndex = 31;
            // 
            // previews_check_listbox
            // 
            this.previews_check_listbox.BackColor = System.Drawing.SystemColors.Window;
            this.previews_check_listbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.previews_check_listbox.FormattingEnabled = true;
            this.previews_check_listbox.Location = new System.Drawing.Point(3, 18);
            this.previews_check_listbox.Name = "previews_check_listbox";
            this.previews_check_listbox.Size = new System.Drawing.Size(240, 75);
            this.previews_check_listbox.TabIndex = 28;
            this.previews_check_listbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_SelectedIndexChanged2);
            this.previews_check_listbox.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Previews (check to force hide)";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.aero_status_label);
            this.panel4.Location = new System.Drawing.Point(3, 379);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(246, 21);
            this.panel4.TabIndex = 30;
            // 
            // aero_status_label
            // 
            this.aero_status_label.AutoSize = true;
            this.aero_status_label.Location = new System.Drawing.Point(3, 4);
            this.aero_status_label.Name = "aero_status_label";
            this.aero_status_label.Size = new System.Drawing.Size(35, 13);
            this.aero_status_label.TabIndex = 0;
            this.aero_status_label.Text = "label4";
            // 
            // previewToyMainBindingSource
            // 
            this.previewToyMainBindingSource.CurrentChanged += new System.EventHandler(this.previewToyMainBindingSource_CurrentChanged);
            // 
            // PreviewToyHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(252, 423);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "PreviewToyHandler";
            this.Text = "EVE Online previewer";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.GlassForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewToyMainBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CheckBox option_hide_active;
        private CheckBox option_hide_all_if_not_right_type;
        private CheckBox option_unique_layout;
        private CheckBox option_sync_size;
        private BindingSource previewToyMainBindingSource;
        private CheckBox option_always_on_top;
        private CheckBox option_show_thumbnail_frames;
        private LinkLabel forum_url;
        private TextBox option_sync_size_x;
        private TextBox option_sync_size_y;
        private CheckBox option_zoom_on_hover;
        private CheckBox option_show_overlay;
        private RadioButton option_zoom_anchor_NW;
        private RadioButton option_zoom_anchor_N;
        private RadioButton option_zoom_anchor_NE;
        private RadioButton option_zoom_anchor_W;
        private RadioButton option_zoom_anchor_C;
        private RadioButton option_zoom_anchor_E;
        private RadioButton option_zoom_anchor_SW;
        private RadioButton option_zoom_anchor_S;
        private RadioButton option_zoom_anchor_SE;
        private TextBox option_zoom_factor;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private Label label2;
        private Panel panel3;
        private Label zoom_anchor_lable;
        private CheckedListBox previews_check_listbox;
        private Label label1;
        private Panel panel4;
        private Label aero_status_label;
        private Panel panel5;
        private CheckBox option_track_client_windows;
        private Panel panel6;
        private Label opacity_label;
        private HScrollBar opacity_bar;


    }
}

