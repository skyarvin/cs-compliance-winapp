namespace CSTool
{
    partial class frmRegisterDevice
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
            this.label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.registrationCode = new System.Windows.Forms.TextBox();
            this.submitCodeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Dock = System.Windows.Forms.DockStyle.Top;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label.ForeColor = System.Drawing.Color.Black;
            this.label.Location = new System.Drawing.Point(0, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(370, 150);
            this.label.TabIndex = 2;
            this.label.Text = "Register this machine";
            this.label.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(63, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please enter the code we sent to your email:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // registrationCode
            // 
            this.registrationCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.registrationCode.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.registrationCode.Location = new System.Drawing.Point(66, 212);
            this.registrationCode.Name = "registrationCode";
            this.registrationCode.Size = new System.Drawing.Size(244, 29);
            this.registrationCode.TabIndex = 4;
            // 
            // submitCodeButton
            // 
            this.submitCodeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.submitCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.submitCodeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.submitCodeButton.ForeColor = System.Drawing.Color.White;
            this.submitCodeButton.Location = new System.Drawing.Point(66, 275);
            this.submitCodeButton.Name = "submitCodeButton";
            this.submitCodeButton.Size = new System.Drawing.Size(244, 39);
            this.submitCodeButton.TabIndex = 5;
            this.submitCodeButton.Text = "Submit";
            this.submitCodeButton.UseVisualStyleBackColor = false;
            this.submitCodeButton.Click += new System.EventHandler(this.registerDevice);
            // 
            // frmRegisterDevice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(370, 376);
            this.Controls.Add(this.submitCodeButton);
            this.Controls.Add(this.registrationCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRegisterDevice";
            this.Text = "Register Device";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeForm);
            this.Load += new System.EventHandler(this.frmRegisterDevice_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox registrationCode;
        private System.Windows.Forms.Button submitCodeButton;
    }
}