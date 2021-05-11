namespace Caduhd.Controller.InputAnalyzer
{
    using System;
    using System.Drawing;
    using Caduhd.Common;
    using Caduhd.HandsDetector;

    /// <summary>
    /// A color based hands analyzer.
    /// </summary>
    public class HandsAnalyzer
    {
        private ColorMap leftHandColorMap;
        private ColorMap rightHandColorMap;

        private BgrImage leftHandAnalysisImage;
        private BgrImage rightHandAnalysisImage;

        /// <summary>
        /// Gets the actual state of the <see cref="HandsAnalyzer"/> object.
        /// </summary>
        public HandsAnalyzerState State { get; private set; }

        /// <summary>
        /// Gets the result of the color based hands analysis.
        /// </summary>
        public HandsAnalyzerResult Result
        {
            get
            {
                if (this.leftHandAnalysisImage == null)
                {
                    throw new InvalidOperationException("Missing left hand analysis.");
                }

                if (this.rightHandAnalysisImage == null)
                {
                    throw new InvalidOperationException("Missing right hand analysis.");
                }

                // the right side of the _leftHandAnalysisImage and
                // the left side of the _rightHandAnalysisImage contain the background
                BgrImage handsBackground = this.rightHandAnalysisImage.Merge(this.leftHandAnalysisImage);

                // the right side of the _rightHandAnalysisImage and
                // the left side of the _leftHandAnalysisImage contain the analyzed hands
                BgrImage handsForeground = this.leftHandAnalysisImage.Merge(this.rightHandAnalysisImage);

                return new HandsAnalyzerResult(
                    new HandsColorMaps(this.leftHandColorMap, this.rightHandColorMap), handsBackground, handsForeground);
            }
        }

        /// <summary>
        /// Advances the state of the <see cref="HandsAnalyzer"/>.
        /// </summary>
        public void AdvanceState()
        {
            switch (this.State)
            {
                case HandsAnalyzerState.ReadyToAnalyzeLeft:
                    this.State = HandsAnalyzerState.AnalyzingLeft;
                    break;
                case HandsAnalyzerState.AnalyzingLeft:
                    if (this.leftHandAnalysisImage == null)
                    {
                        throw new InvalidOperationException($"Left hand has to be analyzed before advancing to the {HandsAnalyzerState.ReadyToAnalyzeRight} state.");
                    }

                    this.State = HandsAnalyzerState.ReadyToAnalyzeRight;
                    break;
                case HandsAnalyzerState.ReadyToAnalyzeRight:
                    this.State = HandsAnalyzerState.AnalyzingRight;
                    break;
                case HandsAnalyzerState.AnalyzingRight:
                    if (this.leftHandAnalysisImage == null || this.rightHandAnalysisImage == null)
                    {
                        throw new InvalidOperationException($"Both left and right hands have to be analyzed before advancing to the {HandsAnalyzerState.Tuning} state.");
                    }

                    this.State = HandsAnalyzerState.Tuning;
                    break;
                case HandsAnalyzerState.Tuning:
                    this.State = HandsAnalyzerState.ReadyToAnalyzeLeft;
                    break;
            }
        }

        /// <summary>
        /// Analyses the left hand.
        /// </summary>
        /// <param name="image">The image in which the left hand and the right hand background can be found.</param>
        /// <param name="roi">The region of interest in which the left hand's BgrPixels can be found.</param>
        public void AnalyzeLeft(BgrImage image, Rectangle roi)
        {
            if (this.State != HandsAnalyzerState.AnalyzingLeft)
            {
                throw new InvalidOperationException($"Hands analyzer has to be in {HandsAnalyzerState.AnalyzingLeft} state in order to analyze left hand.");
            }

            if (image == null)
            {
                throw new ArgumentNullException("The image used for left hand analysis was null.");
            }

            if (this.IsRoiValid(image, roi))
            {
                this.leftHandAnalysisImage = image.Copy();
                image.Roi = roi;
                this.leftHandColorMap = this.ExtractColorMap(image);
                image.Roi = Rectangle.Empty;
            }
            else
            {
                throw new ArgumentException("The ROI of the left hand analysis is not valid.");
            }
        }

        /// <summary>
        /// Analyses the right hand.
        /// </summary>
        /// <param name="image">The image in which the right hand and the left hand background can be found.</param>
        /// <param name="roi">The region of interest in which the right hand's BgrPixels can be found.</param>
        public void AnalyzeRight(BgrImage image, Rectangle roi)
        {
            if (this.State != HandsAnalyzerState.AnalyzingRight)
            {
                throw new InvalidOperationException($"Hands analyzer has to be in {HandsAnalyzerState.AnalyzingRight} state in order to analyze right hand.");
            }

            if (image == null)
            {
                throw new ArgumentNullException("The image used for right hand analysis was null.");
            }

            if (this.leftHandAnalysisImage == null)
            {
                throw new InvalidOperationException("Prior to the right hand analysis the left hand analysis has to be done.");
            }

            if (this.leftHandAnalysisImage.Width != image.Width || this.leftHandAnalysisImage.Height != image.Height)
            {
                throw new ArgumentException("The size of the left hand analysis image and the right hand analysis image have to be the same.");
            }

            if (this.IsRoiValid(image, roi))
            {
                this.rightHandAnalysisImage = image.Copy();
                image.Roi = roi;
                this.rightHandColorMap = this.ExtractColorMap(image);
                image.Roi = Rectangle.Empty;
            }
            else
            {
                throw new ArgumentException("The ROI of the right hand analysis is not valid.");
            }
        }

        /// <summary>
        /// Resets the <see cref="HandsAnalyzer"/> object.
        /// </summary>
        public void Reset()
        {
            this.leftHandColorMap = null;
            this.rightHandColorMap = null;

            this.leftHandAnalysisImage = null;
            this.rightHandAnalysisImage = null;

            this.State = HandsAnalyzerState.ReadyToAnalyzeLeft;
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
