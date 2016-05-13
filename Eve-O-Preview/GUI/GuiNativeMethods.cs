using System;
using System.Runtime.InteropServices;

namespace EveOPreview
{
	// TODO This is a really bad name for this class
	static class GuiNativeMethods
	{
		public const int WM_SIZE = 5;
		public const int SIZE_RESTORED = 0;
		public const int SIZE_MINIMIZED = 1;
		public const int SIZE_MAXIMIZED = 2;
		public const int SIZE_MAXSHOW = 3;
		public const int SIZE_MAXHIDE = 4;

		[DllImport("user32.dll")]
		public static extern int GetWindowRect(IntPtr hwnd, out RECT rect);

		[DllImport("user32.dll")]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
	}
}