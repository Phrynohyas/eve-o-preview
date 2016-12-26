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

		void UpdateConfigListing();
		void LaunchConfigDialog();
		Action UpdateMainConfigListing { get; set; }
		Action LoadSettings { get; set; }
		Action SaveSettings { get; set; }
		Action<string> SetCurrentConfig { get; set; }
	}
}
