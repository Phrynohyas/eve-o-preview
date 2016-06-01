namespace EveOPreview.UI
{
	public class ViewCloseRequest
	{
		public ViewCloseRequest()
		{
			this.Allow = true;
		}

		public bool Allow { get; set; }
	}
}
