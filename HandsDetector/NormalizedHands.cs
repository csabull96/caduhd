using Caduhd.Common;

namespace Caduhd.HandsDetector
{
    public class NormalizedHands
    {
        public NormalizedHand Left { get; private set; }
        public NormalizedHand Right { get; private set; }
        public PointD Center { get; private set; }
        public double RatioOfLeftWeightToRightWeight { get; private set; }

        public NormalizedHands(NormalizedHand left, NormalizedHand right)
        {
            Left = left;
            Right = right;
            double centerX = (left.X + right.X) / 2;
            double centerY = (left.Y + right.Y) / 2;
            Center = new PointD(centerX, centerY);
            RatioOfLeftWeightToRightWeight = left.Weight / right.Weight;
        }
    }
}
