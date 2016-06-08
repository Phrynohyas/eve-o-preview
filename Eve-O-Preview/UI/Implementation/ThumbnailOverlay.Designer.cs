namespace EveOPreview.UI
{
	partial class ThumbnailOverlay
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
			System.Windows.Forms.PictureBox OverlayAreaPictureBox;
			this.OverlayLabel = new System.Windows.Forms.Label();
			OverlayAreaPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(OverlayAreaPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// OverlayAreaPictureBox
			// 
			OverlayAreaPictureBox.BackColor = System.Drawing.Color.Transparent;
			OverlayAreaPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			OverlayAreaPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			OverlayAreaPictureBox.Location = new System.Drawing.Point(0, 0);
			OverlayAreaPictureBox.Name = "OverlayAreaPictureBox";
			OverlayAreaPictureBox.Size = new System.Drawing.Size(284, 262);
			OverlayAreaPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			OverlayAreaPictureBox.TabIndex = 0;
			OverlayAreaPictureBox.TabStop = false;
			OverlayAreaPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OverlayArea_Click);
			// 
			// OverlayLabel
			// 
			this.OverlayLabel.AutoSize = true;
			this.OverlayLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.OverlayLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OverlayLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.OverlayLabel.Location = new System.Drawing.Point(0, 0);
			this.OverlayLabel.Name = "OverlayLabel";
			this.OverlayLabel.Size = new System.Drawing.Size(25, 13);
			this.OverlayLabel.TabIndex = 1;
			this.OverlayLabel.Text = "...";
			// 
			// ThumbnailOverlay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.ControlBox = false;
			this.Controls.Add(this.OverlayLabel);
			this.Controls.Add(OverlayAreaPictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThumbnailOverlay";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "PreviewOverlay";
			this.TransparencyKey = System.Drawing.Color.Black;
			((System.ComponentModel.ISupportInitialize)(OverlayAreaPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label OverlayLabel;
	}
}