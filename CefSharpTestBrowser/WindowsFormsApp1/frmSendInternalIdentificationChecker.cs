﻿using CefSharp.WinForms.Internals;
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

            string imagePath = "";
            if (!string.IsNullOrEmpty(this.filePath))
            {
                try
                {
                    using (var bmp = new Bitmap(this.filePath))
                    {
                        if (Globals.ShouldResizeImage(File.ReadAllBytes(this.filePath).Length))
                        {
                            MessageBox.Show("File is too big! File size exceeds 2MB.");
                            return;
                        }
                        imagePath = this.filePath;
                    }
                }
                catch
                {
                    MessageBox.Show("Couldn't complete your request due to invalid or corrupt image file.");
                    return;
                }
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
                agent_uploaded_photo = imagePath,
                duration = (int)((ServerTime.Now() - (DateTime)start_time).TotalSeconds)
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