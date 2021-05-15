using Caduhd.Drone.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public delegate void NewDroneVideoFrameEventHandler(object source, NewDroneCameraFrameEventArgs args);

    public interface IStreamer
    {
        event NewDroneVideoFrameEventHandler NewCameraFrame;
    }
}
