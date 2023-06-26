using System;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using CSTool.Class;
using CSTool.Properties;

namespace CSTool
{
    public partial class frmCamSettings : Form
    {

        StaffCam staffCam;
        public frmCamSettings()
        {
            InitializeComponent();
        }

        private void frmCamSettings_Load(object sender, EventArgs e)
        {
            loadCameraSources();
        }

        private void loadCameraSources()
        {
            cmbSource.Items.Clear();

            try
            {
                cmbSource.Text = "";
                //Enumerate all video input devices
                if (staffCam != null)
                {
                    staffCam.stopCamera();
                }
                staffCam = new StaffCam();
                staffCam.SetVideoPlaceHolder(pbVideoHandler);
                FilterInfoCollection videoDevices = staffCam.getVideoDevices();
                var i = 0;
                foreach (FilterInfo device in videoDevices)
                {
                    ;
                    cmbSource.Items.Add(device.Name);
                    if (!String.IsNullOrEmpty(Settings.Default.cam_source) && (device.MonikerString == Settings.Default.cam_source))
                    {
                        cmbSource.SelectedIndex = i;
                    }
                    i++;
                }


            }
            catch (ApplicationException)
            {
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadCameraSources();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (cmbSource.Items.Count == 0)
            {
                MessageBox.Show("Please select a camera source.");
                return;
            }
            if (!staffCam.isRunning())
            {
                MessageBox.Show("Camera setup error.");
                return;
            }
            Settings.Default.cam_source = staffCam.getVideoDevices()[cmbSource.SelectedIndex].MonikerString;
            Settings.Default.Save();
            this.Close();
        }

        private void cmbSource_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            staffCam.stopCamera();
            staffCam.startCamera(staffCam.getVideoDevices()[cmbSource.SelectedIndex].MonikerString);
        }

        private void frmCamSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            staffCam.stopCamera();
        }

    }
}