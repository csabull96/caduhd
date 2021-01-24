using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public delegate void DroneVideoEventHandler(object source, DroneVideoEventArgs evetArgs);

    interface IStreamable
    {
        event DroneVideoEventHandler Feed; 
        void StartVideoStream();
        void StopVideoStream();
        string GetVideoStreamAddress();
    }
}
