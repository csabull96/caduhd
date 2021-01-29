using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public class NewDroneCameraFrameEventArgs : EventArgs
    {
        public Bitmap Frame { get; private set; }

        public NewDroneCameraFrameEventArgs(Bitmap frame)
        {
            Frame = frame;
        }
    }
}
