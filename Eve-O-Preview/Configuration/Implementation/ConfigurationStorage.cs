using System.IO;
using Newtonsoft.Json;

namespace EveOPreview.Configuration.Implementation
{
	class ConfigurationStorage : IConfigurationStorage
	{
		private const string CONFIGURATION_FILE_NAME = "EVE-O Preview.json";

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
			string filename = this.GetConfigFileName();

			try
			{
				File.WriteAllText(filename, rawData);
			}
			catch (IOException)
			{
				// Ignore error if for some reason the updated config cannot be written down
			}
		}

		private string GetConfigFileName()
		{
			return string.IsNullOrEmpty(this._appConfig.ConfigFileName) ? ConfigurationStorage.CONFIGURATION_FILE_NAME : this._appConfig.ConfigFileName;
		}
	}
}