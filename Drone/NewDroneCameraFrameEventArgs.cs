using System;
using System.Drawing;

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
