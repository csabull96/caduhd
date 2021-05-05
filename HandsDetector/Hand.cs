namespace Caduhd.HandsDetector
{
    public class Hand
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Weight { get; private set; }

        public Hand() : this(0, 0, 0) { }

        public Hand(int x, int y, int weight)
        {
            X = x;
            Y = y;
            Weight = weight;
        }
    }
}
