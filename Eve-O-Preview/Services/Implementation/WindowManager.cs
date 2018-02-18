using System;
using EveOPreview.Services.Interop;

namespace EveOPreview.Services.Implementation
{
	class WindowManager : IWindowManager
	{
		public WindowManager()
		{
			this.IsCompositionEnabled = DwmApiNativeMethods.DwmIsCompositionEnabled();
		}

		public bool IsCompositionEnabled { get; }

		public IntPtr GetForegroundWindowHandle()
		{
			return User32NativeMethods.GetForegroundWindow();
		}

		public void ActivateWindow(IntPtr handle)
		{
			User32NativeMethods.SetForegroundWindow(handle);

			int style = User32NativeMethods.GetWindowLong(handle, InteropConstants.GWL_STYLE);

			if ((style & InteropConstants.WS_MINIMIZE) == InteropConstants.WS_MINIMIZE)
			{
				User32NativeMethods.ShowWindowAsync(handle, InteropConstants.SW_SHOWNORMAL);
			}
		}

		public void DeactivateWindow(IntPtr handle)
		{
			User32NativeMethods.SendMessage(handle, InteropConstants.WM_SYSCOMMAND, InteropConstants.SC_MINIMIZE, 0);
		}

		public void MoveWindow(IntPtr handle, int left, int top, int width, int height)
		{
			User32NativeMethods.MoveWindow(handle, left, top, width, height, true);
		}

		public void GetWindowCoordinates(IntPtr handle, out int left, out int top, out int right, out int bottom)
		{
			User32NativeMethods.GetWindowRect(handle, out RECT windowRectangle);

			left = windowRectangle.Left;
			top = windowRectangle.Top;
			right = windowRectangle.Right;
			bottom = windowRectangle.Bottom;
		}

		public bool IsWindowMinimized(IntPtr handle)
		{
			return User32NativeMethods.IsIconic(handle);
		}

		public IDwmThumbnail RegisterThumbnail(IntPtr destination, IntPtr source)
		{
			IDwmThumbnail thumbnail = new DwmThumbnail(this);
			thumbnail.Register(destination, source);

			return thumbnail;
		}
	}
}