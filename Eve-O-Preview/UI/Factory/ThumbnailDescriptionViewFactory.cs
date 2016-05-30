using System;

namespace EveOPreview.UI
{
	public class ThumbnailDescriptionViewFactory : IThumbnailDescriptionViewFactory
	{
		private readonly IApplicationController _controller;

		public ThumbnailDescriptionViewFactory(IApplicationController controller)
		{
			this._controller = controller;
		}

		public IThumbnailDescriptionView Create(IntPtr id, string title, bool isDisabled)
		{
			IThumbnailDescriptionView view = this._controller.Create<IThumbnailDescriptionView>();

			view.Id = id;
			view.Title = title;
			view.IsDisabled = isDisabled;

			return view;
		}
	}
}