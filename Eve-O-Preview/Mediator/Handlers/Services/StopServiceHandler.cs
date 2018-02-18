using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.UI;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Services
{
	sealed class StopServiceHandler : IRequestHandler<StopService>
	{
		private readonly IThumbnailManager _manager;

		public StopServiceHandler(IThumbnailManager manager)
		{
			this._manager = manager;
		}

		public Task Handle(StopService message, CancellationToken cancellationToken)
		{
			this._manager.Deactivate();

			return Task.CompletedTask;
		}
	}
}