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

        public static bool operator ==(BgrPixel a, BgrPixel b) => a.Equals(b);

        public static bool operator !=(BgrPixel a, BgrPixel b) => !a.Equals(b);

        public override bool Equals(object obj)
        {
            BgrPixel pixel = obj as BgrPixel;
            return pixel != null && Blue.Equals(pixel.Blue) && Green.Equals(pixel.Green) && Red.Equals(pixel.Red);
        }

        public override int GetHashCode() => (Blue + Green + Red).GetHashCode();
    }
}
