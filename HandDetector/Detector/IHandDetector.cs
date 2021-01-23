using Caduhd.HandDetector.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Detector
{
    interface IHandDetector
    {
        Hands DetectHands(Bitmap frame);
    }
}
