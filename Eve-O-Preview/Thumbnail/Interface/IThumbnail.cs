using System;
using System.Drawing;

namespace EveOPreview.Thumbnails
{
	public interface IThumbnail
	{
		IntPtr Id { get; }

		bool IsZoomEnabled { get; set; }
		bool IsPreviewEnabled { get; set; }
		bool IsOverlayEnabled { get; set; }

		bool IsPreviewHandle(IntPtr handle);

		void ShowThumbnail();
		void HideThumbnail();
		void CloseThumbnail();

		void RegisterShortcut(string shortcut);

		// TODO Turn this into a property
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