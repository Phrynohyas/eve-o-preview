using System;
using System.Drawing;

namespace EveOPreview.Thumbnails
{
	public interface IThumbnailFactory
	{
		IThumbnail Create(IThumbnailManager manager, IntPtr sourceWindow, string title, Size size);
	}
}