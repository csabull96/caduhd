using Caduhd.Common;

namespace Caduhd.HandsDetector
{
    public class HandsDetectorResult
    {
        public NormalizedHands Hands { get; private set; }
        public BgrImage Image { get; private set; }

        public HandsDetectorResult(NormalizedHands hands, BgrImage image)
        {
            Hands = hands;
            Image = image;
        }
    }
}
