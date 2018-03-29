using MediatR;

namespace EveOPreview.Mediator.Messages
{
	abstract class NotificationBase<TValue> : INotification
	{
		protected NotificationBase(TValue value)
		{
			this.Value = value;
		}

		public TValue Value { get; }
	}
}