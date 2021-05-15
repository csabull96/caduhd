namespace Caduhd.HandsDetector
{
    using System;

    /// <summary>
    /// A hand object that is described with its normalized position and weight.
    /// </summary>
    public class NormalizedHand
    {
        private const string INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE =
            "{0} has to be greater or equal than 0.0 and smaller or equal than 1.0. Value was: {1}";

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedHand"/> class.
        /// </summary>
        public NormalizedHand()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedHand"/> class.
        /// </summary>
        /// <param name="x">The normalized x coordinate of the hand's position.</param>
        /// <param name="y">The normalized y coordinate of the hand's position.</param>
        /// <param name="weight">The normalized weight of the hand.</param>
        public NormalizedHand(double x, double y, double weight)
        {
            bool IsNormalizedValueInvalid(double value) =>
                value < 0 || 1 < value;

            if (IsNormalizedValueInvalid(x))
            {
                throw new ArgumentException(string.Format(INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE, "X", this.X));
            }

            if (IsNormalizedValueInvalid(y))
            {
                throw new ArgumentException(string.Format(INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE, "Y", this.Y));
            }

            if (IsNormalizedValueInvalid(weight))
            {
                throw new ArgumentException(string.Format(INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE, "Weight", this.Weight));
            }

            this.X = x;
            this.Y = y;
            this.Weight = weight;
        }

        /// <summary>
        /// Gets the normalized x coordinate of the hand's position.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets the normalized y coordinate of the hand's position.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Gets the normalized weight of the hand.
        /// </summary>
        public double Weight { get; private set; }
    }
}
