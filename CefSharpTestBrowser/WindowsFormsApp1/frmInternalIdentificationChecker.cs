﻿using CefSharp.WinForms.Internals;
using CSTool;
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
    public partial class frmInternalIdentificationChecker : Form
    {
        public frmInternalIdentificationChecker()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
        }

        private void frmInternalIdentificationChecker_Load(object sender, EventArgs e)
        {
            //height 501, 264
        }

        private void frmInternalIdentificationChecker_SizeChanged(object sender, EventArgs e)
        {
            var scrn = Screen.FromControl(Globals.frmMain);
            this.Location = new Point(scrn.Bounds.Right - this.Width - 5, scrn.Bounds.Bottom - this.Height - 50);
        }

        public void update_info()
        {
            var result = InternalIdentificationChecker.Get(Globals.INTERNAL_IIDC.id, Globals.Profile.AgentID);
            lblStatus.Text = result.status;
            switch (result.status)
            {
                case "New":
                    BackColor = Color.Gray;
                    lblStatus.Text = "PENDING";
                    Height = 172;
                    break;
                case "Approved":
                    if (String.IsNullOrEmpty(result.reviewer_uploaded_photo))
                    {
                        label7.Visible = false;
                        lblReviewerPhotoGcsUrl.Visible = false;
                    }
                    BackColor = Color.Green;
                    Height = 310;
                    break;
                case "Denied":
                    if (String.IsNullOrEmpty(result.reviewer_uploaded_photo))
                    {
                        label7.Visible = false;
                        lblReviewerPhotoGcsUrl.Visible = false;
                    }
                    BackColor = Color.Red;
                    Height = 310;
                    break;
                case "Processing":
                    BackColor = Color.FromArgb(230, 126, 34);
                    break;
            }

            txtNotes.Text = result.agent_notes;
            txtReviewerNotes.Text = result.reviewer_notes;
            Globals.INTERNAL_IIDC = result;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void lblReviewerPhotoUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Globals.INTERNAL_IIDC.reviewer_uploaded_photo))
            {
                frmPopup frmpop = new frmPopup(Globals.INTERNAL_IIDC.reviewer_uploaded_photo);
                frmpop.Show();
            }
        }
    }
}