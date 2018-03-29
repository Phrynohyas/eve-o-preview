using System.Drawing;

namespace EveOPreview.Mediator.Messages
{
	sealed class ThumbnailActiveSizeUpdated : NotificationBase<Size>
	{
		public ThumbnailActiveSizeUpdated(Size size)
				: base(size)
		{
		}
	}
}