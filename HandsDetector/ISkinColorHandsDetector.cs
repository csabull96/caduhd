namespace Caduhd.HandsDetector
{
    using Caduhd.Common;

    /// <summary>
    /// Skin color hands detector interface.
    /// </summary>
    public interface ISkinColorHandsDetector
    {
        /// <summary>
        /// Gets a value indicating whether the skin color hands detector is tuned or not.
        /// </summary>
        bool Tuned { get; }

        /// <summary>
        /// Tunes the skin color hands detector.
        /// </summary>
        /// <param name="tuning">The tuning.</param>
        /// <returns>Returns the neutral state hands.</returns>
        NormalizedHands Tune(IHandsDetectorTuning tuning);

        /// <summary>
        /// Invalidates the current tuning.
        /// </summary>
        void InvalidateTuning();

        /// <summary>
        /// Detects the hands based on their color.
        /// </summary>
        /// <param name="image">The image to detect the hands on.</param>
        /// <returns>The result of the handsdetection.</returns>
        HandsDetectorResult DetectHands(BgrImage image);
    }
}
