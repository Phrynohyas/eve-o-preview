using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EveOPreview
{
	/// <summary>
	/// Generic interface for an Inversion Of Control container
	/// </summary>
	public interface IIocContainer
	{
		void Register<TService, TImplementation>()
			where TImplementation : TService;
		void Register(Type serviceType, Assembly container);
		void Register<TService>();
		void Register<TService>(Expression<Func<TService>> factory);
		void Register<TService, TArgument>(Expression<Func<TArgument, TService>> factory);
		void RegisterInstance<TService>(TService instance);
		TService Resolve<TService>();
		IEnumerable<TService> ResolveAll<TService>();
		object Resolve(Type serviceType);
		IEnumerable<object> ResolveAll(Type serviceType);
		bool IsRegistered<TService>();
	}
}