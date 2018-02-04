using System;
using System.Linq.Expressions;
using LightInject;

namespace EveOPreview
{
	// Adapts LighInject to the generic IoC interface
	sealed class LightInjectContainer : IIocContainer
	{
		private readonly ServiceContainer _container;

		public LightInjectContainer()
		{
			this._container = new ServiceContainer(ContainerOptions.Default);
		}

		public bool IsRegistered<TService>()
		{
			return this._container.CanGetInstance(typeof(TService), "");
		}

		public void Register<TService>()
		{
			if (!typeof(TService).IsInterface)
			{
				this._container.Register<TService>(new PerContainerLifetime());
				return;
			}

			foreach (Type implementationType in typeof(TService).Assembly.DefinedTypes)
			{
				if (!implementationType.IsClass || implementationType.IsAbstract)
				{
					continue;
				}

				if (!typeof(TService).IsAssignableFrom(implementationType))
				{
					continue;
				}

				this._container.Register(typeof(TService), implementationType, new PerContainerLifetime());
				break;
			}


		}

		public void Register<TService, TImplementation>()
					where TImplementation : TService
		{
			this._container.Register<TService, TImplementation>(new PerContainerLifetime());
		}

		public void Register<TService, TArgument>(Expression<Func<TArgument, TService>> factory)
		{
			this._container.Register(f => factory);
		}

		public void RegisterInstance<T>(T instance)
		{
			this._container.RegisterInstance(instance);
		}

		public TService Resolve<TService>()
		{
			return this._container.GetInstance<TService>();
		}
	}
}