namespace Caduhd.HandsDetector
{
    using Caduhd.Common;

    /// <summary>
    /// <see cref="IHandsDetectorTuning"/> interface.
    /// </summary>
    public interface IHandsDetectorTuning
    {
        /// <summary>
        /// Gets the color maps of the analyzed hands.
        /// </summary>
        HandsColorMaps HandsColorMaps { get; }

        /// <summary>
        /// Gets the background of the hands.
        /// </summary>
        BgrImage HandsBackground { get; }

        /// <summary>
        /// Gets the foreground of the hands.
        /// </summary>
        BgrImage HandsForeground { get; }
    }
}
