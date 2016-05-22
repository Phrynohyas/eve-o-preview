using System;
using System.Windows.Forms;
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
				.RegisterView<IThumbnailDescriptionView, ThumbnailDescriptionView>()
				.RegisterInstance(new ApplicationContext());

			// Application services
			controller.RegisterService<EveOPreview.Thumbnails.IThumbnailFactory, EveOPreview.Thumbnails.ThumbnailFactory>()
				.RegisterService<EveOPreview.Thumbnails.IThumbnailManager, EveOPreview.Thumbnails.ThumbnailManager>()
				.RegisterService<IThumbnailDescriptionViewFactory, ThumbnailDescriptionViewFactory>();

			controller.Run<EveOPreview.UI.MainPresenter>();
		}
	}
}