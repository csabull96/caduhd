using Caduhd.Common;
using Caduhd.HandsDetector;
using System.Drawing;
using Xunit;

namespace Caduhd.HandDetector.Tests
{
    public class HandsDetectorResultTests
    {
        private NormalizedHands _hands;
        private BgrImage _image;
        private HandsDetectorResult _handsDetectorResult;

        public HandsDetectorResultTests()
        {
            _hands = new NormalizedHands(new NormalizedHand(), new NormalizedHand());
            _image = BgrImage.GetBlank(100, 100, Color.White);
            _handsDetectorResult = new HandsDetectorResult(_hands, _image);
        }

        [Fact]
        public void HandsGetter_ReturnsHandsObjectThatWasSetThroughConstructor()
        {
            Assert.Same(_hands, _handsDetectorResult.Hands);
        }

        [Fact]
        public void ImageGetter_ReturnsImageThatWasSetThroughConstructor()
        {
            Assert.Same(_image, _handsDetectorResult.Image);
        }
    }
}
