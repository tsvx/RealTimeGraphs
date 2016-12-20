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
		public MovingGraphControl()
		{
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
		/// <param name="time">Time given.</param>
		public void Shift(long time)
		{
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}
	}
}
