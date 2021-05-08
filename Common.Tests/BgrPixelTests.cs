using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace Caduhd.Common.Tests
{
    public class BgrPixelTests
    {
        public static IEnumerable<object[]> ColorChannelsTestData => new List<object[]>()
        {
            new object[] { 0, 0, 0 },
            new object[] { 127, 98, 24 },
            new object[] { 101, 233, 27 },
            new object[] { 255, 255, 255 }
        };

        [Theory]
        [MemberData(nameof(ColorChannelsTestData))]
        public void BlueGetter_ConstructedFromColor_ReturnsCorrectBlueValue(int blue, int green, int red)
        {
            Color color = Color.FromArgb(red, green, blue);
            BgrPixel bgrPixel = new BgrPixel(color);
            Assert.Equal(bgrPixel.Blue, blue);
        }

        [Theory]
        [MemberData(nameof(ColorChannelsTestData))]
        public void GreenGetter_ConstructedFromColor_ReturnsCorrectGreenValue(int blue, int green, int red)
        {
            Color color = Color.FromArgb(red, green, blue);
            BgrPixel bgrPixel = new BgrPixel(color);
            Assert.Equal(bgrPixel.Green, green);
        }

        [Theory]
        [MemberData(nameof(ColorChannelsTestData))]
        public void RedGetter_ConstructedFromColor_ReturnsCorrectRedValue(int blue, int green, int red)
        {
            Color color = Color.FromArgb(red, green, blue);
            BgrPixel bgrPixel = new BgrPixel(color);
            Assert.Equal(bgrPixel.Red, red);
        }

        [Theory]
        [MemberData(nameof(ColorChannelsTestData))]
        public void BlueGetter_ConstructedFromColorChannels_ReturnsCorrectBlueValue(int blue, int green, int red)
        {
            BgrPixel bgrPixel = new BgrPixel(blue, green, red);
            Assert.Equal(bgrPixel.Blue, blue);
        }

        [Theory]
        [MemberData(nameof(ColorChannelsTestData))]
        public void GreenGetter_ConstructedFromColorChannels_ReturnsCorrectGreenValue(int blue, int green, int red)
        {
            BgrPixel bgrPixel = new BgrPixel(blue, green, red);
            Assert.Equal(bgrPixel.Green, green);
        }

        [Theory]
        [MemberData(nameof(ColorChannelsTestData))]
        public void RedGetter_ConstructedFromColorChannels_ReturnsCorrectRedValue(int blue, int green, int red)
        {
            BgrPixel bgrPixel = new BgrPixel(blue, green, red);
            Assert.Equal(bgrPixel.Red, red);
        }
    }
}
