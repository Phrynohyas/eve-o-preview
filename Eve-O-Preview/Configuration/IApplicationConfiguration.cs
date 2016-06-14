using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.Configuration
{
	public interface IApplicationConfiguration
	{
		bool MinimizeToTray { get; set; }
		int ThumbnailRefreshPeriod { get; set; }

		double ThumbnailsOpacity { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		bool EnablePerClientThumbnailLayouts { get; set; }

		bool SyncThumbnailsSize { get; set; }
		Size ThumbnailSize { get; set; }
		Size ThumbnailMinimumSize { get; set; }
		Size ThumbnailMaximumSize { get; set; }

		bool EnableThumbnailZoom { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ZoomAnchor ThumbnailZoomAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowThumbnailFrames { get; set; }

		Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation);
		void SetThumbnailLocation(string currentClient, string activeClient, Point location);

		ClientLayout GetClientLayout(string currentClient);
		void SetClientLayout(string currentClient, ClientLayout layout);

		Keys GetClientHotkey(string currentClient);
		void SetClientHotkey(string currentClient, Keys hotkey);
	}
}