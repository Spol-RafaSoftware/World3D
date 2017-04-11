using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    public class BitmapTexture
    {
        public Bitmap Bitmap { get { return bmp; } }
        
        Bitmap bmp;
        public BitmapTexture(int width, int height)
        {
            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawString("Test Bitmap", new Font("Tahoma", 18), Brushes.Red, width / 2, height / 2);
                g.Flush();
            }
        }
    }
}
