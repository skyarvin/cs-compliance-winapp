using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    public partial class frmLogin : Form
    {
        private bool bExitApp = true;
        public frmLogin()
        {
            InitializeComponent();
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment cd = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                lblVersion.Text = string.Concat("v.", cd.CurrentVersion.ToString());
            }
        }
        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExitApp)
            {
                Application.Exit();
            }
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Login();
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
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()) || (!(new EmailAddressAttribute().IsValid(txtEmail.Text))))
                MessageBox.Show("Invalid Email", "Error");
            else
            {
                Globals.ComplianceAgent = Agent.Get(txtEmail.Text);
                if (Globals.ComplianceAgent != null)
                {
                    bExitApp = false;
                    frmMain Mainform = new frmMain();
                    Mainform.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("We cannot find an account with that email address.", "Error");
                }
            }
        }
    }
}
