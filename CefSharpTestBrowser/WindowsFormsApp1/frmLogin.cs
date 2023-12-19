using CSTool;
using CSTool.Models;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                if (!UserAccount.UserLogin(txtEmail.Text, txtPwd.Text))
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
                    if (txtEmail.Text != Settings.Default.email || (Settings.Default.role != Globals.ComplianceAgent.role)) {
                        Settings.Default.irr_id = 0;
                    }
                    Settings.Default.role = Globals.ComplianceAgent.role;
                    Settings.Default.user_type = cmbUtype.Text;
                    Settings.Default.preference = null;
                    Settings.Default.email = txtEmail.Text;
                    Settings.Default.tier_level = Convert.ToInt32(Globals.ComplianceAgent.tier_level);
                    Settings.Default.Save();

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
                        //MessageBox.Show("Please check your User Type.", "Error");
                        Thread.Sleep(60000);
                    }
                }
                else
                {
                    MessageBox.Show("We cannot find an account with that username.", "Error");
                }
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


        private void FrmLogin_Load(object sender, EventArgs e)
        {
        
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
            {
                DirectoryInfo cache_path_di = new DirectoryInfo(cache_path);
                foreach (DirectoryInfo cache_directories in cache_path_di.GetDirectories().Where(directory => directory.Name != "Network"))
                    cache_directories.Delete(true);
                foreach (FileInfo file_directories in cache_path_di.GetFiles().Where(file => file.Name != "LocalPrefs.json"))
                    file_directories.Delete();

                //delete all except Cookies and Cookies-journal
                string cookies_path = string.Concat(cache_path, "\\Network");
                DirectoryInfo cache_network_path_di = new DirectoryInfo(cookies_path);
                if (Directory.Exists(cookies_path))
                {
                    foreach (FileInfo cache_network_files in cache_network_path_di.GetFiles().Where(file => !file.Name.Contains("Cookies")))
                    {
                        cache_network_files.Delete();
                    }
                }
            }
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
    }
}