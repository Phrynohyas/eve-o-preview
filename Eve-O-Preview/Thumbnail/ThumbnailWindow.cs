using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview
{
	public partial class ThumbnailWindow : Form, IThumbnail
	{
		#region Private fields
		private readonly bool _isInitializing;
		private readonly IntPtr _sourceWindow;
		private readonly ThumbnailManager _manager;
		private readonly ThumbnailOverlay _overlay;
		private Hotkey _hotkey; // This field stores the hotkey reference

		private Size _baseSize;
		private Point _basePosition;

		private bool _isThumbnailSetUp;
		private DWM_THUMBNAIL_PROPERTIES _Thumbnail;
		private IntPtr _ThumbnailHandle;

		private bool _ignoreMouseOverEvent;
		private bool _isHoverEffectActive;
		private bool _isZoomActive;
		#endregion

		// This constructor should never be used directly
		public ThumbnailWindow(ThumbnailManager manager, IntPtr sourceWindow, string title, Size size)
		{
			this._isInitializing = true;

			this.IsPreviewEnabled = true;
			this.IsOverlayEnabled = true;

			this._sourceWindow = sourceWindow;
			this._manager = manager;

			this._isThumbnailSetUp = false;
			this._ignoreMouseOverEvent = false;
			this._isHoverEffectActive = false;
			this._isZoomActive = false;

			InitializeComponent();

			this.Text = title;

			this._overlay = new ThumbnailOverlay(this.Preview_Click);

			this._isInitializing = false;

			this.SetSize(size);
		}

		public bool IsZoomEnabled { get; set; }

		public bool IsPreviewEnabled { get; set; }

		public bool IsOverlayEnabled { get; set; }

		public bool IsPreviewHandle(IntPtr handle)
		{
			return (this.Handle == handle) || (this._overlay.Handle == handle);
		}

		public void ShowThumbnail()
		{
			if (this.IsPreviewEnabled)
			{
				this.Show();
				if (this.IsOverlayEnabled)
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
				this.HideThumbnail();
			}
		}

		public void HideThumbnail()
		{
			this.Hide();
			this._overlay.Hide();
		}

		public void CloseThumbnail()
		{
			this._overlay.Close();
			this.Close();
		}

		public void SetLabel(string label)
		{
			this.Text = label;
			this._overlay.SetOverlayLabel(label);
		}

		public string GetLabel()
		{
			return this.Text;
		}

		public void SetSize(Size size)
		{
			this.Size = size;
			this._baseSize = this.Size;
			this._basePosition = this.Location;
		}

		public void SetLocation(Point location)
		{
			if (!(this._isInitializing || this._ignoreMouseOverEvent))
			{
				this.Location = location;
			}

			this.RefreshPreview();
		}

		public Point GetLocation()
		{
			return this.Location;
		}

		public void SetOpacity(double opacity)
		{
			if (this._isHoverEffectActive)
			{
				return;
			}

			this.Opacity = opacity;
		}

		public void RegisterShortcut(string shortcut)
		{
			if (String.IsNullOrEmpty(shortcut))
			{
				return;
			}

			KeysConverter converter = new KeysConverter();
			object keysObject = converter.ConvertFrom(shortcut);
			if (keysObject == null)
			{
				return;
			}

			Keys key = (Keys)keysObject;

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
			hotkey.Pressed += Hotkey_Pressed;
			hotkey.Register(this);

			this._hotkey = hotkey;
		}

		public void SetTopMost(bool topmost)
		{
			if (!this.IsPreviewEnabled)
			{
				return;
			}

			this.TopMost = topmost;
			this.MakeOverlayTopMost();
		}

		public void SetWindowFrames(bool enable)
		{
			this.FormBorderStyle = enable ? FormBorderStyle.SizableToolWindow : FormBorderStyle.None;
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

		private void Preview_MouseHover(object sender, EventArgs e)
		{
			if (!this._ignoreMouseOverEvent)
			{
				this._ignoreMouseOverEvent = true;
				if (this.IsZoomEnabled)
				{
					this.ZoomIn();
				}

				this.SetTopMost(true);
			}

			this.Opacity = 1.0f;
			this._isHoverEffectActive = true;

			this.RefreshPreview();
		}

		private void Preview_MouseLeave(object sender, EventArgs e)
		{
			if (this._ignoreMouseOverEvent)
			{
				if (this.IsZoomEnabled)
				{
					this.ZoomOut();
				}

				this._ignoreMouseOverEvent = false;
			}

			this._isHoverEffectActive = false;
			this.Opacity = Properties.Settings.Default.opacity; // TODO Use local object

			this.RefreshPreview();
		}

		private void Preview_Click(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.ActivateClient();
				this._manager.NotifyPreviewSwitch();
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

		private void Hotkey_Pressed(Object sender, EventArgs e)
		{
			this.ActivateClient();
			this._manager.NotifyPreviewSwitch();
		}

		protected override void OnResize(EventArgs e)
		{
			this.RefreshPreview();

			base.OnResize(e);

			if (!(this._isInitializing || this._ignoreMouseOverEvent))
			{
				this._manager.SyncPreviewSize(this.Size);
			}
		}

		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);

			if (!(this._isInitializing || this._ignoreMouseOverEvent))
			{
				this._manager.UpdatePreviewPosition(this.Text, this.Location);
			}

			this.RefreshPreview();
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
			if (this._isThumbnailSetUp || !DwmApiNativeMethods.DwmIsCompositionEnabled())
			{
				return;
			}

			this._ThumbnailHandle = DwmApiNativeMethods.DwmRegisterThumbnail(this.Handle, this._sourceWindow);

			this._Thumbnail = new DWM_THUMBNAIL_PROPERTIES();
			this._Thumbnail.dwFlags = DWM_TNP_CONSTANTS.DWM_TNP_VISIBLE
									+ DWM_TNP_CONSTANTS.DWM_TNP_OPACITY
									+ DWM_TNP_CONSTANTS.DWM_TNP_RECTDESTINATION
									+ DWM_TNP_CONSTANTS.DWM_TNP_SOURCECLIENTAREAONLY;
			this._Thumbnail.opacity = 255;
			this._Thumbnail.fVisible = true;
			this._Thumbnail.fSourceClientAreaOnly = true;
			this._Thumbnail.rcDestination = new RECT(0, 0, this.ClientRectangle.Right, this.ClientRectangle.Bottom);

			DwmApiNativeMethods.DwmUpdateThumbnailProperties(this._ThumbnailHandle, this._Thumbnail);

			this._isThumbnailSetUp = true;
		}

		private void ZoomIn()
		{
			if (this._isZoomActive)
			{
				return;
			}

			this._isZoomActive = true;

			// TODO Use global settings object
			float zoomFactor = Properties.Settings.Default.zoom_amount;

			this._baseSize = this.Size;
			this._basePosition = this.Location;

			this.Size = new Size((int)(zoomFactor * this.Size.Width), (int)(zoomFactor * this.Size.Height));

			// TODO Use global settings object
			switch ((ZoomAnchor)Properties.Settings.Default.zoom_anchor)
			{
				case ZoomAnchor.NW:
					break;
				case ZoomAnchor.N:
					this.Location = new Point(this.Location.X - this.Size.Width / 2 + this._baseSize.Width / 2, this.Location.Y);
					break;
				case ZoomAnchor.NE:
					this.Location = new Point(this.Location.X - this.Size.Width + this._baseSize.Width, this.Location.Y);
					break;

				case ZoomAnchor.W:
					this.Location = new Point(this.Location.X, this.Location.Y - this.Size.Height / 2 + this._baseSize.Height / 2);
					break;
				case ZoomAnchor.C:
					this.Location = new Point(this.Location.X - this.Size.Width / 2 + this._baseSize.Width / 2, this.Location.Y - this.Size.Height / 2 + this._baseSize.Height / 2);
					break;
				case ZoomAnchor.E:
					this.Location = new Point(this.Location.X - this.Size.Width + this._baseSize.Width, this.Location.Y - this.Size.Height / 2 + this._baseSize.Height / 2);
					break;

				case ZoomAnchor.SW:
					this.Location = new Point(this.Location.X, this.Location.Y - this.Size.Height + this._baseSize.Height);
					break;
				case ZoomAnchor.S:
					this.Location = new Point(this.Location.X - this.Size.Width / 2 + this._baseSize.Width / 2, this.Location.Y - this.Size.Height + this._baseSize.Height);
					break;
				case ZoomAnchor.SE:
					this.Location = new Point(this.Location.X - this.Size.Width + this._baseSize.Width, this.Location.Y - this.Size.Height + this._baseSize.Height);
					break;
			}
		}

		private void ZoomOut()
		{
			if (!this._isZoomActive)
			{
				return;
			}

			this.Size = this._baseSize;
			this.Location = this._basePosition;

			this._isZoomActive = false;
		}

		private void ActivateClient()
		{
			DwmApiNativeMethods.SetForegroundWindow(this._sourceWindow);

			int style = DwmApiNativeMethods.GetWindowLong(this._sourceWindow, DwmApiNativeMethods.GWL_STYLE);
			if ((style & DwmApiNativeMethods.WS_MAXIMIZE) == DwmApiNativeMethods.WS_MAXIMIZE)
			{
				// Client is already maximized, no action is required
			}
			else if ((style & DwmApiNativeMethods.WS_MINIMIZE) == DwmApiNativeMethods.WS_MINIMIZE)
			{
				DwmApiNativeMethods.ShowWindowAsync(this._sourceWindow, DwmApiNativeMethods.SW_SHOWNORMAL);
			}
		}
	}
}