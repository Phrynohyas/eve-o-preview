namespace EveOPreview
{
	public interface IPresenter<in TArgument>
	{
		void Run(TArgument args);
	}
}