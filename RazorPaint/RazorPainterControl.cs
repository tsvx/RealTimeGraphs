// Test control fronend for WindowsForms for RazorGDIPainter library
//   (c) Mokrov Ivan
// special for habrahabr.ru
// under MIT license

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RazorGDIPainter
{
	public class RazorPainterControl : Control
	{
		protected override void Dispose(bool disposing)
		{
			lock (this)
			{
				if (RazorGFX != null) RazorGFX.Dispose();
				if (RazorBMP != null) RazorBMP.Dispose();
				//hDCGraphics.ReleaseHdc();
				if (hDCGraphics != null) hDCGraphics.Dispose();
				//RP.Dispose();
			}
			base.Dispose(disposing);
		}

		private readonly HandleRef hDCRef;
		private readonly Graphics hDCGraphics;
		private readonly RazorPainter RP;

		/// <summary>
		/// root Bitmap
		/// </summary>
		public Bitmap RazorBMP { get; private set; }

		/// <summary>
		/// Graphics object to paint on RazorBMP
		/// </summary>
		public Graphics RazorGFX { get; private set; }

		/// <summary>
		/// Lock it to avoid resize/repaint race
		/// </summary>
		public readonly object RazorLock = new object();

		public RazorPainterControl()
		{
			this.MinimumSize = new Size(1, 1);

			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);

			hDCGraphics = this.CreateGraphics();
			hDCRef = new HandleRef(hDCGraphics, hDCGraphics.GetHdc());

			RP = new RazorPainter();
			RazorBMP = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
			RazorGFX = Graphics.FromImage(RazorBMP);
		}

		protected override void OnResize(EventArgs e)
		{
			lock (RazorLock)
			{
				if (RazorGFX != null) RazorGFX.Dispose();
				if (RazorBMP != null) RazorBMP.Dispose();
				RazorBMP = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
				RazorGFX = Graphics.FromImage(RazorBMP);
			}
			base.OnResize(e);
		}

		/// <summary>
		/// After all in-memory paint on RazorGFX, call it to display it on control
		/// </summary>
		public void RazorPaint()
		{
			RP.Paint(hDCRef, RazorBMP);
		}
	}
}
