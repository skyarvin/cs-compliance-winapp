using CSTool.Models;
using CSTool.Properties;
using System;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace CSTool
{
    public partial class frmSendInternalRequestFacePhoto : Form
    {
        public frmSendInternalRequestFacePhoto()
        {
            InitializeComponent();
        }

        private void frmInternalRequestFacePhoto_Load(object sender, EventArgs e)
        {
            lblUrl.Text = Globals.CurrentUrl;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var start_time = Globals.StartTime_LastAction;
            if (start_time == null)
            {
                start_time = Globals.frmMain.StartTime_BrowserChanged;
            }

            InternalRequestFacePhoto irfp = new InternalRequestFacePhoto()
            {
                url = Globals.CurrentUrl,
                agent_id = Globals.Profile.AgentID,
                duration = (int)((DateTime.Now - (DateTime)start_time).TotalSeconds),
            };

            var result = irfp.Save();
            if (result != null)
            {
                Globals.INTERNAL_IRFP = result;
                Globals.frmMain.StartbgWorkIRFP();

                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
