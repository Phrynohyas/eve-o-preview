using System;
using System.Linq.Expressions;

namespace EveOPreview
{
	/// <summary>
	/// Generic interface for an Inversion Of Control container
	/// </summary>
	public interface IIocContainer
	{
		void Register<TService, TImplementation>() where TImplementation : TService;
		void Register<TService>();
		void RegisterInstance<T>(T instance);
		TService Resolve<TService>();
		bool IsRegistered<TService>();
		void Register<TService, TArgument>(Expression<Func<TArgument, TService>> factory);
	}
}