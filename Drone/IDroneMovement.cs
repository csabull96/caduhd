using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    interface IDroneMovement
    {
        int Horizontal { get; set; }
        int Vertical { get; set; }
        int Longitudinal { get; set; }
        int Yaw { get; set; }
    }
}
