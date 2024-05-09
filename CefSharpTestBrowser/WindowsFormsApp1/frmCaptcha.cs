using CSTool.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace CSTool
{
    public partial class frmCaptcha : Form
    {
        private Bitmap captchaImgBitmap;
        private float angle = 0f;
        private Debouncer debouncer = new Debouncer(750);

        public frmCaptcha()
        {
            InitializeComponent();
            // make imgCaptcha transparent to imgProgress
            imgCaptcha.Parent = imgProgress;
            captchaImgBitmap = new Bitmap(imgCaptcha.Image);
            Random rnd = new Random();
            float randomAngle = rnd.Next(1, 7) * 45f;
            Bitmap rotatedImage = RotateImage(captchaImgBitmap, angle+=randomAngle);
            imgCaptcha.Image = rotatedImage;
        }

        private void rotateClockwise(object sender, EventArgs e)
        {
            Bitmap rotatedImage = RotateImage(captchaImgBitmap, angle+=45f);
            imgCaptcha.Image = rotatedImage;
            checkCaptcha();
        }

        private void rotateCounterClockwise(object sender, EventArgs e)
        {
            Bitmap rotatedImage = RotateImage(captchaImgBitmap, angle-=45f);
            imgCaptcha.Image = rotatedImage;
            checkCaptcha();
        }

        public void checkCaptcha()
        {
            if (angle == 360f)
            {
                angle = 0;
            }
            imgProgress.Image = Properties.Resources.captchaLoader;
            debouncer.Debounce(() => {
                Invoke((Action)(() => {
                    if (angle == 0f)
                    {
                        imgProgress.Image = Properties.Resources.captchaRight;
                        Globals.ShowMessageDialog(this, "RIGHT!");
                        Close();
                    }
                    else
                    {
                        imgProgress.Image = Properties.Resources.captchaWrong;
                    }
                }));
            });
        }

        public Bitmap RotateImage(Bitmap bitmap, float angle)
        {
            Bitmap returnBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            using (Graphics graphics = Graphics.FromImage(returnBitmap))
            {
                graphics.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
                graphics.DrawImage(bitmap, new Point(0, 0));
            }
            return returnBitmap;
        }
    }
}
