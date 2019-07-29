﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Deployment;
using WindowsFormsApp1.Models;
using System.Diagnostics;
using System.Deployment.Application;
using SkydevCSTool.Models;

namespace WindowsFormsApp1
{
    public partial class frmLogin : Form
    {
        private bool bExitApp = true;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment cd = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                lblVersion.Text = string.Concat("v.", cd.CurrentVersion.ToString());
            }
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
            //TODO ADD EMAIL VALIDATION
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                MessageBox.Show("Email required", "Error");
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
                    MessageBox.Show("Invalid Email", "Error");
                }
            }
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExitApp) {
                Application.Exit();
            }
        }
    }
}
