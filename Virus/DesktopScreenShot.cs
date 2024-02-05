using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace Virus
{

    public class DesktopScreenShot
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        static Size GetDesktopSize()
        {
 

            IntPtr desktopHandle = GetDesktopWindow();
            IntPtr desktopDC = GetDC(desktopHandle);

            int screenWidth = GetSystemMetrics(0);  // SM_CXSCREEN
            int screenHeight = GetSystemMetrics(1); // SM_CYSCREEN

            ReleaseDC(desktopHandle, desktopDC);

            return new Size(screenWidth, screenHeight);
        }

        public void CaptureDesktop(string fullPath)
        {
            try
            {
                Size desktopSize = GetDesktopSize();

                using (Bitmap screenshot = new Bitmap(desktopSize.Width, desktopSize.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(screenshot))
                    {
                        IntPtr hdc = GetDC(GetDesktopWindow());
                        graphics.CopyFromScreen(0, 0, 0, 0, desktopSize);
                        ReleaseDC(GetDesktopWindow(), hdc);
                    }

                    // Ekran görüntüsünü direkt olarak masaüstüne kaydet
                    screenshot.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
    }
}