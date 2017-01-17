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
		long curTicks;
		
		public ShiftedControl()
		{
			InitializeComponent();
			tbmp = new TestBitmap(BackColor, ForeColor);
			curTicks = long.MinValue;
		}

		public void Shift(long ticks)
		{
			curTicks = ticks;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			//RazorGFX.DrawImageUnscaled(tbmp.Bitmap, 0, 0);
			if (curTicks != long.MinValue)
			{
				RectangleF rDst = (RectangleF)this.ClientRectangle;
				RectangleF rSrc = new RectangleF((curTicks / 50000f) % this.ClientSize.Width, 0, tbmp.Bitmap.Width / 2, tbmp.Bitmap.Height);
				RazorGFX.DrawImage(tbmp.Bitmap, rDst, rSrc, GraphicsUnit.Pixel);
			}

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
