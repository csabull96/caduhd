using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace Caduhd.Common
{
    public class BgrPixel
    {
        private Bgr _pixel;

        public int Blue => Convert.ToInt32(_pixel.Blue);

        public int Green => Convert.ToInt32(_pixel.Green);

        public int Red => Convert.ToInt32(_pixel.Red);

        public BgrPixel(Color color) : this(color.B, color.G, color.R) { }

        public BgrPixel(int blue, int green, int red)
        {
            _pixel = new Bgr(blue, green, red);
        }
    }
}
