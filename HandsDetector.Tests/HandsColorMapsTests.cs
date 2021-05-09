using Caduhd.Common;
using Caduhd.HandsDetector;
using Moq;
using System;
using Xunit;

namespace Caduhd.HandDetector.Tests
{
    public class HandsColorMapsTests
    {
        private HandsColorMaps _handsColorMaps;
        private ColorMap _leftColorMap;
        private ColorMap _rightColorMap;

        public HandsColorMapsTests()
        {
            var histogramMock = new Mock<IHistogram>();
            _leftColorMap = new ColorMap(histogramMock.Object, histogramMock.Object, histogramMock.Object);
            _rightColorMap = new ColorMap(histogramMock.Object, histogramMock.Object, histogramMock.Object);
            _handsColorMaps = new HandsColorMaps(_leftColorMap, _rightColorMap);
        }

        [Fact]
        public void LeftGetter_ReturnsColorMapThatWasSetThroughConstructor()
        {
            Assert.Same(_leftColorMap, _handsColorMaps.Left);
        }

        [Fact]
        public void RightGetter_ReturnsColorMapThatWasSetThroughConstructor()
        {
            Assert.Same(_rightColorMap, _handsColorMaps.Right);
        }

        [Fact]
        public void Constructor_BothColorMapsAreNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HandsColorMaps(null, null));
        }

        [Fact]
        public void Constructor_OnlyLeftColorMapIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HandsColorMaps(null, _rightColorMap));
        }

        [Fact]
        public void Constructor_OnlyRightMapIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HandsColorMaps(_leftColorMap, null));
        }
    }
}
