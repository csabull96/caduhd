namespace Caduhd.Controller.InputAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Caduhd.Common;
    using Caduhd.HandsDetector;

    /// <summary>
    /// A color based hands analyzer.
    /// </summary>
    public class HandsAnalyzer : IHandsAnalyzer
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
        /// <param name="poi">The points of interest.</param>
        public void AnalyzeLeft(BgrImage image, List<Point> poi)
        {
            if (this.State != HandsAnalyzerState.AnalyzingLeft)
            {
                throw new InvalidOperationException($"Hands analyzer has to be in {HandsAnalyzerState.AnalyzingLeft} state in order to analyze left hand.");
            }

            if (image == null)
            {
                throw new ArgumentNullException("The image used for left hand analysis was null.");
            }

            this.leftHandAnalysisImage = image.Copy();
            this.leftHandColorMap = this.ExtractColorMap(image, poi);
        }

        /// <summary>
        /// Analyses the right hand.
        /// </summary>
        /// <param name="image">The image in which the right hand and the left hand background can be found.</param>
        /// <param name="poi">The points of interest.</param>
        public void AnalyzeRight(BgrImage image, List<Point> poi)
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

            this.rightHandAnalysisImage = image.Copy();
            this.rightHandColorMap = this.ExtractColorMap(image, poi);
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

        private ColorMap ExtractColorMap(BgrImage image, List<Point> poi)
        {
            IHistogram blues = new Histogram(0, 255, 64);
            IHistogram greens = new Histogram(0, 255, 64);
            IHistogram reds = new Histogram(0, 255, 64);

            foreach (Point point in poi)
            {
                BgrPixel pixel = image.GetPixel(point.X, point.Y);

                blues.Insert(pixel.Blue);
                greens.Insert(pixel.Green);
                reds.Insert(pixel.Red);
            }

            return new ColorMap(blues, greens, reds);
        }
    }
}
