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
			this.SuspendLayout();
			// 
			// ThumbnailView
			// 
			this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(153, 89);
			this.ControlBox = false;
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
			this.MouseLeave += new System.EventHandler(this.LostFocus_Handler);
			this.MouseHover += new System.EventHandler(this.Focused_Handler);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ThumbnailActivated_Handler);
			this.Move += new System.EventHandler(this.Move_Handler);
			this.Resize += new System.EventHandler(this.Resize_Handler);
			this.ResumeLayout(false);

        }

        #endregion


    }
}