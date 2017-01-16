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
		TestBitmap tbmp;
		
		public ShiftedControl()
		{
			InitializeComponent();
			tbmp = new TestBitmap(BackColor, ForeColor);
		}

		public void Shift(long ticks)
		{
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			RazorGFX.DrawImageUnscaled(tbmp.Bitmap, this.ClientRectangle.Location);
			this.RazorPaint();
			FramesCounter++;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (Created)
				tbmp.Resize(this.ClientSize.Width, this.ClientSize.Height);
		}
	}
}
