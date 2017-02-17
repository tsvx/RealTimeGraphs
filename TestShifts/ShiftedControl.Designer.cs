namespace TestShifts
{
	partial class ShiftedControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
				if (timer != null)
				{
					timer.Dispose();
					timer = null;
				}
				//if (painter != null)
				//	painter.Dispose();
				if (viewport != null)
					viewport.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ShiftedControl
			// 
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ShiftedControl_MouseClick);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ShiftedControl_MouseClick);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
