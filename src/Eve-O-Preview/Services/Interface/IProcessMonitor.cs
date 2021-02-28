using System.Collections.Generic;

namespace EveOPreview.Services
{
	public interface IProcessMonitor
	{
		ICollection<IProcessInfo> GetAllProcesses();
		void GetUpdatedProcesses(out ICollection<IProcessInfo> addedProcesses, out ICollection<IProcessInfo> updatedProcesses, out ICollection<IProcessInfo> removedProcesses);
	}
}