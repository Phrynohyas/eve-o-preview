using System;

namespace EveOPreview
{
	public interface IWindowManager
	{
		bool IsCompositionEnabled { get; }

		IntPtr GetForegroundWindowHandle();

		void ActivateWindow(IntPtr handle);
		void DeactivateWindow(IntPtr handle);

		void MoveWindow(IntPtr handle, int left, int top, int width, int height);
		void GetWindowCoordinates(IntPtr handle, out int left, out int top, out int right, out int bottom);
	}
}