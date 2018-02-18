using System;

namespace EveOPreview.Services
{
	public static class InteropConstants
	{
		public const int GWL_ID = (-12);
		public const int GWL_STYLE = (-16);
		public const int GWL_EXSTYLE = (-20);

		// Window Styles 
		public const UInt32 WS_OVERLAPPED = 0;
		public const UInt32 WS_POPUP = 0x80000000;
		public const UInt32 WS_CHILD = 0x40000000;
		public const UInt32 WS_MINIMIZE = 0x20000000;
		public const UInt32 WS_VISIBLE = 0x10000000;
		public const UInt32 WS_DISABLED = 0x8000000;
		public const UInt32 WS_CLIPSIBLINGS = 0x4000000;
		public const UInt32 WS_CLIPCHILDREN = 0x2000000;
		public const UInt32 WS_MAXIMIZE = 0x1000000;
		public const UInt32 WS_CAPTION = 0xC00000;      // WS_BORDER or WS_DLGFRAME  
		public const UInt32 WS_BORDER = 0x800000;
		public const UInt32 WS_DLGFRAME = 0x400000;
		public const UInt32 WS_VSCROLL = 0x200000;
		public const UInt32 WS_HSCROLL = 0x100000;
		public const UInt32 WS_SYSMENU = 0x80000;
		public const UInt32 WS_THICKFRAME = 0x40000;
		public const UInt32 WS_GROUP = 0x20000;
		public const UInt32 WS_TABSTOP = 0x10000;
		public const UInt32 WS_MINIMIZEBOX = 0x20000;
		public const UInt32 WS_MAXIMIZEBOX = 0x10000;
		public const UInt32 WS_TILED = WS_OVERLAPPED;
		public const UInt32 WS_ICONIC = WS_MINIMIZE;
		public const UInt32 WS_SIZEBOX = WS_THICKFRAME;

		// Extended Window Styles 
		public const UInt32 WS_EX_DLGMODALFRAME = 0x0001;
		public const UInt32 WS_EX_NOPARENTNOTIFY = 0x0004;
		public const UInt32 WS_EX_TOPMOST = 0x0008;
		public const UInt32 WS_EX_ACCEPTFILES = 0x0010;
		public const UInt32 WS_EX_TRANSPARENT = 0x0020;
		public const UInt32 WS_EX_MDICHILD = 0x0040;
		public const UInt32 WS_EX_TOOLWINDOW = 0x0080;
		public const UInt32 WS_EX_WINDOWEDGE = 0x0100;
		public const UInt32 WS_EX_CLIENTEDGE = 0x0200;
		public const UInt32 WS_EX_CONTEXTHELP = 0x0400;
		public const UInt32 WS_EX_RIGHT = 0x1000;
		public const UInt32 WS_EX_LEFT = 0x0000;
		public const UInt32 WS_EX_RTLREADING = 0x2000;
		public const UInt32 WS_EX_LTRREADING = 0x0000;
		public const UInt32 WS_EX_LEFTSCROLLBAR = 0x4000;
		public const UInt32 WS_EX_RIGHTSCROLLBAR = 0x0000;
		public const UInt32 WS_EX_CONTROLPARENT = 0x10000;
		public const UInt32 WS_EX_STATICEDGE = 0x20000;
		public const UInt32 WS_EX_APPWINDOW = 0x40000;
		public const UInt32 WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
		public const UInt32 WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
		public const UInt32 WS_EX_LAYERED = 0x00080000;
		public const UInt32 WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
		public const UInt32 WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
		public const UInt32 WS_EX_COMPOSITED = 0x02000000;
		public const UInt32 WS_EX_NOACTIVATE = 0x08000000;

		public const int WM_SIZE = 5;
		public const int WM_SYSCOMMAND = 0x0112;

		public const int SC_MINIMIZE = 0xf020;

		public const int SIZE_RESTORED = 0;
		public const int SIZE_MINIMIZED = 1;
		public const int SIZE_MAXIMIZED = 2;
		public const int SIZE_MAXSHOW = 3;
		public const int SIZE_MAXHIDE = 4;

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HTCAPTION = 0x2;

		public const int SW_SHOWNORMAL = 1;
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_SHOWMAXIMIZED = 3;
	}
}