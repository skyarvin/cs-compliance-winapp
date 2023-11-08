using CSTool.Models;
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
    public partial class frmInternalRequestFacePhoto : Form
    {
        public frmInternalRequestFacePhoto()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
        }

        private void frmInternalRequestFacePhoto_SizeChanged(object sender, EventArgs e)
        {
            var scrn = Screen.FromControl(Globals.frmMain);
            this.Location = new Point(scrn.Bounds.Right - this.Width - 5, scrn.Bounds.Bottom - this.Height - 50);
        } 

        public void update_info()
        {
            var result = InternalRequestFacePhoto.Get(Globals.INTERNAL_IRFP.id, Globals.Profile.AgentID);
            lblStatus.Text = result.status;
            Height = 210;
            switch (result.status)
            {
                case "New":
                    BackColor = Color.Gray;
                    lblStatus.Text = "PENDING";
                    break;
                case "Approved":
                    if (result.reviewer_notes.Length > 0)
                    {
                        reviewer_note.Visible = true;
                        reviewer_note_label.Visible = true;
                        reviewer_note.Text = result.reviewer_notes;
                    }
                    BackColor = Color.Green;
                    break;
                case "Denied":
                    reviewer_note.Visible = true;
                    reviewer_note_label.Visible = true;
                    reviewer_note.Text = result.reviewer_notes;
                    BackColor = Color.Red;
                    break;
                case "Processing":
                    BackColor = Color.FromArgb(230, 126, 34);
                    break;
            }
            lblUrl.Text = result.url;
            Globals.INTERNAL_IRFP = result;
        }
    }
}
