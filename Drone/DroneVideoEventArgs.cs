using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public class DroneVideoEventArgs : EventArgs
    {
        public Bitmap Frame { get; private set; }

        public DroneVideoEventArgs(Bitmap frame)
        {
            Frame = frame;
        }
    }
}
