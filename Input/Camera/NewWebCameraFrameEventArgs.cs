using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Input.Camera
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
