namespace Caduhd.Common
{
    /// <summary>
    /// Represents a 2D point with double floating point coordinates.
    /// </summary>
    public class PointD
    {
        /// <summary>
        /// Gets the x coordinate of the PointD object.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets the y coordinate of the PointD object.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointD"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
