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
		private const string ForumUrl = @"https://forums.eveonline.com/default.aspx?g=posts&t=484927";
		#endregion

		#region Private fields
		private readonly IThumbnailConfig _configuration;
		private readonly IConfigurationStorage _configurationStorage;
		private readonly IThumbnailDescriptionViewFactory _thumbnailDescriptionViewFactory;
		private readonly IDictionary<IntPtr, IThumbnailDescriptionView> _thumbnailDescriptionViews;
		private readonly IThumbnailManager _thumbnailManager;

		private bool _exitApplication;
		#endregion

		public MainPresenter(IApplicationController controller, IMainView view, IThumbnailConfig configuration, IConfigurationStorage configurationStorage,
								IThumbnailManager thumbnailManager, IThumbnailDescriptionViewFactory thumbnailDescriptionViewFactory)
			: base(controller, view)
		{
			this._configuration = configuration;
			this._configurationStorage = configurationStorage;

			this._thumbnailDescriptionViewFactory = thumbnailDescriptionViewFactory;
			this._thumbnailManager = thumbnailManager;

			this._thumbnailDescriptionViews = new Dictionary<IntPtr, IThumbnailDescriptionView>();
			this._exitApplication = false;

			this.View.FormActivated = this.Activate;
			this.View.FormMinimized = this.Minimize;
			this.View.FormCloseRequested = this.Close;
			this.View.ApplicationSettingsChanged = this.SaveApplicationSettings;
			this.View.ThumbnailsSizeChanged = this.UpdateThumbnailsSize;
			this.View.ThumbnailStateChanged = this.UpdateThumbnailState;
			this.View.DocumentationLinkActivated = this.OpenDocumentationLink;
			this.View.ApplicationExitRequested = this.ExitApplication;

			this._thumbnailManager.ThumbnailsAdded = this.ThumbnailsAdded;
			this._thumbnailManager.ThumbnailsUpdated = this.ThumbnailsUpdated;
			this._thumbnailManager.ThumbnailsRemoved = this.ThumbnailsRemoved;

			this._thumbnailManager.ThumbnailPositionChanged = this.ThumbnailPositionChanged;
			this._thumbnailManager.ThumbnailSizeChanged = this.ThumbnailSizeChanged;
		}

		private void Activate()
		{
			this.LoadApplicationSettings();
			this.View.SetDocumentationUrl(MainPresenter.ForumUrl);
			this.View.SetVersionInfo(this.GetApplicationVersion());
			if (this._configuration.MinimizeToTray)
			{
				this.View.Minimize();
			}

			this._thumbnailManager.Activate();
		}

		private void Minimize()
		{
			if (!this._configuration.MinimizeToTray)
			{
				return;
			}

			this.View.Hide();
		}

		private void Close(ViewCloseRequest request)
		{
			if (this._exitApplication || !this.View.MinimizeToTray)
			{
				this._thumbnailManager.Deactivate();
				this._configurationStorage.Save();
				request.Allow = true;
				return;
			}

			request.Allow = false;
			this.View.Minimize();
		}

		private void UpdateThumbnailsSize()
		{
			this._thumbnailManager.SetThumbnailsSize(this.View.ThumbnailSize);
			this.SaveApplicationSettings();
		}

		private void LoadApplicationSettings()
		{
			this._configurationStorage.Load();

			this.View.MinimizeToTray = this._configuration.MinimizeToTray;

			this.View.ThumbnailOpacity = this._configuration.ThumbnailOpacity;

			this.View.EnableClientLayoutTracking = this._configuration.EnableClientLayoutTracking;
			this.View.HideActiveClientThumbnail = this._configuration.HideActiveClientThumbnail;
			this.View.ShowThumbnailsAlwaysOnTop = this._configuration.ShowThumbnailsAlwaysOnTop;
			this.View.HideThumbnailsOnLostFocus = this._configuration.HideThumbnailsOnLostFocus;
			this.View.EnablePerClientThumbnailLayouts = this._configuration.EnablePerClientThumbnailLayouts;

			this.View.SetThumbnailSizeLimitations(this._configuration.ThumbnailMinimumSize, this._configuration.ThumbnailMaximumSize);
			this.View.ThumbnailSize = this._configuration.ThumbnailSize;

			this.View.EnableThumbnailZoom = this._configuration.ThumbnailZoomEnabled;
			this.View.ThumbnailZoomFactor = this._configuration.ThumbnailZoomFactor;
			this.View.ThumbnailZoomAnchor = ViewZoomAnchorConverter.Convert(this._configuration.ThumbnailZoomAnchor);

			this.View.ShowThumbnailOverlays = this._configuration.ShowThumbnailOverlays;
			this.View.ShowThumbnailFrames = this._configuration.ShowThumbnailFrames;
			this.View.EnableActiveClientHighlight = this._configuration.EnableActiveClientHighlight;
			this.View.ActiveClientHighlightColor = this._configuration.ActiveClientHighlightColor;
		}

		private void SaveApplicationSettings()
		{
			this._configuration.MinimizeToTray = this.View.MinimizeToTray;

			this._configuration.ThumbnailOpacity = (float)this.View.ThumbnailOpacity;

			this._configuration.EnableClientLayoutTracking = this.View.EnableClientLayoutTracking;
			this._configuration.HideActiveClientThumbnail = this.View.HideActiveClientThumbnail;
			this._configuration.ShowThumbnailsAlwaysOnTop = this.View.ShowThumbnailsAlwaysOnTop;
			this._configuration.HideThumbnailsOnLostFocus = this.View.HideThumbnailsOnLostFocus;
			this._configuration.EnablePerClientThumbnailLayouts = this.View.EnablePerClientThumbnailLayouts;

			this._configuration.ThumbnailSize = this.View.ThumbnailSize;

			this._configuration.ThumbnailZoomEnabled = this.View.EnableThumbnailZoom;
			this._configuration.ThumbnailZoomFactor = this.View.ThumbnailZoomFactor;
			this._configuration.ThumbnailZoomAnchor = ViewZoomAnchorConverter.Convert(this.View.ThumbnailZoomAnchor);

			this._configuration.ShowThumbnailOverlays = this.View.ShowThumbnailOverlays;
			this._configuration.ShowThumbnailFrames = this.View.ShowThumbnailFrames;
			this._configuration.EnableActiveClientHighlight = this.View.EnableActiveClientHighlight;
			this._configuration.ActiveClientHighlightColor = this.View.ActiveClientHighlightColor;

			this._configurationStorage.Save();

			this.View.RefreshZoomSettings();

			this._thumbnailManager.SetupThumbnailFrames();
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

		private void ThumbnailPositionChanged(String thumbnailName, String activeClientName, Point location)
		{
			this._configuration.SetThumbnailLocation(thumbnailName, activeClientName, location);
			this._configurationStorage.Save();
		}

		private void ThumbnailSizeChanged(Size size)
		{
			this.View.ThumbnailSize = size;
		}

		private void UpdateThumbnailState(IntPtr thumbnailId)
		{
			this._thumbnailManager.SetThumbnailState(thumbnailId, this._thumbnailDescriptionViews[thumbnailId].IsDisabled);
		}

		private void OpenDocumentationLink()
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(new Uri(MainPresenter.ForumUrl).AbsoluteUri);
			Process.Start(processStartInfo);
		}

		private string GetApplicationVersion()
		{
			Version version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
			return String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
		}

		private void ExitApplication()
		{
			this._exitApplication = true;
			this.View.Close();
		}
	}
}