using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller.Commands
{
    public abstract class AbstractDroneCommand
    {
        public abstract AbstractDroneCommand Copy();
    }
}
