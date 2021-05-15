namespace Caduhd.HandsDetector.Tests
{
    using Xunit;

    public class HandTests
    {
        private const int X = 111;
        private const int Y = 222;
        private const int WEIGHT = 333;

        private Hand handFromParameterlessConstructor;
        private Hand handFromParameterizedConstructor;

        public HandTests()
        {
            this.handFromParameterlessConstructor = new Hand();
            this.handFromParameterizedConstructor = new Hand(X, Y, WEIGHT);
        }

        [Fact]
        public void XGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.handFromParameterlessConstructor.X);
        }

        [Fact]
        public void YGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.handFromParameterlessConstructor.Y);
        }

        [Fact]
        public void WeightGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.handFromParameterlessConstructor.Weight);
        }

        [Fact]
        public void XGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(X, this.handFromParameterizedConstructor.X);
        }

        [Fact]
        public void YGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(Y, this.handFromParameterizedConstructor.Y);
        }

        [Fact]
        public void WeightGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(WEIGHT, this.handFromParameterizedConstructor.Weight);
        }
    }
}
