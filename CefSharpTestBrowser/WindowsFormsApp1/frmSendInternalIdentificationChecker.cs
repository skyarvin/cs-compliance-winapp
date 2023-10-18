using CefSharp.WinForms.Internals;
using CSTool.Class;
using CSTool.Models;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class frmSendInternalIdentificationChecker : Form
    {
        string filePath = "";

        public frmSendInternalIdentificationChecker()
        {
            InitializeComponent();
        }

        private void frmSendInternalIdentificationChecker_Load(object sender, EventArgs e)
        {
            lblUrl.Text = Globals.CurrentUrl;
            txtNotes.Text = "";
        }

        private void btnSendIIDC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNotes.Text.Trim()))
            {
                MessageBox.Show("Notes cannot be empty!");
                txtNotes.Focus();
                return;
            }

            string b64string = "";
            if (!string.IsNullOrEmpty(this.filePath))
            {
                b64string = this.ResizeImage(File.ReadAllBytes(this.filePath));
            }

            var start_time = Globals.StartTime_LastAction;
            if (start_time == null)
            {
                start_time = Globals.frmMain.StartTime_BrowserChanged;
            }

            InternalIdentificationChecker internalIdentificationChecker = new InternalIdentificationChecker()
            {
                url = Globals.CurrentUrl,
                agent_id = Globals.Profile.AgentID,
                agent_notes = txtNotes.Text,
                agent_uploaded_photo_base64 = b64string,
                duration = (int)((DateTime.Now - (DateTime)start_time).TotalSeconds)
            };

            var result = internalIdentificationChecker.Save();
            if (result != null)
            {
                Globals.INTERNAL_IIDC = result;
                Settings.Default.Save();
                Globals.frmMain.StartbgWorkIIDC();
                this.DialogResult = DialogResult.OK;
            }
        }

        private string ResizeImage(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                try
                {
                    Bitmap bmp = new Bitmap(ms);
                    Bitmap resized = new Bitmap(bmp, new Size(bmp.Width / 2, bmp.Height / 2));
                    ImageConverter converter = new ImageConverter();
                    data = (byte[])converter.ConvertTo(resized, typeof(byte[]));
                    return Convert.ToBase64String(data);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void uploadbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;.*.gif;)|*.bmp;*.jpg;*.jpeg;.*.gif";
            if (opnfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = System.IO.Path.GetFileName(opnfd.FileName);
                upload_photo.Text = filename;
                filePath = opnfd.FileName;
            }

        }
    }
}