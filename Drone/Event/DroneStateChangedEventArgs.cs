using System;

namespace Caduhd.Drone.Event
{
    public class DroneStateChangedEventArgs : EventArgs
    {
        public DroneState DroneState { get; private set; }

        public DroneStateChangedEventArgs(DroneState droneState)
        {
            DroneState = droneState;
        }
    }
}
