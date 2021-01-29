using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
