using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.UI
{
	public partial class ThumbnailOverlay : Form
	{
		#region Private fields
		private readonly Action<object, MouseEventArgs> _areaClickAction;
		private bool _isHighlightEnabled;
		private Color _highlightColor;
		private int _highlightWidthLeft;
		private int _highlightWidthTop;
		private int _highlightWidthRight;
		private int _highlightWidthBottom;
		#endregion

		public ThumbnailOverlay(Form owner, Action<object, MouseEventArgs> areaClickAction)
		{
			this.Owner = owner;
			this._areaClickAction = areaClickAction;

			this._isHighlightEnabled = false;
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

		public void SetHighlightWidth(int left, int top, int right, int bottom)
		{
			this._highlightWidthLeft = left;
			this._highlightWidthTop = top;
			this._highlightWidthRight = right;
			this._highlightWidthBottom = bottom;
		}

		public void EnableHighlight(bool enabled, Color color)
		{
			if (enabled == this._isHighlightEnabled)
			{
				// Nothing to do here
				return;
			}

			this._isHighlightEnabled = enabled;
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

			if (this._isHighlightEnabled)
			{
				ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
											this._highlightColor, this._highlightWidthLeft, ButtonBorderStyle.Solid,
											this._highlightColor, this._highlightWidthTop, ButtonBorderStyle.Solid,
											this._highlightColor, this._highlightWidthRight, ButtonBorderStyle.Solid,
											this._highlightColor, this._highlightWidthBottom, ButtonBorderStyle.Solid);
			}
		}
	}
}
