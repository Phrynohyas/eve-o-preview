using System.Runtime.InteropServices;

namespace EveOPreview.DwmInterop
{
	[StructLayout(LayoutKind.Sequential)]
	class MARGINS
	{
		public int cxLeftWidth;
		public int cxRightWidth;
		public int cyTopHeight;
		public int cyBottomHeight;

		public MARGINS(int left, int top, int right, int bottom)
		{
			cxLeftWidth = left;
			cyTopHeight = top;
			cxRightWidth = right;
			cyBottomHeight = bottom;
		}
	}
}