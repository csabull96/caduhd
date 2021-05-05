namespace Caduhd.HandsDetector
{
    public class HandNormalizer
    {
        private readonly int _imageWidth;
        private readonly int _imageHeight;

        public HandNormalizer(int imageWidth, int imageHeight)
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
        }

        public NormalizedHand Normalize(Hand hand) =>
            new NormalizedHand(
                (double)hand.X / _imageWidth,
                (double)hand.Y / _imageHeight,
                (double)hand.Weight / (_imageWidth * _imageHeight));
    }
}
