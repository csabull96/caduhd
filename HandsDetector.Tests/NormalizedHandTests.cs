using Caduhd.HandsDetector;
using System;
using Xunit;

namespace Caduhd.HandDetector.Tests
{
    public class NormalizedHandTests
    {
        private const double X = 0.3;
        private const double Y = 0.9;
        private const double WEIGHT = 0.333;

        private NormalizedHand _normalizedHandFromParameterlessConstructor;
        private NormalizedHand _normalizedHandFromParameterizedConstructor;

        public NormalizedHandTests()
        {
            _normalizedHandFromParameterlessConstructor = 
                new NormalizedHand();
            _normalizedHandFromParameterizedConstructor =
                new NormalizedHand(X, Y, WEIGHT);
        }

        [Fact]
        public void XGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, _normalizedHandFromParameterlessConstructor.X);
        }

        [Fact]
        public void YGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, _normalizedHandFromParameterlessConstructor.Y);
        }

        [Fact]
        public void WeightGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, _normalizedHandFromParameterlessConstructor.Weight);
        }

        [Fact]
        public void XGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(X, _normalizedHandFromParameterizedConstructor.X);
        }

        [Fact]
        public void YGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(Y, _normalizedHandFromParameterizedConstructor.Y);
        }

        [Fact]
        public void WeightGetter_ParameterizedConstructor_ReturnsZero()
        {
            Assert.Equal(WEIGHT, _normalizedHandFromParameterizedConstructor.Weight);
        }

        [Theory]
        [InlineData(1.1, 0.8, 0.2)]
        [InlineData(0.8, -0.0001, 0.35)]
        [InlineData(0.000001, 0.23, 1.00001)]
        public void Constructor_InvalidNormalizedInputValues_ThrowsArgumentException(double x, double y, double weight)
        {
            Assert.Throws<ArgumentException>(() => new NormalizedHand(x, y, weight));
        }
    }
}
