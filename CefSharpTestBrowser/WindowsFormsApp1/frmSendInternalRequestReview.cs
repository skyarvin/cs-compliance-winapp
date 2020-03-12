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
using System.Threading.Tasks;
using System.Windows.Forms;

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
                skype_compliance = checkBox1.Checked
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
