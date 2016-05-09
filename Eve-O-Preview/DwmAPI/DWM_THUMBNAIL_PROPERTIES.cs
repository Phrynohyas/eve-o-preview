using System.Runtime.InteropServices;

namespace EveOPreview
{
	[StructLayout(LayoutKind.Sequential)]
	internal class DWM_THUMBNAIL_PROPERTIES
	{
		public uint dwFlags;
		public RECT rcDestination;
		public RECT rcSource;
		public byte opacity;
		[MarshalAs(UnmanagedType.Bool)]
		public bool fVisible;
		[MarshalAs(UnmanagedType.Bool)]
		public bool fSourceClientAreaOnly;

		public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
		public const uint DWM_TNP_RECTSOURCE = 0x00000002;
		public const uint DWM_TNP_OPACITY = 0x00000004;
		public const uint DWM_TNP_VISIBLE = 0x00000008;
		public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
	}
}