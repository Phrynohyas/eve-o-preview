using System;
using System.Drawing;

namespace EveOPreview.Services
{
	public interface IThumbnailManager
	{
		void Start();
		void Stop();

		void SetThumbnailState(IntPtr thumbnailId, bool hideAlways);
		void SetThumbnailsSize(Size size);
		void SetupThumbnailFrames();
	}
}