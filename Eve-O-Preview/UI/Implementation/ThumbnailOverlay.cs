using System;
using System.Windows.Forms;

namespace EveOPreview.UI
{
	public partial class ThumbnailOverlay : Form
	{
		#region Private fields
		private readonly Action<object, MouseEventArgs> _areaClickAction;
		#endregion

		public ThumbnailOverlay(Form owner, Action<object, MouseEventArgs> areaClickAction)
		{
			this.Owner = owner;
			this._areaClickAction = areaClickAction;

			InitializeComponent();
		}

		private void OverlayArea_Click(object sender, MouseEventArgs e)
		{
			this._areaClickAction(this, e);
		}

		public void SetOverlayLabel(string label)
		{
			this.OverlayLabel.Text = label;
		}

		public void EnableOverlayLabel(bool enable)
		{
			this.OverlayLabel.Visible = enable;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= (int)InteropConstants.WS_EX_TOOLWINDOW;
				return Params;
			}
		}
	}
}
