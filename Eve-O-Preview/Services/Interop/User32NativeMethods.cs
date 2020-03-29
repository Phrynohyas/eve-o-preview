using System;
using System.Runtime.InteropServices;

namespace EveOPreview.Services.Interop
{
	static class User32NativeMethods
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr window);

		[DllImport("user32.dll")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		public static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool IsZoomed(IntPtr hWnd);
	}
}