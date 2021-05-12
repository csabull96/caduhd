using Caduhd.HandsDetector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller.InputEvaluator
{
    public interface ITuneableDroneControllerHandsInputEvaluator
    {
        void Tune(NormalizedHands hands);

        Dictionary<string, Dictionary<string, List<Point>>> TunerHands { get; }

    }
}
