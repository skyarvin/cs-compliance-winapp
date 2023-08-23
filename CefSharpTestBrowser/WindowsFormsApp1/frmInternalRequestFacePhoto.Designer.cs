namespace WindowsFormsApp1
{
    partial class frmInternalRequestFacePhoto
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.LinkLabel();
            this.reviewer_note_label = new System.Windows.Forms.Label();
            this.reviewer_note = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
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
            this.lblUrl.UseCompatibleTextRendering = true;
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.LinkColor = System.Drawing.Color.LightCyan;
            this.closeButton.Location = new System.Drawing.Point(481, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(16, 16);
            this.closeButton.TabIndex = 14;
            this.closeButton.TabStop = true;
            this.closeButton.Text = "X";
            this.closeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.closeButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.closeButton_linkedClicked);
            // 
            // reviewer_note_label
            // 
            this.reviewer_note_label.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.reviewer_note_label.ForeColor = System.Drawing.Color.White;
            this.reviewer_note_label.Location = new System.Drawing.Point(7, 60);
            this.reviewer_note_label.Name = "reviewer_note_label";
            this.reviewer_note_label.Size = new System.Drawing.Size(61, 29);
            this.reviewer_note_label.TabIndex = 15;
            this.reviewer_note_label.Text = "Reviewer Note: ";
            this.reviewer_note_label.UseCompatibleTextRendering = true;
            this.reviewer_note_label.Visible = false;
            // 
            // reviewer_note
            // 
            this.reviewer_note.BackColor = System.Drawing.SystemColors.Window;
            this.reviewer_note.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.reviewer_note.Location = new System.Drawing.Point(68, 60);
            this.reviewer_note.Multiline = true;
            this.reviewer_note.Name = "reviewer_note";
            this.reviewer_note.ReadOnly = true;
            this.reviewer_note.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reviewer_note.Size = new System.Drawing.Size(415, 91);
            this.reviewer_note.TabIndex = 16;
            this.reviewer_note.Visible = false;
            // 
            // frmInternalRequestFacePhoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(501, 184);
            this.Controls.Add(this.reviewer_note);
            this.Controls.Add(this.reviewer_note_label);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInternalRequestFacePhoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Internal Request Face Photo";
            this.TopMost = true;
            this.SizeChanged += new System.EventHandler(this.frmInternalRequestFacePhoto_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        #endregion
        public System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.LinkLabel closeButton;
        public System.Windows.Forms.Label reviewer_note_label;
        private System.Windows.Forms.TextBox reviewer_note;
    }
}