using System;
using System.Drawing;
using System.Windows.Forms;
using CSTool.Models;
using WindowsFormsApp1;

namespace CSTool
{
    public partial class usrCtrlAnnouncement : UserControl
    {
        private Announcement announcement;
        public usrCtrlAnnouncement(Announcement announcement)
        {
            InitializeComponent();
            this.announcement = announcement;
            this.lblTimePosted.Text = this.announcement.time_since;
            this.lblTitle.Text = this.announcement.read_status == false ? $"• {this.announcement.title}" : $"{this.announcement.title}";
            this.lblTitle.Font = this.announcement.read_status == false ? new Font(this.lblTitle.Font, FontStyle.Bold) : new Font(this.lblTitle.Font, FontStyle.Regular);

            ToolTip announcement_tooltip = new ToolTip();
            announcement_tooltip.SetToolTip(this.lblTitle, this.announcement.title);
        }

        private void usrCtrlAnnouncement_Click(object sender, EventArgs e)
        {
            ShowAnnouncement();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            ShowAnnouncement();
        }

        private void ShowAnnouncement()
        {
            var FrmAnnouncement = new frmAnnouncement(this.announcement);
            FrmAnnouncement.ShowDialog();
        }

        private void lblTimePosted_Click(object sender, EventArgs e)
        {
            ShowAnnouncement();
        }
    }
}
