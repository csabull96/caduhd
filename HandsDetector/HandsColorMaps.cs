using Caduhd.Common;

namespace Caduhd.HandsDetector
{
    public class HandsColorMaps
    {
        public ColorMap Left { get; private set; }
        public ColorMap Right { get; private set; }

        public HandsColorMaps(ColorMap left, ColorMap right)
        {
            Left = left;
            Right = right;
        }
    }
}
