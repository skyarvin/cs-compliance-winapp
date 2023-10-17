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

namespace CSTool
{
    public partial class frmTfa : Form
    {
        public TFA tfa;
        private bool bExitApp = true;
        private string device_name;

        public frmTfa()
        {
            InitializeComponent();
        }

        private void frmTfa_Load(object sender, EventArgs e)
        {
            foreach (var device in tfa.devices)
            {
                device_list.Items.Add(device["device_name"]);
            }
            device_list.SelectedIndex = 0;
        }

        private void Tfa_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SubmitTFA();
            }
        }

        private void submit_tfa_Click(object sender, EventArgs e)
        {
            SubmitTFA();
        }

        private void SubmitTFA()
        {
            try
            {
                if (!TFA.SubmitTfa(tfa_code.Text, tfa.nonce, tfa.user_id, this.device_name))
                {
                    MessageBox.Show("Invalid Two Factor Authenticator Code! \nPlease Try Again.", "Error");
                    return;
                }

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
                        Globals.frmMain = new frmMain();
                        Globals.frmMain.Show();
                        this.Close();
                    }
                    else if (Settings.Default.user_type.ToUpper().Contains("QA") && Settings.Default.role == "CSQA")
                    {
                        Globals.FrmQA = new frmQA();
                        Globals.FrmQA.Show();
                        this.Close();
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
                else
                {
                    MessageBox.Show("We cannot find an account with that username.", "Error");
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
            device_label.Text = "Authenticate your account on " + device_list.GetItemText(device_list.SelectedItem) + "'s Device";
            this.device_name = device_list.GetItemText(device_list.SelectedItem);
        }
    }
}
