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
            lblVersion.Text = Globals.CurrentVersion();
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
        private void Login()
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()) || (!(new EmailAddressAttribute().IsValid(txtEmail.Text))))
                MessageBox.Show("Invalid Email", "Error");
            else
            {
                try
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
                catch (AggregateException e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    MessageBox.Show(String.Concat("Server connection problem", System.Environment.NewLine, "Please refresh and try again.",
                        System.Environment.NewLine, "If error still persist, Please contact Admin"), "Error");
                }
                catch (Exception e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    MessageBox.Show(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
                }
            }
        }
    }
}
