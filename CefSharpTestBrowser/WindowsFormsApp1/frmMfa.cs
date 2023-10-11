﻿using System;
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
    public partial class frmMfa : Form
    {
        public IMFAToken mfa;
        private bool bExitApp = true;
        private string device_name;

        public frmMfa()
        {
            InitializeComponent();
        }

        private void frmMfa_Load(object sender, EventArgs e)
        {
            if (mfa.devices.Count == 1)
            {
                device_label.Text = "Authenticate your device on " + mfa.devices[0]["device_name"] + "'s Device";
                this.device_name = mfa.devices[0]["device_name"];
            }
        }

        private void submit_mfa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Mfa.SubmitMfa(mfa_code.Text, mfa.nonce, mfa.user_id, this.device_name))
                {
                    MessageBox.Show("Invalid Two Factor Authenticator Code! \nPlease Try Again.", "Error");
                    return;
                }

                Globals.ComplianceAgent = Agent.Get(Globals.user_account.username); // next to pre goodjob
                if (Globals.ComplianceAgent != null)
                {
                    if (Globals.ComplianceAgent.tier_level is null)
                    {
                        MessageBox.Show("Tier level is not registered! Please contact admin.", "Error");
                        return;
                    }

                    bExitApp = false;
                    Globals.SaveToDevice();

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

        private void frmMfa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExitApp)
            {
                Application.Exit();
            }
        }
    }
}
