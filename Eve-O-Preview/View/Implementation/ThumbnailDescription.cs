namespace EveOPreview.View
{
	sealed class ThumbnailDescription
	{
		public ThumbnailDescription(string title, bool isDisabled)
		{
			this.Title = title;
			this.IsDisabled = isDisabled;
		}

		public string Title { get; set; }
		public bool IsDisabled { get; set; }
	}
}