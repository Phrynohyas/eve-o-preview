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
			this._thumbnail.Location = new Point(0 + highlightWidthLeft, 0 + highlightWidthTop);
			this._thumbnail.Size = new Size(baseWidth - highlightWidthLeft - highlightWidthRight, baseHeight - highlightWidthTop - highlightWidthBottom);
		}
	}
}