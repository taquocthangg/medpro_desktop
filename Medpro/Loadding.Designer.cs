namespace Login
{
    partial class Loadding
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ProgressIndicator1 = new Guna.UI2.WinForms.Guna2ProgressIndicator();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("iCiel Pequena", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Location = new System.Drawing.Point(128, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 50);
            this.label1.TabIndex = 5;
            this.label1.Tag = "v";
            this.label1.Text = "Vui lòng chờ ...";
            // 
            // guna2ProgressIndicator1
            // 
            this.guna2ProgressIndicator1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.guna2ProgressIndicator1.AutoStart = true;
            this.guna2ProgressIndicator1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ProgressIndicator1.Location = new System.Drawing.Point(170, 144);
            this.guna2ProgressIndicator1.Name = "guna2ProgressIndicator1";
            this.guna2ProgressIndicator1.ProgressColor = System.Drawing.Color.DeepSkyBlue;
            this.guna2ProgressIndicator1.Size = new System.Drawing.Size(143, 139);
            this.guna2ProgressIndicator1.TabIndex = 8;
            this.guna2ProgressIndicator1.UseTransparentBackground = true;
            // 
            // Loadding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.guna2ProgressIndicator1);
            this.Controls.Add(this.label1);
            this.Name = "Loadding";
            this.Size = new System.Drawing.Size(500, 500);
            this.Load += new System.EventHandler(this.Loadding_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ProgressIndicator guna2ProgressIndicator1;
    }
}
