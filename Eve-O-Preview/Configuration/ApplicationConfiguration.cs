using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	public class ApplicationConfiguration : IApplicationConfiguration
	{
		public ApplicationConfiguration()
		{
			// Default values
			this.MinimizeToTray = false;
			this.ThumbnailRefreshPeriod = 500;

			this.ThumbnailsOpacity = 0.5;

			this.EnableClientLayoutTracking = false;
			this.HideActiveClientThumbnail = false;
			this.ShowThumbnailsAlwaysOnTop = true;
			this.HideThumbnailsOnLostFocus = false;
			this.EnablePerClientThumbnailLayouts = false;

			this.SyncThumbnailsSize = true;
			this.ThumbnailSize = new Size(250, 150);
			this.ThumbnailMinimumSize = new Size(100, 80);
			this.ThumbnailMaximumSize = new Size(640, 400);

			this.EnableThumbnailZoom = false;
			this.ThumbnailZoomFactor = 2;
			this.ThumbnailZoomAnchor = ZoomAnchor.NW;

			this.ShowThumbnailOverlays = true;
			this.ShowThumbnailFrames = true;

			this.PerClientLayout = new Dictionary<string, Dictionary<string, Point>>();
			this.FlatLayout = new Dictionary<string, Point>();
			this.ClientLayout = new Dictionary<string, ClientLayout>();
			this.ClientHotkey = new Dictionary<string, string>();
		}

		public bool MinimizeToTray { get; set; }
		public int ThumbnailRefreshPeriod { get; set; }

		public double ThumbnailsOpacity { get; set; }

		public bool EnableClientLayoutTracking { get; set; }
		public bool HideActiveClientThumbnail { get; set; }
		public bool ShowThumbnailsAlwaysOnTop { get; set; }
		public bool HideThumbnailsOnLostFocus { get; set; }
		public bool EnablePerClientThumbnailLayouts { get; set; }

		public bool SyncThumbnailsSize { get; set; }
		public Size ThumbnailSize { get; set; }
		public Size ThumbnailMaximumSize { get; set; }
		public Size ThumbnailMinimumSize { get; set; }

		public bool EnableThumbnailZoom { get; set; }
		public int ThumbnailZoomFactor { get; set; }
		public ZoomAnchor ThumbnailZoomAnchor { get; set; }

		public bool ShowThumbnailOverlays { get; set; }
		public bool ShowThumbnailFrames { get; set; }

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
			Dictionary<string, Point> layoutSource = null;

			if (this.EnablePerClientThumbnailLayouts)
			{
				if (!string.IsNullOrEmpty(activeClient))
				{
					this.PerClientLayout.TryGetValue(activeClient, out layoutSource);
				}
			}
			else
			{
				layoutSource = this.FlatLayout;
			}

			if (layoutSource == null)
			{
				return defaultLocation;
			}

			Point location;
			return layoutSource.TryGetValue(currentClient, out location) ? location : defaultLocation;
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
	}
}