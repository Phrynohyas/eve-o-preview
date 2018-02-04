using System.Runtime.InteropServices;

namespace EveOPreview.WindowManager.Implementation
{
	[StructLayout(LayoutKind.Sequential)]
	class DWM_THUMBNAIL_PROPERTIES
	{
		public uint dwFlags;
		public RECT rcDestination;
		public RECT rcSource;
		public byte opacity;
		[MarshalAs(UnmanagedType.Bool)]
		public bool fVisible;
		[MarshalAs(UnmanagedType.Bool)]
		public bool fSourceClientAreaOnly;
	}
}