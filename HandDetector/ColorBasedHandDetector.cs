using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Ksvydo.HandDetector.Calibrator;
using Ksvydo.HandDetector.Model;
using System;
using System.Drawing;

namespace Ksvydo.HandDetector
{
    public class ColorBasedHandDetector
    {
        private const int WIDTH = 320;
        private const int HEIGHT = 180;
        private const int HAND_AREA_WIDTH = 100;

        private Rectangle m_maskForLeftHand = new Rectangle(35, 50, 45, 65);
        private Rectangle m_maskForRightHand = new Rectangle(250, 50, 45, 65);

        private ColorCharacteristics m_leftHandColorCharacteristics;
        private ColorCharacteristics m_rightHandColorCharacteristics;

        public delegate void HandDetectorStateChangedEventHandler();
        public event HandDetectorStateChangedEventHandler StateChanged;

        public ColorBasedHandDetectorState State { get; private set; }

        private Image<Bgr, byte> m_background;

        ColorCalibrator m_colorCalibrator;

        public ColorBasedHandDetector(ColorCalibrator colorCalibrator)
        {
            m_colorCalibrator = colorCalibrator;
        }

        public void ShiftState()
        {
            switch (State)
            {
                case ColorBasedHandDetectorState.NeedsCalibrating:
                    State = ColorBasedHandDetectorState.ReadyToCaptureBackground;
                    break;
                case ColorBasedHandDetectorState.NeedsReCalibrating:
                    State = ColorBasedHandDetectorState.ReadyToCaptureBackground;
                    break;
                case ColorBasedHandDetectorState.ReadyToCaptureBackground:
                    State = ColorBasedHandDetectorState.ReadyToAnalyzeLeftHand;
                    break;
                case ColorBasedHandDetectorState.ReadyToAnalyzeLeftHand:
                    State = ColorBasedHandDetectorState.ReadyToAnalyzeRightHand;
                    break;
                case ColorBasedHandDetectorState.ReadyToAnalyzeRightHand:
                    State = ColorBasedHandDetectorState.Calibrated;
                    break;
                case ColorBasedHandDetectorState.Calibrated:
                    State = ColorBasedHandDetectorState.Enabled;
                    break;
                case ColorBasedHandDetectorState.Enabled:
                    State = ColorBasedHandDetectorState.NeedsReCalibrating;
                    break;
            }

            StateChanged?.Invoke();
        }

        public Bitmap CaptureBackground(Bitmap frame)
        {
            Image<Bgr, byte> sample = PreProcess(frame);

            // csandor: check why is the disposal necessary here
            m_background?.Dispose();
            m_background = sample.Copy();

            return sample.Bitmap;
        }

        public Bitmap AnalyzeLeftHand(Bitmap frame)
        {
            Image<Bgr, byte> sample = PreProcess(frame);
            m_leftHandColorCharacteristics = m_colorCalibrator.ExtractColorCharacteristics(sample, m_maskForLeftHand);
            sample.Draw(m_maskForLeftHand, new Bgr(Color.LimeGreen), 2);
            return sample.Bitmap;
        }

        public Bitmap AnalyzeRightHand(Bitmap frame)
        {
            Image<Bgr, byte> sample = PreProcess(frame);
            m_rightHandColorCharacteristics = m_colorCalibrator.ExtractColorCharacteristics(sample, m_maskForRightHand);
            sample.Draw(m_maskForRightHand, new Bgr(Color.LimeGreen), 2);
            return sample.Bitmap;
        }

        public Bitmap Update(Bitmap frame)
        {
            Image<Bgr, byte> sample = PreProcess(frame);
            return sample.Bitmap;
        }

        public HandDetectionResult DetectHands(Bitmap frame)
        {
            Image<Bgr, byte> sample = PreProcess(frame);
            return DetectHandsInternally(sample);
        }

        private Image<Bgr, byte> PreProcess(Bitmap frame)
        {
            var image = new Image<Bgr, byte>(frame)
                .Resize(WIDTH, HEIGHT, Inter.Area, false);

            return image;
        }

        private HandDetectionResult DetectHandsInternally(Image<Bgr, byte> sample)
        {
            Hand left = new Hand();
            Hand right = new Hand();

            for (int x = 0; x < HAND_AREA_WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Bgr pixel = sample.GetPixelAt(x, y);

                    if (NotBackgroundAt(pixel, x, y) && MatchesColorCharacteristics(pixel, m_leftHandColorCharacteristics))
                    {
                        sample.SetPixelAt(new Bgr(Color.White), x, y);
                    }
                    else
                    {
                        sample.SetPixelAt(new Bgr(Color.Black), x, y);
                    }

                }
            }

            for (int x = WIDTH - HAND_AREA_WIDTH; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Bgr pixel = sample.GetPixelAt(x, y);
                    
                    if (NotBackgroundAt(pixel, x, y) && MatchesColorCharacteristics(pixel, m_rightHandColorCharacteristics))
                    {
                        sample.SetPixelAt(new Bgr(Color.White), x, y);
                    }
                    else
                    {
                        sample.SetPixelAt(new Bgr(Color.Black), x, y);
                    }
       
                }
            }

            sample.ROI = new Rectangle(0, 0, HAND_AREA_WIDTH, HEIGHT);
            var smoothened = sample.SmoothMedian(9);
            smoothened.CopyTo(sample);
            sample.ROI = Rectangle.Empty;

            sample.ROI = new Rectangle(WIDTH - HAND_AREA_WIDTH, 0, HAND_AREA_WIDTH, HEIGHT);
            smoothened = sample.SmoothMedian(9);
            smoothened.CopyTo(sample);
            sample.ROI = Rectangle.Empty;

            for (int x = 0; x < HAND_AREA_WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                    if (sample.GetPixelAt(x, y).IsWhite())
                        left.Extend(x, y);

            for (int x = WIDTH - HAND_AREA_WIDTH; x < WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                    if (sample.GetPixelAt(x, y).IsWhite())
                        right.Extend(x, y);

            sample.Draw(new CircleF(new PointF(left.Position.X, left.Position.Y), 3), new Bgr(Color.Yellow), 3);
            sample.Draw(new CircleF(new PointF(right.Position.X, right.Position.Y), 3), new Bgr(Color.Yellow), 3);

            return new HandDetectionResult(new Hands(left, right), sample.Bitmap);
        }

        private bool NotBackgroundAt(Bgr pixel, int x, int y)
        {
            Bgr backgroundReferencePixel = m_background.GetPixelAt(x, y);

            double deltaBlue = Math.Abs(backgroundReferencePixel.Blue - pixel.Blue);
            double deltaGreen = Math.Abs(backgroundReferencePixel.Green - pixel.Green);
            double deltaRed = Math.Abs(backgroundReferencePixel.Red - pixel.Red);

            return 20 <= deltaBlue || 20 <= deltaGreen || 20 <= deltaRed;
        }

        private bool MatchesColorCharacteristics(Bgr pixel, ColorCharacteristics colorCharacteristics)
        {
            return colorCharacteristics.Blue.IsWithinRange(pixel.Blue) &&
                   colorCharacteristics.Green.IsWithinRange(pixel.Green) &&
                   colorCharacteristics.Red.IsWithinRange(pixel.Red) &&
                   colorCharacteristics.BluePerGreen.IsWithinRange(pixel.Blue / pixel.Green) &&
                   colorCharacteristics.BluePerRed.IsWithinRange(pixel.Blue / pixel.Red) &&
                   colorCharacteristics.GreenPerRed.IsWithinRange(pixel.Green / pixel.Red);
        }
    }
}
