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
            foreach (var scrn in Screen.AllScreens)
            {
                if (scrn.Bounds.Contains(this.Location))
                {
                    this.Location = new Point(scrn.Bounds.Right - this.Width - 5, scrn.Bounds.Bottom - this.Height - 50);
                    return;
                }
            }
        }

        public void update_info()
        {
            var result = InternalRequestFacePhoto.Get(Globals.INTERNAL_RFP.id);
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

            lblUrl.Text = Globals.CurrentUrl;
            Globals.INTERNAL_RFP = result;
        }


        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
