using Caduhd.Common;
using System;

namespace Caduhd.Input.Camera
{
    public class NewWebCameraFrameEventArgs : EventArgs
    {
        public BgrImage Frame { get; private set; }

        public NewWebCameraFrameEventArgs(BgrImage frame)
        {
            Frame = frame;
        }
    }
}
