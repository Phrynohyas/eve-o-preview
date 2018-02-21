using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Configuration;
using EveOPreview.Mediator.Messages;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Configuration
{
	sealed class SaveConfigurationHandler : IRequestHandler<SaveConfiguration>
	{
		private readonly IConfigurationStorage _storage;

		public SaveConfigurationHandler(IConfigurationStorage storage)
		{
			this._storage = storage;
		}

		public Task Handle(SaveConfiguration message, CancellationToken cancellationToken)
		{
			this._storage.Save();

			return Task.CompletedTask;
		}
	}
}