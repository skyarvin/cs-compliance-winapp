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
            this.reviewer_note_label = new System.Windows.Forms.Label();
            this.reviewer_note = new System.Windows.Forms.TextBox();
            this.panelReviewer = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelReviewer.SuspendLayout();
            this.panel2.SuspendLayout();
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
            // reviewer_note_label
            // 
            this.reviewer_note_label.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.reviewer_note_label.ForeColor = System.Drawing.Color.White;
            this.reviewer_note_label.Location = new System.Drawing.Point(7, 0);
            this.reviewer_note_label.Name = "reviewer_note_label";
            this.reviewer_note_label.Size = new System.Drawing.Size(61, 29);
            this.reviewer_note_label.TabIndex = 15;
            this.reviewer_note_label.Text = "Reviewer Note: ";
            this.reviewer_note_label.UseCompatibleTextRendering = true;
            // 
            // reviewer_note
            // 
            this.reviewer_note.BackColor = System.Drawing.SystemColors.Window;
            this.reviewer_note.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.reviewer_note.Location = new System.Drawing.Point(68, 0);
            this.reviewer_note.Multiline = true;
            this.reviewer_note.Name = "reviewer_note";
            this.reviewer_note.ReadOnly = true;
            this.reviewer_note.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reviewer_note.Size = new System.Drawing.Size(415, 91);
            this.reviewer_note.TabIndex = 16;
            // 
            // panelReviewer
            // 
            this.panelReviewer.Controls.Add(this.reviewer_note_label);
            this.panelReviewer.Controls.Add(this.reviewer_note);
            this.panelReviewer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelReviewer.Location = new System.Drawing.Point(0, 60);
            this.panelReviewer.Name = "panelReviewer";
            this.panelReviewer.Size = new System.Drawing.Size(501, 124);
            this.panelReviewer.TabIndex = 17;
            this.panelReviewer.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblUrl);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblStatus);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(501, 57);
            this.panel2.TabIndex = 18;
            // 
            // frmInternalRequestFacePhoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(501, 184);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelReviewer);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(517, 150);
            this.Name = "frmInternalRequestFacePhoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Internal Request Face Photo";
            this.TopMost = true;
            this.SizeChanged += new System.EventHandler(this.frmInternalRequestFacePhoto_SizeChanged);
            this.panelReviewer.ResumeLayout(false);
            this.panelReviewer.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

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
        public System.Windows.Forms.Label reviewer_note_label;
        private System.Windows.Forms.TextBox reviewer_note;
        private System.Windows.Forms.Panel panelReviewer;
        private System.Windows.Forms.Panel panel2;
    }
}