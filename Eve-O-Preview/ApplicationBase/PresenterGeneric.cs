namespace EveOPreview
{
	public abstract class Presenter<TView, TArgument> : IPresenter<TArgument>
				where TView : IView
	{
		// Properties are used instead of fields so the code remains CLS compliant
		// 'protected readonly' fields would result in non-CLS compliant code
		protected TView View { get; private set; }
		protected IApplicationController Controller { get; private set; }

		protected Presenter(IApplicationController controller, TView view)
		{
			this.Controller = controller;
			this.View = view;
		}

		public abstract void Run(TArgument args);
	}
}