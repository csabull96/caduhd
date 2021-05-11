namespace Caduhd.HandsDetector
{
    using Caduhd.Common;

    /// <summary>
    /// A container class for a normalized left and right hand.
    /// </summary>
    public class NormalizedHands
    {
        /// <summary>
        /// Gets the normalized left hand.
        /// </summary>
        public NormalizedHand Left { get; private set; }

        /// <summary>
        /// Gets the normalized right hand.
        /// </summary>
        public NormalizedHand Right { get; private set; }

        /// <summary>
        /// Gets the normalized center of the hands.
        /// </summary>
        public PointD Center { get; private set; }

        /// <summary>
        /// Gets the weight ratio of the normalized left and right hand.
        /// </summary>
        public double RatioOfLeftWeightToRightWeight { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedHands"/> class.
        /// </summary>
        /// <param name="left">The normalized left hand.</param>
        /// <param name="right">The normalized right hand.</param>
        public NormalizedHands(NormalizedHand left, NormalizedHand right)
        {
            this.Left = left;
            this.Right = right;
            double centerX = (left.X + right.X) / 2;
            double centerY = (left.Y + right.Y) / 2;
            this.Center = new PointD(centerX, centerY);
            this.RatioOfLeftWeightToRightWeight = left.Weight / right.Weight;
        }
    }
}
