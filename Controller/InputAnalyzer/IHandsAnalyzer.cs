namespace Caduhd.Controller.InputAnalyzer
{
    using System.Collections.Generic;
    using System.Drawing;
    using Caduhd.Common;

    /// <summary>
    /// The hands analyzer interface.
    /// </summary>
    public interface IHandsAnalyzer
    {
        /// <summary>
        /// Gets the current state of of the hands analyzer.
        /// </summary>
        HandsAnalyzerState State { get; }

        /// <summary>
        /// Gets the result of the analysis.
        /// </summary>
        HandsAnalyzerResult Result { get; }

        /// <summary>
        /// Advances the state of the hands analyzer.
        /// </summary>
        void AdvanceState();

        /// <summary>
        /// Analyzes the left hand based on the pre defined points of interest.
        /// </summary>
        /// <param name="image">The image containing the left hand to analyze.</param>
        /// <param name="poi">The points of interest.</param>
        void AnalyzeLeft(BgrImage image, List<Point> poi);

        /// <summary>
        /// Analyzes the right hand based on the pre defined points of interest.
        /// </summary>
        /// <param name="image">The image containing the right hand to analyze.</param>
        /// <param name="poi">The points of interest.</param>
        void AnalyzeRight(BgrImage image, List<Point> poi);

        /// <summary>
        /// Resets the hands analyzer.
        /// </summary>
        void Reset();
    }
}
