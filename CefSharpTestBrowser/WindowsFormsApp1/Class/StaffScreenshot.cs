using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
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
                var image = this.CaptureDesktop();
                image.Save(filename, ImageFormat.Jpeg);
                this.checkImageSize(filename, image);
            }
            catch {
                return;
            }
        }

        private void checkImageSize(string filename, Bitmap image)
        {
            if (Globals.ShouldResizeImage(new System.IO.FileInfo(filename).Length))
            {
                image = new Bitmap(image, new Size(image.Width / 2, image.Height / 2));
                image.Save(filename, ImageFormat.Jpeg);
            }
        }
    }
}
