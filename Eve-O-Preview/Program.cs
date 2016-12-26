using System;
using System.Threading;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.UI;

namespace EveOPreview
{
	static class Program
	{
		private static string MutexName = "EVE-O Preview Single Instance Mutex";
		private static string ConfigParameterName = "--config:";

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args)
		{
			// The very usual Mutex-based single-instance screening
			// 'token' variable is used to store reference to the instance Mutex
			// during the app lifetime
			object token = Program.GetInstanceToken();

			// If it was not possible to aquite the app token then another app instance is already running
			// Nothing to do here
			if (token == null)
			{
				return;
			}

			Program.InitializeWinFormsGui();

			IApplicationController controller = Program.InitializeApplicationController();

			Program.SetupApplicationConttroller(controller, Program.GetCustomConfigFile(args));

			controller.Run<MainPresenter>();

			token = null;
		}

		private static string GetCustomConfigFile(string[] args)
		{
			// Parse startup parameters
			// Simple approach is used because something like NParams would be an overkill here
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

		private static object GetInstanceToken()
		{
			// The code might look overcomplicated here for a single Mutex operation
			// Yet we had already experienced a Windows-level issue
			// where .NET finalizer theread was literally paralyzed by
			// a failed Mutex operation. That did lead to weird OutOfMemory
			// exceptions later
			try
			{
				Mutex mutex = Mutex.OpenExisting(Program.MutexName);
				// if that didn't fail then anotherinstance is already running
				return null;
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
			catch (Exception)
			{
				bool result;
				Mutex token = new Mutex(true, Program.MutexName, out result);
				return result ? token : null;
			}
		}

		private static void InitializeWinFormsGui()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		}

		private static IApplicationController InitializeApplicationController()
		{
			IIocContainer container = new LightInjectContainer();

			// UI classes
			IApplicationController controller = new ApplicationController(container)
				.RegisterView<IMainView, MainForm>()
				.RegisterView<IThumbnailView, ThumbnailView>()
				.RegisterView<IThumbnailDescriptionView, ThumbnailDescriptionView>()
				.RegisterView<IConfigView, ConfigForm>()
				.RegisterInstance(new ApplicationContext());

			// Application services
			controller.RegisterService<IThumbnailManager, ThumbnailManager>()
				.RegisterService<IThumbnailViewFactory, ThumbnailViewFactory>()
				.RegisterService<IThumbnailDescriptionViewFactory, ThumbnailDescriptionViewFactory>()
				.RegisterService<IConfigurationStorage, ConfigurationStorage>()
				.RegisterService<IConfigManager, ConfigManager>()
				.RegisterInstance<IAppConfig>(new AppConfig())
				.RegisterInstance<IThumbnailConfig>(new ThumbnailConfig());
			return controller;
		}

		private static void SetupApplicationConttroller(IApplicationController controller, string configFile)
		{
			controller.Create<IAppConfig>().ConfigFileName = configFile;
		}
	}
}