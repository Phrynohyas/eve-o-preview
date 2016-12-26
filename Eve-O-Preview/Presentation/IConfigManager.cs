using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOPreview.UI
{
	public interface IConfigManager
	{
		void Activate();
		void Deactivate();

		Dictionary<string, string> ConfigFiles { get; set; }

		void ScanForConfigFiles();
		void LaunchConfigDialog();
		Action UpdateMainConfigListing { get; set; }
		Action ReloadSettings { get; set; }
		Action<string> SetCurrentConfig { get; set; }
	}
}
