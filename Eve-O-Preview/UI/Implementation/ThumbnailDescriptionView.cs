using System;

namespace EveOPreview.UI
{
	public class ThumbnailDescriptionView : IThumbnailDescriptionView
	{
		public IntPtr Id { get; set; }

		public string Title { get; set; }

		public bool IsDisabled { get; set; }

		public void Show()
		{
		}

		public void Hide()
		{
		}

		public void Close()
		{
		}
	}
}