using Caduhd.Controller.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller
{
    public class DroneControllerInputEvaluatedEventArgs : EventArgs
    {
        public AbstractDroneCommand DroneCommand { get; private set; }

        public DroneControllerInputEvaluatedEventArgs(AbstractDroneCommand droneCommand)
        {
            DroneCommand = droneCommand;
        }
    }
}
