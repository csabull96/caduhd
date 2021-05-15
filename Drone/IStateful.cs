using Caduhd.Drone.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public delegate void DroneStateEventHandler(object source, DroneStateChangedEventArgs args);

    public interface IStateful
    {
        event DroneStateEventHandler StateChanged;
    }
}
