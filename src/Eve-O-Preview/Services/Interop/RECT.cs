using System.Runtime.InteropServices;

namespace EveOPreview.Services.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}
	}
}