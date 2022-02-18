using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermalCameraNet
{
    public class ImageUnit
    {
        // Import Windows GDI conponent and including DeleteObject() function.
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static void BindBitmapToPicture(PictureBox picBox, Bitmap bitmap)
        {
            // Convert .net Bitmap to hBitmap
            IntPtr gdibitmap = bitmap.GetHbitmap();
            // load hHitmap into PictureBox
            picBox.Image = Image.FromHbitmap(gdibitmap);
            // Release hBitmap recourse 
            DeleteObject(gdibitmap);
        }
    }
}
