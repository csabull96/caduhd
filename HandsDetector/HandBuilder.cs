namespace Caduhd.HandsDetector
{
    /// <summary>
    /// Hand builder.
    /// </summary>
    public class HandBuilder
    {
        private int totalX;
        private int totalY;
        private int weight;

        /// <summary>
        /// Appends underlying hand obejct with an additional point.
        /// </summary>
        /// <param name="x">The x coordinate of the hand.</param>
        /// <param name="y">The y coordinate of the hand.</param>
        public void Append(int x, int y)
        {
            this.totalX += x;
            this.totalY += y;
            this.weight++;
        }

        /// <summary>
        /// Build a <see cref="Hand"/> object from the points that this builder object was appended with.
        /// </summary>
        /// <returns>The build <see cref="Hand"/> object.</returns>
        public Hand Build()
        {
            return this.weight == 0 ? new Hand() : new Hand(this.totalX / this.weight, this.totalY / this.weight, this.weight);
        }
    }
}
