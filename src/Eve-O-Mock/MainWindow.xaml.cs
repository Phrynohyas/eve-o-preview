using System;
using System.Windows;
using System.Windows.Media;

namespace EveOMock
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Random random = new Random();
			this.Title += random.Next().ToString("X");
			this.grid.Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
		}
	}
}
