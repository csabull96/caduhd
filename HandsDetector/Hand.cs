namespace Caduhd.HandsDetector
{
    /// <summary>
    /// Describes a hand with its absolute position and weight.
    /// </summary>
    public class Hand
    {
        /// <summary>
        /// Gets the x coordinate of the hand's position.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the y coordinate of the hand's position.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Gets the weight of the hand.
        /// </summary>
        public int Weight { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        public Hand()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        /// <param name="x">The x coordinate of the hand's position.</param>
        /// <param name="y">The y coordinate of the hand's position.</param>
        /// <param name="weight">The weight of the hand.</param>
        public Hand(int x, int y, int weight)
        {
            this.X = x;
            this.Y = y;
            this.Weight = weight;
        }
    }
}
