namespace Caduhd.Controller.Tests.InputAnalyzer
{
    using System.Drawing;
    using Caduhd.Common;
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.HandsDetector;
    using Moq;
    using Xunit;

    public class HandsAnalyzerResultTests
    {
        private readonly HandsColorMaps handsColorMaps;
        private readonly BgrImage handsBackground;
        private readonly BgrImage handsForeground;
        private readonly HandsAnalyzerResult handsAnalyzerResult;

        public HandsAnalyzerResultTests()
        {
            var histogramMock = new Mock<IHistogram>();
            var colorMap = new ColorMap(histogramMock.Object, histogramMock.Object, histogramMock.Object);

            this.handsColorMaps = new HandsColorMaps(colorMap, colorMap);
            this.handsBackground = BgrImage.GetBlank(100, 100, Color.Red);
            this.handsForeground = BgrImage.GetBlank(100, 100, Color.Green);
            this.handsAnalyzerResult = new HandsAnalyzerResult(this.handsColorMaps, this.handsBackground, this.handsForeground);
        }

        [Fact]
        public void HandsColorMapsGetter_ReturnsTheObjectSetThroughTheConstructor()
        {
            Assert.Same(this.handsColorMaps, this.handsAnalyzerResult.HandsColorMaps);
        }

        [Fact]
        public void HandsBackgroundGetter_ReturnsTheObjectSetThroughTheConstructor()
        {
            Assert.Same(this.handsBackground, this.handsAnalyzerResult.HandsBackground);
        }

        [Fact]
        public void HandsForegroundGetter_ReturnsTheObjectSetThroughTheConstructor()
        {
            Assert.Same(this.handsForeground, this.handsAnalyzerResult.HandsForeground);
        }
    }
}
