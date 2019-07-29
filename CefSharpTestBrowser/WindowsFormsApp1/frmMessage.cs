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
        public frmMessage()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Globals._wentIdle = DateTime.MaxValue;
            Globals._idleTicks = 0;
        }
    }
}
