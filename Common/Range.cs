namespace Caduhd.Common
{
    /// <summary>
    /// Represent a range with a specified lower and upper bound.
    /// </summary>
    /// <typeparam name="T">Type of the range.</typeparam>
    public abstract class Range<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> class.
        /// </summary>
        /// <param name="lowerBound">The lower bound of the range.</param>
        /// <param name="upperBound">The upper bound of the range.</param>
        public Range(T lowerBound, T upperBound)
        {
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }

        /// <summary>
        /// Gets the lower bound of the range.
        /// </summary>
        public T LowerBound { get; private set; }

        /// <summary>
        /// Gets the upper bound of the range.
        /// </summary>
        public T UpperBound { get; private set; }

        /// <summary>
        /// Determines whether the provided value is within the range or not.
        /// </summary>
        /// <param name="value">The value to examine.</param>
        /// <param name="isLowerBoundInclusive">Sets the lower bound to inclusive or exclusive. By default it's inclusive.</param>
        /// <param name="isUpperBoundInclusive">Sets the upper bound to inclusive or exclusive. By default it's inclusive.</param>
        /// <returns>True if value is within the range, false otherwise.</returns>
        public abstract bool IsWithinRange(T value, bool isLowerBoundInclusive = true, bool isUpperBoundInclusive = true);
    }
}
