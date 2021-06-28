using CSTool.Models;
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

namespace CSTool
{
    public partial class frmMessage : Form
    {

        public frmMessage(string Message)
        {
            InitializeComponent();
            lblMessage.Text = Message;
        }
    }
}
