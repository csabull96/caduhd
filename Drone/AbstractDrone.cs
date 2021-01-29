using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public abstract class AbstractDrone : IDrone, IStreamable
    {
        public abstract event DroneStateEventHandler StateChanged;
        public abstract void Connect();
        public abstract void Disconnect();
        public abstract void TakeOff();
        public abstract void Land();
        public abstract void Move(DroneMovement controllerState);
       
        public abstract bool IsStreamingVideo { get; }
        public abstract event NewDroneVideoFrameEventHandler NewDroneCameraFrame;
        public abstract void StartStreamingVideo();
        public abstract void StopStreamingVideo();
    }
}
