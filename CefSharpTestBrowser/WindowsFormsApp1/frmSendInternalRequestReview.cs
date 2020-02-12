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

            var start_time = Globals.frmMain.StartTime_LastAction;
            if (start_time == null) {
                start_time = Globals.frmMain.StartTime_BrowserChanged;
            }

            InternalRequestReview rr = new InternalRequestReview()
            {
                url = Globals.CurrentUrl,
                agent_id = Globals.Profile.AgentID,
                agent_notes = txtNotes.Text,
                duration = (int)((DateTime.Now - (DateTime)start_time).TotalSeconds)
            };

            var result = rr.Save();
            if (result != null) {
                Globals.INTERNAL_RR = result;
                Settings.Default.irr_id = Globals.INTERNAL_RR.id;
                Settings.Default.Save();
                Globals.frmMain.StartbgWorkIRR();

                //Broadcast if in buddy system
                if (Globals.IsServer())
                {
                    ServerAsync.SendToAll(new PairCommand { Action = "IRR", Message = Globals.INTERNAL_RR.id.ToString(), Url = Globals.CurrentUrl });
                }
                else if (Globals.IsClient())
                {
                    AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "IRR", Message = Globals.INTERNAL_RR.id.ToString(), Url = Globals.CurrentUrl });
                }

                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
