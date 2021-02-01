using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Net;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

public sealed class Lifetime : FrostingLifetime<Context>
{
	private const string NuGetUrl = @"https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";
	private const string ToolsDirectoryName = "./tools";
	private const string BinDirectoryName = "./bin";
	private const string PublishDirectoryName = "./publish";

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
		if (context.FileExists(ToolsDirectoryName + "/nuget.exe"))
		{
			return;
		}

		if (!context.DirectoryExists(ToolsDirectoryName))
		{
			context.CreateDirectory(ToolsDirectoryName);
		}

		var tempFile = context.DownloadFile(NuGetUrl);
		context.CopyFile(tempFile, new FilePath(ToolsDirectoryName + "/nuget.exe"));
	}

	public override void Setup(Context context)
	{
		context.Information("Setting things up...");

		context.Information("Delete bin and publish folders");
		this.DeleteDirectory(context, BinDirectoryName);
		this.DeleteDirectory(context, PublishDirectoryName);

		context.Information("Download NuGet");
		this.DownloadNuGet(context);

	}

	public override void Teardown(Context context, ITeardownContext info)
	{
		context.Information("Tearing things down...");
		//this.DeleteDirectory(context, ToolsDirectoryName);
	}
}