namespace Caduhd.Common
{
    using System;
    using System.Drawing;
    using Emgu.CV.Structure;

    /// <summary>
    /// A wrapper class around the Emgu.CV.Structure.Bgr class.
    /// </summary>
    public class BgrPixel
    {
        private Bgr pixel;

        /// <summary>
        /// Gets the blue channal value of the pixel.
        /// </summary>
        public int Blue => Convert.ToInt32(this.pixel.Blue);

        /// <summary>
        /// Gets the green channal value of the pixel.
        /// </summary>
        public int Green => Convert.ToInt32(this.pixel.Green);

        /// <summary>
        /// Gets the red channal value of the pixel.
        /// </summary>
        public int Red => Convert.ToInt32(this.pixel.Red);

        /// <summary>
        /// Initializes a new instance of the <see cref="BgrPixel"/> class.
        /// </summary>
        /// <param name="color">The color of the pixel.</param>
        public BgrPixel(Color color) : this(color.B, color.G, color.R) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BgrPixel"/> class.
        /// </summary>
        /// <param name="blue">The blue channal value.</param>
        /// <param name="green">The green channal value.</param>
        /// <param name="red">The red channal value.</param>
        public BgrPixel(int blue, int green, int red)
        {
            this.pixel = new Bgr(blue, green, red);
        }

        public override bool Equals(object comparand)
        {
            if (comparand is BgrPixel bgrPixel)
            {
                return Blue == bgrPixel.Blue && Green == bgrPixel.Green && Red == bgrPixel.Red;
            }
            else if (comparand is Color color)
            {
                return Blue == color.B && Green == color.G && Red == color.R;
            }
            else if (comparand is Bgr bgr)
            {
                return Blue == bgr.Blue && Green == bgr.Green && Red == bgr.Red;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
