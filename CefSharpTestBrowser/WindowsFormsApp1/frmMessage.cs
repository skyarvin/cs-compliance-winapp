using SkydevCSTool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace SkydevCSTool
{
    public partial class frmMessage : Form
    {

        public frmMessage(string Message)
        {
            InitializeComponent();
            lblMessage.Text = Message;
        }

      

        private void FrmMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Globals.SaveActivity();

            Globals._wentIdle = DateTime.MaxValue;
            Globals._idleTicks = 0;
        }
    }
}
