namespace Caduhd.HandsDetector.Tests
{
    using Caduhd.HandsDetector;
    using Xunit;

    public class HandNormalizerTests
    {
        [Theory]
        [InlineData(43, 73, 890, 120, 140)]
        [InlineData(99, 11, 739, 150, 150)]
        [InlineData(13, 44, 299, 101, 103)]
        [InlineData(0, 0, 0, 150, 150)]
        [InlineData(101, 103, 101 * 103, 101, 103)]
        public void Normalize_NormalizesHandCorrectly(int absoluteX, int absoluteY, int absoluteWeight, int imageWidth, int imageHeight)
        {
            var hand = new Hand(absoluteX, absoluteY, absoluteWeight);
            var normalizedHands = new HandNormalizer(imageWidth, imageHeight).Normalize(hand);

            Assert.Equal((double)hand.X / imageWidth, normalizedHands.X);
            Assert.Equal((double)hand.Y / imageHeight, normalizedHands.Y);
            Assert.Equal((double)hand.Weight / (imageWidth * imageHeight), normalizedHands.Weight);
        }
    }
}
