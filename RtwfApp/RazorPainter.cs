// RazorGDIPainter library - ultrafast 2D painting. See test applications
// on http://razorgdipainter.codeplex.com/
//   (c) Mokrov Ivan
// special for habrahabr.ru
// under MIT license

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace RazorGDIPainter
{
	public class RazorPainter
    {
		[DllImport("gdi32")]
		private extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, IntPtr lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFOHEADER
		{
			static readonly int size = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
			static public int Size { get { return size; } }

			public int		bihSize;
			public int		bihWidth;
			public int		bihHeight;
			public short	bihPlanes;
			public short	bihBitCount;
			public int		bihCompression;
			public int		bihSizeImage;
			public double	bihXPelsPerMeter;
			public double	bihClrUsed;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFO
		{
			public BITMAPINFOHEADER biHeader;
			public int biColors;
		}

		private int width;
		private int height;
	    private BITMAPINFO _BI;

		public int Width { get { return width; } }
		public int Height { get { return height; } }

		private void Realloc(int width, int height)
		{
			_BI = new BITMAPINFO
			{
				biHeader =
				{
					bihBitCount = 32,
					bihPlanes = 1,
					bihSize = BITMAPINFOHEADER.Size,
					bihWidth = this.width = width,
					bihHeight = -(this.height = height),
					bihSizeImage = (width * height) << 2
				}
			};
		}

		public void Paint(HandleRef hRef, Bitmap bitmap)
		{
			if (bitmap == null || bitmap.Width == 0 || bitmap.Height == 0)
				throw new ArgumentException("Null or vacuous Bitmap", "bitmap");

			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
				throw new ArgumentException("PixelFormat must be Format32bppArgb", "bitmap");

			if (bitmap.Width != width || bitmap.Height != height)
				Realloc(bitmap.Width, bitmap.Height);

			BitmapData BD = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			SetDIBitsToDevice(hRef, 0, 0, width, height, 0, 0, 0, height, BD.Scan0, ref _BI, 0);
			bitmap.UnlockBits(BD);
		}
    }
}
