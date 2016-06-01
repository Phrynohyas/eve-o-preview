using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace EveOPreview.UI
{
	public class MainPresenter : Presenter<IMainView>
	{
		private const string ForumUrl = @"https://forums.eveonline.com/default.aspx?g=posts&t=389086";

		private readonly IThumbnailDescriptionViewFactory _thumbnailDescriptionViewFactory;
		private readonly IThumbnailManager _manager;
		private readonly IDictionary<IntPtr, IThumbnailDescriptionView> _thumbnailViews;

		private bool _exitApplication;

		public MainPresenter(IApplicationController controller, IMainView view, IThumbnailDescriptionViewFactory thumbnailDescriptionViewFactory, IThumbnailManager manager)
			: base(controller, view)
		{
			this._thumbnailDescriptionViewFactory = thumbnailDescriptionViewFactory;
			this._manager = manager;

			this._thumbnailViews = new Dictionary<IntPtr, IThumbnailDescriptionView>();
			this._exitApplication = false;

			this.View.ApplicationExitRequested += ExitApplication;
			this.View.FormActivated += Activate;
			this.View.FormMinimized += Minimize;
			this.View.FormCloseRequested += Close;
			this.View.ApplicationSettingsChanged += SaveApplicationSettings;
			this.View.ThumbnailsSizeChanged += UpdateThumbnailsSize;
			this.View.ThumbnailStateChanged += UpdateThumbnailState;
			this.View.ForumUrlLinkActivated += OpenForumUrlLink;

			this._manager.ThumbnailsAdded += ThumbnailsAdded;
			this._manager.ThumbnailsUpdated += ThumbnailsUpdated;
			this._manager.ThumbnailsRemoved += ThumbnailsRemoved;
			this._manager.ThumbnailSizeChanged += ThumbnailSizeChanged;
		}

		private void ExitApplication()
		{
			this._exitApplication = true;
			this.View.Close();
		}

		private void Activate()
		{
			this.LoadApplicationSettings();
			this.View.SetForumUrl(MainPresenter.ForumUrl);

			this._manager.Activate();
		}

		private void Minimize()
		{
			if (!this.View.MinimizeToTray)
			{
				return;
			}

			this.View.Hide();
		}

		private void Close(ViewCloseRequest request)
		{
			if (this._exitApplication || !this.View.MinimizeToTray)
			{
				request.Allow = true;
				return;
			}

			request.Allow = false;
			this.View.Minimize();
		}

		private void UpdateThumbnailsSize()
		{
			this._manager.SetThumbnailsSize(new Size(this.View.ThumbnailsWidth, this.View.ThumbnailsHeight));
			this.SaveApplicationSettings();
		}

		private void LoadApplicationSettings()
		{
			this.View.MinimizeToTray = Properties.Settings.Default.minimizeToTray;
			this.View.ThumbnailsOpacity = Properties.Settings.Default.opacity;
			this.View.TrackClientLocations = Properties.Settings.Default.track_client_windows;
			this.View.HideActiveClientThumbnail = Properties.Settings.Default.hide_active;
			this.View.ShowThumbnailsAlwaysOnTop = Properties.Settings.Default.always_on_top;
			this.View.HideAllThumbnailsIfClientIsNotActive = Properties.Settings.Default.hide_all;
			this.View.EnableUniqueThumbnailsLayouts = Properties.Settings.Default.unique_layout;

			this.View.SyncThumbnailsSize = Properties.Settings.Default.sync_resize;
			this.View.ThumbnailsWidth = (int)Properties.Settings.Default.sync_resize_x;
			this.View.ThumbnailsHeight = (int)Properties.Settings.Default.sync_resize_y;

			this.View.EnableZoomOnHover = Properties.Settings.Default.zoom_on_hover;
			this.View.ZoomFactor = (int)Properties.Settings.Default.zoom_amount;
			this.View.ZoomAnchor = (ZoomAnchor)Properties.Settings.Default.zoom_anchor;

			this.View.ShowThumbnailFrames = Properties.Settings.Default.show_thumb_frames;
			this.View.ShowThumbnailOverlays = Properties.Settings.Default.show_overlay;
		}

		private void SaveApplicationSettings()
		{
			Properties.Settings.Default.minimizeToTray = this.View.MinimizeToTray;

			Properties.Settings.Default.opacity = (float)this.View.ThumbnailsOpacity;
			Properties.Settings.Default.track_client_windows = this.View.TrackClientLocations;
			Properties.Settings.Default.hide_active = this.View.HideActiveClientThumbnail;
			Properties.Settings.Default.always_on_top = this.View.ShowThumbnailsAlwaysOnTop;
			Properties.Settings.Default.hide_all = this.View.HideAllThumbnailsIfClientIsNotActive;
			Properties.Settings.Default.unique_layout = this.View.EnableUniqueThumbnailsLayouts;

			Properties.Settings.Default.sync_resize = this.View.SyncThumbnailsSize;
			Properties.Settings.Default.sync_resize_x = (uint)this.View.ThumbnailsWidth;
			Properties.Settings.Default.sync_resize_y = (uint)this.View.ThumbnailsHeight;

			Properties.Settings.Default.zoom_on_hover = this.View.EnableZoomOnHover;
			Properties.Settings.Default.zoom_amount = this.View.ZoomFactor;
			Properties.Settings.Default.zoom_anchor = (byte)this.View.ZoomAnchor;

			Properties.Settings.Default.show_overlay = this.View.ShowThumbnailOverlays;
			Properties.Settings.Default.show_thumb_frames = this.View.ShowThumbnailFrames;

			Properties.Settings.Default.Save();

			this.View.UpdateZoomSettingsView();

			this._manager.SetupThumbnailFrames();
			this._manager.RefreshThumbnails();
		}

		private void ThumbnailsAdded(IList<IThumbnailView> thumbnails)
		{
			this.View.AddThumbnails(this.GetThumbnailViews(thumbnails, false));
		}

		private void ThumbnailsUpdated(IList<IThumbnailView> thumbnails)
		{
			this.View.UpdateThumbnails(this.GetThumbnailViews(thumbnails, false));
		}

		private void ThumbnailsRemoved(IList<IThumbnailView> thumbnails)
		{
			this.View.RemoveThumbnails(this.GetThumbnailViews(thumbnails, true));
		}

		private IList<IThumbnailDescriptionView> GetThumbnailViews(IList<IThumbnailView> thumbnails, bool removeFromCache)
		{
			IList<IThumbnailDescriptionView> thumbnailViews = new List<IThumbnailDescriptionView>(thumbnails.Count);

			// Time for some thread safety
			lock (this._thumbnailViews)
			{
				foreach (IThumbnailView thumbnail in thumbnails)
				{
					IThumbnailDescriptionView thumbnailView;
					bool foundInCache = this._thumbnailViews.TryGetValue(thumbnail.Id, out thumbnailView);

					if (!foundInCache)
					{
						if (removeFromCache)
						{
							// This item was not even cached
							continue;
						}

						thumbnailView = this._thumbnailDescriptionViewFactory.Create(thumbnail.Id, thumbnail.Title, !thumbnail.IsEnabled);
						this._thumbnailViews.Add(thumbnail.Id, thumbnailView);
					}
					else
					{
						if (removeFromCache)
						{
							this._thumbnailViews.Remove(thumbnail.Id);
						}
						else
						{
							thumbnailView.Title = thumbnail.Title;
						}
					}

					thumbnailViews.Add(thumbnailView);
				}
			}

			return thumbnailViews;
		}

		private void ThumbnailSizeChanged(Size size)
		{
			this.View.UpdateThumbnailsSizeView(size);
		}

		private void UpdateThumbnailState(IntPtr thumbnailId)
		{
			this._manager.SetThumbnailState(thumbnailId, this._thumbnailViews[thumbnailId].IsDisabled);
		}

		private void OpenForumUrlLink()
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(new Uri(MainPresenter.ForumUrl).AbsoluteUri);
			Process.Start(processStartInfo);
		}
	}
}