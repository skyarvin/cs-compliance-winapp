using CSTool;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
            if (string.IsNullOrEmpty(txtEmail.Text.Trim())) {
                MessageBox.Show("Invalid Username", "Error");
                return;
            }           
            try
            {
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

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("The download is completed!");
            version_update_panel.Hide();
            WebClient webClient = new WebClient();
            var newVersion = new Version(webClient.DownloadString("https://cscb-dev1.staffme.online/static/update.txt"));

            Process process = new Process();
            process.StartInfo.FileName = "msiexec.exe";
            process.StartInfo.Arguments = string.Format($"/i CS_TOOL_V{newVersion}_472.msi");
            this.Close();
            process.Start();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
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
                Directory.Delete(cache_path, true);

            WebClient webClient = new WebClient();
            var newVersion = new Version(webClient.DownloadString("https://cscb-dev1.staffme.online/static/update.txt"));
            var currentVersion = new Version(Application.ProductVersion);

            Console.WriteLine(newVersion.CompareTo(currentVersion));
            if (newVersion.CompareTo(currentVersion) >= 1)
            {
                if (MessageBox.Show("A new update is available! Do you want to download it?", "CSTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    version_update_panel.Show();
                    version_label.Text = $"From: {currentVersion} => To: {newVersion}";
                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            if (File.Exists($@".\CS_TOOL_V{newVersion}_472.msi")) { File.Delete($@".\CS_TOOL_V{newVersion}_472.msi"); }
                            if (File.Exists($@".\CS_TOOL_V{currentVersion}_472.msi")) { File.Delete($@".\CS_TOOL_V{currentVersion}_472.msi"); }
                            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                            client.DownloadFileAsync(new Uri($"https://cscb-dev1.staffme.online/static/CS_TOOL_V{newVersion}_472.msi"), $@".\CS_TOOL_V{newVersion}_472.msi");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(e);
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
    }
}