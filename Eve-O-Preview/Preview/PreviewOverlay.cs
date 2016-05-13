using System.Windows.Forms;

namespace EveOPreview
{
	public partial class PreviewOverlay : Form
	{
		private readonly Preview _parent;

		public PreviewOverlay(Preview parent)
		{
			this._parent = parent;
			InitializeComponent();
		}

		private void OverlayArea_Click(object sender, MouseEventArgs e)
		{
			this._parent.render_area_Click(sender, e);
		}

		public void MakeTopMost()
		{
			this.TopMost = true;
		}

		public void SetOverlayLabel(string label)
		{
			this.OverlayLabel.Text = label;
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
