using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Presenters;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Thumbnails
{
	sealed class ThumbnailSizeUpdatedHandler : INotificationHandler<ThumbnailSizeUpdated>
	{
		private readonly IMainFormPresenter _presenter;

		public ThumbnailSizeUpdatedHandler(MainFormPresenter presenter)
		{
			this._presenter = presenter;
		}

		public Task Handle(ThumbnailSizeUpdated notification, CancellationToken cancellationToken)
		{
			this._presenter.UpdateThumbnailSize(notification.Value);

			return Task.CompletedTask;
		}
	}
}
