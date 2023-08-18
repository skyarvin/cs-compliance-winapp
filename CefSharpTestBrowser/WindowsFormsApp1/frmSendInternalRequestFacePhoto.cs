using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Globals.FrmInternalRequestFacePhoto = new frmInternalRequestFacePhoto();
            Globals.FrmInternalRequestFacePhoto.ShowDialog();

        }
    }
}
