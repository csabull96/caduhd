namespace Caduhd.HandsDetector
{
    using System;
    using System.Drawing;
    using Caduhd.Common;

    /// <summary>
    /// A class used for skin color based hand detection.
    /// </summary>
    public class SkinColorHandsDetector
    {
        private const int DIVISOR_TO_GET_HAND_AREA_WIDTH_FROM_IMAGE_WIDTH = 3;

        private const int BACKGROUND_PIXEL_TOLERANCE = 18;

        private const int DIVISOR_TO_GET_VALID_HAND_WEIGHT_LOWER_BOUND_FROM_NEUTRAL_HAND_WEIGHT = 6;
        private const int DIVISOR_TO_GET_HAND_MARKER_CIRCLE_RADIUS_FROM_IMAGE_SIZE = 11520;

        private readonly BgrPixel BACKGROUND_PIXEL = new BgrPixel(Color.Red);
        private readonly BgrPixel SKIN_PIXEL = new BgrPixel(Color.White);
        private readonly BgrPixel UNKNOWN_PIXEL = new BgrPixel(Color.Green);

        private IHandsDetectorTuning tuning;

        private NormalizedHands neutralHands;

        /// <summary>
        /// Gets a value indicating whether the hand detector is tuned or not.
        /// </summary>
        public bool Tuned { get; private set; }

        /// <summary>
        /// A method to tune the hand detector.
        /// </summary>
        /// <param name="tuning">The <see cref="IHandsDetectorTuning"/> tuning object.</param>
        /// <returns>The neutral hands that was detected right after tuning.</returns>
        public NormalizedHands Tune(IHandsDetectorTuning tuning)
        {
            this.tuning = tuning;

            HandsDetectorResult result = this.DetectHandsInternally(this.tuning.HandsForeground);
            this.neutralHands = result.Hands;

            this.Tuned = true;

            return this.neutralHands;
        }

        /// <summary>
        /// Invalidates the current tuning of the hand detector.
        /// </summary>
        public void InvalidateTuning() => this.Tuned = false;

        /// <summary>
        /// Detect hand based on color on the left and right third of the image.
        /// </summary>
        /// <param name="image">The image in which the hands should detected.</param>
        /// <returns>The result of the hand detection.</returns>
        public HandsDetectorResult DetectHands(BgrImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("The image sent for hand detection was null.");
            }

            BgrImage imagePreProcessed = this.PreProcess(image);
            return this.DetectHandsInternally(imagePreProcessed);
        }

        private BgrImage PreProcess(BgrImage image)
        {
            // no pre process at the moment
            return image;
        }

        private HandsDetectorResult DetectHandsInternally(BgrImage image)
        {
            int handAreaWidth = image.Width / DIVISOR_TO_GET_HAND_AREA_WIDTH_FROM_IMAGE_WIDTH;

            NormalizedHand left = this.EvaluatePixels(image, 0, handAreaWidth, this.tuning.HandsColorMaps.Left, this.neutralHands?.Left);
            NormalizedHand right = this.EvaluatePixels(image, image.Width - handAreaWidth, handAreaWidth, this.tuning.HandsColorMaps.Right, this.neutralHands?.Right);

            int radius = image.Width * image.Height / DIVISOR_TO_GET_HAND_MARKER_CIRCLE_RADIUS_FROM_IMAGE_SIZE;

            if (this.Tuned && this.neutralHands != null)
            {
                image.DrawLineSegment(left.X, left.Y, this.neutralHands.Center.X, this.neutralHands.Center.Y, Color.Yellow, radius);
                image.DrawLineSegment(this.neutralHands.Center.X, this.neutralHands.Center.Y, right.X, right.Y, Color.Yellow, radius);
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

                    if (this.IsBackground(pixel, x, y))
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

            if (this.Tuned && neutralHand != null && detectedHand.Weight < neutralHand.Weight / DIVISOR_TO_GET_VALID_HAND_WEIGHT_LOWER_BOUND_FROM_NEUTRAL_HAND_WEIGHT)
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
            BgrPixel backgroundReferencePixel = this.tuning.HandsBackground.GetPixel(x, y);

            double deltaBlue = Math.Abs(backgroundReferencePixel.Blue - pixel.Blue);
            double deltaGreen = Math.Abs(backgroundReferencePixel.Green - pixel.Green);
            double deltaRed = Math.Abs(backgroundReferencePixel.Red - pixel.Red);

            return deltaBlue <= BACKGROUND_PIXEL_TOLERANCE && deltaGreen <= BACKGROUND_PIXEL_TOLERANCE && deltaRed <= BACKGROUND_PIXEL_TOLERANCE;
        }
    }
}
