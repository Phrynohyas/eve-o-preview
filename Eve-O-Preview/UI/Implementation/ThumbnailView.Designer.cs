namespace EveOPreview.UI
{
    partial class ThumbnailView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.RenderAreaPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.RenderAreaPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// RenderAreaPictureBox
			// 
			this.RenderAreaPictureBox.BackColor = System.Drawing.Color.Transparent;
			this.RenderAreaPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.RenderAreaPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RenderAreaPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RenderAreaPictureBox.Location = new System.Drawing.Point(0, 0);
			this.RenderAreaPictureBox.Margin = new System.Windows.Forms.Padding(0);
			this.RenderAreaPictureBox.Name = "RenderAreaPictureBox";
			this.RenderAreaPictureBox.Size = new System.Drawing.Size(153, 89);
			this.RenderAreaPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.RenderAreaPictureBox.TabIndex = 0;
			this.RenderAreaPictureBox.TabStop = false;
			this.RenderAreaPictureBox.MouseLeave += new System.EventHandler(this.LostFocus_Handler);
			this.RenderAreaPictureBox.MouseHover += new System.EventHandler(this.Focused_Handler);
			this.RenderAreaPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ThumbnailActivated_Handler);
			// 
			// ThumbnailView
			// 
			this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(153, 89);
			this.ControlBox = false;
			this.Controls.Add(this.RenderAreaPictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(64, 64);
			this.Name = "ThumbnailView";
			this.Opacity = 0.1D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Preview";
			this.TopMost = true;
			this.Move += new System.EventHandler(this.Move_Handler);
			this.Resize += new System.EventHandler(this.Resize_Handler);
			((System.ComponentModel.ISupportInitialize)(this.RenderAreaPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox RenderAreaPictureBox;


    }
}