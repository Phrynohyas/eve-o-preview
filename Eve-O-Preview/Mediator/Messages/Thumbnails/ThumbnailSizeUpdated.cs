using System.Drawing;

namespace EveOPreview.Mediator.Messages
{
	sealed class ThumbnailSizeUpdated : NotificationBase<Size>
	{
		public ThumbnailSizeUpdated(Size size)
				: base(size)
		{
		}
	}
}
