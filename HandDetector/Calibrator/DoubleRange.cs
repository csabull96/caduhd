namespace Ksvydo.HandDetector.Calibrator
{
    public class DoubleRange
    {
        public double LowerBound { get; private set; }
        public double UpperBound { get; private set; }

        public DoubleRange(double lowerBound, double upperBound)
        {
            UpperBound = upperBound;
            LowerBound = lowerBound;
        }

        public bool IsWithinRange(double value, bool isLowerBoundInclusive = true, bool isUpperBoundInclusive = true)
        {
            bool lowerBoundCheck = isLowerBoundInclusive ? LowerBound <= value : LowerBound < value;
            bool upperBoundCheck = isUpperBoundInclusive ? value <= UpperBound : value < UpperBound;
            return lowerBoundCheck && upperBoundCheck;
        }
    }
}
