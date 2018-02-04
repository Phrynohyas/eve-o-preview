using System;

namespace EveOPreview.Mediator
{
	/// <summary>
	/// Message dispatcher.
	/// Consider this as a very simple message bus
	/// </summary>
	public interface IMediator
	{
		void Subscribe<T>(Action<T> handler)
			where T : INotification;

		void Unsubscribe<T>(Action<T> handler)
			where T : INotification;

		void Publish<T>(T notification)
			where T : INotification;
	}
}