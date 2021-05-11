namespace Caduhd.HandsDetector
{
    using Caduhd.Common;

    /// <summary>
    /// A container class for the objects that are considered as the result of a hand detection.
    /// </summary>
    public class HandsDetectorResult
    {
        /// <summary>
        /// Gets the detected hands.
        /// </summary>
        public NormalizedHands Hands { get; private set; }

        /// <summary>
        /// Gets the image of the hand detection.
        /// </summary>
        public BgrImage Image { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandsDetectorResult"/> class.
        /// </summary>
        /// <param name="hands">The detected hands.</param>
        /// <param name="image">The image of the hand detection.</param>
        public HandsDetectorResult(NormalizedHands hands, BgrImage image)
        {
            this.Hands = hands;
            this.Image = image;
        }
    }
}
