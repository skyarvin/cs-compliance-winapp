using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSTool
{
    public partial class frmConfirm : Form
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public string Button1Text { get; set; }
        public string Button2Text { get; set; }
        public bool Button2Visible { get; set; } = true;
        public frmConfirm()
        {
            InitializeComponent();

        }

        private void FrmConfirm_Load(object sender, EventArgs e)
        {
            RedesignForm();
        }
        public void RedesignForm()
        {
            lblTitle.Text = Title;
            lblMessage.Text = Message;
            btnOk.Text = Button1Text;
            btnCancel.Text = Button2Text;
            btnCancel.Visible = Button2Visible;
        }
    }
}
