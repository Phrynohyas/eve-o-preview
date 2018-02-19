using System;
using System.Drawing;

namespace EveOPreview.Services
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