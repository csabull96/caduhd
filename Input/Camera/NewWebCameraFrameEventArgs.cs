using System;
using System.Drawing;

namespace Ksvydo.Input.Camera
{
    public class NewWebCameraFrameEventArgs : EventArgs
    {
        public Bitmap Frame { get; private set; }

        public NewWebCameraFrameEventArgs(Bitmap frame)
        {
            Frame = frame;
        }
    }
}
