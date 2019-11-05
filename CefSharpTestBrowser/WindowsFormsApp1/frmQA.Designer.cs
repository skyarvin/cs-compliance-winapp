namespace WindowsFormsApp1
{
    partial class frmQA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQA));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblChatEnd = new System.Windows.Forms.Label();
            this.lblChatStart = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pbImg = new System.Windows.Forms.PictureBox();
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.pnlURL.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.pnlHeader.Controls.Add(this.pnlURL);
            this.pnlHeader.Controls.Add(this.panel1);
            this.pnlHeader.Controls.Add(this.pnlUser);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(875, 45);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlURL
            // 
            this.pnlURL.Controls.Add(this.label1);
            this.pnlURL.Controls.Add(this.txtUrl);
            this.pnlURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlURL.Location = new System.Drawing.Point(0, 0);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(438, 45);
            this.pnlURL.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtUrl.Location = new System.Drawing.Point(47, 11);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(376, 25);
            this.txtUrl.TabIndex = 1;
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(169)))), ((int)(((byte)(23)))));
            this.panel1.Controls.Add(this.lblChatEnd);
            this.panel1.Controls.Add(this.lblChatStart);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(438, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 45);
            this.panel1.TabIndex = 3;
            // 
            // lblChatEnd
            // 
            this.lblChatEnd.BackColor = System.Drawing.Color.Transparent;
            this.lblChatEnd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold);
            this.lblChatEnd.ForeColor = System.Drawing.Color.White;
            this.lblChatEnd.Location = new System.Drawing.Point(71, 21);
            this.lblChatEnd.Name = "lblChatEnd";
            this.lblChatEnd.Size = new System.Drawing.Size(139, 22);
            this.lblChatEnd.TabIndex = 12;
            this.lblChatEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblChatStart
            // 
            this.lblChatStart.BackColor = System.Drawing.Color.Transparent;
            this.lblChatStart.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold);
            this.lblChatStart.ForeColor = System.Drawing.Color.White;
            this.lblChatStart.Location = new System.Drawing.Point(71, 1);
            this.lblChatStart.Name = "lblChatStart";
            this.lblChatStart.Size = new System.Drawing.Size(139, 22);
            this.lblChatStart.TabIndex = 11;
            this.lblChatStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 22);
            this.label3.TabIndex = 10;
            this.label3.Text = "Chat End:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 22);
            this.label2.TabIndex = 9;
            this.label2.Text = "Chat Start:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUser
            // 
            this.pnlUser.BackColor = System.Drawing.Color.Transparent;
            this.pnlUser.Controls.Add(this.lblProfile);
            this.pnlUser.Controls.Add(this.lblUser);
            this.pnlUser.Controls.Add(this.pbImg);
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUser.Location = new System.Drawing.Point(653, 0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(222, 45);
            this.pnlUser.TabIndex = 1;
            // 
            // lblProfile
            // 
            this.lblProfile.BackColor = System.Drawing.Color.Transparent;
            this.lblProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfile.Font = new System.Drawing.Font("Segoe UI Light", 9.25F);
            this.lblProfile.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lblProfile.Location = new System.Drawing.Point(0, 22);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(177, 23);
            this.lblProfile.TabIndex = 9;
            this.lblProfile.Text = "Profile: QA";
            this.lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUser
            // 
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(0, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(177, 22);
            this.lblUser.TabIndex = 8;
            this.lblUser.Text = "Enrique";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbImg
            // 
            this.pbImg.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbImg.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pbImg.ErrorImage")));
            this.pbImg.Image = ((System.Drawing.Image)(resources.GetObject("pbImg.Image")));
            this.pbImg.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbImg.InitialImage")));
            this.pbImg.Location = new System.Drawing.Point(177, 0);
            this.pbImg.Name = "pbImg";
            this.pbImg.Size = new System.Drawing.Size(45, 45);
            this.pbImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImg.TabIndex = 7;
            this.pbImg.TabStop = false;
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBrowser.Location = new System.Drawing.Point(0, 45);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(875, 405);
            this.pnlBrowser.TabIndex = 1;
            // 
            // frmQA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 450);
            this.Controls.Add(this.pnlBrowser);
            this.Controls.Add(this.pnlHeader);
            this.Name = "frmQA";
            this.Text = "Skydev Browser | QA";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlHeader.ResumeLayout(false);
            this.pnlURL.ResumeLayout(false);
            this.pnlURL.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.PictureBox pbImg;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlBrowser;
        private System.Windows.Forms.Label lblChatEnd;
        private System.Windows.Forms.Label lblChatStart;
    }
}