namespace PreviewToy
{
    partial class PreviewOverlay
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
            this.overlay_area = new System.Windows.Forms.PictureBox();
            this.client_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.overlay_area)).BeginInit();
            this.SuspendLayout();
            // 
            // overlay_area
            // 
            this.overlay_area.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overlay_area.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overlay_area.Location = new System.Drawing.Point(0, 0);
            this.overlay_area.Name = "overlay_area";
            this.overlay_area.Size = new System.Drawing.Size(284, 262);
            this.overlay_area.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.overlay_area.TabIndex = 0;
            this.overlay_area.TabStop = false;
            this.overlay_area.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // client_label
            // 
            this.client_label.AutoSize = true;
            this.client_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.client_label.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.client_label.ForeColor = System.Drawing.Color.DarkGray;
            this.client_label.Location = new System.Drawing.Point(0, 0);
            this.client_label.Name = "client_label";
            this.client_label.Size = new System.Drawing.Size(25, 13);
            this.client_label.TabIndex = 1;
            this.client_label.Text = "...";
            this.client_label.Click += new System.EventHandler(this.client_label_Click);
            // 
            // PreviewOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ControlBox = false;
            this.Controls.Add(this.client_label);
            this.Controls.Add(this.overlay_area);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreviewOverlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PreviewOverlay";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.PreviewOverlay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.overlay_area)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox overlay_area;
        public System.Windows.Forms.Label client_label;

    }
}