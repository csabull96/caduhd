using Caduhd.Common;
using System;

namespace Caduhd.Drone.Event
{
    public class NewDroneCameraFrameEventArgs : EventArgs
    {
        public BgrImage Frame { get; private set; }

        public NewDroneCameraFrameEventArgs(BgrImage frame)
        {
            Frame = frame;
        }
    }
}
