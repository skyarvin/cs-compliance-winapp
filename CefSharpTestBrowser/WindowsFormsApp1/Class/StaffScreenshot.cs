﻿using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

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
            }

            catch {
                return;
            }
            
        }

    }
}
