using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.UI
{
	public partial class ThumbnailOverlay : Form
	{
		#region Private fields
		private readonly Action<object, MouseEventArgs> _areaClickAction;
		private bool _highlightEnabled;
		private Color _highlightColor;
		#endregion

		public ThumbnailOverlay(Form owner, Action<object, MouseEventArgs> areaClickAction)
		{
			this.Owner = owner;
			this._areaClickAction = areaClickAction;

			this._highlightEnabled = false;
			this._highlightColor = Color.Red;

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

		public void EnableHighlight(bool enabled, Color color)
		{
			if (enabled == this._highlightEnabled)
			{
				// Nothing to do here
				return;
			}

			this._highlightEnabled = enabled;
			this._highlightColor = color;
			this.Refresh();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= (int)WindowManagerNativeMethods.WS_EX_TOOLWINDOW;
				return Params;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (this._highlightEnabled)
			{
				ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
											this._highlightColor, 4, ButtonBorderStyle.Solid,
											this._highlightColor, 4, ButtonBorderStyle.Solid,
											this._highlightColor, 4, ButtonBorderStyle.Solid,
											this._highlightColor, 4, ButtonBorderStyle.Solid);
			}
		}
	}
}
