using System.IO;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	class ConfigurationStorage : IConfigurationStorage
	{
		private const string ConfigurationFileName = "EVE-O Preview.json";

		private readonly IAppConfig _appConfig;
		private readonly IThumbnailConfiguration _thumbnailConfiguration;

		public ConfigurationStorage(IAppConfig appConfig, IThumbnailConfiguration thumbnailConfiguration)
		{
			this._appConfig = appConfig;
			this._thumbnailConfiguration = thumbnailConfiguration;
		}

		public void Load()
		{
			string filename = this.GetConfigFileName();

			if (!File.Exists(filename))
			{
				return;
			}

			string rawData = File.ReadAllText(filename);

			JsonConvert.PopulateObject(rawData, this._thumbnailConfiguration);

			// Validate data after loading it
			this._thumbnailConfiguration.ApplyRestrictions();
		}

		public void Save()
		{
			string rawData = JsonConvert.SerializeObject(this._thumbnailConfiguration, Formatting.Indented);

			File.WriteAllText(this.GetConfigFileName(), rawData);
		}

		private string GetConfigFileName()
		{
			return string.IsNullOrEmpty(this._appConfig.ConfigFileName) ? ConfigurationStorage.ConfigurationFileName : this._appConfig.ConfigFileName;
		}
	}
}