using System;
using System.Drawing;
using EveOPreview.Configuration;

namespace EveOPreview.View
{
	sealed class ThumbnailViewFactory : IThumbnailViewFactory
	{
		private readonly IApplicationController _controller;
		private readonly bool _enableWineCompatibilityMode;

		public ThumbnailViewFactory(IApplicationController controller, IThumbnailConfiguration configuration)
		{
			this._controller = controller;
			this._enableWineCompatibilityMode = configuration.EnableWineCompatibilityMode;
		}

		public IThumbnailView Create(IntPtr id, string title, Size size)
		{
			IThumbnailView view = this._enableWineCompatibilityMode
				? (IThumbnailView)this._controller.Create<StaticThumbnailView>()
				: (IThumbnailView)this._controller.Create<LiveThumbnailView>();

			view.Id = id;
			view.Title = title;
			view.ThumbnailSize = size;

			return view;
		}
	}
}