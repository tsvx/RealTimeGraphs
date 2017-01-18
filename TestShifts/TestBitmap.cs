using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace TestShifts
{
	class SinusData
	{
		public double Periods, Phase, Amplitude, Mean;
	}

	public class TestBitmap : IDisposable
	{
		public Bitmap Bitmap;
		Color cBack, cGraph;
		Pen pen;
		PointF[] points;

		static SinusData[] graphs = new SinusData[]
		{
			new SinusData{ Periods = 1, Amplitude = 0.9 } ,
			new SinusData{ Periods = 2, Amplitude = 0.7, Mean = 0.25, Phase = 2 },
			new SinusData{ Periods = 5, Amplitude = 0.4, Mean = 0.5, Phase = 3 },
			new SinusData{ Periods = 8, Amplitude = 0.4, Mean = -0.5, Phase = 4 },
		};

		public TestBitmap(Color backColor, Color graphColor)
		{
			cBack = backColor;
			cGraph = graphColor;
			pen = new Pen(cGraph, 2);
		}

		void DrawSinus(Graphics g, int width, Rectangle r, SinusData s)
		{
			for (int i = 0; i < r.Width; i++)
			{
				double x = (float)i / width * 2 * Math.PI + s.Phase;
				x *= s.Periods;
				double y = Math.Sin(x) * s.Amplitude + s.Mean;
				double j = r.Top + (1 - y) / 2 * r.Height;
				points[i] = new PointF(i + r.Left, (float)j);
			}
			g.DrawLines(pen, points);
		}

		public void Resize(int width, int height)
		{
			Bitmap = new Bitmap(width * 2, height);
			points = new PointF[2 * width];
			using (var g = Graphics.FromImage(Bitmap))
			{
				g.Clear(cBack);
				g.SmoothingMode = SmoothingMode.HighQuality;
				var r = new Rectangle(0, 0, 2 * width, height);
				foreach (var sinus in graphs)
					DrawSinus(g, width, r, sinus);
			}
		}

		public void Dispose()
		{
			if (Bitmap != null)
				Bitmap.Dispose();
			if (pen != null)
				pen.Dispose();
			points = null;
		}
	}
}
