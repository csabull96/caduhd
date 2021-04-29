namespace Caduhd.Common
{
    public class DoubleRange : Range<double>
    {
        public DoubleRange(double lowerBound, double upperBound) : base(lowerBound, upperBound)
        {

        }

        public override bool IsWithinRange(double value, bool isLowerBoundInclusive = true, bool isUpperBoundInclusive = true)
        {
            bool lowerBoundCheck = isLowerBoundInclusive ? LowerBound <= value : LowerBound < value;
            bool upperBoundCheck = isUpperBoundInclusive ? value <= UpperBound : value < UpperBound;
            return lowerBoundCheck && upperBoundCheck;
        }
    }
}
