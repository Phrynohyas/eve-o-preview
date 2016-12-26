using EveOPreview.Configuration;
using EveOPreview.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOPreview.UI
{
	public class ConfigManager : IConfigManager
	{
		private IConfigurationStorage _configurationStorage;
		private IConfigView _activeConfigView;

		public ConfigManager(IConfigurationStorage configStorage)
		{
			this._configurationStorage = configStorage;
			this.ConfigFiles = new Dictionary<string, string>();
		}

		public Dictionary<string, string> ConfigFiles { get; set; }

		public void Activate()
		{
			this.ScanForConfigFiles();
		}

		public void Deactivate()
		{

		}

		public Action UpdateMainConfigListing { get; set; }

		public Action ReloadSettings { get; set; }

		public Action<string> SetCurrentConfig { get; set; }

		public void UpdateConfigListing()
		{
			this.UpdateMainConfigListing?.Invoke();
			if(this._activeConfigView != null)
			{
				this._activeConfigView.ConfigFiles = this.ConfigFiles;
			}
		}

		public void ScanForConfigFiles()
		{
			if(File.Exists(this._configurationStorage.GetConfigFileName()))
				this._configurationStorage.Save(); // saves the current config

			Dictionary<string, string> configs = new Dictionary<string, string>();


			string[] files = Directory.GetFiles("config");
			foreach (string file in files)
			{

				if (!File.Exists(file))
				{
					continue;
				}

				IThumbnailConfig config = new ThumbnailConfig();

				string rawData = File.ReadAllText(file);

				JsonConvert.PopulateObject(rawData, config);

				// Validate data after loading it
				config.ApplyRestrictions();
				configs.Add(file.Replace("\\", "/"), config.Name);
			}

			if (configs != null && configs.Count > 0)
				this.ConfigFiles = configs;

			if (!configs.ContainsKey(this._configurationStorage.GetConfigFileName()))
			{
				this.SetCurrentConfig?.Invoke(configs.Keys.ElementAt(0));
			}

			this.UpdateConfigListing();
		}

		public void LaunchConfigDialog()
		{
			if (this._activeConfigView == null)
			{

				IConfigView configView = new ConfigForm(this.ConfigFiles);
				configView.RenameConfigFile = this.RenameConfigFile;
				configView.RenameConfigFilename = this.RenameConfigFilename;
				configView.CopyConfigFile = this.CopyConfigFile;
				configView.DeleteConfigFile = this.DeleteConfigFile;
				configView.UpdateConfigListing = this.UpdateConfigListing;
				configView.DialogClosed = this.ConfigDialogClosed;

				this._activeConfigView = configView;
			}
			this._activeConfigView.Show();
		}

		private void ConfigDialogClosed()
		{
			this._activeConfigView = null;
		}

		private void RenameConfigFile(string fileName, string newName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			string rawData = File.ReadAllText(fileName);

			IThumbnailConfig config = new ThumbnailConfig();

			JsonConvert.PopulateObject(rawData, config);

			config.Name = newName;

			rawData = JsonConvert.SerializeObject(config, Formatting.Indented);

			File.WriteAllText(fileName, rawData);

			this._configurationStorage.Load();

			if (this.ConfigFiles.ContainsKey(fileName))
			{
				this.ConfigFiles.Remove(fileName);
			}

			this.ConfigFiles.Add(fileName, newName);

			this.UpdateConfigListing();
		}

		private void RenameConfigFilename(string fileName, string newName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			File.Move(fileName, newName);

			//updating appConfig

			IAppConfig appConfig = new AppConfig();
			string appFilename = this._configurationStorage.GetAppConfigFileName();

			string appRawData = File.ReadAllText(appFilename);

			JsonConvert.PopulateObject(appRawData, appConfig);

			if (appConfig.ConfigFileName == fileName)
			{
				appConfig.ConfigFileName = newName;
			}

			string rawData = JsonConvert.SerializeObject(appConfig, Formatting.Indented);

			File.WriteAllText(appFilename, rawData);

			this._configurationStorage.Load();


			string configName = "";

			this.ConfigFiles.TryGetValue(fileName, out configName);

			if (this.ConfigFiles.ContainsKey(fileName))
			{
				this.ConfigFiles.Remove(fileName);
			}

			this.ConfigFiles.Add(newName, configName);

			this.UpdateConfigListing();
			this.ReloadSettings?.Invoke();
		}

		private void CopyConfigFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			string newName = fileName.Substring(0, fileName.Length-5) + " - Copy";

			int copy = 1;
			while (File.Exists(newName+".json"))
			{
				copy++;
				newName = fileName + " - Copy(" + copy + ")";
			}
			newName += ".json";

			File.Copy(fileName, newName);

			string oldName = "";
			this.ConfigFiles.TryGetValue(fileName, out oldName);

			this.RenameConfigFile(newName, oldName + " - Copy");

			this.UpdateConfigListing();

		}

		private void DeleteConfigFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			
			this.ConfigFiles.Remove(fileName);

			File.Delete(fileName);

			if (this.ConfigFiles.Count == 0)
			{
				IConfigurationStorage configStorage = new ConfigurationStorage(new AppConfig(), new ThumbnailConfig());
				configStorage.Save();
				this.ConfigFiles.Add(configStorage.GetConfigFileName(), "Default");

				this._configurationStorage.Load();
			}

			this.ScanForConfigFiles();
			this.UpdateConfigListing();

			this._configurationStorage.Load();
		}
	}
}
