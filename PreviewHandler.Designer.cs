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
            this.previewToyMainBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.previews_check_listbox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewToyMainBindingSource)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // option_hide_active
            // 
            this.option_hide_active.AutoSize = true;
            this.option_hide_active.Checked = true;
            this.option_hide_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_hide_active.Location = new System.Drawing.Point(3, 3);
            this.option_hide_active.Name = "option_hide_active";
            this.option_hide_active.Size = new System.Drawing.Size(81, 17);
            this.option_hide_active.TabIndex = 1;
            this.option_hide_active.Text = "Hide Active";
            this.option_hide_active.UseVisualStyleBackColor = true;
            this.option_hide_active.CheckedChanged += new System.EventHandler(this.option_hide_active_CheckedChanged);
            // 
            // option_hide_all_if_not_right_type
            // 
            this.option_hide_all_if_not_right_type.AutoSize = true;
            this.option_hide_all_if_not_right_type.Checked = true;
            this.option_hide_all_if_not_right_type.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_hide_all_if_not_right_type.Location = new System.Drawing.Point(3, 26);
            this.option_hide_all_if_not_right_type.Name = "option_hide_all_if_not_right_type";
            this.option_hide_all_if_not_right_type.Size = new System.Drawing.Size(210, 17);
            this.option_hide_all_if_not_right_type.TabIndex = 2;
            this.option_hide_all_if_not_right_type.Text = "Hide all if active window not EVE client";
            this.option_hide_all_if_not_right_type.UseVisualStyleBackColor = true;
            this.option_hide_all_if_not_right_type.CheckedChanged += new System.EventHandler(this.option_hide_all_if_noneve_CheckedChanged);
            // 
            // option_unique_layout
            // 
            this.option_unique_layout.AutoSize = true;
            this.option_unique_layout.Checked = true;
            this.option_unique_layout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_unique_layout.Location = new System.Drawing.Point(3, 49);
            this.option_unique_layout.Name = "option_unique_layout";
            this.option_unique_layout.Size = new System.Drawing.Size(161, 17);
            this.option_unique_layout.TabIndex = 3;
            this.option_unique_layout.Text = "Unique layout for each client";
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
            this.option_sync_size.Size = new System.Drawing.Size(113, 17);
            this.option_sync_size.TabIndex = 4;
            this.option_sync_size.Text = "Syncronize Resize";
            this.option_sync_size.UseVisualStyleBackColor = true;
            this.option_sync_size.CheckedChanged += new System.EventHandler(this.option_sync_size_CheckedChanged);
            // 
            // option_always_on_top
            // 
            this.option_always_on_top.AutoSize = true;
            this.option_always_on_top.Checked = true;
            this.option_always_on_top.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_always_on_top.Location = new System.Drawing.Point(90, 3);
            this.option_always_on_top.Name = "option_always_on_top";
            this.option_always_on_top.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_always_on_top.Size = new System.Drawing.Size(92, 17);
            this.option_always_on_top.TabIndex = 8;
            this.option_always_on_top.Text = "Always on top";
            this.option_always_on_top.UseVisualStyleBackColor = true;
            this.option_always_on_top.CheckedChanged += new System.EventHandler(this.option_always_on_top_CheckedChanged);
            // 
            // option_show_thumbnail_frames
            // 
            this.option_show_thumbnail_frames.AutoSize = true;
            this.option_show_thumbnail_frames.Checked = true;
            this.option_show_thumbnail_frames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_show_thumbnail_frames.Location = new System.Drawing.Point(99, 172);
            this.option_show_thumbnail_frames.Name = "option_show_thumbnail_frames";
            this.option_show_thumbnail_frames.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.option_show_thumbnail_frames.Size = new System.Drawing.Size(142, 17);
            this.option_show_thumbnail_frames.TabIndex = 9;
            this.option_show_thumbnail_frames.Text = "Show Thumbnail Frames";
            this.option_show_thumbnail_frames.UseVisualStyleBackColor = true;
            this.option_show_thumbnail_frames.CheckedChanged += new System.EventHandler(this.option_show_thumbnail_frames_CheckedChanged);
            // 
            // forum_url
            // 
            this.forum_url.AutoSize = true;
            this.forum_url.Location = new System.Drawing.Point(3, 305);
            this.forum_url.Name = "forum_url";
            this.forum_url.Size = new System.Drawing.Size(94, 13);
            this.forum_url.TabIndex = 10;
            this.forum_url.TabStop = true;
            this.forum_url.Text = "Made by StinkRay";
            this.forum_url.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // previewToyMainBindingSource
            // 
            this.previewToyMainBindingSource.CurrentChanged += new System.EventHandler(this.previewToyMainBindingSource_CurrentChanged);
            // 
            // option_sync_size_x
            // 
            this.option_sync_size_x.Location = new System.Drawing.Point(137, 3);
            this.option_sync_size_x.Name = "option_sync_size_x";
            this.option_sync_size_x.Size = new System.Drawing.Size(42, 20);
            this.option_sync_size_x.TabIndex = 11;
            this.option_sync_size_x.TextChanged += new System.EventHandler(this.option_sync_size_x_TextChanged);
            // 
            // option_sync_size_y
            // 
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
            this.option_show_overlay.Location = new System.Drawing.Point(3, 172);
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
            this.option_zoom_factor.Location = new System.Drawing.Point(3, 22);
            this.option_zoom_factor.Name = "option_zoom_factor";
            this.option_zoom_factor.Size = new System.Drawing.Size(28, 20);
            this.option_zoom_factor.TabIndex = 24;
            this.option_zoom_factor.TextChanged += new System.EventHandler(this.option_zoom_factor_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.option_hide_active);
            this.flowLayoutPanel1.Controls.Add(this.option_always_on_top);
            this.flowLayoutPanel1.Controls.Add(this.option_hide_all_if_not_right_type);
            this.flowLayoutPanel1.Controls.Add(this.option_unique_layout);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.option_show_overlay);
            this.flowLayoutPanel1.Controls.Add(this.option_show_thumbnail_frames);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.previews_check_listbox);
            this.flowLayoutPanel1.Controls.Add(this.forum_url);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(252, 328);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.option_sync_size);
            this.panel1.Controls.Add(this.option_sync_size_x);
            this.panel1.Controls.Add(this.option_sync_size_y);
            this.panel1.Location = new System.Drawing.Point(3, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 26);
            this.panel1.TabIndex = 26;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.option_zoom_on_hover);
            this.panel2.Controls.Add(this.option_zoom_factor);
            this.panel2.Location = new System.Drawing.Point(3, 104);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 62);
            this.panel2.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "anchor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Zoom factor";
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
            this.panel3.Location = new System.Drawing.Point(178, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(60, 57);
            this.panel3.TabIndex = 28;
            // 
            // previews_check_listbox
            // 
            this.previews_check_listbox.FormattingEnabled = true;
            this.previews_check_listbox.Location = new System.Drawing.Point(3, 208);
            this.previews_check_listbox.Name = "previews_check_listbox";
            this.previews_check_listbox.Size = new System.Drawing.Size(246, 94);
            this.previews_check_listbox.TabIndex = 28;
            this.previews_check_listbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_SelectedIndexChanged2);
            this.previews_check_listbox.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Previews (check to force hide)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // PreviewToyHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 328);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "PreviewToyHandler";
            this.Text = "EVE Online previewer";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.GlassForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewToyMainBindingSource)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
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
        private Label label3;
        private CheckedListBox previews_check_listbox;
        private Label label1;


    }
}

