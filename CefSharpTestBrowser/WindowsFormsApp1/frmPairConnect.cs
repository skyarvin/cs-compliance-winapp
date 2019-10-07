﻿using CefSharp.WinForms.Internals;
using SkydevCSTool.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.IO;
using System.Threading;

namespace SkydevCSTool
{
    public partial class frmPairConnect : Form
    {
        public frmPairConnect()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if(ValidateIP(txtIPaddress.Text))
            {
                string target_ip = txtIPaddress.Text;
                pnlWaiting.Visible = true;
                Application.DoEvents();
                Task.Factory.StartNew(() =>
                {
                    Thread.CurrentThread.Name = "ClientSocketsThread";
                    Globals.frmMain.ClientThread = Thread.CurrentThread;
                    AsynchronousClient.StartClient(target_ip);
                });
                this.Close();
            }
        }

        private bool ValidateIP(string ip)
        {
            try
            {
                IPAddress.Parse(ip);
            }
            catch(FormatException e)
            {
                MessageBox.Show("Invalid IP Address", "Error");
                return false;
            }

            return true;
        }

        private void FrmPairConnect_Load(object sender, EventArgs e)
        {

        }
    }
}
