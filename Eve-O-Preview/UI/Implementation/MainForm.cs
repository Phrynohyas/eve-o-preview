using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.UI
{
	public partial class MainForm : Form, IMainView
	{
		#region Private fields
		private readonly ApplicationContext _context;
		private readonly Dictionary<ViewZoomAnchor, RadioButton> _zoomAnchorMap;
		private ViewZoomAnchor _cachedThumbnailZoomAnchor;
		private bool _suppressEvents;
		private Size _minimumSize;
		private Size _maximumSize;
		#endregion

		public MainForm(ApplicationContext context)
		{
			this._context = context;
			this._zoomAnchorMap = new Dictionary<ViewZoomAnchor, RadioButton>();
			this._cachedThumbnailZoomAnchor = ViewZoomAnchor.NW;
			this._suppressEvents = false;
			this._minimumSize = new Size(80, 60);
			this._maximumSize = new Size(80, 60);

			InitializeComponent();

			this.ThumbnailsList.DisplayMember = "Title";

			this.InitZoomAnchorMap();
		}

		public bool MinimizeToTray
		{
			get
			{
				return this.MinimizeToTrayCheckBox.Checked;
			}
			set
			{
				this.MinimizeToTrayCheckBox.Checked = value;
			}
		}

		public double ThumbnailOpacity
		{
			get
			{
				return Math.Min(this.ThumbnailOpacityTrackBar.Value / 100.00, 1.00);
			}
			set
			{
				int barValue = (int)(100.0 * value);
				if (barValue > 100)
				{
					barValue = 100;
				}
				else if (barValue < 10)
				{
					barValue = 10;
				}

				this.ThumbnailOpacityTrackBar.Value = barValue;
			}
		}

		public bool EnableClientLayoutTracking
		{
			get
			{
				return this.EnableClientLayoutTrackingCheckBox.Checked;
			}
			set
			{
				this.EnableClientLayoutTrackingCheckBox.Checked = value;
			}
		}

		public bool HideActiveClientThumbnail
		{
			get
			{
				return this.HideActiveClientThumbnailCheckBox.Checked;
			}
			set
			{
				this.HideActiveClientThumbnailCheckBox.Checked = value;
			}
		}

		public bool ShowThumbnailsAlwaysOnTop
		{
			get
			{
				return this.ShowThumbnailsAlwaysOnTopCheckBox.Checked;
			}
			set
			{
				this.ShowThumbnailsAlwaysOnTopCheckBox.Checked = value;
			}
		}

		public bool HideThumbnailsOnLostFocus
		{
			get
			{
				return this.HideThumbnailsOnLostFocusCheckBox.Checked;
			}
			set
			{
				this.HideThumbnailsOnLostFocusCheckBox.Checked = value;
			}
		}

		public bool EnablePerClientThumbnailLayouts
		{
			get
			{
				return this.EnablePerClientThumbnailsLayoutsCheckBox.Checked;
			}
			set
			{
				this.EnablePerClientThumbnailsLayoutsCheckBox.Checked = value;
			}
		}

		public Size ThumbnailSize
		{
			get
			{
				return new Size((int)this.ThumbnailsWidthNumericEdit.Value, (int)this.ThumbnailsHeightNumericEdit.Value);
			}
			set
			{
				this.ThumbnailsWidthNumericEdit.Value = value.Width;
				this.ThumbnailsHeightNumericEdit.Value = value.Height;
			}
		}

		public bool EnableThumbnailZoom
		{
			get
			{
				return this.EnableThumbnailZoomCheckBox.Checked;
			}
			set
			{
				this.EnableThumbnailZoomCheckBox.Checked = value;
				this.RefreshZoomSettings();
			}
		}

		public int ThumbnailZoomFactor
		{
			get
			{
				return (int)this.ThumbnailZoomFactorNumericEdit.Value;
			}
			set
			{
				this.ThumbnailZoomFactorNumericEdit.Value = value;
			}
		}

		public ViewZoomAnchor ThumbnailZoomAnchor
		{
			get
			{
				if (this._zoomAnchorMap[this._cachedThumbnailZoomAnchor].Checked)
				{
					return this._cachedThumbnailZoomAnchor;
				}

				foreach (KeyValuePair<ViewZoomAnchor, RadioButton> valuePair in this._zoomAnchorMap)
				{
					if (!valuePair.Value.Checked)
					{
						continue;
					}

					this._cachedThumbnailZoomAnchor = valuePair.Key;
					return this._cachedThumbnailZoomAnchor;
				}

				// Default value
				return ViewZoomAnchor.NW;
			}
			set
			{
				this._cachedThumbnailZoomAnchor = value;
				this._zoomAnchorMap[this._cachedThumbnailZoomAnchor].Checked = true;
			}
		}

		public bool ShowThumbnailOverlays
		{
			get
			{
				return this.ShowThumbnailOverlaysCheckBox.Checked;
			}
			set
			{
				this.ShowThumbnailOverlaysCheckBox.Checked = value;
			}
		}

		public bool ShowThumbnailFrames
		{
			get
			{
				return this.ShowThumbnailFramesCheckBox.Checked;
			}
			set
			{
				this.ShowThumbnailFramesCheckBox.Checked = value;
			}
		}

		public bool EnableActiveClientHighlight
		{
			get
			{
				return this.EnableActiveClientHighlightCheckBox.Checked;
			}
			set
			{
				this.EnableActiveClientHighlightCheckBox.Checked = value;
			}
		}

		public Color ActiveClientHighlightColor
		{
			get
			{
				return this._activeClientHighlightColor;
			}
			set
			{
				this._activeClientHighlightColor = value;
				this.ActiveClientHighlightColorButton.BackColor = value;
			}
		}
		private Color _activeClientHighlightColor;

		public bool SyncThumbnailSizes
		{
			get
			{
				return this.SyncCheckbox.Checked;
			}
			set
			{
				this.SyncCheckbox.Checked = value;
			}
		}

		public bool LockThumbnails
		{
			get
			{
				return this.LockCheckbox.Checked;
			}
			set
			{
				this.LockCheckbox.Checked = value;
			}
		}

		public new void Show()
		{
			// Registers the current instance as the application's Main Form
			this._context.MainForm = this;

			this._suppressEvents = true;
			this.FormActivated?.Invoke();
			this._suppressEvents = false;

			Application.Run(this._context);
		}

		public void SetThumbnailSizeLimitations(Size minimumSize, Size maximumSize)
		{
			this._minimumSize = minimumSize;
			this._maximumSize = maximumSize;
		}

		public void Minimize()
		{
			this.WindowState = FormWindowState.Minimized;
		}

		public void SetForumUrl(string url)
		{
			this.ForumLinkLabel.Text = url;
		}

		public void AddThumbnails(IList<IThumbnailDescriptionView> thumbnails)
		{
			if (thumbnails.Count == 0)
			{
				return;
			}

			this.ThumbnailsList.BeginUpdate();

			foreach (IThumbnailDescriptionView view in thumbnails)
			{
				this.ThumbnailsList.Items.Add(view);
			}

			this.ThumbnailsList.EndUpdate();
		}

		public void UpdateThumbnails(IList<IThumbnailDescriptionView> thumbnails)
		{
			// Just trigger redraw
			if (thumbnails.Count > 0)
			{
				this.ThumbnailsList.Invalidate();
			}
		}

		public void RemoveThumbnails(IList<IThumbnailDescriptionView> thumbnails)
		{
			if (thumbnails.Count == 0)
			{
				return;
			}

			this.ThumbnailsList.BeginUpdate();

			foreach (IThumbnailDescriptionView view in thumbnails)
			{
				this.ThumbnailsList.Items.Remove(view);
			}

			this.ThumbnailsList.EndUpdate();
		}

		public void RefreshZoomSettings()
		{
			bool enableControls = this.EnableThumbnailZoom;
			this.ThumbnailZoomFactorNumericEdit.Enabled = enableControls;
			this.ZoomAnchorPanel.Enabled = enableControls;
		}

		public Action ApplicationExitRequested { get; set; }

		public Action FormActivated { get; set; }

		public Action FormMinimized { get; set; }

		public Action<ViewCloseRequest> FormCloseRequested { get; set; }

		public Action ApplicationSettingsChanged { get; set; }

		public Action ThumbnailsSizeChanged { get; set; }

		public Action<IntPtr> ThumbnailStateChanged { get; set; }

		public Action ForumUrlLinkActivated { get; set; }

		public Action SyncChanged { get; set; }

		public Action LockChanged { get; set; }

		#region UI events
		private void OptionChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			this.ApplicationSettingsChanged?.Invoke();
		}

		private void ThumbnailSizeChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			// Perform some View work that is not properly done in the Control
			this._suppressEvents = true;
			Size thumbnailSize = this.ThumbnailSize;
			thumbnailSize.Width = Math.Min(Math.Max(thumbnailSize.Width, this._minimumSize.Width), this._maximumSize.Width);
			thumbnailSize.Height = Math.Min(Math.Max(thumbnailSize.Height, this._minimumSize.Height), this._maximumSize.Height);
			this.ThumbnailSize = thumbnailSize;
			this._suppressEvents = false;

			this.ThumbnailsSizeChanged?.Invoke();
		}

		private void ActiveClientHighlightColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog())
			{
				dialog.Color = this.ActiveClientHighlightColor;

				if (dialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				this.ActiveClientHighlightColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void ThumbnailsList_ItemCheck_Handler(object sender, ItemCheckEventArgs e)
		{
			IThumbnailDescriptionView selectedItem = this.ThumbnailsList.Items[e.Index] as IThumbnailDescriptionView;
			if (selectedItem == null)
			{
				return;
			}
			selectedItem.IsDisabled = (e.NewValue == CheckState.Checked);

			this.ThumbnailStateChanged?.Invoke(selectedItem.Id);
		}

		private void ForumLinkLabelClicked_Handler(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.ForumUrlLinkActivated?.Invoke();
		}

		private void MainFormResize_Handler(object sender, EventArgs e)
		{
			if (this.WindowState != FormWindowState.Minimized)
			{
				return;
			}

			this.FormMinimized?.Invoke();
		}

		private void MainFormClosing_Handler(object sender, FormClosingEventArgs e)
		{
			ViewCloseRequest request = new ViewCloseRequest();

			this.FormCloseRequested?.Invoke(request);

			e.Cancel = !request.Allow;
		}

		private void RestoreMainForm_Handler(object sender, EventArgs e)
		{
			// This is form's GUI lifecycle event that is invariant to the Form data
			base.Show();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		private void ExitMenuItemClick_Handler(object sender, EventArgs e)
		{
			this.ApplicationExitRequested?.Invoke();
		}
		#endregion

		private void InitZoomAnchorMap()
		{
			this._zoomAnchorMap[ViewZoomAnchor.NW] = this.ZoomAanchorNWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.N] = this.ZoomAanchorNRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.NE] = this.ZoomAanchorNERadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.W] = this.ZoomAanchorWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.C] = this.ZoomAanchorCRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.E] = this.ZoomAanchorERadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.SW] = this.ZoomAanchorSWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.S] = this.ZoomAanchorSRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.SE] = this.ZoomAanchorSERadioButton;
		}

		private void SyncCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplicationSettingsChanged?.Invoke();
			this.SyncChanged?.Invoke();
		}

		private void LockCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplicationSettingsChanged?.Invoke();
			this.LockChanged?.Invoke();
		}
	}
}