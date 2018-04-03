namespace EveOPreview.Configuration
{
	public interface IConfigurationStorage
	{
		void Load();
		void Save();
	}
}