using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
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
                    //if (SkydevCSTool.Properties.Settings.Default.email != txtEmail.Text)
                    //{
                    //    string cache_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/SkydevCsTool/cookies/");
                    //    if (Directory.Exists(cache_path))
                    //        Directory.Delete(cache_path, true);
                    //}

                    Globals.ComplianceAgent = Agent.Get(txtEmail.Text, workshift_list.SelectedValue.ToString());
                    if (Globals.ComplianceAgent != null)
                    {
                        bExitApp = false;
                        
                        Globals.frmMain = new frmMain();
                        SkydevCSTool.Properties.Settings.Default.email = txtEmail.Text;
                        SkydevCSTool.Properties.Settings.Default.workshift = workshift_list.SelectedValue.ToString();
                        SkydevCSTool.Properties.Settings.Default.Save();
                        if (Globals.ComplianceAgent.last_workshift == workshift_list.SelectedValue.ToString() || String.IsNullOrEmpty(Globals.ComplianceAgent.last_workshift))
                        {
                            Globals.ComplianceAgent.last_workshift = workshift_list.SelectedValue.ToString();
                            Globals.frmMain.Show();
                            this.Close();
                            return;
                        } 

                        DialogResult dialogResult = MessageBox.Show(string.Concat("Your previous shift is ",Globals.workshifts[Globals.ComplianceAgent.last_workshift],"\nAre you sure this is your workshift?"), "Workshift Change Detected", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Globals.ComplianceAgent.last_workshift = workshift_list.SelectedValue.ToString();
                            Globals.frmMain.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("We cannot find an account with that email address.", "Error");
                    }
                }
                catch (AggregateException e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    MessageBox.Show(String.Concat("Error connecting to Chaturbate servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If chaturbate/internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
                }
                catch (Exception e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    MessageBox.Show(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
                }
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            workshift_list.DataSource = new BindingSource(Globals.workshifts, null);
            workshift_list.DisplayMember = "Value";
            workshift_list.ValueMember = "Key";

            if (!string.IsNullOrEmpty(SkydevCSTool.Properties.Settings.Default.email))
            {
                txtEmail.Text = SkydevCSTool.Properties.Settings.Default.email;
            }
            if (!string.IsNullOrEmpty(SkydevCSTool.Properties.Settings.Default.workshift))
            {
                workshift_list.SelectedValue = SkydevCSTool.Properties.Settings.Default.workshift;
            }
        }

        private void Workshift_list_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }
}