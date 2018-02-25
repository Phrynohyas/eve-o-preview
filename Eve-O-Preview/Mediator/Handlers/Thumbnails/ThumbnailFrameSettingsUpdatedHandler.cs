using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Services;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Thumbnails
{
	sealed class ThumbnailFrameSettingsUpdatedHandler : INotificationHandler<ThumbnailFrameSettingsUpdated>
	{
		private readonly IThumbnailManager _manager;

		public ThumbnailFrameSettingsUpdatedHandler(IThumbnailManager manager)
		{
			this._manager = manager;
		}

		public Task Handle(ThumbnailFrameSettingsUpdated notification, CancellationToken cancellationToken)
		{
			this._manager.UpdateThumbnailFrames();

			return Task.CompletedTask;
		}
	}
}