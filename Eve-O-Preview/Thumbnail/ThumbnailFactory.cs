using System;
using System.Drawing;

namespace EveOPreview
{
	public class ThumbnailFactory
	{
		public IThumbnail Create(MainForm parent, IntPtr sourceWindow, string title, Size size)
		{
			return new ThumbnailWindow(parent, sourceWindow, title, size);
		}
	}
}