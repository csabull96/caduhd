using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public delegate void NewDroneVideoFrameEventHandler(object source, NewDroneCameraFrameEventArgs evetArgs);

    interface IStreamable
    {
        bool IsStreamingVideo { get; }
        event NewDroneVideoFrameEventHandler NewDroneCameraFrame; 
        void StartStreamingVideo();
        void StopStreamingVideo();
    }
}
