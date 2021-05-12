using Caduhd.Common;
using System.Collections.Generic;
using System.Drawing;

namespace Caduhd.Controller.InputAnalyzer
{
    public interface IHandsAnalyzer
    {
        HandsAnalyzerState State { get; }

        HandsAnalyzerResult Result { get; }

        void AdvanceState();

        void AnalyzeLeft(BgrImage image, List<Point> poi);

        void AnalyzeRight(BgrImage image, List<Point> poi);

        void Reset();
    }
}
