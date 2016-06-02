namespace EveOPreview.Configuration
{
	public class ApplicationConfiguration : IApplicationConfiguration
	{
		public ApplicationConfiguration()
		{
			// Default values
			this.MinimizeToTray = true;
			this.ThumbnailRefreshPeriod = 500;

			this.ThumbnailsOpacity = 0.5;

			this.EnableClientsLocationTracking = false;
			this.HideActiveClientThumbnail = false;
			this.ShowThumbnailsAlwaysOnTop = true;
			this.HideThumbnailsOnLostFocus = false;
			this.EnablePerClientThumbnailsLayouts = false;

			this.SyncThumbnailsSize = true;
			this.ThumbnailsWidth = 250;
			this.ThumbnailsHeight = 150;

			this.EnableThumbnailZoom = false;
			this.ThumbnailZoomFactor = 2;
			this.ThumbnailZoomAnchor = ZoomAnchor.NW;

			this.ShowThumbnailOverlays = true;
			this.ShowThumbnailFrames = true;
		}

		public bool MinimizeToTray { get; set; }
		public int ThumbnailRefreshPeriod { get; set; }

		public double ThumbnailsOpacity { get; set; }

		public bool EnableClientsLocationTracking { get; set; }
		public bool HideActiveClientThumbnail { get; set; }
		public bool ShowThumbnailsAlwaysOnTop { get; set; }
		public bool HideThumbnailsOnLostFocus { get; set; }
		public bool EnablePerClientThumbnailsLayouts { get; set; }

		public bool SyncThumbnailsSize { get; set; }
		public int ThumbnailsWidth { get; set; }
		public int ThumbnailsHeight { get; set; }

		public bool EnableThumbnailZoom { get; set; }
		public int ThumbnailZoomFactor { get; set; }
		public ZoomAnchor ThumbnailZoomAnchor { get; set; }

		public bool ShowThumbnailOverlays { get; set; }
		public bool ShowThumbnailFrames { get; set; }
	}
}