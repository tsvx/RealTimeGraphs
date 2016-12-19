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
			this.BackColor = Color.DarkGreen;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}
	}
}
