using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RtwfApp
{
	public partial class MovingGraphForm : Form
	{
		int prevFrames;
		TestData data;
		Stopwatch sw;

		public MovingGraphForm()
		{
			InitializeComponent();
			data = new TestData(60000, 3, TimeSpan.FromMilliseconds(1));
			movingGraphControl.Data = data;
			sw = Stopwatch.StartNew();
		}

		private void frameTimer_Tick(object sender, EventArgs e)
		{
			movingGraphControl.Shift(sw.ElapsedTicks);
		}

		private void secondTimer_Tick(object sender, EventArgs e)
		{
			int frames = movingGraphControl.FramesNum;
			int df = frames - prevFrames;
			this.Text = String.Format("[{0} fps]", df);
			prevFrames = frames;
		}
	}
}
