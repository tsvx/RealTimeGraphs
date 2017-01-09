using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RtwfApp
{
	public partial class MovingGraphControl : Control
	{
		long prevTime, curTime; // 100ns-ticks
		const double timeScale = 500 * 25400 / 96.0; // ticks-per-pixel, = 500 ms/cm

		public int FramesNum { get; private set; }

		public MovingGraphControl()
		{
			prevTime = curTime = -1;
			InitializeComponent();
			this.SetStyle(
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.Opaque |
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
			DrawAll(pe.Graphics);
			prevTime = curTime;
			FramesNum++;
		}

		void DrawAll(Graphics g)
		{
		}
	}
}
