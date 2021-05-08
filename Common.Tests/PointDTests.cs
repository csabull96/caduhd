using Xunit;

namespace Caduhd.Common.Tests
{
    public class PointDTests
    {
        private const double X = 123.321;
        private const double Y = 321.123;


        private PointD _pointD;

        public PointDTests()
        {
            _pointD = new PointD(X, Y);
        }

        [Fact]
        public void XGetter_PropertyCorrectlySetThroughConstructor()
        {
            Assert.Equal(X, _pointD.X);
        }

        [Fact]
        public void YGetter_PropertyCorrectlySetThroughConstructor()
        {
            Assert.Equal(Y, _pointD.Y);
        }
    }
}
