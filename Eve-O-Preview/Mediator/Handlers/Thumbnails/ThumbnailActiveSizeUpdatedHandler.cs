using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Presenters;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Thumbnails
{
	sealed class ThumbnailActiveSizeUpdatedHandler : INotificationHandler<ThumbnailActiveSizeUpdated>
	{
		private readonly IMainFormPresenter _presenter;

		public ThumbnailActiveSizeUpdatedHandler(MainFormPresenter presenter)
		{
			this._presenter = presenter;
		}

		public Task Handle(ThumbnailActiveSizeUpdated notification, CancellationToken cancellationToken)
		{
			this._presenter.UpdateThumbnailSize(notification.Value);

			return Task.CompletedTask;
		}
	}
}