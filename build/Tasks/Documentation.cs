using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.MarkdownToPdf;
using Markdig;

namespace Build.Tasks
{
	public sealed class Documentation : FrostingTask<Context>
	{
		public override void Run(Context context)
		{
			context.Information("Convert README.MD");

			context.MarkdownFileToPdf("readme.md", Configuration.BinFolder + "/readme.pdf", settings =>
			{
				settings.Theme = Themes.Github;
				settings.UseAdvancedMarkdownTables();
				settings.MarkdownPipeline.UseGridTables();
			});
		}
	}
}