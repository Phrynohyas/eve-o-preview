using System;
using System.Drawing;

namespace EveOPreview
{
	public class ThumbnailFactory
	{
		public IThumbnail Create(ThumbnailManager manager, IntPtr sourceWindow, string title, Size size)
		{
			return new ThumbnailWindow(manager, sourceWindow, title, size);
		}
	}
}