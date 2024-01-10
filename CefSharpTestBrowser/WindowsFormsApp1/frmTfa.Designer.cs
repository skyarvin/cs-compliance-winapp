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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.device_label = new System.Windows.Forms.Label();
            this.subText = new System.Windows.Forms.Label();
            this.device_list = new System.Windows.Forms.ComboBox();
            this.submit_tfa = new System.Windows.Forms.Button();
            this.back_btn = new System.Windows.Forms.Button();
            this.tfa_code = new System.Windows.Forms.TextBox();
            this.resendCodeButton = new System.Windows.Forms.Button();
            this.resendCodeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // device_label
            // 
            this.device_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.device_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_label.ForeColor = System.Drawing.Color.Black;
            this.device_label.Location = new System.Drawing.Point(0, 0);
            this.device_label.Name = "device_label";
            this.device_label.Size = new System.Drawing.Size(370, 150);
            this.device_label.TabIndex = 2;
            this.device_label.Text = "Header1";
            this.device_label.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // subText
            // 
            this.subText.AutoSize = true;
            this.subText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subText.Location = new System.Drawing.Point(20, 150);
            this.subText.Name = "subText";
            this.subText.Size = new System.Drawing.Size(334, 15);
            this.subText.TabIndex = 3;
            this.subText.Text = "Enter 6-digit code from your Two Factor Authenticator Device";
            this.subText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // device_list
            // 
            this.device_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_list.FormattingEnabled = true;
            this.device_list.Location = new System.Drawing.Point(23, 185);
            this.device_list.Name = "device_list";
            this.device_list.Size = new System.Drawing.Size(331, 28);
            this.device_list.TabIndex = 4;
            this.device_list.SelectedIndexChanged += new System.EventHandler(this.device_list_SelectedIndexChanged);
            // 
            // submit_tfa
            // 
            this.submit_tfa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.submit_tfa.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.submit_tfa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.submit_tfa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submit_tfa.ForeColor = System.Drawing.Color.White;
            this.submit_tfa.Location = new System.Drawing.Point(23, 271);
            this.submit_tfa.Name = "submit_tfa";
            this.submit_tfa.Size = new System.Drawing.Size(331, 39);
            this.submit_tfa.TabIndex = 1;
            this.submit_tfa.Text = "Submit";
            this.submit_tfa.UseVisualStyleBackColor = false;
            this.submit_tfa.Click += new System.EventHandler(this.Submit_tfa_Click);
            // 
            // back_btn
            // 
            this.back_btn.BackColor = System.Drawing.Color.Gray;
            this.back_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.back_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.back_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.back_btn.ForeColor = System.Drawing.Color.White;
            this.back_btn.Location = new System.Drawing.Point(23, 364);
            this.back_btn.Name = "back_btn";
            this.back_btn.Size = new System.Drawing.Size(331, 39);
            this.back_btn.TabIndex = 2;
            this.back_btn.Text = "Back";
            this.back_btn.UseVisualStyleBackColor = false;
            this.back_btn.Click += new System.EventHandler(this.back_btn_Click);
            // 
            // tfa_code
            // 
            this.tfa_code.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tfa_code.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tfa_code.Location = new System.Drawing.Point(23, 223);
            this.tfa_code.Name = "tfa_code";
            this.tfa_code.Size = new System.Drawing.Size(331, 29);
            this.tfa_code.TabIndex = 0;
            this.tfa_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tfa_code_KeyDown);
            // 
            // resendCodeButton
            // 
            this.resendCodeButton.BackColor = System.Drawing.Color.Gray;
            this.resendCodeButton.Enabled = false;
            this.resendCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resendCodeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.resendCodeButton.ForeColor = System.Drawing.Color.White;
            this.resendCodeButton.Location = new System.Drawing.Point(23, 316);
            this.resendCodeButton.Name = "resendCodeButton";
            this.resendCodeButton.Size = new System.Drawing.Size(331, 39);
            this.resendCodeButton.TabIndex = 5;
            this.resendCodeButton.Text = "Resend code";
            this.resendCodeButton.UseVisualStyleBackColor = false;
            this.resendCodeButton.Click += new System.EventHandler(this.resendCodeButton_click);
            // 
            // resendCodeTimer
            // 
            this.resendCodeTimer.Interval = 1000;
            this.resendCodeTimer.Tick += new System.EventHandler(this.resendCodeTimer_Tick);
            // 
            // frmTfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 420);
            this.Controls.Add(this.resendCodeButton);
            this.Controls.Add(this.tfa_code);
            this.Controls.Add(this.back_btn);
            this.Controls.Add(this.submit_tfa);
            this.Controls.Add(this.device_list);
            this.Controls.Add(this.subText);
            this.Controls.Add(this.device_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmTfa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authenticator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTfa_FormClosing);
            this.Load += new System.EventHandler(this.frmTfa_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label device_label;
        private System.Windows.Forms.Label subText;
        private System.Windows.Forms.ComboBox device_list;
        private System.Windows.Forms.Button submit_tfa;
        private System.Windows.Forms.Button back_btn;
        private System.Windows.Forms.TextBox tfa_code;
        private System.Windows.Forms.Button resendCodeButton;
        private System.Windows.Forms.Timer resendCodeTimer;
    }
}