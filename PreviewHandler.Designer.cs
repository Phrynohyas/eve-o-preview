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
            ((System.ComponentModel.ISupportInitialize)(this.previewToyMainBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // option_hide_active
            // 
            this.option_hide_active.AutoSize = true;
            this.option_hide_active.Checked = true;
            this.option_hide_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.option_hide_active.Location = new System.Drawing.Point(12, 12);
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
            this.option_hide_all_if_not_right_type.Location = new System.Drawing.Point(12, 35);
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
            this.option_unique_layout.Location = new System.Drawing.Point(12, 81);
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
            this.option_sync_size.Location = new System.Drawing.Point(12, 104);
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
            this.option_always_on_top.Location = new System.Drawing.Point(12, 58);
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
            this.option_show_thumbnail_frames.Location = new System.Drawing.Point(12, 127);
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
            this.forum_url.Location = new System.Drawing.Point(9, 210);
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
            this.option_sync_size_x.Location = new System.Drawing.Point(131, 101);
            this.option_sync_size_x.Name = "option_sync_size_x";
            this.option_sync_size_x.Size = new System.Drawing.Size(42, 20);
            this.option_sync_size_x.TabIndex = 11;
            this.option_sync_size_x.TextChanged += new System.EventHandler(this.option_sync_size_x_TextChanged);
            // 
            // option_sync_size_y
            // 
            this.option_sync_size_y.Location = new System.Drawing.Point(179, 101);
            this.option_sync_size_y.Name = "option_sync_size_y";
            this.option_sync_size_y.Size = new System.Drawing.Size(42, 20);
            this.option_sync_size_y.TabIndex = 12;
            this.option_sync_size_y.TextChanged += new System.EventHandler(this.option_sync_size_y_TextChanged);
            // 
            // PreviewToyHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 232);
            this.Controls.Add(this.option_sync_size_y);
            this.Controls.Add(this.option_sync_size_x);
            this.Controls.Add(this.forum_url);
            this.Controls.Add(this.option_show_thumbnail_frames);
            this.Controls.Add(this.option_always_on_top);
            this.Controls.Add(this.option_sync_size);
            this.Controls.Add(this.option_unique_layout);
            this.Controls.Add(this.option_hide_all_if_not_right_type);
            this.Controls.Add(this.option_hide_active);
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
            this.ResumeLayout(false);
            this.PerformLayout();

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


    }
}

