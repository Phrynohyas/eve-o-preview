using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Presenters;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Thumbnails
{
	sealed class ThumbnailListUpdatedHandler : INotificationHandler<ThumbnailListUpdated>
	{
		#region Private fields
		private readonly IMainFormPresenter _presenter;
		#endregion

		public ThumbnailListUpdatedHandler(MainFormPresenter presenter)
		{
			this._presenter = presenter;
		}

		public Task Handle(ThumbnailListUpdated notification, CancellationToken cancellationToken)
		{
			if (notification.Added.Count > 0)
			{
				this._presenter.AddThumbnails(notification.Added);
			}

			if (notification.Removed.Count > 0)
			{
				this._presenter.RemoveThumbnails(notification.Removed);
			}
			
			return Task.CompletedTask;
		}
	}
}
