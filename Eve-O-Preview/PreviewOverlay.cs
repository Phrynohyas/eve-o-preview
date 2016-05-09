using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreviewToy
{
    public partial class PreviewOverlay : Form
    {
        private Preview parent;
        private Color original_color;
        public PreviewOverlay(Preview parent)
        {
            this.parent = parent;
            InitializeComponent();
            original_color = overlay_area.BackColor;
        }

        private void PreviewOverlay_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            this.parent.render_area_Click(sender, e);
        }

        public void makeTopMost()
        {
            this.TopMost = true;
        }

        private void client_label_Click(object sender, EventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }

    }
}
