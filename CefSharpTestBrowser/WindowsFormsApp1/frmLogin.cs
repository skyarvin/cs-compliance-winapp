using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Services;
using WindowsFormsApp1.Models;
namespace WindowsFormsApp1
{
    public partial class frmLogin : Form
    {
        private bool bExitApp = true;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
           
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }
        private void Login() {
            //TODO ADD EMAIL VALIDATION
            Globals.ComplianceAgent = LoggerServices.GetAgentId(txtEmail.Text);
            if (Globals.ComplianceAgent != null)
            {
                bExitApp = false;
                frmMain Mainform = new frmMain();
                Mainform.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Email", "Error");
            }
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExitApp) {
                Application.Exit();
            }
        }
    }
}
