using System;
using System.IO;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	class ConfigurationStorage : IConfigurationStorage
	{
		private const string ConfigurationFileName = "EVE-O Preview.json";

		private readonly IAppConfig _appConfig;
		private readonly IThumbnailConfig _thumbnailConfig;

		public ConfigurationStorage(IAppConfig appConfig, IThumbnailConfig thumbnailConfig)
		{
			this._appConfig = appConfig;
			this._thumbnailConfig = thumbnailConfig;
		}

		public void Load()
		{
			string filename = this.GetConfigFileName();

			if (!File.Exists(filename))
			{
				return;
			}

			string rawData = File.ReadAllText(filename);

			JsonConvert.PopulateObject(rawData, this._thumbnailConfig);

			// Validate data after loading it
			this._thumbnailConfig.ApplyRestrictions();
		}

		public void Save()
		{
			string rawData = JsonConvert.SerializeObject(this._thumbnailConfig, Formatting.Indented);
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
			return string.IsNullOrEmpty(this._appConfig.ConfigFileName) ? ConfigurationStorage.ConfigurationFileName : this._appConfig.ConfigFileName;
		}
	}
}