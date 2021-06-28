using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSTool.Class;

namespace CSTool
{
    public partial class frmLogViewer : Form
    {
        public frmLogViewer()
        {
            InitializeComponent();
        }
        private void LoadData() {
            var source = new BindingSource();
            var list = Resync.GetLogs();
            source.DataSource = list;
            dgErrorLogs.DataSource = source;
            lblRecordCount.Text = string.Concat("No. of records ", list.Count.ToString());
        }
        private void FrmLogViewer_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
