using Cake.Frosting;

namespace Build.Tasks
{
	[Dependency(typeof(Zip))]
	public sealed class Default : FrostingTask<Context>
	{
	}
}