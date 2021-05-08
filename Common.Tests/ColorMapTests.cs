using Moq;
using System.Collections.Generic;
using Xunit;

namespace Caduhd.Common.Tests
{
    public class ColorMapTests
    {
        private ColorMap _colorMap;

        public static IEnumerable<object[]> SatisfiesTestData => new List<object[]>()
        {
            new object[] { 72, 83, 133 },
            new object[] { 76, 77, 121 },
            new object[] { 69, 49, 113 },
        };

        public ColorMapTests()
        {
            var blues = new Mock<IHistogram>();
            blues.SetupGet(h => h.Smallest).Returns(67.18);
            blues.SetupGet(h => h.Greatest).Returns(83.79);

            var greens = new Mock<IHistogram>();
            greens.SetupGet(h => h.Smallest).Returns(17.43);
            greens.SetupGet(h => h.Greatest).Returns(118);

            var reds = new Mock<IHistogram>();
            reds.SetupGet(h => h.Smallest).Returns(111.78);
            reds.SetupGet(h => h.Greatest).Returns(156.32);

            _colorMap = new ColorMap(blues.Object, greens.Object, reds.Object);
        }

        [Theory]
        [MemberData(nameof(SatisfiesTestData))]
        public void Satisfies_GeneralCases_ColorMapSatisfiesBgrPixel(int blue, int green, int red)
        {
            BgrPixel bgrPixel = new BgrPixel(blue, green, red);
            Assert.True(_colorMap.Satisfies(bgrPixel));
        }
    }
}
