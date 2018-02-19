using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Services;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Services
{
	sealed class StartServiceHandler : IRequestHandler<StartService>
	{
		private readonly IThumbnailManager _manager;

		public StartServiceHandler(IThumbnailManager manager)
		{
			this._manager = manager;
		}

		public Task Handle(StartService message, CancellationToken cancellationToken)
		{
			this._manager.Start();

			return Task.CompletedTask;
		}
	}
}
