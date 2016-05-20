using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EveOPreview
{
	static class HotkeyNativeMethods
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int UnregisterHotKey(IntPtr hWnd, int id);

		public const uint WM_HOTKEY = 0x312;

		public const uint MOD_ALT = 0x1;
		public const uint MOD_CONTROL = 0x2;
		public const uint MOD_SHIFT = 0x4;
		public const uint MOD_WIN = 0x8;

		public const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409;
	}
}