using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller.Commands
{
    public enum DroneCameraCommandType { TurnOn, TurnOff }

    public class DroneCameraCommand : AbstractDroneCommand
    {
        public DroneCameraCommandType Type { get; private set; }

        public DroneCameraCommand(DroneCameraCommandType type)
        {
            Type = type;
        }
    }
}
