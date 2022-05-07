using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using EveOPreview.Services.Interop;

namespace EveOPreview.Services.Implementation
{
	public class WindowManager : IWindowManager
	{
		#region Private constants
		private const int WINDOW_SIZE_THRESHOLD = 300;
		#endregion

		public WindowManager()
		{
			// Composition is always enabled for Windows 8+
			this.IsCompositionEnabled = 
				((Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor >= 2)) // Win 8 and Win 8.1
				|| (Environment.OSVersion.Version.Major >= 10) // Win 10
				|| DwmNativeMethods.DwmIsCompositionEnabled(); // In case of Win 7 an API call is requiredWin 7
		}

		public bool IsCompositionEnabled { get; }

		public IntPtr GetForegroundWindowHandle()
		{
			return User32NativeMethods.GetForegroundWindow();
		}

		public void ActivateWindow(IntPtr handle)
		{
            FinishActiveKeyStrokes();

			User32NativeMethods.SetForegroundWindow(handle);
            User32NativeMethods.SetFocus(handle);
            //User32NativeMethods.EnableWindow(handle, false);
            //System.Windows.Forms.SendKeys.Send("{ENTER}");
            //User32NativeMethods.EnableWindow(handle, true);

            int style = User32NativeMethods.GetWindowLong(handle, InteropConstants.GWL_STYLE);

			if ((style & InteropConstants.WS_MINIMIZE) == InteropConstants.WS_MINIMIZE)
			{
				User32NativeMethods.ShowWindowAsync(handle, InteropConstants.SW_RESTORE);
			}
		}

        public void FinishActiveKeyStrokes()
        {
            var sw = Stopwatch.StartNew();

            bool isDown = false;
            do
            {
                //isDown = Keyboard.IsKeyDown(Key.LeftCtrl) ||
                //    Keyboard.IsKeyDown(Key.RightCtrl);
            }
            while (isDown && sw.ElapsedMilliseconds < 1000);
        }
        
        public void MinimizeWindow(IntPtr handle, bool enableAnimation)
		{
			if (enableAnimation)
			{
				User32NativeMethods.SendMessage(handle, InteropConstants.WM_SYSCOMMAND, InteropConstants.SC_MINIMIZE, 0);
			}
			else
			{
				WINDOWPLACEMENT param = new WINDOWPLACEMENT();
				param.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
				User32NativeMethods.GetWindowPlacement(handle, ref param);
				param.showCmd = WINDOWPLACEMENT.SW_MINIMIZE;
				User32NativeMethods.SetWindowPlacement(handle, ref param);
			}
		}

		public void MoveWindow(IntPtr handle, int left, int top, int width, int height)
		{
			User32NativeMethods.MoveWindow(handle, left, top, width, height, true);
		}

		public void MaximizeWindow(IntPtr handle)
		{
			User32NativeMethods.ShowWindowAsync(handle, InteropConstants.SW_SHOWMAXIMIZED);
		}

		public (int Left, int Top, int Right, int Bottom) GetWindowPosition(IntPtr handle)
		{
			User32NativeMethods.GetWindowRect(handle, out RECT windowRectangle);

			return (windowRectangle.Left, windowRectangle.Top, windowRectangle.Right, windowRectangle.Bottom);
		}

		public bool IsWindowMaximized(IntPtr handle)
		{
			return User32NativeMethods.IsZoomed(handle);
		}

		public bool IsWindowMinimized(IntPtr handle)
		{
			return User32NativeMethods.IsIconic(handle);
		}

		public IDwmThumbnail GetLiveThumbnail(IntPtr destination, IntPtr source)
		{
			IDwmThumbnail thumbnail = new DwmThumbnail(this);
			thumbnail.Register(destination, source);

			return thumbnail;
		}

		public Image GetStaticThumbnail(IntPtr source)
		{
			var sourceContext = User32NativeMethods.GetDC(source);

			User32NativeMethods.GetClientRect(source, out RECT windowRect);

			var width = windowRect.Right - windowRect.Left;
			var height = windowRect.Bottom - windowRect.Top;

			// Check if there is anything to make thumbnail of
			if ((width < WINDOW_SIZE_THRESHOLD) || (height < WINDOW_SIZE_THRESHOLD))
			{
				return null;
			}

			var destContext = Gdi32NativeMethods.CreateCompatibleDC(sourceContext);
			var bitmap = Gdi32NativeMethods.CreateCompatibleBitmap(sourceContext, width, height);

			var oldBitmap = Gdi32NativeMethods.SelectObject(destContext, bitmap);
			Gdi32NativeMethods.BitBlt(destContext, 0, 0, width, height, sourceContext, 0, 0, Gdi32NativeMethods.SRCCOPY);
			Gdi32NativeMethods.SelectObject(destContext, oldBitmap);
			Gdi32NativeMethods.DeleteDC(destContext);
			User32NativeMethods.ReleaseDC(source, sourceContext);

			Image image = Image.FromHbitmap(bitmap);
			Gdi32NativeMethods.DeleteObject(bitmap);

			return image;
		}
	}
}