namespace Caduhd.Controller.InputAnalyzer
{
    using Caduhd.Common;
    using Caduhd.HandsDetector;

    /// <summary>
    /// The result of a colod based hand analysis.
    /// </summary>
    public class HandsAnalyzerResult : IHandsDetectorTuning
    {
        /// <summary>
        /// Gets the color maps of the analyzed hands.
        /// </summary>
        public HandsColorMaps HandsColorMaps { get; private set; }

        /// <summary>
        /// Gets the background of the hands.
        /// </summary>
        public BgrImage HandsBackground { get; private set; }

        /// <summary>
        /// Gets the foreground of the hands.
        /// </summary>
        public BgrImage HandsForeground { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandsAnalyzerResult"/> class.
        /// </summary>
        /// <param name="handsColorMaps">The hands color maps.</param>
        /// <param name="handsBackgrounds">The background of the hands.</param>
        /// <param name="handsForeground">The foreground of the hands.</param>
        public HandsAnalyzerResult(HandsColorMaps handsColorMaps, BgrImage handsBackgrounds, BgrImage handsForeground)
        {
            this.HandsColorMaps = handsColorMaps;
            this.HandsBackground = handsBackgrounds;
            this.HandsForeground = handsForeground;
        }
    }
}
