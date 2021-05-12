using Caduhd.Drone.Command;
using System;

namespace Caduhd.Drone
{
    public abstract class AbstractDrone : IDisposable
    {

        public abstract void Control(DroneCommand droneCommand);
        public abstract void Dispose();
    }
}
