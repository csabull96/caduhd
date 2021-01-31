using Caduhd.Drone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller.Commands
{
    public enum DroneMovementType { Land, TakeOff, Move }

    public class DroneMovementCommand : AbstractDroneCommand
    {
        public DroneMovementType MovementType { get; set; }
        public DroneMovement Movement { get; private set; }

        public DroneMovementCommand(DroneMovementType movementType) : this(movementType, DroneMovement.Idle) { }

        public DroneMovementCommand(DroneMovementType movementType, DroneMovement movement)
        {
            MovementType = movementType;
            Movement = movement;
        }

        public override AbstractDroneCommand Copy()
        {
            DroneMovement movement = new DroneMovement()
            {
                Longitudinal = Movement.Longitudinal,
                Lateral = Movement.Lateral,
                Vertical = Movement.Vertical,
                Yaw = Movement.Yaw
            };

            return new DroneMovementCommand(MovementType, movement);
        }
    }
}
