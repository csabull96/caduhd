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
        /// Initializes a new instance of the <see cref="BgrPixel"/> class.
        /// </summary>
        /// <param name="color">The color of the pixel.</param>
        public BgrPixel(Color color)
            : this(color.B, color.G, color.R)
        {
        }

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
        /// The overriden <see cref="Equals(object)"/> method.
        /// </summary>
        /// <param name="comparand">The comparand.</param>
        /// <returns>True if they are equals, false otherwise.</returns>
        public override bool Equals(object comparand)
        {
            if (comparand is BgrPixel bgrPixel)
            {
                return this.Blue == bgrPixel.Blue && this.Green == bgrPixel.Green && this.Red == bgrPixel.Red;
            }
            else if (comparand is Color color)
            {
                return this.Blue == color.B && this.Green == color.G && this.Red == color.R;
            }
            else if (comparand is Bgr bgr)
            {
                return this.Blue == bgr.Blue && this.Green == bgr.Green && this.Red == bgr.Red;
            }

            return false;
        }

        /// <summary>
        /// The overriden <see cref="GetHashCode"/> method.
        /// </summary>
        /// <returns>The hash code of this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
