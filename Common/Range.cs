namespace Caduhd.Common
{
    public abstract class Range<T>
    {
        public T LowerBound { get; private set; }
        public T UpperBound { get; private set; }

        public Range(T lowerBound, T upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public abstract bool IsWithinRange(T value, bool isLowerBoundInclusive = true, bool isUpperBoundInclusive = true);
    }
}
