using CefSharp.WinForms.Internals;
using CSTool.Class;
using CSTool.Models;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmInternalRequestReview : Form
    {
        public frmInternalRequestReview()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
        }

        private void frmInternalRequestReview_Load(object sender, EventArgs e)
        {
            //height 501, 264

        }
        private void frmInternalRequestReview_SizeChanged(object sender, EventArgs e)
        {
            foreach (var scrn in Screen.AllScreens)
            {
                if (scrn.Bounds.Contains(this.Location))
                {
                    this.Location = new Point(scrn.Bounds.Right - this.Width - 5, scrn.Bounds.Bottom - this.Height - 50);
                    return;
                }
            }
        }
        public void reset_controls()
        {
            this.BackColor = Color.Gray;
            lblStatus.Text = "PENDING";
            lblUrl.Text = "-";
            txtNotes.Text = "";
            txtReviewerNotes.Text = "";
            this.Height = 186;
        }

        public void update_info()
        {
            var result = InternalRequestReview.Get(Globals.INTERNAL_RR.id);
            lblStatus.Text = result.status;
            if (result.status == "New")
            {
                this.BackColor = Color.Gray;
                lblStatus.Text = "PENDING";
                this.Height = 186;
            }

            if (result.status == "Approved")
            {
                this.Height = 291;
                this.BackColor = Color.Green;
            }

            if (result.status == "Denied")
            {
                this.Height = 291;
                this.BackColor = Color.Red;
            }
            if (result.status == "Processing")
            {
                this.BackColor = Color.FromArgb(230, 126, 34);
                this.Height = 186;
            }
            if (result.status == "Waiting SC")
            {
                this.BackColor = Color.FromArgb(0, 0, 255);
                this.Height = 186;
            }

            lblUrl.Text = result.url;
            lblViolation.Text = result.violation_long_name;
            txtNotes.Text = result.agent_notes;
            txtReviewerNotes.Text = result.reviewer_notes;
            if (result.skype_compliance) {
                lblSkypeCompliance.Visible = true;
             }
            

            Globals.INTERNAL_RR = result;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
