using System;

namespace EveOPreview.UI
{
	public interface IThumbnailDescriptionView : IView
	{
		IntPtr Id { get; set; }
		string Title { get; set; }
	}
}