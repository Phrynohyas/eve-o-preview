using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview
{
	public partial class Preview : Form
	{
		#region Private fields
		private readonly bool _isInitializing;
		private readonly IntPtr _sourceWindow;
		private readonly MainForm _parentForm;
		private readonly PreviewOverlay _overlay;

		private Size _normalSize;
		private Point _normalPosition;

		private bool _isThumbnailSetUp;
		private DWM_THUMBNAIL_PROPERTIES _Thumbnail;
		private IntPtr _ThumbnailHandle;
		#endregion

		// TODO Hide the parent form behind an interface
		public Preview(MainForm parent, IntPtr sourceWindow, String title, Size size)
		{
			this._isInitializing = true;

			this._sourceWindow = sourceWindow;
			this._parentForm = parent;

			this._isThumbnailSetUp = false;

			InitializeComponent();

			this.Text = title;

			this._overlay = new PreviewOverlay(this.render_area_Click);

			this._isInitializing = false;

			this.SetSize(size);
		}

		public bool IsPreviewHandle(IntPtr handle)
		{
			return (this.Handle == handle) || (this._overlay.Handle == handle);
		}

		public void SetSize(Size size)
		{
			this.Size = size;
			this._normalSize = this.Size;
			this._normalPosition = this.Location;
		}

		public void MakeTopMost(bool topmost)
		{
			if (!this.hide)
			{
				return;
			}

			this.TopMost = topmost;
			this.MakeOverlayTopMost();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= (int)DwmApiNativeMethods.WS_EX_TOOLWINDOW;
				return Params;
			}
		}

		private void MakeOverlayTopMost()
		{
			this._overlay.TopMost = true;
		}

		private void RefreshPreview()
		{
			if (this._isInitializing)
			{
				return;
			}

			if (DwmApiNativeMethods.DwmIsCompositionEnabled())
			{
				if (this._isThumbnailSetUp == false)
				{
					this.SetUpThumbnail();
				}

				this._Thumbnail.rcDestination = new RECT(0, 0, this.ClientRectangle.Right, this.ClientRectangle.Bottom);
				DwmApiNativeMethods.DwmUpdateThumbnailProperties(this._ThumbnailHandle, this._Thumbnail);
			}
			else
			{
				this._isThumbnailSetUp = false;
			}

			Size overlaySize = this.RenderAreaPictureBox.Size;
			overlaySize.Width -= 2 * 5;
			overlaySize.Height -= 2 * 5;

			Point overlayLocation = this.Location;
			overlayLocation.X += 5 + (this.Size.Width - this.RenderAreaPictureBox.Size.Width) / 2;
			overlayLocation.Y += 5 + (this.Size.Height - this.RenderAreaPictureBox.Size.Height) - (this.Size.Width - this.RenderAreaPictureBox.Size.Width) / 2;

			this._overlay.Size = overlaySize;
			this._overlay.Location = overlayLocation;
		}

		private void SetUpThumbnail()
		{
			// TODO Remove this API call on Win 8+ hosts
			if (this._isThumbnailSetUp || !DwmApiNativeMethods.DwmIsCompositionEnabled())
			{
				return;
			}

			this._ThumbnailHandle = DwmApiNativeMethods.DwmRegisterThumbnail(this.Handle, this._sourceWindow);

			this._Thumbnail = new DWM_THUMBNAIL_PROPERTIES();
			this._Thumbnail.dwFlags = DWM_THUMBNAIL_PROPERTIES.DWM_TNP_VISIBLE
									+ DWM_THUMBNAIL_PROPERTIES.DWM_TNP_OPACITY
									+ DWM_THUMBNAIL_PROPERTIES.DWM_TNP_RECTDESTINATION
									+ DWM_THUMBNAIL_PROPERTIES.DWM_TNP_SOURCECLIENTAREAONLY;
			this._Thumbnail.opacity = 255;
			this._Thumbnail.fVisible = true;
			this._Thumbnail.fSourceClientAreaOnly = true;
			this._Thumbnail.rcDestination = new RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);

			DwmApiNativeMethods.DwmUpdateThumbnailProperties(_ThumbnailHandle, _Thumbnail);

			_isThumbnailSetUp = true;
		}


		public bool show_overlay = true;
		public bool hover_zoom = true;
		public bool is_zoomed = false;
		public bool is_hovered_over = false;

		private bool mouse_over_lock = false;


		private Hotkey hotkey;

		private bool hide = false;

		public void MakeHidden(bool wha)
		{
			hide = wha;
		}

		public override string ToString()
		{
			return this.Text;
		}

		public void preview_MouseHover(object sender, System.EventArgs e)
		{
			if (!mouse_over_lock)
			{
				mouse_over_lock = true;
				if (hover_zoom)
					doZoom();

				this.MakeTopMost(true);
			}
			this.Opacity = 1.0f;
			this.is_hovered_over = true;
			RefreshPreview();
		}

		public void registerShortcut(string shortcut)
		{
			if (shortcut == "")
				return;
			var cvt = new KeysConverter();
			var key = (Keys)cvt.ConvertFrom(shortcut);

			Hotkey hotkey = new Hotkey();

			if ((key & Keys.Shift) == Keys.Shift)
			{
				hotkey.Shift = true;
			}
			if ((key & Keys.Alt) == Keys.Alt)
			{
				hotkey.Alt = true;
			}
			if ((key & Keys.Control) == Keys.Control)
			{
				hotkey.Control = true;
			}

			key = key & ~Keys.Shift & ~Keys.Alt & ~Keys.Control;
			hotkey.KeyCode = key;
			hotkey.Register(this);
			// TODO Make this a method
			hotkey.Pressed += (s, e) => { bring_client_to_foreground(); this._parentForm.preview_did_switch(); };

			this.hotkey = hotkey;
		}

		public void doZoom()
		{
			if (is_zoomed)
				return;

			is_zoomed = true;

			float hover_zoom_factor = Properties.Settings.Default.zoom_amount;

			_normalSize = Size;
			_normalPosition = Location;

			Size = new Size((int)(hover_zoom_factor * (float)Size.Width), (int)(hover_zoom_factor * (float)Size.Height));

			switch ((ZoomAnchor)Properties.Settings.Default.zoom_anchor)
			{
				case (ZoomAnchor.NW):
					break;
				case (ZoomAnchor.N):
					Location = new Point(Location.X - Size.Width / 2 + _normalSize.Width / 2, Location.Y);
					break;
				case (ZoomAnchor.NE):
					Location = new Point(Location.X - Size.Width + _normalSize.Width, Location.Y);
					break;

				case (ZoomAnchor.W):
					Location = new Point(Location.X, Location.Y - Size.Height / 2 + _normalSize.Height / 2);
					break;
				case (ZoomAnchor.C):
					Location = new Point(Location.X - Size.Width / 2 + _normalSize.Width / 2, Location.Y - Size.Height / 2 + _normalSize.Height / 2);
					break;
				case (ZoomAnchor.E):
					Location = new Point(Location.X - Size.Width + _normalSize.Width, Location.Y - Size.Height / 2 + _normalSize.Height / 2);
					break;

				case (ZoomAnchor.SW):
					Location = new Point(Location.X, Location.Y - Size.Height + _normalSize.Height);
					break;
				case (ZoomAnchor.S):
					Location = new Point(Location.X - Size.Width / 2 + _normalSize.Width / 2, Location.Y - Size.Height + _normalSize.Height);
					break;
				case (ZoomAnchor.SE):
					Location = new Point(Location.X - Size.Width + _normalSize.Width, Location.Y - Size.Height + _normalSize.Height);
					break;
			}
		}

		public void restoreZoom()
		{
			if (!is_zoomed)
				return;

			Size = _normalSize;
			Location = _normalPosition;
			is_zoomed = false;
		}

		public void preview_MouseLeave(object sender, System.EventArgs e)
		{
			if (mouse_over_lock)
			{
				if (hover_zoom)
				{
					restoreZoom();
				}
				mouse_over_lock = false;
			}
			this.is_hovered_over = false;
			this.Opacity = Properties.Settings.Default.opacity;
			RefreshPreview();
		}

		protected override void OnResize(EventArgs e)
		{
			RefreshPreview();
			base.OnResize(e);

			if (!this._isInitializing && !mouse_over_lock)
				this._parentForm.syncronize_preview_size(this.Size);
		}

		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			if (!this._isInitializing && !mouse_over_lock)
				this._parentForm.register_preview_position(this.Text, this.Location);

			RefreshPreview();
		}

		public void doMove(Point position)
		{
			if (!this._isInitializing && !mouse_over_lock)
				Location = position;

			RefreshPreview();
		}

		public void SetLabel(String label)
		{
			this.Text = label;
			this._overlay.SetOverlayLabel(label);
		}

		new public void Show()
		{
			if (!hide)
			{
				base.Show();
				if (show_overlay)
				{
					this._overlay.Show();
					this.MakeOverlayTopMost();
				}
				else
				{
					this._overlay.Hide();
				}
			}
			else
			{
				this.Hide();
			}
		}

		new public void Hide()
		{
			base.Hide();
			this._overlay.Hide();
		}

		new public void Close()
		{
			this._overlay.Close();
			base.Close();
		}

		private void Preview_Load(object sender, EventArgs e)
		{

		}

		public void bring_client_to_foreground()
		{
			DwmApiNativeMethods.SetForegroundWindow(this._sourceWindow);
			int style = DwmApiNativeMethods.GetWindowLong(this._sourceWindow, DwmApiNativeMethods.GWL_STYLE);
			if ((style & DwmApiNativeMethods.WS_MAXIMIZE) == DwmApiNativeMethods.WS_MAXIMIZE)
			{
				//It's maximized
			}
			else if ((style & DwmApiNativeMethods.WS_MINIMIZE) == DwmApiNativeMethods.WS_MINIMIZE)
			{
				DwmApiNativeMethods.ShowWindowAsync(this._sourceWindow, DwmApiNativeMethods.SW_SHOWNORMAL);
			}
		}

		public void render_area_Click(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				bring_client_to_foreground();
				this._parentForm.preview_did_switch();
			}
			if (e.Button == MouseButtons.Right)
			{
				// do smth cool?
			}
			if (e.Button == MouseButtons.Middle)
			{
				// do smth cool?
			}
		}
	}
}