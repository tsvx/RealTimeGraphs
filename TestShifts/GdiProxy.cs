using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TestShifts
{
	public class GdiProxy
	{
		[DllImport("gdi32")]
		private extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, uint dwWidth, uint dwHeight, int XSrc, int YSrc, uint uStartScan, uint cScanLines, IntPtr lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFOHEADER
		{
			static readonly int size = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
			static public int Size { get { return size; } }

			public uint biSize;
			public int biWidth;
			public int biHeight;
			public ushort biPlanes;
			public ushort biBitCount;
			public uint biCompression;
			public uint biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public uint biClrUsed;
			public uint biClrImportant;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFO
		{
			public BITMAPINFOHEADER biHeader;
			public int biColors;
		}

		public static void SetDIBitsToDevice(Graphics gfx, Rectangle rect, Bitmap bmp, int x, int y)
		{
			var _BI = new BITMAPINFO
			{
				biHeader =
				{
					biBitCount = 32,
					biPlanes = 1,
					biSize = (uint)BITMAPINFOHEADER.Size,
					biWidth = bmp.Width,
					biHeight = -bmp.Height,
					biSizeImage = (uint)(bmp.Width * bmp.Height) << 2
				}
			};
			var hDCRef = new HandleRef(gfx, gfx.GetHdc());

			var bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			SetDIBitsToDevice(hDCRef, rect.X, rect.Y, (uint)rect.Width, (uint)rect.Height, x, y, 0, (uint)bmp.Height, bd.Scan0, ref _BI, 0);
			bmp.UnlockBits(bd);
			
			gfx.ReleaseHdc();
		}
	}
}
