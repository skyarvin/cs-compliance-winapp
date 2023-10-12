namespace CSTool
{
    partial class frmTfa
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
            this.tfa_code = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.device_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.submit_tfa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tfa_code
            // 
            this.tfa_code.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tfa_code.Location = new System.Drawing.Point(23, 185);
            this.tfa_code.Name = "tfa_code";
            this.tfa_code.Size = new System.Drawing.Size(331, 26);
            this.tfa_code.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // device_label
            // 
            this.device_label.AutoSize = true;
            this.device_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_label.ForeColor = System.Drawing.Color.Black;
            this.device_label.Location = new System.Drawing.Point(26, 127);
            this.device_label.Name = "device_label";
            this.device_label.Size = new System.Drawing.Size(0, 20);
            this.device_label.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(334, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enter 6-digit code from your Two Factor Authenticator Device";
            // 
            // submit_tfa
            // 
            this.submit_tfa.BackColor = System.Drawing.SystemColors.Highlight;
            this.submit_tfa.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.submit_tfa.Location = new System.Drawing.Point(23, 226);
            this.submit_tfa.Name = "submit_tfa";
            this.submit_tfa.Size = new System.Drawing.Size(331, 36);
            this.submit_tfa.TabIndex = 4;
            this.submit_tfa.Text = "Submit";
            this.submit_tfa.UseVisualStyleBackColor = false;
            this.submit_tfa.Click += new System.EventHandler(this.submit_tfa_Click);
            // 
            // frmTfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 388);
            this.Controls.Add(this.submit_tfa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.device_label);
            this.Controls.Add(this.tfa_code);
            this.Name = "frmTfa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Two Factor Auth";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTfa_FormClosing);
            this.Load += new System.EventHandler(this.frmTfa_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tfa_code;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label device_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button submit_tfa;
    }
}