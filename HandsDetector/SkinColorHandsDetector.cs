using Caduhd.Common;
using System;
using System.Drawing;

namespace Caduhd.HandsDetector
{
    public class SkinColorHandsDetector
    {
        private const int DIVISOR_TO_GET_HAND_AREA_WIDTH_FROM_IMAGE_WIDTH = 3;

        private const int BACKGROUND_PIXEL_TOLERANCE = 18;

        private const int DIVISOR_TO_GET_VALID_HAND_WEIGHT_LOWER_BOUND_FROM_NEUTRAL_HAND_WEIGHT = 6;
        private const int DIVISOR_TO_GET_HAND_MARKER_CIRCLE_RADIUS_FROM_IMAGE_SIZE = 11520;

        private readonly BgrPixel BACKGROUND_PIXEL = new BgrPixel(Color.Red);
        private readonly BgrPixel SKIN_PIXEL = new BgrPixel(Color.White);
        private readonly BgrPixel UNKNOWN_PIXEL = new BgrPixel(Color.Green);

        private IHandsDetectorTuning _tuning;

        private NormalizedHands _neutralHands;

        public bool Tuned { get; private set; }

        public NormalizedHands Tune(IHandsDetectorTuning tuning)
        {
            _tuning = tuning;

            HandsDetectorResult result = DetectHandsInternally(_tuning.HandsForeground);
            _neutralHands = result.Hands;

            Tuned = true;

            return _neutralHands;
        }

        public void InvalidateTuning() => Tuned = false;

        public HandsDetectorResult DetectHands(BgrImage image)
        {
            if (image == null)
                throw new ArgumentNullException("The input image sent for hand detection was null.");

            BgrImage imagePreProcessed = PreProcess(image);
            return DetectHandsInternally(imagePreProcessed);
        }

        private BgrImage PreProcess(BgrImage image)
        {
            // no pre process at the moment
            return image;
        }

        private HandsDetectorResult DetectHandsInternally(BgrImage image)
        {
            int handAreaWidth = image.Width / DIVISOR_TO_GET_HAND_AREA_WIDTH_FROM_IMAGE_WIDTH;

            NormalizedHand left = EvaluatePixels(image, 0, handAreaWidth, _tuning.HandsColorMaps.Left, _neutralHands?.Left);
            NormalizedHand right = EvaluatePixels(image, image.Width - handAreaWidth, handAreaWidth, _tuning.HandsColorMaps.Right, _neutralHands?.Right);

            int radius = image.Width * image.Height / DIVISOR_TO_GET_HAND_MARKER_CIRCLE_RADIUS_FROM_IMAGE_SIZE;

            if (Tuned && _neutralHands != null)
            {
                image.DrawLineSegment(left.X, left.Y, _neutralHands.Center.X, _neutralHands.Center.Y, Color.Yellow, radius);
                image.DrawLineSegment(_neutralHands.Center.X, _neutralHands.Center.Y, right.X, right.Y, Color.Yellow, radius);
            }
            
            image.DrawCircle(left.X, left.Y, Color.Blue, radius, BgrImage.FILL_DRAWING);
            image.DrawCircle(right.X, right.Y, Color.Blue, radius, BgrImage.FILL_DRAWING);

            NormalizedHands normalizedHands = new NormalizedHands(left, right);
            return new HandsDetectorResult(normalizedHands, image);
        }

        private NormalizedHand EvaluatePixels(BgrImage image, int startX, int width, ColorMap colorMap, NormalizedHand neutralHand)
        {
            HandBuilder handBuilder = new HandBuilder();

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    BgrPixel pixel = image.GetPixel(x, y);

                    if (IsBackground(pixel, x, y))
                    {
                        image.SetPixel(BACKGROUND_PIXEL, x, y);
                    }
                    else if (!colorMap.Satisfies(pixel))
                    {
                        image.SetPixel(UNKNOWN_PIXEL, x, y);
                    }
                    else
                    {
                        image.SetPixel(SKIN_PIXEL, x, y);
                        handBuilder.Append(x, y);
                    }
                }
            }

            NormalizedHand detectedHand = new HandNormalizer(image.Width, image.Height).Normalize(handBuilder.Build());

            if (Tuned && neutralHand != null && detectedHand.Weight < neutralHand.Weight / DIVISOR_TO_GET_VALID_HAND_WEIGHT_LOWER_BOUND_FROM_NEUTRAL_HAND_WEIGHT)
            {
                // after tuning the neutral hands are cached
                // if the weight of the hand is under a threshold
                // (which probably means that there is no hand on the image but noise)
                // then we're going to use the cached neutral hand instead of the freshly detected (wrong) one
                detectedHand = new NormalizedHand(neutralHand.X, neutralHand.Y, neutralHand.Weight);
            }

            return detectedHand;
        }

        private bool IsBackground(BgrPixel pixel, int x, int y)
        {
            BgrPixel backgroundReferencePixel = _tuning.HandsBackground.GetPixel(x, y);

            double deltaBlue = Math.Abs(backgroundReferencePixel.Blue - pixel.Blue);
            double deltaGreen = Math.Abs(backgroundReferencePixel.Green - pixel.Green);
            double deltaRed = Math.Abs(backgroundReferencePixel.Red - pixel.Red);

            return deltaBlue <= BACKGROUND_PIXEL_TOLERANCE && deltaGreen <= BACKGROUND_PIXEL_TOLERANCE && deltaRed <= BACKGROUND_PIXEL_TOLERANCE;
        }
    }
}
