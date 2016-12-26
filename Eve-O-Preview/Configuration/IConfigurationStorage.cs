namespace EveOPreview.Configuration
{
	public interface IConfigurationStorage
	{
		void Load();
		void Save();
		void SaveOnlyAppConfig();
		string GetConfigFileName();
		string GetAppConfigFileName();
	}
}