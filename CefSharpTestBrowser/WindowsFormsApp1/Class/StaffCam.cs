using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections .Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Class
{


    public class StaffCam
    {
        FilterInfoCollection videoDevices = null;
        VideoCaptureDevice videoSource = null;
        System.Windows.Forms.PictureBox imgVideo = null;
        Bitmap bmpVideo = null;

        public StaffCam() {
            loadCameraSource();
        }

        public void SetVideoPlaceHolder(System.Windows.Forms.PictureBox videoPlaceHolder) {
            this.imgVideo = videoPlaceHolder;
        }
        private void loadCameraSource()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch (ApplicationException)
            {
                videoDevices = null;
            }

        }

        public FilterInfoCollection getVideoDevices() { 
            return videoDevices;
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            if (imgVideo != null) {
                imgVideo.Image = img;
            }
            bmpVideo = img;

        }

        public void stopCamera()
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }

        public void startCamera(string source)
        {
            try
            {
                videoSource = new VideoCaptureDevice(source);
                if (videoSource != null) {
                    videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                    videoSource.Start();
                }
                
            }
            catch
            {
                stopCamera();
            }
        }

        public bool isRunning() {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    return true;
                }
            return false;
        }

        public void captureImage(string filename) {
            if (bmpVideo != null) {
                try
                {
                    Bitmap resized = new Bitmap(bmpVideo, new Size(bmpVideo.Width / 4, bmpVideo.Height / 4));
                    resized.Save(filename);
                }
                catch (Exception)
                {
                    throw;
                }

            } 
        }
    }
}

