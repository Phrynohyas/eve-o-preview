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

        public PreviewOverlay(Preview parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void PreviewOverlay_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.parent.render_area_Click(sender, e);
        }

        private void client_label_Click(object sender, EventArgs e)
        {

        }

    }
}
