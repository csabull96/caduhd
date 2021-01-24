using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public class DroneMovement : IDroneMovement
    {
        public int Lateral { get; set; } = 0;
        public int Vertical { get; set; } = 0;
        public int Longitudinal { get; set; } = 0;
        public int Yaw { get; set; } = 0;

        public static IDroneMovement Idle => new DroneMovement();
    }
}
