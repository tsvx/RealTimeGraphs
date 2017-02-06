using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace TestShifts
{
	public class GdiProxy
	{
		[DllImport("gdi32")]
		private extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, uint dwWidth, uint dwHeight, int XSrc, int YSrc,
			uint uStartScan, uint cScanLines, IntPtr lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

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

		[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
		public static unsafe extern void* CopyMemory(void* dest, void* src, ulong count);

		public enum DeviceCap
		{
			/// <summary>
			/// Device driver version
			/// </summary>
			DRIVERVERSION = 0,
			/// <summary>
			/// Device classification
			/// </summary>
			TECHNOLOGY = 2,
			/// <summary>
			/// Horizontal size in millimeters
			/// </summary>
			HORZSIZE = 4,
			/// <summary>
			/// Vertical size in millimeters
			/// </summary>
			VERTSIZE = 6,
			/// <summary>
			/// Horizontal width in pixels
			/// </summary>
			HORZRES = 8,
			/// <summary>
			/// Vertical height in pixels
			/// </summary>
			VERTRES = 10,
			/// <summary>
			/// Number of bits per pixel
			/// </summary>
			BITSPIXEL = 12,
			/// <summary>
			/// Number of planes
			/// </summary>
			PLANES = 14,
			/// <summary>
			/// Number of brushes the device has
			/// </summary>
			NUMBRUSHES = 16,
			/// <summary>
			/// Number of pens the device has
			/// </summary>
			NUMPENS = 18,
			/// <summary>
			/// Number of markers the device has
			/// </summary>
			NUMMARKERS = 20,
			/// <summary>
			/// Number of fonts the device has
			/// </summary>
			NUMFONTS = 22,
			/// <summary>
			/// Number of colors the device supports
			/// </summary>
			NUMCOLORS = 24,
			/// <summary>
			/// Size required for device descriptor
			/// </summary>
			PDEVICESIZE = 26,
			/// <summary>
			/// Curve capabilities
			/// </summary>
			CURVECAPS = 28,
			/// <summary>
			/// Line capabilities
			/// </summary>
			LINECAPS = 30,
			/// <summary>
			/// Polygonal capabilities
			/// </summary>
			POLYGONALCAPS = 32,
			/// <summary>
			/// Text capabilities
			/// </summary>
			TEXTCAPS = 34,
			/// <summary>
			/// Clipping capabilities
			/// </summary>
			CLIPCAPS = 36,
			/// <summary>
			/// Bitblt capabilities
			/// </summary>
			RASTERCAPS = 38,
			/// <summary>
			/// Length of the X leg
			/// </summary>
			ASPECTX = 40,
			/// <summary>
			/// Length of the Y leg
			/// </summary>
			ASPECTY = 42,
			/// <summary>
			/// Length of the hypotenuse
			/// </summary>
			ASPECTXY = 44,
			/// <summary>
			/// Shading and Blending caps
			/// </summary>
			SHADEBLENDCAPS = 45,

			/// <summary>
			/// Logical pixels inch in X
			/// </summary>
			LOGPIXELSX = 88,
			/// <summary>
			/// Logical pixels inch in Y
			/// </summary>
			LOGPIXELSY = 90,

			/// <summary>
			/// Number of entries in physical palette
			/// </summary>
			SIZEPALETTE = 104,
			/// <summary>
			/// Number of reserved entries in palette
			/// </summary>
			NUMRESERVED = 106,
			/// <summary>
			/// Actual color resolution
			/// </summary>
			COLORRES = 108,

			// Printing related DeviceCaps. These replace the appropriate Escapes
			/// <summary>
			/// Physical Width in device units
			/// </summary>
			PHYSICALWIDTH = 110,
			/// <summary>
			/// Physical Height in device units
			/// </summary>
			PHYSICALHEIGHT = 111,
			/// <summary>
			/// Physical Printable Area x margin
			/// </summary>
			PHYSICALOFFSETX = 112,
			/// <summary>
			/// Physical Printable Area y margin
			/// </summary>
			PHYSICALOFFSETY = 113,
			/// <summary>
			/// Scaling factor x
			/// </summary>
			SCALINGFACTORX = 114,
			/// <summary>
			/// Scaling factor y
			/// </summary>
			SCALINGFACTORY = 115,

			/// <summary>
			/// Current vertical refresh rate of the display device (for displays only) in Hz
			/// </summary>
			VREFRESH = 116,
			/// <summary>
			/// Vertical height of entire desktop in pixels
			/// </summary>
			DESKTOPVERTRES = 117,
			/// <summary>
			/// Horizontal width of entire desktop in pixels
			/// </summary>
			DESKTOPHORZRES = 118,
			/// <summary>
			/// Preferred blt alignment
			/// </summary>
			BLTALIGNMENT = 119
		}

		[DllImport("gdi32.dll")]
		static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		public static int GetDeviceCaps(DeviceCap cap)
		{
			using (var g = Graphics.FromHwnd(IntPtr.Zero))
			{
				//Console.WriteLine("Graphics.DpiX: " + g.DpiX);
				//Console.WriteLine("Graphics.DpiY: " + g.DpiY);
				//Console.WriteLine();

				//var hdc = new HandleRef(g, g.GetHdc());
				IntPtr hdc = g.GetHdc();
				int result = GetDeviceCaps(hdc, (int)cap);
				g.ReleaseHdc();
				return result;
			}
		}
	}
}
