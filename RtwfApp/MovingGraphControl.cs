using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RtwfApp
{
	public partial class MovingGraphControl : RazorGDIPainter.RazorPainterControl
	{
		/// <summary>
		/// 100ns ticks
		/// </summary>
		long prevTime, curTime;

		/// <summary>
		/// ticks-per-pixel, = 500 ms/cm
		/// </summary>
		const double timeScale = 500 * 25400 / 96.0;

		public int FramesNum { get; private set; }

		public IData Data;

		public MovingGraphControl()
		{
			prevTime = curTime = long.MinValue;
			InitializeComponent();
		}

		/// <summary>
		/// Set current time to given one.
		/// </summary>
		/// <param name="time">Time given in 100ns-ticks.</param>
		public void Shift(long time)
		{
			curTime = time;
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			prevTime = long.MinValue;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			lock (this.RazorLock)
			{
				var g = this.RazorGFX;

				var gc = Preamble(g);

				if (Data != null && curTime != long.MinValue)
				{
					if (prevTime == long.MinValue)
						DrawAll(g);
					else
					{
						float dx = (float)((curTime - prevTime) / timeScale);
						//var bmp = RazorBMP.Clone();
						//RazorGFX.DrawImage(RazorBMP, -dx, 0);
					}
				}

				g.EndContainer(gc);

				this.RazorPaint();
				prevTime = long.MinValue;
			}

			prevTime = curTime;
			FramesNum++;
		}

		GraphicsContainer Preamble(Graphics g)
		{
			RectangleF r = this.ClientRectangle;
			var gc = g.BeginContainer();
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			g.SetClip(r);

			return gc;
		}

		void DrawAll(Graphics g)
		{
			RectangleF r = this.ClientRectangle;
			long leftTime = (long)(curTime - r.Width * timeScale);

			g.Clear(this.BackColor);

			var points = new List<PointF>();
			for (int j = 0; j < Data.NumGraphs; j++)
			{
				points.Clear();
				foreach (var p in Data.GetInterval(j, leftTime, curTime))
				{
					float x = r.Left + (float)((p.Key - leftTime) / timeScale);
					float y = (float)(r.Bottom - p.Value * r.Height);
					points.Add(new PointF(x, y));
				}
				if (points.Count > 0)
					g.DrawLines(SystemPens.WindowText, points.ToArray());
			}
		}
	}
}
