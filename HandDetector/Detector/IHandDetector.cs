using Caduhd.HandDetector.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Detector
{
    public delegate void HandDetectorInputProcessedEventHandler(object sender, InputProcessedEventArgs eventArgs);
    public delegate void HandDetectorStateChangedEventHandler(object source, HandDetectorStateChangedEventArgs eventArgs);

    public interface IHandDetector
    {
        event HandDetectorInputProcessedEventHandler InputProcessed;
        event HandDetectorStateChangedEventHandler StateChanged;

        HandDetectorState State { get; }

        void ShiftState();
        void CaptureBackground(Bitmap frame);
        void AnalyzeLeftHand(Bitmap frame);
        void AnalyzeRightHand(Bitmap frame);
        Hands DetectHands(Bitmap frame);
    }
}
