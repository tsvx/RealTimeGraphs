namespace TestShifts
{
	partial class ShiftForm
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
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.secondTimer = new System.Windows.Forms.Timer(this.components);
			this.shiftTimer = new System.Windows.Forms.Timer(this.components);
			this.shiftedControl = new TestShifts.ShiftedControl();
			this.SuspendLayout();
			// 
			// secondTimer
			// 
			this.secondTimer.Enabled = true;
			this.secondTimer.Interval = 1000;
			this.secondTimer.Tick += new System.EventHandler(this.secondTimer_Tick);
			// 
			// shiftTimer
			// 
			this.shiftTimer.Enabled = true;
			this.shiftTimer.Interval = 15;
			this.shiftTimer.Tick += new System.EventHandler(this.shiftTimer_Tick);
			// 
			// shiftedControl
			// 
			this.shiftedControl.BackColor = System.Drawing.Color.Black;
			this.shiftedControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.shiftedControl.ForeColor = System.Drawing.Color.LawnGreen;
			this.shiftedControl.Location = new System.Drawing.Point(0, 0);
			this.shiftedControl.MinimumSize = new System.Drawing.Size(1, 1);
			this.shiftedControl.Name = "shiftedControl";
			this.shiftedControl.Size = new System.Drawing.Size(972, 518);
			this.shiftedControl.TabIndex = 0;
			this.shiftedControl.Text = "shiftedControl";
			// 
			// ShiftForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(972, 518);
			this.Controls.Add(this.shiftedControl);
			this.Name = "ShiftForm";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);

		}

		#endregion

		private ShiftedControl shiftedControl;
		private System.Windows.Forms.Timer secondTimer;
		private System.Windows.Forms.Timer shiftTimer;
	}
}

