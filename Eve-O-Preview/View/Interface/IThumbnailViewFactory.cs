using System;
using System.Drawing;

namespace EveOPreview.View
{
	public interface IThumbnailViewFactory
	{
		IThumbnailView Create(IntPtr id, string title, Size size);
	}
}