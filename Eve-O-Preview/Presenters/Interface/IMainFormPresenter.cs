using System.Collections.Generic;
using System.Drawing;

namespace EveOPreview.Presenters
{
	interface IMainFormPresenter
	{
		void AddThumbnails(IList<string> thumbnailTitles);
		void RemoveThumbnails(IList<string> thumbnailTitles);

		void UpdateThumbnailSize(Size size);
	}
}
