using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace EveOPreview.WindowManager.Implementation
{
	static class DwmApiNativeMethods
	{
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, MARGINS pMargins);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern bool DwmIsCompositionEnabled();

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmGetColorizationColor(
			out int pcrColorization,
			[MarshalAs(UnmanagedType.Bool)]out bool pfOpaqueBlend);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmEnableComposition(bool bEnable);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern IntPtr DwmRegisterThumbnail(IntPtr dest, IntPtr source);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmUpdateThumbnailProperties(IntPtr hThumbnail, DWM_THUMBNAIL_PROPERTIES props);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out Size size);
	}
}