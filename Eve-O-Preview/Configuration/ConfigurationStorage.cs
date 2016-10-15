using System.IO;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	public class ConfigurationStorage : IConfigurationStorage
	{
		private const string ConfigurationFileName = "EVE-O Preview.json";

		private readonly IThumbnailConfig _configuration;

		public ConfigurationStorage(IThumbnailConfig configuration)
		{
			this._configuration = configuration;
		}

		public void Load()
		{
			string filename = this.GetConfigFileName();

			if (!File.Exists(filename))
			{
				return;
			}

			string rawData = File.ReadAllText(filename);

			JsonConvert.PopulateObject(rawData, this._configuration);

			// Validate data after loading it
			this._configuration.ApplyRestrictions();
		}

		public void Save()
		{
			string rawData = JsonConvert.SerializeObject(this._configuration, Formatting.Indented);

			File.WriteAllText(this.GetConfigFileName(), rawData);
		}

		private string GetConfigFileName()
		{
			return string.IsNullOrEmpty(this._configuration.ConfigFileName) ? ConfigurationStorage.ConfigurationFileName : this._configuration.ConfigFileName;
		}
	}
}