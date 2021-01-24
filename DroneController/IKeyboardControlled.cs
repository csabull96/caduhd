﻿using Caduhd.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Controller
{
    public interface IKeyboardControlled
    {
        void HandleInput(Key key, KeyStatus status);
    }
}
