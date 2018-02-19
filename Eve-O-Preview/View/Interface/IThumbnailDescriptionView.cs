using System;

namespace EveOPreview.View
{
	public interface IThumbnailDescriptionView : IView
	{
		IntPtr Id { get; set; }
		string Title { get; set; }
		bool IsDisabled { get; set; }
	}
}