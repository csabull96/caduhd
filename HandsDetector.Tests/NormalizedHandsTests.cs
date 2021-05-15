namespace Caduhd.HandsDetector.Tests
{
    using Xunit;

    public class NormalizedHandsTests
    {
        private const double LEFT_X = 0.32;
        private const double LEFT_Y = 0.64;
        private const double LEFT_WEIGHT = 0.12;
        private const double RIGHT_X = 0.55;
        private const double RIGHT_Y = 0.47;
        private const double RIGHT_WEIGHT = 0.17;

        private readonly NormalizedHand left;
        private readonly NormalizedHand right;
        private readonly NormalizedHands hands;

        public NormalizedHandsTests()
        {
            this.left = new NormalizedHand(LEFT_X, LEFT_Y, LEFT_WEIGHT);
            this.right = new NormalizedHand(RIGHT_X, RIGHT_Y, RIGHT_WEIGHT);
            this.hands = new NormalizedHands(this.left, this.right);
        }

        [Fact]
        public void LeftGetter_ReturnsLeftHandPassedToConstructor()
        {
            Assert.Same(this.left, this.hands.Left);
        }

        [Fact]
        public void RightGetter_ReturnsRightHandPassedToConstructor()
        {
            Assert.Same(this.right, this.hands.Right);
        }

        [Fact]
        public void CenterGetter_ReturnsCenterPointOfHands()
        {
            double centerX = (this.left.X + this.right.X) / 2;
            double centerY = (this.left.Y + this.right.Y) / 2;

            Assert.Equal(centerX, this.hands.Center.X);
            Assert.Equal(centerY, this.hands.Center.Y);
        }

        [Fact]
        public void RatioOfLeftWeightToRightWeightGetter_ReturnsRatioOfHandsWeightCorrectly()
        {
            Assert.Equal(LEFT_WEIGHT / RIGHT_WEIGHT, this.hands.RatioOfLeftWeightToRightWeight);
        }
    }
}
