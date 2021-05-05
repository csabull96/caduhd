namespace Caduhd.HandsDetector
{
    public class HandBuilder
    {
        private int _totalX;
        private int _totalY;
        private int _weight;

        public void Append(int x, int y)
        {
            _totalX += x;
            _totalY += y;
            _weight++;
        }

        public Hand Build()
        {
            return _weight == 0 ? new Hand() : new Hand(_totalX / _weight, _totalY / _weight, _weight);
        }
    }
}
