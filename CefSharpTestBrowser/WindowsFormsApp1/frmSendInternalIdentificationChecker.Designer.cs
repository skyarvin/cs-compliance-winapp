namespace WindowsFormsApp1
{
    partial class frmSendInternalIdentificationChecker
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
            this.btnSendIIDC = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.uploadbtn = new System.Windows.Forms.Button();
            this.upload_photo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSendIIDC
            // 
            this.btnSendIIDC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSendIIDC.ForeColor = System.Drawing.Color.White;
            this.btnSendIIDC.Location = new System.Drawing.Point(475, 210);
            this.btnSendIIDC.Name = "btnSendIIDC";
            this.btnSendIIDC.Size = new System.Drawing.Size(101, 24);
            this.btnSendIIDC.TabIndex = 1;
            this.btnSendIIDC.Text = "Ok";
            this.btnSendIIDC.UseVisualStyleBackColor = false;
            this.btnSendIIDC.Click += new System.EventHandler(this.btnSendIIDC_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Url:";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // lblUrl
            // 
            this.lblUrl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUrl.ForeColor = System.Drawing.Color.White;
            this.lblUrl.Location = new System.Drawing.Point(108, 25);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(467, 29);
            this.lblUrl.TabIndex = 5;
            this.lblUrl.Text = "http://localhost:8080/cb/compliance/show/?id=1580348097#photos";
            this.lblUrl.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Notes:";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // txtNotes
            // 
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtNotes.Location = new System.Drawing.Point(109, 90);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNotes.Size = new System.Drawing.Size(467, 105);
            this.txtNotes.TabIndex = 7;
            // 
            // uploadbtn
            // 
            this.uploadbtn.Location = new System.Drawing.Point(12, 54);
            this.uploadbtn.Name = "uploadbtn";
            this.uploadbtn.Size = new System.Drawing.Size(91, 21);
            this.uploadbtn.TabIndex = 25;
            this.uploadbtn.Text = "Upload Image";
            this.uploadbtn.UseVisualStyleBackColor = true;
            this.uploadbtn.Click += new System.EventHandler(this.uploadbtn_Click);
            // 
            // upload_photo
            // 
            this.upload_photo.Location = new System.Drawing.Point(109, 55);
            this.upload_photo.Name = "upload_photo";
            this.upload_photo.ReadOnly = true;
            this.upload_photo.Size = new System.Drawing.Size(467, 20);
            this.upload_photo.TabIndex = 28;
            // 
            // frmSendInternalIdentificationChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.ClientSize = new System.Drawing.Size(592, 254);
            this.Controls.Add(this.upload_photo);
            this.Controls.Add(this.uploadbtn);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendIIDC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSendInternalIdentificationChecker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Internal Identification Checker";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSendInternalIdentificationChecker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnSendIIDC;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblUrl;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button uploadbtn;
        private System.Windows.Forms.TextBox upload_photo;
    }
}