using System;
using System.Threading;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.Presenters;
using EveOPreview.Services;
using EveOPreview.View;
using MediatR;

namespace EveOPreview
{
	static class Program
	{
		private static string MUTEX_NAME = "EVE-O Preview Single Instance Mutex";

		private static Mutex _singleInstanceMutex;

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main()
		{
			// The very usual Mutex-based single-instance screening
			// 'token' variable is used to store reference to the instance Mutex
			// during the app lifetime
			Program._singleInstanceMutex = Program.GetInstanceToken();

			// If it was not possible to acquire the app token then another app instance is already running
			// Nothing to do here
			if (Program._singleInstanceMutex == null)
			{
				return;
			}

			ExceptionHandler handler = new ExceptionHandler();
			handler.SetupExceptionHandlers();

			IApplicationController controller = Program.InitializeApplicationController();

			Program.InitializeWinForms();
			controller.Run<MainFormPresenter>();
		}

		private static Mutex GetInstanceToken()
		{
			// The code might look overcomplicated here for a single Mutex operation
			// Yet we had already experienced a Windows-level issue
			// where .NET finalizer thread was literally paralyzed by
			// a failed Mutex operation. That did lead to weird OutOfMemory
			// exceptions later
			try
			{
				Mutex.OpenExisting(Program.MUTEX_NAME);
				// if that didn't fail then another instance is already running
				return null;
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
			catch (Exception)
			{
				Mutex token = new Mutex(true, Program.MUTEX_NAME, out var result);
				return result ? token : null;
			}
		}

		private static void InitializeWinForms()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		}

		private static IApplicationController InitializeApplicationController()
		{
			IIocContainer container = new LightInjectContainer();

			// Singleton registration is used for services
			// Low-level services
			container.Register<IWindowManager>();
			container.Register<IProcessMonitor>();

			// MediatR
			container.Register<IMediator, MediatR.Mediator>();
			container.RegisterInstance<ServiceFactory>(t => container.Resolve(t));
			container.Register(typeof(INotificationHandler<>), typeof(Program).Assembly);
			container.Register(typeof(IRequestHandler<>), typeof(Program).Assembly);
			container.Register(typeof(IRequestHandler<,>), typeof(Program).Assembly);

			// Configuration services
			container.Register<IConfigurationStorage>();
			container.Register<IAppConfig>();
			container.Register<IThumbnailConfiguration>();

			// Application services
			container.Register<IThumbnailManager>();
			container.Register<IThumbnailViewFactory>();
			container.Register<IThumbnailDescription>();

			IApplicationController controller = new ApplicationController(container);

			// UI classes
			controller.RegisterView<LiveThumbnailView, LiveThumbnailView>();

			controller.RegisterView<IMainFormView, MainForm>();
			controller.RegisterInstance(new ApplicationContext());

			return controller;
		}
	}
}