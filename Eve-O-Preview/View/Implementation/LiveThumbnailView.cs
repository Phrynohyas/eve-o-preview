using System;
using EveOPreview.Services;

namespace EveOPreview.View
{
	sealed class LiveThumbnailView : ThumbnailView
	{
		#region Private fields
		private IDwmThumbnail _thumbnail;
		#endregion

		public LiveThumbnailView(IWindowManager windowManager)
			: base(windowManager)
		{
		}

		public override void Close()
		{
			this._thumbnail?.Unregister();
			base.Close();
		}

		protected override void RefreshThumbnail(bool forceRefresh)
		{
			// To prevent flickering the old broken thumbnail is removed AFTER the new shiny one is created
			IDwmThumbnail obsoleteThumbnail = forceRefresh ? this._thumbnail : null;

			if ((this._thumbnail == null) || forceRefresh)
			{
				this.RegisterThumbnail();
			}

			obsoleteThumbnail?.Unregister();
		}

		protected override void ResizeThumbnail(int baseWidth, int baseHeight, int highlightWidthTop, int highlightWidthRight, int highlightWidthBottom, int highlightWidthLeft)
		{
			this._thumbnail.Move(0 + highlightWidthLeft, 0 + highlightWidthTop, baseWidth - highlightWidthRight, baseHeight - highlightWidthBottom);
			this._thumbnail.Update();
		}

		private void RegisterThumbnail()
		{
			this._thumbnail = this.WindowManager.GetLiveThumbnail(this.Handle, this.Id);
			this._thumbnail.Update();
		}
	}
}
