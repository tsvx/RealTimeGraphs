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
		const int N = 60000; // point in the data
		const int M = 50;	  // graphics on the plot
		double[] data;
		int prevFrames;

		void InitData()
		{
			data = new double[N];
			var rnd = new Random();
			double x = 0, v = 0, min = 0, max = 0;
			for (int i = 0; i < N; i++)
			{
				if (x < min) min = x;
				if (x > max) max = x;
				data[i] = x;
				x += v / 1000;
				v += (rnd.NextDouble() - 0.5) / 1000;
			}
			for (int i = 0; i < N; i++)
				data[i] = (data[i] - min) / (max - min);
		}

		public Form1()
		{
			InitializeComponent();
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
