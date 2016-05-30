using System;

namespace EveOPreview.UI
{
	public interface IThumbnailDescriptionViewFactory
	{
		IThumbnailDescriptionView Create(IntPtr id, string title, bool isDisabled);
	}
}