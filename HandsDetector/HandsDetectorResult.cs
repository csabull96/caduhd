using Caduhd.Common;

namespace Caduhd.HandsDetector
{
    public class HandsDetectorResult
    {
        public Hands Hands { get; private set; }
        public BgrImage Image { get; private set; }

        public HandsDetectorResult(Hands hands, BgrImage image)
        {
            Hands = hands;
            Image = image;
        }
    }
}
