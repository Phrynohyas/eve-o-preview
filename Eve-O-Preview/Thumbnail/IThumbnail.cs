using System;
using System.Drawing;

namespace EveOPreview
{
	public interface IThumbnail
	{
		bool IsZoomEnabled { get; set; }
		bool IsPreviewEnabled { get; set; }
		bool IsOverlayEnabled { get; set; }

		bool IsPreviewHandle(IntPtr handle);

		void ShowThumbnail();
		void HideThumbnail();
		void CloseThumbnail();

		void RegisterShortcut(string shortcut);

		void SetLabel(string label);
		string GetLabel();

		void SetLocation(Point location);
		Point GetLocation();

		void SetOpacity(double opacity);
		void SetTopMost(bool topmost);
		void SetWindowFrames(bool enable);
		void SetSize(Size size);
	}
}