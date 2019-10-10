using CefSharp.WinForms.Internals;
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
            PairConnect();
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
            if (!string.IsNullOrEmpty(SkydevCSTool.Properties.Settings.Default.server_ip))
                txtIPaddress.Text = SkydevCSTool.Properties.Settings.Default.server_ip;
        }

        private void TxtIPaddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PairConnect();
            }
        }
        private void PairConnect()
        {
            if (ValidateIP(txtIPaddress.Text))
            {

                string target_ip = txtIPaddress.Text;
                SkydevCSTool.Properties.Settings.Default.server_ip = target_ip;
                Globals.frmMain.SetBtnConnectText("Waiting..");
                Application.DoEvents();
                Task.Factory.StartNew(() =>
                {
                    AsynchronousClient.StartClient(target_ip);
                });
                this.Close();
            }
        }
    }
}
