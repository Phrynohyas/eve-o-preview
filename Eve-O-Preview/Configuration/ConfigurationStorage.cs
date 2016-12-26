using System.IO;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	class ConfigurationStorage : IConfigurationStorage
	{
		private const string ConfigurationFileName = "config/EVE-O Preview.json";
		private const string AppConfigFileName = "EVE-O Preview App Config.json";

		private readonly IAppConfig _appConfig;
		private readonly IThumbnailConfig _thumbnailConfig;

		public ConfigurationStorage(IAppConfig appConfig, IThumbnailConfig thumbnailConfig)
		{
			this._appConfig = appConfig;
			this._thumbnailConfig = thumbnailConfig;
		}

		public void Load()
		{


			//App config.  load this first so we use the correct config file
			string appFilename = this.GetAppConfigFileName();

			if (!File.Exists(appFilename))
			{
				SaveOnlyAppConfig(); //writes an empty appconfig file
			}

			string appRawData = File.ReadAllText(appFilename);

			JsonConvert.PopulateObject(appRawData, this._appConfig);



			string filename = this.GetConfigFileName();

			if (!File.Exists(filename))
			{
				this._appConfig.ConfigFileName = ConfigurationStorage.ConfigurationFileName;
				filename = ConfigurationStorage.ConfigurationFileName;
				return;
			}

			string rawData = File.ReadAllText(filename);

			JsonConvert.PopulateObject(rawData, this._thumbnailConfig);

			// Validate data after loading it
			this._thumbnailConfig.ApplyRestrictions();


		}

		public void Save()
		{
			//App config
			string appRawData = JsonConvert.SerializeObject(this._appConfig, Formatting.Indented);

			File.WriteAllText(this.GetAppConfigFileName(), appRawData);


			string rawData = JsonConvert.SerializeObject(this._thumbnailConfig, Formatting.Indented);

			File.WriteAllText(this.GetConfigFileName(), rawData);
		}

		public void SaveOnlyAppConfig()
		{
			string appRawData = JsonConvert.SerializeObject(this._appConfig, Formatting.Indented);

			File.WriteAllText(this.GetAppConfigFileName(), appRawData);
		}

		public string GetConfigFileName()
		{
			if (string.IsNullOrEmpty(this._appConfig.ConfigFileName))
			{
				this._appConfig.ConfigFileName = ConfigurationStorage.ConfigurationFileName;
				this.SaveOnlyAppConfig();
				return ConfigurationStorage.ConfigurationFileName;
			}
			else if(File.Exists(this._appConfig.ConfigFileName))
			{
				return this._appConfig.ConfigFileName;
			} else
			{
				this._appConfig.ConfigFileName = ConfigurationStorage.ConfigurationFileName;
				this.SaveOnlyAppConfig();
				return ConfigurationStorage.ConfigurationFileName;
			}
		}

		public string GetAppConfigFileName()
		{
			return ConfigurationStorage.AppConfigFileName;
		}
	}
}