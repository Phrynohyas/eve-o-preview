using Cake.Core;
using Cake.Frosting;

namespace Build
{
	public class Context : FrostingContext
	{
		public Context(ICakeContext context)
			: base(context)
		{
		}
	}
}