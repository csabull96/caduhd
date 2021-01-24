using Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class KeyStatusChangedEventArgs : EventArgs
    {
        public Key Key { get; set; }
        public KeyStatus KeyStatus { get; set; }

        public KeyStatusChangedEventArgs(Key key, KeyStatus keyStatus)
        {
            Key = key;
            KeyStatus = keyStatus;
        }
    }
}
