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
			this.movingGraphControl1 = new RtwfApp.MovingGraphControl();
			this.SuspendLayout();
			// 
			// movingGraphControl1
			// 
			this.movingGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.movingGraphControl1.Location = new System.Drawing.Point(0, 0);
			this.movingGraphControl1.Name = "movingGraphControl1";
			this.movingGraphControl1.Size = new System.Drawing.Size(945, 449);
			this.movingGraphControl1.TabIndex = 0;
			this.movingGraphControl1.Text = "movingGraphControl1";
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
	}
}

