﻿using System;
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

		double ThumbnailOpacity { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		bool EnablePerClientThumbnailLayouts { get; set; }

		Size ThumbnailSize { get; set; }

		bool EnableThumbnailZoom { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ViewZoomAnchor ThumbnailZoomAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowThumbnailFrames { get; set; }

		bool EnableActiveClientHighlight { get; set; }
		Color ActiveClientHighlightColor { get; set; }

		Dictionary<string, string> ConfigFiles { get; set; }

		string CurrentConfigFile { get; set; }
		string CurrentConfigName { get; set; }

		void SetForumUrl(string url);
		void SetThumbnailSizeLimitations(Size minimumSize, Size maximumSize);

		void Minimize();

		void AddThumbnails(IList<IThumbnailDescriptionView> thumbnails);
		void UpdateThumbnails(IList<IThumbnailDescriptionView> thumbnails);
		void RemoveThumbnails(IList<IThumbnailDescriptionView> thumbnails);
		void RefreshZoomSettings();

		Action ApplicationExitRequested { get; set; }
		Action FormActivated { get; set; }
		Action FormMinimized { get; set; }
		Action<ViewCloseRequest> FormCloseRequested { get; set; }
		Action ApplicationSettingsChanged { get; set; }
		Action ThumbnailsSizeChanged { get; set; }
		Action<IntPtr> ThumbnailStateChanged { get; set; }
		Action ForumUrlLinkActivated { get; set; }
		Action ConfigFileChanged { get; set; }
		Action ScanForConfigFiles { get; set; }
		Action LaunchConfigDialog { get; set; }
	}
}