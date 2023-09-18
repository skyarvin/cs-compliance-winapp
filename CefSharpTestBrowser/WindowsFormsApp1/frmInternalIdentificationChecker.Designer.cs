namespace WindowsFormsApp1
{
    partial class frmInternalIdentificationChecker
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReviewerNotes = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblphoto = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblReviewerPhotoGcsUrl = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 29);
            this.label2.TabIndex = 6;
            this.label2.Text = "Notes:";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.SystemColors.Window;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(68, 112);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNotes.Size = new System.Drawing.Size(415, 100);
            this.txtNotes.TabIndex = 7;
            this.txtNotes.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(7, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 52);
            this.label4.TabIndex = 9;
            this.label4.Text = "Reviewer Notes:";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // txtReviewerNotes
            // 
            this.txtReviewerNotes.BackColor = System.Drawing.SystemColors.Window;
            this.txtReviewerNotes.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.txtReviewerNotes.Location = new System.Drawing.Point(68, 232);
            this.txtReviewerNotes.Multiline = true;
            this.txtReviewerNotes.Name = "txtReviewerNotes";
            this.txtReviewerNotes.ReadOnly = true;
            this.txtReviewerNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReviewerNotes.Size = new System.Drawing.Size(415, 91);
            this.txtReviewerNotes.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(68, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(264, 29);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "PENDING";
            this.lblStatus.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 29);
            this.label3.TabIndex = 12;
            this.label3.Text = "Status:";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Url:";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // lblUrl
            // 
            this.lblUrl.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblUrl.ForeColor = System.Drawing.Color.White;
            this.lblUrl.Location = new System.Drawing.Point(68, 31);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(415, 29);
            this.lblUrl.TabIndex = 5;
            this.lblUrl.Text = "-";
            this.lblUrl.UseCompatibleTextRendering = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.LightCyan;
            this.linkLabel1.Location = new System.Drawing.Point(481, 3);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(16, 16);
            this.linkLabel1.TabIndex = 14;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "X";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Photo:";
            // 
            // lblphoto
            // 
            this.lblphoto.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblphoto.ForeColor = System.Drawing.Color.White;
            this.lblphoto.Location = new System.Drawing.Point(68, 80);
            this.lblphoto.Name = "lblphoto";
            this.lblphoto.Size = new System.Drawing.Size(415, 29);
            this.lblphoto.TabIndex = 24;
            this.lblphoto.Text = "-";
            this.lblphoto.UseCompatibleTextRendering = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(4, 335);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Reviewer Photo Violation:";
            // 
            // lblReviewerPhotoGcsUrl
            // 
            this.lblReviewerPhotoGcsUrl.AutoSize = true;
            this.lblReviewerPhotoGcsUrl.Location = new System.Drawing.Point(139, 335);
            this.lblReviewerPhotoGcsUrl.Name = "lblReviewerPhotoGcsUrl";
            this.lblReviewerPhotoGcsUrl.Size = new System.Drawing.Size(98, 13);
            this.lblReviewerPhotoGcsUrl.TabIndex = 26;
            this.lblReviewerPhotoGcsUrl.TabStop = true;
            this.lblReviewerPhotoGcsUrl.Text = "Click to view image";
            this.lblReviewerPhotoGcsUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblReviewerPhotoUrl_LinkClicked);
            // 
            // frmInternalIdentificationChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(501, 372);
            this.Controls.Add(this.lblReviewerPhotoGcsUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblphoto);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtReviewerNotes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInternalIdentificationChecker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Internal Identification Checker";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmInternalIdentificationChecker_Load);
            this.SizeChanged += new System.EventHandler(this.frmInternalIdentificationChecker_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNotes;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReviewerNotes;
        public System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lblphoto;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel lblReviewerPhotoGcsUrl;
    }
}