using Cake.Frosting;
using Microsoft.Extensions.DependencyInjection;

namespace Build
{
	public class Program : IFrostingStartup
	{
		public static int Main(string[] args)
			=> new CakeHost()
				.UseStartup<Program>()
				.Run(args);

		public void Configure(IServiceCollection services)
		{
			services.UseContext<Context>();
			services.UseLifetime<Lifetime>();

			//move up from build directory and searching for sln or csproj files
			services.UseWorkingDirectory("..");
		}
	}
}