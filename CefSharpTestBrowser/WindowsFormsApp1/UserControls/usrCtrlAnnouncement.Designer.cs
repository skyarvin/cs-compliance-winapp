
namespace CSTool
{
    partial class usrCtrlAnnouncement
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTimePosted = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTitle.Location = new System.Drawing.Point(1, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(214, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "This is a Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblTimePosted
            // 
            this.lblTimePosted.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTimePosted.Location = new System.Drawing.Point(4, 31);
            this.lblTimePosted.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblTimePosted.Name = "lblTimePosted";
            this.lblTimePosted.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTimePosted.Size = new System.Drawing.Size(220, 16);
            this.lblTimePosted.TabIndex = 2;
            this.lblTimePosted.Text = "2 days ago";
            this.lblTimePosted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTimePosted.Click += new System.EventHandler(this.lblTimePosted_Click);
            // 
            // usrCtrlAnnouncement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblTimePosted);
            this.Controls.Add(this.lblTitle);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "usrCtrlAnnouncement";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(228, 51);
            this.Click += new System.EventHandler(this.usrCtrlAnnouncement_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTimePosted;
    }
}
