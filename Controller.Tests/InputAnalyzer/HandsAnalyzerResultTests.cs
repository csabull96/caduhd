using Caduhd.Common;
using Caduhd.Controller.InputAnalyzer;
using Caduhd.HandsDetector;
using Moq;
using System.Drawing;
using Xunit;

namespace Caduhd.Controller.Tests.InputAnalyzer
{
    public class HandsAnalyzerResultTests
    {
        private HandsColorMaps _handsColorMaps;
        private BgrImage _handsBackground;
        private BgrImage _handsForeground;
        private HandsAnalyzerResult _handsAnalyzerResult;

        public HandsAnalyzerResultTests()
        {
            var histogramMock = new Mock<IHistogram>();
            var colorMap = new ColorMap(histogramMock.Object, histogramMock.Object, histogramMock.Object);

            _handsColorMaps = new HandsColorMaps(colorMap, colorMap);
            _handsBackground = BgrImage.GetBlank(100, 100, Color.Red);
            _handsForeground = BgrImage.GetBlank(100, 100, Color.Green);
            _handsAnalyzerResult = new HandsAnalyzerResult(_handsColorMaps, _handsBackground, _handsForeground);
        }

        [Fact]
        public void HandsColorMapsGetter_ReturnsTheObjectSetThroughTheConstructor()
        {
            Assert.Same(_handsColorMaps, _handsAnalyzerResult.HandsColorMaps);
        }

        [Fact]
        public void HandsBackgroundGetter_ReturnsTheObjectSetThroughTheConstructor()
        {
            Assert.Same(_handsBackground, _handsAnalyzerResult.HandsBackground);
        }

        [Fact]
        public void HandsForegroundGetter_ReturnsTheObjectSetThroughTheConstructor()
        {
            Assert.Same(_handsForeground, _handsAnalyzerResult.HandsForeground);
        }
    }
}
