using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.UI;
using MediatR;

namespace EveOPreview.Mediator.Handlers
{
	sealed class StartServicesHandler : INotificationHandler<StartServices>
	{
		private readonly IThumbnailManager _manager;

		public StartServicesHandler(IThumbnailManager manager)
		{
			this._manager = manager;
		}

		public Task Handle(StartServices message, CancellationToken cancellationToken)
		{
			this._manager.Activate();

			return Task.CompletedTask;
		}
	}
}
