namespace Caduhd.HandsDetector.Tests
{
    using Xunit;

    public class HandBuilderTests
    {
        private HandBuilder handBuilder;

        public HandBuilderTests()
        {
            this.handBuilder = new HandBuilder();
        }

        [Theory]
        [InlineData(123, 77)]
        [InlineData(11, 98)]
        [InlineData(47, 101)]
        [InlineData(38, 56)]
        public void Append_OnePointAdded_HandMatchesPoint(int x, int y)
        {
            this.handBuilder.Append(x, y);
            var hand = this.handBuilder.Build();
            Assert.Equal(x, hand.X);
            Assert.Equal(y, hand.Y);
            Assert.Equal(1, hand.Weight);
        }

        [Theory]
        [InlineData(123, 77)]
        [InlineData(11, 98)]
        [InlineData(47, 101)]
        [InlineData(38, 56)]
        public void Append_MultiplePointsAdded_HandAppendedAndBuiltCorrectly(int width, int height)
        {
            int totalX = 0;
            int totalY = 0;
            int weight = width * height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    totalX += x;
                    totalY += y;

                    this.handBuilder.Append(x, y);
                }
            }

            var hand = this.handBuilder.Build();

            Assert.Equal(totalX / weight, hand.X);
            Assert.Equal(totalY / weight, hand.Y);
            Assert.Equal(weight, hand.Weight);
        }
    }
}
