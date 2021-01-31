using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Detector
{
    public enum HandDetectorState 
    { 
        NeedsCalibrating, 
        NeedsReCalibrating,
        ReadyToCaptureBackground,
        ReadyToAnalyzeLeftHand,
        ReadyToAnalyzeRightHand,
        Calibrated,
        Enabled
    }
}
