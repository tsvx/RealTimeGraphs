using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TestShifts
{
	class BlitPainter : IPainter
	{
		Rectangle r;
		HandleRef hd, hs, hb, hOldObj;
		Graphics gd, gs;
		Bitmap b;

		public void Dispose()
		{
			if (hd.Handle == IntPtr.Zero || gd == null)
				return;
			GdiProxy.SelectObject(hs, hOldObj);
			GdiProxy.DeleteObject(hb);
			//gd.ReleaseHdc();
			gs.ReleaseHdc();
			gs.Dispose();
			gd = gs = null;
			b = null;
		}

		public void Assign(Graphics gfx, Rectangle rect, Bitmap bmp)
		{
			Dispose();
			gd = gfx;
			b = bmp;
			r = rect;
			gs = Graphics.FromImage(bmp);
			hs = new HandleRef(gs, gs.GetHdc());
			//hd = new HandleRef(gd, gd.GetHdc());
			hb = new HandleRef(bmp, bmp.GetHbitmap());
			hOldObj = new HandleRef(gs, GdiProxy.SelectObject(hs, hb));
			if (hOldObj.Handle == IntPtr.Zero)
				throw new Win32Exception();
		}

		public void PlaceBitmap(int bmpX)
		{
			hd = new HandleRef(gd, gd.GetHdc());
			GdiProxy.BitBlt(hd, r.X, r.Y, r.Width, r.Height, hs, bmpX, 0, GdiProxy.TernaryRasterOperations.SRCCOPY);
			gd.ReleaseHdc();
		}
	}
}
