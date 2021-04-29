namespace Caduhd.HandsDetector
{
    public class HandConstructor
    {
        private int _totalX = 0;
        private int _totalY = 0;
        private int _weight = 0;

        public void Append(int x, int y)
        {
            _totalX += x;
            _totalY += y;
            _weight++;
        }

        public Hand Construct() => new Hand(_totalX / _weight, _totalY / _weight, _weight);
    }
}
