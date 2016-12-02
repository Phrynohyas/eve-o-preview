using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	class ThumbnailConfig : IThumbnailConfig
	{
		public ThumbnailConfig()
		{
			this.MinimizeToTray = false;
			this.ThumbnailRefreshPeriod = 500;

			this.ThumbnailOpacity = 0.5;

			this.EnableClientLayoutTracking = false;
			this.HideActiveClientThumbnail = false;
			this.ShowThumbnailsAlwaysOnTop = true;
			this.HideThumbnailsOnLostFocus = false;
			this.EnablePerClientThumbnailLayouts = false;

			this.ThumbnailDefaultSize = new Size(250, 150);
			this.ThumbnailMinimumSize = new Size(100, 80);
			this.ThumbnailMaximumSize = new Size(640, 400);

			this.ThumbnailZoomEnabled = false;
			this.ThumbnailZoomFactor = 2;
			this.ThumbnailZoomAnchor = ZoomAnchor.NW;

			this.ShowThumbnailOverlays = true;
			this.ShowThumbnailFrames = true;

			this.EnableActiveClientHighlight = false;
			this.ActiveClientHighlightColor = Color.GreenYellow;
			this.ActiveClientHighlightThickness = 3;

			this.SyncThumbnailSizes = false;

			this.PerClientSizes = new Dictionary<string, Dictionary<string, Size>>();
            this.FlatSizes = new Dictionary<string, Size>();

			this.PerClientLayout = new Dictionary<string, Dictionary<string, Point>>();
			this.FlatLayout = new Dictionary<string, Point>();
			this.ClientLayout = new Dictionary<string, ClientLayout>();
			this.ClientHotkey = new Dictionary<string, string>();
		}

		public bool MinimizeToTray { get; set; }
		public int ThumbnailRefreshPeriod { get; set; }

		[JsonProperty("ThumbnailsOpacity")]
		public double ThumbnailOpacity { get; set; }

		public bool EnableClientLayoutTracking { get; set; }
		public bool HideActiveClientThumbnail { get; set; }
		public bool ShowThumbnailsAlwaysOnTop { get; set; }
		public bool HideThumbnailsOnLostFocus { get; set; }
		public bool EnablePerClientThumbnailLayouts { get; set; }

		[JsonProperty("ThumbnailSize")]
		public Size ThumbnailDefaultSize { get; set; }
		public Size ThumbnailMaximumSize { get; set; }
		public Size ThumbnailMinimumSize { get; set; }

		[JsonProperty("EnableThumbnailZoom")]
		public bool ThumbnailZoomEnabled { get; set; }
		public int ThumbnailZoomFactor { get; set; }
		public ZoomAnchor ThumbnailZoomAnchor { get; set; }

		public bool ShowThumbnailOverlays { get; set; }
		public bool ShowThumbnailFrames { get; set; }

		public bool EnableActiveClientHighlight { get; set; }

		public Color ActiveClientHighlightColor { get; set; }

		public int ActiveClientHighlightThickness { get; set; }

		[JsonProperty("SyncSizesToDefault")]
		public bool SyncThumbnailSizes { get; set; }

        [JsonProperty]
        private Dictionary<string, Dictionary<string, Size>> PerClientSizes { get; set; }
        [JsonProperty]
        private Dictionary<string, Size> FlatSizes { get; set; }

        [JsonProperty]
		private Dictionary<string, Dictionary<string, Point>> PerClientLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, Point> FlatLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, ClientLayout> ClientLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, string> ClientHotkey { get; set; }

		public Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation)
		{
			Point location;

			// What this code does:
			// If Per-Client layouts are enabled
			//    and client name is known
			//    and there is a separate thumbnails layout for this client
			//    and this layout contains an entry for the current client
			// then return that entry
			// otherwise try to get client layout from the flat all-clients layout
			// If there is no layout too then use the default one
			if (this.EnablePerClientThumbnailLayouts && !string.IsNullOrEmpty(activeClient))
			{
				Dictionary<string, Point> layoutSource;
				if (this.PerClientLayout.TryGetValue(activeClient, out layoutSource) && layoutSource.TryGetValue(currentClient, out location))
				{
					return location;
				}
			}

			return this.FlatLayout.TryGetValue(currentClient, out location) ? location : defaultLocation;
		}

		public void SetThumbnailLocation(string currentClient, string activeClient, Point location)
		{
			Dictionary<string, Point> layoutSource;

			if (this.EnablePerClientThumbnailLayouts)
			{
				if (string.IsNullOrEmpty(activeClient))
				{
					return;
				}

				if (!this.PerClientLayout.TryGetValue(activeClient, out layoutSource))
				{
					layoutSource = new Dictionary<string, Point>();
					this.PerClientLayout[activeClient] = layoutSource;
				}
			}
			else
			{
				layoutSource = this.FlatLayout;
			}

			layoutSource[currentClient] = location;
		}

        public Size GetThumbnailSize(string currentClient, string activeClient, Size defaultSize)
        {
            Size size;

            // What this code does:
            // If Per-Client layouts are enabled
            //    and client name is known
            //    and there is a separate thumbnails layout for this client
            //    and this layout contains an entry for the current client
            // then return that entry
            // otherwise try to get client layout from the flat all-clients layout
            // If there is no layout too then use the default one
            if (this.EnablePerClientThumbnailLayouts && !string.IsNullOrEmpty(activeClient))
            {
                Dictionary<string, Size> layoutSource;
                if (this.PerClientSizes.TryGetValue(activeClient, out layoutSource) && layoutSource.TryGetValue(currentClient, out size))
                {
                    return size;
                }
            }

            return this.FlatSizes.TryGetValue(currentClient, out size) ? size : defaultSize;
        }

        public void SetThumbnailSize(string currentClient, string activeClient, Size size)
        {
            Dictionary<string, Size> layoutSource;

            if (this.EnablePerClientThumbnailLayouts)
            {
                if (string.IsNullOrEmpty(activeClient))
                {
                    return;
                }

                if (!this.PerClientSizes.TryGetValue(activeClient, out layoutSource))
                {
                    layoutSource = new Dictionary<string, Size>();
                    this.PerClientSizes[activeClient] = layoutSource;
                }
            }
            else
            {
                layoutSource = this.FlatSizes;
            }

            layoutSource[currentClient] = size;
        }

        public ClientLayout GetClientLayout(string currentClient)
		{
			ClientLayout layout;
			this.ClientLayout.TryGetValue(currentClient, out layout);

			return layout;
		}

		public void SetClientLayout(string currentClient, ClientLayout layout)
		{
			this.ClientLayout[currentClient] = layout;
		}

		public Keys GetClientHotkey(string currentClient)
		{
			string hotkey;
			if (this.ClientHotkey.TryGetValue(currentClient, out hotkey))
			{
				// Protect from incorrect values
				object rawValue = (new KeysConverter()).ConvertFromInvariantString(hotkey);
				return rawValue != null ? (Keys)rawValue : Keys.None;
			}

			return Keys.None;
		}

		public void SetClientHotkey(string currentClient, Keys hotkey)
		{
			this.ClientHotkey[currentClient] = (new KeysConverter()).ConvertToInvariantString(hotkey);
		}

		/// <summary>
		/// Applies restrictions to different parameters of the config
		/// </summary>
		public void ApplyRestrictions()
		{
			this.ThumbnailRefreshPeriod = ThumbnailConfig.ApplyRestrictions(this.ThumbnailRefreshPeriod, 300, 1000);
			this.ThumbnailDefaultSize = new Size(ThumbnailConfig.ApplyRestrictions(this.ThumbnailDefaultSize.Width, this.ThumbnailMinimumSize.Width, this.ThumbnailMaximumSize.Width),
				ThumbnailConfig.ApplyRestrictions(this.ThumbnailDefaultSize.Height, this.ThumbnailMinimumSize.Height, this.ThumbnailMaximumSize.Height));
			this.ThumbnailOpacity = ThumbnailConfig.ApplyRestrictions((int)(this.ThumbnailOpacity * 100.00), 20, 100) / 100.00;
			this.ThumbnailZoomFactor = ThumbnailConfig.ApplyRestrictions(this.ThumbnailZoomFactor, 2, 10);
			this.ActiveClientHighlightThickness = ThumbnailConfig.ApplyRestrictions(this.ActiveClientHighlightThickness, 1, 6);
		}

		private static int ApplyRestrictions(int value, int minimum, int maximum)
		{
			if (value <= minimum)
			{
				return minimum;
			}

			if (value >= maximum)
			{
				return maximum;
			}

			return value;
		}
	}
}