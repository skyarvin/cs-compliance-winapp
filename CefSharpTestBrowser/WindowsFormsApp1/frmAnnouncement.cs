using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CSTool.Models;

namespace WindowsFormsApp1
{
    public partial class frmAnnouncement : Form
    {
        private Announcement announcement;
        private int PreviousIndex;
        private int NextIndex;

        public frmAnnouncement(Announcement announcement)
        {
            InitializeComponent();
            this.announcement = announcement;
        }

        private void frmAnnouncement_Load(object sender, EventArgs e)
        {
            DisplayAnnouncement();
        }

        private void DisplayAnnouncement()
        {
            if (this.announcement != null)
            {
                lblTitle.Text = this.announcement.title;
                lblTitle.Text = this.announcement.title;
                lblMessage.Text = this.announcement.message;
                linklblLinkToPost.Text = this.announcement.link_to_post;
                lblMessage.Text = this.announcement.message;
                lblPostedDate.Text = this.announcement.time_since;
                chkboxMarkAsRead.Checked = this.announcement.read_status == true ? true : false;
                chkboxMarkAsRead.Enabled = this.chkboxMarkAsRead.Checked == true ? false : true;

                var index = Globals.AnnouncementsList.announcements.FindIndex(x => x.id == this.announcement.id);
                btnPrevious.Enabled = Globals.AnnouncementsList.announcements.ElementAtOrDefault(index - 1) != null ? true : false;
                this.PreviousIndex = btnPrevious.Enabled == true ? index - 1 : 0;
                btnNext.Enabled = Globals.AnnouncementsList.announcements.ElementAtOrDefault(index + 1) != null ? true : false;
                this.NextIndex = btnNext.Enabled == true ? index + 1 : 0;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.announcement = Globals.AnnouncementsList.announcements.ElementAtOrDefault(this.NextIndex);
            DisplayAnnouncement();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            this.announcement = Globals.AnnouncementsList.announcements.ElementAtOrDefault(this.PreviousIndex);
            DisplayAnnouncement();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkboxMarkAsRead_Click(object sender, EventArgs e)
        {
            if (this.announcement.read_status == false)
            {
                this.announcement.AcknowledgeAnnouncement();
                chkboxMarkAsRead.Enabled = false;
            }
        }

        private void linklblLinkToPost_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linklblLinkToPost.Text);
        }
    }
}
