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
        }

        private void btnSendRR_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNotes.Text.Trim())) {
                MessageBox.Show("Notes cannot be empty!");
                return;
            }
             
            InternalRequestReview rr = new InternalRequestReview()
            {
                url = Globals.CurrentUrl,
                agent_id = Globals.Profile.AgentID,
                agent_notes = txtNotes.Text
            };

            var result = rr.Save();
            if (result != null) {
                Globals.INTERNAL_RR = result;
                Globals.FrmInternalRequestReview.Show();
                this.DialogResult = DialogResult.OK;
            }
                
        }
    }
}
