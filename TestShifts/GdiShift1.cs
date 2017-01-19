using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TestShifts
{
	//public class GdiShift1 : IDisposable
	//{
	//	private int width;
	//	private int height;
	//	private GdiProxy.BITMAPINFO _BI;
	//	HandleRef hDCRef;
	//	Graphics g;

	//	public int Width { get { return width; } }
	//	public int Height { get { return height; } }

	//	public void Resize(int width, int height)
	//	{
	//		_BI = new GdiProxy.BITMAPINFO
	//		{
	//			biHeader =
	//			{
	//				biBitCount = 32,
	//				biPlanes = 1,
	//				biSize = (uint)GdiProxy.BITMAPINFOHEADER.Size,
	//				biWidth = bmp.Width,
	//				biHeight = -bmp.Height,
	//				biSizeImage = (uint)(bmp.Width * bmp.Height) << 2
	//			}
	//		};
	//		hDCRef = new HandleRef(gfx, gfx.GetHdc());
	//	}

	//	public void SetDIBitsToDevice(Graphics gfx, Rectangle rect, Bitmap bmp, int x, int y)
	//	{
	//		var bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
	//		GdiProxy.SetDIBitsToDevice(hDCRef, rect.X, rect.Y, (uint)rect.Width, (uint)rect.Height, x, y, 0, (uint)bmp.Height, bd.Scan0, ref _BI, 0);
	//		bmp.UnlockBits(bd);
	//	}

	//	public void Dispose()
	//	{
	//		gfx.ReleaseHdc();
	//	}
	//}
}
