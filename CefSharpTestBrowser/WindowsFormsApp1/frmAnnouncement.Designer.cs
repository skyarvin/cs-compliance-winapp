
namespace WindowsFormsApp1
{
    partial class frmAnnouncement
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPostedDate = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.chkboxMarkAsRead = new System.Windows.Forms.CheckBox();
            this.linklblLinkToPost = new System.Windows.Forms.LinkLabel();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.MaximumSize = new System.Drawing.Size(680, 100);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(680, 100);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Announcement Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPostedDate
            // 
            this.lblPostedDate.AutoSize = true;
            this.lblPostedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostedDate.Location = new System.Drawing.Point(576, 302);
            this.lblPostedDate.Name = "lblPostedDate";
            this.lblPostedDate.Size = new System.Drawing.Size(129, 16);
            this.lblPostedDate.TabIndex = 2;
            this.lblPostedDate.Text = "2021-12-14 12:55 PM";
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 100);
            this.lblMessage.MaximumSize = new System.Drawing.Size(680, 140);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(680, 125);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Enter Announcement Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkboxMarkAsRead
            // 
            this.chkboxMarkAsRead.AutoSize = true;
            this.chkboxMarkAsRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxMarkAsRead.Location = new System.Drawing.Point(17, 302);
            this.chkboxMarkAsRead.Name = "chkboxMarkAsRead";
            this.chkboxMarkAsRead.Size = new System.Drawing.Size(120, 22);
            this.chkboxMarkAsRead.TabIndex = 4;
            this.chkboxMarkAsRead.Text = "Mark as Read";
            this.chkboxMarkAsRead.UseVisualStyleBackColor = true;
            this.chkboxMarkAsRead.Click += new System.EventHandler(this.chkboxMarkAsRead_Click);
            // 
            // linklblLinkToPost
            // 
            this.linklblLinkToPost.AutoSize = true;
            this.linklblLinkToPost.Location = new System.Drawing.Point(14, 255);
            this.linklblLinkToPost.Name = "linklblLinkToPost";
            this.linklblLinkToPost.Size = new System.Drawing.Size(70, 15);
            this.linklblLinkToPost.TabIndex = 5;
            this.linklblLinkToPost.TabStop = true;
            this.linklblLinkToPost.Text = "Link to Post";
            this.linklblLinkToPost.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklblLinkToPost_LinkClicked);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(16, 357);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(84, 40);
            this.btnPrevious.TabIndex = 6;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(600, 357);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(84, 40);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(295, 357);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(109, 40);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(516, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Posted:";
            // 
            // frmAnnouncement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 422);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.linklblLinkToPost);
            this.Controls.Add(this.chkboxMarkAsRead);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblPostedDate);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnnouncement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Announcement";
            this.Load += new System.EventHandler(this.frmAnnouncement_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPostedDate;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.CheckBox chkboxMarkAsRead;
        private System.Windows.Forms.LinkLabel linklblLinkToPost;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}