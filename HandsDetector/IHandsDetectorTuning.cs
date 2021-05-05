using Caduhd.Common;

namespace Caduhd.HandsDetector
{
    public interface IHandsDetectorTuning
    {
        HandsColorMaps HandsColorMaps { get; }
        BgrImage HandsBackground { get; }
        BgrImage HandsForeground { get; }
    }
}
