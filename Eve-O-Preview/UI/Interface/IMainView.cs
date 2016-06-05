using System;
using System.Collections.Generic;
using System.Drawing;

namespace EveOPreview.UI
{
	/// <summary>
	/// Main view interface
	/// Presenter uses it to access GUI properties
	/// </summary>
	public interface IMainView : IView
	{
		bool MinimizeToTray { get; set; }

		double ThumbnailsOpacity { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		bool EnablePerClientThumbnailsLayouts { get; set; }

		bool SyncThumbnailsSize { get; set; }
		Size ThumbnailSize { get; set; }

		bool EnableThumbnailZoom { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ViewZoomAnchor ThumbnailZoomAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowThumbnailFrames { get; set; }

		void SetForumUrl(string url);
		void SetThumbnailSizeLimitations(Size minimumSize, Size maximumSize);

		void Minimize();

		void AddThumbnails(IList<IThumbnailDescriptionView> thumbnails);
		void UpdateThumbnails(IList<IThumbnailDescriptionView> thumbnails);
		void RemoveThumbnails(IList<IThumbnailDescriptionView> thumbnails);
		void RefreshZoomSettings();

		event Action ApplicationExitRequested;
		event Action FormActivated;
		event Action FormMinimized;
		event Action<ViewCloseRequest> FormCloseRequested;
		event Action ApplicationSettingsChanged;
		event Action ThumbnailsSizeChanged;
		event Action<IntPtr> ThumbnailStateChanged;
		event Action ForumUrlLinkActivated;
	}
}