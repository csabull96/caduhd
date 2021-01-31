using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Detector
{
    public class HandDetectorStateChangedEventArgs
    {
        public HandDetectorState HandDetectorStatus { get; set; }

        public HandDetectorStateChangedEventArgs(HandDetectorState handDetectorStatus)
        {
            HandDetectorStatus = handDetectorStatus;
        }
    }
}
