using System.Drawing;

namespace Caduhd.HandsDetector
{
    public class Hands
    {
        public Hand Left { get; private set; }
        public Hand Right { get; private set; }
        public Point Center { get; private set; }
        public double RatioOfLeftWeightToRightWeight { get; private set; }

        public Hands(Hand left, Hand right)
        {
            Left = left;
            Right = right;
            int centerX = (left.X + right.X) / 2;
            int centerY = (left.Y + right.Y) / 2;
            Center = new Point(centerX, centerY);
            RatioOfLeftWeightToRightWeight = (double)left.Weight / right.Weight;
        }
    }
}
