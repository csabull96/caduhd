using Caduhd.Drone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller.Commands
{
    public enum DroneMovementCommandType { Land, TakeOff, Move }

    public class DroneMovementCommand : AbstractDroneCommand
    {
        public DroneMovementCommandType MovementType { get; set; }
        public DroneMovement Movement { get; private set; }

        public DroneMovementCommand(DroneMovementCommandType movementType) : this(movementType, DroneMovement.Idle) { }

        public DroneMovementCommand(DroneMovementCommandType movementType, DroneMovement movement)
        {
            MovementType = movementType;
            Movement = movement;
        }
    }
}
