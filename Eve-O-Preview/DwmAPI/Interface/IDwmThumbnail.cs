using System;

namespace EveOPreview
{
	public interface IDwmThumbnail
	{
		void Register(IntPtr destination, IntPtr source);
		void Unregister();

		void Move(int left, int top, int right, int bottom);
		void Update();
	}
}
