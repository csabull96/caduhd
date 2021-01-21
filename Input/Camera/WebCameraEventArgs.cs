using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Input.Camera
{
    public class WebCameraEventArgs : EventArgs
    {
        public Bitmap Frame { get; private set; }

        public WebCameraEventArgs(Bitmap frame)
        {
            Frame = frame;
        }
    }
}
