using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestShifts
{
	public partial class ShiftForm : Form
	{
		int prevFrames;

		public ShiftForm()
		{
			InitializeComponent();
		}

		private void secondTimer_Tick(object sender, EventArgs e)
		{
			int frames = shiftedControl.FramesCounter;
			int df = frames - prevFrames;
			this.Text = String.Format("[{0} fps]", df);
			prevFrames = frames;
		}

		private void shiftTimer_Tick(object sender, EventArgs e)
		{
			shiftedControl.Shift(Stopwatch.GetTimestamp());
		}
	}
}
