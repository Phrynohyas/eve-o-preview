using EveOPreview.Configuration;
using EveOPreview.View;

namespace EveOPreview.UI
{
	static class ViewZoomAnchorConverter
	{
		public static ZoomAnchor Convert(ViewZoomAnchor value)
		{
			// Cheat based on fact that the order and byte values of both enums are the same
			return (ZoomAnchor)((int)value);
		}

		public static ViewZoomAnchor Convert(ZoomAnchor value)
		{
			// Cheat based on fact that the order and byte values of both enums are the same
			return (ViewZoomAnchor)((int)value);
		}
	}
}