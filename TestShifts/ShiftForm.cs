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
			this.Text = String.Format("{0} [monitor {1} vfps] [real {2} fps], move {3:G5} ms/pixel = {4:G4} pixels/vframe = {5:G4} ms/screen",
				this.Name, shiftedControl.MonitorRefreshRate, df,
				shiftedControl.Ms2pixel,
				shiftedControl.PixelsInVRate,
				shiftedControl.Ms2pixel * shiftedControl.ClientSize.Width
			);
			prevFrames = frames;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			shiftedControl.Start();
		}
	}
}
