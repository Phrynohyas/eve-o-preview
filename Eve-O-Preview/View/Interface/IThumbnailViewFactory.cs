using System;
using System.Drawing;
using EveOPreview.View;

namespace EveOPreview.UI
{
	public interface IThumbnailViewFactory
	{
		IThumbnailView Create(IntPtr id, string title, Size size);
	}
}