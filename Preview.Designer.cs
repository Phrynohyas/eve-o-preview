namespace PreviewToy
{
    partial class Preview
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
            this.render_area = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.render_area)).BeginInit();
            this.SuspendLayout();
            // 
            // render_area
            // 
            this.render_area.BackColor = System.Drawing.Color.Black;
            this.render_area.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.render_area.Cursor = System.Windows.Forms.Cursors.Hand;
            this.render_area.Dock = System.Windows.Forms.DockStyle.Fill;
            this.render_area.Location = new System.Drawing.Point(0, 0);
            this.render_area.Margin = new System.Windows.Forms.Padding(0);
            this.render_area.Name = "render_area";
            this.render_area.Size = new System.Drawing.Size(153, 89);
            this.render_area.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.render_area.TabIndex = 0;
            this.render_area.TabStop = false;
            this.render_area.Click += new System.EventHandler(this.render_area_Click);
            // 
            // Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(153, 89);
            this.ControlBox = false;
            this.Controls.Add(this.render_area);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(64, 64);
            this.Name = "Preview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Preview";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Preview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.render_area)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox render_area;


    }
}