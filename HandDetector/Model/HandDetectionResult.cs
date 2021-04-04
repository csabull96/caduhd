using System.Drawing;

namespace Ksvydo.HandDetector.Model
{
    public class HandDetectionResult
    {
        public Hands Hands { get; private set; }
        public Bitmap Frame { get; private set; }

        public HandDetectionResult(Hands hands, Bitmap frame)
        {
            Hands = hands;
            Frame = frame;
        }
    }
}
