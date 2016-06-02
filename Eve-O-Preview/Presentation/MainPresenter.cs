using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using EveOPreview.Configuration;

namespace EveOPreview.UI
{
	public class MainPresenter : Presenter<IMainView>
	{
		#region Private constants
		private const string ForumUrl = @"https://forums.eveonline.com/default.aspx?g=posts&t=389086";
		#endregion

		#region Private fields
		private readonly IApplicationConfiguration _configuration;
		private readonly IConfigurationStorage _configurationStorage;
		private readonly IThumbnailDescriptionViewFactory _thumbnailDescriptionViewFactory;
		private readonly IDictionary<IntPtr, IThumbnailDescriptionView> _thumbnailDescriptionViews;
		private readonly IThumbnailManager _thumbnailManager;

		private bool _exitApplication;
		#endregion

		public MainPresenter(IApplicationController controller, IMainView view, IApplicationConfiguration configuration, IConfigurationStorage configurationStorage,
																IThumbnailDescriptionViewFactory thumbnailDescriptionViewFactory, IThumbnailManager thumbnailManager)
			: base(controller, view)
		{
			this._configuration = configuration;
			this._configurationStorage = configurationStorage;

			this._thumbnailDescriptionViewFactory = thumbnailDescriptionViewFactory;
			this._thumbnailManager = thumbnailManager;

			this._thumbnailDescriptionViews = new Dictionary<IntPtr, IThumbnailDescriptionView>();
			this._exitApplication = false;

			this.View.ApplicationExitRequested += ExitApplication;
			this.View.FormActivated += Activate;
			this.View.FormMinimized += Minimize;
			this.View.FormCloseRequested += Close;
			this.View.ApplicationSettingsChanged += SaveApplicationSettings;
			this.View.ThumbnailsSizeChanged += UpdateThumbnailsSize;
			this.View.ThumbnailStateChanged += UpdateThumbnailState;
			this.View.ForumUrlLinkActivated += OpenForumUrlLink;

			this._thumbnailManager.ThumbnailsAdded += ThumbnailsAdded;
			this._thumbnailManager.ThumbnailsUpdated += ThumbnailsUpdated;
			this._thumbnailManager.ThumbnailsRemoved += ThumbnailsRemoved;
			this._thumbnailManager.ThumbnailSizeChanged += ThumbnailSizeChanged;
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

			this._thumbnailManager.Activate();
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
			this._thumbnailManager.SetThumbnailsSize(new Size(this.View.ThumbnailsWidth, this.View.ThumbnailsHeight));
			this.SaveApplicationSettings();
		}

		private void LoadApplicationSettings()
		{
			this._configurationStorage.Load();

			this.View.MinimizeToTray = this._configuration.MinimizeToTray;

			this.View.ThumbnailsOpacity = this._configuration.ThumbnailsOpacity;

			this.View.EnableClientsLocationTracking = this._configuration.EnableClientsLocationTracking;
			this.View.HideActiveClientThumbnail = this._configuration.HideActiveClientThumbnail;
			this.View.ShowThumbnailsAlwaysOnTop = this._configuration.ShowThumbnailsAlwaysOnTop;
			this.View.HideThumbnailsOnLostFocus = this._configuration.HideThumbnailsOnLostFocus;
			this.View.EnablePerClientThumbnailsLayouts = this._configuration.EnablePerClientThumbnailsLayouts;

			this.View.SyncThumbnailsSize = this._configuration.SyncThumbnailsSize;
			this.View.ThumbnailsWidth = this._configuration.ThumbnailsWidth;
			this.View.ThumbnailsHeight = this._configuration.ThumbnailsHeight;

			this.View.EnableThumbnailZoom = this._configuration.EnableThumbnailZoom;
			this.View.ThumbnailZoomFactor = this._configuration.ThumbnailZoomFactor;
			this.View.ThumbnailZoomAnchor = ViewZoomAnchorConverter.Convert(this._configuration.ThumbnailZoomAnchor);

			this.View.ShowThumbnailOverlays = this._configuration.ShowThumbnailOverlays;
			this.View.ShowThumbnailFrames = this._configuration.ShowThumbnailFrames;
		}

		private void SaveApplicationSettings()
		{
			this._configuration.MinimizeToTray = this.View.MinimizeToTray;

			this._configuration.ThumbnailsOpacity = (float)this.View.ThumbnailsOpacity;

			this._configuration.EnableClientsLocationTracking = this.View.EnableClientsLocationTracking;
			this._configuration.HideActiveClientThumbnail = this.View.HideActiveClientThumbnail;
			this._configuration.ShowThumbnailsAlwaysOnTop = this.View.ShowThumbnailsAlwaysOnTop;
			this._configuration.HideThumbnailsOnLostFocus = this.View.HideThumbnailsOnLostFocus;
			this._configuration.EnablePerClientThumbnailsLayouts = this.View.EnablePerClientThumbnailsLayouts;

			this._configuration.SyncThumbnailsSize = this.View.SyncThumbnailsSize;
			this._configuration.ThumbnailsWidth = this.View.ThumbnailsWidth;
			this._configuration.ThumbnailsHeight = this.View.ThumbnailsHeight;

			this._configuration.EnableThumbnailZoom = this.View.EnableThumbnailZoom;
			this._configuration.ThumbnailZoomFactor = this.View.ThumbnailZoomFactor;
			this._configuration.ThumbnailZoomAnchor = ViewZoomAnchorConverter.Convert(this.View.ThumbnailZoomAnchor);

			this._configuration.ShowThumbnailOverlays = this.View.ShowThumbnailOverlays;
			this._configuration.ShowThumbnailFrames = this.View.ShowThumbnailFrames;

			this._configurationStorage.Save();

			this.View.UpdateZoomSettingsView();

			this._thumbnailManager.SetupThumbnailFrames();
			this._thumbnailManager.RefreshThumbnails();
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
			lock (this._thumbnailDescriptionViews)
			{
				foreach (IThumbnailView thumbnail in thumbnails)
				{
					IThumbnailDescriptionView thumbnailView;
					bool foundInCache = this._thumbnailDescriptionViews.TryGetValue(thumbnail.Id, out thumbnailView);

					if (!foundInCache)
					{
						if (removeFromCache)
						{
							// This item was not even cached
							continue;
						}

						thumbnailView = this._thumbnailDescriptionViewFactory.Create(thumbnail.Id, thumbnail.Title, !thumbnail.IsEnabled);
						this._thumbnailDescriptionViews.Add(thumbnail.Id, thumbnailView);
					}
					else
					{
						if (removeFromCache)
						{
							this._thumbnailDescriptionViews.Remove(thumbnail.Id);
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
			this._thumbnailManager.SetThumbnailState(thumbnailId, this._thumbnailDescriptionViews[thumbnailId].IsDisabled);
		}

		private void OpenForumUrlLink()
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(new Uri(MainPresenter.ForumUrl).AbsoluteUri);
			Process.Start(processStartInfo);
		}
	}
}