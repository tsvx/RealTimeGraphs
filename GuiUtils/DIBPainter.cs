using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GuiUtils
{
	class DIBPainter : IPainter
	{
		GdiProxy.BITMAPINFO _BI;
		Rectangle r;
		HandleRef hDC;
		Graphics g;
		Bitmap b;

		public void Dispose()
		{
			if (hDC.Handle != IntPtr.Zero && g != null)
			{
				//g.ReleaseHdc(hDC.Handle);
				hDC = new HandleRef();
			}
			g = null;
			b = null;
		}

		public void Assign(Graphics gfx, Rectangle rect, Bitmap bmp)
		{
			Dispose();
			g = gfx;
			b = bmp;
			r = rect;
			_BI = new GdiProxy.BITMAPINFO
			{
				biHeader =
				{
					biBitCount = 32,
					biPlanes = 1,
					biSize = (uint)GdiProxy.BITMAPINFOHEADER.Size,
					biWidth = bmp.Width,
					biHeight = -bmp.Height,
					biSizeImage = (uint)(bmp.Width * bmp.Height) << 2
				}
			};
			//hDC = new HandleRef(g, g.GetHdc());
		}

		public void PlaceBitmap(int bmpX)
		{
			if (g == null)
				return;
			hDC = new HandleRef(g, g.GetHdc());
			var bd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			GdiProxy.SetDIBitsToDevice(hDC, r.X, r.Y, (uint)r.Width, (uint)r.Height, bmpX, 0, 0, (uint)b.Height, bd.Scan0, ref _BI, 0);
			b.UnlockBits(bd);
			g.ReleaseHdc();
		}
	}
}
