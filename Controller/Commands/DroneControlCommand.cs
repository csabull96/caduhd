﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller.Commands
{
    public enum DroneControlCommandType { Connect }

    public class DroneControlCommand : AbstractDroneCommand
    {
        public DroneControlCommandType Type { get; }

        public DroneControlCommand(DroneControlCommandType type)
        {
            Type = type;
        }

        public override AbstractDroneCommand Copy() => new DroneControlCommand(Type);
    }
}