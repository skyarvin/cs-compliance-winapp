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
                pnlWaiting.Visible = true;
                Application.DoEvents();
                Globals.Client = new Client(txtIPaddress.Text);
                MessageBox.Show(Globals.Client.Message);
                if (Globals.Client.IsConnected)
                {
                    Globals.Client.Send(new PairCommand { Action = "REQUEST_CACHE" });
                    PairCommand response = Globals.Client.Receive();
                    string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", response.Profile);
                    if (!Directory.Exists(temporary_cookies_directory))
                    {
                        Directory.CreateDirectory(temporary_cookies_directory);
                    }
                    Byte[] bytes = Convert.FromBase64String(response.Message);
                    string path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\",response.Profile,"\\Cookies");
                    File.WriteAllBytes(path, bytes);
                    Globals.Profile = response.Profile;
                    Globals.unixTimestamp = response.Timestamp;
                    // Now send the client cookie back to the server
                    Globals.Client.SendCache();
                }
                
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
