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
		long startTicks;

		public ShiftForm()
		{
			InitializeComponent();
			startTicks = Stopwatch.GetTimestamp();
		}

		private void secondTimer_Tick(object sender, EventArgs e)
		{
			int frames = shiftedControl.FramesCounter;
			int df = frames - prevFrames;
			this.Text = String.Format("{0} [{1} fps] {2}", this.Name, df, shiftedControl.Stats.ToShortString());
			prevFrames = frames;
		}

		private void shiftTimer_Tick(object sender, EventArgs e)
		{
			//shiftedControl.Shift(Stopwatch.GetTimestamp() - startTicks);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			shiftedControl.Start();
		}
	}
}
