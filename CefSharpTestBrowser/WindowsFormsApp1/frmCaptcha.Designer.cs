namespace CSTool
{
    partial class frmCaptcha
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.imgCaptcha = new System.Windows.Forms.PictureBox();
            this.imgProgress = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgCaptcha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgProgress)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(584, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 69);
            this.button1.TabIndex = 1;
            this.button1.Text = "clockwise";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(84, 176);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 69);
            this.button2.TabIndex = 2;
            this.button2.Text = "counter clockwise";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // imgCaptcha
            // 
            this.imgCaptcha.BackColor = System.Drawing.Color.Transparent;
            this.imgCaptcha.Image = global::CSTool.Properties.Resources.test;
            this.imgCaptcha.InitialImage = null;
            this.imgCaptcha.Location = new System.Drawing.Point(273, 228);
            this.imgCaptcha.Name = "imgCaptcha";
            this.imgCaptcha.Size = new System.Drawing.Size(250, 250);
            this.imgCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCaptcha.TabIndex = 0;
            this.imgCaptcha.TabStop = false;
            // 
            // imgProgress
            // 
            this.imgProgress.BackColor = System.Drawing.Color.Transparent;
            this.imgProgress.Cursor = System.Windows.Forms.Cursors.Default;
            this.imgProgress.Location = new System.Drawing.Point(261, 215);
            this.imgProgress.Name = "imgProgress";
            this.imgProgress.Size = new System.Drawing.Size(275, 275);
            this.imgProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgProgress.TabIndex = 3;
            this.imgProgress.TabStop = false;
            // 
            // frmCaptcha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(810, 626);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.imgCaptcha);
            this.Controls.Add(this.imgProgress);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCaptcha";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.imgCaptcha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgProgress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgCaptcha;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox imgProgress;
    }
}