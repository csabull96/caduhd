using System;

namespace Caduhd.HandsDetector
{
    public class NormalizedHand
    {
        private const string INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE =
            "{0} has to be greater or equal than 0.0 and smaller or equal than 1.0. Value was: {1}";

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Weight { get; private set; }

        public NormalizedHand() : this(0, 0, 0) { }

        public NormalizedHand(double x, double y, double weight)
        {
            bool isNormalizedValueInvalid(double value) =>
                value < 0 || 1 < value;

            if (isNormalizedValueInvalid(x))
                throw new ArgumentException(string.Format(INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE, "X", X));

            if (isNormalizedValueInvalid(y))
                throw new ArgumentException(string.Format(INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE, "Y", Y));

            if (isNormalizedValueInvalid(weight))
                throw new ArgumentException(string.Format(INVALID_NORMALIZED_INPUT_PARAMETER_ERROR_MESSAGE, "Weight", Weight));
            
            X = x;
            Y = y;
            Weight = weight;
        }
    }
}
