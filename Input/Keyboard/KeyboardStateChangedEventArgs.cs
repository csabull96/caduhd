using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class KeyboardStateChangedEventArgs : EventArgs
    {
        public Key KeyChanged { get; private set; }

        public KeyboardStateChangedEventArgs(Key keyChanged)
        {
            KeyChanged = keyChanged;
        }
    }
}
