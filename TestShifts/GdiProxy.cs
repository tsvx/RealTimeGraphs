using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public enum TernaryRasterOperations : uint
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062,
			CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
		}

		[DllImport("gdi32.dll")]
		private static extern IntPtr SelectObject([In] HandleRef hdc, [In] HandleRef h);

		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject([In] HandleRef h);

		[DllImport("gdi32.dll")]
		private static extern bool BitBlt(HandleRef hDCDest, int nXDest, int nYDest, int nWidth, int nHeight,
										HandleRef hDCSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

		public static bool BitBlt(Graphics gDst, Rectangle rect, Bitmap srcBmp, Point pSrc, TernaryRasterOperations rop)
		{
			using (Graphics gSrc = Graphics.FromImage(srcBmp))
			{
				var hSrc = new HandleRef(gSrc, gSrc.GetHdc());
				var hDst = new HandleRef(gDst, gDst.GetHdc());
				var hBmp = new HandleRef(srcBmp, srcBmp.GetHbitmap());
				var hOldObj = new HandleRef(gSrc, SelectObject(hSrc, hBmp));
				if (hOldObj.Handle == IntPtr.Zero)
					throw new Win32Exception();
				bool rslt = BitBlt(hDst, rect.X, rect.Y, rect.Width, rect.Height, hSrc, pSrc.X, pSrc.Y, rop);
				SelectObject(hSrc, hOldObj);
				DeleteObject(hBmp);
				gDst.ReleaseHdc();
				gSrc.ReleaseHdc();
				return rslt;
			}
		}
	}
}
