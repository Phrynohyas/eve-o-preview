using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.Configuration
{
	public interface IThumbnailConfiguration
    {
        string CycleForwardHotkey { get; set; }
        string CycleForwardHotkey2 { get; set; }
        string CycleBackwardHotkey { get; set; }
        string CycleBackwardHotkey2 { get; set; }
        Dictionary<string, string> CycleClientsOrder { get; set; }

        string CycleGroup2ForwardHotkey { get; set; }
        string CycleGroup2ForwardHotkey2 { get; set; }
        string CycleGroup2BackwardHotkey { get; set; }
        string CycleGroup2BackwardHotkey2 { get; set; }
        Dictionary<string, string> CycleGroup2ClientsOrder { get; set; }

        Dictionary<string, Color> PerClientActiveClientHighlightColor { get; set; }        

        bool MinimizeToTray { get; set; }
		int ThumbnailRefreshPeriod { get; set; }

		bool EnableCompatibilityMode { get; set; }

		double ThumbnailOpacity { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool MinimizeInactiveClients { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool EnablePerClientThumbnailLayouts { get; set; }

		bool HideThumbnailsOnLostFocus { get; set; }
		int HideThumbnailsDelay { get; set; }

		Size ThumbnailSize { get; set; }
		Size ThumbnailMinimumSize { get; set; }
		Size ThumbnailMaximumSize { get; set; }

		bool EnableThumbnailSnap { get; set; }

		bool ThumbnailZoomEnabled { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ZoomAnchor ThumbnailZoomAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowThumbnailFrames { get; set; }

		bool EnableActiveClientHighlight { get; set; }
		Color ActiveClientHighlightColor { get; set; }
		int ActiveClientHighlightThickness { get; set; }

		Point GetDefaultThumbnailLocation();
		Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation);
		void SetThumbnailLocation(string currentClient, string activeClient, Point location);

		ClientLayout GetClientLayout(string currentClient);
		void SetClientLayout(string currentClient, ClientLayout layout);

		Keys GetClientHotkey(string currentClient);
		Keys StringToKey(string hotkey);
        void SetClientHotkey(string currentClient, Keys hotkey);

		bool IsPriorityClient(string currentClient);

		bool IsThumbnailDisabled(string currentClient);
		void ToggleThumbnail(string currentClient, bool isDisabled);

		void ApplyRestrictions();
	}
}