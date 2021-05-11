using Caduhd.HandsDetector;
using Xunit;

namespace Caduhd.HandsDetector.Tests
{
    public class HandTests
    {
        private const int X = 111;
        private const int Y = 222;
        private const int WEIGHT = 333;

        private Hand _handFromParameterlessConstructor;
        private Hand _handFromParameterizedConstructor;

        public HandTests()
        {
            _handFromParameterlessConstructor = new Hand();
            _handFromParameterizedConstructor = new Hand(X, Y, WEIGHT);
        }

        [Fact]
        public void XGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, _handFromParameterlessConstructor.X);
        }

        [Fact]
        public void YGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, _handFromParameterlessConstructor.Y);
        }

        [Fact]
        public void WeightGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, _handFromParameterlessConstructor.Weight);
        }

        [Fact]
        public void XGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(X, _handFromParameterizedConstructor.X);
        }

        [Fact]
        public void YGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(Y, _handFromParameterizedConstructor.Y);
        }

        [Fact]
        public void WeightGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(WEIGHT, _handFromParameterizedConstructor.Weight);
        }
    }
}
