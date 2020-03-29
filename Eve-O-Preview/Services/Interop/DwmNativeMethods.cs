using System;
using System.Runtime.InteropServices;

namespace EveOPreview.Services.Interop
{
	static class DwmNativeMethods
	{
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern bool DwmIsCompositionEnabled();
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern IntPtr DwmRegisterThumbnail(IntPtr dest, IntPtr source);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmUpdateThumbnailProperties(IntPtr hThumbnail, DWM_THUMBNAIL_PROPERTIES props);
	}
}