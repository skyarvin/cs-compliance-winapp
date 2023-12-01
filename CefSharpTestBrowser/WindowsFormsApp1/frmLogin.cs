using CSTool;
using CSTool.Class;
using CSTool.Handlers.ErrorsHandler;
using CSTool.Handlers.Interfaces;
using CSTool.Models;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    public partial class frmLogin : Form
    {
        private bool bExitApp = true;
        private bool seePass = false;
        public frmLogin()
        {
            InitializeComponent();
            lblVersion.Text = string.Concat("v.",Globals.CurrentVersion());
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
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()) || string.IsNullOrEmpty(txtPwd.Text.Trim()))
            {
                MessageBox.Show("Please fill out all required fields", "Error");
                return;
            }
            try
            {
                Globals.user_account.username = txtEmail.Text;
                Globals.user_account.role = cmbUtype.Text;
                UserToken result = Globals.user_account.UserLogin(txtPwd.Text);
                if (result == null)
                {
                    MessageBox.Show("Username or password is incorrect", "Error");
                    return;
                }

                Globals.ComplianceAgent = Agent.Get(txtEmail.Text);
                if (Globals.ComplianceAgent != null)
                {
                    if (Globals.ComplianceAgent.tier_level is null)
                    {
                        MessageBox.Show("Tier level is not registered! Please contact admin.", "Error");
                        return;
                    }

                    bExitApp = false;
                    Globals.SaveUserSettings();
                    if (Settings.Default.user_type.ToUpper().Contains("AGENT") && Settings.Default.role == "CSA" ||
                        Settings.Default.user_type.ToUpper().Contains("TRAINEE") && Settings.Default.role == "TRAINEE")
                    {
                        Globals.frmMain = new frmMain();
                        Globals.frmMain.Show();
                        this.Close();
                    }
                    else if (Settings.Default.user_type.ToUpper().Contains("QA") && Settings.Default.role == "CSQA")
                    {
                        //new form
                        Globals.FrmQA = new frmQA();
                        Globals.FrmQA.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please check your User Type.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("We cannot find an account with that username.", "Error");
                }
            }
            catch (TFARequiredException tfa)
            {
                bExitApp = false;
                frmTfa frmTfa = new frmTfa(FormType.LoginForm, tfa.userTfa);
                frmTfa.FormClosed += new FormClosedEventHandler(TFA_Closed);
                frmTfa.Show();
                this.Hide();
            }
            catch (TfaRegistrationRequiredException url)
            {
                MessageBox.Show("You need to register a TFA device", "Error");
                var urlToRedirect = url.Message.Replace("127.0.0.1", "localhost");
                System.Diagnostics.Process.Start(urlToRedirect);
            }
            catch (UnauthorizeException unauthorize)
            {
                Globals.SaveToLogFile(unauthorize.ToString(), (int)LogType.Error);
                MessageBox.Show("Username or password is incorrect", "Error");
            }
            catch (AggregateException e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat("Error connecting to Compliance servers", System.Environment.NewLine, "Please refresh and try again.",
                System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
            }
        }

        private void TFA_Closed(object sender, FormClosedEventArgs e)
        {
            if(Globals.frmMain == null && Globals.FrmQA == null)
            {
                bExitApp = true;
                this.Show();
            }
        }


        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtPwd.Controls.Add(eyeViewPictureBox);
            eyeViewPictureBox.Location = new Point(300, 4);

            //workshift_list.DataSource = new BindingSource(Globals.workshifts, null);
            //workshift_list.DisplayMember = "Value";
            //workshift_list.ValueMember = "Key";

            if (!string.IsNullOrEmpty(CSTool.Properties.Settings.Default.email))
            {
                txtEmail.Text = CSTool.Properties.Settings.Default.email;
            }
            //if (!string.IsNullOrEmpty(CSTool.Properties.Settings.Default.workshift))
            //{
            //    workshift_list.SelectedValue = CSTool.Properties.Settings.Default.workshift;
            //}
            if (!string.IsNullOrEmpty(CSTool.Properties.Settings.Default.user_type))
            {
                cmbUtype.Text= CSTool.Properties.Settings.Default.user_type;
            }

            string cache_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/cache/cache/");
            if (Directory.Exists(cache_path))
                Directory.Delete(cache_path, true);
        }

        private void Workshift_list_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void cmbUtype_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void eyeViewPictureBox_Click(object sender, EventArgs e)
        {
            if (seePass)
            {
                eyeViewPictureBox.Image = CSTool.Properties.Resources.eye_hidden;
                seePass = false;
                txtPwd.UseSystemPasswordChar = true;
                return;
            }
            eyeViewPictureBox.Image = CSTool.Properties.Resources.eye_view;
            seePass = true;
            txtPwd.UseSystemPasswordChar = false;
        }
    }
}