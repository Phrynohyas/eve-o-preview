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
		void SetThumbnailsSize(Size size);
		void RefreshThumbnails();
		void SetupThumbnailFrames();

		event Action<IList<IThumbnailView>> ThumbnailsAdded;
		event Action<IList<IThumbnailView>> ThumbnailsUpdated;
		event Action<IList<IThumbnailView>> ThumbnailsRemoved;
		event Action<Size> ThumbnailSizeChanged;
	}
}