namespace Caduhd.Common
{
    public class DoubleRange : Range<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class.
        /// </summary>
        /// <param name="lowerBound">The lower bound of the range.</param>
        /// <param name="upperBound">The upper bound of the range.</param>
        public DoubleRange(double lowerBound, double upperBound)
            : base(lowerBound, upperBound)
        {
        }

        /// <summary>
        /// Determines whether the provided value is within the range or not.
        /// </summary>
        /// <param name="value">The value to examine.</param>
        /// <param name="isLowerBoundInclusive">Sets the lower bound to inclusive or exclusive. By default it's inclusive.</param>
        /// <param name="isUpperBoundInclusive">Sets the upper bound to inclusive or exclusive. By default it's inclusive.</param>
        /// <returns>True if value is within the range, false otherwise.</returns>
        public override bool IsWithinRange(double value, bool isLowerBoundInclusive = true, bool isUpperBoundInclusive = true)
        {
            bool lowerBoundCheck = isLowerBoundInclusive ? LowerBound <= value : LowerBound < value;
            bool upperBoundCheck = isUpperBoundInclusive ? value <= UpperBound : value < UpperBound;
            return lowerBoundCheck && upperBoundCheck;
        }
    }
}
