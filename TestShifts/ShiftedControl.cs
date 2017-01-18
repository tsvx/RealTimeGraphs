using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
			if (curTicks != long.MinValue)
			{
				// 0. 3.3% out of 25%
				//PlaceBitmapDummy(RazorGFX, tbmp.Bitmap, 0);

				// 1. 12 FPS out of 65 FPS.
				//float x = (curTicks / 50000f) % this.ClientSize.Width;
				//PlaceBitmapFloat(RazorGFX, tbmp.Bitmap, x);

				// 2. 24%, 65 FPS.
				int x = (int)((curTicks / 50000) % this.ClientSize.Width);
				//PlaceBitmapInt(RazorGFX, tbmp.Bitmap, x);

				// 3. 24%, 65 FPS.
				//PlaceBitmapUnscaled(RazorGFX, tbmp.Bitmap, 0);

				// 4. 11.6%
				PlaceBitmapUnsafe(RazorBMP, tbmp.Bitmap, x);
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

		void PlaceBitmapDummy(Graphics dstGfx, Bitmap srcBmp, int x)
		{
		}
				
		void PlaceBitmapFloat(Graphics dstGfx, Bitmap srcBmp, float x)
		{
			var rDst = (RectangleF)this.ClientRectangle;
			var rSrc = new RectangleF(x, 0, srcBmp.Width / 2, srcBmp.Height);
			dstGfx.DrawImage(srcBmp, rDst, rSrc, GraphicsUnit.Pixel);
		}

		void PlaceBitmapInt(Graphics dstGfx, Bitmap srcBmp, int x)
		{
			var rDst = this.ClientRectangle;
			var rSrc = new Rectangle(x, 0, srcBmp.Width / 2, srcBmp.Height);
			dstGfx.DrawImage(srcBmp, rDst, rSrc, GraphicsUnit.Pixel);
		}

		void PlaceBitmapUnscaled(Graphics dstGfx, Bitmap srcBmp, int x)
		{
			dstGfx.DrawImageUnscaled(srcBmp, 0, 0);
		}

		static unsafe void PlaceBitmapUnsafe(Bitmap dst, Bitmap src, int x)
		{
			int w = dst.Width, h = dst.Height;

			var bdSrc = src.LockBits(new Rectangle(x, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			var bdDst = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

			int* ps = (int*)bdSrc.Scan0, pd = (int*)bdDst.Scan0;
			for (int i = 0; i < h; i++)
			{
				for (int j = 0; j < w; j++)
					*pd++ = *ps++;
				pd += bdDst.Stride / 4 - w;
				ps += bdSrc.Stride / 4 - w;
			}
			
			dst.UnlockBits(bdDst);
			src.UnlockBits(bdSrc);
		}
	}
}
