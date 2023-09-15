using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;

namespace CSTool.Class
{
    public class StaffCam
    {
        FilterInfoCollection videoDevices = null;
        VideoCaptureDevice videoSource = null;
        System.Windows.Forms.PictureBox imgVideo = null;
        Bitmap bmpVideo = null;

        public StaffCam()
        {
            loadCameraSource();
        }

        public void SetVideoPlaceHolder(System.Windows.Forms.PictureBox videoPlaceHolder)
        {
            this.imgVideo = videoPlaceHolder;
        }

        private void loadCameraSource()
        {
            try
            {
                this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch (ApplicationException)
            {
                this.videoDevices = null;
            }
        }

        public FilterInfoCollection getVideoDevices()
        {
            return this.videoDevices;
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            if (this.imgVideo != null)
            {
                this.imgVideo.Image = img;
            }
            this.bmpVideo = img;
        }

        public void stopCamera()
        {
            if (this.videoSource != null && this.videoSource.IsRunning)
            {
                this.videoSource.SignalToStop();
                this.videoSource = null;
            }
        }

        public void startCamera(string source)
        {
            try
            {
                this.videoSource = new VideoCaptureDevice(source);
                if (this.videoSource != null)
                {
                    this.videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                    this.videoSource.Start();
                }
            }
            catch
            {
                stopCamera();
            }
        }

        public bool isRunning()
        {
            if (this.videoSource != null && this.videoSource.IsRunning)
            {
                return true;
            }
            return false;
        }

        public void captureImage(string filename)
        {
            if (this.bmpVideo != null)
            {
                try
                {
                    Bitmap resized = new Bitmap(this.bmpVideo, new Size(this.bmpVideo.Width / 4, this.bmpVideo.Height / 4));
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