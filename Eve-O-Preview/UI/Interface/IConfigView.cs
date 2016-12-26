using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOPreview.UI
{
	public interface IConfigView : IView
	{

		Action<string, string> RenameConfigFile { get; set; }
		Action<string, string> RenameConfigFilename { get; set; }
		Action<string> CopyConfigFile { get; set; }
		Action<string> DeleteConfigFile { get; set; }
		Action UpdateConfigListing { get; set; }
		Action DialogClosed { get; set; }

		Dictionary<string, string> ConfigFiles { set; }

	}
}
