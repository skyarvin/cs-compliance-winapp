using CSTool;
using CSTool.Class;
using CSTool.Handlers;
using CSTool.Handlers.ErrorsHandler;
using CSTool.Handlers.Interfaces;
using CSTool.Models;
using CSTool.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
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
            GenerateDeviceIdentifier();
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
                System.Diagnostics.Process.Start(url.Message);
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
            {
                string[] IgnoreCredentialDirectories =
                {
                    "Network",
                    "Local Storage",
                    "Session Storage"
                };

                //delete all folders except Network, Local Storage, Session Storage
                DirectoryInfo cache_path_di = new DirectoryInfo(cache_path);
                foreach (DirectoryInfo cache_directories in cache_path_di.GetDirectories().Where(directory => !IgnoreCredentialDirectories.Contains(directory.Name)))
                    cache_directories.Delete(true);
                //delete all files except LocalPrefs.json
                //contains encryption key required for Cookie persistence
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

        private void GenerateDeviceIdentifier()
        {
            var diskDrive = new ManagementObjectSearcher("select SerialNumber from Win32_DiskDrive");
            var processor = new ManagementObjectSearcher("select ProcessorId from Win32_Processor");

            string diskSerialNumber = "";
            string processorId = "";

            foreach (ManagementObject share in diskDrive.Get())
            {
                diskSerialNumber = share["SerialNumber"].ToString();
            }

            foreach (ManagementObject share in processor.Get())
            {
                processorId = share["ProcessorId"].ToString();
            }

            RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey windowsNTKey = localMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion");
            var productID = windowsNTKey.GetValue("ProductId");

            var deviceId = $"{processorId}-{diskSerialNumber}-{productID}";
            Globals.device_identifier = HashHandler.GetHash(deviceId);
        }
    }
}