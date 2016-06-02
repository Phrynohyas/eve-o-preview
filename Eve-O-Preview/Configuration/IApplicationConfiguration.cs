namespace EveOPreview.Configuration
{
	public interface IApplicationConfiguration
	{
		bool MinimizeToTray { get; set; }
		int ThumbnailRefreshPeriod { get; set; }

		double ThumbnailsOpacity { get; set; }

		bool EnableClientsLocationTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		bool EnablePerClientThumbnailsLayouts { get; set; }

		bool SyncThumbnailsSize { get; set; }
		int ThumbnailsWidth { get; set; }
		int ThumbnailsHeight { get; set; }

		bool EnableThumbnailZoom { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ZoomAnchor ThumbnailZoomAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowThumbnailFrames { get; set; }
	}
}