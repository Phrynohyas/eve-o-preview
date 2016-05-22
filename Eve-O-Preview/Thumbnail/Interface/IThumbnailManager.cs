using System;
using System.Collections.Generic;
using System.Drawing;

namespace EveOPreview.Thumbnails
{
	public interface IThumbnailManager
	{
		void Activate();

		void NotifyPreviewSwitch();
		void SetThumbnailState(IntPtr thumbnailId, bool hideAlways);
		void SyncPreviewSize(Size size);
		void UpdatePreviewPosition(string title, Point position);
		void RefreshThumbnails();
		void SetupThumbnailFrames();

		event Action<IList<IThumbnail>> ThumbnailsAdded;
		event Action<IList<IThumbnail>> ThumbnailsUpdated;
		event Action<IList<IThumbnail>> ThumbnailsRemoved;
		event Action<Size> ThumbnailSizeChanged;
	}
}