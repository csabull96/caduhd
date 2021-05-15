namespace Caduhd.HandsDetector.Tests
{
    using System.Drawing;
    using Caduhd.Common;
    using Xunit;

    public class HandsDetectorResultTests
    {
        private readonly NormalizedHands hands;
        private readonly BgrImage image;
        private readonly HandsDetectorResult handsDetectorResult;

        public HandsDetectorResultTests()
        {
            this.hands = new NormalizedHands(new NormalizedHand(), new NormalizedHand());
            this.image = BgrImage.GetBlank(100, 100, Color.White);
            this.handsDetectorResult = new HandsDetectorResult(this.hands, this.image);
        }

        [Fact]
        public void HandsGetter_ReturnsHandsObjectThatWasSetThroughConstructor()
        {
            Assert.Same(this.hands, this.handsDetectorResult.Hands);
        }

        [Fact]
        public void ImageGetter_ReturnsImageThatWasSetThroughConstructor()
        {
            Assert.Same(this.image, this.handsDetectorResult.Image);
        }
    }
}
