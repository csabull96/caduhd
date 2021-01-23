using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    interface IDrone
    {
        void Connect();
        void TakeOff();
        void SetMovement(IDroneControllerState controllerState);
        void Land();
        void Disconnect();
        void StartVideoStream();
        void StopVideoStream();
    }
}
