namespace Caduhd.HandsDetector
{
    /// <summary>
    /// Normalizes a <see cref="Hand"/> obejct based on the size of the image in which the hand object can be found.
    /// </summary>
    public class HandNormalizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandNormalizer"/> class.
        /// </summary>
        /// <param name="imageWidth">The width of the image in which this hand can be found.</param>
        /// <param name="imageHeight">The height of the image in which this hand can be found.</param>
        public HandNormalizer(int imageWidth, int imageHeight)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        /// <summary>
        /// Normalizes a <see cref="Hand"/> object.
        /// </summary>
        /// <param name="hand"><see cref="Hand"/> object to normalize.</param>
        /// <returns>The normalized hand object.</returns>
        public NormalizedHand Normalize(Hand hand) =>
            new NormalizedHand(
                (double)hand.X / this.imageWidth,
                (double)hand.Y / this.imageHeight,
                (double)hand.Weight / (this.imageWidth * this.imageHeight));
    }
}
