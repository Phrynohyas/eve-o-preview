using System;
using System.Collections.Generic;
using System.Drawing;

namespace EveOPreview.UI
{
	public interface IThumbnailManager
	{
		void Activate();
		void Deactivate();

		void SetThumbnailState(IntPtr thumbnailId, bool hideAlways);
		void SetThumbnailsSize(IntPtr id, Size size);
		void SetupThumbnailFrames();

		void SetThumbnailResizeLock(bool locked);
		void SetThumbnailPositionLock(bool locked);

		Action<IList<IThumbnailView>> ThumbnailsAdded { get; set; }
		Action<IList<IThumbnailView>> ThumbnailsUpdated { get; set; }
		Action<IList<IThumbnailView>> ThumbnailsRemoved { get; set; }
		Action<Size> ThumbnailSizeChanged { get; set; }
	}
}