using System;
using System.Drawing;

namespace EveOPreview.UI
{
	public interface IThumbnailView : IView
	{
		IntPtr Id { get; set; }
		string Title { get; set; }

		bool IsEnabled { get; set; }
		bool IsActive { get; set; }
		Point Location { get; set; }
		Size Size { get; set; }
		bool IsOverlayEnabled { get; set; }

		bool IsKnownHandle(IntPtr handle);

		void SetOpacity(double opacity);
		void SetWindowFrames(bool enable);
		void SetTopMost(bool enableTopmost);

		void Refresh();

		event Action<IntPtr> ThumbnailResized;
		event Action<IntPtr> ThumbnailMoved;
		event Action<IntPtr> ThumbnailFocused;
		event Action<IntPtr> ThumbnailLostFocus;
		event Action<IntPtr> ThumbnailActivated;
	}
}