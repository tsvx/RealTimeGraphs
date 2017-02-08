using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestShifts
{
	interface IPainter : IDisposable
	{
		void Assign(Graphics gfx, Rectangle rect, Bitmap bmp);
		void PlaceBitmap(int bmpX);
	}
}
