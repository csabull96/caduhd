using Caduhd.Common;
using System;
using System.Drawing;

namespace Caduhd.HandsDetector
{
    public class SkinColorHandsDetector
    {
        private const int HANDS_AREA_RATIO = 3;

        private const int BACKGROUND_TOLERANCE = 18;

        private readonly BgrPixel BACKGROUND_PIXEL = new BgrPixel(Color.Red);
        private readonly BgrPixel SKIN_PIXEL = new BgrPixel(Color.White);
        private readonly BgrPixel NOT_BACKGROUND_NOT_SKIN_PIXEL = new BgrPixel(Color.Green);

        private IHandsDetectorTuning _tuning;

        public bool Tuned { get; private set; }

        public void Tune(IHandsDetectorTuning tuning)
        {
            _tuning = tuning;
            Tuned = true;
        }

        public void InvalidateTuning() => Tuned = false;

        public HandsDetectorResult DetectHands(BgrImage image)
        {
            BgrImage imagePreProcessed = PreProcess(image);
            return DetectHandsInternally(imagePreProcessed);
        }

        private BgrImage PreProcess(BgrImage image)
        {
            // no pre process currently
            return image;
        }

        private HandsDetectorResult DetectHandsInternally(BgrImage image)
        {
            int handAreaWidth = image.Width / HANDS_AREA_RATIO;

            MarkPixels(image, 0, handAreaWidth);
            MarkPixels(image, image.Width - handAreaWidth, image.Width);

            throw new NotImplementedException("Check whether detection is noisy or not and act accordingly.");

            //sample.Roi = new Rectangle(0, 0, handAreaWidth, height);
            //var smoothened = sample.SmoothMedian(5);
            //smoothened.CopyTo(sample);
            //sample.Roi = Rectangle.Empty;

            //sample.Roi = new Rectangle(width - handAreaWidth, 0, handAreaWidth, height);
            //smoothened = sample.SmoothMedian(5);
            //smoothened.CopyTo(sample);
            //sample.Roi = Rectangle.Empty;

            Hand left = ConstructHand(image, 0, handAreaWidth);
            Hand right = ConstructHand(image, handAreaWidth, image.Width);

            throw new NotImplementedException("Check whether the hands are valid or not and act accordingly.");
            throw new NotImplementedException("Circle size should depend on the size of the input image.");

            image.DrawCircle(left.X, left.Y, Color.Blue, 5, -1);
            image.DrawCircle(right.X, right.Y, Color.Blue, 5, -1);

            return new HandsDetectorResult(new Hands(left, right), image);
        }

        private void MarkPixels(BgrImage image, int start, int width)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = start; x < start + width; x++)
                {
                    BgrPixel pixel = image.GetPixel(x, y);

                    if (IsBackground(pixel, x, y))
                    {
                        image.SetPixel(BACKGROUND_PIXEL, x, y);
                    }
                    else if (!_tuning.HandsColorMaps.Left.Satisfies(pixel))
                    {
                        image.SetPixel(NOT_BACKGROUND_NOT_SKIN_PIXEL, x, y);
                    }
                    else
                    {
                        image.SetPixel(SKIN_PIXEL, x, y);
                    }
                }
            }
        }

        private Hand ConstructHand(BgrImage image, int start, int width)
        {
            HandConstructor handConstructor = new HandConstructor();

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = start; x < width; x++)
                {
                    if (image.GetPixel(x, y) == SKIN_PIXEL)
                    {
                        handConstructor.Append(x, y);
                    }
                }
            }

            return handConstructor.Construct();
        }

        private bool IsBackground(BgrPixel pixel, int x, int y)
        {
            BgrPixel backgroundReferencePixel = _tuning.HandsBackground.GetPixel(x, y);

            double deltaBlue = Math.Abs(backgroundReferencePixel.Blue - pixel.Blue);
            double deltaGreen = Math.Abs(backgroundReferencePixel.Green - pixel.Green);
            double deltaRed = Math.Abs(backgroundReferencePixel.Red - pixel.Red);

            return deltaBlue <= BACKGROUND_TOLERANCE && deltaGreen <= BACKGROUND_TOLERANCE && deltaRed <= BACKGROUND_TOLERANCE;
        }
    }
}
