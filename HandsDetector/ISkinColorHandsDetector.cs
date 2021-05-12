using Caduhd.Common;

namespace Caduhd.HandsDetector
{
    public interface ISkinColorHandsDetector
    {
        bool Tuned { get; }

        NormalizedHands Tune(IHandsDetectorTuning tuning);

        void InvalidateTuning();

        HandsDetectorResult DetectHands(BgrImage image);
    }
}
