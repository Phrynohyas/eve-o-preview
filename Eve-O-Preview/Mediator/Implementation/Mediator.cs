using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace EveOPreview.Mediator.Implementation
{
	class Mediator : IMediator
	{
		#region Private fields
		private readonly ReaderWriterLockSlim _lock;
		private readonly IDictionary<Type, IList> _handlers;
		#endregion

		public Mediator()
		{
			this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			this._handlers = new Dictionary<Type, IList>();
		}

		public void Subscribe<T>(Action<T> handler)
			where T : INotification
		{
			this._lock.EnterWriteLock();
			try
			{
				IList handlers;
				if (!this._handlers.TryGetValue(typeof(T), out handlers))
				{
					handlers = new List<Action<T>>();
					this._handlers.Add(typeof(T), handlers);
				}

				handlers.Add(handler);
			}
			finally
			{
				this._lock.ExitWriteLock();
			}
		}

		public void Unsubscribe<T>(Action<T> handler)
			where T : INotification
		{
			this._lock.EnterWriteLock();
			try
			{
				this._handlers.TryGetValue(typeof(T), out var handlers);
				handlers?.Remove(handler);
			}
			finally
			{
				this._lock.ExitWriteLock();
			}
		}

		public void Publish<T>(T notification)
			where T : INotification
		{
			// Empty notifications are silently swallowed
			if (notification.IsEmpty())
			{
				return;
			}

			IList<Action<T>> handlers;
			this._lock.EnterReadLock();
			try
			{
				if (!this._handlers.TryGetValue(typeof(T), out var untypedHandlers))
				{
					return;
				}

				// Clone the list to minimize lock time
				// and possible deadlock issues (f.e. one of subscribers could raise an event ar add/remove subsctibers etc)
				handlers = new List<Action<T>>((IList<Action<T>>)untypedHandlers);
			}
			finally
			{
				this._lock.ExitReadLock();
			}

			foreach (var handler in handlers)
			{
				handler.Invoke(notification);
			}
		}
	}
}