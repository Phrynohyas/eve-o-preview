namespace EveOPreview
{
	/// <summary>
	/// Application controller
	/// </summary>
	public interface IApplicationController
	{
		IApplicationController RegisterView<TView, TPresenter>()
					where TPresenter : class, TView
					where TView : IView;

		IApplicationController RegisterInstance<T>(T instance);

		IApplicationController RegisterService<TService, TImplementation>()
					where TImplementation : class, TService;

		void Run<TPresenter>()
					where TPresenter : class, IPresenter;

		void Run<TPresenter, TArgument>(TArgument args)
					where TPresenter : class, IPresenter<TArgument>;

		TService Create<TService>()
					where TService : class;
	}
}