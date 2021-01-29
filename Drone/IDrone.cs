using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public delegate void DroneStateEventHandler(object source, DroneStateChangedEventArgs evetArgs);

    interface IDrone
    {
        event DroneStateEventHandler StateChanged;
        void Connect();
        void TakeOff();
        void Move(DroneMovement controllerState);
        void Land();
        void Disconnect();
    }
}
