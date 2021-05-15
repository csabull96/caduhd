namespace Caduhd.Common.Tests
{
    using Xunit;

    public class DoubleRangeTests
    {
        private const double LOWER_BOUND = 123.456;
        private const double UPPER_BOUND = 234.567;

        private readonly DoubleRange doubleRange;

        public DoubleRangeTests()
        {
            this.doubleRange = new DoubleRange(LOWER_BOUND, UPPER_BOUND);
        }

        [Fact]
        public void LowerBoundGetter_PropertyCorrectlySetThroughConstructor()
        {
            Assert.Equal(LOWER_BOUND, this.doubleRange.LowerBound);
        }

        [Fact]
        public void UpperBoundGetter_PropertyCorrectlySetThroughConstructor()
        {
            Assert.Equal(LOWER_BOUND, this.doubleRange.LowerBound);
        }

        [Theory]
        [InlineData(133)]
        [InlineData(147)]
        [InlineData(201)]
        public void IsWithinRange_NumberGreaterThanLowerBoundSmallerThanUpperBound_ReturnsTrue(int value)
        {
            Assert.True(this.doubleRange.IsWithinRange(value));
        }

        [Fact]
        public void IsWithinRange_InclusiveLowerBoundEdgeCase_ReturnsTrue()
        {
            Assert.True(this.doubleRange.IsWithinRange(LOWER_BOUND));
        }

        [Fact]
        public void IsWithinRange_InclusiveUpperBoundEdgeCase_ReturnsTrue()
        {
            Assert.True(this.doubleRange.IsWithinRange(UPPER_BOUND));
        }

        [Fact]
        public void IsWithinRange_ExclusiveLowerBoundEdgeCase_ReturnsFalse()
        {
            Assert.False(this.doubleRange.IsWithinRange(LOWER_BOUND, isLowerBoundInclusive: false));
        }

        [Fact]
        public void IsWithinRange_ExclusiveUpperBoundEdgeCase_ReturnsFalse()
        {
            Assert.False(this.doubleRange.IsWithinRange(UPPER_BOUND, isUpperBoundInclusive: false));
        }
    }
}
