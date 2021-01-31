using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Detector
{
    public class InputProcessedEventArgs : EventArgs
    {
        public Bitmap ProcessedFrame { get; private set; }

        public InputProcessedEventArgs(Bitmap processedFrame)
        {
            ProcessedFrame = processedFrame;
        }
    }
}
