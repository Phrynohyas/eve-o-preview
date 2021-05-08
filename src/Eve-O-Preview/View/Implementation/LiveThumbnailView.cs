using System;
using System.Drawing;
using EveOPreview.Services;

namespace EveOPreview.View
{
	sealed class LiveThumbnailView : ThumbnailView
	{
		#region Private fields
		private IDwmThumbnail _thumbnail;
		private Point _startLocation;
		private Point _endLocation;
		#endregion

		public LiveThumbnailView(IWindowManager windowManager)
			: base(windowManager)
		{
			this._startLocation = new Point(0, 0);
			this._endLocation = new Point(this.ClientSize);
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
			var left = 0 + highlightWidthLeft;
			var top = 0 + highlightWidthTop;
			var right = baseWidth - highlightWidthRight;
			var bottom = baseHeight - highlightWidthBottom;

			if ((this._startLocation.X == left) && (this._startLocation.Y == top) && (this._endLocation.X == right) && (this._endLocation.Y == bottom))
			{
				return; // No update required
			}
			this._startLocation = new Point(left, top);
			this._endLocation = new Point(right, bottom);

			this._thumbnail.Move(left, top, right, bottom);
			this._thumbnail.Update();
		}

		private void RegisterThumbnail()
		{
			this._thumbnail = this.WindowManager.GetLiveThumbnail(this.Handle, this.Id);
			this._thumbnail.Move(this._startLocation.X, this._startLocation.Y, this._endLocation.X, this._endLocation.Y);
			this._thumbnail.Update();
		}
	}
}
