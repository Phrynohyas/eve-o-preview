using System;

namespace EveOPreview.Services
{
	public interface IProcessInfo
	{
		IntPtr Handle { get; }
		string Title { get; }
	}
}