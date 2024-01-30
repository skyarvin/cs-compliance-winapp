namespace CSTool
{
    partial class frmRecoveryCode
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.recoveryCodeInput = new System.Windows.Forms.TextBox();
            this.submitRecoveryCodeButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 150);
            this.label1.TabIndex = 0;
            this.label1.Text = "Recovery Code";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(94, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter your account recovery code:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recoveryCodeInput
            // 
            this.recoveryCodeInput.AcceptsReturn = true;
            this.recoveryCodeInput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recoveryCodeInput.Location = new System.Drawing.Point(22, 206);
            this.recoveryCodeInput.Name = "recoveryCodeInput";
            this.recoveryCodeInput.Size = new System.Drawing.Size(331, 29);
            this.recoveryCodeInput.TabIndex = 2;
            // 
            // submitRecoveryCodeButton
            // 
            this.submitRecoveryCodeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.submitRecoveryCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.submitRecoveryCodeButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitRecoveryCodeButton.ForeColor = System.Drawing.Color.White;
            this.submitRecoveryCodeButton.Location = new System.Drawing.Point(22, 264);
            this.submitRecoveryCodeButton.Name = "submitRecoveryCodeButton";
            this.submitRecoveryCodeButton.Size = new System.Drawing.Size(331, 39);
            this.submitRecoveryCodeButton.TabIndex = 3;
            this.submitRecoveryCodeButton.Text = "Submit";
            this.submitRecoveryCodeButton.UseVisualStyleBackColor = false;
            this.submitRecoveryCodeButton.Click += new System.EventHandler(this.SubmitRecoveryCode);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Gray;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backButton.ForeColor = System.Drawing.Color.White;
            this.backButton.Location = new System.Drawing.Point(22, 313);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(331, 39);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.BackToAuthTfa);
            // 
            // frmRecoveryCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(370, 397);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.submitRecoveryCodeButton);
            this.Controls.Add(this.recoveryCodeInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "frmRecoveryCode";
            this.Text = "Use Recovery Code";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTfa_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox recoveryCodeInput;
        private System.Windows.Forms.Button submitRecoveryCodeButton;
        private System.Windows.Forms.Button backButton;
    }
}