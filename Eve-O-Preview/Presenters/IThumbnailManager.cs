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
		void SetupThumbnailFrames();
	}
}