namespace CSTool
{
    partial class frmSendInternalRequestFacePhoto
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

        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(475, 124);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 29);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Yes";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            this.label1.Visible = false;
            // 
            // lblUrl
            // 
            this.lblUrl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUrl.ForeColor = System.Drawing.Color.White;
            this.lblUrl.Location = new System.Drawing.Point(109, 25);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(467, 29);
            this.lblUrl.TabIndex = 5;
            this.lblUrl.Text = "http://localhost:8080/cb/compliance/show/?id=1580348097#photos";
            this.lblUrl.UseCompatibleTextRendering = true;
            this.lblUrl.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(40, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(518, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Are you sure you want to send Internal Request Face Photo?";
            // 
            // BtnNo
            // 
            this.BtnNo.BackColor = System.Drawing.Color.Red;
            this.BtnNo.ForeColor = System.Drawing.Color.White;
            this.BtnNo.Location = new System.Drawing.Point(364, 124);
            this.BtnNo.Name = "BtnNo";
            this.BtnNo.Size = new System.Drawing.Size(101, 29);
            this.BtnNo.TabIndex = 7;
            this.BtnNo.Text = "No";
            this.BtnNo.UseVisualStyleBackColor = false;
            this.BtnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // frmSendInternalRequestFacePhoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.ClientSize = new System.Drawing.Size(592, 159);
            this.Controls.Add(this.BtnNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSendInternalRequestFacePhoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Internal Request Face Photo";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmInternalRequestFacePhoto_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnNo;
    }
}