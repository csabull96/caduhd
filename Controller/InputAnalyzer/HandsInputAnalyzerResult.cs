using Caduhd.Common;
using Caduhd.HandsDetector;

namespace Caduhd.Controller.InputAnalyzer
{
    public class HandsInputAnalyzerResult : IHandsDetectorTuning
    {
        public HandsColorMaps HandsColorMaps { get; private set; }
        public BgrImage HandsBackground { get; private set; }
        public BgrImage HandsForeground { get; private set; }

        public HandsInputAnalyzerResult(HandsColorMaps handsColorMaps, BgrImage handsBackgrounds, BgrImage handsForeground)
        {
            HandsColorMaps = handsColorMaps;
            HandsBackground = handsBackgrounds;
            HandsForeground = handsForeground;
        }
    }
}
