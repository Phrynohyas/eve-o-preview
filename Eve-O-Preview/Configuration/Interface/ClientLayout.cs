namespace EveOPreview.Configuration
{
	public class ClientLayout
	{
		public ClientLayout()
		{
		}

		public ClientLayout(int x, int y, int width, int height, bool maximized)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			this.IsMaximized = maximized;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public bool IsMaximized { get; set; }
	}
}