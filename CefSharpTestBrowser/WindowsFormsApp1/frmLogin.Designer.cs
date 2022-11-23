namespace WindowsFormsApp1
{
    partial class frmLogin
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
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbUtype = new System.Windows.Forms.ComboBox();
            this.lblTier = new System.Windows.Forms.Label();
            this.cmbTierLevel = new System.Windows.Forms.ComboBox();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(26, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(31, 176);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(337, 29);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtEmail_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(31, 359);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(337, 39);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(31, 403);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(337, 39);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(0, 4);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(49, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "v 1.0.0.0";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(169)))), ((int)(((byte)(23)))));
            this.pnlFooter.Controls.Add(this.lblVersion);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 472);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(396, 22);
            this.pnlFooter.TabIndex = 6;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::CSTool.Properties.Resources.refresh;
            this.pbLogo.Location = new System.Drawing.Point(95, 11);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(212, 135);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 5;
            this.pbLogo.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(26, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "User Type";
            // 
            // cmbUtype
            // 
            this.cmbUtype.BackColor = System.Drawing.Color.White;
            this.cmbUtype.DisplayMember = "DS, MS, NS";
            this.cmbUtype.DropDownHeight = 130;
            this.cmbUtype.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbUtype.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbUtype.FormattingEnabled = true;
            this.cmbUtype.IntegralHeight = false;
            this.cmbUtype.ItemHeight = 21;
            this.cmbUtype.Items.AddRange(new object[] {
            "Agent",
            "Customer Service QA",
            "Trainee"});
            this.cmbUtype.Location = new System.Drawing.Point(31, 247);
            this.cmbUtype.MaxDropDownItems = 3;
            this.cmbUtype.Name = "cmbUtype";
            this.cmbUtype.Size = new System.Drawing.Size(337, 29);
            this.cmbUtype.TabIndex = 9;
            this.cmbUtype.Text = "Agent";
            this.cmbUtype.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbUtype_KeyDown);
            // 
            // lblTier
            // 
            this.lblTier.AutoSize = true;
            this.lblTier.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTier.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblTier.Location = new System.Drawing.Point(26, 281);
            this.lblTier.Name = "lblTier";
            this.lblTier.Size = new System.Drawing.Size(92, 25);
            this.lblTier.TabIndex = 12;
            this.lblTier.Text = "Tier Level";
            // 
            // cmbTierLevel
            // 
            this.cmbTierLevel.BackColor = System.Drawing.Color.White;
            this.cmbTierLevel.DisplayMember = "DS, MS, NS";
            this.cmbTierLevel.DropDownHeight = 130;
            this.cmbTierLevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbTierLevel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbTierLevel.FormattingEnabled = true;
            this.cmbTierLevel.IntegralHeight = false;
            this.cmbTierLevel.ItemHeight = 21;
            this.cmbTierLevel.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbTierLevel.Location = new System.Drawing.Point(31, 308);
            this.cmbTierLevel.MaxDropDownItems = 3;
            this.cmbTierLevel.Name = "cmbTierLevel";
            this.cmbTierLevel.Size = new System.Drawing.Size(337, 29);
            this.cmbTierLevel.TabIndex = 11;
            this.cmbTierLevel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbTierLevel_KeyDown);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(396, 494);
            this.Controls.Add(this.lblTier);
            this.Controls.Add(this.cmbTierLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbUtype);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CS Tool | Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLogin_FormClosing);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbUtype;
        private System.Windows.Forms.Label lblTier;
        private System.Windows.Forms.ComboBox cmbTierLevel;
    }
}