namespace Caduhd.HandsDetector
{
    using System;
    using Caduhd.Common;

    /// <summary>
    /// Hands color maps.
    /// </summary>
    public class HandsColorMaps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandsColorMaps"/> class.
        /// </summary>
        /// <param name="left">Color map of the left hand.</param>
        /// <param name="right">Color map of the right hand.</param>
        public HandsColorMaps(ColorMap left, ColorMap right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("Left hand's color map was null.");
            }

            if (right == null)
            {
                throw new ArgumentNullException("Left right's color map was null.");
            }

            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Gets the color map for the left hand.
        /// </summary>
        public ColorMap Left { get; private set; }

        /// <summary>
        /// Gets the color map for the right hand.
        /// </summary>
        public ColorMap Right { get; private set; }
    }
}
