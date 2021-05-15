namespace Caduhd.HandsDetector.Tests
{
    using System;
    using Caduhd.Common;
    using Moq;
    using Xunit;

    public class HandsColorMapsTests
    {
        private readonly HandsColorMaps handsColorMaps;
        private readonly ColorMap leftColorMap;
        private readonly ColorMap rightColorMap;

        public HandsColorMapsTests()
        {
            var histogramMock = new Mock<IHistogram>();
            this.leftColorMap = new ColorMap(histogramMock.Object, histogramMock.Object, histogramMock.Object);
            this.rightColorMap = new ColorMap(histogramMock.Object, histogramMock.Object, histogramMock.Object);
            this.handsColorMaps = new HandsColorMaps(this.leftColorMap, this.rightColorMap);
        }

        [Fact]
        public void LeftGetter_ReturnsColorMapThatWasSetThroughConstructor()
        {
            Assert.Same(this.leftColorMap, this.handsColorMaps.Left);
        }

        [Fact]
        public void RightGetter_ReturnsColorMapThatWasSetThroughConstructor()
        {
            Assert.Same(this.rightColorMap, this.handsColorMaps.Right);
        }

        [Fact]
        public void Constructor_BothColorMapsAreNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HandsColorMaps(null, null));
        }

        [Fact]
        public void Constructor_OnlyLeftColorMapIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HandsColorMaps(null, this.rightColorMap));
        }

        [Fact]
        public void Constructor_OnlyRightMapIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HandsColorMaps(this.leftColorMap, null));
        }
    }
}
