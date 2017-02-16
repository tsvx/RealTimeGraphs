using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplication1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public int FPS { get; set; }

		DispatcherTimer timer;

		public MainWindow()
		{
			InitializeComponent();
			timer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			timer.Tick += timer_Tick;
			timer.Start();
			CompositionTarget.Rendering += CompositionTarget_Rendering;
		}

		void timer_Tick(object sender, EventArgs e)
		{
			myLabel.Content = FPS;
			this.Title = FPS.ToString();
			FPS = 0;
		}

		void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			FPS++;
		}
	}
}
