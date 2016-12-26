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

			this._configurationStorage.Save(); // saves the current config

			Dictionary<string, string> configs = new Dictionary<string, string>();


			string[] files = Directory.GetFiles("config");
			foreach (string file in files)
			{

				IThumbnailConfig config = new ThumbnailConfig();

				if (!File.Exists(file))
				{
					continue;
				}

				string rawData = File.ReadAllText(file);

				JsonConvert.PopulateObject(rawData, config);

				// Validate data after loading it
				config.ApplyRestrictions();
				configs.Add(file.Replace("\\", "/"), config.Name);
			}

			if (configs != null && configs.Count > 0)
				this.ConfigFiles = configs;


			return;
		}

		public void LaunchConfigDialog()
		{
			if (this._activeConfigView == null)
			{

				IConfigView configView = new ConfigForm(this.ConfigFiles);
				configView.RenameConfigFile = this.RenameConfigFile;
				configView.CopyConfigFile = this.CopyConfigFile;
				configView.DeleteConfigFile = this.DeleteConfigFile;
				configView.UpdateConfigListing = this.UpdateConfigListing;

				this._activeConfigView = configView;
			}
			this._activeConfigView.Show();
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

		private void CopyConfigFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			string newName = fileName + " - Copy";

			int copy = 1;
			while (File.Exists(newName))
			{
				copy++;
				newName = fileName + " - Copy(" + copy + ")";
			}

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

			this.UpdateConfigListing();
		}
	}
}
