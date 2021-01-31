using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector
{
    public static class ImageExtensions
    {
        public static Bgr GetPixelAt(this Image<Bgr, byte> image, int x, int y)
        {
            return new Bgr(image.Data[y, x, 0], image.Data[y, x, 1], image.Data[y, x, 2]);
        }

        public static void SetPixelAt(this Image<Bgr, byte> image, Bgr pixel, int x, int y)
        {
            image.Data[y, x, 0] = (byte)pixel.Blue;
            image.Data[y, x, 1] = (byte)pixel.Green;
            image.Data[y, x, 2] = (byte)pixel.Red;
        }

        public static bool IsWhite(this Bgr pixel) => pixel.Blue == 255 && pixel.Green == 255 && pixel.Red == 255;
    }
}
