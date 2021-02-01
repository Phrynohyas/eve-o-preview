using Cake.Common.Diagnostics;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Frosting;

namespace Build.Tasks
{
	[Dependency(typeof(Documentation))]
	public sealed class Restore : FrostingTask<Context>
	{
		public override void Run(Context context)
		{
			context.Information("Restore started...");
			context.NuGetRestore(Configuration.SolutionName, new NuGetRestoreSettings { NoCache = true });
		}
	}
}
