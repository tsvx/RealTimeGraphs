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
	public partial class Form1 : Form
	{
		int prevFrames;
		TestData data;

		public Form1()
		{
			InitializeComponent();
			data = new TestData(60000, 50);
			movingGraphControl1.Data = data;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			movingGraphControl1.Shift(Stopwatch.GetTimestamp());
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			int frames = movingGraphControl1.FramesNum;
			int df = frames - prevFrames;
			this.Text = String.Format("[{0} fps]", df);
			prevFrames = frames;
		}
	}
}
