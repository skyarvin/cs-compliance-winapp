namespace WindowsFormsApp1
{
    partial class frmSendInternalRequestReview
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
            this.btnSendRR = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbViolation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnSendRR
            // 
            this.btnSendRR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSendRR.ForeColor = System.Drawing.Color.White;
            this.btnSendRR.Location = new System.Drawing.Point(475, 234);
            this.btnSendRR.Name = "btnSendRR";
            this.btnSendRR.Size = new System.Drawing.Size(101, 29);
            this.btnSendRR.TabIndex = 1;
            this.btnSendRR.Text = "Ok";
            this.btnSendRR.UseVisualStyleBackColor = false;
            this.btnSendRR.Click += new System.EventHandler(this.btnSendRR_Click);
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
            this.lblUrl.Location = new System.Drawing.Point(109, 25);
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
            this.label2.Location = new System.Drawing.Point(12, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 29);
            this.label2.TabIndex = 6;
            this.label2.Text = "Notes";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // txtNotes
            // 
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtNotes.Location = new System.Drawing.Point(109, 104);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNotes.Size = new System.Drawing.Size(467, 110);
            this.txtNotes.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 29);
            this.label4.TabIndex = 22;
            this.label4.Text = "Violation:";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // cmbViolation
            // 
            this.cmbViolation.AllowDrop = true;
            this.cmbViolation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbViolation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbViolation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViolation.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbViolation.FormattingEnabled = true;
            this.cmbViolation.Location = new System.Drawing.Point(109, 57);
            this.cmbViolation.Name = "cmbViolation";
            this.cmbViolation.Size = new System.Drawing.Size(467, 25);
            this.cmbViolation.TabIndex = 21;
            // 
            // chkSkypeCompliance
            // 
            this.chkSkypeCompliance.AutoSize = true;
            this.chkSkypeCompliance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSkypeCompliance.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.chkSkypeCompliance.Location = new System.Drawing.Point(111, 228);
            this.chkSkypeCompliance.Name = "chkSkypeCompliance";
            this.chkSkypeCompliance.Size = new System.Drawing.Size(157, 25);
            this.chkSkypeCompliance.TabIndex = 24;
            this.chkSkypeCompliance.Text = "Skype Compliance";
            this.chkSkypeCompliance.UseVisualStyleBackColor = true;
            this.chkSkypeCompliance.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // frmSendInternalRequestReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.ClientSize = new System.Drawing.Size(592, 281);
            this.Controls.Add(this.chkSkypeCompliance);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbViolation);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendRR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSendInternalRequestReview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Internal Request Review";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSendInternalRequestReview_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSendRR;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblUrl;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNotes;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbViolation;
        private System.Windows.Forms.CheckBox chkSkypeCompliance;
    }
}