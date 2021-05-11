using Caduhd.Common;
using Moq;
using System;
using System.Drawing;
using Xunit;

namespace Caduhd.HandsDetector.Tests
{
    public class SkinColorHandsDetectorTests
    {
        private const int WIDTH = 720;
        private const int HEIGHT = 480;

        private const int PIXEL_TOLERANCE = 10;

        private const double HAND_POSITION_TOLERANCE = 0.01;

        private Color _skinColor;

        private BgrImage _handsBackground;
        private BgrImage _handsForeground;

        private SkinColorHandsDetector _handsDetector;

        public SkinColorHandsDetectorTests()
        {
            int blue = 44;
            int green = 87;
            int red = 123;
            _skinColor = Color.FromArgb(red, green, blue);

            _handsBackground = BgrImage.GetBlank(WIDTH, HEIGHT, Color.White);
            _handsForeground = _handsBackground.Copy();

            var blueHistogramMock = new Mock<IHistogram>();
            blueHistogramMock.Setup(bh => bh.Smallest).Returns(blue - PIXEL_TOLERANCE);
            blueHistogramMock.Setup(bh => bh.Greatest).Returns(blue + PIXEL_TOLERANCE);

            var greenHistogramMock = new Mock<IHistogram>();
            greenHistogramMock.Setup(bh => bh.Smallest).Returns(green - PIXEL_TOLERANCE);
            greenHistogramMock.Setup(bh => bh.Greatest).Returns(green + PIXEL_TOLERANCE);

            var redHistogramMock = new Mock<IHistogram>();
            redHistogramMock.Setup(bh => bh.Smallest).Returns(red - PIXEL_TOLERANCE);
            redHistogramMock.Setup(bh => bh.Greatest).Returns(red + PIXEL_TOLERANCE);

            var leftColorMap = new ColorMap(blueHistogramMock.Object,
                greenHistogramMock.Object, redHistogramMock.Object);

            var rightColorMap = new ColorMap(blueHistogramMock.Object,
                greenHistogramMock.Object, redHistogramMock.Object);

            var handsColorMaps = new HandsColorMaps(leftColorMap, rightColorMap);

            var tuningMock = new Mock<IHandsDetectorTuning>();
            tuningMock.Setup(t => t.HandsBackground).Returns(_handsBackground);
            tuningMock.Setup(t => t.HandsForeground).Returns(_handsForeground);
            tuningMock.Setup(t => t.HandsColorMaps).Returns(handsColorMaps);

            _handsDetector = new SkinColorHandsDetector();
            _handsDetector.Tune(tuningMock.Object);
        }

        [Theory]
        [InlineData(0.15, 0.5, 0.85, 0.5, 15)]
        [InlineData(0.18, 0.4, 0.79, 0.7, 19)]
        [InlineData(0.21, 0.66, 0.91, 0.8, 23)]
        public void DetectHands_ColorBased_HandsDetectedCorrectly(double leftX, double leftY, double rightX, double rightY, double radius)
        {
            int fill = -1;
            _handsForeground.DrawCircle(leftX, leftY, _skinColor, radius, fill);
            _handsForeground.DrawCircle(rightX, rightY, _skinColor, radius, fill);

            var handsDetectorResult = _handsDetector.DetectHands(_handsForeground);
            Assert.True(Math.Abs(leftX - handsDetectorResult.Hands.Left.X) < HAND_POSITION_TOLERANCE);
            Assert.True(Math.Abs(leftY - handsDetectorResult.Hands.Left.Y) < HAND_POSITION_TOLERANCE);
            Assert.True(Math.Abs(rightX - handsDetectorResult.Hands.Right.X) < HAND_POSITION_TOLERANCE);
            Assert.True(Math.Abs(rightY - handsDetectorResult.Hands.Right.Y) < HAND_POSITION_TOLERANCE);

            Assert.Equal(handsDetectorResult.Hands.Left.Weight, handsDetectorResult.Hands.Left.Weight);
        }
    }
}
