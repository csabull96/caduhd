using Caduhd.Common;
using System;

namespace Caduhd.HandsDetector
{
    public class HandsColorMaps
    {
        public ColorMap Left { get; private set; }
        public ColorMap Right { get; private set; }

        public HandsColorMaps(ColorMap left, ColorMap right)
        {
            if (left == null)
                throw new ArgumentNullException("Left hand's color map was null.");

            if (right == null)
                throw new ArgumentNullException("Left right's color map was null.");

            Left = left;
            Right = right;
        }
    }
}
