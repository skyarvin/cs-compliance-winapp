using CefSharp.WinForms.Internals;
using SkydevCSTool.Class;
using SkydevCSTool.Models;
using SkydevCSTool.Properties;
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
                    this.Location = new Point(scrn.Bounds.Right - this.Width -5, scrn.Bounds.Bottom - this.Height - 50);
                    return;
                }
            }
        }
        private void reset_controls()
        {
            this.BackColor = Color.Gray;
            lblStatus.Text = "PENDING";
            lblUrl.Text = "-";
            txtNotes.Text = "";
            txtReviewerNotes.Text = "";
            this.Height = 160;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            BackgroundWorker helperBW = sender as BackgroundWorker;
            this.InvokeOnUiThreadIfRequired(() =>
            {
                if (Globals.INTERNAL_RR.id == 0)
                {
                    reset_controls();
                    if (helperBW.CancellationPending)
                    {
                        e.Cancel = true;
                    }
                    backgroundWorker1.CancelAsync();
                    this.Hide();
                    return;
                }
                var result = InternalRequestReview.Get(Globals.INTERNAL_RR.id);
                lblStatus.Text = result.status;
                if(result.status == "New")
                {
                    this.BackColor = Color.Gray;
                    lblStatus.Text = "PENDING";
                    this.Height = 160;
                }

                if(result.status == "Approved")
                {
                    this.Height = 264;
                    this.BackColor = Color.Green;
                }

                if (result.status == "Denied")
                {
                    this.Height = 264;
                    this.BackColor = Color.Red;
                }

                lblUrl.Text = result.url;
                txtNotes.Text = result.agent_notes;
                txtReviewerNotes.Text = result.reviewer_notes;

                Globals.INTERNAL_RR = result;

                if (result.status != "New")
                {
                    backgroundWorker1.CancelAsync();
                    if (!this.Visible)
                        this.Show();
                }
            });

            Thread.Sleep(2000);
            Console.WriteLine("Umaandar");
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Error != null)
                Globals.showMessage(e.Error.Message);
            else
                backgroundWorker1.RunWorkerAsync();
        }

        private void frmInternalRequestReview_FormClosed(object sender, FormClosedEventArgs e)
        {
 
        }

        private void frmInternalRequestReview_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void frmInternalRequestReview_Activated(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy) {
                this.Size = new Size(501, 162);
                backgroundWorker1.RunWorkerAsync();
            }
      
        }
        private void frmInternalRequestReview_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }
    }
}
