using System.Drawing;
using MediatR;

namespace EveOPreview.Mediator.Messages
{
	sealed class ThumbnailLocationUpdated : INotification
	{
		public ThumbnailLocationUpdated(string thumbnailName, string activeClientName, Point location)
		{
			this.ThumbnailName = thumbnailName;
			this.ActiveClientName = activeClientName;
			this.Location = location;
		}

		public string ThumbnailName { get; }

		public string ActiveClientName { get; }

		public Point Location { get; }
	}
}