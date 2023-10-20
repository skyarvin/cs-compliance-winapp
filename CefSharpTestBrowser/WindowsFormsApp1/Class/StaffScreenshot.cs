using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace CSTool.Class
{
    public class StaffScreenshot
    {
        public Bitmap CaptureDesktop()
        {
            Rectangle combinedBounds = Screen.AllScreens.Select(screen => screen.Bounds).Aggregate(Rectangle.Union);
            Bitmap screenshot = new Bitmap(combinedBounds.Width, combinedBounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    graphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.X - combinedBounds.X, screen.Bounds.Y - combinedBounds.Y, screen.Bounds.Size);
                }
            }

            return screenshot;
        }

        public void captureScreenshot(string filename)
        {
            try {
                var image = CaptureDesktop();
                image.Save(filename, ImageFormat.Jpeg);
                this.resizeImage(filename, image);
            }
            catch {
                return;
            }
        }

        private void resizeImage(string filename, Bitmap image)
        {
            int twoMegaByte = 2048;
            int fileSizeInKb = (int)(new System.IO.FileInfo(filename).Length / 1024);
            if (fileSizeInKb > twoMegaByte)
            {
                image = new Bitmap(image, new Size(image.Width / 2, image.Height / 2));
                image.Save(filename, ImageFormat.Jpeg);
            }
        }
    }
}
