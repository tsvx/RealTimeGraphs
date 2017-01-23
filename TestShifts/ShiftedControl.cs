using NIIT.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
		public StatsAccum Stats;
		public BiStatsAccum BiStats;

		TestBitmap tbmp;
		long curTicks, n;
		double msInTimerTick;

		MultimediaTimer.AccurateTimer timer;
		
		public ShiftedControl()
		{
			Stats = new StatsAccum();
			BiStats = new BiStatsAccum(15, true);
			InitializeComponent();
			tbmp = new TestBitmap(BackColor, ForeColor);
			curTicks = long.MinValue;
			n = 0;
			int min, max, cur;
			MultimediaTimer.AccurateTimer.QueryTimerResolution(out min, out max, out cur);
			msInTimerTick = min / 1e4;
		}

		public void Start()
		{
			timer = new MultimediaTimer.AccurateTimer(TimerTick, 15);
		}

		void TimerTick()
		{
			Shift(Stopwatch.GetTimestamp());
		}

		public void Shift(long ticks)
		{
			n++;
			curTicks = ticks;
			lock (this.RazorLock)
				Render();
			//Invalidate();
		}

		//protected override void OnPaint(PaintEventArgs pe)
		//{
		//	base.OnPaint(pe);
		//	lock (this.RazorLock)
		//		Render();
		//}

		void Render()
		{
			if (curTicks != long.MinValue)
			{
				long realTicks = Stopwatch.GetTimestamp(), dt = realTicks - curTicks;
				Stats.Add(dt * 1e3 / Stopwatch.Frequency);
				//BiStats.Add(n, curTicks * 1e3 / Stopwatch.Frequency);
				//Stats.Add(n * msInTimerTick - curTicks * 1e3 / Stopwatch.Frequency);
				//curTicks = realTicks;

				// Tested on:
				// YODA. 1920x1018 AMD HD 6570, W7x64 i5-3470 4@3.2GHz, Mem speed is ~20GB/s, so
				// 8MB*65 times ~ 0.5GB => / 20 = 2,5% read and 5% copy, should be.
				// SEASHELL. 1366x705, W10x64 i5-3317U 2x2@1.7-2.4GHz, Mem speed ~15GB/s, so
				// 4MB*65 ~ 0.25GB => /15 = 1.7% read and 3.3% copy.

				// 0. Nop.
				// YODA: 3.3% out of 25%, 65 FPS (max)
				// SEASHELL: 4.3% out of 25%, 64 FPS (max)
				//PlaceBitmapDummy(RazorGFX, tbmp.Bitmap, 0);

				// 1. float DrawImage
				// YODA: 12 FPS, 25%
				// SEASHELL: 64 FPS, 18%
				//float fx = (curTicks / 50000f) % this.ClientSize.Width;
				//PlaceBitmapFloat(RazorGFX, tbmp.Bitmap, fx);

				// 2. int DrawImage
				// YODA: 24%, 65 FPS.
				// SEASHELL: 18%, 64 FPS
				int x = (int)((curTicks / 5000) % this.ClientSize.Width);
		
				//PlaceBitmapInt(RazorGFX, tbmp.Bitmap, x);

				// 3. DrawImageUnscaled
				// YODA: 24%, 65 FPS, no shift, just to try.
				//PlaceBitmapUnscaled(RazorGFX, tbmp.Bitmap, 0);

				// 4. unsafe copy int*w*h + 2*(Lock+Unlock)Bits
				// YODA: 11.6%
				// SEASHELL: 13-14%
				//PlaceBitmapUnsafe(RazorBMP, tbmp.Bitmap, x);

				// 4'. unsafe memcpy (int*w)*h + 2*(Lock+Unlock)Bits
				// YODA: 6.2%
				// SEASHELL: 6.8%
				PlaceBitmapUnsafe2(RazorBMP, tbmp.Bitmap, x);

				// 5. SetDIBitsToDevice + (Lock/Unlock)Bits + (Get/Release)Hdc
				// YODA: 10.0%
				// SeaShell: 11%
				//PlaceBitmapSetDIBitsToDevice(RazorGFX, tbmp.Bitmap, x);

				// 6. BitBlt + 2*(Get/Release)Hdc + GetHBitmap/DeleteObject + 2*SelectObject
				// YODA: 38 FPS, but can be cached (Bitmap.GetHBitmap and hDCs)
				// SeaShell: 50 FPS, 25%
				//PlaceBitmapBitBlt(RazorGFX, tbmp.Bitmap, x);
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
			int dstAdd = bdDst.Stride / 4 - w, srcAdd = bdSrc.Stride / 4 - w;
			for (int i = 0; i < h; i++)
			{
				for (int j = 0; j < w; j++)
					*pd++ = *ps++;
				pd += dstAdd;
				ps += srcAdd;
			}
			
			dst.UnlockBits(bdDst);
			src.UnlockBits(bdSrc);
		}

		static unsafe void PlaceBitmapUnsafe2(Bitmap dst, Bitmap src, int x)
		{
			int w = dst.Width, h = dst.Height;

			var bdSrc = src.LockBits(new Rectangle(x, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			var bdDst = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

			byte* ps = (byte*)bdSrc.Scan0, pd = (byte*)bdDst.Scan0;
			int dstAdd = bdDst.Stride, srcAdd = bdSrc.Stride;
			for (int i = 0; i < h; i++)
			{
				GdiProxy.CopyMemory(pd, ps, (ulong)(w << 2));
				pd += dstAdd;
				ps += srcAdd;
			}

			dst.UnlockBits(bdDst);
			src.UnlockBits(bdSrc);
		}

		void PlaceBitmapSetDIBitsToDevice(Graphics dstGfx, Bitmap srcBmp, int x)
		{
			GdiProxy.SetDIBitsToDevice(dstGfx, this.ClientRectangle, srcBmp, x, 0);
		}

		void PlaceBitmapBitBlt(Graphics dstGfx, Bitmap srcBmp, int x)
		{
			bool rslt = GdiProxy.BitBlt(dstGfx, this.ClientRectangle, srcBmp, new Point(x, 0), GdiProxy.TernaryRasterOperations.SRCCOPY);
			Debug.Assert(rslt);
		}

	}
}
