using Caduhd.HandDetector.Model;
using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caduhd.HandDetector.Calibrator;

namespace Caduhd.HandDetector.Detector
{
    public class HandDetector : IHandDetector
    {
        private const int WIDTH = 320;
        private const int HEIGHT = 180;
        private const int HAND_AREA_WIDTH = 100;

        private Rectangle m_maskForLeftHand = new Rectangle(35, 50, 45, 65);
        private Rectangle m_maskForRightHand = new Rectangle(250, 50, 45, 65);

        private ColorCharacteristics m_leftHandColorCharacteristics;
        private ColorCharacteristics m_rightHandColorCharacteristics;

        private Image<Bgr, byte> m_background;

        public event HandDetectorInputProcessedEventHandler InputProcessed;
        public event HandDetectorStateChangedEventHandler StateChanged;

        public HandDetectorState State { get; private set; }

        // should use dependency injection later
        private ColorCalibrator m_colorCalibrator = new ColorCalibrator();

        public void ShiftState()
        {
            switch (State)
            {
                case HandDetectorState.NeedsCalibrating:
                    State = HandDetectorState.ReadyToCaptureBackground;
                    break;
                case HandDetectorState.NeedsReCalibrating:
                    State = HandDetectorState.ReadyToCaptureBackground;
                    break;
                case HandDetectorState.ReadyToCaptureBackground:
                    State = HandDetectorState.ReadyToAnalyzeLeftHand;
                    break;
                case HandDetectorState.ReadyToAnalyzeLeftHand:
                    State = HandDetectorState.ReadyToAnalyzeRightHand;
                    break;
                case HandDetectorState.ReadyToAnalyzeRightHand:
                    State = HandDetectorState.Calibrated;
                    break;
                case HandDetectorState.Calibrated:
                    State = HandDetectorState.Enabled;
                    break;
                case HandDetectorState.Enabled:
                    State = HandDetectorState.NeedsReCalibrating;
                    break;
            }

            StateChanged?.Invoke(this, new HandDetectorStateChangedEventArgs(State));
        }

        public void CaptureBackground(Bitmap frame)
        {
            Image<Bgr, byte> sample = PreProcess(frame);

            m_background?.Dispose();
            m_background = sample.Copy();

            InputProcessed?.Invoke(this, new InputProcessedEventArgs(sample.Bitmap));
        }

        public void AnalyzeLeftHand(Bitmap frame)
        {

            m_leftHandColorCharacteristics = AnalyzeHand(frame, m_maskForLeftHand);
        }

        public void AnalyzeRightHand(Bitmap frame) => m_rightHandColorCharacteristics = AnalyzeHand(frame, m_maskForRightHand);

        private ColorCharacteristics AnalyzeHand(Bitmap frame, Rectangle mask)
        {
            Image<Bgr, byte> sample = PreProcess(frame);

            ColorCharacteristics result = m_colorCalibrator.ExtractColorCharacteristics(sample, mask);
            sample.Draw(mask, new Bgr(Color.LimeGreen), 2);

            InputProcessed?.Invoke(this, new InputProcessedEventArgs(sample.Bitmap));
            return result;
        }

        public Hands DetectHands(Bitmap frame)
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

        private Hands DetectHandsInternally(Image<Bgr, byte> sample)
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

            InputProcessed?.Invoke(this, new InputProcessedEventArgs(sample.Bitmap));

            return new Hands(left, right);
        }
    }
}
