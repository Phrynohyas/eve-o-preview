using System;
using System.Drawing;

namespace EveOPreview.Thumbnails
{
	public class ThumbnailFactory : IThumbnailFactory
	{
		public IThumbnail Create(IThumbnailManager manager, IntPtr sourceWindow, string title, Size size)
		{
			return new ThumbnailWindow(manager, sourceWindow, title, size);
		}
	}
}