using Caduhd.Common;
using Caduhd.HandsDetector;
using System;
using System.Drawing;

namespace Caduhd.Controller.InputAnalyzer
{
    public class HandsAnalyzer
    {
        private ColorMap _leftHandColorMap;
        private ColorMap _rightHandColorMap;

        private BgrImage _leftHandAnalysisImage;
        private BgrImage _rightHandAnalysisImage;
     
        public HandsAnalyzerState State { get; private set; }

        public HandsAnalyzerResult Result
        {
            get
            {
                if (_leftHandAnalysisImage == null)
                    throw new InvalidOperationException("Missing left hand analysis.");

                if (_rightHandAnalysisImage == null)
                    throw new InvalidOperationException("Missing right hand analysis.");

                // the right side of the _leftHandAnalysisImage and
                // the left side of the _rightHandAnalysisImage contain the background
                BgrImage handsBackground = _rightHandAnalysisImage.Merge(_leftHandAnalysisImage);
                // the right side of the _rightHandAnalysisImage and
                // the left side of the _leftHandAnalysisImage contain the analyzed hands
                BgrImage handsForeground = _leftHandAnalysisImage.Merge(_rightHandAnalysisImage);

                return new HandsAnalyzerResult(new HandsColorMaps(_leftHandColorMap, _rightHandColorMap),
                        handsBackground, handsForeground);
            }
        }

        public void AdvanceState()
        {
            switch (State)
            {
                case HandsAnalyzerState.ReadyToAnalyzeLeft:
                    State = HandsAnalyzerState.AnalyzingLeft;
                    break;
                case HandsAnalyzerState.AnalyzingLeft:
                    if (_leftHandAnalysisImage == null)
                        throw new InvalidOperationException($"Left hand has to be analyzed before advancing to the {HandsAnalyzerState.ReadyToAnalyzeRight} state.");

                    State = HandsAnalyzerState.ReadyToAnalyzeRight;
                    break;
                case HandsAnalyzerState.ReadyToAnalyzeRight:
                    State = HandsAnalyzerState.AnalyzingRight;
                    break;
                case HandsAnalyzerState.AnalyzingRight:
                    if (_leftHandAnalysisImage == null || _rightHandAnalysisImage == null)
                        throw new InvalidOperationException($"Both left and right hands have to be analyzed before advancing to the {HandsAnalyzerState.Tuning} state.");

                    State = HandsAnalyzerState.Tuning;
                    break;
                case HandsAnalyzerState.Tuning:
                    State = HandsAnalyzerState.ReadyToAnalyzeLeft;
                    break;
            }
        }

        public void AnalyzeLeft(BgrImage image, Rectangle roi)
        {
            if (State != HandsAnalyzerState.AnalyzingLeft)
                throw new InvalidOperationException($"Hands analyzer has to be in {HandsAnalyzerState.AnalyzingLeft} state in order to analyze left hand.");

            if (image == null)
                throw new ArgumentNullException("The image used for left hand analysis was null.");

            if (IsRoiValid(image, roi))
            {
                _leftHandAnalysisImage = image.Copy();
                image.Roi = roi;
                _leftHandColorMap = ExtractColorMap(image);
                image.Roi = Rectangle.Empty;
            }
            else
            {
                throw new ArgumentException("The ROI of the left hand analysis is not valid.");
            }
        }

        public void AnalyzeRight(BgrImage image, Rectangle roi)
        {
            if (State != HandsAnalyzerState.AnalyzingRight)
                throw new InvalidOperationException($"Hands analyzer has to be in {HandsAnalyzerState.AnalyzingRight} state in order to analyze right hand.");

            if (image == null)
                throw new ArgumentNullException("The image used for right hand analysis was null.");

            if (_leftHandAnalysisImage == null)
                throw new InvalidOperationException("Prior to the right hand analysis the left hand analysis has to be done.");

            if (_leftHandAnalysisImage.Width != image.Width || _leftHandAnalysisImage.Height != image.Height)
                throw new ArgumentException("The size of the left hand analysis image and the right hand analysis image have to be the same.");

            if (IsRoiValid(image, roi))
            {
                _rightHandAnalysisImage = image.Copy();
                image.Roi = roi;
                _rightHandColorMap = ExtractColorMap(image);
                image.Roi = Rectangle.Empty;
            }
            else
            {
                throw new ArgumentException("The ROI of the right hand analysis is not valid.");
            }
        }

        public void Reset()
        {
            _leftHandColorMap = null;
            _rightHandColorMap = null;

            _leftHandAnalysisImage = null;
            _rightHandAnalysisImage = null;

            State = HandsAnalyzerState.ReadyToAnalyzeLeft;
        }

        private bool IsRoiValid(BgrImage image, Rectangle roi) =>
            0 <= roi.X && 0 <= roi.Y &&
            0 < roi.Width && 0 < roi.Height &&
            roi.X + roi.Width <= image.Width && roi.Y + roi.Height <= image.Height;

        private ColorMap ExtractColorMap(BgrImage image)
        {
            IHistogram blues = new Histogram(0, 255, 64);
            IHistogram greens = new Histogram(0, 255, 64);
            IHistogram reds = new Histogram(0, 255, 64);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    BgrPixel pixel = image.GetPixel(x, y);

                    blues.Insert(pixel.Blue);
                    greens.Insert(pixel.Green);
                    reds.Insert(pixel.Red);
                }
            }

            return new ColorMap(blues, greens, reds);
        } 
    }
}
