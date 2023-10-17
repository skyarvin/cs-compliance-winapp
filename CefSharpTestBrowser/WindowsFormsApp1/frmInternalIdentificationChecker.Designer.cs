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
            this.label7 = new System.Windows.Forms.Label();
            this.lblReviewerPhotoGcsUrl = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 56);
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
            this.txtNotes.Location = new System.Drawing.Point(68, 53);
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
            this.label4.Location = new System.Drawing.Point(7, 174);
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
            this.txtReviewerNotes.Location = new System.Drawing.Point(68, 173);
            this.txtReviewerNotes.Multiline = true;
            this.txtReviewerNotes.Name = "txtReviewerNotes";
            this.txtReviewerNotes.ReadOnly = true;
            this.txtReviewerNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReviewerNotes.Size = new System.Drawing.Size(415, 91);
            this.txtReviewerNotes.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(65, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(264, 29);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "PENDING";
            this.lblStatus.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 29);
            this.label3.TabIndex = 12;
            this.label3.Text = "Status:";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(6, 282);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Reviewer Photo Violation:";
            // 
            // lblReviewerPhotoGcsUrl
            // 
            this.lblReviewerPhotoGcsUrl.AutoSize = true;
            this.lblReviewerPhotoGcsUrl.Location = new System.Drawing.Point(141, 282);
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
            this.ClientSize = new System.Drawing.Size(501, 315);
            this.Controls.Add(this.lblReviewerPhotoGcsUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtReviewerNotes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInternalIdentificationChecker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Internal Identification Checker";
            this.TopMost = true;
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel lblReviewerPhotoGcsUrl;
    }
}