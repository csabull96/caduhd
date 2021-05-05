namespace Caduhd.HandsDetector
{
    public class NormalizedHand
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Weight { get; private set; }

        public NormalizedHand() : this(0, 0, 0) { }

        public NormalizedHand(double x, double y, double weight)
        {
            X = x;
            Y = y;
            Weight = weight;
        }
    }
}
