using System;

namespace EveOPreview.Services.Implementation
{
	sealed class ProcessInfo : IProcessInfo
	{
		public ProcessInfo(IntPtr handle, string title)
		{
			this.Handle = handle;
			this.Title = title;
		}

		public IntPtr Handle { get; }
		public string Title { get; }
	}
}