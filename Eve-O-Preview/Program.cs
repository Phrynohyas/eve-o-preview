using System;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.UI;

namespace EveOPreview
{
	static class Program
	{
		private static string ConfigParameterName = "--config:";

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

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
				.RegisterService<IConfigurationStorage, ConfigurationStorage>()
				.RegisterInstance<IThumbnailConfig>(new ThumbnailConfig());

			controller.Create<IThumbnailConfig>().ConfigFileName = Program.GetCustomConfigFile(args);

			controller.Run<MainPresenter>();
		}

		// Parse startup parameters
		// Simple approach is used because something like NParams would be an overkill here
		private static string GetCustomConfigFile(string[] args)
		{
			string configFile = null;
			foreach (string arg in args)
			{
				if ((arg.Length <= Program.ConfigParameterName.Length) || !arg.StartsWith(Program.ConfigParameterName, StringComparison.Ordinal))
				{
					continue;
				}

				configFile = arg.Substring(Program.ConfigParameterName.Length);
				break;
			}

			if (string.IsNullOrEmpty(configFile))
			{
				return "";
			}

			// One more check to drop trailing "
			if ((configFile.Length > 3) && (configFile[0] == '"') && (configFile[configFile.Length - 1] == '"'))
			{
				configFile = configFile.Substring(1, configFile.Length - 2);
			}

			return configFile;
		}
	}
}