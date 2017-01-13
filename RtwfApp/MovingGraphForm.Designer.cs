namespace RtwfApp
{
	partial class MovingGraphForm
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
			this.frameTimer = new System.Windows.Forms.Timer(this.components);
			this.secondTimer = new System.Windows.Forms.Timer(this.components);
			this.movingGraphControl = new RtwfApp.MovingGraphControl();
			this.SuspendLayout();
			// 
			// frameTimer
			// 
			this.frameTimer.Enabled = true;
			this.frameTimer.Interval = 15;
			this.frameTimer.Tick += new System.EventHandler(this.frameTimer_Tick);
			// 
			// secondTimer
			// 
			this.secondTimer.Enabled = true;
			this.secondTimer.Interval = 1000;
			this.secondTimer.Tick += new System.EventHandler(this.secondTimer_Tick);
			// 
			// movingGraphControl
			// 
			this.movingGraphControl.BackColor = System.Drawing.Color.MintCream;
			this.movingGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.movingGraphControl.Location = new System.Drawing.Point(0, 0);
			this.movingGraphControl.MinimumSize = new System.Drawing.Size(1, 1);
			this.movingGraphControl.Name = "movingGraphControl";
			this.movingGraphControl.Size = new System.Drawing.Size(945, 449);
			this.movingGraphControl.TabIndex = 0;
			this.movingGraphControl.Text = "movingGraphControl";
			// 
			// MovingGraphForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(945, 449);
			this.Controls.Add(this.movingGraphControl);
			this.Name = "MovingGraphForm";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private MovingGraphControl movingGraphControl;
		private System.Windows.Forms.Timer frameTimer;
		private System.Windows.Forms.Timer secondTimer;
	}
}

