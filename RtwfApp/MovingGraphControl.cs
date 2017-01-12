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
	public partial class MovingGraphControl : Control
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
			prevTime = curTime = -1;
			InitializeComponent();
			this.SetStyle(
				ControlStyles.OptimizedDoubleBuffer |
				//ControlStyles.Opaque |
				ControlStyles.ResizeRedraw |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint,
				true);
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

		protected override void OnPaint(PaintEventArgs pe)
		{
			var g = pe.Graphics;

			var gc = Preamble(g);
			DrawAll(g);
			g.EndContainer(gc);

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

			//g.Clear(this.BackColor);

			return gc;
		}

		void DrawAll(Graphics g)
		{
			RectangleF r = this.ClientRectangle;
			long leftTime = (long)(curTime - r.Width * timeScale);
			// Draw from leftTime to curTime;
			// 

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
				g.DrawLines(SystemPens.WindowText, points.ToArray());
			}
		}
	}
}
