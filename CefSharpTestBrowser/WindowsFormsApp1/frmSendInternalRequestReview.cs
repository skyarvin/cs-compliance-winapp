﻿using CefSharp.WinForms.Internals;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;

namespace WindowsFormsApp1
{
    public partial class frmSendInternalRequestReview : Form
    {
        public frmSendInternalRequestReview()
        {
            InitializeComponent();
        }


        private void frmSendInternalRequestReview_Load(object sender, EventArgs e)
        {
            txtNotes.Text = "";
            lblUrl.Text = Globals.CurrentUrl;
            cmbViolation.DataSource = new BindingSource(InternalRequestReview.violations, null);
            cmbViolation.DisplayMember = "Value";
            cmbViolation.ValueMember = "Key";
            cmbViolation.Text = "";
            int followers = 0;
            string followRaw = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#room_info').children()[2].textContent").Result.Result);
            followRaw = new String(followRaw.Where(Char.IsDigit).ToArray());
            if (!String.IsNullOrEmpty(followRaw)) followers = int.Parse(followRaw);

            if (followers >= (Globals.ComplianceAgent.is_trainee ? Globals.SC_THRESHOLD_TRAINEE : Globals.SC_THRESHOLD))
            { 
                chkSkypeCompliance.Checked = true; 
            }
       }

        private void btnSendRR_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbViolation.Text.Trim()))
            {
                MessageBox.Show("Violation cannot be empty!");
                cmbViolation.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtNotes.Text.Trim()))
            {
                MessageBox.Show("Notes cannot be empty!");
                txtNotes.Focus();
                return;
            }

            var start_time = Globals.StartTime_LastAction;
            if (start_time == null)
            {
                start_time = Globals.frmMain.StartTime_BrowserChanged;
            }

            InternalRequestReview rr = new InternalRequestReview()
            {
                url = Globals.CurrentUrl,
                agent_id = Globals.Profile.AgentID,
                agent_notes = txtNotes.Text,
                duration = (int)((DateTime.Now - (DateTime)start_time).TotalSeconds),
                violation = cmbViolation.SelectedValue.ToString(),
                is_trainee = Globals.ComplianceAgent.is_trainee,
                skype_compliance = chkSkypeCompliance.Checked
            };
               
            var result = rr.Save();
            if (result != null)
            {
                Globals.INTERNAL_RR = result;
                Settings.Default.irr_id = Globals.INTERNAL_RR.id;
                Settings.Default.Save();
                Globals.frmMain.StartbgWorkIRR();

                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
