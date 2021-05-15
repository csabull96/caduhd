namespace Caduhd.Common.Tests
{
    using Xunit;

    public class PointDTests
    {
        private const double X = 123.321;
        private const double Y = 321.123;

        private readonly PointD pointD;

        public PointDTests()
        {
            this.pointD = new PointD(X, Y);
        }

        [Fact]
        public void XGetter_PropertyCorrectlySetThroughConstructor()
        {
            Assert.Equal(X, this.pointD.X);
        }

        [Fact]
        public void YGetter_PropertyCorrectlySetThroughConstructor()
        {
            Assert.Equal(Y, this.pointD.Y);
        }
    }
}
