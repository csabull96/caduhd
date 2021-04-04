namespace Ksvydo.HandDetector.Model
{
    public class Hands
    {
        public Hand Left { get; private set; }
        public Hand Right { get; private set; }

        public Hands(Hand left, Hand right)
        {
            Left = left;
            Right = right;
        }
    }
}
