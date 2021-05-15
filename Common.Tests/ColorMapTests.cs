namespace Caduhd.Common.Tests
{
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public class ColorMapTests
    {
        private readonly ColorMap colorMap;

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

            this.colorMap = new ColorMap(blues.Object, greens.Object, reds.Object);
        }

        public static IEnumerable<object[]> SatisfiesTestData => new List<object[]>()
        {
            new object[] { 72, 83, 133 },
            new object[] { 76, 77, 121 },
            new object[] { 69, 49, 113 },
        };

        [Theory]
        [MemberData(nameof(SatisfiesTestData))]
        public void Satisfies_GeneralCases_ColorMapSatisfiesBgrPixel(int blue, int green, int red)
        {
            BgrPixel bgrPixel = new BgrPixel(blue, green, red);
            Assert.True(this.colorMap.Satisfies(bgrPixel));
        }
    }
}
