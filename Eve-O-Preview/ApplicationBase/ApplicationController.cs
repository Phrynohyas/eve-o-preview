namespace EveOPreview
{
	public class ApplicationController : IApplicationController
	{
		private readonly IIocContainer _container;

		public ApplicationController(IIocContainer container)
		{
			this._container = container;
			this._container.RegisterInstance<IApplicationController>(this);
		}

		public IApplicationController RegisterView<TView, TImplementation>()
			where TView : IView
			where TImplementation : class, TView
		{
			this._container.Register<TView, TImplementation>();
			return this;
		}

		public IApplicationController RegisterInstance<TArgument>(TArgument instance)
		{
			this._container.RegisterInstance(instance);
			return this;
		}

		public IApplicationController RegisterService<TService, TImplementation>()
			where TImplementation : class, TService
		{
			this._container.Register<TService, TImplementation>();
			return this;
		}

		public void Run<TPresenter>()
			where TPresenter : class, IPresenter
		{
			if (!this._container.IsRegistered<TPresenter>())
			{
				this._container.Register<TPresenter>();
			}

			TPresenter presenter = this._container.Resolve<TPresenter>();
			presenter.Run();
		}

		public void Run<TPresenter, TParameter>(TParameter args)
			where TPresenter : class, IPresenter<TParameter>
		{
			if (!this._container.IsRegistered<TPresenter>())
			{
				this._container.Register<TPresenter>();
			}

			TPresenter presenter = this._container.Resolve<TPresenter>();
			presenter.Run(args);
		}

		public TService Create<TService>()
			where TService : class
		{
			if (!this._container.IsRegistered<TService>())
			{
				this._container.Register<TService>();
			}

			return this._container.Resolve<TService>();
		}
	}
}