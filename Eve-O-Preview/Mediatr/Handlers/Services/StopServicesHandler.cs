using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.UI;
using MediatR;

namespace EveOPreview.Mediator.Handlers
{
	sealed class StopServicesHandler : INotificationHandler<StopServices>
	{
		private readonly IThumbnailManager _manager;

		public StopServicesHandler(IThumbnailManager manager)
		{
			this._manager = manager;
		}

		public Task Handle(StopServices message, CancellationToken cancellationToken)
		{
			this._manager.Deactivate();

			return Task.CompletedTask;
		}
	}
}