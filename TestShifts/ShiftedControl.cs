using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestShifts
{
	public partial class ShiftedControl : RazorGDIPainter.RazorPainterControl
	{
		public int FramesCounter { get; private set; }
		
		public ShiftedControl()
		{
			InitializeComponent();
		}

		public void Shift(long ticks)
		{
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			FramesCounter++;
		}
	}
}
