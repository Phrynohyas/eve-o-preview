using System;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.UI;

namespace EveOPreview
{
	static class Program
	{
		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

			// TODO Switch to another container that provides signed assemblies
			IIocContainer container = new LightInjectContainer();

			// UI classes
			IApplicationController controller = new ApplicationController(container)
				.RegisterView<IMainView, MainForm>()
				.RegisterView<IThumbnailView, ThumbnailView>()
				.RegisterView<IThumbnailDescriptionView, ThumbnailDescriptionView>()
				.RegisterInstance(new ApplicationContext());

			// Application services
			controller.RegisterService<IThumbnailManager, ThumbnailManager>()
				.RegisterService<IThumbnailViewFactory, ThumbnailViewFactory>()
				.RegisterService<IThumbnailDescriptionViewFactory, ThumbnailDescriptionViewFactory>()
				.RegisterService<IApplicationConfiguration, ApplicationConfiguration>();

			controller.Run<EveOPreview.UI.MainPresenter>();
		}
	}
}