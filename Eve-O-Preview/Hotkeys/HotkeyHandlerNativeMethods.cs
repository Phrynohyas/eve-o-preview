using System;
using System.Runtime.InteropServices;

namespace EveOPreview.UI.Hotkeys
{
	static class HotkeyHandlerNativeMethods
	{
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		public const uint WM_HOTKEY = 0x0312;

		public const uint MOD_ALT = 0x1;
		public const uint MOD_CONTROL = 0x2;
		public const uint MOD_SHIFT = 0x4;
		public const uint MOD_WIN = 0x8;

		public const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409;
	}
}