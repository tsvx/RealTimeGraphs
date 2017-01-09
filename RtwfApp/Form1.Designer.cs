namespace RtwfApp
{
	partial class Form1
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
			this.movingGraphControl1 = new RtwfApp.MovingGraphControl();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// movingGraphControl1
			// 
			this.movingGraphControl1.BackColor = System.Drawing.Color.DarkGreen;
			this.movingGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.movingGraphControl1.Location = new System.Drawing.Point(0, 0);
			this.movingGraphControl1.Name = "movingGraphControl1";
			this.movingGraphControl1.Size = new System.Drawing.Size(945, 449);
			this.movingGraphControl1.TabIndex = 0;
			this.movingGraphControl1.Text = "movingGraphControl1";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// timer2
			// 
			this.timer2.Enabled = true;
			this.timer2.Interval = 1000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(945, 449);
			this.Controls.Add(this.movingGraphControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private MovingGraphControl movingGraphControl1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Timer timer2;
	}
}

