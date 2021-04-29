using System;

namespace Caduhd.Drone
{

    public class DroneStateChangedEventArgs : EventArgs
    {
        public DroneState DroneState { get; set; }

        public DroneStateChangedEventArgs(DroneState droneState)
        {
            DroneState = droneState;
        }
    }
}
