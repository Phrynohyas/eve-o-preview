using System;
using System.Drawing;
using System.Windows.Forms;
using EveOPreview.Services;

namespace EveOPreview.View
{
	sealed class StaticThumbnailView : ThumbnailView
	{
		#region Private fields
		private readonly PictureBox _thumbnail;
		#endregion

		public StaticThumbnailView(IWindowManager windowManager)
			: base(windowManager)
		{
			this._thumbnail = new StaticThumbnailImage
			{
				TabStop = false,
				SizeMode = PictureBoxSizeMode.StretchImage,
				Location = new Point(0, 0),
				Size = new Size(this.ClientSize.Width, this.ClientSize.Height)
			};
			this.Controls.Add(this._thumbnail);
		}

		protected override void RefreshThumbnail(bool forceRefresh)
		{
			if (!forceRefresh)
			{
				return;
			}

			var thumbnail = this.WindowManager.GetStaticThumbnail(this.Id);
			if (thumbnail != null)
			{
				var oldImage = this._thumbnail.Image;
				this._thumbnail.Image = thumbnail;
				oldImage?.Dispose();
			}
		}

		protected override void ResizeThumbnail(int baseWidth, int baseHeight, int highlightWidthTop, int highlightWidthRight, int highlightWidthBottom, int highlightWidthLeft)
		{
			var left = 0 + highlightWidthLeft;
			var top = 0 + highlightWidthTop;
			if (this.IsLocationUpdateRequired(this._thumbnail.Location, left, top))
			{
				this._thumbnail.Location = new Point(left, top);
			}

			var width = baseWidth - highlightWidthLeft - highlightWidthRight;
			var height = baseHeight - highlightWidthTop - highlightWidthBottom;
			if (this.IsSizeUpdateRequired(this._thumbnail.Size, width, height))
			{
				this._thumbnail.Size = new Size(width, height);
			}
		}

		private bool IsLocationUpdateRequired(Point currentLocation, int left, int top)
		{
			return (currentLocation.X != left) || (currentLocation.Y != top);
		}

		private bool IsSizeUpdateRequired(Size currentSize, int width, int height)
		{
			return (currentSize.Width != width) || (currentSize.Height != height);
		}
	}
}