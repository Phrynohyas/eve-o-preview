namespace EveOPreview.View
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
