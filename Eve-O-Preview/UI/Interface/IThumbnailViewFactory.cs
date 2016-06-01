using System;
using System.Drawing;

namespace EveOPreview.UI
{
	public interface IThumbnailViewFactory
	{
		IThumbnailView Create(IntPtr id, string title, Size size);
	}
}