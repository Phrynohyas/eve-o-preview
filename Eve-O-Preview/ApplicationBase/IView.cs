namespace EveOPreview
{
	/// <summary>
	/// Properties and methods that are common for all views
	/// </summary>
	public interface IView
	{
		void Show();
		void Hide();
		void Close();
	}
}