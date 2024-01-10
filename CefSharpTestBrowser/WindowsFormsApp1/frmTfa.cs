using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1;
using CSTool.Properties;
using CSTool.Models;
using CSTool.Handlers.Interfaces;
using CSTool.Handlers.ErrorsHandler;
using System.Collections;
using CefSharp;
using Newtonsoft.Json;
using System.Net.Http;

namespace CSTool
{
    public partial class frmTfa : Form
    {
        private bool bExitApp = true;
        private string device_id;
        private readonly FormType frmType;
        private readonly UserTFA userTfa;
        private string prev_device_id;
        private TFA tfa = new TFA();
        private int timer =  10;

        public frmTfa(FormType frmType, UserTFA userTfa)
        {
            InitializeComponent();
            this.frmType = frmType;
            this.userTfa = userTfa;
            this.resendCodeTimer.Start();
            this.resetTimer();
        }

        private void frmTfa_Load(object sender, EventArgs e)
        {
            if (this.frmType != FormType.LoginForm)
            {
                back_btn.Hide();
            }

            foreach (var device in this.userTfa.devices)
            {
                device_list.Items.Add(new { Key=device["device_id"], Value=device["device_name"], Type=device["type"] });
            }
            device_list.DisplayMember = "Value";
            device_list.SelectedIndex = 0;
        }

        private void Tfa_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SubmitTFA();
            }
        }

        private void Submit_tfa_Click(object sender, EventArgs e)
        {
            SubmitTFA();
        }

        private void SubmitTFA()
        {
            try
            {
                this.tfa.device_id = this.device_id;
                this.tfa.nonce = this.userTfa.nonce;
                this.tfa.tfa_code = tfa_code.Text;
                this.tfa.user_id = this.userTfa.user_id;
                this.tfa.ValidateTfa();
                Globals.ComplianceAgent = Agent.Get(Globals.user_account.username);
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
                        if(this.frmType == FormType.LoginForm)
                        {
                            Globals.frmMain = new frmMain();
                            Globals.frmMain.Show();
                        }
                        this.Close();
                    }
                    else if (Settings.Default.user_type.ToUpper().Contains("QA") && Settings.Default.role == "CSQA")
                    {
                        if (this.frmType == FormType.LoginForm)
                        { 
                            Globals.FrmQA = new frmQA();
                            Globals.FrmQA.Show();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please check your User Type.", "Error");
                        bExitApp = false;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("We cannot find an account with that username.", "Error");
                }
            }
            catch (UnauthorizeException unauthorize)
            {
                using (HttpContent data = unauthorize.responseContent)
                {
                    var jsonString = data.ReadAsStringAsync();
                    jsonString.Wait();
                    UserTFA result = JsonConvert.DeserializeObject<UserTFA>(jsonString.Result);
                    this.userTfa.nonce = result.nonce;
                    MessageBox.Show("Invalid Two Factor Authenticator Code! \nPlease Try Again.", "Error");
                    return;
                }
            }
            catch (AggregateException ex)
            {
                Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat("Error connecting to Compliance servers", System.Environment.NewLine, "Please refresh and try again.",
                System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
            }
            catch (Exception ex)
            {
                Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat(ex.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
            }
        }

        private void frmTfa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExitApp)
            {
                Application.Exit();
            }
        }

        private void device_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string device_name = device_list.SelectedItem.GetType().GetProperty("Value").GetValue(device_list.SelectedItem, null).ToString();
            string device_id = device_list.SelectedItem.GetType().GetProperty("Key").GetValue(device_list.SelectedItem, null).ToString();
            var device_type = device_list.SelectedItem.GetType().GetProperty("Type").GetValue(device_list.SelectedItem, null).ToString();
            device_label.Text = "Authenticate your account on " + device_name;
            this.device_id = device_id;

            if (device_type != "Authenticator")
            {
                this.resendCodeButton.Visible = true;
                this.resendCodeButton.Enabled = false;
                this.resetTimer();
                this.resendCodeTimer.Start();
            } else
            {
                this.resendCodeButton.Visible = false;
                this.resetTimer();
            }

            if (this.prev_device_id != null && this.prev_device_id != this.device_id)
            {
                try
                {
                    this.tfa.device_id = device_id;
                    this.tfa.prev_device_id = this.prev_device_id;
                    this.tfa.nonce = this.userTfa.nonce;
                    this.tfa.user_id = this.userTfa.user_id;
                    this.userTfa.nonce = this.tfa.ChangeAuthenticatorDevice();
                }
                catch (Exception ex)
                {
                    Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                    MessageBox.Show(String.Concat("Error connecting to Compliance servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
                }
            }
            this.prev_device_id = this.device_id;
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            bExitApp = false;
            this.Close();
        }

        private void resendCodeTimer_Tick(object sender, EventArgs e)
        {
            runTimer();
        }

        private void runTimer()
        {
            this.resendCodeButton.Enabled = false;
            if (--this.timer < 0)
            {
                this.resendCodeButton.Enabled = true;
                this.resendCodeTimer.Stop();
            }
        }

        private void resetTimer()
        {
            this.resendCodeTimer.Stop();
            this.timer = 300;
        }

        private void resendCodeButton_click(object sender, EventArgs e)
        {
            this.resetTimer();
            runTimer();
            this.resendCodeTimer.Start();
            try
            {
                this.tfa.device_id = device_id;
                this.tfa.nonce = this.userTfa.nonce;
                this.tfa.user_id = this.userTfa.user_id;
                this.userTfa.nonce = this.tfa.ResendTfaCode();
                MessageBox.Show("We sent a new TFA code to your email");
            }
            catch (Exception ex)
            {
                Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat("Error connecting to Compliance servers", System.Environment.NewLine, "Please refresh and try again.",
                System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
            }
        }
    }
}
