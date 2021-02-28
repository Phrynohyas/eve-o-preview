using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Net;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
	public sealed class Lifetime : FrostingLifetime<Context>
	{
		private const string NuGetUrl = @"https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";

		private void DeleteDirectory(Context context, string directoryName)
		{
			if (!context.DirectoryExists(directoryName))
			{
				return;
			}

			context.DeleteDirectory(directoryName, new DeleteDirectorySettings { Force = true, Recursive = true });
		}

		private void DownloadNuGet(Context context)
		{
			if (context.FileExists(Configuration.ToolsFolder + "/nuget.exe"))
			{
				return;
			}

			if (!context.DirectoryExists(Configuration.ToolsFolder))
			{
				context.CreateDirectory(Configuration.ToolsFolder);
			}

			var tempFile = context.DownloadFile(NuGetUrl);
			context.CopyFile(tempFile, new FilePath(Configuration.ToolsFolder + "/nuget.exe"));
		}

		public override void Setup(Context context)
		{
			context.Information("Setting things up...");

			context.Information("Delete bin and publish folders");
			this.DeleteDirectory(context, Configuration.BinFolder);
			this.DeleteDirectory(context, Configuration.PublishFolder);

			context.Information("Download NuGet");
			this.DownloadNuGet(context);

		}

		public override void Teardown(Context context, ITeardownContext info)
		{
			context.Information("Tearing things down...");
			//this.DeleteDirectory(context, ToolsDirectoryName);
		}
	}
}