using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller
{
    public class DroneControllerEventArgs : EventArgs
    {
        public AbstractDroneCommand DroneCommand { get; private set; }

        public DroneControllerEventArgs(AbstractDroneCommand droneCommand)
        {
            DroneCommand = droneCommand;
        }
    }
}
