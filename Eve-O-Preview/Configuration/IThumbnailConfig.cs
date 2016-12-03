﻿using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.Configuration
{
	/// <summary>
	/// Thumbnails Manager configuration
	/// </summary>
	public interface IThumbnailConfig
	{
		bool MinimizeToTray { get; set; }
		int ThumbnailRefreshPeriod { get; set; }

		double ThumbnailOpacity { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		bool EnablePerClientThumbnailLayouts { get; set; }

		Size ThumbnailDefaultSize { get; set; }
		Size ThumbnailMinimumSize { get; set; }
		Size ThumbnailMaximumSize { get; set; }

		bool ThumbnailZoomEnabled { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ZoomAnchor ThumbnailZoomAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowThumbnailFrames { get; set; }

		bool EnableActiveClientHighlight { get; set; }
		Color ActiveClientHighlightColor { get; set; }
		int ActiveClientHighlightThickness { get; set; }

		bool SyncThumbnailSizes { get; set; }

		bool LockThumbnails { get; set; }

		Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation);
		void SetThumbnailLocation(string currentClient, string activeClient, Point location);

        Size GetThumbnailSize(string currentClient, string activeClient, Size defaultSize);
        void SetThumbnailSize(string currentClient, string activeClient, Size size);

        ClientLayout GetClientLayout(string currentClient);
		void SetClientLayout(string currentClient, ClientLayout layout);

		Keys GetClientHotkey(string currentClient);
		void SetClientHotkey(string currentClient, Keys hotkey);

		void ApplyRestrictions();
	}
}