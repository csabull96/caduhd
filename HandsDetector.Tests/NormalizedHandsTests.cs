using Caduhd.HandsDetector;
using Xunit;

namespace Caduhd.HandDetector.Tests
{
    public class NormalizedHandsTests
    {
        private const double LEFT_X = 0.32;
        private const double LEFT_Y = 0.64;
        private const double LEFT_WEIGHT = 0.12;
        private const double RIGHT_X = 0.55;
        private const double RIGHT_Y = 0.47;
        private const double RIGHT_WEIGHT = 0.17;

        private NormalizedHand _left;
        private NormalizedHand _right;
        private NormalizedHands _hands;

        public NormalizedHandsTests()
        {
            _left = new NormalizedHand(LEFT_X, LEFT_Y, LEFT_WEIGHT);
            _right = new NormalizedHand(RIGHT_X, RIGHT_Y, RIGHT_WEIGHT);
            _hands = new NormalizedHands(_left, _right);
        }

        [Fact]
        public void LeftGetter_ReturnsLeftHandPassedToConstructor()
        {
            Assert.Same(_left, _hands.Left);
        }

        [Fact]
        public void RightGetter_ReturnsRightHandPassedToConstructor()
        {
            Assert.Same(_right, _hands.Right);
        }

        [Fact]
        public void CenterGetter_ReturnsCenterPointOfHands()
        {
            double centerX = (_left.X + _right.X) / 2;
            double centerY = (_left.Y + _right.Y) / 2;

            Assert.Equal(centerX, _hands.Center.X);
            Assert.Equal(centerY, _hands.Center.Y);
        }

        [Fact]
        public void RatioOfLeftWeightToRightWeightGetter_ReturnsRatioOfHandsWeightCorrectly()
        {
            Assert.Equal(LEFT_WEIGHT / RIGHT_WEIGHT, _hands.RatioOfLeftWeightToRightWeight);
        }

    }
}
