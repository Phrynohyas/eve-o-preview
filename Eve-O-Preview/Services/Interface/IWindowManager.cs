using System;

namespace EveOPreview.Services
{
	public interface IWindowManager
	{
		bool IsCompositionEnabled { get; }

		IntPtr GetForegroundWindowHandle();

		void ActivateWindow(IntPtr handle);
		void MinimizeWindow(IntPtr handle, bool enableAnimation);

		void MoveWindow(IntPtr handle, int left, int top, int width, int height);
		(int Left, int Top, int Right, int Bottom) GetWindowPosition(IntPtr handle);
		bool IsWindowMinimized(IntPtr handle);

		IDwmThumbnail RegisterThumbnail(IntPtr destination, IntPtr source);
	}
}