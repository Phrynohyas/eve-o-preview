using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.UI
{
	public partial class ThumbnailView : Form, IThumbnailView
	{
		#region Private fields

		//private readonly IThumbnailManager _manager;
		private readonly ThumbnailOverlay _overlay;

		//private Size _baseSize;
		//private Point _basePosition;

		// This is pure brainless View
		// Just somewhat more complex than usual
		private bool _isThumbnailSetUp;
		private bool _isOverlayVisible;
		private bool _isPositionChanged;
		private bool _isSizeChanged;
		private DWM_THUMBNAIL_PROPERTIES _thumbnail;
		private IntPtr _thumbnailHandle;
		private Size _baseSize;
		private Point _baseLocation;
		#endregion

		public ThumbnailView()
		{
			this.IsEnabled = true;
			this.IsActive = false;

			this.IsOverlayEnabled = false;
			this._isThumbnailSetUp = false;
			this._isOverlayVisible = false;

			this._isPositionChanged = true;
			this._isSizeChanged = true;

			InitializeComponent();

			this._overlay = new ThumbnailOverlay(this, this.ThumbnailActivated_Handler);
		}

		public IntPtr Id { get; set; }

		public string Title
		{
			get
			{
				return this.Text;
			}
			set
			{
				this.Text = value;
				this._overlay.SetOverlayLabel(value);
			}
		}

		public bool IsEnabled { get; set; }

		public bool IsActive { get; set; }

		public bool IsOverlayEnabled { get; set; }

		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				if ((value.X > 0) || (value.Y > 0))
				{
					this.StartPosition = FormStartPosition.Manual;
				}
				base.Location = value;
			}
		}

		public Action<IntPtr> ThumbnailResized { get; set; }

		public Action<IntPtr> ThumbnailMoved { get; set; }

		public Action<IntPtr> ThumbnailFocused { get; set; }

		public Action<IntPtr> ThumbnailLostFocus { get; set; }

		public Action<IntPtr> ThumbnailActivated { get; set; }

		public new void Show()
		{
			base.Show();

			// Thumbnail will be registered during the Refresh cycle
			this.Refresh();

			this.IsActive = true;
		}

		public new void Hide()
		{
			this.IsActive = false;

			this._overlay.Hide();
			base.Hide();
		}

		public new void Close()
		{
			this.IsActive = false;

			this.UnregisterThumbnail(this._thumbnailHandle);

			this._overlay.Close();
			base.Close();
		}

		// This method is used to determine if the provided Handle is related to client or its thumbnail
		public bool IsKnownHandle(IntPtr handle)
		{
			return (this.Id == handle) || (this.Handle == handle) || (this._overlay.Handle == handle);
		}

		public void SetSizeLimitations(Size minimumSize, Size maximumSize)
		{
			this.MinimumSize = minimumSize;
			this.MaximumSize = maximumSize;
		}

		public void SetOpacity(double opacity)
		{
			this.Opacity = opacity;
		}

		public void SetWindowFrames(bool enable)
		{
			this.FormBorderStyle = enable ? FormBorderStyle.SizableToolWindow : FormBorderStyle.None;

			// Notify about windo contents position change
			this._isSizeChanged = true;
		}

		public void SetTopMost(bool enableTopmost)
		{
			this.TopMost = enableTopmost;
			this._overlay.TopMost = enableTopmost;
		}

		public void ZoomIn(ViewZoomAnchor anchor, int zoomFactor)
		{
			this._baseSize = this.Size;
			this._baseLocation = this.Location;

			int oldWidth = this._baseSize.Width;
			int oldHeight = this._baseSize.Height;

			int locationX = this.Location.X;
			int locationY = this.Location.Y;

			int newWidth = (zoomFactor * this.ClientSize.Width) + (this.Size.Width - this.ClientSize.Width);
			int newHeight = (zoomFactor * this.ClientSize.Height) + (this.Size.Height - this.ClientSize.Height);

			// First change size, THEN move the window
			// Otherwise there is a chance to fail in a loop
			// Zoom requied -> Moved the windows 1st -> Focus is lost -> Window is moved back -> Focus is back on -> Zoom required -> ...
			this.Size = new Size(newWidth, newHeight);

			switch (anchor)
			{
				case ViewZoomAnchor.NW:
					break;
				case ViewZoomAnchor.N:
					this.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY);
					break;
				case ViewZoomAnchor.NE:
					this.Location = new Point(locationX - newWidth + oldWidth, locationY);
					break;

				case ViewZoomAnchor.W:
					this.Location = new Point(locationX, locationY - newHeight / 2 + oldHeight / 2);
					break;
				case ViewZoomAnchor.C:
					this.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY - newHeight / 2 + oldHeight / 2);
					break;
				case ViewZoomAnchor.E:
					this.Location = new Point(locationX - newWidth + oldWidth, locationY - newHeight / 2 + oldHeight / 2);
					break;

				case ViewZoomAnchor.SW:
					this.Location = new Point(locationX, locationY - newHeight + this._baseSize.Height);
					break;
				case ViewZoomAnchor.S:
					this.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY - newHeight + oldHeight);
					break;
				case ViewZoomAnchor.SE:
					this.Location = new Point(locationX - newWidth + oldWidth, locationY - newHeight + oldHeight);
					break;
			}
		}

		public void ZoomOut()
		{
			this.Size = this._baseSize;
			this.Location = this._baseLocation;
		}

		public void Refresh(bool forceRefresh)
		{
			// To prevent flickering the old broken thumbnail is removed AFTER the new shiny one is created
			IntPtr obsoleteThumbnailHanlde = forceRefresh ? this._thumbnailHandle : IntPtr.Zero;

			if ((this._isThumbnailSetUp == false) || forceRefresh)
			{
				this.RegisterThumbnail();
			}

			bool sizeChanged = this._isSizeChanged || forceRefresh;
			bool locationChanged = this._isPositionChanged || forceRefresh;

			if (sizeChanged)
			{
				this._thumbnail.rcDestination = new RECT(0, 0, this.ClientRectangle.Right, this.ClientRectangle.Bottom);
				try
				{
					DwmApiNativeMethods.DwmUpdateThumbnailProperties(this._thumbnailHandle, this._thumbnail);
				}
				catch (ArgumentException)
				{
					//This exception will be thrown if the EVE client disappears while this method is running
				}
				this._isSizeChanged = false;
			}

			if (obsoleteThumbnailHanlde != IntPtr.Zero)
			{
				this.UnregisterThumbnail(obsoleteThumbnailHanlde);
			}

			if (!this.IsOverlayEnabled)
			{
				if (this._isOverlayVisible)
				{
					this._overlay.Hide();
					this._isOverlayVisible = false;
				}

				return;
			}

			if (!this._isOverlayVisible)
			{
				this._overlay.Show();
				this._isOverlayVisible = true;
			}
			else if (!(sizeChanged || locationChanged))
			{
				// No need to adjust in the overlay location if it is already visible and properly set
				return;
			}

			Size overlaySize = this.RenderAreaPictureBox.Size;
			overlaySize.Width -= 2 * 5;
			overlaySize.Height -= 2 * 5;

			Point overlayLocation = this.Location;

			overlayLocation.X += 5 + (this.Size.Width - this.RenderAreaPictureBox.Size.Width) / 2;
			overlayLocation.Y += 5 + (this.Size.Height - this.RenderAreaPictureBox.Size.Height) - (this.Size.Width - this.RenderAreaPictureBox.Size.Width) / 2;

			this._isPositionChanged = false;
			this._overlay.Size = overlaySize;
			this._overlay.Location = overlayLocation;
		}

		#region GUI events
		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= (int)DwmApiNativeMethods.WS_EX_TOOLWINDOW;
				return Params;
			}
		}

		private void Move_Handler(object sender, EventArgs e)
		{
			this._isPositionChanged = true;
			this.ThumbnailMoved?.Invoke(this.Id);
		}

		private void Resize_Handler(object sender, EventArgs e)
		{
			this._isSizeChanged = true;
			this.ThumbnailResized?.Invoke(this.Id);
		}

		private void Focused_Handler(object sender, EventArgs e)
		{
			this.ThumbnailFocused?.Invoke(this.Id);
		}

		private void LostFocus_Handler(object sender, EventArgs e)
		{
			this.ThumbnailLostFocus?.Invoke(this.Id);
		}

		private void ThumbnailActivated_Handler(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.ThumbnailActivated?.Invoke(this.Id);
			}

			//if (e.Button == MouseButtons.Right)
			//{
			//	// do smth cool?
			//}

			if (e.Button == MouseButtons.Middle)
			{
				//// Trigger full thumbnail refresh
				//this.UnregisterThumbnail();
				//this.Refresh();
			}
		}
		#endregion

		private void RegisterThumbnail()
		{
			this._thumbnailHandle = DwmApiNativeMethods.DwmRegisterThumbnail(this.Handle, this.Id);

			this._thumbnail = new DWM_THUMBNAIL_PROPERTIES();
			this._thumbnail.dwFlags = DWM_TNP_CONSTANTS.DWM_TNP_VISIBLE
									+ DWM_TNP_CONSTANTS.DWM_TNP_OPACITY
									+ DWM_TNP_CONSTANTS.DWM_TNP_RECTDESTINATION
									+ DWM_TNP_CONSTANTS.DWM_TNP_SOURCECLIENTAREAONLY;
			this._thumbnail.opacity = 255;
			this._thumbnail.fVisible = true;
			this._thumbnail.fSourceClientAreaOnly = true;

			this._isThumbnailSetUp = true;
		}

		private void UnregisterThumbnail(IntPtr thumbnailHandle)
		{
			try
			{
				DwmApiNativeMethods.DwmUnregisterThumbnail(thumbnailHandle);
			}
			catch (ArgumentException)
			{
			}
		}

		//private Hotkey _hotkey; // This field stores the hotkey reference
		//public void RegisterShortcut(string shortcut)
		//{
		//if (String.IsNullOrEmpty(shortcut))
		//{
		//	return;
		//}

		//KeysConverter converter = new KeysConverter();
		//object keysObject = converter.ConvertFrom(shortcut);
		//if (keysObject == null)
		//{
		//	return;
		//}

		//Keys key = (Keys)keysObject;

		//Hotkey hotkey = new Hotkey();

		//if ((key & Keys.Shift) == Keys.Shift)
		//{
		//	hotkey.Shift = true;
		//}

		//if ((key & Keys.Alt) == Keys.Alt)
		//{
		//	hotkey.Alt = true;
		//}

		//if ((key & Keys.Control) == Keys.Control)
		//{
		//	hotkey.Control = true;
		//}

		//key = key & ~Keys.Shift & ~Keys.Alt & ~Keys.Control;
		//hotkey.KeyCode = key;
		//hotkey.Pressed += Hotkey_Pressed;
		//hotkey.Register(this);

		//this._hotkey = hotkey;
		//}
	}
}