namespace Caduhd.Controller.InputEvaluator
{
    using System.Collections.Generic;
    using System.Drawing;
    using Caduhd.HandsDetector;

    /// <summary>
    /// Tuneable drone controller hands input evaluator interface.
    /// </summary>
    public interface ITuneableDroneControllerHandsInputEvaluator
    {
        /// <summary>
        /// Gets dictionary that contains the points of the predefined tuner hands.
        /// </summary>
        Dictionary<string, Dictionary<string, List<Point>>> TunerHands { get; }

        /// <summary>
        /// Tunes the drone controller hands input evaluator.
        /// </summary>
        /// <param name="hands">The hands used for tuning.</param>
        void Tune(NormalizedHands hands);
    }
}
